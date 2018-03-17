# ObservableConcurrentQueue(*T*) Class
 

**Note: This API is now obsolete.**

\[Missing <summary> documentation for "T:ElevatorApp.Util.ObservableConcurrentQueue`1"\]


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;ElevatorApp.Util.ObservableConcurrentQueue(T)<br />
**Namespace:**&nbsp;<a href="N_ElevatorApp_Util">ElevatorApp.Util</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
[ObsoleteAttribute("Use AsyncObservableCollection instead.")]
public class ObservableConcurrentQueue<T> : ICollection<T>, 
	IEnumerable<T>, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged, IReadOnlyCollection<T>

```


#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>\[Missing <typeparam name="T"/> documentation for "T:ElevatorApp.Util.ObservableConcurrentQueue`1"\]</dd></dl>&nbsp;
The ObservableConcurrentQueue(T) type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_ObservableConcurrentQueue_1__ctor">ObservableConcurrentQueue(T)()</a></td><td>
Initializes a new instance of the ObservableConcurrentQueue(T) class</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_ObservableConcurrentQueue_1__ctor_1">ObservableConcurrentQueue(T)(IEnumerable(T))</a></td><td>
Initializes a new instance of the ObservableConcurrentQueue(T) class</td></tr></table>&nbsp;
<a href="#observableconcurrentqueue(*t*)-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Util_ObservableConcurrentQueue_1_Count">Count</a></td><td /></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Util_ObservableConcurrentQueue_1_IsReadOnly">IsReadOnly</a></td><td /></tr></table>&nbsp;
<a href="#observableconcurrentqueue(*t*)-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_ObservableConcurrentQueue_1_Enqueue">Enqueue</a></td><td /></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bsc2ak47" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/4k87zsw7" target="_blank">Finalize</a></td><td>
Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_ObservableConcurrentQueue_1_GetEnumerator">GetEnumerator</a></td><td /></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/zdee4b3y" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dfwy45w9" target="_blank">GetType</a></td><td>
Gets the <a href="http://msdn2.microsoft.com/en-us/library/42892f65" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/57ctke0a" target="_blank">MemberwiseClone</a></td><td>
Creates a shallow copy of the current <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/7bxwbwt2" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_ObservableConcurrentQueue_1_TryDequeue">TryDequeue</a></td><td /></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_ObservableConcurrentQueue_1_TryPeek">TryPeek</a></td><td /></tr></table>&nbsp;
<a href="#observableconcurrentqueue(*t*)-class">Back to Top</a>

## Events
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Util_ObservableConcurrentQueue_1_CollectionChanged">CollectionChanged</a></td><td /></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Util_ObservableConcurrentQueue_1_PropertyChanged">PropertyChanged</a></td><td /></tr></table>&nbsp;
<a href="#observableconcurrentqueue(*t*)-class">Back to Top</a>

## Extension Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public Extension Method](media/pubextension.gif "Public Extension Method")</td><td><a href="M_ElevatorApp_Util_Extensions_MinByOrDefault__2">MinByOrDefault(T, TKey)</a></td><td> (Defined by <a href="T_ElevatorApp_Util_Extensions">Extensions</a>.)</td></tr></table>&nbsp;
<a href="#observableconcurrentqueue(*t*)-class">Back to Top</a>

## See Also


#### Reference
<a href="N_ElevatorApp_Util">ElevatorApp.Util Namespace</a><br />