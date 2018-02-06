using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorApp.Core
{
    public class RequestButton : IFloorBoundButton
    {
        public string Label { get; }
        public void Push()
        {
            throw new NotImplementedException();
        }

        public Direction RequestDirection { get; set; }

        public int FloorNum { get; }

        public event EventHandler OnPushed;

        public RequestButton(int floorNum)
        {
            this.FloorNum = floorNum;

        }
    }
}
