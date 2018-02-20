using System;
using System.Collections.Generic;
using System.Text;
using ElevatorApp.Core.Interfaces;

namespace ElevatorApp.Core.Models
{
    public class DoorButton : Button<DoorButtonType>, IButton
    {
        public DoorButtonType DoorButtonType { get; set; } = DoorButtonType.Close;

        public override string Label
        {
            get
            {
                switch (DoorButtonType)
                {
                    case DoorButtonType.Open:
                        return "Open";
                    case DoorButtonType.Close:
                        return "Close";
                    default: return "Door";
                }
            }
        }

        public override void Push()
        {
            base.Pushed(this, this.DoorButtonType);
        }

        private DoorButton(DoorButtonType type)
        {
            this.DoorButtonType = type;
        }

        private DoorButton() : this(DoorButtonType.Close)
        {

        }

        public static DoorButton Open => new DoorButton(DoorButtonType.Open);

        public static DoorButton Close => new DoorButton(DoorButtonType.Close);
    }
}
