using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorApp.Core
{
    public class RequestButton : IButton<(int, Direction)>, IFloorBoundButton
    {
        public string Label { get; }
        public void Push()
        {
            this.OnPushed(this, (this.FloorNum, this.RequestDirection));
            throw new NotImplementedException();
        }

        public Direction RequestDirection { get; set; }

        public int FloorNum { get; }

        public event EventHandler<(int, Direction)> OnPushed = delegate { };

        public RequestButton(int floorNum)
        {
            this.FloorNum = floorNum;

        }
    }
}
