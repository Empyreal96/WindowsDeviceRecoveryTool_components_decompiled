using System;
using System.Collections.Specialized;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001D9 RID: 473
	internal abstract class BamlVariableSizedRecord : BamlRecord
	{
		// Token: 0x06001EE5 RID: 7909 RVA: 0x00094224 File Offset: 0x00092424
		internal override bool LoadRecordSize(BinaryReader bamlBinaryReader, long bytesAvailable)
		{
			int recordSize;
			bool flag = BamlVariableSizedRecord.LoadVariableRecordSize(bamlBinaryReader, bytesAvailable, out recordSize);
			if (flag)
			{
				this.RecordSize = recordSize;
			}
			return flag;
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x00094246 File Offset: 0x00092446
		internal static bool LoadVariableRecordSize(BinaryReader bamlBinaryReader, long bytesAvailable, out int recordSize)
		{
			if (bytesAvailable >= 4L)
			{
				recordSize = ((BamlBinaryReader)bamlBinaryReader).Read7BitEncodedInt();
				return true;
			}
			recordSize = -1;
			return false;
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x00094260 File Offset: 0x00092460
		protected int ComputeSizeOfVariableLengthRecord(long start, long end)
		{
			int num = (int)(end - start);
			int num2 = BamlBinaryWriter.SizeOf7bitEncodedSize(num);
			num2 = BamlBinaryWriter.SizeOf7bitEncodedSize(num2 + num);
			return num2 + num;
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x00094288 File Offset: 0x00092488
		internal override void Write(BinaryWriter bamlBinaryWriter)
		{
			if (bamlBinaryWriter == null)
			{
				return;
			}
			bamlBinaryWriter.Write((byte)this.RecordType);
			long num = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			this.WriteRecordData(bamlBinaryWriter);
			long end = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			this.RecordSize = this.ComputeSizeOfVariableLengthRecord(num, end);
			bamlBinaryWriter.Seek((int)num, SeekOrigin.Begin);
			this.WriteRecordSize(bamlBinaryWriter);
			this.WriteRecordData(bamlBinaryWriter);
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x000942E4 File Offset: 0x000924E4
		internal void WriteRecordSize(BinaryWriter bamlBinaryWriter)
		{
			((BamlBinaryWriter)bamlBinaryWriter).Write7BitEncodedInt(this.RecordSize);
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x000942F8 File Offset: 0x000924F8
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlVariableSizedRecord bamlVariableSizedRecord = (BamlVariableSizedRecord)record;
			bamlVariableSizedRecord._recordSize = this._recordSize;
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06001EEB RID: 7915 RVA: 0x0009431F File Offset: 0x0009251F
		// (set) Token: 0x06001EEC RID: 7916 RVA: 0x00094327 File Offset: 0x00092527
		internal override int RecordSize
		{
			get
			{
				return this._recordSize;
			}
			set
			{
				this._recordSize = value;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06001EED RID: 7917 RVA: 0x00094330 File Offset: 0x00092530
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlRecord.LastFlagsSection;
			}
		}

		// Token: 0x040014FB RID: 5371
		internal const int MaxRecordSizeFieldLength = 4;

		// Token: 0x040014FC RID: 5372
		private int _recordSize = -1;
	}
}
