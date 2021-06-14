using System;
using System.Collections.Generic;

namespace MS.Internal.Documents
{
	// Token: 0x020006DB RID: 1755
	internal class PageCacheChangedEventArgs : EventArgs
	{
		// Token: 0x06007165 RID: 29029 RVA: 0x0020738A File Offset: 0x0020558A
		public PageCacheChangedEventArgs(List<PageCacheChange> changes)
		{
			this._changes = changes;
		}

		// Token: 0x17001AE6 RID: 6886
		// (get) Token: 0x06007166 RID: 29030 RVA: 0x00207399 File Offset: 0x00205599
		public List<PageCacheChange> Changes
		{
			get
			{
				return this._changes;
			}
		}

		// Token: 0x04003719 RID: 14105
		private readonly List<PageCacheChange> _changes;
	}
}
