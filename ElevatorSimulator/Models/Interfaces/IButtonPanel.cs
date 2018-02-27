using System.Collections.Generic;

namespace ElevatorApp.Models.Interfaces
{
    public interface IButtonPanel
    {
        ICollection<FloorButton> FloorButtons { get; }
        DoorButton CloseDoorButton { get; }
        DoorButton OpenDoorButton { get; }
    }
}