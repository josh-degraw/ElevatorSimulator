using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    public class Logger : ModelBase, ILogger
    {
        private Logger()
        {
        }

        public static ILogger Instance { get; } = new Logger();

        public event LogEventHandler OnItemLogged = async (sender, @event) =>
        {
            var self = (Logger)Instance;

            if (self.Loggers.Any())
                await Task.WhenAll(self.Loggers.Select(writer => writer.WriteLineAsync(@event.ToString())))
                .ConfigureAwait(false);

        };

        public static readonly IClock Clock = LocalSystemClock.Instance;

        private readonly ConcurrentBag<TextWriter> Loggers = new ConcurrentBag<TextWriter>();
        public IList<LogEvent> Events { get; } = new AsyncObservableCollection<LogEvent>();

        public static void LogEvent(string message, params (object, object)[] parameters)
        {
            Instance.LogEvent(message, parameters);
        }

        public static void LogEvent(LogEvent message)
        {
            Instance.LogEvent(message);
        }

        void ILogger.LogEvent(string message, params (object, object)[] parameters)
        {
            ((ILogger)this).LogEvent(new LogEvent(message, Clock.GetCurrentInstant(), parameters));
        }


        void ILogger.LogEvent(LogEvent message)
        {
            try
            {
                Events.Add(message);
                OnItemLogged(this, message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void LogEvent(string message, (object, object) parameter)
        {
            LogEvent(message, parameters: new[] { parameter });
        }

        public void AddLogger(TextWriter writer)
        {
            Loggers.Add(writer);

        }

        public void ClearItems()
        {
            this.Events.Clear();

        }

        public delegate void LogEventHandler(object sender, LogEvent str);

    }

    public struct LogEvent
    {
        public string Name { get; }

        public Instant Timestamp { get; }

        public (object name, object value)[] Parameters { get; }

        public LogEvent(string name, Instant timeStamp, (object name, object value)[] parameters = null)
        {
            this.Name = name;
            this.Timestamp = timeStamp;
            this.Parameters = parameters ?? new(object name, object value)[] { };
        }

        public const string TIMESTAMP_FORMAT = "HH:mm:ss:ff";

        public override string ToString()
        {
            string timeStamp = this.Timestamp.ToString(TIMESTAMP_FORMAT, null);
            string baseStr = $"{timeStamp} --\t{this.Name}";

            if (this.Parameters?.Length == 0)
                return baseStr;

            return baseStr + "\n" + this.Parameters?.Select(param => $"\t\t{param.name}: {param.value}").ToDelimitedString($"\n") ?? "";
        }

        public LogEvent(string name, params (object name, object value)[] parameters) : this(name, Logger.Clock.GetCurrentInstant(), parameters)
        {
        }
    }


}
