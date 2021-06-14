using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000082 RID: 130
	[StructLayout(LayoutKind.Sequential)]
	internal class MuiResourceTypeIdStringEntry : IDisposable
	{
		// Token: 0x06000227 RID: 551 RVA: 0x00008650 File Offset: 0x00006850
		~MuiResourceTypeIdStringEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00008680 File Offset: 0x00006880
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000868C File Offset: 0x0000688C
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

		// Token: 0x04000231 RID: 561
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr StringIds;

		// Token: 0x04000232 RID: 562
		public uint StringIdsSize;

		// Token: 0x04000233 RID: 563
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr IntegerIds;

		// Token: 0x04000234 RID: 564
		public uint IntegerIdsSize;
	}
}
