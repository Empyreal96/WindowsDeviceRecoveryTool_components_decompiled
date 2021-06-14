using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace MS.Internal.Documents
{
	// Token: 0x020006D6 RID: 1750
	internal class MultiPageTextView : TextViewBase
	{
		// Token: 0x0600710C RID: 28940 RVA: 0x00204E05 File Offset: 0x00203005
		internal MultiPageTextView(DocumentViewerBase viewer, UIElement renderScope, ITextContainer textContainer)
		{
			this._viewer = viewer;
			this._renderScope = renderScope;
			this._textContainer = textContainer;
			this._pageTextViews = new List<DocumentPageTextView>();
			this.OnPagesUpdatedCore();
		}

		// Token: 0x0600710D RID: 28941 RVA: 0x00204E33 File Offset: 0x00203033
		protected override void OnUpdated(EventArgs e)
		{
			base.OnUpdated(e);
			if (this.IsValid)
			{
				this.OnUpdatedWorker(null);
				return;
			}
			this._renderScope.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.OnUpdatedWorker), EventArgs.Empty);
		}

		// Token: 0x0600710E RID: 28942 RVA: 0x00204E74 File Offset: 0x00203074
		internal override ITextPointer GetTextPositionFromPoint(Point point, bool snapToText)
		{
			ITextPointer result = null;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			DocumentPageTextView textViewFromPoint = this.GetTextViewFromPoint(point, false);
			if (textViewFromPoint != null)
			{
				point = this.TransformToDescendant(textViewFromPoint.RenderScope, point);
				result = textViewFromPoint.GetTextPositionFromPoint(point, snapToText);
			}
			return result;
		}

		// Token: 0x0600710F RID: 28943 RVA: 0x00204EC0 File Offset: 0x002030C0
		internal override Rect GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform)
		{
			Rect result = Rect.Empty;
			transform = Transform.Identity;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			DocumentPageTextView textViewFromPosition = this.GetTextViewFromPosition(position);
			if (textViewFromPosition != null)
			{
				Transform firstTransform;
				result = textViewFromPosition.GetRawRectangleFromTextPosition(position, out firstTransform);
				Transform transformToAncestor = this.GetTransformToAncestor(textViewFromPosition.RenderScope);
				transform = this.GetAggregateTransform(firstTransform, transformToAncestor);
			}
			return result;
		}

		// Token: 0x06007110 RID: 28944 RVA: 0x00204F20 File Offset: 0x00203120
		internal override Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			Geometry result = null;
			int i = 0;
			int count = this._pageTextViews.Count;
			while (i < count)
			{
				ReadOnlyCollection<TextSegment> textSegments = this._pageTextViews[i].TextSegments;
				for (int j = 0; j < textSegments.Count; j++)
				{
					TextSegment textSegment = textSegments[j];
					ITextPointer textPointer = (startPosition.CompareTo(textSegment.Start) > 0) ? startPosition : textSegment.Start;
					ITextPointer textPointer2 = (endPosition.CompareTo(textSegment.End) < 0) ? endPosition : textSegment.End;
					if (textPointer.CompareTo(textPointer2) < 0)
					{
						Geometry tightBoundingGeometryFromTextPositions = this._pageTextViews[i].GetTightBoundingGeometryFromTextPositions(textPointer, textPointer2);
						if (tightBoundingGeometryFromTextPositions != null)
						{
							Transform affineTransform = this._pageTextViews[i].RenderScope.TransformToAncestor(this._renderScope).AffineTransform;
							CaretElement.AddTransformToGeometry(tightBoundingGeometryFromTextPositions, affineTransform);
							CaretElement.AddGeometry(ref result, tightBoundingGeometryFromTextPositions);
						}
					}
				}
				i++;
			}
			return result;
		}

		// Token: 0x06007111 RID: 28945 RVA: 0x00205034 File Offset: 0x00203234
		internal override ITextPointer GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			int num;
			return this.GetPositionAtNextLineCore(position, suggestedX, count, out newSuggestedX, out linesMoved, out num);
		}

		// Token: 0x06007112 RID: 28946 RVA: 0x00205068 File Offset: 0x00203268
		internal override ITextPointer GetPositionAtNextPage(ITextPointer position, Point suggestedOffset, int count, out Point newSuggestedOffset, out int pagesMoved)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			int num;
			return this.GetPositionAtNextPageCore(position, suggestedOffset, count, out newSuggestedOffset, out pagesMoved, out num);
		}

		// Token: 0x06007113 RID: 28947 RVA: 0x0020509C File Offset: 0x0020329C
		internal override bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			bool result = false;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			DocumentPageTextView textViewFromPosition = this.GetTextViewFromPosition(position);
			if (textViewFromPosition != null)
			{
				result = textViewFromPosition.IsAtCaretUnitBoundary(position);
			}
			return result;
		}

		// Token: 0x06007114 RID: 28948 RVA: 0x002050D8 File Offset: 0x002032D8
		internal override ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			ITextPointer result = null;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			DocumentPageTextView textViewFromPosition = this.GetTextViewFromPosition(position);
			if (textViewFromPosition != null)
			{
				result = textViewFromPosition.GetNextCaretUnitPosition(position, direction);
			}
			return result;
		}

		// Token: 0x06007115 RID: 28949 RVA: 0x00205114 File Offset: 0x00203314
		internal override ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			ITextPointer result = null;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			DocumentPageTextView textViewFromPosition = this.GetTextViewFromPosition(position);
			if (textViewFromPosition != null)
			{
				result = textViewFromPosition.GetBackspaceCaretUnitPosition(position);
			}
			return result;
		}

		// Token: 0x06007116 RID: 28950 RVA: 0x00205150 File Offset: 0x00203350
		internal override TextSegment GetLineRange(ITextPointer position)
		{
			TextSegment result = TextSegment.Null;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			DocumentPageTextView textViewFromPosition = this.GetTextViewFromPosition(position);
			if (textViewFromPosition != null)
			{
				result = textViewFromPosition.GetLineRange(position);
			}
			return result;
		}

		// Token: 0x06007117 RID: 28951 RVA: 0x0020518F File Offset: 0x0020338F
		internal override bool Contains(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			return this.GetTextViewFromPosition(position) != null;
		}

		// Token: 0x06007118 RID: 28952 RVA: 0x002051B4 File Offset: 0x002033B4
		internal override void BringPositionIntoViewAsync(ITextPointer position, object userState)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this._pendingRequest != null)
			{
				this.OnBringPositionIntoViewCompleted(new BringPositionIntoViewCompletedEventArgs(position, false, null, false, userState));
			}
			MultiPageTextView.BringPositionIntoViewRequest bringPositionIntoViewRequest = new MultiPageTextView.BringPositionIntoViewRequest(position, userState);
			this._pendingRequest = bringPositionIntoViewRequest;
			DocumentPageTextView textViewFromPosition = this.GetTextViewFromPosition(position);
			if (textViewFromPosition != null)
			{
				bringPositionIntoViewRequest.Succeeded = true;
				this.OnBringPositionIntoViewCompleted(bringPositionIntoViewRequest);
				return;
			}
			if (!(position is ContentPosition))
			{
				this.OnBringPositionIntoViewCompleted(bringPositionIntoViewRequest);
				return;
			}
			DynamicDocumentPaginator dynamicDocumentPaginator = this._viewer.Document.DocumentPaginator as DynamicDocumentPaginator;
			if (dynamicDocumentPaginator == null)
			{
				this.OnBringPositionIntoViewCompleted(bringPositionIntoViewRequest);
				return;
			}
			int pageNumber = dynamicDocumentPaginator.GetPageNumber((ContentPosition)position) + 1;
			if (this._viewer.CanGoToPage(pageNumber))
			{
				this._viewer.GoToPage(pageNumber);
				return;
			}
			this.OnBringPositionIntoViewCompleted(bringPositionIntoViewRequest);
		}

		// Token: 0x06007119 RID: 28953 RVA: 0x0020527C File Offset: 0x0020347C
		internal override void BringPointIntoViewAsync(Point point, object userState)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this._pendingRequest != null)
			{
				this.OnBringPointIntoViewCompleted(new BringPointIntoViewCompletedEventArgs(point, null, false, null, false, userState));
				return;
			}
			MultiPageTextView.BringPointIntoViewRequest bringPointIntoViewRequest = new MultiPageTextView.BringPointIntoViewRequest(point, userState);
			this._pendingRequest = bringPointIntoViewRequest;
			DocumentPageTextView textViewFromPoint = this.GetTextViewFromPoint(point, false);
			if (textViewFromPoint != null)
			{
				point = this.TransformToDescendant(textViewFromPoint.RenderScope, point);
				ITextPointer textPositionFromPoint = textViewFromPoint.GetTextPositionFromPoint(point, true);
				bringPointIntoViewRequest.Position = textPositionFromPoint;
				this.OnBringPointIntoViewCompleted(bringPointIntoViewRequest);
				return;
			}
			GeneralTransform generalTransform = this._renderScope.TransformToAncestor(this._viewer);
			generalTransform.TryTransform(point, out point);
			bool flag = false;
			if (this._viewer is FlowDocumentPageViewer)
			{
				flag = ((FlowDocumentPageViewer)this._viewer).BringPointIntoView(point);
			}
			else if (this._viewer is DocumentViewer)
			{
				flag = ((DocumentViewer)this._viewer).BringPointIntoView(point);
			}
			else if (DoubleUtil.LessThan(point.X, 0.0))
			{
				if (this._viewer.CanGoToPreviousPage)
				{
					this._viewer.PreviousPage();
					flag = true;
				}
			}
			else if (DoubleUtil.GreaterThan(point.X, this._viewer.RenderSize.Width))
			{
				if (this._viewer.CanGoToNextPage)
				{
					this._viewer.NextPage();
					flag = true;
				}
			}
			else if (DoubleUtil.LessThan(point.Y, 0.0))
			{
				if (this._viewer.CanGoToPreviousPage)
				{
					this._viewer.PreviousPage();
					flag = true;
				}
			}
			else if (DoubleUtil.GreaterThan(point.Y, this._viewer.RenderSize.Height) && this._viewer.CanGoToNextPage)
			{
				this._viewer.NextPage();
				flag = true;
			}
			if (!flag)
			{
				this.OnBringPointIntoViewCompleted(bringPointIntoViewRequest);
			}
		}

		// Token: 0x0600711A RID: 28954 RVA: 0x00205458 File Offset: 0x00203658
		internal override void BringLineIntoViewAsync(ITextPointer position, double suggestedX, int count, object userState)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this._pendingRequest != null)
			{
				this.OnBringLineIntoViewCompleted(new BringLineIntoViewCompletedEventArgs(position, suggestedX, count, position, suggestedX, 0, false, null, false, userState));
				return;
			}
			this._pendingRequest = new MultiPageTextView.BringLineIntoViewRequest(position, suggestedX, count, userState);
			this.BringLineIntoViewCore((MultiPageTextView.BringLineIntoViewRequest)this._pendingRequest);
		}

		// Token: 0x0600711B RID: 28955 RVA: 0x002054C0 File Offset: 0x002036C0
		internal override void BringPageIntoViewAsync(ITextPointer position, Point suggestedOffset, int count, object userState)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this._pendingRequest != null)
			{
				this.OnBringPageIntoViewCompleted(new BringPageIntoViewCompletedEventArgs(position, suggestedOffset, count, position, suggestedOffset, 0, false, null, false, userState));
				return;
			}
			this._pendingRequest = new MultiPageTextView.BringPageIntoViewRequest(position, suggestedOffset, count, userState);
			this.BringPageIntoViewCore((MultiPageTextView.BringPageIntoViewRequest)this._pendingRequest);
		}

		// Token: 0x0600711C RID: 28956 RVA: 0x00205528 File Offset: 0x00203728
		internal override void CancelAsync(object userState)
		{
			if (this._pendingRequest != null)
			{
				if (this._pendingRequest is MultiPageTextView.BringLineIntoViewRequest)
				{
					MultiPageTextView.BringLineIntoViewRequest bringLineIntoViewRequest = (MultiPageTextView.BringLineIntoViewRequest)this._pendingRequest;
					this.OnBringLineIntoViewCompleted(new BringLineIntoViewCompletedEventArgs(bringLineIntoViewRequest.Position, bringLineIntoViewRequest.SuggestedX, bringLineIntoViewRequest.Count, bringLineIntoViewRequest.NewPosition, bringLineIntoViewRequest.NewSuggestedX, bringLineIntoViewRequest.Count - bringLineIntoViewRequest.NewCount, false, null, true, bringLineIntoViewRequest.UserState));
				}
				else if (this._pendingRequest is MultiPageTextView.BringPageIntoViewRequest)
				{
					MultiPageTextView.BringPageIntoViewRequest bringPageIntoViewRequest = (MultiPageTextView.BringPageIntoViewRequest)this._pendingRequest;
					this.OnBringPageIntoViewCompleted(new BringPageIntoViewCompletedEventArgs(bringPageIntoViewRequest.Position, bringPageIntoViewRequest.SuggestedOffset, bringPageIntoViewRequest.Count, bringPageIntoViewRequest.NewPosition, bringPageIntoViewRequest.NewSuggestedOffset, bringPageIntoViewRequest.Count - bringPageIntoViewRequest.NewCount, false, null, true, bringPageIntoViewRequest.UserState));
				}
				else if (this._pendingRequest is MultiPageTextView.BringPointIntoViewRequest)
				{
					MultiPageTextView.BringPointIntoViewRequest bringPointIntoViewRequest = (MultiPageTextView.BringPointIntoViewRequest)this._pendingRequest;
					this.OnBringPointIntoViewCompleted(new BringPointIntoViewCompletedEventArgs(bringPointIntoViewRequest.Point, bringPointIntoViewRequest.Position, false, null, true, bringPointIntoViewRequest.UserState));
				}
				else if (this._pendingRequest is MultiPageTextView.BringPositionIntoViewRequest)
				{
					MultiPageTextView.BringPositionIntoViewRequest bringPositionIntoViewRequest = (MultiPageTextView.BringPositionIntoViewRequest)this._pendingRequest;
					this.OnBringPositionIntoViewCompleted(new BringPositionIntoViewCompletedEventArgs(bringPositionIntoViewRequest.Position, false, null, true, bringPositionIntoViewRequest.UserState));
				}
				this._pendingRequest = null;
			}
		}

		// Token: 0x0600711D RID: 28957 RVA: 0x0020566C File Offset: 0x0020386C
		internal void OnPagesUpdated()
		{
			this.OnPagesUpdatedCore();
			if (this.IsValid)
			{
				this.OnUpdated(EventArgs.Empty);
			}
		}

		// Token: 0x0600711E RID: 28958 RVA: 0x00201E46 File Offset: 0x00200046
		internal void OnPageLayoutChanged()
		{
			if (this.IsValid)
			{
				this.OnUpdated(EventArgs.Empty);
			}
		}

		// Token: 0x0600711F RID: 28959 RVA: 0x00205687 File Offset: 0x00203887
		internal ITextView GetPageTextViewFromPosition(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			return this.GetTextViewFromPosition(position);
		}

		// Token: 0x17001ADC RID: 6876
		// (get) Token: 0x06007120 RID: 28960 RVA: 0x002056A8 File Offset: 0x002038A8
		internal override UIElement RenderScope
		{
			get
			{
				return this._renderScope;
			}
		}

		// Token: 0x17001ADD RID: 6877
		// (get) Token: 0x06007121 RID: 28961 RVA: 0x002056B0 File Offset: 0x002038B0
		internal override ITextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x17001ADE RID: 6878
		// (get) Token: 0x06007122 RID: 28962 RVA: 0x002056B8 File Offset: 0x002038B8
		internal override bool IsValid
		{
			get
			{
				bool result = false;
				if (this._pageTextViews != null)
				{
					result = true;
					for (int i = 0; i < this._pageTextViews.Count; i++)
					{
						if (!this._pageTextViews[i].IsValid)
						{
							result = false;
							break;
						}
					}
				}
				return result;
			}
		}

		// Token: 0x17001ADF RID: 6879
		// (get) Token: 0x06007123 RID: 28963 RVA: 0x002056FF File Offset: 0x002038FF
		internal override bool RendersOwnSelection
		{
			get
			{
				return this._pageTextViews != null && this._pageTextViews.Count > 0 && this._pageTextViews[0].RendersOwnSelection;
			}
		}

		// Token: 0x17001AE0 RID: 6880
		// (get) Token: 0x06007124 RID: 28964 RVA: 0x0020572C File Offset: 0x0020392C
		internal override ReadOnlyCollection<TextSegment> TextSegments
		{
			get
			{
				List<TextSegment> list = new List<TextSegment>();
				if (this.IsValid)
				{
					for (int i = 0; i < this._pageTextViews.Count; i++)
					{
						list.AddRange(this._pageTextViews[i].TextSegments);
					}
				}
				return new ReadOnlyCollection<TextSegment>(list);
			}
		}

		// Token: 0x06007125 RID: 28965 RVA: 0x0020577C File Offset: 0x0020397C
		private void OnPagesUpdatedCore()
		{
			for (int i = 0; i < this._pageTextViews.Count; i++)
			{
				this._pageTextViews[i].Updated -= this.HandlePageTextViewUpdated;
			}
			this._pageTextViews.Clear();
			ReadOnlyCollection<DocumentPageView> pageViews = this._viewer.PageViews;
			if (pageViews != null)
			{
				for (int i = 0; i < pageViews.Count; i++)
				{
					DocumentPageTextView documentPageTextView = ((IServiceProvider)pageViews[i]).GetService(typeof(ITextView)) as DocumentPageTextView;
					if (documentPageTextView != null)
					{
						this._pageTextViews.Add(documentPageTextView);
						documentPageTextView.Updated += this.HandlePageTextViewUpdated;
					}
				}
			}
		}

		// Token: 0x06007126 RID: 28966 RVA: 0x00205824 File Offset: 0x00203A24
		private void HandlePageTextViewUpdated(object sender, EventArgs e)
		{
			this.OnUpdated(EventArgs.Empty);
		}

		// Token: 0x06007127 RID: 28967 RVA: 0x00205834 File Offset: 0x00203A34
		private void BringLineIntoViewCore(MultiPageTextView.BringLineIntoViewRequest request)
		{
			double newSuggestedX;
			int num;
			int num2;
			ITextPointer positionAtNextLineCore = this.GetPositionAtNextLineCore(request.NewPosition, request.NewSuggestedX, request.NewCount, out newSuggestedX, out num, out num2);
			Invariant.Assert(Math.Abs(request.NewCount) >= Math.Abs(num));
			request.NewPosition = positionAtNextLineCore;
			request.NewSuggestedX = newSuggestedX;
			request.NewCount -= num;
			request.NewPageNumber = num2;
			if (request.NewCount == 0)
			{
				this.OnBringLineIntoViewCompleted(request);
				return;
			}
			if (positionAtNextLineCore is DocumentSequenceTextPointer || positionAtNextLineCore is FixedTextPointer)
			{
				if (this._viewer.CanGoToPage(num2 + 1))
				{
					this._viewer.GoToPage(num2 + 1);
					return;
				}
				this.OnBringLineIntoViewCompleted(request);
				return;
			}
			else if (request.NewCount > 0)
			{
				if (this._viewer.CanGoToNextPage)
				{
					this._viewer.NextPage();
					return;
				}
				this.OnBringLineIntoViewCompleted(request);
				return;
			}
			else
			{
				if (this._viewer.CanGoToPreviousPage)
				{
					this._viewer.PreviousPage();
					return;
				}
				this.OnBringLineIntoViewCompleted(request);
				return;
			}
		}

		// Token: 0x06007128 RID: 28968 RVA: 0x00205930 File Offset: 0x00203B30
		private void BringPageIntoViewCore(MultiPageTextView.BringPageIntoViewRequest request)
		{
			Point newSuggestedOffset;
			int num;
			int num2;
			ITextPointer positionAtNextPageCore = this.GetPositionAtNextPageCore(request.NewPosition, request.NewSuggestedOffset, request.NewCount, out newSuggestedOffset, out num, out num2);
			Invariant.Assert(Math.Abs(request.NewCount) >= Math.Abs(num));
			request.NewPosition = positionAtNextPageCore;
			request.NewSuggestedOffset = newSuggestedOffset;
			request.NewCount -= num;
			if (request.NewCount == 0 || num2 == -1)
			{
				this.OnBringPageIntoViewCompleted(request);
				return;
			}
			num2 += ((request.NewCount > 0) ? 1 : -1);
			if (this._viewer.CanGoToPage(num2 + 1))
			{
				request.NewPageNumber = num2;
				this._viewer.GoToPage(num2 + 1);
				return;
			}
			this.OnBringPageIntoViewCompleted(request);
		}

		// Token: 0x06007129 RID: 28969 RVA: 0x002059E4 File Offset: 0x00203BE4
		private ITextPointer GetPositionAtNextLineCore(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved, out int pageNumber)
		{
			DocumentPageTextView documentPageTextView = this.GetTextViewFromPosition(position);
			ITextPointer result;
			if (documentPageTextView != null)
			{
				int num = count;
				suggestedX = this.TransformToDescendant(documentPageTextView.RenderScope, new Point(suggestedX, 0.0)).X;
				result = documentPageTextView.GetPositionAtNextLine(position, suggestedX, count, out newSuggestedX, out linesMoved);
				pageNumber = ((DocumentPageView)documentPageTextView.RenderScope).PageNumber;
				newSuggestedX = this.TransformToAncestor(documentPageTextView.RenderScope, new Point(newSuggestedX, 0.0)).X;
				while (num != linesMoved)
				{
					int num2 = 0;
					count = num - linesMoved;
					pageNumber += ((count > 0) ? 1 : -1);
					documentPageTextView = this.GetTextViewFromPageNumber(pageNumber);
					if (documentPageTextView == null)
					{
						break;
					}
					ReadOnlyCollection<TextSegment> textSegments = documentPageTextView.TextSegments;
					int num3 = count;
					if (count > 0)
					{
						position = documentPageTextView.GetTextPositionFromPoint(new Point(suggestedX, 0.0), true);
						if (position != null)
						{
							count--;
							linesMoved++;
						}
					}
					else
					{
						position = documentPageTextView.GetTextPositionFromPoint(new Point(suggestedX, documentPageTextView.RenderScope.RenderSize.Height), true);
						if (position != null)
						{
							count++;
							linesMoved--;
						}
					}
					if (position != null)
					{
						if (count == 0)
						{
							result = this.GetPositionAtPageBoundary(num3 > 0, documentPageTextView, position, suggestedX);
							newSuggestedX = suggestedX;
						}
						else
						{
							result = documentPageTextView.GetPositionAtNextLine(position, suggestedX, count, out newSuggestedX, out num2);
							linesMoved += num2;
						}
						newSuggestedX = this.TransformToAncestor(documentPageTextView.RenderScope, new Point(newSuggestedX, 0.0)).X;
					}
				}
			}
			else
			{
				result = position;
				linesMoved = 0;
				newSuggestedX = suggestedX;
				pageNumber = -1;
			}
			return result;
		}

		// Token: 0x0600712A RID: 28970 RVA: 0x00205B80 File Offset: 0x00203D80
		private ITextPointer GetPositionAtNextPageCore(ITextPointer position, Point suggestedOffset, int count, out Point newSuggestedOffset, out int pagesMoved, out int pageNumber)
		{
			ITextPointer textPointer = position;
			pagesMoved = 0;
			newSuggestedOffset = suggestedOffset;
			pageNumber = -1;
			DocumentPageTextView textViewFromPosition = this.GetTextViewFromPosition(position);
			if (textViewFromPosition != null)
			{
				int pageNumber2 = ((DocumentPageView)textViewFromPosition.RenderScope).PageNumber;
				DocumentPageTextView textViewForNextPage = this.GetTextViewForNextPage(pageNumber2, count, out pageNumber);
				pagesMoved = pageNumber - pageNumber2;
				Invariant.Assert(Math.Abs(pagesMoved) <= Math.Abs(count));
				if (pageNumber != pageNumber2 && textViewForNextPage != null)
				{
					Point point = this.TransformToDescendant(textViewFromPosition.RenderScope, suggestedOffset);
					textPointer = textViewForNextPage.GetTextPositionFromPoint(point, true);
					if (textPointer != null)
					{
						Rect rectangleFromTextPosition = textViewForNextPage.GetRectangleFromTextPosition(textPointer);
						point = this.TransformToAncestor(textViewFromPosition.RenderScope, new Point(rectangleFromTextPosition.X, rectangleFromTextPosition.Y));
						newSuggestedOffset = point;
					}
					else
					{
						textPointer = position;
						pagesMoved = 0;
						pageNumber = pageNumber2;
					}
				}
				else
				{
					pagesMoved = 0;
					pageNumber = pageNumber2;
				}
			}
			return textPointer;
		}

		// Token: 0x0600712B RID: 28971 RVA: 0x00205C58 File Offset: 0x00203E58
		private ITextPointer GetPositionAtPageBoundary(bool pageTop, ITextView pageTextView, ITextPointer position, double suggestedX)
		{
			ITextPointer textPointer;
			if (pageTop)
			{
				double suggestedX2;
				int num;
				textPointer = pageTextView.GetPositionAtNextLine(position, suggestedX, 1, out suggestedX2, out num);
				if (num == 1)
				{
					textPointer = pageTextView.GetPositionAtNextLine(textPointer, suggestedX2, -1, out suggestedX2, out num);
				}
				else
				{
					textPointer = position;
				}
			}
			else
			{
				double suggestedX2;
				int num;
				textPointer = pageTextView.GetPositionAtNextLine(position, suggestedX, -1, out suggestedX2, out num);
				if (num == -1)
				{
					textPointer = pageTextView.GetPositionAtNextLine(textPointer, suggestedX2, 1, out suggestedX2, out num);
				}
				else
				{
					textPointer = position;
				}
			}
			return textPointer;
		}

		// Token: 0x0600712C RID: 28972 RVA: 0x00205CB8 File Offset: 0x00203EB8
		private DocumentPageTextView GetTextViewFromPoint(Point point, bool snap)
		{
			DocumentPageTextView documentPageTextView = null;
			for (int i = 0; i < this._pageTextViews.Count; i++)
			{
				if (this.TransformToAncestor(this._pageTextViews[i].RenderScope, new Rect(this._pageTextViews[i].RenderScope.RenderSize)).Contains(point))
				{
					documentPageTextView = this._pageTextViews[i];
					break;
				}
			}
			if (documentPageTextView == null && snap)
			{
				double[] array = new double[this._pageTextViews.Count];
				for (int i = 0; i < this._pageTextViews.Count; i++)
				{
					Rect rect = this.TransformToAncestor(this._pageTextViews[i].RenderScope, new Rect(this._pageTextViews[i].RenderScope.RenderSize));
					double x;
					if (point.X >= rect.Left && point.X <= rect.Right)
					{
						x = 0.0;
					}
					else
					{
						x = Math.Min(Math.Abs(point.X - rect.Left), Math.Abs(point.X - rect.Right));
					}
					double x2;
					if (point.Y >= rect.Top && point.Y <= rect.Bottom)
					{
						x2 = 0.0;
					}
					else
					{
						x2 = Math.Min(Math.Abs(point.Y - rect.Top), Math.Abs(point.Y - rect.Bottom));
					}
					array[i] = Math.Sqrt(Math.Pow(x, 2.0) + Math.Pow(x2, 2.0));
				}
				double num = double.MaxValue;
				for (int i = 0; i < array.Length; i++)
				{
					if (num > array[i])
					{
						num = array[i];
						documentPageTextView = this._pageTextViews[i];
					}
				}
			}
			return documentPageTextView;
		}

		// Token: 0x0600712D RID: 28973 RVA: 0x00205EA8 File Offset: 0x002040A8
		private DocumentPageTextView GetTextViewFromPosition(ITextPointer position)
		{
			DocumentPageTextView result = null;
			for (int i = 0; i < this._pageTextViews.Count; i++)
			{
				if (this._pageTextViews[i].Contains(position))
				{
					result = this._pageTextViews[i];
					break;
				}
			}
			return result;
		}

		// Token: 0x0600712E RID: 28974 RVA: 0x00205EF4 File Offset: 0x002040F4
		private DocumentPageTextView GetTextViewFromPageNumber(int pageNumber)
		{
			DocumentPageTextView result = null;
			for (int i = 0; i < this._pageTextViews.Count; i++)
			{
				if (this._pageTextViews[i].DocumentPageView.PageNumber == pageNumber)
				{
					result = this._pageTextViews[i];
					break;
				}
			}
			return result;
		}

		// Token: 0x0600712F RID: 28975 RVA: 0x00205F44 File Offset: 0x00204144
		private DocumentPageTextView GetTextViewForNextPage(int pageNumber, int count, out int newPageNumber)
		{
			Invariant.Assert(count != 0);
			newPageNumber = pageNumber + count;
			int num = newPageNumber;
			DocumentPageTextView documentPageTextView = null;
			int num2 = Math.Abs(count);
			for (int i = 0; i < this._pageTextViews.Count; i++)
			{
				if (this._pageTextViews[i].DocumentPageView.PageNumber == newPageNumber)
				{
					documentPageTextView = this._pageTextViews[i];
					num = newPageNumber;
					break;
				}
				int pageNumber2 = this._pageTextViews[i].DocumentPageView.PageNumber;
				if (count > 0 && pageNumber2 > pageNumber)
				{
					int num3 = pageNumber2 - pageNumber;
					if (num3 < num2)
					{
						num2 = num3;
						documentPageTextView = this._pageTextViews[i];
						num = pageNumber2;
					}
				}
				else if (count < 0 && pageNumber2 < pageNumber)
				{
					int num4 = Math.Abs(pageNumber2 - pageNumber);
					if (num4 < num2)
					{
						num2 = num4;
						documentPageTextView = this._pageTextViews[i];
						num = pageNumber2;
					}
				}
			}
			if (documentPageTextView != null)
			{
				newPageNumber = num;
			}
			else
			{
				newPageNumber = pageNumber;
				documentPageTextView = this.GetTextViewFromPageNumber(pageNumber);
			}
			Invariant.Assert(newPageNumber >= 0);
			return documentPageTextView;
		}

		// Token: 0x06007130 RID: 28976 RVA: 0x00206044 File Offset: 0x00204244
		private Transform GetTransformToAncestor(Visual innerScope)
		{
			Transform transform = innerScope.TransformToAncestor(this._renderScope) as Transform;
			if (transform == null)
			{
				transform = Transform.Identity;
			}
			return transform;
		}

		// Token: 0x06007131 RID: 28977 RVA: 0x00206070 File Offset: 0x00204270
		private Rect TransformToAncestor(Visual innerScope, Rect rect)
		{
			if (rect != Rect.Empty)
			{
				GeneralTransform generalTransform = innerScope.TransformToAncestor(this._renderScope);
				if (generalTransform != null)
				{
					rect = generalTransform.TransformBounds(rect);
				}
			}
			return rect;
		}

		// Token: 0x06007132 RID: 28978 RVA: 0x002060A4 File Offset: 0x002042A4
		private Point TransformToAncestor(Visual innerScope, Point point)
		{
			GeneralTransform generalTransform = innerScope.TransformToAncestor(this._renderScope);
			if (generalTransform != null)
			{
				point = generalTransform.Transform(point);
			}
			return point;
		}

		// Token: 0x06007133 RID: 28979 RVA: 0x002060CC File Offset: 0x002042CC
		private Point TransformToDescendant(Visual innerScope, Point point)
		{
			GeneralTransform generalTransform = innerScope.TransformToAncestor(this._renderScope);
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

		// Token: 0x06007134 RID: 28980 RVA: 0x002060FD File Offset: 0x002042FD
		private void OnBringPositionIntoViewCompleted(MultiPageTextView.BringPositionIntoViewRequest request)
		{
			this._pendingRequest = null;
			this.OnBringPositionIntoViewCompleted(new BringPositionIntoViewCompletedEventArgs(request.Position, request.Succeeded, null, false, request.UserState));
		}

		// Token: 0x06007135 RID: 28981 RVA: 0x00206125 File Offset: 0x00204325
		private void OnBringPointIntoViewCompleted(MultiPageTextView.BringPointIntoViewRequest request)
		{
			this._pendingRequest = null;
			this.OnBringPointIntoViewCompleted(new BringPointIntoViewCompletedEventArgs(request.Point, request.Position, request.Position != null, null, false, request.UserState));
		}

		// Token: 0x06007136 RID: 28982 RVA: 0x00206158 File Offset: 0x00204358
		private void OnBringLineIntoViewCompleted(MultiPageTextView.BringLineIntoViewRequest request)
		{
			this._pendingRequest = null;
			this.OnBringLineIntoViewCompleted(new BringLineIntoViewCompletedEventArgs(request.Position, request.SuggestedX, request.Count, request.NewPosition, request.NewSuggestedX, request.Count - request.NewCount, request.NewCount == 0, null, false, request.UserState));
		}

		// Token: 0x06007137 RID: 28983 RVA: 0x002061B4 File Offset: 0x002043B4
		private void OnBringPageIntoViewCompleted(MultiPageTextView.BringPageIntoViewRequest request)
		{
			this._pendingRequest = null;
			this.OnBringPageIntoViewCompleted(new BringPageIntoViewCompletedEventArgs(request.Position, request.SuggestedOffset, request.Count, request.NewPosition, request.NewSuggestedOffset, request.Count - request.NewCount, request.NewCount == 0, null, false, request.UserState));
		}

		// Token: 0x06007138 RID: 28984 RVA: 0x00206210 File Offset: 0x00204410
		private object OnUpdatedWorker(object o)
		{
			if (this.IsValid && this._pendingRequest != null)
			{
				if (this._pendingRequest is MultiPageTextView.BringLineIntoViewRequest)
				{
					MultiPageTextView.BringLineIntoViewRequest bringLineIntoViewRequest = (MultiPageTextView.BringLineIntoViewRequest)this._pendingRequest;
					ITextView textView = this.GetTextViewFromPageNumber(bringLineIntoViewRequest.NewPageNumber);
					if (textView != null)
					{
						double x = this.TransformToDescendant(textView.RenderScope, new Point(bringLineIntoViewRequest.NewSuggestedX, 0.0)).X;
						ITextPointer textPositionFromPoint;
						if (bringLineIntoViewRequest.Count > 0)
						{
							textPositionFromPoint = textView.GetTextPositionFromPoint(new Point(-1.0, -1.0), true);
							if (textPositionFromPoint != null)
							{
								bringLineIntoViewRequest.NewCount--;
							}
						}
						else
						{
							textPositionFromPoint = textView.GetTextPositionFromPoint((Point)textView.RenderScope.RenderSize, true);
							if (textPositionFromPoint != null)
							{
								bringLineIntoViewRequest.NewCount++;
							}
						}
						if (textPositionFromPoint == null)
						{
							if (bringLineIntoViewRequest.NewPosition == null)
							{
								bringLineIntoViewRequest.NewPosition = bringLineIntoViewRequest.Position;
								bringLineIntoViewRequest.NewCount = bringLineIntoViewRequest.Count;
							}
							this.OnBringLineIntoViewCompleted(bringLineIntoViewRequest);
						}
						else if (bringLineIntoViewRequest.NewCount != 0)
						{
							bringLineIntoViewRequest.NewPosition = textPositionFromPoint;
							this.BringLineIntoViewCore(bringLineIntoViewRequest);
						}
						else
						{
							bringLineIntoViewRequest.NewPosition = this.GetPositionAtPageBoundary(bringLineIntoViewRequest.Count > 0, textView, textPositionFromPoint, bringLineIntoViewRequest.NewSuggestedX);
							this.OnBringLineIntoViewCompleted(bringLineIntoViewRequest);
						}
					}
					else if (this.IsPageNumberOutOfRange(bringLineIntoViewRequest.NewPageNumber))
					{
						this.OnBringLineIntoViewCompleted(bringLineIntoViewRequest);
					}
				}
				else if (this._pendingRequest is MultiPageTextView.BringPageIntoViewRequest)
				{
					MultiPageTextView.BringPageIntoViewRequest bringPageIntoViewRequest = (MultiPageTextView.BringPageIntoViewRequest)this._pendingRequest;
					ITextView textView = this.GetTextViewFromPageNumber(bringPageIntoViewRequest.NewPageNumber);
					if (textView != null)
					{
						Point point = this.TransformToDescendant(textView.RenderScope, bringPageIntoViewRequest.NewSuggestedOffset);
						Point point2 = point;
						Invariant.Assert(bringPageIntoViewRequest.NewCount != 0);
						ITextPointer textPositionFromPoint = textView.GetTextPositionFromPoint(point2, true);
						if (textPositionFromPoint != null)
						{
							bringPageIntoViewRequest.NewCount = ((bringPageIntoViewRequest.Count > 0) ? (bringPageIntoViewRequest.NewCount - 1) : (bringPageIntoViewRequest.NewCount + 1));
						}
						if (textPositionFromPoint == null)
						{
							if (bringPageIntoViewRequest.NewPosition == null)
							{
								bringPageIntoViewRequest.NewPosition = bringPageIntoViewRequest.Position;
								bringPageIntoViewRequest.NewCount = bringPageIntoViewRequest.Count;
							}
							this.OnBringPageIntoViewCompleted(bringPageIntoViewRequest);
						}
						else if (bringPageIntoViewRequest.NewCount != 0)
						{
							bringPageIntoViewRequest.NewPosition = textPositionFromPoint;
							this.BringPageIntoViewCore(bringPageIntoViewRequest);
						}
						else
						{
							bringPageIntoViewRequest.NewPosition = textPositionFromPoint;
							this.OnBringPageIntoViewCompleted(bringPageIntoViewRequest);
						}
					}
					else if (this.IsPageNumberOutOfRange(bringPageIntoViewRequest.NewPageNumber))
					{
						this.OnBringPageIntoViewCompleted(bringPageIntoViewRequest);
					}
				}
				else if (this._pendingRequest is MultiPageTextView.BringPointIntoViewRequest)
				{
					MultiPageTextView.BringPointIntoViewRequest bringPointIntoViewRequest = (MultiPageTextView.BringPointIntoViewRequest)this._pendingRequest;
					ITextView textView = this.GetTextViewFromPoint(bringPointIntoViewRequest.Point, true);
					if (textView != null)
					{
						Point point = this.TransformToDescendant(textView.RenderScope, bringPointIntoViewRequest.Point);
						bringPointIntoViewRequest.Position = textView.GetTextPositionFromPoint(point, true);
					}
					this.OnBringPointIntoViewCompleted(bringPointIntoViewRequest);
				}
				else if (this._pendingRequest is MultiPageTextView.BringPositionIntoViewRequest)
				{
					MultiPageTextView.BringPositionIntoViewRequest bringPositionIntoViewRequest = (MultiPageTextView.BringPositionIntoViewRequest)this._pendingRequest;
					bringPositionIntoViewRequest.Succeeded = bringPositionIntoViewRequest.Position.HasValidLayout;
					this.OnBringPositionIntoViewCompleted(bringPositionIntoViewRequest);
				}
			}
			return null;
		}

		// Token: 0x06007139 RID: 28985 RVA: 0x00206524 File Offset: 0x00204724
		private bool IsPageNumberOutOfRange(int pageNumber)
		{
			if (pageNumber < 0)
			{
				return true;
			}
			IDocumentPaginatorSource document = this._viewer.Document;
			if (document == null)
			{
				return true;
			}
			DocumentPaginator documentPaginator = document.DocumentPaginator;
			return documentPaginator == null || (documentPaginator.IsPageCountValid && pageNumber >= documentPaginator.PageCount);
		}

		// Token: 0x04003700 RID: 14080
		private readonly DocumentViewerBase _viewer;

		// Token: 0x04003701 RID: 14081
		private readonly UIElement _renderScope;

		// Token: 0x04003702 RID: 14082
		private readonly ITextContainer _textContainer;

		// Token: 0x04003703 RID: 14083
		private List<DocumentPageTextView> _pageTextViews;

		// Token: 0x04003704 RID: 14084
		private MultiPageTextView.BringIntoViewRequest _pendingRequest;

		// Token: 0x02000B3D RID: 2877
		private class BringIntoViewRequest
		{
			// Token: 0x06008D7F RID: 36223 RVA: 0x0025999F File Offset: 0x00257B9F
			internal BringIntoViewRequest(object userState)
			{
				this.UserState = userState;
			}

			// Token: 0x04004AAD RID: 19117
			internal readonly object UserState;
		}

		// Token: 0x02000B3E RID: 2878
		private class BringPositionIntoViewRequest : MultiPageTextView.BringIntoViewRequest
		{
			// Token: 0x06008D80 RID: 36224 RVA: 0x002599AE File Offset: 0x00257BAE
			internal BringPositionIntoViewRequest(ITextPointer position, object userState) : base(userState)
			{
				this.Position = position;
				this.Succeeded = false;
			}

			// Token: 0x04004AAE RID: 19118
			internal readonly ITextPointer Position;

			// Token: 0x04004AAF RID: 19119
			internal bool Succeeded;
		}

		// Token: 0x02000B3F RID: 2879
		private class BringPointIntoViewRequest : MultiPageTextView.BringIntoViewRequest
		{
			// Token: 0x06008D81 RID: 36225 RVA: 0x002599C5 File Offset: 0x00257BC5
			internal BringPointIntoViewRequest(Point point, object userState) : base(userState)
			{
				this.Point = point;
				this.Position = null;
			}

			// Token: 0x04004AB0 RID: 19120
			internal readonly Point Point;

			// Token: 0x04004AB1 RID: 19121
			internal ITextPointer Position;
		}

		// Token: 0x02000B40 RID: 2880
		private class BringLineIntoViewRequest : MultiPageTextView.BringIntoViewRequest
		{
			// Token: 0x06008D82 RID: 36226 RVA: 0x002599DC File Offset: 0x00257BDC
			internal BringLineIntoViewRequest(ITextPointer position, double suggestedX, int count, object userState) : base(userState)
			{
				this.Position = position;
				this.SuggestedX = suggestedX;
				this.Count = count;
				this.NewPosition = position;
				this.NewSuggestedX = suggestedX;
				this.NewCount = count;
			}

			// Token: 0x04004AB2 RID: 19122
			internal readonly ITextPointer Position;

			// Token: 0x04004AB3 RID: 19123
			internal readonly double SuggestedX;

			// Token: 0x04004AB4 RID: 19124
			internal readonly int Count;

			// Token: 0x04004AB5 RID: 19125
			internal ITextPointer NewPosition;

			// Token: 0x04004AB6 RID: 19126
			internal double NewSuggestedX;

			// Token: 0x04004AB7 RID: 19127
			internal int NewCount;

			// Token: 0x04004AB8 RID: 19128
			internal int NewPageNumber;
		}

		// Token: 0x02000B41 RID: 2881
		private class BringPageIntoViewRequest : MultiPageTextView.BringIntoViewRequest
		{
			// Token: 0x06008D83 RID: 36227 RVA: 0x00259A10 File Offset: 0x00257C10
			internal BringPageIntoViewRequest(ITextPointer position, Point suggestedOffset, int count, object userState) : base(userState)
			{
				this.Position = position;
				this.SuggestedOffset = suggestedOffset;
				this.Count = count;
				this.NewPosition = position;
				this.NewSuggestedOffset = suggestedOffset;
				this.NewCount = count;
			}

			// Token: 0x04004AB9 RID: 19129
			internal readonly ITextPointer Position;

			// Token: 0x04004ABA RID: 19130
			internal readonly Point SuggestedOffset;

			// Token: 0x04004ABB RID: 19131
			internal readonly int Count;

			// Token: 0x04004ABC RID: 19132
			internal ITextPointer NewPosition;

			// Token: 0x04004ABD RID: 19133
			internal Point NewSuggestedOffset;

			// Token: 0x04004ABE RID: 19134
			internal int NewCount;

			// Token: 0x04004ABF RID: 19135
			internal int NewPageNumber;
		}
	}
}
