using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Utility;
using MS.Utility;

namespace System.Windows.Documents
{
	/// <summary>Provides information about the <see cref="T:System.Windows.Documents.FixedPage" /> elements within a <see cref="T:System.Windows.Documents.FixedDocument" />.</summary>
	// Token: 0x0200039B RID: 923
	[ContentProperty("Child")]
	public sealed class PageContent : FrameworkElement, IAddChildInternal, IAddChild, IUriContext
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.PageContent" /> class.</summary>
		// Token: 0x06003236 RID: 12854 RVA: 0x000DC4A2 File Offset: 0x000DA6A2
		public PageContent()
		{
			this._Init();
		}

		/// <summary>Loads and returns the <see cref="T:System.Windows.Documents.FixedPage" /> content element. </summary>
		/// <param name="forceReload">
		///       <see langword="true" /> to always reload the <see cref="T:System.Windows.Documents.FixedPage" /> even if it has been previously loaded and cached; <see langword="false" /> to load the <see cref="T:System.Windows.Documents.FixedPage" /> only if there is no cached version.</param>
		/// <returns>The root element of the visual tree for this page.</returns>
		// Token: 0x06003237 RID: 12855 RVA: 0x000DC4B0 File Offset: 0x000DA6B0
		public FixedPage GetPageRoot(bool forceReload)
		{
			if (this._asyncOp != null)
			{
				this._asyncOp.Wait();
			}
			FixedPage fixedPage = null;
			if (!forceReload)
			{
				fixedPage = this._GetLoadedPage();
			}
			if (fixedPage == null)
			{
				fixedPage = this._LoadPage();
			}
			return fixedPage;
		}

