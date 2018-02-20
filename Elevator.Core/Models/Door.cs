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
        #region Event Handlers

        public event DoorStateChangeRequestHandler OnOpening;
        public event DoorStateChangeRequestHandler OnOpened;
        public event DoorStateChangeRequestHandler OnClosing;
        public event DoorStateChangeRequestHandler OnClosed;

        public event DoorStateChangeRequestHandler OnCloseRequested;

        private void DoorClosed(object sender, DoorStateChangeEventArgs args)
        {
            this.DoorState = DoorState.Closed;
        }

        private void DoorOpened(object sender, DoorStateChangeEventArgs args)
        {
            this.DoorState = DoorState.Opened;
        }

        private void DoorOpening(object sender, DoorStateChangeEventArgs args)
        {
            this.DoorState = DoorState.Opening;
        }

        private void DoorClosing(object sender, DoorStateChangeEventArgs args)
        {
            this.DoorState = DoorState.Closing;
        }

        private void CloseRequested(object sender, DoorStateChangeEventArgs args)
        {
            this.DoorState = DoorState.Closing;
        }

        #endregion

        public DoorState DoorState { get; set; } = DoorState.Closed;

        public Door()
        {
            this.OnOpened += this.DoorOpened;
            this.OnClosed += this.DoorClosed;
            this.OnClosing += this.DoorClosing;
            this.OnOpening += this.DoorOpening;
            this.OnCloseRequested = this.CloseRequested;
        }

        public void Open()
        {
            var args = new DoorStateChangeEventArgs();
            this.OnOpening?.Invoke(this, args);
            this.OnOpened?.Invoke(this, args);
        }

        public void RequestClose()
        {
            var args = new DoorStateChangeEventArgs();

            this.OnCloseRequested?.Invoke(this, args);
            if (args.CancelOperation)
                return;

            this.OnClosing?.Invoke(this, args);
            if (args.CancelOperation)
                return;

            this.OnClosed?.Invoke(this, args);

        }

    }
}
