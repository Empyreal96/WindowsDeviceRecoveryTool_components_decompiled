using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Threading;

namespace MS.Internal.Documents
{
	// Token: 0x020006D0 RID: 1744
	internal class ReaderPageViewer : FlowDocumentPageViewer, IFlowDocumentViewer
	{
		// Token: 0x060070BF RID: 28863 RVA: 0x00204988 File Offset: 0x00202B88
		protected override void OnPrintCompleted()
		{
			base.OnPrintCompleted();
			if (this._printCompleted != null)
			{
				this._printCompleted(this, EventArgs.Empty);
			}
		}

		// Token: 0x060070C0 RID: 28864 RVA: 0x002049A9 File Offset: 0x00202BA9
		protected override void OnPrintCommand()
		{
			base.OnPrintCommand();
			if (this._printStarted != null && base.IsPrinting)
			{
				this._printStarted(this, EventArgs.Empty);
			}
		}

		// Token: 0x060070C1 RID: 28865 RVA: 0x002049D4 File Offset: 0x00202BD4
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == DocumentViewerBase.PageCountProperty || e.Property == DocumentViewerBase.MasterPageNumberProperty || e.Property == DocumentViewerBase.CanGoToPreviousPageProperty || e.Property == DocumentViewerBase.CanGoToNextPageProperty)
			{
				if (!this._raisePageNumberChanged && !this._raisePageCountChanged)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.RaisePropertyChangedAsync), null);
				}
				if (e.Property == DocumentViewerBase.PageCountProperty)
				{
					this._raisePageCountChanged = true;
					base.CoerceValue(DocumentViewerBase.CanGoToNextPageProperty);
					return;
				}
				if (e.Property == DocumentViewerBase.MasterPageNumberProperty)
				{
					this._raisePageNumberChanged = true;
					base.CoerceValue(DocumentViewerBase.CanGoToNextPageProperty);
					return;
				}
				this._raisePageNumberChanged = true;
			}
		}

		// Token: 0x060070C2 RID: 28866 RVA: 0x00204A94 File Offset: 0x00202C94
		private object SetTextSelection(object arg)
		{
			ITextSelection textSelection = arg as ITextSelection;
			FlowDocument flowDocument = base.Document as FlowDocument;
			if (textSelection != null && flowDocument != null && textSelection.AnchorPosition != null && textSelection.AnchorPosition.TextContainer == flowDocument.StructuralCache.TextContainer)
			{
				ITextSelection textSelection2 = flowDocument.StructuralCache.TextContainer.TextSelection;
				if (textSelection2 != null)
				{
					textSelection2.SetCaretToPosition(textSelection.AnchorPosition, textSelection.MovingPosition.LogicalDirection, true, true);
					textSelection2.ExtendToPosition(textSelection.MovingPosition);
				}
			}
			return null;
		}

		// Token: 0x060070C3 RID: 28867 RVA: 0x00204B14 File Offset: 0x00202D14
		private object RaisePropertyChangedAsync(object arg)
		{
			if (this._raisePageCountChanged)
			{
				if (this._pageCountChanged != null)
				{
					this._pageCountChanged(this, EventArgs.Empty);
				}
				this._raisePageCountChanged = false;
			}
			if (this._raisePageNumberChanged)
			{
				if (this._pageNumberChanged != null)
				{
					this._pageNumberChanged(this, EventArgs.Empty);
				}
				this._raisePageNumberChanged = false;
			}
			return null;
		}

		// Token: 0x060070C4 RID: 28868 RVA: 0x001A5D4A File Offset: 0x001A3F4A
		void IFlowDocumentViewer.PreviousPage()
		{
			this.OnPreviousPageCommand();
		}

		// Token: 0x060070C5 RID: 28869 RVA: 0x001A5D52 File Offset: 0x001A3F52
		void IFlowDocumentViewer.NextPage()
		{
			this.OnNextPageCommand();
		}

		// Token: 0x060070C6 RID: 28870 RVA: 0x001A5D5A File Offset: 0x001A3F5A
		void IFlowDocumentViewer.FirstPage()
		{
			this.OnFirstPageCommand();
		}

		// Token: 0x060070C7 RID: 28871 RVA: 0x001A5D62 File Offset: 0x001A3F62
		void IFlowDocumentViewer.LastPage()
		{
			this.OnLastPageCommand();
		}

		// Token: 0x060070C8 RID: 28872 RVA: 0x001A5D73 File Offset: 0x001A3F73
		void IFlowDocumentViewer.Print()
		{
			this.OnPrintCommand();
		}

		// Token: 0x060070C9 RID: 28873 RVA: 0x001A5D7B File Offset: 0x001A3F7B
		void IFlowDocumentViewer.CancelPrint()
		{
			this.OnCancelPrintCommand();
		}

		// Token: 0x060070CA RID: 28874 RVA: 0x00204B72 File Offset: 0x00202D72
		void IFlowDocumentViewer.ShowFindResult(ITextRange findResult)
		{
			if (findResult.Start is ContentPosition)
			{
				base.BringContentPositionIntoView((ContentPosition)findResult.Start);
			}
		}

		// Token: 0x060070CB RID: 28875 RVA: 0x00204B93 File Offset: 0x00202D93
		bool IFlowDocumentViewer.CanGoToPage(int pageNumber)
		{
			return this.CanGoToPage(pageNumber);
		}

		// Token: 0x060070CC RID: 28876 RVA: 0x001A5D6A File Offset: 0x001A3F6A
		void IFlowDocumentViewer.GoToPage(int pageNumber)
		{
			this.OnGoToPageCommand(pageNumber);
		}

		// Token: 0x060070CD RID: 28877 RVA: 0x00204B9C File Offset: 0x00202D9C
		void IFlowDocumentViewer.SetDocument(FlowDocument document)
		{
			base.Document = document;
		}

		// Token: 0x17001ACA RID: 6858
		// (get) Token: 0x060070CE RID: 28878 RVA: 0x00204BA5 File Offset: 0x00202DA5
		// (set) Token: 0x060070CF RID: 28879 RVA: 0x00204BAD File Offset: 0x00202DAD
		ContentPosition IFlowDocumentViewer.ContentPosition
		{
			get
			{
				return base.ContentPosition;
			}
			set
			{
				if (value != null && base.Document != null)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(base.BringContentPositionIntoView), value);
				}
			}
		}

		// Token: 0x17001ACB RID: 6859
		// (get) Token: 0x060070D0 RID: 28880 RVA: 0x00204BD4 File Offset: 0x00202DD4
		// (set) Token: 0x060070D1 RID: 28881 RVA: 0x00204BDC File Offset: 0x00202DDC
		ITextSelection IFlowDocumentViewer.TextSelection
		{
			get
			{
				return base.Selection;
			}
			set
			{
				if (value != null && base.Document != null)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.SetTextSelection), value);
				}
			}
		}

		// Token: 0x17001ACC RID: 6860
		// (get) Token: 0x060070D2 RID: 28882 RVA: 0x00204C03 File Offset: 0x00202E03
		bool IFlowDocumentViewer.CanGoToPreviousPage
		{
			get
			{
				return this.CanGoToPreviousPage;
			}
		}

		// Token: 0x17001ACD RID: 6861
		// (get) Token: 0x060070D3 RID: 28883 RVA: 0x00204C0B File Offset: 0x00202E0B
		bool IFlowDocumentViewer.CanGoToNextPage
		{
			get
			{
				return this.CanGoToNextPage;
			}
		}

		// Token: 0x17001ACE RID: 6862
		// (get) Token: 0x060070D4 RID: 28884 RVA: 0x00204C13 File Offset: 0x00202E13
		int IFlowDocumentViewer.PageNumber
		{
			get
			{
				return this.MasterPageNumber;
			}
		}

		// Token: 0x17001ACF RID: 6863
		// (get) Token: 0x060070D5 RID: 28885 RVA: 0x00204C1B File Offset: 0x00202E1B
		int IFlowDocumentViewer.PageCount
		{
			get
			{
				return base.PageCount;
			}
		}

		// Token: 0x14000144 RID: 324
		// (add) Token: 0x060070D6 RID: 28886 RVA: 0x00204C23 File Offset: 0x00202E23
		// (remove) Token: 0x060070D7 RID: 28887 RVA: 0x00204C3C File Offset: 0x00202E3C
		event EventHandler IFlowDocumentViewer.PageNumberChanged
		{
			add
			{
				this._pageNumberChanged = (EventHandler)Delegate.Combine(this._pageNumberChanged, value);
			}
			remove
			{
				this._pageNumberChanged = (EventHandler)Delegate.Remove(this._pageNumberChanged, value);
			}
		}

		// Token: 0x14000145 RID: 325
		// (add) Token: 0x060070D8 RID: 28888 RVA: 0x00204C55 File Offset: 0x00202E55
		// (remove) Token: 0x060070D9 RID: 28889 RVA: 0x00204C6E File Offset: 0x00202E6E
		event EventHandler IFlowDocumentViewer.PageCountChanged
		{
			add
			{
				this._pageCountChanged = (EventHandler)Delegate.Combine(this._pageCountChanged, value);
			}
			remove
			{
				this._pageCountChanged = (EventHandler)Delegate.Remove(this._pageCountChanged, value);
			}
		}

		// Token: 0x14000146 RID: 326
		// (add) Token: 0x060070DA RID: 28890 RVA: 0x00204C87 File Offset: 0x00202E87
		// (remove) Token: 0x060070DB RID: 28891 RVA: 0x00204CA0 File Offset: 0x00202EA0
		event EventHandler IFlowDocumentViewer.PrintStarted
		{
			add
			{
				this._printStarted = (EventHandler)Delegate.Combine(this._printStarted, value);
			}
			remove
			{
				this._printStarted = (EventHandler)Delegate.Remove(this._printStarted, value);
			}
		}

		// Token: 0x14000147 RID: 327
		// (add) Token: 0x060070DC RID: 28892 RVA: 0x00204CB9 File Offset: 0x00202EB9
		// (remove) Token: 0x060070DD RID: 28893 RVA: 0x00204CD2 File Offset: 0x00202ED2
		event EventHandler IFlowDocumentViewer.PrintCompleted
		{
			add
			{
				this._printCompleted = (EventHandler)Delegate.Combine(this._printCompleted, value);
			}
			remove
			{
				this._printCompleted = (EventHandler)Delegate.Remove(this._printCompleted, value);
			}
		}

		// Token: 0x040036FA RID: 14074
		private EventHandler _pageNumberChanged;

		// Token: 0x040036FB RID: 14075
		private EventHandler _pageCountChanged;

		// Token: 0x040036FC RID: 14076
		private EventHandler _printCompleted;

		// Token: 0x040036FD RID: 14077
		private EventHandler _printStarted;

		// Token: 0x040036FE RID: 14078
		private bool _raisePageNumberChanged;

		// Token: 0x040036FF RID: 14079
		private bool _raisePageCountChanged;
	}
}
