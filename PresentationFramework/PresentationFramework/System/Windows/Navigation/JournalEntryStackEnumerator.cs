using System;
using System.Collections;

namespace System.Windows.Navigation
{
	// Token: 0x020002FA RID: 762
	internal class JournalEntryStackEnumerator : IEnumerator
	{
		// Token: 0x060028B3 RID: 10419 RVA: 0x000BD1B0 File Offset: 0x000BB3B0
		public JournalEntryStackEnumerator(Journal journal, int start, int delta, JournalEntryFilter filter)
		{
			this._journal = journal;
			this._version = journal.Version;
			this._start = start;
			this._delta = delta;
			this._filter = filter;
			this.Reset();
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x000BD1E7 File Offset: 0x000BB3E7
		public void Reset()
		{
			this._next = this._start;
			this._current = null;
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x000BD1FC File Offset: 0x000BB3FC
		public bool MoveNext()
		{
			this.VerifyUnchanged();
			while (this._next >= 0 && this._next < this._journal.TotalCount)
			{
				this._current = this._journal[this._next];
				this._next += this._delta;
				if (this._filter == null || this._filter(this._current))
				{
					return true;
				}
			}
			this._current = null;
			return false;
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x060028B6 RID: 10422 RVA: 0x000BD27C File Offset: 0x000BB47C
		public object Current
		{
			get
			{
				return this._current;
			}
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x000BD284 File Offset: 0x000BB484
		protected void VerifyUnchanged()
		{
			if (this._version != this._journal.Version)
			{
				throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
			}
		}

		// Token: 0x04001B9E RID: 7070
		private Journal _journal;

		// Token: 0x04001B9F RID: 7071
		private int _start;

		// Token: 0x04001BA0 RID: 7072
		private int _delta;

		// Token: 0x04001BA1 RID: 7073
		private int _next;

		// Token: 0x04001BA2 RID: 7074
		private JournalEntry _current;

		// Token: 0x04001BA3 RID: 7075
		private JournalEntryFilter _filter;

		// Token: 0x04001BA4 RID: 7076
		private int _version;
	}
}
