using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using ElevatorApp.Core.Annotations;
using ElevatorApp.Core.Interfaces;
using ElevatorApp.GUI.Util;
using ElevatorApp.GUI.ViewModels;

namespace ElevatorApp.GUI
{
    /// <summary>
    /// Interaction logic for ElevatorDoorButton.xaml
    /// </summary>
    public partial class ElevatorButtonControl : UserControl
    {
        //public ICommand Push { get; private set; }


        public IButton Button
        {
            get
            {
                if (this.DataContext is IButton b)
                    return b;

                return null;
            }
        }


        public ElevatorButtonControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Button?.Push();
        }
    }
}