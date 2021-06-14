using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using ComponentAce.Compression.Archiver;

namespace ComponentAce.Compression.Tar
{
	// Token: 0x02000062 RID: 98
	[ToolboxBitmap(typeof(resfinder), "ComponentAce.Compression.Resources.tar16.ico")]
	[ToolboxItem(true)]
	public class TarForge : TarBaseForge
	{
		// Token: 0x06000421 RID: 1057 RVA: 0x0001F592 File Offset: 0x0001E592
		public void AddItem(TarArchiveItem item)
		{
			base.AddItem(item);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001F59C File Offset: 0x0001E59C
		public bool FindFirst(ref TarArchiveItem archiveItem)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindFirst(ref baseArchiveItem);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001F5B4 File Offset: 0x0001E5B4
		public bool FindFirst(string fileMask, ref TarArchiveItem archiveItem)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindFirst(fileMask, ref baseArchiveItem);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0001F5D0 File Offset: 0x0001E5D0
		public bool FindFirst(string fileMask, ref TarArchiveItem archiveItem, FileAttributes searchAttr)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindFirst(fileMask, ref baseArchiveItem, searchAttr);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001F5EC File Offset: 0x0001E5EC
		public bool FindFirst(string fileMask, ref TarArchiveItem archiveItem, FileAttributes searchAttr, string exclusionMask)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindFirst(fileMask, ref baseArchiveItem, searchAttr, exclusionMask);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001F608 File Offset: 0x0001E608
		public bool FindNext(ref TarArchiveItem archiveItem)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindNext(ref baseArchiveItem);
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000427 RID: 1063 RVA: 0x0001F620 File Offset: 0x0001E620
		// (remove) Token: 0x06000428 RID: 1064 RVA: 0x0001F639 File Offset: 0x0001E639
		[Description("Occurs when file is being extracted from archive.")]
		public event OnExtractFileDelegate OnExtractFile;

		// Token: 0x06000429 RID: 1065 RVA: 0x0001F654 File Offset: 0x0001E654
		protected internal override void DoOnExtractFile(ref BaseArchiveItem baseItem)
		{
			if (this.OnExtractFile != null)
			{
				TarArchiveItem tarArchiveItem = baseItem as TarArchiveItem;
				tarArchiveItem.FileName = tarArchiveItem.FileName.Replace('/', '\\');
				this.OnExtractFile(this, ref tarArchiveItem);
				tarArchiveItem.FileName = tarArchiveItem.FileName.Replace('\\', '/');
			}
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x0600042A RID: 1066 RVA: 0x0001F6A9 File Offset: 0x0001E6A9
		// (remove) Token: 0x0600042B RID: 1067 RVA: 0x0001F6C2 File Offset: 0x0001E6C2
		[Description("Occurs when file is being stored into the archive.")]
		public event OnStoreFileDelegate OnStoreFile;

		// Token: 0x0600042C RID: 1068 RVA: 0x0001F6DC File Offset: 0x0001E6DC
		protected internal override void DoOnStoreFile(ref BaseArchiveItem baseItem)
		{
			if (this.OnStoreFile != null)
			{
				TarArchiveItem tarArchiveItem = baseItem as TarArchiveItem;
				tarArchiveItem.FileName = tarArchiveItem.FileName.Replace('/', '\\');
				this.OnStoreFile(this, ref tarArchiveItem);
				tarArchiveItem.FileName = tarArchiveItem.FileName.Replace('\\', '/');
			}
		}
	}
}
