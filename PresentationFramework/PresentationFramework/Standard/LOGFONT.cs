using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000050 RID: 80
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct LOGFONT
	{
		// Token: 0x04000456 RID: 1110
		public int lfHeight;

		// Token: 0x04000457 RID: 1111
		public int lfWidth;

		// Token: 0x04000458 RID: 1112
		public int lfEscapement;

		// Token: 0x04000459 RID: 1113
		public int lfOrientation;

		// Token: 0x0400045A RID: 1114
		public int lfWeight;

		// Token: 0x0400045B RID: 1115
		public byte lfItalic;

		// Token: 0x0400045C RID: 1116
		public byte lfUnderline;

		// Token: 0x0400045D RID: 1117
		public byte lfStrikeOut;

		// Token: 0x0400045E RID: 1118
		public byte lfCharSet;

		// Token: 0x0400045F RID: 1119
		public byte lfOutPrecision;

		// Token: 0x04000460 RID: 1120
		public byte lfClipPrecision;

		// Token: 0x04000461 RID: 1121
		public byte lfQuality;

		// Token: 0x04000462 RID: 1122
		public byte lfPitchAndFamily;

		// Token: 0x04000463 RID: 1123
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string lfFaceName;
	}
}
