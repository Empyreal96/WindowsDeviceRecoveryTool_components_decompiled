using System;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x02000064 RID: 100
	internal static class NativeMethods
	{
		// Token: 0x06000DAF RID: 3503
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptAcquireContextW(out IntPtr hashProv, string pszContainer, string pszProvider, uint provType, uint flags);

		// Token: 0x06000DB0 RID: 3504
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptDestroyHash(IntPtr hashHandle);

		// Token: 0x06000DB1 RID: 3505
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptReleaseContext(IntPtr hashProv, int dwFlags);

		// Token: 0x06000DB2 RID: 3506
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptGetHashParam(IntPtr hashHandle, uint param, byte[] data, ref int pdwDataLen, uint flags);

		// Token: 0x06000DB3 RID: 3507
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptCreateHash(IntPtr hashProv, uint algId, IntPtr hashKey, uint flags, out IntPtr hashHandle);

		// Token: 0x06000DB4 RID: 3508
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptHashData(IntPtr hashHandle, IntPtr data, int dataLen, uint flags);
	}
}
