using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Runtime.Versioning;

namespace System.Windows.Forms
{
	// Token: 0x0200000A RID: 10
	internal static class ConfigurationOptions
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002458 File Offset: 0x00000658
		static ConfigurationOptions()
		{
			ConfigurationOptions.PopulateWinformsSection();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000024A4 File Offset: 0x000006A4
		private static void PopulateWinformsSection()
		{
			if (ConfigurationOptions.NetFrameworkVersion.CompareTo(ConfigurationOptions.featureSupportedMinimumFrameworkVersion) >= 0)
			{
				try
				{
					ConfigurationOptions.applicationConfigOptions = (ConfigurationManager.GetSection("System.Windows.Forms.ApplicationConfigurationSection") as NameValueCollection);
				}
				catch (Exception ex)
				{
				}
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000024EC File Offset: 0x000006EC
		public static Version NetFrameworkVersion
		{
			get
			{
				if (ConfigurationOptions.netFrameworkVersion == null)
				{
					ConfigurationOptions.netFrameworkVersion = new Version(0, 0, 0, 0);
					try
					{
						string targetFrameworkName = AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
						if (!string.IsNullOrEmpty(targetFrameworkName))
						{
							FrameworkName frameworkName = new FrameworkName(targetFrameworkName);
							if (string.Equals(frameworkName.Identifier, ".NETFramework"))
							{
								ConfigurationOptions.netFrameworkVersion = frameworkName.Version;
							}
						}
					}
					catch (Exception ex)
					{
					}
				}
				return ConfigurationOptions.netFrameworkVersion;
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000256C File Offset: 0x0000076C
		public static string GetConfigSettingValue(string settingName)
		{
			if (ConfigurationOptions.applicationConfigOptions != null && !string.IsNullOrEmpty(settingName))
			{
				return ConfigurationOptions.applicationConfigOptions.Get(settingName);
			}
			return null;
		}

		// Token: 0x0400008A RID: 138
		private static NameValueCollection applicationConfigOptions = null;

		// Token: 0x0400008B RID: 139
		private static Version netFrameworkVersion = null;

		// Token: 0x0400008C RID: 140
		private static readonly Version featureSupportedMinimumFrameworkVersion = new Version(4, 7);

		// Token: 0x0400008D RID: 141
		internal static Version OSVersion = Environment.OSVersion.Version;

		// Token: 0x0400008E RID: 142
		internal static readonly Version RS2Version = new Version(10, 0, 14933, 0);
	}
}
