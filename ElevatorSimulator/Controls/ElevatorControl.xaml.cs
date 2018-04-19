using System.Windows.Controls;
using ElevatorApp.Models;
using ElevatorApp.Models.Interfaces;

namespace ElevatorApp.Controls
{
    /// <summary>
    /// Interaction logic for ElevatorControl.xaml
    /// </summary>
    public partial class ElevatorControl : UserControl
    {
        /// <summary>
        /// Gets or sets the elevator.
        /// </summary>
        public Elevator Elevator
        {
            get
            {
                if (this.DataContext is Elevator vm)
                    return vm;
                return null;
            }
            set
            {
                this.DataContext = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorControl"/> class.
        /// </summary>
        public ElevatorControl()
        {
            this.InitializeComponent();
        }
        

    }
}
