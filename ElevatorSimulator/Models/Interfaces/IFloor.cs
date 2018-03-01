using System.Collections.Generic;

namespace ElevatorApp.Models.Interfaces
{
    public interface IFloor
    {
        ElevatorCallPanel CallPanel { get; }
        int FloorNumber { get; }
    }
}