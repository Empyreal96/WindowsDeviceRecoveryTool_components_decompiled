using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.Blob.Protocol
{
	// Token: 0x020000CF RID: 207
	public sealed class ListBlobEntry : IListBlobEntry
	{
		// Token: 0x06001142 RID: 4418 RVA: 0x00040695 File Offset: 0x0003E895
		internal ListBlobEntry(string name, BlobAttributes attributes)
		{
			this.Name = name;
			this.Attributes = attributes;
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06001143 RID: 4419 RVA: 0x000406AB File Offset: 0x0003E8AB
		// (set) Token: 0x06001144 RID: 4420 RVA: 0x000406B3 File Offset: 0x0003E8B3
		internal BlobAttributes Attributes { get; private set; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06001145 RID: 4421 RVA: 0x000406BC File Offset: 0x0003E8BC
		// (set) Token: 0x06001146 RID: 4422 RVA: 0x000406C4 File Offset: 0x0003E8C4
		public string Name { get; private set; }

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06001147 RID: 4423 RVA: 0x000406CD File Offset: 0x0003E8CD
		public BlobProperties Properties
		{
			get
			{
				return this.Attributes.Properties;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x000406DA File Offset: 0x0003E8DA
		public IDictionary<string, string> Metadata
		{
			get
			{
				return this.Attributes.Metadata;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06001149 RID: 4425 RVA: 0x000406E7 File Offset: 0x0003E8E7
		public Uri Uri
		{
			get
			{
				return this.Attributes.Uri;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x000406F4 File Offset: 0x0003E8F4
		public DateTimeOffset? SnapshotTime
		{
			get
			{
				return this.Attributes.SnapshotTime;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600114B RID: 4427 RVA: 0x00040701 File Offset: 0x0003E901
		public CopyState CopyState
		{
			get
			{
				return this.Attributes.CopyState;
			}
		}
	}
}
