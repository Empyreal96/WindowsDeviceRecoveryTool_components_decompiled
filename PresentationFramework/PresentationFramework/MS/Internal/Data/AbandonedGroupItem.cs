using System;

namespace MS.Internal.Data
{
	// Token: 0x0200070D RID: 1805
	internal class AbandonedGroupItem
	{
		// Token: 0x060073E3 RID: 29667 RVA: 0x00212C7E File Offset: 0x00210E7E
		public AbandonedGroupItem(LiveShapingItem lsi, CollectionViewGroupInternal group)
		{
			this._lsi = lsi;
			this._group = group;
		}

		// Token: 0x17001B87 RID: 7047
		// (get) Token: 0x060073E4 RID: 29668 RVA: 0x00212C94 File Offset: 0x00210E94
		public LiveShapingItem Item
		{
			get
			{
				return this._lsi;
			}
		}

		// Token: 0x17001B88 RID: 7048
		// (get) Token: 0x060073E5 RID: 29669 RVA: 0x00212C9C File Offset: 0x00210E9C
		public CollectionViewGroupInternal Group
		{
			get
			{
				return this._group;
			}
		}

		// Token: 0x040037C1 RID: 14273
		private LiveShapingItem _lsi;

		// Token: 0x040037C2 RID: 14274
		private CollectionViewGroupInternal _group;
	}
}
