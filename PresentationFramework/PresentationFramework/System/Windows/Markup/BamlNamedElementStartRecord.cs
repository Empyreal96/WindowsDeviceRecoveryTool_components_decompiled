using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001F9 RID: 505
	internal class BamlNamedElementStartRecord : BamlElementStartRecord
	{
		// Token: 0x06001FFD RID: 8189 RVA: 0x00095B19 File Offset: 0x00093D19
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.TypeId = bamlBinaryReader.ReadInt16();
			this.RuntimeName = bamlBinaryReader.ReadString();
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x00095B33 File Offset: 0x00093D33
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(base.TypeId);
			if (this.RuntimeName != null)
			{
				bamlBinaryWriter.Write(this.RuntimeName);
			}
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x00095B58 File Offset: 0x00093D58
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlNamedElementStartRecord bamlNamedElementStartRecord = (BamlNamedElementStartRecord)record;
			bamlNamedElementStartRecord._isTemplateChild = this._isTemplateChild;
			bamlNamedElementStartRecord._runtimeName = this._runtimeName;
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06002000 RID: 8192 RVA: 0x00095B8B File Offset: 0x00093D8B
		// (set) Token: 0x06002001 RID: 8193 RVA: 0x00095B93 File Offset: 0x00093D93
		internal string RuntimeName
		{
			get
			{
				return this._runtimeName;
			}
			set
			{
				this._runtimeName = value;
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06002002 RID: 8194 RVA: 0x00095B9C File Offset: 0x00093D9C
		// (set) Token: 0x06002003 RID: 8195 RVA: 0x00095BA4 File Offset: 0x00093DA4
		internal bool IsTemplateChild
		{
			get
			{
				return this._isTemplateChild;
			}
			set
			{
				this._isTemplateChild = value;
			}
		}

		// Token: 0x0400153A RID: 5434
		private bool _isTemplateChild;

		// Token: 0x0400153B RID: 5435
		private string _runtimeName;
	}
}
