using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace System.Windows.Interactivity
{
	// Token: 0x02000003 RID: 3
	public abstract class AttachableCollection<T> : FreezableCollection<T>, IAttachedObject where T : DependencyObject, IAttachedObject
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020D0 File Offset: 0x000002D0
		protected DependencyObject AssociatedObject
		{
			get
			{
				base.ReadPreamble();
				return this.associatedObject;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020E0 File Offset: 0x000002E0
		internal AttachableCollection()
		{
			((INotifyCollectionChanged)this).CollectionChanged += this.OnCollectionChanged;
			this.snapshot = new Collection<T>();
		}

		// Token: 0x06000006 RID: 6
		protected abstract void OnAttached();

		// Token: 0x06000007 RID: 7
		protected abstract void OnDetaching();

		// Token: 0x06000008 RID: 8
		internal abstract void ItemAdded(T item);

		// Token: 0x06000009 RID: 9
		internal abstract void ItemRemoved(T item);

		// Token: 0x0600000A RID: 10 RVA: 0x00002114 File Offset: 0x00000314
		[Conditional("DEBUG")]
		private void VerifySnapshotIntegrity()
		{
			bool flag = base.Count == this.snapshot.Count;
			if (flag)
			{
				for (int i = 0; i < base.Count; i++)
				{
					if (base[i] != this.snapshot[i])
					{
						return;
					}
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000216C File Offset: 0x0000036C
		private void VerifyAdd(T item)
		{
			if (this.snapshot.Contains(item))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.DuplicateItemInCollectionExceptionMessage, new object[]
				{
					typeof(T).Name,
					base.GetType().Name
				}));
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021C4 File Offset: 0x000003C4
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				using (IEnumerator enumerator = e.NewItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						T t = (T)((object)obj);
						try
						{
							this.VerifyAdd(t);
							this.ItemAdded(t);
						}
						finally
						{
							this.snapshot.Insert(base.IndexOf(t), t);
						}
					}
					return;
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				goto IL_13A;
			case NotifyCollectionChangedAction.Replace:
				break;
			case NotifyCollectionChangedAction.Move:
				return;
			case NotifyCollectionChangedAction.Reset:
				goto IL_18D;
			default:
				return;
			}
			foreach (object obj2 in e.OldItems)
			{
				T item = (T)((object)obj2);
				this.ItemRemoved(item);
				this.snapshot.Remove(item);
			}
			using (IEnumerator enumerator3 = e.NewItems.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					object obj3 = enumerator3.Current;
					T t2 = (T)((object)obj3);
					try
					{
						this.VerifyAdd(t2);
						this.ItemAdded(t2);
					}
					finally
					{
						this.snapshot.Insert(base.IndexOf(t2), t2);
					}
				}
				return;
			}
			IL_13A:
			using (IEnumerator enumerator4 = e.OldItems.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					object obj4 = enumerator4.Current;
					T item2 = (T)((object)obj4);
					this.ItemRemoved(item2);
					this.snapshot.Remove(item2);
				}
				return;
			}
			IL_18D:
			foreach (T item3 in this.snapshot)
			{
				this.ItemRemoved(item3);
			}
			this.snapshot = new Collection<T>();
			foreach (T item4 in this)
			{
				this.VerifyAdd(item4);
				this.ItemAdded(item4);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002440 File Offset: 0x00000640
		DependencyObject IAttachedObject.AssociatedObject
		{
			get
			{
				return this.AssociatedObject;
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002448 File Offset: 0x00000648
		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject != this.AssociatedObject)
			{
				if (this.AssociatedObject != null)
				{
					throw new InvalidOperationException();
				}
				if (Interaction.ShouldRunInDesignMode || !(bool)base.GetValue(DesignerProperties.IsInDesignModeProperty))
				{
					base.WritePreamble();
					this.associatedObject = dependencyObject;
					base.WritePostscript();
				}
				this.OnAttached();
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000249E File Offset: 0x0000069E
		public void Detach()
		{
			this.OnDetaching();
			base.WritePreamble();
			this.associatedObject = null;
			base.WritePostscript();
		}

		// Token: 0x04000001 RID: 1
		private Collection<T> snapshot;

		// Token: 0x04000002 RID: 2
		private DependencyObject associatedObject;
	}
}
