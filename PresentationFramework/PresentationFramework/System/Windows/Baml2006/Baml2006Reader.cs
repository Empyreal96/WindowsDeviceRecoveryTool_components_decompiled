using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Diagnostics;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xaml;
using MS.Internal.WindowsBase;

namespace System.Windows.Baml2006
{
	/// <summary>Processes XAML in optimized BAML form and produces a XAML node stream.</summary>
	// Token: 0x02000160 RID: 352
	public class Baml2006Reader : System.Xaml.XamlReader, IXamlLineInfo, IFreezeFreezables
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Baml2006.Baml2006Reader" /> class, based on the file name of a local file to read.</summary>
		/// <param name="fileName">String that declares a file path to the file that contains BAML to read.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="fileName" /> is <see langword="null" />.</exception>
		// Token: 0x06000FD4 RID: 4052 RVA: 0x0003D88C File Offset: 0x0003BA8C
		public Baml2006Reader(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			Baml2006SchemaContext schemaContext = new Baml2006SchemaContext(null);
			Baml2006ReaderSettings baml2006ReaderSettings = System.Windows.Markup.XamlReader.CreateBamlReaderSettings();
			baml2006ReaderSettings.OwnsStream = true;
			this.Initialize(stream, schemaContext, baml2006ReaderSettings);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Baml2006.Baml2006Reader" /> class based on an input stream.</summary>
		/// <param name="stream">Input stream of source BAML.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="stream" /> is <see langword="null" />.</exception>
		// Token: 0x06000FD5 RID: 4053 RVA: 0x0003D8F8 File Offset: 0x0003BAF8
		public Baml2006Reader(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			Baml2006SchemaContext schemaContext = new Baml2006SchemaContext(null);
			Baml2006ReaderSettings settings = new Baml2006ReaderSettings();
			this.Initialize(stream, schemaContext, settings);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Baml2006.Baml2006Reader" /> class based on an input stream and reader settings.</summary>
		/// <param name="stream">Input stream of source BAML.</param>
		/// <param name="xamlReaderSettings">Reader settings. See Remarks.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="stream" /> or <paramref name="xamlReaderSettings" /> is <see langword="null" />.</exception>
		// Token: 0x06000FD6 RID: 4054 RVA: 0x0003D950 File Offset: 0x0003BB50
		public Baml2006Reader(Stream stream, XamlReaderSettings xamlReaderSettings)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (xamlReaderSettings == null)
			{
				throw new ArgumentNullException("xamlReaderSettings");
			}
			Baml2006SchemaContext schemaContext;
			if (xamlReaderSettings.ValuesMustBeString)
			{
				schemaContext = new Baml2006SchemaContext(xamlReaderSettings.LocalAssembly, System.Windows.Markup.XamlReader.XamlV3SharedSchemaContext);
			}
			else
			{
				schemaContext = new Baml2006SchemaContext(xamlReaderSettings.LocalAssembly);
			}
			Baml2006ReaderSettings settings = new Baml2006ReaderSettings(xamlReaderSettings);
			this.Initialize(stream, schemaContext, settings);
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x0003D9D8 File Offset: 0x0003BBD8
		internal Baml2006Reader(Stream stream, Baml2006SchemaContext schemaContext, Baml2006ReaderSettings settings)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (schemaContext == null)
			{
				throw new ArgumentNullException("schemaContext");
			}
			this.Initialize(stream, schemaContext, settings ?? new Baml2006ReaderSettings());
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0003DA3A File Offset: 0x0003BC3A
		internal Baml2006Reader(Stream stream, Baml2006SchemaContext baml2006SchemaContext, Baml2006ReaderSettings baml2006ReaderSettings, object root) : this(stream, baml2006SchemaContext, baml2006ReaderSettings)
		{
			this._root = root;
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0003DA50 File Offset: 0x0003BC50
		private void Initialize(Stream stream, Baml2006SchemaContext schemaContext, Baml2006ReaderSettings settings)
		{
			schemaContext.Settings = settings;
			this._settings = settings;
			this._context = new Baml2006ReaderContext(schemaContext);
			this._xamlMainNodeQueue = new XamlNodeQueue(schemaContext);
			this._xamlNodesReader = this._xamlMainNodeQueue.Reader;
			this._xamlNodesWriter = this._xamlMainNodeQueue.Writer;
			this._lookingForAKeyOnAMarkupExtensionInADictionaryDepth = -1;
			this._isBinaryProvider = !settings.ValuesMustBeString;
			if (this._settings.OwnsStream)
			{
				stream = new SharedStream(stream);
			}
			this._binaryReader = new BamlBinaryReader(stream);
			this._context.TemplateStartDepth = -1;
			if (!this._settings.IsBamlFragment)
			{
				this.Process_Header();
			}
		}

		/// <summary>Provides the next XAML node from the source BAML, if a node is available. </summary>
		/// <returns>
		///     <see langword="true" /> if a node is available; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">Reader was disposed during traversal.</exception>
		// Token: 0x06000FDA RID: 4058 RVA: 0x0003DAFC File Offset: 0x0003BCFC
		public override bool Read()
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException("Baml2006Reader");
			}
			if (this.IsEof)
			{
				return false;
			}
			while (!this._xamlNodesReader.Read())
			{
				if (!this.Process_BamlRecords())
				{
					this._isEof = true;
					return false;
				}
			}
			if (this._binaryReader.BaseStream.Length == this._binaryReader.BaseStream.Position && this._xamlNodesReader.NodeType != System.Xaml.XamlNodeType.EndObject)
			{
				this._isEof = true;
				return false;
			}
			return true;
		}

		/// <summary>Gets the type of the current node.</summary>
		/// <returns>A value of the enumeration.</returns>
		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06000FDB RID: 4059 RVA: 0x0003DB7E File Offset: 0x0003BD7E
		public override System.Xaml.XamlNodeType NodeType
		{
			get
			{
				return this._xamlNodesReader.NodeType;
			}
		}

		/// <summary>Gets a value that reports whether the reader position is at the end of file.</summary>
		/// <returns>
		///     <see langword="true" /> if the reader position is at the end of the file; otherwise, <see langword="false" />.</returns>
		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06000FDC RID: 4060 RVA: 0x0003DB8B File Offset: 0x0003BD8B
		public override bool IsEof
		{
			get
			{
				return this._isEof;
			}
		}

		/// <summary>Gets the XAML namespace from the current node.</summary>
		/// <returns>The XAML namespace if available, otherwise <see langword="null" />.</returns>
		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06000FDD RID: 4061 RVA: 0x0003DB93 File Offset: 0x0003BD93
		public override NamespaceDeclaration Namespace
		{
			get
			{
				return this._xamlNodesReader.Namespace;
			}
		}

		/// <summary>Gets an object that provides schema context information for the information set.</summary>
		/// <returns>An object that provides schema context information for the information set.</returns>
		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06000FDE RID: 4062 RVA: 0x0003DBA0 File Offset: 0x0003BDA0
		public override XamlSchemaContext SchemaContext
		{
			get
			{
				return this._xamlNodesReader.SchemaContext;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xaml.XamlType" /> of the current node.</summary>
		/// <returns>The <see cref="T:System.Xaml.XamlType" /> of the current node, or <see langword="null" /> if the position is not on an object.</returns>
		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06000FDF RID: 4063 RVA: 0x0003DBAD File Offset: 0x0003BDAD
		public override XamlType Type
		{
			get
			{
				return this._xamlNodesReader.Type;
			}
		}

		/// <summary>Gets the value of the current node.</summary>
		/// <returns>The value of the current node, or <see langword="null" /> if the position is not on a <see cref="F:System.Xaml.XamlNodeType.Value" /> node type.</returns>
		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x0003DBBA File Offset: 0x0003BDBA
		public override object Value
		{
			get
			{
				return this._xamlNodesReader.Value;
			}
		}

		/// <summary>Gets the current member at the reader position, if the reader position is on a <see cref="F:System.Xaml.XamlNodeType.StartMember" />.</summary>
		/// <returns>The current member, or <see langword="null" /> if the position is not on a member.</returns>
		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06000FE1 RID: 4065 RVA: 0x0003DBC7 File Offset: 0x0003BDC7
		public override XamlMember Member
		{
			get
			{
				return this._xamlNodesReader.Member;
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Baml2006.Baml2006Reader" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release the managed resources; otherwise, <see langword="false" />.</param>
		// Token: 0x06000FE2 RID: 4066 RVA: 0x0003DBD4 File Offset: 0x0003BDD4
		protected override void Dispose(bool disposing)
		{
			if (this._binaryReader != null)
			{
				if (this._settings.OwnsStream)
				{
					SharedStream sharedStream = this._binaryReader.BaseStream as SharedStream;
					if (sharedStream != null && sharedStream.SharedCount < 1)
					{
						this._binaryReader.Close();
					}
				}
				this._binaryReader = null;
				this._context = null;
			}
		}

		/// <summary>See <see cref="P:System.Xaml.IXamlLineInfo.HasLineInfo" />.</summary>
		/// <returns>
		///     <see langword="true" /> if line information is available; otherwise, <see langword="false" />.</returns>
		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06000FE3 RID: 4067 RVA: 0x0003DC2C File Offset: 0x0003BE2C
		bool IXamlLineInfo.HasLineInfo
		{
			get
			{
				return this._context.CurrentFrame != null;
			}
		}

		/// <summary>See <see cref="P:System.Xaml.IXamlLineInfo.LineNumber" />.</summary>
		/// <returns>The line number to report.</returns>
		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x0003DC3C File Offset: 0x0003BE3C
		int IXamlLineInfo.LineNumber
		{
			get
			{
				return this._context.LineNumber;
			}
		}

		/// <summary>See <see cref="P:System.Xaml.IXamlLineInfo.LinePosition" />.</summary>
		/// <returns>The line position to report.</returns>
		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x0003DC49 File Offset: 0x0003BE49
		int IXamlLineInfo.LinePosition
		{
			get
			{
				return this._context.LineOffset;
			}
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0003DC58 File Offset: 0x0003BE58
		internal List<KeyRecord> ReadKeys()
		{
			this._context.KeyList = new List<KeyRecord>();
			this._context.CurrentFrame.IsDeferredContent = true;
			bool flag = true;
			while (flag)
			{
				Baml2006RecordType baml2006RecordType = this.Read_RecordType();
				switch (baml2006RecordType)
				{
				case Baml2006RecordType.DefAttributeKeyString:
					this.Process_DefAttributeKeyString();
					break;
				case Baml2006RecordType.DefAttributeKeyType:
					this.Process_DefAttributeKeyType();
					break;
				case Baml2006RecordType.KeyElementStart:
					this.Process_KeyElementStart();
					for (;;)
					{
						baml2006RecordType = this.Read_RecordType();
						if (baml2006RecordType == Baml2006RecordType.KeyElementEnd)
						{
							break;
						}
						this._binaryReader.BaseStream.Seek(-1L, SeekOrigin.Current);
						this.Process_OneBamlRecord();
					}
					this.Process_KeyElementEnd();
					break;
				default:
					if (baml2006RecordType != Baml2006RecordType.StaticResourceStart)
					{
						if (baml2006RecordType != Baml2006RecordType.OptimizedStaticResource)
						{
							flag = false;
							this._binaryReader.BaseStream.Seek(-1L, SeekOrigin.Current);
						}
						else
						{
							this.Process_OptimizedStaticResource();
						}
					}
					else
					{
						this.Process_StaticResourceStart();
						for (;;)
						{
							baml2006RecordType = this.Read_RecordType();
							if (baml2006RecordType == Baml2006RecordType.StaticResourceEnd)
							{
								break;
							}
							this._binaryReader.BaseStream.Seek(-1L, SeekOrigin.Current);
							this.Process_OneBamlRecord();
						}
						this.Process_StaticResourceEnd();
					}
					break;
				}
				if (this._binaryReader.BaseStream.Length == this._binaryReader.BaseStream.Position)
				{
					break;
				}
			}
			KeyRecord keyRecord = null;
			long position = this._binaryReader.BaseStream.Position;
			foreach (KeyRecord keyRecord2 in this._context.KeyList)
			{
				keyRecord2.ValuePosition += position;
				if (keyRecord != null)
				{
					keyRecord.ValueSize = (int)(keyRecord2.ValuePosition - keyRecord.ValuePosition);
				}
				keyRecord = keyRecord2;
			}
			keyRecord.ValueSize = (int)(this._binaryReader.BaseStream.Length - keyRecord.ValuePosition);
			return this._context.KeyList;
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0003DE34 File Offset: 0x0003C034
		internal System.Xaml.XamlReader ReadObject(KeyRecord record)
		{
			if (record.ValuePosition == this._binaryReader.BaseStream.Length)
			{
				return null;
			}
			this._binaryReader.BaseStream.Seek(record.ValuePosition, SeekOrigin.Begin);
			this._context.CurrentKey = this._context.KeyList.IndexOf(record);
			if (this._xamlMainNodeQueue.Count > 0)
			{
				throw new System.Xaml.XamlParseException();
			}
			if (this.Read_RecordType() != Baml2006RecordType.ElementStart)
			{
				throw new System.Xaml.XamlParseException();
			}
			System.Xaml.XamlWriter xamlNodesWriter = this._xamlNodesWriter;
			int num = (record.ValueSize < 800) ? ((int)((double)record.ValueSize / 2.2)) : ((int)((double)record.ValueSize / 4.25));
			num = ((num < 8) ? 8 : num);
			XamlNodeList xamlNodeList = new XamlNodeList(this._xamlNodesReader.SchemaContext, num);
			this._xamlNodesWriter = xamlNodeList.Writer;
			Baml2006ReaderFrame currentFrame = this._context.CurrentFrame;
			this.Process_ElementStart();
			while (currentFrame != this._context.CurrentFrame)
			{
				this.Process_OneBamlRecord();
			}
			this._xamlNodesWriter.Close();
			this._xamlNodesWriter = xamlNodesWriter;
			return xamlNodeList.GetReader();
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0003DF58 File Offset: 0x0003C158
		internal Type GetTypeOfFirstStartObject(KeyRecord record)
		{
			this._context.CurrentKey = this._context.KeyList.IndexOf(record);
			if (record.ValuePosition == this._binaryReader.BaseStream.Length)
			{
				return null;
			}
			this._binaryReader.BaseStream.Seek(record.ValuePosition, SeekOrigin.Begin);
			if (this.Read_RecordType() != Baml2006RecordType.ElementStart)
			{
				throw new System.Xaml.XamlParseException();
			}
			return this.BamlSchemaContext.GetClrType(this._binaryReader.ReadInt16());
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0003DFD8 File Offset: 0x0003C1D8
		private bool Process_BamlRecords()
		{
			int count = this._xamlMainNodeQueue.Count;
			while (this.Process_OneBamlRecord())
			{
				if (this._xamlMainNodeQueue.Count > count)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0003E00C File Offset: 0x0003C20C
		private bool Process_OneBamlRecord()
		{
			if (this._binaryReader.BaseStream.Position == this._binaryReader.BaseStream.Length)
			{
				this._isEof = true;
				return false;
			}
			Baml2006RecordType baml2006RecordType = this.Read_RecordType();
			switch (baml2006RecordType)
			{
			case Baml2006RecordType.DocumentStart:
				this.SkipBytes(6L);
				return true;
			case Baml2006RecordType.DocumentEnd:
				return false;
			case Baml2006RecordType.ElementStart:
				this.Process_ElementStart();
				return true;
			case Baml2006RecordType.ElementEnd:
				this.Process_ElementEnd();
				return true;
			case Baml2006RecordType.Property:
				this.Process_Property();
				return true;
			case Baml2006RecordType.PropertyCustom:
				this.Process_PropertyCustom();
				return true;
			case Baml2006RecordType.PropertyComplexStart:
				this.Process_PropertyComplexStart();
				return true;
			case Baml2006RecordType.PropertyComplexEnd:
			case Baml2006RecordType.PropertyArrayEnd:
			case Baml2006RecordType.PropertyIListEnd:
				this.Process_PropertyEnd();
				return true;
			case Baml2006RecordType.PropertyArrayStart:
			case Baml2006RecordType.PropertyIListStart:
				this.Process_PropertyArrayStart();
				return true;
			case Baml2006RecordType.PropertyIDictionaryStart:
				this.Process_PropertyIDictionaryStart();
				return true;
			case Baml2006RecordType.PropertyIDictionaryEnd:
				this.Process_PropertyIDictionaryEnd();
				return true;
			case Baml2006RecordType.LiteralContent:
				this.Process_LiteralContent();
				return true;
			case Baml2006RecordType.Text:
				this.Process_Text();
				return true;
			case Baml2006RecordType.TextWithConverter:
				this.Process_TextWithConverter();
				return true;
			case Baml2006RecordType.RoutedEvent:
				this.Process_RoutedEvent();
				return true;
			case Baml2006RecordType.ClrEvent:
				this.Process_ClrEvent();
				return true;
			case Baml2006RecordType.XmlnsProperty:
				throw new System.Xaml.XamlParseException("Found unexpected Xmlns BAML record");
			case Baml2006RecordType.XmlAttribute:
				this.Process_XmlAttribute();
				return true;
			case Baml2006RecordType.ProcessingInstruction:
				this.Process_ProcessingInstruction();
				return true;
			case Baml2006RecordType.Comment:
				this.Process_Comment();
				return true;
			case Baml2006RecordType.DefTag:
				this.Process_DefTag();
				return true;
			case Baml2006RecordType.DefAttribute:
				this.Process_DefAttribute();
				return true;
			case Baml2006RecordType.EndAttributes:
				this.Process_EndAttributes();
				return true;
			case Baml2006RecordType.PIMapping:
				this.Process_PIMapping();
				return true;
			case Baml2006RecordType.AssemblyInfo:
				this.Process_AssemblyInfo();
				return true;
			case Baml2006RecordType.TypeInfo:
				this.Process_TypeInfo();
				return true;
			case Baml2006RecordType.TypeSerializerInfo:
				this.Process_TypeSerializerInfo();
				return true;
			case Baml2006RecordType.AttributeInfo:
				this.Process_AttributeInfo();
				return true;
			case Baml2006RecordType.StringInfo:
				this.Process_StringInfo();
				return true;
			case Baml2006RecordType.PropertyStringReference:
				this.Process_PropertyStringReference();
				return true;
			case Baml2006RecordType.PropertyTypeReference:
				this.Process_PropertyTypeReference();
				return true;
			case Baml2006RecordType.PropertyWithExtension:
				this.Process_PropertyWithExtension();
				return true;
			case Baml2006RecordType.PropertyWithConverter:
				this.Process_PropertyWithConverter();
				return true;
			case Baml2006RecordType.DeferableContentStart:
				this.Process_DeferableContentStart();
				return true;
			case Baml2006RecordType.DefAttributeKeyString:
				this.Process_DefAttributeKeyString();
				return true;
			case Baml2006RecordType.DefAttributeKeyType:
				this.Process_DefAttributeKeyType();
				return true;
			case Baml2006RecordType.KeyElementStart:
				this.Process_KeyElementStart();
				return true;
			case Baml2006RecordType.KeyElementEnd:
				this.Process_KeyElementEnd();
				return true;
			case Baml2006RecordType.ConstructorParametersStart:
				this.Process_ConstructorParametersStart();
				return true;
			case Baml2006RecordType.ConstructorParametersEnd:
				this.Process_ConstructorParametersEnd();
				return true;
			case Baml2006RecordType.ConstructorParameterType:
				this.Process_ConstructorParameterType();
				return true;
			case Baml2006RecordType.ConnectionId:
				this.Process_ConnectionId();
				return true;
			case Baml2006RecordType.ContentProperty:
				this.Process_ContentProperty();
				return true;
			case Baml2006RecordType.NamedElementStart:
				throw new System.Xaml.XamlParseException();
			case Baml2006RecordType.StaticResourceStart:
				this.Process_StaticResourceStart();
				return true;
			case Baml2006RecordType.StaticResourceEnd:
				this.Process_StaticResourceEnd();
				return true;
			case Baml2006RecordType.StaticResourceId:
				this.Process_StaticResourceId();
				return true;
			case Baml2006RecordType.TextWithId:
				this.Process_TextWithId();
				return true;
			case Baml2006RecordType.PresentationOptionsAttribute:
				this.Process_PresentationOptionsAttribute();
				return true;
			case Baml2006RecordType.LineNumberAndPosition:
				this.Process_LineNumberAndPosition();
				return true;
			case Baml2006RecordType.LinePosition:
				this.Process_LinePosition();
				return true;
			case Baml2006RecordType.OptimizedStaticResource:
				this.Process_OptimizedStaticResource();
				return true;
			case Baml2006RecordType.PropertyWithStaticResourceId:
				this.Process_PropertyWithStaticResourceId();
				return true;
			}
			throw new System.Xaml.XamlParseException(string.Format(CultureInfo.CurrentCulture, SR.Get("UnknownBamlRecord", new object[]
			{
				baml2006RecordType
			}), new object[0]));
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0003E384 File Offset: 0x0003C584
		private void Process_ProcessingInstruction()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0003E384 File Offset: 0x0003C584
		private void Process_DefTag()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0003E384 File Offset: 0x0003C584
		private void Process_EndAttributes()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0003E384 File Offset: 0x0003C584
		private void Process_XmlAttribute()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0003E38C File Offset: 0x0003C58C
		private void Process_PresentationOptionsAttribute()
		{
			this.Common_Process_Property();
			this.Read_RecordSize();
			string value = this._binaryReader.ReadString();
			string @string = this._context.SchemaContext.GetString(this._binaryReader.ReadInt16());
			if (this._context.TemplateStartDepth < 0)
			{
				this._xamlNodesWriter.WriteStartMember(XamlReaderHelper.Freeze);
				this._xamlNodesWriter.WriteValue(value);
				this._xamlNodesWriter.WriteEndMember();
			}
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0003E384 File Offset: 0x0003C584
		private void Process_Comment()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0003E404 File Offset: 0x0003C604
		private void Process_LiteralContent()
		{
			this.Read_RecordSize();
			string text = this._binaryReader.ReadString();
			int num = this._binaryReader.ReadInt32();
			int num2 = this._binaryReader.ReadInt32();
			bool flag = this._context.CurrentFrame.Member == null;
			if (flag)
			{
				if (!(this._context.CurrentFrame.XamlType.ContentProperty != null))
				{
					throw new NotImplementedException();
				}
				this.Common_Process_Property();
				this._xamlNodesWriter.WriteStartMember(this._context.CurrentFrame.XamlType.ContentProperty);
			}
			if (!this._isBinaryProvider)
			{
				this._xamlNodesWriter.WriteStartObject(XamlLanguage.XData);
				XamlMember member = XamlLanguage.XData.GetMember("Text");
				this._xamlNodesWriter.WriteStartMember(member);
				this._xamlNodesWriter.WriteValue(text);
				this._xamlNodesWriter.WriteEndMember();
				this._xamlNodesWriter.WriteEndObject();
			}
			else
			{
				XData xdata = new XData();
				xdata.Text = text;
				this._xamlNodesWriter.WriteValue(xdata);
			}
			if (flag)
			{
				this._xamlNodesWriter.WriteEndMember();
			}
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0003E528 File Offset: 0x0003C728
		private void Process_TextWithConverter()
		{
			this.Read_RecordSize();
			string value = this._binaryReader.ReadString();
			short num = this._binaryReader.ReadInt16();
			bool flag = this._context.CurrentFrame.Member == null;
			if (flag)
			{
				this.Common_Process_Property();
				this._xamlNodesWriter.WriteStartMember(XamlLanguage.Initialization);
			}
			this._xamlNodesWriter.WriteValue(value);
			if (flag)
			{
				this._xamlNodesWriter.WriteEndMember();
			}
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0003E5A0 File Offset: 0x0003C7A0
		private void Process_StaticResourceEnd()
		{
			System.Xaml.XamlWriter writer = this.GetLastStaticResource().ResourceNodeList.Writer;
			writer.WriteEndObject();
			writer.Close();
			this._context.InsideStaticResource = false;
			this._xamlNodesWriter = this._xamlWriterStack.Pop();
			this._context.PopScope();
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0003E5F4 File Offset: 0x0003C7F4
		private void Process_StaticResourceStart()
		{
			XamlType xamlType = this.BamlSchemaContext.GetXamlType(this._binaryReader.ReadInt16());
			byte b = this._binaryReader.ReadByte();
			StaticResource staticResource = new StaticResource(xamlType, this.BamlSchemaContext);
			this._context.LastKey.StaticResources.Add(staticResource);
			this._context.InsideStaticResource = true;
			this._xamlWriterStack.Push(this._xamlNodesWriter);
			this._xamlNodesWriter = staticResource.ResourceNodeList.Writer;
			this._context.PushScope();
			this._context.CurrentFrame.XamlType = xamlType;
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0003E694 File Offset: 0x0003C894
		private void Process_StaticResourceId()
		{
			this.InjectPropertyAndFrameIfNeeded(this._context.SchemaContext.GetXamlType(typeof(StaticResourceExtension)), 0);
			short index = this._binaryReader.ReadInt16();
			object obj = this._context.KeyList[this._context.CurrentKey - 1].StaticResources[(int)index];
			StaticResource staticResource = obj as StaticResource;
			if (staticResource != null)
			{
				XamlServices.Transform(staticResource.ResourceNodeList.GetReader(), this._xamlNodesWriter, false);
				return;
			}
			this._xamlNodesWriter.WriteValue(obj);
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0003E384 File Offset: 0x0003C584
		private void Process_ClrEvent()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0003E384 File Offset: 0x0003C584
		private void Process_RoutedEvent()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0003E384 File Offset: 0x0003C584
		private void Process_PropertyStringReference()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0003E728 File Offset: 0x0003C928
		private void Process_OptimizedStaticResource()
		{
			byte flags = this._binaryReader.ReadByte();
			short num = this._binaryReader.ReadInt16();
			OptimizedStaticResource optimizedStaticResource = new OptimizedStaticResource(flags, num);
			if (this._isBinaryProvider)
			{
				if (optimizedStaticResource.IsKeyTypeExtension)
				{
					XamlType xamlType = this.BamlSchemaContext.GetXamlType(num);
					optimizedStaticResource.KeyValue = xamlType.UnderlyingType;
				}
				else if (optimizedStaticResource.IsKeyStaticExtension)
				{
					Type memberType;
					object obj;
					string staticExtensionValue = this.GetStaticExtensionValue(num, out memberType, out obj);
					if (obj == null)
					{
						obj = new StaticExtension(staticExtensionValue)
						{
							MemberType = memberType
						}.ProvideValue(null);
					}
					optimizedStaticResource.KeyValue = obj;
				}
				else
				{
					optimizedStaticResource.KeyValue = this._context.SchemaContext.GetString(num);
				}
			}
			List<object> staticResources = this._context.LastKey.StaticResources;
			staticResources.Add(optimizedStaticResource);
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0003E7F0 File Offset: 0x0003C9F0
		private void Process_DeferableContentStart()
		{
			int num = this._binaryReader.ReadInt32();
			if (this._isBinaryProvider && num > 0)
			{
				object value;
				if (this._settings.OwnsStream)
				{
					long position = this._binaryReader.BaseStream.Position;
					value = new SharedStream(this._binaryReader.BaseStream, position, (long)num);
					this._binaryReader.BaseStream.Seek(position + (long)num, SeekOrigin.Begin);
				}
				else
				{
					value = new MemoryStream(this._binaryReader.ReadBytes(num));
				}
				this.Common_Process_Property();
				this._xamlNodesWriter.WriteStartMember(this.BamlSchemaContext.ResourceDictionaryDeferredContentProperty);
				this._xamlNodesWriter.WriteValue(value);
				this._xamlNodesWriter.WriteEndMember();
				return;
			}
			this._context.KeyList = new List<KeyRecord>();
			this._context.CurrentKey = 0;
			this._context.CurrentFrame.IsDeferredContent = true;
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0003E8DC File Offset: 0x0003CADC
		private void Process_DefAttribute()
		{
			this.Read_RecordSize();
			string text = this._binaryReader.ReadString();
			short stringId = this._binaryReader.ReadInt16();
			XamlMember xamlDirective = this.BamlSchemaContext.GetXamlDirective("http://schemas.microsoft.com/winfx/2006/xaml", this.BamlSchemaContext.GetString(stringId));
			if (xamlDirective == XamlLanguage.Key)
			{
				this._context.CurrentFrame.Key = new KeyRecord(false, false, 0, text);
				return;
			}
			this.Common_Process_Property();
			this._xamlNodesWriter.WriteStartMember(xamlDirective);
			this._xamlNodesWriter.WriteValue(text);
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0003E978 File Offset: 0x0003CB78
		private void Process_DefAttributeKeyString()
		{
			this.Read_RecordSize();
			short stringId = this._binaryReader.ReadInt16();
			int valuePosition = this._binaryReader.ReadInt32();
			bool shared = this._binaryReader.ReadBoolean();
			bool sharedSet = this._binaryReader.ReadBoolean();
			string @string = this._context.SchemaContext.GetString(stringId);
			KeyRecord keyRecord = new KeyRecord(shared, sharedSet, valuePosition, @string);
			if (this._context.CurrentFrame.IsDeferredContent)
			{
				this._context.KeyList.Add(keyRecord);
				return;
			}
			this._context.CurrentFrame.Key = keyRecord;
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0003EA14 File Offset: 0x0003CC14
		private void Process_DefAttributeKeyType()
		{
			short typeId = this._binaryReader.ReadInt16();
			byte b = this._binaryReader.ReadByte();
			int valuePosition = this._binaryReader.ReadInt32();
			bool shared = this._binaryReader.ReadBoolean();
			bool sharedSet = this._binaryReader.ReadBoolean();
			Type type = Baml2006SchemaContext.KnownTypes.GetKnownType(typeId);
			if (type == null)
			{
				type = this.BamlSchemaContext.GetClrType(typeId);
			}
			KeyRecord keyRecord = new KeyRecord(shared, sharedSet, valuePosition, type);
			if (this._context.CurrentFrame.IsDeferredContent)
			{
				this._context.KeyList.Add(keyRecord);
				return;
			}
			this._context.CurrentFrame.Key = keyRecord;
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0003EAC4 File Offset: 0x0003CCC4
		private bool IsStringOnlyWhiteSpace(string value)
		{
			for (int i = 0; i < value.Length; i++)
			{
				if (!char.IsWhiteSpace(value[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0003EAF4 File Offset: 0x0003CCF4
		private void Process_Text()
		{
			this.Read_RecordSize();
			string stringValue = this._binaryReader.ReadString();
			this.Process_Text_Helper(stringValue);
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0003EB1C File Offset: 0x0003CD1C
		private void Process_TextWithId()
		{
			this.Read_RecordSize();
			short stringId = this._binaryReader.ReadInt16();
			string @string = this.BamlSchemaContext.GetString(stringId);
			this.Process_Text_Helper(@string);
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0003EB50 File Offset: 0x0003CD50
		private void Process_Text_Helper(string stringValue)
		{
			if (!this._context.InsideKeyRecord && !this._context.InsideStaticResource)
			{
				this.InjectPropertyAndFrameIfNeeded(this._context.SchemaContext.GetXamlType(typeof(string)), 0);
			}
			if (this.IsStringOnlyWhiteSpace(stringValue) && this._context.CurrentFrame.Member != XamlLanguage.PositionalParameters)
			{
				if (this._context.CurrentFrame.XamlType != null && this._context.CurrentFrame.XamlType.IsCollection)
				{
					if (!this._context.CurrentFrame.XamlType.IsWhitespaceSignificantCollection)
					{
						return;
					}
				}
				else if (this._context.CurrentFrame.Member.Type != null && !this._context.CurrentFrame.Member.Type.UnderlyingType.IsAssignableFrom(typeof(string)))
				{
					return;
				}
			}
			this._xamlNodesWriter.WriteValue(stringValue);
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0003EC5F File Offset: 0x0003CE5F
		private void Process_ConstructorParametersEnd()
		{
			this._xamlNodesWriter.WriteEndMember();
			this._context.CurrentFrame.Member = null;
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0003EC7D File Offset: 0x0003CE7D
		private void Process_ConstructorParametersStart()
		{
			this.Common_Process_Property();
			this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
			this._context.CurrentFrame.Member = XamlLanguage.PositionalParameters;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0003ECAC File Offset: 0x0003CEAC
		private void Process_ConstructorParameterType()
		{
			short typeId = this._binaryReader.ReadInt16();
			if (this._isBinaryProvider)
			{
				this._xamlNodesWriter.WriteValue(this.BamlSchemaContext.GetXamlType(typeId).UnderlyingType);
				return;
			}
			this._xamlNodesWriter.WriteStartObject(XamlLanguage.Type);
			this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
			this._xamlNodesWriter.WriteValue(this.Logic_GetFullyQualifiedNameForType(this.BamlSchemaContext.GetXamlType(typeId)));
			this._xamlNodesWriter.WriteEndMember();
			this._xamlNodesWriter.WriteEndObject();
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0003ED40 File Offset: 0x0003CF40
		private void Process_Header()
		{
			int count = this._binaryReader.ReadInt32();
			byte[] array = this._binaryReader.ReadBytes(count);
			int num = this._binaryReader.ReadInt32();
			int num2 = this._binaryReader.ReadInt32();
			int num3 = this._binaryReader.ReadInt32();
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0003ED8C File Offset: 0x0003CF8C
		private void Process_ElementStart()
		{
			short typeId = this._binaryReader.ReadInt16();
			XamlType xamlType;
			if (this._root != null && this._context.CurrentFrame.Depth == 0)
			{
				Type type = this._root.GetType();
				xamlType = this.BamlSchemaContext.GetXamlType(type);
			}
			else
			{
				xamlType = this.BamlSchemaContext.GetXamlType(typeId);
			}
			sbyte b = this._binaryReader.ReadSByte();
			if (b < 0 || b > 3)
			{
				throw new System.Xaml.XamlParseException();
			}
			this.InjectPropertyAndFrameIfNeeded(xamlType, b);
			this._context.PushScope();
			this._context.CurrentFrame.XamlType = xamlType;
			bool flag = true;
			for (;;)
			{
				Baml2006RecordType baml2006RecordType = this.Read_RecordType();
				if (baml2006RecordType <= Baml2006RecordType.AssemblyInfo)
				{
					if (baml2006RecordType != Baml2006RecordType.XmlnsProperty)
					{
						if (baml2006RecordType != Baml2006RecordType.AssemblyInfo)
						{
							goto IL_DA;
						}
						this.Process_AssemblyInfo();
					}
					else
					{
						this.Process_XmlnsProperty();
					}
				}
				else if (baml2006RecordType != Baml2006RecordType.LineNumberAndPosition)
				{
					if (baml2006RecordType != Baml2006RecordType.LinePosition)
					{
						goto IL_DA;
					}
					this.Process_LinePosition();
				}
				else
				{
					this.Process_LineNumberAndPosition();
				}
				IL_E4:
				if (!flag)
				{
					break;
				}
				continue;
				IL_DA:
				this.SkipBytes(-1L);
				flag = false;
				goto IL_E4;
			}
			bool flag2 = (b & 2) > 0;
			if (flag2)
			{
				this._xamlNodesWriter.WriteGetObject();
			}
			else
			{
				this._xamlNodesWriter.WriteStartObject(this._context.CurrentFrame.XamlType);
			}
			if (this._context.CurrentFrame.Depth == 1 && this._settings.BaseUri != null && !string.IsNullOrEmpty(this._settings.BaseUri.ToString()))
			{
				this._xamlNodesWriter.WriteStartMember(XamlLanguage.Base);
				this._xamlNodesWriter.WriteValue(this._settings.BaseUri.ToString());
				this._xamlNodesWriter.WriteEndMember();
			}
			if (this._context.PreviousFrame.IsDeferredContent && !this._context.InsideStaticResource)
			{
				if (!this._isBinaryProvider)
				{
					this._xamlNodesWriter.WriteStartMember(XamlLanguage.Key);
					KeyRecord keyRecord = this._context.KeyList[this._context.CurrentKey];
					if (!string.IsNullOrEmpty(keyRecord.KeyString))
					{
						this._xamlNodesWriter.WriteValue(keyRecord.KeyString);
					}
					else if (keyRecord.KeyType != null)
					{
						this._xamlNodesWriter.WriteStartObject(XamlLanguage.Type);
						this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
						this._xamlNodesWriter.WriteValue(this.Logic_GetFullyQualifiedNameForType(this.SchemaContext.GetXamlType(keyRecord.KeyType)));
						this._xamlNodesWriter.WriteEndMember();
						this._xamlNodesWriter.WriteEndObject();
					}
					else
					{
						XamlServices.Transform(keyRecord.KeyNodeList.GetReader(), this._xamlNodesWriter, false);
					}
					this._xamlNodesWriter.WriteEndMember();
				}
				Baml2006ReaderContext context = this._context;
				int currentKey = context.CurrentKey;
				context.CurrentKey = currentKey + 1;
			}
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0003F050 File Offset: 0x0003D250
		private void Process_ElementEnd()
		{
			this.RemoveImplicitFrame();
			if (this._context.CurrentFrame.Key != null)
			{
				this._xamlNodesWriter.WriteStartMember(XamlLanguage.Key);
				KeyRecord key = this._context.CurrentFrame.Key;
				if (key.KeyType != null)
				{
					if (this._isBinaryProvider)
					{
						this._xamlNodesWriter.WriteValue(key.KeyType);
					}
					else
					{
						this._xamlNodesWriter.WriteStartObject(XamlLanguage.Type);
						this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
						this._xamlNodesWriter.WriteValue(this.Logic_GetFullyQualifiedNameForType(this.SchemaContext.GetXamlType(key.KeyType)));
						this._xamlNodesWriter.WriteEndMember();
						this._xamlNodesWriter.WriteEndObject();
					}
				}
				else if (key.KeyNodeList != null)
				{
					XamlServices.Transform(key.KeyNodeList.GetReader(), this._xamlNodesWriter, false);
				}
				else
				{
					this._xamlNodesWriter.WriteValue(key.KeyString);
				}
				this._xamlNodesWriter.WriteEndMember();
				this._context.CurrentFrame.Key = null;
			}
			if (this._context.CurrentFrame.DelayedConnectionId != -1)
			{
				this._xamlNodesWriter.WriteStartMember(XamlLanguage.ConnectionId);
				if (this._isBinaryProvider)
				{
					this._xamlNodesWriter.WriteValue(this._context.CurrentFrame.DelayedConnectionId);
				}
				else
				{
					this._xamlNodesWriter.WriteValue(this._context.CurrentFrame.DelayedConnectionId.ToString(TypeConverterHelper.InvariantEnglishUS));
				}
				this._xamlNodesWriter.WriteEndMember();
			}
			this._xamlNodesWriter.WriteEndObject();
			if (this._context.CurrentFrame.IsDeferredContent)
			{
				this._context.KeyList = null;
			}
			this._context.PopScope();
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0003F224 File Offset: 0x0003D424
		private void Process_KeyElementStart()
		{
			short typeId = this._binaryReader.ReadInt16();
			byte flags = this._binaryReader.ReadByte();
			int valuePosition = this._binaryReader.ReadInt32();
			bool shared = this._binaryReader.ReadBoolean();
			bool sharedSet = this._binaryReader.ReadBoolean();
			XamlType xamlType = this._context.SchemaContext.GetXamlType(typeId);
			this._context.PushScope();
			this._context.CurrentFrame.XamlType = xamlType;
			KeyRecord keyRecord = new KeyRecord(shared, sharedSet, valuePosition, this._context.SchemaContext);
			keyRecord.Flags = flags;
			keyRecord.KeyNodeList.Writer.WriteStartObject(xamlType);
			this._context.InsideKeyRecord = true;
			this._xamlWriterStack.Push(this._xamlNodesWriter);
			this._xamlNodesWriter = keyRecord.KeyNodeList.Writer;
			if (this._context.PreviousFrame.IsDeferredContent)
			{
				this._context.KeyList.Add(keyRecord);
				return;
			}
			this._context.PreviousFrame.Key = keyRecord;
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0003F338 File Offset: 0x0003D538
		private void Process_KeyElementEnd()
		{
			KeyRecord keyRecord;
			if (this._context.PreviousFrame.IsDeferredContent)
			{
				keyRecord = this._context.LastKey;
			}
			else
			{
				keyRecord = this._context.PreviousFrame.Key;
			}
			keyRecord.KeyNodeList.Writer.WriteEndObject();
			keyRecord.KeyNodeList.Writer.Close();
			this._xamlNodesWriter = this._xamlWriterStack.Pop();
			this._context.InsideKeyRecord = false;
			this._context.PopScope();
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0003F3C0 File Offset: 0x0003D5C0
		private void Process_Property()
		{
			this.Common_Process_Property();
			this.Read_RecordSize();
			if (this._context.CurrentFrame.XamlType.UnderlyingType == typeof(EventSetter))
			{
				this._xamlNodesWriter.WriteStartMember(this._context.SchemaContext.EventSetterEventProperty);
				XamlMember property = this.GetProperty(this._binaryReader.ReadInt16(), false);
				Type type = property.DeclaringType.UnderlyingType;
				while (null != type)
				{
					SecurityHelper.RunClassConstructor(type);
					type = type.BaseType;
				}
				RoutedEvent routedEventFromName = EventManager.GetRoutedEventFromName(property.Name, property.DeclaringType.UnderlyingType);
				this._xamlNodesWriter.WriteValue(routedEventFromName);
				this._xamlNodesWriter.WriteEndMember();
				this._xamlNodesWriter.WriteStartMember(this._context.SchemaContext.EventSetterHandlerProperty);
				this._xamlNodesWriter.WriteValue(this._binaryReader.ReadString());
				this._xamlNodesWriter.WriteEndMember();
				return;
			}
			XamlMember property2 = this.GetProperty(this._binaryReader.ReadInt16(), this._context.CurrentFrame.XamlType);
			this._xamlNodesWriter.WriteStartMember(property2);
			this._xamlNodesWriter.WriteValue(this._binaryReader.ReadString());
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0003F510 File Offset: 0x0003D710
		private void Common_Process_Property()
		{
			if (this._context.InsideKeyRecord || this._context.InsideStaticResource)
			{
				return;
			}
			this.RemoveImplicitFrame();
			if (this._context.CurrentFrame.XamlType == null)
			{
				throw new System.Xaml.XamlParseException(SR.Get("PropertyFoundOutsideStartElement"));
			}
			if (this._context.CurrentFrame.Member != null)
			{
				throw new System.Xaml.XamlParseException(SR.Get("PropertyOutOfOrder", new object[]
				{
					this._context.CurrentFrame.Member
				}));
			}
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0003F5A8 File Offset: 0x0003D7A8
		private Int32Collection GetInt32Collection()
		{
			BinaryReader binaryReader = new BinaryReader(this._binaryReader.BaseStream);
			XamlInt32CollectionSerializer.IntegerCollectionType integerCollectionType = (XamlInt32CollectionSerializer.IntegerCollectionType)binaryReader.ReadByte();
			int num = binaryReader.ReadInt32();
			if (num < 0)
			{
				throw new ArgumentException(SR.Get("IntegerCollectionLengthLessThanZero", new object[0]));
			}
			Int32Collection int32Collection = new Int32Collection(num);
			switch (integerCollectionType)
			{
			case XamlInt32CollectionSerializer.IntegerCollectionType.Consecutive:
			{
				int num2 = binaryReader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					int32Collection.Add(num2 + i);
				}
				return int32Collection;
			}
			case XamlInt32CollectionSerializer.IntegerCollectionType.Byte:
				for (int j = 0; j < num; j++)
				{
					int32Collection.Add((int)binaryReader.ReadByte());
				}
				return int32Collection;
			case XamlInt32CollectionSerializer.IntegerCollectionType.UShort:
				for (int k = 0; k < num; k++)
				{
					int32Collection.Add((int)binaryReader.ReadUInt16());
				}
				return int32Collection;
			case XamlInt32CollectionSerializer.IntegerCollectionType.Integer:
				for (int l = 0; l < num; l++)
				{
					int value = binaryReader.ReadInt32();
					int32Collection.Add(value);
				}
				return int32Collection;
			default:
				throw new InvalidOperationException(SR.Get("UnableToConvertInt32"));
			}
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0003F6A4 File Offset: 0x0003D8A4
		private XamlMember GetProperty(short propertyId, XamlType parentType)
		{
			return this.BamlSchemaContext.GetProperty(propertyId, parentType);
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0003F6C0 File Offset: 0x0003D8C0
		private XamlMember GetProperty(short propertyId, bool isAttached)
		{
			return this.BamlSchemaContext.GetProperty(propertyId, isAttached);
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0003F6DC File Offset: 0x0003D8DC
		private void Process_PropertyCustom()
		{
			this.Common_Process_Property();
			int num = this.Read_RecordSize();
			XamlMember property = this.GetProperty(this._binaryReader.ReadInt16(), this._context.CurrentFrame.XamlType);
			this._xamlNodesWriter.WriteStartMember(property);
			short num2 = this._binaryReader.ReadInt16();
			if ((num2 & 16384) == 16384)
			{
				num2 &= -16385;
			}
			if (this._isBinaryProvider)
			{
				this.WriteTypeConvertedInstance(num2, num - 5);
			}
			else
			{
				this._xamlNodesWriter.WriteValue(this.GetTextFromBinary(this._binaryReader.ReadBytes(num - 5), num2, property, this._context.CurrentFrame.XamlType));
			}
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0003F798 File Offset: 0x0003D998
		private bool WriteTypeConvertedInstance(short converterId, int dataByteSize)
		{
			if (converterId <= 137)
			{
				if (converterId == 46)
				{
					this._xamlNodesWriter.WriteValue(this._binaryReader.ReadBytes(1)[0] != 0);
					return true;
				}
				if (converterId == 137)
				{
					DependencyProperty value;
					if (dataByteSize == 2)
					{
						value = this.BamlSchemaContext.GetDependencyProperty(this._binaryReader.ReadInt16());
					}
					else
					{
						Type underlyingType = this.BamlSchemaContext.GetXamlType(this._binaryReader.ReadInt16()).UnderlyingType;
						value = DependencyProperty.FromName(this._binaryReader.ReadString(), underlyingType);
					}
					this._xamlNodesWriter.WriteValue(value);
					return true;
				}
			}
			else
			{
				if (converterId == 195)
				{
					TypeConverter typeConverter = new EnumConverter(this._context.CurrentFrame.XamlType.UnderlyingType);
					this._xamlNodesWriter.WriteValue(typeConverter.ConvertFrom(this._binaryReader.ReadBytes(dataByteSize)));
					return true;
				}
				if (converterId == 615)
				{
					this._xamlNodesWriter.WriteValue(this._binaryReader.ReadString());
					return true;
				}
				switch (converterId)
				{
				case 744:
				case 746:
				case 747:
				case 748:
				case 752:
				{
					DeferredBinaryDeserializerExtension value2 = new DeferredBinaryDeserializerExtension(this, this._binaryReader, (int)converterId, dataByteSize);
					this._xamlNodesWriter.WriteValue(value2);
					return true;
				}
				case 745:
					this._xamlNodesWriter.WriteValue(this.GetInt32Collection());
					return true;
				}
			}
			throw new NotImplementedException();
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0003F920 File Offset: 0x0003DB20
		private void Process_PropertyWithConverter()
		{
			this.Common_Process_Property();
			this.Read_RecordSize();
			XamlMember property = this.GetProperty(this._binaryReader.ReadInt16(), this._context.CurrentFrame.XamlType);
			this._xamlNodesWriter.WriteStartMember(property);
			object obj = this._binaryReader.ReadString();
			short num = this._binaryReader.ReadInt16();
			if (this._isBinaryProvider && num < 0 && -num != 615)
			{
				TypeConverter typeConverter = null;
				if (-num == 195)
				{
					Type underlyingType = property.Type.UnderlyingType;
					if (underlyingType.IsEnum && !this._enumTypeConverterMap.TryGetValue(underlyingType, out typeConverter))
					{
						typeConverter = new EnumConverter(underlyingType);
						this._enumTypeConverterMap[underlyingType] = typeConverter;
					}
				}
				else if (!this._typeConverterMap.TryGetValue((int)num, out typeConverter))
				{
					typeConverter = Baml2006SchemaContext.KnownTypes.CreateKnownTypeConverter(num);
					this._typeConverterMap[(int)num] = typeConverter;
				}
				if (typeConverter != null)
				{
					obj = this.CreateTypeConverterMarkupExtension(property, typeConverter, obj, this._settings);
				}
			}
			this._xamlNodesWriter.WriteValue(obj);
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0003FA36 File Offset: 0x0003DC36
		internal virtual object CreateTypeConverterMarkupExtension(XamlMember property, TypeConverter converter, object propertyValue, Baml2006ReaderSettings settings)
		{
			return new TypeConverterMarkupExtension(converter, propertyValue);
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0003FA40 File Offset: 0x0003DC40
		private void Process_PropertyWithExtension()
		{
			this.Common_Process_Property();
			short propertyId = this._binaryReader.ReadInt16();
			short num = this._binaryReader.ReadInt16();
			short num2 = this._binaryReader.ReadInt16();
			XamlMember property = this.GetProperty(propertyId, this._context.CurrentFrame.XamlType);
			short num3 = num & 4095;
			XamlType xamlType = this.BamlSchemaContext.GetXamlType(-num3);
			bool flag = (num & 16384) == 16384;
			bool flag2 = (num & 8192) == 8192;
			Type type = null;
			object value = null;
			this._xamlNodesWriter.WriteStartMember(property);
			bool flag3 = false;
			if (this._isBinaryProvider)
			{
				object obj = null;
				object obj2;
				if (flag2)
				{
					Type memberType = null;
					string staticExtensionValue = this.GetStaticExtensionValue(num2, out memberType, out obj);
					if (obj != null)
					{
						obj2 = obj;
					}
					else
					{
						obj2 = new StaticExtension(staticExtensionValue)
						{
							MemberType = memberType
						}.ProvideValue(null);
					}
				}
				else if (flag)
				{
					obj2 = this.BamlSchemaContext.GetXamlType(num2).UnderlyingType;
				}
				else if (num3 == 634)
				{
					obj2 = this._context.SchemaContext.GetDependencyProperty(num2);
				}
				else if (num3 == 602)
				{
					obj2 = this.GetStaticExtensionValue(num2, out type, out obj);
				}
				else if (num3 == 691)
				{
					obj2 = this.BamlSchemaContext.GetXamlType(num2).UnderlyingType;
				}
				else
				{
					obj2 = this.BamlSchemaContext.GetString(num2);
				}
				if (num3 == 189)
				{
					value = new DynamicResourceExtension(obj2);
					flag3 = true;
				}
				else if (num3 == 603)
				{
					value = new StaticResourceExtension(obj2);
					flag3 = true;
				}
				else if (num3 == 634)
				{
					value = new TemplateBindingExtension((DependencyProperty)obj2);
					flag3 = true;
				}
				else if (num3 == 691)
				{
					value = obj2;
					flag3 = true;
				}
				else if (num3 == 602)
				{
					if (obj != null)
					{
						value = obj;
					}
					else
					{
						value = new StaticExtension((string)obj2)
						{
							MemberType = type
						};
					}
					flag3 = true;
				}
				if (flag3)
				{
					this._xamlNodesWriter.WriteValue(value);
					this._xamlNodesWriter.WriteEndMember();
					return;
				}
			}
			if (!flag3)
			{
				this._xamlNodesWriter.WriteStartObject(xamlType);
				this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
				if (flag2)
				{
					Type type2 = null;
					object obj3;
					value = this.GetStaticExtensionValue(num2, out type2, out obj3);
					if (obj3 != null)
					{
						this._xamlNodesWriter.WriteValue(obj3);
					}
					else
					{
						this._xamlNodesWriter.WriteStartObject(XamlLanguage.Static);
						this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
						this._xamlNodesWriter.WriteValue(value);
						this._xamlNodesWriter.WriteEndMember();
						if (type2 != null)
						{
							this._xamlNodesWriter.WriteStartMember(this.BamlSchemaContext.StaticExtensionMemberTypeProperty);
							this._xamlNodesWriter.WriteValue(type2);
							this._xamlNodesWriter.WriteEndMember();
						}
						this._xamlNodesWriter.WriteEndObject();
					}
				}
				else if (flag)
				{
					this._xamlNodesWriter.WriteStartObject(XamlLanguage.Type);
					this._xamlNodesWriter.WriteStartMember(this.BamlSchemaContext.TypeExtensionTypeProperty);
					Type underlyingType = this.BamlSchemaContext.GetXamlType(num2).UnderlyingType;
					if (this._isBinaryProvider)
					{
						this._xamlNodesWriter.WriteValue(underlyingType);
					}
					else
					{
						this._xamlNodesWriter.WriteValue(this.Logic_GetFullyQualifiedNameForType(this.BamlSchemaContext.GetXamlType(num2)));
					}
					this._xamlNodesWriter.WriteEndMember();
					this._xamlNodesWriter.WriteEndObject();
				}
				else
				{
					if (num3 == 634)
					{
						if (this._isBinaryProvider)
						{
							value = BitConverter.GetBytes(num2);
						}
						else
						{
							value = this.Logic_GetFullyQualifiedNameForMember(num2);
						}
					}
					else if (num3 == 602)
					{
						object obj4;
						value = this.GetStaticExtensionValue(num2, out type, out obj4);
					}
					else if (num3 == 691)
					{
						value = this.BamlSchemaContext.GetXamlType(num2).UnderlyingType;
					}
					else
					{
						value = this.BamlSchemaContext.GetString(num2);
					}
					this._xamlNodesWriter.WriteValue(value);
				}
				this._xamlNodesWriter.WriteEndMember();
				if (type != null)
				{
					this._xamlNodesWriter.WriteStartMember(this.BamlSchemaContext.StaticExtensionMemberTypeProperty);
					this._xamlNodesWriter.WriteValue(type);
					this._xamlNodesWriter.WriteEndMember();
				}
			}
			this._xamlNodesWriter.WriteEndObject();
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0003FE84 File Offset: 0x0003E084
		private void Process_PropertyTypeReference()
		{
			this.Common_Process_Property();
			XamlMember property = this.GetProperty(this._binaryReader.ReadInt16(), this._context.CurrentFrame.XamlType);
			XamlType xamlType = this.BamlSchemaContext.GetXamlType(this._binaryReader.ReadInt16());
			this._xamlNodesWriter.WriteStartMember(property);
			if (this._isBinaryProvider)
			{
				this._xamlNodesWriter.WriteValue(xamlType.UnderlyingType);
			}
			else
			{
				this._xamlNodesWriter.WriteStartObject(XamlLanguage.Type);
				this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
				this._xamlNodesWriter.WriteValue(this.Logic_GetFullyQualifiedNameForType(xamlType));
				this._xamlNodesWriter.WriteEndMember();
				this._xamlNodesWriter.WriteEndObject();
			}
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0003FF4C File Offset: 0x0003E14C
		private void Process_PropertyWithStaticResourceId()
		{
			this.Common_Process_Property();
			short propertyId = this._binaryReader.ReadInt16();
			short index = this._binaryReader.ReadInt16();
			XamlMember property = this._context.SchemaContext.GetProperty(propertyId, this._context.CurrentFrame.XamlType);
			object obj = this._context.KeyList[this._context.CurrentKey - 1].StaticResources[(int)index];
			if (obj is StaticResourceHolder)
			{
				this._xamlNodesWriter.WriteStartMember(property);
				this._xamlNodesWriter.WriteValue(obj);
				this._xamlNodesWriter.WriteEndMember();
				return;
			}
			this._xamlNodesWriter.WriteStartMember(property);
			this._xamlNodesWriter.WriteStartObject(this.BamlSchemaContext.StaticResourceExtensionType);
			this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
			OptimizedStaticResource optimizedStaticResource = obj as OptimizedStaticResource;
			if (optimizedStaticResource != null)
			{
				if (optimizedStaticResource.IsKeyStaticExtension)
				{
					Type type = null;
					object obj2;
					string staticExtensionValue = this.GetStaticExtensionValue(optimizedStaticResource.KeyId, out type, out obj2);
					if (obj2 != null)
					{
						this._xamlNodesWriter.WriteValue(obj2);
					}
					else
					{
						this._xamlNodesWriter.WriteStartObject(XamlLanguage.Static);
						this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
						this._xamlNodesWriter.WriteValue(staticExtensionValue);
						this._xamlNodesWriter.WriteEndMember();
						if (type != null)
						{
							this._xamlNodesWriter.WriteStartMember(this.BamlSchemaContext.StaticExtensionMemberTypeProperty);
							this._xamlNodesWriter.WriteValue(type);
							this._xamlNodesWriter.WriteEndMember();
						}
						this._xamlNodesWriter.WriteEndObject();
					}
				}
				else if (optimizedStaticResource.IsKeyTypeExtension)
				{
					if (this._isBinaryProvider)
					{
						XamlType xamlType = this.BamlSchemaContext.GetXamlType(optimizedStaticResource.KeyId);
						this._xamlNodesWriter.WriteValue(xamlType.UnderlyingType);
					}
					else
					{
						this._xamlNodesWriter.WriteStartObject(XamlLanguage.Type);
						this._xamlNodesWriter.WriteStartMember(XamlLanguage.PositionalParameters);
						this._xamlNodesWriter.WriteValue(this.Logic_GetFullyQualifiedNameForType(this.BamlSchemaContext.GetXamlType(optimizedStaticResource.KeyId)));
						this._xamlNodesWriter.WriteEndMember();
						this._xamlNodesWriter.WriteEndObject();
					}
				}
				else
				{
					string @string = this._context.SchemaContext.GetString(optimizedStaticResource.KeyId);
					this._xamlNodesWriter.WriteValue(@string);
				}
			}
			else
			{
				StaticResource staticResource = obj as StaticResource;
				XamlServices.Transform(staticResource.ResourceNodeList.GetReader(), this._xamlNodesWriter, false);
			}
			this._xamlNodesWriter.WriteEndMember();
			this._xamlNodesWriter.WriteEndObject();
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x000401EC File Offset: 0x0003E3EC
		private void Process_PropertyComplexStart()
		{
			this.Common_Process_Property();
			XamlMember property = this.GetProperty(this._binaryReader.ReadInt16(), this._context.CurrentFrame.XamlType);
			this._context.CurrentFrame.Member = property;
			this._xamlNodesWriter.WriteStartMember(property);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00040240 File Offset: 0x0003E440
		private void Process_PropertyArrayStart()
		{
			this.Common_Process_Property();
			XamlMember property = this.GetProperty(this._binaryReader.ReadInt16(), this._context.CurrentFrame.XamlType);
			this._context.CurrentFrame.Member = property;
			this._xamlNodesWriter.WriteStartMember(property);
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x00040294 File Offset: 0x0003E494
		private void Process_PropertyIDictionaryStart()
		{
			this.Common_Process_Property();
			XamlMember property = this.GetProperty(this._binaryReader.ReadInt16(), this._context.CurrentFrame.XamlType);
			this._context.CurrentFrame.Member = property;
			this._xamlNodesWriter.WriteStartMember(property);
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x000402E6 File Offset: 0x0003E4E6
		private void Process_PropertyEnd()
		{
			this.RemoveImplicitFrame();
			this._context.CurrentFrame.Member = null;
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0004030C File Offset: 0x0003E50C
		private void Process_PropertyIDictionaryEnd()
		{
			if (this._lookingForAKeyOnAMarkupExtensionInADictionaryDepth == this._context.CurrentFrame.Depth)
			{
				this.RestoreSavedFirstItemInDictionary();
			}
			this.RemoveImplicitFrame();
			this._context.CurrentFrame.Member = null;
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00040359 File Offset: 0x0003E559
		private string Logic_GetFullyQualifiedNameForMember(short propertyId)
		{
			return this.Logic_GetFullyQualifiedNameForType(this.BamlSchemaContext.GetPropertyDeclaringType(propertyId)) + "." + this.BamlSchemaContext.GetPropertyName(propertyId, false);
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00040384 File Offset: 0x0003E584
		private string Logic_GetFullyQualifiedNameForType(XamlType type)
		{
			Baml2006ReaderFrame baml2006ReaderFrame = this._context.CurrentFrame;
			IList<string> xamlNamespaces = type.GetXamlNamespaces();
			while (baml2006ReaderFrame != null)
			{
				foreach (string xamlNs in xamlNamespaces)
				{
					string text = null;
					if (baml2006ReaderFrame.TryGetPrefixByNamespace(xamlNs, out text))
					{
						if (string.IsNullOrEmpty(text))
						{
							return type.Name;
						}
						return text + ":" + type.Name;
					}
				}
				baml2006ReaderFrame = (Baml2006ReaderFrame)baml2006ReaderFrame.Previous;
			}
			throw new InvalidOperationException("Could not find prefix for type: " + type.Name);
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00040438 File Offset: 0x0003E638
		private string Logic_GetFullXmlns(string uriInput)
		{
			int num = uriInput.IndexOf(":");
			if (num != -1)
			{
				string a = uriInput.Substring(0, num);
				if (string.Equals(a, "clr-namespace"))
				{
					int num2 = num + 1;
					int num3 = uriInput.IndexOf(";");
					if (-1 == num3)
					{
						return uriInput + ((this._settings.LocalAssembly != null) ? (";assembly=" + this.GetAssemblyNameForNamespace(this._settings.LocalAssembly)) : string.Empty);
					}
					int num4 = num3 + 1;
					int num5 = uriInput.IndexOf("=");
					if (-1 == num5)
					{
						throw new ArgumentException(SR.Get("MissingTagInNamespace", new object[]
						{
							"=",
							uriInput
						}));
					}
					string a2 = uriInput.Substring(num4, num5 - num4);
					if (!string.Equals(a2, "assembly"))
					{
						throw new ArgumentException(SR.Get("AssemblyTagMissing", new object[]
						{
							"assembly",
							uriInput
						}));
					}
					string value = uriInput.Substring(num5 + 1);
					if (string.IsNullOrEmpty(value))
					{
						return uriInput + this.GetAssemblyNameForNamespace(this._settings.LocalAssembly);
					}
				}
			}
			return uriInput;
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00040568 File Offset: 0x0003E768
		internal virtual string GetAssemblyNameForNamespace(Assembly assembly)
		{
			string fullName = assembly.FullName;
			return fullName.Substring(0, fullName.IndexOf(','));
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x00040590 File Offset: 0x0003E790
		private void Process_XmlnsProperty()
		{
			this.Read_RecordSize();
			string prefix = this._binaryReader.ReadString();
			string text = this._binaryReader.ReadString();
			text = this.Logic_GetFullXmlns(text);
			this._context.CurrentFrame.AddNamespace(prefix, text);
			NamespaceDeclaration namespaceDeclaration = new NamespaceDeclaration(text, prefix);
			this._xamlNodesWriter.WriteNamespace(namespaceDeclaration);
			short num = this._binaryReader.ReadInt16();
			if (text.StartsWith("clr-namespace:", StringComparison.Ordinal))
			{
				this.SkipBytes((long)(num * 2));
				return;
			}
			if (num > 0)
			{
				short[] array = new short[(int)num];
				for (int i = 0; i < (int)num; i++)
				{
					array[i] = this._binaryReader.ReadInt16();
				}
				this.BamlSchemaContext.AddXmlnsMapping(text, array);
			}
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0004064C File Offset: 0x0003E84C
		private void Process_LinePosition()
		{
			this._context.LineOffset = this._binaryReader.ReadInt32();
			IXamlLineInfoConsumer xamlLineInfoConsumer = this._xamlNodesWriter as IXamlLineInfoConsumer;
			if (xamlLineInfoConsumer != null)
			{
				xamlLineInfoConsumer.SetLineInfo(this._context.LineNumber, this._context.LineOffset);
			}
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0004069C File Offset: 0x0003E89C
		private void Process_LineNumberAndPosition()
		{
			this._context.LineNumber = this._binaryReader.ReadInt32();
			this._context.LineOffset = this._binaryReader.ReadInt32();
			IXamlLineInfoConsumer xamlLineInfoConsumer = this._xamlNodesWriter as IXamlLineInfoConsumer;
			if (xamlLineInfoConsumer != null)
			{
				xamlLineInfoConsumer.SetLineInfo(this._context.LineNumber, this._context.LineOffset);
			}
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x00040700 File Offset: 0x0003E900
		private void Process_PIMapping()
		{
			this.Read_RecordSize();
			string text = this._binaryReader.ReadString();
			string text2 = this._binaryReader.ReadString();
			short num = this._binaryReader.ReadInt16();
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x00040738 File Offset: 0x0003E938
		private void Process_AssemblyInfo()
		{
			this.Read_RecordSize();
			short assemblyId = this._binaryReader.ReadInt16();
			string assemblyName = this._binaryReader.ReadString();
			this.BamlSchemaContext.AddAssembly(assemblyId, assemblyName);
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x00040774 File Offset: 0x0003E974
		private void Process_TypeInfo()
		{
			this.Read_RecordSize();
			short typeId = this._binaryReader.ReadInt16();
			short num = this._binaryReader.ReadInt16();
			string typeName = this._binaryReader.ReadString();
			Baml2006SchemaContext.TypeInfoFlags flags = (Baml2006SchemaContext.TypeInfoFlags)(num >> 12);
			num &= 4095;
			this.BamlSchemaContext.AddXamlType(typeId, num, typeName, flags);
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x000407CC File Offset: 0x0003E9CC
		private void Process_TypeSerializerInfo()
		{
			this.Read_RecordSize();
			short typeId = this._binaryReader.ReadInt16();
			short num = this._binaryReader.ReadInt16();
			string typeName = this._binaryReader.ReadString();
			short num2 = this._binaryReader.ReadInt16();
			Baml2006SchemaContext.TypeInfoFlags flags = (Baml2006SchemaContext.TypeInfoFlags)(num >> 12);
			num &= 4095;
			this.BamlSchemaContext.AddXamlType(typeId, num, typeName, flags);
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x00040830 File Offset: 0x0003EA30
		private void Process_AttributeInfo()
		{
			this.Read_RecordSize();
			short propertyId = this._binaryReader.ReadInt16();
			short declaringTypeId = this._binaryReader.ReadInt16();
			byte b = this._binaryReader.ReadByte();
			string propertyName = this._binaryReader.ReadString();
			this.BamlSchemaContext.AddProperty(propertyId, declaringTypeId, propertyName);
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x00040884 File Offset: 0x0003EA84
		private void Process_StringInfo()
		{
			this.Read_RecordSize();
			short stringId = this._binaryReader.ReadInt16();
			string value = this._binaryReader.ReadString();
			this.BamlSchemaContext.AddString(stringId, value);
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x000408C0 File Offset: 0x0003EAC0
		private void Process_ContentProperty()
		{
			short num = this._binaryReader.ReadInt16();
			if (num != -174)
			{
				XamlMember xamlMember = this.GetProperty(num, false);
				WpfXamlMember wpfXamlMember = xamlMember as WpfXamlMember;
				if (wpfXamlMember != null)
				{
					xamlMember = wpfXamlMember.AsContentProperty;
				}
				this._context.CurrentFrame.ContentProperty = xamlMember;
			}
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x00040914 File Offset: 0x0003EB14
		private void Process_ConnectionId()
		{
			int num = this._binaryReader.ReadInt32();
			if (this._context.CurrentFrame.Member != null)
			{
				Baml2006ReaderFrame baml2006ReaderFrame = this._context.CurrentFrame;
				if (baml2006ReaderFrame.Flags == Baml2006ReaderFrameFlags.IsImplict)
				{
					baml2006ReaderFrame = this._context.PreviousFrame;
				}
				baml2006ReaderFrame.DelayedConnectionId = num;
				return;
			}
			this.Common_Process_Property();
			this._xamlNodesWriter.WriteStartMember(XamlLanguage.ConnectionId);
			if (this._isBinaryProvider)
			{
				this._xamlNodesWriter.WriteValue(num);
			}
			else
			{
				this._xamlNodesWriter.WriteValue(num.ToString(TypeConverterHelper.InvariantEnglishUS));
			}
			this._xamlNodesWriter.WriteEndMember();
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x000409C4 File Offset: 0x0003EBC4
		private Baml2006RecordType Read_RecordType()
		{
			byte b = this._binaryReader.ReadByte();
			if (b < 0)
			{
				return Baml2006RecordType.DocumentEnd;
			}
			return (Baml2006RecordType)b;
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x000409E4 File Offset: 0x0003EBE4
		private int Read_RecordSize()
		{
			long position = this._binaryReader.BaseStream.Position;
			int num = this._binaryReader.Read7BitEncodedInt();
			int num2 = (int)(this._binaryReader.BaseStream.Position - position);
			if (num2 == 1)
			{
				return num;
			}
			return num - num2 + 1;
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x00040A2D File Offset: 0x0003EC2D
		private void SkipBytes(long offset)
		{
			this._binaryReader.BaseStream.Seek(offset, SeekOrigin.Current);
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x00040A44 File Offset: 0x0003EC44
		private void RemoveImplicitFrame()
		{
			if (this._context.CurrentFrame.Flags == Baml2006ReaderFrameFlags.IsImplict)
			{
				this._xamlNodesWriter.WriteEndMember();
				this._xamlNodesWriter.WriteEndObject();
				this._context.PopScope();
			}
			if (this._context.CurrentFrame.Flags == Baml2006ReaderFrameFlags.HasImplicitProperty)
			{
				if (this._context.CurrentFrame.Depth == this._context.TemplateStartDepth)
				{
					this._xamlNodesWriter.Close();
					this._xamlNodesWriter = this._xamlWriterStack.Pop();
					this._xamlNodesWriter.WriteValue(this._xamlTemplateNodeList);
					this._xamlTemplateNodeList = null;
					this._context.TemplateStartDepth = -1;
				}
				this._xamlNodesWriter.WriteEndMember();
				this._context.CurrentFrame.Member = null;
				this._context.CurrentFrame.Flags = Baml2006ReaderFrameFlags.None;
			}
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x00040B28 File Offset: 0x0003ED28
		private void InjectPropertyAndFrameIfNeeded(XamlType elementType, sbyte flags)
		{
			if (this._lookingForAKeyOnAMarkupExtensionInADictionaryDepth == this._context.CurrentFrame.Depth)
			{
				this.RestoreSavedFirstItemInDictionary();
			}
			XamlType xamlType = this._context.CurrentFrame.XamlType;
			XamlMember xamlMember = this._context.CurrentFrame.Member;
			if (xamlType != null)
			{
				if (xamlMember == null)
				{
					if (this._context.CurrentFrame.ContentProperty != null)
					{
						xamlMember = (this._context.CurrentFrame.Member = this._context.CurrentFrame.ContentProperty);
					}
					else if (xamlType.ContentProperty != null)
					{
						xamlMember = (this._context.CurrentFrame.Member = xamlType.ContentProperty);
					}
					else if (xamlType.IsCollection || xamlType.IsDictionary)
					{
						xamlMember = (this._context.CurrentFrame.Member = XamlLanguage.Items);
					}
					else
					{
						if (!(xamlType.TypeConverter != null))
						{
							throw new System.Xaml.XamlParseException(SR.Get("RecordOutOfOrder", new object[]
							{
								xamlType.Name
							}));
						}
						xamlMember = (this._context.CurrentFrame.Member = XamlLanguage.Initialization);
					}
					this._context.CurrentFrame.Flags = Baml2006ReaderFrameFlags.HasImplicitProperty;
					this._xamlNodesWriter.WriteStartMember(xamlMember);
					if (this._context.TemplateStartDepth < 0 && this._isBinaryProvider && xamlMember == this.BamlSchemaContext.FrameworkTemplateTemplateProperty)
					{
						this._context.TemplateStartDepth = this._context.CurrentFrame.Depth;
						this._xamlTemplateNodeList = new XamlNodeList(this._xamlNodesWriter.SchemaContext);
						this._xamlWriterStack.Push(this._xamlNodesWriter);
						this._xamlNodesWriter = this._xamlTemplateNodeList.Writer;
						if (XamlSourceInfoHelper.IsXamlSourceInfoEnabled)
						{
							IXamlLineInfoConsumer xamlLineInfoConsumer = this._xamlNodesWriter as IXamlLineInfoConsumer;
							if (xamlLineInfoConsumer != null)
							{
								xamlLineInfoConsumer.SetLineInfo(this._context.LineNumber, this._context.LineOffset);
							}
						}
					}
				}
				XamlType type = xamlMember.Type;
				if (type != null && (type.IsCollection || type.IsDictionary) && !xamlMember.IsDirective && (flags & 2) == 0)
				{
					bool flag = false;
					if (xamlMember.IsReadOnly)
					{
						flag = true;
					}
					else if (!elementType.CanAssignTo(type))
					{
						if (!elementType.IsMarkupExtension)
						{
							flag = true;
						}
						else if (this._context.CurrentFrame.Flags == Baml2006ReaderFrameFlags.HasImplicitProperty)
						{
							flag = true;
						}
						else if (elementType == XamlLanguage.Array)
						{
							flag = true;
						}
					}
					if (flag)
					{
						this.EmitGoItemsPreamble(type);
					}
					if (!flag && type.IsDictionary && elementType.IsMarkupExtension)
					{
						this.StartSavingFirstItemInDictionary();
					}
				}
			}
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x00040DE0 File Offset: 0x0003EFE0
		private void StartSavingFirstItemInDictionary()
		{
			this._lookingForAKeyOnAMarkupExtensionInADictionaryDepth = this._context.CurrentFrame.Depth;
			this._lookingForAKeyOnAMarkupExtensionInADictionaryNodeList = new XamlNodeList(this._xamlNodesWriter.SchemaContext);
			this._xamlWriterStack.Push(this._xamlNodesWriter);
			this._xamlNodesWriter = this._lookingForAKeyOnAMarkupExtensionInADictionaryNodeList.Writer;
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x00040E3C File Offset: 0x0003F03C
		private void RestoreSavedFirstItemInDictionary()
		{
			this._xamlNodesWriter.Close();
			this._xamlNodesWriter = this._xamlWriterStack.Pop();
			if (this.NodeListHasAKeySetOnTheRoot(this._lookingForAKeyOnAMarkupExtensionInADictionaryNodeList.GetReader()))
			{
				this.EmitGoItemsPreamble(this._context.CurrentFrame.Member.Type);
			}
			System.Xaml.XamlReader reader = this._lookingForAKeyOnAMarkupExtensionInADictionaryNodeList.GetReader();
			XamlServices.Transform(reader, this._xamlNodesWriter, false);
			this._lookingForAKeyOnAMarkupExtensionInADictionaryDepth = -1;
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x00040EB4 File Offset: 0x0003F0B4
		private void EmitGoItemsPreamble(XamlType parentPropertyType)
		{
			this._context.PushScope();
			this._context.CurrentFrame.XamlType = parentPropertyType;
			this._xamlNodesWriter.WriteGetObject();
			this._context.CurrentFrame.Flags = Baml2006ReaderFrameFlags.IsImplict;
			this._context.CurrentFrame.Member = XamlLanguage.Items;
			this._xamlNodesWriter.WriteStartMember(this._context.CurrentFrame.Member);
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x00040F29 File Offset: 0x0003F129
		private StaticResource GetLastStaticResource()
		{
			return this._context.LastKey.LastStaticResource;
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x00040F3C File Offset: 0x0003F13C
		private string GetTextFromBinary(byte[] bytes, short serializerId, XamlMember property, XamlType type)
		{
			if (serializerId <= 46)
			{
				if (serializerId != 0)
				{
					if (serializerId != 46)
					{
						goto IL_29A;
					}
					if (bytes[0] != 0)
					{
						return true.ToString();
					}
					return false.ToString();
				}
			}
			else if (serializerId != 137)
			{
				if (serializerId == 195)
				{
					return Enum.ToObject(type.UnderlyingType, bytes).ToString();
				}
				switch (serializerId)
				{
				case 744:
					using (MemoryStream memoryStream = new MemoryStream(bytes))
					{
						using (BinaryReader binaryReader = new BinaryReader(memoryStream))
						{
							SolidColorBrush solidColorBrush = SolidColorBrush.DeserializeFrom(binaryReader) as SolidColorBrush;
							return solidColorBrush.ToString();
						}
					}
					break;
				case 745:
					goto IL_1E8;
				case 746:
					break;
				case 747:
					goto IL_10D;
				case 748:
					goto IL_19F;
				case 749:
				case 750:
				case 751:
					goto IL_29A;
				case 752:
					goto IL_156;
				default:
					goto IL_29A;
				}
				using (MemoryStream memoryStream2 = new MemoryStream(bytes))
				{
					using (BinaryReader binaryReader2 = new BinaryReader(memoryStream2))
					{
						XamlPathDataSerializer xamlPathDataSerializer = new XamlPathDataSerializer();
						object obj = xamlPathDataSerializer.ConvertCustomBinaryToObject(binaryReader2);
						return obj.ToString();
					}
				}
				IL_10D:
				using (MemoryStream memoryStream3 = new MemoryStream(bytes))
				{
					using (BinaryReader binaryReader3 = new BinaryReader(memoryStream3))
					{
						XamlPoint3DCollectionSerializer xamlPoint3DCollectionSerializer = new XamlPoint3DCollectionSerializer();
						object obj2 = xamlPoint3DCollectionSerializer.ConvertCustomBinaryToObject(binaryReader3);
						return obj2.ToString();
					}
				}
				IL_156:
				using (MemoryStream memoryStream4 = new MemoryStream(bytes))
				{
					using (BinaryReader binaryReader4 = new BinaryReader(memoryStream4))
					{
						XamlVector3DCollectionSerializer xamlVector3DCollectionSerializer = new XamlVector3DCollectionSerializer();
						object obj3 = xamlVector3DCollectionSerializer.ConvertCustomBinaryToObject(binaryReader4);
						return obj3.ToString();
					}
				}
				IL_19F:
				using (MemoryStream memoryStream5 = new MemoryStream(bytes))
				{
					using (BinaryReader binaryReader5 = new BinaryReader(memoryStream5))
					{
						XamlPointCollectionSerializer xamlPointCollectionSerializer = new XamlPointCollectionSerializer();
						object obj4 = xamlPointCollectionSerializer.ConvertCustomBinaryToObject(binaryReader5);
						return obj4.ToString();
					}
				}
				IL_1E8:
				using (MemoryStream memoryStream6 = new MemoryStream(bytes))
				{
					using (BinaryReader binaryReader6 = new BinaryReader(memoryStream6))
					{
						XamlInt32CollectionSerializer xamlInt32CollectionSerializer = new XamlInt32CollectionSerializer();
						object obj5 = xamlInt32CollectionSerializer.ConvertCustomBinaryToObject(binaryReader6);
						return obj5.ToString();
					}
				}
			}
			if (bytes.Length == 2)
			{
				short propertyId = (short)((int)bytes[0] | (int)bytes[1] << 8);
				return this.Logic_GetFullyQualifiedNameForMember(propertyId);
			}
			using (BinaryReader binaryReader7 = new BinaryReader(new MemoryStream(bytes)))
			{
				XamlType xamlType = this.BamlSchemaContext.GetXamlType(binaryReader7.ReadInt16());
				string str = binaryReader7.ReadString();
				return this.Logic_GetFullyQualifiedNameForType(xamlType) + "." + str;
			}
			IL_29A:
			throw new NotImplementedException();
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0004128C File Offset: 0x0003F48C
		private string GetStaticExtensionValue(short valueId, out Type memberType, out object providedValue)
		{
			string text = "";
			memberType = null;
			providedValue = null;
			if (valueId < 0)
			{
				valueId = -valueId;
				bool flag = true;
				valueId = SystemResourceKey.GetSystemResourceKeyIdFromBamlId(valueId, out flag);
				if (valueId <= 0 || valueId >= 236)
				{
					throw new InvalidOperationException(SR.Get("BamlBadExtensionValue"));
				}
				if (this._isBinaryProvider)
				{
					if (flag)
					{
						providedValue = SystemResourceKey.GetResourceKey(valueId);
					}
					else
					{
						providedValue = SystemResourceKey.GetResource(valueId);
					}
				}
				else
				{
					SystemResourceKeyID id = (SystemResourceKeyID)valueId;
					XamlType xamlType = this._context.SchemaContext.GetXamlType(SystemKeyConverter.GetSystemClassType(id));
					text = this.Logic_GetFullyQualifiedNameForType(xamlType) + ".";
					if (flag)
					{
						text += SystemKeyConverter.GetSystemKeyName(id);
					}
					else
					{
						text += SystemKeyConverter.GetSystemPropertyName(id);
					}
				}
			}
			else if (this._isBinaryProvider)
			{
				memberType = this.BamlSchemaContext.GetPropertyDeclaringType(valueId).UnderlyingType;
				text = this.BamlSchemaContext.GetPropertyName(valueId, false);
				providedValue = CommandConverter.GetKnownControlCommand(memberType, text);
			}
			else
			{
				text = this.Logic_GetFullyQualifiedNameForMember(valueId);
			}
			return text;
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x00041388 File Offset: 0x0003F588
		private bool NodeListHasAKeySetOnTheRoot(System.Xaml.XamlReader reader)
		{
			int num = 0;
			while (reader.Read())
			{
				switch (reader.NodeType)
				{
				case System.Xaml.XamlNodeType.StartObject:
					num++;
					break;
				case System.Xaml.XamlNodeType.EndObject:
					num--;
					break;
				case System.Xaml.XamlNodeType.StartMember:
					if (reader.Member == XamlLanguage.Key && num == 1)
					{
						return true;
					}
					break;
				}
			}
			return false;
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001036 RID: 4150 RVA: 0x000413E7 File Offset: 0x0003F5E7
		// (set) Token: 0x06001037 RID: 4151 RVA: 0x000413F9 File Offset: 0x0003F5F9
		internal bool FreezeFreezables
		{
			get
			{
				return this._context.CurrentFrame.FreezeFreezables;
			}
			set
			{
				this._context.CurrentFrame.FreezeFreezables = value;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001038 RID: 4152 RVA: 0x000413E7 File Offset: 0x0003F5E7
		bool IFreezeFreezables.FreezeFreezables
		{
			get
			{
				return this._context.CurrentFrame.FreezeFreezables;
			}
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0004140C File Offset: 0x0003F60C
		bool IFreezeFreezables.TryFreeze(string value, Freezable freezable)
		{
			if (freezable.CanFreeze)
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

		// Token: 0x0600103A RID: 4154 RVA: 0x00041448 File Offset: 0x0003F648
		Freezable IFreezeFreezables.TryGetFreezable(string value)
		{
			Freezable result = null;
			if (this._freezeCache != null)
			{
				this._freezeCache.TryGetValue(value, out result);
			}
			return result;
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x0004146F File Offset: 0x0003F66F
		private Baml2006SchemaContext BamlSchemaContext
		{
			get
			{
				return (Baml2006SchemaContext)this.SchemaContext;
			}
		}

		// Token: 0x040011C2 RID: 4546
		private Baml2006ReaderSettings _settings;

		// Token: 0x040011C3 RID: 4547
		private bool _isBinaryProvider;

		// Token: 0x040011C4 RID: 4548
		private bool _isEof;

		// Token: 0x040011C5 RID: 4549
		private int _lookingForAKeyOnAMarkupExtensionInADictionaryDepth;

		// Token: 0x040011C6 RID: 4550
		private XamlNodeList _lookingForAKeyOnAMarkupExtensionInADictionaryNodeList;

		// Token: 0x040011C7 RID: 4551
		private BamlBinaryReader _binaryReader;

		// Token: 0x040011C8 RID: 4552
		private Baml2006ReaderContext _context;

		// Token: 0x040011C9 RID: 4553
		private XamlNodeQueue _xamlMainNodeQueue;

		// Token: 0x040011CA RID: 4554
		private XamlNodeList _xamlTemplateNodeList;

		// Token: 0x040011CB RID: 4555
		private System.Xaml.XamlReader _xamlNodesReader;

		// Token: 0x040011CC RID: 4556
		private System.Xaml.XamlWriter _xamlNodesWriter;

		// Token: 0x040011CD RID: 4557
		private Stack<System.Xaml.XamlWriter> _xamlWriterStack = new Stack<System.Xaml.XamlWriter>();

		// Token: 0x040011CE RID: 4558
		private Dictionary<int, TypeConverter> _typeConverterMap = new Dictionary<int, TypeConverter>();

		// Token: 0x040011CF RID: 4559
		private Dictionary<Type, TypeConverter> _enumTypeConverterMap = new Dictionary<Type, TypeConverter>();

		// Token: 0x040011D0 RID: 4560
		private Dictionary<string, Freezable> _freezeCache;

		// Token: 0x040011D1 RID: 4561
		private const short ExtensionIdMask = 4095;

		// Token: 0x040011D2 RID: 4562
		private const short TypeExtensionValueMask = 16384;

		// Token: 0x040011D3 RID: 4563
		private const short StaticExtensionValueMask = 8192;

		// Token: 0x040011D4 RID: 4564
		private const sbyte ReaderFlags_AddedToTree = 2;

		// Token: 0x040011D5 RID: 4565
		private object _root;
	}
}
