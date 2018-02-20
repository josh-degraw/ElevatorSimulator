using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElevatorApp.Core.Models;

namespace ElevatorApp.Core.Interfaces
{
    public interface IElevator: IMasterSubscriber
    {
        int Capacity { get; set; }
        int CurrentFloor { get; }
        Door Door { get; }

        ButtonPanel ButtonPanel { get; set; }

        ObservableCollection<IPassenger> Passengers { get; }
        ObservableConcurrentQueue<int> Path { get; }

        int Speed { get; set; }
        ElevatorState State { get; set; }

        event EventHandler<int> OnArrival;
        event EventHandler<int> OnDeparture;

        void Arrived();
        void Dispatch(int floor);
    }
}