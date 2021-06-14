using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x0200035B RID: 859
	internal class SpecialFolderEnumConverter : AlphaSortedEnumConverter
	{
		// Token: 0x0600354D RID: 13645 RVA: 0x000F3AB0 File Offset: 0x000F1CB0
		public SpecialFolderEnumConverter(Type type) : base(type)
		{
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x000F3ABC File Offset: 0x000F1CBC
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			TypeConverter.StandardValuesCollection standardValues = base.GetStandardValues(context);
			ArrayList arrayList = new ArrayList();
			int count = standardValues.Count;
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				if (standardValues[i] is Environment.SpecialFolder && standardValues[i].Equals(Environment.SpecialFolder.Personal))
				{
					if (!flag)
					{
						flag = true;
						arrayList.Add(standardValues[i]);
					}
				}
				else
				{
					arrayList.Add(standardValues[i]);
				}
			}
			return new TypeConverter.StandardValuesCollection(arrayList);
		}
	}
}
