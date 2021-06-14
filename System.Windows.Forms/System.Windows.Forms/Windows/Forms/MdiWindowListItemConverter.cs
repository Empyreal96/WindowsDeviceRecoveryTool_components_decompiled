using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x020002DF RID: 735
	internal class MdiWindowListItemConverter : ComponentConverter
	{
		// Token: 0x06002C3C RID: 11324 RVA: 0x000CE897 File Offset: 0x000CCA97
		public MdiWindowListItemConverter(Type type) : base(type)
		{
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x000CE8A0 File Offset: 0x000CCAA0
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			MenuStrip menuStrip = context.Instance as MenuStrip;
			if (menuStrip != null)
			{
				TypeConverter.StandardValuesCollection standardValues = base.GetStandardValues(context);
				ArrayList arrayList = new ArrayList();
				int count = standardValues.Count;
				for (int i = 0; i < count; i++)
				{
					ToolStripItem toolStripItem = standardValues[i] as ToolStripItem;
					if (toolStripItem != null && toolStripItem.Owner == menuStrip)
					{
						arrayList.Add(toolStripItem);
					}
				}
				return new TypeConverter.StandardValuesCollection(arrayList);
			}
			return base.GetStandardValues(context);
		}
	}
}
