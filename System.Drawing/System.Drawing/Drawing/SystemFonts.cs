using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Drawing
{
	/// <summary>Specifies the fonts used to display text in Windows display elements.</summary>
	// Token: 0x02000035 RID: 53
	public sealed class SystemFonts
	{
		// Token: 0x0600054A RID: 1354 RVA: 0x00003800 File Offset: 0x00001A00
		private SystemFonts()
		{
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Font" /> that is used to display text in the title bars of windows.</summary>
		/// <returns>A <see cref="T:System.Drawing.Font" /> that is used to display text in the title bars of windows.</returns>
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x000177CC File Offset: 0x000159CC
		public static Font CaptionFont
		{
			get
			{
				Font font = null;
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.lfCaptionFont != null)
				{
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						font = Font.FromLogFont(nonclientmetrics.lfCaptionFont);
					}
					catch (Exception ex)
					{
						if (SystemFonts.IsCriticalFontException(ex))
						{
							throw;
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (font == null)
					{
						font = SystemFonts.DefaultFont;
					}
					else if (font.Unit != GraphicsUnit.Point)
					{
						font = SystemFonts.FontInPoints(font);
					}
				}
				font.SetSystemFontName("CaptionFont");
				return font;
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Font" /> that is used to display text in the title bars of small windows, such as tool windows.</summary>
		/// <returns>A <see cref="T:System.Drawing.Font" /> that is used to display text in the title bars of small windows, such as tool windows.</returns>
		// Token: 0x17000259 RID: 601
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x0001786C File Offset: 0x00015A6C
		public static Font SmallCaptionFont
		{
			get
			{
				Font font = null;
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.lfSmCaptionFont != null)
				{
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						font = Font.FromLogFont(nonclientmetrics.lfSmCaptionFont);
					}
					catch (Exception ex)
					{
						if (SystemFonts.IsCriticalFontException(ex))
						{
							throw;
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (font == null)
					{
						font = SystemFonts.DefaultFont;
					}
					else if (font.Unit != GraphicsUnit.Point)
					{
						font = SystemFonts.FontInPoints(font);
					}
				}
				font.SetSystemFontName("SmallCaptionFont");
				return font;
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Font" /> that is used for menus.</summary>
		/// <returns>A <see cref="T:System.Drawing.Font" /> that is used for menus.</returns>
		// Token: 0x1700025A RID: 602
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x0001790C File Offset: 0x00015B0C
		public static Font MenuFont
		{
			get
			{
				Font font = null;
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.lfMenuFont != null)
				{
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						font = Font.FromLogFont(nonclientmetrics.lfMenuFont);
					}
					catch (Exception ex)
					{
						if (SystemFonts.IsCriticalFontException(ex))
						{
							throw;
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (font == null)
					{
						font = SystemFonts.DefaultFont;
					}
					else if (font.Unit != GraphicsUnit.Point)
					{
						font = SystemFonts.FontInPoints(font);
					}
				}
				font.SetSystemFontName("MenuFont");
				return font;
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Font" /> that is used to display text in the status bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Font" /> that is used to display text in the status bar.</returns>
		// Token: 0x1700025B RID: 603
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x000179AC File Offset: 0x00015BAC
		public static Font StatusFont
		{
			get
			{
				Font font = null;
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.lfStatusFont != null)
				{
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						font = Font.FromLogFont(nonclientmetrics.lfStatusFont);
					}
					catch (Exception ex)
					{
						if (SystemFonts.IsCriticalFontException(ex))
						{
							throw;
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (font == null)
					{
						font = SystemFonts.DefaultFont;
					}
					else if (font.Unit != GraphicsUnit.Point)
					{
						font = SystemFonts.FontInPoints(font);
					}
				}
				font.SetSystemFontName("StatusFont");
				return font;
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Font" /> that is used for message boxes.</summary>
		/// <returns>A <see cref="T:System.Drawing.Font" /> that is used for message boxes</returns>
		// Token: 0x1700025C RID: 604
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x00017A4C File Offset: 0x00015C4C
		public static Font MessageBoxFont
		{
			get
			{
				Font font = null;
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.lfMessageFont != null)
				{
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						font = Font.FromLogFont(nonclientmetrics.lfMessageFont);
					}
					catch (Exception ex)
					{
						if (SystemFonts.IsCriticalFontException(ex))
						{
							throw;
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (font == null)
					{
						font = SystemFonts.DefaultFont;
					}
					else if (font.Unit != GraphicsUnit.Point)
					{
						font = SystemFonts.FontInPoints(font);
					}
				}
				font.SetSystemFontName("MessageBoxFont");
				return font;
			}
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00017AEC File Offset: 0x00015CEC
		private static bool IsCriticalFontException(Exception ex)
		{
			return !(ex is ExternalException) && !(ex is ArgumentException) && !(ex is OutOfMemoryException) && !(ex is InvalidOperationException) && !(ex is NotImplementedException) && !(ex is FileNotFoundException);
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Font" /> that is used for icon titles.</summary>
		/// <returns>A <see cref="T:System.Drawing.Font" /> that is used for icon titles.</returns>
		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x00017B24 File Offset: 0x00015D24
		public static Font IconTitleFont
		{
			get
			{
				Font font = null;
				SafeNativeMethods.LOGFONT logfont = new SafeNativeMethods.LOGFONT();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(31, Marshal.SizeOf(logfont), logfont, 0);
				if (flag && logfont != null)
				{
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						font = Font.FromLogFont(logfont);
					}
					catch (Exception ex)
					{
						if (SystemFonts.IsCriticalFontException(ex))
						{
							throw;
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (font == null)
					{
						font = SystemFonts.DefaultFont;
					}
					else if (font.Unit != GraphicsUnit.Point)
					{
						font = SystemFonts.FontInPoints(font);
					}
				}
				font.SetSystemFontName("IconTitleFont");
				return font;
			}
		}

		/// <summary>Gets the default font that applications can use for dialog boxes and forms.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Font" /> of the system. The value returned will vary depending on the user's operating system and the local culture setting of their system.</returns>
		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x00017BB8 File Offset: 0x00015DB8
		public static Font DefaultFont
		{
			get
			{
				Font font = null;
				bool flag = false;
				if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major <= 4 && (UnsafeNativeMethods.GetSystemDefaultLCID() & 1023) == 17)
				{
					try
					{
						font = new Font("MS UI Gothic", 9f);
					}
					catch (Exception ex)
					{
						if (SystemFonts.IsCriticalFontException(ex))
						{
							throw;
						}
					}
				}
				if (font == null)
				{
					flag = ((UnsafeNativeMethods.GetSystemDefaultLCID() & 1023) == 1);
				}
				if (flag)
				{
					try
					{
						font = new Font("Tahoma", 8f);
					}
					catch (Exception ex2)
					{
						if (SystemFonts.IsCriticalFontException(ex2))
						{
							throw;
						}
					}
				}
				if (font == null)
				{
					IntPtr stockObject = UnsafeNativeMethods.GetStockObject(17);
					try
					{
						Font font2 = null;
						IntSecurity.ObjectFromWin32Handle.Assert();
						try
						{
							font2 = Font.FromHfont(stockObject);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
						try
						{
							font = SystemFonts.FontInPoints(font2);
						}
						finally
						{
							font2.Dispose();
						}
					}
					catch (ArgumentException)
					{
					}
				}
				if (font == null)
				{
					try
					{
						font = new Font("Tahoma", 8f);
					}
					catch (ArgumentException)
					{
					}
				}
				if (font == null)
				{
					font = new Font(FontFamily.GenericSansSerif, 8f);
				}
				if (font.Unit != GraphicsUnit.Point)
				{
					font = SystemFonts.FontInPoints(font);
				}
				font.SetSystemFontName("DefaultFont");
				return font;
			}
		}

		/// <summary>Gets a font that applications can use for dialog boxes and forms.</summary>
		/// <returns>A <see cref="T:System.Drawing.Font" /> that can be used for dialog boxes and forms, depending on the operating system and local culture setting of the system.</returns>
		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x00017D20 File Offset: 0x00015F20
		public static Font DialogFont
		{
			get
			{
				Font font = null;
				if ((UnsafeNativeMethods.GetSystemDefaultLCID() & 1023) == 17)
				{
					font = SystemFonts.DefaultFont;
				}
				else if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
				{
					font = SystemFonts.DefaultFont;
				}
				else
				{
					try
					{
						font = new Font("MS Shell Dlg 2", 8f);
					}
					catch (ArgumentException)
					{
					}
				}
				if (font == null)
				{
					font = SystemFonts.DefaultFont;
				}
				else if (font.Unit != GraphicsUnit.Point)
				{
					font = SystemFonts.FontInPoints(font);
				}
				font.SetSystemFontName("DialogFont");
				return font;
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00017DA8 File Offset: 0x00015FA8
		private static Font FontInPoints(Font font)
		{
			return new Font(font.FontFamily, font.SizeInPoints, font.Style, GraphicsUnit.Point, font.GdiCharSet, font.GdiVerticalFont);
		}

		/// <summary>Returns a font object that corresponds to the specified system font name.</summary>
		/// <param name="systemFontName">The name of the system font you need a font object for.</param>
		/// <returns>A <see cref="T:System.Drawing.Font" /> if the specified name matches a value in <see cref="T:System.Drawing.SystemFonts" />; otherwise, <see langword="null" />.</returns>
		// Token: 0x06000555 RID: 1365 RVA: 0x00017DD0 File Offset: 0x00015FD0
		public static Font GetFontByName(string systemFontName)
		{
			if ("CaptionFont".Equals(systemFontName))
			{
				return SystemFonts.CaptionFont;
			}
			if ("DefaultFont".Equals(systemFontName))
			{
				return SystemFonts.DefaultFont;
			}
			if ("DialogFont".Equals(systemFontName))
			{
				return SystemFonts.DialogFont;
			}
			if ("IconTitleFont".Equals(systemFontName))
			{
				return SystemFonts.IconTitleFont;
			}
			if ("MenuFont".Equals(systemFontName))
			{
				return SystemFonts.MenuFont;
			}
			if ("MessageBoxFont".Equals(systemFontName))
			{
				return SystemFonts.MessageBoxFont;
			}
			if ("SmallCaptionFont".Equals(systemFontName))
			{
				return SystemFonts.SmallCaptionFont;
			}
			if ("StatusFont".Equals(systemFontName))
			{
				return SystemFonts.StatusFont;
			}
			return null;
		}

		// Token: 0x0400031B RID: 795
		private static readonly object SystemFontsKey = new object();
	}
}
