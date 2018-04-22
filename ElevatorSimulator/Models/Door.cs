using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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

    /// <inheritdoc/>
    /// <summary>
    /// Represents the door of the <see cref="T:ElevatorApp.Models.Elevator"/>
    /// </summary>
    public class Door : ModelBase, ISubcriber<Elevator>
    {
        /// <summary>
        /// Used to prevent multiple threads from changing the state of the <see cref="Door"/>
        /// </summary>
        private readonly ReaderWriterLockSlim _stateChangeLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        // private readonly SemaphoreSlim mutex = new SemaphoreSlim(1);

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
        /// Invoked when the <see cref="Door"/> has just Opened
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

        #endregion Event Handlers

        private DoorState _doorState = DoorState.Closed;

        /// <summary>
        /// The state of the <see cref="Door"/>
        /// </summary>
        public DoorState DoorState
        {
            get => this._doorState;

            private set
            {
                this._stateChangeLock.EnterWriteLock();
                try
                {
                    this.SetProperty(ref this._doorState, value);
                }
                finally
                {
                    this._stateChangeLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Determines whether is valid state transition
        /// </summary>
        /// <param name="prev">The previous.</param>
        /// <param name="next">The next.</param>
        /// <returns><c>true</c> if is a valid state transition; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Gets a value indicating whether a close has been requested.
        /// </summary>
        /// <value><c>true</c> if a close has been requested; otherwise, <c>false</c>.</value>
        public bool IsCloseRequested
        {
            get => this._isCloseRequested;
            private set => this.SetProperty(ref this._isCloseRequested, value);
        }

        private bool _isOpenRequested = false;

        /// <summary>
        /// Gets a value indicating whether an open has been requested.
        /// </summary>
        /// <value><c>true</c> if an open has been requested; otherwise, <c>false</c>.</value>
        public bool IsOpenRequested
        {
            get => this._isOpenRequested;
            private set => this.SetProperty(ref this._isOpenRequested, value);
        }

        /// <summary>
        /// Constructs a new <see cref="Door"/>
        /// </summary>
        public Door()
        {
            this.OpenRequested += delegate { Logger.LogEvent("Door Open requested"); };
            this.CloseRequested += delegate { Logger.LogEvent("Door Close requested"); };

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
                await this.RequestOpen(false).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private bool _closeRequestBegun = false;

        private bool _openRequestBegun = false;

        /// <summary>
        /// Requests the door to open.
        /// </summary>
        /// <param name="fromRequestClose">If the.</param>
        /// <returns></returns>
        private async Task RequestOpen(bool fromRequestClose)
        {
            // If the door is already opened, don't do anything
            if (this.IsOpenedOrOpening)
                return;

            try
            {
                var args = new DoorStateChangeEventArgs();

                this._openRequestBegun = true;
                this.OpenRequested?.Invoke(this, args);

                if (this.IsCloseRequested || args.CancelOperation)
                {
                    // If close has already been requested, just let that call complete
                    if (!this._closeRequestBegun) this.RequestClose(true);
                    return;
                }

                //TODO: Tooltips
                args = new DoorStateChangeEventArgs();

                this.DoorState = DoorState.Opening;
                this.Opening?.Invoke(this, args);

                if (this.IsCloseRequested || args.CancelOperation)
                {
                    // If close has already been requested, just let that call complete
                    if (!this._closeRequestBegun)
                    {
                        this.RequestClose(true);
                    }

                    return;
                }

                await Task.Delay(this.TRANSITION_TIME).ConfigureAwait(false);
                args = new DoorStateChangeEventArgs();
                if (this.DoorState == DoorState.Opening)
                {
                    this.DoorState = DoorState.Opened;
                    this.Opened?.Invoke(this, args);
                }

                await Task.Delay(this.TIME_SPENT_OPEN).ConfigureAwait(true);

                if (this.IsCloseRequested || args.CancelOperation)
                {
                    // If close has already been requested, just let that call complete
                    if (!this._closeRequestBegun) this.RequestClose(true);
                    return;
                }

                this.RequestClose(true);
            }
            catch (Exception ex)
            {
                Logger.LogEvent("Error", ("Message", ex.Message));
            }
            finally
            {
                this._openRequestBegun = false;
            }
        }

        /// <summary>
        /// Waits for door to close. If not already in progress, calls <see cref="RequestClose()"/>
        /// </summary>
        /// <returns></returns>
        public async Task WaitForDoorToClose()
        {
            if (this.DoorState == DoorState.Closed)
                return;

            this.IsCloseRequested = true;

            //if (this.DoorState != DoorState.Closing)
            //    this.RequestClose();

            while (this.DoorState != DoorState.Closed)
            {
                await Task.Delay(100).ConfigureAwait(false);
            }

            this.IsCloseRequested = false;
        }

        /// <summary>
        /// Waits for door to open. If not already in progress, calls <see cref="RequestOpen()"/>
        /// </summary>
        /// <returns></returns>
        public async Task WaitForDoorToOpen()
        {
            if (this.DoorState == DoorState.Opened)
                return;

            if (this.DoorState != DoorState.Opening)
                this.RequestOpen();

            while (this.DoorState != DoorState.Opened)
            {
                await Task.Delay(100).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Ask the <see cref="Door"/> to close
        /// </summary>
        /// <returns></returns>
        public void RequestClose()
        {
            this.RequestClose(false);
        }

        /// <summary>
        /// Requests the door to close.
        /// </summary>
        /// <param name="recursed">If called from <see cref="RequestClose()"/></param>
        private async void RequestClose(bool recursed)
        {
            // If the door is already closed, don't do anything
            if (this.IsClosedOrClosing)
                return;

            // await mutex.WaitAsync().ConfigureAwait(false);
            try
            {
                //Trace.TraceInformation(Environment.StackTrace);
                var args = new DoorStateChangeEventArgs();
                this._closeRequestBegun = true;

                this.CloseRequested?.Invoke(this, args);

                if (args.CancelOperation)
                {
                    if (!this._openRequestBegun)
                        await this.RequestOpen(true).ConfigureAwait(false);
                    return;
                }

                this.DoorState = DoorState.Closing;
                this.Closing?.Invoke(this, args);

                if (args.CancelOperation)
                {
                    if (!this._openRequestBegun)
                        await this.RequestOpen(true).ConfigureAwait(false);
                    return;
                }

                await Task.Delay(this.TRANSITION_TIME).ConfigureAwait(false);

                if (this.DoorState == DoorState.Closing)
                {
                    this.DoorState = DoorState.Closed;

                    this.Closed?.Invoke(this, args);
                }
            }
            finally
            {
                this._closeRequestBegun = false;

                //mutex.Release();
            }
        }

        /// <inheritdoc/>
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
            if (this.Subscribed)
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
                        Trace.TraceError(ex.ToString());
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