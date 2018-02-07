using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorApp.Core
{
    public class DoorStateChangeEventArgs : EventArgs
    {
        public bool CancelOperation { get; set; }
    }

    public delegate void DoorStateChangeRequestHandler(object sender, DoorStateChangeEventArgs args);
    

    public class Door
    {
        public DoorState DoorState { get; set; } = DoorState.Closed;

        
        public event DoorStateChangeRequestHandler OnOpening = delegate { };
        public event DoorStateChangeRequestHandler OnOpened = delegate { };
        public event DoorStateChangeRequestHandler OnClosing = delegate { };
        public event DoorStateChangeRequestHandler OnClosed = delegate { };

        public void Open()
        {
            var args = new DoorStateChangeEventArgs();
            this.OnOpening(this, args);
        }

        public void RequestClose()
        {
            var args = new DoorStateChangeEventArgs();


        }
        
    }
}
