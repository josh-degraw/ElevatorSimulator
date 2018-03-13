using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElevatorApp.Util;

namespace ElevatorApp.Models
{
    public class ElevatorSimulator
    {
        public ElevatorMasterController Controller { get; } = new ElevatorMasterController();


        public ElevatorSimulator()
        {
            Logger.LogEvent("Initializing Simulator");
        }

    }
}
