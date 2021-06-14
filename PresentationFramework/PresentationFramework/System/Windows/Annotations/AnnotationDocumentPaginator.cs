using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Annotations.Storage;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Annotations;
using MS.Internal.Annotations.Anchoring;
using MS.Internal.Annotations.Component;
using MS.Internal.Documents;

namespace System.Windows.Annotations
{
	/// <summary>Provides a <see cref="T:System.Windows.Documents.DocumentPaginator" /> for printing a document together with its associated annotations.</summary>
	// Token: 0x020005C9 RID: 1481
	public sealed class AnnotationDocumentPaginator : DocumentPaginator
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Annotations.AnnotationDocumentPaginator" /> class based on a specified <see cref="T:System.Windows.Documents.DocumentPaginator" /> and annotation storage <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="originalPaginator">The document to add the printed annotations to.</param>
		/// <param name="annotationStore">The annotation storage stream to retrieve the annotations from.</param>
		// Token: 0x0600628E RID: 25230 RVA: 0x001BA8CF File Offset: 0x001B8ACF
		public AnnotationDocumentPaginator(DocumentPaginator originalPaginator, Stream annotationStore) : this(originalPaginator, new XmlStreamStore(annotationStore), FlowDirection.LeftToRight)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Annotations.AnnotationDocumentPaginator" /> class based on a specified <see cref="T:System.Windows.Documents.DocumentPaginator" />, annotation storage <see cref="T:System.IO.Stream" />, and text <see cref="T:System.Windows.FlowDirection" />.</summary>
		/// <param name="originalPaginator">The document to add the printed annotations to.</param>
		/// <param name="annotationStore">The annotation storage stream to retrieve the annotations from.</param>
		/// <param name="flowDirection">The text flow direction, <see cref="F:System.Windows.FlowDirection.LeftToRight" /> or <see cref="F:System.Windows.FlowDirection.RightToLeft" />.</param>
		// Token: 0x0600628F RID: 25231 RVA: 0x001BA8DF File Offset: 0x001B8ADF
		public AnnotationDocumentPaginator(DocumentPaginator originalPaginator, Stream annotationStore, FlowDirection flowDirection) : this(originalPaginator, new XmlStreamStore(annotationStore), flowDirection)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Annotations.AnnotationDocumentPaginator" /> class based on a specified <see cref="T:System.Windows.Documents.DocumentPaginator" /> and <see cref="T:System.Windows.Annotations.Storage.AnnotationStore" />.</summary>
		/// <param name="originalPaginator">The document to add the printed annotations to.</param>
		/// <param name="annotationStore">The store to retrieve the annotations from.</param>
		// Token: 0x06006290 RID: 25232 RVA: 0x001BA8EF File Offset: 0x001B8AEF
		public AnnotationDocumentPaginator(DocumentPaginator originalPaginator, AnnotationStore annotationStore) : this(originalPaginator, annotationStore, FlowDirection.LeftToRight)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Annotations.AnnotationDocumentPaginator" /> class based on a specified <see cref="T:System.Windows.Documents.DocumentPaginator" />, <see cref="T:System.Windows.Annotations.Storage.AnnotationStore" />, and text <see cref="T:System.Windows.FlowDirection" />.</summary>
		/// <param name="originalPaginator">The document to add the printed annotations to.</param>
		/// <param name="annotationStore">The store to retrieve the annotations from.</param>
		/// <param name="flowDirection">The text flow direction, <see cref="F:System.Windows.FlowDirection.LeftToRight" /> or <see cref="F:System.Windows.FlowDirection.RightToLeft" />.</param>
		// Token: 0x06006291 RID: 25233 RVA: 0x001BA8FC File Offset: 0x001B8AFC
		public AnnotationDocumentPaginator(DocumentPaginator originalPaginator, AnnotationStore annotationStore, FlowDirection flowDirection)
		{
			this._isFixedContent = (originalPaginator is FixedDocumentPaginator || originalPaginator is FixedDocumentSequencePaginator);
			if (!this._isFixedContent && !(originalPaginator is FlowDocumentPaginator))
			{
				throw new ArgumentException(SR.Get("OnlyFlowAndFixedSupported"));
			}
			this._originalPaginator = originalPaginator;
			this._annotationStore = annotationStore;
			this._locatorManager = new LocatorManager(this._annotationStore);
			this._flowDirection = flowDirection;
			this._originalPaginator.GetPageCompleted += this.HandleGetPageCompleted;
			this._originalPaginator.ComputePageCountCompleted += this.HandleComputePageCountCompleted;
			this._originalPaginator.PagesChanged += this.HandlePagesChanged;
		}

