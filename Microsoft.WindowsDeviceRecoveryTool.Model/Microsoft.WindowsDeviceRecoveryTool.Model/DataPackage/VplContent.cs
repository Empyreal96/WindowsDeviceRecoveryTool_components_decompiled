using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage
{
	// Token: 0x02000022 RID: 34
	public class VplContent
	{
		// Token: 0x06000100 RID: 256 RVA: 0x00003BE8 File Offset: 0x00001DE8
		public VplContent()
		{
			this.fileList = new List<VplFile>();
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00003C00 File Offset: 0x00001E00
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00003C17 File Offset: 0x00001E17
		public string VplFileName { get; private set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00003C20 File Offset: 0x00001E20
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00003C37 File Offset: 0x00001E37
		public string Description { get; private set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00003C40 File Offset: 0x00001E40
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00003C57 File Offset: 0x00001E57
		public string TypeDesignator { get; private set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00003C60 File Offset: 0x00001E60
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00003C77 File Offset: 0x00001E77
		public string ProductCode { get; private set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00003C80 File Offset: 0x00001E80
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00003C97 File Offset: 0x00001E97
		public string SoftwareVersion { get; private set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00003CA0 File Offset: 0x00001EA0
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00003CB7 File Offset: 0x00001EB7
		public string VariantVersion { get; private set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00003CC0 File Offset: 0x00001EC0
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00003CD7 File Offset: 0x00001ED7
		public Dictionary<int, string> RofsVersions { get; private set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00003CE0 File Offset: 0x00001EE0
		public ReadOnlyCollection<VplFile> FileList
		{
			get
			{
				return new ReadOnlyCollection<VplFile>(this.fileList);
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00003D00 File Offset: 0x00001F00
		public static Dictionary<int, string> ParseRofsVersions(string vplFilePath)
		{
			XDocument xdocument = XDocument.Load(vplFilePath);
			XElement variant = xdocument.Descendants("Variant").First<XElement>();
			return VplContent.ParseRofsVersions(variant);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00003D38 File Offset: 0x00001F38
		public void ParseVplFile(string vplFilePath)
		{
			this.VplFileName = Path.GetFileName(vplFilePath);
			XDocument xdocument = XDocument.Load(vplFilePath);
			XElement variant = xdocument.Descendants("Variant").First<XElement>();
			this.ParseVariantIdentification(variant);
			this.ParseSwVersion(variant);
			this.ParseVariantVersion(variant);
			this.ParseFileList(variant);
			this.RofsVersions = VplContent.ParseRofsVersions(variant);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00003DE8 File Offset: 0x00001FE8
		private static Dictionary<int, string> ParseRofsVersions(XElement variant)
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			IEnumerable<XElement> enumerable = variant.Descendants("FlashImage");
			try
			{
				foreach (XElement xelement in enumerable)
				{
					XElement xelement2 = xelement.Descendants("Version").FirstOrDefault<XElement>();
					XElement xelement3 = xelement.Descendants("RofsIndex").FirstOrDefault<XElement>();
					if (xelement2 != null && xelement2.Value.Any<char>() && xelement3 != null)
					{
						short num = Convert.ToInt16(xelement3.Value);
						if (num < 1 || num > 6)
						{
							Tracer<VplContent>.WriteError(string.Format("Illegal ROFS index in VPL ({0}).", num), new object[0]);
							break;
						}
						dictionary.Add((int)num, xelement2.Value);
					}
					else
					{
						if (xelement3 == null)
						{
							Tracer<VplContent>.WriteError("ROFS index element in VPL was invalid or missing", new object[0]);
							break;
						}
						Tracer<VplContent>.WriteError("ROFS version element in VPL was invalid or missing", new object[0]);
						break;
					}
				}
			}
			catch (Exception error)
			{
				Tracer<VplContent>.WriteError(error, "Problem with ROFS parsing.", new object[0]);
				throw;
			}
			return (from elem in dictionary
			orderby elem.Key
			select elem).ToDictionary((KeyValuePair<int, string> elem) => elem.Key, (KeyValuePair<int, string> elem) => elem.Value);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00003FE8 File Offset: 0x000021E8
		private void ParseVariantIdentification(XElement variant)
		{
			XElement xelement = variant.Descendants("VariantIdentification").First<XElement>();
			XElement xelement2 = xelement.Descendants("Description").First<XElement>();
			this.Description = xelement2.Value;
			XElement xelement3 = xelement.Descendants("TypeDesignator").First<XElement>();
			this.TypeDesignator = xelement3.Value;
			XElement xelement4 = xelement.Descendants("ProductCode").First<XElement>();
			this.ProductCode = xelement4.Value;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004078 File Offset: 0x00002278
		private void ParseSwVersion(XElement variant)
		{
			XElement xelement = variant.Descendants("SwVersion").First<XElement>();
			this.SoftwareVersion = xelement.Value;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000040AC File Offset: 0x000022AC
		private void ParseVariantVersion(XElement variant)
		{
			XElement xelement = variant.Descendants("VariantVersion").First<XElement>();
			this.VariantVersion = xelement.Value;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000040E0 File Offset: 0x000022E0
		private void ParseFileList(XElement variant)
		{
			this.fileList.Clear();
			XElement xelement = variant.Descendants("FileList").First<XElement>();
			IEnumerable<XElement> enumerable = xelement.Descendants("File");
			foreach (XElement file in enumerable)
			{
				this.ParseFile(file);
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00004170 File Offset: 0x00002370
		private void ParseFile(XElement file)
		{
			string value = file.Descendants("Name").First<XElement>().Value;
			string value2 = file.Descendants("FileType").First<XElement>().Value;
			string fileSubtype = string.Empty;
			XElement xelement = file.Element("FileSubType");
			if (xelement != null)
			{
				fileSubtype = xelement.Value;
			}
			string strA = "false";
			XElement xelement2 = file.Element("Signed");
			if (xelement2 != null)
			{
				strA = xelement2.Value;
			}
			string strA2 = "false";
			XElement xelement3 = file.Element("Optional");
			if (xelement3 != null)
			{
				strA2 = xelement3.Value;
			}
			string crc = string.Empty;
			XElement xelement4 = file.Element("Crc");
			if (xelement4 != null)
			{
				crc = xelement4.Value;
			}
			bool signed = string.CompareOrdinal(strA, "true") == 0;
			bool optional = string.CompareOrdinal(strA2, "true") == 0;
			VplFile item = new VplFile(value, value2, fileSubtype, signed, optional, crc);
			this.fileList.Add(item);
		}

		// Token: 0x040000B1 RID: 177
		private readonly List<VplFile> fileList;
	}
}
