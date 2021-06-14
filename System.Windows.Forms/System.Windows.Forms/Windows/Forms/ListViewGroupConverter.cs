using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x020002C7 RID: 711
	internal class ListViewGroupConverter : TypeConverter
	{
		// Token: 0x06002AB9 RID: 10937 RVA: 0x000C8E9F File Offset: 0x000C709F
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return (sourceType == typeof(string) && context != null && context.Instance is ListViewItem) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x000C8ED0 File Offset: 0x000C70D0
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || (destinationType == typeof(string) && context != null && context.Instance is ListViewItem) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x000C8F20 File Offset: 0x000C7120
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string b = ((string)value).Trim();
				if (context != null && context.Instance != null)
				{
					ListViewItem listViewItem = context.Instance as ListViewItem;
					if (listViewItem != null && listViewItem.ListView != null)
					{
						foreach (object obj in listViewItem.ListView.Groups)
						{
							ListViewGroup listViewGroup = (ListViewGroup)obj;
							if (listViewGroup.Header == b)
							{
								return listViewGroup;
							}
						}
					}
				}
			}
			if (value == null || value.Equals(SR.GetString("toStringNone")))
			{
				return null;
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x000C8FE8 File Offset: 0x000C71E8
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is ListViewGroup)
			{
				ListViewGroup listViewGroup = (ListViewGroup)value;
				ConstructorInfo constructor = typeof(ListViewGroup).GetConstructor(new Type[]
				{
					typeof(string),
					typeof(HorizontalAlignment)
				});
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[]
					{
						listViewGroup.Header,
						listViewGroup.HeaderAlignment
					}, false);
				}
			}
			if (destinationType == typeof(string) && value == null)
			{
				return SR.GetString("toStringNone");
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x000C90B8 File Offset: 0x000C72B8
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (context != null && context.Instance != null)
			{
				ListViewItem listViewItem = context.Instance as ListViewItem;
				if (listViewItem != null && listViewItem.ListView != null)
				{
					ArrayList arrayList = new ArrayList();
					foreach (object obj in listViewItem.ListView.Groups)
					{
						ListViewGroup value = (ListViewGroup)obj;
						arrayList.Add(value);
					}
					arrayList.Add(null);
					return new TypeConverter.StandardValuesCollection(arrayList);
				}
			}
			return null;
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
