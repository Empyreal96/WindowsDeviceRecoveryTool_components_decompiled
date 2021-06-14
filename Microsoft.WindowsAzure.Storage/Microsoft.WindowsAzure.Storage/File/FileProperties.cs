using System;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x020000D8 RID: 216
	public sealed class FileProperties
	{
		// Token: 0x06001183 RID: 4483 RVA: 0x00041C93 File Offset: 0x0003FE93
		public FileProperties()
		{
			this.Length = -1L;
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x00041CA4 File Offset: 0x0003FEA4
		public FileProperties(FileProperties other)
		{
			CommonUtility.AssertNotNull("other", other);
			this.ContentType = other.ContentType;
			this.ContentDisposition = other.ContentDisposition;
			this.ContentEncoding = other.ContentEncoding;
			this.ContentLanguage = other.ContentLanguage;
			this.CacheControl = other.CacheControl;
			this.ContentMD5 = other.ContentMD5;
			this.Length = other.Length;
			this.ETag = other.ETag;
			this.LastModified = other.LastModified;
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06001185 RID: 4485 RVA: 0x00041D2E File Offset: 0x0003FF2E
		// (set) Token: 0x06001186 RID: 4486 RVA: 0x00041D36 File Offset: 0x0003FF36
		public string CacheControl { get; set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06001187 RID: 4487 RVA: 0x00041D3F File Offset: 0x0003FF3F
		// (set) Token: 0x06001188 RID: 4488 RVA: 0x00041D47 File Offset: 0x0003FF47
		public string ContentDisposition { get; set; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06001189 RID: 4489 RVA: 0x00041D50 File Offset: 0x0003FF50
		// (set) Token: 0x0600118A RID: 4490 RVA: 0x00041D58 File Offset: 0x0003FF58
		public string ContentEncoding { get; set; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x0600118B RID: 4491 RVA: 0x00041D61 File Offset: 0x0003FF61
		// (set) Token: 0x0600118C RID: 4492 RVA: 0x00041D69 File Offset: 0x0003FF69
		public string ContentLanguage { get; set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x0600118D RID: 4493 RVA: 0x00041D72 File Offset: 0x0003FF72
		// (set) Token: 0x0600118E RID: 4494 RVA: 0x00041D7A File Offset: 0x0003FF7A
		public long Length { get; internal set; }

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x0600118F RID: 4495 RVA: 0x00041D83 File Offset: 0x0003FF83
		// (set) Token: 0x06001190 RID: 4496 RVA: 0x00041D8B File Offset: 0x0003FF8B
		public string ContentMD5 { get; set; }

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06001191 RID: 4497 RVA: 0x00041D94 File Offset: 0x0003FF94
		// (set) Token: 0x06001192 RID: 4498 RVA: 0x00041D9C File Offset: 0x0003FF9C
		public string ContentType { get; set; }

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06001193 RID: 4499 RVA: 0x00041DA5 File Offset: 0x0003FFA5
		// (set) Token: 0x06001194 RID: 4500 RVA: 0x00041DAD File Offset: 0x0003FFAD
		public string ETag { get; internal set; }

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06001195 RID: 4501 RVA: 0x00041DB6 File Offset: 0x0003FFB6
		// (set) Token: 0x06001196 RID: 4502 RVA: 0x00041DBE File Offset: 0x0003FFBE
		public DateTimeOffset? LastModified { get; internal set; }
	}
}
