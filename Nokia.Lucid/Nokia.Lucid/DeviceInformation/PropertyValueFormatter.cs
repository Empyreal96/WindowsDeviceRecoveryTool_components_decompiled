using System;
using System.Globalization;
using System.Security.AccessControl;
using System.Text;
using Nokia.Lucid.Properties;

namespace Nokia.Lucid.DeviceInformation
{
	// Token: 0x02000019 RID: 25
	public sealed class PropertyValueFormatter : IPropertyValueFormatter
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x00007690 File Offset: 0x00005890
		public object ReadFrom(byte[] buffer, int index, int count, PropertyType propertyType)
		{
			object result;
			if (!this.TryReadFrom(buffer, index, count, propertyType, out result))
			{
				string message = string.Format(CultureInfo.CurrentCulture, Resources.NotSupportedException_MessageFormat_PropertyTypeNotSupported, new object[]
				{
					propertyType
				});
				throw new NotSupportedException(message);
			}
			return result;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000076D8 File Offset: 0x000058D8
		public bool TryReadFrom(byte[] buffer, int index, int count, PropertyType propertyType, out object value)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			switch (propertyType)
			{
			case PropertyType.Null:
				value = null;
				return true;
			case PropertyType.SByte:
			case PropertyType.Byte:
			case PropertyType.Int16:
			case PropertyType.UInt16:
			case PropertyType.Int64:
			case PropertyType.UInt64:
			case PropertyType.Float:
			case PropertyType.Double:
			case PropertyType.Decimal:
			case PropertyType.Currency:
			case PropertyType.Date:
			case PropertyType.PropertyKey:
				break;
			case PropertyType.Int32:
			case PropertyType.UInt32:
			case PropertyType.PropertyType:
			case PropertyType.Win32Error:
			case PropertyType.NTStatus:
				value = BitConverter.ToInt32(buffer, index);
				return true;
			case PropertyType.Guid:
				value = PropertyValueFormatter.ReadGuid(buffer, index, count);
				return true;
			case PropertyType.FileTime:
				value = PropertyValueFormatter.ReadFileTime(buffer, index);
				return true;
			case PropertyType.Boolean:
				value = PropertyValueFormatter.ReadBoolean(buffer, index);
				return true;
			case PropertyType.String:
			case PropertyType.SecurityDescriptorString:
			case PropertyType.StringIndirect:
				value = PropertyValueFormatter.ReadUnicodeString(buffer, index, count);
				return true;
			case PropertyType.SecurityDescriptor:
				value = PropertyValueFormatter.ReadSecurityDescriptor(buffer, index);
				return true;
			default:
				switch (propertyType)
				{
				case PropertyType.SByteArray:
				case PropertyType.ByteArray:
					value = PropertyValueFormatter.ReadByteArray(buffer, index, count);
					return true;
				default:
					if (propertyType == PropertyType.StringList)
					{
						value = PropertyValueFormatter.ReadUnicodeStringArray(buffer, index, count);
						return true;
					}
					break;
				}
				break;
			}
			value = null;
			return false;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00007804 File Offset: 0x00005A04
		private static byte[] ReadByteArray(byte[] buffer, int index, int count)
		{
			byte[] array = new byte[count];
			Array.Copy(buffer, index, array, 0, array.Length);
			return array;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00007828 File Offset: 0x00005A28
		private static string[] ReadUnicodeStringArray(byte[] buffer, int index, int count)
		{
			string @string = Encoding.Unicode.GetString(buffer, index, count);
			char[] separator = new char[1];
			return @string.Split(separator, StringSplitOptions.RemoveEmptyEntries);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00007850 File Offset: 0x00005A50
		private static DateTime ReadFileTime(byte[] buffer, int index)
		{
			long fileTime = BitConverter.ToInt64(buffer, index);
			return DateTime.FromFileTimeUtc(fileTime);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000786C File Offset: 0x00005A6C
		private static Guid ReadGuid(byte[] buffer, int index, int count)
		{
			if (index == 0 && count == buffer.Length)
			{
				return new Guid(buffer);
			}
			byte[] array = new byte[count];
			Array.Copy(buffer, index, array, 0, array.Length);
			return new Guid(array);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000078A2 File Offset: 0x00005AA2
		private static bool ReadBoolean(byte[] buffer, int index)
		{
			return buffer[index] != 0;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000078B0 File Offset: 0x00005AB0
		private static string ReadUnicodeString(byte[] buffer, int index, int count)
		{
			string @string = Encoding.Unicode.GetString(buffer, index, count);
			string text = @string;
			char[] trimChars = new char[1];
			return text.TrimEnd(trimChars);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000078D9 File Offset: 0x00005AD9
		private static GenericSecurityDescriptor ReadSecurityDescriptor(byte[] buffer, int index)
		{
			return new RawSecurityDescriptor(buffer, index);
		}

		// Token: 0x04000069 RID: 105
		public static readonly PropertyValueFormatter Default = new PropertyValueFormatter();
	}
}
