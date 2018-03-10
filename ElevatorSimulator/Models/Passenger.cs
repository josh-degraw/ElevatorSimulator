using System;
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

        private (int source, int destination) _path;

        public (int source, int destination) Path
        {
            get => _path;
            set => SetProperty(ref _path, value);
        }

        public static readonly TimeSpan TransitionSpeed = TimeSpan.FromSeconds(1.5);

        public Passenger()
        {

        }

        public Passenger(int source, int destination)
        {
            this.Path = (source, destination);

        }

        public override string ToString()
        {
            return $"Passenger: Path: {this.Path.source} -> {this.Path.destination}";
        }

    }
}
