using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000064 RID: 100
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct DWM_TIMING_INFO
	{
		// Token: 0x040004C5 RID: 1221
		public int cbSize;

		// Token: 0x040004C6 RID: 1222
		public UNSIGNED_RATIO rateRefresh;

		// Token: 0x040004C7 RID: 1223
		public ulong qpcRefreshPeriod;

		// Token: 0x040004C8 RID: 1224
		public UNSIGNED_RATIO rateCompose;

		// Token: 0x040004C9 RID: 1225
		public ulong qpcVBlank;

		// Token: 0x040004CA RID: 1226
		public ulong cRefresh;

		// Token: 0x040004CB RID: 1227
		public uint cDXRefresh;

		// Token: 0x040004CC RID: 1228
		public ulong qpcCompose;

		// Token: 0x040004CD RID: 1229
		public ulong cFrame;

		// Token: 0x040004CE RID: 1230
		public uint cDXPresent;

		// Token: 0x040004CF RID: 1231
		public ulong cRefreshFrame;

		// Token: 0x040004D0 RID: 1232
		public ulong cFrameSubmitted;

		// Token: 0x040004D1 RID: 1233
		public uint cDXPresentSubmitted;

		// Token: 0x040004D2 RID: 1234
		public ulong cFrameConfirmed;

		// Token: 0x040004D3 RID: 1235
		public uint cDXPresentConfirmed;

		// Token: 0x040004D4 RID: 1236
		public ulong cRefreshConfirmed;

		// Token: 0x040004D5 RID: 1237
		public uint cDXRefreshConfirmed;

		// Token: 0x040004D6 RID: 1238
		public ulong cFramesLate;

		// Token: 0x040004D7 RID: 1239
		public uint cFramesOutstanding;

		// Token: 0x040004D8 RID: 1240
		public ulong cFrameDisplayed;

		// Token: 0x040004D9 RID: 1241
		public ulong qpcFrameDisplayed;

		// Token: 0x040004DA RID: 1242
		public ulong cRefreshFrameDisplayed;

		// Token: 0x040004DB RID: 1243
		public ulong cFrameComplete;

		// Token: 0x040004DC RID: 1244
		public ulong qpcFrameComplete;

		// Token: 0x040004DD RID: 1245
		public ulong cFramePending;

		// Token: 0x040004DE RID: 1246
		public ulong qpcFramePending;

		// Token: 0x040004DF RID: 1247
		public ulong cFramesDisplayed;

		// Token: 0x040004E0 RID: 1248
		public ulong cFramesComplete;

		// Token: 0x040004E1 RID: 1249
		public ulong cFramesPending;

		// Token: 0x040004E2 RID: 1250
		public ulong cFramesAvailable;

		// Token: 0x040004E3 RID: 1251
		public ulong cFramesDropped;

		// Token: 0x040004E4 RID: 1252
		public ulong cFramesMissed;

		// Token: 0x040004E5 RID: 1253
		public ulong cRefreshNextDisplayed;

		// Token: 0x040004E6 RID: 1254
		public ulong cRefreshNextPresented;

		// Token: 0x040004E7 RID: 1255
		public ulong cRefreshesDisplayed;

		// Token: 0x040004E8 RID: 1256
		public ulong cRefreshesPresented;

		// Token: 0x040004E9 RID: 1257
		public ulong cRefreshStarted;

		// Token: 0x040004EA RID: 1258
		public ulong cPixelsReceived;

		// Token: 0x040004EB RID: 1259
		public ulong cPixelsDrawn;

		// Token: 0x040004EC RID: 1260
		public ulong cBuffersEmpty;
	}
}
