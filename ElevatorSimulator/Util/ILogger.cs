using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace ElevatorApp.Util
{
    public interface ILogger
    {
        List<Logger.Event> Events { get; }

        event Logger.LogEventHandler OnItemLogged;

        void AddLogger(TextWriter writer);
        void ClearItems();
        void LogEvent(Logger.Event message, [CallerMemberName] string callerMember = null);
        void LogEvent(string message, [CallerMemberName] string callerMember = null);
    }
}