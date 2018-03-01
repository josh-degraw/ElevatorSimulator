using System.Windows.Controls;
using ElevatorApp.Models;

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
        }

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
