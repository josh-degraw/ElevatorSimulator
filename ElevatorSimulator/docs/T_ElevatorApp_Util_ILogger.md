# ILogger Interface
 

\[Missing <summary> documentation for "T:ElevatorApp.Util.ILogger"\]

**Namespace:**&nbsp;<a href="N_ElevatorApp_Util">ElevatorApp.Util</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public interface ILogger
```

The ILogger type exposes the following members.


## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Util_ILogger_Events">Events</a></td><td>
A collection of the <a href="T_ElevatorApp_Util_Event">Event</a>s that have been logged</td></tr></table>&nbsp;
<a href="#ilogger-interface">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_ILogger_AddLogger">AddLogger</a></td><td>
Add a <a href="http://msdn2.microsoft.com/en-us/library/ywxh2328" target="_blank">TextWriter</a> to automatically write any new log item as it's added.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_ILogger_ClearItems">ClearItems</a></td><td>
Removes all <a href="T_ElevatorApp_Util_Event">Event</a>s logged up to this point.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Code example](media/CodeExample.png "Code example")</td><td><a href="M_ElevatorApp_Util_ILogger_LogEvent_1">LogEvent(String, ValueTuple(Object, Object)[])</a></td><td>
Log a new <a href="T_ElevatorApp_Util_Event">Event</a></td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_ILogger_LogEvent">LogEvent(Event, Int32)</a></td><td /></tr></table>&nbsp;
<a href="#ilogger-interface">Back to Top</a>

## Events
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Util_ILogger_ItemLogged">ItemLogged</a></td><td>
Invoked whenever a new <a href="T_ElevatorApp_Util_Event">Event</a> is logged. Can be used to trigger calculations, update text fields, etc.</td></tr></table>&nbsp;
<a href="#ilogger-interface">Back to Top</a>

## See Also


#### Reference
<a href="N_ElevatorApp_Util">ElevatorApp.Util Namespace</a><br />