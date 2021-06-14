using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Text;
using System.Windows.Baml2006;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xaml;
using System.Xaml.Permissions;
using System.Xml;
using MS.Internal;
using MS.Internal.Utility;
using MS.Internal.WindowsBase;
using MS.Internal.Xaml.Context;
using MS.Utility;
using MS.Win32;

namespace System.Windows.Markup
{
	/// <summary>Reads XAML input and creates an object graph, using the WPF default XAML reader and an associated XAML object writer. </summary>
	// Token: 0x0200026E RID: 622
	public class XamlReader
	{
		/// <summary>Reads the XAML input in the specified text string and returns an object that corresponds to the root of the specified markup.</summary>
		/// <param name="xamlText">The input XAML, as a single text string.</param>
		/// <returns>The root of the created object tree.</returns>
		// Token: 0x06002382 RID: 9090 RVA: 0x000AD7A8 File Offset: 0x000AB9A8
		public static object Parse(string xamlText)
		{
			StringReader input = new StringReader(xamlText);
			XmlReader reader = XmlReader.Create(input);
			return XamlReader.Load(reader);
		}

		/// <summary>Reads the XAML markup in the specified text string (using a specified <see cref="T:System.Windows.Markup.ParserContext" />) and returns an object that corresponds to the root of the specified markup.</summary>
		/// <param name="xamlText">The input XAML, as a single text string.</param>
		/// <param name="parserContext">Context information used by the parser.</param>
		/// <returns>The root of the created object tree.</returns>
		// Token: 0x06002383 RID: 9091 RVA: 0x000AD7CC File Offset: 0x000AB9CC
		public static object Parse(string xamlText, ParserContext parserContext)
		{
			Stream stream = new MemoryStream(Encoding.Default.GetBytes(xamlText));
			return XamlReader.Load(stream, parserContext);
		}

		/// <summary>Reads the XAML input in the specified <see cref="T:System.IO.Stream" /> and returns an <see cref="T:System.Object" /> that is the root of the corresponding object tree.</summary>
		/// <param name="stream">The XAML to load, in stream form.</param>
		/// <returns>The object at the root of the created object tree.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="stream" /> is <see langword="null" />.</exception>
		// Token: 0x06002384 RID: 9092 RVA: 0x000AD7F1 File Offset: 0x000AB9F1
		public static object Load(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			return XamlReader.Load(stream, null);
		}

		/// <summary>Reads the XAML input in the specified <see cref="T:System.Xml.XmlReader" /> and returns an object that is the root of the corresponding object tree.</summary>
		/// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> that has already loaded the XAML input to load in XML form.</param>
		/// <returns>The object that is the root of the created object tree.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="reader" /> is <see langword="null" />.</exception>
		// Token: 0x06002385 RID: 9093 RVA: 0x000AD808 File Offset: 0x000ABA08
		public static object Load(XmlReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			return XamlReader.Load(reader, null, XamlParseMode.Synchronous);
		}

		/// <summary>Reads the XAML input in the specified <see cref="T:System.IO.Stream" /> and returns an object that is the root of the corresponding object tree.</summary>
		/// <param name="stream">The stream that contains the XAML input to load.</param>
		/// <param name="parserContext">Context information used by the parser.</param>
		/// <returns>The object that is the root of the created object tree.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="stream" /> is <see langword="null" />.-or-
		///         <paramref name="parserContext" /> is <see langword="null" />.</exception>
		// Token: 0x06002386 RID: 9094 RVA: 0x000AD820 File Offset: 0x000ABA20
		public static object Load(Stream stream, ParserContext parserContext)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (parserContext == null)
			{
				parserContext = new ParserContext();
			}
			return XamlReader.Load(stream, parserContext, false);
		}

		/// <summary>Reads the XAML input in the specified <see cref="T:System.IO.Stream" /> and returns the root of the corresponding object tree.</summary>
		/// <param name="stream">The stream containing the XAML input to load.</param>
		/// <returns>The object that is the root of the created object tree.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="stream" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">Multiple load operations are pending concurrently with the same <see cref="T:System.Windows.Markup.XamlReader" />.</exception>
		// Token: 0x06002387 RID: 9095 RVA: 0x000AD842 File Offset: 0x000ABA42
		public object LoadAsync(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._stream = stream;
			if (this._objectWriter != null)
			{
				throw new InvalidOperationException(SR.Get("ParserCannotReuseXamlReader"));
			}
			return this.LoadAsync(stream, null);
		}

