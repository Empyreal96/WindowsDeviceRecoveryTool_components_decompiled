using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Xml;

namespace System.Resources
{
	/// <summary>Enumerates XML resource (.resx) files and streams, and reads the sequential resource name and value pairs.</summary>
	// Token: 0x020000EF RID: 239
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class ResXResourceReader : IResourceReader, IEnumerable, IDisposable
	{
		// Token: 0x06000368 RID: 872 RVA: 0x0000A4E1 File Offset: 0x000086E1
		private ResXResourceReader(ITypeResolutionService typeResolver)
		{
			this.typeResolver = typeResolver;
			this.aliasResolver = new ResXResourceReader.ReaderAliasResolver();
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000A4FB File Offset: 0x000086FB
		private ResXResourceReader(AssemblyName[] assemblyNames)
		{
			this.assemblyNames = assemblyNames;
			this.aliasResolver = new ResXResourceReader.ReaderAliasResolver();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResXResourceReader" /> class for the specified resource file.</summary>
		/// <param name="fileName">The path of the resource file to read. </param>
		// Token: 0x0600036A RID: 874 RVA: 0x0000A515 File Offset: 0x00008715
		public ResXResourceReader(string fileName) : this(fileName, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResXResourceReader" /> class using a file name and a type resolution service. </summary>
		/// <param name="fileName">The name of an XML resource file that contains resources. </param>
		/// <param name="typeResolver">An object that resolves type names specified in a resource.</param>
		// Token: 0x0600036B RID: 875 RVA: 0x0000A520 File Offset: 0x00008720
		public ResXResourceReader(string fileName, ITypeResolutionService typeResolver) : this(fileName, typeResolver, null)
		{
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000A52B File Offset: 0x0000872B
		internal ResXResourceReader(string fileName, ITypeResolutionService typeResolver, IAliasResolver aliasResolver)
		{
			this.fileName = fileName;
			this.typeResolver = typeResolver;
			this.aliasResolver = aliasResolver;
			if (this.aliasResolver == null)
			{
				this.aliasResolver = new ResXResourceReader.ReaderAliasResolver();
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResXResourceReader" /> class for the specified <see cref="T:System.IO.TextReader" />.</summary>
		/// <param name="reader">A text input stream that contains resources. </param>
		// Token: 0x0600036D RID: 877 RVA: 0x0000A55B File Offset: 0x0000875B
		public ResXResourceReader(TextReader reader) : this(reader, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResXResourceReader" /> class using a text stream reader and a type resolution service.  </summary>
		/// <param name="reader">A text stream reader that contains resources. </param>
		/// <param name="typeResolver">An object that resolves type names specified in a resource.</param>
		// Token: 0x0600036E RID: 878 RVA: 0x0000A566 File Offset: 0x00008766
		public ResXResourceReader(TextReader reader, ITypeResolutionService typeResolver) : this(reader, typeResolver, null)
		{
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000A571 File Offset: 0x00008771
		internal ResXResourceReader(TextReader reader, ITypeResolutionService typeResolver, IAliasResolver aliasResolver)
		{
			this.reader = reader;
			this.typeResolver = typeResolver;
			this.aliasResolver = aliasResolver;
			if (this.aliasResolver == null)
			{
				this.aliasResolver = new ResXResourceReader.ReaderAliasResolver();
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResXResourceReader" /> class for the specified stream.</summary>
		/// <param name="stream">An input stream that contains resources. </param>
		// Token: 0x06000370 RID: 880 RVA: 0x0000A5A1 File Offset: 0x000087A1
		public ResXResourceReader(Stream stream) : this(stream, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResXResourceReader" /> class using an input stream and a type resolution service.  </summary>
		/// <param name="stream">An input stream that contains resources. </param>
		/// <param name="typeResolver">An object that resolves type names specified in a resource.</param>
		// Token: 0x06000371 RID: 881 RVA: 0x0000A5AC File Offset: 0x000087AC
		public ResXResourceReader(Stream stream, ITypeResolutionService typeResolver) : this(stream, typeResolver, null)
		{
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000A5B7 File Offset: 0x000087B7
		internal ResXResourceReader(Stream stream, ITypeResolutionService typeResolver, IAliasResolver aliasResolver)
		{
			this.stream = stream;
			this.typeResolver = typeResolver;
			this.aliasResolver = aliasResolver;
			if (this.aliasResolver == null)
			{
				this.aliasResolver = new ResXResourceReader.ReaderAliasResolver();
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResXResourceReader" /> class using a stream and an array of assembly names. </summary>
		/// <param name="stream">An input stream that contains resources. </param>
		/// <param name="assemblyNames">An array of <see cref="T:System.Reflection.AssemblyName" /> objects that specifies one or more assemblies. The assemblies are used to resolve a type name in the resource to an actual type. </param>
		// Token: 0x06000373 RID: 883 RVA: 0x0000A5E7 File Offset: 0x000087E7
		public ResXResourceReader(Stream stream, AssemblyName[] assemblyNames) : this(stream, assemblyNames, null)
		{
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000A5F2 File Offset: 0x000087F2
		internal ResXResourceReader(Stream stream, AssemblyName[] assemblyNames, IAliasResolver aliasResolver)
		{
			this.stream = stream;
			this.assemblyNames = assemblyNames;
			this.aliasResolver = aliasResolver;
			if (this.aliasResolver == null)
			{
				this.aliasResolver = new ResXResourceReader.ReaderAliasResolver();
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResXResourceReader" /> class using a <see cref="T:System.IO.TextReader" /> object and an array of assembly names.</summary>
		/// <param name="reader">An object used to read resources from a stream of text. </param>
		/// <param name="assemblyNames">An array of <see cref="T:System.Reflection.AssemblyName" /> objects that specifies one or more assemblies. The assemblies are used to resolve a type name in the resource to an actual type. </param>
		// Token: 0x06000375 RID: 885 RVA: 0x0000A622 File Offset: 0x00008822
		public ResXResourceReader(TextReader reader, AssemblyName[] assemblyNames) : this(reader, assemblyNames, null)
		{
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000A62D File Offset: 0x0000882D
		internal ResXResourceReader(TextReader reader, AssemblyName[] assemblyNames, IAliasResolver aliasResolver)
		{
			this.reader = reader;
			this.assemblyNames = assemblyNames;
			this.aliasResolver = aliasResolver;
			if (this.aliasResolver == null)
			{
				this.aliasResolver = new ResXResourceReader.ReaderAliasResolver();
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResXResourceReader" /> class using an XML resource file name and an array of assembly names. </summary>
		/// <param name="fileName">The name of an XML resource file that contains resources. </param>
		/// <param name="assemblyNames">An array of <see cref="T:System.Reflection.AssemblyName" /> objects that specifies one or more assemblies. The assemblies are used to resolve a type name in the resource to an actual type. </param>
		// Token: 0x06000377 RID: 887 RVA: 0x0000A65D File Offset: 0x0000885D
		public ResXResourceReader(string fileName, AssemblyName[] assemblyNames) : this(fileName, assemblyNames, null)
		{
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000A668 File Offset: 0x00008868
		internal ResXResourceReader(string fileName, AssemblyName[] assemblyNames, IAliasResolver aliasResolver)
		{
			this.fileName = fileName;
			this.assemblyNames = assemblyNames;
			this.aliasResolver = aliasResolver;
			if (this.aliasResolver == null)
			{
				this.aliasResolver = new ResXResourceReader.ReaderAliasResolver();
			}
		}

		/// <summary>This member overrides the <see cref="M:System.Object.Finalize" /> method. </summary>
		// Token: 0x06000379 RID: 889 RVA: 0x0000A698 File Offset: 0x00008898
		~ResXResourceReader()
		{
			this.Dispose(false);
		}

		/// <summary>Gets or sets the base path for the relative file path specified in a <see cref="T:System.Resources.ResXFileRef" /> object.</summary>
		/// <returns>A path that, if prepended to the relative file path specified in a <see cref="T:System.Resources.ResXFileRef" /> object, yields an absolute path to a resource file.</returns>
		/// <exception cref="T:System.InvalidOperationException">In a set operation, a value cannot be specified because the XML resource file has already been accessed and is in use.</exception>
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600037A RID: 890 RVA: 0x0000A6C8 File Offset: 0x000088C8
		// (set) Token: 0x0600037B RID: 891 RVA: 0x0000A6D0 File Offset: 0x000088D0
		public string BasePath
		{
			get
			{
				return this.basePath;
			}
			set
			{
				if (this.isReaderDirty)
				{
					throw new InvalidOperationException(SR.GetString("InvalidResXBasePathOperation"));
				}
				this.basePath = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether <see cref="T:System.Resources.ResXDataNode" /> objects are returned when reading the current XML resource file or stream.</summary>
		/// <returns>
		///     <see langword="true" /> if resource data nodes are retrieved; <see langword="false" /> if resource data nodes are ignored.</returns>
		/// <exception cref="T:System.InvalidOperationException">In a set operation, the enumerator for the resource file or stream is already open.</exception>
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600037C RID: 892 RVA: 0x0000A6F1 File Offset: 0x000088F1
		// (set) Token: 0x0600037D RID: 893 RVA: 0x0000A6F9 File Offset: 0x000088F9
		public bool UseResXDataNodes
		{
			get
			{
				return this.useResXDataNodes;
			}
			set
			{
				if (this.isReaderDirty)
				{
					throw new InvalidOperationException(SR.GetString("InvalidResXBasePathOperation"));
				}
				this.useResXDataNodes = value;
			}
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Resources.ResXResourceReader" />.</summary>
		// Token: 0x0600037E RID: 894 RVA: 0x0000A71A File Offset: 0x0000891A
		public void Close()
		{
			((IDisposable)this).Dispose();
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Resources.ResXResourceReader" /> and optionally releases the managed resources. For a description of this member, see the <see cref="M:System.IDisposable.Dispose" /> method. </summary>
		// Token: 0x0600037F RID: 895 RVA: 0x0000A722 File Offset: 0x00008922
		void IDisposable.Dispose()
		{
			GC.SuppressFinalize(this);
			this.Dispose(true);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Resources.ResXResourceReader" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06000380 RID: 896 RVA: 0x0000A734 File Offset: 0x00008934
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.fileName != null && this.stream != null)
				{
					this.stream.Close();
					this.stream = null;
				}
				if (this.reader != null)
				{
					this.reader.Close();
					this.reader = null;
				}
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000A780 File Offset: 0x00008980
		private void SetupNameTable(XmlReader reader)
		{
			reader.NameTable.Add("type");
			reader.NameTable.Add("name");
			reader.NameTable.Add("data");
			reader.NameTable.Add("metadata");
			reader.NameTable.Add("mimetype");
			reader.NameTable.Add("value");
			reader.NameTable.Add("resheader");
			reader.NameTable.Add("version");
			reader.NameTable.Add("resmimetype");
			reader.NameTable.Add("reader");
			reader.NameTable.Add("writer");
			reader.NameTable.Add(ResXResourceWriter.BinSerializedObjectMimeType);
			reader.NameTable.Add(ResXResourceWriter.SoapSerializedObjectMimeType);
			reader.NameTable.Add("assembly");
			reader.NameTable.Add("alias");
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000A88C File Offset: 0x00008A8C
		private void EnsureResData()
		{
			if (this.resData == null)
			{
				this.resData = new ListDictionary();
				this.resMetadata = new ListDictionary();
				XmlTextReader xmlTextReader = null;
				try
				{
					if (this.fileContents != null)
					{
						xmlTextReader = new XmlTextReader(new StringReader(this.fileContents));
					}
					else if (this.reader != null)
					{
						xmlTextReader = new XmlTextReader(this.reader);
					}
					else if (this.fileName != null || this.stream != null)
					{
						if (this.stream == null)
						{
							this.stream = new FileStream(this.fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
						}
						xmlTextReader = new XmlTextReader(this.stream);
					}
					this.SetupNameTable(xmlTextReader);
					xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
					this.ParseXml(xmlTextReader);
				}
				finally
				{
					if (this.fileName != null && this.stream != null)
					{
						this.stream.Close();
						this.stream = null;
					}
				}
			}
		}

		/// <summary>Creates a new <see cref="T:System.Resources.ResXResourceReader" /> object and initializes it to read a string whose contents are in the form of an XML resource file.</summary>
		/// <param name="fileContents">A string containing XML resource-formatted information. </param>
		/// <returns>An object that reads resources from the <paramref name="fileContents" /> string.</returns>
		// Token: 0x06000383 RID: 899 RVA: 0x0000A970 File Offset: 0x00008B70
		public static ResXResourceReader FromFileContents(string fileContents)
		{
			return ResXResourceReader.FromFileContents(fileContents, null);
		}

		/// <summary>Creates a new <see cref="T:System.Resources.ResXResourceReader" /> object and initializes it to read a string whose contents are in the form of an XML resource file, and to use an <see cref="T:System.ComponentModel.Design.ITypeResolutionService" /> object to resolve type names specified in a resource.</summary>
		/// <param name="fileContents">A string containing XML resource-formatted information. </param>
		/// <param name="typeResolver">An object that resolves type names specified in a resource.</param>
		/// <returns>An object that reads resources from the <paramref name="fileContents" /> string.</returns>
		// Token: 0x06000384 RID: 900 RVA: 0x0000A97C File Offset: 0x00008B7C
		public static ResXResourceReader FromFileContents(string fileContents, ITypeResolutionService typeResolver)
		{
			return new ResXResourceReader(typeResolver)
			{
				fileContents = fileContents
			};
		}

		/// <summary>Creates a new <see cref="T:System.Resources.ResXResourceReader" /> object and initializes it to read a string whose contents are in the form of an XML resource file, and to use an array of <see cref="T:System.Reflection.AssemblyName" /> objects to resolve type names specified in a resource. </summary>
		/// <param name="fileContents">A string whose contents are in the form of an XML resource file. </param>
		/// <param name="assemblyNames">An array of <see cref="T:System.Reflection.AssemblyName" /> objects that specifies one or more assemblies. The assemblies are used to resolve a type name in the resource to an actual type. </param>
		/// <returns>An object that reads resources from the <paramref name="fileContents" /> string.</returns>
		// Token: 0x06000385 RID: 901 RVA: 0x0000A998 File Offset: 0x00008B98
		public static ResXResourceReader FromFileContents(string fileContents, AssemblyName[] assemblyNames)
		{
			return new ResXResourceReader(assemblyNames)
			{
				fileContents = fileContents
			};
		}

		/// <summary>Returns an enumerator for the current <see cref="T:System.Resources.ResXResourceReader" /> object. For a description of this member, see the <see cref="M:System.Collections.IEnumerable.GetEnumerator" /> method. </summary>
		/// <returns>An enumerator that can iterate through the name/value pairs in the XML resource (.resx) stream or string associated with the current <see cref="T:System.Resources.ResXResourceReader" /> object.</returns>
		// Token: 0x06000386 RID: 902 RVA: 0x0000A9B4 File Offset: 0x00008BB4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>Returns an enumerator for the current <see cref="T:System.Resources.ResXResourceReader" /> object.</summary>
		/// <returns>An enumerator for the current <see cref="T:System.Resources.ResourceReader" /> object.</returns>
		// Token: 0x06000387 RID: 903 RVA: 0x0000A9BC File Offset: 0x00008BBC
		public IDictionaryEnumerator GetEnumerator()
		{
			this.isReaderDirty = true;
			this.EnsureResData();
			return this.resData.GetEnumerator();
		}

		/// <summary>Provides a dictionary enumerator that can retrieve the design-time properties from the current XML resource file or stream.</summary>
		/// <returns>An enumerator for the metadata in a resource.</returns>
		// Token: 0x06000388 RID: 904 RVA: 0x0000A9D6 File Offset: 0x00008BD6
		public IDictionaryEnumerator GetMetadataEnumerator()
		{
			this.EnsureResData();
			return this.resMetadata.GetEnumerator();
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000A9EC File Offset: 0x00008BEC
		private Point GetPosition(XmlReader reader)
		{
			Point result = new Point(0, 0);
			IXmlLineInfo xmlLineInfo = reader as IXmlLineInfo;
			if (xmlLineInfo != null)
			{
				result.Y = xmlLineInfo.LineNumber;
				result.X = xmlLineInfo.LinePosition;
			}
			return result;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000AA28 File Offset: 0x00008C28
		private void ParseXml(XmlTextReader reader)
		{
			bool flag = false;
			try
			{
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						string localName = reader.LocalName;
						if (reader.LocalName.Equals("assembly"))
						{
							this.ParseAssemblyNode(reader, false);
						}
						else if (reader.LocalName.Equals("data"))
						{
							this.ParseDataNode(reader, false);
						}
						else if (reader.LocalName.Equals("resheader"))
						{
							this.ParseResHeaderNode(reader);
						}
						else if (reader.LocalName.Equals("metadata"))
						{
							this.ParseDataNode(reader, true);
						}
					}
				}
				flag = true;
			}
			catch (SerializationException ex)
			{
				Point position = this.GetPosition(reader);
				string @string = SR.GetString("SerializationException", new object[]
				{
					reader["type"],
					position.Y,
					position.X,
					ex.Message
				});
				XmlException innerException = new XmlException(@string, ex, position.Y, position.X);
				SerializationException ex2 = new SerializationException(@string, innerException);
				throw ex2;
			}
			catch (TargetInvocationException ex3)
			{
				Point position2 = this.GetPosition(reader);
				string string2 = SR.GetString("InvocationException", new object[]
				{
					reader["type"],
					position2.Y,
					position2.X,
					ex3.InnerException.Message
				});
				XmlException inner = new XmlException(string2, ex3.InnerException, position2.Y, position2.X);
				TargetInvocationException ex4 = new TargetInvocationException(string2, inner);
				throw ex4;
			}
			catch (XmlException ex5)
			{
				throw new ArgumentException(SR.GetString("InvalidResXFile", new object[]
				{
					ex5.Message
				}), ex5);
			}
			catch (Exception ex6)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex6))
				{
					throw;
				}
				Point position3 = this.GetPosition(reader);
				XmlException ex7 = new XmlException(ex6.Message, ex6, position3.Y, position3.X);
				throw new ArgumentException(SR.GetString("InvalidResXFile", new object[]
				{
					ex7.Message
				}), ex7);
			}
			finally
			{
				if (!flag)
				{
					this.resData = null;
					this.resMetadata = null;
				}
			}
			bool flag2 = false;
			if (object.Equals(this.resHeaderMimeType, ResXResourceWriter.ResMimeType))
			{
				Type typeFromHandle = typeof(ResXResourceReader);
				Type typeFromHandle2 = typeof(ResXResourceWriter);
				string text = this.resHeaderReaderType;
				string text2 = this.resHeaderWriterType;
				if (text != null && text.IndexOf(',') != -1)
				{
					text = text.Split(new char[]
					{
						','
					})[0].Trim();
				}
				if (text2 != null && text2.IndexOf(',') != -1)
				{
					text2 = text2.Split(new char[]
					{
						','
					})[0].Trim();
				}
				if (text != null && text2 != null && text.Equals(typeFromHandle.FullName) && text2.Equals(typeFromHandle2.FullName))
				{
					flag2 = true;
				}
			}
			if (!flag2)
			{
				this.resData = null;
				this.resMetadata = null;
				throw new ArgumentException(SR.GetString("InvalidResXFileReaderWriterTypes"));
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000ADAC File Offset: 0x00008FAC
		private void ParseResHeaderNode(XmlReader reader)
		{
			string text = reader["name"];
			if (text != null)
			{
				reader.ReadStartElement();
				if (object.Equals(text, "version"))
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						this.resHeaderVersion = reader.ReadElementString();
						return;
					}
					this.resHeaderVersion = reader.Value.Trim();
					return;
				}
				else if (object.Equals(text, "resmimetype"))
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						this.resHeaderMimeType = reader.ReadElementString();
						return;
					}
					this.resHeaderMimeType = reader.Value.Trim();
					return;
				}
				else if (object.Equals(text, "reader"))
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						this.resHeaderReaderType = reader.ReadElementString();
						return;
					}
					this.resHeaderReaderType = reader.Value.Trim();
					return;
				}
				else if (object.Equals(text, "writer"))
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						this.resHeaderWriterType = reader.ReadElementString();
						return;
					}
					this.resHeaderWriterType = reader.Value.Trim();
					return;
				}
				else
				{
					string a = text.ToLower(CultureInfo.InvariantCulture);
					if (!(a == "version"))
					{
						if (!(a == "resmimetype"))
						{
							if (!(a == "reader"))
							{
								if (!(a == "writer"))
								{
									return;
								}
								if (reader.NodeType == XmlNodeType.Element)
								{
									this.resHeaderWriterType = reader.ReadElementString();
									return;
								}
								this.resHeaderWriterType = reader.Value.Trim();
							}
							else
							{
								if (reader.NodeType == XmlNodeType.Element)
								{
									this.resHeaderReaderType = reader.ReadElementString();
									return;
								}
								this.resHeaderReaderType = reader.Value.Trim();
								return;
							}
						}
						else
						{
							if (reader.NodeType == XmlNodeType.Element)
							{
								this.resHeaderMimeType = reader.ReadElementString();
								return;
							}
							this.resHeaderMimeType = reader.Value.Trim();
							return;
						}
					}
					else
					{
						if (reader.NodeType == XmlNodeType.Element)
						{
							this.resHeaderVersion = reader.ReadElementString();
							return;
						}
						this.resHeaderVersion = reader.Value.Trim();
						return;
					}
				}
			}
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000AF88 File Offset: 0x00009188
		private void ParseAssemblyNode(XmlReader reader, bool isMetaData)
		{
			string text = reader["alias"];
			string assemblyName = reader["name"];
			AssemblyName assemblyName2 = new AssemblyName(assemblyName);
			if (string.IsNullOrEmpty(text))
			{
				text = assemblyName2.Name;
			}
			this.aliasResolver.PushAlias(text, assemblyName2);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000AFD0 File Offset: 0x000091D0
		private void ParseDataNode(XmlTextReader reader, bool isMetaData)
		{
			DataNodeInfo dataNodeInfo = new DataNodeInfo();
			dataNodeInfo.Name = reader["name"];
			string text = reader["type"];
			string text2 = null;
			AssemblyName assemblyName = null;
			if (!string.IsNullOrEmpty(text))
			{
				text2 = this.GetAliasFromTypeName(text);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				assemblyName = this.aliasResolver.ResolveAlias(text2);
			}
			if (assemblyName != null)
			{
				dataNodeInfo.TypeName = this.GetTypeFromTypeName(text) + ", " + assemblyName.FullName;
			}
			else
			{
				dataNodeInfo.TypeName = reader["type"];
			}
			dataNodeInfo.MimeType = reader["mimetype"];
			bool flag = false;
			dataNodeInfo.ReaderPosition = this.GetPosition(reader);
			while (!flag && reader.Read())
			{
				if (reader.NodeType == XmlNodeType.EndElement && (reader.LocalName.Equals("data") || reader.LocalName.Equals("metadata")))
				{
					flag = true;
				}
				else if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.Name.Equals("value"))
					{
						WhitespaceHandling whitespaceHandling = reader.WhitespaceHandling;
						try
						{
							reader.WhitespaceHandling = WhitespaceHandling.Significant;
							dataNodeInfo.ValueData = reader.ReadString();
							continue;
						}
						finally
						{
							reader.WhitespaceHandling = whitespaceHandling;
						}
					}
					if (reader.Name.Equals("comment"))
					{
						dataNodeInfo.Comment = reader.ReadString();
					}
				}
				else
				{
					dataNodeInfo.ValueData = reader.Value.Trim();
				}
			}
			if (dataNodeInfo.Name == null)
			{
				throw new ArgumentException(SR.GetString("InvalidResXResourceNoName", new object[]
				{
					dataNodeInfo.ValueData
				}));
			}
			ResXDataNode resXDataNode = new ResXDataNode(dataNodeInfo, this.BasePath);
			if (this.UseResXDataNodes)
			{
				this.resData[dataNodeInfo.Name] = resXDataNode;
				return;
			}
			IDictionary dictionary = isMetaData ? this.resMetadata : this.resData;
			if (this.assemblyNames == null)
			{
				dictionary[dataNodeInfo.Name] = resXDataNode.GetValue(this.typeResolver);
				return;
			}
			dictionary[dataNodeInfo.Name] = resXDataNode.GetValue(this.assemblyNames);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000B1E8 File Offset: 0x000093E8
		private string GetAliasFromTypeName(string typeName)
		{
			int num = typeName.IndexOf(",");
			return typeName.Substring(num + 2);
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000B20C File Offset: 0x0000940C
		private string GetTypeFromTypeName(string typeName)
		{
			int length = typeName.IndexOf(",");
			return typeName.Substring(0, length);
		}

		// Token: 0x040003C4 RID: 964
		private string fileName;

		// Token: 0x040003C5 RID: 965
		private TextReader reader;

		// Token: 0x040003C6 RID: 966
		private Stream stream;

		// Token: 0x040003C7 RID: 967
		private string fileContents;

		// Token: 0x040003C8 RID: 968
		private AssemblyName[] assemblyNames;

		// Token: 0x040003C9 RID: 969
		private string basePath;

		// Token: 0x040003CA RID: 970
		private bool isReaderDirty;

		// Token: 0x040003CB RID: 971
		private ITypeResolutionService typeResolver;

		// Token: 0x040003CC RID: 972
		private IAliasResolver aliasResolver;

		// Token: 0x040003CD RID: 973
		private ListDictionary resData;

		// Token: 0x040003CE RID: 974
		private ListDictionary resMetadata;

		// Token: 0x040003CF RID: 975
		private string resHeaderVersion;

		// Token: 0x040003D0 RID: 976
		private string resHeaderMimeType;

		// Token: 0x040003D1 RID: 977
		private string resHeaderReaderType;

		// Token: 0x040003D2 RID: 978
		private string resHeaderWriterType;

		// Token: 0x040003D3 RID: 979
		private bool useResXDataNodes;

		// Token: 0x0200053A RID: 1338
		private sealed class ReaderAliasResolver : IAliasResolver
		{
			// Token: 0x060054B6 RID: 21686 RVA: 0x00163CB4 File Offset: 0x00161EB4
			internal ReaderAliasResolver()
			{
				this.cachedAliases = new Hashtable();
			}

			// Token: 0x060054B7 RID: 21687 RVA: 0x00163CC8 File Offset: 0x00161EC8
			public AssemblyName ResolveAlias(string alias)
			{
				AssemblyName result = null;
				if (this.cachedAliases != null)
				{
					result = (AssemblyName)this.cachedAliases[alias];
				}
				return result;
			}

			// Token: 0x060054B8 RID: 21688 RVA: 0x00163CF2 File Offset: 0x00161EF2
			public void PushAlias(string alias, AssemblyName name)
			{
				if (this.cachedAliases != null && !string.IsNullOrEmpty(alias))
				{
					this.cachedAliases[alias] = name;
				}
			}

			// Token: 0x04003754 RID: 14164
			private Hashtable cachedAliases;
		}
	}
}
