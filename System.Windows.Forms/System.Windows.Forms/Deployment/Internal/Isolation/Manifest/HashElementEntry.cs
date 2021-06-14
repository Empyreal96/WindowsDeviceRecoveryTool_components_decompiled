using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200008B RID: 139
	[StructLayout(LayoutKind.Sequential)]
	internal class HashElementEntry : IDisposable
	{
		// Token: 0x0600023C RID: 572 RVA: 0x0000883C File Offset: 0x00006A3C
		~HashElementEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000886C File Offset: 0x00006A6C
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00008878 File Offset: 0x00006A78
		[SecuritySafeCritical]
		public void Dispose(bool fDisposing)
		{
			if (this.TransformMetadata != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.TransformMetadata);
				this.TransformMetadata = IntPtr.Zero;
			}
			if (this.DigestValue != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.DigestValue);
				this.DigestValue = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x0400024C RID: 588
		public uint index;

		// Token: 0x0400024D RID: 589
		public byte Transform;

		// Token: 0x0400024E RID: 590
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr TransformMetadata;

		// Token: 0x0400024F RID: 591
		public uint TransformMetadataSize;

		// Token: 0x04000250 RID: 592
		public byte DigestMethod;

		// Token: 0x04000251 RID: 593
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr DigestValue;

		// Token: 0x04000252 RID: 594
		public uint DigestValueSize;

		// Token: 0x04000253 RID: 595
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Xml;
	}
}
