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
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonPanelControl"/> class.
        /// </summary>
        public ButtonPanelControl()
        {
            InitializeComponent();
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
