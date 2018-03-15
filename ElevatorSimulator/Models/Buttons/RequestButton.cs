using System;
using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using FontAwesome;

namespace ElevatorApp.Models
{
    delegate void RequestFloor(int floorNumber, Direction requestDirection);

    /// <summary>
    /// Represents a button used to request an <see cref="Elevator"/> from a <see cref="Floor"/>
    /// </summary>
    public class RequestButton : FloorButton
    {
        /// <summary>
        /// The floor that this button requests
        /// </summary>
        public int DestinationFloor { get; }
        
        /// <summary>
        /// The number of the floor that this Button is on
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
            base.Pushed(this, FloorNumber);
            base.Pushed(this, DestinationFloor);
        }
    }
}
