using System;
using System.Collections.Generic;
using System.Text;
using ElevatorApp.Core.Interfaces;

namespace ElevatorApp.Core
{
    public class Passenger : IPassenger
    {
        public int Weight { get; set; }

        public PassengerState State { get; set; }
    }
}
