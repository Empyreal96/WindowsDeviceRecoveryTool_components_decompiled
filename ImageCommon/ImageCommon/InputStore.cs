using System;
using System.Xml.Serialization;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x02000011 RID: 17
	public class InputStore
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x00004E2C File Offset: 0x0000302C
		public InputStore()
		{
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004E34 File Offset: 0x00003034
		public InputStore(string storeType)
		{
			this.StoreType = storeType;
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00004E43 File Offset: 0x00003043
		// (set) Token: 0x060000DC RID: 220 RVA: 0x00004E4B File Offset: 0x0000304B
		public string Id { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00004E54 File Offset: 0x00003054
		// (set) Token: 0x060000DE RID: 222 RVA: 0x00004E5C File Offset: 0x0000305C
		public string StoreType { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00004E65 File Offset: 0x00003065
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00004E6D File Offset: 0x0000306D
		public string DevicePath { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00004E76 File Offset: 0x00003076
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00004E7E File Offset: 0x0000307E
		[CLSCompliant(false)]
		public uint SizeInSectors { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00004E87 File Offset: 0x00003087
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00004E8F File Offset: 0x0000308F
		public bool OnlyAllocateDefinedGptEntries { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00004E98 File Offset: 0x00003098
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00004EA0 File Offset: 0x000030A0
		[XmlArray]
		[XmlArrayItem(ElementName = "Partition", Type = typeof(InputPartition), IsNullable = false)]
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

		// Token: 0x060000E7 RID: 231 RVA: 0x00004EA9 File Offset: 0x000030A9
		public bool IsMainOSStore()
		{
			return string.CompareOrdinal(this.StoreType, "MainOSStore") == 0;
		}

		// Token: 0x04000050 RID: 80
		private InputPartition[] _partitions;
	}
}
