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
        

        public ElevatorControl()
        {
            InitializeComponent();
        }
        

    }
}
