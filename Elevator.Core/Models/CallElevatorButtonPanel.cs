using System;
using System.Collections.Generic;
using System.Text;
using ElevatorApp.Core.Interfaces;

namespace ElevatorApp.Core.Models
{
    public class CallElevatorButtonPanel : ICallElevatorButtonPanel
    {
        public RequestButton GoingUpButton { get; }
        public int FloorNumber { get; }

        public RequestButton GoingDownButton { get; }

        public CallElevatorButtonPanel(int floorNum)
        {
            this.FloorNumber = floorNum;
            this.GoingUpButton = new RequestButton(floorNum);
            this.GoingDownButton = new RequestButton(floorNum);
        }

        public CallElevatorButtonPanel()
        {
            
        }

        public void Subscribe(ElevatorMasterController masterController)
        {
            this.GoingUpButton.OnPushed += RequestButton_OnPushed;

            void RequestButton_OnPushed(object sender, (int, Direction) e)
            {
                var (floor, direction) = e;
                masterController.QueueElevator(floor, direction);
            }
        }
        
    }
}
