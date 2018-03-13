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

        public const int
            TRANSITION_TIME_SECONDS = 2,
            TIME_SPENT_OPEN_SECONDS = 8;

        private readonly TimeSpan
            TRANSITION_TIME = TimeSpan.FromSeconds(TRANSITION_TIME_SECONDS),
            TIME_SPENT_OPEN = TimeSpan.FromSeconds(TIME_SPENT_OPEN_SECONDS);

        #region Event Handlers

        public virtual event DoorStateChangeRequestHandler Opening;
        public virtual event DoorStateChangeRequestHandler Opened;
        public virtual event DoorStateChangeRequestHandler Closing;
        public virtual event DoorStateChangeRequestHandler Closed;

        public virtual event DoorStateChangeRequestHandler CloseRequested;
        public virtual event DoorStateChangeRequestHandler OpenRequested;

        #endregion

        private DoorState _doorState = DoorState.Closed;

        public DoorState DoorState
        {
            get => _doorState;
            private set => SetProperty(ref _doorState, value);

        }

        public Door()
        {
            this.OpenRequested += delegate { Logger.LogEvent("Door Open requested"); };
            this.CloseRequested += delegate { Logger.LogEvent("Door Close requested"); };
            this.Closing += delegate { Logger.LogEvent("Door Closing"); };
            this.Closed += delegate { Logger.LogEvent("Door Closed"); };

            this.Opening += delegate { Logger.LogEvent("Door Opening"); };
            this.Opened += delegate { Logger.LogEvent("Door Opened"); };
        }

        public Task RequestOpen()
        {
            return RequestOpen(false);
        }

        public Task RequestClose()
        {
            return RequestClose(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromCloseRequest">Represents if this call was from </param>
        /// <returns></returns>
        private async Task RequestOpen(bool fromCloseRequest)
        {
            try
            {
                // If the door is already opened, don't do anything
                if (this.DoorState != DoorState.Opened && DoorState != DoorState.Opening)
                {
                    var args = new DoorStateChangeEventArgs();

                    this.OpenRequested?.Invoke(this, args);

                    if (!args.CancelOperation)
                    {
                        this.DoorState = DoorState.Opening;
                        this.Opening?.Invoke(this, args);

                        if (!args.CancelOperation)
                        {
                            await Task.Delay(TRANSITION_TIME);

                            if (this.DoorState != DoorState.Opening)
                                throw new InvalidOperationException("Do not modify the door state while it is transitioning");

                            this.DoorState = DoorState.Opened;
                            this.Opened?.Invoke(this, args);
                            await Task.Delay(TIME_SPENT_OPEN);
                        }
                    }

                    await this.RequestClose(true);
                }

            }
            catch (Exception ex)
            {
                Logger.LogEvent("Error", ("Message", ex.Message));
            }
        }

        private async Task RequestClose(bool fromOpenRequest)
        {
            var args = new DoorStateChangeEventArgs();
            // If the door is already closed, don't do anything
            if (this.DoorState != DoorState.Closed && this.DoorState != DoorState.Closing)
            {
                this.CloseRequested?.Invoke(this, args);
                if (!args.CancelOperation)
                {
                    this.DoorState = DoorState.Closing;
                    this.Closing?.Invoke(this, args);
                    if (!args.CancelOperation)
                    {
                        await Task.Delay(TRANSITION_TIME);
                    }
                }
            }

            if (args.CancelOperation)
            {
                await this.RequestOpen(true);
            }
            else if (this.DoorState == DoorState.Closing)
            {
                this.DoorState = DoorState.Closed;
                this.Closed?.Invoke(this, args);
            }
        }


        public bool Subscribed { get; private set; }

        public Task Subscribe(Elevator elevator)
        {
            try
            {
                if (Subscribed)
                    return Task.CompletedTask;

                elevator.ButtonPanel.CloseDoorButton.OnPushed += async (a, b) =>
                {
                    if (elevator.State == ElevatorState.Idle)
                        await this.RequestClose().ConfigureAwait(false);
                };

                elevator.ButtonPanel.OpenDoorButton.OnPushed += async (a, b) =>
                {
                    if (elevator.State == ElevatorState.Idle)
                        await this.RequestOpen().ConfigureAwait(false);
                };

                elevator.Arrived += async (e, args) => await this.RequestOpen().ConfigureAwait(false);
                
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
