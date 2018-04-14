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
        public MainWindow()
        {
            InitializeComponent();
        }
        

        public SimulatorViewModel ViewModel
        {
            get
            {
                if (this.DataContext is SimulatorViewModel vm)
                    return vm;

                return null;
            }
        }

        private void Init_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel?.Controller.Init();
        }


        private void clkSelectElevator(object sender, RoutedEventArgs e)
        {
        }

        private void ListView_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Stats_Click(object sender, RoutedEventArgs e)
        {


            MessageBox.Show("Elevator Simulator Statistics" + "\n\n" +
                "Average Passenger Wait Time: " + Stats.Instance.GetAverageWaitTime().Seconds + "s \n" +
                "Minimum Passenger Wait Time: " + Stats.Instance.GetMinWaitTime().Seconds + "s \n" +
                "Maximum Passenger Wait Time: " + Stats.Instance.GetMaxWaitTime().Seconds + "s ");
               // "\n\n\n" + "Average Passenger Ride Time: " + "\n" +
               // "Minimum Passenger Ride Time: " + "\n" +
               // "Maximum Passenger Ride Time: ");
          
            Stats.Instance.GetAverageWaitTime();
            Logger.LogEvent("Calculated Current Average Wait Time: " + Stats.Instance.GetAverageWaitTime());
        }

        private void LoggerView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.Controller.Init();
        }

        private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void ElevatorControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
