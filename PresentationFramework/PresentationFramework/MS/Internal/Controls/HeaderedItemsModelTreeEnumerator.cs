using System;
using System.Collections;
using System.Windows.Controls;

namespace MS.Internal.Controls
{
	// Token: 0x02000762 RID: 1890
	internal class HeaderedItemsModelTreeEnumerator : ModelTreeEnumerator
	{
		// Token: 0x06007842 RID: 30786 RVA: 0x002242DF File Offset: 0x002224DF
		internal HeaderedItemsModelTreeEnumerator(HeaderedItemsControl headeredItemsControl, IEnumerator items, object header) : base(header)
		{
			this._owner = headeredItemsControl;
			this._items = items;
		}

		// Token: 0x17001C81 RID: 7297
		// (get) Token: 0x06007843 RID: 30787 RVA: 0x002242F6 File Offset: 0x002224F6
		protected override object Current
		{
			get
			{
				if (base.Index > 0)
				{
					return this._items.Current;
				}
				return base.Current;
			}
		}

		// Token: 0x06007844 RID: 30788 RVA: 0x00224314 File Offset: 0x00222514
		protected override bool MoveNext()
		{
			if (base.Index >= 0)
			{
				int index = base.Index;
				base.Index = index + 1;
				return this._items.MoveNext();
			}
			return base.MoveNext();
		}

		// Token: 0x06007845 RID: 30789 RVA: 0x0022434C File Offset: 0x0022254C
		protected override void Reset()
		{
			base.Reset();
			this._items.Reset();
		}

		// Token: 0x17001C82 RID: 7298
		// (get) Token: 0x06007846 RID: 30790 RVA: 0x00224360 File Offset: 0x00222560
		protected override bool IsUnchanged
		{
			get
			{
				object content = base.Content;
				return content == this._owner.Header;
			}
		}

		// Token: 0x040038EE RID: 14574
		private HeaderedItemsControl _owner;

		// Token: 0x040038EF RID: 14575
		private IEnumerator _items;
	}
}
