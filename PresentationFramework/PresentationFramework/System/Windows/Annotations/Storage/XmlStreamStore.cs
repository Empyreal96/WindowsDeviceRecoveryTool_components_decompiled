using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;
using System.Xml;
using System.Xml.XPath;
using MS.Internal;
using MS.Internal.Annotations;
using MS.Internal.Annotations.Storage;
using MS.Utility;

namespace System.Windows.Annotations.Storage
{
	/// <summary>Represents an XML data store for writing and reading user annotations.</summary>
	// Token: 0x020005D7 RID: 1495
	public sealed class XmlStreamStore : AnnotationStore
	{
		// Token: 0x0600636F RID: 25455 RVA: 0x001BF584 File Offset: 0x001BD784
		static XmlStreamStore()
		{
			XmlStreamStore._predefinedNamespaces = new Dictionary<Uri, IList<Uri>>(6);
			XmlStreamStore._predefinedNamespaces.Add(new Uri("http://schemas.microsoft.com/windows/annotations/2003/11/core"), null);
			XmlStreamStore._predefinedNamespaces.Add(new Uri("http://schemas.microsoft.com/windows/annotations/2003/11/base"), null);
			XmlStreamStore._predefinedNamespaces.Add(new Uri("http://schemas.microsoft.com/winfx/2006/xaml/presentation"), null);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Annotations.Storage.XmlStreamStore" /> class with a specified I/O <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="stream">The I/O stream for reading and writing user annotations.</param>
		// Token: 0x06006370 RID: 25456 RVA: 0x001BF5F0 File Offset: 0x001BD7F0
		public XmlStreamStore(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanSeek)
			{
				throw new ArgumentException(SR.Get("StreamDoesNotSupportSeek"));
			}
			this.SetStream(stream, null);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Annotations.Storage.XmlStreamStore" /> class with a specified I/O <see cref="T:System.IO.Stream" /> and dictionary of known compatible namespaces.</summary>
		/// <param name="stream">The I/O stream for reading and writing user annotations.</param>
		/// <param name="knownNamespaces">A dictionary with a list of known compatible namespaces.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="stream" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Xml.XmlException">
		///         <paramref name="stream" /> contains invalid XML.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="knownNamespaces" /> dictionary contains a duplicate namespace.-or-The <paramref name="knownNamespaces" /> dictionary contains an element that has a <see langword="null" /> key.</exception>
		// Token: 0x06006371 RID: 25457 RVA: 0x001BF63C File Offset: 0x001BD83C
		public XmlStreamStore(Stream stream, IDictionary<Uri, IList<Uri>> knownNamespaces)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.SetStream(stream, knownNamespaces);
		}

