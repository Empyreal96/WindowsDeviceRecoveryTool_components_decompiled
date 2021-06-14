using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000051 RID: 81
	internal struct StoreApplicationReference
	{
		// Token: 0x06000183 RID: 387 RVA: 0x000070A1 File Offset: 0x000052A1
		[SecuritySafeCritical]
		public StoreApplicationReference(Guid RefScheme, string Id, string NcData)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreApplicationReference));
			this.Flags = StoreApplicationReference.RefFlags.Nothing;
			this.GuidScheme = RefScheme;
			this.Identifier = Id;
			this.NonCanonicalData = NcData;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000070D4 File Offset: 0x000052D4
		[SecurityCritical]
		public IntPtr ToIntPtr()
		{
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
			Marshal.StructureToPtr(this, intPtr, false);
			return intPtr;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000710A File Offset: 0x0000530A
		[SecurityCritical]
		public static void Destroy(IntPtr ip)
		{
			if (ip != IntPtr.Zero)
			{
				Marshal.DestroyStructure(ip, typeof(StoreApplicationReference));
				Marshal.FreeCoTaskMem(ip);
			}
		}

		// Token: 0x04000153 RID: 339
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000154 RID: 340
		[MarshalAs(UnmanagedType.U4)]
		public StoreApplicationReference.RefFlags Flags;

		// Token: 0x04000155 RID: 341
		public Guid GuidScheme;

		// Token: 0x04000156 RID: 342
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Identifier;

		// Token: 0x04000157 RID: 343
		[MarshalAs(UnmanagedType.LPWStr)]
		public string NonCanonicalData;

		// Token: 0x0200051B RID: 1307
		[Flags]
		public enum RefFlags
		{
			// Token: 0x040036F6 RID: 14070
			Nothing = 0
		}
	}
}
