# RequestButton Properties
 

The <a href="T_ElevatorApp_Models_RequestButton">RequestButton</a> type exposes the following members.


## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected property](media/protproperty.gif "Protected property")</td><td><a href="P_ElevatorApp_Models_ButtonBase_1_actionArgs">actionArgs</a></td><td>
The arguments used in the <a href="E_ElevatorApp_Models_ButtonBase_1_OnPushed">OnPushed</a> and <a href="E_ElevatorApp_Models_ButtonBase_1_OnActionCompleted">OnActionCompleted</a> events. This is here to let the button do something with the same actions
 (Inherited from <a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_ButtonBase_1_Active">Active</a></td><td>
Represents whether the button has been pressed or not.
 (Inherited from <a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_FloorButton_ButtonType">ButtonType</a></td><td>
The type of button this is
 (Inherited from <a href="T_ElevatorApp_Models_FloorButton">FloorButton</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_RequestButton_DestinationFloor">DestinationFloor</a></td><td>
The number of the <a href="T_ElevatorApp_Models_Floor">Floor</a> that this button tells the <a href="T_ElevatorApp_Models_Elevator">Elevator</a> to go to.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_RequestButton_FloorNumber">FloorNumber</a></td><td>
The number of the <a href="T_ElevatorApp_Models_Floor">Floor</a> that this Button is on. Here, it functions as the Source floor.
 (Overrides <a href="P_ElevatorApp_Models_FloorButton_FloorNumber">FloorButton.FloorNumber</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_ButtonBase_1_Id">Id</a></td><td>
Should be a unique Id for this button
 (Inherited from <a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_ButtonBase_1_IsEnabled">IsEnabled</a></td><td>
Represents whether the button is able to be pressed or not
 (Inherited from <a href="T_ElevatorApp_Models_ButtonBase_1">ButtonBase(T)</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_RequestButton_Label">Label</a></td><td>
The text on the button
 (Overrides <a href="P_ElevatorApp_Models_FloorButton_Label">FloorButton.Label</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Models_FloorButton_Subscribed">Subscribed</a></td><td>
Represents whether or not this object has performed the necessary steps to subscribe to the source, (*T*)
 (Inherited from <a href="T_ElevatorApp_Models_FloorButton">FloorButton</a>.)</td></tr></table>&nbsp;
<a href="#requestbutton-properties">Back to Top</a>

## See Also


#### Reference
<a href="T_ElevatorApp_Models_RequestButton">RequestButton Class</a><br /><a href="N_ElevatorApp_Models">ElevatorApp.Models Namespace</a><br />