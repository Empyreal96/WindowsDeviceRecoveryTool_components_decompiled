using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000058 RID: 88
	internal struct StoreOperationSetCanonicalizationContext
	{
		// Token: 0x06000197 RID: 407 RVA: 0x000074E7 File Offset: 0x000056E7
		[SecurityCritical]
		public StoreOperationSetCanonicalizationContext(string Bases, string Exports)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationSetCanonicalizationContext));
			this.Flags = StoreOperationSetCanonicalizationContext.OpFlags.Nothing;
			this.BaseAddressFilePath = Bases;
			this.ExportsFilePath = Exports;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000701A File Offset: 0x0000521A
		public void Destroy()
		{
		}

		// Token: 0x04000175 RID: 373
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000176 RID: 374
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationSetCanonicalizationContext.OpFlags Flags;

		// Token: 0x04000177 RID: 375
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BaseAddressFilePath;

		// Token: 0x04000178 RID: 376
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ExportsFilePath;

		// Token: 0x02000526 RID: 1318
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04003715 RID: 14101
			Nothing = 0
		}
	}
}
