using System;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace MS.Internal.Progressivity
{
	// Token: 0x02000655 RID: 1621
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("e7b92912-c7ca-4629-8f39-0f537cfab57e")]
	[ComImport]
	internal interface IByteRangeDownloaderService
	{
		// Token: 0x06006BCA RID: 27594
		[SecurityCritical]
		void InitializeByteRangeDownloader([MarshalAs(UnmanagedType.LPWStr)] string url, [MarshalAs(UnmanagedType.LPWStr)] string tempFile, SafeWaitHandle eventHandle);

		// Token: 0x06006BCB RID: 27595
		void RequestDownloadByteRanges([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] byteRanges, int size);

		// Token: 0x06006BCC RID: 27596
		void GetDownloadedByteRanges([MarshalAs(UnmanagedType.LPArray)] out int[] byteRanges, [MarshalAs(UnmanagedType.I4)] out int size);

		// Token: 0x06006BCD RID: 27597
		void ReleaseByteRangeDownloader();
	}
}
