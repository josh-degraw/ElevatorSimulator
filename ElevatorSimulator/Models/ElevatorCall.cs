namespace ElevatorApp.Models
{
    public struct ElevatorCall
    {
        public int DestinationFloor { get; }

        public int SourceFloor { get; }

        public Direction RequestDirection => this.DestinationFloor > this.SourceFloor ? Direction.Up : Direction.Down;

        public ElevatorCall(int sourceFloor, int destination)
        {
            this.DestinationFloor = destination;
            this.SourceFloor = sourceFloor;
        }

        public ElevatorCall(int sourceFloor, Direction requestDirection)
        {
            this.SourceFloor = sourceFloor;
            this.DestinationFloor = requestDirection == Direction.Up ? int.MaxValue : int.MinValue;

        }

        #region Operators

        public static bool operator==(ElevatorCall a, ElevatorCall b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ElevatorCall a, ElevatorCall b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(ElevatorCall other)
        {
            return DestinationFloor == other.DestinationFloor && SourceFloor == other.SourceFloor;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (DestinationFloor * 397) ^ SourceFloor;
            }
        }

        public static implicit operator ElevatorCall((int source, int destination) path)
        {
            return new ElevatorCall(path.source, path.destination);
        }

        public static implicit operator (int source, int destination)(ElevatorCall path)
        {
            return (path.SourceFloor, path.DestinationFloor);
        }

        #endregion
    }

    public static partial class Extensions
    {

        public static void Deconstruct(this ElevatorCall @this, out int destinationFloor, out Direction direction)
        {
            destinationFloor = @this.DestinationFloor;
            direction = @this.RequestDirection;
        }

    }
}
