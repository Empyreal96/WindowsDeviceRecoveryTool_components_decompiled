using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200001C RID: 28
	public class HiveToRegConverter
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x000070C0 File Offset: 0x000052C0
		public HiveToRegConverter(string hiveFile, string keyPrefix = null)
		{
			if (string.IsNullOrEmpty(hiveFile))
			{
				throw new ArgumentNullException("hiveFile");
			}
			if (!LongPathFile.Exists(hiveFile))
			{
				throw new FileNotFoundException(string.Format("Hive file {0} does not exist or cannot be read", hiveFile));
			}
			this.m_hiveFile = hiveFile;
			this.m_keyPrefix = keyPrefix;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00007120 File Offset: 0x00005320
		public void ConvertToReg(string outputFile, HashSet<string> exclusions = null, bool append = false)
		{
			if (string.IsNullOrEmpty(outputFile))
			{
				throw new ArgumentNullException("outputFile");
			}
			if (exclusions != null)
			{
				this.m_exclusions.UnionWith(exclusions);
			}
			FileMode mode = append ? FileMode.Append : FileMode.Create;
			using (this.m_writer = new StreamWriter(LongPathFile.Open(outputFile, mode, FileAccess.Write), Encoding.Unicode))
			{
				this.ConvertToStream(!append, null);
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000719C File Offset: 0x0000539C
		public void ConvertToReg(ref StringBuilder outputStr, HashSet<string> exclusions = null)
		{
			this.ConvertToReg(ref outputStr, null, true, exclusions);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000071A8 File Offset: 0x000053A8
		public void ConvertToReg(ref StringBuilder outputStr, string subKey, bool outputHeader, HashSet<string> exclusions = null)
		{
			if (outputStr == null)
			{
				throw new ArgumentNullException("outputStr");
			}
			if (exclusions != null)
			{
				this.m_exclusions.UnionWith(exclusions);
			}
			using (this.m_writer = new StringWriter(outputStr))
			{
				this.ConvertToStream(outputHeader, subKey);
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000720C File Offset: 0x0000540C
		private void ConvertToStream(bool outputHeader, string subKey)
		{
			if (outputHeader)
			{
				this.m_writer.WriteLine("Windows Registry Editor Version 5.00");
			}
			using (ORRegistryKey orregistryKey = ORRegistryKey.OpenHive(this.m_hiveFile, this.m_keyPrefix))
			{
				ORRegistryKey orregistryKey2 = orregistryKey;
				if (!string.IsNullOrEmpty(subKey))
				{
					orregistryKey2 = orregistryKey.OpenSubKey(subKey);
				}
				this.WriteKeyContents(orregistryKey2);
				this.WalkHive(orregistryKey2);
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00007280 File Offset: 0x00005480
		private void WalkHive(ORRegistryKey root)
		{
			foreach (string subkeyname in root.SubKeys.OrderBy((string x) => x, StringComparer.OrdinalIgnoreCase))
			{
				using (ORRegistryKey orregistryKey = root.OpenSubKey(subkeyname))
				{
					try
					{
						bool flag = this.m_exclusions.Contains(orregistryKey.FullName + "\\*");
						bool flag2 = this.m_exclusions.Contains(orregistryKey.FullName);
						if (!flag)
						{
							if (!flag2)
							{
								this.WriteKeyContents(orregistryKey);
							}
							this.WalkHive(orregistryKey);
						}
					}
					catch (Exception innerException)
					{
						throw new IUException("Failed to iterate through hive", innerException);
					}
				}
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00007374 File Offset: 0x00005574
		private void WriteKeyName(string keyname)
		{
			this.m_writer.WriteLine();
			this.m_writer.WriteLine("[{0}]", keyname);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00007394 File Offset: 0x00005594
		private string FormatValueName(string valueName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (valueName.Equals(""))
			{
				stringBuilder.Append("@=");
			}
			else
			{
				StringBuilder stringBuilder2 = new StringBuilder(valueName);
				stringBuilder2.Replace("\\", "\\\\").Replace("\"", "\\\"");
				stringBuilder.AppendFormat("\"{0}\"=", stringBuilder2.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00007404 File Offset: 0x00005604
		private string FormatValue(ORRegistryKey key, string valueName)
		{
			RegistryValueType valueKind = key.GetValueKind(valueName);
			StringBuilder stringBuilder = new StringBuilder();
			switch (valueKind)
			{
			case RegistryValueType.String:
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.Append(key.GetStringValue(valueName));
				stringBuilder2.Replace("\\", "\\\\").Replace("\"", "\\\"");
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "\"{0}\"", new object[]
				{
					stringBuilder2.ToString()
				});
				goto IL_175;
			}
			case RegistryValueType.DWord:
			{
				uint dwordValue = key.GetDwordValue(valueName);
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "dword:{0:X8}", new object[]
				{
					dwordValue
				});
				goto IL_175;
			}
			case RegistryValueType.MultiString:
			{
				byte[] byteValue = key.GetByteValue(valueName);
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "hex(7):{0}", new object[]
				{
					OfflineRegUtils.ConvertByteArrayToRegStrings(byteValue)
				});
				string[] multiStringValue = key.GetMultiStringValue(valueName);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(this.GetMultiStringValuesAsComments(multiStringValue));
				goto IL_175;
			}
			}
			byte[] byteValue2 = key.GetByteValue(valueName);
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "hex({0,1:X}):{1}", new object[]
			{
				valueKind,
				OfflineRegUtils.ConvertByteArrayToRegStrings(byteValue2)
			});
			if (valueKind == RegistryValueType.ExpandString)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(this.GetExpandStringValueAsComments(key.GetStringValue(valueName)));
			}
			IL_175:
			return stringBuilder.ToString();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000758C File Offset: 0x0000578C
		private string GetMultiStringValuesAsComments(string[] values)
		{
			StringBuilder stringBuilder = new StringBuilder(500);
			int num = 80;
			if (values != null && values.Length > 0)
			{
				stringBuilder.Append(";Values=");
				int num2 = stringBuilder.Length;
				foreach (string text in values)
				{
					stringBuilder.AppendFormat("{0},", text);
					num2 += text.Length + 1;
					if (num2 > num)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append(";");
						num2 = 1;
					}
				}
				stringBuilder.Replace(",", string.Empty, stringBuilder.Length - 1, 1);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00007633 File Offset: 0x00005833
		private string GetExpandStringValueAsComments(string value)
		{
			return string.Format(";Value={0}", value);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00007644 File Offset: 0x00005844
		private void WriteKeyContents(ORRegistryKey key)
		{
			this.WriteKeyName(key.FullName);
			string @class = key.Class;
			if (!string.IsNullOrEmpty(@class))
			{
				this.m_writer.WriteLine(string.Format(";Class=\"{0}\"", @class));
			}
			string[] valueNames = key.ValueNames;
			foreach (string valueName in valueNames.OrderBy((string x) => x, StringComparer.OrdinalIgnoreCase))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.FormatValueName(valueName));
				stringBuilder.Append(this.FormatValue(key, valueName));
				this.m_writer.WriteLine(stringBuilder.ToString());
			}
		}

		// Token: 0x0400005B RID: 91
		private HashSet<string> m_exclusions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400005C RID: 92
		private string m_keyPrefix;

		// Token: 0x0400005D RID: 93
		private string m_hiveFile;

		// Token: 0x0400005E RID: 94
		private TextWriter m_writer;
	}
}
