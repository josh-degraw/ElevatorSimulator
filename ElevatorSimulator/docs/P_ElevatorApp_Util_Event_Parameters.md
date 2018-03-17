# Event.Parameters Property 
 

Optional parameters to further describe what happened with this <a href="T_ElevatorApp_Util_Event">Event</a>

**Namespace:**&nbsp;<a href="N_ElevatorApp_Util">ElevatorApp.Util</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public (Object , Object , , , T1 , T2 ,  ,  )[] Parameters { get; }
```


#### Property Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/mt744804" target="_blank">ValueTuple</a>(<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>, <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>)[]

## Examples
var e = new Event("event", ("param1", 1), ("param2", 2));

## See Also


#### Reference
<a href="T_ElevatorApp_Util_Event">Event Structure</a><br /><a href="N_ElevatorApp_Util">ElevatorApp.Util Namespace</a><br />