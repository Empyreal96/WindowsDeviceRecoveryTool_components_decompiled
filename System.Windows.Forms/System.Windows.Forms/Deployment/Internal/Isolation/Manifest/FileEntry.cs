using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200008E RID: 142
	[StructLayout(LayoutKind.Sequential)]
	internal class FileEntry : IDisposable
	{
		// Token: 0x06000247 RID: 583 RVA: 0x000088E0 File Offset: 0x00006AE0
		~FileEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00008910 File Offset: 0x00006B10
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000891C File Offset: 0x00006B1C
		[SecuritySafeCritical]
		public void Dispose(bool fDisposing)
		{
			if (this.HashValue != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.HashValue);
				this.HashValue = IntPtr.Zero;
			}
			if (fDisposing)
			{
				if (this.MuiMapping != null)
				{
					this.MuiMapping.Dispose(true);
					this.MuiMapping = null;
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x0400025C RID: 604
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x0400025D RID: 605
		public uint HashAlgorithm;

		// Token: 0x0400025E RID: 606
		[MarshalAs(UnmanagedType.LPWStr)]
		public string LoadFrom;

		// Token: 0x0400025F RID: 607
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SourcePath;

		// Token: 0x04000260 RID: 608
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ImportPath;

		// Token: 0x04000261 RID: 609
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SourceName;

		// Token: 0x04000262 RID: 610
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Location;

		// Token: 0x04000263 RID: 611
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr HashValue;

		// Token: 0x04000264 RID: 612
		public uint HashValueSize;

		// Token: 0x04000265 RID: 613
		public ulong Size;

		// Token: 0x04000266 RID: 614
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Group;

		// Token: 0x04000267 RID: 615
		public uint Flags;

		// Token: 0x04000268 RID: 616
		public MuiResourceMapEntry MuiMapping;

		// Token: 0x04000269 RID: 617
		public uint WritableType;

		// Token: 0x0400026A RID: 618
		public ISection HashElements;
	}
}
