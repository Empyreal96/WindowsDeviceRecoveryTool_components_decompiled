using System;

namespace System.Management.Instrumentation
{
	/// <summary>Specifies a source of a management instrumentation event. Objects that implement this interface are known to be sources of management instrumentation events. Classes that do not derive from <see cref="T:System.Management.Instrumentation.BaseEvent" /> should implement this interface instead.          Note: the WMI .NET libraries are now considered in final state, and no further development, enhancements, or updates will be available for non-security related issues affecting these libraries. The MI APIs should be used for all new development.</summary>
	// Token: 0x020000BA RID: 186
	public interface IEvent
	{
		/// <summary>Raises a management event.          </summary>
		// Token: 0x06000504 RID: 1284
		void Fire();
	}
}
