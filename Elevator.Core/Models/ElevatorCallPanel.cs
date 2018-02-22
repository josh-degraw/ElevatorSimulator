using System;
using System.Collections.Generic;
using System.Text;
using ElevatorApp.Core.Interfaces;

namespace ElevatorApp.Core.Models
{
    public class ElevatorCallPanel : IElevatorCallPanel, ISubcriber<ElevatorMasterController>
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

        public void Subscribe(ElevatorMasterController masterController)
        {
            this.GoingUpButton.OnPushed += RequestButton_OnPushed;

            void RequestButton_OnPushed(object sender, ElevatorCall e)
            {
                masterController.Dispatch(e);
            }
        }

        public static ElevatorCallPanel TopFloorPanel(int floorNum) => new ElevatorCallPanel(floorNum) { GoingUpButton = null };

        public static ElevatorCallPanel BottomFloorPanel(int floorNum) => new ElevatorCallPanel(floorNum) { GoingDownButton = null };

    }


}
