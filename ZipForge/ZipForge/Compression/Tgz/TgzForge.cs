using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.GZip;
using ComponentAce.Compression.Tar;

namespace ComponentAce.Compression.Tgz
{
	// Token: 0x0200006B RID: 107
	[ToolboxBitmap(typeof(resfinder), "ComponentAce.Compression.Resources.tgz16.ico")]
	public class TgzForge : TarBaseForge
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x00020765 File Offset: 0x0001F765
		// (set) Token: 0x06000487 RID: 1159 RVA: 0x00020757 File Offset: 0x0001F757
		public override string FileName
		{
			get
			{
				return this._gzip.FileName;
			}
			set
			{
				this._gzip.FileName = value;
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00020772 File Offset: 0x0001F772
		public TgzForge()
		{
			this._gzip = new GzipForge();
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00020788 File Offset: 0x0001F788
		public override void OpenArchive()
		{
			this._gzip.OpenArchive();
			this._tmpFileName = base.GetTempFileName();
			if (this._gzip.FileCount == 1L)
			{
				using (FileStream fileStream = new FileStream(this._tmpFileName, FileMode.Open, FileAccess.ReadWrite))
				{
					this._gzip.ExtractItem(0, fileStream);
				}
			}
			this._fileName = this._tmpFileName;
			base.OpenArchive();
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00020804 File Offset: 0x0001F804
		public override void OpenArchive(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
		{
			this._gzip.OpenArchive(fileMode, fileAccess, fileShare);
			this._tmpFileName = base.GetTempFileName();
			if (this._gzip.FileCount == 1L)
			{
				using (FileStream fileStream = new FileStream(this._tmpFileName, FileMode.Open, FileAccess.ReadWrite))
				{
					this._gzip.ExtractItem(0, fileStream);
				}
			}
			this._fileName = this._tmpFileName;
			base.OpenArchive(fileMode, fileAccess, fileShare);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00020888 File Offset: 0x0001F888
		public override void OpenArchive(Stream stream, bool create)
		{
			this._gzip.OpenArchive(stream, create);
			this._tmpFileName = base.GetTempFileName();
			if (this._gzip.FileCount == 1L)
			{
				using (FileStream fileStream = new FileStream(this._tmpFileName, FileMode.Open, FileAccess.ReadWrite))
				{
					this._gzip.ExtractItem(0, fileStream);
				}
			}
			this._fileName = this._tmpFileName;
			base.OpenArchive(new FileStream(this._tmpFileName, FileMode.Open), create);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00020914 File Offset: 0x0001F914
		public override void CloseArchive()
		{
			if (this._isOpened)
			{
				base.CloseArchive();
				this._gzip.BeginUpdate();
				if (this._gzip.FileCount == 1L)
				{
					this._gzip.DeleteFiles("*.*");
				}
				this._gzip.AddFiles(this._tmpFileName);
				this._gzip.RenameFile(Path.GetFileName(this._tmpFileName), Path.GetFileNameWithoutExtension(this._gzip.FileName) + ".tar");
				this._gzip.EndUpdate();
				this._gzip.CloseArchive();
				try
				{
					if (!CompressionUtils.IsNullOrEmpty(this._tmpFileName) && File.Exists(this._tmpFileName))
					{
						File.Delete(this._tmpFileName);
					}
					return;
				}
				catch
				{
					return;
				}
			}
			base.CloseArchive();
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x000209F4 File Offset: 0x0001F9F4
		public void AddItem(TgzArchiveItem item)
		{
			base.AddItem(item);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00020A00 File Offset: 0x0001FA00
		public bool FindFirst(ref TgzArchiveItem archiveItem)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindFirst(ref baseArchiveItem);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00020A18 File Offset: 0x0001FA18
		public bool FindFirst(string fileMask, ref TgzArchiveItem archiveItem)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindFirst(fileMask, ref baseArchiveItem);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00020A34 File Offset: 0x0001FA34
		public bool FindFirst(string fileMask, ref TgzArchiveItem archiveItem, FileAttributes searchAttr)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindFirst(fileMask, ref baseArchiveItem, searchAttr);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00020A50 File Offset: 0x0001FA50
		public bool FindFirst(string fileMask, ref TgzArchiveItem archiveItem, FileAttributes searchAttr, string exclusionMask)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindFirst(fileMask, ref baseArchiveItem, searchAttr, exclusionMask);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00020A6C File Offset: 0x0001FA6C
		public bool FindNext(ref TgzArchiveItem archiveItem)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindNext(ref baseArchiveItem);
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000494 RID: 1172 RVA: 0x00020A84 File Offset: 0x0001FA84
		// (remove) Token: 0x06000495 RID: 1173 RVA: 0x00020A9D File Offset: 0x0001FA9D
		[Description("Occurs when file is being extracted from archive.")]
		public event OnExtractFileDelegate OnExtractFile;

		// Token: 0x06000496 RID: 1174 RVA: 0x00020AB8 File Offset: 0x0001FAB8
		protected internal override void DoOnExtractFile(ref BaseArchiveItem baseItem)
		{
			if (this.OnExtractFile != null)
			{
				TgzArchiveItem tgzArchiveItem = baseItem as TgzArchiveItem;
				tgzArchiveItem.FileName = tgzArchiveItem.FileName.Replace('/', '\\');
				this.OnExtractFile(this, ref tgzArchiveItem);
				tgzArchiveItem.FileName = tgzArchiveItem.FileName.Replace('\\', '/');
			}
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000497 RID: 1175 RVA: 0x00020B0D File Offset: 0x0001FB0D
		// (remove) Token: 0x06000498 RID: 1176 RVA: 0x00020B26 File Offset: 0x0001FB26
		[Description("Occurs when file is being stored into the archive.")]
		public event OnStoreFileDelegate OnStoreFile;

		// Token: 0x06000499 RID: 1177 RVA: 0x00020B40 File Offset: 0x0001FB40
		protected internal override void DoOnStoreFile(ref BaseArchiveItem baseItem)
		{
			if (this.OnStoreFile != null)
			{
				TgzArchiveItem tgzArchiveItem = baseItem as TgzArchiveItem;
				tgzArchiveItem.FileName = tgzArchiveItem.FileName.Replace('/', '\\');
				this.OnStoreFile(this, ref tgzArchiveItem);
				tgzArchiveItem.FileName = tgzArchiveItem.FileName.Replace('\\', '/');
			}
		}

		// Token: 0x040002C4 RID: 708
		private readonly GzipForge _gzip;

		// Token: 0x040002C5 RID: 709
		private string _tmpFileName;
	}
}
