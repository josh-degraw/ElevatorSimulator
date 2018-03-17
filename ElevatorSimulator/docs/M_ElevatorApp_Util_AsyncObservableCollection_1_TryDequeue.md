# AsyncObservableCollection(*T*).TryDequeue Method 
 

Tries to remove the first item from the collection, mimicking the behavior of <a href="http://msdn2.microsoft.com/en-us/library/dd267265" target="_blank">ConcurrentQueue(T)</a>

**Namespace:**&nbsp;<a href="N_ElevatorApp_Util">ElevatorApp.Util</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public bool TryDequeue(
	out T res
)
```


#### Parameters
&nbsp;<dl><dt>res</dt><dd>Type: <a href="T_ElevatorApp_Util_AsyncObservableCollection_1">*T*</a><br />The item that was dequeud</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/a28wyd50" target="_blank">Boolean</a><br />True if an item was successfully dequeued, else false

## Remarks
Written by Josh DeGraw

## See Also


#### Reference
<a href="T_ElevatorApp_Util_AsyncObservableCollection_1">AsyncObservableCollection(T) Class</a><br /><a href="N_ElevatorApp_Util">ElevatorApp.Util Namespace</a><br />