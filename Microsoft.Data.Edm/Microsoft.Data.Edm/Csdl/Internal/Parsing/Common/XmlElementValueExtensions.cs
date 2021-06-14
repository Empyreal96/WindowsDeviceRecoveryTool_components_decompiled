using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Common
{
	// Token: 0x02000157 RID: 343
	internal static class XmlElementValueExtensions
	{
		// Token: 0x060006B8 RID: 1720 RVA: 0x000113F0 File Offset: 0x0000F5F0
		internal static IEnumerable<XmlElementValue<T>> OfResultType<T>(this IEnumerable<XmlElementValue> elements) where T : class
		{
			foreach (XmlElementValue element in elements)
			{
				XmlElementValue<T> result = element as XmlElementValue<T>;
				if (result != null)
				{
					yield return result;
				}
				else if (element.UntypedValue is T)
				{
					yield return new XmlElementValue<T>(element.Name, element.Location, element.ValueAs<T>());
				}
			}
			yield break;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00011415 File Offset: 0x0000F615
		internal static IEnumerable<T> ValuesOfType<T>(this IEnumerable<XmlElementValue> elements) where T : class
		{
			return from ev in elements.OfResultType<T>()
			select ev.Value;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x000115D0 File Offset: 0x0000F7D0
		internal static IEnumerable<XmlTextValue> OfText(this IEnumerable<XmlElementValue> elements)
		{
			foreach (XmlElementValue element in elements)
			{
				if (element.IsText)
				{
					yield return (XmlTextValue)element;
				}
			}
			yield break;
		}
	}
}
