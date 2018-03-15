using System;
using ElevatorApp.Models.Enums;

namespace ElevatorApp.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Holds the data that represents an <see cref="Elevator"/> movement
    /// </summary>
    public class ElevatorMovementEventArgs : EventArgs
    {
        /// <summary>
        /// The destination of the current movement
        /// </summary>
        public int DestinationFloor { get; }

        /// <summary>
        /// The direction the elevator is going
        /// </summary>
        public Direction Direction { get; }

        /// <summary>
        /// Construct a new instance of <see cref="ElevatorMovementEventArgs"/>
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="direction"></param>
        public ElevatorMovementEventArgs(int destination, Direction direction)
        {
            this.DestinationFloor = destination;
            this.Direction = direction;
        }

        ///<inheritdoc/>
        public override string ToString()
        {
            return $"To: {DestinationFloor}. Direction: {Direction}";
        }
    }

    /// <inheritdoc />
    public class ElevatorApproachingEventArgs : ElevatorMovementEventArgs
    {
        /// <summary>
        /// In the event, set this to <see langword="true"/> to allow the <see cref="Elevator"/> to stop.
        /// </summary>
        public bool ShouldStop { get; set; }

        /// <inheritdoc />
        public ElevatorApproachingEventArgs(int destination, Direction direction) : base(destination, direction)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// The arguments that have been passed along the event cycle
        /// </summary>
        public ElevatorApproachingEventArgs(ElevatorMovementEventArgs args) : base(args.DestinationFloor, args.Direction)
        {

        }
    }
}