using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ElevatorApp.Models;
using NodaTime;

#pragma warning disable CS4026 // The CallerMemberNameAttribute will have no effect because it applies to a member that is used in contexts that do not allow optional arguments
#pragma warning disable CS1066 // The default value specified will have no effect because it applies to a member that is used in contexts that do not allow optional arguments

namespace ElevatorApp.Util
{

    public class Logger : ModelBase, ILogger, INotifyCollectionChanged
    {
        private Logger()
        {

        }

        public static ILogger Instance { get; } = new Logger();

        public event LogEventHandler OnItemLogged = async (sender, @event) =>
        {
            if (((Logger)Instance).Loggers.Any())
                await Task.WhenAll(((Logger)Instance).Loggers.Select(writer => writer.WriteLineAsync(@event.ToString()))).ConfigureAwait(false);
        };

        public static readonly IClock Clock = LocalSystemClock.Instance;

        private readonly ConcurrentBag<TextWriter> Loggers = new ConcurrentBag<TextWriter>();
        public List<Event> Events { get; } = new List<Event>();

        public static void LogEvent(string message, [CallerMemberName] string callerMember = null) => Instance.LogEvent(message, callerMember);

        public static void LogEvent(Event message, [CallerMemberName] string callerMember = null) => Instance.LogEvent(message, callerMember);

        void ILogger.LogEvent(string message, [CallerMemberName] string callerMember = null) => ((ILogger)this).LogEvent(new Event(message, Clock.GetCurrentInstant()), callerMember);


        void ILogger.LogEvent(Event message, [CallerMemberName] string callerMember = null)
        {
            Events.Add(message);
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems: Events));
            OnItemLogged(callerMember, message);
        }

        public void AddLogger(TextWriter writer)
        {
            Loggers.Add(writer);
        }

        public void ClearItems()
        {
            this.Events.Clear();
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, changedItems: this.Events));
        }

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

            public const string TIMESTAMP_FORMAT = "HH:mm:ss:ff";

            public override string ToString()
            {
                return $"{this.Timestamp.ToString(TIMESTAMP_FORMAT, null)}  --  {this.Name}";
            }

            public Event(string name) : this(name, Clock.GetCurrentInstant())
            {
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }


}