		/// <summary>Adds a new <see cref="T:System.Windows.Annotations.Annotation" /> to the store.</summary>
		/// <param name="newAnnotation">The annotation to add to the store.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="newAnnotation" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">An <see cref="T:System.Windows.Annotations.Annotation" /> with the same <see cref="P:System.Windows.Annotations.Annotation.Id" /> already is in the store.</exception>
		/// <exception cref="T:System.InvalidOperationException">An I/O <see cref="T:System.IO.Stream" /> has not been set for the store.</exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///         <see cref="Overload:System.Windows.Annotations.Storage.AnnotationStore.Dispose" /> has been called on the store.</exception>
		// Token: 0x06006372 RID: 25458 RVA: 0x001BF668 File Offset: 0x001BD868
		public override void AddAnnotation(Annotation newAnnotation)
		{
			if (newAnnotation == null)
			{
				throw new ArgumentNullException("newAnnotation");
			}
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAnnotationBegin);
				try
				{
					this.CheckStatus();
					XPathNavigator annotationNodeForId = this.GetAnnotationNodeForId(newAnnotation.Id);
					if (annotationNodeForId != null)
					{
						throw new ArgumentException(SR.Get("AnnotationAlreadyExists"), "newAnnotation");
					}
					if (this._storeAnnotationsMap.FindAnnotation(newAnnotation.Id) != null)
					{
						throw new ArgumentException(SR.Get("AnnotationAlreadyExists"), "newAnnotation");
					}
					this._storeAnnotationsMap.AddAnnotation(newAnnotation, true);
				}
				finally
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAnnotationEnd);
				}
			}
			this.OnStoreContentChanged(new StoreContentChangedEventArgs(StoreContentAction.Added, newAnnotation));
		}

		/// <summary>Deletes the annotation with the specified <see cref="P:System.Windows.Annotations.Annotation.Id" /> from the store. </summary>
		/// <param name="annotationId">The globally unique identifier (GUID) <see cref="P:System.Windows.Annotations.Annotation.Id" /> property of the annotation to be deleted.</param>
		/// <returns>The annotation that was deleted; otherwise, <see langword="null" /> if an annotation with the specified <paramref name="annotationId" /> was not found in the store.</returns>
		/// <exception cref="T:System.ObjectDisposedException">
		///         <see cref="Overload:System.Windows.Annotations.Storage.AnnotationStore.Dispose" /> has been called on the store.</exception>
		/// <exception cref="T:System.InvalidOperationException">An I/O <see cref="T:System.IO.Stream" /> has not been set for the store.</exception>
		// Token: 0x06006373 RID: 25459 RVA: 0x001BF744 File Offset: 0x001BD944
		public override Annotation DeleteAnnotation(Guid annotationId)
		{
			Annotation annotation = null;
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.DeleteAnnotationBegin);
				try
				{
					this.CheckStatus();
					annotation = this._storeAnnotationsMap.FindAnnotation(annotationId);
					XPathNavigator annotationNodeForId = this.GetAnnotationNodeForId(annotationId);
					if (annotationNodeForId != null)
					{
						if (annotation == null)
						{
							annotation = (Annotation)XmlStreamStore._serializer.Deserialize(annotationNodeForId.ReadSubtree());
						}
						annotationNodeForId.DeleteSelf();
					}
					this._storeAnnotationsMap.RemoveAnnotation(annotationId);
				}
				finally
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.DeleteAnnotationEnd);
				}
			}
			if (annotation != null)
			{
				this.OnStoreContentChanged(new StoreContentChangedEventArgs(StoreContentAction.Deleted, annotation));
			}
			return annotation;
		}

		/// <summary>Returns a list of annotations that have <see cref="P:System.Windows.Annotations.Annotation.Anchors" /> with locators that begin with a matching <see cref="T:System.Windows.Annotations.ContentLocatorPart" /> sequence.</summary>
		/// <param name="anchorLocator">The starting <see cref="T:System.Windows.Annotations.ContentLocatorPart" /> sequence to return matching annotations for.</param>
		/// <returns>The list of annotations that have <see cref="P:System.Windows.Annotations.Annotation.Anchors" /> with locators that start and match the given <paramref name="anchorLocator" />; otherwise, <see langword="null" /> if no matching annotations were found.</returns>
		// Token: 0x06006374 RID: 25460 RVA: 0x001BF800 File Offset: 0x001BDA00
		public override IList<Annotation> GetAnnotations(ContentLocator anchorLocator)
		{
			if (anchorLocator == null)
			{
				throw new ArgumentNullException("anchorLocator");
			}
			if (anchorLocator.Parts == null)
			{
				throw new ArgumentNullException("anchorLocator.Parts");
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.GetAnnotationByLocBegin);
			IList<Annotation> result = null;
			try
			{
				string text = "//anc:ContentLocator";
				if (anchorLocator.Parts.Count > 0)
				{
					text += "/child::*[1]/self::";
					for (int i = 0; i < anchorLocator.Parts.Count; i++)
					{
						if (anchorLocator.Parts[i] != null)
						{
							if (i > 0)
							{
								text += "/following-sibling::";
							}
							string queryFragment = anchorLocator.Parts[i].GetQueryFragment(this._namespaceManager);
							if (queryFragment != null)
							{
								text += queryFragment;
							}
							else
							{
								text += "*";
							}
						}
					}
				}
				text += "/ancestor::anc:Anchors/ancestor::anc:Annotation";
				result = this.InternalGetAnnotations(text, anchorLocator);
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.GetAnnotationByLocEnd);
			}
			return result;
		}

		/// <summary>Returns a list of all the annotations in the store.</summary>
		/// <returns>The list of all annotations that are currently in the store.</returns>
		/// <exception cref="T:System.ObjectDisposedException">
		///         <see cref="Overload:System.Windows.Annotations.Storage.AnnotationStore.Dispose" /> has been called on the store.</exception>
		// Token: 0x06006375 RID: 25461 RVA: 0x001BF8F8 File Offset: 0x001BDAF8
		public override IList<Annotation> GetAnnotations()
		{
			IList<Annotation> result = null;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.GetAnnotationsBegin);
			try
			{
				string query = "//anc:Annotation";
				result = this.InternalGetAnnotations(query, null);
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.GetAnnotationsEnd);
			}
			return result;
		}

		/// <summary>Returns the annotation with the specified <see cref="P:System.Windows.Annotations.Annotation.Id" /> from the store.</summary>
		/// <param name="annotationId">The globally unique identifier (GUID) <see cref="P:System.Windows.Annotations.Annotation.Id" /> property of the annotation to be returned.</param>
		/// <returns>The annotation with the given <paramref name="annotationId" />; otherwise, <see langword="null" /> if an annotation with the specified <paramref name="annotationId" /> was not found in the store.</returns>
		/// <exception cref="T:System.ObjectDisposedException">
		///         <see cref="Overload:System.Windows.Annotations.Storage.AnnotationStore.Dispose" /> has been called on the store.</exception>
		// Token: 0x06006376 RID: 25462 RVA: 0x001BF944 File Offset: 0x001BDB44
		public override Annotation GetAnnotation(Guid annotationId)
		{
			object syncRoot = base.SyncRoot;
			Annotation result;
			lock (syncRoot)
			{
				Annotation annotation = null;
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.GetAnnotationByIdBegin);
				try
				{
					this.CheckStatus();
					annotation = this._storeAnnotationsMap.FindAnnotation(annotationId);
					if (annotation != null)
					{
						return annotation;
					}
					XPathNavigator annotationNodeForId = this.GetAnnotationNodeForId(annotationId);
					if (annotationNodeForId != null)
					{
						annotation = (Annotation)XmlStreamStore._serializer.Deserialize(annotationNodeForId.ReadSubtree());
						this._storeAnnotationsMap.AddAnnotation(annotation, false);
					}
				}
				finally
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.GetAnnotationByIdEnd);
				}
				result = annotation;
			}
			return result;
		}

		/// <summary>Forces any annotation data retained in internal buffers to be written to the underlying storage device.</summary>
		/// <exception cref="T:System.ObjectDisposedException">
		///         <see cref="Overload:System.Windows.Annotations.Storage.AnnotationStore.Dispose" /> has been called on the store.</exception>
		/// <exception cref="T:System.InvalidOperationException">An I/O <see cref="T:System.IO.Stream" /> has not been set for the store.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The store I/O <see cref="T:System.IO.Stream" /> is read-only and cannot be accessed for output.</exception>
		// Token: 0x06006377 RID: 25463 RVA: 0x001BF9F8 File Offset: 0x001BDBF8
		public override void Flush()
		{
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				this.CheckStatus();
				if (!this._stream.CanWrite)
				{
					throw new UnauthorizedAccessException(SR.Get("StreamCannotBeWritten"));
				}
				if (this._dirty)
				{
					this.SerializeAnnotations();
					this._stream.Position = 0L;
					this._stream.SetLength(0L);
					this._document.PreserveWhitespace = true;
					this._document.Save(this._stream);
					this._stream.Flush();
					this._dirty = false;
				}
			}
		}

		/// <summary>Returns a list of namespaces that are compatible as an input namespace.</summary>
		/// <param name="name">The starting URI sequence to return the list of namespaces for.</param>
		/// <returns>A list of compatible namespaces that match <paramref name="name" />; otherwise, <see langword="null" /> if there are no compatible namespaces found.</returns>
		// Token: 0x06006378 RID: 25464 RVA: 0x001BFAAC File Offset: 0x001BDCAC
		public static IList<Uri> GetWellKnownCompatibleNamespaces(Uri name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (XmlStreamStore._predefinedNamespaces.ContainsKey(name))
			{
				return XmlStreamStore._predefinedNamespaces[name];
			}
			return null;
		}

		/// <summary>Gets or sets a value that indicates whether data in annotation buffers is to be written immediately to the physical data store.</summary>
		/// <returns>
		///     <see langword="true" /> if data in annotation buffers is to be written immediately to the physical data store for each operation; otherwise, <see langword="false" /> if data in the annotation buffers is to be written when the application explicitly calls <see cref="M:System.Windows.Annotations.Storage.XmlStreamStore.Flush" />.</returns>
		// Token: 0x170017D4 RID: 6100
		// (get) Token: 0x06006379 RID: 25465 RVA: 0x001BFADC File Offset: 0x001BDCDC
		// (set) Token: 0x0600637A RID: 25466 RVA: 0x001BFB20 File Offset: 0x001BDD20
		public override bool AutoFlush
		{
			get
			{
				object syncRoot = base.SyncRoot;
				bool autoFlush;
				lock (syncRoot)
				{
					autoFlush = this._autoFlush;
				}
				return autoFlush;
			}
			set
			{
				object syncRoot = base.SyncRoot;
				lock (syncRoot)
				{
					this._autoFlush = value;
					if (this._autoFlush)
					{
						this.Flush();
					}
				}
			}
		}

		/// <summary>Gets a list of the namespaces that were ignored when the XML stream was loaded.</summary>
		/// <returns>The list of the namespaces that were ignored when the XML stream was loaded.</returns>
		// Token: 0x170017D5 RID: 6101
		// (get) Token: 0x0600637B RID: 25467 RVA: 0x001BFB70 File Offset: 0x001BDD70
		public IList<Uri> IgnoredNamespaces
		{
			get
			{
				return this._ignoredNamespaces;
			}
		}

		/// <summary>Gets a list of all namespaces that are predefined by the Annotations Framework.</summary>
		/// <returns>The list of namespaces that are predefined by the Microsoft Annotations Framework.</returns>
		// Token: 0x170017D6 RID: 6102
		// (get) Token: 0x0600637C RID: 25468 RVA: 0x001BFB78 File Offset: 0x001BDD78
		public static IList<Uri> WellKnownNamespaces
		{
			get
			{
				Uri[] array = new Uri[XmlStreamStore._predefinedNamespaces.Keys.Count];
				XmlStreamStore._predefinedNamespaces.Keys.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x0600637D RID: 25469 RVA: 0x001BFBAC File Offset: 0x001BDDAC
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				this.Cleanup();
			}
		}

		// Token: 0x0600637E RID: 25470 RVA: 0x001BFBC0 File Offset: 0x001BDDC0
		protected override void OnStoreContentChanged(StoreContentChangedEventArgs e)
		{
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				this._dirty = true;
			}
			base.OnStoreContentChanged(e);
		}

		// Token: 0x0600637F RID: 25471 RVA: 0x001BFC08 File Offset: 0x001BDE08
		private List<Guid> FindAnnotationIds(string queryExpression)
		{
			Invariant.Assert(queryExpression != null && queryExpression.Length > 0, "Invalid query expression");
			List<Guid> list = null;
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				this.CheckStatus();
				XPathNavigator xpathNavigator = this._document.CreateNavigator();
				XPathNodeIterator xpathNodeIterator = xpathNavigator.Select(queryExpression, this._namespaceManager);
				if (xpathNodeIterator != null && xpathNodeIterator.Count > 0)
				{
					list = new List<Guid>(xpathNodeIterator.Count);
					using (IEnumerator enumerator = xpathNodeIterator.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							XPathNavigator xpathNavigator2 = (XPathNavigator)obj;
							string attribute = xpathNavigator2.GetAttribute("Id", "");
							if (string.IsNullOrEmpty(attribute))
							{
								throw new XmlException(SR.Get("RequiredAttributeMissing", new object[]
								{
									"Id",
									"Annotation"
								}));
							}
							Guid item;
							try
							{
								item = XmlConvert.ToGuid(attribute);
							}
							catch (FormatException innerException)
							{
								throw new InvalidOperationException(SR.Get("CannotParseId"), innerException);
							}
							list.Add(item);
						}
						return list;
					}
				}
				list = new List<Guid>(0);
			}
			return list;
		}

		// Token: 0x06006380 RID: 25472 RVA: 0x001BFD64 File Offset: 0x001BDF64
		private void HandleAuthorChanged(object sender, AnnotationAuthorChangedEventArgs e)
		{
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				this._dirty = true;
			}
			this.OnAuthorChanged(e);
		}

		// Token: 0x06006381 RID: 25473 RVA: 0x001BFDAC File Offset: 0x001BDFAC
		private void HandleAnchorChanged(object sender, AnnotationResourceChangedEventArgs e)
		{
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				this._dirty = true;
			}
			this.OnAnchorChanged(e);
		}

		// Token: 0x06006382 RID: 25474 RVA: 0x001BFDF4 File Offset: 0x001BDFF4
		private void HandleCargoChanged(object sender, AnnotationResourceChangedEventArgs e)
		{
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				this._dirty = true;
			}
			this.OnCargoChanged(e);
		}

		// Token: 0x06006383 RID: 25475 RVA: 0x001BFE3C File Offset: 0x001BE03C
		private IList<Annotation> MergeAndCacheAnnotations(Dictionary<Guid, Annotation> mapAnnotations, List<Guid> storeAnnotationsId)
		{
			List<Annotation> list = new List<Annotation>(mapAnnotations.Values);
			foreach (Guid guid in storeAnnotationsId)
			{
				Annotation annotation;
				if (!mapAnnotations.TryGetValue(guid, out annotation))
				{
					annotation = this.GetAnnotation(guid);
					list.Add(annotation);
				}
			}
			return list;
		}

		// Token: 0x06006384 RID: 25476 RVA: 0x001BFEB0 File Offset: 0x001BE0B0
		private IList<Annotation> InternalGetAnnotations(string query, ContentLocator anchorLocator)
		{
			Invariant.Assert(query != null, "Parameter 'query' is null.");
			object syncRoot = base.SyncRoot;
			IList<Annotation> result;
			lock (syncRoot)
			{
				this.CheckStatus();
				List<Guid> storeAnnotationsId = this.FindAnnotationIds(query);
				Dictionary<Guid, Annotation> mapAnnotations;
				if (anchorLocator == null)
				{
					mapAnnotations = this._storeAnnotationsMap.FindAnnotations();
				}
				else
				{
					mapAnnotations = this._storeAnnotationsMap.FindAnnotations(anchorLocator);
				}
				result = this.MergeAndCacheAnnotations(mapAnnotations, storeAnnotationsId);
			}
			return result;
		}

		// Token: 0x06006385 RID: 25477 RVA: 0x001BFF34 File Offset: 0x001BE134
		private void LoadStream(IDictionary<Uri, IList<Uri>> knownNamespaces)
		{
			this.CheckKnownNamespaces(knownNamespaces);
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				this._document = new XmlDocument();
				this._document.PreserveWhitespace = false;
				if (this._stream.Length == 0L)
				{
					this._document.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?> <anc:Annotations xmlns:anc=\"http://schemas.microsoft.com/windows/annotations/2003/11/core\" xmlns:anb=\"http://schemas.microsoft.com/windows/annotations/2003/11/base\" />");
				}
				else
				{
					this._xmlCompatibilityReader = this.SetupReader(knownNamespaces);
					this._document.Load(this._xmlCompatibilityReader);
				}
				this._namespaceManager = new XmlNamespaceManager(this._document.NameTable);
				this._namespaceManager.AddNamespace("anc", "http://schemas.microsoft.com/windows/annotations/2003/11/core");
				this._namespaceManager.AddNamespace("anb", "http://schemas.microsoft.com/windows/annotations/2003/11/base");
				XPathNavigator xpathNavigator = this._document.CreateNavigator();
				XPathNodeIterator xpathNodeIterator = xpathNavigator.Select("//anc:Annotations", this._namespaceManager);
				Invariant.Assert(xpathNodeIterator.Count == 1, "More than one annotation returned for the query");
				xpathNodeIterator.MoveNext();
				this._rootNavigator = xpathNodeIterator.Current;
			}
		}

		// Token: 0x06006386 RID: 25478 RVA: 0x001C0050 File Offset: 0x001BE250
		private void CheckKnownNamespaces(IDictionary<Uri, IList<Uri>> knownNamespaces)
		{
			if (knownNamespaces == null)
			{
				return;
			}
			IList<Uri> list = new List<Uri>();
			foreach (Uri item in XmlStreamStore._predefinedNamespaces.Keys)
			{
				list.Add(item);
			}
			foreach (Uri uri in knownNamespaces.Keys)
			{
				if (uri == null)
				{
					throw new ArgumentException(SR.Get("NullUri"), "knownNamespaces");
				}
				if (list.Contains(uri))
				{
					throw new ArgumentException(SR.Get("DuplicatedUri"), "knownNamespaces");
				}
				list.Add(uri);
			}
			foreach (KeyValuePair<Uri, IList<Uri>> keyValuePair in knownNamespaces)
			{
				if (keyValuePair.Value != null)
				{
					foreach (Uri uri2 in keyValuePair.Value)
					{
						if (uri2 == null)
						{
							throw new ArgumentException(SR.Get("NullUri"), "knownNamespaces");
						}
						if (list.Contains(uri2))
						{
							throw new ArgumentException(SR.Get("DuplicatedCompatibleUri"), "knownNamespaces");
						}
						list.Add(uri2);
					}
				}
			}
		}

		// Token: 0x06006387 RID: 25479 RVA: 0x001C01F4 File Offset: 0x001BE3F4
		private XmlCompatibilityReader SetupReader(IDictionary<Uri, IList<Uri>> knownNamespaces)
		{
			IList<string> list = new List<string>();
			foreach (Uri uri in XmlStreamStore._predefinedNamespaces.Keys)
			{
				list.Add(uri.ToString());
			}
			if (knownNamespaces != null)
			{
				foreach (Uri uri2 in knownNamespaces.Keys)
				{
					list.Add(uri2.ToString());
				}
			}
			XmlCompatibilityReader xmlCompatibilityReader = new XmlCompatibilityReader(new XmlTextReader(this._stream), new IsXmlNamespaceSupportedCallback(this.IsXmlNamespaceSupported), list);
			if (knownNamespaces != null)
			{
				foreach (KeyValuePair<Uri, IList<Uri>> keyValuePair in knownNamespaces)
				{
					if (keyValuePair.Value != null)
					{
						foreach (Uri uri3 in keyValuePair.Value)
						{
							xmlCompatibilityReader.DeclareNamespaceCompatibility(keyValuePair.Key.ToString(), uri3.ToString());
						}
					}
				}
			}
			this._ignoredNamespaces.Clear();
			return xmlCompatibilityReader;
		}

		// Token: 0x06006388 RID: 25480 RVA: 0x001C0364 File Offset: 0x001BE564
		private bool IsXmlNamespaceSupported(string xmlNamespace, out string newXmlNamespace)
		{
			if (!string.IsNullOrEmpty(xmlNamespace))
			{
				if (!Uri.IsWellFormedUriString(xmlNamespace, UriKind.RelativeOrAbsolute))
				{
					throw new ArgumentException(SR.Get("InvalidNamespace", new object[]
					{
						xmlNamespace
					}), "xmlNamespace");
				}
				Uri item = new Uri(xmlNamespace, UriKind.RelativeOrAbsolute);
				if (!this._ignoredNamespaces.Contains(item))
				{
					this._ignoredNamespaces.Add(item);
				}
			}
			newXmlNamespace = null;
			return false;
		}

		// Token: 0x06006389 RID: 25481 RVA: 0x001C03C8 File Offset: 0x001BE5C8
		private XPathNavigator GetAnnotationNodeForId(Guid id)
		{
			XPathNavigator result = null;
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				XPathNavigator xpathNavigator = this._document.CreateNavigator();
				XPathNodeIterator xpathNodeIterator = xpathNavigator.Select("//anc:Annotation[@Id=\"" + XmlConvert.ToString(id) + "\"]", this._namespaceManager);
				if (xpathNodeIterator.MoveNext())
				{
					result = xpathNodeIterator.Current;
				}
			}
			return result;
		}

		// Token: 0x0600638A RID: 25482 RVA: 0x001C0448 File Offset: 0x001BE648
		private void CheckStatus()
		{
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				if (base.IsDisposed)
				{
					throw new ObjectDisposedException(null, SR.Get("ObjectDisposed_StoreClosed"));
				}
				if (this._stream == null)
				{
					throw new InvalidOperationException(SR.Get("StreamNotSet"));
				}
			}
		}

		// Token: 0x0600638B RID: 25483 RVA: 0x001C04B4 File Offset: 0x001BE6B4
		private void SerializeAnnotations()
		{
			List<Annotation> list = this._storeAnnotationsMap.FindDirtyAnnotations();
			foreach (Annotation annotation in list)
			{
				XPathNavigator xpathNavigator = this.GetAnnotationNodeForId(annotation.Id);
				if (xpathNavigator == null)
				{
					xpathNavigator = this._rootNavigator.CreateNavigator();
					XmlWriter xmlWriter = xpathNavigator.AppendChild();
					XmlStreamStore._serializer.Serialize(xmlWriter, annotation);
					xmlWriter.Close();
				}
				else
				{
					XmlWriter xmlWriter2 = xpathNavigator.InsertBefore();
					XmlStreamStore._serializer.Serialize(xmlWriter2, annotation);
					xmlWriter2.Close();
					xpathNavigator.DeleteSelf();
				}
			}
			this._storeAnnotationsMap.ValidateDirtyAnnotations();
		}

		// Token: 0x0600638C RID: 25484 RVA: 0x001C0570 File Offset: 0x001BE770
		private void Cleanup()
		{
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				this._xmlCompatibilityReader = null;
				this._ignoredNamespaces = null;
				this._stream = null;
				this._document = null;
				this._rootNavigator = null;
				this._storeAnnotationsMap = null;
			}
		}

		// Token: 0x0600638D RID: 25485 RVA: 0x001C05D4 File Offset: 0x001BE7D4
		private void SetStream(Stream stream, IDictionary<Uri, IList<Uri>> knownNamespaces)
		{
			try
			{
				object syncRoot = base.SyncRoot;
				lock (syncRoot)
				{
					this._storeAnnotationsMap = new StoreAnnotationsMap(new AnnotationAuthorChangedEventHandler(this.HandleAuthorChanged), new AnnotationResourceChangedEventHandler(this.HandleAnchorChanged), new AnnotationResourceChangedEventHandler(this.HandleCargoChanged));
					this._stream = stream;
					this.LoadStream(knownNamespaces);
				}
			}
			catch
			{
				this.Cleanup();
				throw;
			}
		}

		// Token: 0x040031DA RID: 12762
		private bool _dirty;

		// Token: 0x040031DB RID: 12763
		private bool _autoFlush;

		// Token: 0x040031DC RID: 12764
		private XmlDocument _document;

		// Token: 0x040031DD RID: 12765
		private XmlNamespaceManager _namespaceManager;

		// Token: 0x040031DE RID: 12766
		private Stream _stream;

		// Token: 0x040031DF RID: 12767
		private XPathNavigator _rootNavigator;

		// Token: 0x040031E0 RID: 12768
		private StoreAnnotationsMap _storeAnnotationsMap;

		// Token: 0x040031E1 RID: 12769
		private List<Uri> _ignoredNamespaces = new List<Uri>();

		// Token: 0x040031E2 RID: 12770
		private XmlCompatibilityReader _xmlCompatibilityReader;

		// Token: 0x040031E3 RID: 12771
		private static readonly Dictionary<Uri, IList<Uri>> _predefinedNamespaces;

		// Token: 0x040031E4 RID: 12772
		private static readonly Serializer _serializer = new Serializer(typeof(Annotation));
	}
}
