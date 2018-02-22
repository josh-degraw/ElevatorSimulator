using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElevatorApp.Core.Models;

namespace ElevatorApp.Core.Interfaces
{
    public interface IFloor
    {
        ICollection<ElevatorCallPanel> Buttons { get; }
        int FloorNumber { get; }
    }
}