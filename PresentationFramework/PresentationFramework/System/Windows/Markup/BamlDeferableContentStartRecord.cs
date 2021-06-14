using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001FA RID: 506
	internal class BamlDeferableContentStartRecord : BamlRecord
	{
		// Token: 0x06002005 RID: 8197 RVA: 0x00095BB5 File Offset: 0x00093DB5
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.ContentSize = bamlBinaryReader.ReadInt32();
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x00095BC3 File Offset: 0x00093DC3
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			this._contentSizePosition = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			bamlBinaryWriter.Write(this.ContentSize);
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x00095BE0 File Offset: 0x00093DE0
		internal void UpdateContentSize(int contentSize, BinaryWriter bamlBinaryWriter)
		{
			long num = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			int num2 = (int)(this._contentSizePosition - num);
			bamlBinaryWriter.Seek(num2, SeekOrigin.Current);
			bamlBinaryWriter.Write(contentSize);
			bamlBinaryWriter.Seek((int)(-4L - (long)num2), SeekOrigin.Current);
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x00095C20 File Offset: 0x00093E20
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlDeferableContentStartRecord bamlDeferableContentStartRecord = (BamlDeferableContentStartRecord)record;
			bamlDeferableContentStartRecord._contentSize = this._contentSize;
			bamlDeferableContentStartRecord._contentSizePosition = this._contentSizePosition;
			bamlDeferableContentStartRecord._valuesBuffer = this._valuesBuffer;
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06002009 RID: 8201 RVA: 0x00095C5F File Offset: 0x00093E5F
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.DeferableContentStart;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x0600200A RID: 8202 RVA: 0x00095C63 File Offset: 0x00093E63
		// (set) Token: 0x0600200B RID: 8203 RVA: 0x00095C6B File Offset: 0x00093E6B
		internal int ContentSize
		{
			get
			{
				return this._contentSize;
			}
			set
			{
				this._contentSize = value;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x0600200C RID: 8204 RVA: 0x00094CFC File Offset: 0x00092EFC
		// (set) Token: 0x0600200D RID: 8205 RVA: 0x00002137 File Offset: 0x00000337
		internal override int RecordSize
		{
			get
			{
				return 4;
			}
			set
			{
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x0600200E RID: 8206 RVA: 0x00095C74 File Offset: 0x00093E74
		// (set) Token: 0x0600200F RID: 8207 RVA: 0x00095C7C File Offset: 0x00093E7C
		internal byte[] ValuesBuffer
		{
			get
			{
				return this._valuesBuffer;
			}
			set
			{
				this._valuesBuffer = value;
			}
		}

		// Token: 0x0400153C RID: 5436
		private const long ContentSizeSize = 4L;

		// Token: 0x0400153D RID: 5437
		private int _contentSize = -1;

		// Token: 0x0400153E RID: 5438
		private long _contentSizePosition = -1L;

		// Token: 0x0400153F RID: 5439
		private byte[] _valuesBuffer;
	}
}
