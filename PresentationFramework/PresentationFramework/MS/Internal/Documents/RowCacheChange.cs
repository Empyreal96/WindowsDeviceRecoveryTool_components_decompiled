using System;

namespace MS.Internal.Documents
{
	// Token: 0x020006EF RID: 1775
	internal class RowCacheChange
	{
		// Token: 0x060071FD RID: 29181 RVA: 0x002095AF File Offset: 0x002077AF
		public RowCacheChange(int start, int count)
		{
			this._start = start;
			this._count = count;
		}

		// Token: 0x17001B1E RID: 6942
		// (get) Token: 0x060071FE RID: 29182 RVA: 0x002095C5 File Offset: 0x002077C5
		public int Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17001B1F RID: 6943
		// (get) Token: 0x060071FF RID: 29183 RVA: 0x002095CD File Offset: 0x002077CD
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x04003751 RID: 14161
		private readonly int _start;

		// Token: 0x04003752 RID: 14162
		private readonly int _count;
	}
}
