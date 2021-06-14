using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Documents;
using MS.Internal.PtsHost;
using MS.Internal.Text;
using Standard;

namespace System.Windows.Controls
{
	// Token: 0x02000467 RID: 1127
	internal class TextBoxView : FrameworkElement, ITextView, IScrollInfo, IServiceProvider
	{
		// Token: 0x06004150 RID: 16720 RVA: 0x0012AAE4 File Offset: 0x00128CE4
		static TextBoxView()
		{
			FrameworkElement.MarginProperty.OverrideMetadata(typeof(TextBoxView), new FrameworkPropertyMetadata(new Thickness(2.0, 0.0, 2.0, 0.0)));
		}

		// Token: 0x06004151 RID: 16721 RVA: 0x0012AB38 File Offset: 0x00128D38
		internal TextBoxView(ITextBoxViewHost host)
		{
			Invariant.Assert(host is Control);
			this._host = host;
		}

		// Token: 0x06004152 RID: 16722 RVA: 0x0012AB58 File Offset: 0x00128D58
		object IServiceProvider.GetService(Type serviceType)
		{
			object result = null;
			if (serviceType == typeof(ITextView))
			{
				result = this;
			}
			return result;
		}

		// Token: 0x06004153 RID: 16723 RVA: 0x0012AB7C File Offset: 0x00128D7C
		void IScrollInfo.LineUp()
		{
			if (this._scrollData != null)
			{
				this._scrollData.LineUp(this);
			}
		}

		// Token: 0x06004154 RID: 16724 RVA: 0x0012AB92 File Offset: 0x00128D92
		void IScrollInfo.LineDown()
		{
			if (this._scrollData != null)
			{
				this._scrollData.LineDown(this);
			}
		}

		// Token: 0x06004155 RID: 16725 RVA: 0x0012ABA8 File Offset: 0x00128DA8
		void IScrollInfo.LineLeft()
		{
			if (this._scrollData != null)
			{
				this._scrollData.LineLeft(this);
			}
		}

		// Token: 0x06004156 RID: 16726 RVA: 0x0012ABBE File Offset: 0x00128DBE
		void IScrollInfo.LineRight()
		{
			if (this._scrollData != null)
			{
				this._scrollData.LineRight(this);
			}
		}

		// Token: 0x06004157 RID: 16727 RVA: 0x0012ABD4 File Offset: 0x00128DD4
		void IScrollInfo.PageUp()
		{
			if (this._scrollData != null)
			{
				this._scrollData.PageUp(this);
			}
		}

		// Token: 0x06004158 RID: 16728 RVA: 0x0012ABEA File Offset: 0x00128DEA
		void IScrollInfo.PageDown()
		{
			if (this._scrollData != null)
			{
				this._scrollData.PageDown(this);
			}
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x0012AC00 File Offset: 0x00128E00
		void IScrollInfo.PageLeft()
		{
			if (this._scrollData != null)
			{
				this._scrollData.PageLeft(this);
			}
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x0012AC16 File Offset: 0x00128E16
		void IScrollInfo.PageRight()
		{
			if (this._scrollData != null)
			{
				this._scrollData.PageRight(this);
			}
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x0012AC2C File Offset: 0x00128E2C
		void IScrollInfo.MouseWheelUp()
		{
			if (this._scrollData != null)
			{
				this._scrollData.MouseWheelUp(this);
			}
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x0012AC42 File Offset: 0x00128E42
		void IScrollInfo.MouseWheelDown()
		{
			if (this._scrollData != null)
			{
				this._scrollData.MouseWheelDown(this);
			}
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x0012AC58 File Offset: 0x00128E58
		void IScrollInfo.MouseWheelLeft()
		{
			if (this._scrollData != null)
			{
				this._scrollData.MouseWheelLeft(this);
			}
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x0012AC6E File Offset: 0x00128E6E
		void IScrollInfo.MouseWheelRight()
		{
			if (this._scrollData != null)
			{
				this._scrollData.MouseWheelRight(this);
			}
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x0012AC84 File Offset: 0x00128E84
		void IScrollInfo.SetHorizontalOffset(double offset)
		{
			if (this._scrollData != null)
			{
				this._scrollData.SetHorizontalOffset(this, offset);
			}
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x0012AC9B File Offset: 0x00128E9B
		void IScrollInfo.SetVerticalOffset(double offset)
		{
			if (this._scrollData != null)
			{
				this._scrollData.SetVerticalOffset(this, offset);
			}
		}

		// Token: 0x06004161 RID: 16737 RVA: 0x0012ACB2 File Offset: 0x00128EB2
		Rect IScrollInfo.MakeVisible(Visual visual, Rect rectangle)
		{
			if (this._scrollData == null)
			{
				rectangle = Rect.Empty;
			}
			else
			{
				rectangle = this._scrollData.MakeVisible(this, visual, rectangle);
			}
			return rectangle;
		}

		// Token: 0x1700100C RID: 4108
		// (get) Token: 0x06004162 RID: 16738 RVA: 0x0012ACD6 File Offset: 0x00128ED6
		// (set) Token: 0x06004163 RID: 16739 RVA: 0x0012ACED File Offset: 0x00128EED
		bool IScrollInfo.CanVerticallyScroll
		{
			get
			{
				return this._scrollData != null && this._scrollData.CanVerticallyScroll;
			}
			set
			{
				if (this._scrollData != null)
				{
					this._scrollData.CanVerticallyScroll = value;
				}
			}
		}

		// Token: 0x1700100D RID: 4109
		// (get) Token: 0x06004164 RID: 16740 RVA: 0x0012AD03 File Offset: 0x00128F03
		// (set) Token: 0x06004165 RID: 16741 RVA: 0x0012AD1A File Offset: 0x00128F1A
		bool IScrollInfo.CanHorizontallyScroll
		{
			get
			{
				return this._scrollData != null && this._scrollData.CanHorizontallyScroll;
			}
			set
			{
				if (this._scrollData != null)
				{
					this._scrollData.CanHorizontallyScroll = value;
				}
			}
		}

		// Token: 0x1700100E RID: 4110
		// (get) Token: 0x06004166 RID: 16742 RVA: 0x0012AD30 File Offset: 0x00128F30
		double IScrollInfo.ExtentWidth
		{
			get
			{
				double num = 0.0;
				if (this._scrollData != null)
				{
					num = this._scrollData.ExtentWidth;
					if (base.UseLayoutRounding)
					{
						num = UIElement.RoundLayoutValue(num, base.GetDpi().DpiScaleX);
					}
				}
				return num;
			}
		}

		// Token: 0x1700100F RID: 4111
		// (get) Token: 0x06004167 RID: 16743 RVA: 0x0012AD7C File Offset: 0x00128F7C
		double IScrollInfo.ExtentHeight
		{
			get
			{
				double num = 0.0;
				if (this._scrollData != null)
				{
					num = this._scrollData.ExtentHeight;
					if (base.UseLayoutRounding)
					{
						num = UIElement.RoundLayoutValue(num, base.GetDpi().DpiScaleY);
					}
				}
				return num;
			}
		}

		// Token: 0x17001010 RID: 4112
		// (get) Token: 0x06004168 RID: 16744 RVA: 0x0012ADC5 File Offset: 0x00128FC5
		double IScrollInfo.ViewportWidth
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.ViewportWidth;
			}
		}

		// Token: 0x17001011 RID: 4113
		// (get) Token: 0x06004169 RID: 16745 RVA: 0x0012ADE4 File Offset: 0x00128FE4
		double IScrollInfo.ViewportHeight
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.ViewportHeight;
			}
		}

		// Token: 0x17001012 RID: 4114
		// (get) Token: 0x0600416A RID: 16746 RVA: 0x0012AE03 File Offset: 0x00129003
		double IScrollInfo.HorizontalOffset
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.HorizontalOffset;
			}
		}

		// Token: 0x17001013 RID: 4115
		// (get) Token: 0x0600416B RID: 16747 RVA: 0x0012AE22 File Offset: 0x00129022
		double IScrollInfo.VerticalOffset
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.VerticalOffset;
			}
		}

		// Token: 0x17001014 RID: 4116
		// (get) Token: 0x0600416C RID: 16748 RVA: 0x0012AE41 File Offset: 0x00129041
		// (set) Token: 0x0600416D RID: 16749 RVA: 0x0012AE58 File Offset: 0x00129058
		ScrollViewer IScrollInfo.ScrollOwner
		{
			get
			{
				if (this._scrollData == null)
				{
					return null;
				}
				return this._scrollData.ScrollOwner;
			}
			set
			{
				if (this._scrollData == null)
				{
					this._scrollData = new ScrollData();
				}
				this._scrollData.SetScrollOwner(this, value);
			}
		}

		// Token: 0x0600416E RID: 16750 RVA: 0x0012AE7C File Offset: 0x0012907C
		protected override Size MeasureOverride(Size constraint)
		{
			this.EnsureTextContainerListeners();
			if (this._lineMetrics == null)
			{
				this._lineMetrics = new List<TextBoxView.LineRecord>(1);
			}
			this._cache = null;
			this.EnsureCache();
			LineProperties lineProperties = this._cache.LineProperties;
			bool flag = !DoubleUtil.AreClose(constraint.Width, this._previousConstraint.Width);
			if (flag && lineProperties.TextAlignment != TextAlignment.Left)
			{
				this._viewportLineVisuals = null;
			}
			bool flag2 = flag && lineProperties.TextWrapping != TextWrapping.NoWrap;
			Size size;
			if (this._lineMetrics.Count == 0 || flag2)
			{
				this._dirtyList = null;
			}
			else if (this._dirtyList == null && !this.IsBackgroundLayoutPending)
			{
				size = this._contentSize;
				goto IL_17D;
			}
			if (this._dirtyList != null && this._lineMetrics.Count == 1 && this._lineMetrics[0].EndOffset == 0)
			{
				this._lineMetrics.Clear();
				this._viewportLineVisuals = null;
				this._dirtyList = null;
			}
			Size size2 = constraint;
			TextDpi.EnsureValidLineWidth(ref size2);
			if (this._dirtyList == null)
			{
				if (flag2)
				{
					this._lineMetrics.Clear();
					this._viewportLineVisuals = null;
				}
				size = this.FullMeasureTick(size2.Width, lineProperties);
			}
			else
			{
				size = this.IncrementalMeasure(size2.Width, lineProperties);
			}
			Invariant.Assert(this._lineMetrics.Count >= 1);
			this._dirtyList = null;
			double width = this._contentSize.Width;
			this._contentSize = size;
			if (width != size.Width && lineProperties.TextAlignment != TextAlignment.Left)
			{
				this.Rerender();
			}
			IL_17D:
			if (this._scrollData != null)
			{
				size.Width = Math.Min(constraint.Width, size.Width);
				size.Height = Math.Min(constraint.Height, size.Height);
			}
			this._previousConstraint = constraint;
			return size;
		}

		// Token: 0x0600416F RID: 16751 RVA: 0x0012B04A File Offset: 0x0012924A
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			if (this._lineMetrics != null && this._lineMetrics.Count != 0)
			{
				this.EnsureCache();
				this.ArrangeScrollData(arrangeSize);
				this.ArrangeVisuals(arrangeSize);
				this._cache = null;
				this.FireTextViewUpdatedEvent();
			}
			return arrangeSize;
		}