		/// <summary>Reads the XAML input in the specified <see cref="T:System.Xml.XmlReader" /> and returns the root of the corresponding object tree. </summary>
		/// <param name="reader">An existing  <see cref="T:System.Xml.XmlReader" /> that has already loaded/read the XAML input.</param>
		/// <returns>The root of the created object tree.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="reader" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">Multiple load operations are performed concurrently with the same <see cref="T:System.Windows.Markup.XamlReader" />.</exception>
		// Token: 0x06002388 RID: 9096 RVA: 0x000AD879 File Offset: 0x000ABA79
		public object LoadAsync(XmlReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			return this.LoadAsync(reader, null);
		}

		/// <summary>Reads the XAML input in the specified <see cref="T:System.IO.Stream" /> and returns the root of the corresponding object tree. </summary>
		/// <param name="stream">A stream containing the XAML input to load.</param>
		/// <param name="parserContext">Context information used by the parser.</param>
		/// <returns>The root of the created object tree.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="stream" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">Multiple load operations are performed concurrently with the same <see cref="T:System.Windows.Markup.XamlReader" />.</exception>
		// Token: 0x06002389 RID: 9097 RVA: 0x000AD894 File Offset: 0x000ABA94
		public object LoadAsync(Stream stream, ParserContext parserContext)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._stream = stream;
			if (this._objectWriter != null)
			{
				throw new InvalidOperationException(SR.Get("ParserCannotReuseXamlReader"));
			}
			if (parserContext == null)
			{
				parserContext = new ParserContext();
			}
			return this.LoadAsync(new XmlTextReader(stream, XmlNodeType.Document, parserContext)
			{
				DtdProcessing = DtdProcessing.Prohibit
			}, parserContext);
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x000AD8F8 File Offset: 0x000ABAF8
		internal static bool ShouldReWrapException(Exception e, Uri baseUri)
		{
			XamlParseException ex = e as XamlParseException;
			return ex == null || (ex.BaseUri == null && baseUri != null);
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x000AD928 File Offset: 0x000ABB28
		private object LoadAsync(XmlReader reader, ParserContext parserContext)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (parserContext == null)
			{
				parserContext = new ParserContext();
			}
			this._xmlReader = reader;
			object rootObject = null;
			if (parserContext.BaseUri == null || string.IsNullOrEmpty(parserContext.BaseUri.ToString()))
			{
				if (reader.BaseURI == null || string.IsNullOrEmpty(reader.BaseURI.ToString()))
				{
					parserContext.BaseUri = BaseUriHelper.PackAppBaseUri;
				}
				else
				{
					parserContext.BaseUri = new Uri(reader.BaseURI);
				}
			}
			this._baseUri = parserContext.BaseUri;
			XamlXmlReaderSettings xamlXmlReaderSettings = new XamlXmlReaderSettings();
			xamlXmlReaderSettings.IgnoreUidsOnPropertyElements = true;
			xamlXmlReaderSettings.BaseUri = parserContext.BaseUri;
			xamlXmlReaderSettings.ProvideLineInfo = true;
			XamlSchemaContext schemaContext = (parserContext.XamlTypeMapper != null) ? parserContext.XamlTypeMapper.SchemaContext : XamlReader.GetWpfSchemaContext();
			try
			{
				this._textReader = new XamlXmlReader(reader, schemaContext, xamlXmlReaderSettings);
				this._stack = new XamlContextStack<WpfXamlFrame>(() => new WpfXamlFrame());
				XamlObjectWriterSettings xamlObjectWriterSettings = XamlReader.CreateObjectWriterSettings();
				xamlObjectWriterSettings.AfterBeginInitHandler = delegate(object sender, XamlObjectEventArgs args)
				{
					if (rootObject == null)
					{
						rootObject = args.Instance;
						this._styleConnector = (rootObject as IStyleConnector);
					}
					UIElement uielement = args.Instance as UIElement;
					if (uielement != null)
					{
						UIElement uielement2 = uielement;
						XamlReader <>4__this = this;
						int persistId = this._persistId;
						<>4__this._persistId = persistId + 1;
						uielement2.SetPersistId(persistId);
					}
					DependencyObject dependencyObject = args.Instance as DependencyObject;
					if (dependencyObject != null && this._stack.CurrentFrame.XmlnsDictionary != null)
					{
						XmlnsDictionary xmlnsDictionary = this._stack.CurrentFrame.XmlnsDictionary;
						xmlnsDictionary.Seal();
						XmlAttributeProperties.SetXmlnsDictionary(dependencyObject, xmlnsDictionary);
					}
				};
				this._objectWriter = new XamlObjectWriter(this._textReader.SchemaContext, xamlObjectWriterSettings);
				this._parseCancelled = false;
				this._skipJournaledProperties = parserContext.SkipJournaledProperties;
				XamlMember xamlDirective = this._textReader.SchemaContext.GetXamlDirective("http://schemas.microsoft.com/winfx/2006/xaml", "SynchronousMode");
				XamlMember xamlDirective2 = this._textReader.SchemaContext.GetXamlDirective("http://schemas.microsoft.com/winfx/2006/xaml", "AsyncRecords");
				XamlReader textReader = this._textReader;
				IXamlLineInfo xamlLineInfo = textReader as IXamlLineInfo;
				IXamlLineInfoConsumer objectWriter = this._objectWriter;
				bool shouldPassLineNumberInfo = false;
				if (xamlLineInfo != null && xamlLineInfo.HasLineInfo && objectWriter != null && objectWriter.ShouldProvideLineInfo)
				{
					shouldPassLineNumberInfo = true;
				}
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				while (!this._textReader.IsEof)
				{
					WpfXamlLoader.TransformNodes(textReader, this._objectWriter, true, this._skipJournaledProperties, shouldPassLineNumberInfo, xamlLineInfo, objectWriter, this._stack, this._styleConnector);
					if (textReader.NodeType == XamlNodeType.StartMember)
					{
						if (textReader.Member == xamlDirective)
						{
							flag2 = true;
						}
						else if (textReader.Member == xamlDirective2)
						{
							flag3 = true;
						}
					}
					else if (textReader.NodeType == XamlNodeType.Value)
					{
						if (flag2)
						{
							if (textReader.Value as string == "Async")
							{
								flag = true;
							}
						}
						else if (flag3)
						{
							if (textReader.Value is int)
							{
								this._maxAsynxRecords = (int)textReader.Value;
							}
							else if (textReader.Value is string)
							{
								this._maxAsynxRecords = int.Parse(textReader.Value as string, TypeConverterHelper.InvariantEnglishUS);
							}
						}
					}
					else if (textReader.NodeType == XamlNodeType.EndMember)
					{
						flag2 = false;
						flag3 = false;
					}
					if (flag && rootObject != null)
					{
						break;
					}
				}
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || !XamlReader.ShouldReWrapException(ex, parserContext.BaseUri))
				{
					throw;
				}
				XamlReader.RewrapException(ex, parserContext.BaseUri);
			}
			if (!this._textReader.IsEof)
			{
				this.Post();
			}
			else
			{
				this.TreeBuildComplete();
			}
			if (rootObject is DependencyObject)
			{
				if (parserContext.BaseUri != null && !string.IsNullOrEmpty(parserContext.BaseUri.ToString()))
				{
					(rootObject as DependencyObject).SetValue(BaseUriHelper.BaseUriProperty, parserContext.BaseUri);
				}
				WpfXamlLoader.EnsureXmlNamespaceMaps(rootObject, schemaContext);
			}
			Application application = rootObject as Application;
			if (application != null)
			{
				application.ApplicationMarkupBaseUri = XamlReader.GetBaseUri(xamlXmlReaderSettings.BaseUri);
			}
			return rootObject;
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x000ADCF0 File Offset: 0x000ABEF0
		internal static void RewrapException(Exception e, Uri baseUri)
		{
			XamlReader.RewrapException(e, null, baseUri);
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x000ADCFA File Offset: 0x000ABEFA
		internal static void RewrapException(Exception e, IXamlLineInfo lineInfo, Uri baseUri)
		{
			throw XamlReader.WrapException(e, lineInfo, baseUri);
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x000ADD04 File Offset: 0x000ABF04
		internal static XamlParseException WrapException(Exception e, IXamlLineInfo lineInfo, Uri baseUri)
		{
			Exception ex = (e.InnerException == null) ? e : e.InnerException;
			if (ex is XamlParseException)
			{
				XamlParseException ex2 = (XamlParseException)ex;
				ex2.BaseUri = (ex2.BaseUri ?? baseUri);
				if (lineInfo != null && ex2.LinePosition == 0 && ex2.LineNumber == 0)
				{
					ex2.LinePosition = lineInfo.LinePosition;
					ex2.LineNumber = lineInfo.LineNumber;
				}
				return ex2;
			}
			if (e is XamlException)
			{
				XamlException ex3 = (XamlException)e;
				return new XamlParseException(ex3.Message, ex3.LineNumber, ex3.LinePosition, baseUri, ex);
			}
			if (e is XmlException)
			{
				XmlException ex4 = (XmlException)e;
				return new XamlParseException(ex4.Message, ex4.LineNumber, ex4.LinePosition, baseUri, ex);
			}
			if (lineInfo != null)
			{
				return new XamlParseException(e.Message, lineInfo.LineNumber, lineInfo.LinePosition, baseUri, ex);
			}
			return new XamlParseException(e.Message, ex);
		}

		// Token: 0x0600238F RID: 9103 RVA: 0x000ADDEB File Offset: 0x000ABFEB
		internal void Post()
		{
			this.Post(DispatcherPriority.Background);
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x000ADDF4 File Offset: 0x000ABFF4
		internal void Post(DispatcherPriority priority)
		{
			DispatcherOperationCallback method = new DispatcherOperationCallback(this.Dispatch);
			Dispatcher.CurrentDispatcher.BeginInvoke(priority, method, this);
		}

		// Token: 0x06002391 RID: 9105 RVA: 0x000ADE1C File Offset: 0x000AC01C
		private object Dispatch(object o)
		{
			this.DispatchParserQueueEvent((XamlReader)o);
			return null;
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x000ADE2B File Offset: 0x000AC02B
		private void DispatchParserQueueEvent(XamlReader xamlReader)
		{
			xamlReader.HandleAsyncQueueItem();
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x000ADE34 File Offset: 0x000AC034
		internal virtual void HandleAsyncQueueItem()
		{
			try
			{
				int num = SafeNativeMethods.GetTickCount();
				int num2 = this._maxAsynxRecords;
				XamlReader textReader = this._textReader;
				IXamlLineInfo xamlLineInfo = textReader as IXamlLineInfo;
				IXamlLineInfoConsumer objectWriter = this._objectWriter;
				bool shouldPassLineNumberInfo = false;
				if (xamlLineInfo != null && xamlLineInfo.HasLineInfo && objectWriter != null && objectWriter.ShouldProvideLineInfo)
				{
					shouldPassLineNumberInfo = true;
				}
				XamlMember xamlDirective = this._textReader.SchemaContext.GetXamlDirective("http://schemas.microsoft.com/winfx/2006/xaml", "AsyncRecords");
				while (!textReader.IsEof && !this._parseCancelled)
				{
					WpfXamlLoader.TransformNodes(textReader, this._objectWriter, true, this._skipJournaledProperties, shouldPassLineNumberInfo, xamlLineInfo, objectWriter, this._stack, this._styleConnector);
					if (textReader.NodeType == XamlNodeType.Value && this._stack.CurrentFrame.Property == xamlDirective)
					{
						if (textReader.Value is int)
						{
							this._maxAsynxRecords = (int)textReader.Value;
						}
						else if (textReader.Value is string)
						{
							this._maxAsynxRecords = int.Parse(textReader.Value as string, TypeConverterHelper.InvariantEnglishUS);
						}
						num2 = this._maxAsynxRecords;
					}
					int num3 = SafeNativeMethods.GetTickCount() - num;
					if (num3 < 0)
					{
						num = 0;
					}
					else if (num3 > 200)
					{
						break;
					}
					if (--num2 == 0)
					{
						break;
					}
				}
			}
			catch (XamlParseException parseException)
			{
				this._parseException = parseException;
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || !XamlReader.ShouldReWrapException(ex, this._baseUri))
				{
					this._parseException = ex;
				}
				else
				{
					this._parseException = XamlReader.WrapException(ex, null, this._baseUri);
				}
			}
			finally
			{
				if (this._parseException != null || this._parseCancelled)
				{
					this.TreeBuildComplete();
				}
				else if (!this._textReader.IsEof)
				{
					this.Post();
				}
				else
				{
					this.TreeBuildComplete();
				}
			}
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x000AE038 File Offset: 0x000AC238
		internal void TreeBuildComplete()
		{
			if (this.LoadCompleted != null)
			{
				Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(delegate(object obj)
				{
					this.LoadCompleted(this, new AsyncCompletedEventArgs(this._parseException, this._parseCancelled, null));
					return null;
				}), null);
			}
			this._xmlReader.Close();
			this._objectWriter = null;
			this._stream = null;
			this._textReader = null;
			this._stack = null;
		}

		/// <summary>Aborts the current asynchronous load operation, if there is an asynchronous load operation pending.</summary>
		// Token: 0x06002395 RID: 9109 RVA: 0x000AE08E File Offset: 0x000AC28E
		public void CancelAsync()
		{
			this._parseCancelled = true;
		}

		/// <summary>Occurs when an asynchronous load operation completes. </summary>
		// Token: 0x14000056 RID: 86
		// (add) Token: 0x06002396 RID: 9110 RVA: 0x000AE098 File Offset: 0x000AC298
		// (remove) Token: 0x06002397 RID: 9111 RVA: 0x000AE0D0 File Offset: 0x000AC2D0
		public event AsyncCompletedEventHandler LoadCompleted;

		// Token: 0x06002398 RID: 9112 RVA: 0x000AE108 File Offset: 0x000AC308
		internal static XamlObjectWriterSettings CreateObjectWriterSettings()
		{
			return new XamlObjectWriterSettings
			{
				IgnoreCanConvert = true,
				PreferUnconvertedDictionaryKeys = true
			};
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x000AE12C File Offset: 0x000AC32C
		internal static XamlObjectWriterSettings CreateObjectWriterSettings(XamlObjectWriterSettings parentSettings)
		{
			XamlObjectWriterSettings xamlObjectWriterSettings = XamlReader.CreateObjectWriterSettings();
			if (parentSettings != null)
			{
				xamlObjectWriterSettings.SkipDuplicatePropertyCheck = parentSettings.SkipDuplicatePropertyCheck;
				xamlObjectWriterSettings.AccessLevel = parentSettings.AccessLevel;
				xamlObjectWriterSettings.SkipProvideValueOnRoot = parentSettings.SkipProvideValueOnRoot;
				xamlObjectWriterSettings.SourceBamlUri = parentSettings.SourceBamlUri;
			}
			return xamlObjectWriterSettings;
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x000AE174 File Offset: 0x000AC374
		internal static XamlObjectWriterSettings CreateObjectWriterSettingsForBaml()
		{
			XamlObjectWriterSettings xamlObjectWriterSettings = XamlReader.CreateObjectWriterSettings();
			xamlObjectWriterSettings.SkipDuplicatePropertyCheck = true;
			return xamlObjectWriterSettings;
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x000AE190 File Offset: 0x000AC390
		internal static Baml2006ReaderSettings CreateBamlReaderSettings()
		{
			return new Baml2006ReaderSettings
			{
				IgnoreUidsOnPropertyElements = true
			};
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x000AE1AC File Offset: 0x000AC3AC
		internal static XamlSchemaContextSettings CreateSchemaContextSettings()
		{
			return new XamlSchemaContextSettings
			{
				SupportMarkupExtensionsWithDuplicateArity = true
			};
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x0600239D RID: 9117 RVA: 0x000AE1C7 File Offset: 0x000AC3C7
		internal static WpfSharedBamlSchemaContext BamlSharedSchemaContext
		{
			get
			{
				return XamlReader._bamlSharedContext.Value;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x0600239E RID: 9118 RVA: 0x000AE1D3 File Offset: 0x000AC3D3
		internal static WpfSharedBamlSchemaContext XamlV3SharedSchemaContext
		{
			get
			{
				return XamlReader._xamlV3SharedContext.Value;
			}
		}

		/// <summary>Returns a <see cref="T:System.Xaml.XamlSchemaContext" /> object that represents the WPF schema context settings for a <see cref="T:System.Windows.Markup.XamlReader" />.</summary>
		/// <returns>A <see cref="T:System.Xaml.XamlSchemaContext" /> object that represents the WPF schema context settings for a <see cref="T:System.Windows.Markup.XamlReader" />.</returns>
		// Token: 0x0600239F RID: 9119 RVA: 0x000AE1DF File Offset: 0x000AC3DF
		public static XamlSchemaContext GetWpfSchemaContext()
		{
			return XamlReader._xamlSharedContext.Value;
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x000AE1EC File Offset: 0x000AC3EC
		internal static object Load(Stream stream, ParserContext parserContext, bool useRestrictiveXamlReader)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (parserContext == null)
			{
				parserContext = new ParserContext();
			}
			XmlReader reader = XmlReader.Create(stream, null, parserContext);
			object result = XamlReader.Load(reader, parserContext, XamlParseMode.Synchronous, useRestrictiveXamlReader);
			stream.Close();
			return result;
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x000AE230 File Offset: 0x000AC430
		internal static object Load(XmlReader reader, bool useRestrictiveXamlReader = false)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			return XamlReader.Load(reader, null, XamlParseMode.Synchronous, useRestrictiveXamlReader);
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x000AE249 File Offset: 0x000AC449
		internal static object Load(XmlReader reader, ParserContext parserContext, XamlParseMode parseMode)
		{
			return XamlReader.Load(reader, parserContext, parseMode, false);
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x000AE254 File Offset: 0x000AC454
		internal static object Load(XmlReader reader, ParserContext parserContext, XamlParseMode parseMode, bool useRestrictiveXamlReader)
		{
			return XamlReader.Load(reader, parserContext, parseMode, useRestrictiveXamlReader, null);
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x000AE260 File Offset: 0x000AC460
		internal static object Load(XmlReader reader, ParserContext parserContext, XamlParseMode parseMode, bool useRestrictiveXamlReader, List<Type> safeTypes)
		{
			if (parseMode == XamlParseMode.Uninitialized || parseMode == XamlParseMode.Asynchronous)
			{
				XamlReader xamlReader = new XamlReader();
				return xamlReader.LoadAsync(reader, parserContext);
			}
			if (parserContext == null)
			{
				parserContext = new ParserContext();
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Event.WClientParseXmlBegin, parserContext.BaseUri);
			if (TraceMarkup.IsEnabled)
			{
				TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.Load);
			}
			object obj = null;
			try
			{
				if (parserContext.BaseUri == null || string.IsNullOrEmpty(parserContext.BaseUri.ToString()))
				{
					if (reader.BaseURI == null || string.IsNullOrEmpty(reader.BaseURI.ToString()))
					{
						parserContext.BaseUri = BaseUriHelper.PackAppBaseUri;
					}
					else
					{
						parserContext.BaseUri = new Uri(reader.BaseURI);
					}
				}
				XamlXmlReaderSettings xamlXmlReaderSettings = new XamlXmlReaderSettings();
				xamlXmlReaderSettings.IgnoreUidsOnPropertyElements = true;
				xamlXmlReaderSettings.BaseUri = parserContext.BaseUri;
				xamlXmlReaderSettings.ProvideLineInfo = true;
				XamlSchemaContext schemaContext = (parserContext.XamlTypeMapper != null) ? parserContext.XamlTypeMapper.SchemaContext : XamlReader.GetWpfSchemaContext();
				XamlXmlReader xamlReader2 = useRestrictiveXamlReader ? new RestrictiveXamlXmlReader(reader, schemaContext, xamlXmlReaderSettings, safeTypes) : new XamlXmlReader(reader, schemaContext, xamlXmlReaderSettings);
				obj = XamlReader.Load(xamlReader2, parserContext);
				reader.Close();
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || !XamlReader.ShouldReWrapException(ex, parserContext.BaseUri))
				{
					throw;
				}
				XamlReader.RewrapException(ex, parserContext.BaseUri);
			}
			finally
			{
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.Load, obj);
				}
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Event.WClientParseXmlEnd, parserContext.BaseUri);
			}
			return obj;
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x000AE3E8 File Offset: 0x000AC5E8
		internal static object Load(XamlReader xamlReader, ParserContext parserContext)
		{
			if (parserContext == null)
			{
				parserContext = new ParserContext();
			}
			SecurityHelper.RunClassConstructor(typeof(Application));
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Event.WClientParseXamlBegin, parserContext.BaseUri);
			object obj = WpfXamlLoader.Load(xamlReader, parserContext.SkipJournaledProperties, parserContext.BaseUri);
			DependencyObject dependencyObject = obj as DependencyObject;
			if (dependencyObject != null && parserContext.BaseUri != null && !string.IsNullOrEmpty(parserContext.BaseUri.ToString()))
			{
				dependencyObject.SetValue(BaseUriHelper.BaseUriProperty, parserContext.BaseUri);
			}
			Application application = obj as Application;
			if (application != null)
			{
				application.ApplicationMarkupBaseUri = XamlReader.GetBaseUri(parserContext.BaseUri);
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Event.WClientParseXamlEnd, parserContext.BaseUri);
			return obj;
		}

		/// <summary>Reads the XAML input through a provided <see cref="T:System.Xaml.XamlReader" /> and returns an object that is the root of the corresponding object tree.</summary>
		/// <param name="reader">A <see cref="T:System.Xaml.XamlReader" /> object. This is expected to be initialized with input XAML.</param>
		/// <returns>The object that is the root of the created object tree.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="reader" /> is <see langword="null" />.</exception>
		// Token: 0x060023A6 RID: 9126 RVA: 0x000AE49C File Offset: 0x000AC69C
		public static object Load(XamlReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			object obj = null;
			try
			{
				obj = XamlReader.Load(reader, null);
			}
			catch (Exception ex)
			{
				IUriContext uriContext = reader as IUriContext;
				Uri baseUri = (uriContext != null) ? uriContext.BaseUri : null;
				if (CriticalExceptions.IsCriticalException(ex) || !XamlReader.ShouldReWrapException(ex, baseUri))
				{
					throw;
				}
				XamlReader.RewrapException(ex, baseUri);
			}
			finally
			{
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.Load, obj);
				}
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Event.WClientParseXmlEnd);
			}
			return obj;
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x000AE538 File Offset: 0x000AC738
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static object LoadBaml(Stream stream, ParserContext parserContext, object parent, bool closeStream)
		{
			object obj = null;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Event.WClientParseBamlBegin, parserContext.BaseUri);
			if (TraceMarkup.IsEnabled)
			{
				TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.Load);
			}
			try
			{
				IStreamInfo streamInfo = stream as IStreamInfo;
				if (streamInfo != null)
				{
					parserContext.StreamCreatedAssembly = streamInfo.Assembly;
				}
				Baml2006ReaderSettings baml2006ReaderSettings = XamlReader.CreateBamlReaderSettings();
				baml2006ReaderSettings.BaseUri = parserContext.BaseUri;
				baml2006ReaderSettings.LocalAssembly = streamInfo.Assembly;
				if (baml2006ReaderSettings.BaseUri == null || string.IsNullOrEmpty(baml2006ReaderSettings.BaseUri.ToString()))
				{
					baml2006ReaderSettings.BaseUri = BaseUriHelper.PackAppBaseUri;
				}
				Baml2006ReaderInternal xamlReader = new Baml2006ReaderInternal(stream, new Baml2006SchemaContext(baml2006ReaderSettings.LocalAssembly), baml2006ReaderSettings, parent);
				Type left = null;
				if (streamInfo.Assembly != null)
				{
					try
					{
						left = XamlTypeMapper.GetInternalTypeHelperTypeFromAssembly(parserContext);
					}
					catch (Exception ex)
					{
						if (CriticalExceptions.IsCriticalException(ex))
						{
							throw;
						}
					}
				}
				if (left != null)
				{
					XamlAccessLevel xamlAccessLevel = XamlAccessLevel.AssemblyAccessTo(streamInfo.Assembly);
					XamlLoadPermission xamlLoadPermission = new XamlLoadPermission(xamlAccessLevel);
					xamlLoadPermission.Assert();
					try
					{
						obj = WpfXamlLoader.LoadBaml(xamlReader, parserContext.SkipJournaledProperties, parent, xamlAccessLevel, parserContext.BaseUri);
						goto IL_122;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				obj = WpfXamlLoader.LoadBaml(xamlReader, parserContext.SkipJournaledProperties, parent, null, parserContext.BaseUri);
				IL_122:
				DependencyObject dependencyObject = obj as DependencyObject;
				if (dependencyObject != null)
				{
					dependencyObject.SetValue(BaseUriHelper.BaseUriProperty, baml2006ReaderSettings.BaseUri);
				}
				Application application = obj as Application;
				if (application != null)
				{
					application.ApplicationMarkupBaseUri = XamlReader.GetBaseUri(baml2006ReaderSettings.BaseUri);
				}
			}
			finally
			{
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.Load, obj);
				}
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Event.WClientParseBamlEnd, parserContext.BaseUri);
				if (closeStream && stream != null)
				{
					stream.Close();
				}
			}
			return obj;
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x000AE728 File Offset: 0x000AC928
		private static Uri GetBaseUri(Uri uri)
		{
			if (uri == null)
			{
				return BindUriHelper.BaseUri;
			}
			if (!uri.IsAbsoluteUri)
			{
				return new Uri(BindUriHelper.BaseUri, uri);
			}
			return uri;
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x000AE750 File Offset: 0x000AC950
		private static WpfSharedBamlSchemaContext CreateBamlSchemaContext()
		{
			return new WpfSharedBamlSchemaContext(new XamlSchemaContextSettings
			{
				SupportMarkupExtensionsWithDuplicateArity = true
			});
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x000AE770 File Offset: 0x000AC970
		private static WpfSharedXamlSchemaContext CreateXamlSchemaContext(bool useV3Rules)
		{
			return new WpfSharedXamlSchemaContext(new XamlSchemaContextSettings
			{
				SupportMarkupExtensionsWithDuplicateArity = true
			}, useV3Rules);
		}

		// Token: 0x04001AE6 RID: 6886
		private const int AsyncLoopTimeout = 200;

		// Token: 0x04001AE8 RID: 6888
		private Uri _baseUri;

		// Token: 0x04001AE9 RID: 6889
		private XamlReader _textReader;

		// Token: 0x04001AEA RID: 6890
		private XmlReader _xmlReader;

		// Token: 0x04001AEB RID: 6891
		private XamlObjectWriter _objectWriter;

		// Token: 0x04001AEC RID: 6892
		private Stream _stream;

		// Token: 0x04001AED RID: 6893
		private bool _parseCancelled;

		// Token: 0x04001AEE RID: 6894
		private Exception _parseException;

		// Token: 0x04001AEF RID: 6895
		private int _persistId = 1;

		// Token: 0x04001AF0 RID: 6896
		private bool _skipJournaledProperties;

		// Token: 0x04001AF1 RID: 6897
		private XamlContextStack<WpfXamlFrame> _stack;

		// Token: 0x04001AF2 RID: 6898
		private int _maxAsynxRecords = -1;

		// Token: 0x04001AF3 RID: 6899
		private IStyleConnector _styleConnector;

		// Token: 0x04001AF4 RID: 6900
		private static readonly Lazy<WpfSharedBamlSchemaContext> _bamlSharedContext = new Lazy<WpfSharedBamlSchemaContext>(() => XamlReader.CreateBamlSchemaContext());

		// Token: 0x04001AF5 RID: 6901
		private static readonly Lazy<WpfSharedXamlSchemaContext> _xamlSharedContext = new Lazy<WpfSharedXamlSchemaContext>(() => XamlReader.CreateXamlSchemaContext(false));

		// Token: 0x04001AF6 RID: 6902
		private static readonly Lazy<WpfSharedXamlSchemaContext> _xamlV3SharedContext = new Lazy<WpfSharedXamlSchemaContext>(() => XamlReader.CreateXamlSchemaContext(true));
	}
}
