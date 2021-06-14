using System;

namespace System.Data.Services.Client
{
	// Token: 0x02000123 RID: 291
	public sealed class StreamDescriptor : Descriptor
	{
		// Token: 0x0600099C RID: 2460 RVA: 0x0002777D File Offset: 0x0002597D
		internal StreamDescriptor(string name, EntityDescriptor entityDescriptor) : base(EntityStates.Unchanged)
		{
			this.streamLink = new DataServiceStreamLink(name);
			this.entityDescriptor = entityDescriptor;
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00027799 File Offset: 0x00025999
		internal StreamDescriptor(EntityDescriptor entityDescriptor) : base(EntityStates.Unchanged)
		{
			this.streamLink = new DataServiceStreamLink(null);
			this.entityDescriptor = entityDescriptor;
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x000277B5 File Offset: 0x000259B5
		public DataServiceStreamLink StreamLink
		{
			get
			{
				return this.streamLink;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x000277BD File Offset: 0x000259BD
		// (set) Token: 0x060009A0 RID: 2464 RVA: 0x000277C5 File Offset: 0x000259C5
		public EntityDescriptor EntityDescriptor
		{
			get
			{
				return this.entityDescriptor;
			}
			set
			{
				this.entityDescriptor = value;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x000277CE File Offset: 0x000259CE
		internal string Name
		{
			get
			{
				return this.streamLink.Name;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x000277DB File Offset: 0x000259DB
		// (set) Token: 0x060009A3 RID: 2467 RVA: 0x000277E8 File Offset: 0x000259E8
		internal Uri SelfLink
		{
			get
			{
				return this.streamLink.SelfLink;
			}
			set
			{
				this.streamLink.SelfLink = value;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x000277F6 File Offset: 0x000259F6
		// (set) Token: 0x060009A5 RID: 2469 RVA: 0x00027803 File Offset: 0x00025A03
		internal Uri EditLink
		{
			get
			{
				return this.streamLink.EditLink;
			}
			set
			{
				this.streamLink.EditLink = value;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x00027811 File Offset: 0x00025A11
		// (set) Token: 0x060009A7 RID: 2471 RVA: 0x0002781E File Offset: 0x00025A1E
		internal string ContentType
		{
			get
			{
				return this.streamLink.ContentType;
			}
			set
			{
				this.streamLink.ContentType = value;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x0002782C File Offset: 0x00025A2C
		// (set) Token: 0x060009A9 RID: 2473 RVA: 0x00027839 File Offset: 0x00025A39
		internal string ETag
		{
			get
			{
				return this.streamLink.ETag;
			}
			set
			{
				this.streamLink.ETag = value;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x00027847 File Offset: 0x00025A47
		// (set) Token: 0x060009AB RID: 2475 RVA: 0x0002784F File Offset: 0x00025A4F
		internal DataServiceSaveStream SaveStream { get; set; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x00027858 File Offset: 0x00025A58
		internal override DescriptorKind DescriptorKind
		{
			get
			{
				return DescriptorKind.NamedStream;
			}
		}

		// Token: 0x17000251 RID: 593
		// (set) Token: 0x060009AD RID: 2477 RVA: 0x0002785B File Offset: 0x00025A5B
		internal StreamDescriptor TransientNamedStreamInfo
		{
			set
			{
				if (this.transientNamedStreamInfo == null)
				{
					this.transientNamedStreamInfo = value;
					return;
				}
				StreamDescriptor.MergeStreamDescriptor(this.transientNamedStreamInfo, value);
			}
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0002787C File Offset: 0x00025A7C
		internal static void MergeStreamDescriptor(StreamDescriptor existingStreamDescriptor, StreamDescriptor newStreamDescriptor)
		{
			if (newStreamDescriptor.SelfLink != null)
			{
				existingStreamDescriptor.SelfLink = newStreamDescriptor.SelfLink;
			}
			if (newStreamDescriptor.EditLink != null)
			{
				existingStreamDescriptor.EditLink = newStreamDescriptor.EditLink;
			}
			if (newStreamDescriptor.ContentType != null)
			{
				existingStreamDescriptor.ContentType = newStreamDescriptor.ContentType;
			}
			if (newStreamDescriptor.ETag != null)
			{
				existingStreamDescriptor.ETag = newStreamDescriptor.ETag;
			}
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x000278E5 File Offset: 0x00025AE5
		internal override void ClearChanges()
		{
			this.transientNamedStreamInfo = null;
			this.CloseSaveStream();
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x000278F4 File Offset: 0x00025AF4
		internal Uri GetLatestEditLink()
		{
			if (this.transientNamedStreamInfo != null && this.transientNamedStreamInfo.EditLink != null)
			{
				return this.transientNamedStreamInfo.EditLink;
			}
			return this.EditLink;
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00027923 File Offset: 0x00025B23
		internal string GetLatestETag()
		{
			if (this.transientNamedStreamInfo != null && this.transientNamedStreamInfo.ETag != null)
			{
				return this.transientNamedStreamInfo.ETag;
			}
			return this.ETag;
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0002794C File Offset: 0x00025B4C
		internal void CloseSaveStream()
		{
			if (this.SaveStream != null)
			{
				DataServiceSaveStream saveStream = this.SaveStream;
				this.SaveStream = null;
				saveStream.Close();
			}
		}

		// Token: 0x04000597 RID: 1431
		private DataServiceStreamLink streamLink;

		// Token: 0x04000598 RID: 1432
		private EntityDescriptor entityDescriptor;

		// Token: 0x04000599 RID: 1433
		private StreamDescriptor transientNamedStreamInfo;
	}
}