		// Token: 0x06004170 RID: 16752 RVA: 0x0012B084 File Offset: 0x00129284
		protected override void OnRender(DrawingContext context)
		{
			context.DrawRectangle(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)), null, new Rect(0.0, 0.0, base.RenderSize.Width, base.RenderSize.Height));
		}

		// Token: 0x06004171 RID: 16753 RVA: 0x0012B0D9 File Offset: 0x001292D9
		protected override Visual GetVisualChild(int index)
		{
			if (index >= this.VisualChildrenCount)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this._visualChildren[index];
		}

		// Token: 0x17001015 RID: 4117
		// (get) Token: 0x06004172 RID: 16754 RVA: 0x0012B0FB File Offset: 0x001292FB
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._visualChildren != null)
				{
					return this._visualChildren.Count;
				}
				return 0;
			}
		}

		// Token: 0x06004173 RID: 16755 RVA: 0x0012B114 File Offset: 0x00129314
		ITextPointer ITextView.GetTextPositionFromPoint(Point point, bool snapToText)
		{
			Invariant.Assert(this.IsLayoutValid);
			point = this.TransformToDocumentSpace(point);
			int lineIndexFromPoint = this.GetLineIndexFromPoint(point, snapToText);
			ITextPointer textPointer;
			if (lineIndexFromPoint == -1)
			{
				textPointer = null;
			}
			else
			{
				textPointer = this.GetTextPositionFromDistance(lineIndexFromPoint, point.X);
				textPointer.Freeze();
			}
			return textPointer;
		}

		// Token: 0x06004174 RID: 16756 RVA: 0x0012B15C File Offset: 0x0012935C
		Rect ITextView.GetRectangleFromTextPosition(ITextPointer position)
		{
			Invariant.Assert(this.IsLayoutValid);
			Invariant.Assert(this.Contains(position));
			int num = position.Offset;
			if (num > 0 && position.LogicalDirection == LogicalDirection.Backward)
			{
				num--;
			}
			int lineIndexFromOffset = this.GetLineIndexFromOffset(num);
			LineProperties lineProperties;
			FlowDirection flowDirection;
			Rect boundsFromTextPosition;
			using (TextBoxLine formattedLine = this.GetFormattedLine(lineIndexFromOffset, out lineProperties))
			{
				boundsFromTextPosition = formattedLine.GetBoundsFromTextPosition(num, out flowDirection);
			}
			if (!boundsFromTextPosition.IsEmpty)
			{
				boundsFromTextPosition.Y += (double)lineIndexFromOffset * this._lineHeight;
				if (lineProperties.FlowDirection != flowDirection)
				{
					if (position.LogicalDirection == LogicalDirection.Forward || position.Offset == 0)
					{
						boundsFromTextPosition.X = boundsFromTextPosition.Right;
					}
				}
				else if (position.LogicalDirection == LogicalDirection.Backward && position.Offset > 0)
				{
					boundsFromTextPosition.X = boundsFromTextPosition.Right;
				}
				boundsFromTextPosition.Width = 0.0;
			}
			return this.TransformToVisualSpace(boundsFromTextPosition);
		}

		// Token: 0x06004175 RID: 16757 RVA: 0x0012B254 File Offset: 0x00129454
		Rect ITextView.GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform)
		{
			transform = Transform.Identity;
			return ((ITextView)this).GetRectangleFromTextPosition(position);
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x0012B264 File Offset: 0x00129464
		Geometry ITextView.GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			Invariant.Assert(this.IsLayoutValid);
			Geometry result = null;
			double num = ((Control)this._host).FontSize * 0.5;
			int num2 = Math.Min(this._lineMetrics[this._lineMetrics.Count - 1].EndOffset, startPosition.Offset);
			int num3 = Math.Min(this._lineMetrics[this._lineMetrics.Count - 1].EndOffset, endPosition.Offset);
			int num4;
			int num5;
			this.GetVisibleLines(out num4, out num5);
			num4 = Math.Max(num4, this.GetLineIndexFromOffset(num2, LogicalDirection.Forward));
			num5 = Math.Min(num5, this.GetLineIndexFromOffset(num3, LogicalDirection.Backward));
			if (num4 > num5)
			{
				return null;
			}
			bool flag = this._lineMetrics[num4].Offset < num2 || this._lineMetrics[num4].EndOffset > num3;
			bool flag2 = this._lineMetrics[num5].Offset < num2 || this._lineMetrics[num5].EndOffset > num3;
			TextAlignment calculatedTextAlignment = this.CalculatedTextAlignment;
			int i = num4;
			if (flag)
			{
				this.GetTightBoundingGeometryFromLineIndex(i, num2, num3, calculatedTextAlignment, num, ref result);
				i++;
			}
			if (num4 <= num5 && !flag2)
			{
				num5++;
			}
			while (i < num5)
			{
				double contentOffset = this.GetContentOffset(this._lineMetrics[i].Width, calculatedTextAlignment);
				Rect rect = new Rect(contentOffset, (double)i * this._lineHeight, this._lineMetrics[i].Width, this._lineHeight);
				ITextPointer thisPosition = this._host.TextContainer.CreatePointerAtOffset(this._lineMetrics[i].EndOffset, LogicalDirection.Backward);
				if (TextPointerBase.IsNextToPlainLineBreak(thisPosition, LogicalDirection.Backward))
				{
					rect.Width += num;
				}
				rect = this.TransformToVisualSpace(rect);
				CaretElement.AddGeometry(ref result, new RectangleGeometry(rect));
				i++;
			}
			if (i == num5 && flag2)
			{
				this.GetTightBoundingGeometryFromLineIndex(i, num2, num3, calculatedTextAlignment, num, ref result);
			}
			return result;
		}

		// Token: 0x06004177 RID: 16759 RVA: 0x0012B484 File Offset: 0x00129684
		ITextPointer ITextView.GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved)
		{
			Invariant.Assert(this.IsLayoutValid);
			Invariant.Assert(this.Contains(position));
			newSuggestedX = suggestedX;
			int lineIndexFromPosition = this.GetLineIndexFromPosition(position);
			int num = Math.Max(0, Math.Min(this._lineMetrics.Count - 1, lineIndexFromPosition + count));
			linesMoved = num - lineIndexFromPosition;
			ITextPointer textPointer;
			if (linesMoved == 0)
			{
				textPointer = position.GetFrozenPointer(position.LogicalDirection);
			}
			else if (DoubleUtil.IsNaN(suggestedX))
			{
				textPointer = this._host.TextContainer.CreatePointerAtOffset(this._lineMetrics[lineIndexFromPosition + linesMoved].Offset, LogicalDirection.Forward);
			}
			else
			{
				suggestedX -= this.GetTextAlignmentCorrection(this.CalculatedTextAlignment, this.GetWrappingWidth(base.RenderSize.Width));
				textPointer = this.GetTextPositionFromDistance(num, suggestedX);
			}
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06004178 RID: 16760 RVA: 0x0012B54F File Offset: 0x0012974F
		ITextPointer ITextView.GetPositionAtNextPage(ITextPointer position, Point suggestedOffset, int count, out Point newSuggestedOffset, out int pagesMoved)
		{
			Invariant.Assert(false);
			newSuggestedOffset = default(Point);
			pagesMoved = 0;
			return null;
		}

		// Token: 0x06004179 RID: 16761 RVA: 0x0012B564 File Offset: 0x00129764
		bool ITextView.IsAtCaretUnitBoundary(ITextPointer position)
		{
			Invariant.Assert(this.IsLayoutValid);
			Invariant.Assert(this.Contains(position));
			bool result = false;
			int lineIndexFromPosition = this.GetLineIndexFromPosition(position);
			CharacterHit charHit = default(CharacterHit);
			if (position.LogicalDirection == LogicalDirection.Forward)
			{
				charHit = new CharacterHit(position.Offset, 0);
			}
			else if (position.LogicalDirection == LogicalDirection.Backward)
			{
				if (position.Offset <= this._lineMetrics[lineIndexFromPosition].Offset)
				{
					return false;
				}
				charHit = new CharacterHit(position.Offset - 1, 1);
			}
			using (TextBoxLine formattedLine = this.GetFormattedLine(lineIndexFromPosition))
			{
				result = formattedLine.IsAtCaretCharacterHit(charHit);
			}
			return result;
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x0012B618 File Offset: 0x00129818
		ITextPointer ITextView.GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			Invariant.Assert(this.IsLayoutValid);
			Invariant.Assert(this.Contains(position));
			if (position.Offset == 0 && direction == LogicalDirection.Backward)
			{
				return position.GetFrozenPointer(LogicalDirection.Forward);
			}
			if (position.Offset == this._host.TextContainer.SymbolCount && direction == LogicalDirection.Forward)
			{
				return position.GetFrozenPointer(LogicalDirection.Backward);
			}
			int lineIndexFromPosition = this.GetLineIndexFromPosition(position);
			CharacterHit index = new CharacterHit(position.Offset, 0);
			CharacterHit characterHit;
			using (TextBoxLine formattedLine = this.GetFormattedLine(lineIndexFromPosition))
			{
				if (direction == LogicalDirection.Forward)
				{
					characterHit = formattedLine.GetNextCaretCharacterHit(index);
				}
				else
				{
					characterHit = formattedLine.GetPreviousCaretCharacterHit(index);
				}
			}
			LogicalDirection direction2;
			if (characterHit.FirstCharacterIndex + characterHit.TrailingLength == this._lineMetrics[lineIndexFromPosition].EndOffset && direction == LogicalDirection.Forward)
			{
				if (lineIndexFromPosition == this._lineMetrics.Count - 1)
				{
					direction2 = LogicalDirection.Backward;
				}
				else
				{
					direction2 = LogicalDirection.Forward;
				}
			}
			else if (characterHit.FirstCharacterIndex + characterHit.TrailingLength == this._lineMetrics[lineIndexFromPosition].Offset && direction == LogicalDirection.Backward)
			{
				if (lineIndexFromPosition == 0)
				{
					direction2 = LogicalDirection.Forward;
				}
				else
				{
					direction2 = LogicalDirection.Backward;
				}
			}
			else
			{
				direction2 = ((characterHit.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward);
			}
			ITextPointer textPointer = this._host.TextContainer.CreatePointerAtOffset(characterHit.FirstCharacterIndex + characterHit.TrailingLength, direction2);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x0600417B RID: 16763 RVA: 0x0012B770 File Offset: 0x00129970
		ITextPointer ITextView.GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			Invariant.Assert(this.IsLayoutValid);
			Invariant.Assert(this.Contains(position));
			if (position.Offset == 0)
			{
				return position.GetFrozenPointer(LogicalDirection.Forward);
			}
			int lineIndexFromPosition = this.GetLineIndexFromPosition(position, LogicalDirection.Backward);
			CharacterHit index = new CharacterHit(position.Offset, 0);
			CharacterHit backspaceCaretCharacterHit;
			using (TextBoxLine formattedLine = this.GetFormattedLine(lineIndexFromPosition))
			{
				backspaceCaretCharacterHit = formattedLine.GetBackspaceCaretCharacterHit(index);
			}
			LogicalDirection direction;
			if (backspaceCaretCharacterHit.FirstCharacterIndex + backspaceCaretCharacterHit.TrailingLength == this._lineMetrics[lineIndexFromPosition].Offset)
			{
				if (lineIndexFromPosition == 0)
				{
					direction = LogicalDirection.Forward;
				}
				else
				{
					direction = LogicalDirection.Backward;
				}
			}
			else
			{
				direction = ((backspaceCaretCharacterHit.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward);
			}
			ITextPointer textPointer = this._host.TextContainer.CreatePointerAtOffset(backspaceCaretCharacterHit.FirstCharacterIndex + backspaceCaretCharacterHit.TrailingLength, direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x0600417C RID: 16764 RVA: 0x0012B854 File Offset: 0x00129A54
		TextSegment ITextView.GetLineRange(ITextPointer position)
		{
			Invariant.Assert(this.IsLayoutValid);
			Invariant.Assert(this.Contains(position));
			int lineIndexFromPosition = this.GetLineIndexFromPosition(position);
			ITextPointer startPosition = this._host.TextContainer.CreatePointerAtOffset(this._lineMetrics[lineIndexFromPosition].Offset, LogicalDirection.Forward);
			ITextPointer endPosition = this._host.TextContainer.CreatePointerAtOffset(this._lineMetrics[lineIndexFromPosition].Offset + this._lineMetrics[lineIndexFromPosition].ContentLength, LogicalDirection.Forward);
			return new TextSegment(startPosition, endPosition, true);
		}

		// Token: 0x0600417D RID: 16765 RVA: 0x0012B8E0 File Offset: 0x00129AE0
		ReadOnlyCollection<GlyphRun> ITextView.GetGlyphRuns(ITextPointer start, ITextPointer end)
		{
			Invariant.Assert(false);
			return null;
		}

		// Token: 0x0600417E RID: 16766 RVA: 0x0012B8E9 File Offset: 0x00129AE9
		bool ITextView.Contains(ITextPointer position)
		{
			return this.Contains(position);
		}

		// Token: 0x0600417F RID: 16767 RVA: 0x0012B8F2 File Offset: 0x00129AF2
		void ITextView.BringPositionIntoViewAsync(ITextPointer position, object userState)
		{
			Invariant.Assert(false);
		}

		// Token: 0x06004180 RID: 16768 RVA: 0x0012B8F2 File Offset: 0x00129AF2
		void ITextView.BringPointIntoViewAsync(Point point, object userState)
		{
			Invariant.Assert(false);
		}

		// Token: 0x06004181 RID: 16769 RVA: 0x0012B8F2 File Offset: 0x00129AF2
		void ITextView.BringLineIntoViewAsync(ITextPointer position, double suggestedX, int count, object userState)
		{
			Invariant.Assert(false);
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x0012B8F2 File Offset: 0x00129AF2
		void ITextView.BringPageIntoViewAsync(ITextPointer position, Point suggestedOffset, int count, object userState)
		{
			Invariant.Assert(false);
		}

		// Token: 0x06004183 RID: 16771 RVA: 0x0012B8F2 File Offset: 0x00129AF2
		void ITextView.CancelAsync(object userState)
		{
			Invariant.Assert(false);
		}

		// Token: 0x06004184 RID: 16772 RVA: 0x0012B8FA File Offset: 0x00129AFA
		bool ITextView.Validate()
		{
			base.UpdateLayout();
			return this.IsLayoutValid;
		}

		// Token: 0x06004185 RID: 16773 RVA: 0x0012B908 File Offset: 0x00129B08
		bool ITextView.Validate(Point point)
		{
			return ((ITextView)this).Validate();
		}

		// Token: 0x06004186 RID: 16774 RVA: 0x0012B910 File Offset: 0x00129B10
		bool ITextView.Validate(ITextPointer position)
		{
			if (position.TextContainer != this._host.TextContainer)
			{
				return false;
			}
			if (!this.IsLayoutValid)
			{
				base.UpdateLayout();
				if (!this.IsLayoutValid)
				{
					return false;
				}
			}
			int num = this._lineMetrics[this._lineMetrics.Count - 1].EndOffset;
			while (!this.Contains(position))
			{
				base.InvalidateMeasure();
				base.UpdateLayout();
				if (!this.IsLayoutValid)
				{
					break;
				}
				int endOffset = this._lineMetrics[this._lineMetrics.Count - 1].EndOffset;
				if (num >= endOffset)
				{
					break;
				}
				num = endOffset;
			}
			return this.IsLayoutValid && this.Contains(position);
		}

		// Token: 0x06004187 RID: 16775 RVA: 0x0012B9BC File Offset: 0x00129BBC
		void ITextView.ThrottleBackgroundTasksForUserInput()
		{
			if (this._throttleBackgroundTimer == null)
			{
				this._throttleBackgroundTimer = new DispatcherTimer(DispatcherPriority.Background);
				this._throttleBackgroundTimer.Interval = new TimeSpan(0, 0, 2);
				this._throttleBackgroundTimer.Tick += this.OnThrottleBackgroundTimeout;
			}
			else
			{
				this._throttleBackgroundTimer.Stop();
			}
			this._throttleBackgroundTimer.Start();
		}

		// Token: 0x06004188 RID: 16776 RVA: 0x0012BA1F File Offset: 0x00129C1F
		internal void Remeasure()
		{
			if (this._lineMetrics != null)
			{
				this._lineMetrics.Clear();
				this._viewportLineVisuals = null;
			}
			base.InvalidateMeasure();
		}

		// Token: 0x06004189 RID: 16777 RVA: 0x0012BA41 File Offset: 0x00129C41
		internal void Rerender()
		{
			this._viewportLineVisuals = null;
			base.InvalidateArrange();
		}

		// Token: 0x0600418A RID: 16778 RVA: 0x0012BA50 File Offset: 0x00129C50
		internal int GetLineIndexFromOffset(int offset)
		{
			int num = 0;
			int num2 = this._lineMetrics.Count;
			Invariant.Assert(this._lineMetrics.Count >= 1);
			int num3;
			TextBoxView.LineRecord lineRecord;
			for (;;)
			{
				Invariant.Assert(num < num2, "Couldn't find offset!");
				num3 = num + (num2 - num) / 2;
				lineRecord = this._lineMetrics[num3];
				if (offset < lineRecord.Offset)
				{
					num2 = num3;
				}
				else
				{
					if (offset <= lineRecord.EndOffset)
					{
						break;
					}
					num = num3 + 1;
				}
			}
			if (offset == lineRecord.EndOffset && num3 < this._lineMetrics.Count - 1)
			{
				num3++;
			}
			return num3;
		}

		// Token: 0x0600418B RID: 16779 RVA: 0x0012BAE0 File Offset: 0x00129CE0
		internal void RemoveTextContainerListeners()
		{
			if (!this.CheckFlags(TextBoxView.Flags.TextContainerListenersInitialized))
			{
				return;
			}
			this._host.TextContainer.Changing -= this.OnTextContainerChanging;
			this._host.TextContainer.Change -= this.OnTextContainerChange;
			this._host.TextContainer.Highlights.Changed -= this.OnHighlightChanged;
			this.SetFlags(false, TextBoxView.Flags.TextContainerListenersInitialized);
		}

		// Token: 0x17001016 RID: 4118
		// (get) Token: 0x0600418C RID: 16780 RVA: 0x0012BB58 File Offset: 0x00129D58
		internal ITextBoxViewHost Host
		{
			get
			{
				return this._host;
			}
		}

		// Token: 0x17001017 RID: 4119
		// (get) Token: 0x0600418D RID: 16781 RVA: 0x0001B7E3 File Offset: 0x000199E3
		UIElement ITextView.RenderScope
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17001018 RID: 4120
		// (get) Token: 0x0600418E RID: 16782 RVA: 0x0012BB60 File Offset: 0x00129D60
		ITextContainer ITextView.TextContainer
		{
			get
			{
				return this._host.TextContainer;
			}
		}

		// Token: 0x17001019 RID: 4121
		// (get) Token: 0x0600418F RID: 16783 RVA: 0x0012BB6D File Offset: 0x00129D6D
		bool ITextView.IsValid
		{
			get
			{
				return this.IsLayoutValid;
			}
		}

		// Token: 0x1700101A RID: 4122
		// (get) Token: 0x06004190 RID: 16784 RVA: 0x0012BB75 File Offset: 0x00129D75
		bool ITextView.RendersOwnSelection
		{
			get
			{
				return !FrameworkAppContextSwitches.UseAdornerForTextboxSelectionRendering;
			}
		}

		// Token: 0x1700101B RID: 4123
		// (get) Token: 0x06004191 RID: 16785 RVA: 0x0012BB80 File Offset: 0x00129D80
		ReadOnlyCollection<TextSegment> ITextView.TextSegments
		{
			get
			{
				List<TextSegment> list = new List<TextSegment>(1);
				if (this._lineMetrics != null)
				{
					ITextPointer startPosition = this._host.TextContainer.CreatePointerAtOffset(this._lineMetrics[0].Offset, LogicalDirection.Backward);
					ITextPointer endPosition = this._host.TextContainer.CreatePointerAtOffset(this._lineMetrics[this._lineMetrics.Count - 1].EndOffset, LogicalDirection.Forward);
					list.Add(new TextSegment(startPosition, endPosition, true));
				}
				return new ReadOnlyCollection<TextSegment>(list);
			}
		}

		// Token: 0x1400009A RID: 154
		// (add) Token: 0x06004192 RID: 16786 RVA: 0x0012B8F2 File Offset: 0x00129AF2
		// (remove) Token: 0x06004193 RID: 16787 RVA: 0x0012B8F2 File Offset: 0x00129AF2
		event BringPositionIntoViewCompletedEventHandler ITextView.BringPositionIntoViewCompleted
		{
			add
			{
				Invariant.Assert(false);
			}
			remove
			{
				Invariant.Assert(false);
			}
		}

		// Token: 0x1400009B RID: 155
		// (add) Token: 0x06004194 RID: 16788 RVA: 0x0012B8F2 File Offset: 0x00129AF2
		// (remove) Token: 0x06004195 RID: 16789 RVA: 0x0012B8F2 File Offset: 0x00129AF2
		event BringPointIntoViewCompletedEventHandler ITextView.BringPointIntoViewCompleted
		{
			add
			{
				Invariant.Assert(false);
			}
			remove
			{
				Invariant.Assert(false);
			}
		}

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x06004196 RID: 16790 RVA: 0x0012B8F2 File Offset: 0x00129AF2
		// (remove) Token: 0x06004197 RID: 16791 RVA: 0x0012B8F2 File Offset: 0x00129AF2
		event BringLineIntoViewCompletedEventHandler ITextView.BringLineIntoViewCompleted
		{
			add
			{
				Invariant.Assert(false);
			}
			remove
			{
				Invariant.Assert(false);
			}
		}

		// Token: 0x1400009D RID: 157
		// (add) Token: 0x06004198 RID: 16792 RVA: 0x0012B8F2 File Offset: 0x00129AF2
		// (remove) Token: 0x06004199 RID: 16793 RVA: 0x0012B8F2 File Offset: 0x00129AF2
		event BringPageIntoViewCompletedEventHandler ITextView.BringPageIntoViewCompleted
		{
			add
			{
				Invariant.Assert(false);
			}
			remove
			{
				Invariant.Assert(false);
			}
		}

		// Token: 0x1400009E RID: 158
		// (add) Token: 0x0600419A RID: 16794 RVA: 0x0012BC02 File Offset: 0x00129E02
		// (remove) Token: 0x0600419B RID: 16795 RVA: 0x0012BC1B File Offset: 0x00129E1B
		event EventHandler Updated;

		// Token: 0x0600419C RID: 16796 RVA: 0x0012BC34 File Offset: 0x00129E34
		private void EnsureTextContainerListeners()
		{
			if (this.CheckFlags(TextBoxView.Flags.TextContainerListenersInitialized))
			{
				return;
			}
			this._host.TextContainer.Changing += this.OnTextContainerChanging;
			this._host.TextContainer.Change += this.OnTextContainerChange;
			this._host.TextContainer.Highlights.Changed += this.OnHighlightChanged;
			this.SetFlags(true, TextBoxView.Flags.TextContainerListenersInitialized);
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x0012BCAC File Offset: 0x00129EAC
		private void EnsureCache()
		{
			if (this._cache == null)
			{
				this._cache = new TextBoxView.TextCache(this);
			}
		}

		// Token: 0x0600419E RID: 16798 RVA: 0x0012BCC4 File Offset: 0x00129EC4
		private LineProperties GetLineProperties()
		{
			TextProperties defaultTextProperties = new TextProperties((Control)this._host, this._host.IsTypographyDefaultValue);
			return new LineProperties((Control)this._host, (Control)this._host, defaultTextProperties, null, this.CalculatedTextAlignment);
		}

		// Token: 0x0600419F RID: 16799 RVA: 0x00002137 File Offset: 0x00000337
		private void OnTextContainerChanging(object sender, EventArgs args)
		{
		}

		// Token: 0x060041A0 RID: 16800 RVA: 0x0012BD10 File Offset: 0x00129F10
		private void OnTextContainerChange(object sender, TextContainerChangeEventArgs args)
		{
			if (args.Count == 0)
			{
				return;
			}
			if (this._dirtyList == null)
			{
				this._dirtyList = new DtrList();
			}
			DirtyTextRange dtr = new DirtyTextRange(args);
			this._dirtyList.Merge(dtr);
			base.InvalidateMeasure();
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x0012BD54 File Offset: 0x00129F54
		private void OnHighlightChanged(object sender, HighlightChangedEventArgs args)
		{
			if (args.OwnerType != typeof(SpellerHighlightLayer) && (!((ITextView)this).RendersOwnSelection || args.OwnerType != typeof(TextSelection)))
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			if (this._dirtyList == null)
			{
				this._dirtyList = new DtrList();
			}
			DtrList dtrList = new DtrList();
			foreach (object obj in args.Ranges)
			{
				TextSegment textSegment = (TextSegment)obj;
				int num = textSegment.End.Offset - textSegment.Start.Offset;
				DirtyTextRange dtr = new DirtyTextRange(textSegment.Start.Offset, num, num, true);
				dtrList.Merge(dtr);
			}
			DirtyTextRange mergedRange = dtrList.GetMergedRange();
			if (args.OwnerType == typeof(TextSelection))
			{
				this.HandleTextSelectionHighlightChange(mergedRange, ref flag2, ref flag);
			}
			else if (args.OwnerType == typeof(SpellerHighlightLayer))
			{
				this._dirtyList.Merge(mergedRange);
				flag = true;
			}
			if (flag)
			{
				base.InvalidateMeasure();
				return;
			}
			if (flag2)
			{
				base.InvalidateArrange();
			}
		}

		// Token: 0x060041A2 RID: 16802 RVA: 0x0012BEA0 File Offset: 0x0012A0A0
		private void HandleTextSelectionHighlightChange(DirtyTextRange currentSelectionRange, ref bool arrangeNeeded, ref bool measureNeeded)
		{
			if (this._lineMetrics.Count == 0)
			{
				measureNeeded = true;
				return;
			}
			if (this._dirtyList.Length > 0 && this._dirtyList.DtrsFromRange(currentSelectionRange.StartIndex, currentSelectionRange.PositionsAdded) != null)
			{
				this._dirtyList.Merge(currentSelectionRange);
				measureNeeded = true;
				return;
			}
			int[] array = new int[]
			{
				currentSelectionRange.StartIndex,
				currentSelectionRange.StartIndex + currentSelectionRange.PositionsAdded
			};
			using (TextBoxLine textBoxLine = new TextBoxLine(this))
			{
				Control element = (Control)this._host;
				LineProperties lineProperties = this.GetLineProperties();
				TextFormattingMode textFormattingMode = TextOptions.GetTextFormattingMode(element);
				TextFormatter formatter = TextFormatter.FromCurrentDispatcher(textFormattingMode);
				double wrappingWidth = this.GetWrappingWidth(base.RenderSize.Width);
				double wrappingWidth2 = this.GetWrappingWidth(this._previousConstraint.Width);
				foreach (int offset in array)
				{
					int lineIndexFromOffset = this.GetLineIndexFromOffset(offset);
					TextBoxView.LineRecord lineRecord = this._lineMetrics[lineIndexFromOffset];
					textBoxLine.Format(lineRecord.Offset, wrappingWidth2, wrappingWidth, lineProperties, new TextRunCache(), formatter);
					if (lineRecord.Length != textBoxLine.Length)
					{
						measureNeeded = true;
						this._dirtyList.Merge(new DirtyTextRange(lineRecord.Offset, lineRecord.Length, lineRecord.Length, true));
					}
				}
			}
			if (!measureNeeded)
			{
				DirtyTextRange? selectionRenderRange = this.GetSelectionRenderRange(currentSelectionRange);
				if (selectionRenderRange != null)
				{
					this._dirtyList.Merge(selectionRenderRange.Value);
					arrangeNeeded = true;
					this.SetFlags(true, TextBoxView.Flags.ArrangePendingFromHighlightLayer);
					return;
				}
				if (this._dirtyList.Length == 0)
				{
					this._dirtyList = null;
				}
			}
		}

		// Token: 0x060041A3 RID: 16803 RVA: 0x0012C05C File Offset: 0x0012A25C
		private void SetFlags(bool value, TextBoxView.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x060041A4 RID: 16804 RVA: 0x0012C07A File Offset: 0x0012A27A
		private bool CheckFlags(TextBoxView.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x060041A5 RID: 16805 RVA: 0x0012C087 File Offset: 0x0012A287
		private void FireTextViewUpdatedEvent()
		{
			if (this.UpdatedEvent != null)
			{
				this.UpdatedEvent(this, EventArgs.Empty);
			}
		}

		// Token: 0x060041A6 RID: 16806 RVA: 0x0012C0A4 File Offset: 0x0012A2A4
		private int GetLineIndexFromPoint(Point point, bool snapToText)
		{
			Invariant.Assert(this._lineMetrics.Count >= 1);
			if (point.Y < 0.0)
			{
				if (!snapToText)
				{
					return -1;
				}
				return 0;
			}
			else if (point.Y >= this._lineHeight * (double)this._lineMetrics.Count)
			{
				if (!snapToText)
				{
					return -1;
				}
				return this._lineMetrics.Count - 1;
			}
			else
			{
				int num = -1;
				int i = 0;
				int num2 = this._lineMetrics.Count;
				while (i < num2)
				{
					num = i + (num2 - i) / 2;
					TextBoxView.LineRecord lineRecord = this._lineMetrics[num];
					double num3 = this._lineHeight * (double)num;
					if (point.Y < num3)
					{
						num2 = num;
					}
					else if (point.Y >= num3 + this._lineHeight)
					{
						i = num + 1;
					}
					else
					{
						if (!snapToText && (point.X < 0.0 || point.X >= lineRecord.Width))
						{
							num = -1;
							break;
						}
						break;
					}
				}
				if (i >= num2)
				{
					return -1;
				}
				return num;
			}
		}

		// Token: 0x060041A7 RID: 16807 RVA: 0x0012C19B File Offset: 0x0012A39B
		private int GetLineIndexFromPosition(ITextPointer position)
		{
			return this.GetLineIndexFromOffset(position.Offset, position.LogicalDirection);
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x0012C1AF File Offset: 0x0012A3AF
		private int GetLineIndexFromPosition(ITextPointer position, LogicalDirection direction)
		{
			return this.GetLineIndexFromOffset(position.Offset, direction);
		}

		// Token: 0x060041A9 RID: 16809 RVA: 0x0012C1BE File Offset: 0x0012A3BE
		private int GetLineIndexFromOffset(int offset, LogicalDirection direction)
		{
			if (offset > 0 && direction == LogicalDirection.Backward)
			{
				offset--;
			}
			return this.GetLineIndexFromOffset(offset);
		}

		// Token: 0x060041AA RID: 16810 RVA: 0x0012C1D4 File Offset: 0x0012A3D4
		private TextBoxLine GetFormattedLine(int lineIndex)
		{
			LineProperties lineProperties;
			return this.GetFormattedLine(lineIndex, out lineProperties);
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x0012C1EC File Offset: 0x0012A3EC
		private TextBoxLine GetFormattedLine(int lineIndex, out LineProperties lineProperties)
		{
			TextBoxLine textBoxLine = new TextBoxLine(this);
			TextBoxView.LineRecord lineRecord = this._lineMetrics[lineIndex];
			lineProperties = this.GetLineProperties();
			Control element = (Control)this._host;
			TextFormattingMode textFormattingMode = TextOptions.GetTextFormattingMode(element);
			TextFormatter formatter = TextFormatter.FromCurrentDispatcher(textFormattingMode);
			double wrappingWidth = this.GetWrappingWidth(base.RenderSize.Width);
			double wrappingWidth2 = this.GetWrappingWidth(this._previousConstraint.Width);
			textBoxLine.Format(lineRecord.Offset, wrappingWidth2, wrappingWidth, lineProperties, new TextRunCache(), formatter);
			Invariant.Assert(lineRecord.Length == textBoxLine.Length, "Line is out of sync with metrics!");
			return textBoxLine;
		}

		// Token: 0x060041AC RID: 16812 RVA: 0x0012C28C File Offset: 0x0012A48C
		private ITextPointer GetTextPositionFromDistance(int lineIndex, double x)
		{
			LineProperties lineProperties;
			CharacterHit textPositionFromDistance;
			LogicalDirection direction;
			using (TextBoxLine formattedLine = this.GetFormattedLine(lineIndex, out lineProperties))
			{
				textPositionFromDistance = formattedLine.GetTextPositionFromDistance(x);
				direction = ((textPositionFromDistance.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward);
			}
			return this._host.TextContainer.CreatePointerAtOffset(textPositionFromDistance.FirstCharacterIndex + textPositionFromDistance.TrailingLength, direction);
		}

		// Token: 0x060041AD RID: 16813 RVA: 0x0012C2F8 File Offset: 0x0012A4F8
		private void ArrangeScrollData(Size arrangeSize)
		{
			if (this._scrollData == null)
			{
				return;
			}
			bool flag = false;
			if (!DoubleUtil.AreClose(this._scrollData.Viewport, arrangeSize))
			{
				this._scrollData.Viewport = arrangeSize;
				flag = true;
			}
			if (!DoubleUtil.AreClose(this._scrollData.Extent, this._contentSize))
			{
				this._scrollData.Extent = this._contentSize;
				flag = true;
			}
			Vector vector = new Vector(Math.Max(0.0, Math.Min(this._scrollData.ExtentWidth - this._scrollData.ViewportWidth, this._scrollData.HorizontalOffset)), Math.Max(0.0, Math.Min(this._scrollData.ExtentHeight - this._scrollData.ViewportHeight, this._scrollData.VerticalOffset)));
			if (!DoubleUtil.AreClose(vector, this._scrollData.Offset))
			{
				this._scrollData.Offset = vector;
				flag = true;
			}
			if (flag && this._scrollData.ScrollOwner != null)
			{
				this._scrollData.ScrollOwner.InvalidateScrollInfo();
			}
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x0012C410 File Offset: 0x0012A610
		private void ArrangeVisuals(Size arrangeSize)
		{
			Invariant.Assert(this.CheckFlags(TextBoxView.Flags.ArrangePendingFromHighlightLayer) || this._dirtyList == null);
			this.SetFlags(false, TextBoxView.Flags.ArrangePendingFromHighlightLayer);
			if (this._dirtyList != null)
			{
				this.InvalidateDirtyVisuals();
				this._dirtyList = null;
			}
			if (this._visualChildren == null)
			{
				this._visualChildren = new List<TextBoxLineDrawingVisual>(1);
			}
			this.EnsureCache();
			LineProperties lineProperties = this._cache.LineProperties;
			TextBoxLine textBoxLine = new TextBoxLine(this);
			int num;
			int num2;
			this.GetVisibleLines(out num, out num2);
			this.SetViewportLines(num, num2);
			double wrappingWidth = this.GetWrappingWidth(arrangeSize.Width);
			double num3 = this.GetTextAlignmentCorrection(lineProperties.TextAlignment, wrappingWidth);
			double num4 = this.VerticalAlignmentOffset;
			if (this._scrollData != null)
			{
				num3 -= this._scrollData.HorizontalOffset;
				num4 -= this._scrollData.VerticalOffset;
			}
			this.DetachDiscardedVisualChildren();
			double wrappingWidth2 = this.GetWrappingWidth(this._previousConstraint.Width);
			double endOfParaGlyphWidth = ((Control)this._host).FontSize * 0.5;
			bool flag = ((ITextView)this).RendersOwnSelection && ((bool)((Control)this._host).GetValue(TextBoxBase.IsInactiveSelectionHighlightEnabledProperty) || (bool)((Control)this._host).GetValue(TextBoxBase.IsSelectionActiveProperty));
			for (int i = num; i <= num2; i++)
			{
				TextBoxLineDrawingVisual textBoxLineDrawingVisual = this.GetLineVisual(i);
				if (textBoxLineDrawingVisual == null)
				{
					TextBoxView.LineRecord lineRecord = this._lineMetrics[i];
					using (textBoxLine)
					{
						textBoxLine.Format(lineRecord.Offset, wrappingWidth2, wrappingWidth, lineProperties, this._cache.TextRunCache, this._cache.TextFormatter);
						if (!this.IsBackgroundLayoutPending)
						{
							Invariant.Assert(lineRecord.Length == textBoxLine.Length, "Line is out of sync with metrics!");
						}
						Geometry selectionGeometry = null;
						if (flag)
						{
							ITextSelection textSelection = this._host.TextContainer.TextSelection;
							if (!textSelection.IsEmpty)
							{
								this.GetTightBoundingGeometryFromLineIndexForSelection(textBoxLine, i, textSelection.Start.CharOffset, textSelection.End.CharOffset, this.CalculatedTextAlignment, endOfParaGlyphWidth, ref selectionGeometry);
							}
						}
						textBoxLineDrawingVisual = textBoxLine.CreateVisual(selectionGeometry);
					}
					this.SetLineVisual(i, textBoxLineDrawingVisual);
					this.AttachVisualChild(textBoxLineDrawingVisual);
				}
				textBoxLineDrawingVisual.Offset = new Vector(num3, num4 + (double)i * this._lineHeight);
			}
		}

		// Token: 0x060041AF RID: 16815 RVA: 0x0012C684 File Offset: 0x0012A884
		private void InvalidateDirtyVisuals()
		{
			for (int i = 0; i < this._dirtyList.Length; i++)
			{
				DirtyTextRange dirtyTextRange = this._dirtyList[i];
				Invariant.Assert(dirtyTextRange.FromHighlightLayer);
				Invariant.Assert(dirtyTextRange.PositionsAdded == dirtyTextRange.PositionsRemoved);
				int lineIndexFromOffset = this.GetLineIndexFromOffset(dirtyTextRange.StartIndex, LogicalDirection.Forward);
				int offset = Math.Min(dirtyTextRange.StartIndex + dirtyTextRange.PositionsAdded, this._host.TextContainer.SymbolCount);
				int lineIndexFromOffset2 = this.GetLineIndexFromOffset(offset, LogicalDirection.Backward);
				for (int j = lineIndexFromOffset; j <= lineIndexFromOffset2; j++)
				{
					this.ClearLineVisual(j);
				}
			}
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x0012C734 File Offset: 0x0012A934
		private void DetachDiscardedVisualChildren()
		{
			int num = this._visualChildren.Count - 1;
			for (int i = this._visualChildren.Count - 1; i >= 0; i--)
			{
				if (this._visualChildren[i] == null || this._visualChildren[i].DiscardOnArrange)
				{
					base.RemoveVisualChild(this._visualChildren[i]);
					if (i < num)
					{
						this._visualChildren[i] = this._visualChildren[num];
					}
					num--;
				}
			}
			if (num < this._visualChildren.Count - 1)
			{
				this._visualChildren.RemoveRange(num + 1, this._visualChildren.Count - num - 1);
			}
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x0012C7E7 File Offset: 0x0012A9E7
		private void AttachVisualChild(TextBoxLineDrawingVisual lineVisual)
		{
			lineVisual._parentIndex = this._visualChildren.Count;
			base.AddVisualChild(lineVisual);
			this._visualChildren.Add(lineVisual);
		}

		// Token: 0x060041B2 RID: 16818 RVA: 0x0012C810 File Offset: 0x0012AA10
		private void ClearVisualChildren()
		{
			for (int i = 0; i < this._visualChildren.Count; i++)
			{
				base.RemoveVisualChild(this._visualChildren[i]);
			}
			this._visualChildren.Clear();
		}

		// Token: 0x060041B3 RID: 16819 RVA: 0x0012C850 File Offset: 0x0012AA50
		private Point TransformToDocumentSpace(Point point)
		{
			if (this._scrollData != null)
			{
				point = new Point(point.X + this._scrollData.HorizontalOffset, point.Y + this._scrollData.VerticalOffset);
			}
			point.X -= this.GetTextAlignmentCorrection(this.CalculatedTextAlignment, this.GetWrappingWidth(base.RenderSize.Width));
			point.Y -= this.VerticalAlignmentOffset;
			return point;
		}

		// Token: 0x060041B4 RID: 16820 RVA: 0x0012C8D8 File Offset: 0x0012AAD8
		private Rect TransformToVisualSpace(Rect rect)
		{
			if (this._scrollData != null)
			{
				rect.X -= this._scrollData.HorizontalOffset;
				rect.Y -= this._scrollData.VerticalOffset;
			}
			rect.X += this.GetTextAlignmentCorrection(this.CalculatedTextAlignment, this.GetWrappingWidth(base.RenderSize.Width));
			rect.Y += this.VerticalAlignmentOffset;
			return rect;
		}

		// Token: 0x060041B5 RID: 16821 RVA: 0x0012C964 File Offset: 0x0012AB64
		private void GetTightBoundingGeometryFromLineIndex(int lineIndex, int unclippedStartOffset, int unclippedEndOffset, TextAlignment alignment, double endOfParaGlyphWidth, ref Geometry geometry)
		{
			int num = Math.Max(this._lineMetrics[lineIndex].Offset, unclippedStartOffset);
			int num2 = Math.Min(this._lineMetrics[lineIndex].EndOffset, unclippedEndOffset);
			if (num == num2)
			{
				if (unclippedStartOffset != this._lineMetrics[lineIndex].EndOffset)
				{
					Invariant.Assert(num2 == this._lineMetrics[lineIndex].Offset || num2 == this._lineMetrics[lineIndex].Offset + this._lineMetrics[lineIndex].ContentLength);
					return;
				}
				ITextPointer thisPosition = this._host.TextContainer.CreatePointerAtOffset(unclippedStartOffset, LogicalDirection.Backward);
				if (TextPointerBase.IsNextToPlainLineBreak(thisPosition, LogicalDirection.Backward))
				{
					Rect rect = new Rect(0.0, (double)lineIndex * this._lineHeight, endOfParaGlyphWidth, this._lineHeight);
					CaretElement.AddGeometry(ref geometry, new RectangleGeometry(rect));
					return;
				}
			}
			else
			{
				IList<Rect> rangeBounds;
				using (TextBoxLine formattedLine = this.GetFormattedLine(lineIndex))
				{
					rangeBounds = formattedLine.GetRangeBounds(num, num2 - num, 0.0, (double)lineIndex * this._lineHeight);
				}
				for (int i = 0; i < rangeBounds.Count; i++)
				{
					Rect rect2 = this.TransformToVisualSpace(rangeBounds[i]);
					CaretElement.AddGeometry(ref geometry, new RectangleGeometry(rect2));
				}
				if (unclippedEndOffset >= this._lineMetrics[lineIndex].EndOffset)
				{
					ITextPointer thisPosition2 = this._host.TextContainer.CreatePointerAtOffset(num2, LogicalDirection.Backward);
					if (TextPointerBase.IsNextToPlainLineBreak(thisPosition2, LogicalDirection.Backward))
					{
						double contentOffset = this.GetContentOffset(this._lineMetrics[lineIndex].Width, alignment);
						Rect rect3 = new Rect(contentOffset + this._lineMetrics[lineIndex].Width, (double)lineIndex * this._lineHeight, endOfParaGlyphWidth, this._lineHeight);
						rect3 = this.TransformToVisualSpace(rect3);
						CaretElement.AddGeometry(ref geometry, new RectangleGeometry(rect3));
					}
				}
			}
		}

		// Token: 0x060041B6 RID: 16822 RVA: 0x0012CB58 File Offset: 0x0012AD58
		private void GetTightBoundingGeometryFromLineIndexForSelection(TextBoxLine line, int lineIndex, int unclippedStartOffset, int unclippedEndOffset, TextAlignment alignment, double endOfParaGlyphWidth, ref Geometry geometry)
		{
			int offset = this._lineMetrics[lineIndex].Offset;
			int endOffset = this._lineMetrics[lineIndex].EndOffset;
			if (offset > unclippedEndOffset || endOffset <= unclippedStartOffset)
			{
				return;
			}
			int num = Math.Max(offset, unclippedStartOffset);
			int num2 = Math.Min(endOffset, unclippedEndOffset);
			if (num == num2)
			{
				if (unclippedStartOffset != this._lineMetrics[lineIndex].EndOffset)
				{
					Invariant.Assert(num2 == this._lineMetrics[lineIndex].Offset || num2 == this._lineMetrics[lineIndex].Offset + this._lineMetrics[lineIndex].ContentLength);
					return;
				}
				ITextPointer thisPosition = this._host.TextContainer.CreatePointerAtOffset(unclippedStartOffset, LogicalDirection.Backward);
				if (TextPointerBase.IsNextToPlainLineBreak(thisPosition, LogicalDirection.Backward))
				{
					Rect rect = new Rect(0.0, 0.0, endOfParaGlyphWidth, this._lineHeight);
					CaretElement.AddGeometry(ref geometry, new RectangleGeometry(rect));
					return;
				}
			}
			else
			{
				IList<Rect> rangeBounds = line.GetRangeBounds(num, num2 - num, 0.0, 0.0);
				for (int i = 0; i < rangeBounds.Count; i++)
				{
					Rect rect2 = rangeBounds[i];
					CaretElement.AddGeometry(ref geometry, new RectangleGeometry(rect2));
				}
				if (unclippedEndOffset >= this._lineMetrics[lineIndex].EndOffset)
				{
					ITextPointer thisPosition2 = this._host.TextContainer.CreatePointerAtOffset(num2, LogicalDirection.Backward);
					if (TextPointerBase.IsNextToPlainLineBreak(thisPosition2, LogicalDirection.Backward))
					{
						double contentOffset = this.GetContentOffset(this._lineMetrics[lineIndex].Width, alignment);
						Rect rect3 = new Rect(contentOffset + this._lineMetrics[lineIndex].Width, 0.0, endOfParaGlyphWidth, this._lineHeight);
						CaretElement.AddGeometry(ref geometry, new RectangleGeometry(rect3));
					}
				}
			}
		}

		// Token: 0x060041B7 RID: 16823 RVA: 0x0012CD2C File Offset: 0x0012AF2C
		private void GetVisibleLines(out int firstLineIndex, out int lastLineIndex)
		{
			Rect viewport = this.Viewport;
			if (!viewport.IsEmpty)
			{
				firstLineIndex = (int)(viewport.Y / this._lineHeight);
				lastLineIndex = (int)Math.Ceiling((viewport.Y + viewport.Height) / this._lineHeight) - 1;
				firstLineIndex = Math.Max(0, Math.Min(firstLineIndex, this._lineMetrics.Count - 1));
				lastLineIndex = Math.Max(0, Math.Min(lastLineIndex, this._lineMetrics.Count - 1));
				return;
			}
			firstLineIndex = 0;
			lastLineIndex = this._lineMetrics.Count - 1;
		}

		// Token: 0x060041B8 RID: 16824 RVA: 0x0012CDC8 File Offset: 0x0012AFC8
		private Size FullMeasureTick(double constraintWidth, LineProperties lineProperties)
		{
			TextBoxLine textBoxLine = new TextBoxLine(this);
			Size result;
			int num;
			if (this._lineMetrics.Count == 0)
			{
				result = default(Size);
				num = 0;
			}
			else
			{
				result = this._contentSize;
				num = this._lineMetrics[this._lineMetrics.Count - 1].EndOffset;
			}
			DateTime t;
			if ((ScrollBarVisibility)((Control)this._host).GetValue(ScrollViewer.VerticalScrollBarVisibilityProperty) == ScrollBarVisibility.Auto)
			{
				t = DateTime.MaxValue;
			}
			else
			{
				t = DateTime.Now.AddMilliseconds(200.0);
			}
			bool endOfParagraph;
			do
			{
				using (textBoxLine)
				{
					textBoxLine.Format(num, constraintWidth, constraintWidth, lineProperties, this._cache.TextRunCache, this._cache.TextFormatter);
					this._lineHeight = lineProperties.CalcLineAdvance(textBoxLine.Height);
					this._lineMetrics.Add(new TextBoxView.LineRecord(num, textBoxLine));
					result.Width = Math.Max(result.Width, textBoxLine.Width);
					result.Height += this._lineHeight;
					num += textBoxLine.Length;
					endOfParagraph = textBoxLine.EndOfParagraph;
				}
			}
			while (!endOfParagraph && DateTime.Now < t);
			if (!endOfParagraph)
			{
				this.SetFlags(true, TextBoxView.Flags.BackgroundLayoutPending);
				base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.OnBackgroundMeasure), null);
			}
			else
			{
				this.SetFlags(false, TextBoxView.Flags.BackgroundLayoutPending);
			}
			return result;
		}

		// Token: 0x060041B9 RID: 16825 RVA: 0x0012CF40 File Offset: 0x0012B140
		private object OnBackgroundMeasure(object o)
		{
			if (this._throttleBackgroundTimer == null)
			{
				base.InvalidateMeasure();
			}
			return null;
		}

		// Token: 0x060041BA RID: 16826 RVA: 0x0012CF54 File Offset: 0x0012B154
		private Size IncrementalMeasure(double constraintWidth, LineProperties lineProperties)
		{
			Invariant.Assert(this._dirtyList != null);
			Invariant.Assert(this._dirtyList.Length > 0);
			Size contentSize = this._contentSize;
			DirtyTextRange range = this._dirtyList[0];
			if (range.StartIndex > this._lineMetrics[this._lineMetrics.Count - 1].EndOffset)
			{
				Invariant.Assert(this.IsBackgroundLayoutPending);
				return contentSize;
			}
			int startIndex = range.StartIndex;
			int num = range.PositionsAdded;
			int num2 = range.PositionsRemoved;
			for (int i = 1; i < this._dirtyList.Length; i++)
			{
				range = this._dirtyList[i];
				if (range.StartIndex > this._lineMetrics[this._lineMetrics.Count - 1].EndOffset)
				{
					Invariant.Assert(this.IsBackgroundLayoutPending);
					break;
				}
				int num3 = range.StartIndex - startIndex;
				num += num3 + range.PositionsAdded;
				num2 += num3 + range.PositionsRemoved;
				startIndex = range.StartIndex;
			}
			range = new DirtyTextRange(this._dirtyList[0].StartIndex, num, num2, false);
			if (range.PositionsAdded >= range.PositionsRemoved)
			{
				this.IncrementalMeasureLinesAfterInsert(constraintWidth, lineProperties, range, ref contentSize);
			}
			else if (range.PositionsAdded < range.PositionsRemoved)
			{
				this.IncrementalMeasureLinesAfterDelete(constraintWidth, lineProperties, range, ref contentSize);
			}
			return contentSize;
		}

		// Token: 0x060041BB RID: 16827 RVA: 0x0012D0C8 File Offset: 0x0012B2C8
		private void IncrementalMeasureLinesAfterInsert(double constraintWidth, LineProperties lineProperties, DirtyTextRange range, ref Size desiredSize)
		{
			int num = range.PositionsAdded - range.PositionsRemoved;
			Invariant.Assert(num >= 0);
			int num2 = this.GetLineIndexFromOffset(range.StartIndex, LogicalDirection.Forward);
			if (num > 0)
			{
				for (int i = num2 + 1; i < this._lineMetrics.Count; i++)
				{
					this._lineMetrics[i].Offset += num;
				}
			}
			TextBoxLine textBoxLine = new TextBoxLine(this);
			bool flag = false;
			int num3;
			if (num2 > 0)
			{
				this.FormatFirstIncrementalLine(num2 - 1, constraintWidth, lineProperties, textBoxLine, out num3, out flag);
			}
			else
			{
				num3 = this._lineMetrics[num2].Offset;
			}
			if (!flag)
			{
				using (textBoxLine)
				{
					textBoxLine.Format(num3, constraintWidth, constraintWidth, lineProperties, this._cache.TextRunCache, this._cache.TextFormatter);
					this._lineMetrics[num2] = new TextBoxView.LineRecord(num3, textBoxLine);
					num3 += textBoxLine.Length;
					flag = textBoxLine.EndOfParagraph;
				}
				this.ClearLineVisual(num2);
				num2++;
			}
			this.SyncLineMetrics(range, constraintWidth, lineProperties, textBoxLine, flag, num2, num3);
			desiredSize = this.BruteForceCalculateDesiredSize();
		}

		// Token: 0x060041BC RID: 16828 RVA: 0x0012D1FC File Offset: 0x0012B3FC
		private void IncrementalMeasureLinesAfterDelete(double constraintWidth, LineProperties lineProperties, DirtyTextRange range, ref Size desiredSize)
		{
			int num = range.PositionsAdded - range.PositionsRemoved;
			Invariant.Assert(num < 0);
			int lineIndexFromOffset = this.GetLineIndexFromOffset(range.StartIndex);
			int num2 = range.StartIndex + -num - 1;
			if (num2 > this._lineMetrics[this._lineMetrics.Count - 1].EndOffset)
			{
				Invariant.Assert(this.IsBackgroundLayoutPending);
				num2 = this._lineMetrics[this._lineMetrics.Count - 1].EndOffset;
				if (range.StartIndex == num2)
				{
					return;
				}
			}
			int lineIndexFromOffset2 = this.GetLineIndexFromOffset(num2);
			for (int i = lineIndexFromOffset2 + 1; i < this._lineMetrics.Count; i++)
			{
				this._lineMetrics[i].Offset += num;
			}
			TextBoxLine textBoxLine = new TextBoxLine(this);
			int num3 = lineIndexFromOffset;
			int num4;
			bool flag;
			if (num3 > 0)
			{
				this.FormatFirstIncrementalLine(num3 - 1, constraintWidth, lineProperties, textBoxLine, out num4, out flag);
			}
			else
			{
				num4 = this._lineMetrics[num3].Offset;
				flag = false;
			}
			if (!flag && (range.StartIndex > num4 || range.StartIndex + -num < this._lineMetrics[num3].EndOffset))
			{
				using (textBoxLine)
				{
					textBoxLine.Format(num4, constraintWidth, constraintWidth, lineProperties, this._cache.TextRunCache, this._cache.TextFormatter);
					this._lineMetrics[num3] = new TextBoxView.LineRecord(num4, textBoxLine);
					num4 += textBoxLine.Length;
					flag = textBoxLine.EndOfParagraph;
				}
				this.ClearLineVisual(num3);
				num3++;
			}
			this._lineMetrics.RemoveRange(num3, lineIndexFromOffset2 - num3 + 1);
			this.RemoveLineVisualRange(num3, lineIndexFromOffset2 - num3 + 1);
			this.SyncLineMetrics(range, constraintWidth, lineProperties, textBoxLine, flag, num3, num4);
			desiredSize = this.BruteForceCalculateDesiredSize();
		}

		// Token: 0x060041BD RID: 16829 RVA: 0x0012D3F8 File Offset: 0x0012B5F8
		private void FormatFirstIncrementalLine(int lineIndex, double constraintWidth, LineProperties lineProperties, TextBoxLine line, out int lineOffset, out bool endOfParagraph)
		{
			int endOffset = this._lineMetrics[lineIndex].EndOffset;
			lineOffset = this._lineMetrics[lineIndex].Offset;
			try
			{
				line.Format(lineOffset, constraintWidth, constraintWidth, lineProperties, this._cache.TextRunCache, this._cache.TextFormatter);
				this._lineMetrics[lineIndex] = new TextBoxView.LineRecord(lineOffset, line);
				lineOffset += line.Length;
				endOfParagraph = line.EndOfParagraph;
			}
			finally
			{
				if (line != null)
				{
					((IDisposable)line).Dispose();
				}
			}
			if (endOffset != this._lineMetrics[lineIndex].EndOffset)
			{
				this.ClearLineVisual(lineIndex);
			}
		}

		// Token: 0x060041BE RID: 16830 RVA: 0x0012D4B8 File Offset: 0x0012B6B8
		private void SyncLineMetrics(DirtyTextRange range, double constraintWidth, LineProperties lineProperties, TextBoxLine line, bool endOfParagraph, int lineIndex, int lineOffset)
		{
			bool flag = range.PositionsAdded == 0 || range.PositionsRemoved == 0;
			int num = range.StartIndex + Math.Max(range.PositionsAdded, range.PositionsRemoved);
			while (!endOfParagraph && (lineIndex == this._lineMetrics.Count || !flag || lineOffset != this._lineMetrics[lineIndex].Offset))
			{
				if (lineIndex < this._lineMetrics.Count && lineOffset >= this._lineMetrics[lineIndex].EndOffset)
				{
					this._lineMetrics.RemoveAt(lineIndex);
					this.RemoveLineVisualRange(lineIndex, 1);
				}
				else
				{
					try
					{
						line.Format(lineOffset, constraintWidth, constraintWidth, lineProperties, this._cache.TextRunCache, this._cache.TextFormatter);
						TextBoxView.LineRecord lineRecord = new TextBoxView.LineRecord(lineOffset, line);
						if (lineIndex == this._lineMetrics.Count || lineOffset + line.Length <= this._lineMetrics[lineIndex].Offset)
						{
							this._lineMetrics.Insert(lineIndex, lineRecord);
							this.AddLineVisualPlaceholder(lineIndex);
						}
						else
						{
							Invariant.Assert(lineOffset < this._lineMetrics[lineIndex].EndOffset);
							TextBoxView.LineRecord lineRecord2 = this._lineMetrics[lineIndex];
							if (range.FromHighlightLayer && lineRecord2.Offset > num && lineRecord2.ContentLength == lineRecord.ContentLength && lineRecord2.EndOffset == lineRecord.EndOffset && lineRecord2.Length == lineRecord.Length && lineRecord2.Offset == lineRecord.Offset && DoubleUtilities.AreClose(lineRecord2.Width, lineRecord.Width))
							{
								break;
							}
							this._lineMetrics[lineIndex] = lineRecord;
							this.ClearLineVisual(lineIndex);
							flag |= (num <= lineRecord.EndOffset && line.HasLineBreak);
						}
						lineIndex++;
						lineOffset += line.Length;
						endOfParagraph = line.EndOfParagraph;
					}
					finally
					{
						if (line != null)
						{
							((IDisposable)line).Dispose();
						}
					}
				}
			}
			if (endOfParagraph && lineIndex < this._lineMetrics.Count)
			{
				int count = this._lineMetrics.Count - lineIndex;
				this._lineMetrics.RemoveRange(lineIndex, count);
				this.RemoveLineVisualRange(lineIndex, count);
			}
		}

		// Token: 0x060041BF RID: 16831 RVA: 0x0012D728 File Offset: 0x0012B928
		private Size BruteForceCalculateDesiredSize()
		{
			Size result = default(Size);
			for (int i = 0; i < this._lineMetrics.Count; i++)
			{
				result.Width = Math.Max(result.Width, this._lineMetrics[i].Width);
			}
			result.Height = (double)this._lineMetrics.Count * this._lineHeight;
			return result;
		}

		// Token: 0x060041C0 RID: 16832 RVA: 0x0012D794 File Offset: 0x0012B994
		private void SetViewportLines(int firstLineIndex, int lastLineIndex)
		{
			List<TextBoxLineDrawingVisual> viewportLineVisuals = this._viewportLineVisuals;
			int viewportLineVisualsIndex = this._viewportLineVisualsIndex;
			this._viewportLineVisuals = null;
			this._viewportLineVisualsIndex = -1;
			int num = lastLineIndex - firstLineIndex + 1;
			if (num <= 1)
			{
				this.ClearVisualChildren();
				return;
			}
			this._viewportLineVisuals = new List<TextBoxLineDrawingVisual>(num);
			this._viewportLineVisuals.AddRange(new TextBoxLineDrawingVisual[num]);
			this._viewportLineVisualsIndex = firstLineIndex;
			if (viewportLineVisuals == null)
			{
				this.ClearVisualChildren();
				return;
			}
			int num2 = viewportLineVisualsIndex + viewportLineVisuals.Count - 1;
			if (viewportLineVisualsIndex <= lastLineIndex && num2 >= firstLineIndex)
			{
				int num3 = Math.Max(viewportLineVisualsIndex, firstLineIndex);
				int num4 = Math.Min(num2, firstLineIndex + num - 1) - num3 + 1;
				for (int i = 0; i < num4; i++)
				{
					this._viewportLineVisuals[num3 - this._viewportLineVisualsIndex + i] = viewportLineVisuals[num3 - viewportLineVisualsIndex + i];
				}
				for (int j = 0; j < num3 - viewportLineVisualsIndex; j++)
				{
					if (viewportLineVisuals[j] != null)
					{
						viewportLineVisuals[j].DiscardOnArrange = true;
					}
				}
				for (int k = num3 - viewportLineVisualsIndex + num4; k < viewportLineVisuals.Count; k++)
				{
					if (viewportLineVisuals[k] != null)
					{
						viewportLineVisuals[k].DiscardOnArrange = true;
					}
				}
				return;
			}
			this.ClearVisualChildren();
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x0012D8CC File Offset: 0x0012BACC
		private TextBoxLineDrawingVisual GetLineVisual(int lineIndex)
		{
			TextBoxLineDrawingVisual result = null;
			if (this._viewportLineVisuals != null)
			{
				result = this._viewportLineVisuals[lineIndex - this._viewportLineVisualsIndex];
			}
			return result;
		}

		// Token: 0x060041C2 RID: 16834 RVA: 0x0012D8F8 File Offset: 0x0012BAF8
		private void SetLineVisual(int lineIndex, TextBoxLineDrawingVisual lineVisual)
		{
			if (this._viewportLineVisuals != null)
			{
				this._viewportLineVisuals[lineIndex - this._viewportLineVisualsIndex] = lineVisual;
			}
		}

		// Token: 0x060041C3 RID: 16835 RVA: 0x0012D916 File Offset: 0x0012BB16
		private void AddLineVisualPlaceholder(int lineIndex)
		{
			if (this._viewportLineVisuals != null && lineIndex >= this._viewportLineVisualsIndex && lineIndex < this._viewportLineVisualsIndex + this._viewportLineVisuals.Count)
			{
				this._viewportLineVisuals.Insert(lineIndex - this._viewportLineVisualsIndex, null);
			}
		}

		// Token: 0x060041C4 RID: 16836 RVA: 0x0012D954 File Offset: 0x0012BB54
		private void ClearLineVisual(int lineIndex)
		{
			if (this._viewportLineVisuals != null && lineIndex >= this._viewportLineVisualsIndex && lineIndex < this._viewportLineVisualsIndex + this._viewportLineVisuals.Count && this._viewportLineVisuals[lineIndex - this._viewportLineVisualsIndex] != null)
			{
				this._viewportLineVisuals[lineIndex - this._viewportLineVisualsIndex].DiscardOnArrange = true;
				this._viewportLineVisuals[lineIndex - this._viewportLineVisualsIndex] = null;
			}
		}

		// Token: 0x060041C5 RID: 16837 RVA: 0x0012D9CC File Offset: 0x0012BBCC
		private void RemoveLineVisualRange(int lineIndex, int count)
		{
			if (this._viewportLineVisuals != null)
			{
				if (lineIndex < this._viewportLineVisualsIndex)
				{
					count -= this._viewportLineVisualsIndex - lineIndex;
					count = Math.Max(0, count);
					lineIndex = this._viewportLineVisualsIndex;
				}
				if (lineIndex < this._viewportLineVisualsIndex + this._viewportLineVisuals.Count)
				{
					int num = lineIndex - this._viewportLineVisualsIndex;
					count = Math.Min(count, this._viewportLineVisuals.Count - num);
					for (int i = 0; i < count; i++)
					{
						if (this._viewportLineVisuals[num + i] != null)
						{
							this._viewportLineVisuals[num + i].DiscardOnArrange = true;
						}
					}
					this._viewportLineVisuals.RemoveRange(num, count);
				}
			}
		}

		// Token: 0x060041C6 RID: 16838 RVA: 0x0012DA7A File Offset: 0x0012BC7A
		private void OnThrottleBackgroundTimeout(object sender, EventArgs e)
		{
			this._throttleBackgroundTimer.Stop();
			this._throttleBackgroundTimer = null;
			if (this.IsBackgroundLayoutPending)
			{
				this.OnBackgroundMeasure(null);
			}
		}

		// Token: 0x060041C7 RID: 16839 RVA: 0x0012DAA0 File Offset: 0x0012BCA0
		private double GetContentOffset(double lineWidth, TextAlignment aligment)
		{
			double wrappingWidth = this.GetWrappingWidth(base.RenderSize.Width);
			double result;
			if (aligment != TextAlignment.Right)
			{
				if (aligment != TextAlignment.Center)
				{
					result = 0.0;
				}
				else
				{
					result = (wrappingWidth - lineWidth) / 2.0;
				}
			}
			else
			{
				result = wrappingWidth - lineWidth;
			}
			return result;
		}

		// Token: 0x060041C8 RID: 16840 RVA: 0x0012DAF0 File Offset: 0x0012BCF0
		private TextAlignment HorizontalAlignmentToTextAlignment(HorizontalAlignment horizontalAlignment)
		{
			TextAlignment result;
			switch (horizontalAlignment)
			{
			default:
				result = TextAlignment.Left;
				break;
			case HorizontalAlignment.Center:
				result = TextAlignment.Center;
				break;
			case HorizontalAlignment.Right:
				result = TextAlignment.Right;
				break;
			case HorizontalAlignment.Stretch:
				result = TextAlignment.Justify;
				break;
			}
			return result;
		}

		// Token: 0x060041C9 RID: 16841 RVA: 0x0012DB24 File Offset: 0x0012BD24
		private bool Contains(ITextPointer position)
		{
			Invariant.Assert(this.IsLayoutValid);
			return position.TextContainer == this._host.TextContainer && this._lineMetrics != null && this._lineMetrics[this._lineMetrics.Count - 1].EndOffset >= position.Offset;
		}

		// Token: 0x060041CA RID: 16842 RVA: 0x0012DB81 File Offset: 0x0012BD81
		private double GetWrappingWidth(double width)
		{
			if (width < this._contentSize.Width)
			{
				width = this._contentSize.Width;
			}
			if (width > this._previousConstraint.Width)
			{
				width = this._previousConstraint.Width;
			}
			TextDpi.EnsureValidLineWidth(ref width);
			return width;
		}

		// Token: 0x060041CB RID: 16843 RVA: 0x0012DBC4 File Offset: 0x0012BDC4
		private double GetTextAlignmentCorrection(TextAlignment textAlignment, double width)
		{
			double result = 0.0;
			if (textAlignment != TextAlignment.Left && this._contentSize.Width > width)
			{
				result = -this.GetContentOffset(this._contentSize.Width, textAlignment);
			}
			return result;
		}

		// Token: 0x060041CC RID: 16844 RVA: 0x0012DC04 File Offset: 0x0012BE04
		private DirtyTextRange? GetSelectionRenderRange(DirtyTextRange selectionRange)
		{
			DirtyTextRange? result = null;
			int index;
			int index2;
			this.GetVisibleLines(out index, out index2);
			int startIndex = selectionRange.StartIndex;
			int num = selectionRange.StartIndex + selectionRange.PositionsAdded;
			int offset = this._lineMetrics[index].Offset;
			int endOffset = this._lineMetrics[index2].EndOffset;
			if (endOffset >= startIndex && offset <= num)
			{
				int num2 = Math.Max(offset, startIndex);
				int num3 = Math.Min(endOffset, num) - num2;
				result = new DirtyTextRange?(new DirtyTextRange(num2, num3, num3, true));
			}
			return result;
		}

		// Token: 0x1700101C RID: 4124
		// (get) Token: 0x060041CD RID: 16845 RVA: 0x0012DC99 File Offset: 0x0012BE99
		private bool IsLayoutValid
		{
			get
			{
				return base.IsMeasureValid && base.IsArrangeValid;
			}
		}

		// Token: 0x1700101D RID: 4125
		// (get) Token: 0x060041CE RID: 16846 RVA: 0x0012DCAC File Offset: 0x0012BEAC
		private Rect Viewport
		{
			get
			{
				if (this._scrollData != null)
				{
					return new Rect(this._scrollData.HorizontalOffset, this._scrollData.VerticalOffset, this._scrollData.ViewportWidth, this._scrollData.ViewportHeight);
				}
				return Rect.Empty;
			}
		}

		// Token: 0x1700101E RID: 4126
		// (get) Token: 0x060041CF RID: 16847 RVA: 0x0012DCF8 File Offset: 0x0012BEF8
		private bool IsBackgroundLayoutPending
		{
			get
			{
				return this.CheckFlags(TextBoxView.Flags.BackgroundLayoutPending);
			}
		}

		// Token: 0x1700101F RID: 4127
		// (get) Token: 0x060041D0 RID: 16848 RVA: 0x0012DD04 File Offset: 0x0012BF04
		private double VerticalAlignmentOffset
		{
			get
			{
				double result;
				switch (((Control)this._host).VerticalContentAlignment)
				{
				default:
					result = 0.0;
					break;
				case VerticalAlignment.Center:
					result = this.VerticalPadding / 2.0;
					break;
				case VerticalAlignment.Bottom:
					result = this.VerticalPadding;
					break;
				}
				return result;
			}
		}

		// Token: 0x17001020 RID: 4128
		// (get) Token: 0x060041D1 RID: 16849 RVA: 0x0012DD60 File Offset: 0x0012BF60
		private TextAlignment CalculatedTextAlignment
		{
			get
			{
				Control control = (Control)this._host;
				BaseValueSource baseValueSource = DependencyPropertyHelper.GetValueSource(control, TextBox.TextAlignmentProperty).BaseValueSource;
				BaseValueSource baseValueSource2 = DependencyPropertyHelper.GetValueSource(control, Control.HorizontalContentAlignmentProperty).BaseValueSource;
				if (baseValueSource == BaseValueSource.Local)
				{
					return (TextAlignment)control.GetValue(TextBox.TextAlignmentProperty);
				}
				if (baseValueSource2 == BaseValueSource.Local)
				{
					object value = control.GetValue(Control.HorizontalContentAlignmentProperty);
					return this.HorizontalAlignmentToTextAlignment((HorizontalAlignment)value);
				}
				if (baseValueSource == BaseValueSource.Default && baseValueSource2 != BaseValueSource.Default)
				{
					object value = control.GetValue(Control.HorizontalContentAlignmentProperty);
					return this.HorizontalAlignmentToTextAlignment((HorizontalAlignment)value);
				}
				return (TextAlignment)control.GetValue(TextBox.TextAlignmentProperty);
			}
		}

		// Token: 0x17001021 RID: 4129
		// (get) Token: 0x060041D2 RID: 16850 RVA: 0x0012DE0C File Offset: 0x0012C00C
		private double VerticalPadding
		{
			get
			{
				Rect viewport = this.Viewport;
				double result;
				if (viewport.IsEmpty)
				{
					result = 0.0;
				}
				else
				{
					result = Math.Max(0.0, viewport.Height - this._contentSize.Height);
				}
				return result;
			}
		}

		// Token: 0x040027A5 RID: 10149
		private readonly ITextBoxViewHost _host;

		// Token: 0x040027A6 RID: 10150
		private Size _contentSize;

		// Token: 0x040027A7 RID: 10151
		private Size _previousConstraint;

		// Token: 0x040027A8 RID: 10152
		private TextBoxView.TextCache _cache;

		// Token: 0x040027A9 RID: 10153
		private double _lineHeight;

		// Token: 0x040027AA RID: 10154
		private List<TextBoxLineDrawingVisual> _visualChildren;

		// Token: 0x040027AB RID: 10155
		private List<TextBoxView.LineRecord> _lineMetrics;

		// Token: 0x040027AC RID: 10156
		private List<TextBoxLineDrawingVisual> _viewportLineVisuals;

		// Token: 0x040027AD RID: 10157
		private int _viewportLineVisualsIndex;

		// Token: 0x040027AE RID: 10158
		private ScrollData _scrollData;

		// Token: 0x040027AF RID: 10159
		private DtrList _dirtyList;

		// Token: 0x040027B0 RID: 10160
		private DispatcherTimer _throttleBackgroundTimer;

		// Token: 0x040027B1 RID: 10161
		private TextBoxView.Flags _flags;

		// Token: 0x040027B2 RID: 10162
		private EventHandler UpdatedEvent;

		// Token: 0x040027B3 RID: 10163
		private const uint _maxMeasureTimeMs = 200U;

		// Token: 0x040027B4 RID: 10164
		private const int _throttleBackgroundSeconds = 2;

		// Token: 0x0200095A RID: 2394
		[Flags]
		private enum Flags
		{
			// Token: 0x040043F4 RID: 17396
			TextContainerListenersInitialized = 1,
			// Token: 0x040043F5 RID: 17397
			BackgroundLayoutPending = 2,
			// Token: 0x040043F6 RID: 17398
			ArrangePendingFromHighlightLayer = 4
		}

		// Token: 0x0200095B RID: 2395
		private class TextCache
		{
			// Token: 0x06008725 RID: 34597 RVA: 0x0024EF94 File Offset: 0x0024D194
			internal TextCache(TextBoxView owner)
			{
				this._lineProperties = owner.GetLineProperties();
				this._textRunCache = new TextRunCache();
				Control element = (Control)owner.Host;
				TextFormattingMode textFormattingMode = TextOptions.GetTextFormattingMode(element);
				this._textFormatter = TextFormatter.FromCurrentDispatcher(textFormattingMode);
			}

			// Token: 0x17001E86 RID: 7814
			// (get) Token: 0x06008726 RID: 34598 RVA: 0x0024EFDD File Offset: 0x0024D1DD
			internal LineProperties LineProperties
			{
				get
				{
					return this._lineProperties;
				}
			}

			// Token: 0x17001E87 RID: 7815
			// (get) Token: 0x06008727 RID: 34599 RVA: 0x0024EFE5 File Offset: 0x0024D1E5
			internal TextRunCache TextRunCache
			{
				get
				{
					return this._textRunCache;
				}
			}

			// Token: 0x17001E88 RID: 7816
			// (get) Token: 0x06008728 RID: 34600 RVA: 0x0024EFED File Offset: 0x0024D1ED
			internal TextFormatter TextFormatter
			{
				get
				{
					return this._textFormatter;
				}
			}

			// Token: 0x040043F7 RID: 17399
			private readonly LineProperties _lineProperties;

			// Token: 0x040043F8 RID: 17400
			private readonly TextRunCache _textRunCache;

			// Token: 0x040043F9 RID: 17401
			private TextFormatter _textFormatter;
		}

		// Token: 0x0200095C RID: 2396
		private class LineRecord
		{
			// Token: 0x06008729 RID: 34601 RVA: 0x0024EFF5 File Offset: 0x0024D1F5
			internal LineRecord(int offset, TextBoxLine line)
			{
				this._offset = offset;
				this._length = line.Length;
				this._contentLength = line.ContentLength;
				this._width = line.Width;
			}

			// Token: 0x17001E89 RID: 7817
			// (get) Token: 0x0600872A RID: 34602 RVA: 0x0024F028 File Offset: 0x0024D228
			// (set) Token: 0x0600872B RID: 34603 RVA: 0x0024F030 File Offset: 0x0024D230
			internal int Offset
			{
				get
				{
					return this._offset;
				}
				set
				{
					this._offset = value;
				}
			}

			// Token: 0x17001E8A RID: 7818
			// (get) Token: 0x0600872C RID: 34604 RVA: 0x0024F039 File Offset: 0x0024D239
			internal int Length
			{
				get
				{
					return this._length;
				}
			}

			// Token: 0x17001E8B RID: 7819
			// (get) Token: 0x0600872D RID: 34605 RVA: 0x0024F041 File Offset: 0x0024D241
			internal int ContentLength
			{
				get
				{
					return this._contentLength;
				}
			}

			// Token: 0x17001E8C RID: 7820
			// (get) Token: 0x0600872E RID: 34606 RVA: 0x0024F049 File Offset: 0x0024D249
			internal double Width
			{
				get
				{
					return this._width;
				}
			}

			// Token: 0x17001E8D RID: 7821
			// (get) Token: 0x0600872F RID: 34607 RVA: 0x0024F051 File Offset: 0x0024D251
			internal int EndOffset
			{
				get
				{
					return this._offset + this._length;
				}
			}

			// Token: 0x040043FA RID: 17402
			private int _offset;

			// Token: 0x040043FB RID: 17403
			private readonly int _length;

			// Token: 0x040043FC RID: 17404
			private readonly int _contentLength;

			// Token: 0x040043FD RID: 17405
			private readonly double _width;
		}
	}
}
