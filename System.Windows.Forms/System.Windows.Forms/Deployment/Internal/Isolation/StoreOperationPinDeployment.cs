using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000052 RID: 82
	internal struct StoreOperationPinDeployment
	{
		// Token: 0x06000186 RID: 390 RVA: 0x0000712F File Offset: 0x0000532F
		[SecuritySafeCritical]
		public StoreOperationPinDeployment(IDefinitionAppId AppId, StoreApplicationReference Ref)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationPinDeployment));
			this.Flags = StoreOperationPinDeployment.OpFlags.NeverExpires;
			this.Application = AppId;
			this.Reference = Ref.ToIntPtr();
			this.ExpirationTime = 0L;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00007169 File Offset: 0x00005369
		public StoreOperationPinDeployment(IDefinitionAppId AppId, DateTime Expiry, StoreApplicationReference Ref)
		{
			this = new StoreOperationPinDeployment(AppId, Ref);
			this.Flags |= StoreOperationPinDeployment.OpFlags.NeverExpires;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00007181 File Offset: 0x00005381
		[SecurityCritical]
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x04000158 RID: 344
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000159 RID: 345
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationPinDeployment.OpFlags Flags;

		// Token: 0x0400015A RID: 346
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x0400015B RID: 347
		[MarshalAs(UnmanagedType.I8)]
		public long ExpirationTime;

		// Token: 0x0400015C RID: 348
		public IntPtr Reference;

		// Token: 0x0200051C RID: 1308
		[Flags]
		public enum OpFlags
		{
			// Token: 0x040036F8 RID: 14072
			Nothing = 0,
			// Token: 0x040036F9 RID: 14073
			NeverExpires = 1
		}

		// Token: 0x0200051D RID: 1309
		public enum Disposition
		{
			// Token: 0x040036FB RID: 14075
			Failed,
			// Token: 0x040036FC RID: 14076
			Pinned
		}
	}
}
