namespace ElevatorApp.Models.Enums
{
    /// <summary>
    /// Represent the state of a <see cref="Door"/>
    /// </summary>
    public enum DoorState
    {
        /// <summary>
        /// The door has just been closed
        /// </summary>
        Closed,

        /// <summary>
        /// The door has just started closing
        /// </summary>
        Closing,

        /// <summary>
        /// The door has just opened
        /// </summary>
        Opened,

        /// <summary>
        /// The door has just started opening
        /// </summary>
        Opening,
    }

    /// <summary>
    /// Represents the type of <see cref="Door"/> an <see cref="Elevator"/> has
    /// </summary>
    public enum DoorType
    {
        /// <summary>
        /// One door
        /// </summary>
        Single,

        /// <summary>
        /// Two door (not yet implemented)
        /// </summary>
        Double
    }

    /// <summary>
    /// The kind of Button 
    /// </summary>
    public enum ButtonType
    {
        /// <summary>
        /// Opens the <see cref="Door"/>
        /// </summary>
        Open,
        
        /// <summary>
        /// Closes the <see cref="Door"/>
        /// </summary>
        Close,

        /// <summary>
        /// Requests the <see cref="Elevator"/> to go up
        /// </summary>
        RequestUp,

        /// <summary>
        /// Requests the <see cref="Elevator"/> to go down
        /// </summary>
        RequestDown,

        /// <summary>
        /// Requests the <see cref="Elevator"/> to go the given floor
        /// </summary>
        Floor
    }

    /// <summary>
    /// Vertical direction that the <see cref="Elevator"/> can move
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Upwards
        /// </summary>
        Up = 1,

        /// <summary>
        /// Downwards
        /// </summary>
        Down = 2
    }

    /// <summary>
    /// Represents the direction an <see cref="Elevator"/> is currently moving in.
    /// </summary>
    public enum ElevatorDirection
    {
        /// <summary>
        /// The <see cref="Elevator"/> is not moving.
        /// </summary>
        None = 0,

        /// <summary>
        /// The <see cref="Elevator"/> is currently going up
        /// </summary>
        GoingUp = 1,

        /// <summary>
        /// The <see cref="Elevator"/> is currently going down
        /// </summary>
        GoingDown = 2
    }

    /// <summary>
    /// The state an <see cref="Elevator"/> is in
    /// </summary>
    public enum ElevatorState
    {
        /// <summary>
        /// The <see cref="Elevator"/> has no <see cref="Floor"/>s waiting for it, and no <see cref="Passenger"/>s in it with a destination queued.
        /// </summary>
        Idle = 0,

        /// <summary>
        /// The <see cref="Elevator"/> is in the process of leaving a <see cref="Floor"/>. In this state, the <see cref="Elevator"/> has not yet reached full speed.
        /// </summary>
        Departing = 1,

        /// <summary>
        /// The <see cref="Elevator"/> has just left a <see cref="Floor"/>
        /// </summary>
        Departed = 2,
        
        /// <summary>
        /// The <see cref="Elevator"/> has just started slowing down to approach a <see cref="Floor"/>.
        /// </summary>
        Arriving = 3,

        /// <summary>
        /// The <see cref="Elevator"/> has just arrived at a <see cref="Floor"/>.
        /// </summary>
        Arrived = 4
    }

    /// <summary>
    /// Represents the state of a <see cref="Passenger"/>.
    /// </summary>
    public enum PassengerState
    {
        /// <summary>
        /// The <see cref="Passenger"/> is waiting on a <see cref="Floor"/> for an <see cref="Elevator"/> to take them somewhere.
        /// </summary>
        Waiting = 0,

        /// <summary>
        /// The <see cref="Passenger"/> is either entering or exiting the <see cref="Elevator"/>. Either way they are in transition from one final state to another.
        /// </summary>
        Transition = 1,

        /// <summary>
        /// The <see cref="Passenger"/> is safely inside of the <see cref="Elevator"/>
        /// </summary>
        In = 2,

        /// <summary>
        /// The <see cref="Passenger"/> is has safely left the <see cref="Elevator"/>
        /// </summary>
        Out = 3,
    }
}
