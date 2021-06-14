using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000017 RID: 23
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("285a8862-c84a-11d7-850f-005cd062464f")]
	[ComImport]
	internal interface ISection
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000AE RID: 174
		object _NewEnum { [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000AF RID: 175
		uint Count { get; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000B0 RID: 176
		uint SectionID { get; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000B1 RID: 177
		string SectionName { [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
