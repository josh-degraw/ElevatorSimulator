using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;
using NodaTime;

namespace ElevatorApp.Models
{
    /// <summary>
    /// EventArgs class 
    /// </summary>
    public class DoorStateChangeEventArgs : EventArgs
    {
        /// <summary>
        /// If set to true, the next operation(s) in the chain will not be processed.
        /// <para>This may be set in any event handler</para>
        /// </summary>
        public bool CancelOperation { get; set; }
    }

    /// <summary>
    /// Handle the changing state of a <see cref="Door"/>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void DoorStateChangeRequestHandler(object sender, DoorStateChangeEventArgs args);

    /// <summary>
    /// Represents the door of the <see cref="Elevator"/>
    /// </summary>
    public class Door : ModelBase, ISubcriber<Elevator>
    {
        //  private readonly SemaphoreSlim mutex = new SemaphoreSlim(1);

        /// <summary>
        /// The number of seconds it takes for the <see cref="Door"/> to close and to open
        /// </summary>
        public const int TRANSITION_TIME_SECONDS = 2;

        /// <summary>
        /// The number of seconds the <see cref="Door"/> stays open
        /// </summary>
        public const int TIME_SPENT_OPEN_SECONDS = 5;

        /// <summary>
        /// The number of seconds it takes for the <see cref="Door"/> to close and to open
        /// </summary>            
        private readonly TimeSpan TRANSITION_TIME = TimeSpan.FromSeconds(TRANSITION_TIME_SECONDS);

        /// <summary>
        /// The number of seconds the <see cref="Door"/> stays open
        /// </summary>
        private readonly TimeSpan TIME_SPENT_OPEN = TimeSpan.FromSeconds(TIME_SPENT_OPEN_SECONDS);

        #region Event Handlers

        /// <summary>
        /// Invoked when the <see cref="Door"/> has just started Opening
        /// </summary>
        public virtual event DoorStateChangeRequestHandler Opening;

        /// <summary>
        /// Invoked when the <see cref="Door"/> has just  Opened
        /// </summary>
        public virtual event DoorStateChangeRequestHandler Opened;

        /// <summary>
        /// Invoked when the <see cref="Door"/> has just started Closing
        /// </summary>
        public virtual event DoorStateChangeRequestHandler Closing;

        /// <summary>
        /// Invoked when the <see cref="Door"/> has just Closed
        /// </summary>
        public virtual event DoorStateChangeRequestHandler Closed;

        /// <summary>
        /// Invoked when the <see cref="Door"/> has just been asked to close
        /// </summary>
        public virtual event DoorStateChangeRequestHandler CloseRequested;

        /// <summary>
        /// Invoked when the <see cref="Door"/> has just been asked to open
        /// </summary>
        public virtual event DoorStateChangeRequestHandler OpenRequested;

        #endregion

        private DoorState _doorState = DoorState.Closed;

        private readonly object _locker = new object();

        /// <summary>
        /// The state of the <see cref="Door"/>
        /// </summary>
        public DoorState DoorState
        {
            get => _doorState;
            private set
            {
                lock (_locker)
                {
                    Debug.Assert(IsValidStateTransition(_doorState, value), "Invalid state transition", "{0} -> {1}", _doorState, value);
                    SetProperty(ref _doorState, value);
                }
            }
        }

        private bool IsValidStateTransition(DoorState prev, DoorState next)
        {
            if (prev == next)
                return true;

            switch (prev)
            {
                case DoorState.Closed:
                    return next == DoorState.Opening;

                case DoorState.Closing:
                    return next.EqualsAny(DoorState.Closed, DoorState.Opening);

                case DoorState.Opened:
                    return next == DoorState.Closing;

                case DoorState.Opening:
                    return next.EqualsAny(DoorState.Opened, DoorState.Closing);

                default:
                    return false;
            }
        }

        private bool _isCloseRequested = false;

        public bool IsCloseRequested
        {
            get => _isCloseRequested;
            private set => SetProperty(ref _isCloseRequested, value);
        }

        private bool _isOpenRequested = false;

        public bool IsOpenRequested
        {
            get => _isOpenRequested;
            private set => SetProperty(ref _isOpenRequested, value);
        }

