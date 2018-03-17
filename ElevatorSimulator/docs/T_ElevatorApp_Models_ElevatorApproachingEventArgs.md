# ElevatorApproachingEventArgs Class
 

Holds the data that represents an <a href="T_ElevatorApp_Models_Elevator">Elevator</a> movement


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="http://msdn2.microsoft.com/en-us/library/118wxtk3" target="_blank">System.EventArgs</a><br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_ElevatorApp_Models_ElevatorMovementEventArgs">ElevatorApp.Models.ElevatorMovementEventArgs</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ElevatorApp.Models.ElevatorApproachingEventArgs<br />
**Namespace:**&nbsp;<a href="N_ElevatorApp_Models">ElevatorApp.Models</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public class ElevatorApproachingEventArgs : ElevatorMovementEventArgs
```

The ElevatorApproachingEventArgs type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_ElevatorApproachingEventArgs__ctor">ElevatorApproachingEventArgs(ElevatorMovementEventArgs)</a></td><td>
The arguments that have been passed along the event cycle</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_ElevatorApproachingEventArgs__ctor_1">ElevatorApproachingEventArgs(Int32, Int32, Direction)</a></td><td>
Initializes a new instance of the ElevatorApproachingEventArgs class</td></tr></table>&nbsp;
<a href="#elevatorapproachingeventargs-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_ElevatorApproachingEventArgs_DestinationFloor">DestinationFloor</a></td><td>
The final destination of the <a href="T_ElevatorApp_Models_Elevator">Elevator</a> in the current path
 (Overrides <a href="P_ElevatorApp_Models_ElevatorMovementEventArgs_DestinationFloor">ElevatorMovementEventArgs.DestinationFloor</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_ElevatorMovementEventArgs_Direction">Direction</a></td><td>
The direction the elevator is going
 (Inherited from <a href="T_ElevatorApp_Models_ElevatorMovementEventArgs">ElevatorMovementEventArgs</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_ElevatorApproachingEventArgs_IntermediateFloor">IntermediateFloor</a></td><td>
The actual floor that is being approached</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_ElevatorApproachingEventArgs_ShouldStop">ShouldStop</a></td><td>
In the event, set this to `true` (`True` in Visual Basic) to allow the <a href="T_ElevatorApp_Models_Elevator">Elevator</a> to stop.</td></tr></table>&nbsp;
<a href="#elevatorapproachingeventargs-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bsc2ak47" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/4k87zsw7" target="_blank">Finalize</a></td><td>
Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/zdee4b3y" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dfwy45w9" target="_blank">GetType</a></td><td>
Gets the <a href="http://msdn2.microsoft.com/en-us/library/42892f65" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/57ctke0a" target="_blank">MemberwiseClone</a></td><td>
Creates a shallow copy of the current <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_ElevatorMovementEventArgs_ToString">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="T_ElevatorApp_Models_ElevatorMovementEventArgs">ElevatorMovementEventArgs</a>.)</td></tr></table>&nbsp;
<a href="#elevatorapproachingeventargs-class">Back to Top</a>

## See Also


#### Reference
<a href="N_ElevatorApp_Models">ElevatorApp.Models Namespace</a><br />