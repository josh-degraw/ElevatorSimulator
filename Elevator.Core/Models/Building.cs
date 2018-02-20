using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using ElevatorApp.Core.Interfaces;

namespace ElevatorApp.Core
{
    public class Building
    {

        public ICollection<IElevatorMasterController> ElevatorControllers { get; set; } = new IElevatorMasterController[] { };

        public Building()
        {

        }
    }
}
