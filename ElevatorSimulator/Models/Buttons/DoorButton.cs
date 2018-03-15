using System;
using System.Threading.Tasks;
using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;

namespace ElevatorApp.Models
{
    /// <inheritdoc cref="ButtonBase{T}" />
    /// <summary>
    /// Represents a Button that is used to request that a <see cref="T:ElevatorApp.Models.Door" /> change its state
    /// </summary>
    public class DoorButton : ButtonBase<ButtonType>, IButton, ISubcriber<Door>
    {
        private ButtonType _buttonType;

        /// <summary>
        /// Indicates whether this <see cref="DoorButton"/> is used to request the <see cref="Door"/> to open or close
        /// </summary>
        public ButtonType ButtonType
        {
            get => _buttonType;
            set => SetProperty(ref _buttonType, value);
        }

        /// <summary>
        /// Indicates the type of Button this is
        /// </summary>
        public override string Label
        {
            get
            {
                switch (ButtonType)
                {
                    case ButtonType.Open:
                        return "Open";
                    case ButtonType.Close:
                        return "Close";
                    default: return "Door";
                }
            }
        }

        ///<inheritdoc cref="ButtonBase{T}.OnActivated"/>
        public override event EventHandler OnActivated;

        ///<inheritdoc cref="ButtonBase{T}.Push"/>
        public override void Push()
        {
            this.Pushed(this, this.ButtonType);
        }

        private DoorButton(ButtonType type) : base($"DoorBtn {type}")
        {
            this._buttonType = type;
        }

        /// <summary>
        /// Constructs a new <see cref="DoorButton"/>
        /// </summary>
        public DoorButton() : this(ButtonType.Close)
        {

        }

        /// <summary>
        /// Returns a <see cref="DoorButton"/> used to open a door
        /// </summary>
        /// <returns>The newly created <see cref="DoorButton"/> instance</returns>
        public static DoorButton Open() => new DoorButton(ButtonType.Open);

        /// <summary>
        /// Returns a <see cref="DoorButton"/> used to close a door
        /// </summary>
        /// <returns>The newly created <see cref="DoorButton"/> instance</returns>
        public static DoorButton Close() => new DoorButton(ButtonType.Close);

        /// <inheritdoc />
        public bool Subscribed { get; set; } = false;

        /// <inheritdoc />
        public Task Subscribe(Door door)
        {
            if (this.Subscribed)
                return Task.CompletedTask;

            if (this._buttonType == ButtonType.Open)
                door.Opened += (e, args) => base.ActionCompleted(e, this.ButtonType);
            else
                door.Closed += (e, args) => base.ActionCompleted(e, this.ButtonType);

            this.Subscribed = true;
            return Task.CompletedTask;
        }
    }
}
