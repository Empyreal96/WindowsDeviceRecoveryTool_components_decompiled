using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000221 RID: 545
	internal sealed class BufferingXmlReader : XmlReader
	{
		// Token: 0x060010E7 RID: 4327 RVA: 0x0003F29C File Offset: 0x0003D49C
		internal BufferingXmlReader(XmlReader reader, Uri parentXmlBaseUri, Uri documentBaseUri, bool disableXmlBase, int maxInnerErrorDepth, string odataNamespace)
		{
			this.reader = reader;
			this.documentBaseUri = documentBaseUri;
			this.disableXmlBase = disableXmlBase;
			this.maxInnerErrorDepth = maxInnerErrorDepth;
			XmlNameTable nameTable = this.reader.NameTable;
			this.XmlNamespace = nameTable.Add("http://www.w3.org/XML/1998/namespace");
			this.XmlBaseAttributeName = nameTable.Add("base");
			this.XmlLangAttributeName = nameTable.Add("lang");
			this.ODataMetadataNamespace = nameTable.Add("http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
			this.ODataNamespace = nameTable.Add(odataNamespace);
			this.ODataErrorElementName = nameTable.Add("error");
			this.bufferedNodes = new LinkedList<BufferingXmlReader.BufferedNode>();
			this.currentBufferedNode = null;
			this.endOfInputBufferedNode = BufferingXmlReader.BufferedNode.CreateEndOfInput(this.reader.NameTable);
			this.xmlBaseStack = new Stack<BufferingXmlReader.XmlBaseDefinition>();
			if (parentXmlBaseUri != null)
			{
				this.xmlBaseStack.Push(new BufferingXmlReader.XmlBaseDefinition(parentXmlBaseUri, 0));
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060010E8 RID: 4328 RVA: 0x0003F38A File Offset: 0x0003D58A
		public override XmlNodeType NodeType
		{
			get
			{
				if (this.currentBufferedNodeToReport == null)
				{
					return this.reader.NodeType;
				}
				return this.currentBufferedNodeToReport.Value.NodeType;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060010E9 RID: 4329 RVA: 0x0003F3B0 File Offset: 0x0003D5B0
		public override bool IsEmptyElement
		{
			get
			{
				if (this.currentBufferedNodeToReport == null)
				{
					return this.reader.IsEmptyElement;
				}
				return this.currentBufferedNodeToReport.Value.IsEmptyElement;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060010EA RID: 4330 RVA: 0x0003F3D6 File Offset: 0x0003D5D6
		public override string LocalName
		{
			get
			{
				if (this.currentBufferedNodeToReport == null)
				{
					return this.reader.LocalName;
				}
				return this.currentBufferedNodeToReport.Value.LocalName;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x060010EB RID: 4331 RVA: 0x0003F3FC File Offset: 0x0003D5FC
		public override string Prefix
		{
			get
			{
				if (this.currentBufferedNodeToReport == null)
				{
					return this.reader.Prefix;
				}
				return this.currentBufferedNodeToReport.Value.Prefix;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x060010EC RID: 4332 RVA: 0x0003F422 File Offset: 0x0003D622
		public override string NamespaceURI
		{
			get
			{
				if (this.currentBufferedNodeToReport == null)
				{
					return this.reader.NamespaceURI;
				}
				return this.currentBufferedNodeToReport.Value.NamespaceUri;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x060010ED RID: 4333 RVA: 0x0003F448 File Offset: 0x0003D648
		public override string Value
		{
			get
			{
				if (this.currentBufferedNodeToReport == null)
				{
					return this.reader.Value;
				}
				return this.currentBufferedNodeToReport.Value.Value;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x0003F46E File Offset: 0x0003D66E
		public override int Depth
		{
			get
			{
				if (this.currentBufferedNodeToReport == null)
				{
					return this.reader.Depth;
				}
				return this.currentBufferedNodeToReport.Value.Depth;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x0003F494 File Offset: 0x0003D694
		public override bool EOF
		{
			get
			{
				if (this.currentBufferedNodeToReport == null)
				{
					return this.reader.EOF;
				}
				return this.IsEndOfInputNode(this.currentBufferedNodeToReport.Value);
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x0003F4BB File Offset: 0x0003D6BB
		public override ReadState ReadState
		{
			get
			{
				if (this.currentBufferedNodeToReport == null)
				{
					return this.reader.ReadState;
				}
				if (this.IsEndOfInputNode(this.currentBufferedNodeToReport.Value))
				{
					return ReadState.EndOfFile;
				}
				if (this.currentBufferedNodeToReport.Value.NodeType != XmlNodeType.None)
				{
					return ReadState.Interactive;
				}
				return ReadState.Initial;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060010F1 RID: 4337 RVA: 0x0003F4FB File Offset: 0x0003D6FB
		public override XmlNameTable NameTable
		{
			get
			{
				return this.reader.NameTable;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x0003F508 File Offset: 0x0003D708
		public override int AttributeCount
		{
			get
			{
				if (this.currentBufferedNodeToReport == null)
				{
					return this.reader.AttributeCount;
				}
				if (this.currentBufferedNodeToReport.Value.AttributeNodes == null)
				{
					return 0;
				}
				return this.currentBufferedNodeToReport.Value.AttributeNodes.Count;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x0003F547 File Offset: 0x0003D747
		public override string BaseURI
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x060010F4 RID: 4340 RVA: 0x0003F54C File Offset: 0x0003D74C
		public override bool HasValue
		{
			get
			{
				if (this.currentBufferedNodeToReport != null)
				{
					switch (this.NodeType)
					{
					case XmlNodeType.Attribute:
					case XmlNodeType.Text:
					case XmlNodeType.CDATA:
					case XmlNodeType.ProcessingInstruction:
					case XmlNodeType.Comment:
					case XmlNodeType.DocumentType:
					case XmlNodeType.Whitespace:
					case XmlNodeType.SignificantWhitespace:
					case XmlNodeType.XmlDeclaration:
						return true;
					}
					return false;
				}
				return this.reader.HasValue;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x0003F5C1 File Offset: 0x0003D7C1
		internal Uri XmlBaseUri
		{
			get
			{
				if (this.xmlBaseStack.Count <= 0)
				{
					return null;
				}
				return this.xmlBaseStack.Peek().BaseUri;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x0003F5E4 File Offset: 0x0003D7E4
		internal Uri ParentXmlBaseUri
		{
			get
			{
				if (this.xmlBaseStack.Count == 0)
				{
					return null;
				}
				BufferingXmlReader.XmlBaseDefinition xmlBaseDefinition = this.xmlBaseStack.Peek();
				if (xmlBaseDefinition.Depth == this.Depth)
				{
					if (this.xmlBaseStack.Count == 1)
					{
						return null;
					}
					xmlBaseDefinition = this.xmlBaseStack.Skip(1).First<BufferingXmlReader.XmlBaseDefinition>();
				}
				return xmlBaseDefinition.BaseUri;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x060010F7 RID: 4343 RVA: 0x0003F642 File Offset: 0x0003D842
		// (set) Token: 0x060010F8 RID: 4344 RVA: 0x0003F64A File Offset: 0x0003D84A
		internal bool DisableInStreamErrorDetection
		{
			get
			{
				return this.disableInStreamErrorDetection;
			}
			set
			{
				this.disableInStreamErrorDetection = value;
			}
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0003F654 File Offset: 0x0003D854
		public override bool Read()
		{
			if (!this.disableXmlBase && this.xmlBaseStack.Count > 0)
			{
				XmlNodeType xmlNodeType = this.NodeType;
				if (xmlNodeType == XmlNodeType.Attribute)
				{
					this.MoveToElement();
					xmlNodeType = XmlNodeType.Element;
				}
				if (this.xmlBaseStack.Peek().Depth == this.Depth && (xmlNodeType == XmlNodeType.EndElement || (xmlNodeType == XmlNodeType.Element && this.IsEmptyElement)))
				{
					this.xmlBaseStack.Pop();
				}
			}
			bool flag = this.ReadInternal(this.disableInStreamErrorDetection);
			if (flag && !this.disableXmlBase && this.NodeType == XmlNodeType.Element)
			{
				string attributeWithAtomizedName = this.GetAttributeWithAtomizedName(this.XmlBaseAttributeName, this.XmlNamespace);
				if (attributeWithAtomizedName != null)
				{
					Uri uri = new Uri(attributeWithAtomizedName, UriKind.RelativeOrAbsolute);
					if (!uri.IsAbsoluteUri)
					{
						if (this.xmlBaseStack.Count == 0)
						{
							if (this.documentBaseUri == null)
							{
								throw new ODataException(Strings.ODataAtomDeserializer_RelativeUriUsedWithoutBaseUriSpecified(attributeWithAtomizedName));
							}
							uri = UriUtils.UriToAbsoluteUri(this.documentBaseUri, uri);
						}
						else
						{
							uri = UriUtils.UriToAbsoluteUri(this.xmlBaseStack.Peek().BaseUri, uri);
						}
					}
					this.xmlBaseStack.Push(new BufferingXmlReader.XmlBaseDefinition(uri, this.Depth));
				}
			}
			return flag;
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x0003F778 File Offset: 0x0003D978
		public override bool MoveToElement()
		{
			if (this.bufferedNodes.Count <= 0)
			{
				return this.reader.MoveToElement();
			}
			this.MoveFromAttributeValueNode();
			if (this.isBuffering)
			{
				if (this.currentBufferedNodeToReport.Value.NodeType == XmlNodeType.Attribute)
				{
					this.currentBufferedNodeToReport = this.currentBufferedNode;
					return true;
				}
				return false;
			}
			else
			{
				if (this.currentBufferedNodeToReport.Value.NodeType == XmlNodeType.Attribute)
				{
					this.currentBufferedNodeToReport = this.bufferedNodes.First;
					return true;
				}
				return false;
			}
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0003F7F8 File Offset: 0x0003D9F8
		public override bool MoveToFirstAttribute()
		{
			if (this.bufferedNodes.Count <= 0)
			{
				return this.reader.MoveToFirstAttribute();
			}
			BufferingXmlReader.BufferedNode currentElementNode = this.GetCurrentElementNode();
			if (currentElementNode.NodeType == XmlNodeType.Element && currentElementNode.AttributeNodes.Count > 0)
			{
				this.currentAttributeNode = null;
				this.currentBufferedNodeToReport = currentElementNode.AttributeNodes.First;
				return true;
			}
			return false;
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0003F858 File Offset: 0x0003DA58
		public override bool MoveToNextAttribute()
		{
			if (this.bufferedNodes.Count <= 0)
			{
				return this.reader.MoveToNextAttribute();
			}
			LinkedListNode<BufferingXmlReader.BufferedNode> linkedListNode = this.currentAttributeNode;
			if (linkedListNode == null)
			{
				linkedListNode = this.currentBufferedNodeToReport;
			}
			if (linkedListNode.Value.NodeType == XmlNodeType.Attribute)
			{
				if (linkedListNode.Next != null)
				{
					this.currentAttributeNode = null;
					this.currentBufferedNodeToReport = linkedListNode.Next;
					return true;
				}
				return false;
			}
			else
			{
				if (this.currentBufferedNodeToReport.Value.NodeType != XmlNodeType.Element)
				{
					return false;
				}
				if (this.currentBufferedNodeToReport.Value.AttributeNodes.Count > 0)
				{
					this.currentBufferedNodeToReport = this.currentBufferedNodeToReport.Value.AttributeNodes.First;
					return true;
				}
				return false;
			}
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x0003F90C File Offset: 0x0003DB0C
		public override bool ReadAttributeValue()
		{
			if (this.bufferedNodes.Count <= 0)
			{
				return this.reader.ReadAttributeValue();
			}
			if (this.currentBufferedNodeToReport.Value.NodeType != XmlNodeType.Attribute)
			{
				return false;
			}
			if (this.currentAttributeNode != null)
			{
				return false;
			}
			BufferingXmlReader.BufferedNode value = new BufferingXmlReader.BufferedNode(this.currentBufferedNodeToReport.Value.Value, this.currentBufferedNodeToReport.Value.Depth, this.NameTable);
			LinkedListNode<BufferingXmlReader.BufferedNode> linkedListNode = new LinkedListNode<BufferingXmlReader.BufferedNode>(value);
			this.currentAttributeNode = this.currentBufferedNodeToReport;
			this.currentBufferedNodeToReport = linkedListNode;
			return true;
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0003F999 File Offset: 0x0003DB99
		public override void Close()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0003F9A0 File Offset: 0x0003DBA0
		public override string GetAttribute(int i)
		{
			if (this.bufferedNodes.Count <= 0)
			{
				return this.reader.GetAttribute(i);
			}
			if (i < 0 || i >= this.AttributeCount)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			LinkedListNode<BufferingXmlReader.BufferedNode> linkedListNode = this.FindAttributeBufferedNode(i);
			if (linkedListNode != null)
			{
				return linkedListNode.Value.Value;
			}
			return null;
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0003F9F8 File Offset: 0x0003DBF8
		public override string GetAttribute(string name, string namespaceURI)
		{
			if (this.bufferedNodes.Count <= 0)
			{
				return this.reader.GetAttribute(name, namespaceURI);
			}
			LinkedListNode<BufferingXmlReader.BufferedNode> linkedListNode = this.FindAttributeBufferedNode(name, namespaceURI);
			if (linkedListNode != null)
			{
				return linkedListNode.Value.Value;
			}
			return null;
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x0003FA3C File Offset: 0x0003DC3C
		public override string GetAttribute(string name)
		{
			if (this.bufferedNodes.Count <= 0)
			{
				return this.reader.GetAttribute(name);
			}
			LinkedListNode<BufferingXmlReader.BufferedNode> linkedListNode = this.FindAttributeBufferedNode(name);
			if (linkedListNode != null)
			{
				return linkedListNode.Value.Value;
			}
			return null;
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x0003FA7C File Offset: 0x0003DC7C
		public override string LookupNamespace(string prefix)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0003FA84 File Offset: 0x0003DC84
		public override bool MoveToAttribute(string name, string ns)
		{
			if (this.bufferedNodes.Count <= 0)
			{
				return this.reader.MoveToAttribute(name, ns);
			}
			LinkedListNode<BufferingXmlReader.BufferedNode> linkedListNode = this.FindAttributeBufferedNode(name, ns);
			if (linkedListNode != null)
			{
				this.currentAttributeNode = null;
				this.currentBufferedNodeToReport = linkedListNode;
				return true;
			}
			return false;
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x0003FACC File Offset: 0x0003DCCC
		public override bool MoveToAttribute(string name)
		{
			if (this.bufferedNodes.Count <= 0)
			{
				return this.reader.MoveToAttribute(name);
			}
			LinkedListNode<BufferingXmlReader.BufferedNode> linkedListNode = this.FindAttributeBufferedNode(name);
			if (linkedListNode != null)
			{
				this.currentAttributeNode = null;
				this.currentBufferedNodeToReport = linkedListNode;
				return true;
			}
			return false;
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x0003FB10 File Offset: 0x0003DD10
		public override void ResolveEntity()
		{
			throw new InvalidOperationException(Strings.ODataException_GeneralError);
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x0003FB1C File Offset: 0x0003DD1C
		internal void StartBuffering()
		{
			if (this.bufferedNodes.Count == 0)
			{
				this.bufferedNodes.AddFirst(this.BufferCurrentReaderNode());
			}
			else
			{
				this.removeOnNextRead = false;
			}
			this.currentBufferedNode = this.bufferedNodes.First;
			this.currentBufferedNodeToReport = this.currentBufferedNode;
			int count = this.xmlBaseStack.Count;
			switch (count)
			{
			case 0:
				this.bufferStartXmlBaseStack = new Stack<BufferingXmlReader.XmlBaseDefinition>();
				break;
			case 1:
				this.bufferStartXmlBaseStack = new Stack<BufferingXmlReader.XmlBaseDefinition>();
				this.bufferStartXmlBaseStack.Push(this.xmlBaseStack.Peek());
				break;
			default:
			{
				BufferingXmlReader.XmlBaseDefinition[] array = this.xmlBaseStack.ToArray();
				this.bufferStartXmlBaseStack = new Stack<BufferingXmlReader.XmlBaseDefinition>(count);
				for (int i = count - 1; i >= 0; i--)
				{
					this.bufferStartXmlBaseStack.Push(array[i]);
				}
				break;
			}
			}
			this.isBuffering = true;
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0003FBF8 File Offset: 0x0003DDF8
		internal void StopBuffering()
		{
			this.isBuffering = false;
			this.removeOnNextRead = true;
			this.currentBufferedNode = null;
			if (this.bufferedNodes.Count > 0)
			{
				this.currentBufferedNodeToReport = this.bufferedNodes.First;
			}
			this.xmlBaseStack = this.bufferStartXmlBaseStack;
			this.bufferStartXmlBaseStack = null;
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x0003FC4C File Offset: 0x0003DE4C
		private bool ReadInternal(bool ignoreInStreamErrors)
		{
			if (this.removeOnNextRead)
			{
				this.currentBufferedNodeToReport = this.currentBufferedNodeToReport.Next;
				this.bufferedNodes.RemoveFirst();
				this.removeOnNextRead = false;
			}
			bool result;
			if (this.isBuffering)
			{
				this.MoveFromAttributeValueNode();
				if (this.currentBufferedNode.Next != null)
				{
					this.currentBufferedNode = this.currentBufferedNode.Next;
					this.currentBufferedNodeToReport = this.currentBufferedNode;
					result = true;
				}
				else if (ignoreInStreamErrors)
				{
					result = this.reader.Read();
					this.bufferedNodes.AddLast(this.BufferCurrentReaderNode());
					this.currentBufferedNode = this.bufferedNodes.Last;
					this.currentBufferedNodeToReport = this.currentBufferedNode;
				}
				else
				{
					result = this.ReadNextAndCheckForInStreamError();
				}
			}
			else if (this.bufferedNodes.Count == 0)
			{
				result = (ignoreInStreamErrors ? this.reader.Read() : this.ReadNextAndCheckForInStreamError());
			}
			else
			{
				this.currentBufferedNodeToReport = this.bufferedNodes.First;
				BufferingXmlReader.BufferedNode value = this.currentBufferedNodeToReport.Value;
				result = !this.IsEndOfInputNode(value);
				this.removeOnNextRead = true;
			}
			return result;
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0003FD68 File Offset: 0x0003DF68
		private bool ReadNextAndCheckForInStreamError()
		{
			bool result = this.ReadInternal(true);
			if (!this.disableInStreamErrorDetection && this.NodeType == XmlNodeType.Element && this.LocalNameEquals(this.ODataErrorElementName) && this.NamespaceEquals(this.ODataMetadataNamespace))
			{
				ODataError error = ODataAtomErrorDeserializer.ReadErrorElement(this, this.maxInnerErrorDepth);
				throw new ODataErrorException(error);
			}
			return result;
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x0003FDBF File Offset: 0x0003DFBF
		private bool IsEndOfInputNode(BufferingXmlReader.BufferedNode node)
		{
			return object.ReferenceEquals(node, this.endOfInputBufferedNode);
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x0003FDD0 File Offset: 0x0003DFD0
		private BufferingXmlReader.BufferedNode BufferCurrentReaderNode()
		{
			if (this.reader.EOF)
			{
				return this.endOfInputBufferedNode;
			}
			BufferingXmlReader.BufferedNode bufferedNode = new BufferingXmlReader.BufferedNode(this.reader);
			if (this.reader.NodeType == XmlNodeType.Element)
			{
				while (this.reader.MoveToNextAttribute())
				{
					bufferedNode.AttributeNodes.AddLast(new BufferingXmlReader.BufferedNode(this.reader));
				}
				this.reader.MoveToElement();
			}
			return bufferedNode;
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x0003FE3E File Offset: 0x0003E03E
		private BufferingXmlReader.BufferedNode GetCurrentElementNode()
		{
			if (this.isBuffering)
			{
				return this.currentBufferedNode.Value;
			}
			return this.bufferedNodes.First.Value;
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x0003FE64 File Offset: 0x0003E064
		private LinkedListNode<BufferingXmlReader.BufferedNode> FindAttributeBufferedNode(int index)
		{
			BufferingXmlReader.BufferedNode currentElementNode = this.GetCurrentElementNode();
			if (currentElementNode.NodeType == XmlNodeType.Element && currentElementNode.AttributeNodes.Count > 0)
			{
				LinkedListNode<BufferingXmlReader.BufferedNode> linkedListNode = currentElementNode.AttributeNodes.First;
				int num = 0;
				while (linkedListNode != null)
				{
					if (num == index)
					{
						return linkedListNode;
					}
					num++;
					linkedListNode = linkedListNode.Next;
				}
			}
			return null;
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x0003FEB4 File Offset: 0x0003E0B4
		private LinkedListNode<BufferingXmlReader.BufferedNode> FindAttributeBufferedNode(string localName, string namespaceUri)
		{
			BufferingXmlReader.BufferedNode currentElementNode = this.GetCurrentElementNode();
			if (currentElementNode.NodeType == XmlNodeType.Element && currentElementNode.AttributeNodes.Count > 0)
			{
				for (LinkedListNode<BufferingXmlReader.BufferedNode> linkedListNode = currentElementNode.AttributeNodes.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					BufferingXmlReader.BufferedNode value = linkedListNode.Value;
					if (string.CompareOrdinal(value.NamespaceUri, namespaceUri) == 0 && string.CompareOrdinal(value.LocalName, localName) == 0)
					{
						return linkedListNode;
					}
				}
			}
			return null;
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x0003FF20 File Offset: 0x0003E120
		private LinkedListNode<BufferingXmlReader.BufferedNode> FindAttributeBufferedNode(string qualifiedName)
		{
			BufferingXmlReader.BufferedNode currentElementNode = this.GetCurrentElementNode();
			if (currentElementNode.NodeType == XmlNodeType.Element && currentElementNode.AttributeNodes.Count > 0)
			{
				for (LinkedListNode<BufferingXmlReader.BufferedNode> linkedListNode = currentElementNode.AttributeNodes.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					BufferingXmlReader.BufferedNode value = linkedListNode.Value;
					bool flag = !string.IsNullOrEmpty(value.Prefix);
					if ((!flag && string.CompareOrdinal(value.LocalName, qualifiedName) == 0) || (flag && string.CompareOrdinal(value.Prefix + ":" + value.LocalName, qualifiedName) == 0))
					{
						return linkedListNode;
					}
				}
			}
			return null;
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x0003FFAE File Offset: 0x0003E1AE
		private void MoveFromAttributeValueNode()
		{
			if (this.currentAttributeNode != null)
			{
				this.currentBufferedNodeToReport = this.currentAttributeNode;
				this.currentAttributeNode = null;
			}
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x0003FFCC File Offset: 0x0003E1CC
		private string GetAttributeWithAtomizedName(string name, string namespaceURI)
		{
			if (this.bufferedNodes.Count > 0)
			{
				for (LinkedListNode<BufferingXmlReader.BufferedNode> linkedListNode = this.GetCurrentElementNode().AttributeNodes.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					BufferingXmlReader.BufferedNode value = linkedListNode.Value;
					if (object.ReferenceEquals(namespaceURI, value.NamespaceUri) && object.ReferenceEquals(name, value.LocalName))
					{
						return linkedListNode.Value.Value;
					}
				}
				return null;
			}
			string result = null;
			while (this.reader.MoveToNextAttribute())
			{
				if (object.ReferenceEquals(name, this.reader.LocalName) && object.ReferenceEquals(namespaceURI, this.reader.NamespaceURI))
				{
					result = this.reader.Value;
					break;
				}
			}
			this.reader.MoveToElement();
			return result;
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00040087 File Offset: 0x0003E287
		[Conditional("DEBUG")]
		private void ValidateInternalState()
		{
		}

		// Token: 0x04000637 RID: 1591
		internal readonly string XmlNamespace;

		// Token: 0x04000638 RID: 1592
		internal readonly string XmlBaseAttributeName;

		// Token: 0x04000639 RID: 1593
		internal readonly string XmlLangAttributeName;

		// Token: 0x0400063A RID: 1594
		internal readonly string ODataMetadataNamespace;

		// Token: 0x0400063B RID: 1595
		internal readonly string ODataNamespace;

		// Token: 0x0400063C RID: 1596
		internal readonly string ODataErrorElementName;

		// Token: 0x0400063D RID: 1597
		private readonly XmlReader reader;

		// Token: 0x0400063E RID: 1598
		private readonly LinkedList<BufferingXmlReader.BufferedNode> bufferedNodes;

		// Token: 0x0400063F RID: 1599
		private readonly BufferingXmlReader.BufferedNode endOfInputBufferedNode;

		// Token: 0x04000640 RID: 1600
		private readonly bool disableXmlBase;

		// Token: 0x04000641 RID: 1601
		private readonly int maxInnerErrorDepth;

		// Token: 0x04000642 RID: 1602
		private readonly Uri documentBaseUri;

		// Token: 0x04000643 RID: 1603
		private LinkedListNode<BufferingXmlReader.BufferedNode> currentBufferedNode;

		// Token: 0x04000644 RID: 1604
		private LinkedListNode<BufferingXmlReader.BufferedNode> currentAttributeNode;

		// Token: 0x04000645 RID: 1605
		private LinkedListNode<BufferingXmlReader.BufferedNode> currentBufferedNodeToReport;

		// Token: 0x04000646 RID: 1606
		private bool isBuffering;

		// Token: 0x04000647 RID: 1607
		private bool removeOnNextRead;

		// Token: 0x04000648 RID: 1608
		private bool disableInStreamErrorDetection;

		// Token: 0x04000649 RID: 1609
		private Stack<BufferingXmlReader.XmlBaseDefinition> xmlBaseStack;

		// Token: 0x0400064A RID: 1610
		private Stack<BufferingXmlReader.XmlBaseDefinition> bufferStartXmlBaseStack;

		// Token: 0x02000222 RID: 546
		private sealed class BufferedNode
		{
			// Token: 0x06001113 RID: 4371 RVA: 0x0004008C File Offset: 0x0003E28C
			internal BufferedNode(XmlReader reader)
			{
				this.NodeType = reader.NodeType;
				this.NamespaceUri = reader.NamespaceURI;
				this.LocalName = reader.LocalName;
				this.Prefix = reader.Prefix;
				this.Value = reader.Value;
				this.Depth = reader.Depth;
				this.IsEmptyElement = reader.IsEmptyElement;
			}

			// Token: 0x06001114 RID: 4372 RVA: 0x000400F4 File Offset: 0x0003E2F4
			internal BufferedNode(string value, int depth, XmlNameTable nametable)
			{
				string text = nametable.Add(string.Empty);
				this.NodeType = XmlNodeType.Text;
				this.NamespaceUri = text;
				this.LocalName = text;
				this.Prefix = text;
				this.Value = value;
				this.Depth = depth + 1;
				this.IsEmptyElement = false;
			}

			// Token: 0x06001115 RID: 4373 RVA: 0x00040146 File Offset: 0x0003E346
			private BufferedNode(string emptyString)
			{
				this.NodeType = XmlNodeType.None;
				this.NamespaceUri = emptyString;
				this.LocalName = emptyString;
				this.Prefix = emptyString;
				this.Value = emptyString;
			}

			// Token: 0x170003A6 RID: 934
			// (get) Token: 0x06001116 RID: 4374 RVA: 0x00040171 File Offset: 0x0003E371
			// (set) Token: 0x06001117 RID: 4375 RVA: 0x00040179 File Offset: 0x0003E379
			internal XmlNodeType NodeType { get; private set; }

			// Token: 0x170003A7 RID: 935
			// (get) Token: 0x06001118 RID: 4376 RVA: 0x00040182 File Offset: 0x0003E382
			// (set) Token: 0x06001119 RID: 4377 RVA: 0x0004018A File Offset: 0x0003E38A
			internal string NamespaceUri { get; private set; }

			// Token: 0x170003A8 RID: 936
			// (get) Token: 0x0600111A RID: 4378 RVA: 0x00040193 File Offset: 0x0003E393
			// (set) Token: 0x0600111B RID: 4379 RVA: 0x0004019B File Offset: 0x0003E39B
			internal string LocalName { get; private set; }

			// Token: 0x170003A9 RID: 937
			// (get) Token: 0x0600111C RID: 4380 RVA: 0x000401A4 File Offset: 0x0003E3A4
			// (set) Token: 0x0600111D RID: 4381 RVA: 0x000401AC File Offset: 0x0003E3AC
			internal string Prefix { get; private set; }

			// Token: 0x170003AA RID: 938
			// (get) Token: 0x0600111E RID: 4382 RVA: 0x000401B5 File Offset: 0x0003E3B5
			// (set) Token: 0x0600111F RID: 4383 RVA: 0x000401BD File Offset: 0x0003E3BD
			internal string Value { get; private set; }

			// Token: 0x170003AB RID: 939
			// (get) Token: 0x06001120 RID: 4384 RVA: 0x000401C6 File Offset: 0x0003E3C6
			// (set) Token: 0x06001121 RID: 4385 RVA: 0x000401CE File Offset: 0x0003E3CE
			internal int Depth { get; private set; }

			// Token: 0x170003AC RID: 940
			// (get) Token: 0x06001122 RID: 4386 RVA: 0x000401D7 File Offset: 0x0003E3D7
			// (set) Token: 0x06001123 RID: 4387 RVA: 0x000401DF File Offset: 0x0003E3DF
			internal bool IsEmptyElement { get; private set; }

			// Token: 0x170003AD RID: 941
			// (get) Token: 0x06001124 RID: 4388 RVA: 0x000401E8 File Offset: 0x0003E3E8
			internal LinkedList<BufferingXmlReader.BufferedNode> AttributeNodes
			{
				get
				{
					if (this.NodeType == XmlNodeType.Element && this.attributeNodes == null)
					{
						this.attributeNodes = new LinkedList<BufferingXmlReader.BufferedNode>();
					}
					return this.attributeNodes;
				}
			}

			// Token: 0x06001125 RID: 4389 RVA: 0x0004020C File Offset: 0x0003E40C
			internal static BufferingXmlReader.BufferedNode CreateEndOfInput(XmlNameTable nametable)
			{
				string emptyString = nametable.Add(string.Empty);
				return new BufferingXmlReader.BufferedNode(emptyString);
			}

			// Token: 0x0400064B RID: 1611
			private LinkedList<BufferingXmlReader.BufferedNode> attributeNodes;
		}

		// Token: 0x02000223 RID: 547
		private sealed class XmlBaseDefinition
		{
			// Token: 0x06001126 RID: 4390 RVA: 0x0004022B File Offset: 0x0003E42B
			internal XmlBaseDefinition(Uri baseUri, int depth)
			{
				this.BaseUri = baseUri;
				this.Depth = depth;
			}

			// Token: 0x170003AE RID: 942
			// (get) Token: 0x06001127 RID: 4391 RVA: 0x00040241 File Offset: 0x0003E441
			// (set) Token: 0x06001128 RID: 4392 RVA: 0x00040249 File Offset: 0x0003E449
			internal Uri BaseUri { get; private set; }

			// Token: 0x170003AF RID: 943
			// (get) Token: 0x06001129 RID: 4393 RVA: 0x00040252 File Offset: 0x0003E452
			// (set) Token: 0x0600112A RID: 4394 RVA: 0x0004025A File Offset: 0x0003E45A
			internal int Depth { get; private set; }
		}
	}
}
