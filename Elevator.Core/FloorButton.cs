using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorApp.Core
{
    public class FloorButton : IFloorBoundButton
    {
        public string Label { get; }
        public void Push()
        {
            throw new NotImplementedException();
        }

        public event EventHandler OnPushed;
        public int FloorNum { get; }
    }
}
