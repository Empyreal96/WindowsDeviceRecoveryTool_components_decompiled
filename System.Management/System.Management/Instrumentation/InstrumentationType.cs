using System;

namespace System.Management.Instrumentation
{
	/// <summary>Specifies the type of instrumentation provided by a class.           Note: the WMI .NET libraries are now considered in final state, and no further development, enhancements, or updates will be available for non-security related issues affecting these libraries. The MI APIs should be used for all new development.</summary>
	// Token: 0x020000AD RID: 173
	public enum InstrumentationType
	{
		/// <summary>The class provides instances for management instrumentation.</summary>
		// Token: 0x040004E4 RID: 1252
		Instance,
		/// <summary>The class provides events for management instrumentation.</summary>
		// Token: 0x040004E5 RID: 1253
		Event,
		/// <summary>The class defines an abstract class for management instrumentation.</summary>
		// Token: 0x040004E6 RID: 1254
		Abstract
	}
}
