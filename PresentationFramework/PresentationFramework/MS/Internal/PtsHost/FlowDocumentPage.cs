using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000625 RID: 1573
	internal sealed class FlowDocumentPage : DocumentPage, IServiceProvider, IDisposable, IContentHost
	{
		// Token: 0x06006835 RID: 26677 RVA: 0x001D5B41 File Offset: 0x001D3D41
		internal FlowDocumentPage(StructuralCache structuralCache) : base(null)
		{
			this._structuralCache = structuralCache;
			this._ptsPage = new PtsPage(structuralCache.Section);
		}

		// Token: 0x06006836 RID: 26678 RVA: 0x001D5B64 File Offset: 0x001D3D64
		~FlowDocumentPage()
		{
			this.Dispose(false);
		}

		// Token: 0x06006837 RID: 26679 RVA: 0x001D5B94 File Offset: 0x001D3D94
		public override void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
			base.Dispose();
		}

		// Token: 0x17001923 RID: 6435
		// (get) Token: 0x06006838 RID: 26680 RVA: 0x001D5BA9 File Offset: 0x001D3DA9
		public override Visual Visual
		{
			get
			{
				if (this.IsDisposed)
				{
					return null;
				}
				this.UpdateVisual();
				return base.Visual;
			}
		}

		// Token: 0x06006839 RID: 26681 RVA: 0x001D5BC4 File Offset: 0x001D3DC4
		internal void FormatBottomless(Size pageSize, Thickness pageMargin)
		{
			Invariant.Assert(!this.IsDisposed);
			this._formattedLinesCount = 0;
			TextDpi.EnsureValidPageSize(ref pageSize);
			this._pageMargin = pageMargin;
			base.SetSize(pageSize);
			if (!DoubleUtil.AreClose(this._lastFormatWidth, pageSize.Width) || !DoubleUtil.AreClose(this._pageMargin.Left, pageMargin.Left) || !DoubleUtil.AreClose(this._pageMargin.Right, pageMargin.Right))
			{
				this._structuralCache.InvalidateFormatCache(false);
			}
			this._lastFormatWidth = pageSize.Width;
			using (this._structuralCache.SetDocumentFormatContext(this))
			{
				this.OnBeforeFormatPage();
				if (this._ptsPage.PrepareForBottomlessUpdate())
				{
					this._structuralCache.CurrentFormatContext.PushNewPageData(pageSize, this._pageMargin, true, false);
					this._ptsPage.UpdateBottomlessPage();
				}
				else
				{
					this._structuralCache.CurrentFormatContext.PushNewPageData(pageSize, this._pageMargin, false, false);
					this._ptsPage.CreateBottomlessPage();
				}
				pageSize = this._ptsPage.CalculatedSize;
				pageSize.Width += pageMargin.Left + pageMargin.Right;
				pageSize.Height += pageMargin.Top + pageMargin.Bottom;
				base.SetSize(pageSize);
				base.SetContentBox(new Rect(pageMargin.Left, pageMargin.Top, this._ptsPage.CalculatedSize.Width, this._ptsPage.CalculatedSize.Height));
				this._structuralCache.CurrentFormatContext.PopPageData();
				this.OnAfterFormatPage();
				this._structuralCache.DetectInvalidOperation();
			}
		}

		// Token: 0x0600683A RID: 26682 RVA: 0x001D5D9C File Offset: 0x001D3F9C
		internal PageBreakRecord FormatFinite(Size pageSize, Thickness pageMargin, PageBreakRecord breakRecord)
		{
			Invariant.Assert(!this.IsDisposed);
			this._formattedLinesCount = 0;
			TextDpi.EnsureValidPageSize(ref pageSize);
			TextDpi.EnsureValidPageMargin(ref pageMargin, pageSize);
			double num = PtsHelper.CalculatePageMarginAdjustment(this._structuralCache, pageSize.Width - (pageMargin.Left + pageMargin.Right));
			if (!DoubleUtil.IsZero(num))
			{
				pageMargin.Right += num - num / 100.0;
			}
			this._pageMargin = pageMargin;
			base.SetSize(pageSize);
			base.SetContentBox(new Rect(pageMargin.Left, pageMargin.Top, pageSize.Width - (pageMargin.Left + pageMargin.Right), pageSize.Height - (pageMargin.Top + pageMargin.Bottom)));
			using (this._structuralCache.SetDocumentFormatContext(this))
			{
				this.OnBeforeFormatPage();
				if (this._ptsPage.PrepareForFiniteUpdate(breakRecord))
				{
					this._structuralCache.CurrentFormatContext.PushNewPageData(pageSize, this._pageMargin, true, true);
					this._ptsPage.UpdateFinitePage(breakRecord);
				}
				else
				{
					this._structuralCache.CurrentFormatContext.PushNewPageData(pageSize, this._pageMargin, false, true);
					this._ptsPage.CreateFinitePage(breakRecord);
				}
				this._structuralCache.CurrentFormatContext.PopPageData();
				this.OnAfterFormatPage();
				this._structuralCache.DetectInvalidOperation();
			}
			return this._ptsPage.BreakRecord;
		}

		// Token: 0x0600683B RID: 26683 RVA: 0x001D5F20 File Offset: 0x001D4120
		internal void Arrange(Size partitionSize)
		{
			Invariant.Assert(!this.IsDisposed);
			this._partitionSize = partitionSize;
			using (this._structuralCache.SetDocumentArrangeContext(this))
			{
				this._ptsPage.ArrangePage();
				this._structuralCache.DetectInvalidOperation();
			}
			this.ValidateTextView();
		}

		// Token: 0x0600683C RID: 26684 RVA: 0x001D5F88 File Offset: 0x001D4188
		internal void ForceReformat()
		{
			Invariant.Assert(!this.IsDisposed);
			this._ptsPage.ClearUpdateInfo();
			this._structuralCache.ForceReformat = true;
		}

		// Token: 0x0600683D RID: 26685 RVA: 0x001D5FB0 File Offset: 0x001D41B0
		internal IInputElement InputHitTestCore(Point point)
		{
			Invariant.Assert(!this.IsDisposed);
			if (FrameworkElement.GetFrameworkParent(this._structuralCache.FormattingOwner) == null)
			{
				return null;
			}
			IInputElement inputElement = null;
			if (this.IsLayoutDataValid)
			{
				GeneralTransform generalTransform = this.PageVisual.Child.TransformToAncestor(this.PageVisual);
				generalTransform = generalTransform.Inverse;
				if (generalTransform != null)
				{
					point = generalTransform.Transform(point);
					inputElement = this._ptsPage.InputHitTest(point);
				}
			}
			if (inputElement == null)
			{
				return this._structuralCache.FormattingOwner;
			}
			return inputElement;
		}

		// Token: 0x0600683E RID: 26686 RVA: 0x001D6034 File Offset: 0x001D4234
		internal ReadOnlyCollection<Rect> GetRectanglesCore(ContentElement child, bool isLimitedToTextView)
		{
			Invariant.Assert(!this.IsDisposed);
			List<Rect> list = new List<Rect>();
			if (this.IsLayoutDataValid)
			{
				TextPointer textPointer = this.FindElementPosition(child, isLimitedToTextView);
				if (textPointer != null)
				{
					int offsetToPosition = this._structuralCache.TextContainer.Start.GetOffsetToPosition(textPointer);
					int length = 1;
					if (child is TextElement)
					{
						TextPointer position = new TextPointer(((TextElement)child).ElementEnd);
						length = textPointer.GetOffsetToPosition(position);
					}
					list = this._ptsPage.GetRectangles(child, offsetToPosition, length);
				}
			}
			if (this.PageVisual != null && list.Count > 0)
			{
				List<Rect> list2 = new List<Rect>(list.Count);
				GeneralTransform generalTransform = this.PageVisual.Child.TransformToAncestor(this.PageVisual);
				for (int i = 0; i < list.Count; i++)
				{
					list2.Add(generalTransform.TransformBounds(list[i]));
				}
				list = list2;
			}
			Invariant.Assert(list != null);
			return new ReadOnlyCollection<Rect>(list);
		}

		// Token: 0x17001924 RID: 6436
		// (get) Token: 0x0600683F RID: 26687 RVA: 0x001D6128 File Offset: 0x001D4328
		internal IEnumerator<IInputElement> HostedElementsCore
		{
			get
			{
				if (this.IsLayoutDataValid)
				{
					this._textView = this.GetTextView();
					Invariant.Assert(this._textView != null && ((ITextView)this._textView).TextSegments.Count > 0);
					return new HostedElements(((ITextView)this._textView).TextSegments);
				}
				return new HostedElements(new ReadOnlyCollection<TextSegment>(new List<TextSegment>(0)));
			}
		}

		// Token: 0x17001925 RID: 6437
		// (get) Token: 0x06006840 RID: 26688 RVA: 0x001D6190 File Offset: 0x001D4390
		internal ReadOnlyCollection<ParagraphResult> FloatingElementResults
		{
			get
			{
				List<ParagraphResult> list = new List<ParagraphResult>(0);
				List<BaseParaClient> floatingElementList = this._ptsPage.PageContext.FloatingElementList;
				if (floatingElementList != null)
				{
					for (int i = 0; i < floatingElementList.Count; i++)
					{
						ParagraphResult item = floatingElementList[i].CreateParagraphResult();
						list.Add(item);
					}
				}
				return new ReadOnlyCollection<ParagraphResult>(list);
			}
		}

		// Token: 0x06006841 RID: 26689 RVA: 0x001D61E3 File Offset: 0x001D43E3
		internal void OnChildDesiredSizeChangedCore(UIElement child)
		{
			this._structuralCache.FormattingOwner.OnChildDesiredSizeChanged(child);
		}

		// Token: 0x06006842 RID: 26690 RVA: 0x001D61F8 File Offset: 0x001D43F8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal ReadOnlyCollection<ColumnResult> GetColumnResults(out bool hasTextContent)
		{
			Invariant.Assert(!this.IsDisposed);
			List<ColumnResult> list = new List<ColumnResult>(0);
			hasTextContent = false;
			if (!(this._ptsPage.PageHandle == IntPtr.Zero))
			{
				PTS.FSPAGEDETAILS fspagedetails;
				PTS.Validate(PTS.FsQueryPageDetails(this.StructuralCache.PtsContext.Context, this._ptsPage.PageHandle, out fspagedetails));
				if (PTS.ToBoolean(fspagedetails.fSimple))
				{
					PTS.FSTRACKDETAILS fstrackdetails;
					PTS.Validate(PTS.FsQueryTrackDetails(this.StructuralCache.PtsContext.Context, fspagedetails.u.simple.trackdescr.pfstrack, out fstrackdetails));
					if (fstrackdetails.cParas > 0)
					{
						list = new List<ColumnResult>(1);
						ColumnResult columnResult = new ColumnResult(this, ref fspagedetails.u.simple.trackdescr, default(Vector));
						list.Add(columnResult);
						if (columnResult.HasTextContent)
						{
							hasTextContent = true;
						}
					}
				}
				else if (fspagedetails.u.complex.cSections > 0)
				{
					PTS.FSSECTIONDESCRIPTION[] array;
					PtsHelper.SectionListFromPage(this.StructuralCache.PtsContext, this._ptsPage.PageHandle, ref fspagedetails, out array);
					PTS.FSSECTIONDETAILS fssectiondetails;
					PTS.Validate(PTS.FsQuerySectionDetails(this.StructuralCache.PtsContext.Context, array[0].pfssection, out fssectiondetails));
					if (PTS.ToBoolean(fssectiondetails.fFootnotesAsPagenotes) && fssectiondetails.u.withpagenotes.cBasicColumns > 0)
					{
						PTS.FSTRACKDESCRIPTION[] array2;
						PtsHelper.TrackListFromSection(this.StructuralCache.PtsContext, array[0].pfssection, ref fssectiondetails, out array2);
						list = new List<ColumnResult>(fssectiondetails.u.withpagenotes.cBasicColumns);
						foreach (PTS.FSTRACKDESCRIPTION fstrackdescription in array2)
						{
							if (fstrackdescription.pfstrack != IntPtr.Zero)
							{
								PTS.FSTRACKDETAILS fstrackdetails2;
								PTS.Validate(PTS.FsQueryTrackDetails(this.StructuralCache.PtsContext.Context, fstrackdescription.pfstrack, out fstrackdetails2));
								if (fstrackdetails2.cParas > 0)
								{
									ColumnResult columnResult2 = new ColumnResult(this, ref fstrackdescription, default(Vector));
									list.Add(columnResult2);
									if (columnResult2.HasTextContent)
									{
										hasTextContent = true;
									}
								}
							}
						}
					}
				}
			}
			Invariant.Assert(list != null);
			return new ReadOnlyCollection<ColumnResult>(list);
		}

		// Token: 0x06006843 RID: 26691 RVA: 0x001D6444 File Offset: 0x001D4644
		[SecurityCritical]
		internal TextContentRange GetTextContentRangeFromColumn(IntPtr pfstrack)
		{
			Invariant.Assert(!this.IsDisposed);
			PTS.FSTRACKDETAILS fstrackdetails;
			PTS.Validate(PTS.FsQueryTrackDetails(this.StructuralCache.PtsContext.Context, pfstrack, out fstrackdetails));
			TextContentRange textContentRange = new TextContentRange();
			if (fstrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] array;
				PtsHelper.ParaListFromTrack(this.StructuralCache.PtsContext, pfstrack, ref fstrackdetails, out array);
				for (int i = 0; i < array.Length; i++)
				{
					BaseParaClient baseParaClient = this.StructuralCache.PtsContext.HandleToObject(array[i].pfsparaclient) as BaseParaClient;
					PTS.ValidateHandle(baseParaClient);
					textContentRange.Merge(baseParaClient.GetTextContentRange());
				}
			}
			return textContentRange;
		}

		// Token: 0x06006844 RID: 26692 RVA: 0x001D64E8 File Offset: 0x001D46E8
		[SecurityCritical]
		internal ReadOnlyCollection<ParagraphResult> GetParagraphResultsFromColumn(IntPtr pfstrack, Vector parentOffset, out bool hasTextContent)
		{
			Invariant.Assert(!this.IsDisposed);
			PTS.FSTRACKDETAILS fstrackdetails;
			PTS.Validate(PTS.FsQueryTrackDetails(this.StructuralCache.PtsContext.Context, pfstrack, out fstrackdetails));
			hasTextContent = false;
			if (fstrackdetails.cParas == 0)
			{
				return new ReadOnlyCollection<ParagraphResult>(new List<ParagraphResult>(0));
			}
			PTS.FSPARADESCRIPTION[] array;
			PtsHelper.ParaListFromTrack(this.StructuralCache.PtsContext, pfstrack, ref fstrackdetails, out array);
			List<ParagraphResult> list = new List<ParagraphResult>(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				BaseParaClient baseParaClient = this.StructuralCache.PtsContext.HandleToObject(array[i].pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				ParagraphResult paragraphResult = baseParaClient.CreateParagraphResult();
				if (paragraphResult.HasTextContent)
				{
					hasTextContent = true;
				}
				list.Add(paragraphResult);
			}
			return new ReadOnlyCollection<ParagraphResult>(list);
		}

		// Token: 0x06006845 RID: 26693 RVA: 0x001D65B0 File Offset: 0x001D47B0
		internal void OnFormatLine()
		{
			Invariant.Assert(!this.IsDisposed);
			this._formattedLinesCount++;
		}

		// Token: 0x06006846 RID: 26694 RVA: 0x001D65CE File Offset: 0x001D47CE
		internal void EnsureValidVisuals()
		{
			Invariant.Assert(!this.IsDisposed);
			this.UpdateVisual();
		}

		// Token: 0x06006847 RID: 26695 RVA: 0x001D65E4 File Offset: 0x001D47E4
		internal void UpdateViewport(ref PTS.FSRECT viewport, bool drawBackground)
		{
			GeneralTransform generalTransform = this.PageVisual.Child.TransformToAncestor(this.PageVisual);
			generalTransform = generalTransform.Inverse;
			Rect rect = viewport.FromTextDpi();
			if (generalTransform != null)
			{
				rect = generalTransform.TransformBounds(rect);
			}
			if (!this.IsDisposed)
			{
				if (drawBackground)
				{
					this.PageVisual.DrawBackground((Brush)this._structuralCache.PropertyOwner.GetValue(FlowDocument.BackgroundProperty), rect);
				}
				using (this._structuralCache.SetDocumentVisualValidationContext(this))
				{
					PTS.FSRECT fsrect = new PTS.FSRECT(rect);
					this._ptsPage.UpdateViewport(ref fsrect);
					this._structuralCache.DetectInvalidOperation();
				}
				this.ValidateTextView();
			}
		}

		// Token: 0x17001926 RID: 6438
		// (get) Token: 0x06006848 RID: 26696 RVA: 0x001D66A4 File Offset: 0x001D48A4
		// (set) Token: 0x06006849 RID: 26697 RVA: 0x001D66B1 File Offset: 0x001D48B1
		internal bool UseSizingWorkaroundForTextBox
		{
			get
			{
				return this._ptsPage.UseSizingWorkaroundForTextBox;
			}
			set
			{
				this._ptsPage.UseSizingWorkaroundForTextBox = value;
			}
		}

		// Token: 0x17001927 RID: 6439
		// (get) Token: 0x0600684A RID: 26698 RVA: 0x001D66BF File Offset: 0x001D48BF
		internal Thickness Margin
		{
			get
			{
				return this._pageMargin;
			}
		}

		// Token: 0x17001928 RID: 6440
		// (get) Token: 0x0600684B RID: 26699 RVA: 0x001D66C7 File Offset: 0x001D48C7
		internal bool IsDisposed
		{
			get
			{
				return this._disposed != 0 || this._structuralCache.PtsContext.Disposed;
			}
		}

		// Token: 0x17001929 RID: 6441
		// (get) Token: 0x0600684C RID: 26700 RVA: 0x001D66E4 File Offset: 0x001D48E4
		internal Size ContentSize
		{
			get
			{
				Size contentSize = this._ptsPage.ContentSize;
				contentSize.Width += this._pageMargin.Left + this._pageMargin.Right;
				contentSize.Height += this._pageMargin.Top + this._pageMargin.Bottom;
				return contentSize;
			}
		}

		// Token: 0x1700192A RID: 6442
		// (get) Token: 0x0600684D RID: 26701 RVA: 0x001D6748 File Offset: 0x001D4948
		internal bool FinitePage
		{
			get
			{
				return this._ptsPage.FinitePage;
			}
		}

		// Token: 0x1700192B RID: 6443
		// (get) Token: 0x0600684E RID: 26702 RVA: 0x001D6755 File Offset: 0x001D4955
		internal PageContext PageContext
		{
			get
			{
				return this._ptsPage.PageContext;
			}
		}

		// Token: 0x1700192C RID: 6444
		// (get) Token: 0x0600684F RID: 26703 RVA: 0x001D6762 File Offset: 0x001D4962
		internal bool IncrementalUpdate
		{
			get
			{
				return this._ptsPage.IncrementalUpdate;
			}
		}

		// Token: 0x1700192D RID: 6445
		// (get) Token: 0x06006850 RID: 26704 RVA: 0x001D676F File Offset: 0x001D496F
		internal StructuralCache StructuralCache
		{
			get
			{
				return this._structuralCache;
			}
		}

		// Token: 0x1700192E RID: 6446
		// (get) Token: 0x06006851 RID: 26705 RVA: 0x001D6777 File Offset: 0x001D4977
		internal int FormattedLinesCount
		{
			get
			{
				return this._formattedLinesCount;
			}
		}

		// Token: 0x1700192F RID: 6447
		// (get) Token: 0x06006852 RID: 26706 RVA: 0x001D6780 File Offset: 0x001D4980
		internal bool IsLayoutDataValid
		{
			get
			{
				bool result = false;
				if (!this.IsDisposed)
				{
					result = this._structuralCache.FormattingOwner.IsLayoutDataValid;
				}
				return result;
			}
		}

		// Token: 0x17001930 RID: 6448
		// (get) Token: 0x06006853 RID: 26707 RVA: 0x001D67A9 File Offset: 0x001D49A9
		// (set) Token: 0x06006854 RID: 26708 RVA: 0x001D67B1 File Offset: 0x001D49B1
		internal TextPointer DependentMax
		{
			get
			{
				return this._DependentMax;
			}
			set
			{
				if (this._DependentMax == null || (value != null && value.CompareTo(this._DependentMax) > 0))
				{
					this._DependentMax = value;
				}
			}
		}

		// Token: 0x17001931 RID: 6449
		// (get) Token: 0x06006855 RID: 26709 RVA: 0x001D67D4 File Offset: 0x001D49D4
		internal Rect Viewport
		{
			get
			{
				return new Rect(this.Size);
			}
		}

		// Token: 0x06006856 RID: 26710 RVA: 0x001D67E4 File Offset: 0x001D49E4
		private void Dispose(bool disposing)
		{
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) == 0)
			{
				if (disposing)
				{
					if (this.PageVisual != null)
					{
						this.DestroyVisualLinks(this.PageVisual);
						this.PageVisual.Children.Clear();
						this.PageVisual.ClearDrawingContext();
					}
					if (this._ptsPage != null)
					{
						this._ptsPage.Dispose();
					}
				}
				try
				{
					if (disposing)
					{
						base.OnPageDestroyed(EventArgs.Empty);
					}
				}
				finally
				{
					this._ptsPage = null;
					this._structuralCache = null;
					this._textView = null;
					this._DependentMax = null;
				}
			}
		}

		// Token: 0x06006857 RID: 26711 RVA: 0x001D6884 File Offset: 0x001D4A84
		private void UpdateVisual()
		{
			if (this.PageVisual == null)
			{
				base.SetVisual(new PageVisual(this));
			}
			if (this._visualNeedsUpdate)
			{
				this.PageVisual.DrawBackground((Brush)this._structuralCache.PropertyOwner.GetValue(FlowDocument.BackgroundProperty), new Rect(this._partitionSize));
				ContainerVisual containerVisual = null;
				using (this._structuralCache.SetDocumentVisualValidationContext(this))
				{
					containerVisual = this._ptsPage.GetPageVisual();
					this._structuralCache.DetectInvalidOperation();
				}
				this.PageVisual.Child = containerVisual;
				FlowDirection childFD = (FlowDirection)this._structuralCache.PropertyOwner.GetValue(FlowDocument.FlowDirectionProperty);
				PtsHelper.UpdateMirroringTransform(FlowDirection.LeftToRight, childFD, containerVisual, this.Size.Width);
				using (this._structuralCache.SetDocumentVisualValidationContext(this))
				{
					this._ptsPage.ClearUpdateInfo();
					this._structuralCache.DetectInvalidOperation();
				}
				this._visualNeedsUpdate = false;
			}
		}

		// Token: 0x06006858 RID: 26712 RVA: 0x001D69A4 File Offset: 0x001D4BA4
		private void OnBeforeFormatPage()
		{
			if (this._visualNeedsUpdate)
			{
				this._ptsPage.ClearUpdateInfo();
			}
		}

		// Token: 0x06006859 RID: 26713 RVA: 0x001D69B9 File Offset: 0x001D4BB9
		private void OnAfterFormatPage()
		{
			if (this._textView != null)
			{
				this._textView.Invalidate();
			}
			this._visualNeedsUpdate = true;
		}

		// Token: 0x0600685A RID: 26714 RVA: 0x001D69D8 File Offset: 0x001D4BD8
		private TextPointer FindElementPosition(IInputElement e, bool isLimitedToTextView)
		{
			TextPointer textPointer = null;
			if (e is TextElement)
			{
				if ((e as TextElement).TextContainer == this._structuralCache.TextContainer)
				{
					textPointer = new TextPointer((e as TextElement).ElementStart);
				}
			}
			else
			{
				if (this._structuralCache.TextContainer.Start == null || this._structuralCache.TextContainer.End == null)
				{
					return null;
				}
				TextPointer textPointer2 = new TextPointer(this._structuralCache.TextContainer.Start);
				while (textPointer == null && ((ITextPointer)textPointer2).CompareTo(this._structuralCache.TextContainer.End) < 0)
				{
					TextPointerContext pointerContext = textPointer2.GetPointerContext(LogicalDirection.Forward);
					if (pointerContext == TextPointerContext.EmbeddedElement)
					{
						DependencyObject adjacentElement = textPointer2.GetAdjacentElement(LogicalDirection.Forward);
						if ((adjacentElement is ContentElement || adjacentElement is UIElement) && (adjacentElement == e as ContentElement || adjacentElement == e as UIElement))
						{
							textPointer = new TextPointer(textPointer2);
						}
					}
					textPointer2.MoveToNextContextPosition(LogicalDirection.Forward);
				}
			}
			if (textPointer != null && isLimitedToTextView)
			{
				this._textView = this.GetTextView();
				Invariant.Assert(this._textView != null);
				for (int i = 0; i < ((ITextView)this._textView).TextSegments.Count; i++)
				{
					if (((ITextPointer)textPointer).CompareTo(((ITextView)this._textView).TextSegments[i].Start) >= 0 && ((ITextPointer)textPointer).CompareTo(((ITextView)this._textView).TextSegments[i].End) < 0)
					{
						return textPointer;
					}
				}
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x0600685B RID: 26715 RVA: 0x001D6B54 File Offset: 0x001D4D54
		private void DestroyVisualLinks(ContainerVisual visual)
		{
			VisualCollection children = visual.Children;
			if (children != null)
			{
				for (int i = 0; i < children.Count; i++)
				{
					if (children[i] is UIElementIsland)
					{
						children.RemoveAt(i);
					}
					else
					{
						Invariant.Assert(children[i] is ContainerVisual, "The children should always derive from ContainerVisual");
						this.DestroyVisualLinks((ContainerVisual)children[i]);
					}
				}
			}
		}

		// Token: 0x0600685C RID: 26716 RVA: 0x001D6BBE File Offset: 0x001D4DBE
		private void ValidateTextView()
		{
			if (this._textView != null)
			{
				this._textView.OnUpdated();
			}
		}

		// Token: 0x0600685D RID: 26717 RVA: 0x001D6BD4 File Offset: 0x001D4DD4
		private TextDocumentView GetTextView()
		{
			TextDocumentView textDocumentView = (TextDocumentView)((IServiceProvider)this).GetService(typeof(ITextView));
			Invariant.Assert(textDocumentView != null);
			return textDocumentView;
		}

		// Token: 0x17001932 RID: 6450
		// (get) Token: 0x0600685E RID: 26718 RVA: 0x001D6C01 File Offset: 0x001D4E01
		private PageVisual PageVisual
		{
			get
			{
				return base.Visual as PageVisual;
			}
		}

		// Token: 0x0600685F RID: 26719 RVA: 0x001D6C10 File Offset: 0x001D4E10
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextView))
			{
				if (this._textView == null)
				{
					this._textView = new TextDocumentView(this, this._structuralCache.TextContainer);
				}
				return this._textView;
			}
			return null;
		}

		// Token: 0x06006860 RID: 26720 RVA: 0x001D6C6A File Offset: 0x001D4E6A
		IInputElement IContentHost.InputHitTest(Point point)
		{
			return this.InputHitTestCore(point);
		}

		// Token: 0x06006861 RID: 26721 RVA: 0x001D6C73 File Offset: 0x001D4E73
		ReadOnlyCollection<Rect> IContentHost.GetRectangles(ContentElement child)
		{
			return this.GetRectanglesCore(child, true);
		}

		// Token: 0x17001933 RID: 6451
		// (get) Token: 0x06006862 RID: 26722 RVA: 0x001D6C7D File Offset: 0x001D4E7D
		IEnumerator<IInputElement> IContentHost.HostedElements
		{
			get
			{
				return this.HostedElementsCore;
			}
		}

		// Token: 0x06006863 RID: 26723 RVA: 0x001D6C85 File Offset: 0x001D4E85
		void IContentHost.OnChildDesiredSizeChanged(UIElement child)
		{
			this.OnChildDesiredSizeChangedCore(child);
		}

		// Token: 0x040033C8 RID: 13256
		private PtsPage _ptsPage;

		// Token: 0x040033C9 RID: 13257
		private StructuralCache _structuralCache;

		// Token: 0x040033CA RID: 13258
		private int _formattedLinesCount;

		// Token: 0x040033CB RID: 13259
		private TextDocumentView _textView;

		// Token: 0x040033CC RID: 13260
		private Size _partitionSize;

		// Token: 0x040033CD RID: 13261
		private Thickness _pageMargin;

		// Token: 0x040033CE RID: 13262
		private int _disposed;

		// Token: 0x040033CF RID: 13263
		private TextPointer _DependentMax;

		// Token: 0x040033D0 RID: 13264
		private bool _visualNeedsUpdate;

		// Token: 0x040033D1 RID: 13265
		private double _lastFormatWidth;
	}
}
