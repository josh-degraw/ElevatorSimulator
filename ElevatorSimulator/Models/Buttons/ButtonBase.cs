using System;
using System.ComponentModel;
using ElevatorApp.Models.Interfaces;
using ElevatorApp.Util;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Base class for Buttons
    /// </summary>
    /// <typeparam name="T">The type of value held by the button</typeparam>
    public abstract class ButtonBase<T> : ModelBase, IButton, IButton<T>
    {
        /// <summary>
        /// The text on the button
        /// </summary>
        public abstract string Label { get; }

        private bool _active;
        private bool _isEnabled = true;

        /// <summary>
        /// Represents whether the button has been pressed or not.
        /// </summary>
        public bool Active
        {
            get => _active;
            protected set => SetProperty(ref _active, value);
        }

        /// <summary>
        /// Represents whether the button is able to be pressed or not
        /// </summary>
        public bool IsEnabled
        {
            get => _isEnabled;
            protected set => SetProperty(ref _isEnabled, value);
        }

        /// <summary>
        /// Invoked right when <see cref="Push"/> is called.
        /// </summary>
        public virtual event EventHandler<T> OnPushed;

        /// <summary>
        /// Invoked after the events in <see cref="Push"/> have completed.
        /// </summary>
        public virtual event EventHandler<T> OnActionCompleted;

        /// <summary>
        /// Invoked after the button has been pressed and the button is now active
        /// </summary>
        public virtual event EventHandler OnActivated;

        /// <summary>
        /// Invoked when the button is no longer active
        /// </summary>
        public virtual event EventHandler OnDeactivated;

        /// <summary>
        /// The arguments used in the <see cref="OnPushed"/> and <see cref="OnActionCompleted"/> events. This is here to let the button do something with the same actions 
        /// </summary>
        protected T actionArgs { get; private set; }

        /// <summary>
        /// Should be a unique Id for this button
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Construct a new <see cref="ButtonBase{T}"/>
        /// </summary>
        /// <param name="id">The uniqe id for this button</param>
        /// <param name="startActive">Whether the button should initially be active. This won't trigger <see cref="INotifyPropertyChanged.PropertyChanged"/>.</param>
        protected ButtonBase(string id, bool startActive = false)
        {
            this.Id = id;
            this._active = startActive;
            this.OnActivated += _setActiveTrue;
            this.OnDeactivated += _setActiveFalse;
        }

        private void _setActiveTrue(object sender, EventArgs e) => this.Active = true;
        private void _setActiveFalse(object sender, EventArgs e) => this.Active = false;

        /// <summary>
        /// Trigger the <see cref="OnPushed"/> event.
        /// </summary>
        protected void Pushed(object sender, T args)
        {
            this.OnPushed?.Invoke(sender, args);
            actionArgs = args;
            OnActivated?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Trigger the <see cref="OnActionCompleted"/> event
        /// </summary>
        protected void ActionCompleted(object sender, T args)
        {
            this.OnActionCompleted?.Invoke(sender, args);
            this.actionArgs = default;
            OnDeactivated?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Trigger the <see cref="OnActionCompleted"/> event
        /// </summary>
        public void ActionCompleted()
        {
            ActionCompleted(this, this.actionArgs);
        }

        /// <summary>
        /// Push the button
        /// </summary>
        public abstract void Push();
        
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.Label;
        }
    }
}
