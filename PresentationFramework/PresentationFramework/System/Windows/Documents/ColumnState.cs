using System;

namespace System.Windows.Documents
{
	// Token: 0x020003CF RID: 975
	internal class ColumnState
	{
		// Token: 0x06003481 RID: 13441 RVA: 0x000E9916 File Offset: 0x000E7B16
		internal ColumnState()
		{
			this._nCellX = 0L;
			this._row = null;
			this._fFilled = false;
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x06003482 RID: 13442 RVA: 0x000E9934 File Offset: 0x000E7B34
		// (set) Token: 0x06003483 RID: 13443 RVA: 0x000E993C File Offset: 0x000E7B3C
		internal long CellX
		{
			get
			{
				return this._nCellX;
			}
			set
			{
				this._nCellX = value;
			}
		}

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06003484 RID: 13444 RVA: 0x000E9945 File Offset: 0x000E7B45
		// (set) Token: 0x06003485 RID: 13445 RVA: 0x000E994D File Offset: 0x000E7B4D
		internal DocumentNode Row
		{
			get
			{
				return this._row;
			}
			set
			{
				this._row = value;
			}
		}

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x06003486 RID: 13446 RVA: 0x000E9956 File Offset: 0x000E7B56
		// (set) Token: 0x06003487 RID: 13447 RVA: 0x000E995E File Offset: 0x000E7B5E
		internal bool IsFilled
		{
			get
			{
				return this._fFilled;
			}
			set
			{
				this._fFilled = value;
			}
		}

		// Token: 0x040024DA RID: 9434
		private long _nCellX;

		// Token: 0x040024DB RID: 9435
		private DocumentNode _row;

		// Token: 0x040024DC RID: 9436
		private bool _fFilled;
	}
}
