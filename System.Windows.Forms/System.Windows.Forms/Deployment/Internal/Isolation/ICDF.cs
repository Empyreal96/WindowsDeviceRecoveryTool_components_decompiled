using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200001C RID: 28
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("285a8860-c84a-11d7-850f-005cd062464f")]
	[ComImport]
	internal interface ICDF
	{
		// Token: 0x060000BB RID: 187
		ISection GetRootSection(uint SectionId);

		// Token: 0x060000BC RID: 188
		ISectionEntry GetRootSectionEntry(uint SectionId);

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000BD RID: 189
		object _NewEnum { [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000BE RID: 190
		uint Count { get; }

		// Token: 0x060000BF RID: 191
		object GetItem(uint SectionId);
	}
}
