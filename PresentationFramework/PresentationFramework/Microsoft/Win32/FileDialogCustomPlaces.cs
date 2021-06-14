using System;

namespace Microsoft.Win32
{
	/// <summary>Defines the known folders for custom places in file dialog boxes.</summary>
	// Token: 0x02000091 RID: 145
	public static class FileDialogCustomPlaces
	{
		/// <summary>Gets the folder for application-specific data for the current roaming user.</summary>
		/// <returns>The folder for application-specific data for the current roaming user.</returns>
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000228 RID: 552 RVA: 0x00005892 File Offset: 0x00003A92
		public static FileDialogCustomPlace RoamingApplicationData
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("3EB685DB-65F9-4CF6-A03A-E3EF65729F3D"));
			}
		}

		/// <summary>Gets the folder for application-specific data that is used by the current, non-roaming user.</summary>
		/// <returns>The folder for application-specific data that is used by the current, non-roaming user.</returns>
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000229 RID: 553 RVA: 0x000058A3 File Offset: 0x00003AA3
		public static FileDialogCustomPlace LocalApplicationData
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("F1B32785-6FBA-4FCF-9D55-7B8E7F157091"));
			}
		}

		/// <summary>Gets the Internet cookies folder for the current user.</summary>
		/// <returns>The Internet cookies folder for the current user.</returns>
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600022A RID: 554 RVA: 0x000058B4 File Offset: 0x00003AB4
		public static FileDialogCustomPlace Cookies
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("2B0F765D-C0E9-4171-908E-08A611B84FF6"));
			}
		}

		/// <summary>Gets the Contacts folder for the current user.</summary>
		/// <returns>The Contacts folder for the current user.</returns>
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600022B RID: 555 RVA: 0x000058C5 File Offset: 0x00003AC5
		public static FileDialogCustomPlace Contacts
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("56784854-C6CB-462b-8169-88E350ACB882"));
			}
		}

		/// <summary>Gets the Favorites folder for the current user.</summary>
		/// <returns>The  Favorites folder for the current user.</returns>
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600022C RID: 556 RVA: 0x000058D6 File Offset: 0x00003AD6
		public static FileDialogCustomPlace Favorites
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("1777F761-68AD-4D8A-87BD-30B759FA33DD"));
			}
		}

		/// <summary>Gets the folder that contains the program groups for the current user.</summary>
		/// <returns>The folder that contains the program groups for the current user.</returns>
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600022D RID: 557 RVA: 0x000058E7 File Offset: 0x00003AE7
		public static FileDialogCustomPlace Programs
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("A77F5D77-2E2B-44C3-A6A2-ABA601054A51"));
			}
		}

		/// <summary>Gets the Music folder for the current user.</summary>
		/// <returns>The Music folder for the current user.</returns>
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600022E RID: 558 RVA: 0x000058F8 File Offset: 0x00003AF8
		public static FileDialogCustomPlace Music
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("4BD8D571-6D19-48D3-BE97-422220080E43"));
			}
		}

		/// <summary>Gets the Pictures folder for the current user.</summary>
		/// <returns>The Pictures folder for the current user.</returns>
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00005909 File Offset: 0x00003B09
		public static FileDialogCustomPlace Pictures
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("33E28130-4E1E-4676-835A-98395C3BC3BB"));
			}
		}

		/// <summary>Gets the folder that contains the Send To menu items for the current user.</summary>
		/// <returns>The folder that contains the Send To menu items for the current user.</returns>
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000591A File Offset: 0x00003B1A
		public static FileDialogCustomPlace SendTo
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("8983036C-27C0-404B-8F08-102D10DCFD74"));
			}
		}

		/// <summary>Gets the folder that contains the Start menu items for the current user.</summary>
		/// <returns>The folder that contains the Start menu items for the current user.</returns>
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000592B File Offset: 0x00003B2B
		public static FileDialogCustomPlace StartMenu
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("625B53C3-AB48-4EC1-BA1F-A1EF4146FC19"));
			}
		}

		/// <summary>Gets the folder that corresponds to the Startup program group for the current user.</summary>
		/// <returns>The folder that corresponds to the Startup program group for the current user.</returns>
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000593C File Offset: 0x00003B3C
		public static FileDialogCustomPlace Startup
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("B97D20BB-F46A-4C97-BA10-5E3608430854"));
			}
		}

		/// <summary>Gets the System folder.</summary>
		/// <returns>The System folder.</returns>
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0000594D File Offset: 0x00003B4D
		public static FileDialogCustomPlace System
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("1AC14E77-02E7-4E5D-B744-2EB1AE5198B7"));
			}
		}

		/// <summary>Gets the folder for document templates for the current user.</summary>
		/// <returns>The folder for document templates for the current user.</returns>
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000595E File Offset: 0x00003B5E
		public static FileDialogCustomPlace Templates
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("A63293E8-664E-48DB-A079-DF759E0509F7"));
			}
		}

		/// <summary>Gets the folder for storing files on the desktop for the current user.</summary>
		/// <returns>The folder for storing files on the desktop for the current user.</returns>
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000596F File Offset: 0x00003B6F
		public static FileDialogCustomPlace Desktop
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("B4BFCC3A-DB2C-424C-B029-7FE99A87C641"));
			}
		}

		/// <summary>Gets the Documents folder for the current user.</summary>
		/// <returns>The Documents folder for the current user.</returns>
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000236 RID: 566 RVA: 0x00005980 File Offset: 0x00003B80
		public static FileDialogCustomPlace Documents
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("FDD39AD0-238F-46AF-ADB4-6C85480369C7"));
			}
		}

		/// <summary>Gets the Program Files folder.</summary>
		/// <returns>The Program Files folder.</returns>
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00005991 File Offset: 0x00003B91
		public static FileDialogCustomPlace ProgramFiles
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("905E63B6-C1BF-494E-B29C-65B732D3D21A"));
			}
		}

		/// <summary>Gets the folder for components that are shared across applications.</summary>
		/// <returns>The folder for components that are shared across applications.</returns>
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000238 RID: 568 RVA: 0x000059A2 File Offset: 0x00003BA2
		public static FileDialogCustomPlace ProgramFilesCommon
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("F7F1ED05-9F6D-47A2-AAAE-29D317C6F066"));
			}
		}
	}
}
