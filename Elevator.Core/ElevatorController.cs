using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorApp.Core
{
    public class ElevatorController
    {
        public ICollection<Elevator> Elevators { get; set; }
        public Queue<int> FloorsRequested { get; set; }
        public ICollection<ElevatorDispatcher> Dispatchers { get; set; }

        public event EventHandler OnElevatorRequested;

        public void QueueElevator(int floor)
        {
            throw new NotImplementedException();
        }
    }
}
