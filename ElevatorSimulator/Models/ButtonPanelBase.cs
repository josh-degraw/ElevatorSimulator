﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;
using MoreLinq;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Represents the base class of a button panel. 
    /// Subscribes all of the <see cref="Button"/>s it contains to the <see cref="Elevator"/>
    /// </summary>
    public abstract class ButtonPanelBase : IReadOnlyCollection<FloorButton>, ISubcriber<ElevatorMasterController>, IObservable<ElevatorCall>
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
        /// Initializes a new instance of the <see cref="ButtonPanelBase"/> class.
        /// </summary>
        protected ButtonPanelBase()
        {
        }

        #region ICollectionImplementation

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<FloorButton> IEnumerable<FloorButton>.GetEnumerator()
        {
            return FloorButtons.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)FloorButtons).GetEnumerator();
        }

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        int IReadOnlyCollection<FloorButton>.Count => FloorButtons.Count;

        #endregion


        /// <summary>
        /// Represents whether or not this object has performed the necessary steps to subscribe to the source.
        /// </summary>
        /// <inheritdoc />
        public bool Subscribed { get; protected set; }

        /// <summary>
        /// The observers
        /// </summary>
        private readonly AsyncObservableCollection<IObserver<ElevatorCall>> _observers = new AsyncObservableCollection<IObserver<ElevatorCall>>();


        /// <inheritdoc />
        /// <summary>
        /// Subscribes this <see cref="ButtonPanelBase"/> to an <see cref="IObservable{T}"/>, 
        /// in order for the subscriber to be notified whenever any of the <see cref="FloorButton"/>s on this <see cref="ButtonPanelBase"/> have been activated.
        /// </summary>
        /// <param name="observer"></param>
        /// <returns>An <see cref="Unsubscriber{T}"/> that can be disposed of to stop receiving updates</returns>
        public IDisposable Subscribe(IObserver<ElevatorCall> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new Unsubscriber<ElevatorCall>(_observers, observer);
        }

        /// <inheritdoc />
        /// <summary>
        /// Subscribes this Button panel to the <see cref="T:ElevatorApp.Models.ElevatorMasterController" />
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
            }

            this.Subscribed = true;
            return Task.CompletedTask;
        }
    }
}
