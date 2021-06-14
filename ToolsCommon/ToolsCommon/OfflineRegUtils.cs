using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200001B RID: 27
	public class OfflineRegUtils
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x000066AC File Offset: 0x000048AC
		public static IntPtr CreateHive()
		{
			IntPtr zero = IntPtr.Zero;
			int num = OffRegNativeMethods.ORCreateHive(ref zero);
			if (num != 0)
			{
				throw new Win32Exception(num);
			}
			return zero;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000066D4 File Offset: 0x000048D4
		public static IntPtr CreateKey(IntPtr handle, string keyName)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			if (string.IsNullOrEmpty("keyName"))
			{
				throw new ArgumentNullException("keyName");
			}
			IntPtr zero = IntPtr.Zero;
			uint num = 0U;
			string[] array = keyName.Split(OfflineRegUtils.BSLASH_DELIMITER);
			foreach (string text in array)
			{
				int num2 = OffRegNativeMethods.ORCreateKey(handle, keyName, null, 0U, null, ref zero, ref num);
				if (num2 != 0)
				{
					throw new Win32Exception(num2);
				}
			}
			return zero;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000675C File Offset: 0x0000495C
		public static void SetValue(IntPtr handle, string valueName, RegistryValueType type, byte[] value)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			if (valueName == null)
			{
				valueName = string.Empty;
			}
			int num = OffRegNativeMethods.ORSetValue(handle, valueName, (uint)type, value, (uint)value.Length);
			if (num != 0)
			{
				throw new Win32Exception(num);
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000067A4 File Offset: 0x000049A4
		public static void DeleteValue(IntPtr handle, string valueName)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			if (valueName == null)
			{
				valueName = string.Empty;
			}
			int num = OffRegNativeMethods.ORDeleteValue(handle, valueName);
			if (num != 0)
			{
				throw new Win32Exception(num);
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000067E8 File Offset: 0x000049E8
		public static void DeleteKey(IntPtr handle, string keyName)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			int num = OffRegNativeMethods.ORDeleteKey(handle, keyName);
			if (num != 0)
			{
				throw new Win32Exception(num);
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006820 File Offset: 0x00004A20
		public static IntPtr OpenHive(string hivefile)
		{
			if (string.IsNullOrEmpty(hivefile))
			{
				throw new ArgumentNullException("hivefile");
			}
			IntPtr zero = IntPtr.Zero;
			int num = OffRegNativeMethods.OROpenHive(hivefile, ref zero);
			if (num != 0)
			{
				throw new Win32Exception(num);
			}
			return zero;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000685C File Offset: 0x00004A5C
		public static void SaveHive(IntPtr handle, string path, int osMajor, int osMinor)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}
			if (File.Exists(path))
			{
				FileUtils.DeleteFile(path);
			}
			int num = OffRegNativeMethods.ORSaveHive(handle, path, osMajor, osMinor);
			if (num != 0)
			{
				throw new Win32Exception(num);
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000068B8 File Offset: 0x00004AB8
		public static void CloseHive(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			int num = OffRegNativeMethods.ORCloseHive(handle);
			if (num != 0)
			{
				throw new Win32Exception(num);
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000068F0 File Offset: 0x00004AF0
		public static IntPtr OpenKey(IntPtr handle, string subKeyName)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			if (string.IsNullOrEmpty("subKeyName"))
			{
				throw new ArgumentNullException("subKeyName");
			}
			IntPtr zero = IntPtr.Zero;
			int num = OffRegNativeMethods.OROpenKey(handle, subKeyName, ref zero);
			if (num != 0)
			{
				throw new Win32Exception(num);
			}
			return zero;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00006948 File Offset: 0x00004B48
		public static void CloseKey(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			int num = OffRegNativeMethods.ORCloseKey(handle);
			if (num != 0)
			{
				throw new Win32Exception(num);
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000697E File Offset: 0x00004B7E
		public static void ConvertHiveToReg(string inputHiveFile, string outputRegFile)
		{
			new HiveToRegConverter(inputHiveFile, null).ConvertToReg(outputRegFile, null, false);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000698F File Offset: 0x00004B8F
		public static void ConvertHiveToReg(string inputHiveFile, string outputRegFile, string keyPrefix)
		{
			new HiveToRegConverter(inputHiveFile, keyPrefix).ConvertToReg(outputRegFile, null, false);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000069A0 File Offset: 0x00004BA0
		public static void ConvertHiveToReg(string inputHiveFile, string outputRegFile, string keyPrefix, bool appendExisting)
		{
			new HiveToRegConverter(inputHiveFile, keyPrefix).ConvertToReg(outputRegFile, null, appendExisting);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000069B1 File Offset: 0x00004BB1
		public static string ConvertByteArrayToRegStrings(byte[] data)
		{
			return OfflineRegUtils.ConvertByteArrayToRegStrings(data, 40);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000069BC File Offset: 0x00004BBC
		public static string ConvertByteArrayToRegStrings(byte[] data, int maxOnALine)
		{
			string result = string.Empty;
			if (-1 == maxOnALine)
			{
				result = BitConverter.ToString(data).Replace('-', ',');
			}
			else
			{
				int num = 0;
				int i = data.Length;
				StringBuilder stringBuilder = new StringBuilder();
				while (i > 0)
				{
					int num2 = (i > maxOnALine) ? maxOnALine : i;
					string text = BitConverter.ToString(data, num, num2);
					num += num2;
					i -= num2;
					text = text.Replace('-', ',');
					stringBuilder.Append(text);
					if (i > 0)
					{
						stringBuilder.Append(",\\");
						stringBuilder.AppendLine();
					}
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006A4C File Offset: 0x00004C4C
		public static RegistryValueType GetValueType(IntPtr handle, string valueName)
		{
			uint result = 0U;
			uint num = 0U;
			int num2 = OffRegNativeMethods.ORGetValue(handle, null, valueName, out result, null, ref num);
			if (num2 != 0)
			{
				throw new Win32Exception(num2);
			}
			return (RegistryValueType)result;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006A78 File Offset: 0x00004C78
		public static List<KeyValuePair<string, RegistryValueType>> GetValueNamesAndTypes(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			uint num = 0U;
			StringBuilder stringBuilder = new StringBuilder(1024);
			new List<string>();
			List<KeyValuePair<string, RegistryValueType>> list = new List<KeyValuePair<string, RegistryValueType>>();
			int num3;
			for (;;)
			{
				uint capacity = (uint)stringBuilder.Capacity;
				uint num2 = 0U;
				num3 = OffRegNativeMethods.OREnumValue(handle, num, stringBuilder, ref capacity, out num2, IntPtr.Zero, IntPtr.Zero);
				int num4 = num3;
				if (num4 != 0)
				{
					if (num4 != 259)
					{
						break;
					}
				}
				else
				{
					string key = stringBuilder.ToString();
					RegistryValueType value = (RegistryValueType)num2;
					list.Add(new KeyValuePair<string, RegistryValueType>(key, value));
					num += 1U;
				}
				if (num3 == 259)
				{
					return list;
				}
			}
			Win32Exception ex = new Win32Exception(num3);
			throw ex;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006B2C File Offset: 0x00004D2C
		public static string[] GetValueNames(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			return (from a in OfflineRegUtils.GetValueNamesAndTypes(handle)
			select a.Key).ToArray<string>();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006B80 File Offset: 0x00004D80
		public static string GetClass(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			StringBuilder stringBuilder = new StringBuilder(128);
			uint num = (uint)stringBuilder.Capacity;
			uint[] array = new uint[8];
			IntPtr zero = IntPtr.Zero;
			int num2 = OffRegNativeMethods.ORQueryInfoKey(handle, stringBuilder, ref num, out array[0], out array[1], out array[3], out array[4], out array[5], out array[6], out array[7], zero);
			if (num2 == 234)
			{
				num += 1U;
				stringBuilder.Capacity = (int)num;
				num2 = OffRegNativeMethods.ORQueryInfoKey(handle, stringBuilder, ref num, out array[0], out array[1], out array[3], out array[4], out array[5], out array[6], out array[7], zero);
			}
			if (num2 != 0)
			{
				throw new Win32Exception(num2);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006C64 File Offset: 0x00004E64
		public static byte[] GetValue(IntPtr handle, string valueName)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			uint num = 0U;
			uint num2 = 0U;
			int num3 = OffRegNativeMethods.ORGetValue(handle, null, valueName, out num, null, ref num2);
			if (num3 != 0)
			{
				throw new Win32Exception(num3);
			}
			byte[] array = new byte[num2];
			num3 = OffRegNativeMethods.ORGetValue(handle, null, valueName, out num, array, ref num2);
			if (num3 != 0)
			{
				throw new Win32Exception(num3);
			}
			return array;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006CC8 File Offset: 0x00004EC8
		public static string[] GetSubKeys(IntPtr registryKey)
		{
			if (registryKey == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			uint num = 0U;
			StringBuilder stringBuilder = new StringBuilder(1024);
			List<string> list = new List<string>();
			int num3;
			for (;;)
			{
				uint num2 = 0U;
				IntPtr zero = IntPtr.Zero;
				uint capacity = (uint)stringBuilder.Capacity;
				num3 = OffRegNativeMethods.OREnumKey(registryKey, num, stringBuilder, ref capacity, null, ref num2, ref zero);
				int num4 = num3;
				if (num4 != 0)
				{
					if (num4 != 259)
					{
						break;
					}
				}
				else
				{
					list.Add(stringBuilder.ToString());
					num += 1U;
				}
				if (num3 == 259)
				{
					goto Block_4;
				}
			}
			Win32Exception ex = new Win32Exception(num3);
			throw ex;
			Block_4:
			return list.ToArray();
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006D64 File Offset: 0x00004F64
		public static byte[] GetRawRegistrySecurity(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			uint num = 0U;
			int num2 = 234;
			int num3 = OffRegNativeMethods.ORGetKeySecurity(handle, (SecurityInformationFlags)28U, null, ref num);
			if (num2 != num3)
			{
				throw new Win32Exception(num3);
			}
			byte[] array = new byte[num];
			num3 = OffRegNativeMethods.ORGetKeySecurity(handle, (SecurityInformationFlags)28U, array, ref num);
			if (num3 != 0)
			{
				throw new Win32Exception(num3);
			}
			return array;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006DC8 File Offset: 0x00004FC8
		public static void SetRawRegistrySecurity(IntPtr handle, byte[] buf)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			int num = OffRegNativeMethods.ORSetKeySecurity(handle, (SecurityInformationFlags)28U, buf);
			if (num != 0)
			{
				throw new Win32Exception(num);
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00006E04 File Offset: 0x00005004
		public static RegistrySecurity GetRegistrySecurity(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			byte[] rawRegistrySecurity = OfflineRegUtils.GetRawRegistrySecurity(handle);
			SecurityUtils.ConvertSDToStringSD(rawRegistrySecurity, (SecurityInformationFlags)24U);
			RegistrySecurity registrySecurity = new RegistrySecurity();
			registrySecurity.SetSecurityDescriptorBinaryForm(rawRegistrySecurity);
			return registrySecurity;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006E4C File Offset: 0x0000504C
		public static int GetVirtualFlags(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("handle");
			}
			int result = 0;
			OffRegNativeMethods.ORGetVirtualFlags(handle, ref result);
			return result;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006E80 File Offset: 0x00005080
		public static int ExtractFromHive(string hivePath, RegistryValueType type, string targetPath)
		{
			if (string.IsNullOrEmpty("hivePath"))
			{
				throw new ArgumentNullException("hivePath");
			}
			if (string.IsNullOrEmpty("targetPath"))
			{
				throw new ArgumentNullException("targetPath");
			}
			if (!File.Exists(hivePath))
			{
				throw new FileNotFoundException("Hive file {0} does not exist", hivePath);
			}
			int result = 0;
			bool flag = false;
			using (ORRegistryKey orregistryKey = ORRegistryKey.OpenHive(hivePath, null))
			{
				using (ORRegistryKey orregistryKey2 = ORRegistryKey.CreateEmptyHive(null))
				{
					flag = (0 < (result = OfflineRegUtils.ExtractFromHiveRecursive(orregistryKey, type, orregistryKey2)));
					if (flag)
					{
						orregistryKey2.SaveHive(targetPath);
					}
				}
				if (flag)
				{
					orregistryKey.SaveHive(hivePath);
				}
			}
			return result;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006F54 File Offset: 0x00005154
		private static int ExtractFromHiveRecursive(ORRegistryKey srcHiveRoot, RegistryValueType type, ORRegistryKey dstHiveRoot)
		{
			int num = 0;
			string fullName = srcHiveRoot.FullName;
			List<KeyValuePair<string, RegistryValueType>> valueNameAndTypes = srcHiveRoot.ValueNameAndTypes;
			IEnumerable<string> enumerable = from p in valueNameAndTypes
			where p.Value == RegistryValueType.MultiString
			select p into q
			select q.Key;
			foreach (string text in enumerable)
			{
				string valueName = string.IsNullOrEmpty(text) ? null : text;
				string[] multiStringValue = srcHiveRoot.GetMultiStringValue(valueName);
				using (ORRegistryKey orregistryKey = dstHiveRoot.CreateSubKey(fullName))
				{
					orregistryKey.SetValue(valueName, multiStringValue);
					num++;
				}
				srcHiveRoot.DeleteValue(valueName);
			}
			foreach (string subkeyname in srcHiveRoot.SubKeys)
			{
				using (ORRegistryKey orregistryKey2 = srcHiveRoot.OpenSubKey(subkeyname))
				{
					num += OfflineRegUtils.ExtractFromHiveRecursive(orregistryKey2, type, dstHiveRoot);
				}
			}
			return num;
		}

		// Token: 0x04000057 RID: 87
		private static readonly char[] BSLASH_DELIMITER = new char[]
		{
			'\\'
		};
	}
}
