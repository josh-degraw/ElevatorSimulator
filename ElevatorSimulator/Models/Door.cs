using System;
using System.Threading.Tasks;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;
using NodaTime;

namespace ElevatorApp.Models
{
    public class DoorStateChangeEventArgs : EventArgs
    {
        public bool CancelOperation { get; set; }
    }

    public delegate void DoorStateChangeRequestHandler(object sender, DoorStateChangeEventArgs args);


    public class Door : ModelBase, ISubcriber<Elevator>
    {
        private readonly Duration
            TRANSITION_TIME = Duration.FromSeconds(2),
            TIME_SPENT_OPEN = Duration.FromSeconds(10);

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

        public async Task RequestOpen()
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
            await Task.Delay(TRANSITION_TIME.ToTimeSpan());

            if (args.CancelOperation)
            {
                await this.RequestClose();
                return;
            }

            this.DoorState = DoorState.Opened;
            this.OnOpened?.Invoke(this, args);

            await Task.Delay(TIME_SPENT_OPEN.ToTimeSpan());

            await this.RequestClose().ConfigureAwait(false);

        }

        public async Task RequestClose()
        {
            // If the door is already closed, don't do anything
            if (this.DoorState == DoorState.Closed)
            {
                return;
            }

            var args = new DoorStateChangeEventArgs();

            this.OnCloseRequested?.Invoke(this, args);
            if (args.CancelOperation)
            {
                await this.RequestOpen();
                return;
            }

            this.DoorState = DoorState.Closing;
            this.OnClosing?.Invoke(this, args);
            await Task.Delay(TRANSITION_TIME.ToTimeSpan());

            if (args.CancelOperation)
            {
                await this.RequestOpen();
                return;
            }

            this.DoorState = DoorState.Closed;
            this.OnClosed?.Invoke(this, args);
        }

        public bool Subscribed { get; private set; }

        public void Subscribe(Elevator parent)
        {
            //if (Subscribed)
            //    return;

            parent.ButtonPanel.CloseDoorButton.OnPushed += async (a, b) =>
            {
                if (parent.State == ElevatorState.Idle)
                    await this.RequestClose();
            };

            parent.ButtonPanel.OpenDoorButton.OnPushed += async (a, b) =>
            {
                if (parent.State == ElevatorState.Idle)
                    await this.RequestOpen();
            };

            parent.OnArrival += async (e, args) => await this.RequestOpen();


            this.Subscribed = true;
        }

    }
}
