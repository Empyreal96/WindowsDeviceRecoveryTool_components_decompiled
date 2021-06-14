using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200071C RID: 1820
	internal class SystemConvertConverter : IValueConverter
	{
		// Token: 0x060074E6 RID: 29926 RVA: 0x002171FF File Offset: 0x002153FF
		public SystemConvertConverter(Type sourceType, Type targetType)
		{
			this._sourceType = sourceType;
			this._targetType = targetType;
		}

		// Token: 0x060074E7 RID: 29927 RVA: 0x00217215 File Offset: 0x00215415
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			return System.Convert.ChangeType(o, this._targetType, culture);
		}

		// Token: 0x060074E8 RID: 29928 RVA: 0x00217228 File Offset: 0x00215428
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			object obj = DefaultValueConverter.TryParse(o, this._sourceType, culture);
			if (obj == DependencyProperty.UnsetValue)
			{
				return System.Convert.ChangeType(o, this._sourceType, culture);
			}
			return obj;
		}

		// Token: 0x060074E9 RID: 29929 RVA: 0x0021725C File Offset: 0x0021545C
		public static bool CanConvert(Type sourceType, Type targetType)
		{
			if (sourceType == typeof(DateTime))
			{
				return targetType == typeof(string);
			}
			if (targetType == typeof(DateTime))
			{
				return sourceType == typeof(string);
			}
			if (sourceType == typeof(char))
			{
				return SystemConvertConverter.CanConvertChar(targetType);
			}
			if (targetType == typeof(char))
			{
				return SystemConvertConverter.CanConvertChar(sourceType);
			}
			for (int i = 0; i < SystemConvertConverter.SupportedTypes.Length; i++)
			{
				if (sourceType == SystemConvertConverter.SupportedTypes[i])
				{
					for (i++; i < SystemConvertConverter.SupportedTypes.Length; i++)
					{
						if (targetType == SystemConvertConverter.SupportedTypes[i])
						{
							return true;
						}
					}
				}
				else if (targetType == SystemConvertConverter.SupportedTypes[i])
				{
					for (i++; i < SystemConvertConverter.SupportedTypes.Length; i++)
					{
						if (sourceType == SystemConvertConverter.SupportedTypes[i])
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060074EA RID: 29930 RVA: 0x00217360 File Offset: 0x00215560
		private static bool CanConvertChar(Type type)
		{
			for (int i = 0; i < SystemConvertConverter.CharSupportedTypes.Length; i++)
			{
				if (type == SystemConvertConverter.CharSupportedTypes[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04003806 RID: 14342
		private Type _sourceType;

		// Token: 0x04003807 RID: 14343
		private Type _targetType;

		// Token: 0x04003808 RID: 14344
		private static readonly Type[] SupportedTypes = new Type[]
		{
			typeof(string),
			typeof(int),
			typeof(long),
			typeof(float),
			typeof(double),
			typeof(decimal),
			typeof(bool),
			typeof(byte),
			typeof(short),
			typeof(uint),
			typeof(ulong),
			typeof(ushort),
			typeof(sbyte)
		};

		// Token: 0x04003809 RID: 14345
		private static readonly Type[] CharSupportedTypes = new Type[]
		{
			typeof(string),
			typeof(int),
			typeof(long),
			typeof(byte),
			typeof(short),
			typeof(uint),
			typeof(ulong),
			typeof(ushort),
			typeof(sbyte)
		};
	}
}
