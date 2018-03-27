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

        public override async Task Subscribe((ElevatorMasterController, Elevator) parent)
        {
            var (controller, elevator) = parent;

            // This should be handled by the Call panels
            this.OnPushed += async (a, floor) =>
            {
                try
                {
                    // If the elevator's already here, open the door
                    if (elevator.CurrentFloor == this.FloorNumber && (elevator.State == ElevatorState.Arrived || elevator.State == ElevatorState.Idle))
                    {
                        if (elevator.Door.DoorState != DoorState.Opened)
                        {
                            await elevator.Door.RequestOpen();
                        }
                    }

                    Logger.LogEvent($"Pushed floor button {this.FloorNumber}");
                    ((IObserver<int>)controller).OnNext(this.FloorNumber);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            };

            this.Subscribed = true;
        }
    }
}
