using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Ribbon;
using ElevatorApp.Core;
using ElevatorApp.Core.Models;
using ElevatorApp.GUI.ViewModels;

namespace ElevatorApp.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Logger.OnItemLogged += Logger_OnItemLogged;
            // this.DataContext = new ElevatorMasterControllerViewModel(ElevatorMasterController.Instance);
//            this.txtBxLog.Text = @"
//Initializing
//Elevators: 1
//Elevator 1: floor 1
//Floor requested: 4
//Elevator 1 Queued.
//Elevator 1 Moving
//Floor requested: 2
//Elevator 1 Stopping
//Elevator 1 Arrived: floor 2
//Elevator 1 Door opening
//Elevator 1 Door opened
//Floor requested: 4
//Elevator 1 Door closing
//Elevator 1 Door open Requested
//Elevator 1 Door closing
//Elevator 1 Door closed
//Elevator 1 Moving
//Elevator 1 Stopping
//Elevator 1 Arrived: floor 4
//Elevator 1 Door opening
//Elevator 1 Door opened
//";
        }

        private void Logger_OnItemLogged(object sender, Event str)
        {
            this.txtBxLog.Text += $"{str}\n";
        }

        public ElevatorMasterController ViewModel
        {
            get
            {
                if (this.DataContext is ElevatorMasterController vm)
                    return vm;

                return null;
            }
            set => this.DataContext = value;
        }
        
        private void Init_Click(object sender, RoutedEventArgs e)
        {
            Logger.LogEvent("Initializing");
            this.ViewModel.Init();
           
        }

    }
}
