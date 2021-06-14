using System;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x020000F5 RID: 245
	[ConfigurationPermission(SecurityAction.Assert, Unrestricted = true)]
	internal static class PrivilegedConfigurationManager
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0000BE78 File Offset: 0x0000A078
		internal static ConnectionStringSettingsCollection ConnectionStrings
		{
			get
			{
				return ConfigurationManager.ConnectionStrings;
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000BE7F File Offset: 0x0000A07F
		internal static object GetSection(string sectionName)
		{
			return ConfigurationManager.GetSection(sectionName);
		}
	}
}
