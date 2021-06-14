using System;
using System.Collections.Generic;

namespace System.Windows.Controls
{
	// Token: 0x020004AE RID: 1198
	internal class DataGridItemAttachedStorage
	{
		// Token: 0x06004902 RID: 18690 RVA: 0x0014B4D8 File Offset: 0x001496D8
		public void SetValue(object item, DependencyProperty property, object value)
		{
			Dictionary<DependencyProperty, object> dictionary = this.EnsureItem(item);
			dictionary[property] = value;
		}

		// Token: 0x06004903 RID: 18691 RVA: 0x0014B4F8 File Offset: 0x001496F8
		public bool TryGetValue(object item, DependencyProperty property, out object value)
		{
			value = null;
			this.EnsureItemStorageMap();
			Dictionary<DependencyProperty, object> dictionary;
			return this._itemStorageMap.TryGetValue(item, out dictionary) && dictionary.TryGetValue(property, out value);
		}

		// Token: 0x06004904 RID: 18692 RVA: 0x0014B528 File Offset: 0x00149728
		public void ClearValue(object item, DependencyProperty property)
		{
			this.EnsureItemStorageMap();
			Dictionary<DependencyProperty, object> dictionary;
			if (this._itemStorageMap.TryGetValue(item, out dictionary))
			{
				dictionary.Remove(property);
			}
		}

		// Token: 0x06004905 RID: 18693 RVA: 0x0014B553 File Offset: 0x00149753
		public void ClearItem(object item)
		{
			this.EnsureItemStorageMap();
			this._itemStorageMap.Remove(item);
		}

		// Token: 0x06004906 RID: 18694 RVA: 0x0014B568 File Offset: 0x00149768
		public void Clear()
		{
			this._itemStorageMap = null;
		}

		// Token: 0x06004907 RID: 18695 RVA: 0x0014B571 File Offset: 0x00149771
		private void EnsureItemStorageMap()
		{
			if (this._itemStorageMap == null)
			{
				this._itemStorageMap = new Dictionary<object, Dictionary<DependencyProperty, object>>();
			}
		}

		// Token: 0x06004908 RID: 18696 RVA: 0x0014B588 File Offset: 0x00149788
		private Dictionary<DependencyProperty, object> EnsureItem(object item)
		{
			this.EnsureItemStorageMap();
			Dictionary<DependencyProperty, object> dictionary;
			if (!this._itemStorageMap.TryGetValue(item, out dictionary))
			{
				dictionary = new Dictionary<DependencyProperty, object>();
				this._itemStorageMap[item] = dictionary;
			}
			return dictionary;
		}

		// Token: 0x040029C1 RID: 10689
		private Dictionary<object, Dictionary<DependencyProperty, object>> _itemStorageMap;
	}
}
