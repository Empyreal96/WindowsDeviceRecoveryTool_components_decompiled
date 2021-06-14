using System;

namespace MS.Internal.Data
{
	// Token: 0x02000747 RID: 1863
	internal class CollectionRecord
	{
		// Token: 0x17001C46 RID: 7238
		// (get) Token: 0x060076F2 RID: 30450 RVA: 0x0021FCF0 File Offset: 0x0021DEF0
		// (set) Token: 0x060076F3 RID: 30451 RVA: 0x0021FD02 File Offset: 0x0021DF02
		public ViewTable ViewTable
		{
			get
			{
				return (ViewTable)this._wrViewTable.Target;
			}
			set
			{
				this._wrViewTable = new WeakReference(value);
			}
		}

		// Token: 0x17001C47 RID: 7239
		// (get) Token: 0x060076F4 RID: 30452 RVA: 0x0021FD10 File Offset: 0x0021DF10
		public bool IsAlive
		{
			get
			{
				return this.SynchronizationInfo.IsAlive || this._wrViewTable.IsAlive;
			}
		}

		// Token: 0x0400389F RID: 14495
		public SynchronizationInfo SynchronizationInfo;

		// Token: 0x040038A0 RID: 14496
		private WeakReference _wrViewTable = ViewManager.NullWeakRef;
	}
}
