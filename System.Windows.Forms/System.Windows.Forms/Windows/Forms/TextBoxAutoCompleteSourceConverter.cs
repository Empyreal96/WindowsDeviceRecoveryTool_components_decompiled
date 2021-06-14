using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000390 RID: 912
	internal class TextBoxAutoCompleteSourceConverter : EnumConverter
	{
		// Token: 0x06003977 RID: 14711 RVA: 0x0001011F File Offset: 0x0000E31F
		public TextBoxAutoCompleteSourceConverter(Type type) : base(type)
		{
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x000FFB00 File Offset: 0x000FDD00
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			TypeConverter.StandardValuesCollection standardValues = base.GetStandardValues(context);
			ArrayList arrayList = new ArrayList();
			int count = standardValues.Count;
			for (int i = 0; i < count; i++)
			{
				string text = standardValues[i].ToString();
				if (!text.Equals("ListItems"))
				{
					arrayList.Add(standardValues[i]);
				}
			}
			return new TypeConverter.StandardValuesCollection(arrayList);
		}
	}
}
