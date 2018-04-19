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
        /// <summary>
        /// Indicates whether this <see cref="DoorButton"/> is used to request the <see cref="Door"/> to open or close
        /// </summary>
        public override ButtonType ButtonType { get; }

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

        /// <summary>
        /// Invoked after the button has been pressed and the button is now active
        /// </summary>
        /// <inheritdoc cref="ButtonBase{T}.OnActivated" />
        public override event EventHandler OnActivated;

        /// <summary>
        /// Push the button
        /// </summary>
        /// <inheritdoc cref="ButtonBase{T}.Push" />
        public override void Push()
        {
            this.HandlePushed(this, this.ButtonType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoorButton"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        private DoorButton(ButtonType type) : base($"DoorBtn {type}")
        {
            this.ButtonType = type;
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

        /// <summary>
        /// Represents whether or not this object has performed the necessary steps to subscribe to the source.
        /// </summary>
        /// <inheritdoc />
        public bool Subscribed { get; set; } = false;

        /// <summary>
        /// Subscribes the specified door.
        /// </summary>
        /// <param name="door">The door.</param>
        /// <returns></returns>
        /// <inheritdoc />
        public Task Subscribe(Door door)
        {
            if (this.Subscribed)
                return Task.CompletedTask;

            if (this.ButtonType == ButtonType.Open)
                door.Opened += (e, args) => base.ActionCompleted(e, this.ButtonType);
            else
                door.Closed += (e, args) => base.ActionCompleted(e, this.ButtonType);

            this.Subscribed = true;
            return Task.CompletedTask;
        }
    }
}
