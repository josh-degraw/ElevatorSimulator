using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;

namespace ElevatorApp.Models
{
    public class ButtonPanel : ButtonPanelBase, IButtonPanel, ISubcriber<(ElevatorMasterController, Elevator)>
    {
        public DoorButton OpenDoorButton { get; }
        public DoorButton CloseDoorButton { get; }

        public ButtonPanel()
        {
            this.OpenDoorButton = DoorButton.Open();
            this.CloseDoorButton = DoorButton.Close();
        }

        public void Subscribe((ElevatorMasterController, Elevator) parent)
        {

            //if (Subscribed)
            //    return;

            base.Subscribe(parent);
          

            

            this.Subscribed = true;
        }

    }
}
