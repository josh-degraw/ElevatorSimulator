using System;
using System.Collections.Generic;

namespace ElevatorApp.Core
{
    public class Elevator
    {
        public int Speed { get; set; }
        public ElevatorState State { get; set; }
        public Queue<int> Path { get; set; }
        public int Capacity { get; set; }
        public ICollection<Passenger> Passengers { get; set; }
        public IDictionary<int, FloorButton> FloorButtons { get; set; }
        public DoorButton CloseDoorButton { get; }
        public DoorButton OpenDoorButton { get; }

        public int CurrentFloor { get; private set; }
        public Door Door { get; set; }

        public event EventHandler OnArrival;
        public event EventHandler OnDeparture;

        public void Dispatch(int floor)
        {
            throw new NotImplementedException();
        }
    }
}
