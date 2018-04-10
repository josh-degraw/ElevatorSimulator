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
    public class Floor : ModelBase, ISubcriber<ElevatorMasterController>, ISubcriber<(ElevatorMasterController, Elevator)>
    {
        #region Backing fields
        private int _floorNum = 1;
        private ElevatorCallPanel _callPanel = new ElevatorCallPanel(1);

        private bool _subscribed = false, _controllerSubscribed = false, _elevatorAvailable = false;

        private readonly AsyncObservableCollection<Passenger> _waitingPassengers = new AsyncObservableCollection<Passenger>();

        #endregion

        #region Properties

        public int FloorNumber
        {
            get => _floorNum;
            private set => SetProperty(ref _floorNum, value);

        }

        public ElevatorCallPanel CallPanel
        {
            get => _callPanel;
            set => SetProperty(ref _callPanel, value);
        }

        public bool ElevatorAvailable
        {
            get => _elevatorAvailable;
            set => SetProperty(ref _elevatorAvailable, value);
        }

        public IReadOnlyCollection<Passenger> WaitingPassengers => _waitingPassengers;

        #endregion

        public Floor(int floorNumber, ElevatorCallPanel callPanel)
        {
            this.FloorNumber = floorNumber;
            this.CallPanel = callPanel;
        }

        public Floor(int floorNumber) : this(floorNumber, new ElevatorCallPanel(floorNumber))
        {

        }

        public Floor()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        public void QueuePassenger(int destination)
        {
            this._waitingPassengers.Add(new Passenger(this.FloorNumber, destination));
        }

        #region Subscription

        bool ISubcriber<(ElevatorMasterController, Elevator)>.Subscribed => _subscribed;
        bool ISubcriber<ElevatorMasterController>.Subscribed => _controllerSubscribed;

        async Task ISubcriber<ElevatorMasterController>.Subscribe(ElevatorMasterController parent)
        {
            if (_controllerSubscribed)
                return;

            await this.CallPanel.Subscribe(parent).ConfigureAwait(false);

            parent.Floors.AsParallel().ForAll(floor =>
            {
                foreach (FloorButton button in floor.CallPanel)
                {
                    button.OnPushed += (_, floorNum) =>
                    {
                        this.QueuePassenger(floorNum);
                    };
                }
            });


            this._controllerSubscribed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parents"></param>
        /// <returns></returns>
        public async Task Subscribe((ElevatorMasterController, Elevator) parents)
        {
            if (this._subscribed)
                return;

            await this.CallPanel.Subscribe(parents);
            var (controller, elevator) = parents;

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
                if (e.DestinationFloor == this.FloorNumber)
                {
                    if (getPassengersToMove(elevator, e.DestinationFloor).Any())
                        e.ShouldStop = true;
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
                if (controller.ElevatorCount == 1)
                {
                    this.ElevatorAvailable = false;
                }
                else if (controller.ElevatorCount > 1)
                {
                    if (!controller.Elevators.Any(a => a.CurrentFloor == this.FloorNumber && a.Direction == ElevatorDirection.None))
                    {
                        this.ElevatorAvailable = false;
                    }
                }
            }

            //void cancelDoorClosingIfNewPassengerAdded(object sender, DoorStateChangeEventArgs args)
            //{
            //    List<Passenger> shouldGetOn = getPassengersToMove(elevator).ToList();
            //    if (shouldGetOn.Count > 0)
            //    {
            //        args.CancelOperation = true;
            //    }
            //}

            #endregion

            this._subscribed = true;
        }

        private async Task _addPassengersToElevator(Elevator elevator)
        {
            // TODO: Wait for all passengers to get out
            IEnumerable<Passenger> departing = getPassengersToMove(elevator);
            await _addPassengersToElevator(departing, elevator);
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
            void cancelIfStartsToClose(object sender, DoorStateChangeEventArgs args)
            {
                args.CancelOperation = true;
            }
            //There's probably a better way to do this. But the doors were being forced to stay open when adding
            //passengers because CancelOperations was being left "true"
            void resetCancelOp(object sender, DoorStateChangeEventArgs args)
            {
                args.CancelOperation = false;
            }

            elevator.Door.Closing += cancelIfStartsToClose;
            foreach (Passenger passenger in departing)
            {
                await elevator.AddPassenger(passenger);
            }

            // Remove the event handler now because we can let the door close, since all the passengers are on now
            elevator.Door.Closing -= cancelIfStartsToClose;
            //Allow doors to close after keeping them open. 
            elevator.Door.Closing += resetCancelOp;
        }

        IEnumerable<Passenger> getPassengersToMove(Elevator elevator, int? nextFloor = null)
        {
            try
            {
                int nextDest = this._waitingPassengers.Min(p => Math.Abs(p.Path.destination - nextFloor ?? elevator.NextFloor));
                if (nextDest != 0)
                {
                    return this._getPassengersToMove(nextDest, elevator);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return Enumerable.Empty<Passenger>();
        }

        private IEnumerable<Passenger> _getPassengersToMove(int destination, Elevator elevator)
        {
            switch (elevator.Direction)
            {
                // If the elevator is already going up, only add passengers who are going up
                case ElevatorDirection.GoingUp:
                    return this._waitingPassengers.Where(p => p.State == PassengerState.Waiting && p.Path.destination >= destination);

                // If the elevator is already going down, only add passengers who are going down
                case ElevatorDirection.GoingDown:
                    return this._waitingPassengers.Where(p => p.State == PassengerState.Waiting && p.Path.destination <= destination);

                default:
                    return this._waitingPassengers;
            }
        }


        #endregion

    }
}
