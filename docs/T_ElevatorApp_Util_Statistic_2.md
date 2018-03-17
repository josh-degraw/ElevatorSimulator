# Statistic(*TStat*, *TAggregate*) Class
 

Represents a statistical computation. As new items are added, the new statistical values are calculated and updated, so that read operations yield no real cost


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="T_ElevatorApp_Models_ModelBase">ElevatorApp.Models.ModelBase</a><br />&nbsp;&nbsp;&nbsp;&nbsp;ElevatorApp.Util.Statistic(TStat, TAggregate)<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_ElevatorApp_Util_SimpleStatistic_1">ElevatorApp.Util.SimpleStatistic(T)</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_ElevatorApp_Util_Statistics_IntStatistic">ElevatorApp.Util.Statistics.IntStatistic</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_ElevatorApp_Util_Statistics_LongStatistic">ElevatorApp.Util.Statistics.LongStatistic</a><br />
**Namespace:**&nbsp;<a href="N_ElevatorApp_Util">ElevatorApp.Util</a><br />**Assembly:**&nbsp;ElevatorApp (in ElevatorApp.exe) Version: 4.0.0.22673 (4.0.0.0)

## Syntax

**C#**<br />
``` C#
public abstract class Statistic<TStat, TAggregate> : ModelBase, 
	IStatistic
where TStat : struct, new(), IComparable<TStat>
where TAggregate : struct, new(), IComparable<TAggregate>

```


#### Type Parameters
&nbsp;<dl><dt>TStat</dt><dd>The type of statistic that is being calculated</dd><dt>TAggregate</dt><dd>The type that is used for aggregate values e.g. the average</dd></dl>&nbsp;
The Statistic(TStat, TAggregate) type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Util_Statistic_2__ctor">Statistic(TStat, TAggregate)()</a></td><td>
Constructs a new Statistic(TStat, TAggregate) object</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Util_Statistic_2__ctor_1">Statistic(TStat, TAggregate)(IEnumerable(TStat))</a></td><td>
Constructs a new Statistic(TStat, TAggregate) object, with the given collection as the initial values</td></tr></table>&nbsp;
<a href="#statistic(*tstat*,-*taggregate*)-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected property](media/protproperty.gif "Protected property")</td><td><a href="P_ElevatorApp_Util_Statistic_2__collection">_collection</a></td><td>
The collection of statistics, made available in a read-only fashion</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Util_Statistic_2_Average">Average</a></td><td>
The average of all values</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Util_Statistic_2_Count">Count</a></td><td>
The number of values encountered</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Util_Statistic_2_Max">Max</a></td><td>
The maximum value encountered</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_ElevatorApp_Util_Statistic_2_Min">Min</a></td><td>
The minimum value encountered</td></tr></table>&nbsp;
<a href="#statistic(*tstat*,-*taggregate*)-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_Statistic_2_Add">Add</a></td><td>
Adds an item to the collection and calculates the new <a href="P_ElevatorApp_Util_Statistic_2_Min">Min</a>, <a href="P_ElevatorApp_Util_Statistic_2_Max">Max</a>, and <a href="P_ElevatorApp_Util_Statistic_2_Average">Average</a> values</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Util_Statistic_2_AddItems">AddItems</a></td><td>
Provides a function to add two items of type *TStat* together 
Represents `*left* + *right*`</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_Statistic_2_AddRange">AddRange</a></td><td>
Adds a range of items to the collection of statistics, updating the values as they are entered</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Util_Statistic_2_CalculateAverage">CalculateAverage</a></td><td>
A function that will get the average of all statistics held by this object</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ModelBase_DependentPropertyChanged">DependentPropertyChanged</a></td><td>
Similar in function to <a href="M_ElevatorApp_Models_ModelBase_OnPropertyChanged__1">OnPropertyChanged(T)(T, String)</a>, but to use this you must explicitly state the property name, so this is useful for when the object depends on something but has no actual control over changing it, but still needs the change to be notified.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Util_Statistic_2_DivideItems">DivideItems</a></td><td>
Provides a function to divide one items of type *TStat* from another 
Represents `*numerator* / *denominator*`</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bsc2ak47" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/4k87zsw7" target="_blank">Finalize</a></td><td>
Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/zdee4b3y" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dfwy45w9" target="_blank">GetType</a></td><td>
Gets the <a href="http://msdn2.microsoft.com/en-us/library/42892f65" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/57ctke0a" target="_blank">MemberwiseClone</a></td><td>
Creates a shallow copy of the current <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Util_Statistic_2_MultiplyItems">MultiplyItems</a></td><td>
Provides a function to multiply two items of type *TStat* together 
Represents `*left* * *right*`</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ModelBase_OnPropertyChanged__1">OnPropertyChanged(T)</a></td><td>
Use in situations where you can't directly set the value, but still need to trigger the <a href="E_ElevatorApp_Models_ModelBase_PropertyChanged">PropertyChanged</a> event.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ModelBase_SetProperty__1">SetProperty(T)</a></td><td>
Sets the given backing property to the new value (if it has changed), and invokes <a href="E_ElevatorApp_Models_ModelBase_PropertyChanged">PropertyChanged</a>.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Util_Statistic_2_SubtractItems">SubtractItems</a></td><td>
Provides a function to subtract one items of type *TStat* from another 
Represents `*left* - *right*`</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_Statistic_2_ToString">ToString</a></td><td>
Returns a string that represents the current object.
 (Overrides <a href="http://msdn2.microsoft.com/en-us/library/7bxwbwt2" target="_blank">Object.ToString()</a>.)</td></tr></table>&nbsp;
<a href="#statistic(*tstat*,-*taggregate*)-class">Back to Top</a>

## Events
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_ElevatorApp_Models_ModelBase_PropertyChanged">PropertyChanged</a></td><td>
Occurs when a property value changes.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr></table>&nbsp;
<a href="#statistic(*tstat*,-*taggregate*)-class">Back to Top</a>

## See Also


#### Reference
<a href="N_ElevatorApp_Util">ElevatorApp.Util Namespace</a><br />