using ElevatorApp.Models.Enums;
using System;

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
        /// <c>true</c> if the call was from a <see cref="Passenger"/> entering the <see cref="Elevator"/>; otherwise, <c>false</c>.
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

        #region Operators and overloads

        /// <summary>
        /// Deconstructs this <see cref="ElevatorCall"/> into a <see cref="ValueTuple{T1,T2}"/>
        /// </summary>
        /// <param name="floor">The floor.</param>
        /// <param name="direction">The direction.</param>
        public void Deconstruct(out int floor, out Direction direction)
        {
            floor = this.Floor;
            direction = this.Direction;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ElevatorCall"/> to a <see cref="ValueTuple{T1, T2}"/>.
        /// </summary>
        /// <param name="call">The call.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator (int floor, Direction direction) (ElevatorCall call)
        {
            return (call.Floor, call.Direction);
        }

        /// <summary>
        /// Performs an implicit conversion from a <see cref="ValueTuple{T1, T2}"/>. to <see cref="ElevatorCall"/>.
        /// </summary>
        /// <param name="call">The call.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ElevatorCall((int floor, Direction direction) call)
        {
            return new ElevatorCall(call.floor, call.direction);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return (this.Floor, this.Direction).ToString();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is ElevatorCall call && this.Floor == call.Floor && this.Direction == call.Direction && this.FromPassenger == call.FromPassenger;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 564164426;
                hashCode = hashCode * -1521134295 + this.Floor.GetHashCode();
                hashCode = hashCode * -1521134295 + this.Direction.GetHashCode();
                hashCode = hashCode * -1521134295 + this.FromPassenger.GetHashCode();
                return hashCode;
            }
        }

        #endregion Operators and overloads
    }
}