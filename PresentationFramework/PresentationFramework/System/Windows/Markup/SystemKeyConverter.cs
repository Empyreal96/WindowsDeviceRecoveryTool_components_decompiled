using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Markup
{
	// Token: 0x02000277 RID: 631
	internal class SystemKeyConverter : TypeConverter
	{
		// Token: 0x06002401 RID: 9217 RVA: 0x000AF980 File Offset: 0x000ADB80
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == null)
			{
				throw new ArgumentNullException("sourceType");
			}
			return base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x000AF99E File Offset: 0x000ADB9E
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			return (destinationType == typeof(MarkupExtension) && context is IValueSerializerContext) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x000AF9D8 File Offset: 0x000ADBD8
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x000AF9E4 File Offset: 0x000ADBE4
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(MarkupExtension) && this.CanConvertTo(context, destinationType))
			{
				SystemResourceKeyID internalKey;
				if (value is SystemResourceKey)
				{
					internalKey = (value as SystemResourceKey).InternalKey;
				}
				else
				{
					if (!(value is SystemThemeKey))
					{
						throw new ArgumentException(SR.Get("MustBeOfType", new object[]
						{
							"value",
							"SystemResourceKey or SystemThemeKey"
						}));
					}
					internalKey = (value as SystemThemeKey).InternalKey;
				}
				Type systemClassType = SystemKeyConverter.GetSystemClassType(internalKey);
				IValueSerializerContext valueSerializerContext = context as IValueSerializerContext;
				if (valueSerializerContext != null)
				{
					ValueSerializer valueSerializerFor = valueSerializerContext.GetValueSerializerFor(typeof(Type));
					if (valueSerializerFor != null)
					{
						string str = valueSerializerFor.ConvertToString(systemClassType, valueSerializerContext);
						return new StaticExtension(str + "." + SystemKeyConverter.GetSystemKeyName(internalKey));
					}
				}
			}
			return base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x000AFAD4 File Offset: 0x000ADCD4
		internal static Type GetSystemClassType(SystemResourceKeyID id)
		{
			if ((SystemResourceKeyID.InternalSystemColorsStart < id && id < SystemResourceKeyID.InternalSystemColorsEnd) || (SystemResourceKeyID.InternalSystemColorsExtendedStart < id && id < SystemResourceKeyID.InternalSystemColorsExtendedEnd))
			{
				return typeof(SystemColors);
			}
			if (SystemResourceKeyID.InternalSystemFontsStart < id && id < SystemResourceKeyID.InternalSystemFontsEnd)
			{
				return typeof(SystemFonts);
			}
			if (SystemResourceKeyID.InternalSystemParametersStart < id && id < SystemResourceKeyID.InternalSystemParametersEnd)
			{
				return typeof(SystemParameters);
			}
			if (SystemResourceKeyID.MenuItemSeparatorStyle == id)
			{
				return typeof(MenuItem);
			}
			if (SystemResourceKeyID.ToolBarButtonStyle <= id && id <= SystemResourceKeyID.ToolBarMenuStyle)
			{
				return typeof(ToolBar);
			}
			if (SystemResourceKeyID.StatusBarSeparatorStyle == id)
			{
				return typeof(StatusBar);
			}
			if (SystemResourceKeyID.GridViewScrollViewerStyle <= id && id <= SystemResourceKeyID.GridViewItemContainerStyle)
			{
				return typeof(GridView);
			}
			return null;
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x000AFB90 File Offset: 0x000ADD90
		internal static string GetSystemClassName(SystemResourceKeyID id)
		{
			if ((SystemResourceKeyID.InternalSystemColorsStart < id && id < SystemResourceKeyID.InternalSystemColorsEnd) || (SystemResourceKeyID.InternalSystemColorsExtendedStart < id && id < SystemResourceKeyID.InternalSystemColorsExtendedEnd))
			{
				return "SystemColors";
			}
			if (SystemResourceKeyID.InternalSystemFontsStart < id && id < SystemResourceKeyID.InternalSystemFontsEnd)
			{
				return "SystemFonts";
			}
			if (SystemResourceKeyID.InternalSystemParametersStart < id && id < SystemResourceKeyID.InternalSystemParametersEnd)
			{
				return "SystemParameters";
			}
			if (SystemResourceKeyID.MenuItemSeparatorStyle == id)
			{
				return "MenuItem";
			}
			if (SystemResourceKeyID.ToolBarButtonStyle <= id && id <= SystemResourceKeyID.ToolBarMenuStyle)
			{
				return "ToolBar";
			}
			if (SystemResourceKeyID.StatusBarSeparatorStyle == id)
			{
				return "StatusBar";
			}
			if (SystemResourceKeyID.GridViewScrollViewerStyle <= id && id <= SystemResourceKeyID.GridViewItemContainerStyle)
			{
				return "GridView";
			}
			return string.Empty;
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x000AFC2C File Offset: 0x000ADE2C
		internal static string GetSystemKeyName(SystemResourceKeyID id)
		{
			if ((SystemResourceKeyID.InternalSystemColorsStart < id && id < SystemResourceKeyID.InternalSystemParametersEnd) || (SystemResourceKeyID.InternalSystemColorsExtendedStart < id && id < SystemResourceKeyID.InternalSystemColorsExtendedEnd) || (SystemResourceKeyID.GridViewScrollViewerStyle <= id && id <= SystemResourceKeyID.GridViewItemContainerStyle))
			{
				return Enum.GetName(typeof(SystemResourceKeyID), id) + "Key";
			}
			if (SystemResourceKeyID.MenuItemSeparatorStyle == id || SystemResourceKeyID.StatusBarSeparatorStyle == id)
			{
				return "SeparatorStyleKey";
			}
			if (SystemResourceKeyID.ToolBarButtonStyle <= id && id <= SystemResourceKeyID.ToolBarMenuStyle)
			{
				string text = Enum.GetName(typeof(SystemResourceKeyID), id) + "Key";
				return text.Remove(0, 7);
			}
			return string.Empty;
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x000AFCD9 File Offset: 0x000ADED9
		internal static string GetSystemPropertyName(SystemResourceKeyID id)
		{
			if (SystemResourceKeyID.InternalSystemColorsStart < id && id < SystemResourceKeyID.InternalSystemColorsExtendedEnd)
			{
				return Enum.GetName(typeof(SystemResourceKeyID), id);
			}
			return string.Empty;
		}
	}
}
