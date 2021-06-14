using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Windows.Navigation
{
	// Token: 0x020002F7 RID: 759
	internal abstract class JournalEntryStack : IEnumerable, INotifyCollectionChanged
	{
		// Token: 0x060028A7 RID: 10407 RVA: 0x000BD0A0 File Offset: 0x000BB2A0
		internal JournalEntryStack(Journal journal)
		{
			this._journal = journal;
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x000BD0AF File Offset: 0x000BB2AF
		internal void OnCollectionChanged()
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x060028A9 RID: 10409 RVA: 0x000BD0CB File Offset: 0x000BB2CB
		// (set) Token: 0x060028AA RID: 10410 RVA: 0x000BD0D3 File Offset: 0x000BB2D3
		internal JournalEntryFilter Filter
		{
			get
			{
				return this._filter;
			}
			set
			{
				this._filter = value;
			}
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x000BD0DC File Offset: 0x000BB2DC
		internal IEnumerable GetLimitedJournalEntryStackEnumerable()
		{
			if (this._ljese == null)
			{
				this._ljese = new LimitedJournalEntryStackEnumerable(this);
			}
			return this._ljese;
		}

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x060028AC RID: 10412 RVA: 0x000BD0F8 File Offset: 0x000BB2F8
		// (remove) Token: 0x060028AD RID: 10413 RVA: 0x000BD130 File Offset: 0x000BB330
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x060028AE RID: 10414
		public abstract IEnumerator GetEnumerator();

		// Token: 0x04001B9A RID: 7066
		private LimitedJournalEntryStackEnumerable _ljese;

		// Token: 0x04001B9B RID: 7067
		protected JournalEntryFilter _filter;

		// Token: 0x04001B9D RID: 7069
		protected Journal _journal;
	}
}
