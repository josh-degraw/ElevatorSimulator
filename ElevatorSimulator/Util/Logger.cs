using ElevatorApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

#pragma warning disable CS4026 // The CallerMemberNameAttribute will have no effect because it applies to a member that is used in contexts that do not allow optional arguments
#pragma warning disable CS1066 // The default value specified will have no effect because it applies to a member that is used in contexts that do not allow optional arguments

namespace ElevatorApp.Util
{
    /// <inheritdoc cref="ILogger"/>
    /// <summary>
    /// Singleton object used to log events. Can be accessed statically or via <see cref="P:ElevatorApp.Util.Logger.Instance"/>.
    /// </summary>
    public class Logger : ModelBase, ILogger
    {
        /// <summary>
        /// The events
        /// </summary>
        private readonly AsyncObservableCollection<Event> _events = new AsyncObservableCollection<Event>();

        /// <summary>
        /// The rw lock
        /// </summary>
        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

        /// <summary>
        /// Invoked whenever a new <see cref="Event"/> is logged. Can be used to trigger calculations, update text
        /// fields, etc.
        /// </summary>
        public event LogEventHandler ItemLogged;

        /// <summary>
        /// Prevents a default instance of the <see cref="Logger"/> class from being created.
        /// </summary>
        private Logger()
        {
            // Trace.Listeners.Add(new EventTraceListener("ElevatorSimulatorEvents.log"));
        }

        /// <summary>
        /// Returns the singleton instance of the <see cref="Logger"/>. Useful when it's easier to access via instance
        /// property (e.g. in GUI controls)
        /// </summary>
        public static ILogger Instance { get; } = new Logger();

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count
        {
            get
            {
                this._rwLock.EnterReadLock();
                try
                {
                    return this._events.Count;
                }
                finally
                {
                    this._rwLock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// A collection of the <see cref="Event"/> s that have been logged
        /// </summary>
        [Obsolete]
        public static IReadOnlyCollection<Event> Events => ((ILogger)Instance).Events;

        /// <summary>
        /// A collection of the <see cref="Event"/> s that have been logged
        /// </summary>
        [Obsolete]
        IReadOnlyCollection<Event> ILogger.Events
        {
            get
            {
                this._rwLock.EnterReadLock();
                try
                {
                    return this._events;
                }
                finally
                {
                    this._rwLock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Log a new <see cref="Event"/>
        /// </summary>
        /// <param name="name">The name of the event</param>
        /// <param name="parameters">Any parameters to be associated with the <see cref="Event"/></param>
        /// <example>Logger.LogEvent("event name", ("param1", 1), ("param2", 2));</example>
        public static void LogEvent(string name, params (object, object)[] parameters)
        {
            Instance.LogEvent(name, parameters);
        }

        /// <summary>
        /// Add a <see cref="TextWriter"/> to automatically write any new log item as it's added.
        /// </summary>
        /// <param name="writer"></param>
        public void AddLogger(TextWriter writer)
        {
            //_loggers.Add(writer);
        }

        /// <summary>
        /// Removes all <see cref="Event"/> s logged up to this point.
        /// </summary>
        public void ClearItems()
        {
            this._rwLock.EnterWriteLock();
            try
            {
                this._events.Clear();
            }
            finally
            {
                this._rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Log a new <see cref="Event"/>
        /// </summary>
        /// <param name="name">The name of the event</param>
        /// <param name="parameters">Any parameters to be associated with the <see cref="Event"/></param>
        /// <example>Logger.LogEvent("event name", ("param1", 1), ("param2", 2));</example>
        void ILogger.LogEvent(string name, params (object, object)[] parameters)
        {
            ((ILogger)this).LogEvent(new Event(name, parameters));
        }

        /// <summary>
        /// Logs the event, with an optional number of retry times you can specify.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="retryTimes">The retry times.</param>
        void ILogger.LogEvent(Event message, int retryTimes = 3)
        {
            int prevCount = this.Count;

            //int i = 0;
            Console.WriteLine(message);
            this.ItemLogged?.Invoke(this, message.ToString());

            //do
            //{
            //    if (_rwLock.TryEnterWriteLock())
            //    {
            //        try
            //        {
            //            _events.Add(message);
            //            ItemLogged?.Invoke(this, message);
            //        }
            //        catch (Exception e)
            //        {
            //            if (prevCount == _events.Count)
            //            {
            //                Console.WriteLine(e.Message);
            //            }
            //        }
            //        finally
            //        {
            //            _rwLock.ExitWriteLock();
            //        }
            //    }

            //} while (_events.Count == prevCount && i++ < retryTimes);
        }

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="str"></param>
        public delegate void LogEventHandler(object sender, string str);
    }
}