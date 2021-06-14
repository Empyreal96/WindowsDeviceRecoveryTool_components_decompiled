using System;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000131 RID: 305
	internal enum ResourceExpressionType
	{
		// Token: 0x04000694 RID: 1684
		RootResourceSet = 10000,
		// Token: 0x04000695 RID: 1685
		ResourceNavigationProperty,
		// Token: 0x04000696 RID: 1686
		ResourceNavigationPropertySingleton,
		// Token: 0x04000697 RID: 1687
		TakeQueryOption,
		// Token: 0x04000698 RID: 1688
		SkipQueryOption,
		// Token: 0x04000699 RID: 1689
		OrderByQueryOption,
		// Token: 0x0400069A RID: 1690
		FilterQueryOption,
		// Token: 0x0400069B RID: 1691
		InputReference,
		// Token: 0x0400069C RID: 1692
		ProjectionQueryOption,
		// Token: 0x0400069D RID: 1693
		RequestOptions,
		// Token: 0x0400069E RID: 1694
		OperationContext,
		// Token: 0x0400069F RID: 1695
		Resolver
	}
}
