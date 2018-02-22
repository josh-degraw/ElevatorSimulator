using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ElevatorApp.Core;
using ElevatorApp.Core.Interfaces;
using ElevatorApp.GUI.ViewModels;

namespace ElevatorApp.GUI
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
