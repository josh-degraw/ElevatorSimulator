using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NodaTime;

namespace ElevatorApp.Util
{
    
    /// <summary>
    /// Class copied from NodaTime <see cref="T:NodaTime.SystemClock" />, to show the local system time instead of UTC
    /// </summary>
    public sealed class LocalSystemClock : IClock
    {
        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        [NotNull]
        public static IClock Instance { get; } = new LocalSystemClock();

        /// <summary>Constructor present to prevent external construction.</summary>
        private LocalSystemClock()
        {
        }
        
        Instant IClock.GetCurrentInstant() => NodaConstants.BclEpoch.PlusTicks(DateTime.Now.Ticks);

        /// <summary>
        /// Static helper to get the current instant
        /// </summary>
        /// <returns>The current <see cref="Instant"/> in time</returns>
        public static Instant GetCurrentInstant() => Instance.GetCurrentInstant();
    }
}
