using System;
using System.Configuration;
using System.Drawing.Configuration;
using System.IO;
using System.Reflection;

namespace System.Drawing
{
	// Token: 0x020000FC RID: 252
	internal static class BitmapSelector
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x0000C8D0 File Offset: 0x0000AAD0
		// (set) Token: 0x060003FB RID: 1019 RVA: 0x0000C919 File Offset: 0x0000AB19
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

		// Token: 0x060003FC RID: 1020 RVA: 0x0000C924 File Offset: 0x0000AB24
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

		// Token: 0x060003FD RID: 1021 RVA: 0x0000C960 File Offset: 0x0000AB60
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

		// Token: 0x060003FE RID: 1022 RVA: 0x0000C994 File Offset: 0x0000AB94
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

		// Token: 0x060003FF RID: 1023 RVA: 0x0000C9C4 File Offset: 0x0000ABC4
		private static bool DoesAssemblyHaveCustomAttribute(Assembly assembly, string typeName)
		{
			return BitmapSelector.DoesAssemblyHaveCustomAttribute(assembly, assembly.GetType(typeName));
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000C9D4 File Offset: 0x0000ABD4
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

		// Token: 0x06000401 RID: 1025 RVA: 0x0000C9FA File Offset: 0x0000ABFA
		internal static bool SatelliteAssemblyOptIn(Assembly assembly)
		{
			return BitmapSelector.DoesAssemblyHaveCustomAttribute(assembly, typeof(BitmapSuffixInSatelliteAssemblyAttribute)) || BitmapSelector.DoesAssemblyHaveCustomAttribute(assembly, "System.Drawing.BitmapSuffixInSatelliteAssemblyAttribute");
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000CA1B File Offset: 0x0000AC1B
		internal static bool SameAssemblyOptIn(Assembly assembly)
		{
			return BitmapSelector.DoesAssemblyHaveCustomAttribute(assembly, typeof(BitmapSuffixInSameAssemblyAttribute)) || BitmapSelector.DoesAssemblyHaveCustomAttribute(assembly, "System.Drawing.BitmapSuffixInSameAssemblyAttribute");
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000CA3C File Offset: 0x0000AC3C
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

		// Token: 0x06000404 RID: 1028 RVA: 0x0000CAFC File Offset: 0x0000ACFC
		public static Stream GetResourceStream(Type type, string originalName)
		{
			return BitmapSelector.GetResourceStream(type.Module.Assembly, type, originalName);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000CB10 File Offset: 0x0000AD10
		public static Icon CreateIcon(Type type, string originalName)
		{
			return new Icon(BitmapSelector.GetResourceStream(type, originalName));
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000CB1E File Offset: 0x0000AD1E
		public static Bitmap CreateBitmap(Type type, string originalName)
		{
			return new Bitmap(BitmapSelector.GetResourceStream(type, originalName));
		}

		// Token: 0x0400042E RID: 1070
		private static string _suffix;
	}
}
