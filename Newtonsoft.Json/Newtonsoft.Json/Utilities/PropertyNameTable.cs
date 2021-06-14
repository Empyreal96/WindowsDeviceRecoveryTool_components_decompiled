using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000ED RID: 237
	internal class PropertyNameTable
	{
		// Token: 0x06000B3C RID: 2876 RVA: 0x0002D744 File Offset: 0x0002B944
		public PropertyNameTable()
		{
			this._entries = new PropertyNameTable.Entry[this._mask + 1];
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0002D768 File Offset: 0x0002B968
		public string Get(char[] key, int start, int length)
		{
			if (length == 0)
			{
				return string.Empty;
			}
			int num = length + PropertyNameTable.HashCodeRandomizer;
			num += (num << 7 ^ (int)key[start]);
			int num2 = start + length;
			for (int i = start + 1; i < num2; i++)
			{
				num += (num << 7 ^ (int)key[i]);
			}
			num -= num >> 17;
			num -= num >> 11;
			num -= num >> 5;
			for (PropertyNameTable.Entry entry = this._entries[num & this._mask]; entry != null; entry = entry.Next)
			{
				if (entry.HashCode == num && PropertyNameTable.TextEquals(entry.Value, key, start, length))
				{
					return entry.Value;
				}
			}
			return null;
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0002D800 File Offset: 0x0002BA00
		public string Add(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			int length = key.Length;
			if (length == 0)
			{
				return string.Empty;
			}
			int num = length + PropertyNameTable.HashCodeRandomizer;
			for (int i = 0; i < key.Length; i++)
			{
				num += (num << 7 ^ (int)key[i]);
			}
			num -= num >> 17;
			num -= num >> 11;
			num -= num >> 5;
			for (PropertyNameTable.Entry entry = this._entries[num & this._mask]; entry != null; entry = entry.Next)
			{
				if (entry.HashCode == num && entry.Value.Equals(key))
				{
					return entry.Value;
				}
			}
			return this.AddEntry(key, num);
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0002D8A8 File Offset: 0x0002BAA8
		private string AddEntry(string str, int hashCode)
		{
			int num = hashCode & this._mask;
			PropertyNameTable.Entry entry = new PropertyNameTable.Entry(str, hashCode, this._entries[num]);
			this._entries[num] = entry;
			if (this._count++ == this._mask)
			{
				this.Grow();
			}
			return entry.Value;
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0002D8FC File Offset: 0x0002BAFC
		private void Grow()
		{
			PropertyNameTable.Entry[] entries = this._entries;
			int num = this._mask * 2 + 1;
			PropertyNameTable.Entry[] array = new PropertyNameTable.Entry[num + 1];
			foreach (PropertyNameTable.Entry entry in entries)
			{
				while (entry != null)
				{
					int num2 = entry.HashCode & num;
					PropertyNameTable.Entry next = entry.Next;
					entry.Next = array[num2];
					array[num2] = entry;
					entry = next;
				}
			}
			this._entries = array;
			this._mask = num;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0002D974 File Offset: 0x0002BB74
		private static bool TextEquals(string str1, char[] str2, int str2Start, int str2Length)
		{
			if (str1.Length != str2Length)
			{
				return false;
			}
			for (int i = 0; i < str1.Length; i++)
			{
				if (str1[i] != str2[str2Start + i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000409 RID: 1033
		private static readonly int HashCodeRandomizer = Environment.TickCount;

		// Token: 0x0400040A RID: 1034
		private int _count;

		// Token: 0x0400040B RID: 1035
		private PropertyNameTable.Entry[] _entries;

		// Token: 0x0400040C RID: 1036
		private int _mask = 31;

		// Token: 0x020000EE RID: 238
		private class Entry
		{
			// Token: 0x06000B42 RID: 2882 RVA: 0x0002D9AE File Offset: 0x0002BBAE
			internal Entry(string value, int hashCode, PropertyNameTable.Entry next)
			{
				this.Value = value;
				this.HashCode = hashCode;
				this.Next = next;
			}

			// Token: 0x0400040D RID: 1037
			internal readonly string Value;

			// Token: 0x0400040E RID: 1038
			internal readonly int HashCode;

			// Token: 0x0400040F RID: 1039
			internal PropertyNameTable.Entry Next;
		}
	}
}
