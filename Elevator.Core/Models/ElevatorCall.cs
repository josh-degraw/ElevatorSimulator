using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorApp.Core.Models
{
    public struct ElevatorCall
    {
        public int DestinationFloor { get; set; }
        public Direction RequestDirection { get; set; }

        public ElevatorCall(int destination, Direction direction)
        {
            this.DestinationFloor = destination;
            this.RequestDirection = direction;
        }

        #region Operators
        public void Deconstruct(out int destinationFloor, out Direction direction)
        {
            destinationFloor = this.DestinationFloor;
            direction = this.RequestDirection;
        }

        public static implicit operator int(ElevatorCall call) => call.DestinationFloor;
        #endregion
    }
}
