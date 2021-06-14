using System;
using System.ComponentModel;
using System.Globalization;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200033F RID: 831
	internal class DPTypeDescriptorContext : ITypeDescriptorContext, IServiceProvider
	{
		// Token: 0x06002C36 RID: 11318 RVA: 0x000C8DD4 File Offset: 0x000C6FD4
		private DPTypeDescriptorContext(DependencyProperty property, object propertyValue)
		{
			Invariant.Assert(property != null, "property == null");
			Invariant.Assert(propertyValue != null, "propertyValue == null");
			Invariant.Assert(property.IsValidValue(propertyValue), "propertyValue must be of suitable type for the given dependency property");
			this._property = property;
			this._propertyValue = propertyValue;
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x000C8E24 File Offset: 0x000C7024
		internal static string GetStringValue(DependencyProperty property, object propertyValue)
		{
			string text = null;
			if (property == UIElement.BitmapEffectProperty)
			{
				return null;
			}
			if (property == Inline.TextDecorationsProperty)
			{
				text = DPTypeDescriptorContext.TextDecorationsFixup((TextDecorationCollection)propertyValue);
			}
			else if (typeof(CultureInfo).IsAssignableFrom(property.PropertyType))
			{
				text = DPTypeDescriptorContext.CultureInfoFixup(property, (CultureInfo)propertyValue);
			}
			if (text == null)
			{
				DPTypeDescriptorContext context = new DPTypeDescriptorContext(property, propertyValue);
				TypeConverter converter = TypeDescriptor.GetConverter(property.PropertyType);
				Invariant.Assert(converter != null);
				if (converter.CanConvertTo(context, typeof(string)))
				{
					text = (string)converter.ConvertTo(context, CultureInfo.InvariantCulture, propertyValue, typeof(string));
				}
			}
			return text;
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x000C8EC8 File Offset: 0x000C70C8
		private static string TextDecorationsFixup(TextDecorationCollection textDecorations)
		{
			string result = null;
			if (TextDecorations.Underline.ValueEquals(textDecorations))
			{
				result = "Underline";
			}
			else if (TextDecorations.Strikethrough.ValueEquals(textDecorations))
			{
				result = "Strikethrough";
			}
			else if (TextDecorations.OverLine.ValueEquals(textDecorations))
			{
				result = "OverLine";
			}
			else if (TextDecorations.Baseline.ValueEquals(textDecorations))
			{
				result = "Baseline";
			}
			else if (textDecorations.Count == 0)
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06002C39 RID: 11321 RVA: 0x000C8F3C File Offset: 0x000C713C
		private static string CultureInfoFixup(DependencyProperty property, CultureInfo cultureInfo)
		{
			string result = null;
			DPTypeDescriptorContext context = new DPTypeDescriptorContext(property, cultureInfo);
			TypeConverter typeConverter = new CultureInfoIetfLanguageTagConverter();
			if (typeConverter.CanConvertTo(context, typeof(string)))
			{
				result = (string)typeConverter.ConvertTo(context, CultureInfo.InvariantCulture, cultureInfo, typeof(string));
			}
			return result;
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06002C3A RID: 11322 RVA: 0x0000C238 File Offset: 0x0000A438
		IContainer ITypeDescriptorContext.Container
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06002C3B RID: 11323 RVA: 0x000C8F8A File Offset: 0x000C718A
		object ITypeDescriptorContext.Instance
		{
			get
			{
				return this._propertyValue;
			}
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x00002137 File Offset: 0x00000337
		void ITypeDescriptorContext.OnComponentChanged()
		{
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ITypeDescriptorContext.OnComponentChanging()
		{
			return false;
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06002C3E RID: 11326 RVA: 0x0000C238 File Offset: 0x0000A438
		PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x0000C238 File Offset: 0x0000A438
		object IServiceProvider.GetService(Type serviceType)
		{
			return null;
		}

		// Token: 0x04001CCB RID: 7371
		private DependencyProperty _property;

		// Token: 0x04001CCC RID: 7372
		private object _propertyValue;
	}
}
