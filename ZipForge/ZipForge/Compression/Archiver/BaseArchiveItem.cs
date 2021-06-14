using System;
using System.IO;
using ComponentAce.Compression.Interfaces;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x0200001E RID: 30
	public abstract class BaseArchiveItem : IArchiveItem
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000135 RID: 309 RVA: 0x0000FC0C File Offset: 0x0000EC0C
		// (set) Token: 0x06000136 RID: 310 RVA: 0x0000FC14 File Offset: 0x0000EC14
		public string FileName
		{
			get
			{
				return this._fileName;
			}
			set
			{
				if (value.IndexOf("..\\") > -1 || value.IndexOf("\\..") > -1)
				{
					value = Path.GetFullPath(value);
				}
				if (CompressionUtils.IsNullOrEmpty(value))
				{
					value = string.Empty;
				}
				this._fileName = Path.GetFileName(value);
				string text = value.Replace('\\', '/');
				int num = text.LastIndexOf("/");
				if (num >= 0)
				{
					this.storedPath = text.Substring(0, num);
				}
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000137 RID: 311 RVA: 0x0000FC89 File Offset: 0x0000EC89
		// (set) Token: 0x06000138 RID: 312 RVA: 0x0000FC91 File Offset: 0x0000EC91
		public string SrcFileName
		{
			get
			{
				return this._srcFileName;
			}
			set
			{
				if (CompressionUtils.IsNullOrEmpty(value))
				{
					value = string.Empty;
				}
				if (value.IndexOf("..\\") > -1 || value.IndexOf("\\..") > -1)
				{
					value = Path.GetFullPath(value);
				}
				this._srcFileName = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000139 RID: 313 RVA: 0x0000FCCD File Offset: 0x0000ECCD
		public long UncompressedSize
		{
			get
			{
				return this._uncompressedSize;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600013A RID: 314 RVA: 0x0000FCD5 File Offset: 0x0000ECD5
		// (set) Token: 0x0600013B RID: 315 RVA: 0x0000FCDD File Offset: 0x0000ECDD
		public DateTime FileModificationDateTime
		{
			get
			{
				return this.fileModificationDateTime;
			}
			set
			{
				this.fileModificationDateTime = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000FCE8 File Offset: 0x0000ECE8
		public virtual string FullName
		{
			get
			{
				string str = this.storedPath;
				if (this.storedPath != "")
				{
					str += "/";
				}
				return str + this.FileName;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600013D RID: 317 RVA: 0x0000FD26 File Offset: 0x0000ED26
		// (set) Token: 0x0600013E RID: 318 RVA: 0x0000FD2E File Offset: 0x0000ED2E
		public FileAttributes ExternalFileAttributes
		{
			get
			{
				return this._externalFileAttributes;
			}
			set
			{
				this._externalFileAttributes = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600013F RID: 319 RVA: 0x0000FD37 File Offset: 0x0000ED37
		// (set) Token: 0x06000140 RID: 320 RVA: 0x0000FD4E File Offset: 0x0000ED4E
		public string StoredPath
		{
			get
			{
				return this.storedPath.Replace("/", "\\");
			}
			set
			{
				this.IsCustomPath = true;
				this.storedPath = value.Replace("\\", "/");
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000FD70 File Offset: 0x0000ED70
		public virtual void Reset()
		{
			this.IsCustomPath = false;
			this.FileName = string.Empty;
			this.SrcFileName = string.Empty;
			this.storedPath = string.Empty;
			this._uncompressedSize = 0L;
			this.FileModificationDateTime = DateTime.Now;
			this.ExternalFileAttributes = (FileAttributes)0;
			this.Handle = null;
			this.Handle = new InternalSearchRec();
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000FDD1 File Offset: 0x0000EDD1
		protected BaseArchiveItem()
		{
			this.Reset();
		}

		// Token: 0x040000B8 RID: 184
		private string _fileName;

		// Token: 0x040000B9 RID: 185
		private string _srcFileName;

		// Token: 0x040000BA RID: 186
		internal string storedPath;

		// Token: 0x040000BB RID: 187
		internal bool IsCustomPath;

		// Token: 0x040000BC RID: 188
		internal long _uncompressedSize;

		// Token: 0x040000BD RID: 189
		internal DateTime fileModificationDateTime = default(DateTime);

		// Token: 0x040000BE RID: 190
		private FileAttributes _externalFileAttributes;

		// Token: 0x040000BF RID: 191
		internal InternalSearchRec Handle;
	}
}
