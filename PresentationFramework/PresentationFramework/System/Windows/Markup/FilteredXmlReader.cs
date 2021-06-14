using System;
using System.Xml;

namespace System.Windows.Markup
{
	// Token: 0x02000214 RID: 532
	internal class FilteredXmlReader : XmlTextReader
	{
		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06002110 RID: 8464 RVA: 0x00098514 File Offset: 0x00096714
		public override int AttributeCount
		{
			get
			{
				int attributeCount = base.AttributeCount;
				if (this.haveUid)
				{
					return attributeCount - 1;
				}
				return attributeCount;
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06002111 RID: 8465 RVA: 0x00098535 File Offset: 0x00096735
		public override bool HasAttributes
		{
			get
			{
				return this.AttributeCount != 0;
			}
		}

		// Token: 0x170007FB RID: 2043
		public override string this[int attributeIndex]
		{
			get
			{
				return this.GetAttribute(attributeIndex);
			}
		}

		// Token: 0x170007FC RID: 2044
		public override string this[string attributeName]
		{
			get
			{
				return this.GetAttribute(attributeName);
			}
		}

		// Token: 0x170007FD RID: 2045
		public override string this[string localName, string namespaceUri]
		{
			get
			{
				return this.GetAttribute(localName, namespaceUri);
			}
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x0009855C File Offset: 0x0009675C
		public override string GetAttribute(int attributeIndex)
		{
			throw new InvalidOperationException(SR.Get("ParserFilterXmlReaderNoIndexAttributeAccess"));
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x0009856D File Offset: 0x0009676D
		public override string GetAttribute(string attributeName)
		{
			if (attributeName == this.uidQualifiedName)
			{
				return null;
			}
			return base.GetAttribute(attributeName);
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x00098586 File Offset: 0x00096786
		public override string GetAttribute(string localName, string namespaceUri)
		{
			if (localName == "Uid" && namespaceUri == "http://schemas.microsoft.com/winfx/2006/xaml")
			{
				return null;
			}
			return base.GetAttribute(localName, namespaceUri);
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x0009855C File Offset: 0x0009675C
		public override void MoveToAttribute(int attributeIndex)
		{
			throw new InvalidOperationException(SR.Get("ParserFilterXmlReaderNoIndexAttributeAccess"));
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x000985AC File Offset: 0x000967AC
		public override bool MoveToAttribute(string attributeName)
		{
			return !(attributeName == this.uidQualifiedName) && base.MoveToAttribute(attributeName);
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x000985C5 File Offset: 0x000967C5
		public override bool MoveToAttribute(string localName, string namespaceUri)
		{
			return (!(localName == "Uid") || !(namespaceUri == "http://schemas.microsoft.com/winfx/2006/xaml")) && base.MoveToAttribute(localName, namespaceUri);
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x000985EC File Offset: 0x000967EC
		public override bool MoveToFirstAttribute()
		{
			bool previousSuccessValue = base.MoveToFirstAttribute();
			return this.CheckForUidOrNamespaceRedef(previousSuccessValue);
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x0009860C File Offset: 0x0009680C
		public override bool MoveToNextAttribute()
		{
			bool previousSuccessValue = base.MoveToNextAttribute();
			return this.CheckForUidOrNamespaceRedef(previousSuccessValue);
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x0009862C File Offset: 0x0009682C
		public override bool Read()
		{
			bool flag = base.Read();
			if (flag)
			{
				this.CheckForUidAttribute();
			}
			return flag;
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x0009864A File Offset: 0x0009684A
		internal FilteredXmlReader(string xmlFragment, XmlNodeType fragmentType, XmlParserContext context) : base(xmlFragment, fragmentType, context)
		{
			this.haveUid = false;
			this.uidPrefix = "def";
			this.uidQualifiedName = this.uidPrefix + ":Uid";
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x0009867D File Offset: 0x0009687D
		private void CheckForUidAttribute()
		{
			if (base.GetAttribute(this.uidQualifiedName) != null)
			{
				this.haveUid = true;
				return;
			}
			this.haveUid = false;
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x0009869C File Offset: 0x0009689C
		private bool CheckForUidOrNamespaceRedef(bool previousSuccessValue)
		{
			bool flag = previousSuccessValue;
			if (flag && base.LocalName == "Uid" && base.NamespaceURI == "http://schemas.microsoft.com/winfx/2006/xaml")
			{
				this.CheckForPrefixUpdate();
				flag = base.MoveToNextAttribute();
			}
			this.CheckForNamespaceRedef();
			return flag;
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x000986E6 File Offset: 0x000968E6
		private void CheckForPrefixUpdate()
		{
			if (base.Prefix != this.uidPrefix)
			{
				this.uidPrefix = base.Prefix;
				this.uidQualifiedName = this.uidPrefix + ":Uid";
				this.CheckForUidAttribute();
			}
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x00098724 File Offset: 0x00096924
		private void CheckForNamespaceRedef()
		{
			if (base.Prefix == "xmlns" && base.LocalName != this.uidPrefix && base.Value == "http://schemas.microsoft.com/winfx/2006/xaml")
			{
				throw new InvalidOperationException(SR.Get("ParserFilterXmlReaderNoDefinitionPrefixChangeAllowed"));
			}
		}

		// Token: 0x0400157A RID: 5498
		private const string uidLocalName = "Uid";

		// Token: 0x0400157B RID: 5499
		private const string uidNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";

		// Token: 0x0400157C RID: 5500
		private const string defaultPrefix = "def";

		// Token: 0x0400157D RID: 5501
		private string uidPrefix;

		// Token: 0x0400157E RID: 5502
		private string uidQualifiedName;

		// Token: 0x0400157F RID: 5503
		private bool haveUid;
	}
}
