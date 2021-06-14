using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x020002C1 RID: 705
	internal class ListViewSubItemConverter : ExpandableObjectConverter
	{
		// Token: 0x06002960 RID: 10592 RVA: 0x0001F8F0 File Offset: 0x0001DAF0
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x000C0A18 File Offset: 0x000BEC18
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is ListViewItem.ListViewSubItem)
			{
				ListViewItem.ListViewSubItem listViewSubItem = (ListViewItem.ListViewSubItem)value;
				ConstructorInfo constructor;
				if (listViewSubItem.CustomStyle)
				{
					constructor = typeof(ListViewItem.ListViewSubItem).GetConstructor(new Type[]
					{
						typeof(ListViewItem),
						typeof(string),
						typeof(Color),
						typeof(Color),
						typeof(Font)
					});
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, new object[]
						{
							null,
							listViewSubItem.Text,
							listViewSubItem.ForeColor,
							listViewSubItem.BackColor,
							listViewSubItem.Font
						}, true);
					}
				}
				constructor = typeof(ListViewItem.ListViewSubItem).GetConstructor(new Type[]
				{
					typeof(ListViewItem),
					typeof(string)
				});
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[]
					{
						null,
						listViewSubItem.Text
					}, true);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
