using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001D8 RID: 472
	internal abstract class BamlRecord
	{
		// Token: 0x06001ED0 RID: 7888 RVA: 0x00016748 File Offset: 0x00014948
		internal virtual bool LoadRecordSize(BinaryReader bamlBinaryReader, long bytesAvailable)
		{
			return true;
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void LoadRecordData(BinaryReader bamlBinaryReader)
		{
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x000940D9 File Offset: 0x000922D9
		internal virtual void Write(BinaryWriter bamlBinaryWriter)
		{
			if (bamlBinaryWriter == null)
			{
				return;
			}
			bamlBinaryWriter.Write((byte)this.RecordType);
			this.WriteRecordData(bamlBinaryWriter);
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06001ED4 RID: 7892 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06001ED5 RID: 7893 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual int RecordSize
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06001ED6 RID: 7894 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.Unknown;
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06001ED7 RID: 7895 RVA: 0x000940F2 File Offset: 0x000922F2
		// (set) Token: 0x06001ED8 RID: 7896 RVA: 0x000940FA File Offset: 0x000922FA
		internal BamlRecord Next
		{
			get
			{
				return this._nextRecord;
			}
			set
			{
				this._nextRecord = value;
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06001ED9 RID: 7897 RVA: 0x00094103 File Offset: 0x00092303
		internal bool IsPinned
		{
			get
			{
				return this.PinnedCount > 0;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06001EDA RID: 7898 RVA: 0x0009410E File Offset: 0x0009230E
		// (set) Token: 0x06001EDB RID: 7899 RVA: 0x00094120 File Offset: 0x00092320
		internal int PinnedCount
		{
			get
			{
				return this._flags[BamlRecord._pinnedFlagSection];
			}
			set
			{
				this._flags[BamlRecord._pinnedFlagSection] = value;
			}
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x00094134 File Offset: 0x00092334
		internal void Pin()
		{
			if (this.PinnedCount < 3)
			{
				int pinnedCount = this.PinnedCount + 1;
				this.PinnedCount = pinnedCount;
			}
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x0009415C File Offset: 0x0009235C
		internal void Unpin()
		{
			if (this.PinnedCount < 3)
			{
				int pinnedCount = this.PinnedCount - 1;
				this.PinnedCount = pinnedCount;
			}
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x00094182 File Offset: 0x00092382
		internal virtual void Copy(BamlRecord record)
		{
			record._flags = this._flags;
			record._nextRecord = this._nextRecord;
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06001EDF RID: 7903 RVA: 0x0009419C File Offset: 0x0009239C
		internal static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlRecord._pinnedFlagSection;
			}
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x000941A3 File Offset: 0x000923A3
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}", new object[]
			{
				this.RecordType
			});
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x000941C8 File Offset: 0x000923C8
		protected static string GetTypeName(int typeId)
		{
			string result = typeId.ToString(CultureInfo.InvariantCulture);
			if (typeId < 0)
			{
				result = ((KnownElements)(-(KnownElements)typeId)).ToString();
			}
			return result;
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x000941F9 File Offset: 0x000923F9
		internal static bool IsContentRecord(BamlRecordType bamlRecordType)
		{
			return bamlRecordType == BamlRecordType.PropertyComplexStart || bamlRecordType == BamlRecordType.PropertyArrayStart || bamlRecordType == BamlRecordType.PropertyIListStart || bamlRecordType == BamlRecordType.PropertyIDictionaryStart || bamlRecordType == BamlRecordType.Text;
		}

		// Token: 0x040014F7 RID: 5367
		internal BitVector32 _flags;

		// Token: 0x040014F8 RID: 5368
		private static BitVector32.Section _pinnedFlagSection = BitVector32.CreateSection(3);

		// Token: 0x040014F9 RID: 5369
		private BamlRecord _nextRecord;

		// Token: 0x040014FA RID: 5370
		internal const int RecordTypeFieldLength = 1;
	}
}
