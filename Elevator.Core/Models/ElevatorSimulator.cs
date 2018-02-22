using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using ElevatorApp.Core.Interfaces;


namespace ElevatorApp.Core.Models
{
    public class ElevatorSimulator
    {
        public ElevatorMasterController Controller { get; } = new ElevatorMasterController();

        public ICollection<Passenger> People { get; } = new ObservableCollection<Passenger>();

        public ElevatorSimulator()
        {

        }
    }
}
