using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Runtime.Versioning;

namespace System.Windows.Forms
{
	// Token: 0x020000FF RID: 255
	internal static class ConfigurationOptions
	{
		// Token: 0x0600040F RID: 1039 RVA: 0x0000CC94 File Offset: 0x0000AE94
		static ConfigurationOptions()
		{
			ConfigurationOptions.PopulateWinformsSection();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000CCE0 File Offset: 0x0000AEE0
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

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x0000CD28 File Offset: 0x0000AF28
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

		// Token: 0x06000412 RID: 1042 RVA: 0x0000CDA8 File Offset: 0x0000AFA8
		public static string GetConfigSettingValue(string settingName)
		{
			if (ConfigurationOptions.applicationConfigOptions != null && !string.IsNullOrEmpty(settingName))
			{
				return ConfigurationOptions.applicationConfigOptions.Get(settingName);
			}
			return null;
		}

		// Token: 0x04000430 RID: 1072
		private static NameValueCollection applicationConfigOptions = null;

		// Token: 0x04000431 RID: 1073
		private static Version netFrameworkVersion = null;

		// Token: 0x04000432 RID: 1074
		private static readonly Version featureSupportedMinimumFrameworkVersion = new Version(4, 7);

		// Token: 0x04000433 RID: 1075
		internal static Version OSVersion = Environment.OSVersion.Version;

		// Token: 0x04000434 RID: 1076
		internal static readonly Version RS2Version = new Version(10, 0, 14933, 0);
	}
}
