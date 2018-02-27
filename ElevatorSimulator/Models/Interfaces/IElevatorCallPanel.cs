namespace ElevatorApp.Models.Interfaces
{
    public interface IElevatorCallPanel: IMasterSubscriber
    {
        int FloorNumber { get; }
        RequestButton GoingDownButton { get; }
        RequestButton GoingUpButton { get; }
    }
}