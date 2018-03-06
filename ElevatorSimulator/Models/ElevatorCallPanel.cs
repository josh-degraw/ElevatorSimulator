using System.Threading.Tasks;
using ElevatorApp.Models.Interfaces;

namespace ElevatorApp.Models
{
    public class ElevatorCallPanel : ButtonPanelBase, ISubcriber<ElevatorMasterController>
    {
        public RequestButton GoingUpButton { get; private set; }
        public RequestButton GoingDownButton { get; private set; }

        public int FloorNumber { get; }

        public ElevatorCallPanel(int floorNum)
        {
            this.FloorNumber = floorNum;
            this.GoingUpButton = new RequestButton(floorNum);
            this.GoingDownButton = new RequestButton(floorNum);
        }

        public ElevatorCallPanel()
        {
        }

        private bool _subscribed;
        bool ISubcriber<ElevatorMasterController>.Subscribed => _subscribed;

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

        public static ElevatorCallPanel TopFloorPanel(int floorNum) => new ElevatorCallPanel(floorNum) { GoingUpButton = null };

        public static ElevatorCallPanel BottomFloorPanel(int floorNum) => new ElevatorCallPanel(floorNum) { GoingDownButton = null };

    }


}
