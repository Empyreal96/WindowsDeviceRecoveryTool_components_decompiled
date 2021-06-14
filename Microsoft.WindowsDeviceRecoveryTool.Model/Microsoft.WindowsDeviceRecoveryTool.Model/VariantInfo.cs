using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x02000053 RID: 83
	public class VariantInfo
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00007CF8 File Offset: 0x00005EF8
		// (set) Token: 0x0600029E RID: 670 RVA: 0x00007D0F File Offset: 0x00005F0F
		public long Id { get; set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600029F RID: 671 RVA: 0x00007D18 File Offset: 0x00005F18
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x00007D2F File Offset: 0x00005F2F
		public long Size { get; set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x00007D38 File Offset: 0x00005F38
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x00007D4F File Offset: 0x00005F4F
		public string Name { get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x00007D58 File Offset: 0x00005F58
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x00007D6F File Offset: 0x00005F6F
		public string SoftwareVersion { get; set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x00007D78 File Offset: 0x00005F78
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x00007D8F File Offset: 0x00005F8F
		public string VariantVersion { get; set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x00007D98 File Offset: 0x00005F98
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x00007DAF File Offset: 0x00005FAF
		public string AkVersion { get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x00007DB8 File Offset: 0x00005FB8
		// (set) Token: 0x060002AA RID: 682 RVA: 0x00007DCF File Offset: 0x00005FCF
		public string ProductCode { get; set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060002AB RID: 683 RVA: 0x00007DD8 File Offset: 0x00005FD8
		// (set) Token: 0x060002AC RID: 684 RVA: 0x00007DEF File Offset: 0x00005FEF
		public string ProductType { get; set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060002AD RID: 685 RVA: 0x00007DF8 File Offset: 0x00005FF8
		// (set) Token: 0x060002AE RID: 686 RVA: 0x00007E0F File Offset: 0x0000600F
		public string Path { get; set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060002AF RID: 687 RVA: 0x00007E18 File Offset: 0x00006018
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x00007E2F File Offset: 0x0000602F
		public string FfuFilePath { get; set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x00007E38 File Offset: 0x00006038
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x00007E4F File Offset: 0x0000604F
		public bool IsLocal { get; set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x00007E58 File Offset: 0x00006058
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x00007E6F File Offset: 0x0000606F
		public bool OnlyLocal { get; set; }

		// Token: 0x060002B5 RID: 693 RVA: 0x00007E78 File Offset: 0x00006078
		public static VariantInfo GetVariantInfo(string vplPath)
		{
			string name = string.Empty;
			string productCode = string.Empty;
			string softwareVersion = string.Empty;
			string variantVersion = string.Empty;
			string productType = string.Empty;
			string ffuFilePath = string.Empty;
			string akVersion = string.Empty;
			string path = System.IO.Path.GetDirectoryName(vplPath) ?? string.Empty;
			using (Stream stream = new FileStream(vplPath, FileMode.Open, FileAccess.Read))
			{
				using (XmlReader xmlReader = XmlReader.Create(stream))
				{
					while (xmlReader.Read())
					{
						string localName = xmlReader.LocalName;
						switch (localName)
						{
						case "ProductCode":
							productCode = xmlReader.ReadInnerXml();
							break;
						case "SwVersion":
							softwareVersion = xmlReader.ReadInnerXml();
							break;
						case "Description":
							name = xmlReader.ReadInnerXml();
							break;
						case "VariantVersion":
							variantVersion = xmlReader.ReadInnerXml();
							break;
						case "TypeDesignator":
							productType = xmlReader.ReadInnerXml();
							break;
						case "AkVersion":
							akVersion = xmlReader.ReadInnerXml();
							break;
						case "Name":
						{
							string text = xmlReader.ReadInnerXml();
							if (text.EndsWith(".ffu"))
							{
								ffuFilePath = System.IO.Path.Combine(path, text);
							}
							break;
						}
						}
					}
				}
			}
			return new VariantInfo
			{
				Name = name,
				ProductCode = productCode,
				ProductType = productType,
				SoftwareVersion = softwareVersion,
				Path = vplPath,
				AkVersion = akVersion,
				VariantVersion = variantVersion,
				IsLocal = true,
				Size = VariantInfo.GetVariantSizeOnDisk(vplPath),
				FfuFilePath = ffuFilePath
			};
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000811C File Offset: 0x0000631C
		public static long GetVariantSizeOnDisk(string vplPath)
		{
			Tracer<VariantInfo>.LogEntry("GetVariantSizeOnDisk");
			long num = 0L;
			VplContent vplContent = new VplContent();
			vplContent.ParseVplFile(vplPath);
			string directoryName = System.IO.Path.GetDirectoryName(vplPath);
			if (string.IsNullOrEmpty(directoryName))
			{
				throw new DirectoryNotFoundException("Vpl directory not found");
			}
			List<string> list = (from file in vplContent.FileList
			where !string.IsNullOrEmpty(file.Name)
			select file.Name).ToList<string>();
			foreach (string path in list)
			{
				string text = System.IO.Path.Combine(directoryName, path);
				if (File.Exists(text))
				{
					FileInfo fileInfo = new FileInfo(text);
					num += fileInfo.Length;
				}
			}
			FileInfo fileInfo2 = new FileInfo(vplPath);
			num += fileInfo2.Length;
			Tracer<VariantInfo>.WriteInformation("Size on disk: {0}", new object[]
			{
				num
			});
			Tracer<VariantInfo>.LogExit("GetVariantSizeOnDisk");
			return num;
		}
	}
}
