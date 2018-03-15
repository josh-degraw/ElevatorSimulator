using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElevatorApp.Models.Interfaces;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Represents a panel of buttons used to call an <see cref="Elevator"/>, from outside of the <see cref="Elevator"/> (e.g. on a <see cref="Floor"/>).
    /// </summary>
    public class ElevatorCallPanel : ButtonPanelBase, ISubcriber<ElevatorMasterController>, ISubcriber<(ElevatorMasterController, Elevator)>
    {
        /// <summary>
        /// Call an <see cref="Elevator"/> with the intention of going up
        /// </summary>
        public RequestButton GoingUpButton { get; private set; }

        /// <summary>
        /// Call an <see cref="Elevator"/> with the intention of going down
        /// </summary>
        public RequestButton GoingDownButton { get; private set; }

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
            this.GoingUpButton = new RequestButton(floorNum);
            this.GoingDownButton = new RequestButton(floorNum);
        }

        /// <summary>
        /// Create a new <see cref="ElevatorCallPanel"/>
        /// </summary>
        public ElevatorCallPanel()
        {
        }

        private bool _subscribed;
        bool ISubcriber<ElevatorMasterController>.Subscribed => _subscribed;

        /// <inheritdoc />
        public Task Subscribe(ElevatorMasterController masterController)
        {
            if (_subscribed)
                return Task.CompletedTask;

            this.GoingUpButton.OnPushed += async (sender, e) =>
            {
                await masterController.Dispatch(e).ConfigureAwait(false);
            };

            this._subscribed = true;

            return Task.CompletedTask;
        }


        /// <summary>
        /// Perform the necessary steps to subscribe to the target.
        /// </summary>
        /// <param name="parent">The item this object will be subscribed to</param>
        public override async Task Subscribe((ElevatorMasterController, Elevator) parent)
        {
            var thisBtn = this.FloorButtons.FirstOrDefault(b => b.FloorNumber == this.FloorNumber);

            // If the button is for this floor, just remove it
            if (thisBtn != null)
            {
                this.FloorButtons.Remove(thisBtn);
            }

            await base.Subscribe(parent);

        }

        /// <summary>
        /// Create an <see cref="ElevatorCallPanel"/> on the top <see cref="Floor"/>
        /// </summary>
        /// <param name="floorNum"></param>
        /// <returns>
        /// An <see cref="ElevatorCallPanel"/> with no <see cref="GoingUpButton"/>
        /// </returns>
        public static ElevatorCallPanel TopFloorPanel(int floorNum) => new ElevatorCallPanel(floorNum) { GoingUpButton = null };

        /// <summary>
        /// Create an <see cref="ElevatorCallPanel"/> on the bottom <see cref="Floor"/>
        /// </summary>
        /// <param name="floorNum"></param>
        /// <returns>
        /// An <see cref="ElevatorCallPanel"/> with no <see cref="GoingDownButton"/>
        /// </returns>
        public static ElevatorCallPanel BottomFloorPanel(int floorNum) => new ElevatorCallPanel(floorNum) { GoingDownButton = null };

    }


}
