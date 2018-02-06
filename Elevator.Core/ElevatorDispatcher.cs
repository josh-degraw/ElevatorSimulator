using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorApp.Core
{
    public class ElevatorDispatcher
    {
        public int Floor { get; }
        public ICollection<ButtonPanel> Buttons { get; }

        public ElevatorDispatcher(int floor, ICollection<ButtonPanel> buttons)
        {
            this.Floor = floor;
            this.Buttons = buttons;

        }
    }
}
