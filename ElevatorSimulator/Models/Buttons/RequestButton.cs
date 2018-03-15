using ElevatorApp.Models.Enums;
using ElevatorApp.Models.Interfaces;
using FontAwesome;

namespace ElevatorApp.Models
{
    delegate void RequestFloor(int floorNumber, Direction requestDirection);

    /// <summary>
    /// Represents a button used to request an elevator
    /// </summary>
    public class RequestButton : ButtonBase<ElevatorCall>
    {
        public override void Push()
        {
         //   base.Pushed(this, new ElevatorCall(this.FloorNumber, this.RequestDirection));
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
