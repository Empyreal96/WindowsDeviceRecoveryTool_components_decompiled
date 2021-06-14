using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Commands;
using MS.Internal.Documents;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Utility;

namespace System.Windows.Controls
{
	/// <summary>Represents a document viewing control that can host paginated <see cref="T:System.Windows.Documents.FixedDocument" /> content such as an <see cref="T:System.Windows.Xps.Packaging.XpsDocument" />.</summary>
	// Token: 0x020004CC RID: 1228
	[TemplatePart(Name = "PART_FindToolBarHost", Type = typeof(ContentControl))]
	[TemplatePart(Name = "PART_ContentHost", Type = typeof(ScrollViewer))]
	public class DocumentViewer : DocumentViewerBase
	{
		// Token: 0x06004A91 RID: 19089 RVA: 0x001506B0 File Offset: 0x0014E8B0
		static DocumentViewer()
		{
			DocumentViewer.CreateCommandBindings();
			DocumentViewer.RegisterMetadata();
			ControlsTraceLogger.AddControl(TelemetryControls.DocumentViewer);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DocumentViewer" /> class.</summary>
		// Token: 0x06004A92 RID: 19090 RVA: 0x00150BBA File Offset: 0x0014EDBA
		public DocumentViewer()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXInstantiated);
			this.SetUp();
		}

		/// <summary>Displays a thumbnail representation of the pages.</summary>
		// Token: 0x06004A93 RID: 19091 RVA: 0x00150BDB File Offset: 0x0014EDDB
		public void ViewThumbnails()
		{
			this.OnViewThumbnailsCommand();
		}

		/// <summary>Fits a single page to the width of the current viewport. </summary>
		// Token: 0x06004A94 RID: 19092 RVA: 0x00150BE3 File Offset: 0x0014EDE3
		public void FitToWidth()
		{
			this.OnFitToWidthCommand();
		}

		/// <summary>Fits a single page to the height of the current viewport. </summary>
		// Token: 0x06004A95 RID: 19093 RVA: 0x00150BEB File Offset: 0x0014EDEB
		public void FitToHeight()
		{
			this.OnFitToHeightCommand();
		}

		/// <summary>Fits the document to the current <see cref="P:System.Windows.Controls.DocumentViewer.MaxPagesAcross" /> property setting.</summary>
		// Token: 0x06004A96 RID: 19094 RVA: 0x00150BF3 File Offset: 0x0014EDF3
		public void FitToMaxPagesAcross()
		{
			this.OnFitToMaxPagesAcrossCommand();
		}

