using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;

namespace ElevatorApp.Models
{
    ///<inheritdoc/>
    public class ButtonPanel : ButtonPanelBase, ISubcriber<(ElevatorMasterController, Elevator)>
    {
        public DoorButton OpenDoorButton { get; }
        public DoorButton CloseDoorButton { get; }

        ///<inheritdoc/>
        protected override AsyncObservableCollection<FloorButton> _floorButtons { get; }

        ///<inheritdoc/>
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


        ///<inheritdoc/>
        public override async Task Subscribe((ElevatorMasterController, Elevator) parent)
        {
            if (Subscribed)
                return;

            await base.Subscribe(parent);
            await Task.WhenAll(
                OpenDoorButton.Subscribe(parent.Item2.Door),
                CloseDoorButton.Subscribe(parent.Item2.Door));

            this.Subscribed = true;
        }

    }
}
