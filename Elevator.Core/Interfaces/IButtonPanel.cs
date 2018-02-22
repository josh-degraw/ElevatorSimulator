using System.Collections.Generic;
using ElevatorApp.Core.Models;

namespace ElevatorApp.Core.Interfaces
{
    public interface IButtonPanel
    {
        ICollection<FloorButton> FloorButtons { get; }
        DoorButton CloseDoorButton { get; }
        DoorButton OpenDoorButton { get; }
    }
}