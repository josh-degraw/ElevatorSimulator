using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace ElevatorApp.Core
{
    public delegate void LogEventHandler(object sender, string str);

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
            return $"{this.Timestamp}\t- {this.Name}";
        }

        public static implicit operator Event(string str)
        {
            return new Event(str, SystemClock.Instance.GetCurrentInstant());
        }
    }

    public static class Logger
    {
        public static event LogEventHandler OnItemLogged;
        private static readonly IClock clock = SystemClock.Instance;
        private static SortedList<Instant, Event> Events = new SortedList<Instant, Event>();

        private static readonly ConcurrentBag<TextWriter> Loggers = new ConcurrentBag<TextWriter>();

        public static void LogEvent(Event message, [CallerMemberName] string callerMember = null)
        {
            Parallel.ForEach(Loggers, writer =>
            {
                writer.WriteLine(message);
            });
        }

        public static async Task LogEventAsync(Event message, [CallerMemberName] string callerMember = null)
        {
            await Task.WhenAll(Loggers.Select(writer => writer.WriteLineAsync(message.ToString()))).ConfigureAwait(false);
        }
    }
}
