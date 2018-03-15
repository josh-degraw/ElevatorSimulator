using ElevatorApp.Models.Enums;

namespace ElevatorApp.Models
{
    public class ElevatorSettings: ModelBase
    {
        private int _speed, _capacity;
        private DoorType _doorType = DoorType.Single;

        public int Speed
        {
            get => _speed;
            set => SetProperty(ref _speed, value);
        }

        public int Capacity
        {
            get => _capacity;
            set => SetProperty(ref _capacity, value);
        }

        public DoorType DoorType
        {
            get => _doorType;
            set => SetProperty(ref _doorType, value);
        }
    }
}
