using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElevatorApp.Core.Interfaces;
using ElevatorApp.Core.Models;

namespace ElevatorApp.Core
{
    public class Floor : IFloor
    {
        public int FloorNumber { get; set; }
        public bool ElevatorAvailable { get; private set; }

        public ICollection<CallElevatorButtonPanel> Buttons { get; set; }

        public Floor(int floorNumber, ICollection<CallElevatorButtonPanel> buttons)
        {
            this.FloorNumber = floorNumber;
            this.Buttons = buttons;
        }

        public Floor(int floorNumber, int callPanels = 1)
            : this(floorNumber, Enumerable.Range(0, callPanels).Select(a => new CallElevatorButtonPanel(a)).ToArray())
        {

        }

        public Floor()
        {

        }


    }
}
