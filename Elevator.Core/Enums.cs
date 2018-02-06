using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorApp.Core
{

    public enum Direction { Up, Down }

    public enum DoorState
    {
        Closed,
        Closing,
        Open,
        Opening,
    }

    public enum DoorButtonType { Open, Close }

    public enum ElevatorState
    {
        Idle,
        GoingUp,
        GoingDown,
        Stopped
    }

    public enum PassengerState { Out, In, Transition }
}
