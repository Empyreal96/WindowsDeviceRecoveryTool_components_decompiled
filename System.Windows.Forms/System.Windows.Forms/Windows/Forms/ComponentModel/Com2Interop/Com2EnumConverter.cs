using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004A5 RID: 1189
	internal class Com2EnumConverter : TypeConverter
	{
		// Token: 0x06005059 RID: 20569 RVA: 0x0014CEDC File Offset: 0x0014B0DC
		public Com2EnumConverter(Com2Enum enumObj)
		{
			this.com2Enum = enumObj;
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x000B9F74 File Offset: 0x000B8174
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x0014CEEB File Offset: 0x0014B0EB
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
		{
			return base.CanConvertTo(context, destType) || destType.IsEnum;
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x0014CEFF File Offset: 0x0014B0FF
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				return this.com2Enum.FromString((string)value);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x0014CF24 File Offset: 0x0014B124
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value != null)
			{
				string text = this.com2Enum.ToString(value);
				if (text != null)
				{
					return text;
				}
				return "";
			}
			else
			{
				if (destinationType.IsEnum)
				{
					return Enum.ToObject(destinationType, value);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x0014CF94 File Offset: 0x0014B194
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this.values == null)
			{
				object[] array = this.com2Enum.Values;
				if (array != null)
				{
					this.values = new TypeConverter.StandardValuesCollection(array);
				}
			}
			return this.values;
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x0014CFCA File Offset: 0x0014B1CA
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return this.com2Enum.IsStrictEnum;
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x0014CFD8 File Offset: 0x0014B1D8
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			string text = this.com2Enum.ToString(value);
			return text != null && text.Length > 0;
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x0014D000 File Offset: 0x0014B200
		public void RefreshValues()
		{
			this.values = null;
		}

		// Token: 0x04003410 RID: 13328
		internal readonly Com2Enum com2Enum;

		// Token: 0x04003411 RID: 13329
		private TypeConverter.StandardValuesCollection values;
	}
}
