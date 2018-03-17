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
            if (sender is FrameworkElement el && el.Tag is int num)
            {
                if (this.ViewModel.SelectedElevatorNumber != num)
                    this.ViewModel.SelectedElevatorNumber = num;
            }
        }

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Stats_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Average Passenger Wait Time in Seconds: " + Stats.Instance.GetAverageWaitTime().TotalSeconds);
            Stats.Instance.GetAverageWaitTime();
            Logger.LogEvent("Calculated Current Average Wait Time: " + Stats.Instance.GetAverageWaitTime());
        }

        private void LoggerView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.Controller.Init();
        }
    }
}
