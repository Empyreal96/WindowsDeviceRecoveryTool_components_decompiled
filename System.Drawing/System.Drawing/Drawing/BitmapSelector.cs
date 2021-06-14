using System;
using System.Configuration;
using System.Drawing.Configuration;
using System.IO;
using System.Reflection;

namespace System.Drawing
{
	// Token: 0x02000011 RID: 17
	internal static class BitmapSelector
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600004E RID: 78 RVA: 0x000034D0 File Offset: 0x000016D0
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00003519 File Offset: 0x00001719
		internal static string Suffix
		{
			get
			{
				if (BitmapSelector._suffix == null)
				{
					BitmapSelector._suffix = string.Empty;
					SystemDrawingSection systemDrawingSection = ConfigurationManager.GetSection("system.drawing") as SystemDrawingSection;
					if (systemDrawingSection != null)
					{
						string bitmapSuffix = systemDrawingSection.BitmapSuffix;
						if (bitmapSuffix != null && bitmapSuffix != null)
						{
							BitmapSelector._suffix = bitmapSuffix;
						}
					}
				}
				return BitmapSelector._suffix;
			}
			set
			{
				BitmapSelector._suffix = value;
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003524 File Offset: 0x00001724
		internal static string AppendSuffix(string filePath)
		{
			string result;
			try
			{
				result = Path.ChangeExtension(filePath, BitmapSelector.Suffix + Path.GetExtension(filePath));
			}
			catch (ArgumentException)
			{
				result = filePath;
			}
			return result;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003560 File Offset: 0x00001760
		public static string GetFileName(string originalPath)
		{
			if (BitmapSelector.Suffix == string.Empty)
			{
				return originalPath;
			}
			string text = BitmapSelector.AppendSuffix(originalPath);
			if (!File.Exists(text))
			{
				return originalPath;
			}
			return text;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003594 File Offset: 0x00001794
		private static Stream GetResourceStreamHelper(Assembly assembly, Type type, string name)
		{
			Stream result = null;
			try
			{
				result = assembly.GetManifestResourceStream(type, name);
			}
			catch (FileNotFoundException)
			{
			}
			return result;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000035C4 File Offset: 0x000017C4
		private static bool DoesAssemblyHaveCustomAttribute(Assembly assembly, string typeName)
		{
			return BitmapSelector.DoesAssemblyHaveCustomAttribute(assembly, assembly.GetType(typeName));
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000035D4 File Offset: 0x000017D4
		private static bool DoesAssemblyHaveCustomAttribute(Assembly assembly, Type attrType)
		{
			if (attrType != null)
			{
				object[] customAttributes = assembly.GetCustomAttributes(attrType, false);
				if (customAttributes.Length != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000035FA File Offset: 0x000017FA
		internal static bool SatelliteAssemblyOptIn(Assembly assembly)
		{
			return BitmapSelector.DoesAssemblyHaveCustomAttribute(assembly, typeof(BitmapSuffixInSatelliteAssemblyAttribute)) || BitmapSelector.DoesAssemblyHaveCustomAttribute(assembly, "System.Drawing.BitmapSuffixInSatelliteAssemblyAttribute");
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000361B File Offset: 0x0000181B
		internal static bool SameAssemblyOptIn(Assembly assembly)
		{
			return BitmapSelector.DoesAssemblyHaveCustomAttribute(assembly, typeof(BitmapSuffixInSameAssemblyAttribute)) || BitmapSelector.DoesAssemblyHaveCustomAttribute(assembly, "System.Drawing.BitmapSuffixInSameAssemblyAttribute");
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000363C File Offset: 0x0000183C
		public static Stream GetResourceStream(Assembly assembly, Type type, string originalName)
		{
			if (BitmapSelector.Suffix != string.Empty)
			{
				try
				{
					if (BitmapSelector.SameAssemblyOptIn(assembly))
					{
						string name = BitmapSelector.AppendSuffix(originalName);
						Stream resourceStreamHelper = BitmapSelector.GetResourceStreamHelper(assembly, type, name);
						if (resourceStreamHelper != null)
						{
							return resourceStreamHelper;
						}
					}
				}
				catch
				{
				}
				try
				{
					if (BitmapSelector.SatelliteAssemblyOptIn(assembly))
					{
						AssemblyName name2 = assembly.GetName();
						AssemblyName assemblyName = name2;
						assemblyName.Name += BitmapSelector.Suffix;
						name2.ProcessorArchitecture = ProcessorArchitecture.None;
						Assembly assembly2 = Assembly.Load(name2);
						if (assembly2 != null)
						{
							Stream resourceStreamHelper2 = BitmapSelector.GetResourceStreamHelper(assembly2, type, originalName);
							if (resourceStreamHelper2 != null)
							{
								return resourceStreamHelper2;
							}
						}
					}
				}
				catch
				{
				}
			}
			return assembly.GetManifestResourceStream(type, originalName);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000036FC File Offset: 0x000018FC
		public static Stream GetResourceStream(Type type, string originalName)
		{
			return BitmapSelector.GetResourceStream(type.Module.Assembly, type, originalName);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003710 File Offset: 0x00001910
		public static Icon CreateIcon(Type type, string originalName)
		{
			return new Icon(BitmapSelector.GetResourceStream(type, originalName));
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000371E File Offset: 0x0000191E
		public static Bitmap CreateBitmap(Type type, string originalName)
		{
			return new Bitmap(BitmapSelector.GetResourceStream(type, originalName));
		}

		// Token: 0x0400009D RID: 157
		private static string _suffix;
	}
}
