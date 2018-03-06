namespace ElevatorApp.Models.Interfaces
{
    public interface IElevatorCallPanel
    {
        int FloorNumber { get; }
        RequestButton GoingDownButton { get; }
        RequestButton GoingUpButton { get; }
    }
}