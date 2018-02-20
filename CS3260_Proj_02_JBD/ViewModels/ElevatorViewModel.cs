using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ElevatorApp.Core;
using ElevatorApp.Core.Interfaces;
using ElevatorApp.Core.Models;
using ElevatorApp.GUI.Util;

namespace ElevatorApp.GUI.ViewModels
{
    public class ElevatorViewModel : ViewModelBase
    {
        private int _floorCount;
        private IElevator _elevator;

        public ObservableCollection<IPassenger> Passengers => Elevator.Passengers;

        public ObservableConcurrentQueue<int> Path => Elevator.Path;

        public ICommand Dispatch { get; private set; }

        public int FloorCount
        {
            get => _floorCount;
            set => SetProperty(ref _floorCount, value);
        }

        public IElevator Elevator
        {
            get => _elevator;
            set
            {
                SetProperty(ref _elevator, value);
                this.Dispatch = DelegateCommand.Create<int>(value.Dispatch);
            }
        }

        public int Speed
        {
            get => Elevator.Speed;
            set
            {
                Elevator.Speed = value;
                OnPropertyChanged();
            }
        }

        public ElevatorViewModel(IElevator model, int floorsAvailable = 4)
        {
            this.Elevator = model;
            this.FloorCount = floorsAvailable;
        }

        public ElevatorViewModel() : this(new Elevator())
        {

        }

        public void Subscribe(ElevatorMasterController controller)
        {
            Elevator.Subscribe(controller);
        }
    

        public event EventHandler<int> OnArrival
        {
            add => Elevator.OnArrival += value;
            remove => Elevator.OnArrival -= value;
        }

        public event EventHandler<int> OnDeparture
        {
            add => Elevator.OnDeparture += value;
            remove => Elevator.OnDeparture -= value;
        }


        public ElevatorState State
        {
            get => Elevator.State;
            set
            {
                Elevator.State = value;
                OnPropertyChanged();
            }
        }
    }
}
