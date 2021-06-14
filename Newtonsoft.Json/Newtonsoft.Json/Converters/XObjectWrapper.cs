using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000031 RID: 49
	internal class XObjectWrapper : IXmlNode
	{
		// Token: 0x060001E4 RID: 484 RVA: 0x000082D4 File Offset: 0x000064D4
		public XObjectWrapper(XObject xmlObject)
		{
			this._xmlObject = xmlObject;
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x000082E3 File Offset: 0x000064E3
		public object WrappedNode
		{
			get
			{
				return this._xmlObject;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x000082EB File Offset: 0x000064EB
		public virtual XmlNodeType NodeType
		{
			get
			{
				return this._xmlObject.NodeType;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x000082F8 File Offset: 0x000064F8
		public virtual string LocalName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x000082FB File Offset: 0x000064FB
		public virtual IList<IXmlNode> ChildNodes
		{
			get
			{
				return new List<IXmlNode>();
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00008302 File Offset: 0x00006502
		public virtual IList<IXmlNode> Attributes
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00008305 File Offset: 0x00006505
		public virtual IXmlNode ParentNode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00008308 File Offset: 0x00006508
		// (set) Token: 0x060001EC RID: 492 RVA: 0x0000830B File Offset: 0x0000650B
		public virtual string Value
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00008312 File Offset: 0x00006512
		public virtual IXmlNode AppendChild(IXmlNode newChild)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001EE RID: 494 RVA: 0x00008319 File Offset: 0x00006519
		public virtual string NamespaceUri
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040000A4 RID: 164
		private readonly XObject _xmlObject;
	}
}
