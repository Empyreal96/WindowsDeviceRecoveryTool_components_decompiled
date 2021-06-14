using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200005E RID: 94
	public static class RegBuilder
	{
		// Token: 0x0600024A RID: 586 RVA: 0x0000B0E0 File Offset: 0x000092E0
		private static void CheckConflicts(IEnumerable<RegValueInfo> values)
		{
			Dictionary<string, RegValueInfo> dictionary = new Dictionary<string, RegValueInfo>();
			foreach (RegValueInfo regValueInfo in values)
			{
				if (regValueInfo.ValueName != null)
				{
					RegValueInfo regValueInfo2 = null;
					if (dictionary.TryGetValue(regValueInfo.ValueName, out regValueInfo2))
					{
						throw new IUException("Registry conflict discovered: keyName: {0}, valueName: {1}, oldValue: {2}, newValue: {3}", new object[]
						{
							regValueInfo.KeyName,
							regValueInfo.ValueName,
							regValueInfo2.Value,
							regValueInfo.Value
						});
					}
					dictionary.Add(regValueInfo.ValueName, regValueInfo);
				}
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000B18C File Offset: 0x0000938C
		private static void ConvertRegSz(StringBuilder output, string name, string value)
		{
			RegUtil.RegOutput(output, name, value, false);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000B197 File Offset: 0x00009397
		private static void ConvertRegExpandSz(StringBuilder output, string name, string value)
		{
			RegUtil.RegOutput(output, name, value, true);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000B1A4 File Offset: 0x000093A4
		private static void ConvertRegMultiSz(StringBuilder output, string name, string value)
		{
			RegUtil.RegOutput(output, name, value.Split(new char[]
			{
				';'
			}));
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000B1CC File Offset: 0x000093CC
		private static void ConvertRegDWord(StringBuilder output, string name, string value)
		{
			uint value2 = 0U;
			if (!uint.TryParse(value, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture.NumberFormat, out value2))
			{
				throw new IUException("Invalid dword string: {0}", new object[]
				{
					value
				});
			}
			RegUtil.RegOutput(output, name, value2);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000B214 File Offset: 0x00009414
		private static void ConvertRegQWord(StringBuilder output, string name, string value)
		{
			ulong value2 = 0UL;
			if (!ulong.TryParse(value, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture.NumberFormat, out value2))
			{
				throw new IUException("Invalid qword string: {0}", new object[]
				{
					value
				});
			}
			RegUtil.RegOutput(output, name, value2);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000B25C File Offset: 0x0000945C
		private static void ConvertRegBinary(StringBuilder output, string name, string value)
		{
			RegUtil.RegOutput(output, name, RegUtil.HexStringToByteArray(value));
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000B26C File Offset: 0x0000946C
		private static void ConvertRegHex(StringBuilder output, string name, string value)
		{
			new List<byte>();
			Match match = Regex.Match(value, "^hex\\((?<type>[0-9A-Fa-f]+)\\):(?<value>.*)$");
			if (!match.Success)
			{
				throw new IUException("Invalid value '{0}' for REG_HEX type, shoudl be 'hex(<type>):<binary_values>'", new object[]
				{
					value
				});
			}
			int type = 0;
			if (!int.TryParse(match.Groups["type"].Value, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture.NumberFormat, out type))
			{
				throw new IUException("Invalid hex type '{0}' in REG_HEX value '{1}'", new object[]
				{
					match.Groups["type"].Value,
					value
				});
			}
			string value2 = match.Groups["value"].Value;
			RegUtil.RegOutput(output, name, type, RegUtil.HexStringToByteArray(value2));
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000B330 File Offset: 0x00009530
		private static void WriteValue(RegValueInfo value, StringBuilder regContent)
		{
			switch (value.Type)
			{
			case RegValueType.String:
				RegBuilder.ConvertRegSz(regContent, value.ValueName, value.Value);
				return;
			case RegValueType.ExpandString:
				RegBuilder.ConvertRegExpandSz(regContent, value.ValueName, value.Value);
				return;
			case RegValueType.Binary:
				RegBuilder.ConvertRegBinary(regContent, value.ValueName, value.Value);
				return;
			case RegValueType.DWord:
				RegBuilder.ConvertRegDWord(regContent, value.ValueName, value.Value);
				return;
			case RegValueType.MultiString:
				RegBuilder.ConvertRegMultiSz(regContent, value.ValueName, value.Value);
				return;
			case RegValueType.QWord:
				RegBuilder.ConvertRegQWord(regContent, value.ValueName, value.Value);
				return;
			case RegValueType.Hex:
				RegBuilder.ConvertRegHex(regContent, value.ValueName, value.Value);
				return;
			default:
				throw new IUException("Unknown registry value type '{0}'", new object[]
				{
					value.Type
				});
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000B410 File Offset: 0x00009610
		private static void WriteKey(string keyName, IEnumerable<RegValueInfo> values, StringBuilder regContent)
		{
			regContent.AppendFormat("[{0}]", keyName);
			regContent.AppendLine();
			foreach (RegValueInfo regValueInfo in values)
			{
				if (regValueInfo.ValueName != null)
				{
					RegBuilder.WriteValue(regValueInfo, regContent);
				}
			}
			regContent.AppendLine();
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000B47C File Offset: 0x0000967C
		public static void Build(IEnumerable<RegValueInfo> values, string outputFile)
		{
			RegBuilder.Build(values, outputFile, null);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000B490 File Offset: 0x00009690
		public static void Build(IEnumerable<RegValueInfo> values, string outputFile, string headerComment = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Windows Registry Editor Version 5.00");
			if (!string.IsNullOrEmpty(headerComment))
			{
				string[] array = headerComment.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				foreach (string text in array)
				{
					string text2 = text.TrimStart(new char[]
					{
						' '
					});
					if (text2 != string.Empty && text2[0] == ';')
					{
						stringBuilder.AppendLine(text);
					}
					else
					{
						stringBuilder.AppendLine("; " + text);
					}
				}
				stringBuilder.AppendLine("");
			}
			foreach (IGrouping<string, RegValueInfo> grouping in from x in values
			group x by x.KeyName)
			{
				RegBuilder.CheckConflicts(grouping);
				RegBuilder.WriteKey(grouping.Key, grouping, stringBuilder);
			}
			LongPathFile.WriteAllText(outputFile, stringBuilder.ToString(), Encoding.Unicode);
		}
	}
}
