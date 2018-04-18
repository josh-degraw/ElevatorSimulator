using ElevatorApp.Models.Enums;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Represents a request for an <see cref="Elevator"/>
    /// </summary>
    public class ElevatorCall
    {
        /// <summary>
        /// The floor the <see cref="Elevator"/> needs to go to
        /// </summary>
        public int Floor { get; }

        /// <summary>
        /// The direction the <see cref="Elevator"/> will be moving
        /// </summary>
        public Direction Direction { get; }

        /// <summary>
        /// Gets a value indicating whether the call was from a <see cref="Passenger"/> entering the <see cref="Elevator"/>
        /// </summary>
        /// <value>
        ///   <c>true</c> if the call was from a <see cref="Passenger"/> entering the <see cref="Elevator"/>; otherwise, <c>false</c>.
        /// </value>
        public bool FromPassenger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorCall"/> class.
        /// </summary>
        /// <param name="floor">The floor.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="fromPassenger">if set to <c>true</c> [from passenger].</param>
        public ElevatorCall(int floor, Direction direction, bool fromPassenger = true)
        {
            this.Floor = floor;
            this.Direction = direction;
            this.FromPassenger = fromPassenger;
        }

        public void Deconstruct(out int floor, out Direction direction)
        {
            floor = this.Floor;
            direction = this.Direction;
        }

        public static implicit operator (int floor, Direction direction) (ElevatorCall call)
        {
            return (call.Floor, call.Direction);
        }
        public static implicit operator ElevatorCall((int floor, Direction direction) call)
        {
            return new ElevatorCall(call.floor, call.direction);
        }

        public override string ToString()
        {
            return (Floor, Direction).ToString();
        }

        public override bool Equals(object obj)
        {
            var call = obj as ElevatorCall;
            return call != null &&
                   Floor == call.Floor &&
                   Direction == call.Direction &&
                   FromPassenger == call.FromPassenger;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 564164426;
                hashCode = hashCode * -1521134295 + Floor.GetHashCode();
                hashCode = hashCode * -1521134295 + Direction.GetHashCode();
                hashCode = hashCode * -1521134295 + FromPassenger.GetHashCode();
                return hashCode; 
            }
        }
    }
}