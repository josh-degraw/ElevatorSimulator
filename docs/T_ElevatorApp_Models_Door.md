# Door Class
 

Represents the door of the elevator


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="T_ElevatorApp_Models_ModelBase">ElevatorApp.Models.ModelBase</a><br />&nbsp;&nbsp;&nbsp;&nbsp;ElevatorApp.Models.Door<br />
**Namespace:**&nbsp;<a href="N_ElevatorApp_Models">ElevatorApp.Models</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public class Door : ModelBase, ISubcriber<Elevator>
```

The Door type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_Door__ctor">Door</a></td><td>
Constructs a new Door</td></tr></table>&nbsp;
<a href="#door-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Door_DoorState">DoorState</a></td><td>
The state of the Door</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_Door_Subscribed">Subscribed</a></td><td>
Represents if this Door has been subscribed</td></tr></table>&nbsp;
<a href="#door-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ModelBase_DependentPropertyChanged">DependentPropertyChanged</a></td><td>
Similar in function to <a href="M_ElevatorApp_Models_ModelBase_OnPropertyChanged__1">OnPropertyChanged(T)(T, String)</a>, but to use this you must explicitly state the property name, so this is useful for when the object depends on something but has no actual control over changing it, but still needs the change to be notified.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bsc2ak47" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/4k87zsw7" target="_blank">Finalize</a></td><td>
Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/zdee4b3y" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dfwy45w9" target="_blank">GetType</a></td><td>
Gets the <a href="http://msdn2.microsoft.com/en-us/library/42892f65" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/57ctke0a" target="_blank">MemberwiseClone</a></td><td>
Creates a shallow copy of the current <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ModelBase_OnPropertyChanged__1">OnPropertyChanged(T)</a></td><td>
Use in situations where you can't directly set the value, but still need to trigger the <a href="E_ElevatorApp_Models_ModelBase_PropertyChanged">PropertyChanged</a> event.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_Door_RequestClose">RequestClose</a></td><td>
Ask the Door to close</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_Door_RequestOpen">RequestOpen</a></td><td>
Ask the Door to open</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ModelBase_SetProperty__1">SetProperty(T)</a></td><td>
Sets the given backing property to the new value (if it has changed), and invokes <a href="E_ElevatorApp_Models_ModelBase_PropertyChanged">PropertyChanged</a>.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_Door_Subscribe">Subscribe</a></td><td>
Subscribe this Door to its parent <a href="T_ElevatorApp_Models_Elevator">Elevator</a></td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/7bxwbwt2" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr></table>&nbsp;
<a href="#door-class">Back to Top</a>

## Events
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_Door_Closed">Closed</a></td><td>
Invoked when the Door has just Closed</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_Door_CloseRequested">CloseRequested</a></td><td>
Invoked when the Door has just been asked to close</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_Door_Closing">Closing</a></td><td>
Invoked when the Door has just started Closing</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_Door_Opened">Opened</a></td><td>
Invoked when the Door has just Opened</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_Door_Opening">Opening</a></td><td>
Invoked when the Door has just started Opening</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_Door_OpenRequested">OpenRequested</a></td><td>
Invoked when the Door has just been asked to open</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_ModelBase_PropertyChanged">PropertyChanged</a></td><td>
Occurs when a property value changes.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr></table>&nbsp;
<a href="#door-class">Back to Top</a>

## Fields
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public field](media/pubfield.gif "Public field")![Static member](media/static.gif "Static member")</td><td><a href="F_ElevatorApp_Models_Door_TIME_SPENT_OPEN_SECONDS">TIME_SPENT_OPEN_SECONDS</a></td><td>
The number of seconds the Door stays open</td></tr><tr><td>![Public field](media/pubfield.gif "Public field")![Static member](media/static.gif "Static member")</td><td><a href="F_ElevatorApp_Models_Door_TRANSITION_TIME_SECONDS">TRANSITION_TIME_SECONDS</a></td><td>
The number of seconds it takes for the Door to close and to open</td></tr></table>&nbsp;
<a href="#door-class">Back to Top</a>

## See Also


#### Reference
<a href="N_ElevatorApp_Models">ElevatorApp.Models Namespace</a><br />