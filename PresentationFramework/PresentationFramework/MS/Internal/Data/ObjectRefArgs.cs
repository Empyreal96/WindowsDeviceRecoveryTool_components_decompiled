using System;

namespace MS.Internal.Data
{
	// Token: 0x02000732 RID: 1842
	internal class ObjectRefArgs
	{
		// Token: 0x17001C16 RID: 7190
		// (get) Token: 0x060075F0 RID: 30192 RVA: 0x0021A5DA File Offset: 0x002187DA
		// (set) Token: 0x060075F1 RID: 30193 RVA: 0x0021A5E2 File Offset: 0x002187E2
		internal bool IsTracing { get; set; }

		// Token: 0x17001C17 RID: 7191
		// (get) Token: 0x060075F2 RID: 30194 RVA: 0x0021A5EB File Offset: 0x002187EB
		// (set) Token: 0x060075F3 RID: 30195 RVA: 0x0021A5F3 File Offset: 0x002187F3
		internal bool ResolveNamesInTemplate { get; set; }

		// Token: 0x17001C18 RID: 7192
		// (get) Token: 0x060075F4 RID: 30196 RVA: 0x0021A5FC File Offset: 0x002187FC
		// (set) Token: 0x060075F5 RID: 30197 RVA: 0x0021A604 File Offset: 0x00218804
		internal bool NameResolvedInOuterScope { get; set; }
	}
}
