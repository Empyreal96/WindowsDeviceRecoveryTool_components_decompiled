using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000053 RID: 83
	internal class WbemErrorInfo
	{
		// Token: 0x0600034A RID: 842 RVA: 0x00020B24 File Offset: 0x0001ED24
		public static IWbemClassObjectFreeThreaded GetErrorInfo()
		{
			IntPtr intPtr = WmiNetUtilsHelper.GetErrorInfo_f();
			if (IntPtr.Zero != intPtr && new IntPtr(-1) != intPtr)
			{
				IntPtr intPtr2;
				Marshal.QueryInterface(intPtr, ref IWbemClassObjectFreeThreaded.IID_IWbemClassObject, out intPtr2);
				Marshal.Release(intPtr);
				if (intPtr2 != IntPtr.Zero)
				{
					return new IWbemClassObjectFreeThreaded(intPtr2);
				}
			}
			return null;
		}
	}
}
