﻿using System;
using System.Collections.Generic;
using System.Text;
using ElevatorApp.Core.Interfaces;

namespace ElevatorApp.Core.Models
{
    public class DoorButton : Button<DoorButtonType>, IButton
    {
        private DoorButtonType _doorButtonType;

        public DoorButtonType DoorButtonType
        {
            get => _doorButtonType;
            set => SetProperty(ref _doorButtonType, value);
        } 

        public override string Label
        {
            get
            {
                switch (DoorButtonType)
                {
                    case DoorButtonType.Open:
                        return "Open";
                    case DoorButtonType.Close:
                        return "Close";
                    default: return "Door";
                }
            }
        }
        

        public override event EventHandler OnActivated;

        public override void Push()
        {
            this.Pushed(this, this.DoorButtonType);
        }

        private DoorButton(DoorButtonType type):base($"DoorBtn {type}")
        {
            this._doorButtonType = type;
        }

        private DoorButton() : this(DoorButtonType.Close)
        {

        }

        public static DoorButton Open() => new DoorButton(DoorButtonType.Open);

        public static DoorButton Close() => new DoorButton(DoorButtonType.Close);
    }
}