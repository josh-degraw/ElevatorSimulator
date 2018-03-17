# ElevatorCallPanel Class
 

Represents a panel of buttons used to call an <a href="T_ElevatorApp_Models_Elevator">Elevator</a>, from outside of the <a href="T_ElevatorApp_Models_Elevator">Elevator</a> (e.g. on a <a href="T_ElevatorApp_Models_Floor">Floor</a>).


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="T_ElevatorApp_Models_ButtonPanelBase">ElevatorApp.Models.ButtonPanelBase</a><br />&nbsp;&nbsp;&nbsp;&nbsp;ElevatorApp.Models.ElevatorCallPanel<br />
**Namespace:**&nbsp;<a href="N_ElevatorApp_Models">ElevatorApp.Models</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public class ElevatorCallPanel : ButtonPanelBase, 
	ISubcriber<ElevatorMasterController>, ISubcriber<(ElevatorMasterController , Elevator , )>
```

The ElevatorCallPanel type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_ElevatorCallPanel__ctor">ElevatorCallPanel</a></td><td>
Create a new ElevatorCallPanel</td></tr></table>&nbsp;
<a href="#elevatorcallpanel-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected property](media/protproperty.gif "Protected property")</td><td><a href="P_ElevatorApp_Models_ElevatorCallPanel__floorButtons">_floorButtons</a></td><td>
The actual collection of <a href="T_ElevatorApp_Models_FloorButton">FloorButton</a>s
 (Overrides <a href="P_ElevatorApp_Models_ButtonPanelBase__floorButtons">ButtonPanelBase._floorButtons</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_ButtonPanelBase_FloorButtons">FloorButtons</a></td><td>
A collection of <a href="T_ElevatorApp_Models_FloorButton">FloorButton</a>s that will appear on this panel
 (Inherited from <a href="T_ElevatorApp_Models_ButtonPanelBase">ButtonPanelBase</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_ElevatorCallPanel_FloorNumber">FloorNumber</a></td><td>
The number of the <a href="T_ElevatorApp_Models_Floor">Floor</a> this ElevatorCallPanel is on</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_ButtonPanelBase_Subscribed">Subscribed</a></td><td> (Inherited from <a href="T_ElevatorApp_Models_ButtonPanelBase">ButtonPanelBase</a>.)</td></tr></table>&nbsp;
<a href="#elevatorcallpanel-class">Back to Top</a>

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
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_ButtonPanelBase_Subscribe_1">Subscribe(IObserver(Int32))</a></td><td>
Subscribes this <a href="T_ElevatorApp_Models_ButtonPanelBase">ButtonPanelBase</a> to an <a href="http://msdn2.microsoft.com/en-us/library/dd990377" target="_blank">IObservable(T)</a>, in order for the subscriber to be notified whenever any of the <a href="T_ElevatorApp_Models_FloorButton">FloorButton</a>s on this <a href="T_ElevatorApp_Models_ButtonPanelBase">ButtonPanelBase</a> have been activated.
 (Inherited from <a href="T_ElevatorApp_Models_ButtonPanelBase">ButtonPanelBase</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_ElevatorCallPanel_Subscribe">Subscribe(ElevatorMasterController)</a></td><td>
Perform the necessary steps to subscribe to the target.
 (Overrides <a href="M_ElevatorApp_Models_ButtonPanelBase_Subscribe">ButtonPanelBase.Subscribe(ElevatorMasterController)</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_ElevatorCallPanel_Subscribe_1">Subscribe(ValueTuple(ElevatorMasterController, Elevator))</a></td><td>
Perform the necessary steps to subscribe to the target.
 (Overrides <a href="M_ElevatorApp_Models_ButtonPanelBase_Subscribe_2">ButtonPanelBase.Subscribe(ValueTuple(ElevatorMasterController, Elevator))</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/7bxwbwt2" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr></table>&nbsp;
<a href="#elevatorcallpanel-class">Back to Top</a>

## See Also


#### Reference
<a href="N_ElevatorApp_Models">ElevatorApp.Models Namespace</a><br />