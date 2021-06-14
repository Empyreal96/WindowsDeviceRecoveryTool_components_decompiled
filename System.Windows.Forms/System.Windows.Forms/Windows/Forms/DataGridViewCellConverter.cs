using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x02000197 RID: 407
	internal class DataGridViewCellConverter : ExpandableObjectConverter
	{
		// Token: 0x06001ADF RID: 6879 RVA: 0x0001F8F0 File Offset: 0x0001DAF0
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x00086C94 File Offset: 0x00084E94
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			DataGridViewCell dataGridViewCell = value as DataGridViewCell;
			if (destinationType == typeof(InstanceDescriptor) && dataGridViewCell != null)
			{
				ConstructorInfo constructor = dataGridViewCell.GetType().GetConstructor(new Type[0]);
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[0], false);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
