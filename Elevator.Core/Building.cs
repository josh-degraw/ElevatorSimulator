using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ElevatorApp.Core
{
    public class Building : NotifyPropertyChanged
    {
        private int _floorCount = 0, _floorHeight = 0;

        public int FloorCount
        {
            get => _floorCount;
            set => SetValue(ref _floorCount, value);
        }

        public int FloorHeight
        {
            get => _floorHeight;
            set => SetValue(ref _floorHeight, value);
        }

        private ICollection<ElevatorController> _elevatorControllers;

        public ICollection<ElevatorController> ElevatorControllers
        {
            get => _elevatorControllers;
            set => SetValue(ref _elevatorControllers, value);
        }

    }
}
