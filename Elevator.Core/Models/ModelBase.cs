using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using ElevatorApp.Core.Annotations;

namespace ElevatorApp.Core
{
    /// <summary>
    /// Base class to implement <see cref="INotifyPropertyChanged"/>, with a helper method (<see cref="SetValue{T}(ref T, T, string)"/>)
    /// </summary>
    public abstract class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged= (sender, args) =>
        {
            
        };
        
        [NotifyPropertyChangedInvocator]
        protected virtual bool SetValue<T>(ref T prop, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(prop, value)) return false;

            Logger.LogEvent($"Property Changed: {propertyName}. Old Value: {prop} New Value: {value}");
            prop = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

    }
}
