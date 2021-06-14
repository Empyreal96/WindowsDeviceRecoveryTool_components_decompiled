using System;
using System.Diagnostics;
using Microsoft.Data.OData.Evaluation;

namespace Microsoft.Data.OData
{
	// Token: 0x0200028D RID: 653
	[DebuggerDisplay("{Name}")]
	public sealed class ODataAssociationLink : ODataAnnotatable
	{
		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06001600 RID: 5632 RVA: 0x000506C1 File Offset: 0x0004E8C1
		// (set) Token: 0x06001601 RID: 5633 RVA: 0x000506C9 File Offset: 0x0004E8C9
		public string Name { get; set; }

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06001602 RID: 5634 RVA: 0x000506D2 File Offset: 0x0004E8D2
		// (set) Token: 0x06001603 RID: 5635 RVA: 0x0005070C File Offset: 0x0004E90C
		public Uri Url
		{
			get
			{
				if (this.metadataBuilder != null)
				{
					this.url = this.metadataBuilder.GetAssociationLinkUri(this.Name, this.url, this.hasAssociationLinkUrl);
					this.hasAssociationLinkUrl = true;
				}
				return this.url;
			}
			set
			{
				this.url = value;
				this.hasAssociationLinkUrl = true;
			}
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x0005071C File Offset: 0x0004E91C
		internal void SetMetadataBuilder(ODataEntityMetadataBuilder builder)
		{
			this.metadataBuilder = builder;
		}

		// Token: 0x04000860 RID: 2144
		private ODataEntityMetadataBuilder metadataBuilder;

		// Token: 0x04000861 RID: 2145
		private Uri url;

		// Token: 0x04000862 RID: 2146
		private bool hasAssociationLinkUrl;
	}
}
