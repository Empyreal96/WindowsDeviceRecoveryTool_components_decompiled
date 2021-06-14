using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.EdmToClrConversion
{
	// Token: 0x020000C2 RID: 194
	public class EdmToClrConverter
	{
		// Token: 0x060003D6 RID: 982 RVA: 0x00009A80 File Offset: 0x00007C80
		public EdmToClrConverter()
		{
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00009AA9 File Offset: 0x00007CA9
		public EdmToClrConverter(TryCreateObjectInstance tryCreateObjectInstanceDelegate)
		{
			EdmUtil.CheckArgumentNull<TryCreateObjectInstance>(tryCreateObjectInstanceDelegate, "tryCreateObjectInstanceDelegate");
			this.tryCreateObjectInstanceDelegate = tryCreateObjectInstanceDelegate;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00009AE8 File Offset: 0x00007CE8
		public T AsClrValue<T>(IEdmValue edmValue)
		{
			EdmUtil.CheckArgumentNull<IEdmValue>(edmValue, "edmValue");
			bool convertEnumValues = false;
			return (T)((object)this.AsClrValue(edmValue, typeof(T), convertEnumValues));
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00009B1C File Offset: 0x00007D1C
		public object AsClrValue(IEdmValue edmValue, Type clrType)
		{
			EdmUtil.CheckArgumentNull<IEdmValue>(edmValue, "edmValue");
			EdmUtil.CheckArgumentNull<Type>(clrType, "clrType");
			bool convertEnumValues = true;
			return this.AsClrValue(edmValue, clrType, convertEnumValues);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00009B4C File Offset: 0x00007D4C
		public void RegisterConvertedObject(IEdmStructuredValue edmValue, object clrObject)
		{
			this.convertedObjects.Add(edmValue, clrObject);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00009B5B File Offset: 0x00007D5B
		internal static byte[] AsClrByteArray(IEdmValue edmValue)
		{
			EdmUtil.CheckArgumentNull<IEdmValue>(edmValue, "edmValue");
			if (edmValue is IEdmNullValue)
			{
				return null;
			}
			return ((IEdmBinaryValue)edmValue).Value;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00009B7E File Offset: 0x00007D7E
		internal static string AsClrString(IEdmValue edmValue)
		{
			EdmUtil.CheckArgumentNull<IEdmValue>(edmValue, "edmValue");
			if (edmValue is IEdmNullValue)
			{
				return null;
			}
			return ((IEdmStringValue)edmValue).Value;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00009BA1 File Offset: 0x00007DA1
		internal static bool AsClrBoolean(IEdmValue edmValue)
		{
			EdmUtil.CheckArgumentNull<IEdmValue>(edmValue, "edmValue");
			return ((IEdmBooleanValue)edmValue).Value;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00009BBA File Offset: 0x00007DBA
		internal static long AsClrInt64(IEdmValue edmValue)
		{
			EdmUtil.CheckArgumentNull<IEdmValue>(edmValue, "edmValue");
			return ((IEdmIntegerValue)edmValue).Value;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00009BD3 File Offset: 0x00007DD3
		internal static char AsClrChar(IEdmValue edmValue)
		{
			return checked((char)EdmToClrConverter.AsClrInt64(edmValue));
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00009BDC File Offset: 0x00007DDC
		internal static byte AsClrByte(IEdmValue edmValue)
		{
			return checked((byte)EdmToClrConverter.AsClrInt64(edmValue));
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00009BE5 File Offset: 0x00007DE5
		internal static short AsClrInt16(IEdmValue edmValue)
		{
			return checked((short)EdmToClrConverter.AsClrInt64(edmValue));
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00009BEE File Offset: 0x00007DEE
		internal static int AsClrInt32(IEdmValue edmValue)
		{
			return checked((int)EdmToClrConverter.AsClrInt64(edmValue));
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00009BF7 File Offset: 0x00007DF7
		internal static double AsClrDouble(IEdmValue edmValue)
		{
			EdmUtil.CheckArgumentNull<IEdmValue>(edmValue, "edmValue");
			return ((IEdmFloatingValue)edmValue).Value;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00009C10 File Offset: 0x00007E10
		internal static float AsClrSingle(IEdmValue edmValue)
		{
			return (float)EdmToClrConverter.AsClrDouble(edmValue);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00009C19 File Offset: 0x00007E19
		internal static decimal AsClrDecimal(IEdmValue edmValue)
		{
			EdmUtil.CheckArgumentNull<IEdmValue>(edmValue, "edmValue");
			return ((IEdmDecimalValue)edmValue).Value;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00009C32 File Offset: 0x00007E32
		internal static DateTime AsClrDateTime(IEdmValue edmValue)
		{
			EdmUtil.CheckArgumentNull<IEdmValue>(edmValue, "edmValue");
			return ((IEdmDateTimeValue)edmValue).Value;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00009C4B File Offset: 0x00007E4B
		internal static TimeSpan AsClrTime(IEdmValue edmValue)
		{
			EdmUtil.CheckArgumentNull<IEdmValue>(edmValue, "edmValue");
			return ((IEdmTimeValue)edmValue).Value;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00009C64 File Offset: 0x00007E64
		internal static DateTimeOffset AsClrDateTimeOffset(IEdmValue edmValue)
		{
			EdmUtil.CheckArgumentNull<IEdmValue>(edmValue, "edmValue");
			return ((IEdmDateTimeOffsetValue)edmValue).Value;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00009C80 File Offset: 0x00007E80
		private static bool TryConvertAsPrimitiveType(TypeCode typeCode, IEdmValue edmValue, out object clrValue)
		{
			checked
			{
				switch (typeCode)
				{
				case TypeCode.Boolean:
					clrValue = EdmToClrConverter.AsClrBoolean(edmValue);
					return true;
				case TypeCode.Char:
					clrValue = EdmToClrConverter.AsClrChar(edmValue);
					return true;
				case TypeCode.SByte:
					clrValue = (sbyte)EdmToClrConverter.AsClrInt64(edmValue);
					return true;
				case TypeCode.Byte:
					clrValue = EdmToClrConverter.AsClrByte(edmValue);
					return true;
				case TypeCode.Int16:
					clrValue = EdmToClrConverter.AsClrInt16(edmValue);
					return true;
				case TypeCode.UInt16:
					clrValue = (ushort)EdmToClrConverter.AsClrInt64(edmValue);
					return true;
				case TypeCode.Int32:
					clrValue = EdmToClrConverter.AsClrInt32(edmValue);
					return true;
				case TypeCode.UInt32:
					clrValue = (uint)EdmToClrConverter.AsClrInt64(edmValue);
					return true;
				case TypeCode.Int64:
					clrValue = EdmToClrConverter.AsClrInt64(edmValue);
					return true;
				case TypeCode.UInt64:
					clrValue = (ulong)EdmToClrConverter.AsClrInt64(edmValue);
					return true;
				case TypeCode.Single:
					clrValue = EdmToClrConverter.AsClrSingle(edmValue);
					return true;
				case TypeCode.Double:
					clrValue = EdmToClrConverter.AsClrDouble(edmValue);
					return true;
				case TypeCode.Decimal:
					clrValue = EdmToClrConverter.AsClrDecimal(edmValue);
					return true;
				case TypeCode.DateTime:
					clrValue = EdmToClrConverter.AsClrDateTime(edmValue);
					return true;
				case TypeCode.String:
					clrValue = EdmToClrConverter.AsClrString(edmValue);
					return true;
				}
				clrValue = null;
				return false;
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00009DC0 File Offset: 0x00007FC0
		private static MethodInfo FindICollectionOfElementTypeAddMethod(Type collectionType, Type elementType)
		{
			Type type = typeof(ICollection<>).MakeGenericType(new Type[]
			{
				elementType
			});
			return type.GetMethod("Add");
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00009E10 File Offset: 0x00008010
		private static PropertyInfo FindProperty(Type clrObjectType, string propertyName)
		{
			List<PropertyInfo> list = (from p in clrObjectType.GetProperties()
			where p.Name == propertyName
			select p).ToList<PropertyInfo>();
			switch (list.Count)
			{
			case 0:
				return null;
			case 1:
				return list[0];
			default:
			{
				PropertyInfo propertyInfo = list[0];
				for (int i = 1; i < list.Count; i++)
				{
					PropertyInfo propertyInfo2 = list[i];
					if (propertyInfo.DeclaringType.IsAssignableFrom(propertyInfo2.DeclaringType))
					{
						propertyInfo = propertyInfo2;
					}
				}
				return propertyInfo;
			}
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00009EAC File Offset: 0x000080AC
		private static string GetEdmValueInterfaceName(IEdmValue edmValue)
		{
			Type type = typeof(IEdmValue);
			foreach (Type type2 in from i in edmValue.GetType().GetInterfaces()
			orderby i.FullName
			select i)
			{
				if (type.IsAssignableFrom(type2) && type != type2)
				{
					type = type2;
				}
			}
			return type.Name;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00009F40 File Offset: 0x00008140
		private object AsClrValue(IEdmValue edmValue, Type clrType, bool convertEnumValues)
		{
			TypeCode typeCode = PlatformHelper.GetTypeCode(clrType);
			if (typeCode == TypeCode.Object)
			{
				if (clrType.IsGenericType() && clrType.GetGenericTypeDefinition() == EdmToClrConverter.TypeNullableOfT)
				{
					if (edmValue is IEdmNullValue)
					{
						return null;
					}
					return this.AsClrValue(edmValue, clrType.GetGenericArguments().Single<Type>());
				}
				else
				{
					if (clrType == typeof(DateTime))
					{
						return EdmToClrConverter.AsClrDateTime(edmValue);
					}
					if (clrType == typeof(DateTimeOffset))
					{
						return EdmToClrConverter.AsClrDateTimeOffset(edmValue);
					}
					if (clrType == typeof(TimeSpan))
					{
						return EdmToClrConverter.AsClrTime(edmValue);
					}
					if (clrType == typeof(byte[]))
					{
						return EdmToClrConverter.AsClrByteArray(edmValue);
					}
					if (clrType.IsGenericType() && clrType.IsInterface() && (clrType.GetGenericTypeDefinition() == EdmToClrConverter.TypeICollectionOfT || clrType.GetGenericTypeDefinition() == EdmToClrConverter.TypeIListOfT || clrType.GetGenericTypeDefinition() == EdmToClrConverter.TypeIEnumerableOfT))
					{
						return this.AsListOfT(edmValue, clrType);
					}
					return this.AsClrObject(edmValue, clrType);
				}
			}
			else
			{
				bool flag = clrType.IsEnum();
				if (flag)
				{
					IEdmEnumValue edmEnumValue = edmValue as IEdmEnumValue;
					if (edmEnumValue != null)
					{
						edmValue = edmEnumValue.Value;
					}
				}
				object obj;
				if (!EdmToClrConverter.TryConvertAsPrimitiveType(PlatformHelper.GetTypeCode(clrType), edmValue, out obj))
				{
					throw new InvalidCastException(Strings.EdmToClr_UnsupportedTypeCode(typeCode));
				}
				if (!flag || !convertEnumValues)
				{
					return obj;
				}
				return this.GetEnumValue(obj, clrType);
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000A0AC File Offset: 0x000082AC
		private object AsListOfT(IEdmValue edmValue, Type clrType)
		{
			Type type = clrType.GetGenericArguments().Single<Type>();
			MethodInfo methodInfo;
			if (!this.enumerableConverters.TryGetValue(type, out methodInfo))
			{
				methodInfo = EdmToClrConverter.EnumerableToListOfTMethodInfo.MakeGenericMethod(new Type[]
				{
					type
				});
				this.enumerableConverters.Add(type, methodInfo);
			}
			object result;
			try
			{
				result = methodInfo.Invoke(null, new object[]
				{
					this.AsIEnumerable(edmValue, type)
				});
			}
			catch (TargetInvocationException ex)
			{
				if (ex.InnerException != null && ex.InnerException is InvalidCastException)
				{
					throw ex.InnerException;
				}
				throw;
			}
			return result;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000A14C File Offset: 0x0000834C
		private object GetEnumValue(object clrValue, Type clrType)
		{
			MethodInfo methodInfo;
			if (!this.enumTypeConverters.TryGetValue(clrType, out methodInfo))
			{
				methodInfo = EdmToClrConverter.CastToClrTypeMethodInfo.MakeGenericMethod(new Type[]
				{
					clrType
				});
				this.enumTypeConverters.Add(clrType, methodInfo);
			}
			object result;
			try
			{
				result = methodInfo.Invoke(null, new object[]
				{
					clrValue
				});
			}
			catch (TargetInvocationException ex)
			{
				if (ex.InnerException != null && ex.InnerException is InvalidCastException)
				{
					throw ex.InnerException;
				}
				throw;
			}
			return result;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000A1D8 File Offset: 0x000083D8
		private object AsClrObject(IEdmValue edmValue, Type clrObjectType)
		{
			EdmUtil.CheckArgumentNull<IEdmValue>(edmValue, "edmValue");
			EdmUtil.CheckArgumentNull<Type>(clrObjectType, "clrObjectType");
			if (edmValue is IEdmNullValue)
			{
				return null;
			}
			IEdmStructuredValue edmStructuredValue = edmValue as IEdmStructuredValue;
			if (edmStructuredValue == null)
			{
				if (edmValue is IEdmCollectionValue)
				{
					throw new InvalidCastException(Strings.EdmToClr_CannotConvertEdmCollectionValueToClrType(clrObjectType.FullName));
				}
				throw new InvalidCastException(Strings.EdmToClr_CannotConvertEdmValueToClrType(EdmToClrConverter.GetEdmValueInterfaceName(edmValue), clrObjectType.FullName));
			}
			else
			{
				object obj;
				if (this.convertedObjects.TryGetValue(edmStructuredValue, out obj))
				{
					return obj;
				}
				if (!clrObjectType.IsClass())
				{
					throw new InvalidCastException(Strings.EdmToClr_StructuredValueMappedToNonClass);
				}
				bool flag;
				if (this.tryCreateObjectInstanceDelegate != null && this.tryCreateObjectInstanceDelegate(edmStructuredValue, clrObjectType, this, out obj, out flag))
				{
					if (obj != null)
					{
						Type type = obj.GetType();
						if (!clrObjectType.IsAssignableFrom(type))
						{
							throw new InvalidCastException(Strings.EdmToClr_TryCreateObjectInstanceReturnedWrongObject(type.FullName, clrObjectType.FullName));
						}
						clrObjectType = type;
					}
				}
				else
				{
					obj = Activator.CreateInstance(clrObjectType);
					flag = false;
				}
				this.convertedObjects[edmStructuredValue] = obj;
				if (!flag && obj != null)
				{
					this.PopulateObjectProperties(edmStructuredValue, obj, clrObjectType);
				}
				return obj;
			}
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000A2D8 File Offset: 0x000084D8
		private void PopulateObjectProperties(IEdmStructuredValue edmValue, object clrObject, Type clrObjectType)
		{
			HashSetInternal<string> hashSetInternal = new HashSetInternal<string>();
			foreach (IEdmPropertyValue edmPropertyValue in edmValue.PropertyValues)
			{
				PropertyInfo propertyInfo = EdmToClrConverter.FindProperty(clrObjectType, edmPropertyValue.Name);
				if (propertyInfo != null)
				{
					if (hashSetInternal.Contains(edmPropertyValue.Name))
					{
						throw new InvalidCastException(Strings.EdmToClr_StructuredPropertyDuplicateValue(edmPropertyValue.Name));
					}
					if (!this.TrySetCollectionProperty(propertyInfo, clrObject, edmPropertyValue))
					{
						object value = this.AsClrValue(edmPropertyValue.Value, propertyInfo.PropertyType);
						propertyInfo.SetValue(clrObject, value, null);
					}
					hashSetInternal.Add(edmPropertyValue.Name);
				}
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000A394 File Offset: 0x00008594
		private bool TrySetCollectionProperty(PropertyInfo clrProperty, object clrObject, IEdmPropertyValue propertyValue)
		{
			Type propertyType = clrProperty.PropertyType;
			if (propertyType.IsGenericType() && propertyType.IsInterface())
			{
				Type genericTypeDefinition = propertyType.GetGenericTypeDefinition();
				bool flag = genericTypeDefinition == EdmToClrConverter.TypeIEnumerableOfT;
				if (flag || genericTypeDefinition == EdmToClrConverter.TypeICollectionOfT || genericTypeDefinition == EdmToClrConverter.TypeIListOfT)
				{
					object obj = clrProperty.GetValue(clrObject, null);
					Type type = propertyType.GetGenericArguments().Single<Type>();
					Type type2;
					if (obj == null)
					{
						type2 = EdmToClrConverter.TypeListOfT.MakeGenericType(new Type[]
						{
							type
						});
						obj = Activator.CreateInstance(type2);
						clrProperty.SetValue(clrObject, obj, null);
					}
					else
					{
						if (flag)
						{
							throw new InvalidCastException(Strings.EdmToClr_IEnumerableOfTPropertyAlreadyHasValue(clrProperty.Name, clrProperty.DeclaringType.FullName));
						}
						type2 = obj.GetType();
					}
					MethodInfo methodInfo = EdmToClrConverter.FindICollectionOfElementTypeAddMethod(type2, type);
					foreach (object obj2 in this.AsIEnumerable(propertyValue.Value, type))
					{
						try
						{
							methodInfo.Invoke(obj, new object[]
							{
								obj2
							});
						}
						catch (TargetInvocationException ex)
						{
							if (ex.InnerException != null && ex.InnerException is InvalidCastException)
							{
								throw ex.InnerException;
							}
							throw;
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000A6D0 File Offset: 0x000088D0
		private IEnumerable AsIEnumerable(IEdmValue edmValue, Type elementType)
		{
			foreach (IEdmDelayedValue element in ((IEdmCollectionValue)edmValue).Elements)
			{
				yield return this.AsClrValue(element.Value, elementType);
			}
			yield break;
		}

		// Token: 0x0400017F RID: 383
		private static readonly Type TypeICollectionOfT = typeof(ICollection<>);

		// Token: 0x04000180 RID: 384
		private static readonly Type TypeIListOfT = typeof(IList<>);

		// Token: 0x04000181 RID: 385
		private static readonly Type TypeListOfT = typeof(List<>);

		// Token: 0x04000182 RID: 386
		private static readonly Type TypeIEnumerableOfT = typeof(IEnumerable<>);

		// Token: 0x04000183 RID: 387
		private static readonly Type TypeNullableOfT = typeof(Nullable<>);

		// Token: 0x04000184 RID: 388
		private static readonly MethodInfo CastToClrTypeMethodInfo = typeof(EdmToClrConverter.CastHelper).GetMethod("CastToClrType");

		// Token: 0x04000185 RID: 389
		private static readonly MethodInfo EnumerableToListOfTMethodInfo = typeof(EdmToClrConverter.CastHelper).GetMethod("EnumerableToListOfT");

		// Token: 0x04000186 RID: 390
		private readonly TryCreateObjectInstance tryCreateObjectInstanceDelegate;

		// Token: 0x04000187 RID: 391
		private readonly Dictionary<IEdmStructuredValue, object> convertedObjects = new Dictionary<IEdmStructuredValue, object>();

		// Token: 0x04000188 RID: 392
		private readonly Dictionary<Type, MethodInfo> enumTypeConverters = new Dictionary<Type, MethodInfo>();

		// Token: 0x04000189 RID: 393
		private readonly Dictionary<Type, MethodInfo> enumerableConverters = new Dictionary<Type, MethodInfo>();

		// Token: 0x020000C3 RID: 195
		private static class CastHelper
		{
			// Token: 0x060003F6 RID: 1014 RVA: 0x0000A786 File Offset: 0x00008986
			public static T CastToClrType<T>(object obj)
			{
				return (T)((object)obj);
			}

			// Token: 0x060003F7 RID: 1015 RVA: 0x0000A78E File Offset: 0x0000898E
			public static List<T> EnumerableToListOfT<T>(IEnumerable enumerable)
			{
				return enumerable.Cast<T>().ToList<T>();
			}
		}
	}
}
