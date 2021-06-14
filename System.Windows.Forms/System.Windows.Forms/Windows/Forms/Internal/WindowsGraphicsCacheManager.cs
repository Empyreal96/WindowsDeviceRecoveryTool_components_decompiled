using System;
using System.Collections.Generic;
using System.Drawing;

namespace System.Windows.Forms.Internal
{
	// Token: 0x020004FA RID: 1274
	internal class WindowsGraphicsCacheManager
	{
		// Token: 0x060053CF RID: 21455 RVA: 0x000027DB File Offset: 0x000009DB
		private WindowsGraphicsCacheManager()
		{
		}

		// Token: 0x17001441 RID: 5185
		// (get) Token: 0x060053D0 RID: 21456 RVA: 0x0015DFBE File Offset: 0x0015C1BE
		private static List<KeyValuePair<Font, WindowsFont>> WindowsFontCache
		{
			get
			{
				if (WindowsGraphicsCacheManager.windowsFontCache == null)
				{
					WindowsGraphicsCacheManager.currentIndex = -1;
					WindowsGraphicsCacheManager.windowsFontCache = new List<KeyValuePair<Font, WindowsFont>>(10);
				}
				return WindowsGraphicsCacheManager.windowsFontCache;
			}
		}

		// Token: 0x17001442 RID: 5186
		// (get) Token: 0x060053D1 RID: 21457 RVA: 0x0015DFDE File Offset: 0x0015C1DE
		public static WindowsGraphics MeasurementGraphics
		{
			get
			{
				if (WindowsGraphicsCacheManager.measurementGraphics == null || WindowsGraphicsCacheManager.measurementGraphics.DeviceContext == null)
				{
					WindowsGraphicsCacheManager.measurementGraphics = WindowsGraphics.CreateMeasurementWindowsGraphics();
				}
				return WindowsGraphicsCacheManager.measurementGraphics;
			}
		}

		// Token: 0x060053D2 RID: 21458 RVA: 0x0015E002 File Offset: 0x0015C202
		internal static WindowsGraphics GetCurrentMeasurementGraphics()
		{
			return WindowsGraphicsCacheManager.measurementGraphics;
		}

		// Token: 0x060053D3 RID: 21459 RVA: 0x0015E009 File Offset: 0x0015C209
		public static WindowsFont GetWindowsFont(Font font)
		{
			return WindowsGraphicsCacheManager.GetWindowsFont(font, WindowsFontQuality.Default);
		}

		// Token: 0x060053D4 RID: 21460 RVA: 0x0015E014 File Offset: 0x0015C214
		public static WindowsFont GetWindowsFont(Font font, WindowsFontQuality fontQuality)
		{
			if (font == null)
			{
				return null;
			}
			int i = 0;
			int num = WindowsGraphicsCacheManager.currentIndex;
			while (i < WindowsGraphicsCacheManager.WindowsFontCache.Count)
			{
				if (WindowsGraphicsCacheManager.WindowsFontCache[num].Key.Equals(font))
				{
					WindowsFont value = WindowsGraphicsCacheManager.WindowsFontCache[num].Value;
					if (value.Quality == fontQuality)
					{
						return value;
					}
				}
				num--;
				i++;
				if (num < 0)
				{
					num = 9;
				}
			}
			WindowsFont windowsFont = WindowsFont.FromFont(font, fontQuality);
			KeyValuePair<Font, WindowsFont> keyValuePair = new KeyValuePair<Font, WindowsFont>(font, windowsFont);
			WindowsGraphicsCacheManager.currentIndex++;
			if (WindowsGraphicsCacheManager.currentIndex == 10)
			{
				WindowsGraphicsCacheManager.currentIndex = 0;
			}
			if (WindowsGraphicsCacheManager.WindowsFontCache.Count == 10)
			{
				WindowsFont windowsFont2 = null;
				bool flag = false;
				int num2 = WindowsGraphicsCacheManager.currentIndex;
				int num3 = num2 + 1;
				while (!flag)
				{
					if (num3 >= 10)
					{
						num3 = 0;
					}
					if (num3 == num2)
					{
						flag = true;
					}
					windowsFont2 = WindowsGraphicsCacheManager.WindowsFontCache[num3].Value;
					if (!DeviceContexts.IsFontInUse(windowsFont2))
					{
						WindowsGraphicsCacheManager.currentIndex = num3;
						break;
					}
					num3++;
					windowsFont2 = null;
				}
				if (windowsFont2 != null)
				{
					WindowsGraphicsCacheManager.WindowsFontCache[WindowsGraphicsCacheManager.currentIndex] = keyValuePair;
					windowsFont.OwnedByCacheManager = true;
					windowsFont2.OwnedByCacheManager = false;
					windowsFont2.Dispose();
				}
				else
				{
					windowsFont.OwnedByCacheManager = false;
				}
			}
			else
			{
				windowsFont.OwnedByCacheManager = true;
				WindowsGraphicsCacheManager.WindowsFontCache.Add(keyValuePair);
			}
			return windowsFont;
		}

		// Token: 0x04003602 RID: 13826
		[ThreadStatic]
		private static WindowsGraphics measurementGraphics;

		// Token: 0x04003603 RID: 13827
		private const int CacheSize = 10;

		// Token: 0x04003604 RID: 13828
		[ThreadStatic]
		private static int currentIndex;

		// Token: 0x04003605 RID: 13829
		[ThreadStatic]
		private static List<KeyValuePair<Font, WindowsFont>> windowsFontCache;
	}
}
