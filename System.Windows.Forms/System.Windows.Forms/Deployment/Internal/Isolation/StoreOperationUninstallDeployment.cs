using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000055 RID: 85
	internal struct StoreOperationUninstallDeployment
	{
		// Token: 0x0600018E RID: 398 RVA: 0x00007233 File Offset: 0x00005433
		[SecuritySafeCritical]
		public StoreOperationUninstallDeployment(IDefinitionAppId appid, StoreApplicationReference AppRef)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationUninstallDeployment));
			this.Flags = StoreOperationUninstallDeployment.OpFlags.Nothing;
			this.Application = appid;
			this.Reference = AppRef.ToIntPtr();
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00007265 File Offset: 0x00005465
		[SecurityCritical]
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x04000165 RID: 357
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000166 RID: 358
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationUninstallDeployment.OpFlags Flags;

		// Token: 0x04000167 RID: 359
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000168 RID: 360
		public IntPtr Reference;

		// Token: 0x02000522 RID: 1314
		[Flags]
		public enum OpFlags
		{
			// Token: 0x0400370A RID: 14090
			Nothing = 0
		}

		// Token: 0x02000523 RID: 1315
		public enum Disposition
		{
			// Token: 0x0400370C RID: 14092
			Failed,
			// Token: 0x0400370D RID: 14093
			DidNotExist,
			// Token: 0x0400370E RID: 14094
			Uninstalled
		}
	}
}
