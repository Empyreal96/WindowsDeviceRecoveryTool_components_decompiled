using System;
using System.Collections.ObjectModel;

namespace System.Windows
{
	// Token: 0x020000E7 RID: 231
	internal class ResourceDictionaryCollection : ObservableCollection<ResourceDictionary>
	{
		// Token: 0x06000838 RID: 2104 RVA: 0x0001ACF8 File Offset: 0x00018EF8
		internal ResourceDictionaryCollection(ResourceDictionary owner)
		{
			this._owner = owner;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0001AD08 File Offset: 0x00018F08
		protected override void ClearItems()
		{
			for (int i = 0; i < base.Count; i++)
			{
				this._owner.RemoveParentOwners(base[i]);
			}
			base.ClearItems();
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0001AD3E File Offset: 0x00018F3E
		protected override void InsertItem(int index, ResourceDictionary item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.InsertItem(index, item);
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0001AD56 File Offset: 0x00018F56
		protected override void SetItem(int index, ResourceDictionary item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.SetItem(index, item);
		}

		// Token: 0x0400078F RID: 1935
		private ResourceDictionary _owner;
	}
}
