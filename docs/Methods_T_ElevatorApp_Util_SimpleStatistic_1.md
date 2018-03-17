# SimpleStatistic(*T*) Methods
 

The <a href="T_ElevatorApp_Util_SimpleStatistic_1">SimpleStatistic(T)</a> generic type exposes the following members.


## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_Statistic_2_Add">Add</a></td><td>
Adds an item to the collection and calculates the new <a href="P_ElevatorApp_Util_Statistic_2_Min">Min</a>, <a href="P_ElevatorApp_Util_Statistic_2_Max">Max</a>, and <a href="P_ElevatorApp_Util_Statistic_2_Average">Average</a> values
 (Inherited from <a href="T_ElevatorApp_Util_Statistic_2">Statistic(TStat, TAggregate)</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Util_Statistic_2_AddItems">AddItems</a></td><td>
Provides a function to add two items of type *TStat* together 
Represents `*left* + *right*`

 (Inherited from <a href="T_ElevatorApp_Util_Statistic_2">Statistic(TStat, TAggregate)</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_Statistic_2_AddRange">AddRange</a></td><td>
Adds a range of items to the collection of statistics, updating the values as they are entered
 (Inherited from <a href="T_ElevatorApp_Util_Statistic_2">Statistic(TStat, TAggregate)</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Util_Statistic_2_CalculateAverage">CalculateAverage</a></td><td>
A function that will get the average of all statistics held by this object
 (Inherited from <a href="T_ElevatorApp_Util_Statistic_2">Statistic(TStat, TAggregate)</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ModelBase_DependentPropertyChanged">DependentPropertyChanged</a></td><td>
Similar in function to <a href="M_ElevatorApp_Models_ModelBase_OnPropertyChanged__1">OnPropertyChanged(T)(T, String)</a>, but to use this you must explicitly state the property name, so this is useful for when the object depends on something but has no actual control over changing it, but still needs the change to be notified.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Util_Statistic_2_DivideItems">DivideItems</a></td><td>
Provides a function to divide one items of type *TStat* from another 
Represents `*numerator* / *denominator*`

 (Inherited from <a href="T_ElevatorApp_Util_Statistic_2">Statistic(TStat, TAggregate)</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bsc2ak47" target="_blank">Equals</a></td><td>
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
Represents `*left* * *right*`

 (Inherited from <a href="T_ElevatorApp_Util_Statistic_2">Statistic(TStat, TAggregate)</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ModelBase_OnPropertyChanged__1">OnPropertyChanged(T)</a></td><td>
Use in situations where you can't directly set the value, but still need to trigger the <a href="E_ElevatorApp_Models_ModelBase_PropertyChanged">PropertyChanged</a> event.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Models_ModelBase_SetProperty__1">SetProperty(T)</a></td><td>
Sets the given backing property to the new value (if it has changed), and invokes <a href="E_ElevatorApp_Models_ModelBase_PropertyChanged">PropertyChanged</a>.
 (Inherited from <a href="T_ElevatorApp_Models_ModelBase">ModelBase</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_ElevatorApp_Util_Statistic_2_SubtractItems">SubtractItems</a></td><td>
Provides a function to subtract one items of type *TStat* from another 
Represents `*left* - *right*`

 (Inherited from <a href="T_ElevatorApp_Util_Statistic_2">Statistic(TStat, TAggregate)</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_ElevatorApp_Util_Statistic_2_ToString">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="T_ElevatorApp_Util_Statistic_2">Statistic(TStat, TAggregate)</a>.)</td></tr></table>&nbsp;
<a href="#simplestatistic(*t*)-methods">Back to Top</a>

## See Also


#### Reference
<a href="T_ElevatorApp_Util_SimpleStatistic_1">SimpleStatistic(T) Class</a><br /><a href="N_ElevatorApp_Util">ElevatorApp.Util Namespace</a><br />