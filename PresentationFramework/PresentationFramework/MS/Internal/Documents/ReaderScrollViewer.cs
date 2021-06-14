using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;

namespace MS.Internal.Documents
{
	// Token: 0x020006CF RID: 1743
	internal class ReaderScrollViewer : FlowDocumentScrollViewer, IFlowDocumentViewer
	{
		// Token: 0x0600709F RID: 28831 RVA: 0x0020469E File Offset: 0x0020289E
		protected override void OnPrintCompleted()
		{
			base.OnPrintCompleted();
			if (this._printCompleted != null)
			{
				this._printCompleted(this, EventArgs.Empty);
			}
		}

		// Token: 0x060070A0 RID: 28832 RVA: 0x002046BF File Offset: 0x002028BF
		protected override void OnPrintCommand()
		{
			base.OnPrintCommand();
			if (this._printStarted != null && base.IsPrinting)
			{
				this._printStarted(this, EventArgs.Empty);
			}
		}

		// Token: 0x060070A1 RID: 28833 RVA: 0x002046E8 File Offset: 0x002028E8
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == FlowDocumentScrollViewer.DocumentProperty)
			{
				if (this._pageNumberChanged != null)
				{
					this._pageNumberChanged(this, EventArgs.Empty);
				}
				if (this._pageCountChanged != null)
				{
					this._pageCountChanged(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x060070A2 RID: 28834 RVA: 0x0020473C File Offset: 0x0020293C
		private bool IsValidTextSelectionForDocument(ITextSelection textSelection, FlowDocument flowDocument)
		{
			return textSelection.Start != null && textSelection.Start.TextContainer == flowDocument.StructuralCache.TextContainer;
		}

		// Token: 0x060070A3 RID: 28835 RVA: 0x00204764 File Offset: 0x00202964
		private object SetTextSelection(object arg)
		{
			ITextSelection textSelection = arg as ITextSelection;
			if (textSelection != null && base.Document != null && this.IsValidTextSelectionForDocument(textSelection, base.Document))
			{
				ITextSelection textSelection2 = base.Document.StructuralCache.TextContainer.TextSelection;
				if (textSelection2 != null)
				{
					textSelection2.SetCaretToPosition(textSelection.AnchorPosition, textSelection.MovingPosition.LogicalDirection, true, true);
					textSelection2.ExtendToPosition(textSelection.MovingPosition);
				}
			}
			return null;
		}

		// Token: 0x060070A4 RID: 28836 RVA: 0x002047D1 File Offset: 0x002029D1
		void IFlowDocumentViewer.PreviousPage()
		{
			if (base.ScrollViewer != null)
			{
				base.ScrollViewer.PageUp();
			}
		}

		// Token: 0x060070A5 RID: 28837 RVA: 0x002047E6 File Offset: 0x002029E6
		void IFlowDocumentViewer.NextPage()
		{
			if (base.ScrollViewer != null)
			{
				base.ScrollViewer.PageDown();
			}
		}

		// Token: 0x060070A6 RID: 28838 RVA: 0x002047FB File Offset: 0x002029FB
		void IFlowDocumentViewer.FirstPage()
		{
			if (base.ScrollViewer != null)
			{
				base.ScrollViewer.ScrollToHome();
			}
		}

		// Token: 0x060070A7 RID: 28839 RVA: 0x00204810 File Offset: 0x00202A10
		void IFlowDocumentViewer.LastPage()
		{
			if (base.ScrollViewer != null)
			{
				base.ScrollViewer.ScrollToEnd();
			}
		}

		// Token: 0x060070A8 RID: 28840 RVA: 0x00154CE1 File Offset: 0x00152EE1
		void IFlowDocumentViewer.Print()
		{
			this.OnPrintCommand();
		}

		// Token: 0x060070A9 RID: 28841 RVA: 0x00154CE9 File Offset: 0x00152EE9
		void IFlowDocumentViewer.CancelPrint()
		{
			this.OnCancelPrintCommand();
		}

		// Token: 0x060070AA RID: 28842 RVA: 0x00002137 File Offset: 0x00000337
		void IFlowDocumentViewer.ShowFindResult(ITextRange findResult)
		{
		}

		// Token: 0x060070AB RID: 28843 RVA: 0x00204825 File Offset: 0x00202A25
		bool IFlowDocumentViewer.CanGoToPage(int pageNumber)
		{
			return pageNumber == 1;
		}

		// Token: 0x060070AC RID: 28844 RVA: 0x0020482B File Offset: 0x00202A2B
		void IFlowDocumentViewer.GoToPage(int pageNumber)
		{
			if (pageNumber == 1 && base.ScrollViewer != null)
			{
				base.ScrollViewer.ScrollToHome();
			}
		}

		// Token: 0x060070AD RID: 28845 RVA: 0x00204844 File Offset: 0x00202A44
		void IFlowDocumentViewer.SetDocument(FlowDocument document)
		{
			base.Document = document;
		}

		// Token: 0x17001AC4 RID: 6852
		// (get) Token: 0x060070AE RID: 28846 RVA: 0x0020484D File Offset: 0x00202A4D
		// (set) Token: 0x060070AF RID: 28847 RVA: 0x00204855 File Offset: 0x00202A55
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

		// Token: 0x17001AC5 RID: 6853
		// (get) Token: 0x060070B0 RID: 28848 RVA: 0x0020487C File Offset: 0x00202A7C
		// (set) Token: 0x060070B1 RID: 28849 RVA: 0x00204884 File Offset: 0x00202A84
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

		// Token: 0x17001AC6 RID: 6854
		// (get) Token: 0x060070B2 RID: 28850 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IFlowDocumentViewer.CanGoToPreviousPage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001AC7 RID: 6855
		// (get) Token: 0x060070B3 RID: 28851 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IFlowDocumentViewer.CanGoToNextPage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001AC8 RID: 6856
		// (get) Token: 0x060070B4 RID: 28852 RVA: 0x002048AB File Offset: 0x00202AAB
		int IFlowDocumentViewer.PageNumber
		{
			get
			{
				if (base.Document == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x17001AC9 RID: 6857
		// (get) Token: 0x060070B5 RID: 28853 RVA: 0x002048AB File Offset: 0x00202AAB
		int IFlowDocumentViewer.PageCount
		{
			get
			{
				if (base.Document == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x14000140 RID: 320
		// (add) Token: 0x060070B6 RID: 28854 RVA: 0x002048B8 File Offset: 0x00202AB8
		// (remove) Token: 0x060070B7 RID: 28855 RVA: 0x002048D1 File Offset: 0x00202AD1
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

		// Token: 0x14000141 RID: 321
		// (add) Token: 0x060070B8 RID: 28856 RVA: 0x002048EA File Offset: 0x00202AEA
		// (remove) Token: 0x060070B9 RID: 28857 RVA: 0x00204903 File Offset: 0x00202B03
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

		// Token: 0x14000142 RID: 322
		// (add) Token: 0x060070BA RID: 28858 RVA: 0x0020491C File Offset: 0x00202B1C
		// (remove) Token: 0x060070BB RID: 28859 RVA: 0x00204935 File Offset: 0x00202B35
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

		// Token: 0x14000143 RID: 323
		// (add) Token: 0x060070BC RID: 28860 RVA: 0x0020494E File Offset: 0x00202B4E
		// (remove) Token: 0x060070BD RID: 28861 RVA: 0x00204967 File Offset: 0x00202B67
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

		// Token: 0x040036F6 RID: 14070
		private EventHandler _pageNumberChanged;

		// Token: 0x040036F7 RID: 14071
		private EventHandler _pageCountChanged;

		// Token: 0x040036F8 RID: 14072
		private EventHandler _printCompleted;

		// Token: 0x040036F9 RID: 14073
		private EventHandler _printStarted;
	}
}
