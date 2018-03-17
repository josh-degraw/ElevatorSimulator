# AsyncObservableCollection(*T*).AsyncDispatcherEvent(*TEvent*, *TArgs*) Class
 


Wrapper around an event so that any events added from a Dispatcher thread are invoked on that thread. This means that if the UI adds an event and that event is called on a different thread, the callback will be dispatched to the UI thread and called asynchronously. If an event is added from a non-dispatcher thread, or the event is raised from within the same thread as it was added from, it will be called normally.

Note that this means that the callback will be asynchronous and may happen at some time in the future rather than as soon as the event is raised.

This class is thread-safe.



## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;ElevatorApp.Util.AsyncObservableCollection(T).AsyncDispatcherEvent(TEvent, TArgs)<br />
**Namespace:**&nbsp;<a href="N_ElevatorApp_Util">ElevatorApp.Util</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public sealed class AsyncDispatcherEvent<TEvent, TArgs>
where TEvent : class
where TArgs : EventArgs

```


#### Type Parameters
&nbsp;<dl><dt>TEvent</dt><dd>The delagate type to wrap (ie PropertyChangedEventHandler). Must have a void delegate(object, TArgs) signature.</dd><dt>TArgs</dt><dd>Second argument of the TEvent. Must be of type EventArgs.</dd></dl>&nbsp;
The AsyncObservableCollection(T).AsyncDispatcherEvent(TEvent, TArgs) generic type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_AsyncDispatcherEvent_2__ctor">AsyncObservableCollection(T).AsyncDispatcherEvent(TEvent, TArgs)</a></td><td>
Initializes a new instance of the AsyncObservableCollection(T).AsyncDispatcherEvent(TEvent, TArgs) class</td></tr></table>&nbsp;
<a href="#asyncobservablecollection(*t*).asyncdispatcherevent(*tevent*,-*targs*)-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Util_AsyncObservableCollection_1_AsyncDispatcherEvent_2_isEmpty">isEmpty</a></td><td>
Checks if any delegate has been added to this event.</td></tr></table>&nbsp;
<a href="#asyncobservablecollection(*t*).asyncdispatcherevent(*tevent*,-*targs*)-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_AsyncDispatcherEvent_2_add">add</a></td><td>
Adds the delegate to the event.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bsc2ak47" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/zdee4b3y" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dfwy45w9" target="_blank">GetType</a></td><td>
Gets the <a href="http://msdn2.microsoft.com/en-us/library/42892f65" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_AsyncDispatcherEvent_2_raise">raise</a></td><td>
Calls the event.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_AsyncDispatcherEvent_2_remove">remove</a></td><td>
Removes the last instance of delegate from the event (if it exists). Only removes events that were added from the current dispatcher thread (if they were added from one), so make sure to remove from the same thread that added.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/7bxwbwt2" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr></table>&nbsp;
<a href="#asyncobservablecollection(*t*).asyncdispatcherevent(*tevent*,-*targs*)-class">Back to Top</a>

## Examples

```
private readonly AsyncDispatcherEvent{PropertyChangedEventHandler, PropertyChangedEventArgs} _propertyChanged = 
      new DispatcherEventHelper{PropertyChangedEventHandler, PropertyChangedEventArgs}();

   public event PropertyChangedEventHandler PropertyChanged
   {
       add { _propertyChanged.add(value); }
       remove { _propertyChanged.remove(value); }
   }

   private void OnPropertyChanged(PropertyChangedEventArgs args)
   {
       _propertyChanged.invoke(this, args);
   }
```


## See Also


#### Reference
<a href="N_ElevatorApp_Util">ElevatorApp.Util Namespace</a><br />