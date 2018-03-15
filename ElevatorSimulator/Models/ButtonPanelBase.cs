using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Represents the base class of a button panel. Takes care of subscription of 
    /// </summary>
    public abstract class ButtonPanelBase : ICollection<FloorButton>, ISubcriber<(ElevatorMasterController, Elevator)>, ISubcriber<ElevatorMasterController>, IObservable<int>
    {
        /// <summary>
        /// A collection of <see cref="FloorButton"/>s that will appear on this panel
        /// </summary>
        public ICollection<FloorButton> FloorButtons { get; }

        /// <summary>
        /// Initialize a new
        /// </summary>
        protected ButtonPanelBase()
        {
            this.FloorButtons = new AsyncObservableCollection<FloorButton>
            {
                new FloorButton(4),
                new FloorButton(3),
                new FloorButton(2),
                new FloorButton(1),
            };
        }

        #region ICollectionImplementation
        IEnumerator<FloorButton> IEnumerable<FloorButton>.GetEnumerator()
        {
            return FloorButtons.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)FloorButtons).GetEnumerator();
        }

        void ICollection<FloorButton>.Add(FloorButton item)
        {
            FloorButtons.Add(item);
        }

        void ICollection<FloorButton>.Clear()
        {
            FloorButtons.Clear();
        }

        bool ICollection<FloorButton>.Contains(FloorButton item)
        {
            return FloorButtons.Contains(item);
        }

        void ICollection<FloorButton>.CopyTo(FloorButton[] array, int arrayIndex)
        {
            FloorButtons.CopyTo(array, arrayIndex);
        }

        bool ICollection<FloorButton>.Remove(FloorButton item)
        {
            return FloorButtons.Remove(item);
        }

        int ICollection<FloorButton>.Count => FloorButtons.Count;

        bool ICollection<FloorButton>.IsReadOnly => FloorButtons.IsReadOnly;
        #endregion

        public bool Subscribed { get; protected set; }


        /// <summary>
        /// Subscribe to a <see cref="ElevatorMasterController"/> and an <see cref="Elevator"/>.
        /// </summary>
        /// <param name="parent">The Pair toi subscribe to</param>
        public virtual async Task Subscribe((ElevatorMasterController, Elevator) parent)
        {
            var (_, elevator) = parent;

            Logger.LogEvent($"Subscribing {nameof(ButtonPanelBase)}", ("Elevator", elevator.ElevatorNumber));
            await Task.WhenAll(this.FloorButtons.Select(button => button.Subscribe(parent)));

        }

        private readonly AsyncObservableCollection<IObserver<int>> _observers = new AsyncObservableCollection<IObserver<int>>();

        /// <summary>
        /// Notify all <see cref="IObserver{T}"/>s that a <see cref="FloorButton"/> has been pushed
        /// </summary>
        /// <param name="floor"></param>
        private void NotifyObservers(int floor)
        {
            _observers.AsParallel().ForAll(observer => observer.OnNext(floor));
        }
        

        /// <inheritdoc />
        /// <summary>
        /// Subscribes this <see cref="ButtonPanelBase"/> to an <see cref="IObservable{T}"/>, 
        /// in order for the subscriber to be notified whenever any of the <see cref="FloorButton"/>s on this <see cref="ButtonPanelBase"/> have been activated.
        /// </summary>
        /// <param name="observer"></param>
        /// <returns>An <see cref="Unsubscriber{T}"/> that can be disposed of to stop receiving updates</returns>
        public IDisposable Subscribe(IObserver<int> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new Unsubscriber<int>(_observers, observer);
        }

        public async Task Subscribe(ElevatorMasterController parent)
        {
            this.Subscribe((IObserver<int>)parent);

            foreach (FloorButton floorButton in this.FloorButtons)
            {
                floorButton.OnPushed += async (_, floor) =>
                {
                    NotifyObservers(floor);
                };
            }
        }
    }
}
