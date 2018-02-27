using System.Collections.Generic;

namespace ElevatorApp.Models.Interfaces
{
    public interface IFloor
    {
        ICollection<ElevatorCallPanel> Buttons { get; }
        int FloorNumber { get; }
    }
}