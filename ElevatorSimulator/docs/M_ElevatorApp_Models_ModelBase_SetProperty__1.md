# ModelBase.SetProperty(*T*) Method 
 

Sets the given backing property to the new value (if it has changed), and invokes <a href="E_ElevatorApp_Models_ModelBase_PropertyChanged">PropertyChanged</a>.

**Namespace:**&nbsp;<a href="N_ElevatorApp_Models">ElevatorApp.Models</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
protected virtual bool SetProperty<T>(
	ref T prop,
	T value,
	bool alwaysLog = false,
	[CallerMemberNameAttribute] string propertyName = null
)

```


#### Parameters
&nbsp;<dl><dt>prop</dt><dd>Type: *T*<br />A reference to the backing field that is to be changed.</dd><dt>value</dt><dd>Type: *T*<br />The new value that this will be set to.</dd><dt>alwaysLog (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/a28wyd50" target="_blank">System.Boolean</a><br />Specifies whether or not to ignore the global configuration option LogAllPropertyChanges</dd><dt>propertyName (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />The name of the property that is being changed. This is handled by the <a href="http://msdn2.microsoft.com/en-us/library/hh551816" target="_blank">CallerMemberNameAttribute</a> applied to this parameter, so it should never be set directly</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>The type of property being changed</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/a28wyd50" target="_blank">Boolean</a><br />True if the *value* is different from *prop*, and therefore is being changed

## See Also


#### Reference
<a href="T_ElevatorApp_Models_ModelBase">ModelBase Class</a><br /><a href="N_ElevatorApp_Models">ElevatorApp.Models Namespace</a><br />