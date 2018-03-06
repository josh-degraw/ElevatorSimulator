using ElevatorApp.Models.Interfaces;

namespace ElevatorApp.Models
{
    public class Passenger : ModelBase
    {
        public int Weight { get; set; }

        private PassengerState _state;

        public PassengerState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }
        


        public (int source, int destination) Path { get; set; }

    }
}
