using System;
using System.Collections.Generic;
using ElevatorApp.Util;

namespace ElevatorApp.Models.Interfaces
{
    public interface IElevator : IMasterSubscriber
    {
        int Capacity { get; set; }
        int CurrentFloor { get; }
        Door Door { get; }

        ButtonPanel ButtonPanel { get; }

        ICollection<IPassenger> Passengers { get; }
        ObservableConcurrentQueue<int> Path { get; }
        
        ElevatorState State { get; }

        event EventHandler<int> OnArrival;
        event EventHandler<int> OnDeparture;

        void Arrived();
    }
}