using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001FE RID: 510
	internal class BamlStaticResourceIdRecord : BamlRecord
	{
		// Token: 0x06002026 RID: 8230 RVA: 0x00095E41 File Offset: 0x00094041
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.StaticResourceId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x00095E4F File Offset: 0x0009404F
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.StaticResourceId);
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x00095E60 File Offset: 0x00094060
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlStaticResourceIdRecord bamlStaticResourceIdRecord = (BamlStaticResourceIdRecord)record;
			bamlStaticResourceIdRecord._staticResourceId = this._staticResourceId;
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06002029 RID: 8233 RVA: 0x00095E87 File Offset: 0x00094087
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.StaticResourceId;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x0600202A RID: 8234 RVA: 0x00094C44 File Offset: 0x00092E44
		// (set) Token: 0x0600202B RID: 8235 RVA: 0x00002137 File Offset: 0x00000337
		internal override int RecordSize
		{
			get
			{
				return 2;
			}
			set
			{
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x0600202C RID: 8236 RVA: 0x00095E8B File Offset: 0x0009408B
		// (set) Token: 0x0600202D RID: 8237 RVA: 0x00095E93 File Offset: 0x00094093
		internal short StaticResourceId
		{
			get
			{
				return this._staticResourceId;
			}
			set
			{
				this._staticResourceId = value;
			}
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x00095E9C File Offset: 0x0009409C
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} staticResourceId({1})", new object[]
			{
				this.RecordType,
				this.StaticResourceId
			});
		}

		// Token: 0x04001545 RID: 5445
		private short _staticResourceId = -1;
	}
}
