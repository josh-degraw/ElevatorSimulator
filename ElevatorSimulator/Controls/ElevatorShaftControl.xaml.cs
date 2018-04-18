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
        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorShaftControl"/> class.
        /// </summary>
        public ElevatorShaftControl()
        {
            InitializeComponent();
            this.DataContextChanged += ElevatorShaftControl_DataContextChanged;
        }

        /// <summary>
        /// Handles the DataContextChanged event of the ElevatorShaftControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void ElevatorShaftControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            
        }

        /// <summary>
        /// Gets the controller.
        /// </summary>
        /// <value>
        /// The controller.
        /// </value>
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
