using System;
using System.Collections.Generic;
using System.Text;
using ElevatorApp.Core.Interfaces;

namespace ElevatorApp.Core.Models
{
    public class FloorButton : Button<int>, IFloorBoundButton
    {
        public override string Label => this.FloorNum.ToString();

        public int FloorNum { get; }

        public FloorButton(int floorNum, bool active = false):base($"FloorBtn {floorNum}")
        {
            this.FloorNum = floorNum;
            this.Active = active;
        }

        public override void Push()
        {

            this.Pushed(this, this.FloorNum);
        }

    }
}
