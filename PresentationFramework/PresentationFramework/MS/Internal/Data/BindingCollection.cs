using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000706 RID: 1798
	internal class BindingCollection : Collection<BindingBase>
	{
		// Token: 0x06007365 RID: 29541 RVA: 0x0021138A File Offset: 0x0020F58A
		internal BindingCollection(BindingBase owner, BindingCollectionChangedCallback callback)
		{
			Invariant.Assert(owner != null && callback != null);
			this._owner = owner;
			this._collectionChangedCallback = callback;
		}

		// Token: 0x06007366 RID: 29542 RVA: 0x002113AF File Offset: 0x0020F5AF
		private BindingCollection()
		{
		}

		// Token: 0x06007367 RID: 29543 RVA: 0x002113B7 File Offset: 0x0020F5B7
		protected override void ClearItems()
		{
			this._owner.CheckSealed();
			base.ClearItems();
			this.OnBindingCollectionChanged();
		}

		// Token: 0x06007368 RID: 29544 RVA: 0x002113D0 File Offset: 0x0020F5D0
		protected override void RemoveItem(int index)
		{
			this._owner.CheckSealed();
			base.RemoveItem(index);
			this.OnBindingCollectionChanged();
		}

		// Token: 0x06007369 RID: 29545 RVA: 0x002113EA File Offset: 0x0020F5EA
		protected override void InsertItem(int index, BindingBase item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.ValidateItem(item);
			this._owner.CheckSealed();
			base.InsertItem(index, item);
			this.OnBindingCollectionChanged();
		}

		// Token: 0x0600736A RID: 29546 RVA: 0x0021141A File Offset: 0x0020F61A
		protected override void SetItem(int index, BindingBase item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.ValidateItem(item);
			this._owner.CheckSealed();
			base.SetItem(index, item);
			this.OnBindingCollectionChanged();
		}

		// Token: 0x0600736B RID: 29547 RVA: 0x0021144A File Offset: 0x0020F64A
		private void ValidateItem(BindingBase binding)
		{
			if (!(binding is Binding))
			{
				throw new NotSupportedException(SR.Get("BindingCollectionContainsNonBinding", new object[]
				{
					binding.GetType().Name
				}));
			}
		}

		// Token: 0x0600736C RID: 29548 RVA: 0x00211478 File Offset: 0x0020F678
		private void OnBindingCollectionChanged()
		{
			if (this._collectionChangedCallback != null)
			{
				this._collectionChangedCallback();
			}
		}

		// Token: 0x040037A8 RID: 14248
		private BindingBase _owner;

		// Token: 0x040037A9 RID: 14249
		private BindingCollectionChangedCallback _collectionChangedCallback;
	}
}
