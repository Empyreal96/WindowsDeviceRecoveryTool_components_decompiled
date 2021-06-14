using System;
using System.Collections.Specialized;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000209 RID: 521
	internal class BamlTypeInfoRecord : BamlVariableSizedRecord
	{
		// Token: 0x0600206E RID: 8302 RVA: 0x0009631E File Offset: 0x0009451E
		internal BamlTypeInfoRecord()
		{
			base.Pin();
			this.TypeId = -1;
		}

		// Token: 0x0600206F RID: 8303 RVA: 0x0009633C File Offset: 0x0009453C
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.TypeId = bamlBinaryReader.ReadInt16();
			this.AssemblyId = bamlBinaryReader.ReadInt16();
			this.TypeFullName = bamlBinaryReader.ReadString();
			this._typeInfoFlags = (BamlTypeInfoRecord.TypeInfoFlags)(this.AssemblyId >> 12);
			this._assemblyId &= 4095;
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x00096390 File Offset: 0x00094590
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.TypeId);
			bamlBinaryWriter.Write((short)((ushort)this.AssemblyId | (ushort)(this._typeInfoFlags << 12)));
			bamlBinaryWriter.Write(this.TypeFullName);
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x000963C4 File Offset: 0x000945C4
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlTypeInfoRecord bamlTypeInfoRecord = (BamlTypeInfoRecord)record;
			bamlTypeInfoRecord._typeInfoFlags = this._typeInfoFlags;
			bamlTypeInfoRecord._assemblyId = this._assemblyId;
			bamlTypeInfoRecord._typeFullName = this._typeFullName;
			bamlTypeInfoRecord._type = this._type;
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06002072 RID: 8306 RVA: 0x00096410 File Offset: 0x00094610
		// (set) Token: 0x06002073 RID: 8307 RVA: 0x00096447 File Offset: 0x00094647
		internal short TypeId
		{
			get
			{
				short num = (short)this._flags[BamlTypeInfoRecord._typeIdLowSection];
				return num | (short)(this._flags[BamlTypeInfoRecord._typeIdHighSection] << 8);
			}
			set
			{
				this._flags[BamlTypeInfoRecord._typeIdLowSection] = (int)(value & 255);
				this._flags[BamlTypeInfoRecord._typeIdHighSection] = (int)((short)(((int)value & 65280) >> 8));
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06002074 RID: 8308 RVA: 0x0009647B File Offset: 0x0009467B
		// (set) Token: 0x06002075 RID: 8309 RVA: 0x00096483 File Offset: 0x00094683
		internal short AssemblyId
		{
			get
			{
				return this._assemblyId;
			}
			set
			{
				if (this._assemblyId > 4095)
				{
					throw new XamlParseException(SR.Get("ParserTooManyAssemblies"));
				}
				this._assemblyId = value;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06002076 RID: 8310 RVA: 0x000964A9 File Offset: 0x000946A9
		// (set) Token: 0x06002077 RID: 8311 RVA: 0x000964B1 File Offset: 0x000946B1
		internal string TypeFullName
		{
			get
			{
				return this._typeFullName;
			}
			set
			{
				this._typeFullName = value;
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06002078 RID: 8312 RVA: 0x000964BA File Offset: 0x000946BA
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.TypeInfo;
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06002079 RID: 8313 RVA: 0x000964BE File Offset: 0x000946BE
		// (set) Token: 0x0600207A RID: 8314 RVA: 0x000964C6 File Offset: 0x000946C6
		internal Type Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x0600207B RID: 8315 RVA: 0x000964D0 File Offset: 0x000946D0
		internal string ClrNamespace
		{
			get
			{
				int num = this._typeFullName.LastIndexOf('.');
				if (num <= 0)
				{
					return string.Empty;
				}
				return this._typeFullName.Substring(0, num);
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x0600207C RID: 8316 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool HasSerializer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x0600207D RID: 8317 RVA: 0x00096502 File Offset: 0x00094702
		// (set) Token: 0x0600207E RID: 8318 RVA: 0x0009650F File Offset: 0x0009470F
		internal bool IsInternalType
		{
			get
			{
				return (this._typeInfoFlags & BamlTypeInfoRecord.TypeInfoFlags.Internal) == BamlTypeInfoRecord.TypeInfoFlags.Internal;
			}
			set
			{
				if (value)
				{
					this._typeInfoFlags |= BamlTypeInfoRecord.TypeInfoFlags.Internal;
				}
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x0600207F RID: 8319 RVA: 0x00096522 File Offset: 0x00094722
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlTypeInfoRecord._typeIdHighSection;
			}
		}

		// Token: 0x04001551 RID: 5457
		private static BitVector32.Section _typeIdLowSection = BitVector32.CreateSection(255, BamlVariableSizedRecord.LastFlagsSection);

		// Token: 0x04001552 RID: 5458
		private static BitVector32.Section _typeIdHighSection = BitVector32.CreateSection(255, BamlTypeInfoRecord._typeIdLowSection);

		// Token: 0x04001553 RID: 5459
		private BamlTypeInfoRecord.TypeInfoFlags _typeInfoFlags;

		// Token: 0x04001554 RID: 5460
		private short _assemblyId = -1;

		// Token: 0x04001555 RID: 5461
		private string _typeFullName;

		// Token: 0x04001556 RID: 5462
		private Type _type;

		// Token: 0x0200088E RID: 2190
		[Flags]
		private enum TypeInfoFlags : byte
		{
			// Token: 0x04004175 RID: 16757
			Internal = 1,
			// Token: 0x04004176 RID: 16758
			UnusedTwo = 2,
			// Token: 0x04004177 RID: 16759
			UnusedThree = 4
		}
	}
}
