using ElevatorApp.Models;
using System.Windows.Controls;

namespace ElevatorApp.Controls
{
    /// <summary>
    /// Interaction logic for ButtonPanelControl.xaml
    /// </summary>
    public partial class ButtonPanelControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonPanelControl"/> class.
        /// </summary>
        public ButtonPanelControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonPanelControl"/> class.
        /// </summary>
        /// <param name="buttonPanel">The button panel.</param>
        public ButtonPanelControl(ButtonPanel buttonPanel) : this()
        {
            this.DataContext = buttonPanel;
        }
    }
}