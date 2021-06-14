using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Windows.Navigation
{
	// Token: 0x020002FB RID: 763
	internal class LimitedJournalEntryStackEnumerable : IEnumerable, INotifyCollectionChanged
	{
		// Token: 0x060028B8 RID: 10424 RVA: 0x000BD2AC File Offset: 0x000BB4AC
		internal LimitedJournalEntryStackEnumerable(IEnumerable ieble)
		{
			this._ieble = ieble;
			INotifyCollectionChanged notifyCollectionChanged = ieble as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				notifyCollectionChanged.CollectionChanged += this.PropogateCollectionChanged;
			}
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x000BD2E2 File Offset: 0x000BB4E2
		public IEnumerator GetEnumerator()
		{
			return new LimitedJournalEntryStackEnumerator(this._ieble, 9U);
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x000BD2F1 File Offset: 0x000BB4F1
		internal void PropogateCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, e);
			}
		}

		// Token: 0x14000059 RID: 89
		// (add) Token: 0x060028BB RID: 10427 RVA: 0x000BD308 File Offset: 0x000BB508
		// (remove) Token: 0x060028BC RID: 10428 RVA: 0x000BD340 File Offset: 0x000BB540
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x04001BA5 RID: 7077
		private const uint DefaultMaxMenuEntries = 9U;

		// Token: 0x04001BA7 RID: 7079
		private IEnumerable _ieble;
	}
}
