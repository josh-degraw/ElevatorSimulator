using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;
using FontAwesome;

namespace ElevatorApp.Models
{
    delegate void RequestFloor(int floorNumber, Direction requestDirection);

    /// <summary>
    /// Represents a button used to request an <see cref="Elevator"/> from a <see cref="Floor"/>
    /// </summary>
    public class RequestButton : FloorButton
    {
        /// <summary> The number of the <see cref="Floor"/> that this button tells the <see cref="Elevator"/> to go to.</summary>
        public int DestinationFloor { get; }

        private Direction RequestDirection => DestinationFloor > FloorNumber ? Direction.Up : Direction.Down;

        ///<inheritdoc/>
        public override string Label => DestinationFloor.ToString();

        /// <summary>
        /// The number of the <see cref="Floor"/>  that this Button is on. Here, it functions as the Source floor.
        /// </summary>
        public override int FloorNumber => base.FloorNumber;

        ///<inheritdoc/>
        public RequestButton(int sourceFloor, int destinationFloor, bool startActive = false) : base(sourceFloor, startActive)
        {
            this.DestinationFloor = destinationFloor;
        }

        ///<inheritdoc/>
        public override void Push()
        {
            base.Pushed(this, DestinationFloor);
        }

        /// <summary>
        /// Subscribes the specified controller.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns></returns>
        public override Task Subscribe(ElevatorMasterController controller)
        {
            if (this.Subscribed)
                return Task.CompletedTask;

            try
            {
                Elevator elevator = controller.Elevator;
                // This should be handled by the Call panels
                this.OnPushed += (a, floor) =>
                {
                    try
                    {
                        Logger.LogEvent("Elevator Requested", ("From floor", this.FloorNumber));

                        // If the elevator's already here, open the door
                        if (elevator.CurrentFloor == this.FloorNumber &&
                            (elevator.State == ElevatorState.Arrived || elevator.State == ElevatorState.Idle))
                        {
                            if (elevator.Door.IsClosedOrClosing)
                            {
                                elevator.Door.RequestOpen();
                            }
                        }
                        else
                        {
                            // If the elevator isn't already here, tell it to come here
                            elevator.OnNext((this.FloorNumber, RequestDirection));
                        }

                    }
                    catch (Exception ex)
                    {
                        elevator.OnError(ex);
                    }
                };
            }
            finally
            {
                this.Subscribed = true;

            }


            return Task.CompletedTask;
        }

    }
}
