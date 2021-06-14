using System;
using System.IO;
using System.Security;
using System.Windows.Markup;
using System.Windows.Navigation;
using MS.Internal;
using MS.Internal.Utility;

namespace System.Windows.Documents
{
	/// <summary>Provides access to reference a <see cref="T:System.Windows.Documents.FixedDocument" />.</summary>
	// Token: 0x02000337 RID: 823
	[UsableDuringInitialization(false)]
	public sealed class DocumentReference : FrameworkElement, IUriContext
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.DocumentReference" /> class.</summary>
		// Token: 0x06002B56 RID: 11094 RVA: 0x000C6134 File Offset: 0x000C4334
		public DocumentReference()
		{
			this._Init();
		}

		/// <summary>Synchronously loads and parses the document specified by the <see cref="P:System.Windows.Documents.DocumentReference.Source" /> property location.</summary>
		/// <param name="forceReload">
		///       <see langword="true" /> to force a new load of the <see cref="P:System.Windows.Documents.DocumentReference.Source" /> document, even if it was previously loaded.</param>
		/// <returns>The document that was loaded.</returns>
		// Token: 0x06002B57 RID: 11095 RVA: 0x000C6144 File Offset: 0x000C4344
		public FixedDocument GetDocument(bool forceReload)
		{
			base.VerifyAccess();
			FixedDocument fixedDocument = null;
			if (this._doc != null)
			{
				fixedDocument = this._doc;
			}
			else
			{
				if (!forceReload)
				{
					fixedDocument = this.CurrentlyLoadedDoc;
				}
				if (fixedDocument == null)
				{
					FixedDocument fixedDocument2 = this._LoadDocument();
					if (fixedDocument2 != null)
					{
						this._docIdentity = fixedDocument2;
						fixedDocument = fixedDocument2;
					}
				}
			}
			if (fixedDocument != null)
			{
				LogicalTreeHelper.AddLogicalChild(base.Parent, fixedDocument);
			}
			return fixedDocument;
		}

		/// <summary>Attaches a <see cref="T:System.Windows.Documents.FixedDocument" /> to the <see cref="T:System.Windows.Documents.DocumentReference" />.</summary>
		/// <param name="doc">The document that is attached.</param>
		// Token: 0x06002B58 RID: 11096 RVA: 0x000C619A File Offset: 0x000C439A
		public void SetDocument(FixedDocument doc)
		{
			base.VerifyAccess();
			this._docIdentity = null;
			this._doc = doc;
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x000C61B0 File Offset: 0x000C43B0
		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DocumentReference documentReference = (DocumentReference)d;
			if (!object.Equals(e.OldValue, e.NewValue))
			{
				Uri uri = (Uri)e.OldValue;
				Uri uri2 = (Uri)e.NewValue;
				documentReference._doc = null;
			}
		}

		/// <summary>Gets or sets the uniform resource identifier (URI) for this document reference. </summary>
		/// <returns>A <see cref="T:System.Uri" /> representing the document reference.</returns>
		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06002B5A RID: 11098 RVA: 0x000C61FA File Offset: 0x000C43FA
		// (set) Token: 0x06002B5B RID: 11099 RVA: 0x000C620C File Offset: 0x000C440C
		public Uri Source
		{
			get
			{
				return (Uri)base.GetValue(DocumentReference.SourceProperty);
			}
			set
			{
				base.SetValue(DocumentReference.SourceProperty, value);
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Windows.Markup.IUriContext.BaseUri" />.</summary>
		/// <returns>The base URI of the current context.</returns>
		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06002B5C RID: 11100 RVA: 0x000C216F File Offset: 0x000C036F
		// (set) Token: 0x06002B5D RID: 11101 RVA: 0x000C2181 File Offset: 0x000C0381
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

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06002B5E RID: 11102 RVA: 0x000C621A File Offset: 0x000C441A
		internal FixedDocument CurrentlyLoadedDoc
		{
			get
			{
				return this._docIdentity;
			}
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x000C6222 File Offset: 0x000C4422
		private void _Init()
		{
			base.InheritanceBehavior = InheritanceBehavior.SkipToAppNow;
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x000C622C File Offset: 0x000C442C
		[SecurityCritical]
		private Uri _ResolveUri()
		{
			Uri uri = this.Source;
			if (uri != null)
			{
				uri = BindUriHelper.GetUriToNavigate(this, ((IUriContext)this).BaseUri, uri);
			}
			return uri;
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x000C6258 File Offset: 0x000C4458
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private FixedDocument _LoadDocument()
		{
			FixedDocument fixedDocument = null;
			Uri uri = this._ResolveUri();
			if (uri != null)
			{
				ContentType contentType = null;
				Stream stream = WpfWebRequestHelper.CreateRequestAndGetResponseStream(uri, out contentType);
				if (stream == null)
				{
					throw new ApplicationException(SR.Get("DocumentReferenceNotFound"));
				}
				ParserContext parserContext = new ParserContext();
				parserContext.BaseUri = uri;
				if (BindUriHelper.IsXamlMimeType(contentType))
				{
					XpsValidatingLoader xpsValidatingLoader = new XpsValidatingLoader();
					fixedDocument = (xpsValidatingLoader.Load(stream, ((IUriContext)this).BaseUri, parserContext, contentType) as FixedDocument);
				}
				else
				{
					if (!MimeTypeMapper.BamlMime.AreTypeAndSubTypeEqual(contentType))
					{
						throw new ApplicationException(SR.Get("DocumentReferenceUnsupportedMimeType"));
					}
					fixedDocument = (XamlReader.LoadBaml(stream, parserContext, null, true) as FixedDocument);
				}
				fixedDocument.DocumentReference = this;
			}
			return fixedDocument;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.DocumentReference.Source" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.DocumentReference.Source" /> dependency property.</returns>
		// Token: 0x04001C9F RID: 7327
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(DocumentReference), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DocumentReference.OnSourceChanged)));

		// Token: 0x04001CA0 RID: 7328
		private FixedDocument _doc;

		// Token: 0x04001CA1 RID: 7329
		private FixedDocument _docIdentity;
	}
}
