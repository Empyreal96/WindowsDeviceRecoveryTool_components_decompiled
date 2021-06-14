using System;

namespace System.Management.Instrumentation
{
	/// <summary>Specifies a source of a management instrumentation instance. Objects that implement this interface are known to be sources of management instrumentation instances. Classes that do not derive from <see cref="T:System.Management.Instrumentation.Instance" /> should implement this interface instead.          Note: the WMI .NET libraries are now considered in final state, and no further development, enhancements, or updates will be available for non-security related issues affecting these libraries. The MI APIs should be used for all new development.</summary>
	// Token: 0x020000BC RID: 188
	public interface IInstance
	{
		/// <summary>Gets or sets a value indicating whether instances of classes that implement this interface are visible through management instrumentation.                       </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether instances of classes that implement this interface are visible through management instrumentation.</returns>
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000508 RID: 1288
		// (set) Token: 0x06000509 RID: 1289
		bool Published { get; set; }
	}
}
