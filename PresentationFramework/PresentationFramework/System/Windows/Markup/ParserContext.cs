using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Security;
using System.Xml;
using MS.Internal;
using MS.Internal.Xaml.Parser;

namespace System.Windows.Markup
{
	/// <summary>Provides context information required by a XAML parser. </summary>
	// Token: 0x02000223 RID: 547
	public class ParserContext : IUriContext
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.ParserContext" /> class. </summary>
		// Token: 0x060021CD RID: 8653 RVA: 0x000A9492 File Offset: 0x000A7692
		public ParserContext()
		{
			this.Initialize();
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x000A94B6 File Offset: 0x000A76B6
		internal void Initialize()
		{
			this._xmlnsDictionary = null;
			this._nameScopeStack = null;
			this._xmlLang = string.Empty;
			this._xmlSpace = string.Empty;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.ParserContext" /> class by using the specified <see cref="T:System.Xml.XmlParserContext" />.</summary>
		/// <param name="xmlParserContext">The XML processing context to base the new <see cref="T:System.Windows.Markup.ParserContext" /> on.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="xmlParserContext" /> is <see langword="null" />.</exception>
		// Token: 0x060021CF RID: 8655 RVA: 0x000A94DC File Offset: 0x000A76DC
		public ParserContext(XmlParserContext xmlParserContext)
		{
			if (xmlParserContext == null)
			{
				throw new ArgumentNullException("xmlParserContext");
			}
			this._xmlLang = xmlParserContext.XmlLang;
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(XmlSpace));
			if (converter != null)
			{
				this._xmlSpace = converter.ConvertToString(null, TypeConverterHelper.InvariantEnglishUS, xmlParserContext.XmlSpace);
			}
			else
			{
				this._xmlSpace = string.Empty;
			}
			this._xmlnsDictionary = new XmlnsDictionary();
			if (xmlParserContext.BaseURI != null && xmlParserContext.BaseURI.Length > 0)
			{
				this._baseUri = new Uri(xmlParserContext.BaseURI, UriKind.RelativeOrAbsolute);
			}
			XmlNamespaceManager namespaceManager = xmlParserContext.NamespaceManager;
			if (namespaceManager != null)
			{
				foreach (object obj in namespaceManager)
				{
					string prefix = (string)obj;
					this._xmlnsDictionary.Add(prefix, namespaceManager.LookupNamespace(prefix));
				}
			}
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x000A95F0 File Offset: 0x000A77F0
		internal ParserContext(XmlReader xmlReader)
		{
			if (xmlReader.BaseURI != null && xmlReader.BaseURI.Length != 0)
			{
				this.BaseUri = new Uri(xmlReader.BaseURI);
			}
			this.XmlLang = xmlReader.XmlLang;
			if (xmlReader.XmlSpace != System.Xml.XmlSpace.None)
			{
				this.XmlSpace = xmlReader.XmlSpace.ToString();
			}
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x000A9670 File Offset: 0x000A7870
		internal ParserContext(ParserContext parserContext)
		{
			this._xmlLang = parserContext.XmlLang;
			this._xmlSpace = parserContext.XmlSpace;
			this._xamlTypeMapper = parserContext.XamlTypeMapper;
			this._mapTable = parserContext.MapTable;
			this._baseUri = parserContext.BaseUri;
			this._masterBracketCharacterCache = parserContext.MasterBracketCharacterCache;
			this._rootElement = parserContext._rootElement;
			if (parserContext._nameScopeStack != null)
			{
				this._nameScopeStack = (Stack)parserContext._nameScopeStack.Clone();
			}
			else
			{
				this._nameScopeStack = null;
			}
			this._skipJournaledProperties = parserContext._skipJournaledProperties;
			this._xmlnsDictionary = null;
			if (parserContext._xmlnsDictionary != null && parserContext._xmlnsDictionary.Count > 0)
			{
				this._xmlnsDictionary = new XmlnsDictionary();
				XmlnsDictionary xmlnsDictionary = parserContext.XmlnsDictionary;
				if (xmlnsDictionary != null)
				{
					foreach (object obj in xmlnsDictionary.Keys)
					{
						string prefix = (string)obj;
						this._xmlnsDictionary[prefix] = xmlnsDictionary[prefix];
					}
				}
			}
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x000A97AC File Offset: 0x000A79AC
		internal Dictionary<string, SpecialBracketCharacters> InitBracketCharacterCacheForType(Type type)
		{
			if (!this.MasterBracketCharacterCache.ContainsKey(type))
			{
				Dictionary<string, SpecialBracketCharacters> value = this.BuildBracketCharacterCacheForType(type);
				this.MasterBracketCharacterCache.Add(type, value);
			}
			return this.MasterBracketCharacterCache[type];
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x000A97E8 File Offset: 0x000A79E8
		internal void PushScope()
		{
			this._repeat++;
			this._currentFreezeStackFrame.IncrementRepeatCount();
			if (this._xmlnsDictionary != null)
			{
				this._xmlnsDictionary.PushScope();
			}
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x000A9818 File Offset: 0x000A7A18
		internal void PopScope()
		{
			if (this._repeat > 0)
			{
				this._repeat--;
			}
			else if (this._langSpaceStack != null && this._langSpaceStack.Count > 0)
			{
				this._repeat = (int)this._langSpaceStack.Pop();
				this._targetType = (Type)this._langSpaceStack.Pop();
				this._xmlSpace = (string)this._langSpaceStack.Pop();
				this._xmlLang = (string)this._langSpaceStack.Pop();
			}
			if (!this._currentFreezeStackFrame.DecrementRepeatCount())
			{
				this._currentFreezeStackFrame = (ParserContext.FreezeStackFrame)this._freezeStack.Pop();
			}
			if (this._xmlnsDictionary != null)
			{
				this._xmlnsDictionary.PopScope();
			}
		}

		/// <summary>Gets the XAML namespace dictionary for this XAML parser context.</summary>
		/// <returns>The XAML namespace dictionary.</returns>
		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x060021D5 RID: 8661 RVA: 0x000A98E2 File Offset: 0x000A7AE2
		public XmlnsDictionary XmlnsDictionary
		{
			get
			{
				if (this._xmlnsDictionary == null)
				{
					this._xmlnsDictionary = new XmlnsDictionary();
				}
				return this._xmlnsDictionary;
			}
		}

		/// <summary>Gets or sets the <see langword="xml:lang" /> string for this context.</summary>
		/// <returns>The <see langword="xml:lang" /> string value.</returns>
		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x060021D6 RID: 8662 RVA: 0x000A98FD File Offset: 0x000A7AFD
		// (set) Token: 0x060021D7 RID: 8663 RVA: 0x000A9905 File Offset: 0x000A7B05
		public string XmlLang
		{
			get
			{
				return this._xmlLang;
			}
			set
			{
				this.EndRepeat();
				this._xmlLang = ((value == null) ? string.Empty : value);
			}
		}

		/// <summary>Gets or sets the character for <see langword="xml:space" /> or this context.</summary>
		/// <returns>The character for <see langword="xml:space" /> or this context.</returns>
		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x060021D8 RID: 8664 RVA: 0x000A991E File Offset: 0x000A7B1E
		// (set) Token: 0x060021D9 RID: 8665 RVA: 0x000A9926 File Offset: 0x000A7B26
		public string XmlSpace
		{
			get
			{
				return this._xmlSpace;
			}
			set
			{
				this.EndRepeat();
				this._xmlSpace = value;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x060021DA RID: 8666 RVA: 0x000A9935 File Offset: 0x000A7B35
		// (set) Token: 0x060021DB RID: 8667 RVA: 0x000A993D File Offset: 0x000A7B3D
		internal Type TargetType
		{
			get
			{
				return this._targetType;
			}
			set
			{
				this.EndRepeat();
				this._targetType = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Markup.XamlTypeMapper" /> to use with this <see cref="T:System.Windows.Markup.ParserContext" />.</summary>
		/// <returns>The type mapper to use when mapping XAML elements to CLR types. </returns>
		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x060021DC RID: 8668 RVA: 0x000A994C File Offset: 0x000A7B4C
		// (set) Token: 0x060021DD RID: 8669 RVA: 0x000A9954 File Offset: 0x000A7B54
		public XamlTypeMapper XamlTypeMapper
		{
			get
			{
				return this._xamlTypeMapper;
			}
			set
			{
				if (this._xamlTypeMapper != value)
				{
					this._xamlTypeMapper = value;
					this._mapTable = new BamlMapTable(value);
					this._xamlTypeMapper.MapTable = this._mapTable;
				}
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x060021DE RID: 8670 RVA: 0x000A9983 File Offset: 0x000A7B83
		internal Stack NameScopeStack
		{
			get
			{
				if (this._nameScopeStack == null)
				{
					this._nameScopeStack = new Stack(2);
				}
				return this._nameScopeStack;
			}
		}

		/// <summary>Gets or sets the base URI for this context.</summary>
		/// <returns>The base URI, as a string.</returns>
		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x060021DF RID: 8671 RVA: 0x000A999F File Offset: 0x000A7B9F
		// (set) Token: 0x060021E0 RID: 8672 RVA: 0x000A99A7 File Offset: 0x000A7BA7
		public Uri BaseUri
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

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x060021E1 RID: 8673 RVA: 0x000A99B0 File Offset: 0x000A7BB0
		// (set) Token: 0x060021E2 RID: 8674 RVA: 0x000A99B8 File Offset: 0x000A7BB8
		internal bool SkipJournaledProperties
		{
			get
			{
				return this._skipJournaledProperties;
			}
			set
			{
				this._skipJournaledProperties = value;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x060021E3 RID: 8675 RVA: 0x000A99C1 File Offset: 0x000A7BC1
		// (set) Token: 0x060021E4 RID: 8676 RVA: 0x000A99CE File Offset: 0x000A7BCE
		internal Assembly StreamCreatedAssembly
		{
			get
			{
				return this._streamCreatedAssembly.Value;
			}
			[SecurityCritical]
			set
			{
				this._streamCreatedAssembly.Value = value;
			}
		}

		/// <summary>Converts a XAML <see cref="T:System.Windows.Markup.ParserContext" /> to an <see cref="T:System.Xml.XmlParserContext" />.</summary>
		/// <param name="parserContext">The XAML parser context to convert to an <see cref="T:System.Xml.XmlParserContext" />.</param>
		/// <returns>The converted XML parser context.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="parserContext" /> is <see langword="null" />.</exception>
		// Token: 0x060021E5 RID: 8677 RVA: 0x000A99DC File Offset: 0x000A7BDC
		public static implicit operator XmlParserContext(ParserContext parserContext)
		{
			return ParserContext.ToXmlParserContext(parserContext);
		}

		/// <summary>Converts an <see cref="T:System.Windows.Markup.ParserContext" /> to an <see cref="T:System.Xml.XmlParserContext" />.</summary>
		/// <param name="parserContext">The context to convert to an <see cref="T:System.Xml.XmlParserContext" />.</param>
		/// <returns>The XML parser context.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="parserContext" /> is <see langword="null" />.</exception>
		// Token: 0x060021E6 RID: 8678 RVA: 0x000A99E4 File Offset: 0x000A7BE4
		public static XmlParserContext ToXmlParserContext(ParserContext parserContext)
		{
			if (parserContext == null)
			{
				throw new ArgumentNullException("parserContext");
			}
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(new NameTable());
			XmlSpace xmlSpace = System.Xml.XmlSpace.None;
			if (parserContext.XmlSpace != null && parserContext.XmlSpace.Length != 0)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(XmlSpace));
				if (converter != null)
				{
					try
					{
						xmlSpace = (XmlSpace)converter.ConvertFromString(null, TypeConverterHelper.InvariantEnglishUS, parserContext.XmlSpace);
					}
					catch (FormatException)
					{
						xmlSpace = System.Xml.XmlSpace.None;
					}
				}
			}
			if (parserContext._xmlnsDictionary != null)
			{
				foreach (object obj in parserContext._xmlnsDictionary.Keys)
				{
					string prefix = (string)obj;
					xmlNamespaceManager.AddNamespace(prefix, parserContext._xmlnsDictionary[prefix]);
				}
			}
			XmlParserContext xmlParserContext = new XmlParserContext(null, xmlNamespaceManager, parserContext.XmlLang, xmlSpace);
			if (parserContext.BaseUri == null)
			{
				xmlParserContext.BaseURI = null;
			}
			else
			{
				string components = parserContext.BaseUri.GetComponents(UriComponents.SerializationInfoString, UriFormat.SafeUnescaped);
				Uri uri = new Uri(components);
				string components2 = uri.GetComponents(UriComponents.SerializationInfoString, UriFormat.UriEscaped);
				xmlParserContext.BaseURI = components2;
			}
			return xmlParserContext;
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x000A9B28 File Offset: 0x000A7D28
		private void EndRepeat()
		{
			if (this._repeat > 0)
			{
				if (this._langSpaceStack == null)
				{
					this._langSpaceStack = new Stack(1);
				}
				this._langSpaceStack.Push(this.XmlLang);
				this._langSpaceStack.Push(this.XmlSpace);
				this._langSpaceStack.Push(this.TargetType);
				this._langSpaceStack.Push(this._repeat);
				this._repeat = 0;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x060021E8 RID: 8680 RVA: 0x000A9BA2 File Offset: 0x000A7DA2
		// (set) Token: 0x060021E9 RID: 8681 RVA: 0x000A9BAA File Offset: 0x000A7DAA
		internal int LineNumber
		{
			get
			{
				return this._lineNumber;
			}
			set
			{
				this._lineNumber = value;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x060021EA RID: 8682 RVA: 0x000A9BB3 File Offset: 0x000A7DB3
		// (set) Token: 0x060021EB RID: 8683 RVA: 0x000A9BBB File Offset: 0x000A7DBB
		internal int LinePosition
		{
			get
			{
				return this._linePosition;
			}
			set
			{
				this._linePosition = value;
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x060021EC RID: 8684 RVA: 0x000A9BC4 File Offset: 0x000A7DC4
		// (set) Token: 0x060021ED RID: 8685 RVA: 0x000A9BCC File Offset: 0x000A7DCC
		internal bool IsDebugBamlStream
		{
			get
			{
				return this._isDebugBamlStream;
			}
			set
			{
				this._isDebugBamlStream = value;
			}
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x060021EE RID: 8686 RVA: 0x000A9BD5 File Offset: 0x000A7DD5
		// (set) Token: 0x060021EF RID: 8687 RVA: 0x000A9BDD File Offset: 0x000A7DDD
		internal object RootElement
		{
			get
			{
				return this._rootElement;
			}
			set
			{
				this._rootElement = value;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x060021F0 RID: 8688 RVA: 0x000A9BE6 File Offset: 0x000A7DE6
		// (set) Token: 0x060021F1 RID: 8689 RVA: 0x000A9BEE File Offset: 0x000A7DEE
		internal bool OwnsBamlStream
		{
			get
			{
				return this._ownsBamlStream;
			}
			set
			{
				this._ownsBamlStream = value;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x060021F2 RID: 8690 RVA: 0x000A9BF7 File Offset: 0x000A7DF7
		// (set) Token: 0x060021F3 RID: 8691 RVA: 0x000A9BFF File Offset: 0x000A7DFF
		internal BamlMapTable MapTable
		{
			get
			{
				return this._mapTable;
			}
			set
			{
				if (this._mapTable != value)
				{
					this._mapTable = value;
					this._xamlTypeMapper = this._mapTable.XamlTypeMapper;
					this._xamlTypeMapper.MapTable = this._mapTable;
				}
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x060021F4 RID: 8692 RVA: 0x000A9C33 File Offset: 0x000A7E33
		// (set) Token: 0x060021F5 RID: 8693 RVA: 0x000A9C3B File Offset: 0x000A7E3B
		internal IStyleConnector StyleConnector
		{
			get
			{
				return this._styleConnector;
			}
			set
			{
				this._styleConnector = value;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x060021F6 RID: 8694 RVA: 0x000A9C44 File Offset: 0x000A7E44
		internal ProvideValueServiceProvider ProvideValueProvider
		{
			get
			{
				if (this._provideValueServiceProvider == null)
				{
					this._provideValueServiceProvider = new ProvideValueServiceProvider(this);
				}
				return this._provideValueServiceProvider;
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x060021F7 RID: 8695 RVA: 0x000A9C60 File Offset: 0x000A7E60
		internal List<object[]> StaticResourcesStack
		{
			get
			{
				if (this._staticResourcesStack == null)
				{
					this._staticResourcesStack = new List<object[]>();
				}
				return this._staticResourcesStack;
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x060021F8 RID: 8696 RVA: 0x000A9C7B File Offset: 0x000A7E7B
		internal bool InDeferredSection
		{
			get
			{
				return this._staticResourcesStack != null && this._staticResourcesStack.Count > 0;
			}
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x000A9C95 File Offset: 0x000A7E95
		internal ParserContext ScopedCopy()
		{
			return this.ScopedCopy(true);
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x000A9CA0 File Offset: 0x000A7EA0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal ParserContext ScopedCopy(bool copyNameScopeStack)
		{
			ParserContext parserContext = new ParserContext();
			parserContext._baseUri = this._baseUri;
			parserContext._skipJournaledProperties = this._skipJournaledProperties;
			parserContext._xmlLang = this._xmlLang;
			parserContext._xmlSpace = this._xmlSpace;
			parserContext._repeat = this._repeat;
			parserContext._lineNumber = this._lineNumber;
			parserContext._linePosition = this._linePosition;
			parserContext._isDebugBamlStream = this._isDebugBamlStream;
			parserContext._mapTable = this._mapTable;
			parserContext._xamlTypeMapper = this._xamlTypeMapper;
			parserContext._targetType = this._targetType;
			parserContext._streamCreatedAssembly.Value = this._streamCreatedAssembly.Value;
			parserContext._rootElement = this._rootElement;
			parserContext._styleConnector = this._styleConnector;
			if (this._nameScopeStack != null && copyNameScopeStack)
			{
				parserContext._nameScopeStack = ((this._nameScopeStack != null) ? ((Stack)this._nameScopeStack.Clone()) : null);
			}
			else
			{
				parserContext._nameScopeStack = null;
			}
			parserContext._langSpaceStack = ((this._langSpaceStack != null) ? ((Stack)this._langSpaceStack.Clone()) : null);
			if (this._xmlnsDictionary != null)
			{
				parserContext._xmlnsDictionary = new XmlnsDictionary(this._xmlnsDictionary);
			}
			else
			{
				parserContext._xmlnsDictionary = null;
			}
			parserContext._currentFreezeStackFrame = this._currentFreezeStackFrame;
			parserContext._freezeStack = ((this._freezeStack != null) ? ((Stack)this._freezeStack.Clone()) : null);
			return parserContext;
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x000A9E0D File Offset: 0x000A800D
		internal void TrimState()
		{
			if (this._nameScopeStack != null && this._nameScopeStack.Count == 0)
			{
				this._nameScopeStack = null;
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x060021FC RID: 8700 RVA: 0x000A9E2B File Offset: 0x000A802B
		internal Dictionary<Type, Dictionary<string, SpecialBracketCharacters>> MasterBracketCharacterCache
		{
			get
			{
				if (this._masterBracketCharacterCache == null)
				{
					this._masterBracketCharacterCache = new Dictionary<Type, Dictionary<string, SpecialBracketCharacters>>();
				}
				return this._masterBracketCharacterCache;
			}
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x000A9E48 File Offset: 0x000A8048
		internal ParserContext Clone()
		{
			ParserContext parserContext = this.ScopedCopy();
			parserContext._mapTable = ((this._mapTable != null) ? this._mapTable.Clone() : null);
			parserContext._xamlTypeMapper = ((this._xamlTypeMapper != null) ? this._xamlTypeMapper.Clone() : null);
			parserContext._xamlTypeMapper.MapTable = parserContext._mapTable;
			parserContext._mapTable.XamlTypeMapper = parserContext._xamlTypeMapper;
			return parserContext;
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x060021FE RID: 8702 RVA: 0x000A9EB7 File Offset: 0x000A80B7
		// (set) Token: 0x060021FF RID: 8703 RVA: 0x000A9EC4 File Offset: 0x000A80C4
		internal bool FreezeFreezables
		{
			get
			{
				return this._currentFreezeStackFrame.FreezeFreezables;
			}
			set
			{
				if (value != this._currentFreezeStackFrame.FreezeFreezables)
				{
					this._currentFreezeStackFrame.DecrementRepeatCount();
					if (this._freezeStack == null)
					{
						this._freezeStack = new Stack();
					}
					this._freezeStack.Push(this._currentFreezeStackFrame);
					this._currentFreezeStackFrame.Reset(value);
				}
			}
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x000A9F20 File Offset: 0x000A8120
		internal bool TryCacheFreezable(string value, Freezable freezable)
		{
			if (this.FreezeFreezables && freezable.CanFreeze)
			{
				if (!freezable.IsFrozen)
				{
					freezable.Freeze();
				}
				if (this._freezeCache == null)
				{
					this._freezeCache = new Dictionary<string, Freezable>();
				}
				this._freezeCache.Add(value, freezable);
				return true;
			}
			return false;
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x000A9F70 File Offset: 0x000A8170
		internal Freezable TryGetFreezable(string value)
		{
			Freezable result = null;
			if (this._freezeCache != null)
			{
				this._freezeCache.TryGetValue(value, out result);
			}
			return result;
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x000A9F98 File Offset: 0x000A8198
		private Dictionary<string, SpecialBracketCharacters> BuildBracketCharacterCacheForType(Type extensionType)
		{
			Dictionary<string, SpecialBracketCharacters> dictionary = new Dictionary<string, SpecialBracketCharacters>(StringComparer.OrdinalIgnoreCase);
			PropertyInfo[] properties = extensionType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			Type type = null;
			Type type2 = null;
			foreach (PropertyInfo propertyInfo in properties)
			{
				string name = propertyInfo.Name;
				string text = null;
				IList<CustomAttributeData> customAttributes = CustomAttributeData.GetCustomAttributes(propertyInfo);
				SpecialBracketCharacters specialBracketCharacters = null;
				foreach (CustomAttributeData customAttributeData in customAttributes)
				{
					Type attributeType = customAttributeData.AttributeType;
					Assembly assembly = attributeType.Assembly;
					if (type == null || type2 == null)
					{
						type = assembly.GetType("System.Windows.Markup.ConstructorArgumentAttribute");
						type2 = assembly.GetType("System.Windows.Markup.MarkupExtensionBracketCharactersAttribute");
					}
					if (attributeType.IsAssignableFrom(type))
					{
						text = (customAttributeData.ConstructorArguments[0].Value as string);
					}
					else if (attributeType.IsAssignableFrom(type2))
					{
						if (specialBracketCharacters == null)
						{
							specialBracketCharacters = new SpecialBracketCharacters();
						}
						specialBracketCharacters.AddBracketCharacters((char)customAttributeData.ConstructorArguments[0].Value, (char)customAttributeData.ConstructorArguments[1].Value);
					}
				}
				if (specialBracketCharacters != null)
				{
					specialBracketCharacters.EndInit();
					dictionary.Add(name, specialBracketCharacters);
					if (text != null)
					{
						dictionary.Add(text, specialBracketCharacters);
					}
				}
			}
			if (dictionary.Count != 0)
			{
				return dictionary;
			}
			return null;
		}

		// Token: 0x040019B1 RID: 6577
		private XamlTypeMapper _xamlTypeMapper;

		// Token: 0x040019B2 RID: 6578
		private Uri _baseUri;

		// Token: 0x040019B3 RID: 6579
		private XmlnsDictionary _xmlnsDictionary;

		// Token: 0x040019B4 RID: 6580
		private string _xmlLang = string.Empty;

		// Token: 0x040019B5 RID: 6581
		private string _xmlSpace = string.Empty;

		// Token: 0x040019B6 RID: 6582
		private Stack _langSpaceStack;

		// Token: 0x040019B7 RID: 6583
		private int _repeat;

		// Token: 0x040019B8 RID: 6584
		private Type _targetType;

		// Token: 0x040019B9 RID: 6585
		private Dictionary<Type, Dictionary<string, SpecialBracketCharacters>> _masterBracketCharacterCache;

		// Token: 0x040019BA RID: 6586
		private bool _skipJournaledProperties;

		// Token: 0x040019BB RID: 6587
		private SecurityCriticalDataForSet<Assembly> _streamCreatedAssembly;

		// Token: 0x040019BC RID: 6588
		private bool _ownsBamlStream;

		// Token: 0x040019BD RID: 6589
		private ProvideValueServiceProvider _provideValueServiceProvider;

		// Token: 0x040019BE RID: 6590
		private IStyleConnector _styleConnector;

		// Token: 0x040019BF RID: 6591
		private Stack _nameScopeStack;

		// Token: 0x040019C0 RID: 6592
		private List<object[]> _staticResourcesStack;

		// Token: 0x040019C1 RID: 6593
		private object _rootElement;

		// Token: 0x040019C2 RID: 6594
		private ParserContext.FreezeStackFrame _currentFreezeStackFrame;

		// Token: 0x040019C3 RID: 6595
		private Dictionary<string, Freezable> _freezeCache;

		// Token: 0x040019C4 RID: 6596
		private Stack _freezeStack;

		// Token: 0x040019C5 RID: 6597
		private int _lineNumber;

		// Token: 0x040019C6 RID: 6598
		private int _linePosition;

		// Token: 0x040019C7 RID: 6599
		private BamlMapTable _mapTable;

		// Token: 0x040019C8 RID: 6600
		private bool _isDebugBamlStream;

		// Token: 0x02000896 RID: 2198
		private struct FreezeStackFrame
		{
			// Token: 0x06008396 RID: 33686 RVA: 0x00245CF6 File Offset: 0x00243EF6
			internal void IncrementRepeatCount()
			{
				this._repeatCount++;
			}

			// Token: 0x06008397 RID: 33687 RVA: 0x00245D06 File Offset: 0x00243F06
			internal bool DecrementRepeatCount()
			{
				if (this._repeatCount > 0)
				{
					this._repeatCount--;
					return true;
				}
				return false;
			}

			// Token: 0x17001DD5 RID: 7637
			// (get) Token: 0x06008398 RID: 33688 RVA: 0x00245D22 File Offset: 0x00243F22
			internal bool FreezeFreezables
			{
				get
				{
					return this._freezeFreezables;
				}
			}

			// Token: 0x06008399 RID: 33689 RVA: 0x00245D2A File Offset: 0x00243F2A
			internal void Reset(bool freezeFreezables)
			{
				this._freezeFreezables = freezeFreezables;
				this._repeatCount = 0;
			}

			// Token: 0x04004193 RID: 16787
			private bool _freezeFreezables;

			// Token: 0x04004194 RID: 16788
			private int _repeatCount;
		}
	}
}
