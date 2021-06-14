using System;
using System.ComponentModel;

namespace MS.Internal.Data
{
	// Token: 0x02000746 RID: 1862
	internal class ViewRecord
	{
		// Token: 0x060076EC RID: 30444 RVA: 0x0021FCA4 File Offset: 0x0021DEA4
		internal ViewRecord(ICollectionView view)
		{
			this._view = view;
			this._version = -1;
		}

		// Token: 0x17001C43 RID: 7235
		// (get) Token: 0x060076ED RID: 30445 RVA: 0x0021FCBA File Offset: 0x0021DEBA
		internal ICollectionView View
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x17001C44 RID: 7236
		// (get) Token: 0x060076EE RID: 30446 RVA: 0x0021FCC2 File Offset: 0x0021DEC2
		// (set) Token: 0x060076EF RID: 30447 RVA: 0x0021FCCA File Offset: 0x0021DECA
		internal int Version
		{
			get
			{
				return this._version;
			}
			set
			{
				this._version = value;
			}
		}

		// Token: 0x17001C45 RID: 7237
		// (get) Token: 0x060076F0 RID: 30448 RVA: 0x0021FCD3 File Offset: 0x0021DED3
		internal bool IsInitialized
		{
			get
			{
				return this._isInitialized;
			}
		}

		// Token: 0x060076F1 RID: 30449 RVA: 0x0021FCDB File Offset: 0x0021DEDB
		internal void InitializeView()
		{
			this._view.MoveCurrentToFirst();
			this._isInitialized = true;
		}

		// Token: 0x0400389C RID: 14492
		private ICollectionView _view;

		// Token: 0x0400389D RID: 14493
		private int _version;

		// Token: 0x0400389E RID: 14494
		private bool _isInitialized;
	}
}
