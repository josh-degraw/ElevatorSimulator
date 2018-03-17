# ModelBase.DependentPropertyChanged Method 
 

Similar in function to <a href="M_ElevatorApp_Models_ModelBase_OnPropertyChanged__1">OnPropertyChanged(T)(T, String)</a>, but to use this you must explicitly state the property name, so this is useful for when the object depends on something but has no actual control over changing it, but still needs the change to be notified.

**Namespace:**&nbsp;<a href="N_ElevatorApp_Models">ElevatorApp.Models</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
protected void DependentPropertyChanged(
	string propertyName
)
```


#### Parameters
&nbsp;<dl><dt>propertyName</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br /></dd></dl>

## See Also


#### Reference
<a href="T_ElevatorApp_Models_ModelBase">ModelBase Class</a><br /><a href="N_ElevatorApp_Models">ElevatorApp.Models Namespace</a><br />