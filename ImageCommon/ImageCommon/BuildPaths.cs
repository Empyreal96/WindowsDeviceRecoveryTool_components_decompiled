using System;
using Microsoft.WindowsPhone.ImageUpdate.Tools.Common;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x02000002 RID: 2
	public class BuildPaths
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		public static string GetImagingTempPath(string defaultPath)
		{
			string environmentVariable = Environment.GetEnvironmentVariable("BUILD_PRODUCT");
			string text = Environment.GetEnvironmentVariable("OBJECT_ROOT");
			if ((!string.IsNullOrEmpty(environmentVariable) && environmentVariable.Equals("nt", StringComparison.OrdinalIgnoreCase)) || string.IsNullOrEmpty(text))
			{
				text = Environment.GetEnvironmentVariable("TEMP");
				if (string.IsNullOrEmpty(text))
				{
					text = Environment.GetEnvironmentVariable("TMP");
					if (string.IsNullOrEmpty(text))
					{
						text = defaultPath;
					}
				}
			}
			return FileUtils.GetTempFile(text);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x0000213F File Offset: 0x0000033F
		public static string OEMKitFMSchema
		{
			get
			{
				return "OEMKitFM.xsd";
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002146 File Offset: 0x00000346
		public static string PropsProjectSchema
		{
			get
			{
				return "PropsProject.xsd";
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000004 RID: 4 RVA: 0x0000214D File Offset: 0x0000034D
		public static string PropsGuidMappingsSchema
		{
			get
			{
				return "PropsGuidMappings.xsd";
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002154 File Offset: 0x00000354
		public static string PublishingPackageInfoSchema
		{
			get
			{
				return "PublishingPackageInfo.xsd";
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000006 RID: 6 RVA: 0x0000215B File Offset: 0x0000035B
		public static string FMCollectionSchema
		{
			get
			{
				return "FMCollection.xsd";
			}
		}
	}
}
