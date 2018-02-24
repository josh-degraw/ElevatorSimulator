using System;
using System.Collections.Generic;
using System.Text;
using ElevatorApp.Core.Interfaces;

namespace ElevatorApp.Core.Models
{
    public class Passenger : ModelBase, IPassenger
    {
        public int Weight { get; set; }

        private PassengerState _state;

        public PassengerState State
        {
            get => _state;
            set
            {
                SetProperty(ref _state, value);
            }
        }
        


        public (int source, int destination) Path { get; set; }

    }
}
