using System;
using System.Diagnostics;
using Microsoft.Data.OData.Evaluation;

namespace Microsoft.Data.OData
{
	// Token: 0x020002AA RID: 682
	[DebuggerDisplay("{Name}")]
	public sealed class ODataNavigationLink : ODataItem
	{
		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x060016FD RID: 5885 RVA: 0x00052FF9 File Offset: 0x000511F9
		// (set) Token: 0x060016FE RID: 5886 RVA: 0x00053001 File Offset: 0x00051201
		public bool? IsCollection { get; set; }

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x060016FF RID: 5887 RVA: 0x0005300A File Offset: 0x0005120A
		// (set) Token: 0x06001700 RID: 5888 RVA: 0x00053012 File Offset: 0x00051212
		public string Name { get; set; }

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001701 RID: 5889 RVA: 0x0005301B File Offset: 0x0005121B
		// (set) Token: 0x06001702 RID: 5890 RVA: 0x00053055 File Offset: 0x00051255
		public Uri Url
		{
			get
			{
				if (this.metadataBuilder != null)
				{
					this.url = this.metadataBuilder.GetNavigationLinkUri(this.Name, this.url, this.hasNavigationLink);
					this.hasNavigationLink = true;
				}
				return this.url;
			}
			set
			{
				this.url = value;
				this.hasNavigationLink = true;
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001703 RID: 5891 RVA: 0x00053065 File Offset: 0x00051265
		// (set) Token: 0x06001704 RID: 5892 RVA: 0x0005309F File Offset: 0x0005129F
		public Uri AssociationLinkUrl
		{
			get
			{
				if (this.metadataBuilder != null)
				{
					this.associationLinkUrl = this.metadataBuilder.GetAssociationLinkUri(this.Name, this.associationLinkUrl, this.hasAssociationUrl);
					this.hasAssociationUrl = true;
				}
				return this.associationLinkUrl;
			}
			set
			{
				this.associationLinkUrl = value;
				this.hasAssociationUrl = true;
			}
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x000530AF File Offset: 0x000512AF
		internal void SetMetadataBuilder(ODataEntityMetadataBuilder builder)
		{
			this.metadataBuilder = builder;
		}

		// Token: 0x0400097E RID: 2430
		private ODataEntityMetadataBuilder metadataBuilder;

		// Token: 0x0400097F RID: 2431
		private Uri url;

		// Token: 0x04000980 RID: 2432
		private bool hasNavigationLink;

		// Token: 0x04000981 RID: 2433
		private Uri associationLinkUrl;

		// Token: 0x04000982 RID: 2434
		private bool hasAssociationUrl;
	}
}
