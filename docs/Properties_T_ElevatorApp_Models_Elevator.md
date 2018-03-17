# Elevator Properties
 

The <a href="T_ElevatorApp_Models_Elevator">Elevator</a> type exposes the following members.


## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_ButtonPanel">ButtonPanel</a></td><td> **Obsolete. **
The button panel inside of the elevator</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_CurrentCapacity">CurrentCapacity</a></td><td>
Represents the total weight currently held on the <a href="T_ElevatorApp_Models_Elevator">Elevator</a></td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_CurrentFloor">CurrentFloor</a></td><td>
Represents the current floor of the <a href="T_ElevatorApp_Models_Elevator">Elevator</a>
TODO: Either switch this to int? and have a null reference (`Nothing` in Visual Basic) mean it's moving, or figure out a better way to show the floor it's passing while it's moving</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_CurrentSpeed">CurrentSpeed</a></td><td>
TODO</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_Direction">Direction</a></td><td>
The direction the elevator is going</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_Door">Door</a></td><td>
The door of the <a href="T_ElevatorApp_Models_Elevator">Elevator</a></td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_ElevatorNumber">ElevatorNumber</a></td><td>
The number of this <a href="T_ElevatorApp_Models_Elevator">Elevator</a>. This is generated in order of initialization</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")![Static member](media/static.gif "Static member")</td><td><a href="P_ElevatorApp_Models_Elevator_FloorMovementSpeed">FloorMovementSpeed</a></td><td>
Same as <a href="F_ElevatorApp_Models_Elevator_FLOOR_MOVEMENT_SPEED">FLOOR_MOVEMENT_SPEED</a>, but in a format that can be used by the GUI</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_FloorsToStopAt">FloorsToStopAt</a></td><td>
A collection of the floor numbers that want the <a href="T_ElevatorApp_Models_Elevator">Elevator</a> to stop there</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_NextFloor">NextFloor</a></td><td>
Represents the closest elevator, If the <a href="T_ElevatorApp_Models_Elevator">Elevator</a> is idle, this will be the same as <a href="P_ElevatorApp_Models_Elevator_CurrentFloor">CurrentFloor</a></td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_Passengers">Passengers</a></td><td>
The people currently inside the <a href="T_ElevatorApp_Models_Elevator">Elevator</a></td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_RelativeSpeed">RelativeSpeed</a></td><td>
The relative speed of the <a href="T_ElevatorApp_Models_Elevator">Elevator</a>. Negative values indicate downward movement.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_Speed">Speed</a></td><td>
How fast the elevator is going 
TODO: Set the speed when the elevator moves</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_State">State</a></td><td>
Represents the state of the elevator</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_Subscribed">Subscribed</a></td><td>
Represents whether or not this object has performed the necessary steps to subscribe to the source, (*T*)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_TotalCapacity">TotalCapacity</a></td><td>
The total number of <a href="T_ElevatorApp_Models_Passenger">Passenger</a>s the <a href="T_ElevatorApp_Models_Elevator">Elevator</a> can hold</td></tr></table>&nbsp;
<a href="#elevator-properties">Back to Top</a>

## See Also


#### Reference
<a href="T_ElevatorApp_Models_Elevator">Elevator Class</a><br /><a href="N_ElevatorApp_Models">ElevatorApp.Models Namespace</a><br />