using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200005D RID: 93
	public static class RegUtil
	{
		// Token: 0x0600023C RID: 572 RVA: 0x0000ACF1 File Offset: 0x00008EF1
		private static string QuoteString(string input)
		{
			return "\"" + input.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000AD21 File Offset: 0x00008F21
		private static string NormalizeValueName(string name)
		{
			if (name == "@")
			{
				return "@";
			}
			return RegUtil.QuoteString(name);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000AD3C File Offset: 0x00008F3C
		private static byte[] RegStringToBytes(string value)
		{
			return Encoding.Unicode.GetBytes(value);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000AD4C File Offset: 0x00008F4C
		public static RegValueType RegValueTypeForString(string strType)
		{
			Type typeFromHandle = typeof(RegValueType);
			FieldInfo[] fields = typeFromHandle.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(XmlEnumAttribute), false);
				if (customAttributes.Length == 1 && strType.Equals(((XmlEnumAttribute)customAttributes[0]).Name, StringComparison.OrdinalIgnoreCase))
				{
					return (RegValueType)fieldInfo.GetRawConstantValue();
				}
			}
			throw new ArgumentException(string.Format("Unknown Registry value type: {0}", strType));
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000ADD8 File Offset: 0x00008FD8
		public static byte[] HexStringToByteArray(string hexString)
		{
			List<byte> list = new List<byte>();
			if (hexString != string.Empty)
			{
				foreach (string s in hexString.Split(new char[]
				{
					','
				}))
				{
					byte item = 0;
					if (!byte.TryParse(s, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture.NumberFormat, out item))
					{
						throw new IUException("Invalid hex string: {0}", new object[]
						{
							hexString
						});
					}
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000AE68 File Offset: 0x00009068
		public static void ByteArrayToRegString(StringBuilder output, byte[] data, int maxOnALine = 2147483647)
		{
			int num = 0;
			int i = data.Length;
			while (i > 0)
			{
				int num2 = Math.Min(i, maxOnALine);
				string text = BitConverter.ToString(data, num, num2);
				text = text.Replace('-', ',');
				output.Append(text);
				num += num2;
				i -= num2;
				if (i > 0)
				{
					output.AppendLine(",\\");
				}
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000AED0 File Offset: 0x000090D0
		public static void RegOutput(StringBuilder output, string name, IEnumerable<string> values)
		{
			string arg = RegUtil.NormalizeValueName(name);
			output.AppendFormat(";Value:{0}", string.Join(";", from x in values
			select x.Replace(";", "\\;")));
			output.AppendLine();
			output.AppendFormat("{0}=hex(7):", arg);
			RegUtil.ByteArrayToRegString(output, RegUtil.RegStringToBytes(string.Join("\0", values) + "\0\0"), RegUtil.BinaryLineLength / 3);
			output.AppendLine();
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000AF60 File Offset: 0x00009160
		public static void RegOutput(StringBuilder output, string name, string value, bool expandable)
		{
			string arg = RegUtil.NormalizeValueName(name);
			if (expandable)
			{
				output.AppendFormat(";Value:{0}", value);
				output.AppendLine();
				output.AppendFormat("{0}=hex(2):", arg);
				RegUtil.ByteArrayToRegString(output, RegUtil.RegStringToBytes(value + "\0"), RegUtil.BinaryLineLength / 3);
			}
			else
			{
				output.AppendFormat("{0}={1}", arg, RegUtil.QuoteString(value));
			}
			output.AppendLine();
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000AFD4 File Offset: 0x000091D4
		public static void RegOutput(StringBuilder output, string name, ulong value)
		{
			string arg = RegUtil.NormalizeValueName(name);
			output.AppendFormat(";Value:0X{0:X16}", value);
			output.AppendLine();
			output.AppendFormat("{0}=hex(b):", arg);
			RegUtil.ByteArrayToRegString(output, BitConverter.GetBytes(value), int.MaxValue);
			output.AppendLine();
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000B028 File Offset: 0x00009228
		public static void RegOutput(StringBuilder output, string name, uint value)
		{
			string arg = RegUtil.NormalizeValueName(name);
			output.AppendFormat("{0}=dword:{1:X8}", arg, value);
			output.AppendLine();
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000B058 File Offset: 0x00009258
		public static void RegOutput(StringBuilder output, string name, byte[] value)
		{
			string arg = RegUtil.NormalizeValueName(name);
			output.AppendFormat("{0}=hex:", arg);
			RegUtil.ByteArrayToRegString(output, value.ToArray<byte>(), RegUtil.BinaryLineLength / 3);
			output.AppendLine();
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000B094 File Offset: 0x00009294
		public static void RegOutput(StringBuilder output, string name, int type, byte[] value)
		{
			string arg = RegUtil.NormalizeValueName(name);
			output.AppendFormat("{0}=hex({1:x}):", arg, type);
			RegUtil.ByteArrayToRegString(output, value.ToArray<byte>(), RegUtil.BinaryLineLength / 3);
			output.AppendLine();
		}

		// Token: 0x0400016C RID: 364
		private const string c_strDefaultValueName = "@";

		// Token: 0x0400016D RID: 365
		private const int c_iBinaryStringLengthPerByte = 3;

		// Token: 0x0400016E RID: 366
		public const string c_strRegHeader = "Windows Registry Editor Version 5.00";

		// Token: 0x0400016F RID: 367
		public static int BinaryLineLength = 120;
	}
}
