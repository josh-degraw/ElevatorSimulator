using System;
using System.Threading;
using System.Threading.Tasks;
using ElevatorApp.Core.Interfaces;

namespace ElevatorApp.Core.Models
{
    public class DoorStateChangeEventArgs : EventArgs
    {
        public bool CancelOperation { get; set; }
    }

    public delegate void DoorStateChangeRequestHandler(object sender, DoorStateChangeEventArgs args);


    public class Door : ModelBase, ISubcriber<Elevator>, ISubcriber<ButtonPanel>
    {
        private const int TRANSITION_TIME_MS = 500, TIME_SPENT_OPEN = 5 * 1000;

        #region Event Handlers

        public virtual event DoorStateChangeRequestHandler OnOpening;
        public virtual event DoorStateChangeRequestHandler OnOpened;
        public virtual event DoorStateChangeRequestHandler OnClosing;
        public virtual event DoorStateChangeRequestHandler OnClosed;

        public virtual event DoorStateChangeRequestHandler OnCloseRequested;
        public virtual event DoorStateChangeRequestHandler OnOpenRequested;

        private void DoorClosed(object sender, DoorStateChangeEventArgs args)
        {
            Logger.LogEvent("Door Closed");
        }

        private void DoorOpened(object sender, DoorStateChangeEventArgs args)
        {
            Logger.LogEvent("Door Opened");
        }

        private void DoorOpening(object sender, DoorStateChangeEventArgs args)
        {
            Logger.LogEvent("Door Opening");
        }

        private void DoorClosing(object sender, DoorStateChangeEventArgs args)
        {
            Logger.LogEvent("Door Closing");
        }

        private void CloseRequested(object sender, DoorStateChangeEventArgs args)
        {
            Logger.LogEvent("Door Close requested");
        }

        #endregion

        private DoorState _doorState = DoorState.Closed;

        public DoorState DoorState
        {
            get => _doorState;
            set => SetProperty(ref _doorState, value, false);

        }

        public Door()
        {
            this.OnOpenRequested += OpenRequested;
            this.OnCloseRequested = this.CloseRequested;
            this.OnClosing += this.DoorClosing;
            this.OnClosed += this.DoorClosed;

            this.OnOpening += this.DoorOpening;
            this.OnOpened += this.DoorOpened;
        }

        private void OpenRequested(object sender, DoorStateChangeEventArgs args)
        {
        }

        public async void RequestOpen()
        {
            // If the door is already opened, don't do anything
            if (this.DoorState == DoorState.Opened)
            {
                return;
            }
            var args = new DoorStateChangeEventArgs();
            this.OnOpenRequested?.Invoke(this, args);
            if (args.CancelOperation)
                return;
            
            this.DoorState = DoorState.Opening;
            this.OnOpening?.Invoke(this, args);
            await Task.Delay(TRANSITION_TIME_MS);

            if (args.CancelOperation)
            {
                this.RequestClose();
                return;
            }

            await Task.Delay(TRANSITION_TIME_MS);
            this.OnOpened?.Invoke(this, args);
            this.DoorState = DoorState.Opened;

            await Task.Delay(TIME_SPENT_OPEN);

            this.RequestClose();

        }

        public async void RequestClose()
        {
            // If the door is already closed, don't do anything
            if (this.DoorState == DoorState.Closed)
            {
                return;
            }

            var args = new DoorStateChangeEventArgs();

            this.OnCloseRequested?.Invoke(this, args);
            await Task.Delay(TRANSITION_TIME_MS);
            if (args.CancelOperation)
            {
                this.RequestOpen();
                return;
            }

            this.DoorState = DoorState.Closing;
            this.OnClosing?.Invoke(this, args);
            await Task.Delay(TRANSITION_TIME_MS);

            if (args.CancelOperation)
            {
                this.RequestOpen();
                return;
            }

            this.DoorState = DoorState.Closed;
            this.OnClosed?.Invoke(this, args);
        }

        public void Subscribe(Elevator parent)
        {
            this.Subscribe(parent.ButtonPanel);
        }

        public void Subscribe(ButtonPanel parent)
        {
            parent.CloseDoorButton.OnPushed += (a, b) =>
            {
                this.RequestClose();
            };

            parent.OpenDoorButton.OnPushed += (a, b) =>
            {
                this.RequestOpen();
            };
        }
    }
}
