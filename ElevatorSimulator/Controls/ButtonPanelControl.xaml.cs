using System.Windows.Controls;
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

        public ButtonPanelControl(IButtonPanel buttonPanel) : this()
        {
            this.DataContext = buttonPanel;
        }
    }
}
