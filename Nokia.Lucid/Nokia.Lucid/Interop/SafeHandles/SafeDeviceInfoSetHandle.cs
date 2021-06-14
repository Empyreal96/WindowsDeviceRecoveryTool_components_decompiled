using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace Nokia.Lucid.Interop.SafeHandles
{
	// Token: 0x02000034 RID: 52
	public sealed class SafeDeviceInfoSetHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600015D RID: 349 RVA: 0x0000B414 File Offset: 0x00009614
		private SafeDeviceInfoSetHandle() : base(true)
		{
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000B41D File Offset: 0x0000961D
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected override bool ReleaseHandle()
		{
			return SetupApiNativeMethods.SetupDiDestroyDeviceInfoList(this.handle);
		}
	}
}
