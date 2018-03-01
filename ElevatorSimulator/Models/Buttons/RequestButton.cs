using ElevatorApp.Models.Interfaces;
using FontAwesome;

namespace ElevatorApp.Models
{
    delegate void RequestFloor(int floorNumber, Direction requestDirection);

    public class RequestButton : Button<ElevatorCall>, IFloorBoundButton
    {
        public override void Push()
        {
            base.Pushed(this, new ElevatorCall(this.FloorNum, this.RequestDirection));
        }

        public override string Label => this.RequestDirection.ToString();

        public Direction RequestDirection { get; set; }
        
        public int FloorNum { get; }
        
        public RequestButton(int floorNumber) : base($"RequestBtn {floorNumber}")
        {
            this.FloorNum = floorNumber;
        }

    }
}
