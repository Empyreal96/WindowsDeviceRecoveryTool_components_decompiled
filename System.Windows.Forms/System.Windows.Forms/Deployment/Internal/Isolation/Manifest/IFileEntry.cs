using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000090 RID: 144
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("A2A55FAD-349B-469b-BF12-ADC33D14A937")]
	[ComImport]
	internal interface IFileEntry
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600024B RID: 587
		FileEntry AllData { [SecurityCritical] get; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600024C RID: 588
		string Name { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600024D RID: 589
		uint HashAlgorithm { [SecurityCritical] get; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600024E RID: 590
		string LoadFrom { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600024F RID: 591
		string SourcePath { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000250 RID: 592
		string ImportPath { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000251 RID: 593
		string SourceName { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000252 RID: 594
		string Location { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000253 RID: 595
		object HashValue { [SecurityCritical] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000254 RID: 596
		ulong Size { [SecurityCritical] get; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000255 RID: 597
		string Group { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000256 RID: 598
		uint Flags { [SecurityCritical] get; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000257 RID: 599
		IMuiResourceMapEntry MuiMapping { [SecurityCritical] get; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000258 RID: 600
		uint WritableType { [SecurityCritical] get; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000259 RID: 601
		ISection HashElements { [SecurityCritical] get; }
	}
}
