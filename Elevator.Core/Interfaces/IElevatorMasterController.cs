using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ElevatorApp.Core.Models;

namespace ElevatorApp.Core.Interfaces
{
    public interface IElevatorMasterController
    {
        int ElevatorCount { get; set; }
        ObservableCollection<Elevator> Elevators { get; set; }
        ElevatorSettings ElevatorSettings { get; set; }
        int FloorCount { get; set; }
        int FloorHeight { get; set; }
        ObservableCollection<Floor> Floors { get; set; }
        ObservableConcurrentQueue<int> FloorsRequested { get; }

        event EventHandler<int> OnElevatorRequested;

        void Dispatch(int floor, Direction direction);
        void Init();
        void QueueElevator(int floor);
        void ReportArrival(IElevator elevator);
    }
}