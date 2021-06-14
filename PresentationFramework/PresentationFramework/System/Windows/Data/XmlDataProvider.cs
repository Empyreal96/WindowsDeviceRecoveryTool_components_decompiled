using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Net;
using System.Threading;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using MS.Internal;
using MS.Internal.Data;
using MS.Internal.Utility;

namespace System.Windows.Data
{
	/// <summary>Enables declarative access to XML data for data binding.</summary>
	// Token: 0x020001BF RID: 447
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	[ContentProperty("XmlSerializer")]
	public class XmlDataProvider : DataSourceProvider, IUriContext
	{
		/// <summary>Gets or sets the <see cref="T:System.Uri" /> of the XML data file to use as the binding source.</summary>
		/// <returns>The <see cref="T:System.Uri" /> of the XML data file to use as the binding source. The default value is <see langword="null" />.</returns>
		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06001CCA RID: 7370 RVA: 0x00086B48 File Offset: 0x00084D48
		// (set) Token: 0x06001CCB RID: 7371 RVA: 0x00086B50 File Offset: 0x00084D50
		public Uri Source
		{
			get
			{
				return this._source;
			}
			set
			{
				if (this._domSetDocument != null || this._source != value)
				{
					this._domSetDocument = null;
					this._source = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Source"));
					if (!base.IsRefreshDeferred)
					{
						base.Refresh();
					}
				}
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Data.XmlDataProvider.Source" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001CCC RID: 7372 RVA: 0x00086B9F File Offset: 0x00084D9F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeSource()
		{
			return this._domSetDocument == null && this._source != null;
		}

		/// <summary>Gets or sets the <see cref="T:System.Xml.XmlDocument" /> to use as the binding source.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlDocument" /> to use as the binding source. The default value is <see langword="null" />.</returns>
		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06001CCD RID: 7373 RVA: 0x00086BB7 File Offset: 0x00084DB7
		// (set) Token: 0x06001CCE RID: 7374 RVA: 0x00086BC0 File Offset: 0x00084DC0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public XmlDocument Document
		{
			get
			{
				return this._document;
			}
			set
			{
				if (this._domSetDocument == null || this._source != null || this._document != value)
				{
					this._domSetDocument = value;
					this._source = null;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Source"));
					this.ChangeDocument(value);
					if (!base.IsRefreshDeferred)
					{
						base.Refresh();
					}
				}
			}
		}

		/// <summary>Gets or sets the <see langword="XPath" /> query used to generate the data collection.</summary>
		/// <returns>The <see langword="XPath" /> query used to generate the data collection. The default is an empty string.</returns>
		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06001CCF RID: 7375 RVA: 0x00086C1F File Offset: 0x00084E1F
		// (set) Token: 0x06001CD0 RID: 7376 RVA: 0x00086C27 File Offset: 0x00084E27
		[DesignerSerializationOptions(DesignerSerializationOptions.SerializeAsAttribute)]
		public string XPath
		{
			get
			{
				return this._xPath;
			}
			set
			{
				if (this._xPath != value)
				{
					this._xPath = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("XPath"));
					if (!base.IsRefreshDeferred)
					{
						base.Refresh();
					}
				}
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Data.XmlDataProvider.XPath" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001CD1 RID: 7377 RVA: 0x00086C5C File Offset: 0x00084E5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeXPath()
		{
			return this._xPath != null && this._xPath.Length != 0;
		}

		/// <summary>Gets or sets the <see cref="T:System.Xml.XmlNamespaceManager" /> used to run <see cref="P:System.Windows.Data.XmlDataProvider.XPath" /> queries.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlNamespaceManager" /> used to run <see cref="P:System.Windows.Data.XmlDataProvider.XPath" /> queries. The default value is <see langword="null" />.</returns>
		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06001CD2 RID: 7378 RVA: 0x00086C76 File Offset: 0x00084E76
		// (set) Token: 0x06001CD3 RID: 7379 RVA: 0x00086C7E File Offset: 0x00084E7E
		[DefaultValue(null)]
		public XmlNamespaceManager XmlNamespaceManager
		{
			get
			{
				return this._nsMgr;
			}
			set
			{
				if (this._nsMgr != value)
				{
					this._nsMgr = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("XmlNamespaceManager"));
					if (!base.IsRefreshDeferred)
					{
						base.Refresh();
					}
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether node collection creation will be performed in a worker thread or in the active context.</summary>
		/// <returns>
		///     <see langword="true" /> to perform node collection creation in a worker thread; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06001CD4 RID: 7380 RVA: 0x00086CAE File Offset: 0x00084EAE
		// (set) Token: 0x06001CD5 RID: 7381 RVA: 0x00086CB6 File Offset: 0x00084EB6
		[DefaultValue(true)]
		public bool IsAsynchronous
		{
			get
			{
				return this._isAsynchronous;
			}
			set
			{
				this._isAsynchronous = value;
			}
		}

		/// <summary>Gets the inline XML content.</summary>
		/// <returns>The inline XML content.</returns>
		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06001CD6 RID: 7382 RVA: 0x00086CBF File Offset: 0x00084EBF
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public IXmlSerializable XmlSerializer
		{
			get
			{
				if (this._xmlSerializer == null)
				{
					this._xmlSerializer = new XmlDataProvider.XmlIslandSerializer(this);
				}
				return this._xmlSerializer;
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Data.XmlDataProvider.XmlSerializer" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001CD7 RID: 7383 RVA: 0x00086CDB File Offset: 0x00084EDB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeXmlSerializer()
		{
			return this.DocumentForSerialization != null;
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The base URI.</returns>
		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06001CD8 RID: 7384 RVA: 0x00086CE6 File Offset: 0x00084EE6
		// (set) Token: 0x06001CD9 RID: 7385 RVA: 0x00086CEE File Offset: 0x00084EEE
		Uri IUriContext.BaseUri
		{
			get
			{
				return this.BaseUri;
			}
			set
			{
				this.BaseUri = value;
			}
		}

		/// <summary> This type or member supports the WPF infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The base URI.</returns>
		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06001CDA RID: 7386 RVA: 0x00086CF7 File Offset: 0x00084EF7
		// (set) Token: 0x06001CDB RID: 7387 RVA: 0x00086CFF File Offset: 0x00084EFF
		protected virtual Uri BaseUri
		{
			get
			{
				return this._baseUri;
			}
			set
			{
				this._baseUri = value;
			}
		}

		/// <summary>Prepares the loading of either the inline XML or the external XML file to produce a collection of XML nodes.</summary>
		// Token: 0x06001CDC RID: 7388 RVA: 0x00086D08 File Offset: 0x00084F08
		protected override void BeginQuery()
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.BeginQuery(new object[]
				{
					TraceData.Identify(this),
					this.IsAsynchronous ? "asynchronous" : "synchronous"
				}));
			}
			if (this._source != null)
			{
				this.DiscardInline();
				this.LoadFromSource();
				return;
			}
			XmlDocument xmlDocument;
			if (this._domSetDocument != null)
			{
				this.DiscardInline();
				xmlDocument = this._domSetDocument;
			}
			else
			{
				if (this._inEndInit)
				{
					return;
				}
				xmlDocument = this._savedDocument;
			}
			if (this.IsAsynchronous && xmlDocument != null)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.BuildNodeCollectionAsynch), xmlDocument);
				return;
			}
			if (xmlDocument != null || base.Data != null)
			{
				this.BuildNodeCollection(xmlDocument);
			}
		}

		/// <summary>Indicates that the initialization of this element has completed; this causes a <see cref="M:System.Windows.Data.DataSourceProvider.Refresh" /> if no other <see cref="M:System.Windows.Data.DataSourceProvider.DeferRefresh" /> is outstanding.</summary>
		// Token: 0x06001CDD RID: 7389 RVA: 0x00086DC4 File Offset: 0x00084FC4
		protected override void EndInit()
		{
			try
			{
				this._inEndInit = true;
				base.EndInit();
			}
			finally
			{
				this._inEndInit = false;
			}
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x00086DF8 File Offset: 0x00084FF8
		private void LoadFromSource()
		{
			Uri uri = this.Source;
			if (!uri.IsAbsoluteUri)
			{
				Uri baseUri = (this._baseUri != null) ? this._baseUri : BindUriHelper.BaseUri;
				uri = BindUriHelper.GetResolvedUri(baseUri, uri);
			}
			WebRequest webRequest = PackWebRequestFactory.CreateWebRequest(uri);
			if (webRequest == null)
			{
				throw new Exception(SR.Get("WebRequestCreationFailed"));
			}
			if (this.IsAsynchronous)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.CreateDocFromExternalSourceAsynch), webRequest);
				return;
			}
			this.CreateDocFromExternalSource(webRequest);
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x00086E78 File Offset: 0x00085078
		private void ParseInline(XmlReader xmlReader)
		{
			if (this._source == null && this._domSetDocument == null && this._tryInlineDoc)
			{
				if (this.IsAsynchronous)
				{
					this._waitForInlineDoc = new ManualResetEvent(false);
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.CreateDocFromInlineXmlAsync), xmlReader);
					return;
				}
				this.CreateDocFromInlineXml(xmlReader);
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06001CE0 RID: 7392 RVA: 0x00086ED2 File Offset: 0x000850D2
		private XmlDocument DocumentForSerialization
		{
			get
			{
				if (this._tryInlineDoc || this._savedDocument != null || this._domSetDocument != null)
				{
					if (this._waitForInlineDoc != null)
					{
						this._waitForInlineDoc.WaitOne();
					}
					return this._document;
				}
				return null;
			}
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x00086F08 File Offset: 0x00085108
		private void CreateDocFromInlineXmlAsync(object arg)
		{
			XmlReader xmlReader = (XmlReader)arg;
			this.CreateDocFromInlineXml(xmlReader);
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x00086F24 File Offset: 0x00085124
		private void CreateDocFromInlineXml(XmlReader xmlReader)
		{
			if (!this._tryInlineDoc)
			{
				this._savedDocument = null;
				if (this._waitForInlineDoc != null)
				{
					this._waitForInlineDoc.Set();
				}
				return;
			}
			Exception ex = null;
			XmlDocument xmlDocument;
			try
			{
				xmlDocument = new XmlDocument();
				try
				{
					if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.XmlLoadInline(new object[]
						{
							TraceData.Identify(this),
							base.Dispatcher.CheckAccess() ? "synchronous" : "asynchronous"
						}));
					}
					xmlDocument.Load(xmlReader);
				}
				catch (XmlException ex2)
				{
					if (TraceData.IsEnabled)
					{
						TraceData.Trace(TraceEventType.Error, TraceData.XmlDPInlineDocError, ex2);
					}
					ex = ex2;
				}
				if (ex == null)
				{
					this._savedDocument = (XmlDocument)xmlDocument.Clone();
				}
			}
			finally
			{
				xmlReader.Close();
				if (this._waitForInlineDoc != null)
				{
					this._waitForInlineDoc.Set();
				}
			}
			if (TraceData.IsEnabled)
			{
				XmlNode documentElement = xmlDocument.DocumentElement;
				if (documentElement != null && documentElement.NamespaceURI == xmlReader.LookupNamespace(string.Empty))
				{
					TraceData.Trace(TraceEventType.Error, TraceData.XmlNamespaceNotSet);
				}
			}
			if (ex == null)
			{
				this.BuildNodeCollection(xmlDocument);
				return;
			}
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.QueryFinished(new object[]
				{
					TraceData.Identify(this),
					base.Dispatcher.CheckAccess() ? "synchronous" : "asynchronous",
					TraceData.Identify(null),
					TraceData.IdentifyException(ex)
				}));
			}
			this.OnQueryFinished(null, ex, this.CompletedCallback, null);
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x000870AC File Offset: 0x000852AC
		private void CreateDocFromExternalSourceAsynch(object arg)
		{
			WebRequest request = (WebRequest)arg;
			this.CreateDocFromExternalSource(request);
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x000870C8 File Offset: 0x000852C8
		private void CreateDocFromExternalSource(WebRequest request)
		{
			bool flag = TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer);
			XmlDocument xmlDocument = new XmlDocument();
			Exception ex = null;
			try
			{
				if (flag)
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.XmlLoadSource(new object[]
					{
						TraceData.Identify(this),
						base.Dispatcher.CheckAccess() ? "synchronous" : "asynchronous",
						TraceData.Identify(request.RequestUri.ToString())
					}));
				}
				WebResponse response = WpfWebRequestHelper.GetResponse(request);
				if (response == null)
				{
					throw new InvalidOperationException(SR.Get("GetResponseFailed"));
				}
				Stream responseStream = response.GetResponseStream();
				if (flag)
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.XmlLoadDoc(new object[]
					{
						TraceData.Identify(this)
					}));
				}
				xmlDocument.Load(responseStream);
				responseStream.Close();
			}
			catch (Exception ex2)
			{
				if (CriticalExceptions.IsCriticalException(ex2))
				{
					throw;
				}
				ex = ex2;
				if (TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.XmlDPAsyncDocError, this.Source, ex);
				}
			}
			catch
			{
				throw;
			}
			if (ex != null)
			{
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.QueryFinished(new object[]
					{
						TraceData.Identify(this),
						base.Dispatcher.CheckAccess() ? "synchronous" : "asynchronous",
						TraceData.Identify(null),
						TraceData.IdentifyException(ex)
					}));
				}
				this.OnQueryFinished(null, ex, this.CompletedCallback, null);
				return;
			}
			this.BuildNodeCollection(xmlDocument);
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x00087238 File Offset: 0x00085438
		private void BuildNodeCollectionAsynch(object arg)
		{
			XmlDocument doc = (XmlDocument)arg;
			this.BuildNodeCollection(doc);
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x00087254 File Offset: 0x00085454
		private void BuildNodeCollection(XmlDocument doc)
		{
			XmlDataCollection xmlDataCollection = null;
			if (doc != null)
			{
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.CreateExpression))
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.XmlBuildCollection(new object[]
					{
						TraceData.Identify(this)
					}));
				}
				XmlNodeList resultNodeList = this.GetResultNodeList(doc);
				xmlDataCollection = new XmlDataCollection(this);
				if (resultNodeList != null)
				{
					xmlDataCollection.SynchronizeCollection(resultNodeList);
				}
			}
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.QueryFinished(new object[]
				{
					TraceData.Identify(this),
					base.Dispatcher.CheckAccess() ? "synchronous" : "asynchronous",
					TraceData.Identify(xmlDataCollection),
					TraceData.IdentifyException(null)
				}));
			}
			this.OnQueryFinished(xmlDataCollection, null, this.CompletedCallback, doc);
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x00087302 File Offset: 0x00085502
		private object OnCompletedCallback(object arg)
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.QueryResult(new object[]
				{
					TraceData.Identify(this),
					TraceData.Identify(base.Data)
				}));
			}
			this.ChangeDocument((XmlDocument)arg);
			return null;
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x00087342 File Offset: 0x00085542
		private void ChangeDocument(XmlDocument doc)
		{
			if (this._document != doc)
			{
				if (this._document != null)
				{
					this.UnHook();
				}
				this._document = doc;
				if (this._document != null)
				{
					this.Hook();
				}
				this.OnPropertyChanged(new PropertyChangedEventArgs("Document"));
			}
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x00087380 File Offset: 0x00085580
		private void DiscardInline()
		{
			this._tryInlineDoc = false;
			this._savedDocument = null;
			if (this._waitForInlineDoc != null)
			{
				this._waitForInlineDoc.Set();
			}
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x000873A4 File Offset: 0x000855A4
		private void Hook()
		{
			if (!this._isListening)
			{
				this._document.NodeInserted += this.NodeChangeHandler;
				this._document.NodeRemoved += this.NodeChangeHandler;
				this._document.NodeChanged += this.NodeChangeHandler;
				this._isListening = true;
			}
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x000873F4 File Offset: 0x000855F4
		private void UnHook()
		{
			if (this._isListening)
			{
				this._document.NodeInserted -= this.NodeChangeHandler;
				this._document.NodeRemoved -= this.NodeChangeHandler;
				this._document.NodeChanged -= this.NodeChangeHandler;
				this._isListening = false;
			}
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x00087444 File Offset: 0x00085644
		private void OnNodeChanged(object sender, XmlNodeChangedEventArgs e)
		{
			if (this.XmlDataCollection == null)
			{
				return;
			}
			this.UnHook();
			XmlNodeList resultNodeList = this.GetResultNodeList((XmlDocument)sender);
			this.XmlDataCollection.SynchronizeCollection(resultNodeList);
			this.Hook();
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x00087480 File Offset: 0x00085680
		private XmlNodeList GetResultNodeList(XmlDocument doc)
		{
			XmlNodeList result = null;
			if (doc.DocumentElement != null)
			{
				string text = string.IsNullOrEmpty(this.XPath) ? "/" : this.XPath;
				try
				{
					if (this.XmlNamespaceManager != null)
					{
						result = doc.SelectNodes(text, this.XmlNamespaceManager);
					}
					else
					{
						result = doc.SelectNodes(text);
					}
				}
				catch (XPathException p)
				{
					if (TraceData.IsEnabled)
					{
						TraceData.Trace(TraceEventType.Error, TraceData.XmlDPSelectNodesFailed, text, p);
					}
				}
			}
			return result;
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06001CEE RID: 7406 RVA: 0x000874FC File Offset: 0x000856FC
		private XmlDataCollection XmlDataCollection
		{
			get
			{
				return (XmlDataCollection)base.Data;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06001CEF RID: 7407 RVA: 0x00087509 File Offset: 0x00085709
		private DispatcherOperationCallback CompletedCallback
		{
			get
			{
				if (this._onCompletedCallback == null)
				{
					this._onCompletedCallback = new DispatcherOperationCallback(this.OnCompletedCallback);
				}
				return this._onCompletedCallback;
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x0008752B File Offset: 0x0008572B
		private XmlNodeChangedEventHandler NodeChangeHandler
		{
			get
			{
				if (this._nodeChangedHandler == null)
				{
					this._nodeChangedHandler = new XmlNodeChangedEventHandler(this.OnNodeChanged);
				}
				return this._nodeChangedHandler;
			}
		}

		// Token: 0x040013ED RID: 5101
		private XmlDocument _document;

		// Token: 0x040013EE RID: 5102
		private XmlDocument _domSetDocument;

		// Token: 0x040013EF RID: 5103
		private XmlDocument _savedDocument;

		// Token: 0x040013F0 RID: 5104
		private ManualResetEvent _waitForInlineDoc;

		// Token: 0x040013F1 RID: 5105
		private XmlNamespaceManager _nsMgr;

		// Token: 0x040013F2 RID: 5106
		private Uri _source;

		// Token: 0x040013F3 RID: 5107
		private Uri _baseUri;

		// Token: 0x040013F4 RID: 5108
		private string _xPath = string.Empty;

		// Token: 0x040013F5 RID: 5109
		private bool _tryInlineDoc = true;

		// Token: 0x040013F6 RID: 5110
		private bool _isListening;

		// Token: 0x040013F7 RID: 5111
		private XmlDataProvider.XmlIslandSerializer _xmlSerializer;

		// Token: 0x040013F8 RID: 5112
		private bool _isAsynchronous = true;

		// Token: 0x040013F9 RID: 5113
		private bool _inEndInit;

		// Token: 0x040013FA RID: 5114
		private DispatcherOperationCallback _onCompletedCallback;

		// Token: 0x040013FB RID: 5115
		private XmlNodeChangedEventHandler _nodeChangedHandler;

		// Token: 0x02000886 RID: 2182
		private class XmlIslandSerializer : IXmlSerializable
		{
			// Token: 0x0600833C RID: 33596 RVA: 0x00244F89 File Offset: 0x00243189
			internal XmlIslandSerializer(XmlDataProvider host)
			{
				this._host = host;
			}

			// Token: 0x0600833D RID: 33597 RVA: 0x0000C238 File Offset: 0x0000A438
			public XmlSchema GetSchema()
			{
				return null;
			}

			// Token: 0x0600833E RID: 33598 RVA: 0x00244F98 File Offset: 0x00243198
			public void WriteXml(XmlWriter writer)
			{
				XmlDocument documentForSerialization = this._host.DocumentForSerialization;
				if (documentForSerialization != null)
				{
					documentForSerialization.Save(writer);
				}
			}

			// Token: 0x0600833F RID: 33599 RVA: 0x00244FBB File Offset: 0x002431BB
			public void ReadXml(XmlReader reader)
			{
				this._host.ParseInline(reader);
			}

			// Token: 0x04004161 RID: 16737
			private XmlDataProvider _host;
		}
	}
}
