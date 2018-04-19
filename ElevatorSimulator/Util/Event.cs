using System.Linq;
using MoreLinq;
using NodaTime;

namespace ElevatorApp.Util
{
    /// <summary>
    /// Represents an item logged by <see cref="Logger"/>. Holds a name, timestamp, and optional descriptive parameters.
    /// </summary>
    public struct Event
    {
        /// <summary>
        /// The format of the string when serialized
        /// </summary>
        public const string TIMESTAMP_FORMAT = "HH:mm:ss:ff";

        private Event(string name, Instant timeStamp, (object name, object value)[] parameters)
        {
            this.Name = name;
            this.Timestamp = timeStamp;
            this.Parameters = parameters ?? new(object name, object value)[] { };
        }

        /// <summary>
        /// Constructs a new <see cref="Event"/>
        /// </summary>
        /// <param name="name">The name of the <see cref="Event"/></param>
        /// <param name="parameters">Optional parameters to further describe what happened with this <see cref="Event"/></param>
        public Event(string name, params (object name, object value)[] parameters) : this(name, LocalSystemClock.GetCurrentInstant(), parameters)
        {
        }

        /// <summary>
        /// The name of the <see cref="Event"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Optional parameters to further describe what happened with this <see cref="Event"/>
        /// </summary>
        /// <example>var e = new Event("event", ("param1", 1), ("param2", 2));</example>
        public (object name, object value)[] Parameters { get; }

        /// <summary>
        /// The <see cref="Instant"/> This <see cref="Event"/> occurred
        /// </summary>
        public Instant Timestamp { get; }

        /// <summary>
        /// Returns a string representation of this <see cref="Event"/>
        /// </summary>
        /// <returns>
        /// The <see cref="Timestamp"/> of this <see cref="Event"/>, followed by the <see cref="Name"/>, with any
        /// <see cref="Parameters"/> appearing below
        /// </returns>
        public override string ToString()
        {
            return this.ToString(true);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="includeTimeStamp">if set to <c>true</c> include the time stamp in the string.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public string ToString(bool includeTimeStamp)
        {
            string baseStr = $"{this.Name} ";
            if (includeTimeStamp)
            {
                string timeStamp = this.Timestamp.ToString(TIMESTAMP_FORMAT, null);
                baseStr = $"{timeStamp} -- {baseStr}";
            }

            if (this.Parameters?.Length == 0)
                return baseStr;

            return
                baseStr
                + (this.Parameters?.Length == 1 ? "" : "\n")
                + this.Parameters?.Select(param => $"{param.name + ":"} {param.value}").ToDelimitedString($"\n") ?? "";
        }
    }
}