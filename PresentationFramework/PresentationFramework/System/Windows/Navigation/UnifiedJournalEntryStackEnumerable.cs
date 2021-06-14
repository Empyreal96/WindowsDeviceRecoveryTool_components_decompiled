using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Windows.Navigation
{
	// Token: 0x02000308 RID: 776
	internal class UnifiedJournalEntryStackEnumerable : IEnumerable, INotifyCollectionChanged
	{
		// Token: 0x0600291C RID: 10524 RVA: 0x000BE0BC File Offset: 0x000BC2BC
		internal UnifiedJournalEntryStackEnumerable(LimitedJournalEntryStackEnumerable backStack, LimitedJournalEntryStackEnumerable forwardStack)
		{
			this._backStack = backStack;
			this._backStack.CollectionChanged += this.StacksChanged;
			this._forwardStack = forwardStack;
			this._forwardStack.CollectionChanged += this.StacksChanged;
		}

		// Token: 0x0600291D RID: 10525 RVA: 0x000BE10C File Offset: 0x000BC30C
		public IEnumerator GetEnumerator()
		{
			if (this._items == null)
			{
				this._items = new ArrayList(19);
				foreach (object obj in this._forwardStack)
				{
					JournalEntry journalEntry = (JournalEntry)obj;
					this._items.Insert(0, journalEntry);
					JournalEntryUnifiedViewConverter.SetJournalEntryPosition(journalEntry, JournalEntryPosition.Forward);
				}
				DependencyObject dependencyObject = new DependencyObject();
				dependencyObject.SetValue(JournalEntry.NameProperty, SR.Get("NavWindowMenuCurrentPage"));
				this._items.Add(dependencyObject);
				foreach (object obj2 in this._backStack)
				{
					JournalEntry journalEntry2 = (JournalEntry)obj2;
					this._items.Add(journalEntry2);
					JournalEntryUnifiedViewConverter.SetJournalEntryPosition(journalEntry2, JournalEntryPosition.Back);
				}
			}
			return this._items.GetEnumerator();
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x000BE21C File Offset: 0x000BC41C
		internal void StacksChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this._items = null;
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x0600291F RID: 10527 RVA: 0x000BE240 File Offset: 0x000BC440
		// (remove) Token: 0x06002920 RID: 10528 RVA: 0x000BE278 File Offset: 0x000BC478
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x04001BCD RID: 7117
		private LimitedJournalEntryStackEnumerable _backStack;

		// Token: 0x04001BCE RID: 7118
		private LimitedJournalEntryStackEnumerable _forwardStack;

		// Token: 0x04001BCF RID: 7119
		private ArrayList _items;
	}
}
