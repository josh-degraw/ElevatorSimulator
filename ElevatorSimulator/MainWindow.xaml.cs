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

            MessageBox.Show($@"
Passenger Wait Times:
Average: {Stats.Instance.PassengerWaitTimes.Average.Seconds}s 
Minimum: {Stats.Instance.PassengerWaitTimes.Min.Seconds}s
Maximum: {Stats.Instance.PassengerWaitTimes.Max.Seconds}s",
                "Elevator Simulator Statistics"); 
              
          
            Logger.LogEvent("Average Wait Time: " + Stats.Instance.PassengerWaitTimes.Average);
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
