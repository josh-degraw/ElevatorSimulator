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
        ICollection<Elevator> Elevators { get; }
        ElevatorSettings ElevatorSettings { get; }
        int FloorCount { get; set; }
        int FloorHeight { get; set; }
        ObservableConcurrentQueue<ElevatorCall> FloorsRequested { get; }

        event EventHandler<ElevatorCall> OnElevatorRequested;
        
        void Dispatch(ElevatorCall call);
        void Init();
    }
}