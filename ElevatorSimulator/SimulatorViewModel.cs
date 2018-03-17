using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElevatorApp.Models;
using ElevatorApp.Util;

namespace ElevatorApp
{
    /// <summary>
    /// Represents a 
    /// </summary>
    public class SimulatorViewModel : ModelBase
    {
        /// <summary>
        /// The root controller for the simulator
        /// </summary>
        public ElevatorMasterController Controller { get; } = new ElevatorMasterController();

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
