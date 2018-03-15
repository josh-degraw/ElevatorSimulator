using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ElevatorApp.Models;
using NodaTime;
using MoreLinq;

#pragma warning disable CS4026 // The CallerMemberNameAttribute will have no effect because it applies to a member that is used in contexts that do not allow optional arguments
#pragma warning disable CS1066 // The default value specified will have no effect because it applies to a member that is used in contexts that do not allow optional arguments

namespace ElevatorApp.Util
{
    
    /// <summary>
    /// Singleton object used to log events. Can be accessed statically or via <see cref="Logger.Instance"/>.
    /// </summary>
    public class Logger : ModelBase, ILogger
    {
        private Logger()
        {
        }

        /// <summary>
        /// Returns the singleton instance of the <see cref="Logger"/>. Useful when it's easier to access via instance property (e.g. in GUI controls)
        /// </summary>
        public static ILogger Instance { get; } = new Logger();

        /// <summary>
        /// Invoked whenever a new <see cref="Event"/> is logged. Can be used to trigger calculations, update text fields, etc.
        /// </summary>
        public event LogEventHandler ItemLogged = async (sender, @event) =>
        {
            var self = (Logger)Instance;

            if (self._loggers.Any())
                await Task.WhenAll(self._loggers.Select(writer => writer.WriteLineAsync(@event.ToString())))
                .ConfigureAwait(false);

        };
        
        private readonly ConcurrentBag<TextWriter> _loggers = new ConcurrentBag<TextWriter>();

        private readonly AsyncObservableCollection<Event> _events = new AsyncObservableCollection<Event>();

        /// <summary>
        /// A collection of the <see cref="Event"/>s that have been logged 
        /// </summary>
        public IReadOnlyCollection<Event> Events => _events;

        /// <summary>
        /// Log a new <see cref="Event"/>
        /// </summary>
        /// <param name="name">The name of the event</param>
        /// <param name="parameters">Any parameters to be associated with the <see cref="Event"/></param>
        /// <example>
        /// Logger.LogEvent("event name", ("param1", 1), ("param2", 2));
        /// </example>
        public static void LogEvent(string name, params (object, object)[] parameters)
        {
            Instance.LogEvent(name, parameters);
        }

        void ILogger.LogEvent(string name, params (object, object)[] parameters)
        {
            ((ILogger)this).LogEvent(new Event(name, parameters));
        }

        void ILogger.LogEvent(Event message)
        {
            try
            {
                _events.Add(message);
                ItemLogged(this, message);
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message);
            }
        }
        
        /// <summary>
        /// Add a <see cref="TextWriter"/> to automatically write any new log item as it's added.
        /// </summary>
        /// <param name="writer"></param>
        public void AddLogger(TextWriter writer)
        {
            _loggers.Add(writer);
        }


        /// <summary>
        /// Removes all <see cref="Event"/>s logged up to this point.
        /// </summary>
        public void ClearItems()
        {
            this._events.Clear();
        }

        /// <summary>
        /// Handle 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="str"></param>
        public delegate void LogEventHandler(object sender, Event str);
    }

    /// <summary>
    /// Represents an item logged by <see cref="Logger"/>. Holds a name, timestamp, and optional descriptive parameters.
    /// </summary>
    public struct Event
    {
        /// <summary>
        /// The name of the <see cref="Event"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The <see cref="Instant"/> This <see cref="Event"/> occurred
        /// </summary>
        public Instant Timestamp { get; }

        /// <summary>
        /// Optional parameters to further describe what happened with this <see cref="Event"/>
        /// </summary>
        /// <example>
        /// var e = new Event("event", ("param1", 1), ("param2", 2));
        /// </example>
        public (object name, object value)[] Parameters { get; }

        private Event(string name, Instant timeStamp, (object name, object value)[] parameters)
        {
            this.Name = name;
            this.Timestamp = timeStamp;
            this.Parameters = parameters ?? new(object name, object value)[] { };
        }

        /// <summary>
        /// The format of the string when serialized
        /// </summary>
        public const string TIMESTAMP_FORMAT = "HH:mm:ss:ff";

        /// <summary>
        /// Returns a string representation of this <see cref="Event"/>
        /// </summary>
        /// <returns>The <see cref="Timestamp"/> of this <see cref="Event"/>, followed by the <see cref="Name"/>, with any <see cref="Parameters"/> appearing below</returns>
        public override string ToString()
        {
            string timeStamp = this.Timestamp.ToString(TIMESTAMP_FORMAT, null);
            string baseStr = $"{timeStamp} --\t{this.Name}";

            if (this.Parameters?.Length == 0)
                return baseStr;

            return baseStr + "\n" + this.Parameters?.Select(param => $"\t\t{param.name}: {param.value}").ToDelimitedString($"\n") ?? "";
        }

        /// <summary>
        /// Constructs a new <see cref="Event"/>
        /// </summary>
        /// <param name="name">The name of the <see cref="Event"/></param>
        /// <param name="parameters">Optional parameters to further describe what happened with this <see cref="Event"/></param>
        public Event(string name, params (object name, object value)[] parameters) : this(name, LocalSystemClock.GetCurrentInstant(), parameters)
        {
        }
    }


}
