using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using System;
using System.ComponentModel;

namespace ElevatorApp.Models
{
    /// <inheritdoc cref="IButton{T}"/>
    /// <summary>
    /// Base class for Buttons
    /// </summary>
    /// <typeparam name="T">The type of value held by the button</typeparam>
    public abstract class ButtonBase<T> : ModelBase, IButton<T>
    {
        #region Private Fields

        private bool _active;

        private bool _isEnabled = true;

        #endregion Private Fields

        #region Public Events

        /// <inheritdoc/>
        /// <summary>
        /// Invoked after the events in <see cref="Push"/> have completed.
        /// </summary>
        public virtual event EventHandler<T> OnActionCompleted;

        /// <inheritdoc/>
        /// <summary>
        /// Invoked after the button has been pressed and the button is now active
        /// </summary>
        public virtual event EventHandler OnActivated;

        /// <inheritdoc/>
        /// <summary>
        /// Invoked when the button is no longer active
        /// </summary>
        public virtual event EventHandler OnDeactivated;

        /// <inheritdoc/>
        /// <summary>
        /// Invoked right when <see cref="Push"/> is called.
        /// </summary>
        public virtual event EventHandler<T> OnPushed;

        #endregion Public Events

        #region Protected Constructors

        /// <summary>
        /// Construct a new <see cref="ButtonBase{T}"/>
        /// </summary>
        /// <param name="id">The uniqe id for this button</param>
        /// <param name="startActive">Whether the button should initially be active. This won't trigger <see cref="INotifyPropertyChanged.PropertyChanged"/>.</param>
        protected ButtonBase(string id, bool startActive = false)
        {
            this.Id = id;
            this._active = startActive;
            this.OnActivated += this._setActiveTrue;
            this.OnDeactivated += this._setActiveFalse;
        }

        #endregion Protected Constructors

        #region Protected Properties

        /// <summary>
        /// The arguments used in the <see cref="OnPushed"/> and <see cref="OnActionCompleted"/> events. This is here to
        /// let the button do something with the same actions
        /// </summary>
        protected T actionArgs { get; private set; }

        #endregion Protected Properties

        #region Public Properties

        /// <summary>
        /// Represents whether the button has been pressed or not.
        /// </summary>
        public bool Active
        {
            get => this._active;
            protected set => this.SetProperty(ref this._active, value);
        }

        /// <inheritdoc/>
        /// <summary>
        /// The type of button this is.
        /// </summary>
        public abstract ButtonType ButtonType { get; }

        /// <summary>
        /// Should be a unique Id for this button
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Represents whether the button is able to be pressed or not
        /// </summary>
        public bool IsEnabled
        {
            get => this._isEnabled;
            protected set => this.SetProperty(ref this._isEnabled, value);
        }

        /// <inheritdoc/>
        /// <summary>
        /// The text on the button
        /// </summary>
        public abstract string Label { get; }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Sets <see cref="Active"/> to <see langword="false"/> when deactivated
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void _setActiveFalse(object sender, EventArgs e) => this.Active = false;

        /// <summary>
        /// Sets <see cref="Active"/> to <see langword="true"/> when activated
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void _setActiveTrue(object sender, EventArgs e) => this.Active = true;

        #endregion Private Methods

        #region Protected Methods

        /// <summary>
        /// Trigger the <see cref="OnActionCompleted"/> event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        protected void ActionCompleted(object sender, T args)
        {
            this.OnActionCompleted?.Invoke(sender, args);
            this.actionArgs = default;
            this.OnDeactivated?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Trigger the <see cref="OnPushed"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        protected void HandlePushed(object sender, T args)
        {
            this.OnPushed?.Invoke(sender, args);
            this.actionArgs = args;
            this.OnActivated?.Invoke(sender, EventArgs.Empty);
        }

        #endregion Protected Methods

        #region Public Methods

        /// <inheritdoc/>
        /// <summary>
        /// Trigger the <see cref="OnActionCompleted"/> event
        /// </summary>
        public void HandleActionCompleted()
        {
            this.ActionCompleted(this, this.actionArgs);
        }

        /// <inheritdoc/>
        /// <summary>
        /// Push the button
        /// </summary>
        public abstract void Push();

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.Label;
        }

        #endregion Public Methods
    }
}