using System;

namespace System.Windows.Forms.Internal
{
	// Token: 0x020004F2 RID: 1266
	internal static class MeasurementDCInfo
	{
		// Token: 0x0600532B RID: 21291 RVA: 0x0015CE98 File Offset: 0x0015B098
		internal static bool IsMeasurementDC(DeviceContext dc)
		{
			WindowsGraphics currentMeasurementGraphics = WindowsGraphicsCacheManager.GetCurrentMeasurementGraphics();
			return currentMeasurementGraphics != null && currentMeasurementGraphics.DeviceContext != null && currentMeasurementGraphics.DeviceContext.Hdc == dc.Hdc;
		}

		// Token: 0x17001432 RID: 5170
		// (get) Token: 0x0600532C RID: 21292 RVA: 0x0015CECE File Offset: 0x0015B0CE
		// (set) Token: 0x0600532D RID: 21293 RVA: 0x0015CEE3 File Offset: 0x0015B0E3
		internal static WindowsFont LastUsedFont
		{
			get
			{
				if (MeasurementDCInfo.cachedMeasurementDCInfo != null)
				{
					return MeasurementDCInfo.cachedMeasurementDCInfo.LastUsedFont;
				}
				return null;
			}
			set
			{
				if (MeasurementDCInfo.cachedMeasurementDCInfo == null)
				{
					MeasurementDCInfo.cachedMeasurementDCInfo = new MeasurementDCInfo.CachedInfo();
				}
				MeasurementDCInfo.cachedMeasurementDCInfo.UpdateFont(value);
			}
		}

		// Token: 0x0600532E RID: 21294 RVA: 0x0015CF04 File Offset: 0x0015B104
		internal static IntNativeMethods.DRAWTEXTPARAMS GetTextMargins(WindowsGraphics wg, WindowsFont font)
		{
			MeasurementDCInfo.CachedInfo cachedInfo = MeasurementDCInfo.cachedMeasurementDCInfo;
			if (cachedInfo != null && cachedInfo.LeftTextMargin > 0 && cachedInfo.RightTextMargin > 0 && font == cachedInfo.LastUsedFont)
			{
				return new IntNativeMethods.DRAWTEXTPARAMS(cachedInfo.LeftTextMargin, cachedInfo.RightTextMargin);
			}
			if (cachedInfo == null)
			{
				cachedInfo = new MeasurementDCInfo.CachedInfo();
				MeasurementDCInfo.cachedMeasurementDCInfo = cachedInfo;
			}
			IntNativeMethods.DRAWTEXTPARAMS textMargins = wg.GetTextMargins(font);
			cachedInfo.LeftTextMargin = textMargins.iLeftMargin;
			cachedInfo.RightTextMargin = textMargins.iRightMargin;
			return new IntNativeMethods.DRAWTEXTPARAMS(cachedInfo.LeftTextMargin, cachedInfo.RightTextMargin);
		}

		// Token: 0x0600532F RID: 21295 RVA: 0x0015CF88 File Offset: 0x0015B188
		internal static void ResetIfIsMeasurementDC(IntPtr hdc)
		{
			WindowsGraphics currentMeasurementGraphics = WindowsGraphicsCacheManager.GetCurrentMeasurementGraphics();
			if (currentMeasurementGraphics != null && currentMeasurementGraphics.DeviceContext != null && currentMeasurementGraphics.DeviceContext.Hdc == hdc)
			{
				MeasurementDCInfo.CachedInfo cachedInfo = MeasurementDCInfo.cachedMeasurementDCInfo;
				if (cachedInfo != null)
				{
					cachedInfo.UpdateFont(null);
				}
			}
		}

		// Token: 0x06005330 RID: 21296 RVA: 0x0015CFCC File Offset: 0x0015B1CC
		internal static void Reset()
		{
			MeasurementDCInfo.CachedInfo cachedInfo = MeasurementDCInfo.cachedMeasurementDCInfo;
			if (cachedInfo != null)
			{
				cachedInfo.UpdateFont(null);
			}
		}

		// Token: 0x040035CC RID: 13772
		[ThreadStatic]
		private static MeasurementDCInfo.CachedInfo cachedMeasurementDCInfo;

		// Token: 0x0200086C RID: 2156
		private sealed class CachedInfo
		{
			// Token: 0x06007044 RID: 28740 RVA: 0x0019B2DC File Offset: 0x001994DC
			internal void UpdateFont(WindowsFont font)
			{
				if (this.LastUsedFont != font)
				{
					this.LastUsedFont = font;
					this.LeftTextMargin = -1;
					this.RightTextMargin = -1;
				}
			}

			// Token: 0x040043B0 RID: 17328
			public WindowsFont LastUsedFont;

			// Token: 0x040043B1 RID: 17329
			public int LeftTextMargin;

			// Token: 0x040043B2 RID: 17330
			public int RightTextMargin;
		}
	}
}
