using ElevatorApp.Models.Enums;

namespace ElevatorApp.Models
{
    /// <summary>
    /// Ideally, represents settings of an elevator. Currently unused
    /// </summary>
    /// <seealso cref="ElevatorApp.Models.ModelBase" />
    public class ElevatorSettings: ModelBase
    {
        private int _speed, _capacity;
        private DoorType _doorType = DoorType.Single;

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>
        /// The speed.
        /// </value>
        public int Speed
        {
            get => _speed;
            set => SetProperty(ref _speed, value);
        }

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        /// <value>
        /// The capacity.
        /// </value>
        public int Capacity
        {
            get => _capacity;
            set => SetProperty(ref _capacity, value);
        }

        /// <summary>
        /// Gets or sets the type of the door.
        /// </summary>
        /// <value>
        /// The type of the door.
        /// </value>
        public DoorType DoorType
        {
            get => _doorType;
            set => SetProperty(ref _doorType, value);
        }
    }
}
