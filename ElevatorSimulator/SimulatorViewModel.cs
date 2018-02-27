using System.Collections.Generic;
using System.Linq;
using ElevatorApp.Models;
using ElevatorApp.Util;

namespace ElevatorApp
{

    public class SimulatorViewModel : ModelBase
    {
        public ElevatorSimulator Simulator { get; } = new ElevatorSimulator();

        public ElevatorMasterController Controller => Simulator.Controller;

        public ICollection<Passenger> People => Simulator.People;

        public ILogger Logger => Util.Logger.Instance;

        public ButtonPanel SelectedButtonPanel => Controller.Elevators.SingleOrDefault(a => a.ElevatorNumber == SelectedElevatorNumber)?.ButtonPanel;

        private int _selectedElevatorNumber;

        public int SelectedElevatorNumber
        {
            get => _selectedElevatorNumber;
            set
            {
                if (_selectedElevatorNumber != value)
                {
                    SetProperty(ref _selectedElevatorNumber, value);

                    base.DependantPropertyChanged(nameof(SelectedButtonPanel));
                }
            }
        }
    }
}
