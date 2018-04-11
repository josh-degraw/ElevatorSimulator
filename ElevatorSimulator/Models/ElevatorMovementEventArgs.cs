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
        public virtual int DestinationFloor { get; }

        /// <summary>
        /// The direction the elevator is going
        /// </summary>
        public Direction Direction { get; }

        /// <summary>
        /// Set to <see langword="true"/> to prevent other events in the chain from being processed
        /// </summary>
        public bool Handled { get; set; }

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
            return $"{DestinationFloor}. Direction: {Direction}";
        }
    }

    /// <inheritdoc />
    public class ElevatorApproachingEventArgs : ElevatorMovementEventArgs
    {
        /// <summary>
        /// In the event, set this to <see langword="true"/> to allow the <see cref="Elevator"/> to stop.
        /// </summary>
        public bool ShouldStop { get; set; }

        /// <summary>
        /// The actual floor that is being approached
        /// </summary>
        public int IntermediateFloor { get; }

        /// <summary>
        /// The final destination of the <see cref="Elevator"/> in the current path
        /// </summary>
        public override int DestinationFloor => base.DestinationFloor;

        /// <inheritdoc />
        public ElevatorApproachingEventArgs(int intermediateFloor, int destination, Direction direction) : base(destination, direction)
        {
            this.IntermediateFloor = intermediateFloor;
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