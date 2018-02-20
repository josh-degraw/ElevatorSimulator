using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorApp.Core.Models
{
    public class ElevatorSettings
    {
        public int Speed { get; set; }
        public int Capacity { get; set; }

        public DoorType DoorType { get; set; } = DoorType.Single;
    }
}
