using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x020001FB RID: 507
	internal class DataGridViewRowConverter : ExpandableObjectConverter
	{
		// Token: 0x06001F20 RID: 7968 RVA: 0x0001F8F0 File Offset: 0x0001DAF0
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x0009CBEC File Offset: 0x0009ADEC
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			DataGridViewRow dataGridViewRow = value as DataGridViewRow;
			if (destinationType == typeof(InstanceDescriptor) && dataGridViewRow != null)
			{
				ConstructorInfo constructor = dataGridViewRow.GetType().GetConstructor(new Type[0]);
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[0], false);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
