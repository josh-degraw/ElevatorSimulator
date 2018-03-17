# AsyncObservableCollection(*T*).CopyTo Method 
 

Copies the elements of the <a href="http://msdn2.microsoft.com/en-us/library/92t2ye13" target="_blank">ICollection(T)</a> to an <a href="http://msdn2.microsoft.com/en-us/library/czz5hkty" target="_blank">Array</a>, starting at a particular <a href="http://msdn2.microsoft.com/en-us/library/czz5hkty" target="_blank">Array</a> index.

**Namespace:**&nbsp;<a href="N_ElevatorApp_Util">ElevatorApp.Util</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public void CopyTo(
	T[] array,
	int arrayIndex
)
```


#### Parameters
&nbsp;<dl><dt>array</dt><dd>Type: <a href="T_ElevatorApp_Util_AsyncObservableCollection_1">*T*</a>[]<br />The one-dimensional <a href="http://msdn2.microsoft.com/en-us/library/czz5hkty" target="_blank">Array</a> that is the destination of the elements copied from <a href="http://msdn2.microsoft.com/en-us/library/92t2ye13" target="_blank">ICollection(T)</a>. The <a href="http://msdn2.microsoft.com/en-us/library/czz5hkty" target="_blank">Array</a> must have zero-based indexing.</dd><dt>arrayIndex</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">System.Int32</a><br />The zero-based index in *array* at which copying begins.</dd></dl>

#### Implements
<a href="http://msdn2.microsoft.com/en-us/library/0efx51xw" target="_blank">ICollection(T).CopyTo(T[], Int32)</a><br />

## Exceptions
&nbsp;<table><tr><th>Exception</th><th>Condition</th></tr><tr><td><a href="http://msdn2.microsoft.com/en-us/library/27426hcy" target="_blank">ArgumentNullException</a></td><td>*array* is a null reference (`Nothing` in Visual Basic).</td></tr><tr><td><a href="http://msdn2.microsoft.com/en-us/library/8xt94y6e" target="_blank">ArgumentOutOfRangeException</a></td><td>*arrayIndex* is less than 0.</td></tr><tr><td><a href="http://msdn2.microsoft.com/en-us/library/3w1b3114" target="_blank">ArgumentException</a></td><td>The number of elements in the source <a href="http://msdn2.microsoft.com/en-us/library/92t2ye13" target="_blank">ICollection(T)</a> is greater than the available space from *arrayIndex* to the end of the destination *array*.</td></tr></table>

## See Also


#### Reference
<a href="T_ElevatorApp_Util_AsyncObservableCollection_1">AsyncObservableCollection(T) Class</a><br /><a href="N_ElevatorApp_Util">ElevatorApp.Util Namespace</a><br />