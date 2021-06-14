using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000088 RID: 136
	[StructLayout(LayoutKind.Sequential)]
	internal class MuiResourceMapEntry : IDisposable
	{
		// Token: 0x06000235 RID: 565 RVA: 0x00008798 File Offset: 0x00006998
		~MuiResourceMapEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000087C8 File Offset: 0x000069C8
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x000087D4 File Offset: 0x000069D4
		[SecuritySafeCritical]
		public void Dispose(bool fDisposing)
		{
			if (this.ResourceTypeIdInt != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.ResourceTypeIdInt);
				this.ResourceTypeIdInt = IntPtr.Zero;
			}
			if (this.ResourceTypeIdString != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.ResourceTypeIdString);
				this.ResourceTypeIdString = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x04000243 RID: 579
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr ResourceTypeIdInt;

		// Token: 0x04000244 RID: 580
		public uint ResourceTypeIdIntSize;

		// Token: 0x04000245 RID: 581
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr ResourceTypeIdString;

		// Token: 0x04000246 RID: 582
		public uint ResourceTypeIdStringSize;
	}
}
