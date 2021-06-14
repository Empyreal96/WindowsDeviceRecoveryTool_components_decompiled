using System;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x020000DF RID: 223
	public sealed class SharedAccessFileHeaders
	{
		// Token: 0x060011C4 RID: 4548 RVA: 0x000423E6 File Offset: 0x000405E6
		public SharedAccessFileHeaders()
		{
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x000423F0 File Offset: 0x000405F0
		public SharedAccessFileHeaders(SharedAccessFileHeaders sharedAccessFileHeaders)
		{
			CommonUtility.AssertNotNull("sharedAccessFileHeaders", sharedAccessFileHeaders);
			this.ContentType = sharedAccessFileHeaders.ContentType;
			this.ContentDisposition = sharedAccessFileHeaders.ContentDisposition;
			this.ContentEncoding = sharedAccessFileHeaders.ContentEncoding;
			this.ContentLanguage = sharedAccessFileHeaders.ContentLanguage;
			this.CacheControl = sharedAccessFileHeaders.CacheControl;
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060011C6 RID: 4550 RVA: 0x0004244A File Offset: 0x0004064A
		// (set) Token: 0x060011C7 RID: 4551 RVA: 0x00042452 File Offset: 0x00040652
		public string CacheControl { get; set; }

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060011C8 RID: 4552 RVA: 0x0004245B File Offset: 0x0004065B
		// (set) Token: 0x060011C9 RID: 4553 RVA: 0x00042463 File Offset: 0x00040663
		public string ContentDisposition { get; set; }

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060011CA RID: 4554 RVA: 0x0004246C File Offset: 0x0004066C
		// (set) Token: 0x060011CB RID: 4555 RVA: 0x00042474 File Offset: 0x00040674
		public string ContentEncoding { get; set; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x0004247D File Offset: 0x0004067D
		// (set) Token: 0x060011CD RID: 4557 RVA: 0x00042485 File Offset: 0x00040685
		public string ContentLanguage { get; set; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060011CE RID: 4558 RVA: 0x0004248E File Offset: 0x0004068E
		// (set) Token: 0x060011CF RID: 4559 RVA: 0x00042496 File Offset: 0x00040696
		public string ContentType { get; set; }
	}
}
