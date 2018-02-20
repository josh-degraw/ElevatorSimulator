using ElevatorApp.Core.Models;

namespace ElevatorApp.Core.Interfaces
{
    public interface ICallElevatorButtonPanel: IMasterSubscriber
    {
        int FloorNumber { get; }
        RequestButton GoingDownButton { get; }
        RequestButton GoingUpButton { get; }
    }
}