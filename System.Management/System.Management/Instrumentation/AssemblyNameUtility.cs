using System;
using System.Globalization;
using System.Reflection;

namespace System.Management.Instrumentation
{
	// Token: 0x020000AB RID: 171
	internal sealed class AssemblyNameUtility
	{
		// Token: 0x06000486 RID: 1158 RVA: 0x00021FA0 File Offset: 0x000201A0
		private static string BinToString(byte[] rg)
		{
			if (rg == null)
			{
				return "";
			}
			string text = "";
			for (int i = 0; i < rg.GetLength(0); i++)
			{
				text += string.Format("{0:x2}", rg[i]);
			}
			return text;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00021FE8 File Offset: 0x000201E8
		public static string UniqueToAssemblyMinorVersion(Assembly assembly)
		{
			AssemblyName name = assembly.GetName(true);
			return string.Concat(new object[]
			{
				name.Name,
				"_SN_",
				AssemblyNameUtility.BinToString(name.GetPublicKeyToken()),
				"_Version_",
				name.Version.Major,
				".",
				name.Version.Minor
			});
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00022060 File Offset: 0x00020260
		public static string UniqueToAssemblyFullVersion(Assembly assembly)
		{
			AssemblyName name = assembly.GetName(true);
			return string.Concat(new object[]
			{
				name.Name,
				"_SN_",
				AssemblyNameUtility.BinToString(name.GetPublicKeyToken()),
				"_Version_",
				name.Version.Major,
				".",
				name.Version.Minor,
				".",
				name.Version.Build,
				".",
				name.Version.Revision
			});
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00022110 File Offset: 0x00020310
		private static string UniqueToAssemblyVersion(Assembly assembly)
		{
			AssemblyName name = assembly.GetName(true);
			return string.Concat(new object[]
			{
				name.Name,
				"_SN_",
				AssemblyNameUtility.BinToString(name.GetPublicKeyToken()),
				"_Version_",
				name.Version.Major,
				".",
				name.Version.Minor,
				".",
				name.Version.Build,
				".",
				name.Version.Revision
			});
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x000221C0 File Offset: 0x000203C0
		public static string UniqueToAssemblyBuild(Assembly assembly)
		{
			return AssemblyNameUtility.UniqueToAssemblyVersion(assembly) + "_Mvid_" + MetaDataInfo.GetMvid(assembly).ToString().ToLower(CultureInfo.InvariantCulture);
		}
	}
}