		/// <summary>Asynchronously loads and returns the <see cref="T:System.Windows.Documents.FixedPage" /> content element. </summary>
		/// <param name="forceReload">
		///       <see langword="true" /> to always reload the <see cref="T:System.Windows.Documents.FixedPage" /> even if it has been previously loaded and cached; <see langword="false" /> to load the <see cref="T:System.Windows.Documents.FixedPage" /> only if there is no cached version.</param>
		// Token: 0x06003238 RID: 12856 RVA: 0x000DC4E8 File Offset: 0x000DA6E8
		public void GetPageRootAsync(bool forceReload)
		{
			if (this._asyncOp != null)
			{
				return;
			}
			FixedPage fixedPage = null;
			if (!forceReload)
			{
				fixedPage = this._GetLoadedPage();
			}
			if (fixedPage != null)
			{
				this._NotifyPageCompleted(fixedPage, null, false, null);
				return;
			}
			Dispatcher dispatcher = base.Dispatcher;
			Uri uri = this._ResolveUri();
			if (uri != null || this._child != null)
			{
				this._asyncOp = new PageContentAsyncResult(new AsyncCallback(this._RequestPageCallback), null, dispatcher, uri, uri, this._child);
				this._asyncOp.DispatcherOperation = dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this._asyncOp.Dispatch), null);
			}
		}

		/// <summary>Cancels any current <see cref="M:System.Windows.Documents.PageContent.GetPageRootAsync(System.Boolean)" /> operation in progress.</summary>
		// Token: 0x06003239 RID: 12857 RVA: 0x000DC57D File Offset: 0x000DA77D
		public void GetPageRootAsyncCancel()
		{
			if (this._asyncOp != null)
			{
				this._asyncOp.Cancel();
				this._asyncOp = null;
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Windows.Markup.IAddChild.AddChild(System.Object)" />.</summary>
		/// <param name="value">The child <see cref="T:System.Object" /> to add.</param>
		// Token: 0x0600323A RID: 12858 RVA: 0x000DC59C File Offset: 0x000DA79C
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			FixedPage fixedPage = value as FixedPage;
			if (fixedPage == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(FixedPage)
				}), "value");
			}
			if (this._child != null)
			{
				throw new InvalidOperationException(SR.Get("CanOnlyHaveOneChild", new object[]
				{
					typeof(PageContent),
					value
				}));
			}
			this._pageRef = null;
			this._child = fixedPage;
			LogicalTreeHelper.AddLogicalChild(this, this._child);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Windows.Markup.IAddChild.AddText(System.String)" />.</summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x0600323B RID: 12859 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x000DC63C File Offset: 0x000DA83C
		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PageContent pageContent = (PageContent)d;
			pageContent._pageRef = null;
		}

		/// <summary>Gets or sets the uniform resource identifier (URI) to the <see cref="T:System.Windows.Documents.FixedPage" /> content data stream.  </summary>
		/// <returns>The <see cref="T:System.Uri" /> of the corresponding <see cref="T:System.Windows.Documents.FixedPage" />.</returns>
		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x0600323D RID: 12861 RVA: 0x000DC657 File Offset: 0x000DA857
		// (set) Token: 0x0600323E RID: 12862 RVA: 0x000DC669 File Offset: 0x000DA869
		public Uri Source
		{
			get
			{
				return (Uri)base.GetValue(PageContent.SourceProperty);
			}
			set
			{
				base.SetValue(PageContent.SourceProperty, value);
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.Windows.Documents.LinkTarget" /> elements that identify the hyperlink-addressable locations on the page. </summary>
		/// <returns>The <see cref="T:System.Windows.Documents.LinkTargetCollection" /> of <see cref="T:System.Windows.Documents.LinkTarget" /> elements that identify the hyperlink-addressable locations on the page.</returns>
		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x0600323F RID: 12863 RVA: 0x000DC677 File Offset: 0x000DA877
		public LinkTargetCollection LinkTargets
		{
			get
			{
				if (this._linkTargets == null)
				{
					this._linkTargets = new LinkTargetCollection();
				}
				return this._linkTargets;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Documents.FixedPage" /> associated with this <see cref="T:System.Windows.Documents.PageContent" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Documents.FixedPage" /> associated with this <see cref="T:System.Windows.Documents.PageContent" />, or null when the <see cref="T:System.Windows.Documents.FixedPage" /> is set by the <see cref="P:System.Windows.Documents.PageContent.Source" /> property. </returns>
		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06003240 RID: 12864 RVA: 0x000DC692 File Offset: 0x000DA892
		// (set) Token: 0x06003241 RID: 12865 RVA: 0x000DC69C File Offset: 0x000DA89C
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public FixedPage Child
		{
			get
			{
				return this._child;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this._child != null)
				{
					throw new InvalidOperationException(SR.Get("CanOnlyHaveOneChild", new object[]
					{
						typeof(PageContent),
						value
					}));
				}
				this._pageRef = null;
				this._child = value;
				LogicalTreeHelper.AddLogicalChild(this, this._child);
			}
		}

		/// <summary>Gets a value indicating whether the value of the <see cref="P:System.Windows.Documents.PageContent.Child" /> property should be serialized when this <see cref="T:System.Windows.Documents.PageContent" /> is serialized.</summary>
		/// <param name="manager">The serialization services provider.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="manager" /> is not <see langword="null" /> and it does not have an XmlWriter; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x06003242 RID: 12866 RVA: 0x000DC700 File Offset: 0x000DA900
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeChild(XamlDesignerSerializationManager manager)
		{
			bool result = false;
			if (manager != null)
			{
				result = (manager.XmlWriter == null);
			}
			return result;
		}

		/// <summary>For a description of this member, see <see cref="P:System.Windows.Markup.IUriContext.BaseUri" />.</summary>
		/// <returns>The base URI of the current context. </returns>
		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06003243 RID: 12867 RVA: 0x000C216F File Offset: 0x000C036F
		// (set) Token: 0x06003244 RID: 12868 RVA: 0x000C2181 File Offset: 0x000C0381
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

		/// <summary>Occurs when <see cref="M:System.Windows.Documents.PageContent.GetPageRootAsync(System.Boolean)" /> has completed.</summary>
		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06003245 RID: 12869 RVA: 0x000DC720 File Offset: 0x000DA920
		// (remove) Token: 0x06003246 RID: 12870 RVA: 0x000DC758 File Offset: 0x000DA958
		public event GetPageRootCompletedEventHandler GetPageRootCompleted;

		// Token: 0x06003247 RID: 12871 RVA: 0x000DC790 File Offset: 0x000DA990
		internal bool IsOwnerOf(FixedPage pageVisual)
		{
			if (pageVisual == null)
			{
				throw new ArgumentNullException("pageVisual");
			}
			if (this._child == pageVisual)
			{
				return true;
			}
			if (this._pageRef != null)
			{
				FixedPage fixedPage = (FixedPage)this._pageRef.Target;
				if (fixedPage == pageVisual)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x000DC7D8 File Offset: 0x000DA9D8
		internal Stream GetPageStream()
		{
			Uri uri = this._ResolveUri();
			Stream stream = null;
			if (uri != null)
			{
				stream = WpfWebRequestHelper.CreateRequestAndGetResponseStream(uri);
				if (stream == null)
				{
					throw new ApplicationException(SR.Get("PageContentNotFound"));
				}
			}
			return stream;
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x06003249 RID: 12873 RVA: 0x000DC692 File Offset: 0x000DA892
		internal FixedPage PageStream
		{
			get
			{
				return this._child;
			}
		}

		// Token: 0x0600324A RID: 12874 RVA: 0x000DC814 File Offset: 0x000DAA14
		internal bool ContainsID(string elementID)
		{
			bool result = false;
			foreach (object obj in this.LinkTargets)
			{
				LinkTarget linkTarget = (LinkTarget)obj;
				if (elementID.Equals(linkTarget.Name))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x0600324B RID: 12875 RVA: 0x000DC87C File Offset: 0x000DAA7C
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				FixedPage fixedPage = this._child;
				if (fixedPage == null)
				{
					fixedPage = this._GetLoadedPage();
				}
				FixedPage[] array;
				if (fixedPage == null)
				{
					array = new FixedPage[0];
				}
				else
				{
					array = new FixedPage[]
					{
						fixedPage
					};
				}
				return array.GetEnumerator();
			}
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x000DC8B7 File Offset: 0x000DAAB7
		private void _Init()
		{
			base.InheritanceBehavior = InheritanceBehavior.SkipToAppNow;
			this._pendingStreams = new HybridDictionary();
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x000DC8CC File Offset: 0x000DAACC
		private void _NotifyPageCompleted(FixedPage result, Exception error, bool cancelled, object userToken)
		{
			if (this.GetPageRootCompleted != null)
			{
				GetPageRootCompletedEventArgs e = new GetPageRootCompletedEventArgs(result, error, cancelled, userToken);
				this.GetPageRootCompleted(this, e);
			}
		}

		// Token: 0x0600324E RID: 12878 RVA: 0x000DC8FC File Offset: 0x000DAAFC
		private Uri _ResolveUri()
		{
			Uri uri = this.Source;
			if (uri != null)
			{
				uri = BindUriHelper.GetUriToNavigate(this, ((IUriContext)this).BaseUri, uri);
			}
			return uri;
		}

		// Token: 0x0600324F RID: 12879 RVA: 0x000DC928 File Offset: 0x000DAB28
		private void _RequestPageCallback(IAsyncResult ar)
		{
			PageContentAsyncResult pageContentAsyncResult = (PageContentAsyncResult)ar;
			if (pageContentAsyncResult == this._asyncOp && pageContentAsyncResult.Result != null)
			{
				LogicalTreeHelper.AddLogicalChild(this, pageContentAsyncResult.Result);
				this._pageRef = new WeakReference(pageContentAsyncResult.Result);
			}
			this._asyncOp = null;
			this._NotifyPageCompleted(pageContentAsyncResult.Result, pageContentAsyncResult.Exception, pageContentAsyncResult.IsCancelled, pageContentAsyncResult.AsyncState);
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x000DC990 File Offset: 0x000DAB90
		private FixedPage _LoadPage()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXGetPageBegin);
			FixedPage fixedPage = null;
			try
			{
				if (this._child != null)
				{
					fixedPage = this._child;
				}
				else
				{
					Uri uri = this._ResolveUri();
					if (uri != null)
					{
						Stream stream;
						PageContent._LoadPageImpl(((IUriContext)this).BaseUri, uri, out fixedPage, out stream);
						if (fixedPage == null || fixedPage.IsInitialized)
						{
							stream.Close();
						}
						else
						{
							this._pendingStreams.Add(fixedPage, stream);
							fixedPage.Initialized += this._OnPaserFinished;
						}
					}
				}
				if (fixedPage != null)
				{
					LogicalTreeHelper.AddLogicalChild(this, fixedPage);
					this._pageRef = new WeakReference(fixedPage);
				}
				else
				{
					this._pageRef = null;
				}
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXGetPageEnd);
			}
			return fixedPage;
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x000DCA50 File Offset: 0x000DAC50
		private FixedPage _GetLoadedPage()
		{
			FixedPage result = null;
			if (this._pageRef != null)
			{
				result = (FixedPage)this._pageRef.Target;
			}
			return result;
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x000DCA7C File Offset: 0x000DAC7C
		private void _OnPaserFinished(object sender, EventArgs args)
		{
			if (this._pendingStreams.Contains(sender))
			{
				Stream stream = (Stream)this._pendingStreams[sender];
				stream.Close();
				this._pendingStreams.Remove(sender);
			}
		}

		// Token: 0x06003253 RID: 12883 RVA: 0x000DCABC File Offset: 0x000DACBC
		internal static void _LoadPageImpl(Uri baseUri, Uri uriToLoad, out FixedPage fixedPage, out Stream pageStream)
		{
			ContentType contentType;
			pageStream = WpfWebRequestHelper.CreateRequestAndGetResponseStream(uriToLoad, out contentType);
			if (pageStream == null)
			{
				throw new ApplicationException(SR.Get("PageContentNotFound"));
			}
			ParserContext parserContext = new ParserContext();
			parserContext.BaseUri = uriToLoad;
			object obj;
			if (BindUriHelper.IsXamlMimeType(contentType))
			{
				XpsValidatingLoader xpsValidatingLoader = new XpsValidatingLoader();
				obj = xpsValidatingLoader.Load(pageStream, baseUri, parserContext, contentType);
			}
			else
			{
				if (!MimeTypeMapper.BamlMime.AreTypeAndSubTypeEqual(contentType))
				{
					throw new ApplicationException(SR.Get("PageContentUnsupportedMimeType"));
				}
				obj = XamlReader.LoadBaml(pageStream, parserContext, null, true);
			}
			if (obj != null && !(obj is FixedPage))
			{
				throw new ApplicationException(SR.Get("PageContentUnsupportedPageType", new object[]
				{
					obj.GetType()
				}));
			}
			fixedPage = (FixedPage)obj;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.PageContent.Source" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.PageContent.Source" /> dependency property.</returns>
		// Token: 0x04001EAD RID: 7853
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(PageContent), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(PageContent.OnSourceChanged)));

		// Token: 0x04001EAF RID: 7855
		private WeakReference _pageRef;

		// Token: 0x04001EB0 RID: 7856
		private FixedPage _child;

		// Token: 0x04001EB1 RID: 7857
		private PageContentAsyncResult _asyncOp;

		// Token: 0x04001EB2 RID: 7858
		private HybridDictionary _pendingStreams;

		// Token: 0x04001EB3 RID: 7859
		private LinkTargetCollection _linkTargets;
	}
}
