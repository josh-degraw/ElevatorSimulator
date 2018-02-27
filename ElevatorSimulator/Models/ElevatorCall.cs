namespace ElevatorApp.Models
{
    public struct ElevatorCall
    {
        public int DestinationFloor { get; set; }
        public Direction RequestDirection { get; set; }

        public ElevatorCall(int destination, Direction direction)
        {
            this.DestinationFloor = destination;
            this.RequestDirection = direction;
        }

        #region Operators
        public static implicit operator int(ElevatorCall call) => call.DestinationFloor;
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
