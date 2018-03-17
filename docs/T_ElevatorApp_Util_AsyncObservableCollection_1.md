# AsyncObservableCollection(*T*) Class
 


A version of <a href="http://msdn2.microsoft.com/en-us/library/ms668604" target="_blank">ObservableCollection(T)</a> that is locked so that it can be accessed by multiple threads.

When you enumerate it (foreach), you will get a snapshot of the current contents. Also the <a href="http://msdn2.microsoft.com/en-us/library/ms653382" target="_blank">CollectionChanged</a> event will be called on the thread that added it if that thread is a Dispatcher (WPF/Silverlight/WinRT) thread. This means that you can update this from any thread and recieve notifications of those updates on the UI thread.

You can't modify the collection during a callback (on the thread that recieved the callback -- other threads can do whatever they want). This is the
 same as <a href="http://msdn2.microsoft.com/en-us/library/ms668604" target="_blank">ObservableCollection(T)</a>.


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;ElevatorApp.Util.AsyncObservableCollection(T)<br />
**Namespace:**&nbsp;<a href="N_ElevatorApp_Util">ElevatorApp.Util</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
[SerializableAttribute]
public sealed class AsyncObservableCollection<T> : IList<T>, 
	ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyList<T>, IReadOnlyCollection<T>, 
	IList, ICollection, INotifyCollectionChanged, INotifyPropertyChanged, ISerializable

```


#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>The type of item in the collections</dd></dl>&nbsp;
The AsyncObservableCollection(T) type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1__ctor">AsyncObservableCollection(T)()</a></td><td>
Initializes a new instance of the <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a> class.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1__ctor_1">AsyncObservableCollection(T)(IEnumerable(T))</a></td><td>
Initializes a new instance of the AsyncObservableCollection(T) class</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1__ctor_2">AsyncObservableCollection(T)(SerializationInfo, StreamingContext)</a></td><td>
Constructor is only here for serialization, you should use the default constructor instead.</td></tr></table>&nbsp;
<a href="#asyncobservablecollection(*t*)-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Util_AsyncObservableCollection_1_Count">Count</a></td><td>
Gets the number of elements contained in the <a href="http://msdn2.microsoft.com/en-us/library/92t2ye13" target="_blank">ICollection(T)</a>.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Util_AsyncObservableCollection_1_Item">Item</a></td><td>
Gets or sets the element at the specified index.</td></tr></table>&nbsp;
<a href="#asyncobservablecollection(*t*)-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_Add">Add</a></td><td>
Adds an item to the <a href="http://msdn2.microsoft.com/en-us/library/92t2ye13" target="_blank">ICollection(T)</a>.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_AddDistinct">AddDistinct</a></td><td>
Adds an item if it does not already exist in the collection</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_AddRange">AddRange</a></td><td /></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_Clear">Clear</a></td><td>
Removes all items from the <a href="http://msdn2.microsoft.com/en-us/library/92t2ye13" target="_blank">ICollection(T)</a>.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_Contains">Contains</a></td><td>
Determines whether the <a href="http://msdn2.microsoft.com/en-us/library/92t2ye13" target="_blank">ICollection(T)</a> contains a specific value.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_CopyTo">CopyTo</a></td><td>
Copies the elements of the <a href="http://msdn2.microsoft.com/en-us/library/92t2ye13" target="_blank">ICollection(T)</a> to an <a href="http://msdn2.microsoft.com/en-us/library/czz5hkty" target="_blank">Array</a>, starting at a particular <a href="http://msdn2.microsoft.com/en-us/library/czz5hkty" target="_blank">Array</a> index.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_Enqueue">Enqueue</a></td><td>
Adds an item to the collection, mimicking the behavior of <a href="http://msdn2.microsoft.com/en-us/library/dd267265" target="_blank">ConcurrentQueue(T)</a></td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bsc2ak47" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_GetEnumerator">GetEnumerator</a></td><td>
Returns an enumerator that iterates through the collection.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/zdee4b3y" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dfwy45w9" target="_blank">GetType</a></td><td>
Gets the <a href="http://msdn2.microsoft.com/en-us/library/42892f65" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_IndexOf">IndexOf</a></td><td>
Determines the index of a specific item in the <a href="http://msdn2.microsoft.com/en-us/library/5y536ey6" target="_blank">IList(T)</a>.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_Insert">Insert</a></td><td>
Inserts an item to the <a href="http://msdn2.microsoft.com/en-us/library/5y536ey6" target="_blank">IList(T)</a> at the specified index.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_Move">Move</a></td><td /></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_Remove">Remove</a></td><td>
Removes the first occurrence of a specific object from the <a href="http://msdn2.microsoft.com/en-us/library/92t2ye13" target="_blank">ICollection(T)</a>.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_RemoveAt">RemoveAt</a></td><td>
Removes the <a href="http://msdn2.microsoft.com/en-us/library/5y536ey6" target="_blank">IList(T)</a> item at the specified index.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_ToArray">ToArray</a></td><td /></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/7bxwbwt2" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_TryDequeue">TryDequeue</a></td><td>
Tries to remove the first item from the collection, mimicking the behavior of <a href="http://msdn2.microsoft.com/en-us/library/dd267265" target="_blank">ConcurrentQueue(T)</a></td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_TryPeek">TryPeek</a></td><td>
Tries to read the first item in the collection</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_AsyncObservableCollection_1_TryRemove">TryRemove</a></td><td>
Tries to remove the given item from the collection</td></tr></table>&nbsp;
<a href="#asyncobservablecollection(*t*)-class">Back to Top</a>

## Events
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Util_AsyncObservableCollection_1_CollectionChanged">CollectionChanged</a></td><td>
Occurs when the collection changes.</td></tr></table>&nbsp;
<a href="#asyncobservablecollection(*t*)-class">Back to Top</a>

## Extension Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public Extension Method](media/pubextension.gif "Public Extension Method")</td><td><a href="M_ElevatorApp_Util_Extensions_MinByOrDefault__2">MinByOrDefault(T, TKey)</a></td><td> (Defined by <a href="T_ElevatorApp_Util_Extensions">Extensions</a>.)</td></tr></table>&nbsp;
<a href="#asyncobservablecollection(*t*)-class">Back to Top</a>

## Remarks
All credit for this code, unless explicitly declared otherwise, is from source: https://pastebin.com/hKQi6EHD

## See Also


#### Reference
<a href="N_ElevatorApp_Util">ElevatorApp.Util Namespace</a><br />