		/// <summary>Gets a value that indicates whether <see cref="P:System.Windows.Annotations.AnnotationDocumentPaginator.PageCount" /> is the total number of pages.</summary>
		/// <returns>
		///     <see langword="true" /> if pagination is complete and <see cref="P:System.Windows.Annotations.AnnotationDocumentPaginator.PageCount" /> represents the total number of pages; otherwise, <see langword="false" /> if pagination is in process and <see cref="P:System.Windows.Annotations.AnnotationDocumentPaginator.PageCount" /> represents the number of pages currently formatted. </returns>
		// Token: 0x170017B5 RID: 6069
		// (get) Token: 0x06006292 RID: 25234 RVA: 0x001BA9B4 File Offset: 0x001B8BB4
		public override bool IsPageCountValid
		{
			get
			{
				return this._originalPaginator.IsPageCountValid;
			}
		}

		/// <summary>Gets a value that indicates the number of pages currently formatted.</summary>
		/// <returns>If <see cref="P:System.Windows.Annotations.AnnotationDocumentPaginator.IsPageCountValid" /> is <see langword="true" />, the total number of annotation pages; otherwise if <see cref="P:System.Windows.Annotations.AnnotationDocumentPaginator.IsPageCountValid" /> is <see langword="false" />, the number of pages currently formatted (pagination in process).</returns>
		// Token: 0x170017B6 RID: 6070
		// (get) Token: 0x06006293 RID: 25235 RVA: 0x001BA9C1 File Offset: 0x001B8BC1
		public override int PageCount
		{
			get
			{
				return this._originalPaginator.PageCount;
			}
		}

		/// <summary>Gets or sets the suggested width and height of each page.</summary>
		/// <returns>The suggested width and height for formatting pages.</returns>
		// Token: 0x170017B7 RID: 6071
		// (get) Token: 0x06006294 RID: 25236 RVA: 0x001BA9CE File Offset: 0x001B8BCE
		// (set) Token: 0x06006295 RID: 25237 RVA: 0x001BA9DB File Offset: 0x001B8BDB
		public override Size PageSize
		{
			get
			{
				return this._originalPaginator.PageSize;
			}
			set
			{
				this._originalPaginator.PageSize = value;
			}
		}

		/// <summary>Gets the source document that is being paginated.</summary>
		/// <returns>The source document that is being paginated.</returns>
		// Token: 0x170017B8 RID: 6072
		// (get) Token: 0x06006296 RID: 25238 RVA: 0x001BA9E9 File Offset: 0x001B8BE9
		public override IDocumentPaginatorSource Source
		{
			get
			{
				return this._originalPaginator.Source;
			}
		}

		/// <summary>Returns a <see cref="T:System.Windows.Documents.DocumentPage" /> together with associated user-annotations for a specified page number.</summary>
		/// <param name="pageNumber">The zero-based page number of the <see cref="T:System.Windows.Documents.DocumentPage" /> to return.</param>
		/// <returns>The <see cref="T:System.Windows.Documents.DocumentPage" /> for the specified <paramref name="pageNumber" />; or <see cref="F:System.Windows.Documents.DocumentPage.Missing" />, if the specified <paramref name="pageNumber" /> does not exist.</returns>
		// Token: 0x06006297 RID: 25239 RVA: 0x001BA9F8 File Offset: 0x001B8BF8
		public override DocumentPage GetPage(int pageNumber)
		{
			DocumentPage documentPage = this._originalPaginator.GetPage(pageNumber);
			if (documentPage != DocumentPage.Missing)
			{
				documentPage = this.ComposePageWithAnnotationVisuals(pageNumber, documentPage);
			}
			return documentPage;
		}

		/// <summary>Asynchronously returns a <see cref="T:System.Windows.Documents.DocumentPage" /> together with associated user-annotations for a specified page number.</summary>
		/// <param name="pageNumber">The zero-based page number of the <see cref="T:System.Windows.Documents.DocumentPage" /> to retrieve.</param>
		/// <param name="userState">An application-defined object that is used to identify the asynchronous operation.</param>
		// Token: 0x06006298 RID: 25240 RVA: 0x001BAA24 File Offset: 0x001B8C24
		public override void GetPageAsync(int pageNumber, object userState)
		{
			this._originalPaginator.GetPageAsync(pageNumber, userState);
		}

		/// <summary>Forces a pagination of the content, updates <see cref="P:System.Windows.Annotations.AnnotationDocumentPaginator.PageCount" /> with the new total, and sets <see cref="P:System.Windows.Annotations.AnnotationDocumentPaginator.IsPageCountValid" /> to <see langword="true" />.</summary>
		// Token: 0x06006299 RID: 25241 RVA: 0x001BAA33 File Offset: 0x001B8C33
		public override void ComputePageCount()
		{
			this._originalPaginator.ComputePageCount();
		}

