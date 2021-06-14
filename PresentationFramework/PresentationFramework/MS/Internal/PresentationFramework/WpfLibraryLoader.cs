using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace MS.Internal.PresentationFramework
{
	// Token: 0x020007FF RID: 2047
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	internal static class WpfLibraryLoader
	{
		// Token: 0x06007D9C RID: 32156
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr LoadLibrary(string lpFileName);

		// Token: 0x17001D2D RID: 7469
		// (get) Token: 0x06007D9D RID: 32157 RVA: 0x00234722 File Offset: 0x00232922
		private static string WpfInstallPath { get; } = WpfLibraryLoader.GetWPFInstallPath();

		// Token: 0x06007D9E RID: 32158 RVA: 0x00234729 File Offset: 0x00232929
		internal static void EnsureLoaded(string dllName)
		{
			WpfLibraryLoader.LoadLibrary(Path.Combine(WpfLibraryLoader.WpfInstallPath, dllName));
		}

		// Token: 0x06007D9F RID: 32159 RVA: 0x0023473C File Offset: 0x0023293C
		[EnvironmentPermission(SecurityAction.Assert, Read = "COMPLUS_Version;COMPLUS_InstallRoot")]
		private static string GetWPFInstallPath()
		{
			string text = null;
			string environmentVariable = Environment.GetEnvironmentVariable("COMPLUS_Version");
			if (!string.IsNullOrEmpty(environmentVariable))
			{
				text = Environment.GetEnvironmentVariable("COMPLUS_InstallRoot");
				if (string.IsNullOrEmpty(text))
				{
					text = WpfLibraryLoader.ReadLocalMachineString("Software\\Microsoft\\.NETFramework", "InstallRoot");
				}
				if (!string.IsNullOrEmpty(text))
				{
					text = Path.Combine(text, environmentVariable);
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				text = WpfLibraryLoader.ReadLocalMachineString("Software\\Microsoft\\Net Framework Setup\\NDP\\v4\\Client\\", "InstallPath");
			}
			return Path.Combine(text, "WPF");
		}

		// Token: 0x06007DA0 RID: 32160 RVA: 0x002347B8 File Offset: 0x002329B8
		private static string ReadLocalMachineString(string key, string valueName)
		{
			string text = "HKEY_LOCAL_MACHINE\\" + key;
			new RegistryPermission(RegistryPermissionAccess.Read, text).Assert();
			return Registry.GetValue(text, valueName, null) as string;
		}

		// Token: 0x04003B45 RID: 15173
		private const string COMPLUS_Version = "COMPLUS_Version";

		// Token: 0x04003B46 RID: 15174
		private const string COMPLUS_InstallRoot = "COMPLUS_InstallRoot";

		// Token: 0x04003B47 RID: 15175
		private const string EnvironmentVariables = "COMPLUS_Version;COMPLUS_InstallRoot";

		// Token: 0x04003B48 RID: 15176
		private const string FRAMEWORK_RegKey = "Software\\Microsoft\\Net Framework Setup\\NDP\\v4\\Client\\";

		// Token: 0x04003B49 RID: 15177
		private const string FRAMEWORK_RegKey_FullPath = "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Net Framework Setup\\NDP\\v4\\Client\\";

		// Token: 0x04003B4A RID: 15178
		private const string FRAMEWORK_InstallPath_RegValue = "InstallPath";

		// Token: 0x04003B4B RID: 15179
		private const string DOTNET_RegKey = "Software\\Microsoft\\.NETFramework";

		// Token: 0x04003B4C RID: 15180
		private const string DOTNET_Install_RegValue = "InstallRoot";

		// Token: 0x04003B4D RID: 15181
		private const string WPF_SUBDIR = "WPF";
	}
}
