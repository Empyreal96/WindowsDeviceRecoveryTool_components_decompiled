using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Printing;
using System.Windows.Annotations;
using System.Windows.Automation.Peers;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Xps;
using MS.Internal;
using MS.Internal.Annotations.Anchoring;
using MS.Internal.Commands;
using MS.Internal.Controls;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Provides a base class for viewers that are intended to display fixed or flow content (represented by a <see cref="T:System.Windows.Documents.FixedDocument" /> or <see cref="T:System.Windows.Documents.FlowDocument" />, respectively).</summary>
	// Token: 0x02000584 RID: 1412
	[ContentProperty("Document")]
	public abstract class DocumentViewerBase : Control, IAddChild, IServiceProvider
	{
		// Token: 0x06005D9E RID: 23966 RVA: 0x001A5B70 File Offset: 0x001A3D70
		static DocumentViewerBase()
		{
			DocumentViewerBase.CreateCommandBindings();
			EventManager.RegisterClassHandler(typeof(DocumentViewerBase), FrameworkElement.RequestBringIntoViewEvent, new RequestBringIntoViewEventHandler(DocumentViewerBase.HandleRequestBringIntoView));
			TextBoxBase.AutoWordSelectionProperty.OverrideMetadata(typeof(DocumentViewerBase), new FrameworkPropertyMetadata(true));
		}

		/// <summary>Initializes base class values when called by a derived class.</summary>
		// Token: 0x06005D9F RID: 23967 RVA: 0x001A5D1B File Offset: 0x001A3F1B
		protected DocumentViewerBase()
		{
			this._pageViews = new ReadOnlyCollection<DocumentPageView>(new List<DocumentPageView>());
			this.SetFlags(true, DocumentViewerBase.Flags.IsSelectionEnabled);
		}

		/// <summary>Builds the visual tree for the viewer.</summary>
		// Token: 0x06005DA0 RID: 23968 RVA: 0x001A5D3C File Offset: 0x001A3F3C
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.UpdatePageViews();
		}

		/// <summary>Causes the viewer to jump to the previous page of the current document (represented by the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.Document" /> property).</summary>
		// Token: 0x06005DA1 RID: 23969 RVA: 0x001A5D4A File Offset: 0x001A3F4A
		public void PreviousPage()
		{
			this.OnPreviousPageCommand();
		}

		/// <summary>Causes the viewer to jump to the next page in the current document (represented by the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.Document" /> property).</summary>
		// Token: 0x06005DA2 RID: 23970 RVA: 0x001A5D52 File Offset: 0x001A3F52
		public void NextPage()
		{
			this.OnNextPageCommand();
		}

		/// <summary>Causes the viewer to jump to the first page of the current document (represented by the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.Document" /> property).</summary>
		// Token: 0x06005DA3 RID: 23971 RVA: 0x001A5D5A File Offset: 0x001A3F5A
		public void FirstPage()
		{
			this.OnFirstPageCommand();
		}

		/// <summary>Causes the viewer to jump to the last page in the current document (represented by the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.Document" /> property).</summary>
		// Token: 0x06005DA4 RID: 23972 RVA: 0x001A5D62 File Offset: 0x001A3F62
		public void LastPage()
		{
			this.OnLastPageCommand();
		}

		/// <summary>Causes the viewer to jump to a specified page number.</summary>
		/// <param name="pageNumber">The number of the page to jump to.</param>
		// Token: 0x06005DA5 RID: 23973 RVA: 0x001A5D6A File Offset: 0x001A3F6A
		public void GoToPage(int pageNumber)
		{
			this.OnGoToPageCommand(pageNumber);
		}

		/// <summary>Invokes a standard Print dialog which can be used to print the contents of the viewer and configure printing preferences.</summary>
		// Token: 0x06005DA6 RID: 23974 RVA: 0x001A5D73 File Offset: 0x001A3F73
		public void Print()
		{
			this.OnPrintCommand();
		}

		/// <summary>Cancels any current printing job.</summary>
		// Token: 0x06005DA7 RID: 23975 RVA: 0x001A5D7B File Offset: 0x001A3F7B
		public void CancelPrint()
		{
			this.OnCancelPrintCommand();
		}

		/// <summary>Returns a value that indicates whether or the viewer is able to jump to the specified page number.</summary>
		/// <param name="pageNumber">A page number to check for as a valid jump target.</param>
		/// <returns>A Boolean value that indicates whether or the viewer is able to jump to the specified page number.</returns>
		// Token: 0x06005DA8 RID: 23976 RVA: 0x001A5D83 File Offset: 0x001A3F83
		public virtual bool CanGoToPage(int pageNumber)
		{
			return (pageNumber > 0 && pageNumber <= this.PageCount) || (this._document != null && pageNumber - 1 == this.PageCount && !this._document.DocumentPaginator.IsPageCountValid);
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Documents.IDocumentPaginatorSource" /> to be paginated and displayed by the viewer. </summary>
		/// <returns>A <see cref="T:System.Windows.Documents.IDocumentPaginatorSource" /> to be paginated and displayed by the viewer.The default property is null.</returns>
		// Token: 0x1700169F RID: 5791
		// (get) Token: 0x06005DA9 RID: 23977 RVA: 0x001A5DBC File Offset: 0x001A3FBC
		// (set) Token: 0x06005DAA RID: 23978 RVA: 0x001A5DC4 File Offset: 0x001A3FC4
		public IDocumentPaginatorSource Document
		{
			get
			{
				return this._document;
			}
			set
			{
				base.SetValue(DocumentViewerBase.DocumentProperty, value);
			}
		}

		/// <summary>Gets the total number of pages in the current <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.Document" />. </summary>
		/// <returns>The number of pages in the current document, or 0 if no document is currently loaded.This property has no default value.</returns>
		// Token: 0x170016A0 RID: 5792
		// (get) Token: 0x06005DAB RID: 23979 RVA: 0x001A5DD2 File Offset: 0x001A3FD2
		public int PageCount
		{
			get
			{
				return (int)base.GetValue(DocumentViewerBase.PageCountProperty);
			}
		}

		/// <summary>Gets the page number for the current master page. </summary>
		/// <returns>The page number for the current master page, or 0 if no Document is currently loaded.This property has no default value.</returns>
		// Token: 0x170016A1 RID: 5793
		// (get) Token: 0x06005DAC RID: 23980 RVA: 0x001A5DE4 File Offset: 0x001A3FE4
		public virtual int MasterPageNumber
		{
			get
			{
				return (int)base.GetValue(DocumentViewerBase.MasterPageNumberProperty);
			}
		}

		/// <summary>Gets a value that indicates whether or not the viewer can jump to the previous page in the current <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.Document" />. </summary>
		/// <returns>
		///     true if the viewer can jump to the previous page; otherwise, false.This property has no default value.</returns>
		// Token: 0x170016A2 RID: 5794
		// (get) Token: 0x06005DAD RID: 23981 RVA: 0x001A5DF6 File Offset: 0x001A3FF6
		public virtual bool CanGoToPreviousPage
		{
			get
			{
				return (bool)base.GetValue(DocumentViewerBase.CanGoToPreviousPageProperty);
			}
		}

		/// <summary>Gets a value that indicates whether or not the viewer can jump to the next page in the current <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.Document" />. </summary>
		/// <returns>
		///     true if the viewer can jump to the next page; otherwise, false.This property has no default value.</returns>
		// Token: 0x170016A3 RID: 5795
		// (get) Token: 0x06005DAE RID: 23982 RVA: 0x001A5E08 File Offset: 0x001A4008
		public virtual bool CanGoToNextPage
		{
			get
			{
				return (bool)base.GetValue(DocumentViewerBase.CanGoToNextPageProperty);
			}
		}

		/// <summary>Gets a read-only collection of the active <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> objects contained within the viewer.</summary>
		/// <returns>A read-only collection of the active <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> objects contained within the viewer.This property has no default value.</returns>
		// Token: 0x170016A4 RID: 5796
		// (get) Token: 0x06005DAF RID: 23983 RVA: 0x001A5E1A File Offset: 0x001A401A
		[CLSCompliant(false)]
		public ReadOnlyCollection<DocumentPageView> PageViews
		{
			get
			{
				return this._pageViews;
			}
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.IsMasterPage" /> attached property for a specified dependency object.</summary>
		/// <param name="element">A dependency object from which to retrieve the value of <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.IsMasterPage" />.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.IsMasterPage" /> attached property, read from the dependency object specified by element.</returns>
		/// <exception cref="T:System.ArgumentNullException">Raised if <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06005DB0 RID: 23984 RVA: 0x001A5E22 File Offset: 0x001A4022
		public static bool GetIsMasterPage(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(DocumentViewerBase.IsMasterPageProperty);
		}

		/// <summary>Sets the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.IsMasterPage" /> attached property on a specified dependency object.</summary>
		/// <param name="element">A dependency object on which to set the value of <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.IsMasterPage" />.</param>
		/// <param name="value">The new value to set the property to.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised if element is null.</exception>
		// Token: 0x06005DB1 RID: 23985 RVA: 0x001A5E42 File Offset: 0x001A4042
		public static void SetIsMasterPage(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(DocumentViewerBase.IsMasterPageProperty, value);
		}

		/// <summary>Occurs when the collection of <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> items associated with this viewer (represented by the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.PageViews" /> property) changes.</summary>
		// Token: 0x14000115 RID: 277
		// (add) Token: 0x06005DB2 RID: 23986 RVA: 0x001A5E60 File Offset: 0x001A4060
		// (remove) Token: 0x06005DB3 RID: 23987 RVA: 0x001A5E98 File Offset: 0x001A4098
		public event EventHandler PageViewsChanged;

		/// <summary>Creates and returns an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this viewer.</summary>
		/// <returns>An <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this viewer.</returns>
		// Token: 0x06005DB4 RID: 23988 RVA: 0x001A5ECD File Offset: 0x001A40CD
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DocumentViewerBaseAutomationPeer(this);
		}

		/// <summary>Invoked when the DPI scale changes. Sets the DPI of the flow document to the new scale.</summary>
		/// <param name="oldDpiScaleInfo">The previous DPI scale setting.</param>
		/// <param name="newDpiScaleInfo">The new DPI scale setting.</param>
		// Token: 0x06005DB5 RID: 23989 RVA: 0x001A5ED8 File Offset: 0x001A40D8
		protected override void OnDpiChanged(DpiScale oldDpiScaleInfo, DpiScale newDpiScaleInfo)
		{
			FlowDocument flowDocument = this._document as FlowDocument;
			if (flowDocument != null)
			{
				flowDocument.SetDpi(newDpiScaleInfo);
			}
		}

		/// <summary>Causes the working <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.PageViews" /> collection to be re-built.</summary>
		// Token: 0x06005DB6 RID: 23990 RVA: 0x001A5EFB File Offset: 0x001A40FB
		protected void InvalidatePageViews()
		{
			this.UpdatePageViews();
			base.InvalidateMeasure();
		}

		/// <summary>Returns the current master <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> for this viewer.</summary>
		/// <returns>The current master <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> for this viewer, or null if no master <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> can be found.</returns>
		// Token: 0x06005DB7 RID: 23991 RVA: 0x001A5F0C File Offset: 0x001A410C
		protected DocumentPageView GetMasterPageView()
		{
			DocumentPageView documentPageView = null;
			for (int i = 0; i < this._pageViews.Count; i++)
			{
				if (DocumentViewerBase.GetIsMasterPage(this._pageViews[i]))
				{
					documentPageView = this._pageViews[i];
					break;
				}
			}
			if (documentPageView == null)
			{
				documentPageView = ((this._pageViews.Count > 0) ? this._pageViews[0] : null);
			}
			return documentPageView;
		}

		/// <summary>Creates and returns a new, read-only collection of <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> objects that are associated with the current display document (represented by the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.Document" /> property).</summary>
		/// <param name="changed">Returns <see langword="true" /> on the first call to <see cref="M:System.Windows.Controls.Primitives.DocumentViewerBase.GetPageViewsCollection(System.Boolean@)" /> or if the collection has not changed since the previous <see cref="M:System.Windows.Controls.Primitives.DocumentViewerBase.GetPageViewsCollection(System.Boolean@)" /> call; otherwise, <see langword="false" /> if the collection has changed since the last <see cref="M:System.Windows.Controls.Primitives.DocumentViewerBase.GetPageViewsCollection(System.Boolean@)" /> call.</param>
		/// <returns>A read-only collection of <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> objects that are associated with the current display document.</returns>
		// Token: 0x06005DB8 RID: 23992 RVA: 0x001A5F78 File Offset: 0x001A4178
		protected virtual ReadOnlyCollection<DocumentPageView> GetPageViewsCollection(out bool changed)
		{
			List<DocumentPageView> list = new List<DocumentPageView>(1);
			this.FindDocumentPageViews(this, list);
			AdornerDecorator adornerDecorator = this.FindAdornerDecorator(this);
			this.TextEditorRenderScope = ((adornerDecorator != null) ? (adornerDecorator.Child as FrameworkElement) : null);
			for (int i = 0; i < this._pageViews.Count; i++)
			{
				this._pageViews[i].DocumentPaginator = null;
			}
			changed = true;
			return new ReadOnlyCollection<DocumentPageView>(list);
		}

		/// <summary>Called whenever the working set of <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> objects for this viewer (represented by the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.PageViews" /> property) is modified.</summary>
		// Token: 0x06005DB9 RID: 23993 RVA: 0x001A5FE4 File Offset: 0x001A41E4
		protected virtual void OnPageViewsChanged()
		{
			if (this.PageViewsChanged != null)
			{
				this.PageViewsChanged(this, EventArgs.Empty);
			}
			this.OnMasterPageNumberChanged();
		}

		/// <summary>Called whenever the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.MasterPageNumber" /> property is modified.</summary>
		// Token: 0x06005DBA RID: 23994 RVA: 0x001A6005 File Offset: 0x001A4205
		protected virtual void OnMasterPageNumberChanged()
		{
			this.UpdateReadOnlyProperties(true, true);
		}

		/// <summary>Invoked whenever the <see cref="E:System.Windows.FrameworkElement.RequestBringIntoView" /> event reaches an element derived from this class in its route.  Implement this method to add class handling for this event.</summary>
		/// <param name="element">The element from which the <see cref="E:System.Windows.FrameworkElement.RequestBringIntoView" /> event originated.</param>
		/// <param name="rect">A rectangular region, in the coordinate space of <paramref name="element" />, which should be made visible.</param>
		/// <param name="pageNumber">The page number for the page that contains <paramref name="element" />.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised if element is null.</exception>
		// Token: 0x06005DBB RID: 23995 RVA: 0x001A600F File Offset: 0x001A420F
		protected virtual void OnBringIntoView(DependencyObject element, Rect rect, int pageNumber)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			this.OnGoToPageCommand(pageNumber);
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.NavigationCommands.PreviousPage" /> routed command.</summary>
		// Token: 0x06005DBC RID: 23996 RVA: 0x001A6026 File Offset: 0x001A4226
		protected virtual void OnPreviousPageCommand()
		{
			if (this.CanGoToPreviousPage)
			{
				this.ShiftPagesByOffset(-1);
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.NavigationCommands.NextPage" /> routed command.</summary>
		// Token: 0x06005DBD RID: 23997 RVA: 0x001A6037 File Offset: 0x001A4237
		protected virtual void OnNextPageCommand()
		{
			if (this.CanGoToNextPage)
			{
				this.ShiftPagesByOffset(1);
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.NavigationCommands.FirstPage" /> routed command.</summary>
		// Token: 0x06005DBE RID: 23998 RVA: 0x001A6048 File Offset: 0x001A4248
		protected virtual void OnFirstPageCommand()
		{
			this.ShiftPagesByOffset(1 - this.MasterPageNumber);
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.NavigationCommands.LastPage" /> routed command.</summary>
		// Token: 0x06005DBF RID: 23999 RVA: 0x001A6058 File Offset: 0x001A4258
		protected virtual void OnLastPageCommand()
		{
			this.ShiftPagesByOffset(this.PageCount - this.MasterPageNumber);
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.NavigationCommands.GoToPage" /> routed command.</summary>
		/// <param name="pageNumber">The number of the page to jump to.</param>
		// Token: 0x06005DC0 RID: 24000 RVA: 0x001A606D File Offset: 0x001A426D
		protected virtual void OnGoToPageCommand(int pageNumber)
		{
			if (this.CanGoToPage(pageNumber))
			{
				this.ShiftPagesByOffset(pageNumber - this.MasterPageNumber);
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.ApplicationCommands.Print" /> routed command.</summary>
		// Token: 0x06005DC1 RID: 24001 RVA: 0x001A6088 File Offset: 0x001A4288
		protected virtual void OnPrintCommand()
		{
			PrintDocumentImageableArea printDocumentImageableArea = null;
			if (this._documentWriter != null)
			{
				return;
			}
			if (this._document != null)
			{
				XpsDocumentWriter xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(ref printDocumentImageableArea);
				if (xpsDocumentWriter != null && printDocumentImageableArea != null)
				{
					this._documentWriter = xpsDocumentWriter;
					this._documentWriter.WritingCompleted += this.HandlePrintCompleted;
					this._documentWriter.WritingCancelled += this.HandlePrintCancelled;
					CommandManager.InvalidateRequerySuggested();
					if (this._document is FixedDocumentSequence)
					{
						xpsDocumentWriter.WriteAsync(this._document as FixedDocumentSequence);
						return;
					}
					if (this._document is FixedDocument)
					{
						xpsDocumentWriter.WriteAsync(this._document as FixedDocument);
						return;
					}
					xpsDocumentWriter.WriteAsync(this._document.DocumentPaginator);
				}
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.ApplicationCommands.CancelPrint" /> routed command.</summary>
		// Token: 0x06005DC2 RID: 24002 RVA: 0x001A6148 File Offset: 0x001A4348
		protected virtual void OnCancelPrintCommand()
		{
			if (this._documentWriter != null)
			{
				this._documentWriter.CancelAsync();
			}
		}

		/// <summary>Called whenever the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.Document" /> property is modified.</summary>
		// Token: 0x06005DC3 RID: 24003 RVA: 0x001A6160 File Offset: 0x001A4360
		protected virtual void OnDocumentChanged()
		{
			for (int i = 0; i < this._pageViews.Count; i++)
			{
				this._pageViews[i].DocumentPaginator = ((this._document != null) ? this._document.DocumentPaginator : null);
			}
			this.UpdateReadOnlyProperties(true, true);
			this.AttachTextEditor();
		}

		/// <summary>Gets an enumerator for the children in the logical tree of the viewer.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to enumerate the logical children of the viewer.This property has no default value.</returns>
		// Token: 0x170016A5 RID: 5797
		// (get) Token: 0x06005DC4 RID: 24004 RVA: 0x001A61B8 File Offset: 0x001A43B8
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (base.HasLogicalChildren && this._document != null)
				{
					return new SingleChildEnumerator(this._document);
				}
				return EmptyEnumerator.Instance;
			}
		}

		// Token: 0x06005DC5 RID: 24005 RVA: 0x001A61DB File Offset: 0x001A43DB
		internal bool IsMasterPageView(DocumentPageView pageView)
		{
			Invariant.Assert(pageView != null);
			return pageView == this.GetMasterPageView();
		}

		// Token: 0x06005DC6 RID: 24006 RVA: 0x001A61F0 File Offset: 0x001A43F0
		internal ITextRange Find(FindToolBar findToolBar)
		{
			ITextView masterPageTextView = null;
			DocumentPageView masterPageView = this.GetMasterPageView();
			if (masterPageView != null && masterPageView != null)
			{
				masterPageTextView = (((IServiceProvider)masterPageView).GetService(typeof(ITextView)) as ITextView);
			}
			return DocumentViewerHelper.Find(findToolBar, this._textEditor, this._textView, masterPageTextView);
		}

		// Token: 0x170016A6 RID: 5798
		// (get) Token: 0x06005DC7 RID: 24007 RVA: 0x001A6235 File Offset: 0x001A4435
		// (set) Token: 0x06005DC8 RID: 24008 RVA: 0x001A623F File Offset: 0x001A443F
		internal bool IsSelectionEnabled
		{
			get
			{
				return this.CheckFlags(DocumentViewerBase.Flags.IsSelectionEnabled);
			}
			set
			{
				this.SetFlags(value, DocumentViewerBase.Flags.IsSelectionEnabled);
				this.AttachTextEditor();
			}
		}

		// Token: 0x170016A7 RID: 5799
		// (get) Token: 0x06005DC9 RID: 24009 RVA: 0x001A6250 File Offset: 0x001A4450
		internal TextEditor TextEditor
		{
			get
			{
				return this._textEditor;
			}
		}

		// Token: 0x170016A8 RID: 5800
		// (get) Token: 0x06005DCA RID: 24010 RVA: 0x001A6258 File Offset: 0x001A4458
		// (set) Token: 0x06005DCB RID: 24011 RVA: 0x001A6260 File Offset: 0x001A4460
		internal FrameworkElement TextEditorRenderScope
		{
			get
			{
				return this._textEditorRenderScope;
			}
			set
			{
				this._textEditorRenderScope = value;
				this.AttachTextEditor();
			}
		}

		// Token: 0x06005DCC RID: 24012 RVA: 0x001A6270 File Offset: 0x001A4470
		private ITextPointer GetMasterPageTextPointer(bool startOfPage)
		{
			ITextPointer textPointer = null;
			DocumentPageView masterPageView = this.GetMasterPageView();
			if (masterPageView != null && masterPageView != null)
			{
				ITextView textView = ((IServiceProvider)masterPageView).GetService(typeof(ITextView)) as ITextView;
				if (textView != null && textView.IsValid)
				{
					foreach (TextSegment textSegment in textView.TextSegments)
					{
						if (!textSegment.IsNull)
						{
							if (textPointer == null)
							{
								textPointer = (startOfPage ? textSegment.Start : textSegment.End);
							}
							else if (startOfPage)
							{
								if (textSegment.Start.CompareTo(textPointer) < 0)
								{
									textPointer = textSegment.Start;
								}
							}
							else if (textSegment.End.CompareTo(textPointer) > 0)
							{
								textPointer = textSegment.End;
							}
						}
					}
				}
			}
			return textPointer;
		}

		// Token: 0x06005DCD RID: 24013 RVA: 0x001A6350 File Offset: 0x001A4550
		private void UpdatePageViews()
		{
			bool flag;
			ReadOnlyCollection<DocumentPageView> pageViewsCollection = this.GetPageViewsCollection(out flag);
			if (flag)
			{
				this.VerifyDocumentPageViews(pageViewsCollection);
				this._pageViews = pageViewsCollection;
				for (int i = 0; i < this._pageViews.Count; i++)
				{
					this._pageViews[i].DocumentPaginator = ((this._document != null) ? this._document.DocumentPaginator : null);
				}
				if (this._textView != null)
				{
					this._textView.OnPagesUpdated();
				}
				this.OnPageViewsChanged();
			}
		}

		// Token: 0x06005DCE RID: 24014 RVA: 0x001A63D0 File Offset: 0x001A45D0
		private void VerifyDocumentPageViews(ReadOnlyCollection<DocumentPageView> pageViews)
		{
			bool flag = false;
			if (pageViews == null)
			{
				throw new ArgumentException(SR.Get("DocumentViewerPageViewsCollectionEmpty"));
			}
			for (int i = 0; i < pageViews.Count; i++)
			{
				if (DocumentViewerBase.GetIsMasterPage(pageViews[i]))
				{
					if (flag)
					{
						throw new ArgumentException(SR.Get("DocumentViewerOneMasterPage"));
					}
					flag = true;
				}
			}
		}

		// Token: 0x06005DCF RID: 24015 RVA: 0x001A6428 File Offset: 0x001A4628
		private void FindDocumentPageViews(Visual root, List<DocumentPageView> pageViews)
		{
			Invariant.Assert(root != null);
			Invariant.Assert(pageViews != null);
			int internalVisualChildrenCount = root.InternalVisualChildrenCount;
			for (int i = 0; i < internalVisualChildrenCount; i++)
			{
				Visual visual = root.InternalGetVisualChild(i);
				FrameworkElement frameworkElement = visual as FrameworkElement;
				if (frameworkElement != null)
				{
					if (frameworkElement.TemplatedParent != null)
					{
						if (frameworkElement is DocumentPageView)
						{
							pageViews.Add(frameworkElement as DocumentPageView);
						}
						else
						{
							this.FindDocumentPageViews(frameworkElement, pageViews);
						}
					}
				}
				else
				{
					this.FindDocumentPageViews(visual, pageViews);
				}
			}
		}

		// Token: 0x06005DD0 RID: 24016 RVA: 0x001A649C File Offset: 0x001A469C
		private AdornerDecorator FindAdornerDecorator(Visual root)
		{
			Invariant.Assert(root != null);
			AdornerDecorator adornerDecorator = null;
			int internalVisualChildrenCount = root.InternalVisualChildrenCount;
			for (int i = 0; i < internalVisualChildrenCount; i++)
			{
				Visual visual = root.InternalGetVisualChild(i);
				FrameworkElement frameworkElement = visual as FrameworkElement;
				if (frameworkElement != null)
				{
					if (frameworkElement.TemplatedParent != null)
					{
						if (frameworkElement is AdornerDecorator)
						{
							adornerDecorator = (AdornerDecorator)frameworkElement;
						}
						else if (!(frameworkElement is DocumentPageView))
						{
							adornerDecorator = this.FindAdornerDecorator(frameworkElement);
						}
					}
				}
				else
				{
					adornerDecorator = this.FindAdornerDecorator(visual);
				}
				if (adornerDecorator != null)
				{
					break;
				}
			}
			return adornerDecorator;
		}

		// Token: 0x06005DD1 RID: 24017 RVA: 0x001A6514 File Offset: 0x001A4714
		private void AttachTextEditor()
		{
			AnnotationService service = AnnotationService.GetService(this);
			if (this._textEditor != null)
			{
				this._textEditor.OnDetach();
				this._textEditor = null;
				if (this._textView.TextContainer.TextView == this._textView)
				{
					this._textView.TextContainer.TextView = null;
				}
				this._textView = null;
			}
			if (service != null)
			{
				service.Disable();
			}
			ITextContainer textContainer = this.TextContainer;
			if (textContainer != null && this.TextEditorRenderScope != null && textContainer.TextSelection == null)
			{
				this._textView = new MultiPageTextView(this, this.TextEditorRenderScope, textContainer);
				this._textEditor = new TextEditor(textContainer, this, false);
				this._textEditor.IsReadOnly = !DocumentViewerBase.IsEditingEnabled;
				this._textEditor.TextView = this._textView;
				textContainer.TextView = this._textView;
			}
			if (service != null)
			{
				service.Enable(service.Store);
			}
		}

		// Token: 0x06005DD2 RID: 24018 RVA: 0x001A65F5 File Offset: 0x001A47F5
		private void HandlePrintCompleted(object sender, WritingCompletedEventArgs e)
		{
			this.CleanUpPrintOperation();
		}

		// Token: 0x06005DD3 RID: 24019 RVA: 0x001A65F5 File Offset: 0x001A47F5
		private void HandlePrintCancelled(object sender, WritingCancelledEventArgs e)
		{
			this.CleanUpPrintOperation();
		}

		// Token: 0x06005DD4 RID: 24020 RVA: 0x001A65FD File Offset: 0x001A47FD
		private void HandlePaginationCompleted(object sender, EventArgs e)
		{
			this.UpdateReadOnlyProperties(true, false);
		}

		// Token: 0x06005DD5 RID: 24021 RVA: 0x001A65FD File Offset: 0x001A47FD
		private void HandlePaginationProgress(object sender, EventArgs e)
		{
			this.UpdateReadOnlyProperties(true, false);
		}

		// Token: 0x06005DD6 RID: 24022 RVA: 0x001A6608 File Offset: 0x001A4808
		private void HandleGetPageNumberCompleted(object sender, GetPageNumberCompletedEventArgs e)
		{
			this.UpdateReadOnlyProperties(true, false);
			if (this._document != null && sender == this._document.DocumentPaginator && e != null && !e.Cancelled && e.Error == null)
			{
				DocumentViewerBase.BringIntoViewState bringIntoViewState = e.UserState as DocumentViewerBase.BringIntoViewState;
				if (bringIntoViewState != null && bringIntoViewState.Source == this)
				{
					this.OnBringIntoView(bringIntoViewState.TargetObject, bringIntoViewState.TargetRect, e.PageNumber + 1);
				}
			}
		}

		// Token: 0x06005DD7 RID: 24023 RVA: 0x001A6678 File Offset: 0x001A4878
		private void HandleRequestBringIntoView(RequestBringIntoViewEventArgs args)
		{
			Rect rect = Rect.Empty;
			if (args != null && args.TargetObject != null && this._document is DependencyObject)
			{
				DependencyObject dependencyObject = this._document as DependencyObject;
				if (args.TargetObject == this._document)
				{
					this.OnGoToPageCommand(1);
					args.Handled = true;
				}
				else
				{
					DependencyObject dependencyObject2 = args.TargetObject;
					while (dependencyObject2 != null && dependencyObject2 != dependencyObject)
					{
						FrameworkElement frameworkElement = dependencyObject2 as FrameworkElement;
						if (frameworkElement != null && frameworkElement.TemplatedParent != null)
						{
							dependencyObject2 = frameworkElement.TemplatedParent;
						}
						else
						{
							dependencyObject2 = LogicalTreeHelper.GetParent(dependencyObject2);
						}
					}
					if (dependencyObject2 != null)
					{
						if (args.TargetObject is UIElement)
						{
							UIElement uielement = (UIElement)args.TargetObject;
							if (VisualTreeHelper.IsAncestorOf(this, uielement))
							{
								rect = args.TargetRect;
								if (rect.IsEmpty)
								{
									rect = new Rect(uielement.RenderSize);
								}
								GeneralTransform generalTransform = uielement.TransformToAncestor(this);
								rect = generalTransform.TransformBounds(rect);
								rect.IntersectsWith(new Rect(base.RenderSize));
							}
						}
						if (rect.IsEmpty)
						{
							DynamicDocumentPaginator dynamicDocumentPaginator = this._document.DocumentPaginator as DynamicDocumentPaginator;
							if (dynamicDocumentPaginator != null)
							{
								ContentPosition objectPosition = dynamicDocumentPaginator.GetObjectPosition(args.TargetObject);
								if (objectPosition != null && objectPosition != ContentPosition.Missing)
								{
									DocumentViewerBase.BringIntoViewState userState = new DocumentViewerBase.BringIntoViewState(this, objectPosition, args.TargetObject, args.TargetRect);
									dynamicDocumentPaginator.GetPageNumberAsync(objectPosition, userState);
								}
							}
						}
						args.Handled = true;
					}
				}
				if (args.Handled)
				{
					if (rect.IsEmpty)
					{
						base.BringIntoView();
						return;
					}
					base.BringIntoView(rect);
				}
			}
		}

		// Token: 0x06005DD8 RID: 24024 RVA: 0x001A6800 File Offset: 0x001A4A00
		private void UpdateReadOnlyProperties(bool pageCountChanged, bool masterPageChanged)
		{
			if (pageCountChanged)
			{
				base.SetValue(DocumentViewerBase.PageCountPropertyKey, (this._document != null) ? this._document.DocumentPaginator.PageCount : 0);
			}
			bool flag = false;
			if (masterPageChanged)
			{
				int num = 0;
				if (this._document != null && this._pageViews.Count > 0)
				{
					DocumentPageView masterPageView = this.GetMasterPageView();
					if (masterPageView != null)
					{
						num = masterPageView.PageNumber + 1;
					}
				}
				base.SetValue(DocumentViewerBase.MasterPageNumberPropertyKey, num);
				base.SetValue(DocumentViewerBase.CanGoToPreviousPagePropertyKey, this.MasterPageNumber > 1);
				flag = true;
			}
			if (pageCountChanged || masterPageChanged)
			{
				bool value = false;
				if (this._document != null)
				{
					value = (this.MasterPageNumber < this._document.DocumentPaginator.PageCount || !this._document.DocumentPaginator.IsPageCountValid);
				}
				base.SetValue(DocumentViewerBase.CanGoToNextPagePropertyKey, value);
				flag = true;
			}
			if (flag)
			{
				CommandManager.InvalidateRequerySuggested();
			}
		}

		// Token: 0x06005DD9 RID: 24025 RVA: 0x001A68E8 File Offset: 0x001A4AE8
		private void ShiftPagesByOffset(int offset)
		{
			if (offset != 0)
			{
				for (int i = 0; i < this._pageViews.Count; i++)
				{
					this._pageViews[i].PageNumber += offset;
				}
				this.OnMasterPageNumberChanged();
			}
		}

		// Token: 0x06005DDA RID: 24026 RVA: 0x001A692D File Offset: 0x001A4B2D
		private void SetFlags(bool value, DocumentViewerBase.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x06005DDB RID: 24027 RVA: 0x001A694B File Offset: 0x001A4B4B
		private bool CheckFlags(DocumentViewerBase.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x06005DDC RID: 24028 RVA: 0x001A6958 File Offset: 0x001A4B58
		private void DocumentChanged(IDocumentPaginatorSource oldDocument, IDocumentPaginatorSource newDocument)
		{
			this._document = newDocument;
			if (oldDocument != null)
			{
				if (this.CheckFlags(DocumentViewerBase.Flags.DocumentAsLogicalChild))
				{
					base.RemoveLogicalChild(oldDocument);
				}
				DynamicDocumentPaginator dynamicDocumentPaginator = oldDocument.DocumentPaginator as DynamicDocumentPaginator;
				if (dynamicDocumentPaginator != null)
				{
					dynamicDocumentPaginator.PaginationProgress -= new PaginationProgressEventHandler(this.HandlePaginationProgress);
					dynamicDocumentPaginator.PaginationCompleted -= this.HandlePaginationCompleted;
					dynamicDocumentPaginator.GetPageNumberCompleted -= this.HandleGetPageNumberCompleted;
				}
				DependencyObject dependencyObject = oldDocument as DependencyObject;
				if (dependencyObject != null)
				{
					dependencyObject.ClearValue(PathNode.HiddenParentProperty);
				}
			}
			DependencyObject dependencyObject2 = this._document as DependencyObject;
			if (dependencyObject2 != null && LogicalTreeHelper.GetParent(dependencyObject2) != null && dependencyObject2 is ContentElement)
			{
				ContentOperations.SetParent((ContentElement)dependencyObject2, this);
				this.SetFlags(false, DocumentViewerBase.Flags.DocumentAsLogicalChild);
			}
			else
			{
				this.SetFlags(true, DocumentViewerBase.Flags.DocumentAsLogicalChild);
			}
			if (this._document != null)
			{
				if (this.CheckFlags(DocumentViewerBase.Flags.DocumentAsLogicalChild))
				{
					base.AddLogicalChild(this._document);
				}
				DynamicDocumentPaginator dynamicDocumentPaginator = this._document.DocumentPaginator as DynamicDocumentPaginator;
				if (dynamicDocumentPaginator != null)
				{
					dynamicDocumentPaginator.PaginationProgress += new PaginationProgressEventHandler(this.HandlePaginationProgress);
					dynamicDocumentPaginator.PaginationCompleted += this.HandlePaginationCompleted;
					dynamicDocumentPaginator.GetPageNumberCompleted += this.HandleGetPageNumberCompleted;
				}
				DependencyObject dependencyObject3 = this._document as DependencyObject;
				FlowDocument flowDocument;
				if (this._document is FixedDocument || this._document is FixedDocumentSequence)
				{
					base.ClearValue(AnnotationService.DataIdProperty);
					AnnotationService.SetSubTreeProcessorId(this, FixedPageProcessor.Id);
					dependencyObject3.SetValue(PathNode.HiddenParentProperty, this);
					AnnotationService service = AnnotationService.GetService(this);
					if (service != null)
					{
						service.LocatorManager.RegisterSelectionProcessor(new FixedTextSelectionProcessor(), typeof(TextRange));
						service.LocatorManager.RegisterSelectionProcessor(new FixedTextSelectionProcessor(), typeof(TextAnchor));
					}
				}
				else if ((flowDocument = (this._document as FlowDocument)) != null)
				{
					flowDocument.SetDpi(base.GetDpi());
					flowDocument.SetValue(PathNode.HiddenParentProperty, this);
					AnnotationService service2 = AnnotationService.GetService(this);
					if (service2 != null)
					{
						service2.LocatorManager.RegisterSelectionProcessor(new TextSelectionProcessor(), typeof(TextRange));
						service2.LocatorManager.RegisterSelectionProcessor(new TextSelectionProcessor(), typeof(TextAnchor));
						service2.LocatorManager.RegisterSelectionProcessor(new TextViewSelectionProcessor(), typeof(DocumentViewerBase));
					}
					AnnotationService.SetDataId(this, "FlowDocument");
				}
				else
				{
					base.ClearValue(AnnotationService.SubTreeProcessorIdProperty);
					base.ClearValue(AnnotationService.DataIdProperty);
				}
			}
			DocumentViewerBaseAutomationPeer documentViewerBaseAutomationPeer = UIElementAutomationPeer.FromElement(this) as DocumentViewerBaseAutomationPeer;
			if (documentViewerBaseAutomationPeer != null)
			{
				documentViewerBaseAutomationPeer.InvalidatePeer();
			}
			this.OnDocumentChanged();
		}

		// Token: 0x06005DDD RID: 24029 RVA: 0x001A6BE0 File Offset: 0x001A4DE0
		private void CleanUpPrintOperation()
		{
			if (this._documentWriter != null)
			{
				this._documentWriter.WritingCompleted -= this.HandlePrintCompleted;
				this._documentWriter.WritingCancelled -= this.HandlePrintCancelled;
				this._documentWriter = null;
				CommandManager.InvalidateRequerySuggested();
			}
		}

		// Token: 0x06005DDE RID: 24030 RVA: 0x001A6C30 File Offset: 0x001A4E30
		private static void CreateCommandBindings()
		{
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(DocumentViewerBase.ExecutedRoutedEventHandler);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(DocumentViewerBase.CanExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewerBase), NavigationCommands.PreviousPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewerBase), NavigationCommands.NextPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewerBase), NavigationCommands.FirstPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewerBase), NavigationCommands.LastPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewerBase), NavigationCommands.GoToPage, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewerBase), ApplicationCommands.Print, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.P, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(DocumentViewerBase), ApplicationCommands.CancelPrint, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			TextEditor.RegisterCommandHandlers(typeof(DocumentViewerBase), true, !DocumentViewerBase.IsEditingEnabled, true);
		}

		// Token: 0x06005DDF RID: 24031 RVA: 0x001A6D14 File Offset: 0x001A4F14
		private static void CanExecuteRoutedEventHandler(object target, CanExecuteRoutedEventArgs args)
		{
			DocumentViewerBase documentViewerBase = target as DocumentViewerBase;
			Invariant.Assert(documentViewerBase != null, "Target of CanExecuteRoutedEventHandler must be DocumentViewerBase.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (args.Command == ApplicationCommands.Print)
			{
				args.CanExecute = (documentViewerBase.Document != null && documentViewerBase._documentWriter == null);
				args.Handled = true;
				return;
			}
			if (args.Command == ApplicationCommands.CancelPrint)
			{
				args.CanExecute = (documentViewerBase._documentWriter != null);
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x06005DE0 RID: 24032 RVA: 0x001A6D98 File Offset: 0x001A4F98
		private static void ExecutedRoutedEventHandler(object target, ExecutedRoutedEventArgs args)
		{
			DocumentViewerBase documentViewerBase = target as DocumentViewerBase;
			Invariant.Assert(documentViewerBase != null, "Target of ExecuteEvent must be DocumentViewerBase.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (args.Command == NavigationCommands.PreviousPage)
			{
				documentViewerBase.OnPreviousPageCommand();
				return;
			}
			if (args.Command == NavigationCommands.NextPage)
			{
				documentViewerBase.OnNextPageCommand();
				return;
			}
			if (args.Command == NavigationCommands.FirstPage)
			{
				documentViewerBase.OnFirstPageCommand();
				return;
			}
			if (args.Command == NavigationCommands.LastPage)
			{
				documentViewerBase.OnLastPageCommand();
				return;
			}
			if (args.Command == NavigationCommands.GoToPage)
			{
				if (args.Parameter != null)
				{
					int num = -1;
					try
					{
						num = Convert.ToInt32(args.Parameter, CultureInfo.CurrentCulture);
					}
					catch (InvalidCastException)
					{
					}
					catch (OverflowException)
					{
					}
					catch (FormatException)
					{
					}
					if (num >= 0)
					{
						documentViewerBase.OnGoToPageCommand(num);
						return;
					}
				}
			}
			else
			{
				if (args.Command == ApplicationCommands.Print)
				{
					documentViewerBase.OnPrintCommand();
					return;
				}
				if (args.Command == ApplicationCommands.CancelPrint)
				{
					documentViewerBase.OnCancelPrintCommand();
					return;
				}
				Invariant.Assert(false, "Command not handled in ExecutedRoutedEventHandler.");
			}
		}

		// Token: 0x06005DE1 RID: 24033 RVA: 0x001A6EB4 File Offset: 0x001A50B4
		private static void HandleRequestBringIntoView(object sender, RequestBringIntoViewEventArgs args)
		{
			if (sender != null && sender is DocumentViewerBase)
			{
				((DocumentViewerBase)sender).HandleRequestBringIntoView(args);
			}
		}

		// Token: 0x06005DE2 RID: 24034 RVA: 0x001A6ECD File Offset: 0x001A50CD
		private static void DocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is DocumentViewerBase);
			((DocumentViewerBase)d).DocumentChanged((IDocumentPaginatorSource)e.OldValue, (IDocumentPaginatorSource)e.NewValue);
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x170016A9 RID: 5801
		// (get) Token: 0x06005DE3 RID: 24035 RVA: 0x001A6F0C File Offset: 0x001A510C
		private ITextContainer TextContainer
		{
			get
			{
				ITextContainer result = null;
				if (this._document != null && this._document is IServiceProvider && this.CheckFlags(DocumentViewerBase.Flags.IsSelectionEnabled))
				{
					result = (((IServiceProvider)this._document).GetService(typeof(ITextContainer)) as ITextContainer);
				}
				return result;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value"> An object to add as a child.</param>
		// Token: 0x06005DE4 RID: 24036 RVA: 0x001A6F5C File Offset: 0x001A515C
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.Document != null)
			{
				throw new InvalidOperationException(SR.Get("DocumentViewerCanHaveOnlyOneChild"));
			}
			IDocumentPaginatorSource documentPaginatorSource = value as IDocumentPaginatorSource;
			if (documentPaginatorSource == null)
			{
				throw new ArgumentException(SR.Get("DocumentViewerChildMustImplementIDocumentPaginatorSource"), "value");
			}
			this.Document = documentPaginatorSource;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="text"> A string to add to the object.</param>
		// Token: 0x06005DE5 RID: 24037 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="serviceType"> An object that specifies the type of service object to get.</param>
		/// <returns> A service object of type <paramref name="serviceType" />.</returns>
		// Token: 0x06005DE6 RID: 24038 RVA: 0x001A6FB8 File Offset: 0x001A51B8
		object IServiceProvider.GetService(Type serviceType)
		{
			object result = null;
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextView))
			{
				result = this._textView;
			}
			else if (serviceType == typeof(TextContainer) || serviceType == typeof(ITextContainer))
			{
				result = this.TextContainer;
			}
			return result;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.Document" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.Document" /> dependency property.</returns>
		// Token: 0x04003029 RID: 12329
		public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(IDocumentPaginatorSource), typeof(DocumentViewerBase), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DocumentViewerBase.DocumentChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.PageCount" /> dependency property key.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.PageCount" /> dependency property key.</returns>
		// Token: 0x0400302A RID: 12330
		protected static readonly DependencyPropertyKey PageCountPropertyKey = DependencyProperty.RegisterReadOnly("PageCount", typeof(int), typeof(DocumentViewerBase), new FrameworkPropertyMetadata(0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.PageCount" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.PageCount" /> dependency property.</returns>
		// Token: 0x0400302B RID: 12331
		public static readonly DependencyProperty PageCountProperty = DocumentViewerBase.PageCountPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.MasterPageNumber" /> dependency property key.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.MasterPageNumber" /> dependency property key.</returns>
		// Token: 0x0400302C RID: 12332
		protected static readonly DependencyPropertyKey MasterPageNumberPropertyKey = DependencyProperty.RegisterReadOnly("MasterPageNumber", typeof(int), typeof(DocumentViewerBase), new FrameworkPropertyMetadata(0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.MasterPageNumber" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.MasterPageNumber" /> dependency property.</returns>
		// Token: 0x0400302D RID: 12333
		public static readonly DependencyProperty MasterPageNumberProperty = DocumentViewerBase.MasterPageNumberPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.CanGoToPreviousPage" /> dependency property key.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.CanGoToPreviousPage" /> dependency property key.</returns>
		// Token: 0x0400302E RID: 12334
		protected static readonly DependencyPropertyKey CanGoToPreviousPagePropertyKey = DependencyProperty.RegisterReadOnly("CanGoToPreviousPage", typeof(bool), typeof(DocumentViewerBase), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.CanGoToPreviousPage" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.CanGoToPreviousPage" /> dependency property.</returns>
		// Token: 0x0400302F RID: 12335
		public static readonly DependencyProperty CanGoToPreviousPageProperty = DocumentViewerBase.CanGoToPreviousPagePropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.CanGoToNextPage" /> dependency property key.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.CanGoToNextPage" /> dependency property key.</returns>
		// Token: 0x04003030 RID: 12336
		protected static readonly DependencyPropertyKey CanGoToNextPagePropertyKey = DependencyProperty.RegisterReadOnly("CanGoToNextPage", typeof(bool), typeof(DocumentViewerBase), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.CanGoToNextPage" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.CanGoToNextPage" /> dependency property.</returns>
		// Token: 0x04003031 RID: 12337
		public static readonly DependencyProperty CanGoToNextPageProperty = DocumentViewerBase.CanGoToNextPagePropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.IsMasterPage" /> attached property</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.IsMasterPage" /> attached property.</returns>
		// Token: 0x04003032 RID: 12338
		public static readonly DependencyProperty IsMasterPageProperty = DependencyProperty.RegisterAttached("IsMasterPage", typeof(bool), typeof(DocumentViewerBase), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003034 RID: 12340
		private ReadOnlyCollection<DocumentPageView> _pageViews;

		// Token: 0x04003035 RID: 12341
		private FrameworkElement _textEditorRenderScope;

		// Token: 0x04003036 RID: 12342
		private MultiPageTextView _textView;

		// Token: 0x04003037 RID: 12343
		private TextEditor _textEditor;

		// Token: 0x04003038 RID: 12344
		private IDocumentPaginatorSource _document;

		// Token: 0x04003039 RID: 12345
		private DocumentViewerBase.Flags _flags;

		// Token: 0x0400303A RID: 12346
		private XpsDocumentWriter _documentWriter;

		// Token: 0x0400303B RID: 12347
		private static bool IsEditingEnabled = false;

		// Token: 0x020009E6 RID: 2534
		[Flags]
		private enum Flags
		{
			// Token: 0x04004660 RID: 18016
			IsSelectionEnabled = 32,
			// Token: 0x04004661 RID: 18017
			DocumentAsLogicalChild = 64
		}

		// Token: 0x020009E7 RID: 2535
		private class BringIntoViewState
		{
			// Token: 0x06008974 RID: 35188 RVA: 0x00255123 File Offset: 0x00253323
			internal BringIntoViewState(DocumentViewerBase source, ContentPosition contentPosition, DependencyObject targetObject, Rect targetRect)
			{
				this.Source = source;
				this.ContentPosition = contentPosition;
				this.TargetObject = targetObject;
				this.TargetRect = targetRect;
			}

			// Token: 0x04004662 RID: 18018
			internal DocumentViewerBase Source;

			// Token: 0x04004663 RID: 18019
			internal ContentPosition ContentPosition;

			// Token: 0x04004664 RID: 18020
			internal DependencyObject TargetObject;

			// Token: 0x04004665 RID: 18021
			internal Rect TargetRect;
		}
	}
}
