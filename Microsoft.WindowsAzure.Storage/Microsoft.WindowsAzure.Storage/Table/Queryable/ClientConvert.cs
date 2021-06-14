using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000108 RID: 264
	internal static class ClientConvert
	{
		// Token: 0x060012B6 RID: 4790 RVA: 0x000458A0 File Offset: 0x00043AA0
		internal static object ChangeType(string propertyValue, Type propertyType)
		{
			object result;
			try
			{
				switch (ClientConvert.IndexOfStorage(propertyType))
				{
				case 0:
					result = XmlConvert.ToBoolean(propertyValue);
					break;
				case 1:
					result = XmlConvert.ToByte(propertyValue);
					break;
				case 2:
					result = Convert.FromBase64String(propertyValue);
					break;
				case 3:
					result = XmlConvert.ToChar(propertyValue);
					break;
				case 4:
					result = propertyValue.ToCharArray();
					break;
				case 5:
					result = XmlConvert.ToDateTime(propertyValue, XmlDateTimeSerializationMode.RoundtripKind);
					break;
				case 6:
					result = XmlConvert.ToDateTimeOffset(propertyValue);
					break;
				case 7:
					result = XmlConvert.ToDecimal(propertyValue);
					break;
				case 8:
					result = XmlConvert.ToDouble(propertyValue);
					break;
				case 9:
					result = new Guid(propertyValue);
					break;
				case 10:
					result = XmlConvert.ToInt16(propertyValue);
					break;
				case 11:
					result = XmlConvert.ToInt32(propertyValue);
					break;
				case 12:
					result = XmlConvert.ToInt64(propertyValue);
					break;
				case 13:
					result = XmlConvert.ToSingle(propertyValue);
					break;
				case 14:
					result = propertyValue;
					break;
				case 15:
					result = XmlConvert.ToSByte(propertyValue);
					break;
				case 16:
					result = XmlConvert.ToTimeSpan(propertyValue);
					break;
				case 17:
					result = Type.GetType(propertyValue, true);
					break;
				case 18:
					result = XmlConvert.ToUInt16(propertyValue);
					break;
				case 19:
					result = XmlConvert.ToUInt32(propertyValue);
					break;
				case 20:
					result = XmlConvert.ToUInt64(propertyValue);
					break;
				case 21:
					result = ClientConvert.CreateUri(propertyValue, UriKind.RelativeOrAbsolute);
					break;
				case 22:
					result = ((0 < propertyValue.Length) ? XDocument.Parse(propertyValue) : new XDocument());
					break;
				case 23:
					result = XElement.Parse(propertyValue);
					break;
				case 24:
					result = Activator.CreateInstance(ClientConvert.KnownTypes[24], new object[]
					{
						Convert.FromBase64String(propertyValue)
					});
					break;
				default:
					result = propertyValue;
					break;
				}
			}
			catch (FormatException innerException)
			{
				propertyValue = ((propertyValue.Length == 0) ? "String.Empty" : "String");
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The current value '{1}' type is not compatible with the expected '{0}' type.", new object[]
				{
					propertyType.ToString(),
					propertyValue
				}), innerException);
			}
			catch (OverflowException innerException2)
			{
				propertyValue = ((propertyValue.Length == 0) ? "String.Empty" : "String");
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The current value '{1}' type is not compatible with the expected '{0}' type.", new object[]
				{
					propertyType.ToString(),
					propertyValue
				}), innerException2);
			}
			return result;
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00045B98 File Offset: 0x00043D98
		internal static bool IsBinaryValue(object value)
		{
			return 24 == ClientConvert.IndexOfStorage(value.GetType());
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00045BAC File Offset: 0x00043DAC
		internal static bool TryKeyBinaryToString(object binaryValue, out string result)
		{
			byte[] value = (byte[])binaryValue.GetType().InvokeMember("ToArray", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, binaryValue, null, null, CultureInfo.InvariantCulture, null);
			return WebConvert.TryKeyPrimitiveToString(value, out result);
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00045BE8 File Offset: 0x00043DE8
		internal static bool TryKeyPrimitiveToString(object value, out string result)
		{
			if (ClientConvert.IsBinaryValue(value))
			{
				return ClientConvert.TryKeyBinaryToString(value, out result);
			}
			if (value is DateTimeOffset)
			{
				value = ((DateTimeOffset)value).UtcDateTime;
			}
			else if (value is DateTimeOffset?)
			{
				value = ((DateTimeOffset?)value).Value.UtcDateTime;
			}
			return WebConvert.TryKeyPrimitiveToString(value, out result);
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00045C50 File Offset: 0x00043E50
		internal static bool ToNamedType(string typeName, out Type type)
		{
			type = typeof(string);
			return string.IsNullOrEmpty(typeName) || ClientConvert.NamedTypesMap.TryGetValue(typeName, out type);
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00045C74 File Offset: 0x00043E74
		internal static string ToTypeName(Type type)
		{
			foreach (KeyValuePair<string, Type> keyValuePair in ClientConvert.NamedTypesMap)
			{
				if (keyValuePair.Value == type)
				{
					return keyValuePair.Key;
				}
			}
			return type.FullName;
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x00045CE0 File Offset: 0x00043EE0
		internal static string ToString(object propertyValue, bool atomDateConstruct)
		{
			switch (ClientConvert.IndexOfStorage(propertyValue.GetType()))
			{
			case 0:
				return XmlConvert.ToString((bool)propertyValue);
			case 1:
				return XmlConvert.ToString((byte)propertyValue);
			case 2:
				return Convert.ToBase64String((byte[])propertyValue);
			case 3:
				return XmlConvert.ToString((char)propertyValue);
			case 4:
				return new string((char[])propertyValue);
			case 5:
			{
				DateTime dateTime = (DateTime)propertyValue;
				return XmlConvert.ToString((dateTime.Kind == DateTimeKind.Unspecified && atomDateConstruct) ? new DateTime(dateTime.Ticks, DateTimeKind.Utc) : dateTime, XmlDateTimeSerializationMode.RoundtripKind);
			}
			case 6:
				return XmlConvert.ToString((DateTimeOffset)propertyValue);
			case 7:
				return XmlConvert.ToString((decimal)propertyValue);
			case 8:
				return XmlConvert.ToString((double)propertyValue);
			case 9:
				return ((Guid)propertyValue).ToString();
			case 10:
				return XmlConvert.ToString((short)propertyValue);
			case 11:
				return XmlConvert.ToString((int)propertyValue);
			case 12:
				return XmlConvert.ToString((long)propertyValue);
			case 13:
				return XmlConvert.ToString((float)propertyValue);
			case 14:
				return (string)propertyValue;
			case 15:
				return XmlConvert.ToString((sbyte)propertyValue);
			case 16:
				return XmlConvert.ToString((TimeSpan)propertyValue);
			case 17:
				return ((Type)propertyValue).AssemblyQualifiedName;
			case 18:
				return XmlConvert.ToString((ushort)propertyValue);
			case 19:
				return XmlConvert.ToString((uint)propertyValue);
			case 20:
				return XmlConvert.ToString((ulong)propertyValue);
			case 21:
				return ((Uri)propertyValue).ToString();
			case 22:
				return ((XDocument)propertyValue).ToString();
			case 23:
				return ((XElement)propertyValue).ToString();
			case 24:
				return propertyValue.ToString();
			default:
				return propertyValue.ToString();
			}
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00045EB7 File Offset: 0x000440B7
		internal static bool IsKnownType(Type type)
		{
			return 0 <= ClientConvert.IndexOfStorage(type);
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00045EC5 File Offset: 0x000440C5
		internal static bool IsKnownNullableType(Type type)
		{
			return ClientConvert.IsKnownType(Nullable.GetUnderlyingType(type) ?? type);
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00045ED7 File Offset: 0x000440D7
		internal static bool IsSupportedPrimitiveTypeForUri(Type type)
		{
			return ClientConvert.ContainsReference<Type>(ClientConvert.NamedTypesMap.Values.ToArray<Type>(), type);
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00045EEE File Offset: 0x000440EE
		internal static bool ContainsReference<T>(T[] array, T value) where T : class
		{
			return 0 <= ClientConvert.IndexOfReference<T>(array, value);
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00045F00 File Offset: 0x00044100
		internal static string GetEdmType(Type propertyType)
		{
			switch (ClientConvert.IndexOfStorage(propertyType))
			{
			case 0:
				return "Edm.Boolean";
			case 1:
				return "Edm.Byte";
			case 2:
			case 24:
				return "Edm.Binary";
			case 3:
			case 4:
			case 14:
			case 17:
			case 21:
			case 22:
			case 23:
				return null;
			case 5:
				return "Edm.DateTime";
			case 6:
			case 16:
			case 18:
			case 19:
			case 20:
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Can't cast to unsupported type '{0}'", new object[]
				{
					propertyType.Name
				}));
			case 7:
				return "Edm.Decimal";
			case 8:
				return "Edm.Double";
			case 9:
				return "Edm.Guid";
			case 10:
				return "Edm.Int16";
			case 11:
				return "Edm.Int32";
			case 12:
				return "Edm.Int64";
			case 13:
				return "Edm.Single";
			case 15:
				return "Edm.SByte";
			default:
				return null;
			}
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00045FF4 File Offset: 0x000441F4
		private static Type[] CreateKnownPrimitives()
		{
			return new Type[]
			{
				typeof(bool),
				typeof(byte),
				typeof(byte[]),
				typeof(char),
				typeof(char[]),
				typeof(DateTime),
				typeof(DateTimeOffset),
				typeof(decimal),
				typeof(double),
				typeof(Guid),
				typeof(short),
				typeof(int),
				typeof(long),
				typeof(float),
				typeof(string),
				typeof(sbyte),
				typeof(TimeSpan),
				typeof(Type),
				typeof(ushort),
				typeof(uint),
				typeof(ulong),
				typeof(Uri),
				typeof(XDocument),
				typeof(XElement),
				null
			};
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00046158 File Offset: 0x00044358
		private static Dictionary<string, Type> CreateKnownNamesMap()
		{
			return new Dictionary<string, Type>(EqualityComparer<string>.Default)
			{
				{
					"Edm.String",
					typeof(string)
				},
				{
					"Edm.Boolean",
					typeof(bool)
				},
				{
					"Edm.Byte",
					typeof(byte)
				},
				{
					"Edm.DateTime",
					typeof(DateTime)
				},
				{
					"Edm.Decimal",
					typeof(decimal)
				},
				{
					"Edm.Double",
					typeof(double)
				},
				{
					"Edm.Guid",
					typeof(Guid)
				},
				{
					"Edm.Int16",
					typeof(short)
				},
				{
					"Edm.Int32",
					typeof(int)
				},
				{
					"Edm.Int64",
					typeof(long)
				},
				{
					"Edm.SByte",
					typeof(sbyte)
				},
				{
					"Edm.Single",
					typeof(float)
				},
				{
					"Edm.Binary",
					typeof(byte[])
				}
			};
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x00046284 File Offset: 0x00044484
		private static int IndexOfStorage(Type type)
		{
			int num = ClientConvert.IndexOfReference<Type>(ClientConvert.KnownTypes, type);
			if (num < 0 && ClientConvert.needSystemDataLinqBinary && type.Name == "Binary")
			{
				return ClientConvert.LoadSystemDataLinqBinary(type);
			}
			return num;
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x000462C4 File Offset: 0x000444C4
		internal static int IndexOfReference<T>(T[] array, T value) where T : class
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (object.ReferenceEquals(array[i], value))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x000462FB File Offset: 0x000444FB
		internal static Uri CreateUri(string value, UriKind kind)
		{
			if (value != null)
			{
				return new Uri(value, kind);
			}
			return null;
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x0004630C File Offset: 0x0004450C
		private static int LoadSystemDataLinqBinary(Type type)
		{
			if (type.Namespace == "System.Data.Linq" && AssemblyName.ReferenceMatchesDefinition(type.Assembly.GetName(), new AssemblyName("System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")))
			{
				ClientConvert.KnownTypes[24] = type;
				ClientConvert.needSystemDataLinqBinary = false;
				return 24;
			}
			return -1;
		}

		// Token: 0x04000567 RID: 1383
		private const string SystemDataLinq = "System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		// Token: 0x04000568 RID: 1384
		private static readonly Type[] KnownTypes = ClientConvert.CreateKnownPrimitives();

		// Token: 0x04000569 RID: 1385
		private static readonly Dictionary<string, Type> NamedTypesMap = ClientConvert.CreateKnownNamesMap();

		// Token: 0x0400056A RID: 1386
		private static bool needSystemDataLinqBinary = true;

		// Token: 0x02000109 RID: 265
		internal enum StorageType
		{
			// Token: 0x0400056C RID: 1388
			Boolean,
			// Token: 0x0400056D RID: 1389
			Byte,
			// Token: 0x0400056E RID: 1390
			ByteArray,
			// Token: 0x0400056F RID: 1391
			Char,
			// Token: 0x04000570 RID: 1392
			CharArray,
			// Token: 0x04000571 RID: 1393
			DateTime,
			// Token: 0x04000572 RID: 1394
			DateTimeOffset,
			// Token: 0x04000573 RID: 1395
			Decimal,
			// Token: 0x04000574 RID: 1396
			Double,
			// Token: 0x04000575 RID: 1397
			Guid,
			// Token: 0x04000576 RID: 1398
			Int16,
			// Token: 0x04000577 RID: 1399
			Int32,
			// Token: 0x04000578 RID: 1400
			Int64,
			// Token: 0x04000579 RID: 1401
			Single,
			// Token: 0x0400057A RID: 1402
			String,
			// Token: 0x0400057B RID: 1403
			SByte,
			// Token: 0x0400057C RID: 1404
			TimeSpan,
			// Token: 0x0400057D RID: 1405
			Type,
			// Token: 0x0400057E RID: 1406
			UInt16,
			// Token: 0x0400057F RID: 1407
			UInt32,
			// Token: 0x04000580 RID: 1408
			UInt64,
			// Token: 0x04000581 RID: 1409
			Uri,
			// Token: 0x04000582 RID: 1410
			XDocument,
			// Token: 0x04000583 RID: 1411
			XElement,
			// Token: 0x04000584 RID: 1412
			Binary
		}
	}
}
