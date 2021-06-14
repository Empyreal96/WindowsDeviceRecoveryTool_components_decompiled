using System;
using System.IO;
using System.IO.Packaging;
using System.Text;
using System.Windows.Markup;
using System.Xml;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000345 RID: 837
	internal class XpsSchemaValidator
	{
		// Token: 0x06002CEE RID: 11502 RVA: 0x000CAC20 File Offset: 0x000C8E20
		public XpsSchemaValidator(XpsValidatingLoader loader, XpsSchema schema, ContentType mimeType, Stream objectStream, Uri packageUri, Uri baseUri)
		{
			XmlReader xmlReader = new XpsSchemaValidator.XmlEncodingEnforcingTextReader(objectStream)
			{
				ProhibitDtd = true,
				Normalization = true
			};
			string[] array = XpsSchemaValidator._predefinedNamespaces;
			if (!string.IsNullOrEmpty(schema.RootNamespaceUri))
			{
				array = new string[XpsSchemaValidator._predefinedNamespaces.Length + 1];
				array[0] = schema.RootNamespaceUri;
				XpsSchemaValidator._predefinedNamespaces.CopyTo(array, 1);
			}
			xmlReader = new XmlCompatibilityReader(xmlReader, array);
			xmlReader = XmlReader.Create(xmlReader, schema.GetXmlReaderSettings());
			if (schema.HasUriAttributes(mimeType) && packageUri != null && baseUri != null)
			{
				xmlReader = new XpsSchemaValidator.RootXMLNSAndUriValidatingXmlReader(loader, schema, xmlReader, packageUri, baseUri);
			}
			else
			{
				xmlReader = new XpsSchemaValidator.RootXMLNSAndUriValidatingXmlReader(loader, schema, xmlReader);
			}
			this._compatReader = xmlReader;
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x06002CEF RID: 11503 RVA: 0x000CACD3 File Offset: 0x000C8ED3
		public XmlReader XmlReader
		{
			get
			{
				return this._compatReader;
			}
		}

		// Token: 0x04001D4B RID: 7499
		private XmlReader _compatReader;

		// Token: 0x04001D4C RID: 7500
		private static string[] _predefinedNamespaces = new string[]
		{
			"http://schemas.microsoft.com/xps/2005/06/resourcedictionary-key"
		};

		// Token: 0x020008CE RID: 2254
		private class XmlEncodingEnforcingTextReader : XmlTextReader
		{
			// Token: 0x06008483 RID: 33923 RVA: 0x002486B2 File Offset: 0x002468B2
			public XmlEncodingEnforcingTextReader(Stream objectStream) : base(objectStream)
			{
			}

			// Token: 0x06008484 RID: 33924 RVA: 0x002486BC File Offset: 0x002468BC
			public override bool Read()
			{
				bool flag = base.Read();
				if (flag && !this._encodingChecked)
				{
					if (base.NodeType == XmlNodeType.XmlDeclaration)
					{
						string text = base["encoding"];
						if (text != null && !text.Equals(Encoding.Unicode.WebName, StringComparison.OrdinalIgnoreCase) && !text.Equals(Encoding.UTF8.WebName, StringComparison.OrdinalIgnoreCase))
						{
							throw new FileFormatException(SR.Get("XpsValidatingLoaderUnsupportedEncoding"));
						}
					}
					if (!(base.Encoding is UTF8Encoding) && !(base.Encoding is UnicodeEncoding))
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderUnsupportedEncoding"));
					}
					this._encodingChecked = true;
				}
				return flag;
			}

			// Token: 0x04004238 RID: 16952
			private bool _encodingChecked;
		}

		// Token: 0x020008CF RID: 2255
		private class RootXMLNSAndUriValidatingXmlReader : XmlWrappingReader
		{
			// Token: 0x06008485 RID: 33925 RVA: 0x00248762 File Offset: 0x00246962
			public RootXMLNSAndUriValidatingXmlReader(XpsValidatingLoader loader, XpsSchema schema, XmlReader xmlReader, Uri packageUri, Uri baseUri) : base(xmlReader)
			{
				this._loader = loader;
				this._schema = schema;
				this._packageUri = packageUri;
				this._baseUri = baseUri;
			}

			// Token: 0x06008486 RID: 33926 RVA: 0x00248789 File Offset: 0x00246989
			public RootXMLNSAndUriValidatingXmlReader(XpsValidatingLoader loader, XpsSchema schema, XmlReader xmlReader) : base(xmlReader)
			{
				this._loader = loader;
				this._schema = schema;
			}

			// Token: 0x06008487 RID: 33927 RVA: 0x002487A0 File Offset: 0x002469A0
			private void CheckUri(string attr)
			{
				this.CheckUri(base.Reader.LocalName, attr);
			}

			// Token: 0x06008488 RID: 33928 RVA: 0x002487B4 File Offset: 0x002469B4
			private void CheckUri(string localName, string attr)
			{
				if (attr != this._lastAttr)
				{
					this._lastAttr = attr;
					string[] array = this._schema.ExtractUriFromAttr(localName, attr);
					if (array != null)
					{
						foreach (string text in array)
						{
							if (text.Length > 0)
							{
								Uri partUri = PackUriHelper.ResolvePartUri(this._baseUri, new Uri(text, UriKind.Relative));
								Uri uri = PackUriHelper.Create(this._packageUri, partUri);
								this._loader.UriHitHandler(this._node, uri);
							}
						}
					}
				}
			}

			// Token: 0x17001E03 RID: 7683
			// (get) Token: 0x06008489 RID: 33929 RVA: 0x00248835 File Offset: 0x00246A35
			public override string Value
			{
				get
				{
					this.CheckUri(base.Reader.Value);
					return base.Reader.Value;
				}
			}

			// Token: 0x0600848A RID: 33930 RVA: 0x00248854 File Offset: 0x00246A54
			public override string GetAttribute(string name)
			{
				string attribute = base.Reader.GetAttribute(name);
				this.CheckUri(name, attribute);
				return attribute;
			}

			// Token: 0x0600848B RID: 33931 RVA: 0x00248878 File Offset: 0x00246A78
			public override string GetAttribute(string name, string namespaceURI)
			{
				string attribute = base.Reader.GetAttribute(name, namespaceURI);
				this.CheckUri(attribute);
				return attribute;
			}

			// Token: 0x0600848C RID: 33932 RVA: 0x0024889C File Offset: 0x00246A9C
			public override string GetAttribute(int i)
			{
				string attribute = base.Reader.GetAttribute(i);
				this.CheckUri(attribute);
				return attribute;
			}

			// Token: 0x0600848D RID: 33933 RVA: 0x002488C0 File Offset: 0x00246AC0
			public override bool Read()
			{
				this._node++;
				bool result = base.Reader.Read();
				if (base.Reader.NodeType == XmlNodeType.Element && !this._rootXMLNSChecked)
				{
					if (!this._schema.IsValidRootNamespaceUri(base.Reader.NamespaceURI))
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderUnsupportedRootNamespaceUri"));
					}
					this._rootXMLNSChecked = true;
				}
				return result;
			}

			// Token: 0x04004239 RID: 16953
			private XpsValidatingLoader _loader;

			// Token: 0x0400423A RID: 16954
			private XpsSchema _schema;

			// Token: 0x0400423B RID: 16955
			private Uri _packageUri;

			// Token: 0x0400423C RID: 16956
			private Uri _baseUri;

			// Token: 0x0400423D RID: 16957
			private string _lastAttr;

			// Token: 0x0400423E RID: 16958
			private int _node;

			// Token: 0x0400423F RID: 16959
			private bool _rootXMLNSChecked;
		}
	}
}
