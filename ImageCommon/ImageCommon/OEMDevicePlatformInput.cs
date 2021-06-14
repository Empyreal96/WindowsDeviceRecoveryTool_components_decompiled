using System;
using System.Xml.Serialization;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x02000010 RID: 16
	[XmlType(Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
	[XmlRoot(ElementName = "OEMDevicePlatform", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate", IsNullable = false)]
	public class OEMDevicePlatformInput
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00004DB8 File Offset: 0x00002FB8
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00004E1B File Offset: 0x0000301B
		[XmlArrayItem(ElementName = "ID")]
		[XmlArray("DevicePlatformIDs")]
		public string[] DevicePlatformIDs
		{
			get
			{
				if (this.DevicePlatformID != null && this._idArray != null)
				{
					throw new ImageCommonException("Please specify either a DevicePlatformID or a group of DevicePlatformIDs in the device platform package, but not both.");
				}
				if (this.DevicePlatformID == null && this._idArray == null)
				{
					throw new ImageCommonException("Please specify either a DevicePlatformID or a group of DevicePlatformIDs in the device platform package. No platform ID is currently present.");
				}
				if (this.DevicePlatformID != null)
				{
					return new string[]
					{
						this.DevicePlatformID
					};
				}
				return this._idArray;
			}
			set
			{
				this._idArray = value;
			}
		}

		// Token: 0x04000048 RID: 72
		[XmlElement("DevicePlatformID")]
		public string DevicePlatformID;

		// Token: 0x04000049 RID: 73
		private string[] _idArray;

		// Token: 0x0400004A RID: 74
		[XmlArray("CompressedPartitions")]
		[XmlArrayItem(ElementName = "Name")]
		public string[] CompressedPartitions;

		// Token: 0x0400004B RID: 75
		[XmlArray("UncompressedPartitions")]
		[XmlArrayItem(ElementName = "Name")]
		public string[] UncompressedPartitions;

		// Token: 0x0400004C RID: 76
		[CLSCompliant(false)]
		public uint MinSectorCount;

		// Token: 0x0400004D RID: 77
		[CLSCompliant(false)]
		public uint AdditionalMainOSFreeSectorsRequest;

		// Token: 0x0400004E RID: 78
		[CLSCompliant(false)]
		public uint MMOSPartitionTotalSectorsOverride;

		// Token: 0x0400004F RID: 79
		[XmlElement("MainOSRTCDataReservedSectors")]
		[CLSCompliant(false)]
		public uint MainOSRTCDataReservedSectors;
	}
}
