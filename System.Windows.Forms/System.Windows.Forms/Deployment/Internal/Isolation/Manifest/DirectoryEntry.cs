using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000D9 RID: 217
	[StructLayout(LayoutKind.Sequential)]
	internal class DirectoryEntry : IDisposable
	{
		// Token: 0x06000307 RID: 775 RVA: 0x00008B54 File Offset: 0x00006D54
		~DirectoryEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00008B84 File Offset: 0x00006D84
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00008B8D File Offset: 0x00006D8D
		[SecuritySafeCritical]
		public void Dispose(bool fDisposing)
		{
			if (this.SecurityDescriptor != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.SecurityDescriptor);
				this.SecurityDescriptor = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x04000376 RID: 886
		public uint Flags;

		// Token: 0x04000377 RID: 887
		public uint Protection;

		// Token: 0x04000378 RID: 888
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BuildFilter;

		// Token: 0x04000379 RID: 889
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr SecurityDescriptor;

		// Token: 0x0400037A RID: 890
		public uint SecurityDescriptorSize;
	}
}
