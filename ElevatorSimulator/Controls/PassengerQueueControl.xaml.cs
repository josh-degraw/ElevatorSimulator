using System;
using System.Collections.Generic;
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
using ElevatorApp.Models;

namespace ElevatorApp.Controls
{
    /// <summary>
    /// Interaction logic for PassengerQueueControl.xaml
    /// </summary>
    public partial class PassengerQueueControl : UserControl
    {
        public PassengerQueueControl()
        {
            InitializeComponent();
        }

        public bool ShowAddButtons { get; set; } = true;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.DataContext is FloorButton fb)
            {
                this.Floor?.QueuePassenger(fb.FloorNumber);
            }
        }

        private Floor Floor
        {
            get
            {
                if (this.DataContext is Floor f)
                    return f;

                return null;
            }
        }

        public bool ElevatorAvailable => this.Floor?.ElevatorAvailable ?? false;

        private void Expander_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
