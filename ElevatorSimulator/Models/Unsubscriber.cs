using System;
using System.Collections.Generic;
using ElevatorApp.Util;

namespace ElevatorApp.Models
{
    
    /// <summary>
    /// Class used in <see cref="T:System.IObservable`1" /> / <see cref="T:System.IObserver`1" /> pattern.
    /// When disposed, the observer will no longer be in the co
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Unsubscriber<T> : IDisposable
    {
        public bool Disposed { get; private set; } = false;

        private readonly Action _remove;

        internal Unsubscriber(ICollection<IObserver<T>> observers, IObserver<T> observer)
        {
            // Using an Action delegate allows us to remove the observer from the collection without holding explicit references to them, via closure properties
            // This is nice because it limits the amount of contact we can have with the observer and the observable
            this._remove = () =>
            {
                if (observers.Contains(observer))
                    observers.Remove(observer);
            };
        }

        public void Dispose()
        {
            if (!Disposed)
                _remove();
        }
    }
}