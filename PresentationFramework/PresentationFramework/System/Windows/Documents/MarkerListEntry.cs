using System;

namespace System.Windows.Documents
{
	// Token: 0x020003C2 RID: 962
	internal class MarkerListEntry
	{
		// Token: 0x060033D0 RID: 13264 RVA: 0x000E7027 File Offset: 0x000E5227
		internal MarkerListEntry()
		{
			this._marker = MarkerStyle.MarkerBullet;
			this._nILS = -1L;
			this._nStartIndexOverride = -1L;
			this._nStartIndexDefault = -1L;
			this._nVirtualListLevel = -1L;
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x060033D1 RID: 13265 RVA: 0x000E7057 File Offset: 0x000E5257
		// (set) Token: 0x060033D2 RID: 13266 RVA: 0x000E705F File Offset: 0x000E525F
		internal MarkerStyle Marker
		{
			get
			{
				return this._marker;
			}
			set
			{
				this._marker = value;
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x060033D3 RID: 13267 RVA: 0x000E7068 File Offset: 0x000E5268
		// (set) Token: 0x060033D4 RID: 13268 RVA: 0x000E7070 File Offset: 0x000E5270
		internal long StartIndexOverride
		{
			get
			{
				return this._nStartIndexOverride;
			}
			set
			{
				this._nStartIndexOverride = value;
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x060033D5 RID: 13269 RVA: 0x000E7079 File Offset: 0x000E5279
		// (set) Token: 0x060033D6 RID: 13270 RVA: 0x000E7081 File Offset: 0x000E5281
		internal long StartIndexDefault
		{
			get
			{
				return this._nStartIndexDefault;
			}
			set
			{
				this._nStartIndexDefault = value;
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x060033D7 RID: 13271 RVA: 0x000E708A File Offset: 0x000E528A
		// (set) Token: 0x060033D8 RID: 13272 RVA: 0x000E7092 File Offset: 0x000E5292
		internal long VirtualListLevel
		{
			get
			{
				return this._nVirtualListLevel;
			}
			set
			{
				this._nVirtualListLevel = value;
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x060033D9 RID: 13273 RVA: 0x000E709B File Offset: 0x000E529B
		internal long StartIndexToUse
		{
			get
			{
				if (this._nStartIndexOverride <= 0L)
				{
					return this._nStartIndexDefault;
				}
				return this._nStartIndexOverride;
			}
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x060033DA RID: 13274 RVA: 0x000E70B4 File Offset: 0x000E52B4
		// (set) Token: 0x060033DB RID: 13275 RVA: 0x000E70BC File Offset: 0x000E52BC
		internal long ILS
		{
			get
			{
				return this._nILS;
			}
			set
			{
				this._nILS = value;
			}
		}

		// Token: 0x040024AC RID: 9388
		private MarkerStyle _marker;

		// Token: 0x040024AD RID: 9389
		private long _nStartIndexOverride;

		// Token: 0x040024AE RID: 9390
		private long _nStartIndexDefault;

		// Token: 0x040024AF RID: 9391
		private long _nVirtualListLevel;

		// Token: 0x040024B0 RID: 9392
		private long _nILS;
	}
}
