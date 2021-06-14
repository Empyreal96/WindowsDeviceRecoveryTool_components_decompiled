using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x0200024D RID: 589
	internal class Formatter
	{
		// Token: 0x06002419 RID: 9241 RVA: 0x000B0140 File Offset: 0x000AE340
		public static object FormatObject(object value, Type targetType, TypeConverter sourceConverter, TypeConverter targetConverter, string formatString, IFormatProvider formatInfo, object formattedNullValue, object dataSourceNullValue)
		{
			if (Formatter.IsNullData(value, dataSourceNullValue))
			{
				value = DBNull.Value;
			}
			Type type = targetType;
			targetType = Formatter.NullableUnwrap(targetType);
			sourceConverter = Formatter.NullableUnwrap(sourceConverter);
			targetConverter = Formatter.NullableUnwrap(targetConverter);
			bool flag = targetType != type;
			object obj = Formatter.FormatObjectInternal(value, targetType, sourceConverter, targetConverter, formatString, formatInfo, formattedNullValue);
			if (type.IsValueType && obj == null && !flag)
			{
				throw new FormatException(Formatter.GetCantConvertMessage(value, targetType));
			}
			return obj;
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x000B01AC File Offset: 0x000AE3AC
		private static object FormatObjectInternal(object value, Type targetType, TypeConverter sourceConverter, TypeConverter targetConverter, string formatString, IFormatProvider formatInfo, object formattedNullValue)
		{
			if (value == DBNull.Value || value == null)
			{
				if (formattedNullValue != null)
				{
					return formattedNullValue;
				}
				if (targetType == Formatter.stringType)
				{
					return string.Empty;
				}
				if (targetType == Formatter.checkStateType)
				{
					return CheckState.Indeterminate;
				}
				return null;
			}
			else
			{
				if (targetType == Formatter.stringType && value is IFormattable && !string.IsNullOrEmpty(formatString))
				{
					return (value as IFormattable).ToString(formatString, formatInfo);
				}
				Type type = value.GetType();
				TypeConverter converter = TypeDescriptor.GetConverter(type);
				if (sourceConverter != null && sourceConverter != converter && sourceConverter.CanConvertTo(targetType))
				{
					return sourceConverter.ConvertTo(null, Formatter.GetFormatterCulture(formatInfo), value, targetType);
				}
				TypeConverter converter2 = TypeDescriptor.GetConverter(targetType);
				if (targetConverter != null && targetConverter != converter2 && targetConverter.CanConvertFrom(type))
				{
					return targetConverter.ConvertFrom(null, Formatter.GetFormatterCulture(formatInfo), value);
				}
				if (targetType == Formatter.checkStateType)
				{
					if (type == Formatter.booleanType)
					{
						return ((bool)value) ? CheckState.Checked : CheckState.Unchecked;
					}
					if (sourceConverter == null)
					{
						sourceConverter = converter;
					}
					if (sourceConverter != null && sourceConverter.CanConvertTo(Formatter.booleanType))
					{
						return ((bool)sourceConverter.ConvertTo(null, Formatter.GetFormatterCulture(formatInfo), value, Formatter.booleanType)) ? CheckState.Checked : CheckState.Unchecked;
					}
				}
				if (targetType.IsAssignableFrom(type))
				{
					return value;
				}
				if (sourceConverter == null)
				{
					sourceConverter = converter;
				}
				if (targetConverter == null)
				{
					targetConverter = converter2;
				}
				if (sourceConverter != null && sourceConverter.CanConvertTo(targetType))
				{
					return sourceConverter.ConvertTo(null, Formatter.GetFormatterCulture(formatInfo), value, targetType);
				}
				if (targetConverter != null && targetConverter.CanConvertFrom(type))
				{
					return targetConverter.ConvertFrom(null, Formatter.GetFormatterCulture(formatInfo), value);
				}
				if (value is IConvertible)
				{
					return Formatter.ChangeType(value, targetType, formatInfo);
				}
				throw new FormatException(Formatter.GetCantConvertMessage(value, targetType));
			}
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x000B0350 File Offset: 0x000AE550
		public static object ParseObject(object value, Type targetType, Type sourceType, TypeConverter targetConverter, TypeConverter sourceConverter, IFormatProvider formatInfo, object formattedNullValue, object dataSourceNullValue)
		{
			Type type = targetType;
			sourceType = Formatter.NullableUnwrap(sourceType);
			targetType = Formatter.NullableUnwrap(targetType);
			sourceConverter = Formatter.NullableUnwrap(sourceConverter);
			targetConverter = Formatter.NullableUnwrap(targetConverter);
			bool flag = targetType != type;
			object obj = Formatter.ParseObjectInternal(value, targetType, sourceType, targetConverter, sourceConverter, formatInfo, formattedNullValue);
			if (obj == DBNull.Value)
			{
				return Formatter.NullData(type, dataSourceNullValue);
			}
			return obj;
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x000B03AC File Offset: 0x000AE5AC
		private static object ParseObjectInternal(object value, Type targetType, Type sourceType, TypeConverter targetConverter, TypeConverter sourceConverter, IFormatProvider formatInfo, object formattedNullValue)
		{
			if (Formatter.EqualsFormattedNullValue(value, formattedNullValue, formatInfo) || value == DBNull.Value)
			{
				return DBNull.Value;
			}
			TypeConverter converter = TypeDescriptor.GetConverter(targetType);
			if (targetConverter != null && converter != targetConverter && targetConverter.CanConvertFrom(sourceType))
			{
				return targetConverter.ConvertFrom(null, Formatter.GetFormatterCulture(formatInfo), value);
			}
			TypeConverter converter2 = TypeDescriptor.GetConverter(sourceType);
			if (sourceConverter != null && converter2 != sourceConverter && sourceConverter.CanConvertTo(targetType))
			{
				return sourceConverter.ConvertTo(null, Formatter.GetFormatterCulture(formatInfo), value, targetType);
			}
			if (value is string)
			{
				object obj = Formatter.InvokeStringParseMethod(value, targetType, formatInfo);
				if (obj != Formatter.parseMethodNotFound)
				{
					return obj;
				}
			}
			else if (value is CheckState)
			{
				CheckState checkState = (CheckState)value;
				if (checkState == CheckState.Indeterminate)
				{
					return DBNull.Value;
				}
				if (targetType == Formatter.booleanType)
				{
					return checkState == CheckState.Checked;
				}
				if (targetConverter == null)
				{
					targetConverter = converter;
				}
				if (targetConverter != null && targetConverter.CanConvertFrom(Formatter.booleanType))
				{
					return targetConverter.ConvertFrom(null, Formatter.GetFormatterCulture(formatInfo), checkState == CheckState.Checked);
				}
			}
			else if (value != null && targetType.IsAssignableFrom(value.GetType()))
			{
				return value;
			}
			if (targetConverter == null)
			{
				targetConverter = converter;
			}
			if (sourceConverter == null)
			{
				sourceConverter = converter2;
			}
			if (targetConverter != null && targetConverter.CanConvertFrom(sourceType))
			{
				return targetConverter.ConvertFrom(null, Formatter.GetFormatterCulture(formatInfo), value);
			}
			if (sourceConverter != null && sourceConverter.CanConvertTo(targetType))
			{
				return sourceConverter.ConvertTo(null, Formatter.GetFormatterCulture(formatInfo), value, targetType);
			}
			if (value is IConvertible)
			{
				return Formatter.ChangeType(value, targetType, formatInfo);
			}
			throw new FormatException(Formatter.GetCantConvertMessage(value, targetType));
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x000B051C File Offset: 0x000AE71C
		private static object ChangeType(object value, Type type, IFormatProvider formatInfo)
		{
			object result;
			try
			{
				if (formatInfo == null)
				{
					formatInfo = CultureInfo.CurrentCulture;
				}
				result = Convert.ChangeType(value, type, formatInfo);
			}
			catch (InvalidCastException ex)
			{
				throw new FormatException(ex.Message, ex);
			}
			return result;
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x000B0560 File Offset: 0x000AE760
		private static bool EqualsFormattedNullValue(object value, object formattedNullValue, IFormatProvider formatInfo)
		{
			string text = formattedNullValue as string;
			string text2 = value as string;
			if (text != null && text2 != null)
			{
				return text.Length == text2.Length && string.Compare(text2, text, true, Formatter.GetFormatterCulture(formatInfo)) == 0;
			}
			return object.Equals(value, formattedNullValue);
		}

		// Token: 0x0600241F RID: 9247 RVA: 0x000B05AC File Offset: 0x000AE7AC
		private static string GetCantConvertMessage(object value, Type targetType)
		{
			string name = (value == null) ? "Formatter_CantConvertNull" : "Formatter_CantConvert";
			return string.Format(CultureInfo.CurrentCulture, SR.GetString(name), new object[]
			{
				value,
				targetType.Name
			});
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x000B05EC File Offset: 0x000AE7EC
		private static CultureInfo GetFormatterCulture(IFormatProvider formatInfo)
		{
			if (formatInfo is CultureInfo)
			{
				return formatInfo as CultureInfo;
			}
			return CultureInfo.CurrentCulture;
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x000B0604 File Offset: 0x000AE804
		public static object InvokeStringParseMethod(object value, Type targetType, IFormatProvider formatInfo)
		{
			object result;
			try
			{
				MethodInfo method = targetType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[]
				{
					Formatter.stringType,
					typeof(NumberStyles),
					typeof(IFormatProvider)
				}, null);
				if (method != null)
				{
					result = method.Invoke(null, new object[]
					{
						(string)value,
						NumberStyles.Any,
						formatInfo
					});
				}
				else
				{
					method = targetType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[]
					{
						Formatter.stringType,
						typeof(IFormatProvider)
					}, null);
					if (method != null)
					{
						result = method.Invoke(null, new object[]
						{
							(string)value,
							formatInfo
						});
					}
					else
					{
						method = targetType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[]
						{
							Formatter.stringType
						}, null);
						if (method != null)
						{
							result = method.Invoke(null, new object[]
							{
								(string)value
							});
						}
						else
						{
							result = Formatter.parseMethodNotFound;
						}
					}
				}
			}
			catch (TargetInvocationException ex)
			{
				throw new FormatException(ex.InnerException.Message, ex.InnerException);
			}
			return result;
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x000B0750 File Offset: 0x000AE950
		public static bool IsNullData(object value, object dataSourceNullValue)
		{
			return value == null || value == DBNull.Value || object.Equals(value, Formatter.NullData(value.GetType(), dataSourceNullValue));
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x000B0771 File Offset: 0x000AE971
		public static object NullData(Type type, object dataSourceNullValue)
		{
			if (!type.IsGenericType || !(type.GetGenericTypeDefinition() == typeof(Nullable<>)))
			{
				return dataSourceNullValue;
			}
			if (dataSourceNullValue == null || dataSourceNullValue == DBNull.Value)
			{
				return null;
			}
			return dataSourceNullValue;
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x000B07A4 File Offset: 0x000AE9A4
		private static Type NullableUnwrap(Type type)
		{
			if (type == Formatter.stringType)
			{
				return Formatter.stringType;
			}
			Type underlyingType = Nullable.GetUnderlyingType(type);
			return underlyingType ?? type;
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x000B07D4 File Offset: 0x000AE9D4
		private static TypeConverter NullableUnwrap(TypeConverter typeConverter)
		{
			NullableConverter nullableConverter = typeConverter as NullableConverter;
			if (nullableConverter == null)
			{
				return typeConverter;
			}
			return nullableConverter.UnderlyingTypeConverter;
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x000B07F3 File Offset: 0x000AE9F3
		public static object GetDefaultDataSourceNullValue(Type type)
		{
			if (!(type != null) || type.IsValueType)
			{
				return Formatter.defaultDataSourceNullValue;
			}
			return null;
		}

		// Token: 0x04000F7D RID: 3965
		private static Type stringType = typeof(string);

		// Token: 0x04000F7E RID: 3966
		private static Type booleanType = typeof(bool);

		// Token: 0x04000F7F RID: 3967
		private static Type checkStateType = typeof(CheckState);

		// Token: 0x04000F80 RID: 3968
		private static object parseMethodNotFound = new object();

		// Token: 0x04000F81 RID: 3969
		private static object defaultDataSourceNullValue = DBNull.Value;
	}
}
