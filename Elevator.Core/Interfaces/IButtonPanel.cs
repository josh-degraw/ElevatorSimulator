using System.Collections.Generic;
using ElevatorApp.Core.Models;

namespace ElevatorApp.Core.Interfaces
{
    public interface IButtonPanel: IMasterSubscriber
    {
        ICollection<FloorButton> FloorButtons { get; set; }
        DoorButton CloseDoorButton { get; set; }
        DoorButton OpenDoorButton { get; set; }
    }
}