using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200001A RID: 26
	public class ORRegistryKey : IDisposable
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00006000 File Offset: 0x00004200
		private ORRegistryKey(string name, IntPtr handle, bool isRoot, ORRegistryKey parent)
		{
			this.m_name = name;
			this.m_handle = handle;
			this.m_isRoot = isRoot;
			this.m_parent = parent;
			if (this.m_parent != null)
			{
				this.m_parent.m_children[this] = true;
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00006080 File Offset: 0x00004280
		~ORRegistryKey()
		{
			this.Dispose(false);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000060B0 File Offset: 0x000042B0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000060C0 File Offset: 0x000042C0
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (ORRegistryKey orregistryKey in this.m_children.Keys)
				{
					orregistryKey.Close();
				}
				this.m_children.Clear();
				if (this.m_parent != null)
				{
					this.m_parent.m_children.Remove(this);
				}
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00006140 File Offset: 0x00004340
		public static ORRegistryKey OpenHive(string hivefile, string prefix = null)
		{
			if (prefix == null)
			{
				prefix = "\\";
			}
			return new ORRegistryKey(prefix, OfflineRegUtils.OpenHive(hivefile), true, null);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000615A File Offset: 0x0000435A
		public static ORRegistryKey CreateEmptyHive(string prefix = null)
		{
			return new ORRegistryKey(string.IsNullOrEmpty(prefix) ? "\\" : prefix, OfflineRegUtils.CreateHive(), true, null);
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00006178 File Offset: 0x00004378
		public ORRegistryKey Parent
		{
			get
			{
				return this.m_parent;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00006180 File Offset: 0x00004380
		public string[] SubKeys
		{
			get
			{
				return OfflineRegUtils.GetSubKeys(this.m_handle);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000618D File Offset: 0x0000438D
		public string FullName
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00006195 File Offset: 0x00004395
		public string Class
		{
			get
			{
				return OfflineRegUtils.GetClass(this.m_handle);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000061A2 File Offset: 0x000043A2
		public string[] ValueNames
		{
			get
			{
				return OfflineRegUtils.GetValueNames(this.m_handle);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000061AF File Offset: 0x000043AF
		public List<KeyValuePair<string, RegistryValueType>> ValueNameAndTypes
		{
			get
			{
				return OfflineRegUtils.GetValueNamesAndTypes(this.m_handle);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000061BC File Offset: 0x000043BC
		public RegistrySecurity RegistrySecurity
		{
			get
			{
				return OfflineRegUtils.GetRegistrySecurity(this.m_handle);
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000061CC File Offset: 0x000043CC
		public ORRegistryKey OpenSubKey(string subkeyname)
		{
			int num = subkeyname.IndexOf("\\");
			ORRegistryKey result;
			if (-1 < num)
			{
				string[] array = subkeyname.Split(this.BSLASH_DELIMITER);
				ORRegistryKey orregistryKey = this;
				ORRegistryKey orregistryKey2 = null;
				foreach (string subkeyname2 in array)
				{
					orregistryKey2 = orregistryKey.OpenSubKey(subkeyname2);
					orregistryKey = orregistryKey2;
				}
				result = orregistryKey2;
			}
			else
			{
				IntPtr handle = OfflineRegUtils.OpenKey(this.m_handle, subkeyname);
				string name = this.CombineSubKeys(this.m_name, subkeyname);
				result = new ORRegistryKey(name, handle, false, this);
			}
			return result;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00006256 File Offset: 0x00004456
		public RegistryValueType GetValueKind(string valueName)
		{
			return OfflineRegUtils.GetValueType(this.m_handle, valueName);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00006264 File Offset: 0x00004464
		public byte[] GetByteValue(string valueName)
		{
			return OfflineRegUtils.GetValue(this.m_handle, valueName);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00006274 File Offset: 0x00004474
		[CLSCompliant(false)]
		public uint GetDwordValue(string valueName)
		{
			byte[] byteValue = this.GetByteValue(valueName);
			if (byteValue.Length != 0)
			{
				return BitConverter.ToUInt32(byteValue, 0);
			}
			return 0U;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00006298 File Offset: 0x00004498
		[CLSCompliant(false)]
		public ulong GetQwordValue(string valueName)
		{
			byte[] byteValue = this.GetByteValue(valueName);
			if (byteValue.Length != 0)
			{
				return BitConverter.ToUInt64(byteValue, 0);
			}
			return 0UL;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000062BC File Offset: 0x000044BC
		public string GetStringValue(string valueName)
		{
			byte[] byteValue = this.GetByteValue(valueName);
			string result = string.Empty;
			if (byteValue.Length > 1 && byteValue[byteValue.Length - 1] == 0 && byteValue[byteValue.Length - 2] == 0)
			{
				result = Encoding.Unicode.GetString(byteValue, 0, byteValue.Length - 2);
			}
			else
			{
				result = Encoding.Unicode.GetString(byteValue);
			}
			return result;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00006310 File Offset: 0x00004510
		public string[] GetMultiStringValue(string valueName)
		{
			byte[] byteValue = this.GetByteValue(valueName);
			string @string = Encoding.Unicode.GetString(byteValue);
			char[] separator = new char[1];
			return @string.Split(separator, StringSplitOptions.RemoveEmptyEntries);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006340 File Offset: 0x00004540
		public object GetValue(string valueName)
		{
			RegistryValueType valueKind = this.GetValueKind(valueName);
			object result = null;
			switch (valueKind)
			{
			case RegistryValueType.None:
				result = this.GetByteValue(valueName);
				break;
			case RegistryValueType.String:
				result = this.GetStringValue(valueName);
				break;
			case RegistryValueType.ExpandString:
				result = this.GetStringValue(valueName);
				break;
			case RegistryValueType.Binary:
				result = this.GetByteValue(valueName);
				break;
			case RegistryValueType.DWord:
				result = this.GetDwordValue(valueName);
				break;
			case RegistryValueType.DWordBigEndian:
				result = this.GetByteValue(valueName);
				break;
			case RegistryValueType.Link:
				result = this.GetByteValue(valueName);
				break;
			case RegistryValueType.MultiString:
				result = this.GetMultiStringValue(valueName);
				break;
			case RegistryValueType.RegResourceList:
				result = this.GetByteValue(valueName);
				break;
			case RegistryValueType.RegFullResourceDescriptor:
				result = this.GetByteValue(valueName);
				break;
			case RegistryValueType.RegResourceRequirementsList:
				result = this.GetByteValue(valueName);
				break;
			case RegistryValueType.QWord:
				result = this.GetQwordValue(valueName);
				break;
			}
			return result;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006415 File Offset: 0x00004615
		public void SaveHive(string path)
		{
			if (!this.m_isRoot)
			{
				throw new IUException("Invalid operation - This registry key does not represent hive root");
			}
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}
			OfflineRegUtils.SaveHive(this.m_handle, path, 6, 3);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000644C File Offset: 0x0000464C
		public ORRegistryKey CreateSubKey(string subkeyName)
		{
			int num = subkeyName.IndexOf("\\");
			ORRegistryKey result;
			if (-1 != num)
			{
				string[] array = subkeyName.Split(this.BSLASH_DELIMITER, StringSplitOptions.RemoveEmptyEntries);
				ORRegistryKey orregistryKey = this;
				ORRegistryKey orregistryKey2 = null;
				foreach (string subkeyName2 in array)
				{
					orregistryKey2 = orregistryKey.CreateSubKey(subkeyName2);
					orregistryKey = orregistryKey2;
				}
				result = orregistryKey2;
			}
			else
			{
				IntPtr handle = OfflineRegUtils.CreateKey(this.m_handle, subkeyName);
				string name = this.CombineSubKeys(this.m_name, subkeyName);
				result = new ORRegistryKey(name, handle, false, this);
			}
			return result;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000064D7 File Offset: 0x000046D7
		public void SetValue(string valueName, byte[] value)
		{
			OfflineRegUtils.SetValue(this.m_handle, valueName, RegistryValueType.Binary, value);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000064E8 File Offset: 0x000046E8
		public void SetValue(string valueName, string value)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(value);
			OfflineRegUtils.SetValue(this.m_handle, valueName, RegistryValueType.String, bytes);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00006510 File Offset: 0x00004710
		public void SetValue(string valueName, string[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			StringBuilder stringBuilder = new StringBuilder(1024);
			foreach (string arg in values)
			{
				stringBuilder.AppendFormat("{0}{1}", arg, "\0");
			}
			stringBuilder.Append("\0");
			byte[] bytes = Encoding.Unicode.GetBytes(stringBuilder.ToString());
			OfflineRegUtils.SetValue(this.m_handle, valueName, RegistryValueType.MultiString, bytes);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000658C File Offset: 0x0000478C
		public void SetValue(string valueName, int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			OfflineRegUtils.SetValue(this.m_handle, valueName, RegistryValueType.DWord, bytes);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000065B0 File Offset: 0x000047B0
		public void SetValue(string valueName, long value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			OfflineRegUtils.SetValue(this.m_handle, valueName, RegistryValueType.QWord, bytes);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000065D3 File Offset: 0x000047D3
		public void DeleteValue(string valueName)
		{
			OfflineRegUtils.DeleteValue(this.m_handle, valueName);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000065E1 File Offset: 0x000047E1
		public void DeleteKey(string keyName)
		{
			OfflineRegUtils.DeleteKey(this.m_handle, keyName);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000065F0 File Offset: 0x000047F0
		private string CombineSubKeys(string path1, string path2)
		{
			if (path1 == null)
			{
				throw new ArgumentNullException("first");
			}
			if (path2 == null)
			{
				throw new ArgumentNullException("second");
			}
			if (-1 < path2.IndexOf("\\") || path1.Length == 0)
			{
				return path2;
			}
			if (path2.Length == 0)
			{
				return path1;
			}
			if (path1.Length == path1.LastIndexOfAny(this.BSLASH_DELIMITER) + 1)
			{
				return path1 + path2;
			}
			return path1 + this.BSLASH_DELIMITER[0] + path2;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0000666D File Offset: 0x0000486D
		private void Close()
		{
			if (this.m_handle != IntPtr.Zero)
			{
				if (this.m_isRoot)
				{
					OfflineRegUtils.CloseHive(this.m_handle);
				}
				else
				{
					OfflineRegUtils.CloseKey(this.m_handle);
				}
				this.m_handle = IntPtr.Zero;
			}
		}

		// Token: 0x0400004F RID: 79
		private const string STR_ROOT = "\\";

		// Token: 0x04000050 RID: 80
		private const string STR_NULLCHAR = "\0";

		// Token: 0x04000051 RID: 81
		private IntPtr m_handle = IntPtr.Zero;

		// Token: 0x04000052 RID: 82
		private string m_name = string.Empty;

		// Token: 0x04000053 RID: 83
		private bool m_isRoot;

		// Token: 0x04000054 RID: 84
		private ORRegistryKey m_parent;

		// Token: 0x04000055 RID: 85
		private readonly char[] BSLASH_DELIMITER = new char[]
		{
			'\\'
		};

		// Token: 0x04000056 RID: 86
		private Dictionary<ORRegistryKey, bool> m_children = new Dictionary<ORRegistryKey, bool>();
	}
}
