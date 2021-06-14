using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Threading;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	/// <summary>Hosts one or more <see cref="T:System.Windows.Documents.DocumentReference" /> elements that define a sequence of fixed documents. </summary>
	// Token: 0x02000339 RID: 825
	[ContentProperty("References")]
	public class FixedDocumentSequence : FrameworkContentElement, IDocumentPaginatorSource, IAddChildInternal, IAddChild, IServiceProvider, IFixedNavigate, IUriContext
	{
		// Token: 0x06002B6E RID: 11118 RVA: 0x000C6460 File Offset: 0x000C4660
		static FixedDocumentSequence()
		{
			ContentElement.FocusableProperty.OverrideMetadata(typeof(FixedDocumentSequence), new FrameworkPropertyMetadata(true));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.FixedDocumentSequence" /> class.</summary>
		// Token: 0x06002B6F RID: 11119 RVA: 0x000C64B5 File Offset: 0x000C46B5
		public FixedDocumentSequence()
		{
			this._Init();
		}

		/// <summary>Gets the service object of the specified type.</summary>
		/// <param name="serviceType">An object that specifies the type of service object to get. </param>
		/// <returns>A service object of type <paramref name="serviceType" />.-or- 
		///     <see langword="null" /> if there is no service object of type <paramref name="serviceType" />.</returns>
		// Token: 0x06002B70 RID: 11120 RVA: 0x000C64C4 File Offset: 0x000C46C4
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextContainer))
			{
				return this.TextContainer;
			}
			if (serviceType == typeof(RubberbandSelector))
			{
				if (this._rubberbandSelector == null)
				{
					this._rubberbandSelector = new RubberbandSelector();
				}
				return this._rubberbandSelector;
			}
			return null;
		}

		/// <summary>Adds a child object. </summary>
		/// <param name="value">The child object to add.</param>
		// Token: 0x06002B71 RID: 11121 RVA: 0x000C652C File Offset: 0x000C472C
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			DocumentReference documentReference = value as DocumentReference;
			if (documentReference == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(DocumentReference)
				}), "value");
			}
			if (documentReference.IsInitialized)
			{
				this._references.Add(documentReference);
				return;
			}
			if (this._partialRef == null)
			{
				this._partialRef = documentReference;
				this._partialRef.Initialized += this._OnDocumentReferenceInitialized;
				return;
			}
			throw new InvalidOperationException(SR.Get("PrevoiusUninitializedDocumentReferenceOutstanding"));
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06002B72 RID: 11122 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x000C65CD File Offset: 0x000C47CD
		void IFixedNavigate.NavigateAsync(string elementID)
		{
			if (this.IsPageCountValid)
			{
				FixedHyperLink.NavigateToElement(this, elementID);
				return;
			}
			this._navigateAfterPagination = true;
			this._navigateFragment = elementID;
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x000C65F0 File Offset: 0x000C47F0
		UIElement IFixedNavigate.FindElementByID(string elementID, out FixedPage rootFixedPage)
		{
			UIElement uielement = null;
			rootFixedPage = null;
			if (char.IsDigit(elementID[0]))
			{
				int pageNumber = Convert.ToInt32(elementID, CultureInfo.InvariantCulture) - 1;
				DynamicDocumentPaginator paginator;
				int pageNumber2;
				if (this.TranslatePageNumber(pageNumber, out paginator, out pageNumber2))
				{
					FixedDocument fixedDocument = paginator.Source as FixedDocument;
					if (fixedDocument != null)
					{
						uielement = fixedDocument.GetFixedPage(pageNumber2);
					}
				}
			}
			else
			{
				foreach (DocumentReference docRef in this.References)
				{
					DynamicDocumentPaginator paginator = this.GetPaginator(docRef);
					FixedDocument fixedDocument = paginator.Source as FixedDocument;
					if (fixedDocument != null)
					{
						uielement = ((IFixedNavigate)fixedDocument).FindElementByID(elementID, out rootFixedPage);
						if (uielement != null)
						{
							break;
						}
					}
				}
			}
			return uielement;
		}

		/// <summary>Gets an enumerator for accessing the document sequence's <see cref="T:System.Windows.Documents.DocumentReference" /> child elements. </summary>
		/// <returns>An enumerator for accessing the document sequence's <see cref="T:System.Windows.Documents.DocumentReference" /> child elements.</returns>
		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06002B75 RID: 11125 RVA: 0x000C66AC File Offset: 0x000C48AC
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				DocumentReference[] array = new DocumentReference[this._references.Count];
				this._references.CopyTo(array, 0);
				return array.GetEnumerator();
			}
		}

		/// <summary>Gets the paginator for the <see cref="T:System.Windows.Documents.FixedDocument" /> that provides page-oriented services such as getting a particular page and repaginating in response to changes. </summary>
		/// <returns>An object of a class derived from <see cref="T:System.Windows.Documents.DocumentPaginator" /> that provides pagination services</returns>
		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06002B76 RID: 11126 RVA: 0x000C66DD File Offset: 0x000C48DD
		public DocumentPaginator DocumentPaginator
		{
			get
			{
				return this._paginator;
			}
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x000C66E8 File Offset: 0x000C48E8
		internal DocumentPage GetPage(int pageNumber)
		{
			if (pageNumber < 0)
			{
				throw new ArgumentOutOfRangeException("pageNumber", SR.Get("IDPNegativePageNumber"));
			}
			DynamicDocumentPaginator dynamicDocumentPaginator;
			int pageNumber2;
			if (this.TranslatePageNumber(pageNumber, out dynamicDocumentPaginator, out pageNumber2))
			{
				DocumentPage page = dynamicDocumentPaginator.GetPage(pageNumber2);
				return new FixedDocumentSequenceDocumentPage(this, dynamicDocumentPaginator, page);
			}
			return DocumentPage.Missing;
		}

		// Token: 0x06002B78 RID: 11128 RVA: 0x000C6734 File Offset: 0x000C4934
		internal DocumentPage GetPage(FixedDocument document, int fixedDocPageNumber)
		{
			if (fixedDocPageNumber < 0)
			{
				throw new ArgumentOutOfRangeException("fixedDocPageNumber", SR.Get("IDPNegativePageNumber"));
			}
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}
			DocumentPage page = document.GetPage(fixedDocPageNumber);
			return new FixedDocumentSequenceDocumentPage(this, document.DocumentPaginator as DynamicDocumentPaginator, page);
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x000C6784 File Offset: 0x000C4984
		internal void GetPageAsync(int pageNumber, object userState)
		{
			if (pageNumber < 0)
			{
				throw new ArgumentOutOfRangeException("pageNumber", SR.Get("IDPNegativePageNumber"));
			}
			if (userState == null)
			{
				throw new ArgumentNullException("userState");
			}
			FixedDocumentSequence.GetPageAsyncRequest getPageAsyncRequest = new FixedDocumentSequence.GetPageAsyncRequest(new FixedDocumentSequence.RequestedPage(pageNumber), userState);
			this._asyncOps[userState] = getPageAsyncRequest;
			DispatcherOperationCallback method = new DispatcherOperationCallback(this._GetPageAsyncDelegate);
			base.Dispatcher.BeginInvoke(DispatcherPriority.Background, method, getPageAsyncRequest);
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x000C67F0 File Offset: 0x000C49F0
		internal int GetPageNumber(ContentPosition contentPosition)
		{
			if (contentPosition == null)
			{
				throw new ArgumentNullException("contentPosition");
			}
			DynamicDocumentPaginator dynamicDocumentPaginator = null;
			ContentPosition contentPosition2 = null;
			if (contentPosition is DocumentSequenceTextPointer)
			{
				DocumentSequenceTextPointer documentSequenceTextPointer = (DocumentSequenceTextPointer)contentPosition;
				dynamicDocumentPaginator = this.GetPaginator(documentSequenceTextPointer.ChildBlock.DocRef);
				contentPosition2 = (documentSequenceTextPointer.ChildPointer as ContentPosition);
			}
			if (contentPosition2 == null)
			{
				throw new ArgumentException(SR.Get("IDPInvalidContentPosition"));
			}
			int pageNumber = dynamicDocumentPaginator.GetPageNumber(contentPosition2);
			int result;
			this._SynthesizeGlobalPageNumber(dynamicDocumentPaginator, pageNumber, out result);
			return result;
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x000C6868 File Offset: 0x000C4A68
		internal void CancelAsync(object userState)
		{
			if (userState == null)
			{
				throw new ArgumentNullException("userState");
			}
			if (this._asyncOps.ContainsKey(userState))
			{
				FixedDocumentSequence.GetPageAsyncRequest getPageAsyncRequest = this._asyncOps[userState];
				if (getPageAsyncRequest != null)
				{
					getPageAsyncRequest.Cancelled = true;
					if (getPageAsyncRequest.Page.ChildPaginator != null)
					{
						getPageAsyncRequest.Page.ChildPaginator.CancelAsync(getPageAsyncRequest);
					}
				}
			}
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x000C68C8 File Offset: 0x000C4AC8
		internal ContentPosition GetObjectPosition(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			foreach (DocumentReference docRef in this.References)
			{
				DynamicDocumentPaginator paginator = this.GetPaginator(docRef);
				if (paginator != null)
				{
					ContentPosition objectPosition = paginator.GetObjectPosition(o);
					if (objectPosition != ContentPosition.Missing && objectPosition is ITextPointer)
					{
						ChildDocumentBlock childBlock = new ChildDocumentBlock(this.TextContainer, docRef);
						return new DocumentSequenceTextPointer(childBlock, (ITextPointer)objectPosition);
					}
				}
			}
			return ContentPosition.Missing;
		}

		// Token: 0x06002B7D RID: 11133 RVA: 0x000C6968 File Offset: 0x000C4B68
		internal ContentPosition GetPagePosition(DocumentPage page)
		{
			FixedDocumentSequenceDocumentPage fixedDocumentSequenceDocumentPage = page as FixedDocumentSequenceDocumentPage;
			if (fixedDocumentSequenceDocumentPage == null)
			{
				return ContentPosition.Missing;
			}
			return fixedDocumentSequenceDocumentPage.ContentPosition;
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06002B7E RID: 11134 RVA: 0x000C698C File Offset: 0x000C4B8C
		internal bool IsPageCountValid
		{
			get
			{
				bool result = true;
				if (base.IsInitialized)
				{
					using (IEnumerator<DocumentReference> enumerator = this.References.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							DocumentReference docRef = enumerator.Current;
							DynamicDocumentPaginator paginator = this.GetPaginator(docRef);
							if (paginator == null || !paginator.IsPageCountValid)
							{
								result = false;
								break;
							}
						}
						return result;
					}
				}
				result = false;
				return result;
			}
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06002B7F RID: 11135 RVA: 0x000C69F8 File Offset: 0x000C4BF8
		internal int PageCount
		{
			get
			{
				int num = 0;
				foreach (DocumentReference docRef in this.References)
				{
					DynamicDocumentPaginator paginator = this.GetPaginator(docRef);
					if (paginator != null)
					{
						num += paginator.PageCount;
						if (!paginator.IsPageCountValid)
						{
							break;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06002B80 RID: 11136 RVA: 0x000C6A60 File Offset: 0x000C4C60
		// (set) Token: 0x06002B81 RID: 11137 RVA: 0x000C6A68 File Offset: 0x000C4C68
		internal Size PageSize
		{
			get
			{
				return this._pageSize;
			}
			set
			{
				this._pageSize = value;
			}
		}

		/// <summary>Gets or sets the base URI of the current application context. </summary>
		/// <returns>The base URI of the application context.</returns>
		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06002B82 RID: 11138 RVA: 0x000C216F File Offset: 0x000C036F
		// (set) Token: 0x06002B83 RID: 11139 RVA: 0x000C2181 File Offset: 0x000C0381
		Uri IUriContext.BaseUri
		{
			get
			{
				return (Uri)base.GetValue(BaseUriHelper.BaseUriProperty);
			}
			set
			{
				base.SetValue(BaseUriHelper.BaseUriProperty, value);
			}
		}

		/// <summary>Gets a collection of the document sequence's <see cref="T:System.Windows.Documents.DocumentReference" /> child elements. </summary>
		/// <returns>A collection of the document sequence's <see cref="T:System.Windows.Documents.DocumentReference" /> child elements.</returns>
		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06002B84 RID: 11140 RVA: 0x000C6A71 File Offset: 0x000C4C71
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[CLSCompliant(false)]
		public DocumentReferenceCollection References
		{
			get
			{
				return this._references;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Printing.PrintTicket" /> that is associated with this document sequence. </summary>
		/// <returns>The <see cref="T:System.Printing.PrintTicket" /> for this sequence.</returns>
		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06002B85 RID: 11141 RVA: 0x000C6A7C File Offset: 0x000C4C7C
		// (set) Token: 0x06002B86 RID: 11142 RVA: 0x000C6A96 File Offset: 0x000C4C96
		public object PrintTicket
		{
			get
			{
				return base.GetValue(FixedDocumentSequence.PrintTicketProperty);
			}
			set
			{
				base.SetValue(FixedDocumentSequence.PrintTicketProperty, value);
			}
		}

		/// <summary>Creates an automation peer for the sequence.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.DocumentAutomationPeer" /> that exposes the <see cref="T:System.Windows.Documents.FixedDocumentSequence" /> to Microsoft UI Automation.</returns>
		// Token: 0x06002B87 RID: 11143 RVA: 0x000C6AA4 File Offset: 0x000C4CA4
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DocumentAutomationPeer(this);
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x000C6AAC File Offset: 0x000C4CAC
		internal DynamicDocumentPaginator GetPaginator(DocumentReference docRef)
		{
			DynamicDocumentPaginator dynamicDocumentPaginator = null;
			IDocumentPaginatorSource documentPaginatorSource = docRef.CurrentlyLoadedDoc;
			if (documentPaginatorSource != null)
			{
				dynamicDocumentPaginator = (documentPaginatorSource.DocumentPaginator as DynamicDocumentPaginator);
			}
			else
			{
				documentPaginatorSource = docRef.GetDocument(false);
				if (documentPaginatorSource != null)
				{
					dynamicDocumentPaginator = (documentPaginatorSource.DocumentPaginator as DynamicDocumentPaginator);
					dynamicDocumentPaginator.PaginationCompleted += this._OnChildPaginationCompleted;
					dynamicDocumentPaginator.PaginationProgress += this._OnChildPaginationProgress;
					dynamicDocumentPaginator.PagesChanged += this._OnChildPagesChanged;
				}
			}
			return dynamicDocumentPaginator;
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x000C6B24 File Offset: 0x000C4D24
		internal bool TranslatePageNumber(int pageNumber, out DynamicDocumentPaginator childPaginator, out int childPageNumber)
		{
			childPaginator = null;
			childPageNumber = 0;
			foreach (DocumentReference docRef in this.References)
			{
				childPaginator = this.GetPaginator(docRef);
				if (childPaginator != null)
				{
					childPageNumber = pageNumber;
					if (childPaginator.PageCount > childPageNumber)
					{
						return true;
					}
					if (!childPaginator.IsPageCountValid)
					{
						break;
					}
					pageNumber -= childPaginator.PageCount;
				}
			}
			return false;
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06002B8A RID: 11146 RVA: 0x000C6BA8 File Offset: 0x000C4DA8
		internal DocumentSequenceTextContainer TextContainer
		{
			get
			{
				if (this._textContainer == null)
				{
					this._textContainer = new DocumentSequenceTextContainer(this);
				}
				return this._textContainer;
			}
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x000C6BC4 File Offset: 0x000C4DC4
		private void _Init()
		{
			this._paginator = new FixedDocumentSequencePaginator(this);
			this._references = new DocumentReferenceCollection();
			this._references.CollectionChanged += this._OnCollectionChanged;
			this._asyncOps = new Dictionary<object, FixedDocumentSequence.GetPageAsyncRequest>();
			this._pendingPages = new List<FixedDocumentSequence.RequestedPage>();
			this._pageSize = new Size(816.0, 1056.0);
			base.Initialized += this.OnInitialized;
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x000C6C44 File Offset: 0x000C4E44
		private void OnInitialized(object sender, EventArgs e)
		{
			bool flag = true;
			foreach (DocumentReference docRef in this.References)
			{
				DynamicDocumentPaginator paginator = this.GetPaginator(docRef);
				if (paginator == null || !paginator.IsPageCountValid)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this._paginator.NotifyPaginationCompleted(EventArgs.Empty);
			}
			if (this.PageCount > 0)
			{
				DocumentPage page = this.GetPage(0);
				if (page != null)
				{
					FixedPage fixedPage = page.Visual as FixedPage;
					if (fixedPage != null)
					{
						base.Language = fixedPage.Language;
					}
				}
			}
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x000C6CEC File Offset: 0x000C4EEC
		private void _OnDocumentReferenceInitialized(object sender, EventArgs e)
		{
			DocumentReference documentReference = (DocumentReference)sender;
			if (documentReference == this._partialRef)
			{
				this._partialRef.Initialized -= this._OnDocumentReferenceInitialized;
				this._partialRef = null;
				this._references.Add(documentReference);
			}
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x000C6D34 File Offset: 0x000C4F34
		private void _OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (args.Action != NotifyCollectionChangedAction.Add)
			{
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					args.Action
				}));
			}
			if (args.NewItems.Count != 1)
			{
				throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
			}
			object obj = args.NewItems[0];
			base.AddLogicalChild(obj);
			int pageCount = this.PageCount;
			DynamicDocumentPaginator paginator = this.GetPaginator((DocumentReference)obj);
			if (paginator == null)
			{
				throw new ApplicationException(SR.Get("DocumentReferenceHasInvalidDocument"));
			}
			int pageCount2 = paginator.PageCount;
			int start = pageCount - pageCount2;
			if (pageCount2 > 0)
			{
				this._paginator.NotifyPaginationProgress(new PaginationProgressEventArgs(start, pageCount2));
				this._paginator.NotifyPagesChanged(new PagesChangedEventArgs(start, pageCount2));
				return;
			}
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x000C6E00 File Offset: 0x000C5000
		private bool _SynthesizeGlobalPageNumber(DynamicDocumentPaginator childPaginator, int childPageNumber, out int pageNumber)
		{
			pageNumber = 0;
			foreach (DocumentReference docRef in this.References)
			{
				DynamicDocumentPaginator paginator = this.GetPaginator(docRef);
				if (paginator != null)
				{
					if (paginator == childPaginator)
					{
						pageNumber += childPageNumber;
						return true;
					}
					pageNumber += paginator.PageCount;
				}
			}
			return false;
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x000C6E70 File Offset: 0x000C5070
		private void _OnChildPaginationCompleted(object sender, EventArgs args)
		{
			if (this.IsPageCountValid)
			{
				this._paginator.NotifyPaginationCompleted(EventArgs.Empty);
				if (this._navigateAfterPagination)
				{
					FixedHyperLink.NavigateToElement(this, this._navigateFragment);
					this._navigateAfterPagination = false;
				}
			}
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x000C6EA8 File Offset: 0x000C50A8
		private void _OnChildPaginationProgress(object sender, PaginationProgressEventArgs args)
		{
			int start;
			if (this._SynthesizeGlobalPageNumber((DynamicDocumentPaginator)sender, args.Start, out start))
			{
				this._paginator.NotifyPaginationProgress(new PaginationProgressEventArgs(start, args.Count));
			}
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x000C6EE4 File Offset: 0x000C50E4
		private void _OnChildPagesChanged(object sender, PagesChangedEventArgs args)
		{
			int start;
			if (this._SynthesizeGlobalPageNumber((DynamicDocumentPaginator)sender, args.Start, out start))
			{
				this._paginator.NotifyPagesChanged(new PagesChangedEventArgs(start, args.Count));
				return;
			}
			this._paginator.NotifyPagesChanged(new PagesChangedEventArgs(this.PageCount, int.MaxValue));
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x000C6F3C File Offset: 0x000C513C
		private object _GetPageAsyncDelegate(object arg)
		{
			FixedDocumentSequence.GetPageAsyncRequest getPageAsyncRequest = (FixedDocumentSequence.GetPageAsyncRequest)arg;
			int pageNumber = getPageAsyncRequest.Page.PageNumber;
			if (getPageAsyncRequest.Cancelled || !this.TranslatePageNumber(pageNumber, out getPageAsyncRequest.Page.ChildPaginator, out getPageAsyncRequest.Page.ChildPageNumber) || getPageAsyncRequest.Cancelled)
			{
				this._NotifyGetPageAsyncCompleted(DocumentPage.Missing, pageNumber, null, true, getPageAsyncRequest.UserState);
				this._asyncOps.Remove(getPageAsyncRequest.UserState);
				return null;
			}
			if (!this._pendingPages.Contains(getPageAsyncRequest.Page))
			{
				this._pendingPages.Add(getPageAsyncRequest.Page);
				getPageAsyncRequest.Page.ChildPaginator.GetPageCompleted += this._OnGetPageCompleted;
				getPageAsyncRequest.Page.ChildPaginator.GetPageAsync(getPageAsyncRequest.Page.ChildPageNumber, getPageAsyncRequest);
			}
			return null;
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x000C7010 File Offset: 0x000C5210
		private void _OnGetPageCompleted(object sender, GetPageCompletedEventArgs args)
		{
			FixedDocumentSequence.GetPageAsyncRequest getPageAsyncRequest = (FixedDocumentSequence.GetPageAsyncRequest)args.UserState;
			this._pendingPages.Remove(getPageAsyncRequest.Page);
			DocumentPage page = DocumentPage.Missing;
			int pageNumber = getPageAsyncRequest.Page.PageNumber;
			if (!args.Cancelled && args.Error == null && args.DocumentPage != DocumentPage.Missing)
			{
				page = new FixedDocumentSequenceDocumentPage(this, (DynamicDocumentPaginator)sender, args.DocumentPage);
				this._SynthesizeGlobalPageNumber((DynamicDocumentPaginator)sender, args.PageNumber, out pageNumber);
			}
			if (!args.Cancelled)
			{
				ArrayList arrayList = new ArrayList();
				IEnumerator<KeyValuePair<object, FixedDocumentSequence.GetPageAsyncRequest>> enumerator = this._asyncOps.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<object, FixedDocumentSequence.GetPageAsyncRequest> keyValuePair = enumerator.Current;
						FixedDocumentSequence.GetPageAsyncRequest value = keyValuePair.Value;
						if (getPageAsyncRequest.Page.Equals(value.Page))
						{
							ArrayList arrayList2 = arrayList;
							keyValuePair = enumerator.Current;
							arrayList2.Add(keyValuePair.Key);
							this._NotifyGetPageAsyncCompleted(page, pageNumber, args.Error, value.Cancelled, value.UserState);
						}
					}
				}
				finally
				{
					foreach (object key in arrayList)
					{
						this._asyncOps.Remove(key);
					}
				}
			}
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x000C7174 File Offset: 0x000C5374
		private void _NotifyGetPageAsyncCompleted(DocumentPage page, int pageNumber, Exception error, bool cancelled, object userState)
		{
			this._paginator.NotifyGetPageCompleted(new GetPageCompletedEventArgs(page, pageNumber, error, cancelled, userState));
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.FixedDocumentSequence.PrintTicket" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.FixedDocumentSequence.PrintTicket" /> dependency property.</returns>
		// Token: 0x04001CA4 RID: 7332
		public static readonly DependencyProperty PrintTicketProperty = DependencyProperty.RegisterAttached("PrintTicket", typeof(object), typeof(FixedDocumentSequence), new FrameworkPropertyMetadata(null));

		// Token: 0x04001CA5 RID: 7333
		private DocumentReferenceCollection _references;

		// Token: 0x04001CA6 RID: 7334
		private DocumentReference _partialRef;

		// Token: 0x04001CA7 RID: 7335
		private FixedDocumentSequencePaginator _paginator;

		// Token: 0x04001CA8 RID: 7336
		private IDictionary<object, FixedDocumentSequence.GetPageAsyncRequest> _asyncOps;

		// Token: 0x04001CA9 RID: 7337
		private IList<FixedDocumentSequence.RequestedPage> _pendingPages;

		// Token: 0x04001CAA RID: 7338
		private Size _pageSize;

		// Token: 0x04001CAB RID: 7339
		private bool _navigateAfterPagination;

		// Token: 0x04001CAC RID: 7340
		private string _navigateFragment;

		// Token: 0x04001CAD RID: 7341
		private DocumentSequenceTextContainer _textContainer;

		// Token: 0x04001CAE RID: 7342
		private RubberbandSelector _rubberbandSelector;

		// Token: 0x020008C9 RID: 2249
		private struct RequestedPage
		{
			// Token: 0x06008471 RID: 33905 RVA: 0x00248442 File Offset: 0x00246642
			internal RequestedPage(int pageNumber)
			{
				this.PageNumber = pageNumber;
				this.ChildPageNumber = 0;
				this.ChildPaginator = null;
			}

			// Token: 0x06008472 RID: 33906 RVA: 0x00248459 File Offset: 0x00246659
			public override int GetHashCode()
			{
				return this.PageNumber;
			}

			// Token: 0x06008473 RID: 33907 RVA: 0x00248461 File Offset: 0x00246661
			public override bool Equals(object obj)
			{
				return obj is FixedDocumentSequence.RequestedPage && this.Equals((FixedDocumentSequence.RequestedPage)obj);
			}

			// Token: 0x06008474 RID: 33908 RVA: 0x00248479 File Offset: 0x00246679
			public bool Equals(FixedDocumentSequence.RequestedPage obj)
			{
				return this.PageNumber == obj.PageNumber;
			}

			// Token: 0x06008475 RID: 33909 RVA: 0x00248489 File Offset: 0x00246689
			public static bool operator ==(FixedDocumentSequence.RequestedPage obj1, FixedDocumentSequence.RequestedPage obj2)
			{
				return obj1.Equals(obj2);
			}

			// Token: 0x06008476 RID: 33910 RVA: 0x00248493 File Offset: 0x00246693
			public static bool operator !=(FixedDocumentSequence.RequestedPage obj1, FixedDocumentSequence.RequestedPage obj2)
			{
				return !obj1.Equals(obj2);
			}

			// Token: 0x0400422D RID: 16941
			internal DynamicDocumentPaginator ChildPaginator;

			// Token: 0x0400422E RID: 16942
			internal int ChildPageNumber;

			// Token: 0x0400422F RID: 16943
			internal int PageNumber;
		}

		// Token: 0x020008CA RID: 2250
		private class GetPageAsyncRequest
		{
			// Token: 0x06008477 RID: 33911 RVA: 0x002484A0 File Offset: 0x002466A0
			internal GetPageAsyncRequest(FixedDocumentSequence.RequestedPage page, object userState)
			{
				this.Page = page;
				this.UserState = userState;
				this.Cancelled = false;
			}

			// Token: 0x04004230 RID: 16944
			internal FixedDocumentSequence.RequestedPage Page;

			// Token: 0x04004231 RID: 16945
			internal object UserState;

			// Token: 0x04004232 RID: 16946
			internal bool Cancelled;
		}
	}
}
