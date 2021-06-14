using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Common
{
	// Token: 0x02000158 RID: 344
	internal abstract class XmlElementParser
	{
		// Token: 0x060006BC RID: 1724 RVA: 0x000115ED File Offset: 0x0000F7ED
		protected XmlElementParser(string elementName, Dictionary<string, XmlElementParser> children)
		{
			this.ElementName = elementName;
			this.childParsers = children;
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x00011603 File Offset: 0x0000F803
		// (set) Token: 0x060006BE RID: 1726 RVA: 0x0001160B File Offset: 0x0000F80B
		internal string ElementName { get; private set; }

		// Token: 0x060006BF RID: 1727 RVA: 0x00011614 File Offset: 0x0000F814
		public void AddChildParser(XmlElementParser child)
		{
			this.childParsers[child.ElementName] = child;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00011630 File Offset: 0x0000F830
		internal static XmlElementParser<TResult> Create<TResult>(string elementName, Func<XmlElementInfo, XmlElementValueCollection, TResult> parserFunc, IEnumerable<XmlElementParser> childParsers, IEnumerable<XmlElementParser> descendantParsers)
		{
			Func<XmlElementParser, string> func = null;
			Dictionary<string, XmlElementParser> children = null;
			if (childParsers != null)
			{
				if (func == null)
				{
					func = ((XmlElementParser p) => p.ElementName);
				}
				children = childParsers.ToDictionary(func);
			}
			return new XmlElementParser<TResult>(elementName, children, parserFunc);
		}

		// Token: 0x060006C1 RID: 1729
		internal abstract XmlElementValue Parse(XmlElementInfo element, IList<XmlElementValue> children);

		// Token: 0x060006C2 RID: 1730 RVA: 0x00011664 File Offset: 0x0000F864
		internal bool TryGetChildElementParser(string elementName, out XmlElementParser elementParser)
		{
			elementParser = null;
			return this.childParsers != null && this.childParsers.TryGetValue(elementName, out elementParser);
		}

		// Token: 0x04000383 RID: 899
		private readonly Dictionary<string, XmlElementParser> childParsers;
	}
}
