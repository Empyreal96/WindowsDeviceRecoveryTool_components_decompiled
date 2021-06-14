using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000D5 RID: 213
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("49e1fe8d-ebb8-4593-8c4e-3e14c845b142")]
	[ComImport]
	internal interface IRegistryValueEntry
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060002F6 RID: 758
		RegistryValueEntry AllData { [SecurityCritical] get; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060002F7 RID: 759
		uint Flags { [SecurityCritical] get; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060002F8 RID: 760
		uint OperationHint { [SecurityCritical] get; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060002F9 RID: 761
		uint Type { [SecurityCritical] get; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060002FA RID: 762
		string Value { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060002FB RID: 763
		string BuildFilter { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
