using System;

namespace System.Windows.Markup
{
	// Token: 0x020001DE RID: 478
	internal interface IOptimizedMarkupExtension
	{
		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06001F1A RID: 7962
		short ExtensionTypeId { get; }

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06001F1B RID: 7963
		short ValueId { get; }

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06001F1C RID: 7964
		bool IsValueTypeExtension { get; }

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06001F1D RID: 7965
		bool IsValueStaticExtension { get; }
	}
}
