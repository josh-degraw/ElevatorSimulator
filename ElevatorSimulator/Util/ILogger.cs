using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace ElevatorApp.Util
{
    public interface ILogger
    {
        IList<Logger.Event> Events { get; }

        event Logger.LogEventHandler OnItemLogged;

        void AddLogger(TextWriter writer);
        void ClearItems();
        void LogEvent(string message, params (object, object)[] parameters);
        void LogEvent(Logger.Event message);
    }
}