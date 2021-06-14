using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000034 RID: 52
	internal class XContainerWrapper : XObjectWrapper
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001FE RID: 510 RVA: 0x000083CF File Offset: 0x000065CF
		private XContainer Container
		{
			get
			{
				return (XContainer)base.WrappedNode;
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x000083DC File Offset: 0x000065DC
		public XContainerWrapper(XContainer container) : base(container)
		{
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000200 RID: 512 RVA: 0x000083E5 File Offset: 0x000065E5
		public override IList<IXmlNode> ChildNodes
		{
			get
			{
				if (this._childNodes == null)
				{
					this._childNodes = this.Container.Nodes().Select(new Func<XNode, IXmlNode>(XContainerWrapper.WrapNode)).ToList<IXmlNode>();
				}
				return this._childNodes;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0000841C File Offset: 0x0000661C
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Container.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Container.Parent);
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00008440 File Offset: 0x00006640
		internal static IXmlNode WrapNode(XObject node)
		{
			if (node is XDocument)
			{
				return new XDocumentWrapper((XDocument)node);
			}
			if (node is XElement)
			{
				return new XElementWrapper((XElement)node);
			}
			if (node is XContainer)
			{
				return new XContainerWrapper((XContainer)node);
			}
			if (node is XProcessingInstruction)
			{
				return new XProcessingInstructionWrapper((XProcessingInstruction)node);
			}
			if (node is XText)
			{
				return new XTextWrapper((XText)node);
			}
			if (node is XComment)
			{
				return new XCommentWrapper((XComment)node);
			}
			if (node is XAttribute)
			{
				return new XAttributeWrapper((XAttribute)node);
			}
			if (node is XDocumentType)
			{
				return new XDocumentTypeWrapper((XDocumentType)node);
			}
			return new XObjectWrapper(node);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x000084F3 File Offset: 0x000066F3
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			this.Container.Add(newChild.WrappedNode);
			this._childNodes = null;
			return newChild;
		}

		// Token: 0x040000A7 RID: 167
		private IList<IXmlNode> _childNodes;
	}
}
