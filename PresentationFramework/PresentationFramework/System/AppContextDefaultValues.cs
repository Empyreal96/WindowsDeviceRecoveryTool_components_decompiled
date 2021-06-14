using System;

namespace System
{
	// Token: 0x02000094 RID: 148
	internal static class AppContextDefaultValues
	{
		// Token: 0x06000256 RID: 598 RVA: 0x00005EB0 File Offset: 0x000040B0
		public static void PopulateDefaultValues()
		{
			string platformIdentifier;
			string profile;
			int version;
			AppContextDefaultValues.ParseTargetFrameworkName(out platformIdentifier, out profile, out version);
			AppContextDefaultValues.PopulateDefaultValuesPartial(platformIdentifier, profile, version);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00005ED0 File Offset: 0x000040D0
		private static void ParseTargetFrameworkName(out string identifier, out string profile, out int version)
		{
			string targetFrameworkName = AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
			if (!AppContextDefaultValues.TryParseFrameworkName(targetFrameworkName, out identifier, out version, out profile))
			{
				identifier = ".NETFramework";
				version = 40000;
				profile = string.Empty;
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00005F10 File Offset: 0x00004110
		private static bool TryParseFrameworkName(string frameworkName, out string identifier, out int version, out string profile)
		{
			string empty;
			profile = (empty = string.Empty);
			identifier = empty;
			version = 0;
			if (frameworkName == null || frameworkName.Length == 0)
			{
				return false;
			}
			string[] array = frameworkName.Split(new char[]
			{
				','
			});
			version = 0;
			if (array.Length < 2 || array.Length > 3)
			{
				return false;
			}
			identifier = array[0].Trim();
			if (identifier.Length == 0)
			{
				return false;
			}
			bool result = false;
			profile = null;
			for (int i = 1; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					'='
				});
				if (array2.Length != 2)
				{
					return false;
				}
				string text = array2[0].Trim();
				string text2 = array2[1].Trim();
				if (text.Equals("Version", StringComparison.OrdinalIgnoreCase))
				{
					result = true;
					if (text2.Length > 0 && (text2[0] == 'v' || text2[0] == 'V'))
					{
						text2 = text2.Substring(1);
					}
					Version version2 = new Version(text2);
					version = version2.Major * 10000;
					if (version2.Minor > 0)
					{
						version += version2.Minor * 100;
					}
					if (version2.Build > 0)
					{
						version += version2.Build;
					}
				}
				else
				{
					if (!text.Equals("Profile", StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
					if (!string.IsNullOrEmpty(text2))
					{
						profile = text2;
					}
				}
			}
			return result;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000606C File Offset: 0x0000426C
		private static void PopulateDefaultValuesPartial(string platformIdentifier, string profile, int version)
		{
			if (platformIdentifier == ".NETFramework")
			{
				if (version <= 40502)
				{
					LocalAppContext.DefineSwitchDefault("Switch.MS.Internal.DoNotApplyLayoutRoundingToMarginsAndBorderThickness", true);
				}
				if (version <= 40602)
				{
					LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Controls.Grid.StarDefinitionsCanExceedAvailableSpace", true);
				}
				if (version <= 40700)
				{
					LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Controls.TabControl.SelectionPropertiesCanLagBehindSelectionChangedEvent", true);
				}
				if (version <= 40701)
				{
					LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Data.DoNotUseFollowParentWhenBindingToADODataRelation", true);
				}
				if (40000 <= version && version <= 40702)
				{
					LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Data.Binding.IListIndexerHidesCustomIndexer", true);
				}
				if (version <= 40800)
				{
					LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Controls.SelectorUpdatesSelectionPropertiesWhenDisconnected", true);
				}
				LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", true);
			}
		}
	}
}
