using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElevatorApp.Models;
using ElevatorApp.Util;

namespace ElevatorApp
{

    public class SimulatorViewModel : ModelBase
    {
        public ElevatorSimulator Simulator { get; } = new ElevatorSimulator();

        public ElevatorMasterController Controller => Simulator.Controller;

        public ILogger Logger => Util.Logger.Instance;

        public ButtonPanel SelectedButtonPanel => Controller.Elevators.SingleOrDefault(a => a.ElevatorNumber == SelectedElevatorNumber)?.ButtonPanel;

        public IReadOnlyCollection<Elevator> Elevators => Controller.Elevators;

        public IReadOnlyCollection<Floor> Floors => Controller.Floors;

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
