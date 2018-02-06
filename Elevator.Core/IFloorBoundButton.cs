using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorApp.Core
{
    public interface IFloorBoundButton : IButton
    {
        int FloorNum { get; }
    }
}
