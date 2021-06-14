using System;
using System.Collections;
using System.Globalization;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x02000482 RID: 1154
	internal class AttributeTypeSorter : IComparer
	{
		// Token: 0x06004E19 RID: 19993 RVA: 0x001400FC File Offset: 0x0013E2FC
		private static string GetTypeIdString(Attribute a)
		{
			object typeId = a.TypeId;
			if (typeId == null)
			{
				return "";
			}
			string text;
			if (AttributeTypeSorter.typeIds == null)
			{
				AttributeTypeSorter.typeIds = new Hashtable();
				text = null;
			}
			else
			{
				text = (AttributeTypeSorter.typeIds[typeId] as string);
			}
			if (text == null)
			{
				text = typeId.ToString();
				AttributeTypeSorter.typeIds[typeId] = text;
			}
			return text;
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x00140158 File Offset: 0x0013E358
		public int Compare(object obj1, object obj2)
		{
			Attribute attribute = obj1 as Attribute;
			Attribute attribute2 = obj2 as Attribute;
			if (attribute == null && attribute2 == null)
			{
				return 0;
			}
			if (attribute == null)
			{
				return -1;
			}
			if (attribute2 == null)
			{
				return 1;
			}
			return string.Compare(AttributeTypeSorter.GetTypeIdString(attribute), AttributeTypeSorter.GetTypeIdString(attribute2), false, CultureInfo.InvariantCulture);
		}

		// Token: 0x0400332E RID: 13102
		private static IDictionary typeIds;
	}
}
