using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;
using MoreLinq;

namespace ElevatorApp.Models
{

    /// <summary>
    /// Represents a floor of a building.
    /// </summary>
    public class Floor : ModelBase, ISubcriber<ElevatorMasterController>
    {
        #region Backing fields
        private int _floorNum = 1;
        private ElevatorCallPanel _callPanel = new ElevatorCallPanel(1);

        private bool _subscribed = false, _controllerSubscribed = false, _elevatorAvailable = false;

        private readonly AsyncObservableCollection<Passenger> _waitingPassengers = new AsyncObservableCollection<Passenger>();

        #endregion

        #region Properties

        /// <summary>
        /// The number of the floor
        /// </summary>
        public int FloorNumber
        {
            get => _floorNum;
            private set => SetProperty(ref _floorNum, value);

        }


        /// <summary>
        /// The panel outside of the <see cref="Elevator"/>, used to call the elevator to the floor.
        /// </summary>
        public ElevatorCallPanel CallPanel
        {
            get => _callPanel;
            set => SetProperty(ref _callPanel, value);
        }

        /// <summary>
        /// Represents whether the <see cref="Elevator"/> is at this floor at the given moment
        /// </summary>
        public bool ElevatorAvailable
        {
            get => _elevatorAvailable;
            private set => SetProperty(ref _elevatorAvailable, value);
        }

        /// <summary>
        /// The passengers waiting on the floor for the <see cref="Elevator"/>
        /// </summary>
        public IReadOnlyCollection<Passenger> WaitingPassengers => _waitingPassengers;

        #endregion

        /// <summary>
        /// Constructs a new <see cref="Floor"/> with the given floor number and call panel passed in
        /// </summary>
        public Floor(int floorNumber, ElevatorCallPanel callPanel)
        {
            this.FloorNumber = floorNumber;
            this.CallPanel = callPanel;
        }


        /// <summary>
        /// Constructs a new <see cref="Floor"/> with the given floor number
        /// </summary>
        public Floor(int floorNumber) : this(floorNumber, new ElevatorCallPanel(floorNumber))
        {

        }


        /// <summary>
        /// Parameterless constructor. Shouldn't be called directly. Only for use with the designer
        /// </summary>
        public Floor()
        {

        }

        /// <summary>
        /// Adds a passenger to the list of waiting passengers
        /// </summary>
        /// <param name="destination"></param>
        public void QueuePassenger(int destination)
        {
            this._waitingPassengers.Add(new Passenger(this.FloorNumber, destination));
        }

        #region Subscription

        /// <inheritdoc />
        public bool Subscribed { get; private set; }


