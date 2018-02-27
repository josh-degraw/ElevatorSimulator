using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorApp.Util.Annotations;
using NodaTime;

namespace ElevatorApp.Util
{
    /// <inheritdoc />
    /// <summary>
    /// Class copied from NodaTime <see cref="T:NodaTime.SystemClock" />, to show the local system time instead of UTC
    /// </summary>
    public sealed class LocalSystemClock : IClock
    {
        [NotNull]
        public static LocalSystemClock Instance { get; } = new LocalSystemClock();

        /// <summary>Constructor present to prevent external construction.</summary>
        private LocalSystemClock()
        {
        }
        
        public Instant GetCurrentInstant() => NodaConstants.BclEpoch.PlusTicks(DateTime.Now.Ticks);

    }
}
