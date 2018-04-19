using ElevatorApp.Util;
using System.Linq;
using System.Threading.Tasks;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Represents a panel of buttons used to call an <see cref="Elevator"/>, from outside of the <see cref="Elevator"/>
    /// (e.g. on a <see cref="Floor"/>).
    /// </summary>
    public class ElevatorCallPanel : ButtonPanelBase
    {
        /// <summary>
        /// The actual collection of <see cref="FloorButton"/> s
        /// </summary>
        protected override AsyncObservableCollection<FloorButton> _floorButtons { get; }

        /// <summary>
        /// The number of the <see cref="Floor"/> this <see cref="ElevatorCallPanel"/> is on
        /// </summary>
        public int FloorNumber { get; }

        /// <summary>
        /// Create a new <see cref="ElevatorCallPanel"/>
        /// </summary>
        /// <param name="floorNum">The number of the <see cref="Floor"/> this <see cref="ElevatorCallPanel"/> is on</param>
        public ElevatorCallPanel(int floorNum)
        {
            this.FloorNumber = floorNum;
            this._floorButtons = new AsyncObservableCollection<FloorButton>
            {
                new RequestButton(floorNum, 4),
                new RequestButton(floorNum, 3),
                new RequestButton(floorNum, 2),
                new RequestButton(floorNum, 1),
            };
        }

        /// <inheritdoc/>
        /// <summary>
        /// Perform the necessary steps to subscribe to the target.
        /// </summary>
        /// <param name="masterController">The item this object will be subscribed to</param>
        public override async Task Subscribe(ElevatorMasterController masterController)
        {
            if (this.Subscribed)
                return;

            var thisBtn = this.FloorButtons.FirstOrDefault(b =>
            {
                if (b is RequestButton r)
                    return r.DestinationFloor == this.FloorNumber;

                return b.FloorNumber == this.FloorNumber;
            });

            // If the button is for this floor, just remove it
            if (thisBtn != null)
            {
                this._floorButtons.Remove(thisBtn);
            }

            await base.Subscribe(masterController);

            this.Subscribed = true;
        }
    }
}