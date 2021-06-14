using System;
using System.Collections;

namespace System.Windows.Navigation
{
	// Token: 0x020002FC RID: 764
	internal class LimitedJournalEntryStackEnumerator : IEnumerator
	{
		// Token: 0x060028BD RID: 10429 RVA: 0x000BD375 File Offset: 0x000BB575
		internal LimitedJournalEntryStackEnumerator(IEnumerable ieble, uint viewLimit)
		{
			this._ienum = ieble.GetEnumerator();
			this._viewLimit = viewLimit;
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x000BD390 File Offset: 0x000BB590
		public void Reset()
		{
			this._itemsReturned = 0U;
			this._ienum.Reset();
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x000BD3A4 File Offset: 0x000BB5A4
		public bool MoveNext()
		{
			bool flag;
			if (this._itemsReturned == this._viewLimit)
			{
				flag = false;
			}
			else
			{
				flag = this._ienum.MoveNext();
				if (flag)
				{
					this._itemsReturned += 1U;
				}
			}
			return flag;
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x060028C0 RID: 10432 RVA: 0x000BD3E1 File Offset: 0x000BB5E1
		public object Current
		{
			get
			{
				return this._ienum.Current;
			}
		}

		// Token: 0x04001BA8 RID: 7080
		private uint _itemsReturned;

		// Token: 0x04001BA9 RID: 7081
		private uint _viewLimit;

		// Token: 0x04001BAA RID: 7082
		private IEnumerator _ienum;
	}
}
