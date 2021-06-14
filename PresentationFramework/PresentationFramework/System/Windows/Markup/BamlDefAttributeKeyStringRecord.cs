using System;
using System.Collections.Specialized;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001E0 RID: 480
	internal class BamlDefAttributeKeyStringRecord : BamlStringValueRecord, IBamlDictionaryKey
	{
		// Token: 0x06001F32 RID: 7986 RVA: 0x00094802 File Offset: 0x00092A02
		internal BamlDefAttributeKeyStringRecord()
		{
			base.Pin();
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x00094818 File Offset: 0x00092A18
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.ValueId = bamlBinaryReader.ReadInt16();
			this._valuePosition = bamlBinaryReader.ReadInt32();
			((IBamlDictionaryKey)this).Shared = bamlBinaryReader.ReadBoolean();
			((IBamlDictionaryKey)this).SharedSet = bamlBinaryReader.ReadBoolean();
			this._keyObject = null;
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x00094851 File Offset: 0x00092A51
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.ValueId);
			this._valuePositionPosition = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			bamlBinaryWriter.Write(this._valuePosition);
			bamlBinaryWriter.Write(((IBamlDictionaryKey)this).Shared);
			bamlBinaryWriter.Write(((IBamlDictionaryKey)this).SharedSet);
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x00094894 File Offset: 0x00092A94
		void IBamlDictionaryKey.UpdateValuePosition(int newPosition, BinaryWriter bamlBinaryWriter)
		{
			long num = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			int num2 = (int)(this._valuePositionPosition - num);
			bamlBinaryWriter.Seek(num2, SeekOrigin.Current);
			bamlBinaryWriter.Write(newPosition);
			bamlBinaryWriter.Seek(-4 - num2, SeekOrigin.Current);
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x000948D0 File Offset: 0x00092AD0
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlDefAttributeKeyStringRecord bamlDefAttributeKeyStringRecord = (BamlDefAttributeKeyStringRecord)record;
			bamlDefAttributeKeyStringRecord._valuePosition = this._valuePosition;
			bamlDefAttributeKeyStringRecord._valuePositionPosition = this._valuePositionPosition;
			bamlDefAttributeKeyStringRecord._keyObject = this._keyObject;
			bamlDefAttributeKeyStringRecord._valueId = this._valueId;
			bamlDefAttributeKeyStringRecord._staticResourceValues = this._staticResourceValues;
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06001F37 RID: 7991 RVA: 0x00094927 File Offset: 0x00092B27
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.DefAttributeKeyString;
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06001F38 RID: 7992 RVA: 0x0009492B File Offset: 0x00092B2B
		// (set) Token: 0x06001F39 RID: 7993 RVA: 0x00094933 File Offset: 0x00092B33
		int IBamlDictionaryKey.ValuePosition
		{
			get
			{
				return this._valuePosition;
			}
			set
			{
				this._valuePosition = value;
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06001F3A RID: 7994 RVA: 0x0009493C File Offset: 0x00092B3C
		// (set) Token: 0x06001F3B RID: 7995 RVA: 0x00094954 File Offset: 0x00092B54
		bool IBamlDictionaryKey.Shared
		{
			get
			{
				return this._flags[BamlDefAttributeKeyStringRecord._sharedSection] == 1;
			}
			set
			{
				this._flags[BamlDefAttributeKeyStringRecord._sharedSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06001F3C RID: 7996 RVA: 0x0009496D File Offset: 0x00092B6D
		// (set) Token: 0x06001F3D RID: 7997 RVA: 0x00094985 File Offset: 0x00092B85
		bool IBamlDictionaryKey.SharedSet
		{
			get
			{
				return this._flags[BamlDefAttributeKeyStringRecord._sharedSetSection] == 1;
			}
			set
			{
				this._flags[BamlDefAttributeKeyStringRecord._sharedSetSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06001F3E RID: 7998 RVA: 0x0009499E File Offset: 0x00092B9E
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlDefAttributeKeyStringRecord._sharedSetSection;
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06001F3F RID: 7999 RVA: 0x000949A5 File Offset: 0x00092BA5
		// (set) Token: 0x06001F40 RID: 8000 RVA: 0x000949AD File Offset: 0x00092BAD
		object IBamlDictionaryKey.KeyObject
		{
			get
			{
				return this._keyObject;
			}
			set
			{
				this._keyObject = value;
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06001F41 RID: 8001 RVA: 0x000949B6 File Offset: 0x00092BB6
		// (set) Token: 0x06001F42 RID: 8002 RVA: 0x000949BE File Offset: 0x00092BBE
		long IBamlDictionaryKey.ValuePositionPosition
		{
			get
			{
				return this._valuePositionPosition;
			}
			set
			{
				this._valuePositionPosition = value;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06001F43 RID: 8003 RVA: 0x000949C7 File Offset: 0x00092BC7
		// (set) Token: 0x06001F44 RID: 8004 RVA: 0x000949CF File Offset: 0x00092BCF
		internal short ValueId
		{
			get
			{
				return this._valueId;
			}
			set
			{
				this._valueId = value;
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06001F45 RID: 8005 RVA: 0x000949D8 File Offset: 0x00092BD8
		// (set) Token: 0x06001F46 RID: 8006 RVA: 0x000949E0 File Offset: 0x00092BE0
		object[] IBamlDictionaryKey.StaticResourceValues
		{
			get
			{
				return this._staticResourceValues;
			}
			set
			{
				this._staticResourceValues = value;
			}
		}

		// Token: 0x0400150C RID: 5388
		private static BitVector32.Section _sharedSection = BitVector32.CreateSection(1, BamlVariableSizedRecord.LastFlagsSection);

		// Token: 0x0400150D RID: 5389
		private static BitVector32.Section _sharedSetSection = BitVector32.CreateSection(1, BamlDefAttributeKeyStringRecord._sharedSection);

		// Token: 0x0400150E RID: 5390
		internal const int ValuePositionSize = 4;

		// Token: 0x0400150F RID: 5391
		private int _valuePosition;

		// Token: 0x04001510 RID: 5392
		private long _valuePositionPosition = -1L;

		// Token: 0x04001511 RID: 5393
		private object _keyObject;

		// Token: 0x04001512 RID: 5394
		private short _valueId;

		// Token: 0x04001513 RID: 5395
		private object[] _staticResourceValues;
	}
}
