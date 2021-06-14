using System;

namespace System.Windows.Markup
{
	// Token: 0x020001D0 RID: 464
	internal enum ReaderFlags : ushort
	{
		// Token: 0x0400146A RID: 5226
		Unknown,
		// Token: 0x0400146B RID: 5227
		DependencyObject = 4096,
		// Token: 0x0400146C RID: 5228
		ClrObject = 8192,
		// Token: 0x0400146D RID: 5229
		PropertyComplexClr = 12288,
		// Token: 0x0400146E RID: 5230
		PropertyComplexDP = 16384,
		// Token: 0x0400146F RID: 5231
		PropertyArray = 20480,
		// Token: 0x04001470 RID: 5232
		PropertyIList = 24576,
		// Token: 0x04001471 RID: 5233
		PropertyIDictionary = 28672,
		// Token: 0x04001472 RID: 5234
		PropertyIAddChild = 32768,
		// Token: 0x04001473 RID: 5235
		RealizeDeferContent = 36864,
		// Token: 0x04001474 RID: 5236
		ConstructorParams = 40960,
		// Token: 0x04001475 RID: 5237
		ContextTypeMask = 61440,
		// Token: 0x04001476 RID: 5238
		StyleObject = 256,
		// Token: 0x04001477 RID: 5239
		FrameworkTemplateObject = 512,
		// Token: 0x04001478 RID: 5240
		TableTemplateObject = 1024,
		// Token: 0x04001479 RID: 5241
		SingletonConstructorParam = 2048,
		// Token: 0x0400147A RID: 5242
		NeedToAddToTree = 1,
		// Token: 0x0400147B RID: 5243
		AddedToTree,
		// Token: 0x0400147C RID: 5244
		InjectedElement = 4,
		// Token: 0x0400147D RID: 5245
		CollectionHolder = 8,
		// Token: 0x0400147E RID: 5246
		IDictionary = 16,
		// Token: 0x0400147F RID: 5247
		IList = 32,
		// Token: 0x04001480 RID: 5248
		ArrayExt = 64,
		// Token: 0x04001481 RID: 5249
		IAddChild = 128
	}
}
