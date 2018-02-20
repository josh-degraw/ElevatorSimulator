using System;
using System.Windows;
using System.Windows.Input;
using ElevatorApp.Core.Interfaces;
using ElevatorApp.Core.Models;
using ElevatorApp.GUI.Util;

namespace ElevatorApp.GUI.ViewModels
{
    public class ElevatorButtonViewModel : ViewModelBase
    {
        public string Label => Button?.Label ?? "";
        public bool Active => Button?.Active ?? false;
        public IButton Button { get; set; }

        ICommand _push;

        public ICommand Push
        {
            get => _push;
            set => SetProperty(ref _push, value);
        }

        public event EventHandler OnActivated
        {
            add => Button.OnActivated += value;
            remove => Button.OnActivated -= value;
        }

        public event EventHandler OnDeactivated
        {
            add => Button.OnDeactivated += value;
            remove => Button.OnDeactivated -= value;
        }
        
        public ElevatorButtonViewModel() : this(DoorButton.Open)
        {
        }

        public ElevatorButtonViewModel(IButton button)
        {
            this.Button = button;
            this.Push = DelegateCommand.Create(button.Push);
        }

    }
}
