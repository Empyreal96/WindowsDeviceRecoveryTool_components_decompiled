using System;

namespace System.Data.Services.Client
{
	// Token: 0x020000D5 RID: 213
	internal enum ResourceExpressionType
	{
		// Token: 0x04000428 RID: 1064
		RootResourceSet = 10000,
		// Token: 0x04000429 RID: 1065
		ResourceNavigationProperty,
		// Token: 0x0400042A RID: 1066
		ResourceNavigationPropertySingleton,
		// Token: 0x0400042B RID: 1067
		TakeQueryOption,
		// Token: 0x0400042C RID: 1068
		SkipQueryOption,
		// Token: 0x0400042D RID: 1069
		OrderByQueryOption,
		// Token: 0x0400042E RID: 1070
		FilterQueryOption,
		// Token: 0x0400042F RID: 1071
		InputReference,
		// Token: 0x04000430 RID: 1072
		ProjectionQueryOption,
		// Token: 0x04000431 RID: 1073
		ExpandQueryOption
	}
}
