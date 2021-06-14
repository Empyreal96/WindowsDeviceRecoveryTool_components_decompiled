using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000203 RID: 515
	internal class BamlDocumentStartRecord : BamlRecord
	{
		// Token: 0x0600204A RID: 8266 RVA: 0x00096078 File Offset: 0x00094278
		internal override void Write(BinaryWriter bamlBinaryWriter)
		{
			if (this.FilePos == -1L && bamlBinaryWriter != null)
			{
				this.FilePos = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			}
			base.Write(bamlBinaryWriter);
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x0009609C File Offset: 0x0009429C
		internal virtual void UpdateWrite(BinaryWriter bamlBinaryWriter)
		{
			long num = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			bamlBinaryWriter.Seek((int)this.FilePos, SeekOrigin.Begin);
			this.Write(bamlBinaryWriter);
			bamlBinaryWriter.Seek((int)num, SeekOrigin.Begin);
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x000960D2 File Offset: 0x000942D2
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.LoadAsync = bamlBinaryReader.ReadBoolean();
			this.MaxAsyncRecords = bamlBinaryReader.ReadInt32();
			this.DebugBaml = bamlBinaryReader.ReadBoolean();
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000960F8 File Offset: 0x000942F8
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.LoadAsync);
			bamlBinaryWriter.Write(this.MaxAsyncRecords);
			bamlBinaryWriter.Write(this.DebugBaml);
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x00096120 File Offset: 0x00094320
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlDocumentStartRecord bamlDocumentStartRecord = (BamlDocumentStartRecord)record;
			bamlDocumentStartRecord._maxAsyncRecords = this._maxAsyncRecords;
			bamlDocumentStartRecord._loadAsync = this._loadAsync;
			bamlDocumentStartRecord._filePos = this._filePos;
			bamlDocumentStartRecord._debugBaml = this._debugBaml;
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x0600204F RID: 8271 RVA: 0x00016748 File Offset: 0x00014948
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.DocumentStart;
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06002050 RID: 8272 RVA: 0x0009616B File Offset: 0x0009436B
		// (set) Token: 0x06002051 RID: 8273 RVA: 0x00096173 File Offset: 0x00094373
		internal bool LoadAsync
		{
			get
			{
				return this._loadAsync;
			}
			set
			{
				this._loadAsync = value;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06002052 RID: 8274 RVA: 0x0009617C File Offset: 0x0009437C
		// (set) Token: 0x06002053 RID: 8275 RVA: 0x00096184 File Offset: 0x00094384
		internal int MaxAsyncRecords
		{
			get
			{
				return this._maxAsyncRecords;
			}
			set
			{
				this._maxAsyncRecords = value;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06002054 RID: 8276 RVA: 0x0009618D File Offset: 0x0009438D
		// (set) Token: 0x06002055 RID: 8277 RVA: 0x00096195 File Offset: 0x00094395
		internal long FilePos
		{
			get
			{
				return this._filePos;
			}
			set
			{
				this._filePos = value;
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06002056 RID: 8278 RVA: 0x0009619E File Offset: 0x0009439E
		// (set) Token: 0x06002057 RID: 8279 RVA: 0x000961A6 File Offset: 0x000943A6
		internal bool DebugBaml
		{
			get
			{
				return this._debugBaml;
			}
			set
			{
				this._debugBaml = value;
			}
		}

		// Token: 0x04001549 RID: 5449
		private int _maxAsyncRecords = -1;

		// Token: 0x0400154A RID: 5450
		private bool _loadAsync;

		// Token: 0x0400154B RID: 5451
		private long _filePos = -1L;

		// Token: 0x0400154C RID: 5452
		private bool _debugBaml;
	}
}
