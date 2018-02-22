using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ElevatorApp.Core.Interfaces;
using ElevatorApp.Core.Models;
using ElevatorApp.GUI.Util;
using ElevatorApp.GUI.ViewModels;

namespace ElevatorApp.GUI.Controls
{
    /// <summary>
    /// Interaction logic for ElevatorControl.xaml
    /// </summary>
    public partial class ElevatorControl : UserControl
    {

        public IElevator Elevator
        {
            get
            {
                if (this.DataContext is IElevator vm)
                    return vm;
                return null;
            }
            set
            {
                this.DataContext = value;
            }
        }

        public ICommand Dispatch { get; }

        public ElevatorControl()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.Elevator = new Elevator();
                this.Dispatch = DelegateCommand.Create<int>(Elevator.Dispatch);
            }
            InitializeComponent();
        }

        public ElevatorControl(IElevator elevator)
        {
            this.DataContext = elevator;

            this.Dispatch = DelegateCommand.Create<int>(elevator.Dispatch);
        }

    }
}
