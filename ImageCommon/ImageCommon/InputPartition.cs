using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x02000012 RID: 18
	public class InputPartition
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00004EBE File Offset: 0x000030BE
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00004EC6 File Offset: 0x000030C6
		public string Name { get; set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00004ECF File Offset: 0x000030CF
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00004ED7 File Offset: 0x000030D7
		public string Type { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00004EE0 File Offset: 0x000030E0
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00004EE8 File Offset: 0x000030E8
		public string Id { get; set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00004EF1 File Offset: 0x000030F1
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00004EF9 File Offset: 0x000030F9
		public bool ReadOnly { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00004F02 File Offset: 0x00003102
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00004F0A File Offset: 0x0000310A
		public bool AttachDriveLetter { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00004F13 File Offset: 0x00003113
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00004F1B File Offset: 0x0000311B
		public bool Hidden { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00004F24 File Offset: 0x00003124
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00004F2C File Offset: 0x0000312C
		public bool Bootable { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00004F35 File Offset: 0x00003135
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00004F3D File Offset: 0x0000313D
		[CLSCompliant(false)]
		public uint TotalSectors { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00004F46 File Offset: 0x00003146
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00004F4E File Offset: 0x0000314E
		public bool UseAllSpace { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00004F57 File Offset: 0x00003157
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00004F5F File Offset: 0x0000315F
		public string FileSystem { get; set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00004F68 File Offset: 0x00003168
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00004F70 File Offset: 0x00003170
		public string UpdateType { get; set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00004F79 File Offset: 0x00003179
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00004F81 File Offset: 0x00003181
		public bool Compressed { get; set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00004F8A File Offset: 0x0000318A
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00004F92 File Offset: 0x00003192
		[XmlElement("RequiresCompression")]
		public bool RequiresCompression { get; set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00004FA4 File Offset: 0x000031A4
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00004F9B File Offset: 0x0000319B
		public string PrimaryPartition
		{
			get
			{
				if (string.IsNullOrEmpty(this._primaryPartition))
				{
					return this.Name;
				}
				return this._primaryPartition;
			}
			set
			{
				this._primaryPartition = value;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00004FC0 File Offset: 0x000031C0
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00004FC8 File Offset: 0x000031C8
		public bool RequiredToFlash { get; set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00004FD1 File Offset: 0x000031D1
		// (set) Token: 0x06000107 RID: 263 RVA: 0x00004FD9 File Offset: 0x000031D9
		public bool SingleSectorAlignment { get; set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00004FE2 File Offset: 0x000031E2
		// (set) Token: 0x06000109 RID: 265 RVA: 0x00004FEA File Offset: 0x000031EA
		[CLSCompliant(false)]
		[XmlIgnore]
		public uint ByteAlignment { get; set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00004FF4 File Offset: 0x000031F4
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00005014 File Offset: 0x00003214
		[XmlElement("ByteAlignment")]
		public string ByteAlignmentString
		{
			get
			{
				return this.ByteAlignment.ToString(CultureInfo.InvariantCulture);
			}
			set
			{
				uint byteAlignment = 0U;
				if (!InputHelpers.StringToUint(value, out byteAlignment))
				{
					throw new ImageCommonException(string.Format("Partition {0}'s byte alignment cannot be parsed.", string.IsNullOrEmpty(this.Name) ? "Unknown" : this.Name));
				}
				this.ByteAlignment = byteAlignment;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600010C RID: 268 RVA: 0x0000505E File Offset: 0x0000325E
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00005066 File Offset: 0x00003266
		[CLSCompliant(false)]
		[XmlIgnore]
		public uint ClusterSize { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00005070 File Offset: 0x00003270
		// (set) Token: 0x0600010F RID: 271 RVA: 0x00005090 File Offset: 0x00003290
		[XmlElement("ClusterSize")]
		public string ClusterSizeString
		{
			get
			{
				return this.ClusterSize.ToString(CultureInfo.InvariantCulture);
			}
			set
			{
				uint clusterSize = 0U;
				if (!InputHelpers.StringToUint(value, out clusterSize))
				{
					throw new ImageCommonException(string.Format("Partition {0}'s cluster size cannot be parsed.", string.IsNullOrEmpty(this.Name) ? "Unknown" : this.Name));
				}
				this.ClusterSize = clusterSize;
			}
		}

		// Token: 0x04000056 RID: 86
		[CLSCompliant(false)]
		public uint MinFreeSectors;

		// Token: 0x04000057 RID: 87
		[CLSCompliant(false)]
		public uint GeneratedFileOverheadSectors;

		// Token: 0x04000058 RID: 88
		private string _primaryPartition;
	}
}
