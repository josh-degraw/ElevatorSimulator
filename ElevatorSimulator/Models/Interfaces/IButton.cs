using System;
using ElevatorApp.Models.Enums;

namespace ElevatorApp.Models.Interfaces
{
    public interface IButton
    {
        string Label { get; }
        bool Active { get; }
        void Push();
        event EventHandler OnActivated;
        event EventHandler OnDeactivated;

        ButtonType ButtonType { get; }
    }

    public interface IButton<T> : IButton
    {
        event EventHandler<T> OnPushed;
        event EventHandler<T> OnActionCompleted;
        
        void ActionCompleted();

    }
}
