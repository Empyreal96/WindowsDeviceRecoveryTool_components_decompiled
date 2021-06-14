using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x02000245 RID: 581
	internal class FlatButtonAppearanceConverter : ExpandableObjectConverter
	{
		// Token: 0x06002263 RID: 8803 RVA: 0x000A7674 File Offset: 0x000A5874
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return "";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x000A769C File Offset: 0x000A589C
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			if (context != null && context.Instance is Button)
			{
				Attribute[] array = new Attribute[attributes.Length + 1];
				attributes.CopyTo(array, 0);
				array[attributes.Length] = new ApplicableToButtonAttribute();
				attributes = array;
			}
			return TypeDescriptor.GetProperties(value, attributes);
		}
	}
}
