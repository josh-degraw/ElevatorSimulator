using System;
using System.Collections.Generic;
using System.Text;
using ElevatorApp.Core.Interfaces;

namespace ElevatorApp.Core.Models
{
    public class FloorButton : Button<int>, IFloorBoundButton, ISubcriber<(ElevatorMasterController, Elevator)>
    {
        public override string Label => this.FloorNum.ToString();
        public override event EventHandler<int> OnPushed;

        public int FloorNum { get; }

        public FloorButton(int floorNum, bool active = false) : base($"FloorBtn {floorNum}")
        {
            this.FloorNum = floorNum;
            this._active = active;
        }

        public override void Push()
        {
            this.Pushed(this, this.FloorNum);
        }

        public void Subscribe((ElevatorMasterController, Elevator) parent)
        {
            var (controller, elevator) = parent;
            this.OnPushed += (a, b) =>
            {
                controller.Dispatch(this.FloorNum, elevator.CurrentFloor > this.FloorNum ? Direction.Down : Direction.Up);
            };
        }
    }
}
