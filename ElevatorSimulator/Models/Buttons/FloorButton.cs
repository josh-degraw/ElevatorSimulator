using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;
using System.Threading.Tasks;

namespace ElevatorApp.Models
{
    /// <inheritdoc cref="ButtonBase{T}"/>
    /// <summary>
    /// Represents a Button that tells the elevator to go to a specified floor
    /// </summary>
    public class FloorButton : ButtonBase<int>, ISubcriber<ElevatorMasterController>
    {
        private bool _subscribed = false;

        /// <inheritdoc/>
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ElevatorApp.Models.FloorButton"/> class.
        /// </summary>
        /// <param name="floorNumber">The floor number.</param>
        /// <param name="startActive">if set to <c>true</c> [start active].</param>
        public FloorButton(int floorNumber, bool startActive = false) : base($"FloorBtn {floorNumber}", startActive)
        {
            this.FloorNumber = floorNumber;
        }

        /// <summary>
        /// The type of button this is
        /// </summary>
        /// <inheritdoc/>
        public override ButtonType ButtonType => ButtonType.Floor;

        /// <summary>
        /// The number of the <see cref="Floor"/> this <see cref="FloorButton"/> is tied to
        /// </summary>
        public virtual int FloorNumber { get; }

        /// <summary>
        /// The text on the button
        /// </summary>
        /// <inheritdoc/>
        public override string Label => this.FloorNumber.ToString();

        /// <summary>
        /// Represents whether or not this object has performed the necessary steps to subscribe to the source.
        /// </summary>
        /// <inheritdoc/>
        public bool Subscribed
        {
            get => this._subscribed;
            protected set => this.SetProperty(ref this._subscribed, value);
        }

        /// <summary>
        /// Disables this instance.
        /// </summary>
        public void Disable()
        {
            this.IsEnabled = false;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Push the button
        /// </summary>
        public override void Push()
        {
            this.HandlePushed(this, this.FloorNumber);
        }

        /// <summary>
        /// Perform the necessary steps to subscribe to the target.
        /// </summary>
        /// <param name="parent">The item this object will be subscribed to</param>
        /// <returns></returns>
        /// <inheritdoc/>
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
                parent.Elevator.OnNext((this.FloorNumber, parent.Elevator.Direction));
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
            //        this.HandleActionCompleted(e, floor.DestinationFloor);
            //};

            this.Subscribed = true;
            return Task.CompletedTask;
        }
    }
}