		/// <summary>Fits the document to a specified maximum number of page widths.</summary>
		/// <param name="pagesAcross">The maximum number of pages to fit in the current <see cref="P:System.Windows.Controls.DocumentViewer.ExtentWidth" />. </param>
		// Token: 0x06004A97 RID: 19095 RVA: 0x00150BFB File Offset: 0x0014EDFB
		public void FitToMaxPagesAcross(int pagesAcross)
		{
			if (!DocumentViewer.ValidateMaxPagesAcross(pagesAcross))
			{
				throw new ArgumentOutOfRangeException("pagesAcross");
			}
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.FitColumns(pagesAcross);
				return;
			}
		}

		/// <summary>Moves focus to the find toolbar to search the document content.</summary>
		// Token: 0x06004A98 RID: 19096 RVA: 0x00150C2A File Offset: 0x0014EE2A
		public void Find()
		{
			this.OnFindCommand();
		}

		/// <summary>Scrolls up one viewport.</summary>
		// Token: 0x06004A99 RID: 19097 RVA: 0x00150C32 File Offset: 0x0014EE32
		public void ScrollPageUp()
		{
			this.OnScrollPageUpCommand();
		}

		/// <summary>Scrolls down one viewport.</summary>
		// Token: 0x06004A9A RID: 19098 RVA: 0x00150C3A File Offset: 0x0014EE3A
		public void ScrollPageDown()
		{
			this.OnScrollPageDownCommand();
		}

		/// <summary>Scrolls left one viewport.</summary>
		// Token: 0x06004A9B RID: 19099 RVA: 0x00150C42 File Offset: 0x0014EE42
		public void ScrollPageLeft()
		{
			this.OnScrollPageLeftCommand();
		}

		/// <summary>Scrolls right one viewport.</summary>
		// Token: 0x06004A9C RID: 19100 RVA: 0x00150C4A File Offset: 0x0014EE4A
		public void ScrollPageRight()
		{
			this.OnScrollPageRightCommand();
		}

		/// <summary>Scrolls the document content up 16 device independent pixels.</summary>
		// Token: 0x06004A9D RID: 19101 RVA: 0x00150C52 File Offset: 0x0014EE52
		public void MoveUp()
		{
			this.OnMoveUpCommand();
		}

		/// <summary>Scrolls the document content down 16 device independent pixels.</summary>
		// Token: 0x06004A9E RID: 19102 RVA: 0x00150C5A File Offset: 0x0014EE5A
		public void MoveDown()
		{
			this.OnMoveDownCommand();
		}

		/// <summary>Scrolls the document content left 16 device independent pixels.</summary>
		// Token: 0x06004A9F RID: 19103 RVA: 0x00150C62 File Offset: 0x0014EE62
		public void MoveLeft()
		{
			this.OnMoveLeftCommand();
		}

		/// <summary>Scrolls the document content right 16 device independent pixels.</summary>
		// Token: 0x06004AA0 RID: 19104 RVA: 0x00150C6A File Offset: 0x0014EE6A
		public void MoveRight()
		{
			this.OnMoveRightCommand();
		}

		/// <summary>Zooms in on the document content by one zoom step. </summary>
		// Token: 0x06004AA1 RID: 19105 RVA: 0x00150C72 File Offset: 0x0014EE72
		public void IncreaseZoom()
		{
			this.OnIncreaseZoomCommand();
		}

		/// <summary>Zooms out of the document content by one zoom step. </summary>
		// Token: 0x06004AA2 RID: 19106 RVA: 0x00150C7A File Offset: 0x0014EE7A
		public void DecreaseZoom()
		{
			this.OnDecreaseZoomCommand();
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" /> method.</summary>
		// Token: 0x06004AA3 RID: 19107 RVA: 0x00150C82 File Offset: 0x0014EE82
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.FindContentHost();
			this.InstantiateFindToolBar();
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXStyleCreated);
			if (base.ContextMenu == null)
			{
				base.ContextMenu = null;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Input.RoutedUICommand" /> that performs the <see cref="M:System.Windows.Controls.DocumentViewer.ViewThumbnails" /> operation.</summary>
		/// <returns>The routed command that performs the <see cref="M:System.Windows.Controls.DocumentViewer.ViewThumbnails" /> operation.</returns>
		// Token: 0x17001233 RID: 4659
		// (get) Token: 0x06004AA4 RID: 19108 RVA: 0x00150CB1 File Offset: 0x0014EEB1
		public static RoutedUICommand ViewThumbnailsCommand
		{
			get
			{
				return DocumentViewer._viewThumbnailsCommand;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Input.RoutedUICommand" /> that performs the <see cref="M:System.Windows.Controls.DocumentViewer.FitToWidth" /> operation.</summary>
		/// <returns>The routed command that performs the <see cref="M:System.Windows.Controls.DocumentViewer.FitToWidth" /> operation.</returns>
		// Token: 0x17001234 RID: 4660
		// (get) Token: 0x06004AA5 RID: 19109 RVA: 0x00150CB8 File Offset: 0x0014EEB8
		public static RoutedUICommand FitToWidthCommand
		{
			get
			{
				return DocumentViewer._fitToWidthCommand;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Input.RoutedUICommand" /> that performs the <see cref="M:System.Windows.Controls.DocumentViewer.FitToHeight" /> operation.</summary>
		/// <returns>The routed command that performs the <see cref="M:System.Windows.Controls.DocumentViewer.FitToHeight" /> operation.</returns>
		// Token: 0x17001235 RID: 4661
		// (get) Token: 0x06004AA6 RID: 19110 RVA: 0x00150CBF File Offset: 0x0014EEBF
		public static RoutedUICommand FitToHeightCommand
		{
			get
			{
				return DocumentViewer._fitToHeightCommand;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Input.RoutedUICommand" /> that performs the <see cref="P:System.Windows.Controls.DocumentViewer.MaxPagesAcross" /> operation.</summary>
		/// <returns>The routed command that performs the <see cref="P:System.Windows.Controls.DocumentViewer.MaxPagesAcross" /> operation.</returns>
		// Token: 0x17001236 RID: 4662
		// (get) Token: 0x06004AA7 RID: 19111 RVA: 0x00150CC6 File Offset: 0x0014EEC6
		public static RoutedUICommand FitToMaxPagesAcrossCommand
		{
			get
			{
				return DocumentViewer._fitToMaxPagesAcrossCommand;
			}
		}

		/// <summary>Gets or sets the horizontal scroll position. </summary>
		/// <returns>The current horizontal scroll position specified in device independent pixels.  The initial default is 0.0.</returns>
		/// <exception cref="T:System.ArgumentException">The value specified to set is negative.</exception>
		// Token: 0x17001237 RID: 4663
		// (get) Token: 0x06004AA8 RID: 19112 RVA: 0x00150CCD File Offset: 0x0014EECD
		// (set) Token: 0x06004AA9 RID: 19113 RVA: 0x00150CDF File Offset: 0x0014EEDF
		public double HorizontalOffset
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.HorizontalOffsetProperty);
			}
			set
			{
				base.SetValue(DocumentViewer.HorizontalOffsetProperty, value);
			}
		}

		/// <summary>Gets or sets the vertical scroll position.  </summary>
		/// <returns>The current vertical scroll position offset in device independent pixels.  The default is 0.0.</returns>
		// Token: 0x17001238 RID: 4664
		// (get) Token: 0x06004AAA RID: 19114 RVA: 0x00150CF2 File Offset: 0x0014EEF2
		// (set) Token: 0x06004AAB RID: 19115 RVA: 0x00150D04 File Offset: 0x0014EF04
		public double VerticalOffset
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.VerticalOffsetProperty);
			}
			set
			{
				base.SetValue(DocumentViewer.VerticalOffsetProperty, value);
			}
		}

		/// <summary>Gets the overall horizontal width of the paginated document. </summary>
		/// <returns>The current horizontal width of the content layout area specified in device independent pixels.  The default is 0.0.</returns>
		// Token: 0x17001239 RID: 4665
		// (get) Token: 0x06004AAC RID: 19116 RVA: 0x00150D17 File Offset: 0x0014EF17
		public double ExtentWidth
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.ExtentWidthProperty);
			}
		}

		/// <summary>Gets the overall vertical height of the paginated document. </summary>
		/// <returns>The overall vertical height of the paginated content specified in device independent pixels.  The default is 0.0.</returns>
		// Token: 0x1700123A RID: 4666
		// (get) Token: 0x06004AAD RID: 19117 RVA: 0x00150D29 File Offset: 0x0014EF29
		public double ExtentHeight
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.ExtentHeightProperty);
			}
		}

		/// <summary>Gets the horizontal size of the scrollable content area. </summary>
		/// <returns>The horizontal size of the scrollable content area in device independent pixels.  The default is 0.0.</returns>
		// Token: 0x1700123B RID: 4667
		// (get) Token: 0x06004AAE RID: 19118 RVA: 0x00150D3B File Offset: 0x0014EF3B
		public double ViewportWidth
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.ViewportWidthProperty);
			}
		}

		/// <summary>Gets the vertical size of the scrollable content area. </summary>
		/// <returns>The vertical size of the scrollable content area  in device independent pixels.  The default is 0.0.</returns>
		// Token: 0x1700123C RID: 4668
		// (get) Token: 0x06004AAF RID: 19119 RVA: 0x00150D4D File Offset: 0x0014EF4D
		public double ViewportHeight
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.ViewportHeightProperty);
			}
		}

		/// <summary>Gets or sets a value that indicates whether drop-shadow page borders are displayed. </summary>
		/// <returns>
		///     <see langword="true" /> if drop-shadow borders are displayed; otherwise, <see langword="false" />.  The default is <see langword="true" />.</returns>
		// Token: 0x1700123D RID: 4669
		// (get) Token: 0x06004AB0 RID: 19120 RVA: 0x00150D5F File Offset: 0x0014EF5F
		// (set) Token: 0x06004AB1 RID: 19121 RVA: 0x00150D71 File Offset: 0x0014EF71
		public bool ShowPageBorders
		{
			get
			{
				return (bool)base.GetValue(DocumentViewer.ShowPageBordersProperty);
			}
			set
			{
				base.SetValue(DocumentViewer.ShowPageBordersProperty, value);
			}
		}

		/// <summary>Gets or sets the document zoom percentage. </summary>
		/// <returns>The zoom percentage expressed as a value, 5.0 to 5000.0.  The default is 100.0, which corresponds to 100.0%.</returns>
		// Token: 0x1700123E RID: 4670
		// (get) Token: 0x06004AB2 RID: 19122 RVA: 0x00150D7F File Offset: 0x0014EF7F
		// (set) Token: 0x06004AB3 RID: 19123 RVA: 0x00150D91 File Offset: 0x0014EF91
		public double Zoom
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.ZoomProperty);
			}
			set
			{
				base.SetValue(DocumentViewer.ZoomProperty, value);
			}
		}

		/// <summary>Gets or sets a value defining the maximum number of page columns to display. </summary>
		/// <returns>The maximum number of page columns to be displayed.  The default is 1.</returns>
		// Token: 0x1700123F RID: 4671
		// (get) Token: 0x06004AB4 RID: 19124 RVA: 0x00150DA4 File Offset: 0x0014EFA4
		// (set) Token: 0x06004AB5 RID: 19125 RVA: 0x00150DB6 File Offset: 0x0014EFB6
		public int MaxPagesAcross
		{
			get
			{
				return (int)base.GetValue(DocumentViewer.MaxPagesAcrossProperty);
			}
			set
			{
				base.SetValue(DocumentViewer.MaxPagesAcrossProperty, value);
			}
		}

		/// <summary>Gets or sets the vertical spacing between displayed pages. </summary>
		/// <returns>The vertical space between displayed pages in device independent pixels.  The default is 10.0.</returns>
		// Token: 0x17001240 RID: 4672
		// (get) Token: 0x06004AB6 RID: 19126 RVA: 0x00150DC9 File Offset: 0x0014EFC9
		// (set) Token: 0x06004AB7 RID: 19127 RVA: 0x00150DDB File Offset: 0x0014EFDB
		public double VerticalPageSpacing
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.VerticalPageSpacingProperty);
			}
			set
			{
				base.SetValue(DocumentViewer.VerticalPageSpacingProperty, value);
			}
		}

		/// <summary>Gets or sets the horizontal space between pages. </summary>
		/// <returns>The horizontal space between displayed pages specified in device independent pixels.  The default is 10.0.</returns>
		/// <exception cref="T:System.ArgumentException">The value specified to set is negative.</exception>
		// Token: 0x17001241 RID: 4673
		// (get) Token: 0x06004AB8 RID: 19128 RVA: 0x00150DEE File Offset: 0x0014EFEE
		// (set) Token: 0x06004AB9 RID: 19129 RVA: 0x00150E00 File Offset: 0x0014F000
		public double HorizontalPageSpacing
		{
			get
			{
				return (double)base.GetValue(DocumentViewer.HorizontalPageSpacingProperty);
			}
			set
			{
				base.SetValue(DocumentViewer.HorizontalPageSpacingProperty, value);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.DocumentViewer" /> can move up more in the document. </summary>
		/// <returns>
		///     <see langword="true" /> if the control can move up more in the document; otherwise, <see langword="false" /> if the document is at the topmost edge.</returns>
		// Token: 0x17001242 RID: 4674
		// (get) Token: 0x06004ABA RID: 19130 RVA: 0x00150E13 File Offset: 0x0014F013
		public bool CanMoveUp
		{
			get
			{
				return (bool)base.GetValue(DocumentViewer.CanMoveUpProperty);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.DocumentViewer" /> can move down more in the document. </summary>
		/// <returns>
		///     <see langword="true" /> if the control can move down more in the document; otherwise, <see langword="false" /> if the document is at the bottom.</returns>
		// Token: 0x17001243 RID: 4675
		// (get) Token: 0x06004ABB RID: 19131 RVA: 0x00150E25 File Offset: 0x0014F025
		public bool CanMoveDown
		{
			get
			{
				return (bool)base.GetValue(DocumentViewer.CanMoveDownProperty);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.DocumentViewer" /> can move more to the left in the document. </summary>
		/// <returns>
		///     <see langword="true" /> if the control can move more left in the document; otherwise, <see langword="false" /> if the document is at the leftmost edge.</returns>
		// Token: 0x17001244 RID: 4676
		// (get) Token: 0x06004ABC RID: 19132 RVA: 0x00150E37 File Offset: 0x0014F037
		public bool CanMoveLeft
		{
			get
			{
				return (bool)base.GetValue(DocumentViewer.CanMoveLeftProperty);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.DocumentViewer" /> can move more to the right in the document. </summary>
		/// <returns>
		///     <see langword="true" /> if the control can move more to the right in the document; otherwise, <see langword="false" /> if the document is at the rightmost edge.</returns>
		// Token: 0x17001245 RID: 4677
		// (get) Token: 0x06004ABD RID: 19133 RVA: 0x00150E49 File Offset: 0x0014F049
		public bool CanMoveRight
		{
			get
			{
				return (bool)base.GetValue(DocumentViewer.CanMoveRightProperty);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.DocumentViewer" /> can zoom in more. </summary>
		/// <returns>
		///     <see langword="true" /> if the control can zoom in more; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001246 RID: 4678
		// (get) Token: 0x06004ABE RID: 19134 RVA: 0x00150E5B File Offset: 0x0014F05B
		public bool CanIncreaseZoom
		{
			get
			{
				return (bool)base.GetValue(DocumentViewer.CanIncreaseZoomProperty);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.DocumentViewer" /> can zoom out more. </summary>
		/// <returns>
		///     <see langword="true" /> if the control can zoom out more; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001247 RID: 4679
		// (get) Token: 0x06004ABF RID: 19135 RVA: 0x00150E6D File Offset: 0x0014F06D
		public bool CanDecreaseZoom
		{
			get
			{
				return (bool)base.GetValue(DocumentViewer.CanDecreaseZoomProperty);
			}
		}

		/// <summary>Creates and returns an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for this <see cref="T:System.Windows.Controls.DocumentViewer" /> control.</summary>
		/// <returns>The new <see cref="T:System.Windows.Automation.Peers.DocumentViewerAutomationPeer" />.</returns>
		// Token: 0x06004AC0 RID: 19136 RVA: 0x00150E7F File Offset: 0x0014F07F
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DocumentViewerAutomationPeer(this);
		}

		/// <summary>Responds to calls when the document to display is changed.</summary>
		// Token: 0x06004AC1 RID: 19137 RVA: 0x00150E88 File Offset: 0x0014F088
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnDocumentChanged()
		{
			if (!(base.Document is FixedDocument) && !(base.Document is FixedDocumentSequence) && base.Document != null)
			{
				throw new NotSupportedException(SR.Get("DocumentViewerOnlySupportsFixedDocumentSequence"));
			}
			base.OnDocumentChanged();
			this.AttachContent();
			if (this._findToolbar != null)
			{
				this._findToolbar.DocumentLoaded = (base.Document != null);
			}
			if (!this._firstDocumentAssignment)
			{
				this.OnGoToPageCommand(1);
			}
			this._firstDocumentAssignment = false;
		}

		/// <summary>Responds to the <see cref="M:System.Windows.Controls.Primitives.DocumentViewerBase.OnBringIntoView(System.Windows.DependencyObject,System.Windows.Rect,System.Int32)" /> method from the <see cref="T:System.Windows.Controls.Primitives.DocumentViewerBase" /> implementation.</summary>
		/// <param name="element">The object to make visible.</param>
		/// <param name="rect">The rectangular region of the <paramref name="element" /> to make visible.</param>
		/// <param name="pageNumber">The number of the page to be viewed.</param>
		// Token: 0x06004AC2 RID: 19138 RVA: 0x00150F08 File Offset: 0x0014F108
		protected override void OnBringIntoView(DependencyObject element, Rect rect, int pageNumber)
		{
			int num = pageNumber - 1;
			if (num >= 0 && num < base.PageCount)
			{
				this._documentScrollInfo.MakeVisible(element, rect, num);
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.Primitives.DocumentViewerBase.PreviousPage" /> method.</summary>
		// Token: 0x06004AC3 RID: 19139 RVA: 0x00150F35 File Offset: 0x0014F135
		protected override void OnPreviousPageCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.ScrollToPreviousRow();
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.Primitives.DocumentViewerBase.NextPage" /> method.</summary>
		// Token: 0x06004AC4 RID: 19140 RVA: 0x00150F4A File Offset: 0x0014F14A
		protected override void OnNextPageCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.ScrollToNextRow();
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.Primitives.DocumentViewerBase.FirstPage" /> method.</summary>
		// Token: 0x06004AC5 RID: 19141 RVA: 0x00150F5F File Offset: 0x0014F15F
		protected override void OnFirstPageCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.MakePageVisible(0);
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.Primitives.DocumentViewerBase.LastPage" /> method.</summary>
		// Token: 0x06004AC6 RID: 19142 RVA: 0x00150F75 File Offset: 0x0014F175
		protected override void OnLastPageCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.MakePageVisible(base.PageCount - 1);
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.Primitives.DocumentViewerBase.GoToPage(System.Int32)" /> method.</summary>
		/// <param name="pageNumber">The page number to position to.</param>
		// Token: 0x06004AC7 RID: 19143 RVA: 0x00150F92 File Offset: 0x0014F192
		protected override void OnGoToPageCommand(int pageNumber)
		{
			if (this.CanGoToPage(pageNumber) && this._documentScrollInfo != null)
			{
				Invariant.Assert(pageNumber > 0, "PageNumber must be positive.");
				this._documentScrollInfo.MakePageVisible(pageNumber - 1);
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.DocumentViewer.ViewThumbnails" /> method.</summary>
		// Token: 0x06004AC8 RID: 19144 RVA: 0x00150FC1 File Offset: 0x0014F1C1
		protected virtual void OnViewThumbnailsCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.ViewThumbnails();
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.DocumentViewer.FitToWidth" /> method.</summary>
		// Token: 0x06004AC9 RID: 19145 RVA: 0x00150FD6 File Offset: 0x0014F1D6
		protected virtual void OnFitToWidthCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.FitToPageWidth();
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.DocumentViewer.FitToHeight" /> method.</summary>
		// Token: 0x06004ACA RID: 19146 RVA: 0x00150FEB File Offset: 0x0014F1EB
		protected virtual void OnFitToHeightCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.FitToPageHeight();
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.DocumentViewer.FitToMaxPagesAcross" /> method.</summary>
		// Token: 0x06004ACB RID: 19147 RVA: 0x00151000 File Offset: 0x0014F200
		protected virtual void OnFitToMaxPagesAcrossCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.FitColumns(this.MaxPagesAcross);
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.DocumentViewer.FitToMaxPagesAcross(System.Int32)" /> method.</summary>
		/// <param name="pagesAcross">The number of pages to fit across the content area.</param>
		// Token: 0x06004ACC RID: 19148 RVA: 0x00150BFB File Offset: 0x0014EDFB
		protected virtual void OnFitToMaxPagesAcrossCommand(int pagesAcross)
		{
			if (!DocumentViewer.ValidateMaxPagesAcross(pagesAcross))
			{
				throw new ArgumentOutOfRangeException("pagesAcross");
			}
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.FitColumns(pagesAcross);
				return;
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.DocumentViewer.Find" /> method.</summary>
		// Token: 0x06004ACD RID: 19149 RVA: 0x0015101B File Offset: 0x0014F21B
		protected virtual void OnFindCommand()
		{
			this.GoToFind();
		}

		/// <summary>Responds to <see cref="E:System.Windows.UIElement.KeyDown" /> events.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004ACE RID: 19150 RVA: 0x00151023 File Offset: 0x0014F223
		protected override void OnKeyDown(KeyEventArgs e)
		{
			e = this.ProcessFindKeys(e);
			base.OnKeyDown(e);
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.DocumentViewer.ScrollPageUp" /> method.</summary>
		// Token: 0x06004ACF RID: 19151 RVA: 0x00151035 File Offset: 0x0014F235
		protected virtual void OnScrollPageUpCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.PageUp();
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.DocumentViewer.ScrollPageDown" /> method.</summary>
		// Token: 0x06004AD0 RID: 19152 RVA: 0x0015104A File Offset: 0x0014F24A
		protected virtual void OnScrollPageDownCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.PageDown();
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.DocumentViewer.ScrollPageLeft" /> method.</summary>
		// Token: 0x06004AD1 RID: 19153 RVA: 0x0015105F File Offset: 0x0014F25F
		protected virtual void OnScrollPageLeftCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.PageLeft();
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.DocumentViewer.ScrollPageRight" /> method.</summary>
		// Token: 0x06004AD2 RID: 19154 RVA: 0x00151074 File Offset: 0x0014F274
		protected virtual void OnScrollPageRightCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.PageRight();
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.DocumentViewer.MoveUp" /> method.</summary>
		// Token: 0x06004AD3 RID: 19155 RVA: 0x00151089 File Offset: 0x0014F289
		protected virtual void OnMoveUpCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.LineUp();
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.DocumentViewer.MoveDown" /> method.</summary>
		// Token: 0x06004AD4 RID: 19156 RVA: 0x0015109E File Offset: 0x0014F29E
		protected virtual void OnMoveDownCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.LineDown();
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.DocumentViewer.MoveLeft" /> method.</summary>
		// Token: 0x06004AD5 RID: 19157 RVA: 0x001510B3 File Offset: 0x0014F2B3
		protected virtual void OnMoveLeftCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.LineLeft();
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.DocumentViewer.MoveRight" /> method.</summary>
		// Token: 0x06004AD6 RID: 19158 RVA: 0x001510C8 File Offset: 0x0014F2C8
		protected virtual void OnMoveRightCommand()
		{
			if (this._documentScrollInfo != null)
			{
				this._documentScrollInfo.LineRight();
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.DocumentViewer.IncreaseZoom" /> method.</summary>
		// Token: 0x06004AD7 RID: 19159 RVA: 0x001510E0 File Offset: 0x0014F2E0
		protected virtual void OnIncreaseZoomCommand()
		{
			if (this.CanIncreaseZoom)
			{
				double zoom = this.Zoom;
				this.FindZoomLevelIndex();
				if (this._zoomLevelIndex > 0)
				{
					this._zoomLevelIndex--;
				}
				this._updatingInternalZoomLevel = true;
				this.Zoom = DocumentViewer._zoomLevelCollection[this._zoomLevelIndex];
				this._updatingInternalZoomLevel = false;
			}
		}

		/// <summary>Responds to calls to the <see cref="M:System.Windows.Controls.DocumentViewer.DecreaseZoom" /> method.</summary>
		// Token: 0x06004AD8 RID: 19160 RVA: 0x0015113C File Offset: 0x0014F33C
		protected virtual void OnDecreaseZoomCommand()
		{
			if (this.CanDecreaseZoom)
			{
				double zoom = this.Zoom;
				this.FindZoomLevelIndex();
				if (zoom == DocumentViewer._zoomLevelCollection[this._zoomLevelIndex] && this._zoomLevelIndex < DocumentViewer._zoomLevelCollection.Length - 1)
				{
					this._zoomLevelIndex++;
				}
				this._updatingInternalZoomLevel = true;
				this.Zoom = DocumentViewer._zoomLevelCollection[this._zoomLevelIndex];
				this._updatingInternalZoomLevel = false;
			}
		}

		/// <summary>Returns a collection of <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> elements that are currently displayed.</summary>
		/// <param name="changed">When this method returns, contains <see langword="true" /> if the collection of <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> elements changed after the last call to <see cref="M:System.Windows.Controls.DocumentViewer.GetPageViewsCollection(System.Boolean@)" />; otherwise, <see langword="false" />.</param>
		/// <returns>The collection of <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> elements that are currently displayed in the <see cref="T:System.Windows.Controls.DocumentViewer" /> control.</returns>
		// Token: 0x06004AD9 RID: 19161 RVA: 0x001511AC File Offset: 0x0014F3AC
		protected override ReadOnlyCollection<DocumentPageView> GetPageViewsCollection(out bool changed)
		{
			changed = this._pageViewCollectionChanged;
			this._pageViewCollectionChanged = false;
			ReadOnlyCollection<DocumentPageView> result;
			if (this._documentScrollInfo != null && this._documentScrollInfo.PageViews != null)
			{
				result = this._documentScrollInfo.PageViews;
			}
			else
			{
				result = new ReadOnlyCollection<DocumentPageView>(new List<DocumentPageView>(0));
			}
			return result;
		}

		/// <summary>Responds to <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> events.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004ADA RID: 19162 RVA: 0x001511FA File Offset: 0x0014F3FA
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			if (!e.Handled)
			{
				base.Focus();
				e.Handled = true;
			}
		}

		/// <summary>Responds to <see cref="E:System.Windows.UIElement.PreviewMouseWheel" /> events.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004ADB RID: 19163 RVA: 0x00151219 File Offset: 0x0014F419
		protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
		{
			if (e.Handled)
			{
				return;
			}
			if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
			{
				e.Handled = true;
				if (e.Delta < 0)
				{
					this.DecreaseZoom();
					return;
				}
				this.IncreaseZoom();
			}
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x00151254 File Offset: 0x0014F454
		internal void InvalidateDocumentScrollInfo()
		{
			this._internalIDSIChange = true;
			base.SetValue(DocumentViewer.ExtentWidthPropertyKey, this._documentScrollInfo.ExtentWidth);
			base.SetValue(DocumentViewer.ExtentHeightPropertyKey, this._documentScrollInfo.ExtentHeight);
			base.SetValue(DocumentViewer.ViewportWidthPropertyKey, this._documentScrollInfo.ViewportWidth);
			base.SetValue(DocumentViewer.ViewportHeightPropertyKey, this._documentScrollInfo.ViewportHeight);
			if (this.HorizontalOffset != this._documentScrollInfo.HorizontalOffset)
			{
				this.HorizontalOffset = this._documentScrollInfo.HorizontalOffset;
			}
			if (this.VerticalOffset != this._documentScrollInfo.VerticalOffset)
			{
				this.VerticalOffset = this._documentScrollInfo.VerticalOffset;
			}
			base.SetValue(DocumentViewerBase.MasterPageNumberPropertyKey, this._documentScrollInfo.FirstVisiblePageNumber + 1);
			double num = DocumentViewer.ScaleToZoom(this._documentScrollInfo.Scale);
			if (this.Zoom != num)
			{
				this.Zoom = num;
			}
			if (this.MaxPagesAcross != this._documentScrollInfo.MaxPagesAcross)
			{
				this.MaxPagesAcross = this._documentScrollInfo.MaxPagesAcross;
			}
			this._internalIDSIChange = false;
		}

		// Token: 0x06004ADD RID: 19165 RVA: 0x00151385 File Offset: 0x0014F585
		internal void InvalidatePageViewsInternal()
		{
			this._pageViewCollectionChanged = true;
			base.InvalidatePageViews();
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x00151394 File Offset: 0x0014F594
		internal bool BringPointIntoView(Point point)
		{
			FrameworkElement frameworkElement = this._documentScrollInfo as FrameworkElement;
			if (frameworkElement != null)
			{
				Transform transform = base.TransformToDescendant(frameworkElement) as Transform;
				Rect rect = Rect.Transform(new Rect(frameworkElement.RenderSize), transform.Value);
				double num = this.VerticalOffset;
				double num2 = this.HorizontalOffset;
				if (point.Y > rect.Y + rect.Height)
				{
					num += point.Y - (rect.Y + rect.Height);
				}
				else if (point.Y < rect.Y)
				{
					num -= rect.Y - point.Y;
				}
				if (point.X < rect.X)
				{
					num2 -= rect.X - point.X;
				}
				else if (point.X > rect.X + rect.Width)
				{
					num2 += point.X - (rect.X + rect.Width);
				}
				this.VerticalOffset = Math.Max(num, 0.0);
				this.HorizontalOffset = Math.Max(num2, 0.0);
			}
			return false;
		}

		// Token: 0x17001248 RID: 4680
		// (get) Token: 0x06004ADF RID: 19167 RVA: 0x001514C2 File Offset: 0x0014F6C2
		internal ITextSelection TextSelection
		{
			get
			{
				if (base.TextEditor != null)
				{
					return base.TextEditor.Selection;
				}
				return null;
			}
		}

		// Token: 0x17001249 RID: 4681
		// (get) Token: 0x06004AE0 RID: 19168 RVA: 0x001514D9 File Offset: 0x0014F6D9
		internal IDocumentScrollInfo DocumentScrollInfo
		{
			get
			{
				return this._documentScrollInfo;
			}
		}

		// Token: 0x1700124A RID: 4682
		// (get) Token: 0x06004AE1 RID: 19169 RVA: 0x001514E1 File Offset: 0x0014F6E1
		internal ScrollViewer ScrollViewer
		{
			get
			{
				return this._scrollViewer;
			}
		}

		// Token: 0x06004AE2 RID: 19170 RVA: 0x001514EC File Offset: 0x0014F6EC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void CreateCommandBindings()
		{
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(DocumentViewer.ExecutedRoutedEventHandler);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(DocumentViewer.QueryEnabledHandler);
			DocumentViewer._viewThumbnailsCommand = new RoutedUICommand(SR.Get("DocumentViewerViewThumbnailsCommandText"), "ViewThumbnailsCommand", typeof(DocumentViewer), null);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), DocumentViewer._viewThumbnailsCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			DocumentViewer._fitToWidthCommand = new RoutedUICommand(SR.Get("DocumentViewerViewFitToWidthCommandText"), "FitToWidthCommand", typeof(DocumentViewer), null);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), DocumentViewer._fitToWidthCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.D2, ModifierKeys.Control));
			DocumentViewer._fitToHeightCommand = new RoutedUICommand(SR.Get("DocumentViewerViewFitToHeightCommandText"), "FitToHeightCommand", typeof(DocumentViewer), null);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), DocumentViewer._fitToHeightCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			DocumentViewer._fitToMaxPagesAcrossCommand = new RoutedUICommand(SR.Get("DocumentViewerViewFitToMaxPagesAcrossCommandText"), "FitToMaxPagesAcrossCommand", typeof(DocumentViewer), null);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), DocumentViewer._fitToMaxPagesAcrossCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ApplicationCommands.Find, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ComponentCommands.ScrollPageUp, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Prior);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ComponentCommands.ScrollPageDown, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Next);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ComponentCommands.ScrollPageLeft, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ComponentCommands.ScrollPageRight, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ComponentCommands.MoveUp, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Up);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ComponentCommands.MoveDown, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Down);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ComponentCommands.MoveLeft, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Left);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), ComponentCommands.MoveRight, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Right);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), NavigationCommands.Zoom, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), NavigationCommands.IncreaseZoom, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.Add, ModifierKeys.Control), new KeyGesture(Key.Add, ModifierKeys.Control | ModifierKeys.Shift), new KeyGesture(Key.OemPlus, ModifierKeys.Control), new KeyGesture(Key.OemPlus, ModifierKeys.Control | ModifierKeys.Shift));
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), NavigationCommands.DecreaseZoom, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.Subtract, ModifierKeys.Control), new KeyGesture(Key.Subtract, ModifierKeys.Control | ModifierKeys.Shift), new KeyGesture(Key.OemMinus, ModifierKeys.Control), new KeyGesture(Key.OemMinus, ModifierKeys.Control | ModifierKeys.Shift));
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), NavigationCommands.PreviousPage, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.Prior, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), NavigationCommands.NextPage, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.Next, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), NavigationCommands.FirstPage, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.Home, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewer), NavigationCommands.LastPage, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.End, ModifierKeys.Control));
			InputBinding inputBinding = new InputBinding(NavigationCommands.Zoom, new KeyGesture(Key.D1, ModifierKeys.Control));
			inputBinding.CommandParameter = 100.0;
			CommandManager.RegisterClassInputBinding(typeof(DocumentViewer), inputBinding);
			InputBinding inputBinding2 = new InputBinding(DocumentViewer.FitToMaxPagesAcrossCommand, new KeyGesture(Key.D3, ModifierKeys.Control));
			inputBinding2.CommandParameter = 1;
			CommandManager.RegisterClassInputBinding(typeof(DocumentViewer), inputBinding2);
			InputBinding inputBinding3 = new InputBinding(DocumentViewer.FitToMaxPagesAcrossCommand, new KeyGesture(Key.D4, ModifierKeys.Control));
			inputBinding3.CommandParameter = 2;
			CommandManager.RegisterClassInputBinding(typeof(DocumentViewer), inputBinding3);
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x00151874 File Offset: 0x0014FA74
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void QueryEnabledHandler(object target, CanExecuteRoutedEventArgs args)
		{
			DocumentViewer documentViewer = target as DocumentViewer;
			Invariant.Assert(documentViewer != null, "Target of QueryEnabledEvent must be DocumentViewer.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (documentViewer == null)
			{
				return;
			}
			args.Handled = true;
			if (args.Command == DocumentViewer.ViewThumbnailsCommand || args.Command == DocumentViewer.FitToWidthCommand || args.Command == DocumentViewer.FitToHeightCommand || args.Command == DocumentViewer.FitToMaxPagesAcrossCommand || args.Command == NavigationCommands.Zoom)
			{
				args.CanExecute = true;
				return;
			}
			if (args.Command == ApplicationCommands.Find)
			{
				args.CanExecute = (documentViewer.TextEditor != null);
				return;
			}
			if (args.Command == ComponentCommands.ScrollPageUp || args.Command == ComponentCommands.MoveUp)
			{
				args.CanExecute = documentViewer.CanMoveUp;
				return;
			}
			if (args.Command == ComponentCommands.ScrollPageDown || args.Command == ComponentCommands.MoveDown)
			{
				args.CanExecute = documentViewer.CanMoveDown;
				return;
			}
			if (args.Command == ComponentCommands.ScrollPageLeft || args.Command == ComponentCommands.MoveLeft)
			{
				args.CanExecute = documentViewer.CanMoveLeft;
				return;
			}
			if (args.Command == ComponentCommands.ScrollPageRight || args.Command == ComponentCommands.MoveRight)
			{
				args.CanExecute = documentViewer.CanMoveRight;
				return;
			}
			if (args.Command == NavigationCommands.IncreaseZoom)
			{
				args.CanExecute = documentViewer.CanIncreaseZoom;
				return;
			}
			if (args.Command == NavigationCommands.DecreaseZoom)
			{
				args.CanExecute = documentViewer.CanDecreaseZoom;
				return;
			}
			if (args.Command == NavigationCommands.PreviousPage || args.Command == NavigationCommands.FirstPage)
			{
				args.CanExecute = documentViewer.CanGoToPreviousPage;
				return;
			}
			if (args.Command == NavigationCommands.NextPage || args.Command == NavigationCommands.LastPage)
			{
				args.CanExecute = documentViewer.CanGoToNextPage;
				return;
			}
			if (args.Command == NavigationCommands.GoToPage)
			{
				args.CanExecute = (documentViewer.Document != null);
				return;
			}
			args.Handled = false;
			Invariant.Assert(false, "Command not handled in QueryEnabledHandler.");
		}

		// Token: 0x06004AE4 RID: 19172 RVA: 0x00151A64 File Offset: 0x0014FC64
		private static void ExecutedRoutedEventHandler(object target, ExecutedRoutedEventArgs args)
		{
			DocumentViewer documentViewer = target as DocumentViewer;
			Invariant.Assert(documentViewer != null, "Target of ExecuteEvent must be DocumentViewer.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (documentViewer == null)
			{
				return;
			}
			if (args.Command == DocumentViewer.ViewThumbnailsCommand)
			{
				documentViewer.OnViewThumbnailsCommand();
				return;
			}
			if (args.Command == DocumentViewer.FitToWidthCommand)
			{
				documentViewer.OnFitToWidthCommand();
				return;
			}
			if (args.Command == DocumentViewer.FitToHeightCommand)
			{
				documentViewer.OnFitToHeightCommand();
				return;
			}
			if (args.Command == DocumentViewer.FitToMaxPagesAcrossCommand)
			{
				DocumentViewer.DoFitToMaxPagesAcross(documentViewer, args.Parameter);
				return;
			}
			if (args.Command == ApplicationCommands.Find)
			{
				documentViewer.OnFindCommand();
				return;
			}
			if (args.Command == ComponentCommands.ScrollPageUp)
			{
				documentViewer.OnScrollPageUpCommand();
				return;
			}
			if (args.Command == ComponentCommands.ScrollPageDown)
			{
				documentViewer.OnScrollPageDownCommand();
				return;
			}
			if (args.Command == ComponentCommands.ScrollPageLeft)
			{
				documentViewer.OnScrollPageLeftCommand();
				return;
			}
			if (args.Command == ComponentCommands.ScrollPageRight)
			{
				documentViewer.OnScrollPageRightCommand();
				return;
			}
			if (args.Command == ComponentCommands.MoveUp)
			{
				documentViewer.OnMoveUpCommand();
				return;
			}
			if (args.Command == ComponentCommands.MoveDown)
			{
				documentViewer.OnMoveDownCommand();
				return;
			}
			if (args.Command == ComponentCommands.MoveLeft)
			{
				documentViewer.OnMoveLeftCommand();
				return;
			}
			if (args.Command == ComponentCommands.MoveRight)
			{
				documentViewer.OnMoveRightCommand();
				return;
			}
			if (args.Command == NavigationCommands.Zoom)
			{
				DocumentViewer.DoZoom(documentViewer, args.Parameter);
				return;
			}
			if (args.Command == NavigationCommands.DecreaseZoom)
			{
				documentViewer.DecreaseZoom();
				return;
			}
			if (args.Command == NavigationCommands.IncreaseZoom)
			{
				documentViewer.IncreaseZoom();
				return;
			}
			if (args.Command == NavigationCommands.PreviousPage)
			{
				documentViewer.PreviousPage();
				return;
			}
			if (args.Command == NavigationCommands.NextPage)
			{
				documentViewer.NextPage();
				return;
			}
			if (args.Command == NavigationCommands.FirstPage)
			{
				documentViewer.FirstPage();
				return;
			}
			if (args.Command == NavigationCommands.LastPage)
			{
				documentViewer.LastPage();
				return;
			}
			Invariant.Assert(false, "Command not handled in ExecutedRoutedEventHandler.");
		}

		// Token: 0x06004AE5 RID: 19173 RVA: 0x00151C40 File Offset: 0x0014FE40
		private static void DoFitToMaxPagesAcross(DocumentViewer dv, object data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			int pagesAcross = 0;
			bool flag = true;
			if (data is int)
			{
				pagesAcross = (int)data;
			}
			else if (data is string)
			{
				try
				{
					pagesAcross = Convert.ToInt32((string)data, CultureInfo.CurrentCulture);
				}
				catch (ArgumentNullException)
				{
					flag = false;
				}
				catch (FormatException)
				{
					flag = false;
				}
				catch (OverflowException)
				{
					flag = false;
				}
			}
			if (!flag)
			{
				throw new ArgumentException(SR.Get("DocumentViewerArgumentMustBeInteger"), "data");
			}
			dv.OnFitToMaxPagesAcrossCommand(pagesAcross);
		}

		// Token: 0x06004AE6 RID: 19174 RVA: 0x00151CE4 File Offset: 0x0014FEE4
		private static void DoZoom(DocumentViewer dv, object data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (dv._zoomPercentageConverter == null)
			{
				dv._zoomPercentageConverter = new ZoomPercentageConverter();
			}
			object obj = dv._zoomPercentageConverter.ConvertBack(data, typeof(double), null, CultureInfo.InvariantCulture);
			if (obj == DependencyProperty.UnsetValue)
			{
				throw new ArgumentException(SR.Get("DocumentViewerArgumentMustBePercentage"), "data");
			}
			dv.Zoom = (double)obj;
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x00151D58 File Offset: 0x0014FF58
		private static void RegisterMetadata()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DocumentViewer), new FrameworkPropertyMetadata(typeof(DocumentViewer)));
			DocumentViewer._dType = DependencyObjectType.FromSystemTypeInternal(typeof(DocumentViewer));
		}

		// Token: 0x06004AE8 RID: 19176 RVA: 0x00151D91 File Offset: 0x0014FF91
		private void SetUp()
		{
			base.IsSelectionEnabled = true;
			base.SetValue(TextBoxBase.AcceptsTabProperty, false);
			this.CreateIDocumentScrollInfo();
		}

		// Token: 0x06004AE9 RID: 19177 RVA: 0x00151DAC File Offset: 0x0014FFAC
		private void CreateIDocumentScrollInfo()
		{
			if (this._documentScrollInfo == null)
			{
				this._documentScrollInfo = new DocumentGrid();
				this._documentScrollInfo.DocumentViewerOwner = this;
				FrameworkElement frameworkElement = this._documentScrollInfo as FrameworkElement;
				if (frameworkElement != null)
				{
					frameworkElement.Name = "DocumentGrid";
					frameworkElement.Focusable = false;
					frameworkElement.SetValue(KeyboardNavigation.IsTabStopProperty, false);
					base.TextEditorRenderScope = frameworkElement;
				}
			}
			this.AttachContent();
			this._documentScrollInfo.VerticalPageSpacing = this.VerticalPageSpacing;
			this._documentScrollInfo.HorizontalPageSpacing = this.HorizontalPageSpacing;
		}

		// Token: 0x06004AEA RID: 19178 RVA: 0x00151E34 File Offset: 0x00150034
		private void AttachContent()
		{
			this._documentScrollInfo.Content = ((base.Document != null) ? (base.Document.DocumentPaginator as DynamicDocumentPaginator) : null);
			base.IsSelectionEnabled = true;
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x00151E64 File Offset: 0x00150064
		private void FindContentHost()
		{
			ScrollViewer scrollViewer = base.Template.FindName("PART_ContentHost", this) as ScrollViewer;
			if (scrollViewer == null)
			{
				throw new NotSupportedException(SR.Get("DocumentViewerStyleMustIncludeContentHost"));
			}
			this._scrollViewer = scrollViewer;
			this._scrollViewer.Focusable = false;
			Invariant.Assert(this._documentScrollInfo != null, "IDocumentScrollInfo cannot be null.");
			this._scrollViewer.Content = this._documentScrollInfo;
			this._scrollViewer.ScrollInfo = this._documentScrollInfo;
			if (this._documentScrollInfo.Content != base.Document)
			{
				this.AttachContent();
			}
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x00151EFC File Offset: 0x001500FC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void InstantiateFindToolBar()
		{
			ContentControl contentControl = base.Template.FindName("PART_FindToolBarHost", this) as ContentControl;
			if (contentControl != null)
			{
				if (this._findToolbar == null)
				{
					this._findToolbar = new FindToolBar();
					this._findToolbar.FindClicked += this.OnFindInvoked;
					this._findToolbar.DocumentLoaded = (base.Document != null);
				}
				if (!this._findToolbar.IsAncestorOf(this))
				{
					((IAddChild)contentControl).AddChild(this._findToolbar);
				}
			}
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x00151F80 File Offset: 0x00150180
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnFindInvoked(object sender, EventArgs e)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXFindBegin);
			try
			{
				if (this._findToolbar != null && base.TextEditor != null)
				{
					ITextRange textRange = base.Find(this._findToolbar);
					if (textRange != null && !textRange.IsEmpty)
					{
						base.Focus();
						if (this._documentScrollInfo != null)
						{
							this._documentScrollInfo.MakeSelectionVisible();
						}
						this._findToolbar.GoToTextBox();
					}
					else
					{
						string text = this._findToolbar.SearchUp ? SR.Get("DocumentViewerSearchUpCompleteLabel") : SR.Get("DocumentViewerSearchDownCompleteLabel");
						text = string.Format(CultureInfo.CurrentCulture, text, new object[]
						{
							this._findToolbar.SearchText
						});
						Window parent = null;
						if (Application.Current != null && Application.Current.CheckAccess())
						{
							parent = Application.Current.MainWindow;
						}
						SecurityHelper.ShowMessageBoxHelper(parent, text, SR.Get("DocumentViewerSearchCompleteTitle"), MessageBoxButton.OK, MessageBoxImage.Asterisk);
					}
				}
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXFindEnd);
			}
		}

		// Token: 0x06004AEE RID: 19182 RVA: 0x00152088 File Offset: 0x00150288
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void GoToFind()
		{
			if (this._findToolbar != null)
			{
				this._findToolbar.GoToTextBox();
			}
		}

		// Token: 0x06004AEF RID: 19183 RVA: 0x001520A0 File Offset: 0x001502A0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private KeyEventArgs ProcessFindKeys(KeyEventArgs e)
		{
			if (this._findToolbar == null || base.Document == null)
			{
				return e;
			}
			if (e.Key == Key.F3)
			{
				e.Handled = true;
				this._findToolbar.SearchUp = ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift);
				this.OnFindInvoked(this, EventArgs.Empty);
			}
			return e;
		}

		// Token: 0x06004AF0 RID: 19184 RVA: 0x001520F8 File Offset: 0x001502F8
		private void FindZoomLevelIndex()
		{
			if (DocumentViewer._zoomLevelCollection != null)
			{
				if (this._zoomLevelIndex < 0 || this._zoomLevelIndex >= DocumentViewer._zoomLevelCollection.Length)
				{
					this._zoomLevelIndex = 0;
					this._zoomLevelIndexValid = false;
				}
				if (!this._zoomLevelIndexValid)
				{
					double zoom = this.Zoom;
					int num = 0;
					while (num < DocumentViewer._zoomLevelCollection.Length - 1 && zoom < DocumentViewer._zoomLevelCollection[num])
					{
						num++;
					}
					this._zoomLevelIndex = num;
					this._zoomLevelIndexValid = true;
				}
			}
		}

		// Token: 0x06004AF1 RID: 19185 RVA: 0x00152170 File Offset: 0x00150370
		private static bool DoubleValue_Validate(object value)
		{
			bool result;
			if (value is double)
			{
				double d = (double)value;
				result = (!double.IsNaN(d) && !double.IsInfinity(d));
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004AF2 RID: 19186 RVA: 0x001521A7 File Offset: 0x001503A7
		private static double ScaleToZoom(double scale)
		{
			return scale * 100.0;
		}

		// Token: 0x06004AF3 RID: 19187 RVA: 0x001521B4 File Offset: 0x001503B4
		private static double ZoomToScale(double zoom)
		{
			return zoom / 100.0;
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x001521C1 File Offset: 0x001503C1
		private static bool ValidateOffset(object value)
		{
			return DocumentViewer.DoubleValue_Validate(value) && (double)value >= 0.0;
		}

		// Token: 0x06004AF5 RID: 19189 RVA: 0x001521E4 File Offset: 0x001503E4
		private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			double num = (double)e.NewValue;
			if (!documentViewer._internalIDSIChange && documentViewer._documentScrollInfo != null)
			{
				documentViewer._documentScrollInfo.SetHorizontalOffset(num);
			}
			documentViewer.SetValue(DocumentViewer.CanMoveLeftPropertyKey, num > 0.0);
			documentViewer.SetValue(DocumentViewer.CanMoveRightPropertyKey, num < documentViewer.ExtentWidth - documentViewer.ViewportWidth);
		}

		// Token: 0x06004AF6 RID: 19190 RVA: 0x00152254 File Offset: 0x00150454
		private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			double num = (double)e.NewValue;
			if (!documentViewer._internalIDSIChange && documentViewer._documentScrollInfo != null)
			{
				documentViewer._documentScrollInfo.SetVerticalOffset(num);
			}
			documentViewer.SetValue(DocumentViewer.CanMoveUpPropertyKey, num > 0.0);
			documentViewer.SetValue(DocumentViewer.CanMoveDownPropertyKey, num < documentViewer.ExtentHeight - documentViewer.ViewportHeight);
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x001522C4 File Offset: 0x001504C4
		private static void OnExtentWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			documentViewer.SetValue(DocumentViewer.CanMoveRightPropertyKey, documentViewer.HorizontalOffset < (double)e.NewValue - documentViewer.ViewportWidth);
		}

		// Token: 0x06004AF8 RID: 19192 RVA: 0x00152300 File Offset: 0x00150500
		private static void OnExtentHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			documentViewer.SetValue(DocumentViewer.CanMoveDownPropertyKey, documentViewer.VerticalOffset < (double)e.NewValue - documentViewer.ViewportHeight);
		}

		// Token: 0x06004AF9 RID: 19193 RVA: 0x0015233C File Offset: 0x0015053C
		private static void OnViewportWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			double num = (double)e.NewValue;
			documentViewer.SetValue(DocumentViewer.CanMoveRightPropertyKey, documentViewer.HorizontalOffset < documentViewer.ExtentWidth - (double)e.NewValue);
		}

		// Token: 0x06004AFA RID: 19194 RVA: 0x00152384 File Offset: 0x00150584
		private static void OnViewportHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			double num = (double)e.NewValue;
			documentViewer.SetValue(DocumentViewer.CanMoveDownPropertyKey, documentViewer.VerticalOffset < documentViewer.ExtentHeight - num);
		}

		// Token: 0x06004AFB RID: 19195 RVA: 0x001523C0 File Offset: 0x001505C0
		private static void OnShowPageBordersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			if (documentViewer._documentScrollInfo != null)
			{
				documentViewer._documentScrollInfo.ShowPageBorders = (bool)e.NewValue;
			}
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x001523F4 File Offset: 0x001505F4
		private static object CoerceZoom(DependencyObject d, object value)
		{
			double num = (double)value;
			if (num < DocumentViewerConstants.MinimumZoom)
			{
				return DocumentViewerConstants.MinimumZoom;
			}
			if (num > DocumentViewerConstants.MaximumZoom)
			{
				return DocumentViewerConstants.MaximumZoom;
			}
			return value;
		}

		// Token: 0x06004AFD RID: 19197 RVA: 0x00152430 File Offset: 0x00150630
		private static void OnZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			if (documentViewer._documentScrollInfo != null)
			{
				double num = (double)e.NewValue;
				if (!documentViewer._internalIDSIChange)
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXZoom, (int)num);
					documentViewer._documentScrollInfo.SetScale(DocumentViewer.ZoomToScale(num));
				}
				documentViewer.SetValue(DocumentViewer.CanIncreaseZoomPropertyKey, num < DocumentViewerConstants.MaximumZoom);
				documentViewer.SetValue(DocumentViewer.CanDecreaseZoomPropertyKey, num > DocumentViewerConstants.MinimumZoom);
				if (!documentViewer._updatingInternalZoomLevel)
				{
					documentViewer._zoomLevelIndexValid = false;
				}
			}
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x001524BC File Offset: 0x001506BC
		private static bool ValidateMaxPagesAcross(object value)
		{
			int num = (int)value;
			return num > 0 && num <= DocumentViewerConstants.MaximumMaxPagesAcross;
		}

		// Token: 0x06004AFF RID: 19199 RVA: 0x001524E4 File Offset: 0x001506E4
		private static void OnMaxPagesAcrossChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			if (!documentViewer._internalIDSIChange)
			{
				documentViewer._documentScrollInfo.SetColumns((int)e.NewValue);
			}
		}

		// Token: 0x06004B00 RID: 19200 RVA: 0x001521C1 File Offset: 0x001503C1
		private static bool ValidatePageSpacing(object value)
		{
			return DocumentViewer.DoubleValue_Validate(value) && (double)value >= 0.0;
		}

		// Token: 0x06004B01 RID: 19201 RVA: 0x00152518 File Offset: 0x00150718
		private static void OnVerticalPageSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			if (documentViewer._documentScrollInfo != null)
			{
				documentViewer._documentScrollInfo.VerticalPageSpacing = (double)e.NewValue;
			}
		}

		// Token: 0x06004B02 RID: 19202 RVA: 0x0015254C File Offset: 0x0015074C
		private static void OnHorizontalPageSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentViewer documentViewer = (DocumentViewer)d;
			if (documentViewer._documentScrollInfo != null)
			{
				documentViewer._documentScrollInfo.HorizontalPageSpacing = (double)e.NewValue;
			}
		}

		// Token: 0x1700124B RID: 4683
		// (get) Token: 0x06004B03 RID: 19203 RVA: 0x0015257F File Offset: 0x0015077F
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return DocumentViewer._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.HorizontalOffset" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.HorizontalOffset" /> dependency property. </returns>
		// Token: 0x04002A69 RID: 10857
		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DocumentViewer.OnHorizontalOffsetChanged)), new ValidateValueCallback(DocumentViewer.ValidateOffset));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.VerticalOffset" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.VerticalOffset" /> dependency property. </returns>
		// Token: 0x04002A6A RID: 10858
		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DocumentViewer.OnVerticalOffsetChanged)), new ValidateValueCallback(DocumentViewer.ValidateOffset));

		// Token: 0x04002A6B RID: 10859
		private static readonly DependencyPropertyKey ExtentWidthPropertyKey = DependencyProperty.RegisterReadOnly("ExtentWidth", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DocumentViewer.OnExtentWidthChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.ExtentWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.ExtentWidth" /> dependency property. </returns>
		// Token: 0x04002A6C RID: 10860
		public static readonly DependencyProperty ExtentWidthProperty = DocumentViewer.ExtentWidthPropertyKey.DependencyProperty;

		// Token: 0x04002A6D RID: 10861
		private static readonly DependencyPropertyKey ExtentHeightPropertyKey = DependencyProperty.RegisterReadOnly("ExtentHeight", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DocumentViewer.OnExtentHeightChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.ExtentHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.ExtentHeight" /> dependency property. </returns>
		// Token: 0x04002A6E RID: 10862
		public static readonly DependencyProperty ExtentHeightProperty = DocumentViewer.ExtentHeightPropertyKey.DependencyProperty;

		// Token: 0x04002A6F RID: 10863
		private static readonly DependencyPropertyKey ViewportWidthPropertyKey = DependencyProperty.RegisterReadOnly("ViewportWidth", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DocumentViewer.OnViewportWidthChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.ViewportWidth" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.ViewportWidth" /> dependency property.</returns>
		// Token: 0x04002A70 RID: 10864
		public static readonly DependencyProperty ViewportWidthProperty = DocumentViewer.ViewportWidthPropertyKey.DependencyProperty;

		// Token: 0x04002A71 RID: 10865
		private static readonly DependencyPropertyKey ViewportHeightPropertyKey = DependencyProperty.RegisterReadOnly("ViewportHeight", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DocumentViewer.OnViewportHeightChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.ViewportHeight" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.ViewportHeight" /> dependency property.</returns>
		// Token: 0x04002A72 RID: 10866
		public static readonly DependencyProperty ViewportHeightProperty = DocumentViewer.ViewportHeightPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.ShowPageBorders" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.ShowPageBorders" /> dependency property.</returns>
		// Token: 0x04002A73 RID: 10867
		public static readonly DependencyProperty ShowPageBordersProperty = DependencyProperty.Register("ShowPageBorders", typeof(bool), typeof(DocumentViewer), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DocumentViewer.OnShowPageBordersChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.Zoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.Zoom" /> dependency property.</returns>
		// Token: 0x04002A74 RID: 10868
		public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DocumentViewer.OnZoomChanged), new CoerceValueCallback(DocumentViewer.CoerceZoom)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.MaxPagesAcross" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.MaxPagesAcross" /> dependency property. </returns>
		// Token: 0x04002A75 RID: 10869
		public static readonly DependencyProperty MaxPagesAcrossProperty = DependencyProperty.Register("MaxPagesAcross", typeof(int), typeof(DocumentViewer), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DocumentViewer.OnMaxPagesAcrossChanged)), new ValidateValueCallback(DocumentViewer.ValidateMaxPagesAcross));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.VerticalPageSpacing" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.VerticalPageSpacing" /> dependency property.</returns>
		// Token: 0x04002A76 RID: 10870
		public static readonly DependencyProperty VerticalPageSpacingProperty = DependencyProperty.Register("VerticalPageSpacing", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(10.0, new PropertyChangedCallback(DocumentViewer.OnVerticalPageSpacingChanged)), new ValidateValueCallback(DocumentViewer.ValidatePageSpacing));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.HorizontalPageSpacing" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.HorizontalPageSpacing" /> dependency property.</returns>
		// Token: 0x04002A77 RID: 10871
		public static readonly DependencyProperty HorizontalPageSpacingProperty = DependencyProperty.Register("HorizontalPageSpacing", typeof(double), typeof(DocumentViewer), new FrameworkPropertyMetadata(10.0, new PropertyChangedCallback(DocumentViewer.OnHorizontalPageSpacingChanged)), new ValidateValueCallback(DocumentViewer.ValidatePageSpacing));

		// Token: 0x04002A78 RID: 10872
		private static readonly DependencyPropertyKey CanMoveUpPropertyKey = DependencyProperty.RegisterReadOnly("CanMoveUp", typeof(bool), typeof(DocumentViewer), new FrameworkPropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.CanMoveUp" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.CanMoveUp" /> dependency property. </returns>
		// Token: 0x04002A79 RID: 10873
		public static readonly DependencyProperty CanMoveUpProperty = DocumentViewer.CanMoveUpPropertyKey.DependencyProperty;

		// Token: 0x04002A7A RID: 10874
		private static readonly DependencyPropertyKey CanMoveDownPropertyKey = DependencyProperty.RegisterReadOnly("CanMoveDown", typeof(bool), typeof(DocumentViewer), new FrameworkPropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.CanMoveDown" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.CanMoveDown" /> dependency property.</returns>
		// Token: 0x04002A7B RID: 10875
		public static readonly DependencyProperty CanMoveDownProperty = DocumentViewer.CanMoveDownPropertyKey.DependencyProperty;

		// Token: 0x04002A7C RID: 10876
		private static readonly DependencyPropertyKey CanMoveLeftPropertyKey = DependencyProperty.RegisterReadOnly("CanMoveLeft", typeof(bool), typeof(DocumentViewer), new FrameworkPropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.CanMoveLeft" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.CanMoveLeft" /> dependency property. </returns>
		// Token: 0x04002A7D RID: 10877
		public static readonly DependencyProperty CanMoveLeftProperty = DocumentViewer.CanMoveLeftPropertyKey.DependencyProperty;

		// Token: 0x04002A7E RID: 10878
		private static readonly DependencyPropertyKey CanMoveRightPropertyKey = DependencyProperty.RegisterReadOnly("CanMoveRight", typeof(bool), typeof(DocumentViewer), new FrameworkPropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.CanMoveRight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.CanMoveRight" /> dependency property. </returns>
		// Token: 0x04002A7F RID: 10879
		public static readonly DependencyProperty CanMoveRightProperty = DocumentViewer.CanMoveRightPropertyKey.DependencyProperty;

		// Token: 0x04002A80 RID: 10880
		private static readonly DependencyPropertyKey CanIncreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly("CanIncreaseZoom", typeof(bool), typeof(DocumentViewer), new FrameworkPropertyMetadata(true));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.CanIncreaseZoom" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.CanIncreaseZoom" /> dependency property. </returns>
		// Token: 0x04002A81 RID: 10881
		public static readonly DependencyProperty CanIncreaseZoomProperty = DocumentViewer.CanIncreaseZoomPropertyKey.DependencyProperty;

		// Token: 0x04002A82 RID: 10882
		private static readonly DependencyPropertyKey CanDecreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly("CanDecreaseZoom", typeof(bool), typeof(DocumentViewer), new FrameworkPropertyMetadata(true));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DocumentViewer.CanDecreaseZoom" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DocumentViewer.CanDecreaseZoom" /> dependency property. </returns>
		// Token: 0x04002A83 RID: 10883
		public static readonly DependencyProperty CanDecreaseZoomProperty = DocumentViewer.CanDecreaseZoomPropertyKey.DependencyProperty;

		// Token: 0x04002A84 RID: 10884
		private IDocumentScrollInfo _documentScrollInfo;

		// Token: 0x04002A85 RID: 10885
		private ScrollViewer _scrollViewer;

		// Token: 0x04002A86 RID: 10886
		private ZoomPercentageConverter _zoomPercentageConverter;

		// Token: 0x04002A87 RID: 10887
		private FindToolBar _findToolbar;

		// Token: 0x04002A88 RID: 10888
		private const double _horizontalOffsetDefault = 0.0;

		// Token: 0x04002A89 RID: 10889
		private const double _verticalOffsetDefault = 0.0;

		// Token: 0x04002A8A RID: 10890
		private const double _extentWidthDefault = 0.0;

		// Token: 0x04002A8B RID: 10891
		private const double _extentHeightDefault = 0.0;

		// Token: 0x04002A8C RID: 10892
		private const double _viewportWidthDefault = 0.0;

		// Token: 0x04002A8D RID: 10893
		private const double _viewportHeightDefault = 0.0;

		// Token: 0x04002A8E RID: 10894
		private const bool _showPageBordersDefault = true;

		// Token: 0x04002A8F RID: 10895
		private const double _zoomPercentageDefault = 100.0;

		// Token: 0x04002A90 RID: 10896
		private const int _maxPagesAcrossDefault = 1;

		// Token: 0x04002A91 RID: 10897
		private const double _verticalPageSpacingDefault = 10.0;

		// Token: 0x04002A92 RID: 10898
		private const double _horizontalPageSpacingDefault = 10.0;

		// Token: 0x04002A93 RID: 10899
		private const bool _canMoveUpDefault = false;

		// Token: 0x04002A94 RID: 10900
		private const bool _canMoveDownDefault = false;

		// Token: 0x04002A95 RID: 10901
		private const bool _canMoveLeftDefault = false;

		// Token: 0x04002A96 RID: 10902
		private const bool _canMoveRightDefault = false;

		// Token: 0x04002A97 RID: 10903
		private const bool _canIncreaseZoomDefault = true;

		// Token: 0x04002A98 RID: 10904
		private const bool _canDecreaseZoomDefault = true;

		// Token: 0x04002A99 RID: 10905
		private static RoutedUICommand _viewThumbnailsCommand;

		// Token: 0x04002A9A RID: 10906
		private static RoutedUICommand _fitToWidthCommand;

		// Token: 0x04002A9B RID: 10907
		private static RoutedUICommand _fitToHeightCommand;

		// Token: 0x04002A9C RID: 10908
		private static RoutedUICommand _fitToMaxPagesAcrossCommand;

		// Token: 0x04002A9D RID: 10909
		private static double[] _zoomLevelCollection = new double[]
		{
			5000.0,
			4000.0,
			3200.0,
			2400.0,
			2000.0,
			1600.0,
			1200.0,
			800.0,
			400.0,
			300.0,
			200.0,
			175.0,
			150.0,
			125.0,
			100.0,
			75.0,
			66.0,
			50.0,
			33.0,
			25.0,
			10.0,
			5.0
		};

		// Token: 0x04002A9E RID: 10910
		private int _zoomLevelIndex;

		// Token: 0x04002A9F RID: 10911
		private bool _zoomLevelIndexValid;

		// Token: 0x04002AA0 RID: 10912
		private bool _updatingInternalZoomLevel;

		// Token: 0x04002AA1 RID: 10913
		private bool _internalIDSIChange;

		// Token: 0x04002AA2 RID: 10914
		private bool _pageViewCollectionChanged;

		// Token: 0x04002AA3 RID: 10915
		private bool _firstDocumentAssignment = true;

		// Token: 0x04002AA4 RID: 10916
		private const string _findToolBarHostName = "PART_FindToolBarHost";

		// Token: 0x04002AA5 RID: 10917
		private const string _contentHostName = "PART_ContentHost";

		// Token: 0x04002AA6 RID: 10918
		private static DependencyObjectType _dType;
	}
}
