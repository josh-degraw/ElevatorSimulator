using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElevatorApp.Models;
using ElevatorApp.Util;

namespace ElevatorApp
{
    /// <inheritdoc />
    /// <summary>
    /// ViewModel for the Elevator simulator
    /// </summary>
    public class SimulatorViewModel : ModelBase
    {
        /// <summary>
        /// The root controller for the simulator
        /// </summary>
        public ElevatorMasterController Controller { get; } = new ElevatorMasterController();

        /// <summary>
        /// A reference to the singleton instance of <see cref="Util.Logger"/>
        /// </summary>
        public ILogger Logger => Util.Logger.Instance;
        
        /// <summary>
        /// The elevator 
        /// </summary>
        public Elevator Elevator => Controller.Elevator;

        /// <summary>
        /// The <see cref="Floor"/>s of the <see cref="ElevatorMasterController"/>
        /// </summary>
        public IReadOnlyCollection<Floor> Floors => Controller.Floors;
        

    }
}
