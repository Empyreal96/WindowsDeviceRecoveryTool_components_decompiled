using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000742 RID: 1858
	internal class SortFieldComparer : IComparer
	{
		// Token: 0x060076D1 RID: 30417 RVA: 0x0021F68C File Offset: 0x0021D88C
		internal SortFieldComparer(SortDescriptionCollection sortFields, CultureInfo culture)
		{
			this._sortFields = sortFields;
			this._fields = this.CreatePropertyInfo(this._sortFields);
			this._comparer = ((culture == null || culture == CultureInfo.InvariantCulture) ? Comparer.DefaultInvariant : ((culture == CultureInfo.CurrentCulture) ? Comparer.Default : new Comparer(culture)));
		}

		// Token: 0x17001C3F RID: 7231
		// (get) Token: 0x060076D2 RID: 30418 RVA: 0x0021F6E5 File Offset: 0x0021D8E5
		internal IComparer BaseComparer
		{
			get
			{
				return this._comparer;
			}
		}

		// Token: 0x060076D3 RID: 30419 RVA: 0x0021F6F0 File Offset: 0x0021D8F0
		public int Compare(object o1, object o2)
		{
			int num = 0;
			for (int i = 0; i < this._fields.Length; i++)
			{
				object value = this._fields[i].GetValue(o1);
				object value2 = this._fields[i].GetValue(o2);
				num = this._comparer.Compare(value, value2);
				if (this._fields[i].descending)
				{
					num = -num;
				}
				if (num != 0)
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x060076D4 RID: 30420 RVA: 0x0021F760 File Offset: 0x0021D960
		internal static void SortHelper(ArrayList al, IComparer comparer)
		{
			SortFieldComparer sortFieldComparer = comparer as SortFieldComparer;
			if (sortFieldComparer == null)
			{
				al.Sort(comparer);
				return;
			}
			int count = al.Count;
			int nFields = sortFieldComparer._fields.Length;
			SortFieldComparer.CachedValueItem[] array = new SortFieldComparer.CachedValueItem[count];
			for (int i = 0; i < count; i++)
			{
				array[i].Initialize(al[i], nFields);
			}
			Array.Sort(array, sortFieldComparer);
			for (int j = 0; j < count; j++)
			{
				al[j] = array[j].OriginalItem;
			}
		}

		// Token: 0x060076D5 RID: 30421 RVA: 0x0021F7E8 File Offset: 0x0021D9E8
		private SortFieldComparer.SortPropertyInfo[] CreatePropertyInfo(SortDescriptionCollection sortFields)
		{
			SortFieldComparer.SortPropertyInfo[] array = new SortFieldComparer.SortPropertyInfo[sortFields.Count];
			for (int i = 0; i < sortFields.Count; i++)
			{
				PropertyPath info;
				if (string.IsNullOrEmpty(sortFields[i].PropertyName))
				{
					info = null;
				}
				else
				{
					info = new PropertyPath(sortFields[i].PropertyName, new object[0]);
				}
				array[i].index = i;
				array[i].info = info;
				array[i].descending = (sortFields[i].Direction == ListSortDirection.Descending);
			}
			return array;
		}

		// Token: 0x04003895 RID: 14485
		private SortFieldComparer.SortPropertyInfo[] _fields;

		// Token: 0x04003896 RID: 14486
		private SortDescriptionCollection _sortFields;

		// Token: 0x04003897 RID: 14487
		private Comparer _comparer;

		// Token: 0x02000B60 RID: 2912
		private struct SortPropertyInfo
		{
			// Token: 0x06008DE1 RID: 36321 RVA: 0x0025A65F File Offset: 0x0025885F
			internal object GetValue(object o)
			{
				if (o is SortFieldComparer.CachedValueItem)
				{
					return this.GetValueFromCVI((SortFieldComparer.CachedValueItem)o);
				}
				return this.GetValueCore(o);
			}

			// Token: 0x06008DE2 RID: 36322 RVA: 0x0025A680 File Offset: 0x00258880
			private object GetValueFromCVI(SortFieldComparer.CachedValueItem cvi)
			{
				object obj = cvi[this.index];
				if (obj == DependencyProperty.UnsetValue)
				{
					obj = (cvi[this.index] = this.GetValueCore(cvi.OriginalItem));
				}
				return obj;
			}

			// Token: 0x06008DE3 RID: 36323 RVA: 0x0025A6C4 File Offset: 0x002588C4
			private object GetValueCore(object o)
			{
				object obj;
				if (this.info == null)
				{
					obj = o;
				}
				else
				{
					using (this.info.SetContext(o))
					{
						obj = this.info.GetValue();
					}
				}
				if (obj == DependencyProperty.UnsetValue || BindingExpressionBase.IsNullValue(obj))
				{
					obj = null;
				}
				return obj;
			}

			// Token: 0x04004B1C RID: 19228
			internal int index;

			// Token: 0x04004B1D RID: 19229
			internal PropertyPath info;

			// Token: 0x04004B1E RID: 19230
			internal bool descending;
		}

		// Token: 0x02000B61 RID: 2913
		private struct CachedValueItem
		{
			// Token: 0x17001F89 RID: 8073
			// (get) Token: 0x06008DE4 RID: 36324 RVA: 0x0025A728 File Offset: 0x00258928
			public object OriginalItem
			{
				get
				{
					return this._item;
				}
			}

			// Token: 0x06008DE5 RID: 36325 RVA: 0x0025A730 File Offset: 0x00258930
			public void Initialize(object item, int nFields)
			{
				this._item = item;
				this._values = new object[nFields];
				this._values[0] = DependencyProperty.UnsetValue;
			}

			// Token: 0x17001F8A RID: 8074
			public object this[int index]
			{
				get
				{
					return this._values[index];
				}
				set
				{
					this._values[index] = value;
					if (++index < this._values.Length)
					{
						this._values[index] = DependencyProperty.UnsetValue;
					}
				}
			}

			// Token: 0x04004B1F RID: 19231
			private object _item;

			// Token: 0x04004B20 RID: 19232
			private object[] _values;
		}
	}
}
