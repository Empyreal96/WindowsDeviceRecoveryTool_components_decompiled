using System;

namespace System.Management
{
	/// <summary>Describes the possible text formats that can be used with <see cref="M:System.Management.ManagementBaseObject.GetText(System.Management.TextFormat)" />.          </summary>
	// Token: 0x02000005 RID: 5
	public enum TextFormat
	{
		/// <summary>
		///     Managed Object Format
		///   </summary>
		// Token: 0x04000051 RID: 81
		Mof,
		/// <summary>XML DTD that corresponds to CIM DTD version 2.0.             </summary>
		// Token: 0x04000052 RID: 82
		CimDtd20,
		/// <summary>XML WMI DTD that corresponds to CIM DTD version 2.0. Using this value enables a few WMI-specific extensions, like embedded objects.             </summary>
		// Token: 0x04000053 RID: 83
		WmiDtd20
	}
}
