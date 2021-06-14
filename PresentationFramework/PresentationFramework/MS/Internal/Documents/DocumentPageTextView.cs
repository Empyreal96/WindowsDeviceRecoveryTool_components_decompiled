using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020006C0 RID: 1728
	internal class DocumentPageTextView : TextViewBase
	{
		// Token: 0x06006FA6 RID: 28582 RVA: 0x002018E0 File Offset: 0x001FFAE0
		internal DocumentPageTextView(DocumentPageView owner, ITextContainer textContainer)
		{
			Invariant.Assert(owner != null && textContainer != null);
			this._owner = owner;
			this._page = owner.DocumentPageInternal;
			this._textContainer = textContainer;
			if (this._page is IServiceProvider)
			{
				this._pageTextView = (((IServiceProvider)this._page).GetService(typeof(ITextView)) as ITextView);
			}
			if (this._pageTextView != null)
			{
				this._pageTextView.Updated += this.HandlePageTextViewUpdated;
			}
		}

		// Token: 0x06006FA7 RID: 28583 RVA: 0x00201970 File Offset: 0x001FFB70
		internal DocumentPageTextView(FlowDocumentView owner, ITextContainer textContainer)
		{
			Invariant.Assert(owner != null && textContainer != null);
			this._owner = owner;
			this._page = owner.DocumentPage;
			this._textContainer = textContainer;
			if (this._page is IServiceProvider)
			{
				this._pageTextView = (((IServiceProvider)this._page).GetService(typeof(ITextView)) as ITextView);
			}
			if (this._pageTextView != null)
			{
				this._pageTextView.Updated += this.HandlePageTextViewUpdated;
			}
		}

		// Token: 0x06006FA8 RID: 28584 RVA: 0x002019FD File Offset: 0x001FFBFD
		internal override ITextPointer GetTextPositionFromPoint(Point point, bool snapToText)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this.IsPageMissing)
			{
				return null;
			}
			point = this.TransformToDescendant(point);
			return this._pageTextView.GetTextPositionFromPoint(point, snapToText);
		}

		// Token: 0x06006FA9 RID: 28585 RVA: 0x00201A38 File Offset: 0x001FFC38
		internal override Rect GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform)
		{
			transform = Transform.Identity;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this.IsPageMissing)
			{
				return Rect.Empty;
			}
			Transform transform2;
			Rect rawRectangleFromTextPosition = this._pageTextView.GetRawRectangleFromTextPosition(position, out transform2);
			Invariant.Assert(transform2 != null);
			Transform transformToAncestor = this.GetTransformToAncestor();
			transform = this.GetAggregateTransform(transform2, transformToAncestor);
			return rawRectangleFromTextPosition;
		}

		// Token: 0x06006FAA RID: 28586 RVA: 0x00201A9C File Offset: 0x001FFC9C
		internal override Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			Geometry geometry = null;
			if (!this.IsPageMissing)
			{
				geometry = this._pageTextView.GetTightBoundingGeometryFromTextPositions(startPosition, endPosition);
				if (geometry != null)
				{
					Transform affineTransform = this.GetTransformToAncestor().AffineTransform;
					CaretElement.AddTransformToGeometry(geometry, affineTransform);
				}
			}
			return geometry;
		}

		// Token: 0x06006FAB RID: 28587 RVA: 0x00201AF0 File Offset: 0x001FFCF0
		internal override ITextPointer GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this.IsPageMissing)
			{
				newSuggestedX = suggestedX;
				linesMoved = 0;
				return position;
			}
			suggestedX = this.TransformToDescendant(new Point(suggestedX, 0.0)).X;
			ITextPointer positionAtNextLine = this._pageTextView.GetPositionAtNextLine(position, suggestedX, count, out newSuggestedX, out linesMoved);
			newSuggestedX = this.TransformToAncestor(new Point(newSuggestedX, 0.0)).X;
			return positionAtNextLine;
		}

		// Token: 0x06006FAC RID: 28588 RVA: 0x00201B7C File Offset: 0x001FFD7C
		internal override ITextPointer GetPositionAtNextPage(ITextPointer position, Point suggestedOffset, int count, out Point newSuggestedOffset, out int pagesMoved)
		{
			ITextPointer textPointer = position;
			newSuggestedOffset = suggestedOffset;
			Point point = suggestedOffset;
			pagesMoved = 0;
			if (count == 0)
			{
				return textPointer;
			}
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this.IsPageMissing)
			{
				return position;
			}
			point.Y = this.GetYOffsetAtNextPage(point.Y, count, out pagesMoved);
			if (pagesMoved != 0)
			{
				point = this.TransformToDescendant(point);
				textPointer = this._pageTextView.GetTextPositionFromPoint(point, true);
				Invariant.Assert(textPointer != null);
				Rect rectangleFromTextPosition = this._pageTextView.GetRectangleFromTextPosition(position);
				newSuggestedOffset = this.TransformToAncestor(new Point(rectangleFromTextPosition.X, rectangleFromTextPosition.Y));
			}
			return textPointer;
		}

		// Token: 0x06006FAD RID: 28589 RVA: 0x00201C29 File Offset: 0x001FFE29
		internal override bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			return !this.IsPageMissing && this._pageTextView.IsAtCaretUnitBoundary(position);
		}

		// Token: 0x06006FAE RID: 28590 RVA: 0x00201C59 File Offset: 0x001FFE59
		internal override ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this.IsPageMissing)
			{
				return null;
			}
			return this._pageTextView.GetNextCaretUnitPosition(position, direction);
		}

		// Token: 0x06006FAF RID: 28591 RVA: 0x00201C8A File Offset: 0x001FFE8A
		internal override ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this.IsPageMissing)
			{
				return null;
			}
			return this._pageTextView.GetBackspaceCaretUnitPosition(position);
		}

		// Token: 0x06006FB0 RID: 28592 RVA: 0x00201CBA File Offset: 0x001FFEBA
		internal override TextSegment GetLineRange(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this.IsPageMissing)
			{
				return TextSegment.Null;
			}
			return this._pageTextView.GetLineRange(position);
		}

		// Token: 0x06006FB1 RID: 28593 RVA: 0x00201CEE File Offset: 0x001FFEEE
		internal override ReadOnlyCollection<GlyphRun> GetGlyphRuns(ITextPointer start, ITextPointer end)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this.IsPageMissing)
			{
				return new ReadOnlyCollection<GlyphRun>(new List<GlyphRun>());
			}
			return this._pageTextView.GetGlyphRuns(start, end);
		}

		// Token: 0x06006FB2 RID: 28594 RVA: 0x00201D28 File Offset: 0x001FFF28
		internal override bool Contains(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			return !this.IsPageMissing && this._pageTextView.Contains(position);
		}

		// Token: 0x06006FB3 RID: 28595 RVA: 0x00201D58 File Offset: 0x001FFF58
		internal void OnPageConnected()
		{
			this.OnPageDisconnected();
			if (this._owner is DocumentPageView)
			{
				this._page = ((DocumentPageView)this._owner).DocumentPageInternal;
			}
			else if (this._owner is FlowDocumentView)
			{
				this._page = ((FlowDocumentView)this._owner).DocumentPage;
			}
			if (this._page is IServiceProvider)
			{
				this._pageTextView = (((IServiceProvider)this._page).GetService(typeof(ITextView)) as ITextView);
			}
			if (this._pageTextView != null)
			{
				this._pageTextView.Updated += this.HandlePageTextViewUpdated;
			}
			if (this.IsValid)
			{
				this.OnUpdated(EventArgs.Empty);
			}
		}

		// Token: 0x06006FB4 RID: 28596 RVA: 0x00201E17 File Offset: 0x00200017
		internal void OnPageDisconnected()
		{
			if (this._pageTextView != null)
			{
				this._pageTextView.Updated -= this.HandlePageTextViewUpdated;
			}
			this._pageTextView = null;
			this._page = null;
		}

		// Token: 0x06006FB5 RID: 28597 RVA: 0x00201E46 File Offset: 0x00200046
		internal void OnTransformChanged()
		{
			if (this.IsValid)
			{
				this.OnUpdated(EventArgs.Empty);
			}
		}

		// Token: 0x06006FB6 RID: 28598 RVA: 0x00201E5B File Offset: 0x0020005B
		internal override bool Validate()
		{
			if (!this._owner.IsMeasureValid || !this._owner.IsArrangeValid)
			{
				this._owner.UpdateLayout();
			}
			return this._pageTextView != null && this._pageTextView.Validate();
		}

		// Token: 0x06006FB7 RID: 28599 RVA: 0x00201E98 File Offset: 0x00200098
		internal override bool Validate(ITextPointer position)
		{
			FlowDocumentView flowDocumentView = this._owner as FlowDocumentView;
			bool result;
			if (flowDocumentView == null || flowDocumentView.Document == null)
			{
				result = base.Validate(position);
			}
			else
			{
				if (this.Validate())
				{
					BackgroundFormatInfo backgroundFormatInfo = flowDocumentView.Document.StructuralCache.BackgroundFormatInfo;
					FlowDocumentFormatter bottomlessFormatter = flowDocumentView.Document.BottomlessFormatter;
					int num = -1;
					while (this.IsValid && !this.Contains(position))
					{
						backgroundFormatInfo.BackgroundFormat(bottomlessFormatter, true);
						this._owner.UpdateLayout();
						if (backgroundFormatInfo.CPInterrupted <= num)
						{
							break;
						}
						num = backgroundFormatInfo.CPInterrupted;
					}
				}
				result = (this.IsValid && this.Contains(position));
			}
			return result;
		}

		// Token: 0x06006FB8 RID: 28600 RVA: 0x00201F3C File Offset: 0x0020013C
		internal override void ThrottleBackgroundTasksForUserInput()
		{
			FlowDocumentView flowDocumentView = this._owner as FlowDocumentView;
			if (flowDocumentView != null && flowDocumentView.Document != null)
			{
				flowDocumentView.Document.StructuralCache.ThrottleBackgroundFormatting();
			}
		}

		// Token: 0x17001A84 RID: 6788
		// (get) Token: 0x06006FB9 RID: 28601 RVA: 0x00201F70 File Offset: 0x00200170
		internal override UIElement RenderScope
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17001A85 RID: 6789
		// (get) Token: 0x06006FBA RID: 28602 RVA: 0x00201F78 File Offset: 0x00200178
		internal override ITextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x17001A86 RID: 6790
		// (get) Token: 0x06006FBB RID: 28603 RVA: 0x00201F80 File Offset: 0x00200180
		internal override bool IsValid
		{
			get
			{
				return this._owner.IsMeasureValid && this._owner.IsArrangeValid && this._page != null && (this.IsPageMissing || (this._pageTextView != null && this._pageTextView.IsValid));
			}
		}

		// Token: 0x17001A87 RID: 6791
		// (get) Token: 0x06006FBC RID: 28604 RVA: 0x00201FD0 File Offset: 0x002001D0
		internal override bool RendersOwnSelection
		{
			get
			{
				return this._pageTextView != null && this._pageTextView.RendersOwnSelection;
			}
		}

		// Token: 0x17001A88 RID: 6792
		// (get) Token: 0x06006FBD RID: 28605 RVA: 0x00201FE7 File Offset: 0x002001E7
		internal override ReadOnlyCollection<TextSegment> TextSegments
		{
			get
			{
				if (!this.IsValid || this.IsPageMissing)
				{
					return new ReadOnlyCollection<TextSegment>(new List<TextSegment>());
				}
				return this._pageTextView.TextSegments;
			}
		}

		// Token: 0x17001A89 RID: 6793
		// (get) Token: 0x06006FBE RID: 28606 RVA: 0x0020200F File Offset: 0x0020020F
		internal DocumentPageView DocumentPageView
		{
			get
			{
				return this._owner as DocumentPageView;
			}
		}

		// Token: 0x06006FBF RID: 28607 RVA: 0x0020201C File Offset: 0x0020021C
		private void HandlePageTextViewUpdated(object sender, EventArgs e)
		{
			Invariant.Assert(this._pageTextView != null);
			if (sender == this._pageTextView)
			{
				this.OnUpdated(EventArgs.Empty);
			}
		}

		// Token: 0x06006FC0 RID: 28608 RVA: 0x00202040 File Offset: 0x00200240
		private Transform GetTransformToAncestor()
		{
			Invariant.Assert(this.IsValid && !this.IsPageMissing);
			Transform transform = this._page.Visual.TransformToAncestor(this._owner) as Transform;
			if (transform == null)
			{
				transform = Transform.Identity;
			}
			return transform;
		}

		// Token: 0x06006FC1 RID: 28609 RVA: 0x0020208C File Offset: 0x0020028C
		private Point TransformToAncestor(Point point)
		{
			Invariant.Assert(this.IsValid && !this.IsPageMissing);
			GeneralTransform generalTransform = this._page.Visual.TransformToAncestor(this._owner);
			if (generalTransform != null)
			{
				point = generalTransform.Transform(point);
			}
			return point;
		}

		// Token: 0x06006FC2 RID: 28610 RVA: 0x002020D8 File Offset: 0x002002D8
		private Point TransformToDescendant(Point point)
		{
			Invariant.Assert(this.IsValid && !this.IsPageMissing);
			GeneralTransform generalTransform = this._page.Visual.TransformToAncestor(this._owner);
			if (generalTransform != null)
			{
				generalTransform = generalTransform.Inverse;
				if (generalTransform != null)
				{
					point = generalTransform.Transform(point);
				}
			}
			return point;
		}

		// Token: 0x06006FC3 RID: 28611 RVA: 0x0020212C File Offset: 0x0020032C
		private double GetYOffsetAtNextPage(double offset, int count, out int pagesMoved)
		{
			double num = offset;
			pagesMoved = 0;
			if (this._owner is IScrollInfo && ((IScrollInfo)this._owner).ScrollOwner != null)
			{
				IScrollInfo scrollInfo = (IScrollInfo)this._owner;
				double viewportHeight = scrollInfo.ViewportHeight;
				double extentHeight = scrollInfo.ExtentHeight;
				if (count > 0)
				{
					while (pagesMoved < count)
					{
						if (!DoubleUtil.LessThanOrClose(offset + viewportHeight, extentHeight))
						{
							break;
						}
						num += viewportHeight;
						pagesMoved++;
					}
				}
				else
				{
					while (Math.Abs(pagesMoved) < Math.Abs(count) && DoubleUtil.GreaterThanOrClose(offset - viewportHeight, 0.0))
					{
						num -= viewportHeight;
						pagesMoved--;
					}
				}
			}
			return num;
		}

		// Token: 0x17001A8A RID: 6794
		// (get) Token: 0x06006FC4 RID: 28612 RVA: 0x002021C6 File Offset: 0x002003C6
		private bool IsPageMissing
		{
			get
			{
				return this._page == DocumentPage.Missing;
			}
		}

		// Token: 0x040036CE RID: 14030
		private readonly UIElement _owner;

		// Token: 0x040036CF RID: 14031
		private readonly ITextContainer _textContainer;

		// Token: 0x040036D0 RID: 14032
		private DocumentPage _page;

		// Token: 0x040036D1 RID: 14033
		private ITextView _pageTextView;
	}
}
