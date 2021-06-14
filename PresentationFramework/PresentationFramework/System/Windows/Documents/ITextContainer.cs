using System;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000381 RID: 897
	internal interface ITextContainer
	{
		// Token: 0x060030B4 RID: 12468
		void BeginChange();

		// Token: 0x060030B5 RID: 12469
		void BeginChangeNoUndo();

		// Token: 0x060030B6 RID: 12470
		void EndChange();

		// Token: 0x060030B7 RID: 12471
		void EndChange(bool skipEvents);

		// Token: 0x060030B8 RID: 12472
		ITextPointer CreatePointerAtOffset(int offset, LogicalDirection direction);

		// Token: 0x060030B9 RID: 12473
		ITextPointer CreatePointerAtCharOffset(int charOffset, LogicalDirection direction);

		// Token: 0x060030BA RID: 12474
		ITextPointer CreateDynamicTextPointer(StaticTextPointer position, LogicalDirection direction);

		// Token: 0x060030BB RID: 12475
		StaticTextPointer CreateStaticPointerAtOffset(int offset);

		// Token: 0x060030BC RID: 12476
		TextPointerContext GetPointerContext(StaticTextPointer pointer, LogicalDirection direction);

		// Token: 0x060030BD RID: 12477
		int GetOffsetToPosition(StaticTextPointer position1, StaticTextPointer position2);

		// Token: 0x060030BE RID: 12478
		int GetTextInRun(StaticTextPointer position, LogicalDirection direction, char[] textBuffer, int startIndex, int count);

		// Token: 0x060030BF RID: 12479
		object GetAdjacentElement(StaticTextPointer position, LogicalDirection direction);

		// Token: 0x060030C0 RID: 12480
		DependencyObject GetParent(StaticTextPointer position);

		// Token: 0x060030C1 RID: 12481
		StaticTextPointer CreatePointer(StaticTextPointer position, int offset);

		// Token: 0x060030C2 RID: 12482
		StaticTextPointer GetNextContextPosition(StaticTextPointer position, LogicalDirection direction);

		// Token: 0x060030C3 RID: 12483
		int CompareTo(StaticTextPointer position1, StaticTextPointer position2);

		// Token: 0x060030C4 RID: 12484
		int CompareTo(StaticTextPointer position1, ITextPointer position2);

		// Token: 0x060030C5 RID: 12485
		object GetValue(StaticTextPointer position, DependencyProperty formattingProperty);

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x060030C6 RID: 12486
		bool IsReadOnly { get; }

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x060030C7 RID: 12487
		ITextPointer Start { get; }

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x060030C8 RID: 12488
		ITextPointer End { get; }

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x060030C9 RID: 12489
		DependencyObject Parent { get; }

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x060030CA RID: 12490
		Highlights Highlights { get; }

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x060030CB RID: 12491
		// (set) Token: 0x060030CC RID: 12492
		ITextSelection TextSelection { get; set; }

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x060030CD RID: 12493
		UndoManager UndoManager { get; }

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x060030CE RID: 12494
		// (set) Token: 0x060030CF RID: 12495
		ITextView TextView { get; set; }

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x060030D0 RID: 12496
		int SymbolCount { get; }

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x060030D1 RID: 12497
		int IMECharCount { get; }

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x060030D2 RID: 12498
		uint Generation { get; }

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x060030D3 RID: 12499
		// (remove) Token: 0x060030D4 RID: 12500
		event EventHandler Changing;

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x060030D5 RID: 12501
		// (remove) Token: 0x060030D6 RID: 12502
		event TextContainerChangeEventHandler Change;

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x060030D7 RID: 12503
		// (remove) Token: 0x060030D8 RID: 12504
		event TextContainerChangedEventHandler Changed;
	}
}
