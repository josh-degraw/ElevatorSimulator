using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorApp.Core
{
    public class ButtonPanel
    {
        public RequestButton GoingUpButton { get; }
        public RequestButton GoingDownButton { get; }

        public ButtonPanel(int floorNum)
        {
            this.GoingUpButton = new RequestButton(floorNum);
            this.GoingDownButton = new RequestButton(floorNum);

        }
    }
}
