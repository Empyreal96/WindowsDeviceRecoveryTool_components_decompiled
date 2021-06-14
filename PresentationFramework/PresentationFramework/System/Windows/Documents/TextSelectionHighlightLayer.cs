using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000414 RID: 1044
	internal class TextSelectionHighlightLayer : HighlightLayer
	{
		// Token: 0x06003C65 RID: 15461 RVA: 0x00117340 File Offset: 0x00115540
		internal TextSelectionHighlightLayer(ITextSelection selection)
		{
			this._selection = selection;
			this._selection.Changed += this.OnSelectionChanged;
			this._oldStart = this._selection.Start;
			this._oldEnd = this._selection.End;
		}

		// Token: 0x06003C66 RID: 15462 RVA: 0x00117394 File Offset: 0x00115594
		internal override object GetHighlightValue(StaticTextPointer textPosition, LogicalDirection direction)
		{
			object result;
			if (this.IsContentHighlighted(textPosition, direction))
			{
				result = TextSelectionHighlightLayer._selectedValue;
			}
			else
			{
				result = DependencyProperty.UnsetValue;
			}
			return result;
		}

		// Token: 0x06003C67 RID: 15463 RVA: 0x001173BC File Offset: 0x001155BC
		internal override bool IsContentHighlighted(StaticTextPointer textPosition, LogicalDirection direction)
		{
			if (this._selection.IsInterimSelection)
			{
				return false;
			}
			List<TextSegment> textSegments = this._selection.TextSegments;
			int count = textSegments.Count;
			for (int i = 0; i < count; i++)
			{
				TextSegment textSegment = textSegments[i];
				if ((direction == LogicalDirection.Forward && textSegment.Start.CompareTo(textPosition) <= 0 && textPosition.CompareTo(textSegment.End) < 0) || (direction == LogicalDirection.Backward && textSegment.Start.CompareTo(textPosition) < 0 && textPosition.CompareTo(textSegment.End) <= 0))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003C68 RID: 15464 RVA: 0x0011744C File Offset: 0x0011564C
		internal override StaticTextPointer GetNextChangePosition(StaticTextPointer textPosition, LogicalDirection direction)
		{
			StaticTextPointer result = StaticTextPointer.Null;
			if (!this.IsTextRangeEmpty(this._selection) && !this._selection.IsInterimSelection)
			{
				List<TextSegment> textSegments = this._selection.TextSegments;
				int count = textSegments.Count;
				if (direction == LogicalDirection.Forward)
				{
					for (int i = 0; i < count; i++)
					{
						TextSegment textSegment = textSegments[i];
						if (textSegment.Start.CompareTo(textSegment.End) != 0)
						{
							if (textPosition.CompareTo(textSegment.Start) < 0)
							{
								result = textSegment.Start.CreateStaticPointer();
								break;
							}
							if (textPosition.CompareTo(textSegment.End) < 0)
							{
								result = textSegment.End.CreateStaticPointer();
								break;
							}
						}
					}
				}
				else
				{
					for (int j = count - 1; j >= 0; j--)
					{
						TextSegment textSegment = textSegments[j];
						if (textSegment.Start.CompareTo(textSegment.End) != 0)
						{
							if (textPosition.CompareTo(textSegment.End) > 0)
							{
								result = textSegment.End.CreateStaticPointer();
								break;
							}
							if (textPosition.CompareTo(textSegment.Start) > 0)
							{
								result = textSegment.Start.CreateStaticPointer();
								break;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06003C69 RID: 15465 RVA: 0x0011757C File Offset: 0x0011577C
		internal void InternalOnSelectionChanged()
		{
			ITextPointer textPointer;
			if (!this._selection.IsInterimSelection)
			{
				textPointer = this._selection.Start;
			}
			else
			{
				textPointer = this._selection.End;
			}
			ITextPointer end = this._selection.End;
			ITextPointer textPointer2;
			ITextPointer textPointer3;
			if (this._oldStart.CompareTo(textPointer) < 0)
			{
				textPointer2 = this._oldStart;
				textPointer3 = TextPointerBase.Min(textPointer, this._oldEnd);
			}
			else
			{
				textPointer2 = textPointer;
				textPointer3 = TextPointerBase.Min(end, this._oldStart);
			}
			ITextPointer textPointer4;
			ITextPointer textPointer5;
			if (this._oldEnd.CompareTo(end) < 0)
			{
				textPointer4 = TextPointerBase.Max(textPointer, this._oldEnd);
				textPointer5 = end;
			}
			else
			{
				textPointer4 = TextPointerBase.Max(end, this._oldStart);
				textPointer5 = this._oldEnd;
			}
			this._oldStart = textPointer;
			this._oldEnd = end;
			if (this.Changed != null && (textPointer2.CompareTo(textPointer3) != 0 || textPointer4.CompareTo(textPointer5) != 0))
			{
				TextSelectionHighlightLayer.TextSelectionHighlightChangedEventArgs args = new TextSelectionHighlightLayer.TextSelectionHighlightChangedEventArgs(textPointer2, textPointer3, textPointer4, textPointer5);
				this.Changed(this, args);
			}
		}

		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x06003C6A RID: 15466 RVA: 0x000C73A9 File Offset: 0x000C55A9
		internal override Type OwnerType
		{
			get
			{
				return typeof(TextSelection);
			}
		}

		// Token: 0x14000092 RID: 146
		// (add) Token: 0x06003C6B RID: 15467 RVA: 0x00117670 File Offset: 0x00115870
		// (remove) Token: 0x06003C6C RID: 15468 RVA: 0x001176A8 File Offset: 0x001158A8
		internal override event HighlightChangedEventHandler Changed;

		// Token: 0x06003C6D RID: 15469 RVA: 0x001176DD File Offset: 0x001158DD
		private void OnSelectionChanged(object sender, EventArgs e)
		{
			Invariant.Assert(this._selection == (ITextSelection)sender);
			this.InternalOnSelectionChanged();
		}

		// Token: 0x06003C6E RID: 15470 RVA: 0x001176F8 File Offset: 0x001158F8
		private bool IsTextRangeEmpty(ITextRange textRange)
		{
			Invariant.Assert(textRange._TextSegments.Count > 0);
			return textRange._TextSegments[0].Start.CompareTo(textRange._TextSegments[textRange._TextSegments.Count - 1].End) == 0;
		}

		// Token: 0x04002617 RID: 9751
		private readonly ITextSelection _selection;

		// Token: 0x04002618 RID: 9752
		private ITextPointer _oldStart;

		// Token: 0x04002619 RID: 9753
		private ITextPointer _oldEnd;

		// Token: 0x0400261A RID: 9754
		private static readonly object _selectedValue = new object();

		// Token: 0x0200090D RID: 2317
		private class TextSelectionHighlightChangedEventArgs : HighlightChangedEventArgs
		{
			// Token: 0x060085E4 RID: 34276 RVA: 0x0024B63C File Offset: 0x0024983C
			internal TextSelectionHighlightChangedEventArgs(ITextPointer invalidRangeLeftStart, ITextPointer invalidRangeLeftEnd, ITextPointer invalidRangeRightStart, ITextPointer invalidRangeRightEnd)
			{
				Invariant.Assert(invalidRangeLeftStart != invalidRangeLeftEnd || invalidRangeRightStart != invalidRangeRightEnd, "Unexpected empty range!");
				List<TextSegment> list;
				if (invalidRangeLeftStart.CompareTo(invalidRangeLeftEnd) == 0)
				{
					list = new List<TextSegment>(1);
					list.Add(new TextSegment(invalidRangeRightStart, invalidRangeRightEnd));
				}
				else if (invalidRangeRightStart.CompareTo(invalidRangeRightEnd) == 0)
				{
					list = new List<TextSegment>(1);
					list.Add(new TextSegment(invalidRangeLeftStart, invalidRangeLeftEnd));
				}
				else
				{
					list = new List<TextSegment>(2);
					list.Add(new TextSegment(invalidRangeLeftStart, invalidRangeLeftEnd));
					list.Add(new TextSegment(invalidRangeRightStart, invalidRangeRightEnd));
				}
				this._ranges = new ReadOnlyCollection<TextSegment>(list);
			}

			// Token: 0x17001E3C RID: 7740
			// (get) Token: 0x060085E5 RID: 34277 RVA: 0x0024B6D6 File Offset: 0x002498D6
			internal override IList Ranges
			{
				get
				{
					return this._ranges;
				}
			}

			// Token: 0x17001E3D RID: 7741
			// (get) Token: 0x060085E6 RID: 34278 RVA: 0x000C73A9 File Offset: 0x000C55A9
			internal override Type OwnerType
			{
				get
				{
					return typeof(TextSelection);
				}
			}

			// Token: 0x04004324 RID: 17188
			private readonly ReadOnlyCollection<TextSegment> _ranges;
		}
	}
}
