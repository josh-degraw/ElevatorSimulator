using JetBrains.Annotations;
using NodaTime;
using System;

namespace ElevatorApp.Util
{
    /// <inheritdoc/>
    /// <summary>
    /// Class copied from NodaTime <see cref="SystemClock"/>, to show the local system time instead of UTC
    /// </summary>
    public sealed class LocalSystemClock : IClock
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="LocalSystemClock"/> class from being created.
        /// </summary>
        private LocalSystemClock()
        {
        }

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        [NotNull]
        public static IClock Instance { get; } = new LocalSystemClock();

        /// <summary>
        /// Gets the current <see cref="T:NodaTime.Instant"/> on the time line according to this clock.
        /// </summary>
        /// <returns>The current instant on the time line according to this clock.</returns>
        public static Instant GetCurrentInstant() => Instance.GetCurrentInstant();

        /// <summary>
        /// Gets the current <see cref="T:NodaTime.Instant"/> on the time line according to this clock.
        /// </summary>
        /// <returns>The current instant on the time line according to this clock.</returns>
        /// <inheritdoc/>
        Instant IClock.GetCurrentInstant() => NodaConstants.BclEpoch.PlusTicks(DateTime.Now.Ticks);
    }
}