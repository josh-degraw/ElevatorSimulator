using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElevatorApp.Util;

namespace ElevatorApp.Models.Interfaces
{
    public interface IElevator 
    {
        int Capacity { get; set; }
        int CurrentFloor { get; }
        Door Door { get; }

        ButtonPanel ButtonPanel { get; }

        ICollection<Passenger> Passengers { get; }
        ObservableConcurrentQueue<int> Path { get; }
        
        ElevatorState State { get; }

        event EventHandler<int> OnArrival;
        event EventHandler<int> OnDeparture;

        Task Arrived();
    }
}