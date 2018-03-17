# ModelBase.OnPropertyChanged(*T*) Method 
 

Use in situations where you can't directly set the value, but still need to trigger the <a href="E_ElevatorApp_Models_ModelBase_PropertyChanged">PropertyChanged</a> event.

**Namespace:**&nbsp;<a href="N_ElevatorApp_Models">ElevatorApp.Models</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
protected void OnPropertyChanged<T>(
	T newValue,
	[CallerMemberNameAttribute] string propertyName = null
)

```


#### Parameters
&nbsp;<dl><dt>newValue</dt><dd>Type: *T*<br /></dd><dt>propertyName (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br /></dd></dl>

#### Type Parameters
&nbsp;<dl><dt>T</dt><dd /></dl>

## See Also


#### Reference
<a href="T_ElevatorApp_Models_ModelBase">ModelBase Class</a><br /><a href="N_ElevatorApp_Models">ElevatorApp.Models Namespace</a><br />