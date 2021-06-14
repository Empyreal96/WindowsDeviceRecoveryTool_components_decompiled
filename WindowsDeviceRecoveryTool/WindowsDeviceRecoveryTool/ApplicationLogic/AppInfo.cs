using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic
{
	// Token: 0x02000005 RID: 5
	public static class AppInfo
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002F64 File Offset: 0x00001164
		public static int ApplicationId
		{
			get
			{
				return ApplicationBuildSettings.ApplicationId;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002F7C File Offset: 0x0000117C
		public static string ApplicationName
		{
			get
			{
				if (string.IsNullOrEmpty(AppInfo.applicationName))
				{
					AppInfo.applicationName = AppInfo.GetAppName();
				}
				return AppInfo.applicationName;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002FB0 File Offset: 0x000011B0
		public static string Version
		{
			get
			{
				if (string.IsNullOrEmpty(AppInfo.version))
				{
					AppInfo.version = AppInfo.GetVersion();
				}
				return AppInfo.version;
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002FE4 File Offset: 0x000011E4
		public static string AppName()
		{
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			string result;
			if (entryAssembly != null)
			{
				result = entryAssembly.GetName().Name;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003020 File Offset: 0x00001220
		public static string AppTitle()
		{
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			object[] customAttributes = entryAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
			string result;
			if (customAttributes.Length == 1)
			{
				result = ((AssemblyTitleAttribute)customAttributes[0]).Title;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003070 File Offset: 0x00001270
		public static string AppVersion()
		{
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			string result;
			if (entryAssembly != null)
			{
				string text = entryAssembly.GetName().Version.Major.ToString(CultureInfo.InvariantCulture);
				string text2 = entryAssembly.GetName().Version.Minor.ToString(CultureInfo.InvariantCulture);
				string text3 = entryAssembly.GetName().Version.Build.ToString(CultureInfo.InvariantCulture);
				result = string.Concat(new object[]
				{
					text,
					'.',
					text2,
					'.',
					text3
				});
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003138 File Offset: 0x00001338
		public static string Copyrights()
		{
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			if (entryAssembly != null)
			{
				object[] customAttributes = entryAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (customAttributes.Length > 0)
				{
					return ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
				}
			}
			return string.Empty;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003198 File Offset: 0x00001398
		public static string AppAssemblyFilePath()
		{
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			string result;
			if (entryAssembly != null)
			{
				result = new Uri(entryAssembly.CodeBase, UriKind.Absolute).LocalPath;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000031D8 File Offset: 0x000013D8
		public static bool IsAnotherInstanceRunning()
		{
			string value = Assembly.GetExecutingAssembly().GetCustomAttributes(false).OfType<GuidAttribute>().First<GuidAttribute>().Value;
			bool flag;
			AppInfo.mutex = new Mutex(false, value, ref flag);
			return !flag;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003220 File Offset: 0x00001420
		private static string GetVersion()
		{
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			string result;
			if (entryAssembly != null)
			{
				Version version = entryAssembly.GetName().Version;
				string text = version.Major.ToString(CultureInfo.InvariantCulture);
				string text2 = version.Minor.ToString(CultureInfo.InvariantCulture);
				string text3 = version.Build.ToString(CultureInfo.InvariantCulture);
				result = string.Concat(new object[]
				{
					text,
					'.',
					text2,
					'.',
					text3
				});
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000032D8 File Offset: 0x000014D8
		private static string GetAppName()
		{
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			string result;
			if (entryAssembly != null)
			{
				result = entryAssembly.GetName().Name;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x04000015 RID: 21
		private static string version;

		// Token: 0x04000016 RID: 22
		private static string applicationName;

		// Token: 0x04000017 RID: 23
		private static Mutex mutex;
	}
}
