using System;
using System.Threading.Tasks;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;

namespace ElevatorApp.Models
{
    /// <inheritdoc cref="ButtonBase{T}" />
    /// <summary>
    /// Represents a Button that tells the elevator to go to a specified floor
    /// </summary>
    public class FloorButton : ButtonBase<int>, ISubcriber<(ElevatorMasterController, Elevator)>
    {
        /// <inheritdoc />
        public override string Label => this.FloorNumber.ToString();

        /// <summary>
        /// The number of the <see cref="Floor"/> this <see cref="FloorButton"/> is tied to
        /// </summary>
        public int FloorNumber { get; }

        public FloorButton(int floorNumber, bool startActive = false) : base($"FloorBtn {floorNumber}", startActive)
        {
            this.FloorNumber = floorNumber;
        }

        public override void Push()
        {
            this.Pushed(this, this.FloorNumber);
        }

        private bool _subscribed = false;

        /// <inheritdoc />
        public bool Subscribed
        {
            get => _subscribed;
            private set => SetProperty(ref _subscribed, value);
        }

        public void Disable()
        {
            this.IsEnabled = false;
        }

        /// <inheritdoc />
        public Task Subscribe((ElevatorMasterController, Elevator) parent)
        {
            if (this.Subscribed)
                return Task.CompletedTask;

            var (controller, elevator) = parent;

            Logger.LogEvent($"Subscribing {nameof(FloorButton)}", ("Elevator", elevator.ElevatorNumber));

            //this.IsEnabled = this.FloorNumber != elevator.CurrentFloor;

            this.OnPushed += async (a, b) =>
            {
                Logger.LogEvent($"Pushed floor button {this.FloorNumber}");
                await controller.Dispatch(new ElevatorCall(elevator.CurrentFloor, this.FloorNumber)).ConfigureAwait(false);
            };

            //elevator.Departed += (e, floor) =>
            //{
            //    if (floor == this.FloorNumber)
            //    {
            //        this.IsEnabled = true;
            //        this.Active = false;
            //    }
            //};

            //elevator.Arrived += (e, floor) =>
            //{
            //    if (floor == this.FloorNumber)
            //        this.ActionCompleted(e, floor);
            //};

            //elevator.PropertyChanged += (e, args) =>
            //{
            //    if (args.PropertyName == nameof(Elevator.CurrentFloor))
            //    {
            //        this.IsEnabled = this.FloorNumber != elevator.CurrentFloor;
            //    }
            //};

            this.Subscribed = true;
            return Task.CompletedTask;
        }
    }
}
