using ElevatorApp.Models;
using System.Windows;
using System.Windows.Controls;

namespace ElevatorApp.Controls
{
    /// <summary>
    /// Interaction logic for PassengerQueueControl.xaml
    /// </summary>
    public partial class PassengerQueueControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PassengerQueueControl"/> class.
        /// </summary>
        public PassengerQueueControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.DataContext is RequestButton fb)
            {
                fb.Push();
            }
        }

        /// <summary>
        /// Gets the floor.
        /// </summary>
        private Floor Floor
        {
            get
            {
                if (this.DataContext is Floor f)
                    return f;

                return null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the elevator is available.
        /// </summary>
        /// <value><c>true</c> if the elevator is available; otherwise, <c>false</c>.</value>
        public bool ElevatorAvailable => this.Floor?.ElevatorAvailable ?? false;
    }
}