		/// <summary>Starts an asynchronous pagination of the content, updates <see cref="P:System.Windows.Annotations.AnnotationDocumentPaginator.PageCount" /> with the new total, and sets <see cref="P:System.Windows.Annotations.AnnotationDocumentPaginator.IsPageCountValid" /> to <see langword="true" /> when it is finished.</summary>
		/// <param name="userState">An application-defined object for identifying the asynchronous operation.</param>
		// Token: 0x0600629A RID: 25242 RVA: 0x001BAA40 File Offset: 0x001B8C40
		public override void ComputePageCountAsync(object userState)
		{
			this._originalPaginator.ComputePageCountAsync(userState);
		}

		/// <summary>Cancels all asynchronous operations initiated with a given <paramref name="userState" /> object.</summary>
		/// <param name="userState">The unique application-defined identifier passed in the call to start the asynchronous operation.</param>
		// Token: 0x0600629B RID: 25243 RVA: 0x001BAA4E File Offset: 0x001B8C4E
		public override void CancelAsync(object userState)
		{
			this._originalPaginator.CancelAsync(userState);
		}

		// Token: 0x0600629C RID: 25244 RVA: 0x001BAA5C File Offset: 0x001B8C5C
		private void HandleGetPageCompleted(object sender, GetPageCompletedEventArgs e)
		{
			if (!e.Cancelled && e.Error == null && e.DocumentPage != DocumentPage.Missing)
			{
				DocumentPage page = this.ComposePageWithAnnotationVisuals(e.PageNumber, e.DocumentPage);
				e = new GetPageCompletedEventArgs(page, e.PageNumber, e.Error, e.Cancelled, e.UserState);
			}
			this.OnGetPageCompleted(e);
		}

		// Token: 0x0600629D RID: 25245 RVA: 0x001BAAC0 File Offset: 0x001B8CC0
		private void HandleComputePageCountCompleted(object sender, AsyncCompletedEventArgs e)
		{
			this.OnComputePageCountCompleted(e);
		}

		// Token: 0x0600629E RID: 25246 RVA: 0x001BAAC9 File Offset: 0x001B8CC9
		private void HandlePagesChanged(object sender, PagesChangedEventArgs e)
		{
			this.OnPagesChanged(e);
		}

		// Token: 0x0600629F RID: 25247 RVA: 0x001BAAD4 File Offset: 0x001B8CD4
		private DocumentPage ComposePageWithAnnotationVisuals(int pageNumber, DocumentPage page)
		{
			Size size = page.Size;
			AdornerDecorator adornerDecorator = new AdornerDecorator();
			adornerDecorator.FlowDirection = this._flowDirection;
			DocumentPageView documentPageView = new DocumentPageView();
			documentPageView.UseAsynchronousGetPage = false;
			documentPageView.DocumentPaginator = this._originalPaginator;
			documentPageView.PageNumber = pageNumber;
			adornerDecorator.Child = documentPageView;
			adornerDecorator.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			adornerDecorator.Arrange(new Rect(adornerDecorator.DesiredSize));
			adornerDecorator.UpdateLayout();
			AnnotationComponentManager annotationComponentManager = new AnnotationComponentManager(null);
			if (this._isFixedContent)
			{
				AnnotationService.SetSubTreeProcessorId(adornerDecorator, FixedPageProcessor.Id);
				this._locatorManager.RegisterSelectionProcessor(new FixedTextSelectionProcessor(), typeof(TextRange));
			}
			else
			{
				AnnotationService.SetDataId(adornerDecorator, "FlowDocument");
				this._locatorManager.RegisterSelectionProcessor(new TextViewSelectionProcessor(), typeof(DocumentPageView));
				TextSelectionProcessor textSelectionProcessor = new TextSelectionProcessor();
				textSelectionProcessor.SetTargetDocumentPageView(documentPageView);
				this._locatorManager.RegisterSelectionProcessor(textSelectionProcessor, typeof(TextRange));
			}
			IList<IAttachedAnnotation> list = this.ProcessAnnotations(documentPageView);
			foreach (IAttachedAnnotation attachedAnnotation in list)
			{
				if (attachedAnnotation.AttachmentLevel != AttachmentLevel.Unresolved && attachedAnnotation.AttachmentLevel != AttachmentLevel.Incomplete)
				{
					annotationComponentManager.AddAttachedAnnotation(attachedAnnotation, false);
				}
			}
			adornerDecorator.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			adornerDecorator.Arrange(new Rect(adornerDecorator.DesiredSize));
			adornerDecorator.UpdateLayout();
			return new AnnotationDocumentPaginator.AnnotatedDocumentPage(page, adornerDecorator, size, new Rect(size), new Rect(size));
		}

