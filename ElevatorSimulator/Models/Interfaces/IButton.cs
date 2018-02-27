using System;

namespace ElevatorApp.Models.Interfaces
{
    public interface IButton
    {
        string Label { get; }
        bool Active { get; }
        void Push();
        event EventHandler OnActivated;
        event EventHandler OnDeactivated;
    }

    public interface IButton<T> : IButton
    {
        event EventHandler<T> OnPushed;
        event EventHandler<T> OnActionCompleted;
        
        void ActionCompleted();

    }
}
