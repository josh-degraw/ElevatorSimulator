# ElevatorApp.Util Namespace
 

This namespace holds helper classes, including logging, collections, delegates, and anything else that didn't really fit into any of the other namespaces.


## Classes
&nbsp;<table><tr><th></th><th>Class</th><th>Description</th></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Util_AsyncObservableCollection_1">AsyncObservableCollection(T)</a></td><td>

A version of <a href="http://msdn2.microsoft.com/en-us/library/ms668604" target="_blank">ObservableCollection(T)</a> that is locked so that it can be accessed by multiple threads.

When you enumerate it (foreach), you will get a snapshot of the current contents. Also the <a href="http://msdn2.microsoft.com/en-us/library/ms653382" target="_blank">CollectionChanged</a> event will be called on the thread that added it if that thread is a Dispatcher (WPF/Silverlight/WinRT) thread. This means that you can update this from any thread and recieve notifications of those updates on the UI thread.

You can't modify the collection during a callback (on the thread that recieved the callback -- other threads can do whatever they want). This is the
 same as <a href="http://msdn2.microsoft.com/en-us/library/ms668604" target="_blank">ObservableCollection(T)</a>.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")![Code example](media/CodeExample.png "Code example")</td><td><a href="T_ElevatorApp_Util_AsyncObservableCollection_1_AsyncDispatcherEvent_2">AsyncObservableCollection(T).AsyncDispatcherEvent(TEvent, TArgs)</a></td><td>

Wrapper around an event so that any events added from a Dispatcher thread are invoked on that thread. This means that if the UI adds an event and that event is called on a different thread, the callback will be dispatched to the UI thread and called asynchronously. If an event is added from a non-dispatcher thread, or the event is raised from within the same thread as it was added from, it will be called normally.

Note that this means that the callback will be asynchronous and may happen at some time in the future rather than as soon as the event is raised.

This class is thread-safe.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Util_Extensions">Extensions</a></td><td>
Holds helper functions</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Util_LocalSystemClock">LocalSystemClock</a></td><td>
Class copied from NodaTime <a href="T_NodaTime_SystemClock">SystemClock</a>, to show the local system time instead of UTC</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Util_Logger">Logger</a></td><td>
Singleton object used to log events. Can be accessed statically or via <a href="P_ElevatorApp_Util_Logger_Instance">Instance</a>.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Util_ObservableConcurrentQueue_1">ObservableConcurrentQueue(T)</a></td><td> **Obsolete. **</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Util_SimpleStatistic_1">SimpleStatistic(T)</a></td><td>
Represents a statistical computation which aggregates to the same type. As new items are added, the new statistical values are calculated and updated, so that read operations yield no real cost</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_ElevatorApp_Util_Statistic_2">Statistic(TStat, TAggregate)</a></td><td>
Represents a statistical computation. As new items are added, the new statistical values are calculated and updated, so that read operations yield no real cost</td></tr></table>

## Structures
&nbsp;<table><tr><th></th><th>Structure</th><th>Description</th></tr><tr><td>![Public structure](media/pubstructure.gif "Public structure")</td><td><a href="T_ElevatorApp_Util_Event">Event</a></td><td>
Represents an item logged by <a href="T_ElevatorApp_Util_Logger">Logger</a>. Holds a name, timestamp, and optional descriptive parameters.</td></tr></table>

## Interfaces
&nbsp;<table><tr><th></th><th>Interface</th><th>Description</th></tr><tr><td>![Public interface](media/pubinterface.gif "Public interface")</td><td><a href="T_ElevatorApp_Util_ILogger">ILogger</a></td><td /></tr><tr><td>![Public interface](media/pubinterface.gif "Public interface")</td><td><a href="T_ElevatorApp_Util_IStatistic">IStatistic</a></td><td>
The base interface for statistical calculations</td></tr></table>

## Delegates
&nbsp;<table><tr><th></th><th>Delegate</th><th>Description</th></tr><tr><td>![Public delegate](media/pubdelegate.gif "Public delegate")</td><td><a href="T_ElevatorApp_Util_Logger_LogEventHandler">Logger.LogEventHandler</a></td><td>
Handle</td></tr></table>&nbsp;
