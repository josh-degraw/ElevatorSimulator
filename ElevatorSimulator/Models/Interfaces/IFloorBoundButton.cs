namespace ElevatorApp.Models.Interfaces
{
    public interface IFloorBoundButton : IButton
    {
        int FloorNum { get; }
    }
}
