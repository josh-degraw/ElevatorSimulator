using System;
using System.Threading.Tasks;
using ElevatorApp.Models.Interfaces;

namespace ElevatorApp.Models
{
    public class DoorButton : ButtonBase<DoorButtonType>, IButton, ISubcriber<Door>
    {
        private DoorButtonType _doorButtonType;

        public DoorButtonType DoorButtonType
        {
            get => _doorButtonType;
            set => SetProperty(ref _doorButtonType, value);
        }

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


        public override event EventHandler OnActivated;

        public override void Push()
        {
            this.Pushed(this, this.DoorButtonType);
        }

        private DoorButton(DoorButtonType type) : base($"DoorBtn {type}")
        {
            this._doorButtonType = type;
        }

        public DoorButton() : this(DoorButtonType.Close)
        {

        }

        public static DoorButton Open() => new DoorButton(DoorButtonType.Open);

        public static DoorButton Close() => new DoorButton(DoorButtonType.Close);

        public bool Subscribed { get; set; } = false;

        public Task Subscribe(Door door)
        {
            if (this.Subscribed)
                return Task.CompletedTask;

            if (this._doorButtonType == DoorButtonType.Open)
                door.Opened += (e, args) => base.ActionCompleted(e, this.DoorButtonType);
            else
                door.Closed += (e, args) => base.ActionCompleted(e, this.DoorButtonType);

            this.Subscribed = true;
            return Task.CompletedTask;
        }
    }
}
