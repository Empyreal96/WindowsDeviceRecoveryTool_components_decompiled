using System;

namespace System.Windows.Documents
{
	// Token: 0x02000377 RID: 887
	internal abstract class HighlightLayer
	{
		// Token: 0x06002FFD RID: 12285
		internal abstract object GetHighlightValue(StaticTextPointer textPosition, LogicalDirection direction);

		// Token: 0x06002FFE RID: 12286
		internal abstract bool IsContentHighlighted(StaticTextPointer textPosition, LogicalDirection direction);

		// Token: 0x06002FFF RID: 12287
		internal abstract StaticTextPointer GetNextChangePosition(StaticTextPointer textPosition, LogicalDirection direction);

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x06003000 RID: 12288
		internal abstract Type OwnerType { get; }

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x06003001 RID: 12289
		// (remove) Token: 0x06003002 RID: 12290
		internal abstract event HighlightChangedEventHandler Changed;
	}
}
