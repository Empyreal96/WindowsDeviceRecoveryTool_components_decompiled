using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x02000380 RID: 896
	internal class TableLayoutPanelCellPositionTypeConverter : TypeConverter
	{
		// Token: 0x06003883 RID: 14467 RVA: 0x0001F8F0 File Offset: 0x0001DAF0
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x000B9F74 File Offset: 0x000B8174
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x000FD8A0 File Offset: 0x000FBAA0
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (!(value is string))
			{
				return base.ConvertFrom(context, culture, value);
			}
			string text = ((string)value).Trim();
			if (text.Length == 0)
			{
				return null;
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			char c = culture.TextInfo.ListSeparator[0];
			string[] array = text.Split(new char[]
			{
				c
			});
			int[] array2 = new int[array.Length];
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = (int)converter.ConvertFromString(context, culture, array[i]);
			}
			if (array2.Length == 2)
			{
				return new TableLayoutPanelCellPosition(array2[0], array2[1]);
			}
			throw new ArgumentException(SR.GetString("TextParseFailedFormat", new object[]
			{
				text,
				"column, row"
			}));
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x000FD980 File Offset: 0x000FBB80
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is TableLayoutPanelCellPosition)
			{
				TableLayoutPanelCellPosition tableLayoutPanelCellPosition = (TableLayoutPanelCellPosition)value;
				return new InstanceDescriptor(typeof(TableLayoutPanelCellPosition).GetConstructor(new Type[]
				{
					typeof(int),
					typeof(int)
				}), new object[]
				{
					tableLayoutPanelCellPosition.Column,
					tableLayoutPanelCellPosition.Row
				});
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x000FDA28 File Offset: 0x000FBC28
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			return new TableLayoutPanelCellPosition((int)propertyValues["Column"], (int)propertyValues["Row"]);
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x000FDA54 File Offset: 0x000FBC54
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(TableLayoutPanelCellPosition), attributes);
			return properties.Sort(new string[]
			{
				"Column",
				"Row"
			});
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
