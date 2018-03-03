using System;
using System.Threading.Tasks;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;

namespace ElevatorApp.Models
{
    public class FloorButton : ButtonBase<int>, ISubcriber<(ElevatorMasterController, Elevator)>
    {
        public override string Label => this.FloorNum.ToString();

        public int FloorNum { get; }

        public FloorButton(int floorNum, bool active = false) : base($"FloorBtn {floorNum}")
        {
            this.FloorNum = floorNum;
            this._active = active;
        }

        public override void Push()
        {
            this.Pushed(this, this.FloorNum);
        }

        private bool _subscribed = false;

        public bool Subscribed
        {
            get => _subscribed;
            private set => SetProperty(ref _subscribed, value);
        }

        public void Disable()
        {
            this.IsEnabled = false;
        }


        public Task Subscribe((ElevatorMasterController, Elevator) parent)
        {
            if (this.Subscribed)
                return Task.CompletedTask;

            var (controller, elevator) = parent;

            Logger.LogEvent($"Subscribing {nameof(FloorButton)}", ("Elevator", elevator.ElevatorNumber));

            this.IsEnabled = this.FloorNum != elevator.CurrentFloor;

            this.OnPushed += async (a, b) =>
            {
                Logger.LogEvent($"Pushed floor button {this.FloorNum}");
                await controller.Dispatch(this.FloorNum, elevator.CurrentFloor > this.FloorNum ? Direction.Down : Direction.Up).ConfigureAwait(true);
            };

            elevator.OnDeparture += (e, floor) =>
            {
                if (floor == this.FloorNum)
                {
                    this.IsEnabled = true;
                    this.Active = false;
                }
            };

            elevator.OnArrival += (e, floor) =>
            {
                if (floor == this.FloorNum)
                    this.ActionCompleted(e, floor);
            };

            elevator.PropertyChanged += (e, args) =>
            {
                if (args.PropertyName == nameof(Elevator.CurrentFloor))
                {
                    this.IsEnabled = this.FloorNum != elevator.CurrentFloor;
                }
            };

            this.Subscribed = true;
            return Task.CompletedTask;
        }
    }
}
