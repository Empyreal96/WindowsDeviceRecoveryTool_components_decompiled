using System;
using ComponentAce.Compression.Archiver;

namespace ComponentAce.Compression.ZipForgeRealTime
{
	// Token: 0x0200006F RID: 111
	public class RealTimeArchiverParameters
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x000215BF File Offset: 0x000205BF
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x000215C7 File Offset: 0x000205C7
		public string CommentsToArchive
		{
			get
			{
				return this._commentsToArchive;
			}
			set
			{
				this._commentsToArchive = (CompressionUtils.IsNullOrEmpty(value) ? string.Empty : value);
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x000215DF File Offset: 0x000205DF
		// (set) Token: 0x060004BC RID: 1212 RVA: 0x000215E7 File Offset: 0x000205E7
		public RealTimeCompressionLevel CompressionLevel
		{
			get
			{
				return this._compressionLevel;
			}
			set
			{
				this._compressionLevel = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x000215F0 File Offset: 0x000205F0
		// (set) Token: 0x060004BE RID: 1214 RVA: 0x000215F8 File Offset: 0x000205F8
		public RealTimeCompressionMethod CompressionMethod
		{
			get
			{
				return this._compressionMethod;
			}
			set
			{
				this._compressionMethod = value;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x00021601 File Offset: 0x00020601
		// (set) Token: 0x060004C0 RID: 1216 RVA: 0x00021609 File Offset: 0x00020609
		public RealTimeEncryptionAlgorithm EncryptionAlgorithm
		{
			get
			{
				return this._encryptionAlgorithm;
			}
			set
			{
				this._encryptionAlgorithm = value;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x00021612 File Offset: 0x00020612
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x0002161A File Offset: 0x0002061A
		public string EncryptionPassword
		{
			get
			{
				return this._encryptionPassword;
			}
			set
			{
				this._encryptionPassword = value;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00021623 File Offset: 0x00020623
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x0002162B File Offset: 0x0002062B
		public bool UseUnicodeFileNameExtraField
		{
			get
			{
				return this._useUnicodeFileNameExtraField;
			}
			set
			{
				this._useUnicodeFileNameExtraField = value;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00021634 File Offset: 0x00020634
		// (set) Token: 0x060004C6 RID: 1222 RVA: 0x0002163C File Offset: 0x0002063C
		public bool UseZip64
		{
			get
			{
				return this._useZip64;
			}
			set
			{
				this._useZip64 = value;
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00021648 File Offset: 0x00020648
		public RealTimeArchiverParameters()
		{
			this.CommentsToArchive = string.Empty;
			this.CompressionLevel = RealTimeCompressionLevel.Fastest;
			this.CompressionMethod = RealTimeCompressionMethod.Deflate;
			this.EncryptionAlgorithm = RealTimeEncryptionAlgorithm.None;
			this.EncryptionPassword = string.Empty;
			this.UseUnicodeFileNameExtraField = false;
			this.UseZip64 = false;
		}

		// Token: 0x040002CA RID: 714
		private string _commentsToArchive;

		// Token: 0x040002CB RID: 715
		private RealTimeCompressionLevel _compressionLevel;

		// Token: 0x040002CC RID: 716
		private RealTimeCompressionMethod _compressionMethod;

		// Token: 0x040002CD RID: 717
		private RealTimeEncryptionAlgorithm _encryptionAlgorithm;

		// Token: 0x040002CE RID: 718
		private string _encryptionPassword;

		// Token: 0x040002CF RID: 719
		private bool _useUnicodeFileNameExtraField;

		// Token: 0x040002D0 RID: 720
		private bool _useZip64;
	}
}
