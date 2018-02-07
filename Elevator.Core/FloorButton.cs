using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorApp.Core
{
    public class FloorButton : IFloorBoundButton, IButton<int>
    {
        public string Label => this.FloorNum.ToString();

        public event EventHandler<int> OnPushed = delegate { };
        public int FloorNum { get; }

        public FloorButton(int floorNum)
        {
            this.FloorNum = floorNum;
        }

        public void Push()
        {
            this.OnPushed(this, this.FloorNum);
        }

    }
}
