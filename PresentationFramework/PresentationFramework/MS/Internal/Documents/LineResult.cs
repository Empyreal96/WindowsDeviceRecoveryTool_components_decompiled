using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Documents
{
	// Token: 0x020006D5 RID: 1749
	internal abstract class LineResult
	{
		// Token: 0x060070FC RID: 28924
		internal abstract ITextPointer GetTextPositionFromDistance(double distance);

		// Token: 0x060070FD RID: 28925
		internal abstract bool IsAtCaretUnitBoundary(ITextPointer position);

		// Token: 0x060070FE RID: 28926
		internal abstract ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction);

		// Token: 0x060070FF RID: 28927
		internal abstract ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position);

		// Token: 0x06007100 RID: 28928
		internal abstract ReadOnlyCollection<GlyphRun> GetGlyphRuns(ITextPointer start, ITextPointer end);

		// Token: 0x06007101 RID: 28929
		internal abstract ITextPointer GetContentEndPosition();

		// Token: 0x06007102 RID: 28930
		internal abstract ITextPointer GetEllipsesPosition();

		// Token: 0x06007103 RID: 28931
		internal abstract int GetContentEndPositionCP();

		// Token: 0x06007104 RID: 28932
		internal abstract int GetEllipsesPositionCP();

		// Token: 0x17001AD6 RID: 6870
		// (get) Token: 0x06007105 RID: 28933
		internal abstract ITextPointer StartPosition { get; }

		// Token: 0x17001AD7 RID: 6871
		// (get) Token: 0x06007106 RID: 28934
		internal abstract ITextPointer EndPosition { get; }

		// Token: 0x17001AD8 RID: 6872
		// (get) Token: 0x06007107 RID: 28935
		internal abstract int StartPositionCP { get; }

		// Token: 0x17001AD9 RID: 6873
		// (get) Token: 0x06007108 RID: 28936
		internal abstract int EndPositionCP { get; }

		// Token: 0x17001ADA RID: 6874
		// (get) Token: 0x06007109 RID: 28937
		internal abstract Rect LayoutBox { get; }

		// Token: 0x17001ADB RID: 6875
		// (get) Token: 0x0600710A RID: 28938
		internal abstract double Baseline { get; }
	}
}
