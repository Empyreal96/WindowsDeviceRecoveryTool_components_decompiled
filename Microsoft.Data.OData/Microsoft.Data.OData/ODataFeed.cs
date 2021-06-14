using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData
{
	// Token: 0x020002A9 RID: 681
	public sealed class ODataFeed : ODataItem
	{
		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x060016F0 RID: 5872 RVA: 0x00052F54 File Offset: 0x00051154
		// (set) Token: 0x060016F1 RID: 5873 RVA: 0x00052F5C File Offset: 0x0005115C
		public long? Count { get; set; }

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x060016F2 RID: 5874 RVA: 0x00052F65 File Offset: 0x00051165
		// (set) Token: 0x060016F3 RID: 5875 RVA: 0x00052F6D File Offset: 0x0005116D
		public string Id { get; set; }

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x060016F4 RID: 5876 RVA: 0x00052F76 File Offset: 0x00051176
		// (set) Token: 0x060016F5 RID: 5877 RVA: 0x00052F7E File Offset: 0x0005117E
		public Uri NextPageLink
		{
			get
			{
				return this.nextPageLink;
			}
			set
			{
				if (this.DeltaLink != null)
				{
					throw new ODataException(Strings.ODataFeed_MustNotContainBothNextPageLinkAndDeltaLink);
				}
				this.nextPageLink = value;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x060016F6 RID: 5878 RVA: 0x00052FA0 File Offset: 0x000511A0
		// (set) Token: 0x060016F7 RID: 5879 RVA: 0x00052FA8 File Offset: 0x000511A8
		public Uri DeltaLink
		{
			get
			{
				return this.deltaLink;
			}
			set
			{
				if (this.NextPageLink != null)
				{
					throw new ODataException(Strings.ODataFeed_MustNotContainBothNextPageLinkAndDeltaLink);
				}
				this.deltaLink = value;
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x060016F8 RID: 5880 RVA: 0x00052FCA File Offset: 0x000511CA
		// (set) Token: 0x060016F9 RID: 5881 RVA: 0x00052FD2 File Offset: 0x000511D2
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

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x060016FA RID: 5882 RVA: 0x00052FDB File Offset: 0x000511DB
		// (set) Token: 0x060016FB RID: 5883 RVA: 0x00052FE3 File Offset: 0x000511E3
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

		// Token: 0x04000979 RID: 2425
		private Uri nextPageLink;

		// Token: 0x0400097A RID: 2426
		private Uri deltaLink;

		// Token: 0x0400097B RID: 2427
		private ODataFeedAndEntrySerializationInfo serializationInfo;
	}
}
