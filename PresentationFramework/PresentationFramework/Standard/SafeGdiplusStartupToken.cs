using System;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Standard
{
	// Token: 0x02000040 RID: 64
	[SecurityCritical]
	internal sealed class SafeGdiplusStartupToken : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00002EF9 File Offset: 0x000010F9
		[SecurityCritical]
		private SafeGdiplusStartupToken() : base(true)
		{
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000030CC File Offset: 0x000012CC
		[SecurityCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			Status status = NativeMethods.GdiplusShutdown(this.handle);
			return status == Status.Ok;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000030EC File Offset: 0x000012EC
		[SecurityCritical]
		public static SafeGdiplusStartupToken Startup()
		{
			SafeGdiplusStartupToken safeGdiplusStartupToken = new SafeGdiplusStartupToken();
			IntPtr handle;
			StartupOutput startupOutput;
			if (NativeMethods.GdiplusStartup(out handle, new StartupInput(), out startupOutput) == Status.Ok)
			{
				safeGdiplusStartupToken.handle = handle;
				return safeGdiplusStartupToken;
			}
			safeGdiplusStartupToken.Dispose();
			throw new Exception("Unable to initialize GDI+");
		}
	}
}
