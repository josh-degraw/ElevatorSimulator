using ElevatorApp.Models.Enums;
using System;

namespace ElevatorApp.Models.Interfaces
{
    /// <summary>
    /// Represents the functionality of a button.
    /// </summary>
    public interface IButton
    {
        /// <summary>
        /// Occurs when on activated.
        /// </summary>
        event EventHandler OnActivated;

        /// <summary>
        /// Occurs when on deactivated.
        /// </summary>
        event EventHandler OnDeactivated;

        /// <summary>
        /// Gets a value indicating whether this <see cref="IButton"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        bool Active { get; }

        /// <summary>
        /// The type of the button.
        /// </summary>
        ButtonType ButtonType { get; }

        /// <summary>
        /// What would be printed on the button
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Pushes this button
        /// </summary>
        void Push();
    }

    /// <summary>
    /// Represents a button that contains a specific value, e.g. a floor number
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IButton<T> : IButton
    {
        /// <summary>
        /// Occurs when the action intended by this button is completed.
        /// </summary>
        event EventHandler<T> OnActionCompleted;

        /// <summary>
        /// Occurs when pushed.
        /// </summary>
        event EventHandler<T> OnPushed;

        /// <summary>
        /// Handles (triggers) the action completed event.
        /// </summary>
        void HandleActionCompleted();
    }
}