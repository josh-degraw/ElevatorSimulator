using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace ElevatorApp.Core
{
    public delegate void LogEventHandler(object sender, Event str);

    public struct Event
    {
        public string Name { get; }

        public Instant Timestamp { get; }

        public Event(string name, Instant timeStamp)
        {
            this.Name = name;
            this.Timestamp = timeStamp;
        }

        public override string ToString()
        {
            return $"{this.Timestamp:HH:mm:ss:ff}  --  {this.Name}";
        }

        public Event(string name) : this(name, Logger.Clock.GetCurrentInstant())
        {
        }
    }
    
    public static class Logger
    {
        public static event LogEventHandler OnItemLogged = async (sender, @event) =>
        {
            if (Loggers.Any())
                await Task.WhenAll(Loggers.Select(writer => writer.WriteLineAsync(@event.ToString()))).ConfigureAwait(false);
        };

        public static readonly IClock Clock = SystemClock.Instance;
        
        private static readonly ConcurrentBag<TextWriter> Loggers = new ConcurrentBag<TextWriter>();
        public static List<Event> Events { get; } = new List<Event>();

        public static void LogEvent(string message, [CallerMemberName] string callerMember = null) => LogEvent(new Event(message, Clock.GetCurrentInstant()));

        public static void LogEvent(Event message, [CallerMemberName] string callerMember = null)
        {
            Events.Add(message);
            OnItemLogged?.Invoke(callerMember, message);
        }

        public static void AddLogger(TextWriter writer)
        {
            Loggers.Add(writer);
        }


    }


}
