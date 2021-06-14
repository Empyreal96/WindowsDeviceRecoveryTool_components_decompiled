using System;
using System.ComponentModel;
using System.IO;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x0200005D RID: 93
	[ToolboxItem(false)]
	public class ArchiverOptions : Component
	{
		// Token: 0x060003D0 RID: 976 RVA: 0x0001E824 File Offset: 0x0001D824
		public void Assign(object source)
		{
			if (source is ArchiverOptions)
			{
				ArchiverOptions archiverOptions = (ArchiverOptions)source;
				this._storePath = archiverOptions.StorePath;
				this._recurse = archiverOptions.Recurse;
				this._overwriteMode = archiverOptions.Overwrite;
				this._createDirs = archiverOptions.CreateDirs;
				this._replaceReadOnly = archiverOptions.ReplaceReadOnly;
				this._setAttributes = archiverOptions.SetAttributes;
				this._searchAttr = archiverOptions.SearchAttr;
				this._flushBuffers = archiverOptions.FlushBuffers;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x0001E8A0 File Offset: 0x0001D8A0
		// (set) Token: 0x060003D2 RID: 978 RVA: 0x0001E8A8 File Offset: 0x0001D8A8
		[Description("Specifies if archive operation will search files recursively.")]
		public bool Recurse
		{
			get
			{
				return this._recurse;
			}
			set
			{
				this._recurse = value;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x0001E8B1 File Offset: 0x0001D8B1
		// (set) Token: 0x060003D4 RID: 980 RVA: 0x0001E8B9 File Offset: 0x0001D8B9
		[Description("Defines how path information will be stored for the file or directory within the archive file.")]
		public StorePathMode StorePath
		{
			get
			{
				return this._storePath;
			}
			set
			{
				this._storePath = value;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x0001E8C2 File Offset: 0x0001D8C2
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x0001E8CA File Offset: 0x0001D8CA
		[Description("Specifies how the files being added to the archive will be opened for simultaneous access.")]
		public FileShare ShareMode
		{
			get
			{
				return this._shareMode;
			}
			set
			{
				this._shareMode = value;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x0001E8D3 File Offset: 0x0001D8D3
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x0001E8DB File Offset: 0x0001D8DB
		[Description("Specifies whether extracted files will overwrite existing files.")]
		public OverwriteMode Overwrite
		{
			get
			{
				return this._overwriteMode;
			}
			set
			{
				this._overwriteMode = value;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x0001E8E4 File Offset: 0x0001D8E4
		// (set) Token: 0x060003DA RID: 986 RVA: 0x0001E8EC File Offset: 0x0001D8EC
		[Description("Specifies whether to create folders when extracting an archive.")]
		public bool CreateDirs
		{
			get
			{
				return this._createDirs;
			}
			set
			{
				this._createDirs = value;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060003DB RID: 987 RVA: 0x0001E8F5 File Offset: 0x0001D8F5
		// (set) Token: 0x060003DC RID: 988 RVA: 0x0001E8FD File Offset: 0x0001D8FD
		[Description("Specifies whether to replace read-only files when extracting files from archive.")]
		public bool ReplaceReadOnly
		{
			get
			{
				return this._replaceReadOnly;
			}
			set
			{
				this._replaceReadOnly = value;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060003DD RID: 989 RVA: 0x0001E906 File Offset: 0x0001D906
		// (set) Token: 0x060003DE RID: 990 RVA: 0x0001E90E File Offset: 0x0001D90E
		[Description("Determines whether attributes stored within the archive will be applied to extracted files.")]
		public bool SetAttributes
		{
			get
			{
				return this._setAttributes;
			}
			set
			{
				this._setAttributes = value;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0001E917 File Offset: 0x0001D917
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x0001E91F File Offset: 0x0001D91F
		[Description("Specifies the special files to include in addition to all normal files.")]
		public FileAttributes SearchAttr
		{
			get
			{
				return this._searchAttr;
			}
			set
			{
				this._searchAttr = value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0001E928 File Offset: 0x0001D928
		// (set) Token: 0x060003E2 RID: 994 RVA: 0x0001E930 File Offset: 0x0001D930
		[Description("Specifies whether the file buffers are flushed after archive file modification and after extracting files from archive.")]
		public bool FlushBuffers
		{
			get
			{
				return this._flushBuffers;
			}
			set
			{
				this._flushBuffers = value;
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0001E93C File Offset: 0x0001D93C
		public ArchiverOptions()
		{
			this._storePath = StorePathMode.RelativePath;
			this._recurse = true;
			this._overwriteMode = OverwriteMode.Always;
			this._createDirs = true;
			this._replaceReadOnly = true;
			this._setAttributes = true;
			this._searchAttr = (FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System | FileAttributes.Directory | FileAttributes.Archive | FileAttributes.Device | FileAttributes.Normal | FileAttributes.Temporary | FileAttributes.SparseFile | FileAttributes.ReparsePoint | FileAttributes.Compressed | FileAttributes.Offline | FileAttributes.NotContentIndexed | FileAttributes.Encrypted);
			this._shareMode = FileShare.ReadWrite;
			this._flushBuffers = true;
		}

		// Token: 0x04000283 RID: 643
		private StorePathMode _storePath;

		// Token: 0x04000284 RID: 644
		private bool _recurse;

		// Token: 0x04000285 RID: 645
		private FileShare _shareMode;

		// Token: 0x04000286 RID: 646
		private OverwriteMode _overwriteMode;

		// Token: 0x04000287 RID: 647
		private bool _createDirs;

		// Token: 0x04000288 RID: 648
		private bool _replaceReadOnly;

		// Token: 0x04000289 RID: 649
		private bool _setAttributes;

		// Token: 0x0400028A RID: 650
		private FileAttributes _searchAttr;

		// Token: 0x0400028B RID: 651
		private bool _flushBuffers;
	}
}
