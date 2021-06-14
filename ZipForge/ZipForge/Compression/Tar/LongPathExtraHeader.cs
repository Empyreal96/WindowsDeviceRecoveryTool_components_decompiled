using System;
using System.IO;
using System.Text;
using ComponentAce.Compression.Archiver;

namespace ComponentAce.Compression.Tar
{
	// Token: 0x0200005C RID: 92
	internal class LongPathExtraHeader
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060003CB RID: 971 RVA: 0x0001E6DA File Offset: 0x0001D6DA
		// (set) Token: 0x060003CC RID: 972 RVA: 0x0001E6E2 File Offset: 0x0001D6E2
		public string FileName
		{
			get
			{
				return this._fileName;
			}
			set
			{
				this._fileName = value;
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0001E6EC File Offset: 0x0001D6EC
		public LongPathExtraHeader(int codepage, DoOnStreamOperationFailureDelegate writeToStreamFailureDelegate, DoOnStreamOperationFailureDelegate readFromStreamFailureDelegate)
		{
			this._codepage = codepage;
			this._header = new OldStyleHeader(codepage);
			this._header.FileName = "././@LongLink";
			this._header.TypeFlag = 'L';
			this._header.LastModification = DateTime.Now;
			this._writeToStreamFailureDelegate = writeToStreamFailureDelegate;
			this._readFromStreamFailureDelegate = readFromStreamFailureDelegate;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001E750 File Offset: 0x0001D750
		public void Write(Stream stream)
		{
			MemoryStream data = new MemoryStream(Encoding.GetEncoding(this._codepage).GetBytes(this._fileName));
			LegacyTarWriter legacyTarWriter = new LegacyTarWriter(stream, this._codepage, this._writeToStreamFailureDelegate, this._readFromStreamFailureDelegate);
			legacyTarWriter.Write(data, (long)this._fileName.Length, this._header.FileName, this._header.UserId, this._header.GroupId, this._header.Mode, this._header.LastModification, this._header.TypeFlag);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0001E7E7 File Offset: 0x0001D7E7
		public int SizeOf()
		{
			return 512 + this._fileName.Length / 512 * 512 + ((this._fileName.Length % 512 == 0) ? 0 : 512);
		}

		// Token: 0x0400027E RID: 638
		private readonly int _codepage;

		// Token: 0x0400027F RID: 639
		private readonly OldStyleHeader _header;

		// Token: 0x04000280 RID: 640
		private readonly DoOnStreamOperationFailureDelegate _writeToStreamFailureDelegate;

		// Token: 0x04000281 RID: 641
		private readonly DoOnStreamOperationFailureDelegate _readFromStreamFailureDelegate;

		// Token: 0x04000282 RID: 642
		private string _fileName;
	}
}
