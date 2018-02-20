using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using ElevatorApp.Core.Interfaces;
using ElevatorApp.Core.Models;

namespace ElevatorApp.GUI.ViewModels
{
    public class ButtonPanelControlViewModel : DependencyObject
    {
        public IButtonPanel ButtonPanel { get; set; }
        public ButtonPanelControlViewModel()
        {
        }

        public ButtonPanelControlViewModel(IButtonPanel buttonPanel)
        {
            this.Load(buttonPanel);
        }

        public ObservableCollection<FloorButton> FloorButtons { get; set; }

        public ElevatorButtonViewModel OpenDoorButton { get; set; }

        public ElevatorButtonViewModel CloseDoorButton { get; set; }

        //ICollection<FloorButton> IButtonPanel.FloorButtons
        //{
        //    get => Buttons;
        //    set
        //    {
        //        if (value is ObservableCollection<FloorButton> obs)
        //            this.Buttons = obs;
        //        this.Buttons = new ObservableCollection<FloorButton>(value);
        //    }
        //}

        //public DoorButton CloseDoorButton
        //{
        //    get => GetValue(CloseDoorButtonProperty) as DoorButton;
        //    set => SetValue(CloseDoorButtonProperty, value);
        //}

        //public DoorButton OpenDoorButton
        //{
        //    get => GetValue(OpenDoorButtonProperty) as DoorButton;
        //    set => SetValue(OpenDoorButtonProperty, value);
        //}

        //public readonly DependencyProperty CloseDoorButtonProperty =
        //    DependencyProperty.Register(nameof(CloseDoorButton), typeof(DoorButton), typeof(ButtonPanelControlViewModel));

        //public readonly DependencyProperty OpenDoorButtonProperty =
        //    DependencyProperty.Register(nameof(OpenDoorButton), typeof(DoorButton), typeof(ButtonPanelControlViewModel));

        public void Load(IButtonPanel buttonPanel)
        {
            this.ButtonPanel = ButtonPanel;
            this.FloorButtons = new ObservableCollection<FloorButton>(ButtonPanel.FloorButtons);
            this.CloseDoorButton = new ElevatorButtonViewModel( buttonPanel.CloseDoorButton);
            this.OpenDoorButton = new ElevatorButtonViewModel(buttonPanel.OpenDoorButton);
        }
    }
}
