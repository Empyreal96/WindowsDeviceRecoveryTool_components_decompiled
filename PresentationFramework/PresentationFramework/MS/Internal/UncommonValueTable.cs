using System;
using System.Windows;

namespace MS.Internal
{
	// Token: 0x020005F2 RID: 1522
	internal struct UncommonValueTable
	{
		// Token: 0x0600654D RID: 25933 RVA: 0x001C6D30 File Offset: 0x001C4F30
		public bool HasValue(int id)
		{
			return (this._bitmask & 1U << id) > 0U;
		}

		// Token: 0x0600654E RID: 25934 RVA: 0x001C6D42 File Offset: 0x001C4F42
		public object GetValue(int id)
		{
			return this.GetValue(id, DependencyProperty.UnsetValue);
		}

		// Token: 0x0600654F RID: 25935 RVA: 0x001C6D50 File Offset: 0x001C4F50
		public object GetValue(int id, object defaultValue)
		{
			int num = this.IndexOf(id);
			if (num >= 0)
			{
				return this._table[num];
			}
			return defaultValue;
		}

		// Token: 0x06006550 RID: 25936 RVA: 0x001C6D74 File Offset: 0x001C4F74
		public void SetValue(int id, object value)
		{
			int num = this.Find(id);
			if (num < 0)
			{
				if (this._table == null)
				{
					this._table = new object[1];
					num = 0;
				}
				else
				{
					int num2 = this._table.Length;
					object[] array = new object[num2 + 1];
					num = ~num;
					Array.Copy(this._table, 0, array, 0, num);
					Array.Copy(this._table, num, array, num + 1, num2 - num);
					this._table = array;
				}
				this._bitmask |= 1U << id;
			}
			this._table[num] = value;
		}

		// Token: 0x06006551 RID: 25937 RVA: 0x001C6E00 File Offset: 0x001C5000
		public void ClearValue(int id)
		{
			int num = this.Find(id);
			if (num >= 0)
			{
				int num2 = this._table.Length - 1;
				if (num2 == 0)
				{
					this._table = null;
				}
				else
				{
					object[] array = new object[num2];
					Array.Copy(this._table, 0, array, 0, num);
					Array.Copy(this._table, num + 1, array, num, num2 - num);
					this._table = array;
				}
				this._bitmask &= ~(1U << id);
			}
		}

		// Token: 0x06006552 RID: 25938 RVA: 0x001C6E74 File Offset: 0x001C5074
		private int IndexOf(int id)
		{
			if (!this.HasValue(id))
			{
				return -1;
			}
			return this.GetIndex(id);
		}

		// Token: 0x06006553 RID: 25939 RVA: 0x001C6E88 File Offset: 0x001C5088
		private int Find(int id)
		{
			int num = this.GetIndex(id);
			if (!this.HasValue(id))
			{
				num = ~num;
			}
			return num;
		}

		// Token: 0x06006554 RID: 25940 RVA: 0x001C6EAC File Offset: 0x001C50AC
		private int GetIndex(int id)
		{
			uint num = this._bitmask << 31 - id << 1;
			num -= (num >> 1 & 1431655765U);
			num = (num & 858993459U) + (num >> 2 & 858993459U);
			num = (num + (num >> 4) & 252645135U);
			return (int)(num * 16843009U >> 24);
		}

		// Token: 0x040032BE RID: 12990
		private object[] _table;

		// Token: 0x040032BF RID: 12991
		private uint _bitmask;
	}
}
