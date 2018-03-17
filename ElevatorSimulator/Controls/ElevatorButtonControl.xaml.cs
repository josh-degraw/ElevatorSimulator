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