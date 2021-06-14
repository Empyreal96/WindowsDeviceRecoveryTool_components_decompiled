using System;

namespace System.Windows.Documents
{
	// Token: 0x020003CC RID: 972
	internal class ListOverride
	{
		// Token: 0x06003420 RID: 13344 RVA: 0x000E7DA7 File Offset: 0x000E5FA7
		internal ListOverride()
		{
			this._id = 0L;
			this._index = 0L;
			this._levels = null;
			this._nStartIndex = -1L;
		}

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06003421 RID: 13345 RVA: 0x000E7DCE File Offset: 0x000E5FCE
		// (set) Token: 0x06003422 RID: 13346 RVA: 0x000E7DD6 File Offset: 0x000E5FD6
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

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06003423 RID: 13347 RVA: 0x000E7DDF File Offset: 0x000E5FDF
		// (set) Token: 0x06003424 RID: 13348 RVA: 0x000E7DE7 File Offset: 0x000E5FE7
		internal long Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x06003425 RID: 13349 RVA: 0x000E7DF0 File Offset: 0x000E5FF0
		// (set) Token: 0x06003426 RID: 13350 RVA: 0x000E7DF8 File Offset: 0x000E5FF8
		internal ListLevelTable Levels
		{
			get
			{
				return this._levels;
			}
			set
			{
				this._levels = value;
			}
		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x06003427 RID: 13351 RVA: 0x000E7E01 File Offset: 0x000E6001
		// (set) Token: 0x06003428 RID: 13352 RVA: 0x000E7E09 File Offset: 0x000E6009
		internal long StartIndex
		{
			get
			{
				return this._nStartIndex;
			}
			set
			{
				this._nStartIndex = value;
			}
		}

		// Token: 0x040024C2 RID: 9410
		private long _id;

		// Token: 0x040024C3 RID: 9411
		private long _index;

		// Token: 0x040024C4 RID: 9412
		private long _nStartIndex;

		// Token: 0x040024C5 RID: 9413
		private ListLevelTable _levels;
	}
}
