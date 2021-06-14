using System;

namespace System.Management
{
	/// <summary>Describes the enumeration of all WMI error codes that are currently defined. </summary>
	// Token: 0x02000019 RID: 25
	public enum ManagementStatus
	{
		/// <summary>The operation was successful. </summary>
		// Token: 0x04000098 RID: 152
		NoError,
		/// <summary>This value is returned when no more objects are available, the number of objects returned is less than the number requested, or at the end of an enumeration. It is also returned when the method is called with a value of 0 for the parameter.</summary>
		// Token: 0x04000099 RID: 153
		False,
		/// <summary>An overridden property was deleted. This value is returned to signal that the original, non-overridden value has been restored as a result of the deletion.</summary>
		// Token: 0x0400009A RID: 154
		ResetToDefault = 262146,
		/// <summary>The compared items (such as objects and classes) are not identical.</summary>
		// Token: 0x0400009B RID: 155
		Different,
		/// <summary>A call timed out. This is not an error condition; therefore, some results may have been returned.</summary>
		// Token: 0x0400009C RID: 156
		Timedout,
		/// <summary>No more data is available from the enumeration; the user should terminate the enumeration. </summary>
		// Token: 0x0400009D RID: 157
		NoMoreData,
		/// <summary>The operation was canceled.</summary>
		// Token: 0x0400009E RID: 158
		OperationCanceled,
		/// <summary>A request is still in progress; however, the results are not yet available.</summary>
		// Token: 0x0400009F RID: 159
		Pending,
		/// <summary>More than one copy of the same object was detected in the result set of an enumeration. </summary>
		// Token: 0x040000A0 RID: 160
		DuplicateObjects,
		/// <summary>The user did not receive all of the requested objects because of inaccessible resources (other than security violations).</summary>
		// Token: 0x040000A1 RID: 161
		PartialResults = 262160,
		/// <summary>The call failed.</summary>
		// Token: 0x040000A2 RID: 162
		Failed = -2147217407,
		/// <summary>The object could not be found. </summary>
		// Token: 0x040000A3 RID: 163
		NotFound,
		/// <summary>The current user does not have permission to perform the action. </summary>
		// Token: 0x040000A4 RID: 164
		AccessDenied,
		/// <summary>The provider failed after initialization. </summary>
		// Token: 0x040000A5 RID: 165
		ProviderFailure,
		/// <summary>A type mismatch occurred. </summary>
		// Token: 0x040000A6 RID: 166
		TypeMismatch,
		/// <summary>There was not enough memory for the operation. </summary>
		// Token: 0x040000A7 RID: 167
		OutOfMemory,
		/// <summary>The context object is not valid.</summary>
		// Token: 0x040000A8 RID: 168
		InvalidContext,
		/// <summary>One of the parameters to the call is not correct. </summary>
		// Token: 0x040000A9 RID: 169
		InvalidParameter,
		/// <summary>The resource, typically a remote server, is not currently available. </summary>
		// Token: 0x040000AA RID: 170
		NotAvailable,
		/// <summary>An internal, critical, and unexpected error occurred. Report this error to Microsoft Technical Support.</summary>
		// Token: 0x040000AB RID: 171
		CriticalError,
		/// <summary>One or more network packets were corrupted during a remote session.</summary>
		// Token: 0x040000AC RID: 172
		InvalidStream,
		/// <summary>The feature or operation is not supported. </summary>
		// Token: 0x040000AD RID: 173
		NotSupported,
		/// <summary>The specified superclass is not valid. </summary>
		// Token: 0x040000AE RID: 174
		InvalidSuperclass,
		/// <summary>The specified namespace could not be found. </summary>
		// Token: 0x040000AF RID: 175
		InvalidNamespace,
		/// <summary>The specified instance is not valid. </summary>
		// Token: 0x040000B0 RID: 176
		InvalidObject,
		/// <summary>The specified class is not valid. </summary>
		// Token: 0x040000B1 RID: 177
		InvalidClass,
		/// <summary>A provider referenced in the schema does not have a corresponding registration. </summary>
		// Token: 0x040000B2 RID: 178
		ProviderNotFound,
		/// <summary>A provider referenced in the schema has an incorrect or incomplete registration. </summary>
		// Token: 0x040000B3 RID: 179
		InvalidProviderRegistration,
		/// <summary>COM cannot locate a provider referenced in the schema. </summary>
		// Token: 0x040000B4 RID: 180
		ProviderLoadFailure,
		/// <summary>A component, such as a provider, failed to initialize for internal reasons. </summary>
		// Token: 0x040000B5 RID: 181
		InitializationFailure,
		/// <summary> A networking error that prevents normal operation has occurred. </summary>
		// Token: 0x040000B6 RID: 182
		TransportFailure,
		/// <summary>The requested operation is not valid. This error usually applies to invalid attempts to delete classes or properties. </summary>
		// Token: 0x040000B7 RID: 183
		InvalidOperation,
		/// <summary>The query was not syntactically valid. </summary>
		// Token: 0x040000B8 RID: 184
		InvalidQuery,
		/// <summary>The requested query language is not supported.</summary>
		// Token: 0x040000B9 RID: 185
		InvalidQueryType,
		/// <summary>In a put operation, the wbemChangeFlagCreateOnly flag was specified, but the instance already exists.</summary>
		// Token: 0x040000BA RID: 186
		AlreadyExists,
		/// <summary>The add operation cannot be performed on the qualifier because the owning object does not permit overrides.</summary>
		// Token: 0x040000BB RID: 187
		OverrideNotAllowed,
		/// <summary>The user attempted to delete a qualifier that was not owned. The qualifier was inherited from a parent class. </summary>
		// Token: 0x040000BC RID: 188
		PropagatedQualifier,
		/// <summary>The user attempted to delete a property that was not owned. The property was inherited from a parent class. </summary>
		// Token: 0x040000BD RID: 189
		PropagatedProperty,
		/// <summary>The client made an unexpected and illegal sequence of calls. </summary>
		// Token: 0x040000BE RID: 190
		Unexpected,
		/// <summary>The user requested an illegal operation, such as spawning a class from an instance.</summary>
		// Token: 0x040000BF RID: 191
		IllegalOperation,
		/// <summary>There was an illegal attempt to specify a key qualifier on a property that cannot be a key. The keys are specified in the class definition for an object and cannot be altered on a per-instance basis.</summary>
		// Token: 0x040000C0 RID: 192
		CannotBeKey,
		/// <summary>The current object is not a valid class definition. Either it is incomplete, or it has not been registered with WMI using <see cref="M:System.Management.ManagementObject.Put" />().</summary>
		// Token: 0x040000C1 RID: 193
		IncompleteClass,
		/// <summary>Reserved for future use. </summary>
		// Token: 0x040000C2 RID: 194
		InvalidSyntax,
		/// <summary>Reserved for future use. </summary>
		// Token: 0x040000C3 RID: 195
		NondecoratedObject,
		/// <summary>The property that you are attempting to modify is read-only.</summary>
		// Token: 0x040000C4 RID: 196
		ReadOnly,
		/// <summary>The provider cannot perform the requested operation, such as requesting a query that is too complex, retrieving an instance, creating or updating a class, deleting a class, or enumerating a class. </summary>
		// Token: 0x040000C5 RID: 197
		ProviderNotCapable,
		/// <summary>An attempt was made to make a change that would invalidate a derived class.</summary>
		// Token: 0x040000C6 RID: 198
		ClassHasChildren,
		/// <summary>An attempt has been made to delete or modify a class that has instances. </summary>
		// Token: 0x040000C7 RID: 199
		ClassHasInstances,
		/// <summary>Reserved for future use. </summary>
		// Token: 0x040000C8 RID: 200
		QueryNotImplemented,
		/// <summary>A value of null was specified for a property that may not be null, such as one that is marked by a Key, Indexed, or Not_Null qualifier.</summary>
		// Token: 0x040000C9 RID: 201
		IllegalNull,
		/// <summary>The value provided for a qualifier was not a legal qualifier type.</summary>
		// Token: 0x040000CA RID: 202
		InvalidQualifierType,
		/// <summary>The CIM type specified for a property is not valid. </summary>
		// Token: 0x040000CB RID: 203
		InvalidPropertyType,
		/// <summary>The request was made with an out-of-range value, or is incompatible with the type. </summary>
		// Token: 0x040000CC RID: 204
		ValueOutOfRange,
		/// <summary>An illegal attempt was made to make a class singleton, such as when the class is derived from a non-singleton class.</summary>
		// Token: 0x040000CD RID: 205
		CannotBeSingleton,
		/// <summary>The CIM type specified is not valid. </summary>
		// Token: 0x040000CE RID: 206
		InvalidCimType,
		/// <summary>The requested method is not available. </summary>
		// Token: 0x040000CF RID: 207
		InvalidMethod,
		/// <summary>The parameters provided for the method are not valid. </summary>
		// Token: 0x040000D0 RID: 208
		InvalidMethodParameters,
		/// <summary>There was an attempt to get qualifiers on a system property. </summary>
		// Token: 0x040000D1 RID: 209
		SystemProperty,
		/// <summary>The property type is not recognized. </summary>
		// Token: 0x040000D2 RID: 210
		InvalidProperty,
		/// <summary>An asynchronous process has been canceled internally or by the user. Note that because of the timing and nature of the asynchronous operation, the operation may not have been truly canceled. </summary>
		// Token: 0x040000D3 RID: 211
		CallCanceled,
		/// <summary>The user has requested an operation while WMI is in the process of closing.</summary>
		// Token: 0x040000D4 RID: 212
		ShuttingDown,
		/// <summary>An attempt was made to reuse an existing method name from a superclass, and the signatures did not match. </summary>
		// Token: 0x040000D5 RID: 213
		PropagatedMethod,
		/// <summary>One or more parameter values, such as a query text, is too complex or unsupported. WMI is requested to retry the operation with simpler parameters. </summary>
		// Token: 0x040000D6 RID: 214
		UnsupportedParameter,
		/// <summary>A parameter was missing from the method call. </summary>
		// Token: 0x040000D7 RID: 215
		MissingParameterID,
		/// <summary>A method parameter has an invalid ID qualifier. </summary>
		// Token: 0x040000D8 RID: 216
		InvalidParameterID,
		/// <summary>One or more of the method parameters have ID qualifiers that are out of sequence. </summary>
		// Token: 0x040000D9 RID: 217
		NonconsecutiveParameterIDs,
		/// <summary>The return value for a method has an ID qualifier. </summary>
		// Token: 0x040000DA RID: 218
		ParameterIDOnRetval,
		/// <summary>The specified object path was invalid. </summary>
		// Token: 0x040000DB RID: 219
		InvalidObjectPath,
		/// <summary>There is not enough free disk space to continue the operation. </summary>
		// Token: 0x040000DC RID: 220
		OutOfDiskSpace,
		/// <summary>The supplied buffer was too small to hold all the objects in the enumerator or to read a string property. </summary>
		// Token: 0x040000DD RID: 221
		BufferTooSmall,
		/// <summary>The provider does not support the requested put operation. </summary>
		// Token: 0x040000DE RID: 222
		UnsupportedPutExtension,
		/// <summary>An object with an incorrect type or version was encountered during marshaling. </summary>
		// Token: 0x040000DF RID: 223
		UnknownObjectType,
		/// <summary>A packet with an incorrect type or version was encountered during marshaling. </summary>
		// Token: 0x040000E0 RID: 224
		UnknownPacketType,
		/// <summary>The packet has an unsupported version. </summary>
		// Token: 0x040000E1 RID: 225
		MarshalVersionMismatch,
		/// <summary>The packet is corrupted.</summary>
		// Token: 0x040000E2 RID: 226
		MarshalInvalidSignature,
		/// <summary>An attempt has been made to mismatch qualifiers, such as putting [ManagementKey] on an object instead of a property. </summary>
		// Token: 0x040000E3 RID: 227
		InvalidQualifier,
		/// <summary>A duplicate parameter has been declared in a CIM method. </summary>
		// Token: 0x040000E4 RID: 228
		InvalidDuplicateParameter,
		/// <summary>Reserved for future use. </summary>
		// Token: 0x040000E5 RID: 229
		TooMuchData,
		/// <summary>The delivery of an event has failed. The provider may choose to re-raise the event.</summary>
		// Token: 0x040000E6 RID: 230
		ServerTooBusy,
		/// <summary>The specified flavor was invalid. </summary>
		// Token: 0x040000E7 RID: 231
		InvalidFlavor,
		/// <summary>An attempt has been made to create a reference that is circular (for example, deriving a class from itself). </summary>
		// Token: 0x040000E8 RID: 232
		CircularReference,
		/// <summary>The specified class is not supported. </summary>
		// Token: 0x040000E9 RID: 233
		UnsupportedClassUpdate,
		/// <summary>An attempt was made to change a key when instances or derived classes are already using the key. </summary>
		// Token: 0x040000EA RID: 234
		CannotChangeKeyInheritance,
		/// <summary>An attempt was made to change an index when instances or derived classes are already using the index. </summary>
		// Token: 0x040000EB RID: 235
		CannotChangeIndexInheritance = -2147217328,
		/// <summary>An attempt was made to create more properties than the current version of the class supports. </summary>
		// Token: 0x040000EC RID: 236
		TooManyProperties,
		/// <summary>A property was redefined with a conflicting type in a derived class. </summary>
		// Token: 0x040000ED RID: 237
		UpdateTypeMismatch,
		/// <summary>An attempt was made in a derived class to override a non-overrideable qualifier. </summary>
		// Token: 0x040000EE RID: 238
		UpdateOverrideNotAllowed,
		/// <summary>A method was redeclared with a conflicting signature in a derived class. </summary>
		// Token: 0x040000EF RID: 239
		UpdatePropagatedMethod,
		/// <summary>An attempt was made to execute a method not marked with [implemented] in any relevant class. </summary>
		// Token: 0x040000F0 RID: 240
		MethodNotImplemented,
		/// <summary>An attempt was made to execute a method marked with [disabled]. </summary>
		// Token: 0x040000F1 RID: 241
		MethodDisabled,
		/// <summary>The refresher is busy with another operation. </summary>
		// Token: 0x040000F2 RID: 242
		RefresherBusy,
		/// <summary>The filtering query is syntactically invalid. </summary>
		// Token: 0x040000F3 RID: 243
		UnparsableQuery,
		/// <summary>The FROM clause of a filtering query references a class that is not an event class. </summary>
		// Token: 0x040000F4 RID: 244
		NotEventClass,
		/// <summary>A GROUP BY clause was used without the corresponding GROUP WITHIN clause. </summary>
		// Token: 0x040000F5 RID: 245
		MissingGroupWithin,
		/// <summary>A GROUP BY clause was used. Aggregation on all properties is not supported. </summary>
		// Token: 0x040000F6 RID: 246
		MissingAggregationList,
		/// <summary>Dot notation was used on a property that is not an embedded object. </summary>
		// Token: 0x040000F7 RID: 247
		PropertyNotAnObject,
		/// <summary>A GROUP BY clause references a property that is an embedded object without using dot notation. </summary>
		// Token: 0x040000F8 RID: 248
		AggregatingByObject,
		/// <summary>An event provider registration query (__EventProviderRegistration) did not specify the classes for which events were provided. </summary>
		// Token: 0x040000F9 RID: 249
		UninterpretableProviderQuery = -2147217313,
		/// <summary>An request was made to back up or restore the repository while WinMgmt.exe was using it. </summary>
		// Token: 0x040000FA RID: 250
		BackupRestoreWinmgmtRunning,
		/// <summary>The asynchronous delivery queue overflowed from the event consumer being too slow. </summary>
		// Token: 0x040000FB RID: 251
		QueueOverflow,
		/// <summary>The operation failed because the client did not have the necessary security privilege. </summary>
		// Token: 0x040000FC RID: 252
		PrivilegeNotHeld,
		/// <summary>The operator is not valid for this property type.</summary>
		// Token: 0x040000FD RID: 253
		InvalidOperator,
		/// <summary>The user specified a user name, password, or authority on a local connection. The user must use an empty user name and password and rely on default security. </summary>
		// Token: 0x040000FE RID: 254
		LocalCredentials,
		/// <summary>The class was made abstract when its superclass is not abstract. </summary>
		// Token: 0x040000FF RID: 255
		CannotBeAbstract,
		/// <summary>An amended object was used in a put operation without the WBEM_FLAG_USE_AMENDED_QUALIFIERS flag being specified. </summary>
		// Token: 0x04000100 RID: 256
		AmendedObject,
		/// <summary>The client was not retrieving objects quickly enough from an enumeration. </summary>
		// Token: 0x04000101 RID: 257
		ClientTooSlow,
		/// <summary>The provider registration overlaps with the system event domain. </summary>
		// Token: 0x04000102 RID: 258
		RegistrationTooBroad = -2147213311,
		/// <summary>A WITHIN clause was not used in this query. </summary>
		// Token: 0x04000103 RID: 259
		RegistrationTooPrecise
	}
}
