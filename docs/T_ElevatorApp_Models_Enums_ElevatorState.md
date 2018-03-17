# ElevatorState Enumeration
 

The state an <a href="T_ElevatorApp_Models_Elevator">Elevator</a> is in

**Namespace:**&nbsp;<a href="N_ElevatorApp_Models_Enums">ElevatorApp.Models.Enums</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public enum ElevatorState
```


## Members
&nbsp;<table><tr><th></th><th>Member name</th><th>Value</th><th>Description</th></tr><tr><td /><td target="F:ElevatorApp.Models.Enums.ElevatorState.Idle">**Idle**</td><td>0</td><td>The <a href="T_ElevatorApp_Models_Elevator">Elevator</a> has no <a href="T_ElevatorApp_Models_Floor">Floor</a>s waiting for it, and no <a href="T_ElevatorApp_Models_Passenger">Passenger</a>s in it with a destination queued.</td></tr><tr><td /><td target="F:ElevatorApp.Models.Enums.ElevatorState.Departing">**Departing**</td><td>1</td><td>The <a href="T_ElevatorApp_Models_Elevator">Elevator</a> is in the process of leaving a <a href="T_ElevatorApp_Models_Floor">Floor</a>. In this state, the <a href="T_ElevatorApp_Models_Elevator">Elevator</a> has not yet reached full speed.</td></tr><tr><td /><td target="F:ElevatorApp.Models.Enums.ElevatorState.Departed">**Departed**</td><td>2</td><td>The <a href="T_ElevatorApp_Models_Elevator">Elevator</a> has just left a <a href="T_ElevatorApp_Models_Floor">Floor</a></td></tr><tr><td /><td target="F:ElevatorApp.Models.Enums.ElevatorState.Arriving">**Arriving**</td><td>3</td><td>The <a href="T_ElevatorApp_Models_Elevator">Elevator</a> has just started slowing down to approach a <a href="T_ElevatorApp_Models_Floor">Floor</a>.</td></tr><tr><td /><td target="F:ElevatorApp.Models.Enums.ElevatorState.Arrived">**Arrived**</td><td>4</td><td>The <a href="T_ElevatorApp_Models_Elevator">Elevator</a> has just arrived at a <a href="T_ElevatorApp_Models_Floor">Floor</a>.</td></tr></table>

## See Also


#### Reference
<a href="N_ElevatorApp_Models_Enums">ElevatorApp.Models.Enums Namespace</a><br />