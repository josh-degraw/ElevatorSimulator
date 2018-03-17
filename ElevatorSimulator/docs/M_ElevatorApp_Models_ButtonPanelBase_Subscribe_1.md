# ButtonPanelBase.Subscribe Method (IObserver(Int32))
 

Subscribes this <a href="T_ElevatorApp_Models_ButtonPanelBase">ButtonPanelBase</a> to an <a href="http://msdn2.microsoft.com/en-us/library/dd990377" target="_blank">IObservable(T)</a>, in order for the subscriber to be notified whenever any of the <a href="T_ElevatorApp_Models_FloorButton">FloorButton</a>s on this <a href="T_ElevatorApp_Models_ButtonPanelBase">ButtonPanelBase</a> have been activated.

**Namespace:**&nbsp;<a href="N_ElevatorApp_Models">ElevatorApp.Models</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public IDisposable Subscribe(
	IObserver<int> observer
)
```


#### Parameters
&nbsp;<dl><dt>observer</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/dd783449" target="_blank">System.IObserver</a>(<a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">Int32</a>)<br /></dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/aax125c9" target="_blank">IDisposable</a><br />An Unsubscriber that can be disposed of to stop receiving updates

#### Implements
<a href="http://msdn2.microsoft.com/en-us/library/dd782981" target="_blank">IObservable(T).Subscribe(IObserver(T))</a><br />

## See Also


#### Reference
<a href="T_ElevatorApp_Models_ButtonPanelBase">ButtonPanelBase Class</a><br /><a href="Overload_ElevatorApp_Models_ButtonPanelBase_Subscribe">Subscribe Overload</a><br /><a href="N_ElevatorApp_Models">ElevatorApp.Models Namespace</a><br />