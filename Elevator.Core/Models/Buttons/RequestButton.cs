using System;
using System.Collections.Generic;
using System.Text;
using ElevatorApp.Core.Interfaces;

namespace ElevatorApp.Core.Models
{
    delegate void RequestFloor(int floorNumber, Direction requestDirection);

    public class RequestButton : Button<(int, Direction)>, IFloorBoundButton
    {
        public override void Push()
        {
            base.Pushed(this, (this.FloorNum, this.RequestDirection));
        }

        public override string Label => this.RequestDirection.ToString();

        public Direction RequestDirection { get; set; }

        public int FloorNum { get; set; }

        public RequestButton()
        {
            
        }

        public RequestButton(int floorNumber) : base()
        {
            this.FloorNum = floorNumber;
        }

    }
}