		// Token: 0x060062A0 RID: 25248 RVA: 0x001BAC84 File Offset: 0x001B8E84
		private IList<IAttachedAnnotation> ProcessAnnotations(DocumentPageView dpv)
		{
			if (dpv == null)
			{
				throw new ArgumentNullException("dpv");
			}
			IList<IAttachedAnnotation> list = new List<IAttachedAnnotation>();
			IList<ContentLocatorBase> list2 = this._locatorManager.GenerateLocators(dpv);
			if (list2.Count > 0)
			{
				ContentLocator[] array = new ContentLocator[list2.Count];
				list2.CopyTo(array, 0);
				IList<Annotation> annotations = this._annotationStore.GetAnnotations(array[0]);
				foreach (ContentLocatorBase contentLocatorBase in list2)
				{
					ContentLocator contentLocator = (ContentLocator)contentLocatorBase;
					if (contentLocator.Parts[contentLocator.Parts.Count - 1].NameValuePairs.ContainsKey("IncludeOverlaps"))
					{
						contentLocator.Parts.RemoveAt(contentLocator.Parts.Count - 1);
					}
				}
				foreach (Annotation annotation in annotations)
				{
					foreach (AnnotationResource annotationResource in annotation.Anchors)
					{
						foreach (ContentLocatorBase locator in annotationResource.ContentLocators)
						{
							AttachmentLevel attachmentLevel;
							object attachedAnchor = this._locatorManager.FindAttachedAnchor(dpv, array, locator, out attachmentLevel);
							if (attachmentLevel != AttachmentLevel.Unresolved)
							{
								Invariant.Assert(VisualTreeHelper.GetChildrenCount(dpv) == 1, "DocumentPageView has no visual children.");
								DependencyObject child = VisualTreeHelper.GetChild(dpv, 0);
								list.Add(new AttachedAnnotation(this._locatorManager, annotation, annotationResource, attachedAnchor, attachmentLevel, child as DocumentPageHost));
								break;
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x040031A1 RID: 12705
		private AnnotationStore _annotationStore;

		// Token: 0x040031A2 RID: 12706
		private DocumentPaginator _originalPaginator;

		// Token: 0x040031A3 RID: 12707
		private LocatorManager _locatorManager;

		// Token: 0x040031A4 RID: 12708
		private bool _isFixedContent;

		// Token: 0x040031A5 RID: 12709
		private FlowDirection _flowDirection;

		// Token: 0x020009FF RID: 2559
		private class AnnotatedDocumentPage : DocumentPage, IContentHost
		{
			// Token: 0x060089E5 RID: 35301 RVA: 0x00256986 File Offset: 0x00254B86
			public AnnotatedDocumentPage(DocumentPage basePage, Visual visual, Size pageSize, Rect bleedBox, Rect contentBox) : base(visual, pageSize, bleedBox, contentBox)
			{
				this._basePage = (basePage as IContentHost);
			}

			// Token: 0x17001F23 RID: 7971
			// (get) Token: 0x060089E6 RID: 35302 RVA: 0x002569A0 File Offset: 0x00254BA0
			public IEnumerator<IInputElement> HostedElements
			{
				get
				{
					if (this._basePage != null)
					{
						return this._basePage.HostedElements;
					}
					return new HostedElements(new ReadOnlyCollection<TextSegment>(new List<TextSegment>(0)));
				}
			}

			// Token: 0x060089E7 RID: 35303 RVA: 0x002569C6 File Offset: 0x00254BC6
			public ReadOnlyCollection<Rect> GetRectangles(ContentElement child)
			{
				if (this._basePage != null)
				{
					return this._basePage.GetRectangles(child);
				}
				return new ReadOnlyCollection<Rect>(new List<Rect>(0));
			}

			// Token: 0x060089E8 RID: 35304 RVA: 0x002569E8 File Offset: 0x00254BE8
			public IInputElement InputHitTest(Point point)
			{
				if (this._basePage != null)
				{
					return this._basePage.InputHitTest(point);
				}
				return null;
			}

			// Token: 0x060089E9 RID: 35305 RVA: 0x00256A00 File Offset: 0x00254C00
			public void OnChildDesiredSizeChanged(UIElement child)
			{
				if (this._basePage != null)
				{
					this._basePage.OnChildDesiredSizeChanged(child);
				}
			}

			// Token: 0x040046AB RID: 18091
			private IContentHost _basePage;
		}
	}
}
