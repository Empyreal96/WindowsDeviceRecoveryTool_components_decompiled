using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000093 RID: 147
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0C66F299-E08E-48c5-9264-7CCBEB4D5CBB")]
	[ComImport]
	internal interface IFileAssociationEntry
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600025B RID: 603
		FileAssociationEntry AllData { [SecurityCritical] get; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600025C RID: 604
		string Extension { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600025D RID: 605
		string Description { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600025E RID: 606
		string ProgID { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600025F RID: 607
		string DefaultIcon { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000260 RID: 608
		string Parameter { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
