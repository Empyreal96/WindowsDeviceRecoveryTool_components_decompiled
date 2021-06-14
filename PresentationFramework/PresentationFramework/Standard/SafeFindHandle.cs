using System;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace Standard
{
	// Token: 0x0200003D RID: 61
	internal sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00002EF9 File Offset: 0x000010F9
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		private SafeFindHandle() : base(true)
		{
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002F02 File Offset: 0x00001102
		[SecurityCritical]
		protected override bool ReleaseHandle()
		{
			return NativeMethods.FindClose(this.handle);
		}
	}
}
