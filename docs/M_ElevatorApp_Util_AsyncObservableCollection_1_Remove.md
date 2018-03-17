# AsyncObservableCollection(*T*).Remove Method 
 

Removes the first occurrence of a specific object from the <a href="http://msdn2.microsoft.com/en-us/library/92t2ye13" target="_blank">ICollection(T)</a>.

**Namespace:**&nbsp;<a href="N_ElevatorApp_Util">ElevatorApp.Util</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public bool Remove(
	T item
)
```


#### Parameters
&nbsp;<dl><dt>item</dt><dd>Type: <a href="T_ElevatorApp_Util_AsyncObservableCollection_1">*T*</a><br />The object to remove from the <a href="http://msdn2.microsoft.com/en-us/library/92t2ye13" target="_blank">ICollection(T)</a>.</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/a28wyd50" target="_blank">Boolean</a><br />`true` (`True` in Visual Basic) if *item* was successfully removed from the <a href="http://msdn2.microsoft.com/en-us/library/92t2ye13" target="_blank">ICollection(T)</a>; otherwise, `false` (`False` in Visual Basic). This method also returns `false` (`False` in Visual Basic) if *item* is not found in the original <a href="http://msdn2.microsoft.com/en-us/library/92t2ye13" target="_blank">ICollection(T)</a>.

#### Implements
<a href="http://msdn2.microsoft.com/en-us/library/bye7h94w" target="_blank">ICollection(T).Remove(T)</a><br />

## Exceptions
&nbsp;<table><tr><th>Exception</th><th>Condition</th></tr><tr><td><a href="http://msdn2.microsoft.com/en-us/library/8a7a4e64" target="_blank">NotSupportedException</a></td><td>The <a href="http://msdn2.microsoft.com/en-us/library/92t2ye13" target="_blank">ICollection(T)</a> is read-only.</td></tr></table>

## See Also


#### Reference
<a href="T_ElevatorApp_Util_AsyncObservableCollection_1">AsyncObservableCollection(T) Class</a><br /><a href="N_ElevatorApp_Util">ElevatorApp.Util Namespace</a><br />