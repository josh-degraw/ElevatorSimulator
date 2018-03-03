using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElevatorApp.Models;
using ElevatorApp.Util;

namespace ElevatorApp
{

    public class SimulatorViewModel : ModelBase
    {
        public ElevatorMasterController Controller { get; } = new ElevatorMasterController();

        public ICollection<Passenger> People { get; } = new  AsyncObservableCollection<Passenger>();

        public ILogger Logger => Util.Logger.Instance;

        public ButtonPanel SelectedButtonPanel => Controller.Elevators.SingleOrDefault(a => a.ElevatorNumber == SelectedElevatorNumber)?.ButtonPanel;

        public ICollection<Elevator> Elevators => Controller.Elevators;

        public ICollection<Floor> Floors => Controller.Floors;

        private int _selectedElevatorNumber;

        public int SelectedElevatorNumber
        {
            get => _selectedElevatorNumber;
            set
            {
                if (_selectedElevatorNumber != value)
                {
                    SetProperty(ref _selectedElevatorNumber, value);

                    base.DependentPropertyChanged(nameof(SelectedButtonPanel));
                }
            }
        }


    }
}
