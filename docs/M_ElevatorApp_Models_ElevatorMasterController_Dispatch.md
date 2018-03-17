# ElevatorMasterController.Dispatch Method 
 

Dispatch an elevator to go to the given floor 
This will add the *destination* to <a href="P_ElevatorApp_Models_ElevatorMasterController_FloorsRequested">FloorsRequested</a> and be processed further by the <a href="T_ElevatorApp_Models_Elevator">Elevator</a> that is chosen to respond


**Namespace:**&nbsp;<a href="N_ElevatorApp_Models">ElevatorApp.Models</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public Task Dispatch(
	int destination
)
```


#### Parameters
&nbsp;<dl><dt>destination</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">System.Int32</a><br />The destination to be dispatched</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/dd235678" target="_blank">Task</a><br />\[Missing <returns> documentation for "M:ElevatorApp.Models.ElevatorMasterController.Dispatch(System.Int32)"\]

## See Also


#### Reference
<a href="T_ElevatorApp_Models_ElevatorMasterController">ElevatorMasterController Class</a><br /><a href="N_ElevatorApp_Models">ElevatorApp.Models Namespace</a><br />