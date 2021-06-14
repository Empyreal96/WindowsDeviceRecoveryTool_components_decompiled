using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x0200036C RID: 876
	internal sealed class FixedTextView : TextViewBase
	{
		// Token: 0x06002EDE RID: 11998 RVA: 0x000D3637 File Offset: 0x000D1837
		internal FixedTextView(FixedDocumentPage docPage)
		{
			this._docPage = docPage;
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x000D3648 File Offset: 0x000D1848
		internal override ITextPointer GetTextPositionFromPoint(Point point, bool snapToText)
		{
			if (point.Y == 1.7976931348623157E+308 && point.X == 1.7976931348623157E+308)
			{
				ITextPointer textPointer = this.End;
				FixedPosition fixedPosition;
				if (this._GetFixedPosition(this.End, out fixedPosition))
				{
					textPointer = this._CreateTextPointer(fixedPosition, LogicalDirection.Backward);
					if (textPointer == null)
					{
						textPointer = this.End;
					}
				}
				return textPointer;
			}
			ITextPointer textPointer2 = null;
			UIElement uielement;
			bool flag = this._HitTest(point, out uielement);
			if (flag)
			{
				Glyphs glyphs = uielement as Glyphs;
				if (glyphs != null)
				{
					textPointer2 = this._CreateTextPointerFromGlyphs(glyphs, point);
				}
				else if (uielement is Image)
				{
					Image e = (Image)uielement;
					FixedPosition fixedPosition2 = new FixedPosition(this.FixedPage.CreateFixedNode(this.PageIndex, e), 0);
					textPointer2 = this._CreateTextPointer(fixedPosition2, LogicalDirection.Forward);
				}
				else if (uielement is Path)
				{
					Path path = (Path)uielement;
					if (path.Fill is ImageBrush)
					{
						FixedPosition fixedPosition3 = new FixedPosition(this.FixedPage.CreateFixedNode(this.PageIndex, path), 0);
						textPointer2 = this._CreateTextPointer(fixedPosition3, LogicalDirection.Forward);
					}
				}
			}
			if (snapToText && textPointer2 == null)
			{
				textPointer2 = this._SnapToText(point);
			}
			return textPointer2;
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x000D3760 File Offset: 0x000D1960
		internal override Rect GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform)
		{
			FixedTextPointer fixedTextPointer = this.Container.VerifyPosition(position);
			Rect result = new Rect(0.0, 0.0, 0.0, 10.0);
			transform = Transform.Identity;
			FixedPosition fixedPosition;
			if (fixedTextPointer.FlowPosition.IsBoundary)
			{
				if (!this._GetFirstFixedPosition(fixedTextPointer, out fixedPosition))
				{
					return result;
				}
			}
			else if (!this._GetFixedPosition(fixedTextPointer, out fixedPosition))
			{
				if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.None)
				{
					return result;
				}
				ITextPointer position2 = position.CreatePointer(1);
				FixedTextPointer ftp = this.Container.VerifyPosition(position2);
				if (!this._GetFixedPosition(ftp, out fixedPosition))
				{
					return result;
				}
			}
			if (fixedPosition.Page != this.PageIndex)
			{
				return result;
			}
			DependencyObject element = this.FixedPage.GetElement(fixedPosition.Node);
			if (element is Glyphs)
			{
				Glyphs glyphs = (Glyphs)element;
				result = FixedTextView._GetGlyphRunDesignRect(glyphs, fixedPosition.Offset, fixedPosition.Offset);
				GeneralTransform transform2 = glyphs.TransformToAncestor(this.FixedPage);
				result = this._GetTransformedCaretRect(transform2, result.TopLeft, result.Height);
			}
			else if (element is Image)
			{
				Image image = (Image)element;
				GeneralTransform transform3 = image.TransformToAncestor(this.FixedPage);
				Point origin = new Point(0.0, 0.0);
				if (fixedPosition.Offset > 0)
				{
					origin.X += image.ActualWidth;
				}
				result = this._GetTransformedCaretRect(transform3, origin, image.ActualHeight);
			}
			else if (element is Path)
			{
				Path path = (Path)element;
				GeneralTransform transform4 = path.TransformToAncestor(this.FixedPage);
				Rect bounds = path.Data.Bounds;
				Point topLeft = bounds.TopLeft;
				if (fixedPosition.Offset > 0)
				{
					topLeft.X += bounds.Width;
				}
				result = this._GetTransformedCaretRect(transform4, topLeft, bounds.Height);
			}
			return result;
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x000D394C File Offset: 0x000D1B4C
		internal override Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			PathGeometry pathGeometry = new PathGeometry();
			Dictionary<FixedPage, ArrayList> dictionary = new Dictionary<FixedPage, ArrayList>();
			FixedTextPointer start = this.Container.VerifyPosition(startPosition);
			FixedTextPointer end = this.Container.VerifyPosition(endPosition);
			this.Container.GetMultiHighlights(start, end, dictionary, FixedHighlightType.TextSelection, null, null);
			ArrayList arrayList;
			dictionary.TryGetValue(this.FixedPage, out arrayList);
			if (arrayList != null)
			{
				foreach (object obj in arrayList)
				{
					FixedHighlight fixedHighlight = (FixedHighlight)obj;
					if (fixedHighlight.HighlightType != FixedHighlightType.None)
					{
						Rect rect = fixedHighlight.ComputeDesignRect();
						if (!(rect == Rect.Empty))
						{
							GeneralTransform generalTransform = fixedHighlight.Element.TransformToAncestor(this.FixedPage);
							Transform transform = generalTransform.AffineTransform;
							if (transform == null)
							{
								transform = Transform.Identity;
							}
							Glyphs glyphs = fixedHighlight.Glyphs;
							if (fixedHighlight.Element.Clip != null)
							{
								Rect bounds = fixedHighlight.Element.Clip.Bounds;
								rect.Intersect(bounds);
							}
							Geometry geometry = new RectangleGeometry(rect);
							geometry.Transform = transform;
							rect = generalTransform.TransformBounds(rect);
							pathGeometry.AddGeometry(geometry);
						}
					}
				}
			}
			return pathGeometry;
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x000D3A9C File Offset: 0x000D1C9C
		internal override ITextPointer GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved)
		{
			newSuggestedX = suggestedX;
			linesMoved = 0;
			LogicalDirection logicalDirection = position.LogicalDirection;
			LogicalDirection logicalDirection2 = LogicalDirection.Forward;
			FixedTextPointer fixedTextPointer = this.Container.VerifyPosition(position);
			FixedTextPointer fixedTextPointer2 = new FixedTextPointer(true, logicalDirection, (FlowPosition)fixedTextPointer.FlowPosition.Clone());
			this._SkipFormattingTags(fixedTextPointer2);
			FixedPosition fixedPosition;
			bool flag;
			if (count == 0 || ((flag = this._GetFixedPosition(fixedTextPointer2, out fixedPosition)) && fixedPosition.Page != this.PageIndex))
			{
				return position;
			}
			if (count < 0)
			{
				count = -count;
				logicalDirection2 = LogicalDirection.Backward;
			}
			if (!flag)
			{
				if (this.Contains(position))
				{
					fixedTextPointer2 = new FixedTextPointer(true, logicalDirection2, (FlowPosition)fixedTextPointer.FlowPosition.Clone());
					((ITextPointer)fixedTextPointer2).MoveToInsertionPosition(logicalDirection2);
					((ITextPointer)fixedTextPointer2).MoveToNextInsertionPosition(logicalDirection2);
					if (this.Contains(fixedTextPointer2))
					{
						linesMoved = ((logicalDirection2 == LogicalDirection.Forward) ? 1 : -1);
						return fixedTextPointer2;
					}
				}
				return position;
			}
			if (DoubleUtil.IsNaN(suggestedX))
			{
				suggestedX = 0.0;
			}
			while (count > linesMoved && this._GetNextLineGlyphs(ref fixedPosition, ref logicalDirection, suggestedX, logicalDirection2))
			{
				linesMoved++;
			}
			if (linesMoved == 0)
			{
				return position.CreatePointer();
			}
			if (logicalDirection2 == LogicalDirection.Backward)
			{
				linesMoved = -linesMoved;
			}
			ITextPointer textPointer = this._CreateTextPointer(fixedPosition, logicalDirection);
			if (textPointer.CompareTo(position) == 0)
			{
				linesMoved = 0;
			}
			return textPointer;
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x000D3BD0 File Offset: 0x000D1DD0
		internal override bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			FixedTextPointer ftp = this.Container.VerifyPosition(position);
			FixedPosition fixedPosition;
			if (this._GetFixedPosition(ftp, out fixedPosition))
			{
				DependencyObject element = this.FixedPage.GetElement(fixedPosition.Node);
				if (element is Glyphs)
				{
					Glyphs glyphs = (Glyphs)element;
					int num = (glyphs.UnicodeString == null) ? 0 : glyphs.UnicodeString.Length;
					if (fixedPosition.Offset == num)
					{
						return true;
					}
					GlyphRun measurementGlyphRun = glyphs.MeasurementGlyphRun;
					return measurementGlyphRun.CaretStops == null || measurementGlyphRun.CaretStops[fixedPosition.Offset];
				}
				else if (element is Image || element is Path)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x000D3C7C File Offset: 0x000D1E7C
		internal override ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			FixedTextPointer ftp = this.Container.VerifyPosition(position);
			FixedPosition fixedPosition;
			if (this._GetFixedPosition(ftp, out fixedPosition))
			{
				DependencyObject element = this.FixedPage.GetElement(fixedPosition.Node);
				if (element is Glyphs)
				{
					Glyphs glyphs = (Glyphs)element;
					GlyphRun glyphRun = glyphs.ToGlyphRun();
					int num = (glyphRun.Characters == null) ? 0 : glyphRun.Characters.Count;
					CharacterHit characterHit = (fixedPosition.Offset == num) ? new CharacterHit(fixedPosition.Offset - 1, 1) : new CharacterHit(fixedPosition.Offset, 0);
					CharacterHit obj = (direction == LogicalDirection.Forward) ? glyphRun.GetNextCaretCharacterHit(characterHit) : glyphRun.GetPreviousCaretCharacterHit(characterHit);
					if (!characterHit.Equals(obj))
					{
						LogicalDirection edge = LogicalDirection.Forward;
						if (obj.TrailingLength > 0)
						{
							edge = LogicalDirection.Backward;
						}
						int offset = obj.FirstCharacterIndex + obj.TrailingLength;
						return this._CreateTextPointer(new FixedPosition(fixedPosition.Node, offset), edge);
					}
				}
			}
			ITextPointer textPointer = position.CreatePointer();
			textPointer.MoveToNextInsertionPosition(direction);
			return textPointer;
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x0003E384 File Offset: 0x0003C584
		internal override ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002EE6 RID: 12006 RVA: 0x000D3D88 File Offset: 0x000D1F88
		internal override TextSegment GetLineRange(ITextPointer position)
		{
			FixedTextPointer ftp = this.Container.VerifyPosition(position);
			FixedPosition fixedPosition;
			if (!this._GetFixedPosition(ftp, out fixedPosition))
			{
				return new TextSegment(position, position, true);
			}
			int num = 0;
			FixedNode[] array = this.Container.FixedTextBuilder.GetNextLine(fixedPosition.Node, true, ref num);
			if (array == null)
			{
				array = new FixedNode[]
				{
					fixedPosition.Node
				};
			}
			FixedNode fixedNode = array[array.Length - 1];
			DependencyObject element = this.FixedPage.GetElement(fixedNode);
			int offset = 1;
			if (element is Glyphs)
			{
				offset = ((Glyphs)element).UnicodeString.Length;
			}
			ITextPointer textPointer = this._CreateTextPointer(new FixedPosition(array[0], 0), LogicalDirection.Forward);
			ITextPointer textPointer2 = this._CreateTextPointer(new FixedPosition(fixedNode, offset), LogicalDirection.Backward);
			if (textPointer.CompareTo(textPointer2) > 0)
			{
				ITextPointer textPointer3 = textPointer;
				textPointer = textPointer2;
				textPointer2 = textPointer3;
			}
			return new TextSegment(textPointer, textPointer2, true);
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x000D3E70 File Offset: 0x000D2070
		internal override bool Contains(ITextPointer position)
		{
			FixedTextPointer fixedTextPointer = this.Container.VerifyPosition(position);
			return (fixedTextPointer.CompareTo(this.Start) > 0 && fixedTextPointer.CompareTo(this.End) < 0) || (fixedTextPointer.CompareTo(this.Start) == 0 && (fixedTextPointer.LogicalDirection == LogicalDirection.Forward || this.IsContainerStart)) || (fixedTextPointer.CompareTo(this.End) == 0 && (fixedTextPointer.LogicalDirection == LogicalDirection.Backward || this.IsContainerEnd));
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool Validate()
		{
			return true;
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x06002EE9 RID: 12009 RVA: 0x000D3EEC File Offset: 0x000D20EC
		internal override UIElement RenderScope
		{
			get
			{
				Visual visual = this._docPage.Visual;
				while (visual != null && !(visual is UIElement))
				{
					visual = (VisualTreeHelper.GetParent(visual) as Visual);
				}
				return visual as UIElement;
			}
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x06002EEA RID: 12010 RVA: 0x000D3F24 File Offset: 0x000D2124
		internal override ITextContainer TextContainer
		{
			get
			{
				return this.Container;
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x06002EEB RID: 12011 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool IsValid
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x06002EEC RID: 12012 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool RendersOwnSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x06002EED RID: 12013 RVA: 0x000D3F2C File Offset: 0x000D212C
		internal override ReadOnlyCollection<TextSegment> TextSegments
		{
			get
			{
				if (this._textSegments == null)
				{
					this._textSegments = new ReadOnlyCollection<TextSegment>(new List<TextSegment>(1)
					{
						new TextSegment(this.Start, this.End, true)
					});
				}
				return this._textSegments;
			}
		}

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x06002EEE RID: 12014 RVA: 0x000D3F74 File Offset: 0x000D2174
		internal FixedTextPointer Start
		{
			get
			{
				if (this._start == null)
				{
					FlowPosition pageStartFlowPosition = this.Container.FixedTextBuilder.GetPageStartFlowPosition(this.PageIndex);
					this._start = new FixedTextPointer(false, LogicalDirection.Forward, pageStartFlowPosition);
				}
				return this._start;
			}
		}

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x06002EEF RID: 12015 RVA: 0x000D3FB4 File Offset: 0x000D21B4
		internal FixedTextPointer End
		{
			get
			{
				if (this._end == null)
				{
					FlowPosition pageEndFlowPosition = this.Container.FixedTextBuilder.GetPageEndFlowPosition(this.PageIndex);
					this._end = new FixedTextPointer(false, LogicalDirection.Backward, pageEndFlowPosition);
				}
				return this._end;
			}
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x000D3FF4 File Offset: 0x000D21F4
		private bool _HitTest(Point pt, out UIElement e)
		{
			e = null;
			HitTestResult hitTestResult = VisualTreeHelper.HitTest(this.FixedPage, pt);
			for (DependencyObject dependencyObject = (hitTestResult != null) ? hitTestResult.VisualHit : null; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(dependencyObject))
			{
				DependencyObjectType dependencyObjectType = dependencyObject.DependencyObjectType;
				if (dependencyObjectType == FixedTextView.UIElementType || dependencyObjectType.IsSubclassOf(FixedTextView.UIElementType))
				{
					e = (UIElement)dependencyObject;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x000D4054 File Offset: 0x000D2254
		private void _GlyphRunHitTest(Glyphs g, double xoffset, out int charIndex, out LogicalDirection edge)
		{
			charIndex = 0;
			edge = LogicalDirection.Forward;
			GlyphRun glyphRun = g.ToGlyphRun();
			double distance;
			if ((glyphRun.BidiLevel & 1) != 0)
			{
				distance = glyphRun.BaselineOrigin.X - xoffset;
			}
			else
			{
				distance = xoffset - glyphRun.BaselineOrigin.X;
			}
			bool flag;
			CharacterHit caretCharacterHitFromDistance = glyphRun.GetCaretCharacterHitFromDistance(distance, out flag);
			charIndex = caretCharacterHitFromDistance.FirstCharacterIndex + caretCharacterHitFromDistance.TrailingLength;
			edge = ((caretCharacterHitFromDistance.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward);
		}

		// Token: 0x06002EF2 RID: 12018 RVA: 0x000D40CC File Offset: 0x000D22CC
		private ITextPointer _SnapToText(Point point)
		{
			FixedNode[] line = this.Container.FixedTextBuilder.GetLine(this.PageIndex, point);
			ITextPointer textPointer;
			if (line != null && line.Length != 0)
			{
				double num = double.MaxValue;
				double xoffset = 0.0;
				Glyphs glyphs = null;
				FixedNode fixedNode = line[0];
				foreach (FixedNode fixedNode2 in line)
				{
					Glyphs glyphsElement = this.FixedPage.GetGlyphsElement(fixedNode2);
					GeneralTransform generalTransform = this.FixedPage.TransformToDescendant(glyphsElement);
					Point inPoint = point;
					if (generalTransform != null)
					{
						generalTransform.TryTransform(inPoint, out inPoint);
					}
					GlyphRun glyphRun = glyphsElement.ToGlyphRun();
					Rect rect = glyphRun.ComputeAlignmentBox();
					rect.Offset(glyphsElement.OriginX, glyphsElement.OriginY);
					double num2 = Math.Max(0.0, (inPoint.X > rect.X) ? (inPoint.X - rect.Right) : (rect.X - inPoint.X));
					double num3 = Math.Max(0.0, (inPoint.Y > rect.Y) ? (inPoint.Y - rect.Bottom) : (rect.Y - inPoint.Y));
					double num4 = num2 + num3;
					if (glyphs == null || num4 < num)
					{
						num = num4;
						glyphs = glyphsElement;
						fixedNode = fixedNode2;
						xoffset = inPoint.X;
					}
				}
				int offset;
				LogicalDirection edge;
				this._GlyphRunHitTest(glyphs, xoffset, out offset, out edge);
				FixedPosition fixedPosition = new FixedPosition(fixedNode, offset);
				textPointer = this._CreateTextPointer(fixedPosition, edge);
			}
			else if (point.Y < this.FixedPage.Height / 2.0)
			{
				textPointer = ((ITextPointer)this.Start).CreatePointer(LogicalDirection.Forward);
				textPointer.MoveToInsertionPosition(LogicalDirection.Forward);
			}
			else
			{
				textPointer = ((ITextPointer)this.End).CreatePointer(LogicalDirection.Backward);
				textPointer.MoveToInsertionPosition(LogicalDirection.Backward);
			}
			return textPointer;
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x000D42B4 File Offset: 0x000D24B4
		private bool _GetNextLineGlyphs(ref FixedPosition fixedp, ref LogicalDirection edge, double suggestedX, LogicalDirection scanDir)
		{
			int num = 1;
			int page = fixedp.Page;
			bool result = false;
			FixedNode[] nextLine = this.Container.FixedTextBuilder.GetNextLine(fixedp.Node, scanDir == LogicalDirection.Forward, ref num);
			if (nextLine != null && nextLine.Length != 0)
			{
				FixedPage fixedPage = this.Container.FixedDocument.SyncGetPage(page, false);
				if (double.IsInfinity(suggestedX))
				{
					suggestedX = 0.0;
				}
				Point point = new Point(suggestedX, 0.0);
				Point point2 = new Point(suggestedX, 1000.0);
				FixedNode fixedNode = nextLine[0];
				Glyphs g = null;
				double num2 = double.MaxValue;
				double xoffset = 0.0;
				for (int i = nextLine.Length - 1; i >= 0; i--)
				{
					FixedNode fixedNode2 = nextLine[i];
					Glyphs glyphsElement = fixedPage.GetGlyphsElement(fixedNode2);
					if (glyphsElement != null)
					{
						GeneralTransform generalTransform = fixedPage.TransformToDescendant(glyphsElement);
						Point inPoint = point;
						Point inPoint2 = point2;
						if (generalTransform != null)
						{
							generalTransform.TryTransform(inPoint, out inPoint);
							generalTransform.TryTransform(inPoint2, out inPoint2);
						}
						double num3 = (inPoint2.X - inPoint.X) / (inPoint2.Y - inPoint.Y);
						GlyphRun glyphRun = glyphsElement.ToGlyphRun();
						Rect rect = glyphRun.ComputeAlignmentBox();
						rect.Offset(glyphsElement.OriginX, glyphsElement.OriginY);
						double num4;
						double num5;
						if (num3 > 1000.0 || num3 < -1000.0)
						{
							num4 = 0.0;
							num5 = ((inPoint.Y > rect.Y) ? (inPoint.Y - rect.Bottom) : (rect.Y - inPoint.Y));
						}
						else
						{
							double num6 = (rect.Top + rect.Bottom) / 2.0;
							num4 = inPoint.X + num3 * (num6 - inPoint.Y);
							num5 = ((num4 > rect.X) ? (num4 - rect.Right) : (rect.X - num4));
						}
						if (num5 < num2)
						{
							num2 = num5;
							xoffset = num4;
							fixedNode = fixedNode2;
							g = glyphsElement;
							if (num5 <= 0.0)
							{
								break;
							}
						}
					}
				}
				int offset;
				this._GlyphRunHitTest(g, xoffset, out offset, out edge);
				fixedp = new FixedPosition(fixedNode, offset);
				result = true;
			}
			return result;
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x000D4500 File Offset: 0x000D2700
		private static double _GetDistanceToCharacter(GlyphRun run, int charOffset)
		{
			int num = charOffset;
			int trailingLength = 0;
			int num2 = (run.Characters == null) ? 0 : run.Characters.Count;
			if (num == num2)
			{
				num--;
				trailingLength = 1;
			}
			return run.GetDistanceFromCaretCharacterHit(new CharacterHit(num, trailingLength));
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x000D4540 File Offset: 0x000D2740
		internal static Rect _GetGlyphRunDesignRect(Glyphs g, int charStart, int charEnd)
		{
			GlyphRun glyphRun = g.ToGlyphRun();
			if (glyphRun == null)
			{
				return Rect.Empty;
			}
			Rect result = glyphRun.ComputeAlignmentBox();
			result.Offset(glyphRun.BaselineOrigin.X, glyphRun.BaselineOrigin.Y);
			int num = 0;
			if (glyphRun.Characters != null)
			{
				num = glyphRun.Characters.Count;
			}
			else if (g.UnicodeString != null)
			{
				num = g.UnicodeString.Length;
			}
			if (charStart > num)
			{
				charStart = num;
			}
			else if (charStart < 0)
			{
				charStart = 0;
			}
			if (charEnd > num)
			{
				charEnd = num;
			}
			else if (charEnd < 0)
			{
				charEnd = 0;
			}
			double num2 = FixedTextView._GetDistanceToCharacter(glyphRun, charStart);
			double num3 = FixedTextView._GetDistanceToCharacter(glyphRun, charEnd);
			double width = num3 - num2;
			if ((glyphRun.BidiLevel & 1) != 0)
			{
				result.X = glyphRun.BaselineOrigin.X - num3;
			}
			else
			{
				result.X = glyphRun.BaselineOrigin.X + num2;
			}
			result.Width = width;
			return result;
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x000D4634 File Offset: 0x000D2834
		private Rect _GetTransformedCaretRect(GeneralTransform transform, Point origin, double height)
		{
			Point point = origin;
			point.Y += height;
			transform.TryTransform(origin, out origin);
			transform.TryTransform(point, out point);
			Rect result = new Rect(origin, point);
			if (result.Width > 0.0)
			{
				result.X += result.Width / 2.0;
				result.Width = 0.0;
			}
			if (result.Height < 1.0)
			{
				result.Height = 1.0;
			}
			return result;
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x000D46D4 File Offset: 0x000D28D4
		private bool _GetFixedPosition(FixedTextPointer ftp, out FixedPosition fixedp)
		{
			LogicalDirection logicalDirection = ftp.LogicalDirection;
			TextPointerContext pointerContext = ((ITextPointer)ftp).GetPointerContext(logicalDirection);
			if (ftp.FlowPosition.IsBoundary || pointerContext == TextPointerContext.None)
			{
				return this._GetFirstFixedPosition(ftp, out fixedp);
			}
			if (pointerContext == TextPointerContext.ElementStart || pointerContext == TextPointerContext.ElementEnd)
			{
				if (pointerContext != TextPointerContext.ElementStart)
				{
					if (pointerContext == TextPointerContext.ElementEnd)
					{
						logicalDirection = LogicalDirection.Backward;
					}
				}
				else
				{
					logicalDirection = LogicalDirection.Forward;
				}
				FixedTextPointer fixedTextPointer = new FixedTextPointer(true, logicalDirection, (FlowPosition)ftp.FlowPosition.Clone());
				this._SkipFormattingTags(fixedTextPointer);
				pointerContext = ((ITextPointer)fixedTextPointer).GetPointerContext(logicalDirection);
				if (pointerContext != TextPointerContext.Text && pointerContext != TextPointerContext.EmbeddedElement)
				{
					if (((ITextPointer)fixedTextPointer).MoveToNextInsertionPosition(logicalDirection) && this.Container.GetPageNumber(fixedTextPointer) == this.PageIndex)
					{
						return this.Container.FixedTextBuilder.GetFixedPosition(fixedTextPointer.FlowPosition, logicalDirection, out fixedp);
					}
					fixedp = new FixedPosition(this.Container.FixedTextBuilder.FixedFlowMap.FixedStartEdge, 0);
					return false;
				}
				else
				{
					ftp = fixedTextPointer;
				}
			}
			return this.Container.FixedTextBuilder.GetFixedPosition(ftp.FlowPosition, logicalDirection, out fixedp);
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x000D47C8 File Offset: 0x000D29C8
		private bool _GetFirstFixedPosition(FixedTextPointer ftp, out FixedPosition fixedP)
		{
			LogicalDirection logicalDirection = LogicalDirection.Forward;
			if (ftp.FlowPosition.FlowNode.Fp != 0)
			{
				logicalDirection = LogicalDirection.Backward;
			}
			FlowPosition flowPosition = (FlowPosition)ftp.FlowPosition.Clone();
			flowPosition.Move(logicalDirection);
			FixedTextPointer fixedTextPointer = new FixedTextPointer(true, logicalDirection, flowPosition);
			if (flowPosition.IsStart || flowPosition.IsEnd)
			{
				((ITextPointer)fixedTextPointer).MoveToNextInsertionPosition(logicalDirection);
			}
			if (this.Container.GetPageNumber(fixedTextPointer) == this.PageIndex)
			{
				return this.Container.FixedTextBuilder.GetFixedPosition(fixedTextPointer.FlowPosition, logicalDirection, out fixedP);
			}
			fixedP = new FixedPosition(this.Container.FixedTextBuilder.FixedFlowMap.FixedStartEdge, 0);
			return false;
		}

		// Token: 0x06002EF9 RID: 12025 RVA: 0x000D4874 File Offset: 0x000D2A74
		private ITextPointer _CreateTextPointer(FixedPosition fixedPosition, LogicalDirection edge)
		{
			FlowPosition flowPosition = this.Container.FixedTextBuilder.CreateFlowPosition(fixedPosition);
			if (flowPosition != null)
			{
				return new FixedTextPointer(true, edge, flowPosition);
			}
			return null;
		}

		// Token: 0x06002EFA RID: 12026 RVA: 0x000D48A0 File Offset: 0x000D2AA0
		private ITextPointer _CreateTextPointerFromGlyphs(Glyphs g, Point point)
		{
			GeneralTransform generalTransform = this.VisualRoot.TransformToDescendant(g);
			if (generalTransform != null)
			{
				generalTransform.TryTransform(point, out point);
			}
			int offset;
			LogicalDirection edge;
			this._GlyphRunHitTest(g, point.X, out offset, out edge);
			FixedPosition fixedPosition = new FixedPosition(this.FixedPage.CreateFixedNode(this.PageIndex, g), offset);
			return this._CreateTextPointer(fixedPosition, edge);
		}

		// Token: 0x06002EFB RID: 12027 RVA: 0x000D48FC File Offset: 0x000D2AFC
		private void _SkipFormattingTags(ITextPointer textPointer)
		{
			LogicalDirection logicalDirection = textPointer.LogicalDirection;
			int offset = (logicalDirection == LogicalDirection.Forward) ? 1 : -1;
			while (TextSchema.IsFormattingType(textPointer.GetElementType(logicalDirection)))
			{
				textPointer.MoveByOffset(offset);
			}
		}

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x06002EFC RID: 12028 RVA: 0x000D4931 File Offset: 0x000D2B31
		private FixedTextContainer Container
		{
			get
			{
				return this._docPage.TextContainer;
			}
		}

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x06002EFD RID: 12029 RVA: 0x000D493E File Offset: 0x000D2B3E
		private Visual VisualRoot
		{
			get
			{
				return this._docPage.Visual;
			}
		}

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06002EFE RID: 12030 RVA: 0x000D494B File Offset: 0x000D2B4B
		private FixedPage FixedPage
		{
			get
			{
				return this._docPage.FixedPage;
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06002EFF RID: 12031 RVA: 0x000D4958 File Offset: 0x000D2B58
		private int PageIndex
		{
			get
			{
				return this._docPage.PageIndex;
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06002F00 RID: 12032 RVA: 0x000D4965 File Offset: 0x000D2B65
		private bool IsContainerStart
		{
			get
			{
				return this.Start.CompareTo(this.TextContainer.Start) == 0;
			}
		}

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06002F01 RID: 12033 RVA: 0x000D4980 File Offset: 0x000D2B80
		private bool IsContainerEnd
		{
			get
			{
				return this.End.CompareTo(this.TextContainer.End) == 0;
			}
		}

		// Token: 0x04001E17 RID: 7703
		private readonly FixedDocumentPage _docPage;

		// Token: 0x04001E18 RID: 7704
		private FixedTextPointer _start;

		// Token: 0x04001E19 RID: 7705
		private FixedTextPointer _end;

		// Token: 0x04001E1A RID: 7706
		private ReadOnlyCollection<TextSegment> _textSegments;

		// Token: 0x04001E1B RID: 7707
		private static DependencyObjectType UIElementType = DependencyObjectType.FromSystemTypeInternal(typeof(UIElement));
	}
}
