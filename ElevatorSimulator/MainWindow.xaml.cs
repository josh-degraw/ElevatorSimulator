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
            // These are to tell the stats class which thing to use, waitTime, rideTimes, or totalTimes
            int x = 1, y = 2, z = 3;



            // check to see if i can pass the collection in right here
            MessageBox.Show("Elevator Simulator Statistics" + "\n\n" +
                "Average Passenger Wait Time: " + Stats.Instance.GetAverageTime(x).Seconds + "s \n" +
                "Minimum Passenger Wait Time: " + Stats.Instance.GetMinTime(y).Seconds + "s \n" +
                "Maximum Passenger Wait Time: " + Stats.Instance.GetMaxTime(z).Seconds + "s \n\n" + 

                "Average Passenger Ride Time: " + Stats.Instance.GetAverageTime(x).Seconds + "s \n" +
                "Minimum Passenger Ride Time: " + Stats.Instance.GetMinTime(y).Seconds + "s \n" +
                "Maximum Passenger Ride Time: " + Stats.Instance.GetMaxTime(z).Seconds + "s \n\n" +

                "Average Passenger Total Time: " + Stats.Instance.GetAverageTime(x).Seconds + "s \n" +
                "Minimum Passenger Total Time: " + Stats.Instance.GetMinTime(y).Seconds + "s \n" +
                "Maximum Passenger Total Time: " + Stats.Instance.GetMaxTime(z).Seconds + "s"); 
               
          
            Stats.Instance.GetAverageTime(x);
            Logger.LogEvent("Calculated Current Average Wait Time: " + Stats.Instance.GetAverageTime(x));
        }

        private void LoggerView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.Controller.Init();
        }
    }
}
