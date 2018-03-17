# Event Structure
 

Represents an item logged by <a href="T_ElevatorApp_Util_Logger">Logger</a>. Holds a name, timestamp, and optional descriptive parameters.

**Namespace:**&nbsp;<a href="N_ElevatorApp_Util">ElevatorApp.Util</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public struct Event
```

The Event type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_Event__ctor">Event</a></td><td>
Constructs a new Event</td></tr></table>&nbsp;
<a href="#event-structure">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Util_Event_Name">Name</a></td><td>
The name of the Event.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")![Code example](media/CodeExample.png "Code example")</td><td><a href="P_ElevatorApp_Util_Event_Parameters">Parameters</a></td><td>
Optional parameters to further describe what happened with this Event</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Util_Event_Timestamp">Timestamp</a></td><td>
The <a href="T_NodaTime_Instant">Instant</a> This Event occurred</td></tr></table>&nbsp;
<a href="#event-structure">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/2dts52z7" target="_blank">Equals</a></td><td>
Indicates whether this instance and a specified object are equal.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/aey3s293" target="_blank">ValueType</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/y3509fc2" target="_blank">GetHashCode</a></td><td>
Returns the hash code for this instance.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/aey3s293" target="_blank">ValueType</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dfwy45w9" target="_blank">GetType</a></td><td>
Gets the <a href="http://msdn2.microsoft.com/en-us/library/42892f65" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_Event_ToString">ToString</a></td><td>
Returns a string representation of this Event
 (Overrides <a href="http://msdn2.microsoft.com/en-us/library/wb77sz3h" target="_blank">ValueType.ToString()</a>.)</td></tr></table>&nbsp;
<a href="#event-structure">Back to Top</a>

## Fields
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public field](media/pubfield.gif "Public field")![Static member](media/static.gif "Static member")</td><td><a href="F_ElevatorApp_Util_Event_TIMESTAMP_FORMAT">TIMESTAMP_FORMAT</a></td><td>
The format of the string when serialized</td></tr></table>&nbsp;
<a href="#event-structure">Back to Top</a>

## See Also


#### Reference
<a href="N_ElevatorApp_Util">ElevatorApp.Util Namespace</a><br />