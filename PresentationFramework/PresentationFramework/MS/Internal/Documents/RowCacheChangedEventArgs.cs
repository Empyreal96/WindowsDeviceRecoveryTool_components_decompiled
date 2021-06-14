using System;
using System.Collections.Generic;

namespace MS.Internal.Documents
{
	// Token: 0x020006EE RID: 1774
	internal class RowCacheChangedEventArgs : EventArgs
	{
		// Token: 0x060071FB RID: 29179 RVA: 0x00209598 File Offset: 0x00207798
		public RowCacheChangedEventArgs(List<RowCacheChange> changes)
		{
			this._changes = changes;
		}

		// Token: 0x17001B1D RID: 6941
		// (get) Token: 0x060071FC RID: 29180 RVA: 0x002095A7 File Offset: 0x002077A7
		public List<RowCacheChange> Changes
		{
			get
			{
				return this._changes;
			}
		}

		// Token: 0x04003750 RID: 14160
		private readonly List<RowCacheChange> _changes;
	}
}
