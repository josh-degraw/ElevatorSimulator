using System;
using ElevatorApp.Models.Enums;

namespace ElevatorApp.Models.Interfaces
{
    /// <summary>
    /// Represents the functionality of a button.
    /// </summary>
    public interface IButton
    {
        /// <summary>
        /// What would be printed on the button
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IButton"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        bool Active { get; }

        /// <summary>
        /// Pushes this button
        /// </summary>
        void Push();

        /// <summary>
        /// Occurs when on activated.
        /// </summary>
        event EventHandler OnActivated;

        /// <summary>
        /// Occurs when on deactivated.
        /// </summary>
        event EventHandler OnDeactivated;

        /// <summary>
        /// The type of the button.
        /// </summary>
        ButtonType ButtonType { get; }
    }

    /// <summary>
    /// Represents a button that contains a specific value, e.g. a floor number
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IButton<T> : IButton
    {
        /// <summary>
        /// Occurs when pushed.
        /// </summary>
        event EventHandler<T> OnPushed;

        /// <summary>
        /// Occurs when the action intended by this button is completed.
        /// </summary>
        event EventHandler<T> OnActionCompleted;

        /// <summary>
        /// Handles (triggers) the action completed event.
        /// </summary>
        void HandleActionCompleted();

    }
}
