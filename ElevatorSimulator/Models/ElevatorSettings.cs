using ElevatorApp.Models.Enums;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Ideally, represents settings of an elevator. Currently unused
    /// </summary>
    /// <seealso cref="ElevatorApp.Models.ModelBase"/>
    public class ElevatorSettings : ModelBase
    {
        private DoorType _doorType = DoorType.Single;
        private int _speed, _capacity;

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        /// <value>The capacity.</value>
        public int Capacity
        {
            get => this._capacity;
            set => this.SetProperty(ref this._capacity, value);
        }

        /// <summary>
        /// Gets or sets the type of the door.
        /// </summary>
        /// <value>The type of the door.</value>
        public DoorType DoorType
        {
            get => this._doorType;
            set => this.SetProperty(ref this._doorType, value);
        }

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public int Speed
        {
            get => this._speed;
            set => this.SetProperty(ref this._speed, value);
        }
    }
}