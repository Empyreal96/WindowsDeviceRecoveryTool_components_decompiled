using System;
using Microsoft.Data.OData.Evaluation;

namespace Microsoft.Data.OData
{
	// Token: 0x02000109 RID: 265
	internal interface IODataFeedAndEntryTypeContext
	{
		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000735 RID: 1845
		string EntitySetName { get; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000736 RID: 1846
		string EntitySetElementTypeName { get; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000737 RID: 1847
		string ExpectedEntityTypeName { get; }

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000738 RID: 1848
		bool IsMediaLinkEntry { get; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000739 RID: 1849
		UrlConvention UrlConvention { get; }
	}
}
