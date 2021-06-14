using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200001A RID: 26
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("285a8861-c84a-11d7-850f-005cd062464f")]
	[ComImport]
	internal interface ISectionEntry
	{
		// Token: 0x060000B5 RID: 181
		object GetField(uint fieldId);

		// Token: 0x060000B6 RID: 182
		string GetFieldName(uint fieldId);
	}
}
