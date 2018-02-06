using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorApp.Core
{
    public class Door
    {
        public DoorState DoorState { get; set; }

        public event EventHandler OnOpening;
        public event EventHandler OnOpened;
        public event EventHandler OnClosing;
        public event EventHandler OnClosed;
    }
}
