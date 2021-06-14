using System;
using System.Collections.Specialized;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001DB RID: 475
	internal class BamlPIMappingRecord : BamlVariableSizedRecord
	{
		// Token: 0x06001EFA RID: 7930 RVA: 0x0009448E File Offset: 0x0009268E
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.XmlNamespace = bamlBinaryReader.ReadString();
			this.ClrNamespace = bamlBinaryReader.ReadString();
			this.AssemblyId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x000944B4 File Offset: 0x000926B4
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.XmlNamespace);
			bamlBinaryWriter.Write(this.ClrNamespace);
			bamlBinaryWriter.Write(this.AssemblyId);
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x000944DC File Offset: 0x000926DC
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPIMappingRecord bamlPIMappingRecord = (BamlPIMappingRecord)record;
			bamlPIMappingRecord._xmlns = this._xmlns;
			bamlPIMappingRecord._clrns = this._clrns;
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06001EFD RID: 7933 RVA: 0x0009450F File Offset: 0x0009270F
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PIMapping;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06001EFE RID: 7934 RVA: 0x00094513 File Offset: 0x00092713
		// (set) Token: 0x06001EFF RID: 7935 RVA: 0x0009451B File Offset: 0x0009271B
		internal string XmlNamespace
		{
			get
			{
				return this._xmlns;
			}
			set
			{
				this._xmlns = value;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06001F00 RID: 7936 RVA: 0x00094524 File Offset: 0x00092724
		// (set) Token: 0x06001F01 RID: 7937 RVA: 0x0009452C File Offset: 0x0009272C
		internal string ClrNamespace
		{
			get
			{
				return this._clrns;
			}
			set
			{
				this._clrns = value;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x00094538 File Offset: 0x00092738
		// (set) Token: 0x06001F03 RID: 7939 RVA: 0x0009456F File Offset: 0x0009276F
		internal short AssemblyId
		{
			get
			{
				short num = (short)this._flags[BamlPIMappingRecord._assemblyIdLowSection];
				return num | (short)(this._flags[BamlPIMappingRecord._assemblyIdHighSection] << 8);
			}
			set
			{
				this._flags[BamlPIMappingRecord._assemblyIdLowSection] = (int)(value & 255);
				this._flags[BamlPIMappingRecord._assemblyIdHighSection] = (int)((short)(((int)value & 65280) >> 8));
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06001F04 RID: 7940 RVA: 0x000945A3 File Offset: 0x000927A3
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlPIMappingRecord._assemblyIdHighSection;
			}
		}

		// Token: 0x04001500 RID: 5376
		private static BitVector32.Section _assemblyIdLowSection = BitVector32.CreateSection(255, BamlVariableSizedRecord.LastFlagsSection);

		// Token: 0x04001501 RID: 5377
		private static BitVector32.Section _assemblyIdHighSection = BitVector32.CreateSection(255, BamlPIMappingRecord._assemblyIdLowSection);

		// Token: 0x04001502 RID: 5378
		private string _xmlns;

		// Token: 0x04001503 RID: 5379
		private string _clrns;
	}
}
