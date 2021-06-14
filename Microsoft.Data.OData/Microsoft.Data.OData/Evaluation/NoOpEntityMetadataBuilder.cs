using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x02000102 RID: 258
	internal sealed class NoOpEntityMetadataBuilder : ODataEntityMetadataBuilder
	{
		// Token: 0x060006F6 RID: 1782 RVA: 0x000183B5 File Offset: 0x000165B5
		internal NoOpEntityMetadataBuilder(ODataEntry entry)
		{
			this.entry = entry;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x000183C4 File Offset: 0x000165C4
		internal override Uri GetEditLink()
		{
			return this.entry.NonComputedEditLink;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x000183D1 File Offset: 0x000165D1
		internal override Uri GetReadLink()
		{
			return this.entry.NonComputedReadLink;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x000183DE File Offset: 0x000165DE
		internal override string GetId()
		{
			return this.entry.NonComputedId;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x000183EB File Offset: 0x000165EB
		internal override string GetETag()
		{
			return this.entry.NonComputedETag;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x000183F8 File Offset: 0x000165F8
		internal override ODataStreamReferenceValue GetMediaResource()
		{
			return this.entry.NonComputedMediaResource;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00018405 File Offset: 0x00016605
		internal override IEnumerable<ODataProperty> GetProperties(IEnumerable<ODataProperty> nonComputedProperties)
		{
			return nonComputedProperties;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00018408 File Offset: 0x00016608
		internal override IEnumerable<ODataAction> GetActions()
		{
			return this.entry.NonComputedActions;
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00018415 File Offset: 0x00016615
		internal override IEnumerable<ODataFunction> GetFunctions()
		{
			return this.entry.NonComputedFunctions;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00018422 File Offset: 0x00016622
		internal override Uri GetNavigationLinkUri(string navigationPropertyName, Uri navigationLinkUrl, bool hasNavigationLinkUrl)
		{
			return navigationLinkUrl;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00018425 File Offset: 0x00016625
		internal override Uri GetAssociationLinkUri(string navigationPropertyName, Uri associationLinkUrl, bool hasAssociationLinkUrl)
		{
			return associationLinkUrl;
		}

		// Token: 0x040002A4 RID: 676
		private readonly ODataEntry entry;
	}
}
