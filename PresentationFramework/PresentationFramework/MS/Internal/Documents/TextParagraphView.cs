using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Documents
{
	// Token: 0x020006F6 RID: 1782
	internal class TextParagraphView : TextViewBase
	{
		// Token: 0x060072A9 RID: 29353 RVA: 0x0020EFBB File Offset: 0x0020D1BB
		internal TextParagraphView(TextBlock owner, ITextContainer textContainer)
		{
			this._owner = owner;
			this._textContainer = textContainer;
		}

		// Token: 0x060072AA RID: 29354 RVA: 0x0020EFD4 File Offset: 0x0020D1D4
		internal override ITextPointer GetTextPositionFromPoint(Point point, bool snapToText)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ITextPointer textPositionFromPoint = TextParagraphView.GetTextPositionFromPoint(this.Lines, point, snapToText);
			Invariant.Assert(textPositionFromPoint == null || textPositionFromPoint.HasValidLayout);
			return textPositionFromPoint;
		}

		// Token: 0x060072AB RID: 29355 RVA: 0x0020F01C File Offset: 0x0020D21C
		internal override Rect GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform)
		{
			transform = Transform.Identity;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			return this._owner.GetRectangleFromTextPosition(position);
		}

		// Token: 0x060072AC RID: 29356 RVA: 0x0020F074 File Offset: 0x0020D274
		internal override Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (startPosition == null)
			{
				throw new ArgumentNullException("startPosition");
			}
			if (endPosition == null)
			{
				throw new ArgumentNullException("endPosition");
			}
			ValidationHelper.VerifyPosition(this._textContainer, startPosition, "startPosition");
			ValidationHelper.VerifyDirection(startPosition.LogicalDirection, "startPosition.LogicalDirection");
			ValidationHelper.VerifyPosition(this._textContainer, endPosition, "endPosition");
			return this._owner.GetTightBoundingGeometryFromTextPositions(startPosition, endPosition);
		}

		// Token: 0x060072AD RID: 29357 RVA: 0x0020F0F8 File Offset: 0x0020D2F8
		internal override ITextPointer GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			newSuggestedX = suggestedX;
			linesMoved = 0;
			if (count == 0)
			{
				return position;
			}
			ReadOnlyCollection<LineResult> lines = this.Lines;
			int num = TextParagraphView.GetLineFromPosition(lines, position);
			if (num < 0 || num >= lines.Count)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			int num2 = num;
			num = Math.Max(0, num + count);
			num = Math.Min(lines.Count - 1, num);
			linesMoved = num - num2;
			ITextPointer textPointer;
			if (linesMoved == 0)
			{
				textPointer = position;
			}
			else if (!DoubleUtil.IsNaN(suggestedX))
			{
				textPointer = lines[num].GetTextPositionFromDistance(suggestedX);
			}
			else
			{
				textPointer = lines[num].StartPosition.CreatePointer(LogicalDirection.Forward);
			}
			Invariant.Assert(textPointer == null || textPointer.HasValidLayout);
			return textPointer;
		}

		// Token: 0x060072AE RID: 29358 RVA: 0x0020F1D8 File Offset: 0x0020D3D8
		internal override bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			int lineFromPosition = TextParagraphView.GetLineFromPosition(this.Lines, position);
			int startPositionCP = this.Lines[lineFromPosition].StartPositionCP;
			return this._owner.IsAtCaretUnitBoundary(position, startPositionCP, lineFromPosition);
		}

		// Token: 0x060072AF RID: 29359 RVA: 0x0020F23C File Offset: 0x0020D43C
		internal override ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			int lineFromPosition = TextParagraphView.GetLineFromPosition(this.Lines, position);
			int startPositionCP = this.Lines[lineFromPosition].StartPositionCP;
			ITextPointer nextCaretUnitPosition = this._owner.GetNextCaretUnitPosition(position, direction, startPositionCP, lineFromPosition);
			Invariant.Assert(nextCaretUnitPosition == null || nextCaretUnitPosition.HasValidLayout);
			return nextCaretUnitPosition;
		}

		// Token: 0x060072B0 RID: 29360 RVA: 0x0020F2B4 File Offset: 0x0020D4B4
		internal override ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			int lineFromPosition = TextParagraphView.GetLineFromPosition(this.Lines, position);
			int startPositionCP = this.Lines[lineFromPosition].StartPositionCP;
			ITextPointer backspaceCaretUnitPosition = this._owner.GetBackspaceCaretUnitPosition(position, startPositionCP, lineFromPosition);
			Invariant.Assert(backspaceCaretUnitPosition == null || backspaceCaretUnitPosition.HasValidLayout);
			return backspaceCaretUnitPosition;
		}

		// Token: 0x060072B1 RID: 29361 RVA: 0x0020F32C File Offset: 0x0020D52C
		internal override TextSegment GetLineRange(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			ReadOnlyCollection<LineResult> lines = this.Lines;
			int lineFromPosition = TextParagraphView.GetLineFromPosition(lines, position);
			return new TextSegment(lines[lineFromPosition].StartPosition, lines[lineFromPosition].GetContentEndPosition(), true);
		}

		// Token: 0x060072B2 RID: 29362 RVA: 0x0020F39D File Offset: 0x0020D59D
		internal override bool Contains(ITextPointer position)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			return true;
		}

		// Token: 0x060072B3 RID: 29363 RVA: 0x0020F3D7 File Offset: 0x0020D5D7
		internal override bool Validate()
		{
			this._owner.UpdateLayout();
			return this.IsValid;
		}

		// Token: 0x060072B4 RID: 29364 RVA: 0x0020F3EC File Offset: 0x0020D5EC
		internal static ITextPointer GetTextPositionFromPoint(ReadOnlyCollection<LineResult> lines, Point point, bool snapToText)
		{
			int lineFromPoint = TextParagraphView.GetLineFromPoint(lines, point, snapToText);
			ITextPointer result;
			if (lineFromPoint < 0)
			{
				result = null;
			}
			else
			{
				result = lines[lineFromPoint].GetTextPositionFromDistance(point.X);
			}
			return result;
		}

		// Token: 0x060072B5 RID: 29365 RVA: 0x0020F420 File Offset: 0x0020D620
		internal static int GetLineFromPosition(ReadOnlyCollection<LineResult> lines, ITextPointer position)
		{
			int i = 0;
			int num = lines.Count - 1;
			int num2 = lines[0].StartPosition.GetOffsetToPosition(position) + lines[0].StartPositionCP;
			if (num2 >= lines[0].StartPositionCP && num2 <= lines[lines.Count - 1].EndPositionCP)
			{
				int num3 = 0;
				while (i < num)
				{
					if (num - i < 2)
					{
						num3 = ((num3 == i) ? num : i);
					}
					else
					{
						num3 = i + (num - i) / 2;
					}
					if (num2 < lines[num3].StartPositionCP)
					{
						num = num3;
					}
					else if (num2 > lines[num3].EndPositionCP)
					{
						i = num3;
					}
					else if (num2 == lines[num3].EndPositionCP)
					{
						if (position.LogicalDirection == LogicalDirection.Forward && num3 != lines.Count - 1)
						{
							num3++;
							break;
						}
						break;
					}
					else
					{
						if (num2 == lines[num3].StartPositionCP && position.LogicalDirection == LogicalDirection.Backward && num3 != 0)
						{
							num3--;
							break;
						}
						break;
					}
				}
				return num3;
			}
			if (num2 >= lines[0].StartPositionCP)
			{
				return lines.Count - 1;
			}
			return 0;
		}

		// Token: 0x060072B6 RID: 29366 RVA: 0x00205824 File Offset: 0x00203A24
		internal void OnUpdated()
		{
			this.OnUpdated(EventArgs.Empty);
		}

		// Token: 0x060072B7 RID: 29367 RVA: 0x0020F530 File Offset: 0x0020D730
		internal void Invalidate()
		{
			this._lines = null;
		}

		// Token: 0x17001B39 RID: 6969
		// (get) Token: 0x060072B8 RID: 29368 RVA: 0x0020F539 File Offset: 0x0020D739
		internal override UIElement RenderScope
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17001B3A RID: 6970
		// (get) Token: 0x060072B9 RID: 29369 RVA: 0x0020F541 File Offset: 0x0020D741
		internal override ITextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x17001B3B RID: 6971
		// (get) Token: 0x060072BA RID: 29370 RVA: 0x0020F549 File Offset: 0x0020D749
		internal override bool IsValid
		{
			get
			{
				return this._owner.IsLayoutDataValid;
			}
		}

		// Token: 0x17001B3C RID: 6972
		// (get) Token: 0x060072BB RID: 29371 RVA: 0x0020F558 File Offset: 0x0020D758
		internal override ReadOnlyCollection<TextSegment> TextSegments
		{
			get
			{
				return new ReadOnlyCollection<TextSegment>(new List<TextSegment>(1)
				{
					new TextSegment(this._textContainer.Start, this._textContainer.End, true)
				});
			}
		}

		// Token: 0x17001B3D RID: 6973
		// (get) Token: 0x060072BC RID: 29372 RVA: 0x0020F594 File Offset: 0x0020D794
		internal ReadOnlyCollection<LineResult> Lines
		{
			get
			{
				if (this._lines == null)
				{
					this._lines = this._owner.GetLineResults();
				}
				return this._lines;
			}
		}

		// Token: 0x060072BD RID: 29373 RVA: 0x0020F5B8 File Offset: 0x0020D7B8
		internal static int GetLineFromPoint(ReadOnlyCollection<LineResult> lines, Point point, bool snapToText)
		{
			int result;
			bool flag = TextParagraphView.GetVerticalLineFromPoint(lines, point, snapToText, out result);
			if (flag)
			{
				flag = TextParagraphView.GetHorizontalLineFromPoint(lines, point, snapToText, ref result);
			}
			if (!flag)
			{
				return -1;
			}
			return result;
		}

		// Token: 0x060072BE RID: 29374 RVA: 0x0020F5E4 File Offset: 0x0020D7E4
		private static bool GetVerticalLineFromPoint(ReadOnlyCollection<LineResult> lines, Point point, bool snapToText, out int lineIndex)
		{
			bool flag = false;
			double height = lines[0].LayoutBox.Height;
			lineIndex = Math.Max(Math.Min((int)(point.Y / height), lines.Count - 1), 0);
			while (!flag)
			{
				Rect layoutBox = lines[lineIndex].LayoutBox;
				if (point.Y < layoutBox.Y)
				{
					if (lineIndex <= 0)
					{
						flag = snapToText;
						break;
					}
					lineIndex--;
				}
				else
				{
					if (point.Y <= layoutBox.Y + layoutBox.Height)
					{
						double num = 0.0;
						if (lineIndex > 0)
						{
							Rect layoutBox2 = lines[lineIndex - 1].LayoutBox;
							num = layoutBox.Y - (layoutBox2.Y + layoutBox2.Height);
						}
						if (num < 0.0)
						{
							if (point.Y < layoutBox.Y - num / 2.0)
							{
								lineIndex--;
							}
						}
						else
						{
							num = 0.0;
							if (lineIndex < lines.Count - 1)
							{
								num = lines[lineIndex + 1].LayoutBox.Y - (layoutBox.Y + layoutBox.Height);
							}
							if (num < 0.0 && point.Y > layoutBox.Y + layoutBox.Height + num / 2.0)
							{
								lineIndex++;
							}
						}
						flag = true;
						break;
					}
					if (lineIndex >= lines.Count - 1)
					{
						flag = snapToText;
						break;
					}
					Rect layoutBox3 = lines[lineIndex + 1].LayoutBox;
					if (point.Y < layoutBox3.Y)
					{
						double num2 = layoutBox3.Y - (layoutBox.Y + layoutBox.Height);
						if (point.Y > layoutBox.Y + layoutBox.Height + num2 / 2.0)
						{
							lineIndex++;
						}
						flag = snapToText;
						break;
					}
					lineIndex++;
				}
			}
			return flag;
		}

		// Token: 0x060072BF RID: 29375 RVA: 0x0020F7FC File Offset: 0x0020D9FC
		private static bool GetHorizontalLineFromPoint(ReadOnlyCollection<LineResult> lines, Point point, bool snapToText, ref int lineIndex)
		{
			bool result = false;
			bool flag = true;
			while (flag)
			{
				Rect layoutBox = lines[lineIndex].LayoutBox;
				if (point.X < layoutBox.X && lineIndex > 0)
				{
					Rect layoutBox2 = lines[lineIndex - 1].LayoutBox;
					if (!DoubleUtil.AreClose(layoutBox2.Y, layoutBox.Y))
					{
						result = snapToText;
						break;
					}
					if (point.X > layoutBox2.X + layoutBox2.Width)
					{
						double num = Math.Max(layoutBox.X - (layoutBox2.X + layoutBox2.Width), 0.0);
						if (point.X < layoutBox.X - num / 2.0)
						{
							lineIndex--;
						}
						result = snapToText;
						break;
					}
					lineIndex--;
				}
				else
				{
					if (point.X <= layoutBox.X + layoutBox.Width || lineIndex >= lines.Count - 1)
					{
						result = (snapToText || (point.X >= layoutBox.X && point.X <= layoutBox.X + layoutBox.Width));
						break;
					}
					Rect layoutBox2 = lines[lineIndex + 1].LayoutBox;
					if (!DoubleUtil.AreClose(layoutBox2.Y, layoutBox.Y))
					{
						result = snapToText;
						break;
					}
					if (point.X < layoutBox2.X)
					{
						double num = Math.Max(layoutBox2.X - (layoutBox.X + layoutBox.Width), 0.0);
						if (point.X > layoutBox2.X - num / 2.0)
						{
							lineIndex++;
						}
						result = snapToText;
						break;
					}
					lineIndex++;
				}
			}
			return result;
		}

		// Token: 0x04003766 RID: 14182
		private readonly TextBlock _owner;

		// Token: 0x04003767 RID: 14183
		private readonly ITextContainer _textContainer;

		// Token: 0x04003768 RID: 14184
		private ReadOnlyCollection<LineResult> _lines;
	}
}
