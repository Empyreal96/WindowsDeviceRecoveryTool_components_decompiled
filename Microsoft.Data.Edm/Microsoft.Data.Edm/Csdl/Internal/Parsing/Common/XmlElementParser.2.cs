using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Common
{
	// Token: 0x02000159 RID: 345
	internal class XmlElementParser<TResult> : XmlElementParser
	{
		// Token: 0x060006C4 RID: 1732 RVA: 0x00011680 File Offset: 0x0000F880
		internal XmlElementParser(string elementName, Dictionary<string, XmlElementParser> children, Func<XmlElementInfo, XmlElementValueCollection, TResult> parser) : base(elementName, children)
		{
			this.parserFunc = parser;
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00011694 File Offset: 0x0000F894
		internal override XmlElementValue Parse(XmlElementInfo element, IList<XmlElementValue> children)
		{
			TResult newValue = this.parserFunc(element, XmlElementValueCollection.FromList(children));
			return new XmlElementValue<TResult>(element.Name, element.Location, newValue);
		}

		// Token: 0x04000385 RID: 901
		private readonly Func<XmlElementInfo, XmlElementValueCollection, TResult> parserFunc;
	}
}
