using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000103 RID: 259
	internal static class ApiHelper
	{
		// Token: 0x0600043C RID: 1084 RVA: 0x0000D7B8 File Offset: 0x0000B9B8
		public static bool IsApiAvailable(string libName, string procName)
		{
			bool flag = false;
			if (!string.IsNullOrEmpty(libName) && !string.IsNullOrEmpty(procName))
			{
				Tuple<string, string> key = new Tuple<string, string>(libName, procName);
				if (ApiHelper.availableApis.TryGetValue(key, out flag))
				{
					return flag;
				}
				IntPtr intPtr = CommonUnsafeNativeMethods.LoadLibraryFromSystemPathIfAvailable(libName);
				if (intPtr != IntPtr.Zero)
				{
					IntPtr procAddress = CommonUnsafeNativeMethods.GetProcAddress(new HandleRef(flag, intPtr), procName);
					if (procAddress != IntPtr.Zero)
					{
						flag = true;
					}
				}
				CommonUnsafeNativeMethods.FreeLibrary(new HandleRef(flag, intPtr));
				ApiHelper.availableApis[key] = flag;
			}
			return flag;
		}

		// Token: 0x0400044D RID: 1101
		private static ConcurrentDictionary<Tuple<string, string>, bool> availableApis = new ConcurrentDictionary<Tuple<string, string>, bool>();
	}
}
