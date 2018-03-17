# AsyncObservableCollection(*T*).RemoveAt Method 
 

Removes the <a href="http://msdn2.microsoft.com/en-us/library/5y536ey6" target="_blank">IList(T)</a> item at the specified index.

**Namespace:**&nbsp;<a href="N_ElevatorApp_Util">ElevatorApp.Util</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public void RemoveAt(
	int index
)
```


#### Parameters
&nbsp;<dl><dt>index</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">System.Int32</a><br />The zero-based index of the item to remove.</dd></dl>

#### Implements
<a href="http://msdn2.microsoft.com/en-us/library/c93ab5c9" target="_blank">IList(T).RemoveAt(Int32)</a><br /><a href="http://msdn2.microsoft.com/en-us/library/x5zwtyhy" target="_blank">IList.RemoveAt(Int32)</a><br />

## Exceptions
&nbsp;<table><tr><th>Exception</th><th>Condition</th></tr><tr><td><a href="http://msdn2.microsoft.com/en-us/library/8xt94y6e" target="_blank">ArgumentOutOfRangeException</a></td><td>*index* is not a valid index in the <a href="http://msdn2.microsoft.com/en-us/library/5y536ey6" target="_blank">IList(T)</a>.</td></tr><tr><td><a href="http://msdn2.microsoft.com/en-us/library/8a7a4e64" target="_blank">NotSupportedException</a></td><td>The <a href="http://msdn2.microsoft.com/en-us/library/5y536ey6" target="_blank">IList(T)</a> is read-only.</td></tr></table>

## See Also


#### Reference
<a href="T_ElevatorApp_Util_AsyncObservableCollection_1">AsyncObservableCollection(T) Class</a><br /><a href="N_ElevatorApp_Util">ElevatorApp.Util Namespace</a><br />