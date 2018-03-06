using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ElevatorApp.Models;
using ElevatorApp.Util;

namespace ElevatorApp.Controls
{
    /// <summary>
    /// Interaction logic for ElevatorShaftControl.xaml
    /// </summary>
    public partial class ElevatorShaftControl : UserControl
    {
        public ElevatorShaftControl()
        {
            InitializeComponent();
            this.DataContextChanged += ElevatorShaftControl_DataContextChanged;
        }
        

        private void ElevatorShaftControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            
        }

        public Elevator SingleElevator => Controller?.Elevators.FirstOrDefault();

        private SimulatorViewModel Controller
        {
            get
            {
                if (this.DataContext is SimulatorViewModel c)
                    return c;

                return null;
            }
        }


    }
}
