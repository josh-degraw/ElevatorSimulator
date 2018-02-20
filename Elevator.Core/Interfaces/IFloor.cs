using System.Collections.Generic;
using ElevatorApp.Core.Models;

namespace ElevatorApp.Core.Interfaces
{
    public interface IFloor
    {
        ICollection<CallElevatorButtonPanel> Buttons { get; }
        int FloorNumber { get; }
    }
}