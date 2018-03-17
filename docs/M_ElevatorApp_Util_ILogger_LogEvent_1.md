# ILogger.LogEvent Method (String, ValueTuple(Object, Object)[])
 

Log a new <a href="T_ElevatorApp_Util_Event">Event</a>

**Namespace:**&nbsp;<a href="N_ElevatorApp_Util">ElevatorApp.Util</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
void LogEvent(
	string name,
	params (Object , Object , )[] parameters
)
```


#### Parameters
&nbsp;<dl><dt>name</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />The name of the event</dd><dt>parameters</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/mt744804" target="_blank">System.ValueTuple</a>(<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>, <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>)[]<br />Any parameters to be associated with the <a href="T_ElevatorApp_Util_Event">Event</a></dd></dl>

## Examples
Logger.LogEvent("event name", ("param1", 1), ("param2", 2));

## See Also


#### Reference
<a href="T_ElevatorApp_Util_ILogger">ILogger Interface</a><br /><a href="Overload_ElevatorApp_Util_ILogger_LogEvent">LogEvent Overload</a><br /><a href="N_ElevatorApp_Util">ElevatorApp.Util Namespace</a><br />