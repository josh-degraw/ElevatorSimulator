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

        #endregion

        private DoorState _doorState = DoorState.Closed;

        public DoorState DoorState
        {
            get => _doorState;
            set => SetProperty(ref _doorState, value, false);

        }

        public Door()
        {
            this.OnOpenRequested += delegate { Logger.LogEvent("Door Open requested"); }; 
            this.OnCloseRequested += delegate { Logger.LogEvent("Door Close requested"); };
            this.OnClosing += delegate { Logger.LogEvent("Door Closing"); };
            this.OnClosed += delegate { Logger.LogEvent("Door Closed"); };

            this.OnOpening += delegate { Logger.LogEvent("Door Opening"); };
            this.OnOpened += delegate { Logger.LogEvent("Door Opened"); };
        }
        
        public async Task RequestOpen()
        {
            try
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
                await Task.Delay(TRANSITION_TIME.ToTimeSpan()).ConfigureAwait(false);

                if (args.CancelOperation)
                {
                    await this.RequestClose().ConfigureAwait(false);
                    return;
                }

                this.DoorState = DoorState.Opened;
                this.OnOpened?.Invoke(this, args);

                await Task.Delay(TIME_SPENT_OPEN.ToTimeSpan()).ConfigureAwait(false);

                await this.RequestClose().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogEvent("Error", ("Message", ex.Message));
            }

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

        public Task Subscribe(Elevator parent)
        {
            try
            {
                if (Subscribed)
                    return Task.CompletedTask;

                parent.ButtonPanel.CloseDoorButton.OnPushed += async (a, b) =>
                {
                    if (parent.State == ElevatorState.Idle)
                        await this.RequestClose().ConfigureAwait(false);
                };

                parent.ButtonPanel.OpenDoorButton.OnPushed += async (a, b) =>
                {
                    if (parent.State == ElevatorState.Idle)
                        await this.RequestOpen().ConfigureAwait(false);
                };

                parent.OnArrival += async (e, args) => await this.RequestOpen().ConfigureAwait(false);

                this.Subscribed = true;

            }
            catch (Exception e)
            {
                Logger.LogEvent("Error", ("Message", e.Message));

            }

            return Task.CompletedTask;
        }

    }
}
