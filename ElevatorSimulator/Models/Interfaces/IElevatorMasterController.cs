using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElevatorApp.Util;

namespace ElevatorApp.Models.Interfaces
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
        
        Task Dispatch(ElevatorCall call);
        Task Init();
    }
}