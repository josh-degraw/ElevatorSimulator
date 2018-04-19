using System.Windows;
using System.Windows.Controls;
using ElevatorApp.Models.Interfaces;

namespace ElevatorApp.Controls
{
    /// <summary>
    /// Interaction logic for ElevatorDoorButton.xaml
    /// </summary>
    public partial class ElevatorButtonControl : Button
    {
        /// <summary>
        /// Helper property to get the DataContext as an <see cref="IButton"/>
        /// </summary>
        public IButton Button
        {
            get
            {
                if (this.DataContext is IButton b)
                    return b;

                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorButtonControl"/> class.
        /// </summary>
        public ElevatorButtonControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the Button control.
        /// <para>
        /// Pushes the <see cref="IButton"/>
        /// </para>
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Button?.Push();
        }
    }
}