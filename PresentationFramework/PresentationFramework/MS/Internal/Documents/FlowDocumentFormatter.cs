using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020006C7 RID: 1735
	internal class FlowDocumentFormatter : IFlowDocumentFormatter
	{
		// Token: 0x06007001 RID: 28673 RVA: 0x00202B3F File Offset: 0x00200D3F
		internal FlowDocumentFormatter(FlowDocument document)
		{
			this._document = document;
			this._documentPage = new FlowDocumentPage(this._document.StructuralCache);
		}

		// Token: 0x06007002 RID: 28674 RVA: 0x00202B64 File Offset: 0x00200D64
		internal void Format(Size constraint)
		{
			if (this._document.StructuralCache.IsFormattingInProgress)
			{
				throw new InvalidOperationException(SR.Get("FlowDocumentFormattingReentrancy"));
			}
			if (this._document.StructuralCache.IsContentChangeInProgress)
			{
				throw new InvalidOperationException(SR.Get("TextContainerChangingReentrancyInvalid"));
			}
			if (this._document.StructuralCache.IsFormattedOnce)
			{
				if (!this._lastFormatSuccessful)
				{
					this._document.StructuralCache.InvalidateFormatCache(true);
				}
				if (!this._arrangedAfterFormat && (!this._document.StructuralCache.ForceReformat || !this._document.StructuralCache.DestroyStructure))
				{
					this._documentPage.Arrange(this._documentPage.ContentSize);
					this._documentPage.EnsureValidVisuals();
				}
			}
			this._arrangedAfterFormat = false;
			this._lastFormatSuccessful = false;
			this._isContentFormatValid = false;
			Size pageSize = this.ComputePageSize(constraint);
			Thickness pageMargin = this.ComputePageMargin();
			using (this._document.Dispatcher.DisableProcessing())
			{
				this._document.StructuralCache.IsFormattingInProgress = true;
				try
				{
					this._document.StructuralCache.BackgroundFormatInfo.ViewportHeight = constraint.Height;
					this._documentPage.FormatBottomless(pageSize, pageMargin);
				}
				finally
				{
					this._document.StructuralCache.IsFormattingInProgress = false;
				}
			}
			this._lastFormatSuccessful = true;
		}

		// Token: 0x06007003 RID: 28675 RVA: 0x00202CE4 File Offset: 0x00200EE4
		internal void Arrange(Size arrangeSize, Rect viewport)
		{
			Invariant.Assert(this._document.StructuralCache.DtrList == null || this._document.StructuralCache.DtrList.Length == 0 || (this._document.StructuralCache.DtrList.Length == 1 && this._document.StructuralCache.BackgroundFormatInfo.DoesFinalDTRCoverRestOfText));
			this._documentPage.Arrange(arrangeSize);
			this._documentPage.EnsureValidVisuals();
			this._arrangedAfterFormat = true;
			if (viewport.IsEmpty)
			{
				viewport = new Rect(0.0, 0.0, arrangeSize.Width, this._document.StructuralCache.BackgroundFormatInfo.ViewportHeight);
			}
			PTS.FSRECT fsrect = new PTS.FSRECT(viewport);
			this._documentPage.UpdateViewport(ref fsrect, true);
			this._isContentFormatValid = true;
		}

		// Token: 0x17001A9B RID: 6811
		// (get) Token: 0x06007004 RID: 28676 RVA: 0x00202DCB File Offset: 0x00200FCB
		internal FlowDocumentPage DocumentPage
		{
			get
			{
				return this._documentPage;
			}
		}

		// Token: 0x14000139 RID: 313
		// (add) Token: 0x06007005 RID: 28677 RVA: 0x00202DD4 File Offset: 0x00200FD4
		// (remove) Token: 0x06007006 RID: 28678 RVA: 0x00202E0C File Offset: 0x0020100C
		internal event EventHandler ContentInvalidated;

		// Token: 0x1400013A RID: 314
		// (add) Token: 0x06007007 RID: 28679 RVA: 0x00202E44 File Offset: 0x00201044
		// (remove) Token: 0x06007008 RID: 28680 RVA: 0x00202E7C File Offset: 0x0020107C
		internal event EventHandler Suspended;

		// Token: 0x06007009 RID: 28681 RVA: 0x00202EB4 File Offset: 0x002010B4
		private Size ComputePageSize(Size constraint)
		{
			Size result = new Size(this._document.PageWidth, double.PositiveInfinity);
			if (DoubleUtil.IsNaN(result.Width))
			{
				result.Width = constraint.Width;
				double maxPageWidth = this._document.MaxPageWidth;
				if (result.Width > maxPageWidth)
				{
					result.Width = maxPageWidth;
				}
				double minPageWidth = this._document.MinPageWidth;
				if (result.Width < minPageWidth)
				{
					result.Width = minPageWidth;
				}
			}
			if (double.IsPositiveInfinity(result.Width))
			{
				result.Width = 500.0;
			}
			return result;
		}

		// Token: 0x0600700A RID: 28682 RVA: 0x00202F54 File Offset: 0x00201154
		private Thickness ComputePageMargin()
		{
			double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(this._document);
			Thickness pagePadding = this._document.PagePadding;
			if (DoubleUtil.IsNaN(pagePadding.Left))
			{
				pagePadding.Left = lineHeightValue;
			}
			if (DoubleUtil.IsNaN(pagePadding.Top))
			{
				pagePadding.Top = lineHeightValue;
			}
			if (DoubleUtil.IsNaN(pagePadding.Right))
			{
				pagePadding.Right = lineHeightValue;
			}
			if (DoubleUtil.IsNaN(pagePadding.Bottom))
			{
				pagePadding.Bottom = lineHeightValue;
			}
			return pagePadding;
		}

		// Token: 0x0600700B RID: 28683 RVA: 0x00202FD2 File Offset: 0x002011D2
		void IFlowDocumentFormatter.OnContentInvalidated(bool affectsLayout)
		{
			if (affectsLayout)
			{
				if (!this._arrangedAfterFormat)
				{
					this._document.StructuralCache.InvalidateFormatCache(true);
				}
				this._isContentFormatValid = false;
			}
			if (this.ContentInvalidated != null)
			{
				this.ContentInvalidated(this, EventArgs.Empty);
			}
		}

		// Token: 0x0600700C RID: 28684 RVA: 0x00203010 File Offset: 0x00201210
		void IFlowDocumentFormatter.OnContentInvalidated(bool affectsLayout, ITextPointer start, ITextPointer end)
		{
			((IFlowDocumentFormatter)this).OnContentInvalidated(affectsLayout);
		}

		// Token: 0x0600700D RID: 28685 RVA: 0x00203019 File Offset: 0x00201219
		void IFlowDocumentFormatter.Suspend()
		{
			if (this.Suspended != null)
			{
				this.Suspended(this, EventArgs.Empty);
			}
		}

		// Token: 0x17001A9C RID: 6812
		// (get) Token: 0x0600700E RID: 28686 RVA: 0x00203034 File Offset: 0x00201234
		bool IFlowDocumentFormatter.IsLayoutDataValid
		{
			get
			{
				return this._documentPage != null && this._document.StructuralCache.IsFormattedOnce && !this._document.StructuralCache.ForceReformat && this._isContentFormatValid && !this._document.StructuralCache.IsContentChangeInProgress && !this._document.StructuralCache.IsFormattingInProgress;
			}
		}

		// Token: 0x040036E0 RID: 14048
		private readonly FlowDocument _document;

		// Token: 0x040036E1 RID: 14049
		private FlowDocumentPage _documentPage;

		// Token: 0x040036E2 RID: 14050
		private bool _arrangedAfterFormat;

		// Token: 0x040036E3 RID: 14051
		private bool _lastFormatSuccessful;

		// Token: 0x040036E4 RID: 14052
		private const double _defaultWidth = 500.0;

		// Token: 0x040036E5 RID: 14053
		private bool _isContentFormatValid;
	}
}
