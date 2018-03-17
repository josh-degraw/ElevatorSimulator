# ButtonPanelBase Class
 

Represents the base class of a button panel. Takes care of subscription of


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;ElevatorApp.Models.ButtonPanelBase<br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_ElevatorApp_Models_ButtonPanel">ElevatorApp.Models.ButtonPanel</a><br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_ElevatorApp_Models_ElevatorCallPanel">ElevatorApp.Models.ElevatorCallPanel</a><br />
**Namespace:**&nbsp;<a href="N_ElevatorApp_Models">ElevatorApp.Models</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public abstract class ButtonPanelBase : IReadOnlyCollection<FloorButton>, 
	IEnumerable<FloorButton>, IEnumerable, ISubcriber<(ElevatorMasterController , Elevator , )>, 
	ISubcriber<ElevatorMasterController>, IObservable<int>
```

The ButtonPanelBase type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ButtonPanelBase__ctor">ButtonPanelBase</a></td><td>
Initialize a new ButtonPanelBase</td></tr></table>&nbsp;
<a href="#buttonpanelbase-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected property](media/protproperty.gif "Protected property")</td><td><a href="P_ElevatorApp_Models_ButtonPanelBase__floorButtons">_floorButtons</a></td><td>
The actual collection of <a href="T_ElevatorApp_Models_FloorButton">FloorButton</a>s</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_ButtonPanelBase_FloorButtons">FloorButtons</a></td><td>
A collection of <a href="T_ElevatorApp_Models_FloorButton">FloorButton</a>s that will appear on this panel</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_ButtonPanelBase_Subscribed">Subscribed</a></td><td /></tr></table>&nbsp;
<a href="#buttonpanelbase-class">Back to Top</a>

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
Subscribes this ButtonPanelBase to an <a href="http://msdn2.microsoft.com/en-us/library/dd990377" target="_blank">IObservable(T)</a>, in order for the subscriber to be notified whenever any of the <a href="T_ElevatorApp_Models_FloorButton">FloorButton</a>s on this ButtonPanelBase have been activated.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_ButtonPanelBase_Subscribe">Subscribe(ElevatorMasterController)</a></td><td /></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Models_ButtonPanelBase_Subscribe_2">Subscribe(ValueTuple(ElevatorMasterController, Elevator))</a></td><td>
Subscribe to a <a href="T_ElevatorApp_Models_ElevatorMasterController">ElevatorMasterController</a> and an <a href="T_ElevatorApp_Models_Elevator">Elevator</a>.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/7bxwbwt2" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr></table>&nbsp;
<a href="#buttonpanelbase-class">Back to Top</a>

## Extension Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public Extension Method](media/pubextension.gif "Public Extension Method")</td><td><a href="M_ElevatorApp_Util_Extensions_MinByOrDefault__2">MinByOrDefault(FloorButton, TKey)</a></td><td> (Defined by <a href="T_ElevatorApp_Util_Extensions">Extensions</a>.)</td></tr></table>&nbsp;
<a href="#buttonpanelbase-class">Back to Top</a>

## See Also


#### Reference
<a href="N_ElevatorApp_Models">ElevatorApp.Models Namespace</a><br />