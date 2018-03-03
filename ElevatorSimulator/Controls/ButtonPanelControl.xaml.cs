using System.Windows.Controls;
using ElevatorApp.Models;
using ElevatorApp.Models.Interfaces;

namespace ElevatorApp.Controls
{
    /// <summary>
    /// Interaction logic for ButtonPanelControl.xaml
    /// </summary>
    public partial class ButtonPanelControl : UserControl
    {
        public ButtonPanelControl()
        {
            InitializeComponent();
        }

        public ButtonPanelControl(ButtonPanel buttonPanel) : this()
        {
            this.DataContext = buttonPanel;
        }
    }
}
