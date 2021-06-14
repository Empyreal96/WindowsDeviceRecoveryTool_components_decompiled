using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000D6 RID: 214
	[StructLayout(LayoutKind.Sequential)]
	internal class RegistryKeyEntry : IDisposable
	{
		// Token: 0x060002FC RID: 764 RVA: 0x00008A88 File Offset: 0x00006C88
		~RegistryKeyEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00008AB8 File Offset: 0x00006CB8
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00008AC4 File Offset: 0x00006CC4
		[SecuritySafeCritical]
		public void Dispose(bool fDisposing)
		{
			if (this.SecurityDescriptor != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.SecurityDescriptor);
				this.SecurityDescriptor = IntPtr.Zero;
			}
			if (this.Values != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.Values);
				this.Values = IntPtr.Zero;
			}
			if (this.Keys != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.Keys);
				this.Keys = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x04000363 RID: 867
		public uint Flags;

		// Token: 0x04000364 RID: 868
		public uint Protection;

		// Token: 0x04000365 RID: 869
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BuildFilter;

		// Token: 0x04000366 RID: 870
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr SecurityDescriptor;

		// Token: 0x04000367 RID: 871
		public uint SecurityDescriptorSize;

		// Token: 0x04000368 RID: 872
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr Values;

		// Token: 0x04000369 RID: 873
		public uint ValuesSize;

		// Token: 0x0400036A RID: 874
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr Keys;

		// Token: 0x0400036B RID: 875
		public uint KeysSize;
	}
}
