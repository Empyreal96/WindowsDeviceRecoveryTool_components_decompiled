using System;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Standard
{
	// Token: 0x0200003F RID: 63
	[SecurityCritical]
	internal sealed class SafeHBITMAP : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600005D RID: 93 RVA: 0x00002EF9 File Offset: 0x000010F9
		[SecurityCritical]
		private SafeHBITMAP() : base(true)
		{
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000030BD File Offset: 0x000012BD
		[SecurityCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			return NativeMethods.DeleteObject(this.handle);
		}
	}
}
