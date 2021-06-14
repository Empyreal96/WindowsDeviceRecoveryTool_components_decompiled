using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004AA RID: 1194
	internal class Com2IDispatchConverter : Com2ExtendedTypeConverter
	{
		// Token: 0x06005081 RID: 20609 RVA: 0x0014D500 File Offset: 0x0014B700
		public Com2IDispatchConverter(Com2PropertyDescriptor propDesc, bool allowExpand, TypeConverter baseConverter) : base(baseConverter)
		{
			this.propDesc = propDesc;
			this.allowExpand = allowExpand;
		}

		// Token: 0x06005082 RID: 20610 RVA: 0x0014D517 File Offset: 0x0014B717
		public Com2IDispatchConverter(Com2PropertyDescriptor propDesc, bool allowExpand) : base(propDesc.PropertyType)
		{
			this.propDesc = propDesc;
			this.allowExpand = allowExpand;
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return false;
		}

		// Token: 0x06005084 RID: 20612 RVA: 0x0014D533 File Offset: 0x0014B733
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string);
		}

		// Token: 0x06005085 RID: 20613 RVA: 0x0014D548 File Offset: 0x0014B748
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!(destinationType == typeof(string)))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			if (value == null)
			{
				return Com2IDispatchConverter.none;
			}
			string text = ComNativeDescriptor.Instance.GetName(value);
			if (text == null || text.Length == 0)
			{
				text = ComNativeDescriptor.Instance.GetClassName(value);
			}
			if (text == null)
			{
				return "(Object)";
			}
			return text;
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x0014D5AA File Offset: 0x0014B7AA
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(value, attributes);
		}

		// Token: 0x06005087 RID: 20615 RVA: 0x0014D5B3 File Offset: 0x0014B7B3
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return this.allowExpand;
		}

		// Token: 0x06005088 RID: 20616 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x04003415 RID: 13333
		private Com2PropertyDescriptor propDesc;

		// Token: 0x04003416 RID: 13334
		protected static readonly string none = SR.GetString("toStringNone");

		// Token: 0x04003417 RID: 13335
		private bool allowExpand;
	}
}