        /// <summary>
        /// Subscribes the <see cref="Floor"/> to the given master controller and elevator
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public async Task Subscribe(ElevatorMasterController controller)
        {
            if (this.Subscribed)
                return;

            foreach (FloorButton button in this.CallPanel)
            {
                button.OnPushed += (_, floorNum) =>
                {
                    this.QueuePassenger(floorNum);
                };
            }

            await this.CallPanel.Subscribe(controller);

            var elevator = controller.Elevator;

            if (elevator.CurrentFloor == this.FloorNumber)
            {
                this.ElevatorAvailable = true;
            }

            _waitingPassengers.CollectionChanged += onPassengerAdded;
            elevator.PassengerAdded += removePassengerFromQueue;
            elevator.Door.Opened += addPassengersToElevator;
            //elevator.Door.CloseRequested += cancelDoorClosingIfNewPassengerAdded;
            //elevator.Door.Closing += cancelDoorClosingIfNewPassengerAdded;
            elevator.Departed += onElevatorDeparted;

            elevator.Approaching += Elevator_Approaching;
            elevator.Arrived += onElevatorArrivedAtThisFloor;

            #region Local methods for event handlers, which do the real work

            // Respond to passengers trying to get on the elevator
            async void onPassengerAdded(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    // If the elevator is here, add the passengers and get going
                    if (this.ElevatorAvailable)
                    {
                        try
                        {
                            if (elevator.Door.DoorState == DoorState.Opened)
                            {
                                IEnumerable<Passenger> departing = getPassengersToMove(elevator);
                                await _addPassengersToElevator(departing, elevator);
                            }
                            else
                            {
                                await elevator.Door.RequestOpen();
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    }
                }
            }

            async void addPassengersToElevator(object _, DoorStateChangeEventArgs args)
            {
                if (elevator.CurrentFloor == this.FloorNumber)
                {
                    try
                    {
                        await _addPassengersToElevator(elevator);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
            }

            void Elevator_Approaching(object sender, ElevatorApproachingEventArgs e)
            {
                if (e.IntermediateFloor == this.FloorNumber)
                {
                    e.ShouldStop = getPassengersToMove(elevator, e.DestinationFloor).Any();
                }
            }

            void removePassengerFromQueue(object _, Passenger passenger)
            {
                try
                {
                    this._waitingPassengers.Remove(passenger);
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentOutOfRangeException)
                    {
                        ; // Ignore
                    }
                }
            }

            // Alert the Floor that an elevator is available when one arrives on this floor
            void onElevatorArrivedAtThisFloor(object _, ElevatorMovementEventArgs args)
            {
                if (args.DestinationFloor == this.FloorNumber)
                {
                    this.ElevatorAvailable = true;
                }
            }

            void onElevatorDeparted(object _, ElevatorMovementEventArgs call)
            {
                this.ElevatorAvailable = false;
            }

            #endregion

            this.Subscribed = true;
        }

        private async Task _addPassengersToElevator(Elevator elevator)
        {
            // TODO: Wait for all passengers to get out
            IEnumerable<Passenger> departing = getPassengersToMove(elevator);
            await _addPassengersToElevator(departing, elevator);
        }

        private bool _adding = false;
        void cancelIfStartsToClose(object sender, DoorStateChangeEventArgs args)
        {
            args.CancelOperation = _adding;
        }
        
        /// <summary>
        /// Takes care of:
        /// <para>Opening the Elevator door</para>
        /// <para>Moving passengers from this <see cref="Floor"/> onto the <see cref="Elevator"/></para>
        /// <para>Keeping the door open until all <see cref="Passenger"/>s are added</para>
        /// </summary>
        /// <param name="departing"></param>
        /// <param name="elevator"></param>
        /// <returns></returns>
        private async Task _addPassengersToElevator(IEnumerable<Passenger> departing, Elevator elevator)
        {
            // Event handler to make sure the door stays open while passengers are trying to get in

            //There's probably a better way to do this. But the doors were being forced to stay open when adding
            //passengers because CancelOperations was being left "true"
            //void resetCancelOp(object sender, DoorStateChangeEventArgs args)
            //{
            //    args.CancelOperation = false;
            //}

            elevator.Door.Closing += cancelIfStartsToClose;
            _adding = true;
            foreach (Passenger passenger in departing)
            {
                await elevator.AddPassenger(passenger);
            }
            _adding = false;
            if(elevator.Direction == Direction.None)
            {
                await elevator.Dispatch(elevator.FloorsToStopAt.MinBy(a => Math.Abs(a - elevator.CurrentFloor)));
            }

            //// Remove the event handler now because we can let the door close, since all the passengers are on now
            //elevator.Door.Closing -= cancelIfStartsToClose;
            ////Allow doors to close after keeping them open. 
            //elevator.Door.Closing += resetCancelOp;
        }

        /// <summary>
        /// Gets the passengers that are going 
        /// </summary>
        IEnumerable<Passenger> getPassengersToMove(Elevator elevator, int? nextFloor = null)
        {
            try
            {
                int nextDest = 0;
                var direction = Direction.None;
                if (elevator.Direction == Direction.None)
                {
                    // Get the floor of the first passenger added to the waitlist. 
                    Passenger pass = this.WaitingPassengers
                        .MinByOrDefault(p => p.PassengerNumber);

                    direction = pass?.Direction ?? Direction.None;

                    nextDest = pass?.Path.destination ?? elevator.NextFloor;
                }
                else
                {
                    Passenger pass = this.WaitingPassengers
                        .Where(p => p.Direction == elevator.Direction)
                        .MinByOrDefault(p => Math.Abs(p.Path.destination - nextFloor ?? elevator.NextFloor));

                    direction = pass?.Direction ?? Direction.None;
                    nextDest = pass?.Path.destination ?? elevator.NextFloor;
                }

                if (direction != Direction.None)
                    return this._getPassengersToMove(nextDest, direction);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return Enumerable.Empty<Passenger>();
        }

        private IEnumerable<Passenger> _getPassengersToMove(int destination, Direction direction)
        {
            var waiting = this.WaitingPassengers
                .Where(p => p.State == PassengerState.Waiting && p.Direction == direction);

            switch (direction)
            {
                // If the elevator is already going up, only add passengers who are going up
                case Direction.Up:
                    return waiting
                            .Where(p => p.Path.destination >= destination);

                // If the elevator is already going down, only add passengers who are going down
                case Direction.Down:
                    return this.WaitingPassengers
                        .Where(p => p.Path.destination <= destination);

                default:
                    throw new InvalidOperationException("Direction cannot be None");
            }
        }


        #endregion

    }
}
