using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000053 RID: 83
	internal struct StoreOperationUnpinDeployment
	{
		// Token: 0x06000189 RID: 393 RVA: 0x0000718E File Offset: 0x0000538E
		[SecuritySafeCritical]
		public StoreOperationUnpinDeployment(IDefinitionAppId app, StoreApplicationReference reference)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationUnpinDeployment));
			this.Flags = StoreOperationUnpinDeployment.OpFlags.Nothing;
			this.Application = app;
			this.Reference = reference.ToIntPtr();
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000071C0 File Offset: 0x000053C0
		[SecurityCritical]
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x0400015D RID: 349
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x0400015E RID: 350
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationUnpinDeployment.OpFlags Flags;

		// Token: 0x0400015F RID: 351
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000160 RID: 352
		public IntPtr Reference;

		// Token: 0x0200051E RID: 1310
		[Flags]
		public enum OpFlags
		{
			// Token: 0x040036FE RID: 14078
			Nothing = 0
		}

		// Token: 0x0200051F RID: 1311
		public enum Disposition
		{
			// Token: 0x04003700 RID: 14080
			Failed,
			// Token: 0x04003701 RID: 14081
			Unpinned
		}
	}
}
