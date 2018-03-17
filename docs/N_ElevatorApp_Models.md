# ElevatorApp.Models Namespace
 

This namespace contains classes and structs that model the behavior of the elevator and related objects.


## Classes
&nbsp;<table><tr><th></th><th>Class</th><th>Description</th></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a></td><td>
Base class for Buttons</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_ButtonPanel">ButtonPanel</a></td><td>
Represents the base class of a button panel. Takes care of subscription of</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_ButtonPanelBase">ButtonPanelBase</a></td><td>
Represents the base class of a button panel. Takes care of subscription of</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_Door">Door</a></td><td>
Represents the door of the elevator</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_DoorButton">DoorButton</a></td><td>
Represents a Button that is used to request that a <a href="T_ElevatorApp_Models_Door">Door</a> change its state</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_DoorStateChangeEventArgs">DoorStateChangeEventArgs</a></td><td>
EventArgs class</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_Elevator">Elevator</a></td><td>
Represents an elevator. 
Implements <a href="http://msdn2.microsoft.com/en-us/library/dd783449" target="_blank">IObserver(T)</a> to observe when a floor wants an elevator</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_ElevatorApproachingEventArgs">ElevatorApproachingEventArgs</a></td><td>
Holds the data that represents an <a href="T_ElevatorApp_Models_Elevator">Elevator</a> movement</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_ElevatorCallPanel">ElevatorCallPanel</a></td><td>
Represents a panel of buttons used to call an <a href="T_ElevatorApp_Models_Elevator">Elevator</a>, from outside of the <a href="T_ElevatorApp_Models_Elevator">Elevator</a> (e.g. on a <a href="T_ElevatorApp_Models_Floor">Floor</a>).</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_ElevatorMasterController">ElevatorMasterController</a></td><td /></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_ElevatorMovementEventArgs">ElevatorMovementEventArgs</a></td><td>
Holds the data that represents an <a href="T_ElevatorApp_Models_Elevator">Elevator</a> movement</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_ElevatorSettings">ElevatorSettings</a></td><td /></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_ElevatorSimulator">ElevatorSimulator</a></td><td> **Obsolete. **</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_Floor">Floor</a></td><td /></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_FloorButton">FloorButton</a></td><td>
Represents a Button that tells the elevator to go to a specified floor</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_ModelBase">ModelBase</a></td><td>
Base class to implement <a href="http://msdn2.microsoft.com/en-us/library/ms133020" target="_blank">INotifyPropertyChanged</a>, with a helper method (<a href="M_ElevatorApp_Models_ModelBase_SetProperty__1">SetProperty(T)(T, T, Boolean, String)</a>)</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_Passenger">Passenger</a></td><td /></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Models_RequestButton">RequestButton</a></td><td>
Represents a button used to request an <a href="T_ElevatorApp_Models_Elevator">Elevator</a> from a <a href="T_ElevatorApp_Models_Floor">Floor</a></td></tr></table>

## Delegates
&nbsp;<table><tr><th></th><th>Delegate</th><th>Description</th></tr><tr><td>![Public delegate](media/pubdelegate.gif "Public delegate")</td><td><a href="T_ElevatorApp_Models_DoorStateChangeRequestHandler">DoorStateChangeRequestHandler</a></td><td>
Handle the changing state of a <a href="T_ElevatorApp_Models_Door">Door</a></td></tr></table>&nbsp;
