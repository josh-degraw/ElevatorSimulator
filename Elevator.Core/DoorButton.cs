using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorApp.Core
{
    public class DoorButton : IButton<DoorButtonType>
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

        public event EventHandler<DoorButtonType> OnPushed;

    }


}
