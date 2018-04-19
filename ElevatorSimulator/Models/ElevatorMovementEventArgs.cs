using ElevatorApp.Models.Enums;
using System;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Represents information about an event occurring when the elevator is approaching a floor
    /// </summary>
    /// <seealso cref="ElevatorApp.Models.ElevatorMovementEventArgs" />
    /// <inheritdoc />
    public class ElevatorApproachingEventArgs : ElevatorMovementEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorApproachingEventArgs"/> class.
        /// </summary>
        /// <param name="intermediateFloor">The intermediate floor.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="direction">The direction.</param>
        /// <inheritdoc />
        public ElevatorApproachingEventArgs(int intermediateFloor, int destination, Direction direction) : base(destination, direction)
        {
            this.IntermediateFloor = intermediateFloor;
        }

        /// <inheritdoc/>
        /// <summary>
        /// The arguments that have been passed along the event cycle
        /// </summary>
        public ElevatorApproachingEventArgs(ElevatorMovementEventArgs args) : base(args.DestinationFloor, args.Direction)
        {
        }

        /// <summary>
        /// The actual floor that is being approached
        /// </summary>
        public int IntermediateFloor { get; }

        /// <summary>
        /// In the event, set this to <see langword="true"/> to allow the <see cref="Elevator"/> to stop.
        /// </summary>
        public bool ShouldStop { get; set; }

        /// <inheritdoc/>
        /// <summary>
        /// The final destination of the <see cref="T:ElevatorApp.Models.Elevator"/> in the current path
        /// </summary>
        public override int DestinationFloor => base.DestinationFloor;
    }

    /// <inheritdoc/>
    /// <summary>
    /// Holds the data that represents an <see cref="Elevator"/> movement
    /// </summary>
    public class ElevatorMovementEventArgs : EventArgs
    {
        /// <inheritdoc/>
        /// <summary>
        /// Construct a new instance of <see cref="T:ElevatorApp.Models.ElevatorMovementEventArgs"/>
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="direction"></param>
        public ElevatorMovementEventArgs(int destination, Direction direction) : this((destination, direction))
        {
        }

        /// <inheritdoc/>
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ElevatorApp.Models.ElevatorMovementEventArgs"/> class.
        /// </summary>
        /// <param name="call">The call.</param>
        public ElevatorMovementEventArgs(ElevatorCall call)
        {
            this.Call = call;
        }

        /// <summary>
        /// Gets or sets the call.
        /// </summary>
        public ElevatorCall Call { get; set; }

        /// <summary>
        /// The destination of the current movement
        /// </summary>
        public virtual int DestinationFloor => this.Call.Floor;

        /// <summary>
        /// The direction the elevator is going
        /// </summary>
        public Direction Direction => this.Call.Direction;

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.DestinationFloor}. Direction: {this.Direction}";
        }
    }
}