using System;
using System.Threading.Tasks;
using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;

namespace ElevatorApp.Models
{
    /// <inheritdoc cref="ButtonBase{T}" />
    /// <summary>
    /// Represents a Button that tells the elevator to go to a specified floor
    /// </summary>
    public class FloorButton : ButtonBase<int>, ISubcriber<ElevatorMasterController>
    {
        /// <inheritdoc />
        public override string Label => this.FloorNumber.ToString();

        /// <inheritdoc />
        public override ButtonType ButtonType => ButtonType.Floor;

        /// <summary>
        /// The number of the <see cref="Floor"/> this <see cref="FloorButton"/> is tied to
        /// </summary>
        public virtual int FloorNumber { get; }

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
            protected set => SetProperty(ref _subscribed, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Disable()
        {
            this.IsEnabled = false;
        }

        /// <inheritdoc />
        public virtual Task Subscribe(ElevatorMasterController parent)
        {
            if (this.Subscribed)
                return Task.CompletedTask;


            Logger.LogEvent($"Subscribing {nameof(FloorButton)}", ("Elevator", parent.Elevator.ElevatorNumber));

            //this.IsEnabled = this.FloorNumber != elevator.CurrentFloor;

            // This should be handled by the Call panels
            this.OnPushed += (a, floor) =>
            {
                Logger.LogEvent($"Pushed floor button {this.FloorNumber}");
                parent.Elevator.OnNext(this.FloorNumber);
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
            //    if (floor.DestinationFloor == this.FloorNumber)
            //        this.ActionCompleted(e, floor.DestinationFloor);
            //};
            

            this.Subscribed = true;
            return Task.CompletedTask;
        }
    }
}
