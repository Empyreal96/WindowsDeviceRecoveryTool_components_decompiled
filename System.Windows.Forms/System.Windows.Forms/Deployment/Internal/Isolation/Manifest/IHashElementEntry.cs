using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200008D RID: 141
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9D46FB70-7B54-4f4f-9331-BA9E87833FF5")]
	[ComImport]
	internal interface IHashElementEntry
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000240 RID: 576
		HashElementEntry AllData { [SecurityCritical] get; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000241 RID: 577
		uint index { [SecurityCritical] get; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000242 RID: 578
		byte Transform { [SecurityCritical] get; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000243 RID: 579
		object TransformMetadata { [SecurityCritical] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000244 RID: 580
		byte DigestMethod { [SecurityCritical] get; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000245 RID: 581
		object DigestValue { [SecurityCritical] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000246 RID: 582
		string Xml { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
