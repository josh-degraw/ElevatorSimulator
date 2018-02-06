using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorApp.Core
{
    public class Building
    {
        public int FloorCount { get; set; }
        public int FloorHeight { get; set; }

        public ICollection<ElevatorController> ElevatorControllers { get; set; }
    }
}
