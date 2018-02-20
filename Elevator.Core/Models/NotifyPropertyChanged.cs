using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using ElevatorApp.Core.Annotations;

namespace ElevatorApp.Core
{
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        // ReSharper disable once RedundantAssignment
        [NotifyPropertyChangedInvocator]
        protected virtual void SetValue<T>(ref T prop, T newVal, [CallerMemberName] string propertyName = null)
        {
            prop = newVal;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
