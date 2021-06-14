using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x0200000E RID: 14
	[XmlType(Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
	[XmlRoot(ElementName = "DeviceLayout", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate", IsNullable = false)]
	public class DeviceLayoutInput
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00004BF9 File Offset: 0x00002DF9
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x00004C01 File Offset: 0x00002E01
		[XmlArrayItem(ElementName = "Partition", Type = typeof(InputPartition), IsNullable = false)]
		[XmlArray]
		public InputPartition[] Partitions
		{
			get
			{
				return this._partitions;
			}
			set
			{
				this._partitions = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004C0A File Offset: 0x00002E0A
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00004C12 File Offset: 0x00002E12
		[XmlElement("SectorSize")]
		[CLSCompliant(false)]
		public uint SectorSize
		{
			get
			{
				return this._sectorSize;
			}
			set
			{
				this._sectorSize = value;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004C1B File Offset: 0x00002E1B
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00004C23 File Offset: 0x00002E23
		[XmlElement("ChunkSize")]
		[CLSCompliant(false)]
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

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004C2C File Offset: 0x00002E2C
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00004C34 File Offset: 0x00002E34
		[CLSCompliant(false)]
		[XmlIgnore]
		public uint DefaultPartitionByteAlignment { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00004C3D File Offset: 0x00002E3D
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00004C45 File Offset: 0x00002E45
		[XmlElement("VersionTag")]
		public string VersionTag { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004C50 File Offset: 0x00002E50
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00004C70 File Offset: 0x00002E70
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

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00004CA1 File Offset: 0x00002EA1
		public InputPartition[] MainOSPartitions
		{
			get
			{
				return this.Partitions;
			}
		}

		// Token: 0x0400003D RID: 61
		private InputPartition[] _partitions;

		// Token: 0x0400003E RID: 62
		private uint _sectorSize;

		// Token: 0x0400003F RID: 63
		private uint _chunkSize = 256U;
	}
}
