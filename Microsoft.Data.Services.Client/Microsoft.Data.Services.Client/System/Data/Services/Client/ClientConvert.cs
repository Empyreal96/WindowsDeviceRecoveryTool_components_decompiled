using System;
using System.Globalization;
using System.Reflection;

namespace System.Data.Services.Client
{
	// Token: 0x02000104 RID: 260
	internal static class ClientConvert
	{
		// Token: 0x06000868 RID: 2152 RVA: 0x000232C4 File Offset: 0x000214C4
		internal static object ChangeType(string propertyValue, Type propertyType)
		{
			PrimitiveType primitiveType;
			if (PrimitiveType.TryGetPrimitiveType(propertyType, out primitiveType) && primitiveType.TypeConverter != null)
			{
				try
				{
					return primitiveType.TypeConverter.Parse(propertyValue);
				}
				catch (FormatException innerException)
				{
					propertyValue = ((propertyValue.Length == 0) ? "String.Empty" : "String");
					throw Error.InvalidOperation(Strings.Deserialize_Current(propertyType.ToString(), propertyValue), innerException);
				}
				catch (OverflowException innerException2)
				{
					propertyValue = ((propertyValue.Length == 0) ? "String.Empty" : "String");
					throw Error.InvalidOperation(Strings.Deserialize_Current(propertyType.ToString(), propertyValue), innerException2);
				}
				return propertyValue;
			}
			return propertyValue;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00023368 File Offset: 0x00021568
		internal static bool TryConvertBinaryToByteArray(object binaryValue, out byte[] converted)
		{
			Type type = binaryValue.GetType();
			PrimitiveType primitiveType;
			if (PrimitiveType.TryGetPrimitiveType(type, out primitiveType) && type == BinaryTypeConverter.BinaryType)
			{
				converted = (byte[])type.InvokeMember("ToArray", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, binaryValue, null, CultureInfo.InvariantCulture);
				return true;
			}
			converted = null;
			return false;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x000233B8 File Offset: 0x000215B8
		internal static bool ToNamedType(string typeName, out Type type)
		{
			type = typeof(string);
			if (string.IsNullOrEmpty(typeName))
			{
				return true;
			}
			PrimitiveType primitiveType;
			if (PrimitiveType.TryGetPrimitiveType(typeName, out primitiveType))
			{
				type = primitiveType.ClrType;
				return true;
			}
			return false;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x000233F0 File Offset: 0x000215F0
		internal static string ToString(object propertyValue)
		{
			PrimitiveType primitiveType;
			if (PrimitiveType.TryGetPrimitiveType(propertyValue.GetType(), out primitiveType) && primitiveType.TypeConverter != null)
			{
				return primitiveType.TypeConverter.ToString(propertyValue);
			}
			return propertyValue.ToString();
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00023428 File Offset: 0x00021628
		internal static string GetEdmType(Type propertyType)
		{
			PrimitiveType primitiveType;
			if (!PrimitiveType.TryGetPrimitiveType(propertyType, out primitiveType))
			{
				return null;
			}
			if (primitiveType.EdmTypeName != null)
			{
				return primitiveType.EdmTypeName;
			}
			throw new NotSupportedException(Strings.ALinq_CantCastToUnsupportedPrimitive(propertyType.Name));
		}
	}
}
