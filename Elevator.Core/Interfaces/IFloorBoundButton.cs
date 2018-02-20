namespace ElevatorApp.Core.Interfaces
{
    public interface IFloorBoundButton : IButton
    {
        int FloorNum { get; }
    }
}
