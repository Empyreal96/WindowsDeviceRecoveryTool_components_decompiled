using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Common
{
	// Token: 0x0200014F RID: 335
	internal abstract class XmlDocumentParser
	{
		// Token: 0x0600063E RID: 1598 RVA: 0x0000F9AF File Offset: 0x0000DBAF
		protected XmlDocumentParser(XmlReader underlyingReader, string documentPath)
		{
			this.reader = underlyingReader;
			this.docPath = documentPath;
			this.errors = new List<EdmError>();
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x0000F9DB File Offset: 0x0000DBDB
		internal string DocumentPath
		{
			get
			{
				return this.docPath;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x0000F9E3 File Offset: 0x0000DBE3
		// (set) Token: 0x06000641 RID: 1601 RVA: 0x0000F9EB File Offset: 0x0000DBEB
		internal string DocumentNamespace { get; private set; }

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0000F9F4 File Offset: 0x0000DBF4
		// (set) Token: 0x06000643 RID: 1603 RVA: 0x0000F9FC File Offset: 0x0000DBFC
		internal Version DocumentVersion { get; private set; }

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0000FA05 File Offset: 0x0000DC05
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x0000FA0D File Offset: 0x0000DC0D
		internal CsdlLocation DocumentElementLocation { get; private set; }

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x0000FA16 File Offset: 0x0000DC16
		// (set) Token: 0x06000647 RID: 1607 RVA: 0x0000FA1E File Offset: 0x0000DC1E
		internal bool HasErrors { get; private set; }

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x0000FA27 File Offset: 0x0000DC27
		// (set) Token: 0x06000649 RID: 1609 RVA: 0x0000FA2F File Offset: 0x0000DC2F
		internal XmlElementValue Result { get; private set; }

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x0000FA38 File Offset: 0x0000DC38
		internal CsdlLocation Location
		{
			get
			{
				if (this.xmlLineInfo != null && this.xmlLineInfo.HasLineInfo())
				{
					return new CsdlLocation(this.xmlLineInfo.LineNumber, this.xmlLineInfo.LinePosition);
				}
				return new CsdlLocation(0, 0);
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x0000FA72 File Offset: 0x0000DC72
		internal IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x0000FA7C File Offset: 0x0000DC7C
		private bool IsTextNode
		{
			get
			{
				XmlNodeType nodeType = this.reader.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					break;
				default:
					if (nodeType != XmlNodeType.SignificantWhitespace)
					{
						return false;
					}
					break;
				}
				return true;
			}
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0000FAB0 File Offset: 0x0000DCB0
		internal void ParseDocumentElement()
		{
			this.reader = this.InitializeReader(this.reader);
			this.xmlLineInfo = (this.reader as IXmlLineInfo);
			if (this.reader.NodeType != XmlNodeType.Element)
			{
				while (this.reader.Read() && this.reader.NodeType != XmlNodeType.Element)
				{
				}
			}
			if (this.reader.EOF)
			{
				this.ReportEmptyFile();
				return;
			}
			this.DocumentNamespace = this.reader.NamespaceURI;
			Version documentVersion;
			string[] expectedNamespaces;
			if (!this.TryGetDocumentVersion(this.DocumentNamespace, out documentVersion, out expectedNamespaces))
			{
				this.ReportUnexpectedRootNamespace(this.reader.LocalName, this.DocumentNamespace, expectedNamespaces);
				return;
			}
			this.DocumentVersion = documentVersion;
			this.DocumentElementLocation = this.Location;
			bool isEmptyElement = this.reader.IsEmptyElement;
			XmlElementInfo xmlElementInfo = this.ReadElement(this.reader.LocalName, this.DocumentElementLocation);
			XmlElementParser elementParser;
			if (!this.TryGetRootElementParser(this.DocumentVersion, xmlElementInfo, out elementParser))
			{
				this.ReportUnexpectedRootElement(xmlElementInfo.Location, xmlElementInfo.Name, this.DocumentNamespace);
				return;
			}
			this.BeginElement(elementParser, xmlElementInfo);
			if (isEmptyElement)
			{
				this.EndElement();
				return;
			}
			this.Parse();
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0000FBD5 File Offset: 0x0000DDD5
		protected void ReportError(CsdlLocation errorLocation, EdmErrorCode errorCode, string errorMessage)
		{
			this.errors.Add(new EdmError(errorLocation, errorCode, errorMessage));
			this.HasErrors = true;
		}

		// Token: 0x0600064F RID: 1615
		protected abstract XmlReader InitializeReader(XmlReader inputReader);

		// Token: 0x06000650 RID: 1616
		protected abstract bool TryGetDocumentVersion(string xmlNamespaceName, out Version version, out string[] expectedNamespaces);

		// Token: 0x06000651 RID: 1617
		protected abstract bool TryGetRootElementParser(Version artifactVersion, XmlElementInfo rootElement, out XmlElementParser parser);

		// Token: 0x06000652 RID: 1618 RVA: 0x0000FBF1 File Offset: 0x0000DDF1
		protected virtual bool IsOwnedNamespace(string namespaceName)
		{
			return this.DocumentNamespace.EqualsOrdinal(namespaceName);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0000FBFF File Offset: 0x0000DDFF
		protected virtual XmlElementParser<TResult> Element<TResult>(string elementName, Func<XmlElementInfo, XmlElementValueCollection, TResult> parserFunc, params XmlElementParser[] childParsers)
		{
			return XmlElementParser.Create<TResult>(elementName, parserFunc, childParsers, null);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0000FC0A File Offset: 0x0000DE0A
		private void Parse()
		{
			while (this.currentBranch.Count > 0 && this.reader.Read())
			{
				this.ProcessNode();
			}
			if (this.reader.EOF)
			{
				return;
			}
			this.reader.Read();
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0000FC74 File Offset: 0x0000DE74
		private void EndElement()
		{
			XmlDocumentParser.ElementScope elementScope = this.currentBranch.Pop();
			this.currentScope = ((this.currentBranch.Count > 0) ? this.currentBranch.Peek() : null);
			XmlElementParser parser = elementScope.Parser;
			XmlElementValue xmlElementValue = parser.Parse(elementScope.Element, elementScope.ChildValues);
			if (xmlElementValue != null)
			{
				if (this.currentScope != null)
				{
					this.currentScope.AddChildValue(xmlElementValue);
				}
				else
				{
					this.Result = xmlElementValue;
				}
			}
			foreach (XmlAttributeInfo xmlAttributeInfo in elementScope.Element.Attributes.Unused)
			{
				this.ReportUnexpectedAttribute(xmlAttributeInfo.Location, xmlAttributeInfo.Name);
			}
			IEnumerable<XmlElementValue> source = from v in elementScope.ChildValues
			where v.IsText
			select v;
			IEnumerable<XmlElementValue> source2 = from t in source
			where !t.IsUsed
			select t;
			if (source2.Any<XmlElementValue>())
			{
				XmlTextValue xmlTextValue;
				if (source2.Count<XmlElementValue>() == source.Count<XmlElementValue>())
				{
					xmlTextValue = (XmlTextValue)source.First<XmlElementValue>();
				}
				else
				{
					xmlTextValue = (XmlTextValue)source2.First<XmlElementValue>();
				}
				this.ReportTextNotAllowed(xmlTextValue.Location, xmlTextValue.Value);
			}
			foreach (XmlElementValue xmlElementValue2 in from v in elementScope.ChildValues
			where !v.IsText && !v.IsUsed
			select v)
			{
				this.ReportUnusedElement(xmlElementValue2.Location, xmlElementValue2.Name);
			}
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0000FE50 File Offset: 0x0000E050
		private void BeginElement(XmlElementParser elementParser, XmlElementInfo element)
		{
			XmlDocumentParser.ElementScope item = new XmlDocumentParser.ElementScope(elementParser, element);
			this.currentBranch.Push(item);
			this.currentScope = item;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0000FE78 File Offset: 0x0000E078
		private void ProcessNode()
		{
			if (this.IsTextNode)
			{
				if (this.currentText == null)
				{
					this.currentText = new StringBuilder();
					this.currentTextLocation = this.Location;
				}
				this.currentText.Append(this.reader.Value);
				return;
			}
			if (this.currentText != null)
			{
				string text = this.currentText.ToString();
				CsdlLocation textLocation = this.currentTextLocation;
				this.currentText = null;
				this.currentTextLocation = null;
				if (!EdmUtil.IsNullOrWhiteSpaceInternal(text) && !string.IsNullOrEmpty(text))
				{
					this.currentScope.AddChildValue(new XmlTextValue(textLocation, text));
				}
			}
			switch (this.reader.NodeType)
			{
			case XmlNodeType.Element:
				this.ProcessElement();
				return;
			case XmlNodeType.EntityReference:
			case XmlNodeType.DocumentType:
				this.reader.Skip();
				return;
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
			case XmlNodeType.Notation:
			case XmlNodeType.Whitespace:
			case XmlNodeType.XmlDeclaration:
				return;
			case XmlNodeType.EndElement:
				this.EndElement();
				return;
			}
			this.ReportUnexpectedNodeType(this.reader.NodeType);
			this.reader.Skip();
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0000FFA0 File Offset: 0x0000E1A0
		private void ProcessElement()
		{
			bool isEmptyElement = this.reader.IsEmptyElement;
			string namespaceURI = this.reader.NamespaceURI;
			string localName = this.reader.LocalName;
			if (namespaceURI == this.DocumentNamespace)
			{
				XmlElementParser elementParser;
				if (!this.currentScope.Parser.TryGetChildElementParser(localName, out elementParser))
				{
					this.ReportUnexpectedElement(this.Location, this.reader.Name);
					if (!isEmptyElement)
					{
						this.reader.Read();
					}
					return;
				}
				XmlElementInfo element = this.ReadElement(localName, this.Location);
				this.BeginElement(elementParser, element);
				if (isEmptyElement)
				{
					this.EndElement();
					return;
				}
			}
			else
			{
				if (string.IsNullOrEmpty(namespaceURI) || this.IsOwnedNamespace(namespaceURI))
				{
					this.ReportUnexpectedElement(this.Location, this.reader.Name);
					this.reader.Skip();
					return;
				}
				XmlReader xmlReader = this.reader.ReadSubtree();
				xmlReader.MoveToContent();
				string value = xmlReader.ReadOuterXml();
				this.currentScope.Element.AddAnnotation(new XmlAnnotationInfo(this.Location, namespaceURI, localName, value, false));
			}
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x000100B0 File Offset: 0x0000E2B0
		private XmlElementInfo ReadElement(string elementName, CsdlLocation elementLocation)
		{
			List<XmlAttributeInfo> list = null;
			List<XmlAnnotationInfo> list2 = null;
			bool flag = this.reader.MoveToFirstAttribute();
			while (flag)
			{
				string namespaceURI = this.reader.NamespaceURI;
				if (string.IsNullOrEmpty(namespaceURI) || namespaceURI.EqualsOrdinal(this.DocumentNamespace))
				{
					if (list == null)
					{
						list = new List<XmlAttributeInfo>();
					}
					list.Add(new XmlAttributeInfo(this.reader.LocalName, this.reader.Value, this.Location));
				}
				else if (this.IsOwnedNamespace(namespaceURI))
				{
					this.ReportUnexpectedAttribute(this.Location, this.reader.Name);
				}
				else
				{
					if (list2 == null)
					{
						list2 = new List<XmlAnnotationInfo>();
					}
					list2.Add(new XmlAnnotationInfo(this.Location, this.reader.NamespaceURI, this.reader.LocalName, this.reader.Value, true));
				}
				flag = this.reader.MoveToNextAttribute();
			}
			return new XmlElementInfo(elementName, elementLocation, list, list2);
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x000101A0 File Offset: 0x0000E3A0
		private void ReportEmptyFile()
		{
			string errorMessage = (this.DocumentPath == null) ? Strings.XmlParser_EmptySchemaTextReader : Strings.XmlParser_EmptyFile(this.DocumentPath);
			this.ReportError(this.Location, EdmErrorCode.EmptyFile, errorMessage);
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x000101D8 File Offset: 0x0000E3D8
		private void ReportUnexpectedRootNamespace(string elementName, string namespaceUri, string[] expectedNamespaces)
		{
			string text = string.Join(", ", expectedNamespaces);
			string errorMessage = string.IsNullOrEmpty(namespaceUri) ? Strings.XmlParser_UnexpectedRootElementNoNamespace(text) : Strings.XmlParser_UnexpectedRootElementWrongNamespace(namespaceUri, text);
			this.ReportError(this.Location, EdmErrorCode.UnexpectedXmlElement, errorMessage);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00010218 File Offset: 0x0000E418
		private void ReportUnexpectedRootElement(CsdlLocation elementLocation, string elementName, string expectedNamespace)
		{
			this.ReportError(elementLocation, EdmErrorCode.UnexpectedXmlElement, Strings.XmlParser_UnexpectedRootElement(elementName, "Schema"));
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0001022E File Offset: 0x0000E42E
		private void ReportUnexpectedAttribute(CsdlLocation errorLocation, string attributeName)
		{
			this.ReportError(errorLocation, EdmErrorCode.UnexpectedXmlAttribute, Strings.XmlParser_UnexpectedAttribute(attributeName));
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0001023F File Offset: 0x0000E43F
		private void ReportUnexpectedNodeType(XmlNodeType nodeType)
		{
			this.ReportError(this.Location, EdmErrorCode.UnexpectedXmlNodeType, Strings.XmlParser_UnexpectedNodeType(nodeType));
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00010259 File Offset: 0x0000E459
		private void ReportUnexpectedElement(CsdlLocation errorLocation, string elementName)
		{
			this.ReportError(errorLocation, EdmErrorCode.UnexpectedXmlElement, Strings.XmlParser_UnexpectedElement(elementName));
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0001026A File Offset: 0x0000E46A
		private void ReportUnusedElement(CsdlLocation errorLocation, string elementName)
		{
			this.ReportError(errorLocation, EdmErrorCode.UnexpectedXmlElement, Strings.XmlParser_UnusedElement(elementName));
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0001027B File Offset: 0x0000E47B
		private void ReportTextNotAllowed(CsdlLocation errorLocation, string textValue)
		{
			this.ReportError(errorLocation, EdmErrorCode.TextNotAllowed, Strings.XmlParser_TextNotAllowed(textValue));
		}

		// Token: 0x0400035B RID: 859
		private readonly string docPath;

		// Token: 0x0400035C RID: 860
		private readonly Stack<XmlDocumentParser.ElementScope> currentBranch = new Stack<XmlDocumentParser.ElementScope>();

		// Token: 0x0400035D RID: 861
		private XmlReader reader;

		// Token: 0x0400035E RID: 862
		private IXmlLineInfo xmlLineInfo;

		// Token: 0x0400035F RID: 863
		private List<EdmError> errors;

		// Token: 0x04000360 RID: 864
		private StringBuilder currentText;

		// Token: 0x04000361 RID: 865
		private CsdlLocation currentTextLocation;

		// Token: 0x04000362 RID: 866
		private XmlDocumentParser.ElementScope currentScope;

		// Token: 0x02000150 RID: 336
		private class ElementScope
		{
			// Token: 0x06000665 RID: 1637 RVA: 0x0001028C File Offset: 0x0000E48C
			internal ElementScope(XmlElementParser parser, XmlElementInfo element)
			{
				this.Parser = parser;
				this.Element = element;
			}

			// Token: 0x170002B5 RID: 693
			// (get) Token: 0x06000666 RID: 1638 RVA: 0x000102A2 File Offset: 0x0000E4A2
			// (set) Token: 0x06000667 RID: 1639 RVA: 0x000102AA File Offset: 0x0000E4AA
			internal XmlElementParser Parser { get; private set; }

			// Token: 0x170002B6 RID: 694
			// (get) Token: 0x06000668 RID: 1640 RVA: 0x000102B3 File Offset: 0x0000E4B3
			// (set) Token: 0x06000669 RID: 1641 RVA: 0x000102BB File Offset: 0x0000E4BB
			internal XmlElementInfo Element { get; private set; }

			// Token: 0x170002B7 RID: 695
			// (get) Token: 0x0600066A RID: 1642 RVA: 0x000102C4 File Offset: 0x0000E4C4
			internal IList<XmlElementValue> ChildValues
			{
				get
				{
					return this.childValues ?? XmlDocumentParser.ElementScope.EmptyValues;
				}
			}

			// Token: 0x0600066B RID: 1643 RVA: 0x000102D5 File Offset: 0x0000E4D5
			internal void AddChildValue(XmlElementValue value)
			{
				if (this.childValues == null)
				{
					this.childValues = new List<XmlElementValue>();
				}
				this.childValues.Add(value);
			}

			// Token: 0x0400036B RID: 875
			private static readonly IList<XmlElementValue> EmptyValues = new ReadOnlyCollection<XmlElementValue>(new XmlElementValue[0]);

			// Token: 0x0400036C RID: 876
			private List<XmlElementValue> childValues;
		}
	}
}
