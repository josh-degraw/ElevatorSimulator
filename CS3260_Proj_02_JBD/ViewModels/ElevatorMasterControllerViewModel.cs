using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ElevatorApp.Core;
using ElevatorApp.Core.Interfaces;
using ElevatorApp.Core.Models;
using ElevatorApp.GUI.Util;

namespace ElevatorApp.GUI.ViewModels
{
    public class ElevatorMasterControllerViewModel : ViewModelBase
    {
        private ElevatorMasterController _masterController;
        private ElevatorMasterController MasterController
        {
            get => _masterController;
            set
            {
                SetProperty(ref _masterController, value);
                
                this.RequestElevatorCommand = DelegateCommand.Create<int>(this._masterController.QueueElevator);
            }
        }

        public ICommand RequestElevatorCommand { get; private set; }

        public ElevatorMasterControllerViewModel() : this(ElevatorMasterController.Instance)
        {
        }

        public ElevatorMasterControllerViewModel(ElevatorMasterController controller)
        {
            this.MasterController = controller;

        }

        public int FloorHeight
        {
            get => _masterController.FloorHeight;
            set
            {
                _masterController.FloorHeight = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Floor> Floors { get; set; }

        public int ElevatorCount
        {
            get => _masterController.ElevatorCount;
            set
            {
                _masterController.ElevatorCount = value;
              //  OnPropertyChanged();
            }
        }

        public ElevatorSettings ElevatorSettings
        {
            get => _masterController.ElevatorSettings;
            set
            {
                _masterController.ElevatorSettings = value;
                OnPropertyChanged();;
            }
        }

        public int FloorCount
        {
            get => _masterController.FloorCount;
            set
            {
                _masterController.FloorCount = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Elevator> Elevators => _masterController.Elevators;

        public ObservableConcurrentQueue<int> FloorsRequested => _masterController.FloorsRequested;

        public event EventHandler<int> OnElevatorRequested
        {
            add => _masterController.OnElevatorRequested += value;
            remove => _masterController.OnElevatorRequested -= value;
        }

        public void Dispatch(int floor, Direction direction)
        {
            _masterController.Dispatch(floor, direction);
        }

        public void Init()
        {
            _masterController.Init();
        }

        public void QueueElevator(int floor)
        {
            _masterController.QueueElevator(floor);
        }

        public void ReportArrival(IElevator elevator)
        {
            _masterController.ReportArrival(elevator);
        }
    }
}
