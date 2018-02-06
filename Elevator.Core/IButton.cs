using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorApp.Core
{
    public interface IButton
    {
        string Label { get; }
        void Push();
        event EventHandler OnPushed;
    }
}
