using System;
using System.Printing;
using System.Security;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Serialization;
using MS.Internal;
using MS.Internal.PresentationFramework;
using MS.Internal.Printing;

namespace System.Windows.Controls
{
	/// <summary>Invokes a standard Microsoft Windows print dialog box that configures a <see cref="T:System.Printing.PrintTicket" /> and <see cref="T:System.Printing.PrintQueue" /> according to user input and then prints a document. </summary>
	// Token: 0x0200051B RID: 1307
	public class PrintDialog
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.PrintDialog" /> class.</summary>
		// Token: 0x0600546C RID: 21612 RVA: 0x001760B8 File Offset: 0x001742B8
		[SecurityCritical]
		public PrintDialog()
		{
			this._dialogInvoked = false;
			this._printQueue = null;
			this._printTicket = null;
			this._isPrintableAreaWidthUpdated = false;
			this._isPrintableAreaHeightUpdated = false;
			this._pageRangeSelection = PageRangeSelection.AllPages;
			this._minPage = 1U;
			this._maxPage = 9999U;
			this._userPageRangeEnabled = false;
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Controls.PageRangeSelection" /> for this instance of <see cref="T:System.Windows.Controls.PrintDialog" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.PageRangeSelection" /> value that represents the type of page range to print. </returns>
		// Token: 0x17001485 RID: 5253
		// (get) Token: 0x0600546D RID: 21613 RVA: 0x0017610E File Offset: 0x0017430E
		// (set) Token: 0x0600546E RID: 21614 RVA: 0x00176116 File Offset: 0x00174316
		public PageRangeSelection PageRangeSelection
		{
			get
			{
				return this._pageRangeSelection;
			}
			set
			{
				this._pageRangeSelection = value;
			}
		}

		/// <summary>Gets or sets the range of pages to print when <see cref="P:System.Windows.Controls.PrintDialog.PageRangeSelection" /> is set to <see cref="F:System.Windows.Controls.PageRangeSelection.UserPages" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Controls.PageRange" /> that represents the range of pages that are printed. </returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Windows.Controls.PageRange" /> object that is being used to set the property has either the beginning of the range or the end of the range set to a value that is less than 1.</exception>
		// Token: 0x17001486 RID: 5254
		// (get) Token: 0x0600546F RID: 21615 RVA: 0x0017611F File Offset: 0x0017431F
		// (set) Token: 0x06005470 RID: 21616 RVA: 0x00176128 File Offset: 0x00174328
		public PageRange PageRange
		{
			get
			{
				return this._pageRange;
			}
			set
			{
				if (value.PageTo <= 0 || value.PageFrom <= 0)
				{
					throw new ArgumentException(SR.Get("PrintDialogInvalidPageRange"), "PageRange");
				}
				this._pageRange = value;
				if (this._pageRange.PageFrom > this._pageRange.PageTo)
				{
					int pageFrom = this._pageRange.PageFrom;
					this._pageRange.PageFrom = this._pageRange.PageTo;
					this._pageRange.PageTo = pageFrom;
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether users of the Print dialog box have the option to specify ranges of pages to print.</summary>
		/// <returns>
		///     <see langword="true" /> if the option is available; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001487 RID: 5255
		// (get) Token: 0x06005471 RID: 21617 RVA: 0x001761AB File Offset: 0x001743AB
		// (set) Token: 0x06005472 RID: 21618 RVA: 0x001761B3 File Offset: 0x001743B3
		public bool UserPageRangeEnabled
		{
			get
			{
				return this._userPageRangeEnabled;
			}
			set
			{
				this._userPageRangeEnabled = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the option to print the selected pages is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the option to print the selected pages is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001488 RID: 5256
		// (get) Token: 0x06005473 RID: 21619 RVA: 0x001761BC File Offset: 0x001743BC
		// (set) Token: 0x06005474 RID: 21620 RVA: 0x001761C4 File Offset: 0x001743C4
		public bool SelectedPagesEnabled
		{
			get
			{
				return this._selectedPagesEnabled;
			}
			set
			{
				this._selectedPagesEnabled = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the option to print the current page is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the option to print the current page is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001489 RID: 5257
		// (get) Token: 0x06005475 RID: 21621 RVA: 0x001761CD File Offset: 0x001743CD
		// (set) Token: 0x06005476 RID: 21622 RVA: 0x001761D5 File Offset: 0x001743D5
		public bool CurrentPageEnabled
		{
			get
			{
				return this._currentPageEnabled;
			}
			set
			{
				this._currentPageEnabled = value;
			}
		}

		/// <summary>Gets or sets the lowest page number that is allowed in page ranges.</summary>
		/// <returns>A <see cref="T:System.UInt32" /> that represents the lowest page number that can be used in a page range in the Print dialog box. </returns>
		/// <exception cref="T:System.ArgumentException">The property is being set to less than 1.</exception>
		// Token: 0x1700148A RID: 5258
		// (get) Token: 0x06005477 RID: 21623 RVA: 0x001761DE File Offset: 0x001743DE
		// (set) Token: 0x06005478 RID: 21624 RVA: 0x001761E6 File Offset: 0x001743E6
		public uint MinPage
		{
			get
			{
				return this._minPage;
			}
			set
			{
				if (this._minPage <= 0U)
				{
					throw new ArgumentException(SR.Get("PrintDialogZeroNotAllowed", new object[]
					{
						"MinPage"
					}));
				}
				this._minPage = value;
			}
		}

		/// <summary>Gets or sets the highest page number that is allowed in page ranges.</summary>
		/// <returns>A <see cref="T:System.UInt32" /> that represents the highest page number that can be used in a page range in the Print dialog box. </returns>
		/// <exception cref="T:System.ArgumentException">The property is being set to less than 1.</exception>
		// Token: 0x1700148B RID: 5259
		// (get) Token: 0x06005479 RID: 21625 RVA: 0x00176216 File Offset: 0x00174416
		// (set) Token: 0x0600547A RID: 21626 RVA: 0x0017621E File Offset: 0x0017441E
		public uint MaxPage
		{
			get
			{
				return this._maxPage;
			}
			set
			{
				if (this._maxPage <= 0U)
				{
					throw new ArgumentException(SR.Get("PrintDialogZeroNotAllowed", new object[]
					{
						"MaxPage"
					}));
				}
				this._maxPage = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Printing.PrintQueue" /> that represents the printer that is selected.</summary>
		/// <returns>The <see cref="T:System.Printing.PrintQueue" /> that the user selected. </returns>
		// Token: 0x1700148C RID: 5260
		// (get) Token: 0x0600547B RID: 21627 RVA: 0x0017624E File Offset: 0x0017444E
		// (set) Token: 0x0600547C RID: 21628 RVA: 0x0017626F File Offset: 0x0017446F
		public PrintQueue PrintQueue
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandPrintDialogPermissions();
				if (this._printQueue == null)
				{
					this._printQueue = this.AcquireDefaultPrintQueue();
				}
				return this._printQueue;
			}
			[SecurityCritical]
			set
			{
				SecurityHelper.DemandPrintDialogPermissions();
				this._printQueue = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Printing.PrintTicket" /> that is used by the <see cref="T:System.Windows.Controls.PrintDialog" /> when the user clicks Print for the current print job.</summary>
		/// <returns>A <see cref="T:System.Printing.PrintTicket" /> that is used the next time the Print button in the dialog box is clicked.Setting this <see cref="P:System.Windows.Controls.PrintDialog.PrintTicket" /> property does not validate or modify the specified <see cref="T:System.Printing.PrintTicket" /> for a particular <see cref="T:System.Printing.PrintQueue" />.  If needed, use the <see cref="M:System.Printing.PrintQueue.MergeAndValidatePrintTicket(System.Printing.PrintTicket,System.Printing.PrintTicket)" /> method to create a <see cref="T:System.Printing.PrintQueue" />-specific <see cref="T:System.Printing.PrintTicket" /> that is valid for a given printer.</returns>
		// Token: 0x1700148D RID: 5261
		// (get) Token: 0x0600547D RID: 21629 RVA: 0x0017627D File Offset: 0x0017447D
		// (set) Token: 0x0600547E RID: 21630 RVA: 0x001762A4 File Offset: 0x001744A4
		public PrintTicket PrintTicket
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandPrintDialogPermissions();
				if (this._printTicket == null)
				{
					this._printTicket = this.AcquireDefaultPrintTicket(this.PrintQueue);
				}
				return this._printTicket;
			}
			[SecurityCritical]
			set
			{
				SecurityHelper.DemandPrintDialogPermissions();
				this._printTicket = value;
			}
		}

		/// <summary>Gets the width of the printable area of the page.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the width of the printable page area.</returns>
		// Token: 0x1700148E RID: 5262
		// (get) Token: 0x0600547F RID: 21631 RVA: 0x001762B2 File Offset: 0x001744B2
		public double PrintableAreaWidth
		{
			get
			{
				if ((!this._isPrintableAreaWidthUpdated && !this._isPrintableAreaHeightUpdated) || (this._isPrintableAreaWidthUpdated && !this._isPrintableAreaHeightUpdated))
				{
					this._isPrintableAreaWidthUpdated = true;
					this._isPrintableAreaHeightUpdated = false;
					this.UpdatePrintableAreaSize();
				}
				return this._printableAreaWidth;
			}
		}

		/// <summary>Gets the height of the printable area of the page.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the height of the printable page area.</returns>
		// Token: 0x1700148F RID: 5263
		// (get) Token: 0x06005480 RID: 21632 RVA: 0x001762EE File Offset: 0x001744EE
		public double PrintableAreaHeight
		{
			get
			{
				if ((!this._isPrintableAreaWidthUpdated && !this._isPrintableAreaHeightUpdated) || (!this._isPrintableAreaWidthUpdated && this._isPrintableAreaHeightUpdated))
				{
					this._isPrintableAreaWidthUpdated = false;
					this._isPrintableAreaHeightUpdated = true;
					this.UpdatePrintableAreaSize();
				}
				return this._printableAreaHeight;
			}
		}

		/// <summary>Invokes the <see cref="T:System.Windows.Controls.PrintDialog" /> as a modal dialog box. </summary>
		/// <returns>
		///     <see langword="true" /> if a user clicks Print; <see langword="false" /> if a user clicks Cancel; or <see langword="null" /> if a user closes the dialog box without clicking Print or Cancel.</returns>
		// Token: 0x06005481 RID: 21633 RVA: 0x0017632C File Offset: 0x0017452C
		[SecurityCritical]
		public bool? ShowDialog()
		{
			this._dialogInvoked = false;
			Win32PrintDialog win32PrintDialog = new Win32PrintDialog();
			win32PrintDialog.PrintTicket = this._printTicket;
			win32PrintDialog.PrintQueue = this._printQueue;
			win32PrintDialog.MinPage = Math.Max(1U, Math.Min(this._minPage, this._maxPage));
			win32PrintDialog.MaxPage = Math.Max(win32PrintDialog.MinPage, Math.Max(this._minPage, this._maxPage));
			win32PrintDialog.PageRangeEnabled = this._userPageRangeEnabled;
			win32PrintDialog.SelectedPagesEnabled = this._selectedPagesEnabled;
			win32PrintDialog.CurrentPageEnabled = this._currentPageEnabled;
			win32PrintDialog.PageRange = new PageRange(Math.Max((int)win32PrintDialog.MinPage, this._pageRange.PageFrom), Math.Min((int)win32PrintDialog.MaxPage, this._pageRange.PageTo));
			win32PrintDialog.PageRangeSelection = this._pageRangeSelection;
			uint num = win32PrintDialog.ShowDialog();
			if (num == 2U || num == 1U)
			{
				this._printTicket = win32PrintDialog.PrintTicket;
				this._printQueue = win32PrintDialog.PrintQueue;
				this._pageRange = win32PrintDialog.PageRange;
				this._pageRangeSelection = win32PrintDialog.PageRangeSelection;
				this._dialogInvoked = true;
			}
			return new bool?(num == 1U);
		}

		/// <summary>Prints a visual (non-text) object, which is derived from the <see cref="T:System.Windows.Media.Visual" /> class, to the <see cref="T:System.Printing.PrintQueue" /> that is currently selected.</summary>
		/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> to print.</param>
		/// <param name="description">A description of the job that is to be printed. This text appears in the user interface (UI) of the printer.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="visual" /> is <see langword="null" />. </exception>
		// Token: 0x06005482 RID: 21634 RVA: 0x00176454 File Offset: 0x00174654
		[SecurityCritical]
		public void PrintVisual(Visual visual, string description)
		{
			if (visual == null)
			{
				throw new ArgumentNullException("visual");
			}
			XpsDocumentWriter xpsDocumentWriter = this.CreateWriter(description);
			xpsDocumentWriter.Write(visual, this._printTicket);
			this._printableAreaWidth = 0.0;
			this._printableAreaHeight = 0.0;
			this._isPrintableAreaWidthUpdated = false;
			this._isPrintableAreaHeightUpdated = false;
			this._dialogInvoked = false;
		}

		/// <summary>Prints a <see cref="T:System.Windows.Documents.DocumentPaginator" /> object to the <see cref="T:System.Printing.PrintQueue" /> that is currently selected.</summary>
		/// <param name="documentPaginator">The <see cref="T:System.Windows.Documents.DocumentPaginator" /> object to print.</param>
		/// <param name="description">A description of the job that is to be printed. This text appears in the user interface (UI) of the printer.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="documentPaginator" /> is <see langword="null" />. </exception>
		// Token: 0x06005483 RID: 21635 RVA: 0x001764B8 File Offset: 0x001746B8
		[SecurityCritical]
		public void PrintDocument(DocumentPaginator documentPaginator, string description)
		{
			if (documentPaginator == null)
			{
				throw new ArgumentNullException("documentPaginator");
			}
			XpsDocumentWriter xpsDocumentWriter = this.CreateWriter(description);
			xpsDocumentWriter.Write(documentPaginator, this._printTicket);
			this._printableAreaWidth = 0.0;
			this._printableAreaHeight = 0.0;
			this._isPrintableAreaWidthUpdated = false;
			this._isPrintableAreaHeightUpdated = false;
			this._dialogInvoked = false;
		}

		// Token: 0x06005484 RID: 21636 RVA: 0x0017651C File Offset: 0x0017471C
		[SecurityCritical]
		private PrintQueue AcquireDefaultPrintQueue()
		{
			PrintQueue result = null;
			SystemDrawingHelper.NewDefaultPrintingPermission().Assert();
			try
			{
				LocalPrintServer localPrintServer = new LocalPrintServer();
				result = localPrintServer.DefaultPrintQueue;
			}
			catch (PrintSystemException)
			{
				result = null;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x06005485 RID: 21637 RVA: 0x0017656C File Offset: 0x0017476C
		[SecurityCritical]
		private PrintTicket AcquireDefaultPrintTicket(PrintQueue printQueue)
		{
			PrintTicket printTicket = null;
			SystemDrawingHelper.NewDefaultPrintingPermission().Assert();
			try
			{
				if (printQueue != null)
				{
					printTicket = printQueue.UserPrintTicket;
					if (printTicket == null)
					{
						printTicket = printQueue.DefaultPrintTicket;
					}
				}
			}
			catch (PrintSystemException)
			{
				printTicket = null;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			if (printTicket == null)
			{
				printTicket = new PrintTicket();
			}
			return printTicket;
		}

		// Token: 0x06005486 RID: 21638 RVA: 0x001765CC File Offset: 0x001747CC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void UpdatePrintableAreaSize()
		{
			PrintQueue printQueue = null;
			PrintTicket printTicket = null;
			this.PickCorrectPrintingEnvironment(ref printQueue, ref printTicket);
			PrintCapabilities printCapabilities = null;
			if (printQueue != null)
			{
				printCapabilities = printQueue.GetPrintCapabilities(printTicket);
			}
			if (printCapabilities != null && printCapabilities.OrientedPageMediaWidth != null && printCapabilities.OrientedPageMediaHeight != null)
			{
				this._printableAreaWidth = printCapabilities.OrientedPageMediaWidth.Value;
				this._printableAreaHeight = printCapabilities.OrientedPageMediaHeight.Value;
				return;
			}
			this._printableAreaWidth = 816.0;
			this._printableAreaHeight = 1056.0;
			if (printTicket.PageMediaSize != null && printTicket.PageMediaSize.Width != null && printTicket.PageMediaSize.Height != null)
			{
				this._printableAreaWidth = printTicket.PageMediaSize.Width.Value;
				this._printableAreaHeight = printTicket.PageMediaSize.Height.Value;
			}
			if (printTicket.PageOrientation != null)
			{
				PageOrientation value = printTicket.PageOrientation.Value;
				if (value == PageOrientation.Landscape || value == PageOrientation.ReverseLandscape)
				{
					double printableAreaWidth = this._printableAreaWidth;
					this._printableAreaWidth = this._printableAreaHeight;
					this._printableAreaHeight = printableAreaWidth;
				}
			}
		}

		// Token: 0x06005487 RID: 21639 RVA: 0x00176710 File Offset: 0x00174910
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private XpsDocumentWriter CreateWriter(string description)
		{
			PrintQueue printQueue = null;
			PrintTicket printTicket = null;
			XpsDocumentWriter xpsDocumentWriter = null;
			this.PickCorrectPrintingEnvironment(ref printQueue, ref printTicket);
			SystemDrawingHelper.NewDefaultPrintingPermission().Assert();
			try
			{
				if (printQueue != null)
				{
					printQueue.CurrentJobSettings.Description = description;
				}
				xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(printQueue);
				PrintDialog.PrintDlgPrintTicketEventHandler @object = new PrintDialog.PrintDlgPrintTicketEventHandler(printTicket);
				xpsDocumentWriter.WritingPrintTicketRequired += @object.SetPrintTicket;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return xpsDocumentWriter;
		}

		// Token: 0x06005488 RID: 21640 RVA: 0x00176780 File Offset: 0x00174980
		[SecurityCritical]
		private void PickCorrectPrintingEnvironment(ref PrintQueue printQueue, ref PrintTicket printTicket)
		{
			if (!this._dialogInvoked)
			{
				try
				{
					SecurityHelper.DemandPrintDialogPermissions();
				}
				catch (SecurityException)
				{
					throw new PrintDialogException(SR.Get("PartialTrustPrintDialogMustBeInvoked"));
				}
			}
			if (this._printQueue == null)
			{
				this._printQueue = this.AcquireDefaultPrintQueue();
			}
			if (this._printTicket == null)
			{
				this._printTicket = this.AcquireDefaultPrintTicket(this._printQueue);
			}
			printQueue = this._printQueue;
			printTicket = this._printTicket;
		}

		// Token: 0x04002D92 RID: 11666
		[SecurityCritical]
		private PrintTicket _printTicket;

		// Token: 0x04002D93 RID: 11667
		[SecurityCritical]
		private PrintQueue _printQueue;

		// Token: 0x04002D94 RID: 11668
		[SecurityCritical]
		private bool _dialogInvoked;

		// Token: 0x04002D95 RID: 11669
		private PageRangeSelection _pageRangeSelection;

		// Token: 0x04002D96 RID: 11670
		private PageRange _pageRange;

		// Token: 0x04002D97 RID: 11671
		private bool _userPageRangeEnabled;

		// Token: 0x04002D98 RID: 11672
		private bool _selectedPagesEnabled;

		// Token: 0x04002D99 RID: 11673
		private bool _currentPageEnabled;

		// Token: 0x04002D9A RID: 11674
		private uint _minPage;

		// Token: 0x04002D9B RID: 11675
		private uint _maxPage;

		// Token: 0x04002D9C RID: 11676
		private double _printableAreaWidth;

		// Token: 0x04002D9D RID: 11677
		private double _printableAreaHeight;

		// Token: 0x04002D9E RID: 11678
		private bool _isPrintableAreaWidthUpdated;

		// Token: 0x04002D9F RID: 11679
		private bool _isPrintableAreaHeightUpdated;

		// Token: 0x020009B1 RID: 2481
		internal class PrintDlgPrintTicketEventHandler
		{
			// Token: 0x0600884E RID: 34894 RVA: 0x00251FEB File Offset: 0x002501EB
			[SecurityCritical]
			[SecurityTreatAsSafe]
			public PrintDlgPrintTicketEventHandler(PrintTicket printTicket)
			{
				this._printTicket = printTicket;
			}

			// Token: 0x0600884F RID: 34895 RVA: 0x00251FFA File Offset: 0x002501FA
			[SecurityCritical]
			[SecurityTreatAsSafe]
			public void SetPrintTicket(object sender, WritingPrintTicketRequiredEventArgs args)
			{
				if (args.CurrentPrintTicketLevel == PrintTicketLevel.FixedDocumentSequencePrintTicket)
				{
					args.CurrentPrintTicket = this._printTicket;
				}
			}

			// Token: 0x04004523 RID: 17699
			[SecurityCritical]
			private PrintTicket _printTicket;
		}
	}
}
