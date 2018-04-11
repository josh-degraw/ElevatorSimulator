﻿using System;
using System.Diagnostics;
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
        /// Ask the <see cref="Door"/> to open
        /// </summary>
        public async Task RequestOpen()
        {
            await RequestOpen(false).ConfigureAwait(false);
        }

        private async Task RequestOpen(bool fromRequestClose)
        {
            try
            {
                //Trace.TraceInformation(Environment.StackTrace);
                // If the door is already opened, don't do anything
                if (this.IsNotOpenedOrOpening)
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

                    if (!fromRequestClose)
                        await this.RequestClose();
                }

            }
            catch (Exception ex)
            {
                Logger.LogEvent("Error", ("Message", ex.Message));
            }
        }

        public bool IsNotOpenedOrOpening => this.DoorState != DoorState.Opened && DoorState != DoorState.Opening;

        public bool IsNotClosedOrClosing => this.DoorState != DoorState.Closed && this.DoorState != DoorState.Closing;


        /// <summary>
        /// Ask the <see cref="Door"/> to close
        /// </summary>
        /// <returns></returns>
        public async Task RequestClose()
        {
            //Trace.TraceInformation(Environment.StackTrace);
            var args = new DoorStateChangeEventArgs();
            // If the door is already closed, don't do anything
            if (this.IsNotClosedOrClosing)
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

                if (args.CancelOperation)
                {
                    await this.RequestOpen(true);
                }
            }

            if (this.DoorState == DoorState.Closing)
            {
                this.DoorState = DoorState.Closed;
                this.Closed?.Invoke(this, args);
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
            try
            {
                if (Subscribed)
                    return Task.CompletedTask;

                elevator.ButtonPanel.CloseDoorButton.OnPushed += async (a, b) =>
                {
                    try
                    {
                        if (elevator.Direction == Direction.None)
                            await this.RequestClose();
                    }
                    catch (Exception e)
                    {
                        Debug.Fail(e.Message);
                    }
                };

                elevator.ButtonPanel.OpenDoorButton.OnPushed += async (a, b) =>
                {
                    try
                    {
                        if (elevator.Direction == Direction.None)
                            await this.RequestOpen();
                    }
                    catch (Exception e)
                    {
                        Debug.Fail(e.Message);
                    }
                };

                elevator.Arrived += async (e, args) =>
                {
                    try
                    {
                        await this.RequestOpen();
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
