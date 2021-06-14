using System;

namespace System.Drawing
{
	/// <summary>Each property of the <see cref="T:System.Drawing.SystemIcons" /> class is an <see cref="T:System.Drawing.Icon" /> object for Windows system-wide icons. This class cannot be inherited.</summary>
	// Token: 0x02000036 RID: 54
	public sealed class SystemIcons
	{
		// Token: 0x06000557 RID: 1367 RVA: 0x00003800 File Offset: 0x00001A00
		private SystemIcons()
		{
		}

		/// <summary>Gets an <see cref="T:System.Drawing.Icon" /> object that contains the default application icon (WIN32: IDI_APPLICATION).</summary>
		/// <returns>An <see cref="T:System.Drawing.Icon" /> object that contains the default application icon.</returns>
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x00017E82 File Offset: 0x00016082
		public static Icon Application
		{
			get
			{
				if (SystemIcons._application == null)
				{
					SystemIcons._application = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32512));
				}
				return SystemIcons._application;
			}
		}

		/// <summary>Gets an <see cref="T:System.Drawing.Icon" /> object that contains the system asterisk icon (WIN32: IDI_ASTERISK).</summary>
		/// <returns>An <see cref="T:System.Drawing.Icon" /> object that contains the system asterisk icon.</returns>
		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x00017EA9 File Offset: 0x000160A9
		public static Icon Asterisk
		{
			get
			{
				if (SystemIcons._asterisk == null)
				{
					SystemIcons._asterisk = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32516));
				}
				return SystemIcons._asterisk;
			}
		}

		/// <summary>Gets an <see cref="T:System.Drawing.Icon" /> object that contains the system error icon (WIN32: IDI_ERROR).</summary>
		/// <returns>An <see cref="T:System.Drawing.Icon" /> object that contains the system error icon.</returns>
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x00017ED0 File Offset: 0x000160D0
		public static Icon Error
		{
			get
			{
				if (SystemIcons._error == null)
				{
					SystemIcons._error = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32513));
				}
				return SystemIcons._error;
			}
		}

		/// <summary>Gets an <see cref="T:System.Drawing.Icon" /> object that contains the system exclamation icon (WIN32: IDI_EXCLAMATION).</summary>
		/// <returns>An <see cref="T:System.Drawing.Icon" /> object that contains the system exclamation icon.</returns>
		// Token: 0x17000263 RID: 611
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x00017EF7 File Offset: 0x000160F7
		public static Icon Exclamation
		{
			get
			{
				if (SystemIcons._exclamation == null)
				{
					SystemIcons._exclamation = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32515));
				}
				return SystemIcons._exclamation;
			}
		}

		/// <summary>Gets an <see cref="T:System.Drawing.Icon" /> object that contains the system hand icon (WIN32: IDI_HAND).</summary>
		/// <returns>An <see cref="T:System.Drawing.Icon" /> object that contains the system hand icon.</returns>
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x00017F1E File Offset: 0x0001611E
		public static Icon Hand
		{
			get
			{
				if (SystemIcons._hand == null)
				{
					SystemIcons._hand = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32513));
				}
				return SystemIcons._hand;
			}
		}

		/// <summary>Gets an <see cref="T:System.Drawing.Icon" /> object that contains the system information icon (WIN32: IDI_INFORMATION).</summary>
		/// <returns>An <see cref="T:System.Drawing.Icon" /> object that contains the system information icon.</returns>
		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x00017F45 File Offset: 0x00016145
		public static Icon Information
		{
			get
			{
				if (SystemIcons._information == null)
				{
					SystemIcons._information = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32516));
				}
				return SystemIcons._information;
			}
		}

		/// <summary>Gets an <see cref="T:System.Drawing.Icon" /> object that contains the system question icon (WIN32: IDI_QUESTION).</summary>
		/// <returns>An <see cref="T:System.Drawing.Icon" /> object that contains the system question icon.</returns>
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x00017F6C File Offset: 0x0001616C
		public static Icon Question
		{
			get
			{
				if (SystemIcons._question == null)
				{
					SystemIcons._question = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32514));
				}
				return SystemIcons._question;
			}
		}

		/// <summary>Gets an <see cref="T:System.Drawing.Icon" /> object that contains the system warning icon (WIN32: IDI_WARNING).</summary>
		/// <returns>An <see cref="T:System.Drawing.Icon" /> object that contains the system warning icon.</returns>
		// Token: 0x17000267 RID: 615
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x00017F93 File Offset: 0x00016193
		public static Icon Warning
		{
			get
			{
				if (SystemIcons._warning == null)
				{
					SystemIcons._warning = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32515));
				}
				return SystemIcons._warning;
			}
		}

		/// <summary>Gets an <see cref="T:System.Drawing.Icon" /> object that contains the Windows logo icon (WIN32: IDI_WINLOGO).</summary>
		/// <returns>An <see cref="T:System.Drawing.Icon" /> object that contains the Windows logo icon.</returns>
		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x00017FBA File Offset: 0x000161BA
		public static Icon WinLogo
		{
			get
			{
				if (SystemIcons._winlogo == null)
				{
					SystemIcons._winlogo = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32517));
				}
				return SystemIcons._winlogo;
			}
		}

		/// <summary>Gets an <see cref="T:System.Drawing.Icon" /> object that contains the shield icon.</summary>
		/// <returns>An <see cref="T:System.Drawing.Icon" /> object that contains the shield icon.</returns>
		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x00017FE4 File Offset: 0x000161E4
		public static Icon Shield
		{
			get
			{
				if (SystemIcons._shield == null)
				{
					try
					{
						if (Environment.OSVersion.Version.Major >= 6)
						{
							IntPtr zero = IntPtr.Zero;
							if (SafeNativeMethods.LoadIconWithScaleDown(NativeMethods.NullHandleRef, 32518, 32, 32, ref zero) == 0)
							{
								SystemIcons._shield = new Icon(zero);
							}
						}
					}
					catch
					{
					}
				}
				if (SystemIcons._shield == null)
				{
					SystemIcons._shield = new Icon(typeof(SystemIcons), "ShieldIcon.ico");
				}
				return SystemIcons._shield;
			}
		}

		// Token: 0x0400031C RID: 796
		private static Icon _application;

		// Token: 0x0400031D RID: 797
		private static Icon _asterisk;

		// Token: 0x0400031E RID: 798
		private static Icon _error;

		// Token: 0x0400031F RID: 799
		private static Icon _exclamation;

		// Token: 0x04000320 RID: 800
		private static Icon _hand;

		// Token: 0x04000321 RID: 801
		private static Icon _information;

		// Token: 0x04000322 RID: 802
		private static Icon _question;

		// Token: 0x04000323 RID: 803
		private static Icon _warning;

		// Token: 0x04000324 RID: 804
		private static Icon _winlogo;

		// Token: 0x04000325 RID: 805
		private static Icon _shield;
	}
}
