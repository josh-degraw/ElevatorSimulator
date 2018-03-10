namespace ElevatorApp.Models
{
    public enum DoorState
    {
        Closed,
        Closing,
        Opened,
        Opening,
    }

    public enum DoorType { Single, Double }

    public enum DoorButtonType { Open, Close, RequestUp, RequestDown, Floor }

    public enum Direction { Up = 1, Down = 2 }

    public enum ElevatorState
    {
        Idle = 0,
        GoingUp = 1,
        GoingDown = 2
    }

    public enum PassengerState
    {
        Waiting = 0,
        Transition = 1,
        In = 2,
        Out = 3,
    }

    public enum ButtonType
    {
        DoorOpen,
        DoorClose,
        Floor,
        Call
    }
}
