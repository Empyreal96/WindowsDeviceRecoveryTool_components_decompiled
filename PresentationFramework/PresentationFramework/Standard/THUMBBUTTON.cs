using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000077 RID: 119
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 8)]
	internal struct THUMBBUTTON
	{
		// Token: 0x04000565 RID: 1381
		public const int THBN_CLICKED = 6144;

		// Token: 0x04000566 RID: 1382
		public THB dwMask;

		// Token: 0x04000567 RID: 1383
		public uint iId;

		// Token: 0x04000568 RID: 1384
		public uint iBitmap;

		// Token: 0x04000569 RID: 1385
		public IntPtr hIcon;

		// Token: 0x0400056A RID: 1386
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string szTip;

		// Token: 0x0400056B RID: 1387
		public THBF dwFlags;
	}
}
