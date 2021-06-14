using System;

namespace System.Windows.Documents
{
	// Token: 0x020003C8 RID: 968
	internal class ListLevel
	{
		// Token: 0x0600340B RID: 13323 RVA: 0x000E7C2A File Offset: 0x000E5E2A
		internal ListLevel()
		{
			this._nStartIndex = 1L;
			this._numberType = MarkerStyle.MarkerArabic;
		}

		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x0600340C RID: 13324 RVA: 0x000E7C41 File Offset: 0x000E5E41
		// (set) Token: 0x0600340D RID: 13325 RVA: 0x000E7C49 File Offset: 0x000E5E49
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

		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x0600340E RID: 13326 RVA: 0x000E7C52 File Offset: 0x000E5E52
		// (set) Token: 0x0600340F RID: 13327 RVA: 0x000E7C5A File Offset: 0x000E5E5A
		internal MarkerStyle Marker
		{
			get
			{
				return this._numberType;
			}
			set
			{
				this._numberType = value;
			}
		}

		// Token: 0x17000D60 RID: 3424
		// (set) Token: 0x06003410 RID: 13328 RVA: 0x000E7C63 File Offset: 0x000E5E63
		internal FormatState FormatState
		{
			set
			{
				this._formatState = value;
			}
		}

		// Token: 0x040024BB RID: 9403
		private long _nStartIndex;

		// Token: 0x040024BC RID: 9404
		private MarkerStyle _numberType;

		// Token: 0x040024BD RID: 9405
		private FormatState _formatState;
	}
}
