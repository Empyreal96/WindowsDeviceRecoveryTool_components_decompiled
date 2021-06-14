using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace MS.Internal.Annotations
{
	// Token: 0x020007C4 RID: 1988
	internal class AnnotationObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged2, IOwnedObject
	{
		// Token: 0x06007B73 RID: 31603 RVA: 0x0022B890 File Offset: 0x00229A90
		public AnnotationObservableCollection()
		{
			this._listener = new PropertyChangedEventHandler(this.OnItemPropertyChanged);
		}

		// Token: 0x06007B74 RID: 31604 RVA: 0x0022B8C1 File Offset: 0x00229AC1
		public AnnotationObservableCollection(List<T> list) : base(list)
		{
			this._listener = new PropertyChangedEventHandler(this.OnItemPropertyChanged);
		}

		// Token: 0x06007B75 RID: 31605 RVA: 0x0022B8F4 File Offset: 0x00229AF4
		protected override void ClearItems()
		{
			foreach (!0 ! in this)
			{
				INotifyPropertyChanged2 item = !;
				this.SetOwned(item, false);
			}
			this.ProtectedClearItems();
		}

		// Token: 0x06007B76 RID: 31606 RVA: 0x0022B948 File Offset: 0x00229B48
		protected override void RemoveItem(int index)
		{
			T t = base[index];
			this.SetOwned(t, false);
			base.RemoveItem(index);
		}

		// Token: 0x06007B77 RID: 31607 RVA: 0x0022B971 File Offset: 0x00229B71
		protected override void InsertItem(int index, T item)
		{
			if (this.ItemOwned(item))
			{
				throw new ArgumentException(SR.Get("AlreadyHasParent"));
			}
			base.InsertItem(index, item);
			this.SetOwned(item, true);
		}

		// Token: 0x06007B78 RID: 31608 RVA: 0x0022B9A8 File Offset: 0x00229BA8
		protected override void SetItem(int index, T item)
		{
			if (this.ItemOwned(item))
			{
				throw new ArgumentException(SR.Get("AlreadyHasParent"));
			}
			T t = base[index];
			this.SetOwned(t, false);
			this.ProtectedSetItem(index, item);
			this.SetOwned(item, true);
		}

		// Token: 0x06007B79 RID: 31609 RVA: 0x0022B9FD File Offset: 0x00229BFD
		protected virtual void ProtectedClearItems()
		{
			base.ClearItems();
		}

		// Token: 0x06007B7A RID: 31610 RVA: 0x0022BA05 File Offset: 0x00229C05
		protected virtual void ProtectedSetItem(int index, T item)
		{
			base.Items[index] = item;
			this.OnPropertyChanged(new PropertyChangedEventArgs(this.CountString));
			this.OnPropertyChanged(new PropertyChangedEventArgs(this.IndexerName));
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x06007B7B RID: 31611 RVA: 0x0022BA42 File Offset: 0x00229C42
		protected void ObservableCollectionSetItem(int index, T item)
		{
			base.SetItem(index, item);
		}

		// Token: 0x06007B7C RID: 31612 RVA: 0x0022BA4C File Offset: 0x00229C4C
		protected virtual void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x06007B7D RID: 31613 RVA: 0x0022BA5C File Offset: 0x00229C5C
		private bool ItemOwned(object item)
		{
			if (item != null)
			{
				IOwnedObject ownedObject = item as IOwnedObject;
				return ownedObject.Owned;
			}
			return false;
		}

		// Token: 0x06007B7E RID: 31614 RVA: 0x0022BA7C File Offset: 0x00229C7C
		private void SetOwned(object item, bool owned)
		{
			if (item != null)
			{
				IOwnedObject ownedObject = item as IOwnedObject;
				ownedObject.Owned = owned;
				if (owned)
				{
					((INotifyPropertyChanged2)item).PropertyChanged += this._listener;
					return;
				}
				((INotifyPropertyChanged2)item).PropertyChanged -= this._listener;
			}
		}

		// Token: 0x04003A1C RID: 14876
		private readonly PropertyChangedEventHandler _listener;

		// Token: 0x04003A1D RID: 14877
		internal readonly string CountString = "Count";

		// Token: 0x04003A1E RID: 14878
		internal readonly string IndexerName = "Item[]";
	}
}
