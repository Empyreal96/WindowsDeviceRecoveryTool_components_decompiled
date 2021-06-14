using System;
using System.Collections.Generic;
using System.IO;

namespace System.Windows.Documents
{
	// Token: 0x02000383 RID: 899
	internal interface ITextRange
	{
		// Token: 0x06003106 RID: 12550
		bool Contains(ITextPointer position);

		// Token: 0x06003107 RID: 12551
		void Select(ITextPointer position1, ITextPointer position2);

		// Token: 0x06003108 RID: 12552
		void SelectWord(ITextPointer position);

		// Token: 0x06003109 RID: 12553
		void SelectParagraph(ITextPointer position);

		// Token: 0x0600310A RID: 12554
		void ApplyTypingHeuristics(bool overType);

		// Token: 0x0600310B RID: 12555
		object GetPropertyValue(DependencyProperty formattingProperty);

		// Token: 0x0600310C RID: 12556
		UIElement GetUIElementSelected();

		// Token: 0x0600310D RID: 12557
		bool CanSave(string dataFormat);

		// Token: 0x0600310E RID: 12558
		void Save(Stream stream, string dataFormat);

		// Token: 0x0600310F RID: 12559
		void Save(Stream stream, string dataFormat, bool preserveTextElements);

		// Token: 0x06003110 RID: 12560
		void BeginChange();

		// Token: 0x06003111 RID: 12561
		void BeginChangeNoUndo();

		// Token: 0x06003112 RID: 12562
		void EndChange();

		// Token: 0x06003113 RID: 12563
		void EndChange(bool disableScroll, bool skipEvents);

		// Token: 0x06003114 RID: 12564
		IDisposable DeclareChangeBlock();

		// Token: 0x06003115 RID: 12565
		IDisposable DeclareChangeBlock(bool disableScroll);

		// Token: 0x06003116 RID: 12566
		void NotifyChanged(bool disableScroll, bool skipEvents);

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06003117 RID: 12567
		bool IgnoreTextUnitBoundaries { get; }

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x06003118 RID: 12568
		ITextPointer Start { get; }

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x06003119 RID: 12569
		ITextPointer End { get; }

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x0600311A RID: 12570
		bool IsEmpty { get; }

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x0600311B RID: 12571
		List<TextSegment> TextSegments { get; }

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x0600311C RID: 12572
		bool HasConcreteTextContainer { get; }

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x0600311D RID: 12573
		// (set) Token: 0x0600311E RID: 12574
		string Text { get; set; }

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x0600311F RID: 12575
		string Xml { get; }

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x06003120 RID: 12576
		bool IsTableCellRange { get; }

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x06003121 RID: 12577
		int ChangeBlockLevel { get; }

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x06003122 RID: 12578
		// (remove) Token: 0x06003123 RID: 12579
		event EventHandler Changed;

		// Token: 0x06003124 RID: 12580
		void FireChanged();

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x06003125 RID: 12581
		// (set) Token: 0x06003126 RID: 12582
		uint _ContentGeneration { get; set; }

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x06003127 RID: 12583
		// (set) Token: 0x06003128 RID: 12584
		bool _IsTableCellRange { get; set; }

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x06003129 RID: 12585
		// (set) Token: 0x0600312A RID: 12586
		List<TextSegment> _TextSegments { get; set; }

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x0600312B RID: 12587
		// (set) Token: 0x0600312C RID: 12588
		int _ChangeBlockLevel { get; set; }

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x0600312D RID: 12589
		// (set) Token: 0x0600312E RID: 12590
		ChangeBlockUndoRecord _ChangeBlockUndoRecord { get; set; }

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x0600312F RID: 12591
		// (set) Token: 0x06003130 RID: 12592
		bool _IsChanged { get; set; }
	}
}
