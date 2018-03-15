using System;
using ElevatorApp.Models.Enums;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Represents a call to the <see cref="Elevator"/>
    /// </summary>
    public interface IElevatorCall
    {
        /// <summary>
        /// The number of the <see cref="Floor"/> that the <see cref="Elevator"/> will arrive at
        /// </summary>
        int DestinationFloor { get; }

        /// <summary>
        /// The direction the <see cref="Elevator"/> is going.
        /// </summary>
        Direction RequestDirection { get; }
    }

    ///// <summary>
    ///// Represents a call to the <see cref="Elevator"/>
    ///// </summary>
    //[Obsolete]
    //public struct ElevatorCall : IElevatorCall
    //{
    //    /// <summary>
    //    /// The number of the <see cref="Floor"/> that the <see cref="Elevator"/> will arrive at
    //    /// </summary>
    //    public int DestinationFloor { get; }

    //    /// <summary>
    //    /// The number of the <see cref="Floor"/> that the <see cref="Elevator"/> will arrive at
    //    /// </summary>
    //    public int SourceFloor { get; }

    //    /// <summary>
    //    /// The direction the <see cref="Elevator"/> will be going.
    //    /// <para>e.g. If the <see cref="SourceFloor"/> is above the <see cref="DestinationFloor"/>, the elevator is going Up</para>
    //    /// </summary>
    //    public Direction RequestDirection => this.DestinationFloor > this.SourceFloor ? Direction.Up : Direction.Down;

    //    /// <summary>
    //    /// Create a new <see cref="ElevatorCall"/> 
    //    /// </summary>
    //    /// <param name="sourceFloor"></param>
    //    /// <param name="destination"></param>
    //    public ElevatorCall(int sourceFloor, int destination)
    //    {
    //        this.DestinationFloor = destination;
    //        this.SourceFloor = sourceFloor;
    //    }

    //    /// <summary>
    //    /// Create a new <see cref="ElevatorCall"/> to go in the given direction
    //    /// </summary>
    //    /// <param name="sourceFloor"></param>
    //    /// <param name="requestDirection"></param>
    //    public ElevatorCall(int sourceFloor, Direction requestDirection)
    //    {
    //        this.SourceFloor = sourceFloor;
    //        this.DestinationFloor = requestDirection == Direction.Up ? int.MaxValue : int.MinValue;

    //    }

    //    #region Operators

    //    /// <summary>
    //    /// Assert equality
    //    /// </summary>
    //    /// <param name="a"></param>
    //    /// <param name="b"></param>
    //    /// <returns>True</returns>
    //    public static bool operator ==(ElevatorCall a, ElevatorCall b)
    //    {
    //        return a.Equals(b);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="a"></param>
    //    /// <param name="b"></param>
    //    /// <returns></returns>
    //    public static bool operator !=(ElevatorCall a, ElevatorCall b)
    //    {
    //        return !(a == b);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    /// <returns></returns>
    //    public override bool Equals(object obj)
    //    {
    //        return base.Equals(obj);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="other"></param>
    //    /// <returns></returns>
    //    public bool Equals(ElevatorCall other)
    //    {
    //        return DestinationFloor == other.DestinationFloor && SourceFloor == other.SourceFloor;
    //    }

    //    ///<inheritdoc/>
    //    public override int GetHashCode()
    //    {
    //        unchecked
    //        {
    //            return (DestinationFloor * 397) ^ SourceFloor;
    //        }
    //    }

    //    /// <summary>
    //    /// Implicitly convert an <see cref="ElevatorCall"/> to a <see cref="ValueTuple{T1, T2}"/> of two <see langword="int"/>s
    //    /// </summary>
    //    /// <param name="path"></param>
    //    public static implicit operator ElevatorCall((int source, int destination) path)
    //    {
    //        return new ElevatorCall(path.source, path.destination);
    //    }

    //    #endregion
    //}

    ///<inheritdoc cref="IElevatorCall"/>
    public struct DirectionalElevatorCall : IElevatorCall
    {
        /// <inheritdoc />
        public int DestinationFloor { get; }


        /// <inheritdoc />
        public Direction RequestDirection { get; }

        /// <summary>
        /// Create a new <see cref="DirectionalElevatorCall"/> to go in the given direction
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="requestDirection"></param>
        public DirectionalElevatorCall(int destination, Direction requestDirection)
        {
            this.DestinationFloor = destination;
            this.RequestDirection = requestDirection;

        }

        #region Operators

        /// <summary>
        /// Assert equality
        /// </summary>
        public static bool operator ==(DirectionalElevatorCall a, DirectionalElevatorCall b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Not equals operator
        /// </summary>
        public static bool operator !=(DirectionalElevatorCall a, DirectionalElevatorCall b)
        {
            return !(a == b);
        }

        ///<inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is IElevatorCall c)
                return this.Equals(c);

            return base.Equals(obj);
        }
        
        
        ///<inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                return (DestinationFloor * 397) ^ (int) RequestDirection;
            }
        }

        /// <summary>
        /// Checks if the two <see cref="IElevatorCall"/>s are equal
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IElevatorCall other)
        {
            return DestinationFloor == other.DestinationFloor && RequestDirection == other.RequestDirection;
        }
        
        #endregion
    }
}
