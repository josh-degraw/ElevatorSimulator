using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace ElevatorApp.Util
{
    public interface ILogger
    {
        IReadOnlyCollection<Event> Events { get; }

        event Logger.LogEventHandler ItemLogged;

        void AddLogger(TextWriter writer);
        void ClearItems();
        void LogEvent(string message, params (object, object)[] parameters);
        void LogEvent(Event message);
    }
}