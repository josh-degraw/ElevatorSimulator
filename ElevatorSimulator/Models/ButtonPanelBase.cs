using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;
using MoreLinq;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Represents the base class of a button panel. 
    /// Subscribes all of the <see cref="Button"/>s it contains to the <see cref="Elevator"/>
    /// </summary>
    public abstract class ButtonPanelBase : IReadOnlyCollection<FloorButton>, ISubcriber<ElevatorMasterController>, IObservable<int>
    {
        /// <summary>
        /// The actual collection of <see cref="FloorButton"/>s
        /// </summary>
        protected abstract AsyncObservableCollection<FloorButton> _floorButtons { get; }

        /// <summary>
        /// A collection of <see cref="FloorButton"/>s that will appear on this panel
        /// </summary>
        public virtual IReadOnlyCollection<FloorButton> FloorButtons => _floorButtons;
        
        /// <summary>
        /// Initialize a new ButtonPanelBase
        /// </summary>
        protected ButtonPanelBase()
        {
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
        
        int IReadOnlyCollection<FloorButton>.Count => FloorButtons.Count;
        
        #endregion


        ///<inheritdoc/>
        public bool Subscribed { get; protected set; }

   
        private readonly AsyncObservableCollection<IObserver<int>> _observers = new AsyncObservableCollection<IObserver<int>>();

        /// <summary>
        /// Notify all <see cref="IObserver{T}"/>s that a <see cref="FloorButton"/> has been pushed
        /// </summary>
        /// <param name="floor"></param>
        private void NotifyObservers(int floor)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(floor);
            }
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

        /// <summary>
        /// Subscribes this Button panel to the <see cref="ElevatorMasterController"/>
        /// </summary>
        public virtual Task Subscribe(ElevatorMasterController parent)
        {
            if (this.Subscribed)
                return Task.CompletedTask;

            Logger.LogEvent($"Subscribing {nameof(ButtonPanelBase)}");

            this.Subscribe(parent.Elevator);

            foreach (FloorButton floorButton in this.FloorButtons)
            {
                floorButton.Subscribe(parent);
                //floorButton.OnPushed += (_, floor) =>
                //{
                //    NotifyObservers(floor);
                //};
            }

            this.Subscribed = true;
            return Task.CompletedTask;
        }
    }
}
