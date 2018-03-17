# AmbiguousTimeException Class
 

\[Missing <summary> documentation for "T:NodaTime.AmbiguousTimeException"\]


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="http://msdn2.microsoft.com/en-us/library/c18k6c59" target="_blank">System.Exception</a><br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="http://msdn2.microsoft.com/en-us/library/z3h75xk6" target="_blank">System.SystemException</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="http://msdn2.microsoft.com/en-us/library/3w1b3114" target="_blank">System.ArgumentException</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="http://msdn2.microsoft.com/en-us/library/8xt94y6e" target="_blank">System.ArgumentOutOfRangeException</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;NodaTime.AmbiguousTimeException<br />
**Namespace:**&nbsp;<a href="N_NodaTime">NodaTime</a><br />**Assembly:**&nbsp;NodaTime (in NodaTime.dll) Version: 2.2.4

## Syntax

**C#**<br />
``` C#
[SerializableAttribute]
public sealed class AmbiguousTimeException : ArgumentOutOfRangeException
```

The AmbiguousTimeException type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_NodaTime_AmbiguousTimeException__ctor">AmbiguousTimeException</a></td><td>
Initializes a new instance of the AmbiguousTimeException class</td></tr></table>&nbsp;
<a href="#ambiguoustimeexception-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/wakz300y" target="_blank">ActualValue</a></td><td>
Gets the argument value that causes this exception.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/8xt94y6e" target="_blank">ArgumentOutOfRangeException</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/2wyfbc48" target="_blank">Data</a></td><td>
Gets a collection of key/value pairs that provide additional user-defined information about the exception.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/c18k6c59" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_NodaTime_AmbiguousTimeException_EarlierMapping">EarlierMapping</a></td><td /></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/71tawy4s" target="_blank">HelpLink</a></td><td>
Gets or sets a link to the help file associated with this exception.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/c18k6c59" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/sh5cw61c" target="_blank">HResult</a></td><td>
Gets or sets HRESULT, a coded numerical value that is assigned to a specific exception.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/c18k6c59" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/902sca80" target="_blank">InnerException</a></td><td>
Gets the <a href="http://msdn2.microsoft.com/en-us/library/c18k6c59" target="_blank">Exception</a> instance that caused the current exception.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/c18k6c59" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_NodaTime_AmbiguousTimeException_LaterMapping">LaterMapping</a></td><td /></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/w1tz50dd" target="_blank">Message</a></td><td>
Gets the error message and the string representation of the invalid argument value, or only the error message if the argument value is null.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/8xt94y6e" target="_blank">ArgumentOutOfRangeException</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/69xy8t8x" target="_blank">ParamName</a></td><td>
Gets the name of the parameter that causes this exception.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/3w1b3114" target="_blank">ArgumentException</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/85weac5w" target="_blank">Source</a></td><td>
Gets or sets the name of the application or the object that causes the error.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/c18k6c59" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dxzhy005" target="_blank">StackTrace</a></td><td>
Gets a string representation of the immediate frames on the call stack.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/c18k6c59" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/2wchw354" target="_blank">TargetSite</a></td><td>
Gets the method that throws the current exception.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/c18k6c59" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_NodaTime_AmbiguousTimeException_Zone">Zone</a></td><td /></tr></table>&nbsp;
<a href="#ambiguoustimeexception-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bsc2ak47" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/49kcee3b" target="_blank">GetBaseException</a></td><td>
When overridden in a derived class, returns the <a href="http://msdn2.microsoft.com/en-us/library/c18k6c59" target="_blank">Exception</a> that is the root cause of one or more subsequent exceptions.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/c18k6c59" target="_blank">Exception</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/zdee4b3y" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/azeyz2y0" target="_blank">GetObjectData</a></td><td>
Sets the <a href="http://msdn2.microsoft.com/en-us/library/a9b6042e" target="_blank">SerializationInfo</a> object with the invalid argument value and additional exception information.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/8xt94y6e" target="_blank">ArgumentOutOfRangeException</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/44zb316t" target="_blank">GetType</a></td><td>
Gets the runtime type of the current instance.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/c18k6c59" target="_blank">Exception</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/es4y6f7e" target="_blank">ToString</a></td><td>
Creates and returns a string representation of the current exception.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/c18k6c59" target="_blank">Exception</a>.)</td></tr></table>&nbsp;
<a href="#ambiguoustimeexception-class">Back to Top</a>

## See Also


#### Reference
<a href="N_NodaTime">NodaTime Namespace</a><br />