using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000A6 RID: 166
	[StructLayout(LayoutKind.Sequential)]
	internal class AssemblyReferenceDependentAssemblyEntry : IDisposable
	{
		// Token: 0x06000281 RID: 641 RVA: 0x00008978 File Offset: 0x00006B78
		~AssemblyReferenceDependentAssemblyEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x000089A8 File Offset: 0x00006BA8
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x000089B1 File Offset: 0x00006BB1
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
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x040002AC RID: 684
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Group;

		// Token: 0x040002AD RID: 685
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Codebase;

		// Token: 0x040002AE RID: 686
		public ulong Size;

		// Token: 0x040002AF RID: 687
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr HashValue;

		// Token: 0x040002B0 RID: 688
		public uint HashValueSize;

		// Token: 0x040002B1 RID: 689
		public uint HashAlgorithm;

		// Token: 0x040002B2 RID: 690
		public uint Flags;

		// Token: 0x040002B3 RID: 691
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ResourceFallbackCulture;

		// Token: 0x040002B4 RID: 692
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x040002B5 RID: 693
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SupportUrl;

		// Token: 0x040002B6 RID: 694
		public ISection HashElements;
	}
}
