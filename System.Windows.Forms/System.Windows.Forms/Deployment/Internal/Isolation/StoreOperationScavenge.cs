using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000059 RID: 89
	internal struct StoreOperationScavenge
	{
		// Token: 0x06000199 RID: 409 RVA: 0x00007514 File Offset: 0x00005714
		[SecuritySafeCritical]
		public StoreOperationScavenge(bool Light, ulong SizeLimit, ulong RunLimit, uint ComponentLimit)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationScavenge));
			this.Flags = StoreOperationScavenge.OpFlags.Nothing;
			if (Light)
			{
				this.Flags |= StoreOperationScavenge.OpFlags.Light;
			}
			this.SizeReclaimationLimit = SizeLimit;
			if (SizeLimit != 0UL)
			{
				this.Flags |= StoreOperationScavenge.OpFlags.LimitSize;
			}
			this.RuntimeLimit = RunLimit;
			if (RunLimit != 0UL)
			{
				this.Flags |= StoreOperationScavenge.OpFlags.LimitTime;
			}
			this.ComponentCountLimit = ComponentLimit;
			if (ComponentLimit != 0U)
			{
				this.Flags |= StoreOperationScavenge.OpFlags.LimitCount;
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00007598 File Offset: 0x00005798
		public StoreOperationScavenge(bool Light)
		{
			this = new StoreOperationScavenge(Light, 0UL, 0UL, 0U);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000701A File Offset: 0x0000521A
		public void Destroy()
		{
		}

		// Token: 0x04000179 RID: 377
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x0400017A RID: 378
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationScavenge.OpFlags Flags;

		// Token: 0x0400017B RID: 379
		[MarshalAs(UnmanagedType.U8)]
		public ulong SizeReclaimationLimit;

		// Token: 0x0400017C RID: 380
		[MarshalAs(UnmanagedType.U8)]
		public ulong RuntimeLimit;

		// Token: 0x0400017D RID: 381
		[MarshalAs(UnmanagedType.U4)]
		public uint ComponentCountLimit;

		// Token: 0x02000527 RID: 1319
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04003717 RID: 14103
			Nothing = 0,
			// Token: 0x04003718 RID: 14104
			Light = 1,
			// Token: 0x04003719 RID: 14105
			LimitSize = 2,
			// Token: 0x0400371A RID: 14106
			LimitTime = 4,
			// Token: 0x0400371B RID: 14107
			LimitCount = 8
		}
	}
}
