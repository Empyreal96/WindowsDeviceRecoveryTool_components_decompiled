using System;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x0200000F RID: 15
	[XmlRoot(ElementName = "DeviceLayout", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate/v2", IsNullable = false)]
	[XmlType(Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate/v2")]
	public class DeviceLayoutInputv2
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004CBC File Offset: 0x00002EBC
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00004CC4 File Offset: 0x00002EC4
		[XmlArray]
		[XmlArrayItem(ElementName = "Store", Type = typeof(InputStore), IsNullable = false)]
		public InputStore[] Stores { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004CCD File Offset: 0x00002ECD
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00004CD5 File Offset: 0x00002ED5
		[XmlElement("SectorSize")]
		public uint SectorSize { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004CDE File Offset: 0x00002EDE
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00004CE6 File Offset: 0x00002EE6
		[XmlElement("ChunkSize")]
		public uint ChunkSize
		{
			get
			{
				return this._chunkSize;
			}
			set
			{
				this._chunkSize = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00004CEF File Offset: 0x00002EEF
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00004CF7 File Offset: 0x00002EF7
		[XmlIgnore]
		public uint DefaultPartitionByteAlignment { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00004D00 File Offset: 0x00002F00
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00004D08 File Offset: 0x00002F08
		[XmlElement("VersionTag")]
		public string VersionTag { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00004D14 File Offset: 0x00002F14
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00004D34 File Offset: 0x00002F34
		[XmlElement("DefaultPartitionByteAlignment")]
		public string DefaultPartitionByteAlignmentAsString
		{
			get
			{
				return this.DefaultPartitionByteAlignment.ToString(CultureInfo.InvariantCulture);
			}
			set
			{
				uint defaultPartitionByteAlignment = 0U;
				if (!InputHelpers.StringToUint(value, out defaultPartitionByteAlignment))
				{
					throw new ImageCommonException(string.Format("The default byte alignment cannot be parsed: {0}", value));
				}
				this.DefaultPartitionByteAlignment = defaultPartitionByteAlignment;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00004D6D File Offset: 0x00002F6D
		public InputStore MainOSStore
		{
			get
			{
				return this.Stores.FirstOrDefault((InputStore x) => x.IsMainOSStore());
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00004D97 File Offset: 0x00002F97
		public InputPartition[] MainOSPartitions
		{
			get
			{
				return this.MainOSStore.Partitions;
			}
		}

		// Token: 0x04000042 RID: 66
		private uint _chunkSize = 256U;
	}
}
