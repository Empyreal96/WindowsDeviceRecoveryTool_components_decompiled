using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000A6 RID: 166
	internal sealed class BlobAttributes
	{
		// Token: 0x06001059 RID: 4185 RVA: 0x0003E3A0 File Offset: 0x0003C5A0
		internal BlobAttributes()
		{
			this.Properties = new BlobProperties();
			this.Metadata = new Dictionary<string, string>();
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600105A RID: 4186 RVA: 0x0003E3BE File Offset: 0x0003C5BE
		// (set) Token: 0x0600105B RID: 4187 RVA: 0x0003E3C6 File Offset: 0x0003C5C6
		public BlobProperties Properties { get; internal set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600105C RID: 4188 RVA: 0x0003E3CF File Offset: 0x0003C5CF
		// (set) Token: 0x0600105D RID: 4189 RVA: 0x0003E3D7 File Offset: 0x0003C5D7
		public IDictionary<string, string> Metadata { get; internal set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600105E RID: 4190 RVA: 0x0003E3E0 File Offset: 0x0003C5E0
		public Uri Uri
		{
			get
			{
				return this.StorageUri.PrimaryUri;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x0003E3ED File Offset: 0x0003C5ED
		// (set) Token: 0x06001060 RID: 4192 RVA: 0x0003E3F5 File Offset: 0x0003C5F5
		public StorageUri StorageUri { get; internal set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06001061 RID: 4193 RVA: 0x0003E3FE File Offset: 0x0003C5FE
		// (set) Token: 0x06001062 RID: 4194 RVA: 0x0003E406 File Offset: 0x0003C606
		public DateTimeOffset? SnapshotTime { get; internal set; }

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06001063 RID: 4195 RVA: 0x0003E40F File Offset: 0x0003C60F
		// (set) Token: 0x06001064 RID: 4196 RVA: 0x0003E417 File Offset: 0x0003C617
		public CopyState CopyState { get; internal set; }

		// Token: 0x06001065 RID: 4197 RVA: 0x0003E420 File Offset: 0x0003C620
		internal void AssertNoSnapshot()
		{
			if (this.SnapshotTime != null)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot perform this operation on a blob representing a snapshot.", new object[0]);
				throw new InvalidOperationException(message);
			}
		}
	}
}
