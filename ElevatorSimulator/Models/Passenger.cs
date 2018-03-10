using System;
using System.Collections.Generic;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;
using NodaTime;
using static ElevatorApp.Util.Logger;

namespace ElevatorApp.Models
{
    public class Passenger : ModelBase
    {
        private Instant Entered = default;
        private Instant Exited = default;

        private Instant WaitStart = Clock.GetCurrentInstant();
        private Instant WaitEnd = default;

        public Duration TimeWaiting => WaitEnd - WaitStart;
        public Duration TimeSpentInElevator => Exited - Entered;

        public int Weight { get; set; }

        private PassengerState _state = PassengerState.Waiting;

        public PassengerState State
        {
            get => _state;
            set
            {
                PassengerState oldVal = _state;
                SetProperty(ref _state, value);

                switch (value)
                {
                    case PassengerState.Waiting:
                        WaitStart = Clock.GetCurrentInstant();
                        break;
                    case PassengerState.Transition when oldVal == PassengerState.Waiting:
                        WaitEnd = Clock.GetCurrentInstant();
                        break;
                    case PassengerState.In:
                        Entered = Clock.GetCurrentInstant();
                        break;
                    case PassengerState.Out:
                        Exited = Clock.GetCurrentInstant();
                        break;
                    default:
                        break;
                }
            }
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

        public string GetWaitingString()
        {
            if (WaitEnd == default)
                return "";

            return $"Time Spent Waiting: {TimeWaiting:mm:ss}";
        }

        public string GetTimeInElevatorString()
        {
            if (Exited == default)
                return "";

            return $"Time Spent in Elevator: {TimeSpentInElevator:mm:ss}";
        }

        public override string ToString() => $"Path: {this.Path.source} -> {this.Path.destination}";

        public static implicit operator (object, object)[] (Passenger passenger)
        {
            const string duration_format = "mm:ss";

            var obj = new List<(object, object)>(3)
            {
                ("Path", $"{passenger.Path.source} -> {passenger.Path.destination}")
            };

            if (passenger.WaitEnd != default)
                obj.Add(("Time Spent Waiting:", passenger.TimeWaiting.ToString(duration_format, null)));

            if (passenger.Exited != default)
                obj.Add(("Time Spent in Elevator:", passenger.TimeSpentInElevator.ToString(duration_format, null)));

            return obj.ToArray();
        }
    }
}
