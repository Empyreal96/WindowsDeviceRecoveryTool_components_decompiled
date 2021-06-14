using System;

namespace System.Windows.Documents
{
	// Token: 0x020003CA RID: 970
	internal class ListTableEntry
	{
		// Token: 0x06003415 RID: 13333 RVA: 0x000E7CDF File Offset: 0x000E5EDF
		internal ListTableEntry()
		{
			this._id = 0L;
			this._templateID = 0L;
			this._levels = new ListLevelTable();
		}

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x06003416 RID: 13334 RVA: 0x000E7D02 File Offset: 0x000E5F02
		// (set) Token: 0x06003417 RID: 13335 RVA: 0x000E7D0A File Offset: 0x000E5F0A
		internal long ID
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		// Token: 0x17000D63 RID: 3427
		// (set) Token: 0x06003418 RID: 13336 RVA: 0x000E7D13 File Offset: 0x000E5F13
		internal long TemplateID
		{
			set
			{
				this._templateID = value;
			}
		}

		// Token: 0x17000D64 RID: 3428
		// (set) Token: 0x06003419 RID: 13337 RVA: 0x000E7D1C File Offset: 0x000E5F1C
		internal bool Simple
		{
			set
			{
				this._simple = value;
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x0600341A RID: 13338 RVA: 0x000E7D25 File Offset: 0x000E5F25
		internal ListLevelTable Levels
		{
			get
			{
				return this._levels;
			}
		}

		// Token: 0x040024BE RID: 9406
		private long _id;

		// Token: 0x040024BF RID: 9407
		private long _templateID;

		// Token: 0x040024C0 RID: 9408
		private bool _simple;

		// Token: 0x040024C1 RID: 9409
		private ListLevelTable _levels;
	}
}
