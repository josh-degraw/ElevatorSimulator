using System.Windows.Controls;
using ElevatorApp.Models.Interfaces;

namespace ElevatorApp.Controls
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
        

        public ElevatorControl()
        {
            InitializeComponent();
        }
        

    }
}
