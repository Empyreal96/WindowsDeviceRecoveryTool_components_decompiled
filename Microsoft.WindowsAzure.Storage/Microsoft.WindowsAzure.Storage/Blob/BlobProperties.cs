using System;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000AE RID: 174
	public sealed class BlobProperties
	{
		// Token: 0x06001085 RID: 4229 RVA: 0x0003E786 File Offset: 0x0003C986
		public BlobProperties()
		{
			this.Length = -1L;
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0003E798 File Offset: 0x0003C998
		public BlobProperties(BlobProperties other)
		{
			CommonUtility.AssertNotNull("other", other);
			this.BlobType = other.BlobType;
			this.ContentType = other.ContentType;
			this.ContentDisposition = other.ContentDisposition;
			this.ContentEncoding = other.ContentEncoding;
			this.ContentLanguage = other.ContentLanguage;
			this.CacheControl = other.CacheControl;
			this.ContentMD5 = other.ContentMD5;
			this.Length = other.Length;
			this.ETag = other.ETag;
			this.LastModified = other.LastModified;
			this.PageBlobSequenceNumber = other.PageBlobSequenceNumber;
			this.AppendBlobCommittedBlockCount = other.AppendBlobCommittedBlockCount;
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x0003E846 File Offset: 0x0003CA46
		// (set) Token: 0x06001088 RID: 4232 RVA: 0x0003E84E File Offset: 0x0003CA4E
		public string CacheControl { get; set; }

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06001089 RID: 4233 RVA: 0x0003E857 File Offset: 0x0003CA57
		// (set) Token: 0x0600108A RID: 4234 RVA: 0x0003E85F File Offset: 0x0003CA5F
		public string ContentDisposition { get; set; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x0600108B RID: 4235 RVA: 0x0003E868 File Offset: 0x0003CA68
		// (set) Token: 0x0600108C RID: 4236 RVA: 0x0003E870 File Offset: 0x0003CA70
		public string ContentEncoding { get; set; }

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x0600108D RID: 4237 RVA: 0x0003E879 File Offset: 0x0003CA79
		// (set) Token: 0x0600108E RID: 4238 RVA: 0x0003E881 File Offset: 0x0003CA81
		public string ContentLanguage { get; set; }

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x0600108F RID: 4239 RVA: 0x0003E88A File Offset: 0x0003CA8A
		// (set) Token: 0x06001090 RID: 4240 RVA: 0x0003E892 File Offset: 0x0003CA92
		public long Length { get; internal set; }

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06001091 RID: 4241 RVA: 0x0003E89B File Offset: 0x0003CA9B
		// (set) Token: 0x06001092 RID: 4242 RVA: 0x0003E8A3 File Offset: 0x0003CAA3
		public string ContentMD5 { get; set; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06001093 RID: 4243 RVA: 0x0003E8AC File Offset: 0x0003CAAC
		// (set) Token: 0x06001094 RID: 4244 RVA: 0x0003E8B4 File Offset: 0x0003CAB4
		public string ContentType { get; set; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06001095 RID: 4245 RVA: 0x0003E8BD File Offset: 0x0003CABD
		// (set) Token: 0x06001096 RID: 4246 RVA: 0x0003E8C5 File Offset: 0x0003CAC5
		public string ETag { get; internal set; }

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06001097 RID: 4247 RVA: 0x0003E8CE File Offset: 0x0003CACE
		// (set) Token: 0x06001098 RID: 4248 RVA: 0x0003E8D6 File Offset: 0x0003CAD6
		public DateTimeOffset? LastModified { get; internal set; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06001099 RID: 4249 RVA: 0x0003E8DF File Offset: 0x0003CADF
		// (set) Token: 0x0600109A RID: 4250 RVA: 0x0003E8E7 File Offset: 0x0003CAE7
		public BlobType BlobType { get; internal set; }

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x0600109B RID: 4251 RVA: 0x0003E8F0 File Offset: 0x0003CAF0
		// (set) Token: 0x0600109C RID: 4252 RVA: 0x0003E8F8 File Offset: 0x0003CAF8
		public LeaseStatus LeaseStatus { get; internal set; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x0600109D RID: 4253 RVA: 0x0003E901 File Offset: 0x0003CB01
		// (set) Token: 0x0600109E RID: 4254 RVA: 0x0003E909 File Offset: 0x0003CB09
		public LeaseState LeaseState { get; internal set; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x0600109F RID: 4255 RVA: 0x0003E912 File Offset: 0x0003CB12
		// (set) Token: 0x060010A0 RID: 4256 RVA: 0x0003E91A File Offset: 0x0003CB1A
		public LeaseDuration LeaseDuration { get; internal set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060010A1 RID: 4257 RVA: 0x0003E923 File Offset: 0x0003CB23
		// (set) Token: 0x060010A2 RID: 4258 RVA: 0x0003E92B File Offset: 0x0003CB2B
		public long? PageBlobSequenceNumber { get; internal set; }

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060010A3 RID: 4259 RVA: 0x0003E934 File Offset: 0x0003CB34
		// (set) Token: 0x060010A4 RID: 4260 RVA: 0x0003E93C File Offset: 0x0003CB3C
		public int? AppendBlobCommittedBlockCount { get; internal set; }
	}
}
