using ElevatorApp.Util;
using System.Threading.Tasks;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Represents a button panel on the inside of an elevator, including floor buttons and door open / close buttons
    /// </summary>
    /// <seealso cref="ElevatorApp.Models.ButtonPanelBase"/>
    /// <inheritdoc/>
    public class ButtonPanel : ButtonPanelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonPanel"/> class.
        /// </summary>
        /// <inheritdoc/>
        public ButtonPanel()
        {
            this._floorButtons = new AsyncObservableCollection<FloorButton>
            {
                new FloorButton(4),
                new FloorButton(3),
                new FloorButton(2),
                new FloorButton(1),
            };

            this.OpenDoorButton = DoorButton.Open();
            this.CloseDoorButton = DoorButton.Close();
        }

        /// <summary>
        /// The actual collection of <see cref="FloorButton"/> s
        /// </summary>
        /// <inheritdoc/>
        protected override AsyncObservableCollection<FloorButton> _floorButtons { get; }

        /// <summary>
        /// Close the door of the <see cref="Elevator"/>
        /// </summary>
        public DoorButton CloseDoorButton { get; }

        /// <summary>
        /// Opens the door of the <see cref="Elevator"/>
        /// </summary>
        public DoorButton OpenDoorButton { get; }

        /// <summary>
        /// Subscribes this Button panel to the <see cref="ElevatorMasterController"/>
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        /// <inheritdoc/>
        public override async Task Subscribe(ElevatorMasterController parent)
        {
            if (this.Subscribed)
                return;

            await Task.WhenAll(this.OpenDoorButton.Subscribe(parent.Elevator.Door), this.CloseDoorButton.Subscribe(parent.Elevator.Door));

            await base.Subscribe(parent);
        }
    }
}