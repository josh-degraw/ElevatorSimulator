using ElevatorApp.Util;
using JetBrains.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ElevatorApp.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Base class to implement <see cref="T:System.ComponentModel.INotifyPropertyChanged" />, with a helper method ( <see cref="M:ElevatorApp.Models.ModelBase.SetProperty``1(``0@,``0,System.Boolean,System.String)" />)
    /// </summary>
    public abstract class ModelBase : INotifyPropertyChanged
    {
        private readonly object _modelBaseLocker = new object();

        /// <summary>
        /// If <see langword="true" />, any time a property value changes, an event is logged.
        /// </summary>
        private static bool _logAllPropertyChanges { get; set; } = false;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets the given backing property to the new value (if it has changed), and invokes <see cref="PropertyChanged" />.
        /// </summary>
        /// <param name="prop">A reference to the backing field that is to be changed.</param>
        /// <param name="value">The new value that this will be set to.</param>
        /// <param name="alwaysLog">Specifies whether or not to ignore the global configuration option <see cref="_logAllPropertyChanges" /></param>
        /// <param name="propertyName">
        /// The name of the property that is being changed. This is handled by the
        /// <see cref="CallerMemberNameAttribute" /> applied to this parameter, so it should never be set directly
        /// </param>
        /// <typeparam name="T">The type of property being changed</typeparam>
        /// <returns>
        /// True if the <paramref name="value" /> is different from <paramref name="prop" />, and therefore is being changed
        /// </returns>
        [NotifyPropertyChangedInvocator]
        protected virtual bool SetProperty<T>(ref T prop, T value, bool alwaysLog = false, [CallerMemberName] string propertyName = null)
        {
            lock (_modelBaseLocker)
            {
                if (prop.Equals(value))
                {
                    return false;
                }

                if (_logAllPropertyChanges || alwaysLog)
                    Logger.LogEvent($"Property Changed: {propertyName}.", ($"{prop}", value?.ToString()));

                prop = value;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        /// <summary>
        /// Use in situations where you can't directly set the value, but still need to trigger the
        /// <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged<T>(T newValue, [CallerMemberName] string propertyName = null)
        {
            if (_logAllPropertyChanges)
                Logger.LogEvent($"Property Changed: {propertyName}. New Value: {newValue}");

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Similar in function to <see cref="OnPropertyChanged{T}(T, string)" />, but to use this you must explicitly
        /// state the property name, so this is useful for when the object depends on something but has no actual control
        /// over changing it, but still needs the change to be notified.
        /// </summary>
        /// <param name="propertyName"></param>
        protected void DependentPropertyChanged(string propertyName)
        {
            if (_logAllPropertyChanges)
                Logger.LogEvent($"Property Changed: {propertyName}");

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}