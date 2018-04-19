using ElevatorApp.Models.Enums;
using ElevatorApp.Util;
using System;
using System.Threading.Tasks;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Represents a button used to request an <see cref="Elevator"/> from a <see cref="Floor"/>
    /// </summary>
    public class RequestButton : FloorButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestButton"/> class.
        /// </summary>
        /// <param name="sourceFloor">The source floor.</param>
        /// <param name="destinationFloor">The destination floor.</param>
        /// <param name="startActive">if set to <c>true</c> [start active].</param>
        /// <inheritdoc/>
        public RequestButton(int sourceFloor, int destinationFloor, bool startActive = false) : base(sourceFloor, startActive)
        {
            this.DestinationFloor = destinationFloor;
        }

        /// <summary>
        /// Gets the request direction.
        /// </summary>
        private Direction _requestDirection => this.DestinationFloor > this.FloorNumber ? Direction.Up : Direction.Down;

        /// <summary>
        /// The number of the <see cref="Floor"/> that this button tells the <see cref="Elevator"/> to go to.
        /// </summary>
        public int DestinationFloor { get; }

        /// <summary>
        /// The text on the button
        /// </summary>
        /// <inheritdoc/>
        public override string Label => this.DestinationFloor.ToString();

        /// <inheritdoc/>
        /// <summary>
        /// The number of the <see cref="T:ElevatorApp.Models.Floor"/> that this Button is on. Here, it functions as the
        /// Source floor.
        /// </summary>
        public override int FloorNumber => base.FloorNumber;

        /// <inheritdoc/>
        /// <summary>
        /// Push the button
        /// </summary>
        public override void Push()
        {
            this.HandlePushed(this, this.DestinationFloor);
        }

        /// <inheritdoc/>
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
                            elevator.OnNext(new ElevatorCall(this.FloorNumber, this._requestDirection, false));
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