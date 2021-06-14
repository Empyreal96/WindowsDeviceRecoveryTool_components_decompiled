using System;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000C0 RID: 192
	public sealed class SharedAccessBlobHeaders
	{
		// Token: 0x060010ED RID: 4333 RVA: 0x0003F154 File Offset: 0x0003D354
		public SharedAccessBlobHeaders()
		{
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x0003F15C File Offset: 0x0003D35C
		public SharedAccessBlobHeaders(SharedAccessBlobHeaders sharedAccessBlobHeaders)
		{
			CommonUtility.AssertNotNull("sharedAccessBlobHeaders", sharedAccessBlobHeaders);
			this.ContentType = sharedAccessBlobHeaders.ContentType;
			this.ContentDisposition = sharedAccessBlobHeaders.ContentDisposition;
			this.ContentEncoding = sharedAccessBlobHeaders.ContentEncoding;
			this.ContentLanguage = sharedAccessBlobHeaders.ContentLanguage;
			this.CacheControl = sharedAccessBlobHeaders.CacheControl;
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x0003F1B6 File Offset: 0x0003D3B6
		// (set) Token: 0x060010F0 RID: 4336 RVA: 0x0003F1BE File Offset: 0x0003D3BE
		public string CacheControl { get; set; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060010F1 RID: 4337 RVA: 0x0003F1C7 File Offset: 0x0003D3C7
		// (set) Token: 0x060010F2 RID: 4338 RVA: 0x0003F1CF File Offset: 0x0003D3CF
		public string ContentDisposition { get; set; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x0003F1D8 File Offset: 0x0003D3D8
		// (set) Token: 0x060010F4 RID: 4340 RVA: 0x0003F1E0 File Offset: 0x0003D3E0
		public string ContentEncoding { get; set; }

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x0003F1E9 File Offset: 0x0003D3E9
		// (set) Token: 0x060010F6 RID: 4342 RVA: 0x0003F1F1 File Offset: 0x0003D3F1
		public string ContentLanguage { get; set; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060010F7 RID: 4343 RVA: 0x0003F1FA File Offset: 0x0003D3FA
		// (set) Token: 0x060010F8 RID: 4344 RVA: 0x0003F202 File Offset: 0x0003D402
		public string ContentType { get; set; }
	}
}
