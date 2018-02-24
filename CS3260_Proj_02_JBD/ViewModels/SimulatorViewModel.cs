using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorApp.Core;
using ElevatorApp.Core.Models;

namespace ElevatorApp.GUI.ViewModels
{

    public class SimulatorViewModel : ModelBase
    {
        public ElevatorSimulator Simulator { get; } = new ElevatorSimulator();

        public ElevatorMasterController Controller => Simulator.Controller;

        public ICollection<Passenger> People => Simulator.People;

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
