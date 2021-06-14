using System;
using System.IO;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;

namespace ComponentAce.Compression.Tar
{
	// Token: 0x02000061 RID: 97
	public class TarArchiveItem : BaseArchiveItem
	{
		// Token: 0x0600041D RID: 1053 RVA: 0x0001F438 File Offset: 0x0001E438
		public TarArchiveItem()
		{
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0001F440 File Offset: 0x0001E440
		public TarArchiveItem(string fileName) : this(fileName, string.Empty, StorePathMode.RelativePath)
		{
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0001F450 File Offset: 0x0001E450
		public TarArchiveItem(string fileName, string baseDir, StorePathMode storePathMode)
		{
			if (fileName.IndexOf("..\\") > -1 || fileName.IndexOf("\\..") > -1)
			{
				fileName = Path.GetFullPath(fileName);
			}
			base.SrcFileName = fileName;
			string archiveFileName = CompressionUtils.GetArchiveFileName(fileName, baseDir, storePathMode);
			bool flag = FileUtils.DirectotyExists(fileName);
			if (!flag && !File.Exists(fileName))
			{
				throw ExceptionBuilder.Exception(ErrorCode.FileNotFound, new object[]
				{
					fileName
				});
			}
			if (!flag)
			{
				base.FileName = Path.GetFileName(archiveFileName);
				this.storedPath = Path.GetDirectoryName(archiveFileName).Replace('\\', '/');
				FileInfo fileInfo = new FileInfo(fileName);
				this._uncompressedSize = fileInfo.Length;
				base.FileModificationDateTime = fileInfo.LastWriteTime;
				base.ExternalFileAttributes = fileInfo.Attributes;
			}
			else
			{
				base.FileName = "";
				this.storedPath = archiveFileName;
				DirectoryInfo directoryInfo = new DirectoryInfo(fileName);
				this._uncompressedSize = 0L;
				base.FileModificationDateTime = directoryInfo.LastWriteTime;
				base.ExternalFileAttributes = directoryInfo.Attributes;
			}
			this.Handle = new InternalSearchRec();
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x0001F554 File Offset: 0x0001E554
		public override string FullName
		{
			get
			{
				string text = this.storedPath;
				if (this.storedPath != "")
				{
					text += "/";
				}
				return Path.Combine(text, base.FileName);
			}
		}
	}
}
