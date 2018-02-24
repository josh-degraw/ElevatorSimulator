using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using ElevatorApp.Core.Annotations;

namespace ElevatorApp.Core
{
    /// <summary>
    /// Base class to implement <see cref="INotifyPropertyChanged"/>, with a helper method (<see cref="SetProperty{T}"/>)
    /// </summary>
    public abstract class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, args) =>
        {

        };

        [NotifyPropertyChangedInvocator]
        protected virtual bool SetProperty<T>(ref T prop, T value, bool log = true, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(prop, value)) return false;

            if (log)
                Logger.LogEvent($"Property Changed: {propertyName}. {prop} --> {value}");
            prop = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        protected void OnPropertyChanged<T>(T newValue, [CallerMemberName] string propertyName = null)
        {
            Logger.LogEvent($"Property Changed: {propertyName}. New Value: {newValue}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        protected void DependantPropertyChanged(string propertyName)
        {
            Logger.LogEvent($"Property Changed: {propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
