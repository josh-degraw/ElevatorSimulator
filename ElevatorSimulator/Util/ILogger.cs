using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace ElevatorApp.Util
{
    public interface ILogger
    {
        /// <summary>
        /// A collection of the <see cref="Event"/>s that have been logged 
        /// </summary>
        IReadOnlyCollection<Event> Events { get; }

        /// <summary>
        /// Invoked whenever a new <see cref="Event"/> is logged. Can be used to trigger calculations, update text fields, etc.
        /// </summary>
        event Logger.LogEventHandler ItemLogged;

        /// <summary>
        /// Add a <see cref="TextWriter"/> to automatically write any new log item as it's added.
        /// </summary>
        /// <param name="writer"></param>
        void AddLogger(TextWriter writer);

        /// <summary>
        /// Removes all <see cref="Event"/>s logged up to this point.
        /// </summary>
        void ClearItems();

        /// <summary>
        /// Log a new <see cref="Event"/>
        /// </summary>
        /// <param name="name">The name of the event</param>
        /// <param name="parameters">Any parameters to be associated with the <see cref="Event"/></param>
        /// <example>
        /// Logger.LogEvent("event name", ("param1", 1), ("param2", 2));
        /// </example>
        void LogEvent(string name, params (object, object)[] parameters);


        void LogEvent(Event message, int retryTimes = 3);
    }
}