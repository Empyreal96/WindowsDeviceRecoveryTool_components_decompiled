using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200001D RID: 29
	internal struct BLOB : IDisposable
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x00006966 File Offset: 0x00004B66
		[SecuritySafeCritical]
		public void Dispose()
		{
			if (this.BlobData != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.BlobData);
				this.BlobData = IntPtr.Zero;
			}
		}

		// Token: 0x040000FA RID: 250
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x040000FB RID: 251
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr BlobData;
	}
}
