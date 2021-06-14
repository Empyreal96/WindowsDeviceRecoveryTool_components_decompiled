using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Common
{
	// Token: 0x0200015A RID: 346
	internal class XmlElementValueCollection : IEnumerable<XmlElementValue>, IEnumerable
	{
		// Token: 0x060006C6 RID: 1734 RVA: 0x000116C6 File Offset: 0x0000F8C6
		private XmlElementValueCollection(IList<XmlElementValue> list, ILookup<string, XmlElementValue> nameMap)
		{
			this.values = list;
			this.nameLookup = nameMap;
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x000116DC File Offset: 0x0000F8DC
		internal XmlTextValue FirstText
		{
			get
			{
				return this.values.OfText().FirstOrDefault<XmlTextValue>() ?? XmlTextValue.Missing;
			}
		}

		// Token: 0x170002CE RID: 718
		internal XmlElementValue this[string elementName]
		{
			get
			{
				return this.EnsureLookup()[elementName].FirstOrDefault<XmlElementValue>() ?? XmlElementValueCollection.MissingXmlElementValue.Instance;
			}
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00011713 File Offset: 0x0000F913
		public IEnumerator<XmlElementValue> GetEnumerator()
		{
			return this.values.GetEnumerator();
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00011720 File Offset: 0x0000F920
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.values.GetEnumerator();
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001172D File Offset: 0x0000F92D
		internal static XmlElementValueCollection FromList(IList<XmlElementValue> values)
		{
			if (values == null || values.Count == 0)
			{
				return XmlElementValueCollection.empty;
			}
			return new XmlElementValueCollection(values, null);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00011747 File Offset: 0x0000F947
		internal IEnumerable<XmlElementValue> FindByName(string elementName)
		{
			return this.EnsureLookup()[elementName];
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00011755 File Offset: 0x0000F955
		internal IEnumerable<XmlElementValue<TResult>> FindByName<TResult>(string elementName) where TResult : class
		{
			return this.FindByName(elementName).OfResultType<TResult>();
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001176C File Offset: 0x0000F96C
		private ILookup<string, XmlElementValue> EnsureLookup()
		{
			ILookup<string, XmlElementValue> result;
			if ((result = this.nameLookup) == null)
			{
				result = (this.nameLookup = this.values.ToLookup((XmlElementValue value) => value.Name));
			}
			return result;
		}

		// Token: 0x04000386 RID: 902
		private static readonly XmlElementValueCollection empty = new XmlElementValueCollection(new XmlElementValue[0], new XmlElementValue[0].ToLookup((XmlElementValue value) => value.Name));

		// Token: 0x04000387 RID: 903
		private readonly IList<XmlElementValue> values;

		// Token: 0x04000388 RID: 904
		private ILookup<string, XmlElementValue> nameLookup;

		// Token: 0x0200015C RID: 348
		internal sealed class MissingXmlElementValue : XmlElementValue
		{
			// Token: 0x060006DC RID: 1756 RVA: 0x0001184B File Offset: 0x0000FA4B
			private MissingXmlElementValue() : base(null, null)
			{
			}

			// Token: 0x170002D5 RID: 725
			// (get) Token: 0x060006DD RID: 1757 RVA: 0x00011855 File Offset: 0x0000FA55
			internal override object UntypedValue
			{
				get
				{
					return null;
				}
			}

			// Token: 0x170002D6 RID: 726
			// (get) Token: 0x060006DE RID: 1758 RVA: 0x00011858 File Offset: 0x0000FA58
			internal override bool IsUsed
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0400038D RID: 909
			internal static readonly XmlElementValueCollection.MissingXmlElementValue Instance = new XmlElementValueCollection.MissingXmlElementValue();
		}
	}
}
