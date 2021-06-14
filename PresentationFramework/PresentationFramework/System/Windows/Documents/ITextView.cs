using System;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x02000385 RID: 901
	internal interface ITextView
	{
		// Token: 0x06003147 RID: 12615
		ITextPointer GetTextPositionFromPoint(Point point, bool snapToText);

		// Token: 0x06003148 RID: 12616
		Rect GetRectangleFromTextPosition(ITextPointer position);

		// Token: 0x06003149 RID: 12617
		Rect GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform);

		// Token: 0x0600314A RID: 12618
		Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition);

		// Token: 0x0600314B RID: 12619
		ITextPointer GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved);

		// Token: 0x0600314C RID: 12620
		ITextPointer GetPositionAtNextPage(ITextPointer position, Point suggestedOffset, int count, out Point newSuggestedOffset, out int pagesMoved);

		// Token: 0x0600314D RID: 12621
		bool IsAtCaretUnitBoundary(ITextPointer position);

		// Token: 0x0600314E RID: 12622
		ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction);

		// Token: 0x0600314F RID: 12623
		ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position);

		// Token: 0x06003150 RID: 12624
		TextSegment GetLineRange(ITextPointer position);

		// Token: 0x06003151 RID: 12625
		ReadOnlyCollection<GlyphRun> GetGlyphRuns(ITextPointer start, ITextPointer end);

		// Token: 0x06003152 RID: 12626
		bool Contains(ITextPointer position);

		// Token: 0x06003153 RID: 12627
		void BringPositionIntoViewAsync(ITextPointer position, object userState);

		// Token: 0x06003154 RID: 12628
		void BringPointIntoViewAsync(Point point, object userState);

		// Token: 0x06003155 RID: 12629
		void BringLineIntoViewAsync(ITextPointer position, double suggestedX, int count, object userState);

		// Token: 0x06003156 RID: 12630
		void BringPageIntoViewAsync(ITextPointer position, Point suggestedOffset, int count, object userState);

		// Token: 0x06003157 RID: 12631
		void CancelAsync(object userState);

		// Token: 0x06003158 RID: 12632
		bool Validate();

		// Token: 0x06003159 RID: 12633
		bool Validate(Point point);

		// Token: 0x0600315A RID: 12634
		bool Validate(ITextPointer position);

		// Token: 0x0600315B RID: 12635
		void ThrottleBackgroundTasksForUserInput();

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x0600315C RID: 12636
		UIElement RenderScope { get; }

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x0600315D RID: 12637
		ITextContainer TextContainer { get; }

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x0600315E RID: 12638
		bool IsValid { get; }

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x0600315F RID: 12639
		bool RendersOwnSelection { get; }

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x06003160 RID: 12640
		ReadOnlyCollection<TextSegment> TextSegments { get; }

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x06003161 RID: 12641
		// (remove) Token: 0x06003162 RID: 12642
		event BringPositionIntoViewCompletedEventHandler BringPositionIntoViewCompleted;

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x06003163 RID: 12643
		// (remove) Token: 0x06003164 RID: 12644
		event BringPointIntoViewCompletedEventHandler BringPointIntoViewCompleted;

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x06003165 RID: 12645
		// (remove) Token: 0x06003166 RID: 12646
		event BringLineIntoViewCompletedEventHandler BringLineIntoViewCompleted;

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x06003167 RID: 12647
		// (remove) Token: 0x06003168 RID: 12648
		event BringPageIntoViewCompletedEventHandler BringPageIntoViewCompleted;

		// Token: 0x14000083 RID: 131
		// (add) Token: 0x06003169 RID: 12649
		// (remove) Token: 0x0600316A RID: 12650
		event EventHandler Updated;
	}
}
