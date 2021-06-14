using System;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x0200036E RID: 878
	internal sealed class StringSorter
	{
		// Token: 0x06003718 RID: 14104 RVA: 0x000F98E4 File Offset: 0x000F7AE4
		private StringSorter(CultureInfo culture, string[] keys, object[] items, int options)
		{
			if (keys == null)
			{
				if (items is string[])
				{
					keys = (string[])items;
					items = null;
				}
				else
				{
					keys = new string[items.Length];
					for (int i = 0; i < items.Length; i++)
					{
						object obj = items[i];
						if (obj != null)
						{
							keys[i] = obj.ToString();
						}
					}
				}
			}
			this.keys = keys;
			this.items = items;
			this.lcid = ((culture == null) ? SafeNativeMethods.GetThreadLocale() : culture.LCID);
			this.options = (options & 200711);
			this.descending = ((options & int.MinValue) != 0);
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x000F997A File Offset: 0x000F7B7A
		internal static int ArrayLength(object[] array)
		{
			if (array == null)
			{
				return 0;
			}
			return array.Length;
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x000F9984 File Offset: 0x000F7B84
		public static int Compare(string s1, string s2)
		{
			return StringSorter.Compare(SafeNativeMethods.GetThreadLocale(), s1, s2, 0);
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x000F9993 File Offset: 0x000F7B93
		public static int Compare(string s1, string s2, int options)
		{
			return StringSorter.Compare(SafeNativeMethods.GetThreadLocale(), s1, s2, options);
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x000F99A2 File Offset: 0x000F7BA2
		public static int Compare(CultureInfo culture, string s1, string s2, int options)
		{
			return StringSorter.Compare(culture.LCID, s1, s2, options);
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x000F99B2 File Offset: 0x000F7BB2
		private static int Compare(int lcid, string s1, string s2, int options)
		{
			if (s1 == null)
			{
				if (s2 != null)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (s2 == null)
				{
					return 1;
				}
				return string.Compare(s1, s2, false, CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x000F99D0 File Offset: 0x000F7BD0
		private int CompareKeys(string s1, string s2)
		{
			int num = StringSorter.Compare(this.lcid, s1, s2, this.options);
			if (!this.descending)
			{
				return num;
			}
			return -num;
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x000F9A00 File Offset: 0x000F7C00
		private void QuickSort(int left, int right)
		{
			do
			{
				int num = left;
				int num2 = right;
				string text = this.keys[num + num2 >> 1];
				for (;;)
				{
					if (this.CompareKeys(this.keys[num], text) >= 0)
					{
						while (this.CompareKeys(text, this.keys[num2]) < 0)
						{
							num2--;
						}
						if (num > num2)
						{
							break;
						}
						if (num < num2)
						{
							string text2 = this.keys[num];
							this.keys[num] = this.keys[num2];
							this.keys[num2] = text2;
							if (this.items != null)
							{
								object obj = this.items[num];
								this.items[num] = this.items[num2];
								this.items[num2] = obj;
							}
						}
						num++;
						num2--;
						if (num > num2)
						{
							break;
						}
					}
					else
					{
						num++;
					}
				}
				if (num2 - left <= right - num)
				{
					if (left < num2)
					{
						this.QuickSort(left, num2);
					}
					left = num;
				}
				else
				{
					if (num < right)
					{
						this.QuickSort(num, right);
					}
					right = num2;
				}
			}
			while (left < right);
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x000F9AE2 File Offset: 0x000F7CE2
		public static void Sort(object[] items)
		{
			StringSorter.Sort(null, null, items, 0, StringSorter.ArrayLength(items), 0);
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x000F9AF4 File Offset: 0x000F7CF4
		public static void Sort(object[] items, int index, int count)
		{
			StringSorter.Sort(null, null, items, index, count, 0);
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x000F9B01 File Offset: 0x000F7D01
		public static void Sort(string[] keys, object[] items)
		{
			StringSorter.Sort(null, keys, items, 0, StringSorter.ArrayLength(items), 0);
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x000F9B13 File Offset: 0x000F7D13
		public static void Sort(string[] keys, object[] items, int index, int count)
		{
			StringSorter.Sort(null, keys, items, index, count, 0);
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x000F9B20 File Offset: 0x000F7D20
		public static void Sort(object[] items, int options)
		{
			StringSorter.Sort(null, null, items, 0, StringSorter.ArrayLength(items), options);
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x000F9B32 File Offset: 0x000F7D32
		public static void Sort(object[] items, int index, int count, int options)
		{
			StringSorter.Sort(null, null, items, index, count, options);
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x000F9B3F File Offset: 0x000F7D3F
		public static void Sort(string[] keys, object[] items, int options)
		{
			StringSorter.Sort(null, keys, items, 0, StringSorter.ArrayLength(items), options);
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x000F9B51 File Offset: 0x000F7D51
		public static void Sort(string[] keys, object[] items, int index, int count, int options)
		{
			StringSorter.Sort(null, keys, items, index, count, options);
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x000F9B5F File Offset: 0x000F7D5F
		public static void Sort(CultureInfo culture, object[] items, int options)
		{
			StringSorter.Sort(culture, null, items, 0, StringSorter.ArrayLength(items), options);
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x000F9B71 File Offset: 0x000F7D71
		public static void Sort(CultureInfo culture, object[] items, int index, int count, int options)
		{
			StringSorter.Sort(culture, null, items, index, count, options);
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x000F9B7F File Offset: 0x000F7D7F
		public static void Sort(CultureInfo culture, string[] keys, object[] items, int options)
		{
			StringSorter.Sort(culture, keys, items, 0, StringSorter.ArrayLength(items), options);
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x000F9B94 File Offset: 0x000F7D94
		public static void Sort(CultureInfo culture, string[] keys, object[] items, int index, int count, int options)
		{
			if (items == null || (keys != null && keys.Length != items.Length))
			{
				throw new ArgumentException(SR.GetString("ArraysNotSameSize", new object[]
				{
					"keys",
					"items"
				}));
			}
			if (count > 1)
			{
				StringSorter stringSorter = new StringSorter(culture, keys, items, options);
				stringSorter.QuickSort(index, index + count - 1);
			}
		}

		// Token: 0x040021F1 RID: 8689
		public const int IgnoreCase = 1;

		// Token: 0x040021F2 RID: 8690
		public const int IgnoreKanaType = 65536;

		// Token: 0x040021F3 RID: 8691
		public const int IgnoreNonSpace = 2;

		// Token: 0x040021F4 RID: 8692
		public const int IgnoreSymbols = 4;

		// Token: 0x040021F5 RID: 8693
		public const int IgnoreWidth = 131072;

		// Token: 0x040021F6 RID: 8694
		public const int StringSort = 4096;

		// Token: 0x040021F7 RID: 8695
		public const int Descending = -2147483648;

		// Token: 0x040021F8 RID: 8696
		private const int CompareOptions = 200711;

		// Token: 0x040021F9 RID: 8697
		private string[] keys;

		// Token: 0x040021FA RID: 8698
		private object[] items;

		// Token: 0x040021FB RID: 8699
		private int lcid;

		// Token: 0x040021FC RID: 8700
		private int options;

		// Token: 0x040021FD RID: 8701
		private bool descending;
	}
}
