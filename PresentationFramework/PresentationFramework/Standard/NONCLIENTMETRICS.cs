using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000052 RID: 82
	internal struct NONCLIENTMETRICS
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003278 File Offset: 0x00001478
		public static NONCLIENTMETRICS VistaMetricsStruct
		{
			get
			{
				return new NONCLIENTMETRICS
				{
					cbSize = Marshal.SizeOf(typeof(NONCLIENTMETRICS))
				};
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000032A4 File Offset: 0x000014A4
		public static NONCLIENTMETRICS XPMetricsStruct
		{
			get
			{
				return new NONCLIENTMETRICS
				{
					cbSize = Marshal.SizeOf(typeof(NONCLIENTMETRICS)) - 4
				};
			}
		}

		// Token: 0x04000469 RID: 1129
		public int cbSize;

		// Token: 0x0400046A RID: 1130
		public int iBorderWidth;

		// Token: 0x0400046B RID: 1131
		public int iScrollWidth;

		// Token: 0x0400046C RID: 1132
		public int iScrollHeight;

		// Token: 0x0400046D RID: 1133
		public int iCaptionWidth;

		// Token: 0x0400046E RID: 1134
		public int iCaptionHeight;

		// Token: 0x0400046F RID: 1135
		public LOGFONT lfCaptionFont;

		// Token: 0x04000470 RID: 1136
		public int iSmCaptionWidth;

		// Token: 0x04000471 RID: 1137
		public int iSmCaptionHeight;

		// Token: 0x04000472 RID: 1138
		public LOGFONT lfSmCaptionFont;

		// Token: 0x04000473 RID: 1139
		public int iMenuWidth;

		// Token: 0x04000474 RID: 1140
		public int iMenuHeight;

		// Token: 0x04000475 RID: 1141
		public LOGFONT lfMenuFont;

		// Token: 0x04000476 RID: 1142
		public LOGFONT lfStatusFont;

		// Token: 0x04000477 RID: 1143
		public LOGFONT lfMessageFont;

		// Token: 0x04000478 RID: 1144
		public int iPaddedBorderWidth;
	}
}
