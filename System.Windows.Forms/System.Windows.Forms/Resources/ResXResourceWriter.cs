using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace System.Resources
{
	/// <summary>Writes resources in an XML resource (.resx) file or an output stream.</summary>
	// Token: 0x020000F1 RID: 241
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class ResXResourceWriter : IResourceWriter, IDisposable
	{
		/// <summary>Gets or sets the base path for the relative file path specified in a <see cref="T:System.Resources.ResXFileRef" /> object.</summary>
		/// <returns>A path that, if prepended to the relative file path specified in a <see cref="T:System.Resources.ResXFileRef" /> object, yields an absolute path to an XML resource file.</returns>
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0000B28F File Offset: 0x0000948F
		// (set) Token: 0x06000395 RID: 917 RVA: 0x0000B297 File Offset: 0x00009497
		public string BasePath
		{
			get
			{
				return this.basePath;
			}
			set
			{
				this.basePath = value;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResXResourceWriter" /> class that writes the resources to the specified file.</summary>
		/// <param name="fileName">The output file name. </param>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The directory specified in <paramref name="filename" /> does not exist. </exception>
		// Token: 0x06000396 RID: 918 RVA: 0x0000B2A0 File Offset: 0x000094A0
		public ResXResourceWriter(string fileName)
		{
			this.fileName = fileName;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResXResourceWriter" /> class that writes the resources to a specified file and sets a delegate that enables resource assemblies to be written that target versions of the .NET Framework before the .NET Framework 4 by using qualified assembly names. </summary>
		/// <param name="fileName">The file to send output to. </param>
		/// <param name="typeNameConverter">The delegate that is used to target earlier versions of the .NET Framework.</param>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The directory specified in <paramref name="filename" /> does not exist. </exception>
		// Token: 0x06000397 RID: 919 RVA: 0x0000B2BA File Offset: 0x000094BA
		public ResXResourceWriter(string fileName, Func<Type, string> typeNameConverter)
		{
			this.fileName = fileName;
			this.typeNameConverter = typeNameConverter;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResXResourceWriter" /> class that writes the resources to the specified stream object.</summary>
		/// <param name="stream">The output stream. </param>
		// Token: 0x06000398 RID: 920 RVA: 0x0000B2DB File Offset: 0x000094DB
		public ResXResourceWriter(Stream stream)
		{
			this.stream = stream;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResXResourceWriter" /> class that writes the resources to a specified stream object and sets a converter delegate. This delegate enables resource assemblies to be written that target versions of the .NET Framework before the .NET Framework 4 by using qualified assembly names. </summary>
		/// <param name="stream">The stream to send the output to.</param>
		/// <param name="typeNameConverter">The delegate that is used to target earlier versions of the .NET Framework.</param>
		// Token: 0x06000399 RID: 921 RVA: 0x0000B2F5 File Offset: 0x000094F5
		public ResXResourceWriter(Stream stream, Func<Type, string> typeNameConverter)
		{
			this.stream = stream;
			this.typeNameConverter = typeNameConverter;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResXResourceWriter" /> class that writes to the specified <see cref="T:System.IO.TextWriter" /> object.</summary>
		/// <param name="textWriter">The <see cref="T:System.IO.TextWriter" /> object to send the output to. </param>
		// Token: 0x0600039A RID: 922 RVA: 0x0000B316 File Offset: 0x00009516
		public ResXResourceWriter(TextWriter textWriter)
		{
			this.textWriter = textWriter;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResXResourceWriter" /> class that writes the resources to a specified <see cref="T:System.IO.TextWriter" /> object and sets a delegate that enables resource assemblies to be written that target versions of the .NET Framework before the .NET Framework 4 by using qualified assembly names. </summary>
		/// <param name="textWriter">The object to send output to.</param>
		/// <param name="typeNameConverter">The delegate that is used to target earlier versions of the .NET Framework.</param>
		// Token: 0x0600039B RID: 923 RVA: 0x0000B330 File Offset: 0x00009530
		public ResXResourceWriter(TextWriter textWriter, Func<Type, string> typeNameConverter)
		{
			this.textWriter = textWriter;
			this.typeNameConverter = typeNameConverter;
		}

		/// <summary>This member overrides the <see cref="M:System.Object.Finalize" /> method. </summary>
		// Token: 0x0600039C RID: 924 RVA: 0x0000B354 File Offset: 0x00009554
		~ResXResourceWriter()
		{
			this.Dispose(false);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000B384 File Offset: 0x00009584
		private void InitializeWriter()
		{
			if (this.xmlTextWriter == null)
			{
				bool flag = false;
				if (this.textWriter != null)
				{
					this.textWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					flag = true;
					this.xmlTextWriter = new XmlTextWriter(this.textWriter);
				}
				else if (this.stream != null)
				{
					this.xmlTextWriter = new XmlTextWriter(this.stream, Encoding.UTF8);
				}
				else
				{
					this.xmlTextWriter = new XmlTextWriter(this.fileName, Encoding.UTF8);
				}
				this.xmlTextWriter.Formatting = Formatting.Indented;
				this.xmlTextWriter.Indentation = 2;
				if (!flag)
				{
					this.xmlTextWriter.WriteStartDocument();
				}
			}
			else
			{
				this.xmlTextWriter.WriteStartDocument();
			}
			this.xmlTextWriter.WriteStartElement("root");
			XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(ResXResourceWriter.ResourceSchema));
			xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
			this.xmlTextWriter.WriteNode(xmlTextReader, true);
			this.xmlTextWriter.WriteStartElement("resheader");
			this.xmlTextWriter.WriteAttributeString("name", "resmimetype");
			this.xmlTextWriter.WriteStartElement("value");
			this.xmlTextWriter.WriteString(ResXResourceWriter.ResMimeType);
			this.xmlTextWriter.WriteEndElement();
			this.xmlTextWriter.WriteEndElement();
			this.xmlTextWriter.WriteStartElement("resheader");
			this.xmlTextWriter.WriteAttributeString("name", "version");
			this.xmlTextWriter.WriteStartElement("value");
			this.xmlTextWriter.WriteString(ResXResourceWriter.Version);
			this.xmlTextWriter.WriteEndElement();
			this.xmlTextWriter.WriteEndElement();
			this.xmlTextWriter.WriteStartElement("resheader");
			this.xmlTextWriter.WriteAttributeString("name", "reader");
			this.xmlTextWriter.WriteStartElement("value");
			this.xmlTextWriter.WriteString(MultitargetUtil.GetAssemblyQualifiedName(typeof(ResXResourceReader), this.typeNameConverter));
			this.xmlTextWriter.WriteEndElement();
			this.xmlTextWriter.WriteEndElement();
			this.xmlTextWriter.WriteStartElement("resheader");
			this.xmlTextWriter.WriteAttributeString("name", "writer");
			this.xmlTextWriter.WriteStartElement("value");
			this.xmlTextWriter.WriteString(MultitargetUtil.GetAssemblyQualifiedName(typeof(ResXResourceWriter), this.typeNameConverter));
			this.xmlTextWriter.WriteEndElement();
			this.xmlTextWriter.WriteEndElement();
			this.initialized = true;
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600039E RID: 926 RVA: 0x0000B5FB File Offset: 0x000097FB
		private XmlWriter Writer
		{
			get
			{
				if (!this.initialized)
				{
					this.InitializeWriter();
				}
				return this.xmlTextWriter;
			}
		}

		/// <summary>Adds the specified alias to a list of aliases. </summary>
		/// <param name="aliasName">The name of the alias.</param>
		/// <param name="assemblyName">The name of the assembly represented by <paramref name="aliasName" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="assemblyName" /> is <see langword="null" />.</exception>
		// Token: 0x0600039F RID: 927 RVA: 0x0000B611 File Offset: 0x00009811
		public virtual void AddAlias(string aliasName, AssemblyName assemblyName)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (this.cachedAliases == null)
			{
				this.cachedAliases = new Hashtable();
			}
			this.cachedAliases[assemblyName.FullName] = aliasName;
		}

		/// <summary>Adds a design-time property whose value is specifed as a byte array to the list of resources to write.</summary>
		/// <param name="name">The name of a property.</param>
		/// <param name="value">A byte array containing the value of the property to add.</param>
		/// <exception cref="T:System.InvalidOperationException">The resource specified by the <paramref name="name" /> parameter has already been added.</exception>
		// Token: 0x060003A0 RID: 928 RVA: 0x0000B646 File Offset: 0x00009846
		public void AddMetadata(string name, byte[] value)
		{
			this.AddDataRow("metadata", name, value);
		}

		/// <summary>Adds a design-time property whose value is specified as a string to the list of resources to write.</summary>
		/// <param name="name">The name of a property.</param>
		/// <param name="value">A string that is the value of the property to add.</param>
		/// <exception cref="T:System.InvalidOperationException">The resource specified by the <paramref name="name" /> property has already been added.</exception>
		// Token: 0x060003A1 RID: 929 RVA: 0x0000B655 File Offset: 0x00009855
		public void AddMetadata(string name, string value)
		{
			this.AddDataRow("metadata", name, value);
		}

		/// <summary>Adds a design-time property whose value is specified as an object to the list of resources to write.</summary>
		/// <param name="name">The name of a property.</param>
		/// <param name="value">An object that is the value of the property to add.</param>
		/// <exception cref="T:System.InvalidOperationException">The resource specified by the <paramref name="name" /> parameter has already been added.</exception>
		// Token: 0x060003A2 RID: 930 RVA: 0x0000B664 File Offset: 0x00009864
		public void AddMetadata(string name, object value)
		{
			this.AddDataRow("metadata", name, value);
		}

		/// <summary>Adds a named resource specified as a byte array to the list of resources to write.</summary>
		/// <param name="name">The name of the resource. </param>
		/// <param name="value">The value of the resource to add as an 8-bit unsigned integer array. </param>
		// Token: 0x060003A3 RID: 931 RVA: 0x0000B673 File Offset: 0x00009873
		public void AddResource(string name, byte[] value)
		{
			this.AddDataRow("data", name, value);
		}

		/// <summary>Adds a named resource specified as an object to the list of resources to write.</summary>
		/// <param name="name">The name of the resource. </param>
		/// <param name="value">The value of the resource. </param>
		// Token: 0x060003A4 RID: 932 RVA: 0x0000B682 File Offset: 0x00009882
		public void AddResource(string name, object value)
		{
			if (value is ResXDataNode)
			{
				this.AddResource((ResXDataNode)value);
				return;
			}
			this.AddDataRow("data", name, value);
		}

		/// <summary>Adds a string resource to the resources.</summary>
		/// <param name="name">The name of the resource. </param>
		/// <param name="value">The value of the resource. </param>
		// Token: 0x060003A5 RID: 933 RVA: 0x0000B6A6 File Offset: 0x000098A6
		public void AddResource(string name, string value)
		{
			this.AddDataRow("data", name, value);
		}

		/// <summary>Adds a named resource specified in a <see cref="T:System.Resources.ResXDataNode" /> object to the list of resources to write.</summary>
		/// <param name="node">A <see cref="T:System.Resources.ResXDataNode" /> object that contains a resource name/value pair.</param>
		// Token: 0x060003A6 RID: 934 RVA: 0x0000B6B8 File Offset: 0x000098B8
		public void AddResource(ResXDataNode node)
		{
			ResXDataNode resXDataNode = node.DeepClone();
			ResXFileRef fileRef = resXDataNode.FileRef;
			string text = this.BasePath;
			if (!string.IsNullOrEmpty(text))
			{
				if (!text.EndsWith("\\"))
				{
					text += "\\";
				}
				if (fileRef != null)
				{
					fileRef.MakeFilePathRelative(text);
				}
			}
			DataNodeInfo dataNodeInfo = resXDataNode.GetDataNodeInfo();
			this.AddDataRow("data", dataNodeInfo.Name, dataNodeInfo.ValueData, dataNodeInfo.TypeName, dataNodeInfo.MimeType, dataNodeInfo.Comment);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000B735 File Offset: 0x00009935
		private void AddDataRow(string elementName, string name, byte[] value)
		{
			this.AddDataRow(elementName, name, ResXResourceWriter.ToBase64WrappedString(value), this.TypeNameWithAssembly(typeof(byte[])), null, null);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000B758 File Offset: 0x00009958
		private void AddDataRow(string elementName, string name, object value)
		{
			if (value is string)
			{
				this.AddDataRow(elementName, name, (string)value);
				return;
			}
			if (value is byte[])
			{
				this.AddDataRow(elementName, name, (byte[])value);
				return;
			}
			if (value is ResXFileRef)
			{
				ResXFileRef resXFileRef = (ResXFileRef)value;
				ResXDataNode resXDataNode = new ResXDataNode(name, resXFileRef, this.typeNameConverter);
				if (resXFileRef != null)
				{
					resXFileRef.MakeFilePathRelative(this.BasePath);
				}
				DataNodeInfo dataNodeInfo = resXDataNode.GetDataNodeInfo();
				this.AddDataRow(elementName, dataNodeInfo.Name, dataNodeInfo.ValueData, dataNodeInfo.TypeName, dataNodeInfo.MimeType, dataNodeInfo.Comment);
				return;
			}
			ResXDataNode resXDataNode2 = new ResXDataNode(name, value, this.typeNameConverter);
			DataNodeInfo dataNodeInfo2 = resXDataNode2.GetDataNodeInfo();
			this.AddDataRow(elementName, dataNodeInfo2.Name, dataNodeInfo2.ValueData, dataNodeInfo2.TypeName, dataNodeInfo2.MimeType, dataNodeInfo2.Comment);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000B82C File Offset: 0x00009A2C
		private void AddDataRow(string elementName, string name, string value)
		{
			if (value == null)
			{
				this.AddDataRow(elementName, name, value, MultitargetUtil.GetAssemblyQualifiedName(typeof(ResXNullRef), this.typeNameConverter), null, null);
				return;
			}
			this.AddDataRow(elementName, name, value, null, null, null);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000B860 File Offset: 0x00009A60
		private void AddDataRow(string elementName, string name, string value, string type, string mimeType, string comment)
		{
			if (this.hasBeenSaved)
			{
				throw new InvalidOperationException(SR.GetString("ResXResourceWriterSaved"));
			}
			string text = null;
			if (!string.IsNullOrEmpty(type) && elementName == "data")
			{
				string fullName = this.GetFullName(type);
				if (string.IsNullOrEmpty(fullName))
				{
					try
					{
						Type type2 = Type.GetType(type);
						if (type2 == typeof(string))
						{
							type = null;
						}
						else if (type2 != null)
						{
							fullName = this.GetFullName(MultitargetUtil.GetAssemblyQualifiedName(type2, this.typeNameConverter));
							text = this.GetAliasFromName(new AssemblyName(fullName));
						}
						goto IL_A2;
					}
					catch
					{
						goto IL_A2;
					}
				}
				text = this.GetAliasFromName(new AssemblyName(this.GetFullName(type)));
			}
			IL_A2:
			this.Writer.WriteStartElement(elementName);
			this.Writer.WriteAttributeString("name", name);
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(type) && elementName == "data")
			{
				string typeName = this.GetTypeName(type);
				string value2 = typeName + ", " + text;
				this.Writer.WriteAttributeString("type", value2);
			}
			else if (type != null)
			{
				this.Writer.WriteAttributeString("type", type);
			}
			if (mimeType != null)
			{
				this.Writer.WriteAttributeString("mimetype", mimeType);
			}
			if ((type == null && mimeType == null) || (type != null && type.StartsWith("System.Char", StringComparison.Ordinal)))
			{
				this.Writer.WriteAttributeString("xml", "space", null, "preserve");
			}
			this.Writer.WriteStartElement("value");
			if (!string.IsNullOrEmpty(value))
			{
				this.Writer.WriteString(value);
			}
			this.Writer.WriteEndElement();
			if (!string.IsNullOrEmpty(comment))
			{
				this.Writer.WriteStartElement("comment");
				this.Writer.WriteString(comment);
				this.Writer.WriteEndElement();
			}
			this.Writer.WriteEndElement();
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000BA54 File Offset: 0x00009C54
		private void AddAssemblyRow(string elementName, string alias, string name)
		{
			this.Writer.WriteStartElement(elementName);
			if (!string.IsNullOrEmpty(alias))
			{
				this.Writer.WriteAttributeString("alias", alias);
			}
			if (!string.IsNullOrEmpty(name))
			{
				this.Writer.WriteAttributeString("name", name);
			}
			this.Writer.WriteEndElement();
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000BAAC File Offset: 0x00009CAC
		private string GetAliasFromName(AssemblyName assemblyName)
		{
			if (this.cachedAliases == null)
			{
				this.cachedAliases = new Hashtable();
			}
			string text = (string)this.cachedAliases[assemblyName.FullName];
			if (string.IsNullOrEmpty(text))
			{
				text = assemblyName.Name;
				this.AddAlias(text, assemblyName);
				this.AddAssemblyRow("assembly", text, assemblyName.FullName);
			}
			return text;
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Resources.ResXResourceWriter" />.</summary>
		// Token: 0x060003AD RID: 941 RVA: 0x0000BB0D File Offset: 0x00009D0D
		public void Close()
		{
			this.Dispose();
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Resources.ResXResourceWriter" />.</summary>
		// Token: 0x060003AE RID: 942 RVA: 0x0000BB15 File Offset: 0x00009D15
		public virtual void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Resources.ResXResourceWriter" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x060003AF RID: 943 RVA: 0x0000BB24 File Offset: 0x00009D24
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!this.hasBeenSaved)
				{
					this.Generate();
				}
				if (this.xmlTextWriter != null)
				{
					this.xmlTextWriter.Close();
					this.xmlTextWriter = null;
				}
				if (this.stream != null)
				{
					this.stream.Close();
					this.stream = null;
				}
				if (this.textWriter != null)
				{
					this.textWriter.Close();
					this.textWriter = null;
				}
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000BB90 File Offset: 0x00009D90
		private string GetTypeName(string typeName)
		{
			int num = typeName.IndexOf(",");
			if (num != -1)
			{
				return typeName.Substring(0, num);
			}
			return typeName;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000BBB8 File Offset: 0x00009DB8
		private string GetFullName(string typeName)
		{
			int num = typeName.IndexOf(",");
			if (num == -1)
			{
				return null;
			}
			return typeName.Substring(num + 2);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000BBE0 File Offset: 0x00009DE0
		private static string ToBase64WrappedString(byte[] data)
		{
			string text = Convert.ToBase64String(data);
			if (text.Length > 80)
			{
				StringBuilder stringBuilder = new StringBuilder(text.Length + text.Length / 80 * 3);
				int i;
				for (i = 0; i < text.Length - 80; i += 80)
				{
					stringBuilder.Append("\r\n");
					stringBuilder.Append("        ");
					stringBuilder.Append(text, i, 80);
				}
				stringBuilder.Append("\r\n");
				stringBuilder.Append("        ");
				stringBuilder.Append(text, i, text.Length - i);
				stringBuilder.Append("\r\n");
				return stringBuilder.ToString();
			}
			return text;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000BC90 File Offset: 0x00009E90
		private string TypeNameWithAssembly(Type type)
		{
			return MultitargetUtil.GetAssemblyQualifiedName(type, this.typeNameConverter);
		}

		/// <summary>Writes all resources added by the <see cref="M:System.Resources.ResXResourceWriter.AddResource(System.String,System.Byte[])" /> method to the output file or stream.</summary>
		/// <exception cref="T:System.InvalidOperationException">The resource has already been saved. </exception>
		// Token: 0x060003B4 RID: 948 RVA: 0x0000BCAB File Offset: 0x00009EAB
		public void Generate()
		{
			if (this.hasBeenSaved)
			{
				throw new InvalidOperationException(SR.GetString("ResXResourceWriterSaved"));
			}
			this.hasBeenSaved = true;
			this.Writer.WriteEndElement();
			this.Writer.Flush();
		}

		// Token: 0x040003D4 RID: 980
		internal const string TypeStr = "type";

		// Token: 0x040003D5 RID: 981
		internal const string NameStr = "name";

		// Token: 0x040003D6 RID: 982
		internal const string DataStr = "data";

		// Token: 0x040003D7 RID: 983
		internal const string MetadataStr = "metadata";

		// Token: 0x040003D8 RID: 984
		internal const string MimeTypeStr = "mimetype";

		// Token: 0x040003D9 RID: 985
		internal const string ValueStr = "value";

		// Token: 0x040003DA RID: 986
		internal const string ResHeaderStr = "resheader";

		// Token: 0x040003DB RID: 987
		internal const string VersionStr = "version";

		// Token: 0x040003DC RID: 988
		internal const string ResMimeTypeStr = "resmimetype";

		// Token: 0x040003DD RID: 989
		internal const string ReaderStr = "reader";

		// Token: 0x040003DE RID: 990
		internal const string WriterStr = "writer";

		// Token: 0x040003DF RID: 991
		internal const string CommentStr = "comment";

		// Token: 0x040003E0 RID: 992
		internal const string AssemblyStr = "assembly";

		// Token: 0x040003E1 RID: 993
		internal const string AliasStr = "alias";

		// Token: 0x040003E2 RID: 994
		private Hashtable cachedAliases;

		// Token: 0x040003E3 RID: 995
		private static TraceSwitch ResValueProviderSwitch = new TraceSwitch("ResX", "Debug the resource value provider");

		// Token: 0x040003E4 RID: 996
		internal static readonly string Beta2CompatSerializedObjectMimeType = "text/microsoft-urt/psuedoml-serialized/base64";

		// Token: 0x040003E5 RID: 997
		internal static readonly string CompatBinSerializedObjectMimeType = "text/microsoft-urt/binary-serialized/base64";

		// Token: 0x040003E6 RID: 998
		internal static readonly string CompatSoapSerializedObjectMimeType = "text/microsoft-urt/soap-serialized/base64";

		/// <summary>Specifies the default content type for a binary object. This field is read-only.</summary>
		// Token: 0x040003E7 RID: 999
		public static readonly string BinSerializedObjectMimeType = "application/x-microsoft.net.object.binary.base64";

		/// <summary>Specifies the content type for a SOAP object. This field is read-only.</summary>
		// Token: 0x040003E8 RID: 1000
		public static readonly string SoapSerializedObjectMimeType = "application/x-microsoft.net.object.soap.base64";

		/// <summary>Specifies the default content type for an object. This field is read-only.</summary>
		// Token: 0x040003E9 RID: 1001
		public static readonly string DefaultSerializedObjectMimeType = ResXResourceWriter.BinSerializedObjectMimeType;

		/// <summary>Specifies the default content type for a byte array object. This field is read-only.</summary>
		// Token: 0x040003EA RID: 1002
		public static readonly string ByteArraySerializedObjectMimeType = "application/x-microsoft.net.object.bytearray.base64";

		/// <summary>Specifies the content type of an XML resource. This field is read-only.</summary>
		// Token: 0x040003EB RID: 1003
		public static readonly string ResMimeType = "text/microsoft-resx";

		/// <summary>Specifies the version of the schema that the XML output conforms to. This field is read-only.</summary>
		// Token: 0x040003EC RID: 1004
		public static readonly string Version = "2.0";

		/// <summary>Specifies the schema to use in writing the XML file. This field is read-only.</summary>
		// Token: 0x040003ED RID: 1005
		public static readonly string ResourceSchema = string.Concat(new string[]
		{
			"\r\n    <!-- \r\n    Microsoft ResX Schema \r\n    \r\n    Version ",
			ResXResourceWriter.Version,
			"\r\n    \r\n    The primary goals of this format is to allow a simple XML format \r\n    that is mostly human readable. The generation and parsing of the \r\n    various data types are done through the TypeConverter classes \r\n    associated with the data types.\r\n    \r\n    Example:\r\n    \r\n    ... ado.net/XML headers & schema ...\r\n    <resheader name=\"resmimetype\">text/microsoft-resx</resheader>\r\n    <resheader name=\"version\">",
			ResXResourceWriter.Version,
			"</resheader>\r\n    <resheader name=\"reader\">System.Resources.ResXResourceReader, System.Windows.Forms, ...</resheader>\r\n    <resheader name=\"writer\">System.Resources.ResXResourceWriter, System.Windows.Forms, ...</resheader>\r\n    <data name=\"Name1\"><value>this is my long string</value><comment>this is a comment</comment></data>\r\n    <data name=\"Color1\" type=\"System.Drawing.Color, System.Drawing\">Blue</data>\r\n    <data name=\"Bitmap1\" mimetype=\"",
			ResXResourceWriter.BinSerializedObjectMimeType,
			"\">\r\n        <value>[base64 mime encoded serialized .NET Framework object]</value>\r\n    </data>\r\n    <data name=\"Icon1\" type=\"System.Drawing.Icon, System.Drawing\" mimetype=\"",
			ResXResourceWriter.ByteArraySerializedObjectMimeType,
			"\">\r\n        <value>[base64 mime encoded string representing a byte array form of the .NET Framework object]</value>\r\n        <comment>This is a comment</comment>\r\n    </data>\r\n                \r\n    There are any number of \"resheader\" rows that contain simple \r\n    name/value pairs.\r\n    \r\n    Each data row contains a name, and value. The row also contains a \r\n    type or mimetype. Type corresponds to a .NET class that support \r\n    text/value conversion through the TypeConverter architecture. \r\n    Classes that don't support this are serialized and stored with the \r\n    mimetype set.\r\n    \r\n    The mimetype is used for serialized objects, and tells the \r\n    ResXResourceReader how to depersist the object. This is currently not \r\n    extensible. For a given mimetype the value must be set accordingly:\r\n    \r\n    Note - ",
			ResXResourceWriter.BinSerializedObjectMimeType,
			" is the format \r\n    that the ResXResourceWriter will generate, however the reader can \r\n    read any of the formats listed below.\r\n    \r\n    mimetype: ",
			ResXResourceWriter.BinSerializedObjectMimeType,
			"\r\n    value   : The object must be serialized with \r\n            : System.Runtime.Serialization.Formatters.Binary.BinaryFormatter\r\n            : and then encoded with base64 encoding.\r\n    \r\n    mimetype: ",
			ResXResourceWriter.SoapSerializedObjectMimeType,
			"\r\n    value   : The object must be serialized with \r\n            : System.Runtime.Serialization.Formatters.Soap.SoapFormatter\r\n            : and then encoded with base64 encoding.\r\n\r\n    mimetype: ",
			ResXResourceWriter.ByteArraySerializedObjectMimeType,
			"\r\n    value   : The object must be serialized into a byte array \r\n            : using a System.ComponentModel.TypeConverter\r\n            : and then encoded with base64 encoding.\r\n    -->\r\n    <xsd:schema id=\"root\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n        <xsd:import namespace=\"http://www.w3.org/XML/1998/namespace\"/>\r\n        <xsd:element name=\"root\" msdata:IsDataSet=\"true\">\r\n            <xsd:complexType>\r\n                <xsd:choice maxOccurs=\"unbounded\">\r\n                    <xsd:element name=\"metadata\">\r\n                        <xsd:complexType>\r\n                            <xsd:sequence>\r\n                            <xsd:element name=\"value\" type=\"xsd:string\" minOccurs=\"0\"/>\r\n                            </xsd:sequence>\r\n                            <xsd:attribute name=\"name\" use=\"required\" type=\"xsd:string\"/>\r\n                            <xsd:attribute name=\"type\" type=\"xsd:string\"/>\r\n                            <xsd:attribute name=\"mimetype\" type=\"xsd:string\"/>\r\n                            <xsd:attribute ref=\"xml:space\"/>                            \r\n                        </xsd:complexType>\r\n                    </xsd:element>\r\n                    <xsd:element name=\"assembly\">\r\n                      <xsd:complexType>\r\n                        <xsd:attribute name=\"alias\" type=\"xsd:string\"/>\r\n                        <xsd:attribute name=\"name\" type=\"xsd:string\"/>\r\n                      </xsd:complexType>\r\n                    </xsd:element>\r\n                    <xsd:element name=\"data\">\r\n                        <xsd:complexType>\r\n                            <xsd:sequence>\r\n                                <xsd:element name=\"value\" type=\"xsd:string\" minOccurs=\"0\" msdata:Ordinal=\"1\" />\r\n                                <xsd:element name=\"comment\" type=\"xsd:string\" minOccurs=\"0\" msdata:Ordinal=\"2\" />\r\n                            </xsd:sequence>\r\n                            <xsd:attribute name=\"name\" type=\"xsd:string\" use=\"required\" msdata:Ordinal=\"1\" />\r\n                            <xsd:attribute name=\"type\" type=\"xsd:string\" msdata:Ordinal=\"3\" />\r\n                            <xsd:attribute name=\"mimetype\" type=\"xsd:string\" msdata:Ordinal=\"4\" />\r\n                            <xsd:attribute ref=\"xml:space\"/>\r\n                        </xsd:complexType>\r\n                    </xsd:element>\r\n                    <xsd:element name=\"resheader\">\r\n                        <xsd:complexType>\r\n                            <xsd:sequence>\r\n                                <xsd:element name=\"value\" type=\"xsd:string\" minOccurs=\"0\" msdata:Ordinal=\"1\" />\r\n                            </xsd:sequence>\r\n                            <xsd:attribute name=\"name\" type=\"xsd:string\" use=\"required\" />\r\n                        </xsd:complexType>\r\n                    </xsd:element>\r\n                </xsd:choice>\r\n            </xsd:complexType>\r\n        </xsd:element>\r\n        </xsd:schema>\r\n        "
		});

		// Token: 0x040003EE RID: 1006
		private IFormatter binaryFormatter = new BinaryFormatter();

		// Token: 0x040003EF RID: 1007
		private string fileName;

		// Token: 0x040003F0 RID: 1008
		private Stream stream;

		// Token: 0x040003F1 RID: 1009
		private TextWriter textWriter;

		// Token: 0x040003F2 RID: 1010
		private XmlTextWriter xmlTextWriter;

		// Token: 0x040003F3 RID: 1011
		private string basePath;

		// Token: 0x040003F4 RID: 1012
		private bool hasBeenSaved;

		// Token: 0x040003F5 RID: 1013
		private bool initialized;

		// Token: 0x040003F6 RID: 1014
		private Func<Type, string> typeNameConverter;
	}
}
