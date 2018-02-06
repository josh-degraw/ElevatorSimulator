using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorApp.Core
{
    public class DoorButton : IButton
    {
        public DoorButtonType DoorButtonType { get; }

        public DoorButton(DoorButtonType type)
        {
            this.DoorButtonType = type;
        }

        public string Label { get; }

        public void Push()
        {
            throw new NotImplementedException();
        }

        public event EventHandler OnPushed;

    }


}
