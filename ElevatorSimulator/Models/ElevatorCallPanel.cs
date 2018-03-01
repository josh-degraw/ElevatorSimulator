using ElevatorApp.Models.Interfaces;

namespace ElevatorApp.Models
{
    public class ElevatorCallPanel :ButtonPanelBase, ISubcriber<ElevatorMasterController>
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

        public void Subscribe(ElevatorMasterController masterController)
        {
            //if (Subscribed)
            //    return;

            this.GoingUpButton.OnPushed += RequestButton_OnPushed;

            void RequestButton_OnPushed(object sender, ElevatorCall e)
            {
                masterController.Dispatch(e);
            }

            this._subscribed = true;
        }

        public static ElevatorCallPanel TopFloorPanel(int floorNum) => new ElevatorCallPanel(floorNum) { GoingUpButton = null };

        public static ElevatorCallPanel BottomFloorPanel(int floorNum) => new ElevatorCallPanel(floorNum) { GoingDownButton = null };

    }


}
