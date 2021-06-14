using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020006F5 RID: 1781
	internal class TextDocumentView : TextViewBase
	{
		// Token: 0x0600725A RID: 29274 RVA: 0x0020B2A5 File Offset: 0x002094A5
		internal TextDocumentView(FlowDocumentPage owner, ITextContainer textContainer)
		{
			this._owner = owner;
			this._textContainer = textContainer;
		}

		// Token: 0x0600725B RID: 29275 RVA: 0x0020B2BC File Offset: 0x002094BC
		internal override ITextPointer GetTextPositionFromPoint(Point point, bool snapToText)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			this._owner.EnsureValidVisuals();
			this.TransformToContent(ref point);
			return this.GetTextPositionFromPoint(this.Columns, this.FloatingElements, point, snapToText);
		}

		// Token: 0x0600725C RID: 29276 RVA: 0x0020B308 File Offset: 0x00209508
		internal override Rect GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			if (!this.ContainsCore(position))
			{
				throw new ArgumentOutOfRangeException("position");
			}
			this._owner.EnsureValidVisuals();
			Rect rectangleFromTextPosition = this.GetRectangleFromTextPosition(this.Columns, this.FloatingElements, position);
			this.TransformFromContent(ref rectangleFromTextPosition, out transform);
			return rectangleFromTextPosition;
		}

		// Token: 0x0600725D RID: 29277 RVA: 0x0020B37C File Offset: 0x0020957C
		internal override Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			Geometry geometry = null;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ValidationHelper.VerifyPosition(this._textContainer, startPosition, "startPosition");
			ValidationHelper.VerifyPosition(this._textContainer, endPosition, "endPosition");
			this._owner.EnsureValidVisuals();
			Rect rect = this.CalculateViewportRect();
			bool flag = false;
			if (this.FloatingElements.Count > 0)
			{
				Geometry tightBoundingGeometryFromTextPositionsInFloatingElements = TextDocumentView.GetTightBoundingGeometryFromTextPositionsInFloatingElements(this.FloatingElements, startPosition, endPosition, 0.0, rect, out flag);
				CaretElement.AddGeometry(ref geometry, tightBoundingGeometryFromTextPositionsInFloatingElements);
				if (geometry != null)
				{
					this.TransformFromContent(geometry);
				}
			}
			if (!flag)
			{
				Invariant.Assert(geometry == null);
				ReadOnlyCollection<TextSegment> textSegments = this.TextSegments;
				for (int i = 0; i < textSegments.Count; i++)
				{
					TextSegment textSegment = textSegments[i];
					ITextPointer textPointer = (startPosition.CompareTo(textSegment.Start) > 0) ? startPosition : textSegment.Start;
					ITextPointer textPointer2 = (endPosition.CompareTo(textSegment.End) < 0) ? endPosition : textSegment.End;
					if (textPointer.CompareTo(textPointer2) < 0)
					{
						ReadOnlyCollection<ColumnResult> columns = this.Columns;
						for (int j = 0; j < columns.Count; j++)
						{
							Rect layoutBox = columns[j].LayoutBox;
							layoutBox.X = rect.X;
							if (layoutBox.IntersectsWith(rect))
							{
								Geometry tightBoundingGeometryFromTextPositionsHelper = TextDocumentView.GetTightBoundingGeometryFromTextPositionsHelper(columns[j].Paragraphs, textPointer, textPointer2, 0.0, rect);
								CaretElement.AddGeometry(ref geometry, tightBoundingGeometryFromTextPositionsHelper);
							}
						}
						if (geometry != null)
						{
							this.TransformFromContent(geometry);
						}
					}
				}
			}
			return geometry;
		}

		// Token: 0x0600725E RID: 29278 RVA: 0x0020B514 File Offset: 0x00209714
		private Rect CalculateViewportRect()
		{
			Rect result = Rect.Empty;
			if (this.RenderScope is IScrollInfo)
			{
				IScrollInfo scrollInfo = (IScrollInfo)this.RenderScope;
				if (scrollInfo.ViewportWidth != 0.0 && scrollInfo.ViewportHeight != 0.0)
				{
					result = new Rect(scrollInfo.HorizontalOffset, scrollInfo.VerticalOffset, scrollInfo.ViewportWidth, scrollInfo.ViewportHeight);
				}
			}
			if (result.IsEmpty)
			{
				result = this._owner.Viewport;
			}
			this.TransformToContent(ref result);
			return result;
		}

		// Token: 0x0600725F RID: 29279 RVA: 0x0020B5A0 File Offset: 0x002097A0
		internal override ITextPointer GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			if (!this.ContainsCore(position))
			{
				throw new ArgumentOutOfRangeException("position");
			}
			this._owner.EnsureValidVisuals();
			Point point = new Point(suggestedX, 0.0);
			this.TransformToContent(ref point);
			suggestedX = (newSuggestedX = point.X);
			linesMoved = count;
			if (count == 0)
			{
				return position;
			}
			bool flag;
			ITextPointer textPointer = this.GetPositionAtNextLine(this.Columns, this.FloatingElements, position, suggestedX, ref count, out newSuggestedX, out flag);
			linesMoved -= count;
			point = new Point(newSuggestedX, 0.0);
			this.TransformFromContent(ref point);
			newSuggestedX = point.X;
			if (textPointer == null || !this.ContainsCore(textPointer))
			{
				textPointer = position;
				linesMoved = 0;
			}
			return textPointer;
		}

		// Token: 0x06007260 RID: 29280 RVA: 0x0020B684 File Offset: 0x00209884
		internal override bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			if (!this.ContainsCore(position))
			{
				throw new ArgumentOutOfRangeException("position");
			}
			return this.IsAtCaretUnitBoundary(this.Columns, this.FloatingElements, position);
		}

		// Token: 0x06007261 RID: 29281 RVA: 0x0020B6E4 File Offset: 0x002098E4
		internal override ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			ValidationHelper.VerifyDirection(direction, "direction");
			if (!this.ContainsCore(position))
			{
				throw new ArgumentOutOfRangeException("position");
			}
			return this.GetNextCaretUnitPosition(this.Columns, this.FloatingElements, position, direction);
		}

		// Token: 0x06007262 RID: 29282 RVA: 0x0020B750 File Offset: 0x00209950
		internal override ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			if (!this.ContainsCore(position))
			{
				throw new ArgumentOutOfRangeException("position");
			}
			return this.GetBackspaceCaretUnitPosition(this.Columns, this.FloatingElements, position);
		}

		// Token: 0x06007263 RID: 29283 RVA: 0x0020B7B0 File Offset: 0x002099B0
		internal override TextSegment GetLineRange(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			if (!this.ContainsCore(position))
			{
				throw new ArgumentOutOfRangeException("position");
			}
			return this.GetLineRangeFromPosition(this.Columns, this.FloatingElements, position);
		}

		// Token: 0x06007264 RID: 29284 RVA: 0x0020B810 File Offset: 0x00209A10
		internal override ReadOnlyCollection<GlyphRun> GetGlyphRuns(ITextPointer start, ITextPointer end)
		{
			List<GlyphRun> list = new List<GlyphRun>();
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ValidationHelper.VerifyPosition(this._textContainer, start, "start");
			ValidationHelper.VerifyPosition(this._textContainer, end, "end");
			ValidationHelper.VerifyPositionPair(start, end);
			if (!this.ContainsCore(start))
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (!this.ContainsCore(end))
			{
				throw new ArgumentOutOfRangeException("end");
			}
			this.GetGlyphRuns(list, start, end, this.Columns, this.FloatingElements);
			return new ReadOnlyCollection<GlyphRun>(list);
		}

		// Token: 0x06007265 RID: 29285 RVA: 0x0020B8A7 File Offset: 0x00209AA7
		internal override bool Contains(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ValidationHelper.VerifyPosition(this._textContainer, position, "position");
			return this.ContainsCore(position);
		}

		// Token: 0x06007266 RID: 29286 RVA: 0x0020B8D9 File Offset: 0x00209AD9
		internal override bool Validate()
		{
			return this.IsValid;
		}

		// Token: 0x06007267 RID: 29287 RVA: 0x0020B8E1 File Offset: 0x00209AE1
		internal override void ThrottleBackgroundTasksForUserInput()
		{
			this._owner.StructuralCache.ThrottleBackgroundFormatting();
		}

		// Token: 0x06007268 RID: 29288 RVA: 0x0020B8F3 File Offset: 0x00209AF3
		internal CellInfo GetCellInfoFromPoint(Point point, Table tableFilter)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			return this.GetCellInfoFromPoint(this.Columns, this.FloatingElements, point, tableFilter);
		}

		// Token: 0x06007269 RID: 29289 RVA: 0x00205824 File Offset: 0x00203A24
		internal void OnUpdated()
		{
			this.OnUpdated(EventArgs.Empty);
		}

		// Token: 0x0600726A RID: 29290 RVA: 0x0020B921 File Offset: 0x00209B21
		internal void Invalidate()
		{
			this._columns = null;
			this._segments = null;
			this._floatingElements = null;
		}

		// Token: 0x0600726B RID: 29291 RVA: 0x0020B938 File Offset: 0x00209B38
		internal static bool Contains(ITextPointer position, ReadOnlyCollection<TextSegment> segments)
		{
			bool flag = false;
			Invariant.Assert(segments != null);
			foreach (TextSegment textSegment in segments)
			{
				if (textSegment.Start.CompareTo(position) < 0 && textSegment.End.CompareTo(position) > 0)
				{
					flag = true;
					break;
				}
				if (textSegment.Start.CompareTo(position) == 0)
				{
					if (position.LogicalDirection == LogicalDirection.Forward)
					{
						flag = true;
						break;
					}
					if (textSegment.Start.LogicalDirection == LogicalDirection.Backward)
					{
						flag = true;
						break;
					}
				}
				if (textSegment.End.CompareTo(position) == 0)
				{
					if (position.LogicalDirection == LogicalDirection.Backward)
					{
						flag = true;
						break;
					}
					if (textSegment.End.LogicalDirection == LogicalDirection.Forward)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag && segments.Count > 0)
			{
				if (position.TextContainer.Start.CompareTo(position) == 0 && position.LogicalDirection == LogicalDirection.Backward)
				{
					flag = (position.TextContainer.Start.CompareTo(segments[0].Start) == 0);
				}
				else if (position.TextContainer.End.CompareTo(position) == 0 && position.LogicalDirection == LogicalDirection.Forward)
				{
					flag = (position.TextContainer.End.CompareTo(segments[segments.Count - 1].End) == 0);
				}
			}
			return flag;
		}

		// Token: 0x17001B32 RID: 6962
		// (get) Token: 0x0600726C RID: 29292 RVA: 0x0020BAA4 File Offset: 0x00209CA4
		internal override UIElement RenderScope
		{
			get
			{
				UIElement result = null;
				if (!this._owner.IsDisposed)
				{
					Visual visual = this._owner.Visual;
					while (visual != null && !(visual is UIElement))
					{
						visual = (VisualTreeHelper.GetParent(visual) as Visual);
					}
					result = (visual as UIElement);
				}
				return result;
			}
		}

		// Token: 0x17001B33 RID: 6963
		// (get) Token: 0x0600726D RID: 29293 RVA: 0x0020BAED File Offset: 0x00209CED
		internal override ITextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x17001B34 RID: 6964
		// (get) Token: 0x0600726E RID: 29294 RVA: 0x0020BAF5 File Offset: 0x00209CF5
		internal override bool IsValid
		{
			get
			{
				return this._owner.IsLayoutDataValid;
			}
		}

		// Token: 0x17001B35 RID: 6965
		// (get) Token: 0x0600726F RID: 29295 RVA: 0x0020BB02 File Offset: 0x00209D02
		internal override ReadOnlyCollection<TextSegment> TextSegments
		{
			get
			{
				if (!this.IsValid)
				{
					return new ReadOnlyCollection<TextSegment>(new List<TextSegment>());
				}
				return this.TextSegmentsCore;
			}
		}

		// Token: 0x17001B36 RID: 6966
		// (get) Token: 0x06007270 RID: 29296 RVA: 0x0020BB1D File Offset: 0x00209D1D
		private ReadOnlyCollection<TextSegment> TextSegmentsCore
		{
			get
			{
				if (this._segments == null)
				{
					this._segments = this.GetTextSegments();
					Invariant.Assert(this._segments != null, "TextSegment collection is empty.");
				}
				return this._segments;
			}
		}

		// Token: 0x17001B37 RID: 6967
		// (get) Token: 0x06007271 RID: 29297 RVA: 0x0020BB4C File Offset: 0x00209D4C
		internal ReadOnlyCollection<ColumnResult> Columns
		{
			get
			{
				Invariant.Assert(this.IsValid, "TextView is not updated.");
				if (this._columns == null)
				{
					this._columns = this._owner.GetColumnResults(out this._hasTextContent);
					Invariant.Assert(this._columns != null, "Column collection is null.");
				}
				return this._columns;
			}
		}

		// Token: 0x17001B38 RID: 6968
		// (get) Token: 0x06007272 RID: 29298 RVA: 0x0020BBA4 File Offset: 0x00209DA4
		internal ReadOnlyCollection<ParagraphResult> FloatingElements
		{
			get
			{
				Invariant.Assert(this.IsValid, "TextView is not updated.");
				if (this._floatingElements == null)
				{
					this._floatingElements = this._owner.FloatingElementResults;
					Invariant.Assert(this._floatingElements != null, "Floating elements collection is null.");
				}
				return this._floatingElements;
			}
		}

		// Token: 0x06007273 RID: 29299 RVA: 0x0020BBF4 File Offset: 0x00209DF4
		private ITextPointer GetTextPositionFromPoint(ReadOnlyCollection<ParagraphResult> paragraphs, ReadOnlyCollection<ParagraphResult> floatingElements, Point point, bool snapToText, bool snapToTextInFloatingElements)
		{
			Invariant.Assert(paragraphs != null, "Paragraph collection is empty.");
			Invariant.Assert(floatingElements != null, "Floating element collection is empty.");
			int num = this.GetParagraphFromPointInFloatingElements(floatingElements, point, snapToTextInFloatingElements);
			ParagraphResult paragraph;
			if (num < 0)
			{
				Invariant.Assert(!snapToTextInFloatingElements || floatingElements.Count == 0, "When snap to text is enabled a valid text position is required if paragraphs exist.");
				if (snapToTextInFloatingElements)
				{
					return null;
				}
				num = this.GetParagraphFromPoint(paragraphs, point, snapToText);
				if (num < 0)
				{
					Invariant.Assert(!snapToText || paragraphs.Count == 0, "When snap to text is enabled a valid text position is required if paragraphs exist.");
					return null;
				}
				Invariant.Assert(num < paragraphs.Count);
				paragraph = paragraphs[num];
			}
			else
			{
				Invariant.Assert(num < floatingElements.Count);
				paragraph = floatingElements[num];
			}
			return this.GetTextPositionFromPoint(paragraph, point, snapToText);
		}

		// Token: 0x06007274 RID: 29300 RVA: 0x0020BCB4 File Offset: 0x00209EB4
		private ITextPointer GetTextPositionFromPoint(ParagraphResult paragraph, Point point, bool snapToText)
		{
			ITextPointer result = null;
			Rect layoutBox = paragraph.LayoutBox;
			if (paragraph is ContainerParagraphResult)
			{
				ReadOnlyCollection<ParagraphResult> paragraphs = ((ContainerParagraphResult)paragraph).Paragraphs;
				Invariant.Assert(paragraphs != null, "Paragraph collection is null.");
				if (paragraphs.Count > 0)
				{
					result = this.GetTextPositionFromPoint(paragraphs, TextDocumentView._emptyParagraphCollection, point, snapToText, false);
				}
				else if (point.X <= layoutBox.Width)
				{
					result = paragraph.StartPosition.CreatePointer(LogicalDirection.Forward);
				}
				else
				{
					result = paragraph.EndPosition.CreatePointer(LogicalDirection.Backward);
				}
			}
			else if (paragraph is TextParagraphResult)
			{
				ReadOnlyCollection<LineResult> lines = ((TextParagraphResult)paragraph).Lines;
				Invariant.Assert(lines != null, "Lines collection is null");
				if (!((TextParagraphResult)paragraph).HasTextContent)
				{
					result = null;
				}
				else
				{
					result = TextParagraphView.GetTextPositionFromPoint(lines, point, snapToText);
				}
			}
			else if (paragraph is TableParagraphResult)
			{
				ReadOnlyCollection<ParagraphResult> paragraphs2 = ((TableParagraphResult)paragraph).Paragraphs;
				Invariant.Assert(paragraphs2 != null, "Paragraph collection is null.");
				int paragraphFromPoint = this.GetParagraphFromPoint(paragraphs2, point, snapToText);
				if (paragraphFromPoint != -1)
				{
					ParagraphResult paragraphResult = paragraphs2[paragraphFromPoint];
					if (point.X > paragraphResult.LayoutBox.Right)
					{
						result = ((TextElement)paragraphResult.Element).ElementEnd;
					}
					else
					{
						ReadOnlyCollection<ParagraphResult> paragraphsFromPoint = ((TableParagraphResult)paragraph).GetParagraphsFromPoint(point, snapToText);
						result = this.GetTextPositionFromPoint(paragraphsFromPoint, TextDocumentView._emptyParagraphCollection, point, snapToText, false);
					}
				}
				else
				{
					result = null;
					if (snapToText)
					{
						result = ((TextElement)paragraph.Element).ContentStart;
					}
				}
			}
			else if (paragraph is SubpageParagraphResult)
			{
				SubpageParagraphResult subpageParagraphResult = (SubpageParagraphResult)paragraph;
				point.X -= subpageParagraphResult.ContentOffset.X;
				point.Y -= subpageParagraphResult.ContentOffset.Y;
				result = this.GetTextPositionFromPoint(subpageParagraphResult.Columns, subpageParagraphResult.FloatingElements, point, snapToText);
			}
			else if (paragraph is FigureParagraphResult || paragraph is FloaterParagraphResult)
			{
				ReadOnlyCollection<ColumnResult> columns;
				ReadOnlyCollection<ParagraphResult> floatingElements;
				if (paragraph is FloaterParagraphResult)
				{
					FloaterParagraphResult floaterParagraphResult = (FloaterParagraphResult)paragraph;
					columns = floaterParagraphResult.Columns;
					floatingElements = floaterParagraphResult.FloatingElements;
					TextDocumentView.TransformToSubpage(ref point, floaterParagraphResult.ContentOffset);
				}
				else
				{
					FigureParagraphResult figureParagraphResult = (FigureParagraphResult)paragraph;
					columns = figureParagraphResult.Columns;
					floatingElements = figureParagraphResult.FloatingElements;
					TextDocumentView.TransformToSubpage(ref point, figureParagraphResult.ContentOffset);
				}
				Invariant.Assert(columns != null, "Columns collection is null.");
				Invariant.Assert(floatingElements != null, "Floating elements collection is null.");
				if (columns.Count > 0 || floatingElements.Count > 0)
				{
					result = this.GetTextPositionFromPoint(columns, floatingElements, point, snapToText);
				}
				else
				{
					result = null;
				}
			}
			else if (paragraph is UIElementParagraphResult)
			{
				BlockUIContainer blockUIContainer = paragraph.Element as BlockUIContainer;
				if (blockUIContainer != null)
				{
					result = null;
					if (layoutBox.Contains(point) || snapToText)
					{
						if (DoubleUtil.LessThanOrClose(point.X, layoutBox.X + layoutBox.Width / 2.0))
						{
							result = blockUIContainer.ContentStart.CreatePointer(LogicalDirection.Forward);
						}
						else
						{
							result = blockUIContainer.ContentEnd.CreatePointer(LogicalDirection.Backward);
						}
					}
				}
			}
			else if (point.X <= layoutBox.Width)
			{
				result = paragraph.StartPosition.CreatePointer(LogicalDirection.Forward);
			}
			else
			{
				result = paragraph.EndPosition.CreatePointer(LogicalDirection.Backward);
			}
			return result;
		}

		// Token: 0x06007275 RID: 29301 RVA: 0x0020BFF8 File Offset: 0x0020A1F8
		private ITextPointer GetTextPositionFromPoint(ReadOnlyCollection<ColumnResult> columns, ReadOnlyCollection<ParagraphResult> floatingElements, Point point, bool snapToText)
		{
			Invariant.Assert(floatingElements != null);
			int columnFromPoint = this.GetColumnFromPoint(columns, point, snapToText);
			ITextPointer textPointer;
			if (columnFromPoint < 0 && floatingElements.Count == 0)
			{
				textPointer = null;
			}
			else
			{
				bool snapToTextInFloatingElements = false;
				ReadOnlyCollection<ParagraphResult> paragraphs;
				if (columnFromPoint < columns.Count && columnFromPoint >= 0)
				{
					ColumnResult columnResult = columns[columnFromPoint];
					if (!columnResult.HasTextContent)
					{
						snapToTextInFloatingElements = true;
					}
					paragraphs = columnResult.Paragraphs;
				}
				else
				{
					paragraphs = TextDocumentView._emptyParagraphCollection;
				}
				textPointer = this.GetTextPositionFromPoint(paragraphs, floatingElements, point, snapToText, snapToTextInFloatingElements);
			}
			if (textPointer != null && !this.ContainsCore(textPointer))
			{
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x06007276 RID: 29302 RVA: 0x0020C07C File Offset: 0x0020A27C
		private CellInfo GetCellInfoFromPoint(ReadOnlyCollection<ParagraphResult> paragraphs, ReadOnlyCollection<ParagraphResult> floatingElements, Point point, Table tableFilter)
		{
			CellInfo result = null;
			Invariant.Assert(paragraphs != null, "Paragraph collection is empty.");
			Invariant.Assert(floatingElements != null, "Floating element collection is empty.");
			int num = this.GetParagraphFromPointInFloatingElements(floatingElements, point, false);
			ParagraphResult paragraphResult = null;
			if (num >= 0)
			{
				Invariant.Assert(num < floatingElements.Count);
				paragraphResult = floatingElements[num];
			}
			else
			{
				num = this.GetParagraphFromPoint(paragraphs, point, false);
				if (num >= 0)
				{
					Invariant.Assert(num < paragraphs.Count);
					paragraphResult = paragraphs[num];
				}
			}
			if (paragraphResult != null)
			{
				result = this.GetCellInfoFromPoint(paragraphResult, point, tableFilter);
			}
			return result;
		}

		// Token: 0x06007277 RID: 29303 RVA: 0x0020C104 File Offset: 0x0020A304
		private CellInfo GetCellInfoFromPoint(ParagraphResult paragraph, Point point, Table tableFilter)
		{
			CellInfo cellInfo = null;
			if (paragraph is ContainerParagraphResult)
			{
				ReadOnlyCollection<ParagraphResult> paragraphs = ((ContainerParagraphResult)paragraph).Paragraphs;
				Invariant.Assert(paragraphs != null, "Paragraph collection is null");
				if (paragraphs.Count > 0)
				{
					cellInfo = this.GetCellInfoFromPoint(paragraphs, TextDocumentView._emptyParagraphCollection, point, tableFilter);
				}
			}
			else if (paragraph is TableParagraphResult)
			{
				ReadOnlyCollection<ParagraphResult> paragraphsFromPoint = ((TableParagraphResult)paragraph).GetParagraphsFromPoint(point, false);
				Invariant.Assert(paragraphsFromPoint != null, "Paragraph collection is null");
				if (paragraphsFromPoint.Count > 0)
				{
					cellInfo = this.GetCellInfoFromPoint(paragraphsFromPoint, TextDocumentView._emptyParagraphCollection, point, tableFilter);
				}
				if (cellInfo == null)
				{
					cellInfo = ((TableParagraphResult)paragraph).GetCellInfoFromPoint(point);
				}
			}
			else if (paragraph is SubpageParagraphResult)
			{
				SubpageParagraphResult subpageParagraphResult = (SubpageParagraphResult)paragraph;
				point.X -= subpageParagraphResult.ContentOffset.X;
				point.Y -= subpageParagraphResult.ContentOffset.Y;
				cellInfo = this.GetCellInfoFromPoint(subpageParagraphResult.Columns, subpageParagraphResult.FloatingElements, point, tableFilter);
				if (cellInfo != null)
				{
					cellInfo.Adjust(new Point(subpageParagraphResult.ContentOffset.X, subpageParagraphResult.ContentOffset.Y));
				}
			}
			else if (paragraph is FigureParagraphResult)
			{
				FigureParagraphResult figureParagraphResult = (FigureParagraphResult)paragraph;
				TextDocumentView.TransformToSubpage(ref point, figureParagraphResult.ContentOffset);
				cellInfo = this.GetCellInfoFromPoint(figureParagraphResult.Columns, figureParagraphResult.FloatingElements, point, tableFilter);
				if (cellInfo != null)
				{
					cellInfo.Adjust(new Point(figureParagraphResult.ContentOffset.X, figureParagraphResult.ContentOffset.Y));
				}
			}
			else if (paragraph is FloaterParagraphResult)
			{
				FloaterParagraphResult floaterParagraphResult = (FloaterParagraphResult)paragraph;
				TextDocumentView.TransformToSubpage(ref point, floaterParagraphResult.ContentOffset);
				cellInfo = this.GetCellInfoFromPoint(floaterParagraphResult.Columns, floaterParagraphResult.FloatingElements, point, tableFilter);
				if (cellInfo != null)
				{
					cellInfo.Adjust(new Point(floaterParagraphResult.ContentOffset.X, floaterParagraphResult.ContentOffset.Y));
				}
			}
			if (tableFilter != null && cellInfo != null && cellInfo.Cell.Table != tableFilter)
			{
				cellInfo = null;
			}
			return cellInfo;
		}

		// Token: 0x06007278 RID: 29304 RVA: 0x0020C320 File Offset: 0x0020A520
		private CellInfo GetCellInfoFromPoint(ReadOnlyCollection<ColumnResult> columns, ReadOnlyCollection<ParagraphResult> floatingElements, Point point, Table tableFilter)
		{
			Invariant.Assert(floatingElements != null);
			int columnFromPoint = this.GetColumnFromPoint(columns, point, false);
			CellInfo result;
			if (columnFromPoint < 0 && floatingElements.Count == 0)
			{
				result = null;
			}
			else
			{
				ReadOnlyCollection<ParagraphResult> paragraphs = (columnFromPoint < columns.Count && columnFromPoint >= 0) ? columns[columnFromPoint].Paragraphs : TextDocumentView._emptyParagraphCollection;
				result = this.GetCellInfoFromPoint(paragraphs, floatingElements, point, tableFilter);
			}
			return result;
		}

		// Token: 0x06007279 RID: 29305 RVA: 0x0020C380 File Offset: 0x0020A580
		private Rect GetRectangleFromTextPosition(ReadOnlyCollection<ParagraphResult> paragraphs, ReadOnlyCollection<ParagraphResult> floatingElements, ITextPointer position)
		{
			Invariant.Assert(paragraphs != null, "Paragraph collection is null");
			Invariant.Assert(floatingElements != null, "Floating element collection is null");
			Rect result = Rect.Empty;
			bool flag = false;
			int paragraphFromPosition = TextDocumentView.GetParagraphFromPosition(paragraphs, floatingElements, position, out flag);
			ParagraphResult paragraphResult = null;
			if (flag)
			{
				Invariant.Assert(paragraphFromPosition < floatingElements.Count);
				paragraphResult = floatingElements[paragraphFromPosition];
			}
			else if (paragraphFromPosition < paragraphs.Count)
			{
				paragraphResult = paragraphs[paragraphFromPosition];
			}
			if (paragraphResult != null)
			{
				result = this.GetRectangleFromTextPosition(paragraphResult, position);
			}
			return result;
		}

		// Token: 0x0600727A RID: 29306 RVA: 0x0020C3F8 File Offset: 0x0020A5F8
		private Rect GetRectangleFromTextPosition(ParagraphResult paragraph, ITextPointer position)
		{
			Rect rect = Rect.Empty;
			if (paragraph is ContainerParagraphResult)
			{
				rect = this.GetRectangleFromEdge(paragraph, position);
				if (rect == Rect.Empty)
				{
					ReadOnlyCollection<ParagraphResult> paragraphs = ((ContainerParagraphResult)paragraph).Paragraphs;
					Invariant.Assert(paragraphs != null, "Paragraph collection is null.");
					if (paragraphs.Count > 0)
					{
						rect = this.GetRectangleFromTextPosition(paragraphs, TextDocumentView._emptyParagraphCollection, position);
					}
				}
			}
			else if (paragraph is TextParagraphResult)
			{
				rect = ((TextParagraphResult)paragraph).GetRectangleFromTextPosition(position);
			}
			else if (paragraph is TableParagraphResult)
			{
				rect = this.GetRectangleFromEdge(paragraph, position);
				if (rect == Rect.Empty)
				{
					ReadOnlyCollection<ParagraphResult> paragraphsFromPosition = ((TableParagraphResult)paragraph).GetParagraphsFromPosition(position);
					Invariant.Assert(paragraphsFromPosition != null, "Paragraph collection is null.");
					if (paragraphsFromPosition.Count > 0)
					{
						rect = this.GetRectangleFromTextPosition(paragraphsFromPosition, TextDocumentView._emptyParagraphCollection, position);
					}
					else if (position is TextPointer && ((TextPointer)position).IsAtRowEnd)
					{
						rect = ((TableParagraphResult)paragraph).GetRectangleFromRowEndPosition(position);
					}
				}
			}
			else if (paragraph is SubpageParagraphResult)
			{
				SubpageParagraphResult subpageParagraphResult = (SubpageParagraphResult)paragraph;
				rect = this.GetRectangleFromTextPosition(subpageParagraphResult.Columns, subpageParagraphResult.FloatingElements, position);
				if (rect != Rect.Empty)
				{
					rect.X += subpageParagraphResult.ContentOffset.X;
					rect.Y += subpageParagraphResult.ContentOffset.Y;
				}
			}
			else if (paragraph is FloaterParagraphResult)
			{
				FloaterParagraphResult floaterParagraphResult = (FloaterParagraphResult)paragraph;
				ReadOnlyCollection<ColumnResult> columns = floaterParagraphResult.Columns;
				ReadOnlyCollection<ParagraphResult> floatingElements = floaterParagraphResult.FloatingElements;
				Invariant.Assert(columns != null, "Columns collection is null.");
				Invariant.Assert(floatingElements != null, "Paragraph collection is null.");
				if (floatingElements.Count > 0 || columns.Count > 0)
				{
					rect = this.GetRectangleFromTextPosition(columns, floatingElements, position);
					TextDocumentView.TransformFromSubpage(ref rect, floaterParagraphResult.ContentOffset);
				}
			}
			else if (paragraph is FigureParagraphResult)
			{
				FigureParagraphResult figureParagraphResult = (FigureParagraphResult)paragraph;
				ReadOnlyCollection<ColumnResult> columns2 = figureParagraphResult.Columns;
				ReadOnlyCollection<ParagraphResult> floatingElements2 = figureParagraphResult.FloatingElements;
				Invariant.Assert(columns2 != null, "Columns collection is null.");
				Invariant.Assert(floatingElements2 != null, "Paragraph collection is null.");
				if (floatingElements2.Count > 0 || columns2.Count > 0)
				{
					rect = this.GetRectangleFromTextPosition(columns2, floatingElements2, position);
					TextDocumentView.TransformFromSubpage(ref rect, figureParagraphResult.ContentOffset);
				}
			}
			else if (paragraph is UIElementParagraphResult)
			{
				rect = this.GetRectangleFromEdge(paragraph, position);
				if (rect == Rect.Empty)
				{
					rect = this.GetRectangleFromContentEdge(paragraph, position);
				}
			}
			return rect;
		}

		// Token: 0x0600727B RID: 29307 RVA: 0x0020C684 File Offset: 0x0020A884
		private Rect GetRectangleFromTextPosition(ReadOnlyCollection<ColumnResult> columns, ReadOnlyCollection<ParagraphResult> floatingElements, ITextPointer position)
		{
			Rect result = Rect.Empty;
			Invariant.Assert(floatingElements != null);
			int columnFromPosition = this.GetColumnFromPosition(columns, position);
			if (columnFromPosition < columns.Count || floatingElements.Count > 0)
			{
				ReadOnlyCollection<ParagraphResult> paragraphs = (columnFromPosition < columns.Count && columnFromPosition >= 0) ? columns[columnFromPosition].Paragraphs : TextDocumentView._emptyParagraphCollection;
				result = this.GetRectangleFromTextPosition(paragraphs, floatingElements, position);
			}
			return result;
		}

		// Token: 0x0600727C RID: 29308 RVA: 0x0020C6E8 File Offset: 0x0020A8E8
		internal static Geometry GetTightBoundingGeometryFromTextPositionsHelper(ReadOnlyCollection<ParagraphResult> paragraphs, ITextPointer startPosition, ITextPointer endPosition, double paragraphTopSpace, Rect visibleRect)
		{
			Geometry result = null;
			int count = paragraphs.Count;
			int num = 0;
			while (num < count && endPosition.CompareTo(paragraphs[num].StartPosition) > 0)
			{
				if (startPosition.CompareTo(paragraphs[num].EndPosition) <= 0)
				{
					Rect layoutBox = TextDocumentView.GetLayoutBox(paragraphs[num]);
					layoutBox.X = visibleRect.X;
					if (layoutBox.IntersectsWith(visibleRect))
					{
						Geometry addedGeometry = null;
						if (paragraphs[num] is ContainerParagraphResult)
						{
							addedGeometry = ((ContainerParagraphResult)paragraphs[num]).GetTightBoundingGeometryFromTextPositions(startPosition, endPosition, visibleRect);
						}
						else if (paragraphs[num] is TextParagraphResult)
						{
							addedGeometry = ((TextParagraphResult)paragraphs[num]).GetTightBoundingGeometryFromTextPositions(startPosition, endPosition, paragraphTopSpace, visibleRect);
						}
						else if (paragraphs[num] is TableParagraphResult)
						{
							addedGeometry = ((TableParagraphResult)paragraphs[num]).GetTightBoundingGeometryFromTextPositions(startPosition, endPosition, visibleRect);
						}
						else if (paragraphs[num] is UIElementParagraphResult)
						{
							addedGeometry = ((UIElementParagraphResult)paragraphs[num]).GetTightBoundingGeometryFromTextPositions(startPosition, endPosition);
						}
						CaretElement.AddGeometry(ref result, addedGeometry);
					}
				}
				num++;
			}
			return result;
		}

		// Token: 0x0600727D RID: 29309 RVA: 0x0020C810 File Offset: 0x0020AA10
		internal static Geometry GetTightBoundingGeometryFromTextPositionsHelper(ReadOnlyCollection<ParagraphResult> paragraphs, ReadOnlyCollection<ParagraphResult> floatingElements, ITextPointer startPosition, ITextPointer endPosition, double paragraphTopSpace, Rect visibleRect)
		{
			Geometry result = null;
			bool flag = false;
			if (floatingElements != null && floatingElements.Count > 0)
			{
				result = TextDocumentView.GetTightBoundingGeometryFromTextPositionsInFloatingElements(floatingElements, startPosition, endPosition, paragraphTopSpace, visibleRect, out flag);
			}
			if (!flag)
			{
				result = TextDocumentView.GetTightBoundingGeometryFromTextPositionsHelper(paragraphs, startPosition, endPosition, paragraphTopSpace, visibleRect);
			}
			return result;
		}

		// Token: 0x0600727E RID: 29310 RVA: 0x0020C850 File Offset: 0x0020AA50
		private static Geometry GetTightBoundingGeometryFromTextPositionsInFloatingElements(ReadOnlyCollection<ParagraphResult> floatingElements, ITextPointer startPosition, ITextPointer endPosition, double paragraphTopSpace, Rect visibleRect, out bool success)
		{
			Geometry result = null;
			success = false;
			int count = floatingElements.Count;
			for (int i = 0; i < count; i++)
			{
				if (startPosition.CompareTo(floatingElements[i].StartPosition) > 0 && endPosition.CompareTo(floatingElements[i].EndPosition) < 0)
				{
					Rect layoutBox = TextDocumentView.GetLayoutBox(floatingElements[i]);
					Rect rect = visibleRect;
					layoutBox.X = rect.X;
					if (layoutBox.IntersectsWith(rect))
					{
						Geometry geometry = null;
						Invariant.Assert(floatingElements[i] is FloaterParagraphResult || floatingElements[i] is FigureParagraphResult);
						if (floatingElements[i] is FloaterParagraphResult)
						{
							FloaterParagraphResult floaterParagraphResult = (FloaterParagraphResult)floatingElements[i];
							TextDocumentView.TransformToSubpage(ref rect, floaterParagraphResult.ContentOffset);
							geometry = floaterParagraphResult.GetTightBoundingGeometryFromTextPositions(startPosition, endPosition, rect, out success);
							TextDocumentView.TransformFromSubpage(geometry, floaterParagraphResult.ContentOffset);
						}
						else if (floatingElements[i] is FigureParagraphResult)
						{
							FigureParagraphResult figureParagraphResult = (FigureParagraphResult)floatingElements[i];
							TextDocumentView.TransformToSubpage(ref rect, figureParagraphResult.ContentOffset);
							geometry = figureParagraphResult.GetTightBoundingGeometryFromTextPositions(startPosition, endPosition, rect, out success);
							TextDocumentView.TransformFromSubpage(geometry, figureParagraphResult.ContentOffset);
						}
						CaretElement.AddGeometry(ref result, geometry);
						if (success)
						{
							break;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600727F RID: 29311 RVA: 0x0020C9A0 File Offset: 0x0020ABA0
		private static Rect GetLayoutBox(ParagraphResult paragraph)
		{
			if (!(paragraph is SubpageParagraphResult) && !(paragraph is RowParagraphResult))
			{
				return paragraph.LayoutBox;
			}
			return Rect.Empty;
		}

		// Token: 0x06007280 RID: 29312 RVA: 0x0020C9C0 File Offset: 0x0020ABC0
		private bool IsAtCaretUnitBoundary(ReadOnlyCollection<ParagraphResult> paragraphs, ReadOnlyCollection<ParagraphResult> floatingElements, ITextPointer position)
		{
			Invariant.Assert(paragraphs != null, "Paragraph collection is null");
			Invariant.Assert(floatingElements != null, "Floating element collection is null");
			bool result = false;
			bool flag;
			int paragraphFromPosition = TextDocumentView.GetParagraphFromPosition(paragraphs, floatingElements, position, out flag);
			ParagraphResult paragraphResult = null;
			if (flag)
			{
				Invariant.Assert(paragraphFromPosition < floatingElements.Count);
				paragraphResult = floatingElements[paragraphFromPosition];
			}
			else if (paragraphFromPosition < paragraphs.Count)
			{
				paragraphResult = paragraphs[paragraphFromPosition];
			}
			if (paragraphResult != null)
			{
				result = this.IsAtCaretUnitBoundary(paragraphResult, position);
			}
			return result;
		}

		// Token: 0x06007281 RID: 29313 RVA: 0x0020CA34 File Offset: 0x0020AC34
		private bool IsAtCaretUnitBoundary(ParagraphResult paragraph, ITextPointer position)
		{
			bool result = false;
			if (paragraph is ContainerParagraphResult)
			{
				ReadOnlyCollection<ParagraphResult> paragraphs = ((ContainerParagraphResult)paragraph).Paragraphs;
				Invariant.Assert(paragraphs != null, "Paragraph collection is null.");
				if (paragraphs.Count > 0)
				{
					result = this.IsAtCaretUnitBoundary(paragraphs, TextDocumentView._emptyParagraphCollection, position);
				}
			}
			else if (paragraph is TextParagraphResult)
			{
				result = ((TextParagraphResult)paragraph).IsAtCaretUnitBoundary(position);
			}
			else if (paragraph is TableParagraphResult)
			{
				ReadOnlyCollection<ParagraphResult> paragraphsFromPosition = ((TableParagraphResult)paragraph).GetParagraphsFromPosition(position);
				Invariant.Assert(paragraphsFromPosition != null, "Paragraph collection is null.");
				if (paragraphsFromPosition.Count > 0)
				{
					result = this.IsAtCaretUnitBoundary(paragraphsFromPosition, TextDocumentView._emptyParagraphCollection, position);
				}
			}
			else if (paragraph is SubpageParagraphResult)
			{
				SubpageParagraphResult subpageParagraphResult = (SubpageParagraphResult)paragraph;
				ReadOnlyCollection<ColumnResult> columns = subpageParagraphResult.Columns;
				ReadOnlyCollection<ParagraphResult> floatingElements = subpageParagraphResult.FloatingElements;
				Invariant.Assert(columns != null, "Column collection is null.");
				Invariant.Assert(floatingElements != null, "Paragraph collection is null.");
				if (columns.Count > 0 || floatingElements.Count > 0)
				{
					result = this.IsAtCaretUnitBoundary(columns, floatingElements, position);
				}
			}
			else if (paragraph is FigureParagraphResult)
			{
				FigureParagraphResult figureParagraphResult = (FigureParagraphResult)paragraph;
				ReadOnlyCollection<ColumnResult> columns2 = figureParagraphResult.Columns;
				ReadOnlyCollection<ParagraphResult> floatingElements2 = figureParagraphResult.FloatingElements;
				Invariant.Assert(columns2 != null, "Column collection is null.");
				Invariant.Assert(floatingElements2 != null, "Paragraph collection is null.");
				if (columns2.Count > 0 || floatingElements2.Count > 0)
				{
					result = this.IsAtCaretUnitBoundary(columns2, floatingElements2, position);
				}
			}
			else if (paragraph is FloaterParagraphResult)
			{
				FloaterParagraphResult floaterParagraphResult = (FloaterParagraphResult)paragraph;
				ReadOnlyCollection<ColumnResult> columns3 = floaterParagraphResult.Columns;
				ReadOnlyCollection<ParagraphResult> floatingElements3 = floaterParagraphResult.FloatingElements;
				Invariant.Assert(columns3 != null, "Column collection is null.");
				Invariant.Assert(floatingElements3 != null, "Paragraph collection is null.");
				if (columns3.Count > 0 || floatingElements3.Count > 0)
				{
					result = this.IsAtCaretUnitBoundary(columns3, floatingElements3, position);
				}
			}
			return result;
		}

		// Token: 0x06007282 RID: 29314 RVA: 0x0020CC08 File Offset: 0x0020AE08
		private bool IsAtCaretUnitBoundary(ReadOnlyCollection<ColumnResult> columns, ReadOnlyCollection<ParagraphResult> floatingElements, ITextPointer position)
		{
			int columnFromPosition = this.GetColumnFromPosition(columns, position);
			if (columnFromPosition < columns.Count || floatingElements.Count > 0)
			{
				ReadOnlyCollection<ParagraphResult> paragraphs = (columnFromPosition < columns.Count && columnFromPosition >= 0) ? columns[columnFromPosition].Paragraphs : TextDocumentView._emptyParagraphCollection;
				return this.IsAtCaretUnitBoundary(paragraphs, floatingElements, position);
			}
			return false;
		}

		// Token: 0x06007283 RID: 29315 RVA: 0x0020CC5C File Offset: 0x0020AE5C
		private ITextPointer GetNextCaretUnitPosition(ReadOnlyCollection<ParagraphResult> paragraphs, ReadOnlyCollection<ParagraphResult> floatingElements, ITextPointer position, LogicalDirection direction)
		{
			Invariant.Assert(paragraphs != null, "Paragraph collection is null");
			Invariant.Assert(floatingElements != null, "Floating element collection is null");
			ITextPointer result = position;
			bool flag;
			int paragraphFromPosition = TextDocumentView.GetParagraphFromPosition(paragraphs, floatingElements, position, out flag);
			ParagraphResult paragraphResult = null;
			if (flag)
			{
				Invariant.Assert(paragraphFromPosition < floatingElements.Count);
				paragraphResult = floatingElements[paragraphFromPosition];
			}
			else if (paragraphFromPosition < paragraphs.Count)
			{
				paragraphResult = paragraphs[paragraphFromPosition];
			}
			if (paragraphResult != null)
			{
				result = this.GetNextCaretUnitPosition(paragraphResult, position, direction);
			}
			return result;
		}

		// Token: 0x06007284 RID: 29316 RVA: 0x0020CCD0 File Offset: 0x0020AED0
		private ITextPointer GetNextCaretUnitPosition(ParagraphResult paragraph, ITextPointer position, LogicalDirection direction)
		{
			ITextPointer result = position;
			if (paragraph is ContainerParagraphResult)
			{
				ReadOnlyCollection<ParagraphResult> paragraphs = ((ContainerParagraphResult)paragraph).Paragraphs;
				Invariant.Assert(paragraphs != null, "Paragraph collection is null.");
				if (paragraphs.Count > 0)
				{
					result = this.GetNextCaretUnitPosition(paragraphs, TextDocumentView._emptyParagraphCollection, position, direction);
				}
			}
			else if (paragraph is TextParagraphResult)
			{
				result = ((TextParagraphResult)paragraph).GetNextCaretUnitPosition(position, direction);
			}
			else if (paragraph is TableParagraphResult)
			{
				ReadOnlyCollection<ParagraphResult> paragraphsFromPosition = ((TableParagraphResult)paragraph).GetParagraphsFromPosition(position);
				Invariant.Assert(paragraphsFromPosition != null, "Paragraph collection is null.");
				if (paragraphsFromPosition.Count > 0)
				{
					result = this.GetNextCaretUnitPosition(paragraphsFromPosition, TextDocumentView._emptyParagraphCollection, position, direction);
				}
			}
			else if (paragraph is SubpageParagraphResult)
			{
				SubpageParagraphResult subpageParagraphResult = (SubpageParagraphResult)paragraph;
				ReadOnlyCollection<ColumnResult> columns = subpageParagraphResult.Columns;
				ReadOnlyCollection<ParagraphResult> floatingElements = subpageParagraphResult.FloatingElements;
				Invariant.Assert(columns != null, "Column collection is null.");
				Invariant.Assert(floatingElements != null, "Paragraph collection is null.");
				if (columns.Count > 0 || floatingElements.Count > 0)
				{
					result = this.GetNextCaretUnitPosition(columns, floatingElements, position, direction);
				}
			}
			else if (paragraph is FigureParagraphResult)
			{
				FigureParagraphResult figureParagraphResult = (FigureParagraphResult)paragraph;
				ReadOnlyCollection<ColumnResult> columns2 = figureParagraphResult.Columns;
				ReadOnlyCollection<ParagraphResult> floatingElements2 = figureParagraphResult.FloatingElements;
				Invariant.Assert(columns2 != null, "Column collection is null.");
				Invariant.Assert(floatingElements2 != null, "Paragraph collection is null.");
				if (columns2.Count > 0 || floatingElements2.Count > 0)
				{
					result = this.GetNextCaretUnitPosition(columns2, floatingElements2, position, direction);
				}
			}
			else if (paragraph is FloaterParagraphResult)
			{
				FloaterParagraphResult floaterParagraphResult = (FloaterParagraphResult)paragraph;
				ReadOnlyCollection<ColumnResult> columns3 = floaterParagraphResult.Columns;
				ReadOnlyCollection<ParagraphResult> floatingElements3 = floaterParagraphResult.FloatingElements;
				Invariant.Assert(columns3 != null, "Column collection is null.");
				Invariant.Assert(floatingElements3 != null, "Paragraph collection is null.");
				if (columns3.Count > 0 || floatingElements3.Count > 0)
				{
					result = this.GetNextCaretUnitPosition(columns3, floatingElements3, position, direction);
				}
			}
			return result;
		}

		// Token: 0x06007285 RID: 29317 RVA: 0x0020CEAC File Offset: 0x0020B0AC
		private ITextPointer GetNextCaretUnitPosition(ReadOnlyCollection<ColumnResult> columns, ReadOnlyCollection<ParagraphResult> floatingElements, ITextPointer position, LogicalDirection direction)
		{
			int columnFromPosition = this.GetColumnFromPosition(columns, position);
			if (columnFromPosition < columns.Count || floatingElements.Count > 0)
			{
				ReadOnlyCollection<ParagraphResult> paragraphs = (columnFromPosition < columns.Count && columnFromPosition >= 0) ? columns[columnFromPosition].Paragraphs : TextDocumentView._emptyParagraphCollection;
				return this.GetNextCaretUnitPosition(paragraphs, floatingElements, position, direction);
			}
			return position;
		}

		// Token: 0x06007286 RID: 29318 RVA: 0x0020CF04 File Offset: 0x0020B104
		private ITextPointer GetBackspaceCaretUnitPosition(ReadOnlyCollection<ParagraphResult> paragraphs, ReadOnlyCollection<ParagraphResult> floatingElements, ITextPointer position)
		{
			Invariant.Assert(paragraphs != null, "Paragraph collection is null");
			Invariant.Assert(floatingElements != null, "Floating element collection is null");
			ITextPointer result = position;
			bool flag;
			int paragraphFromPosition = TextDocumentView.GetParagraphFromPosition(paragraphs, floatingElements, position, out flag);
			ParagraphResult paragraphResult = null;
			if (flag)
			{
				Invariant.Assert(paragraphFromPosition < floatingElements.Count);
				paragraphResult = floatingElements[paragraphFromPosition];
			}
			else if (paragraphFromPosition < paragraphs.Count)
			{
				paragraphResult = paragraphs[paragraphFromPosition];
			}
			if (paragraphResult != null)
			{
				result = this.GetBackspaceCaretUnitPosition(paragraphResult, position);
			}
			return result;
		}

		// Token: 0x06007287 RID: 29319 RVA: 0x0020CF78 File Offset: 0x0020B178
		private ITextPointer GetBackspaceCaretUnitPosition(ParagraphResult paragraph, ITextPointer position)
		{
			ITextPointer result = position;
			if (paragraph is ContainerParagraphResult)
			{
				ReadOnlyCollection<ParagraphResult> paragraphs = ((ContainerParagraphResult)paragraph).Paragraphs;
				Invariant.Assert(paragraphs != null, "Paragraph collection is null.");
				if (paragraphs.Count > 0)
				{
					result = this.GetBackspaceCaretUnitPosition(paragraphs, TextDocumentView._emptyParagraphCollection, position);
				}
			}
			else if (paragraph is TextParagraphResult)
			{
				result = ((TextParagraphResult)paragraph).GetBackspaceCaretUnitPosition(position);
			}
			else if (paragraph is TableParagraphResult)
			{
				ReadOnlyCollection<ParagraphResult> paragraphsFromPosition = ((TableParagraphResult)paragraph).GetParagraphsFromPosition(position);
				Invariant.Assert(paragraphsFromPosition != null, "Paragraph collection is null.");
				if (paragraphsFromPosition.Count > 0)
				{
					result = this.GetBackspaceCaretUnitPosition(paragraphsFromPosition, TextDocumentView._emptyParagraphCollection, position);
				}
			}
			else if (paragraph is SubpageParagraphResult)
			{
				SubpageParagraphResult subpageParagraphResult = (SubpageParagraphResult)paragraph;
				ReadOnlyCollection<ColumnResult> columns = subpageParagraphResult.Columns;
				ReadOnlyCollection<ParagraphResult> floatingElements = subpageParagraphResult.FloatingElements;
				Invariant.Assert(columns != null, "Column collection is null.");
				Invariant.Assert(floatingElements != null, "Paragraph collection is null.");
				if (columns.Count > 0 || floatingElements.Count > 0)
				{
					result = this.GetBackspaceCaretUnitPosition(columns, floatingElements, position);
				}
			}
			else if (paragraph is FigureParagraphResult)
			{
				FigureParagraphResult figureParagraphResult = (FigureParagraphResult)paragraph;
				ReadOnlyCollection<ColumnResult> columns2 = figureParagraphResult.Columns;
				ReadOnlyCollection<ParagraphResult> floatingElements2 = figureParagraphResult.FloatingElements;
				Invariant.Assert(columns2 != null, "Column collection is null.");
				Invariant.Assert(floatingElements2 != null, "Paragraph collection is null.");
				if (columns2.Count > 0 || floatingElements2.Count > 0)
				{
					result = this.GetBackspaceCaretUnitPosition(columns2, floatingElements2, position);
				}
			}
			else if (paragraph is FloaterParagraphResult)
			{
				FloaterParagraphResult floaterParagraphResult = (FloaterParagraphResult)paragraph;
				ReadOnlyCollection<ColumnResult> columns3 = floaterParagraphResult.Columns;
				ReadOnlyCollection<ParagraphResult> floatingElements3 = floaterParagraphResult.FloatingElements;
				Invariant.Assert(columns3 != null, "Column collection is null.");
				Invariant.Assert(floatingElements3 != null, "Paragraph collection is null.");
				if (columns3.Count > 0 || floatingElements3.Count > 0)
				{
					result = this.GetBackspaceCaretUnitPosition(columns3, floatingElements3, position);
				}
			}
			return result;
		}

		// Token: 0x06007288 RID: 29320 RVA: 0x0020D14C File Offset: 0x0020B34C
		private ITextPointer GetBackspaceCaretUnitPosition(ReadOnlyCollection<ColumnResult> columns, ReadOnlyCollection<ParagraphResult> floatingElements, ITextPointer position)
		{
			int columnFromPosition = this.GetColumnFromPosition(columns, position);
			if (columnFromPosition < columns.Count || floatingElements.Count > 0)
			{
				ReadOnlyCollection<ParagraphResult> paragraphs = (columnFromPosition < columns.Count && columnFromPosition >= 0) ? columns[columnFromPosition].Paragraphs : TextDocumentView._emptyParagraphCollection;
				return this.GetBackspaceCaretUnitPosition(paragraphs, floatingElements, position);
			}
			return position;
		}

		// Token: 0x06007289 RID: 29321 RVA: 0x0020D1A0 File Offset: 0x0020B3A0
		private int GetColumnFromPoint(ReadOnlyCollection<ColumnResult> columns, Point point, bool snapToText)
		{
			int num = -1;
			bool flag = false;
			Invariant.Assert(columns != null, "Column collection is null");
			for (int i = 0; i < columns.Count; i++)
			{
				Rect layoutBox = columns[i].LayoutBox;
				if (!columns[i].HasTextContent)
				{
					if (i == columns.Count - 1)
					{
						num = ((num == -1) ? i : num);
						flag = snapToText;
					}
				}
				else
				{
					num = i;
					Invariant.Assert(num == i);
					if (point.X < layoutBox.Left)
					{
						flag = snapToText;
						break;
					}
					if (point.X > layoutBox.Right)
					{
						if (i >= columns.Count - 1)
						{
							flag = snapToText;
							break;
						}
						Rect layoutBox2 = columns[i + 1].LayoutBox;
						if (point.X < layoutBox2.Left)
						{
							double num2 = layoutBox2.Left - layoutBox.Right;
							if (point.X > layoutBox.Right + num2 / 2.0 && columns[i + 1].HasTextContent)
							{
								i++;
								num = i;
							}
							flag = snapToText;
							break;
						}
					}
					else
					{
						if (i >= columns.Count - 1)
						{
							flag = true;
							break;
						}
						Rect layoutBox3 = columns[i + 1].LayoutBox;
						if (point.X < layoutBox3.Left)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (flag)
			{
				Rect layoutBox = columns[num].LayoutBox;
				flag = (snapToText || (point.Y >= layoutBox.Top && point.Y <= layoutBox.Bottom));
			}
			Invariant.Assert(!flag || num < columns.Count, "Column not found.");
			if (!flag)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x0600728A RID: 29322 RVA: 0x0020D348 File Offset: 0x0020B548
		private int GetParagraphFromPoint(ReadOnlyCollection<ParagraphResult> paragraphs, Point point, bool snapToText)
		{
			int num = -1;
			bool flag = false;
			Invariant.Assert(paragraphs != null, "Paragraph collection is null");
			for (int i = 0; i < paragraphs.Count; i++)
			{
				Rect layoutBox = paragraphs[i].LayoutBox;
				if (!paragraphs[i].HasTextContent)
				{
					if (i == paragraphs.Count - 1)
					{
						num = ((num == -1) ? i : num);
						flag = snapToText;
					}
				}
				else
				{
					num = i;
					Invariant.Assert(num == i);
					if (point.Y < layoutBox.Top)
					{
						flag = snapToText;
						break;
					}
					if (point.Y > layoutBox.Bottom)
					{
						if (i >= paragraphs.Count - 1)
						{
							flag = snapToText;
							break;
						}
						Rect layoutBox2 = paragraphs[i + 1].LayoutBox;
						if (point.Y < layoutBox2.Top)
						{
							double num2 = layoutBox2.Top - layoutBox.Bottom;
							if (point.Y > layoutBox.Bottom + num2 / 2.0 && paragraphs[i + 1].HasTextContent)
							{
								i++;
								num = i;
							}
							flag = snapToText;
							break;
						}
					}
					else
					{
						if (i >= paragraphs.Count - 1)
						{
							flag = true;
							break;
						}
						Rect layoutBox3 = paragraphs[i + 1].LayoutBox;
						if (point.Y < layoutBox3.Top)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (flag)
			{
				Rect layoutBox = paragraphs[num].LayoutBox;
				flag = (snapToText || (point.X >= layoutBox.Left && point.X <= layoutBox.Right));
			}
			Invariant.Assert(!flag || num < paragraphs.Count, "Paragraph not found.");
			if (!flag)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x0600728B RID: 29323 RVA: 0x0020D4F0 File Offset: 0x0020B6F0
		private int GetParagraphFromPointInFloatingElements(ReadOnlyCollection<ParagraphResult> floatingElements, Point point, bool snapToText)
		{
			Invariant.Assert(floatingElements != null, "Paragraph collection is null");
			double num = double.MaxValue;
			int result = -1;
			for (int i = 0; i < floatingElements.Count; i++)
			{
				Rect layoutBox = floatingElements[i].LayoutBox;
				if (layoutBox.Contains(point))
				{
					return i;
				}
				Point point2 = new Point(layoutBox.X + layoutBox.Width / 2.0, layoutBox.Y + layoutBox.Height / 2.0);
				double num2 = Math.Abs(point.X - point2.X) + Math.Abs(point.Y - point2.Y);
				if (num2 < num)
				{
					result = i;
					num = num2;
				}
			}
			if (!snapToText)
			{
				return -1;
			}
			return result;
		}

		// Token: 0x0600728C RID: 29324 RVA: 0x0020D5BC File Offset: 0x0020B7BC
		private int GetColumnFromPosition(ReadOnlyCollection<ColumnResult> columns, ITextPointer position)
		{
			Invariant.Assert(columns != null, "Column collection is null");
			int num = 0;
			if (columns.Count > 0)
			{
				if (columns.Count == 1)
				{
					num = 0;
				}
				else
				{
					num = 0;
					while (num < columns.Count && !columns[num].Contains(position, true))
					{
						num++;
					}
					if (num >= columns.Count)
					{
						if (position.CompareTo(columns[0].StartPosition) == 0)
						{
							num = 0;
						}
						else if (position.CompareTo(columns[columns.Count - 1].EndPosition) == 0)
						{
							num = columns.Count - 1;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600728D RID: 29325 RVA: 0x0020D658 File Offset: 0x0020B858
		private static int GetParagraphFromPosition(ReadOnlyCollection<ParagraphResult> paragraphs, ReadOnlyCollection<ParagraphResult> floatingElements, ITextPointer position, out bool isFloatingPara)
		{
			Invariant.Assert(paragraphs != null, "Paragraph collection is null.");
			Invariant.Assert(floatingElements != null, "Floating element collection is null.");
			isFloatingPara = false;
			int paragraphFromPosition = TextDocumentView.GetParagraphFromPosition(floatingElements, position);
			if (paragraphFromPosition < floatingElements.Count)
			{
				isFloatingPara = true;
				return paragraphFromPosition;
			}
			return TextDocumentView.GetParagraphFromPosition(paragraphs, position);
		}

		// Token: 0x0600728E RID: 29326 RVA: 0x0020D6A4 File Offset: 0x0020B8A4
		private static int GetParagraphFromPosition(ReadOnlyCollection<ParagraphResult> paragraphs, ITextPointer position)
		{
			Invariant.Assert(paragraphs != null, "Paragraph collection is null.");
			int num = 0;
			int num2 = paragraphs.Count - 1;
			int num3 = 0;
			bool flag = false;
			if (paragraphs.Count > 0)
			{
				for (;;)
				{
					num = (num2 + num3) / 2;
					if (paragraphs[num].Contains(position, true))
					{
						break;
					}
					if (num2 == num3)
					{
						goto IL_6A;
					}
					if (position.CompareTo(paragraphs[num].StartPosition) < 0)
					{
						num2 = num - 1;
					}
					else
					{
						num3 = num + 1;
					}
					if (num2 < num3)
					{
						goto IL_6A;
					}
				}
				flag = true;
				IL_6A:
				if (!flag)
				{
					if (position.CompareTo(paragraphs[0].StartPosition) == 0)
					{
						num = 0;
					}
					else if (position.CompareTo(paragraphs[paragraphs.Count - 1].EndPosition) == 0)
					{
						num = paragraphs.Count - 1;
					}
					else
					{
						num = paragraphs.Count;
					}
				}
			}
			return num;
		}

		// Token: 0x0600728F RID: 29327 RVA: 0x0020D764 File Offset: 0x0020B964
		private TextSegment GetLineRangeFromPosition(ReadOnlyCollection<ParagraphResult> paragraphs, ReadOnlyCollection<ParagraphResult> floatingElements, ITextPointer position)
		{
			Invariant.Assert(paragraphs != null, "Paragraph collection is null");
			Invariant.Assert(floatingElements != null, "Floating element collection is null");
			TextSegment result = TextSegment.Null;
			bool flag;
			int paragraphFromPosition = TextDocumentView.GetParagraphFromPosition(paragraphs, floatingElements, position, out flag);
			ParagraphResult paragraphResult = null;
			if (flag)
			{
				Invariant.Assert(paragraphFromPosition < floatingElements.Count);
				paragraphResult = floatingElements[paragraphFromPosition];
			}
			else if (paragraphFromPosition < paragraphs.Count)
			{
				paragraphResult = paragraphs[paragraphFromPosition];
			}
			if (paragraphResult != null)
			{
				result = this.GetLineRangeFromPosition(paragraphResult, position);
			}
			return result;
		}

		// Token: 0x06007290 RID: 29328 RVA: 0x0020D7DC File Offset: 0x0020B9DC
		private TextSegment GetLineRangeFromPosition(ParagraphResult paragraph, ITextPointer position)
		{
			TextSegment result = TextSegment.Null;
			if (paragraph is ContainerParagraphResult)
			{
				ReadOnlyCollection<ParagraphResult> paragraphs = ((ContainerParagraphResult)paragraph).Paragraphs;
				Invariant.Assert(paragraphs != null, "Paragraph collection is null.");
				if (paragraphs.Count > 0)
				{
					result = this.GetLineRangeFromPosition(paragraphs, TextDocumentView._emptyParagraphCollection, position);
				}
			}
			else if (paragraph is TextParagraphResult)
			{
				ReadOnlyCollection<LineResult> lines = ((TextParagraphResult)paragraph).Lines;
				Invariant.Assert(lines != null, "Lines collection is null");
				if (!((TextParagraphResult)paragraph).HasTextContent)
				{
					result = new TextSegment(((TextParagraphResult)paragraph).EndPosition, ((TextParagraphResult)paragraph).EndPosition, true);
				}
				else
				{
					int lineFromPosition = TextParagraphView.GetLineFromPosition(lines, position);
					Invariant.Assert(lineFromPosition >= 0 && lineFromPosition < lines.Count, "Line not found.");
					result = new TextSegment(lines[lineFromPosition].StartPosition, lines[lineFromPosition].GetContentEndPosition(), true);
				}
			}
			else if (paragraph is TableParagraphResult)
			{
				ReadOnlyCollection<ParagraphResult> paragraphsFromPosition = ((TableParagraphResult)paragraph).GetParagraphsFromPosition(position);
				Invariant.Assert(paragraphsFromPosition != null, "Paragraph collection is null.");
				if (paragraphsFromPosition.Count > 0)
				{
					result = this.GetLineRangeFromPosition(paragraphsFromPosition, TextDocumentView._emptyParagraphCollection, position);
				}
			}
			else if (paragraph is SubpageParagraphResult)
			{
				SubpageParagraphResult subpageParagraphResult = (SubpageParagraphResult)paragraph;
				ReadOnlyCollection<ColumnResult> columns = subpageParagraphResult.Columns;
				ReadOnlyCollection<ParagraphResult> floatingElements = subpageParagraphResult.FloatingElements;
				Invariant.Assert(columns != null, "Column collection is null.");
				Invariant.Assert(floatingElements != null, "Paragraph collection is null.");
				if (columns.Count > 0 || floatingElements.Count > 0)
				{
					result = this.GetLineRangeFromPosition(columns, floatingElements, position);
				}
			}
			else if (paragraph is FigureParagraphResult)
			{
				FigureParagraphResult figureParagraphResult = (FigureParagraphResult)paragraph;
				ReadOnlyCollection<ColumnResult> columns2 = figureParagraphResult.Columns;
				ReadOnlyCollection<ParagraphResult> floatingElements2 = figureParagraphResult.FloatingElements;
				Invariant.Assert(columns2 != null, "Column collection is null.");
				Invariant.Assert(floatingElements2 != null, "Paragraph collection is null.");
				if (columns2.Count > 0 || floatingElements2.Count > 0)
				{
					result = this.GetLineRangeFromPosition(columns2, floatingElements2, position);
				}
			}
			else if (paragraph is FloaterParagraphResult)
			{
				FloaterParagraphResult floaterParagraphResult = (FloaterParagraphResult)paragraph;
				ReadOnlyCollection<ColumnResult> columns3 = floaterParagraphResult.Columns;
				ReadOnlyCollection<ParagraphResult> floatingElements3 = floaterParagraphResult.FloatingElements;
				Invariant.Assert(columns3 != null, "Column collection is null.");
				Invariant.Assert(floatingElements3 != null, "Paragraph collection is null.");
				if (columns3.Count > 0 || floatingElements3.Count > 0)
				{
					result = this.GetLineRangeFromPosition(columns3, floatingElements3, position);
				}
			}
			else if (paragraph is UIElementParagraphResult)
			{
				BlockUIContainer blockUIContainer = paragraph.Element as BlockUIContainer;
				if (blockUIContainer != null)
				{
					result = new TextSegment(blockUIContainer.ContentStart.CreatePointer(LogicalDirection.Forward), blockUIContainer.ContentEnd.CreatePointer(LogicalDirection.Backward));
				}
			}
			return result;
		}

		// Token: 0x06007291 RID: 29329 RVA: 0x0020DA80 File Offset: 0x0020BC80
		private TextSegment GetLineRangeFromPosition(ReadOnlyCollection<ColumnResult> columns, ReadOnlyCollection<ParagraphResult> floatingElements, ITextPointer position)
		{
			int columnFromPosition = this.GetColumnFromPosition(columns, position);
			if (columnFromPosition < columns.Count || floatingElements.Count > 0)
			{
				ReadOnlyCollection<ParagraphResult> paragraphs = (columnFromPosition < columns.Count && columnFromPosition >= 0) ? columns[columnFromPosition].Paragraphs : TextDocumentView._emptyParagraphCollection;
				return this.GetLineRangeFromPosition(paragraphs, floatingElements, position);
			}
			return TextSegment.Null;
		}

		// Token: 0x06007292 RID: 29330 RVA: 0x0020DAD8 File Offset: 0x0020BCD8
		private ITextPointer GetPositionAtNextLine(ReadOnlyCollection<ParagraphResult> paragraphs, ITextPointer position, double suggestedX, ref int count, out bool positionFound)
		{
			Invariant.Assert(paragraphs != null, "Paragraph collection is empty.");
			ITextPointer textPointer = position;
			positionFound = false;
			int num = TextDocumentView.GetParagraphFromPosition(paragraphs, position);
			if (num < paragraphs.Count)
			{
				positionFound = true;
				if (paragraphs[num] is ContainerParagraphResult)
				{
					Rect layoutBox = paragraphs[num].LayoutBox;
					ReadOnlyCollection<ParagraphResult> paragraphs2 = ((ContainerParagraphResult)paragraphs[num]).Paragraphs;
					Invariant.Assert(paragraphs2 != null, "Paragraph collection is null.");
					if (paragraphs2.Count > 0)
					{
						textPointer = this.GetPositionAtNextLine(paragraphs2, position, suggestedX, ref count, out positionFound);
					}
				}
				else if (paragraphs[num] is TextParagraphResult)
				{
					ReadOnlyCollection<LineResult> lines = ((TextParagraphResult)paragraphs[num]).Lines;
					Invariant.Assert(lines != null, "Lines collection is null");
					if (!((TextParagraphResult)paragraphs[num]).HasTextContent)
					{
						textPointer = position;
					}
					else
					{
						int num2 = TextParagraphView.GetLineFromPosition(lines, position);
						Invariant.Assert(num2 >= 0 && num2 < lines.Count, "Line not found.");
						Rect layoutBox2 = paragraphs[num].LayoutBox;
						int num3 = num2;
						if (num2 + count < 0)
						{
							num2 = 0;
							count += num3;
						}
						else if (num2 + count > lines.Count - 1)
						{
							num2 = lines.Count - 1;
							count -= lines.Count - 1 - num3;
						}
						else
						{
							num2 += count;
							count = 0;
						}
						if (count == 0)
						{
							if (!DoubleUtil.IsNaN(suggestedX))
							{
								textPointer = lines[num2].GetTextPositionFromDistance(suggestedX);
							}
							else
							{
								textPointer = lines[num2].StartPosition.CreatePointer(LogicalDirection.Forward);
							}
						}
						else if (num2 == num3)
						{
							textPointer = position;
						}
						else if (count < 0)
						{
							if (!DoubleUtil.IsNaN(suggestedX))
							{
								textPointer = lines[0].GetTextPositionFromDistance(suggestedX);
							}
							else
							{
								textPointer = lines[0].StartPosition.CreatePointer(LogicalDirection.Forward);
							}
						}
						else if (!DoubleUtil.IsNaN(suggestedX))
						{
							textPointer = lines[lines.Count - 1].GetTextPositionFromDistance(suggestedX);
						}
						else
						{
							textPointer = lines[lines.Count - 1].StartPosition.CreatePointer(LogicalDirection.Forward);
						}
					}
				}
				else if (paragraphs[num] is TableParagraphResult)
				{
					TableParagraphResult tableParagraphResult = (TableParagraphResult)paragraphs[num];
					CellParaClient cellParaClientFromPosition = tableParagraphResult.GetCellParaClientFromPosition(position);
					CellParaClient cellParaClient = cellParaClientFromPosition;
					Rect layoutBox3 = paragraphs[num].LayoutBox;
					while ((count != 0 && cellParaClient != null) & positionFound)
					{
						SubpageParagraphResult subpageParagraphResult = (SubpageParagraphResult)cellParaClient.CreateParagraphResult();
						ReadOnlyCollection<ParagraphResult> paragraphs3 = subpageParagraphResult.Columns[0].Paragraphs;
						Invariant.Assert(paragraphs3 != null, "Paragraph collection is null.");
						if (paragraphs3.Count > 0)
						{
							if (cellParaClient != cellParaClientFromPosition)
							{
								int paragraphIndex = (count > 0) ? 0 : (paragraphs3.Count - 1);
								textPointer = this.GetPositionAtNextLineFromSiblingPara(paragraphs3, paragraphIndex, suggestedX - TextDpi.FromTextDpi(cellParaClient.Rect.u), ref count);
								if (textPointer == null)
								{
									textPointer = position;
								}
							}
							else
							{
								textPointer = this.GetPositionAtNextLine(paragraphs3, position, suggestedX - subpageParagraphResult.ContentOffset.X, ref count, out positionFound);
							}
						}
						if (count < 0 & positionFound)
						{
							cellParaClient = tableParagraphResult.GetCellAbove(suggestedX, cellParaClient.Cell.RowGroupIndex, cellParaClient.Cell.RowIndex);
						}
						else if (count > 0 & positionFound)
						{
							cellParaClient = tableParagraphResult.GetCellBelow(suggestedX, cellParaClient.Cell.RowGroupIndex, cellParaClient.Cell.RowIndex + cellParaClient.Cell.RowSpan - 1);
						}
					}
				}
				else if (paragraphs[num] is SubpageParagraphResult)
				{
					SubpageParagraphResult subpageParagraphResult2 = (SubpageParagraphResult)paragraphs[num];
					double num4;
					textPointer = this.GetPositionAtNextLine(((SubpageParagraphResult)paragraphs[num]).Columns, subpageParagraphResult2.FloatingElements, position, suggestedX - subpageParagraphResult2.ContentOffset.X, ref count, out num4, out positionFound);
				}
				if (count != 0 & positionFound)
				{
					if (count > 0)
					{
						num++;
					}
					else
					{
						num--;
					}
					if (num >= 0 && num < paragraphs.Count)
					{
						textPointer = this.GetPositionAtNextLineFromSiblingPara(paragraphs, num, suggestedX, ref count);
						if (textPointer == null)
						{
							textPointer = position;
						}
					}
				}
			}
			Invariant.Assert(textPointer != null);
			return textPointer;
		}

		// Token: 0x06007293 RID: 29331 RVA: 0x0020DF08 File Offset: 0x0020C108
		private ITextPointer GetPositionAtNextLineInFloatingElements(ReadOnlyCollection<ParagraphResult> floatingElements, ITextPointer position, double suggestedX, ref int count, out bool positionFound)
		{
			ITextPointer textPointer = position;
			positionFound = false;
			int paragraphFromPosition = TextDocumentView.GetParagraphFromPosition(TextDocumentView._emptyParagraphCollection, floatingElements, position, out positionFound);
			if (positionFound)
			{
				Invariant.Assert(paragraphFromPosition < floatingElements.Count);
				ParagraphResult paragraphResult = floatingElements[paragraphFromPosition];
				Invariant.Assert(paragraphResult is FigureParagraphResult || paragraphResult is FloaterParagraphResult);
				if (paragraphResult is FigureParagraphResult)
				{
					FigureParagraphResult figureParagraphResult = (FigureParagraphResult)paragraphResult;
					ReadOnlyCollection<ColumnResult> columns = figureParagraphResult.Columns;
					ReadOnlyCollection<ParagraphResult> floatingElements2 = figureParagraphResult.FloatingElements;
					Invariant.Assert(columns != null, "Column collection is null.");
					Invariant.Assert(floatingElements2 != null, "Paragraph collection is null.");
					if (columns.Count > 0 || floatingElements2.Count > 0)
					{
						double num;
						bool flag;
						textPointer = this.GetPositionAtNextLine(columns, floatingElements2, position, suggestedX - figureParagraphResult.ContentOffset.X, ref count, out num, out flag);
					}
				}
				else
				{
					FloaterParagraphResult floaterParagraphResult = (FloaterParagraphResult)paragraphResult;
					ReadOnlyCollection<ColumnResult> columns2 = floaterParagraphResult.Columns;
					ReadOnlyCollection<ParagraphResult> floatingElements3 = floaterParagraphResult.FloatingElements;
					Invariant.Assert(columns2 != null, "Column collection is null.");
					Invariant.Assert(floatingElements3 != null, "Paragraph collection is null.");
					if (columns2.Count > 0 || floatingElements3.Count > 0)
					{
						bool flag;
						double num2;
						textPointer = this.GetPositionAtNextLine(columns2, floatingElements3, position, suggestedX - floaterParagraphResult.ContentOffset.X, ref count, out num2, out flag);
					}
				}
			}
			Invariant.Assert(textPointer != null);
			return textPointer;
		}

		// Token: 0x06007294 RID: 29332 RVA: 0x0020E05C File Offset: 0x0020C25C
		private ITextPointer GetPositionAtNextLine(ReadOnlyCollection<ColumnResult> columns, ReadOnlyCollection<ParagraphResult> floatingElements, ITextPointer position, double suggestedX, ref int count, out double newSuggestedX, out bool positionFound)
		{
			ITextPointer textPointer = null;
			newSuggestedX = suggestedX;
			positionFound = false;
			if (floatingElements.Count > 0)
			{
				textPointer = this.GetPositionAtNextLineInFloatingElements(floatingElements, position, suggestedX, ref count, out positionFound);
			}
			if (!positionFound)
			{
				int num = this.GetColumnFromPosition(columns, position);
				if (num < columns.Count)
				{
					positionFound = true;
					textPointer = this.GetPositionAtNextLine(columns[num].Paragraphs, position, suggestedX, ref count, out positionFound);
					int index = num;
					if (count != 0 & positionFound)
					{
						if (count > 0)
						{
							num++;
						}
						else
						{
							num--;
						}
						if (num >= 0 && num < columns.Count)
						{
							suggestedX = suggestedX - columns[index].LayoutBox.Left + columns[num].LayoutBox.Left;
							ITextPointer positionAtNextLineFromSiblingColumn = this.GetPositionAtNextLineFromSiblingColumn(columns, num, suggestedX, ref newSuggestedX, ref count);
							if (positionAtNextLineFromSiblingColumn != null)
							{
								textPointer = positionAtNextLineFromSiblingColumn;
							}
						}
					}
				}
			}
			Invariant.Assert(textPointer != null);
			return textPointer;
		}

		// Token: 0x06007295 RID: 29333 RVA: 0x0020E140 File Offset: 0x0020C340
		private ITextPointer GetPositionAtNextLineFromSiblingPara(ReadOnlyCollection<ParagraphResult> paragraphs, int paragraphIndex, double suggestedX, ref int count)
		{
			Invariant.Assert(count != 0);
			Invariant.Assert(paragraphIndex >= 0 && paragraphIndex < paragraphs.Count, "Paragraph collection is empty.");
			ITextPointer result = null;
			while (paragraphIndex >= 0 && paragraphIndex < paragraphs.Count)
			{
				if (paragraphs[paragraphIndex] is ContainerParagraphResult)
				{
					Rect layoutBox = paragraphs[paragraphIndex].LayoutBox;
					ReadOnlyCollection<ParagraphResult> paragraphs2 = ((ContainerParagraphResult)paragraphs[paragraphIndex]).Paragraphs;
					Invariant.Assert(paragraphs2 != null, "Paragraph collection is null.");
					if (paragraphs2.Count > 0)
					{
						int paragraphIndex2 = (count > 0) ? 0 : (paragraphs2.Count - 1);
						result = this.GetPositionAtNextLineFromSiblingPara(paragraphs2, paragraphIndex2, suggestedX, ref count);
					}
				}
				else if (paragraphs[paragraphIndex] is TextParagraphResult)
				{
					result = this.GetPositionAtNextLineFromSiblingTextPara((TextParagraphResult)paragraphs[paragraphIndex], suggestedX, ref count);
					if (count == 0)
					{
						break;
					}
				}
				else if (paragraphs[paragraphIndex] is TableParagraphResult)
				{
					TableParagraphResult tableParagraphResult = (TableParagraphResult)paragraphs[paragraphIndex];
					Rect layoutBox2 = paragraphs[paragraphIndex].LayoutBox;
					CellParaClient cellParaClient = null;
					if (count < 0)
					{
						cellParaClient = tableParagraphResult.GetCellAbove(suggestedX, int.MaxValue, int.MaxValue);
					}
					else if (count > 0)
					{
						cellParaClient = tableParagraphResult.GetCellBelow(suggestedX, int.MinValue, int.MinValue);
					}
					while (count != 0)
					{
						if (cellParaClient == null)
						{
							break;
						}
						SubpageParagraphResult subpageParagraphResult = (SubpageParagraphResult)cellParaClient.CreateParagraphResult();
						ReadOnlyCollection<ParagraphResult> paragraphs3 = subpageParagraphResult.Columns[0].Paragraphs;
						Invariant.Assert(paragraphs3 != null, "Paragraph collection is null.");
						if (paragraphs3.Count > 0)
						{
							int paragraphIndex3 = (count > 0) ? 0 : (paragraphs3.Count - 1);
							result = this.GetPositionAtNextLineFromSiblingPara(paragraphs3, paragraphIndex3, suggestedX - subpageParagraphResult.ContentOffset.X, ref count);
						}
						if (count < 0)
						{
							cellParaClient = tableParagraphResult.GetCellAbove(suggestedX, cellParaClient.Cell.RowGroupIndex, cellParaClient.Cell.RowIndex);
						}
						else if (count > 0)
						{
							cellParaClient = tableParagraphResult.GetCellBelow(suggestedX, cellParaClient.Cell.RowGroupIndex, cellParaClient.Cell.RowIndex + cellParaClient.Cell.RowSpan - 1);
						}
					}
				}
				else if (paragraphs[paragraphIndex] is SubpageParagraphResult)
				{
					Rect layoutBox3 = paragraphs[paragraphIndex].LayoutBox;
					SubpageParagraphResult subpageParagraphResult2 = (SubpageParagraphResult)paragraphs[paragraphIndex];
					ReadOnlyCollection<ParagraphResult> paragraphs4 = subpageParagraphResult2.Columns[0].Paragraphs;
					Invariant.Assert(paragraphs4 != null, "Paragraph collection is null.");
					if (paragraphs4.Count > 0)
					{
						int paragraphIndex4 = (count > 0) ? 0 : (paragraphs4.Count - 1);
						result = this.GetPositionAtNextLineFromSiblingPara(paragraphs4, paragraphIndex4, suggestedX - subpageParagraphResult2.ContentOffset.X, ref count);
					}
				}
				else if (paragraphs[paragraphIndex] is UIElementParagraphResult)
				{
					if (count < 0)
					{
						count++;
					}
					else
					{
						count--;
					}
					if (count == 0)
					{
						Rect layoutBox4 = paragraphs[paragraphIndex].LayoutBox;
						BlockUIContainer blockUIContainer = paragraphs[paragraphIndex].Element as BlockUIContainer;
						if (blockUIContainer != null)
						{
							if (DoubleUtil.LessThanOrClose(suggestedX, layoutBox4.Width / 2.0))
							{
								result = blockUIContainer.ContentStart.CreatePointer(LogicalDirection.Forward);
							}
							else
							{
								result = blockUIContainer.ContentEnd.CreatePointer(LogicalDirection.Backward);
							}
						}
					}
				}
				if (count < 0)
				{
					paragraphIndex--;
				}
				else
				{
					if (count <= 0)
					{
						break;
					}
					paragraphIndex++;
				}
			}
			return result;
		}

		// Token: 0x06007296 RID: 29334 RVA: 0x0020E4B0 File Offset: 0x0020C6B0
		private ITextPointer GetPositionAtNextLineFromSiblingTextPara(TextParagraphResult paragraph, double suggestedX, ref int count)
		{
			ReadOnlyCollection<LineResult> lines = paragraph.Lines;
			Invariant.Assert(lines != null, "Lines collection is null");
			ITextPointer result;
			if (!paragraph.HasTextContent)
			{
				result = null;
			}
			else
			{
				Rect layoutBox = paragraph.LayoutBox;
				int num = (count > 0) ? 0 : (lines.Count - 1);
				if (count < 0)
				{
					count++;
				}
				else
				{
					count--;
				}
				if (num + count < 0)
				{
					count += num;
				}
				else if (num + count > lines.Count - 1)
				{
					count -= lines.Count - 1 - num;
				}
				else
				{
					num += count;
					count = 0;
				}
				if (count == 0)
				{
					if (!DoubleUtil.IsNaN(suggestedX))
					{
						result = lines[num].GetTextPositionFromDistance(suggestedX);
					}
					else
					{
						result = lines[num].StartPosition.CreatePointer(LogicalDirection.Forward);
					}
				}
				else if (count < 0)
				{
					if (!DoubleUtil.IsNaN(suggestedX))
					{
						result = lines[0].GetTextPositionFromDistance(suggestedX);
					}
					else
					{
						result = lines[0].StartPosition.CreatePointer(LogicalDirection.Forward);
					}
				}
				else if (!DoubleUtil.IsNaN(suggestedX))
				{
					result = lines[lines.Count - 1].GetTextPositionFromDistance(suggestedX);
				}
				else
				{
					result = lines[lines.Count - 1].StartPosition.CreatePointer(LogicalDirection.Forward);
				}
			}
			return result;
		}

		// Token: 0x06007297 RID: 29335 RVA: 0x0020E5E4 File Offset: 0x0020C7E4
		private ITextPointer GetPositionAtNextLineFromSiblingColumn(ReadOnlyCollection<ColumnResult> columns, int columnIndex, double columnSuggestedX, ref double newSuggestedX, ref int count)
		{
			ITextPointer result = null;
			while (columnIndex >= 0 && columnIndex < columns.Count)
			{
				double num = columnSuggestedX + columns[columnIndex].LayoutBox.Left;
				ReadOnlyCollection<ParagraphResult> paragraphs = columns[columnIndex].Paragraphs;
				Invariant.Assert(paragraphs != null, "Paragraph collection is null.");
				if (paragraphs.Count > 0)
				{
					int paragraphIndex = (count > 0) ? 0 : (paragraphs.Count - 1);
					result = this.GetPositionAtNextLineFromSiblingPara(paragraphs, paragraphIndex, columnSuggestedX, ref count);
				}
				newSuggestedX = columnSuggestedX;
				if (count < 0)
				{
					columnIndex--;
				}
				else
				{
					if (count <= 0)
					{
						break;
					}
					columnIndex++;
				}
			}
			return result;
		}

		// Token: 0x06007298 RID: 29336 RVA: 0x0020E680 File Offset: 0x0020C880
		private bool ContainsCore(ITextPointer position)
		{
			ReadOnlyCollection<TextSegment> textSegmentsCore = this.TextSegmentsCore;
			Invariant.Assert(textSegmentsCore != null, "TextSegment collection is empty.");
			return TextDocumentView.Contains(position, textSegmentsCore);
		}

		// Token: 0x06007299 RID: 29337 RVA: 0x0020E6AC File Offset: 0x0020C8AC
		private bool GetGlyphRunsFromParagraphs(List<GlyphRun> glyphRuns, ITextPointer start, ITextPointer end, ReadOnlyCollection<ParagraphResult> paragraphs)
		{
			Invariant.Assert(paragraphs != null, "Paragraph collection is null.");
			bool flag = true;
			for (int i = 0; i < paragraphs.Count; i++)
			{
				ParagraphResult paragraphResult = paragraphs[i];
				if (paragraphResult is TextParagraphResult)
				{
					TextParagraphResult textParagraphResult = (TextParagraphResult)paragraphResult;
					if (start.CompareTo(textParagraphResult.EndPosition) < 0 && end.CompareTo(textParagraphResult.StartPosition) > 0)
					{
						ITextPointer start2 = (start.CompareTo(textParagraphResult.StartPosition) < 0) ? textParagraphResult.StartPosition : start;
						ITextPointer end2 = (end.CompareTo(textParagraphResult.EndPosition) < 0) ? end : textParagraphResult.EndPosition;
						textParagraphResult.GetGlyphRuns(glyphRuns, start2, end2);
					}
					if (end.CompareTo(textParagraphResult.EndPosition) < 0)
					{
						flag = false;
						break;
					}
				}
				else if (paragraphResult is ContainerParagraphResult)
				{
					ReadOnlyCollection<ParagraphResult> paragraphs2 = ((ContainerParagraphResult)paragraphResult).Paragraphs;
					Invariant.Assert(paragraphs2 != null, "Paragraph collection is null.");
					if (paragraphs2.Count > 0)
					{
						flag = this.GetGlyphRunsFromParagraphs(glyphRuns, start, end, paragraphs2);
						if (!flag)
						{
							break;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x0600729A RID: 29338 RVA: 0x0020E7A8 File Offset: 0x0020C9A8
		private void GetGlyphRunsFromFloatingElements(List<GlyphRun> glyphRuns, ITextPointer start, ITextPointer end, ReadOnlyCollection<ParagraphResult> floatingElements, out bool success)
		{
			Invariant.Assert(floatingElements != null, "Paragraph collection is null.");
			success = false;
			int i = 0;
			while (i < floatingElements.Count)
			{
				ParagraphResult paragraphResult = floatingElements[i];
				Invariant.Assert(paragraphResult is FigureParagraphResult || paragraphResult is FloaterParagraphResult);
				if (paragraphResult.Contains(start, true))
				{
					success = true;
					ITextPointer end2 = (end.CompareTo(paragraphResult.EndPosition) < 0) ? end : paragraphResult.EndPosition;
					if (paragraphResult is FigureParagraphResult)
					{
						FigureParagraphResult figureParagraphResult = (FigureParagraphResult)paragraphResult;
						ReadOnlyCollection<ColumnResult> columns = figureParagraphResult.Columns;
						ReadOnlyCollection<ParagraphResult> floatingElements2 = figureParagraphResult.FloatingElements;
						Invariant.Assert(columns != null, "Column collection is null.");
						Invariant.Assert(floatingElements2 != null, "Paragraph collection is null.");
						if (columns.Count > 0 || floatingElements2.Count > 0)
						{
							this.GetGlyphRuns(glyphRuns, start, end2, columns, floatingElements2);
							return;
						}
						break;
					}
					else
					{
						if (!(paragraphResult is FloaterParagraphResult))
						{
							break;
						}
						FloaterParagraphResult floaterParagraphResult = (FloaterParagraphResult)paragraphResult;
						ReadOnlyCollection<ColumnResult> columns2 = floaterParagraphResult.Columns;
						ReadOnlyCollection<ParagraphResult> floatingElements3 = floaterParagraphResult.FloatingElements;
						Invariant.Assert(columns2 != null, "Column collection is null.");
						Invariant.Assert(floatingElements3 != null, "Paragraph collection is null.");
						if (columns2.Count > 0 || floatingElements3.Count > 0)
						{
							this.GetGlyphRuns(glyphRuns, start, end2, columns2, floatingElements3);
							return;
						}
						break;
					}
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x0600729B RID: 29339 RVA: 0x0020E8F0 File Offset: 0x0020CAF0
		private void GetGlyphRuns(List<GlyphRun> glyphRuns, ITextPointer start, ITextPointer end, ReadOnlyCollection<ColumnResult> columns, ReadOnlyCollection<ParagraphResult> floatingElements)
		{
			bool flag = false;
			if (floatingElements.Count > 0)
			{
				this.GetGlyphRunsFromFloatingElements(glyphRuns, start, end, floatingElements, out flag);
			}
			if (!flag)
			{
				int i;
				for (i = 0; i < columns.Count; i++)
				{
					ColumnResult columnResult = columns[i];
					if (start.CompareTo(columnResult.StartPosition) >= 0 && start.CompareTo(columnResult.EndPosition) <= 0)
					{
						break;
					}
				}
				int j;
				for (j = i; j < columns.Count; j++)
				{
					ColumnResult columnResult2 = columns[i];
					if (end.CompareTo(columnResult2.StartPosition) >= 0 && end.CompareTo(columnResult2.EndPosition) <= 0)
					{
						break;
					}
				}
				Invariant.Assert(i < columns.Count && j < columns.Count, "Start or End position does not belong to TextView's content range");
				while (i <= j)
				{
					ReadOnlyCollection<ParagraphResult> paragraphs = columns[i].Paragraphs;
					if (paragraphs != null && paragraphs.Count > 0)
					{
						this.GetGlyphRunsFromParagraphs(glyphRuns, start, end, paragraphs);
					}
					i++;
				}
			}
		}

		// Token: 0x0600729C RID: 29340 RVA: 0x0020E9E8 File Offset: 0x0020CBE8
		private ReadOnlyCollection<TextSegment> GetTextSegments()
		{
			ReadOnlyCollection<TextSegment> readOnlyCollection;
			if (!this._owner.FinitePage)
			{
				ITextPointer endPosition = this._textContainer.End;
				BackgroundFormatInfo backgroundFormatInfo = this._owner.StructuralCache.BackgroundFormatInfo;
				if (backgroundFormatInfo != null && backgroundFormatInfo.CPInterrupted != -1)
				{
					endPosition = this._textContainer.Start.CreatePointer(backgroundFormatInfo.CPInterrupted, LogicalDirection.Backward);
				}
				readOnlyCollection = new ReadOnlyCollection<TextSegment>(new List<TextSegment>(1)
				{
					new TextSegment(this._textContainer.Start, endPosition, true)
				});
			}
			else
			{
				TextContentRange textContentRange = new TextContentRange();
				ReadOnlyCollection<ColumnResult> columns = this.Columns;
				Invariant.Assert(columns != null, "Column collection is empty.");
				for (int i = 0; i < columns.Count; i++)
				{
					textContentRange.Merge(columns[i].TextContentRange);
				}
				readOnlyCollection = textContentRange.GetTextSegments();
			}
			Invariant.Assert(readOnlyCollection != null);
			return readOnlyCollection;
		}

		// Token: 0x0600729D RID: 29341 RVA: 0x0020EAC8 File Offset: 0x0020CCC8
		private void TransformToContent(ref Point point)
		{
			FlowDirection flowDirection = (FlowDirection)this._owner.StructuralCache.PropertyOwner.GetValue(FlowDocument.FlowDirectionProperty);
			if (flowDirection == FlowDirection.RightToLeft)
			{
				MatrixTransform matrixTransform = new MatrixTransform(-1.0, 0.0, 0.0, 1.0, this._owner.Size.Width, 0.0);
				Point point2;
				matrixTransform.TryTransform(point, out point2);
				point = point2;
			}
		}

		// Token: 0x0600729E RID: 29342 RVA: 0x0020EB58 File Offset: 0x0020CD58
		private void TransformToContent(ref Rect rect)
		{
			FlowDirection flowDirection = (FlowDirection)this._owner.StructuralCache.PropertyOwner.GetValue(FlowDocument.FlowDirectionProperty);
			if (flowDirection == FlowDirection.RightToLeft)
			{
				MatrixTransform matrixTransform = new MatrixTransform(-1.0, 0.0, 0.0, 1.0, this._owner.Size.Width, 0.0);
				rect = matrixTransform.TransformBounds(rect);
			}
		}

		// Token: 0x0600729F RID: 29343 RVA: 0x0020EBE4 File Offset: 0x0020CDE4
		private void TransformFromContent(ref Rect rect, out Transform transform)
		{
			transform = Transform.Identity;
			if (rect == Rect.Empty)
			{
				return;
			}
			FlowDirection flowDirection = (FlowDirection)this._owner.StructuralCache.PropertyOwner.GetValue(FlowDocument.FlowDirectionProperty);
			if (flowDirection == FlowDirection.RightToLeft)
			{
				transform = new MatrixTransform(-1.0, 0.0, 0.0, 1.0, this._owner.Size.Width, 0.0);
			}
		}

		// Token: 0x060072A0 RID: 29344 RVA: 0x0020EC78 File Offset: 0x0020CE78
		private void TransformFromContent(ref Point point)
		{
			FlowDirection flowDirection = (FlowDirection)this._owner.StructuralCache.PropertyOwner.GetValue(FlowDocument.FlowDirectionProperty);
			if (flowDirection == FlowDirection.RightToLeft)
			{
				MatrixTransform matrixTransform = new MatrixTransform(-1.0, 0.0, 0.0, 1.0, this._owner.Size.Width, 0.0);
				Point point2;
				matrixTransform.TryTransform(point, out point2);
				point = point2;
			}
		}

		// Token: 0x060072A1 RID: 29345 RVA: 0x0020ED08 File Offset: 0x0020CF08
		private void TransformFromContent(Geometry geometry)
		{
			FlowDirection flowDirection = (FlowDirection)this._owner.StructuralCache.PropertyOwner.GetValue(FlowDocument.FlowDirectionProperty);
			if (flowDirection == FlowDirection.RightToLeft)
			{
				MatrixTransform transformToAdd = new MatrixTransform(-1.0, 0.0, 0.0, 1.0, this._owner.Size.Width, 0.0);
				CaretElement.AddTransformToGeometry(geometry, transformToAdd);
			}
		}

		// Token: 0x060072A2 RID: 29346 RVA: 0x0020ED86 File Offset: 0x0020CF86
		private static void TransformToSubpage(ref Point point, Vector subpageOffset)
		{
			point -= subpageOffset;
		}

		// Token: 0x060072A3 RID: 29347 RVA: 0x0020ED9A File Offset: 0x0020CF9A
		private static void TransformToSubpage(ref Rect rect, Vector subpageOffset)
		{
			if (rect == Rect.Empty)
			{
				return;
			}
			rect.Offset(-subpageOffset);
		}

		// Token: 0x060072A4 RID: 29348 RVA: 0x0020EDBB File Offset: 0x0020CFBB
		private static void TransformFromSubpage(ref Rect rect, Vector subpageOffset)
		{
			if (rect == Rect.Empty)
			{
				return;
			}
			rect.Offset(subpageOffset);
		}

		// Token: 0x060072A5 RID: 29349 RVA: 0x0020EDD8 File Offset: 0x0020CFD8
		private static void TransformFromSubpage(Geometry geometry, Vector subpageOffset)
		{
			if (geometry != null && (!DoubleUtil.IsZero(subpageOffset.X) || !DoubleUtil.IsZero(subpageOffset.Y)))
			{
				TranslateTransform transformToAdd = new TranslateTransform(subpageOffset.X, subpageOffset.Y);
				CaretElement.AddTransformToGeometry(geometry, transformToAdd);
			}
		}

		// Token: 0x060072A6 RID: 29350 RVA: 0x0020EE20 File Offset: 0x0020D020
		private Rect GetRectangleFromEdge(ParagraphResult paragraphResult, ITextPointer textPointer)
		{
			TextElement textElement = paragraphResult.Element as TextElement;
			if (textElement != null)
			{
				if (textPointer.LogicalDirection == LogicalDirection.Forward && textPointer.CompareTo(textElement.ElementStart) == 0)
				{
					return new Rect(paragraphResult.LayoutBox.Left, paragraphResult.LayoutBox.Top, 0.0, paragraphResult.LayoutBox.Height);
				}
				if (textPointer.LogicalDirection == LogicalDirection.Backward && textPointer.CompareTo(textElement.ElementEnd) == 0)
				{
					return new Rect(paragraphResult.LayoutBox.Right, paragraphResult.LayoutBox.Top, 0.0, paragraphResult.LayoutBox.Height);
				}
			}
			return Rect.Empty;
		}

		// Token: 0x060072A7 RID: 29351 RVA: 0x0020EEE4 File Offset: 0x0020D0E4
		private Rect GetRectangleFromContentEdge(ParagraphResult paragraphResult, ITextPointer textPointer)
		{
			TextElement textElement = paragraphResult.Element as TextElement;
			if (textElement != null)
			{
				Invariant.Assert(textElement is BlockUIContainer, "Expecting BlockUIContainer");
				if (textPointer.CompareTo(textElement.ContentStart) == 0)
				{
					return new Rect(paragraphResult.LayoutBox.Left, paragraphResult.LayoutBox.Top, 0.0, paragraphResult.LayoutBox.Height);
				}
				if (textPointer.CompareTo(textElement.ContentEnd) == 0)
				{
					return new Rect(paragraphResult.LayoutBox.Right, paragraphResult.LayoutBox.Top, 0.0, paragraphResult.LayoutBox.Height);
				}
			}
			return Rect.Empty;
		}

		// Token: 0x0400375F RID: 14175
		private readonly FlowDocumentPage _owner;

		// Token: 0x04003760 RID: 14176
		private readonly ITextContainer _textContainer;

		// Token: 0x04003761 RID: 14177
		private ReadOnlyCollection<ColumnResult> _columns;

		// Token: 0x04003762 RID: 14178
		private ReadOnlyCollection<ParagraphResult> _floatingElements;

		// Token: 0x04003763 RID: 14179
		private static ReadOnlyCollection<ParagraphResult> _emptyParagraphCollection = new ReadOnlyCollection<ParagraphResult>(new List<ParagraphResult>(0));

		// Token: 0x04003764 RID: 14180
		private ReadOnlyCollection<TextSegment> _segments;

		// Token: 0x04003765 RID: 14181
		private bool _hasTextContent;
	}
}