        /// <summary>
        /// Constructs a new <see cref="Door"/>
        /// </summary>
        public Door()
        {
            this.OpenRequested += delegate
            {
                IsOpenRequested = true;
                Logger.LogEvent("Door Open requested");
            };
            this.CloseRequested += delegate
            {
                IsCloseRequested = true;
                Logger.LogEvent("Door Close requested");
            };

            this.Closing += delegate { Logger.LogEvent("Door Closing"); };
            this.Closed += delegate { Logger.LogEvent("Door Closed"); };

            this.Opening += delegate { Logger.LogEvent("Door Opening"); };
            this.Opened += delegate { Logger.LogEvent("Door Opened"); };
        }

        /// <summary>
        /// Helper property to know if the door is either already opened or in the process of opening
        /// </summary>
        public bool IsOpenedOrOpening => this.DoorState.EqualsAny(DoorState.Opened, DoorState.Opening);

        /// <summary>
        /// Helper property to know if the door is either already closed or in the process of closing
        /// </summary>
        public bool IsClosedOrClosing => this.DoorState.EqualsAny(DoorState.Closed, DoorState.Closing);


        /// <summary>
        /// Ask the <see cref="Door"/> to open
        /// </summary>
        public async void RequestOpen()
        {
            try
            {
                await RequestOpen(false).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task RequestOpen(bool fromRequestClose)
        {
            // If the door is already opened, don't do anything
            if (this.IsOpenedOrOpening)
                return;

            try
            {
                //Trace.TraceInformation(Environment.StackTrace);

                {
                    var args = new DoorStateChangeEventArgs();

                    this.OpenRequested?.Invoke(this, args);

                    if (!args.CancelOperation)
                    {
                        this.DoorState = DoorState.Opening;
                        this.Opening?.Invoke(this, args);

                        if (!args.CancelOperation)
                        {
                            await Task.Delay(TRANSITION_TIME).ConfigureAwait(false);

                            if (this.DoorState != DoorState.Opening)
                                throw new InvalidOperationException("Do not modify the door state while it is transitioning");

                            this.DoorState = DoorState.Opened;
                            this.Opened?.Invoke(this, args);
                            await Task.Delay(TIME_SPENT_OPEN).ConfigureAwait(false);
                        }
                    }

                    this.RequestClose();

                }

            }
            catch (Exception ex)
            {
                Logger.LogEvent("Error", ("Message", ex.Message));
            }
        }


        public async Task WaitForDoorToClose()
        {
            if (DoorState == DoorState.Closed)
                return;

            if (DoorState != DoorState.Closing)
                this.RequestClose();

            while (DoorState != DoorState.Closed)
            {
                await Task.Delay(100).ConfigureAwait(false);
            }
        }

        public async Task WaitForDoorToOpen()
        {
            if (DoorState == DoorState.Opened)
                return;

            if (DoorState != DoorState.Opening)
                this.RequestOpen();

            while (DoorState != DoorState.Opened)
            {
                await Task.Delay(100).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// Ask the <see cref="Door"/> to close
        /// </summary>
        /// <returns></returns>
        public async void RequestClose()
        {
            // If the door is already closed, don't do anything
            if (this.IsClosedOrClosing)
                return;

            // await mutex.WaitAsync().ConfigureAwait(false);
            try
            {
                //Trace.TraceInformation(Environment.StackTrace);
                var args = new DoorStateChangeEventArgs();

                this.CloseRequested?.Invoke(this, args);
                if (!args.CancelOperation)
                {
                    this.DoorState = DoorState.Closing;
                    this.Closing?.Invoke(this, args);

                    if (!args.CancelOperation)
                    {
                        await Task.Delay(TRANSITION_TIME).ConfigureAwait(false);
                    }
                }

                if (args.CancelOperation)
                {
                    await this.RequestOpen(true).ConfigureAwait(false);
                    return; // Return here because request open calls back here, so we don't need to do the rest again
                }

                if (this.DoorState == DoorState.Closing)
                {
                    this.DoorState = DoorState.Closed;
                    this.Closed?.Invoke(this, args);
                }
            }
            finally
            {
                //mutex.Release();
            }
        }


        /// <summary>
        /// Represents if this <see cref="Door"/> has been subscribed
        /// </summary>
        public bool Subscribed { get; private set; }

        /// <summary>
        /// Subscribe this <see cref="Door"/> to its parent <see cref="Elevator"/>
        /// </summary>
        /// <param name="elevator"></param>
        public Task Subscribe(Elevator elevator)
        {
            if (Subscribed)
                return Task.CompletedTask;
            try
            {

                elevator.Arrived += (e, args) =>
                {
                    try
                    {
                        this.RequestOpen();
                    }
                    catch (Exception ex)
                    {
                        Debug.Fail(ex.Message);
                    }
                };

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
