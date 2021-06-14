using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004A7 RID: 1191
	internal class Com2ExtendedTypeConverter : TypeConverter
	{
		// Token: 0x06005067 RID: 20583 RVA: 0x0014D01B File Offset: 0x0014B21B
		public Com2ExtendedTypeConverter(TypeConverter innerConverter)
		{
			this.innerConverter = innerConverter;
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x0014D02A File Offset: 0x0014B22A
		public Com2ExtendedTypeConverter(Type baseType)
		{
			this.innerConverter = TypeDescriptor.GetConverter(baseType);
		}

		// Token: 0x170013DA RID: 5082
		// (get) Token: 0x06005069 RID: 20585 RVA: 0x0014D03E File Offset: 0x0014B23E
		public TypeConverter InnerConverter
		{
			get
			{
				return this.innerConverter;
			}
		}

		// Token: 0x0600506A RID: 20586 RVA: 0x0014D048 File Offset: 0x0014B248
		public TypeConverter GetWrappedConverter(Type t)
		{
			for (TypeConverter typeConverter = this.innerConverter; typeConverter != null; typeConverter = ((Com2ExtendedTypeConverter)typeConverter).InnerConverter)
			{
				if (t.IsInstanceOfType(typeConverter))
				{
					return typeConverter;
				}
				if (!(typeConverter is Com2ExtendedTypeConverter))
				{
					break;
				}
			}
			return null;
		}

		// Token: 0x0600506B RID: 20587 RVA: 0x0014D081 File Offset: 0x0014B281
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.CanConvertFrom(context, sourceType);
			}
			return base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x0600506C RID: 20588 RVA: 0x0014D0A1 File Offset: 0x0014B2A1
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.CanConvertTo(context, destinationType);
			}
			return base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600506D RID: 20589 RVA: 0x0014D0C1 File Offset: 0x0014B2C1
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.ConvertFrom(context, culture, value);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x0600506E RID: 20590 RVA: 0x0014D0E3 File Offset: 0x0014B2E3
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.ConvertTo(context, culture, value, destinationType);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x0600506F RID: 20591 RVA: 0x0014D109 File Offset: 0x0014B309
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.CreateInstance(context, propertyValues);
			}
			return base.CreateInstance(context, propertyValues);
		}

		// Token: 0x06005070 RID: 20592 RVA: 0x0014D129 File Offset: 0x0014B329
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.GetCreateInstanceSupported(context);
			}
			return base.GetCreateInstanceSupported(context);
		}

		// Token: 0x06005071 RID: 20593 RVA: 0x0014D147 File Offset: 0x0014B347
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.GetProperties(context, value, attributes);
			}
			return base.GetProperties(context, value, attributes);
		}

		// Token: 0x06005072 RID: 20594 RVA: 0x0014D169 File Offset: 0x0014B369
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.GetPropertiesSupported(context);
			}
			return base.GetPropertiesSupported(context);
		}

		// Token: 0x06005073 RID: 20595 RVA: 0x0014D187 File Offset: 0x0014B387
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.GetStandardValues(context);
			}
			return base.GetStandardValues(context);
		}

		// Token: 0x06005074 RID: 20596 RVA: 0x0014D1A5 File Offset: 0x0014B3A5
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.GetStandardValuesExclusive(context);
			}
			return base.GetStandardValuesExclusive(context);
		}

		// Token: 0x06005075 RID: 20597 RVA: 0x0014D1C3 File Offset: 0x0014B3C3
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.GetStandardValuesSupported(context);
			}
			return base.GetStandardValuesSupported(context);
		}

		// Token: 0x06005076 RID: 20598 RVA: 0x0014D1E1 File Offset: 0x0014B3E1
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			if (this.innerConverter != null)
			{
				return this.innerConverter.IsValid(context, value);
			}
			return base.IsValid(context, value);
		}

		// Token: 0x04003412 RID: 13330
		private TypeConverter innerConverter;
	}
}
