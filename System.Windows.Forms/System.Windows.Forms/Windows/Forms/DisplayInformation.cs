using System;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x0200021E RID: 542
	internal class DisplayInformation
	{
		// Token: 0x06002111 RID: 8465 RVA: 0x000A3D8A File Offset: 0x000A1F8A
		static DisplayInformation()
		{
			SystemEvents.UserPreferenceChanging += DisplayInformation.UserPreferenceChanging;
			SystemEvents.DisplaySettingsChanging += DisplayInformation.DisplaySettingsChanging;
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06002112 RID: 8466 RVA: 0x000A3DAE File Offset: 0x000A1FAE
		public static short BitsPerPixel
		{
			get
			{
				if (DisplayInformation.bitsPerPixel == 0)
				{
					DisplayInformation.bitsPerPixel = (short)Screen.PrimaryScreen.BitsPerPixel;
				}
				return DisplayInformation.bitsPerPixel;
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06002113 RID: 8467 RVA: 0x000A3DCC File Offset: 0x000A1FCC
		public static bool LowResolution
		{
			get
			{
				if (DisplayInformation.lowResSettingValid && !DisplayInformation.lowRes)
				{
					return DisplayInformation.lowRes;
				}
				DisplayInformation.lowRes = (DisplayInformation.BitsPerPixel <= 8);
				DisplayInformation.lowResSettingValid = true;
				return DisplayInformation.lowRes;
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06002114 RID: 8468 RVA: 0x000A3DFD File Offset: 0x000A1FFD
		public static bool HighContrast
		{
			get
			{
				if (DisplayInformation.highContrastSettingValid)
				{
					return DisplayInformation.highContrast;
				}
				DisplayInformation.highContrast = SystemInformation.HighContrast;
				DisplayInformation.highContrastSettingValid = true;
				return DisplayInformation.highContrast;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06002115 RID: 8469 RVA: 0x000A3E21 File Offset: 0x000A2021
		public static bool IsDropShadowEnabled
		{
			get
			{
				if (DisplayInformation.dropShadowSettingValid)
				{
					return DisplayInformation.dropShadowEnabled;
				}
				DisplayInformation.dropShadowEnabled = SystemInformation.IsDropShadowEnabled;
				DisplayInformation.dropShadowSettingValid = true;
				return DisplayInformation.dropShadowEnabled;
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06002116 RID: 8470 RVA: 0x000A3E45 File Offset: 0x000A2045
		public static bool TerminalServer
		{
			get
			{
				if (DisplayInformation.terminalSettingValid)
				{
					return DisplayInformation.isTerminalServerSession;
				}
				DisplayInformation.isTerminalServerSession = SystemInformation.TerminalServerSession;
				DisplayInformation.terminalSettingValid = true;
				return DisplayInformation.isTerminalServerSession;
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06002117 RID: 8471 RVA: 0x000A3E69 File Offset: 0x000A2069
		public static bool MenuAccessKeysUnderlined
		{
			get
			{
				if (DisplayInformation.menuAccessKeysUnderlinedValid)
				{
					return DisplayInformation.menuAccessKeysUnderlined;
				}
				DisplayInformation.menuAccessKeysUnderlined = SystemInformation.MenuAccessKeysUnderlined;
				DisplayInformation.menuAccessKeysUnderlinedValid = true;
				return DisplayInformation.menuAccessKeysUnderlined;
			}
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x000A3E8D File Offset: 0x000A208D
		private static void DisplaySettingsChanging(object obj, EventArgs ea)
		{
			DisplayInformation.highContrastSettingValid = false;
			DisplayInformation.lowResSettingValid = false;
			DisplayInformation.terminalSettingValid = false;
			DisplayInformation.dropShadowSettingValid = false;
			DisplayInformation.menuAccessKeysUnderlinedValid = false;
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x000A3EAD File Offset: 0x000A20AD
		private static void UserPreferenceChanging(object obj, UserPreferenceChangingEventArgs e)
		{
			DisplayInformation.highContrastSettingValid = false;
			DisplayInformation.lowResSettingValid = false;
			DisplayInformation.terminalSettingValid = false;
			DisplayInformation.dropShadowSettingValid = false;
			DisplayInformation.bitsPerPixel = 0;
			if (e.Category == UserPreferenceCategory.General)
			{
				DisplayInformation.menuAccessKeysUnderlinedValid = false;
			}
		}

		// Token: 0x04000E37 RID: 3639
		private static bool highContrast;

		// Token: 0x04000E38 RID: 3640
		private static bool lowRes;

		// Token: 0x04000E39 RID: 3641
		private static bool isTerminalServerSession;

		// Token: 0x04000E3A RID: 3642
		private static bool highContrastSettingValid;

		// Token: 0x04000E3B RID: 3643
		private static bool lowResSettingValid;

		// Token: 0x04000E3C RID: 3644
		private static bool terminalSettingValid;

		// Token: 0x04000E3D RID: 3645
		private static short bitsPerPixel;

		// Token: 0x04000E3E RID: 3646
		private static bool dropShadowSettingValid;

		// Token: 0x04000E3F RID: 3647
		private static bool dropShadowEnabled;

		// Token: 0x04000E40 RID: 3648
		private static bool menuAccessKeysUnderlinedValid;

		// Token: 0x04000E41 RID: 3649
		private static bool menuAccessKeysUnderlined;
	}
}
