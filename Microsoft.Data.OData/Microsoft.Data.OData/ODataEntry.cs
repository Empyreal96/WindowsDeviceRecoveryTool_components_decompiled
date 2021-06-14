using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Data.OData.Evaluation;

namespace Microsoft.Data.OData
{
	// Token: 0x020002AD RID: 685
	[DebuggerDisplay("Id: {Id} TypeName: {TypeName}")]
	public sealed class ODataEntry : ODataItem
	{
		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x0600171A RID: 5914 RVA: 0x0005321D File Offset: 0x0005141D
		// (set) Token: 0x0600171B RID: 5915 RVA: 0x0005322A File Offset: 0x0005142A
		public string ETag
		{
			get
			{
				return this.MetadataBuilder.GetETag();
			}
			set
			{
				this.etag = value;
				this.hasNonComputedETag = true;
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x0600171C RID: 5916 RVA: 0x0005323A File Offset: 0x0005143A
		// (set) Token: 0x0600171D RID: 5917 RVA: 0x00053247 File Offset: 0x00051447
		public string Id
		{
			get
			{
				return this.MetadataBuilder.GetId();
			}
			set
			{
				this.id = value;
				this.hasNonComputedId = true;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x0600171E RID: 5918 RVA: 0x00053257 File Offset: 0x00051457
		// (set) Token: 0x0600171F RID: 5919 RVA: 0x00053264 File Offset: 0x00051464
		public Uri EditLink
		{
			get
			{
				return this.MetadataBuilder.GetEditLink();
			}
			set
			{
				this.editLink = value;
				this.hasNonComputedEditLink = true;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06001720 RID: 5920 RVA: 0x00053274 File Offset: 0x00051474
		// (set) Token: 0x06001721 RID: 5921 RVA: 0x00053281 File Offset: 0x00051481
		public Uri ReadLink
		{
			get
			{
				return this.MetadataBuilder.GetReadLink();
			}
			set
			{
				this.readLink = value;
				this.hasNonComputedReadLink = true;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06001722 RID: 5922 RVA: 0x00053291 File Offset: 0x00051491
		// (set) Token: 0x06001723 RID: 5923 RVA: 0x0005329E File Offset: 0x0005149E
		public ODataStreamReferenceValue MediaResource
		{
			get
			{
				return this.MetadataBuilder.GetMediaResource();
			}
			set
			{
				this.mediaResource = value;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06001724 RID: 5924 RVA: 0x000532A7 File Offset: 0x000514A7
		// (set) Token: 0x06001725 RID: 5925 RVA: 0x000532AF File Offset: 0x000514AF
		public IEnumerable<ODataAssociationLink> AssociationLinks { get; set; }

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001726 RID: 5926 RVA: 0x000532B8 File Offset: 0x000514B8
		// (set) Token: 0x06001727 RID: 5927 RVA: 0x000532C5 File Offset: 0x000514C5
		public IEnumerable<ODataAction> Actions
		{
			get
			{
				return this.MetadataBuilder.GetActions();
			}
			set
			{
				this.actions = value;
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001728 RID: 5928 RVA: 0x000532CE File Offset: 0x000514CE
		// (set) Token: 0x06001729 RID: 5929 RVA: 0x000532DB File Offset: 0x000514DB
		public IEnumerable<ODataFunction> Functions
		{
			get
			{
				return this.MetadataBuilder.GetFunctions();
			}
			set
			{
				this.functions = value;
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x0600172A RID: 5930 RVA: 0x000532E4 File Offset: 0x000514E4
		// (set) Token: 0x0600172B RID: 5931 RVA: 0x000532F7 File Offset: 0x000514F7
		public IEnumerable<ODataProperty> Properties
		{
			get
			{
				return this.MetadataBuilder.GetProperties(this.properties);
			}
			set
			{
				this.properties = value;
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x0600172C RID: 5932 RVA: 0x00053300 File Offset: 0x00051500
		// (set) Token: 0x0600172D RID: 5933 RVA: 0x00053308 File Offset: 0x00051508
		public string TypeName { get; set; }

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x0600172E RID: 5934 RVA: 0x00053311 File Offset: 0x00051511
		// (set) Token: 0x0600172F RID: 5935 RVA: 0x00053319 File Offset: 0x00051519
		public ICollection<ODataInstanceAnnotation> InstanceAnnotations
		{
			get
			{
				return base.GetInstanceAnnotations();
			}
			set
			{
				base.SetInstanceAnnotations(value);
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001730 RID: 5936 RVA: 0x00053322 File Offset: 0x00051522
		// (set) Token: 0x06001731 RID: 5937 RVA: 0x0005333E File Offset: 0x0005153E
		internal ODataEntityMetadataBuilder MetadataBuilder
		{
			get
			{
				if (this.metadataBuilder == null)
				{
					this.metadataBuilder = new NoOpEntityMetadataBuilder(this);
				}
				return this.metadataBuilder;
			}
			set
			{
				this.metadataBuilder = value;
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001732 RID: 5938 RVA: 0x00053347 File Offset: 0x00051547
		internal string NonComputedId
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001733 RID: 5939 RVA: 0x0005334F File Offset: 0x0005154F
		internal bool HasNonComputedId
		{
			get
			{
				return this.hasNonComputedId;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001734 RID: 5940 RVA: 0x00053357 File Offset: 0x00051557
		internal Uri NonComputedEditLink
		{
			get
			{
				return this.editLink;
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001735 RID: 5941 RVA: 0x0005335F File Offset: 0x0005155F
		internal bool HasNonComputedEditLink
		{
			get
			{
				return this.hasNonComputedEditLink;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001736 RID: 5942 RVA: 0x00053367 File Offset: 0x00051567
		internal Uri NonComputedReadLink
		{
			get
			{
				return this.readLink;
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001737 RID: 5943 RVA: 0x0005336F File Offset: 0x0005156F
		internal bool HasNonComputedReadLink
		{
			get
			{
				return this.hasNonComputedReadLink;
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001738 RID: 5944 RVA: 0x00053377 File Offset: 0x00051577
		internal string NonComputedETag
		{
			get
			{
				return this.etag;
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001739 RID: 5945 RVA: 0x0005337F File Offset: 0x0005157F
		internal bool HasNonComputedETag
		{
			get
			{
				return this.hasNonComputedETag;
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x0600173A RID: 5946 RVA: 0x00053387 File Offset: 0x00051587
		internal ODataStreamReferenceValue NonComputedMediaResource
		{
			get
			{
				return this.mediaResource;
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x0600173B RID: 5947 RVA: 0x0005338F File Offset: 0x0005158F
		internal IEnumerable<ODataProperty> NonComputedProperties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x0600173C RID: 5948 RVA: 0x00053397 File Offset: 0x00051597
		internal IEnumerable<ODataAction> NonComputedActions
		{
			get
			{
				return this.actions;
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x0600173D RID: 5949 RVA: 0x0005339F File Offset: 0x0005159F
		internal IEnumerable<ODataFunction> NonComputedFunctions
		{
			get
			{
				return this.functions;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x0600173E RID: 5950 RVA: 0x000533A7 File Offset: 0x000515A7
		// (set) Token: 0x0600173F RID: 5951 RVA: 0x000533AF File Offset: 0x000515AF
		internal ODataFeedAndEntrySerializationInfo SerializationInfo
		{
			get
			{
				return this.serializationInfo;
			}
			set
			{
				this.serializationInfo = ODataFeedAndEntrySerializationInfo.Validate(value);
			}
		}

		// Token: 0x04000992 RID: 2450
		private ODataEntityMetadataBuilder metadataBuilder;

		// Token: 0x04000993 RID: 2451
		private string etag;

		// Token: 0x04000994 RID: 2452
		private bool hasNonComputedETag;

		// Token: 0x04000995 RID: 2453
		private string id;

		// Token: 0x04000996 RID: 2454
		private bool hasNonComputedId;

		// Token: 0x04000997 RID: 2455
		private Uri editLink;

		// Token: 0x04000998 RID: 2456
		private bool hasNonComputedEditLink;

		// Token: 0x04000999 RID: 2457
		private Uri readLink;

		// Token: 0x0400099A RID: 2458
		private bool hasNonComputedReadLink;

		// Token: 0x0400099B RID: 2459
		private ODataStreamReferenceValue mediaResource;

		// Token: 0x0400099C RID: 2460
		private IEnumerable<ODataProperty> properties;

		// Token: 0x0400099D RID: 2461
		private IEnumerable<ODataAction> actions;

		// Token: 0x0400099E RID: 2462
		private IEnumerable<ODataFunction> functions;

		// Token: 0x0400099F RID: 2463
		private ODataFeedAndEntrySerializationInfo serializationInfo;
	}
}
