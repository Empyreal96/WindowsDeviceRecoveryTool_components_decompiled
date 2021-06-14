using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000085 RID: 133
	[StructLayout(LayoutKind.Sequential)]
	internal class MuiResourceTypeIdIntEntry : IDisposable
	{
		// Token: 0x0600022E RID: 558 RVA: 0x000086F4 File Offset: 0x000068F4
		~MuiResourceTypeIdIntEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00008724 File Offset: 0x00006924
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00008730 File Offset: 0x00006930
		[SecuritySafeCritical]
		public void Dispose(bool fDisposing)
		{
			if (this.StringIds != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.StringIds);
				this.StringIds = IntPtr.Zero;
			}
			if (this.IntegerIds != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.IntegerIds);
				this.IntegerIds = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x0400023A RID: 570
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr StringIds;

		// Token: 0x0400023B RID: 571
		public uint StringIdsSize;

		// Token: 0x0400023C RID: 572
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr IntegerIds;

		// Token: 0x0400023D RID: 573
		public uint IntegerIdsSize;
	}
}
