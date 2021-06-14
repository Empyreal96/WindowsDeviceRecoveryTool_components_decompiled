using System;
using System.ComponentModel;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;

namespace ComponentAce.Compression.GZip
{
	// Token: 0x02000039 RID: 57
	[ToolboxItem(false)]
	public class GzipArchiveOptions : Component
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600022B RID: 555 RVA: 0x000167EB File Offset: 0x000157EB
		// (set) Token: 0x0600022C RID: 556 RVA: 0x00016808 File Offset: 0x00015808
		[Description("Specifies directory to store gzipped files.")]
		public string OutputFilesDir
		{
			get
			{
				if (CompressionUtils.IsNullOrEmpty(this._outputFilesDir))
				{
					ExceptionBuilder.Exception(ErrorCode.OutputFilesDirNotSpecified);
				}
				return this._outputFilesDir;
			}
			set
			{
				this._outputFilesDir = value;
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00016811 File Offset: 0x00015811
		public GzipArchiveOptions()
		{
			this.OutputFilesDir = string.Empty;
			this.CreateDirs = true;
			this.ReplaceReadOnly = true;
			this.Overwrite = OverwriteMode.Always;
		}

		// Token: 0x040001A0 RID: 416
		private string _outputFilesDir;

		// Token: 0x040001A1 RID: 417
		[Description("Specifies whether to create folders when store gzipped files.")]
		public bool CreateDirs;

		// Token: 0x040001A2 RID: 418
		[Description("Specifies whether to replace read-only files when store gzipped files.")]
		public bool ReplaceReadOnly;

		// Token: 0x040001A3 RID: 419
		[Description("Specifies whether stored gzipped files will overwrite existing files.")]
		public OverwriteMode Overwrite;
	}
}
