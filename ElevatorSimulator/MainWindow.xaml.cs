using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using ElevatorApp.Util;

namespace ElevatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        private readonly object _locker = new object();
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        public SimulatorViewModel ViewModel
        {
            get
            {
                if (this.DataContext is SimulatorViewModel vm)
                    return vm;

                return null;
            }
        }

        /// <summary>
        /// Handles the Click event of the Stats control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Stats_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show($@"
Passenger Wait Times:
Average: {Stats.Instance.PassengerWaitTimes.Average.Seconds}s 
Minimum: {Stats.Instance.PassengerWaitTimes.Min.Seconds}s
Maximum: {Stats.Instance.PassengerWaitTimes.Max.Seconds}s",
                "Elevator Simulator Statistics"); 
              
          
            Logger.LogEvent("Average Wait Time: " + Stats.Instance.PassengerWaitTimes.Average);
        }

    }
}
