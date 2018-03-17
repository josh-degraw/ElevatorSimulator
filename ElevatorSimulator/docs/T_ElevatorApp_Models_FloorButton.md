# FloorButton Class
 

Represents a Button that tells the elevator to go to a specified floor


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="T_ElevatorApp_Models_ModelBase">ElevatorApp.Models.ModelBase</a><br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_ElevatorApp_Models_ButtonBase_1">ElevatorApp.Models.ButtonBase</a>(<a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">Int32</a>)<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ElevatorApp.Models.FloorButton<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_ElevatorApp_Models_RequestButton">ElevatorApp.Models.RequestButton</a><br />
**Namespace:**&nbsp;<a href="N_ElevatorApp_Models">ElevatorApp.Models</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public class FloorButton : ButtonBase<int>, 
	ISubcriber<(ElevatorMasterController , Elevator , )>
```

The FloorButton type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_FloorButton__ctor">FloorButton</a></td><td>
Initializes a new instance of the FloorButton class</td></tr></table>&nbsp;
<a href="#floorbutton-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected property](media/protproperty.gif "Protected property")</td><td><a href="P_ElevatorApp_Models_ButtonBase_1_actionArgs">actionArgs</a></td><td>
The arguments used in the <a href="E_ElevatorApp_Models_ButtonBase_1_OnPushed">OnPushed</a> and <a href="E_ElevatorApp_Models_ButtonBase_1_OnActionCompleted">OnActionCompleted</a> events. This is here to let the button do something with the same actions
 (Inherited from <a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_ButtonBase_1_Active">Active</a></td><td>
Represents whether the button has been pressed or not.
 (Inherited from <a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_FloorButton_ButtonType">ButtonType</a></td><td>
The type of button this is
 (Overrides <a href="P_ElevatorApp_Models_ButtonBase_1_ButtonType">ButtonBase(T).ButtonType</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_FloorButton_FloorNumber">FloorNumber</a></td><td>
The number of the <a href="T_ElevatorApp_Models_Floor">Floor</a> this FloorButton is tied to</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_ButtonBase_1_Id">Id</a></td><td>
Should be a unique Id for this button
 (Inherited from <a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_ButtonBase_1_IsEnabled">IsEnabled</a></td><td>
Represents whether the button is able to be pressed or not
 (Inherited from <a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_FloorButton_Label">Label</a></td><td>
The text on the button
 (Overrides <a href="P_ElevatorApp_Models_ButtonBase_1_Label">ButtonBase(T).Label</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_FloorButton_Subscribed">Subscribed</a></td><td>
Represents whether or not this object has performed the necessary steps to subscribe to the source, (*T*)</td></tr></table>&nbsp;
<a href="#floorbutton-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_ButtonBase_1_ActionCompleted">ActionCompleted()</a></td><td>
Trigger the <a href="E_ElevatorApp_Models_ButtonBase_1_OnActionCompleted">OnActionCompleted</a> event
 (Inherited from <a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ButtonBase_1_ActionCompleted_1">ActionCompleted(Object, T)</a></td><td>
Trigger the <a href="E_ElevatorApp_Models_ButtonBase_1_OnActionCompleted">OnActionCompleted</a> event
 (Inherited from <a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ModelBase_DependentPropertyChanged">DependentPropertyChanged</a></td><td>
Similar in function to <a href="M_ElevatorApp_Models_ModelBase_OnPropertyChanged__1">OnPropertyChanged(T)(T, String)</a>, but to use this you must explicitly state the property name, so this is useful for when the object depends on something but has no actual control over changing it, but still needs the change to be notified.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_FloorButton_Disable">Disable</a></td><td></td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bsc2ak47" target="_blank">Equals</a></td><td>
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
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_FloorButton_Push">Push</a></td><td> (Overrides <a href="M_ElevatorApp_Models_ButtonBase_1_Push">ButtonBase(T).Push()</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ButtonBase_1_Pushed">Pushed</a></td><td>
Trigger the <a href="E_ElevatorApp_Models_ButtonBase_1_OnPushed">OnPushed</a> event.
 (Inherited from <a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ModelBase_SetProperty__1">SetProperty(T)</a></td><td>
Sets the given backing property to the new value (if it has changed), and invokes <a href="E_ElevatorApp_Models_ModelBase_PropertyChanged">PropertyChanged</a>.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_FloorButton_Subscribe">Subscribe</a></td><td>
Perform the necessary steps to subscribe to the target.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_ButtonBase_1_ToString">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a>.)</td></tr></table>&nbsp;
<a href="#floorbutton-class">Back to Top</a>

## Events
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_ButtonBase_1_OnActionCompleted">OnActionCompleted</a></td><td>
Invoked after the events in <a href="M_ElevatorApp_Models_ButtonBase_1_Push">Push()</a> have completed.
 (Inherited from <a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a>.)</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_ButtonBase_1_OnActivated">OnActivated</a></td><td>
Invoked after the button has been pressed and the button is now active
 (Inherited from <a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a>.)</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_ButtonBase_1_OnDeactivated">OnDeactivated</a></td><td>
Invoked when the button is no longer active
 (Inherited from <a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a>.)</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_ButtonBase_1_OnPushed">OnPushed</a></td><td>
Invoked right when <a href="M_ElevatorApp_Models_ButtonBase_1_Push">Push()</a> is called.
 (Inherited from <a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a>.)</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_ModelBase_PropertyChanged">PropertyChanged</a></td><td>
Occurs when a property value changes.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr></table>&nbsp;
<a href="#floorbutton-class">Back to Top</a>

## See Also


#### Reference
<a href="N_ElevatorApp_Models">ElevatorApp.Models Namespace</a><br />