# PassengerState Enumeration
 

Represents the state of a <a href="T_ElevatorApp_Models_Passenger">Passenger</a>.

**Namespace:**&nbsp;<a href="N_ElevatorApp_Models_Enums">ElevatorApp.Models.Enums</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public enum PassengerState
```


## Members
&nbsp;<table><tr><th></th><th>Member name</th><th>Value</th><th>Description</th></tr><tr><td /><td target="F:ElevatorApp.Models.Enums.PassengerState.Waiting">**Waiting**</td><td>0</td><td>The <a href="T_ElevatorApp_Models_Passenger">Passenger</a> is waiting on a <a href="T_ElevatorApp_Models_Floor">Floor</a> for an <a href="T_ElevatorApp_Models_Elevator">Elevator</a> to take them somewhere.</td></tr><tr><td /><td target="F:ElevatorApp.Models.Enums.PassengerState.Transition">**Transition**</td><td>1</td><td>The <a href="T_ElevatorApp_Models_Passenger">Passenger</a> is either entering or exiting the <a href="T_ElevatorApp_Models_Elevator">Elevator</a>. Either way they are in transition from one final state to another.</td></tr><tr><td /><td target="F:ElevatorApp.Models.Enums.PassengerState.In">**In**</td><td>2</td><td>The <a href="T_ElevatorApp_Models_Passenger">Passenger</a> is safely inside of the <a href="T_ElevatorApp_Models_Elevator">Elevator</a></td></tr><tr><td /><td target="F:ElevatorApp.Models.Enums.PassengerState.Out">**Out**</td><td>3</td><td>The <a href="T_ElevatorApp_Models_Passenger">Passenger</a> is has safely left the <a href="T_ElevatorApp_Models_Elevator">Elevator</a></td></tr></table>

## See Also


#### Reference
<a href="N_ElevatorApp_Models_Enums">ElevatorApp.Models.Enums Namespace</a><br />