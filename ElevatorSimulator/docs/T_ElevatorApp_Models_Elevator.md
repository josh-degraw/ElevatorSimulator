# Elevator Class
 

Represents an elevator. 
Implements <a href="http://msdn2.microsoft.com/en-us/library/dd783449" target="_blank">IObserver(T)</a> to observe when a floor wants an elevator



## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="T_ElevatorApp_Models_ModelBase">ElevatorApp.Models.ModelBase</a><br />&nbsp;&nbsp;&nbsp;&nbsp;ElevatorApp.Models.Elevator<br />
**Namespace:**&nbsp;<a href="N_ElevatorApp_Models">ElevatorApp.Models</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public class Elevator : ModelBase, ISubcriber<ElevatorMasterController>
```

The Elevator type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_Elevator__ctor">Elevator</a></td><td>
Instantiates a new Elevator</td></tr></table>&nbsp;
<a href="#elevator-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_ButtonPanel">ButtonPanel</a></td><td> **Obsolete. **
The button panel inside of the elevator</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_CurrentCapacity">CurrentCapacity</a></td><td>
Represents the total weight currently held on the Elevator</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_CurrentFloor">CurrentFloor</a></td><td>
Represents the current floor of the Elevator
TODO: Either switch this to int? and have a null reference (`Nothing` in Visual Basic) mean it's moving, or figure out a better way to show the floor it's passing while it's moving</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_CurrentSpeed">CurrentSpeed</a></td><td>
TODO</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_Direction">Direction</a></td><td>
The direction the elevator is going</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_Door">Door</a></td><td>
The door of the Elevator</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_ElevatorNumber">ElevatorNumber</a></td><td>
The number of this Elevator. This is generated in order of initialization</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")![Static member](media/static.gif "Static member")</td><td><a href="P_ElevatorApp_Models_Elevator_FloorMovementSpeed">FloorMovementSpeed</a></td><td>
Same as <a href="F_ElevatorApp_Models_Elevator_FLOOR_MOVEMENT_SPEED">FLOOR_MOVEMENT_SPEED</a>, but in a format that can be used by the GUI</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_FloorsToStopAt">FloorsToStopAt</a></td><td>
A collection of the floor numbers that want the Elevator to stop there</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_NextFloor">NextFloor</a></td><td>
Represents the closest elevator, If the Elevator is idle, this will be the same as <a href="P_ElevatorApp_Models_Elevator_CurrentFloor">CurrentFloor</a></td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_Passengers">Passengers</a></td><td>
The people currently inside the Elevator</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_RelativeSpeed">RelativeSpeed</a></td><td>
The relative speed of the Elevator. Negative values indicate downward movement.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_Speed">Speed</a></td><td>
How fast the elevator is going 
TODO: Set the speed when the elevator moves</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_State">State</a></td><td>
Represents the state of the elevator</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_Subscribed">Subscribed</a></td><td>
Represents whether or not this object has performed the necessary steps to subscribe to the source, (*T*)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Elevator_TotalCapacity">TotalCapacity</a></td><td>
The total number of <a href="T_ElevatorApp_Models_Passenger">Passenger</a>s the Elevator can hold</td></tr></table>&nbsp;
<a href="#elevator-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_Elevator_AddPassenger">AddPassenger</a></td><td>
Adds a new <a href="T_ElevatorApp_Models_Passenger">Passenger</a> to the Elevator</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ModelBase_DependentPropertyChanged">DependentPropertyChanged</a></td><td>
Similar in function to <a href="M_ElevatorApp_Models_ModelBase_OnPropertyChanged__1">OnPropertyChanged(T)(T, String)</a>, but to use this you must explicitly state the property name, so this is useful for when the object depends on something but has no actual control over changing it, but still needs the change to be notified.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bsc2ak47" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_Elevator_Finalize">Finalize</a></td><td>
Finalizer. Decrements the global count of elevators
 (Overrides <a href="http://msdn2.microsoft.com/en-us/library/4k87zsw7" target="_blank">Object.Finalize()</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/zdee4b3y" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dfwy45w9" target="_blank">GetType</a></td><td>
Gets the <a href="http://msdn2.microsoft.com/en-us/library/42892f65" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/57ctke0a" target="_blank">MemberwiseClone</a></td><td>
Creates a shallow copy of the current <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ModelBase_OnPropertyChanged__1">OnPropertyChanged(T)</a></td><td>
Use in situations where you can't directly set the value, but still need to trigger the <a href="E_ElevatorApp_Models_ModelBase_PropertyChanged">PropertyChanged</a> event.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ModelBase_SetProperty__1">SetProperty(T)</a></td><td>
Sets the given backing property to the new value (if it has changed), and invokes <a href="E_ElevatorApp_Models_ModelBase_PropertyChanged">PropertyChanged</a>.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_Elevator_Subscribe">Subscribe</a></td><td>
Wires up this Elevator to the given <a href="T_ElevatorApp_Models_ElevatorMasterController">ElevatorMasterController</a></td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/7bxwbwt2" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr></table>&nbsp;
<a href="#elevator-class">Back to Top</a>

## Events
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_Elevator_Approaching">Approaching</a></td><td>
Called when the Elevator is getting close to a <a href="T_ElevatorApp_Models_Floor">Floor</a>, but before it's officially started arriving. After this event is handled,</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_Elevator_Arrived">Arrived</a></td><td>
Called when the Elevator has arrived, right before the doors open</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_Elevator_Arriving">Arriving</a></td><td>
Called when the elevator is about to arrive</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_Elevator_Departed">Departed</a></td><td>
Called when the Elevator has just left</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_Elevator_Departing">Departing</a></td><td>
Called when the Elevator is about to leave, right after the doors have closed</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_Elevator_PassengerAdded">PassengerAdded</a></td><td>
Called when a <a href="T_ElevatorApp_Models_Passenger">Passenger</a> has just entered the Elevator</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_Elevator_PassengerExited">PassengerExited</a></td><td>
Called when a <a href="T_ElevatorApp_Models_Passenger">Passenger</a> has just left the Elevator</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_ModelBase_PropertyChanged">PropertyChanged</a></td><td>
Occurs when a property value changes.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr></table>&nbsp;
<a href="#elevator-class">Back to Top</a>

## Fields
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public field](media/pubfield.gif "Public field")![Static member](media/static.gif "Static member")</td><td><a href="F_ElevatorApp_Models_Elevator_ACCELERATION_DELAY">ACCELERATION_DELAY</a></td><td>
The time it takes to get up to full speed once departing</td></tr><tr><td>![Public field](media/pubfield.gif "Public field")![Static member](media/static.gif "Static member")</td><td><a href="F_ElevatorApp_Models_Elevator_DECELERATION_DELAY">DECELERATION_DELAY</a></td><td>
The time it takes to slow down before arriving</td></tr><tr><td>![Public field](media/pubfield.gif "Public field")![Static member](media/static.gif "Static member")</td><td><a href="F_ElevatorApp_Models_Elevator_FLOOR_MOVEMENT_SPEED">FLOOR_MOVEMENT_SPEED</a></td><td>
The time it takes to move from one floor to another</td></tr></table>&nbsp;
<a href="#elevator-class">Back to Top</a>

## See Also


#### Reference
<a href="N_ElevatorApp_Models">ElevatorApp.Models Namespace</a><br />