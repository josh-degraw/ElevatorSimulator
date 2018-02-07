using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorApp.Core
{
    public interface IButton
    {
        string Label { get; }
        void Push();
    }

    public interface IButton<T> : IButton
    {
        event EventHandler<T> OnPushed;
    }
}
