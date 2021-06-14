using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x020003FC RID: 1020
	internal class TextEditorThreadLocalStore
	{
		// Token: 0x060038E2 RID: 14562 RVA: 0x0000326D File Offset: 0x0000146D
		internal TextEditorThreadLocalStore()
		{
		}

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x060038E3 RID: 14563 RVA: 0x0010142B File Offset: 0x000FF62B
		// (set) Token: 0x060038E4 RID: 14564 RVA: 0x00101433 File Offset: 0x000FF633
		internal int InputLanguageChangeEventHandlerCount
		{
			get
			{
				return this._inputLanguageChangeEventHandlerCount;
			}
			set
			{
				this._inputLanguageChangeEventHandlerCount = value;
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x060038E5 RID: 14565 RVA: 0x0010143C File Offset: 0x000FF63C
		// (set) Token: 0x060038E6 RID: 14566 RVA: 0x00101444 File Offset: 0x000FF644
		internal ArrayList PendingInputItems
		{
			get
			{
				return this._pendingInputItems;
			}
			set
			{
				this._pendingInputItems = value;
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x060038E7 RID: 14567 RVA: 0x0010144D File Offset: 0x000FF64D
		// (set) Token: 0x060038E8 RID: 14568 RVA: 0x00101455 File Offset: 0x000FF655
		internal bool PureControlShift
		{
			get
			{
				return this._pureControlShift;
			}
			set
			{
				this._pureControlShift = value;
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x060038E9 RID: 14569 RVA: 0x0010145E File Offset: 0x000FF65E
		// (set) Token: 0x060038EA RID: 14570 RVA: 0x00101466 File Offset: 0x000FF666
		internal bool Bidi
		{
			get
			{
				return this._bidi;
			}
			set
			{
				this._bidi = value;
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x060038EB RID: 14571 RVA: 0x0010146F File Offset: 0x000FF66F
		// (set) Token: 0x060038EC RID: 14572 RVA: 0x00101477 File Offset: 0x000FF677
		internal TextSelection FocusedTextSelection
		{
			get
			{
				return this._focusedTextSelection;
			}
			set
			{
				this._focusedTextSelection = value;
			}
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x060038ED RID: 14573 RVA: 0x00101480 File Offset: 0x000FF680
		// (set) Token: 0x060038EE RID: 14574 RVA: 0x00101488 File Offset: 0x000FF688
		internal TextServicesHost TextServicesHost
		{
			get
			{
				return this._textServicesHost;
			}
			set
			{
				this._textServicesHost = value;
			}
		}

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x060038EF RID: 14575 RVA: 0x00101491 File Offset: 0x000FF691
		// (set) Token: 0x060038F0 RID: 14576 RVA: 0x00101499 File Offset: 0x000FF699
		internal bool HideCursor
		{
			get
			{
				return this._hideCursor;
			}
			set
			{
				this._hideCursor = value;
			}
		}

		// Token: 0x0400259A RID: 9626
		private int _inputLanguageChangeEventHandlerCount;

		// Token: 0x0400259B RID: 9627
		private ArrayList _pendingInputItems;

		// Token: 0x0400259C RID: 9628
		private bool _pureControlShift;

		// Token: 0x0400259D RID: 9629
		private bool _bidi;

		// Token: 0x0400259E RID: 9630
		private TextSelection _focusedTextSelection;

		// Token: 0x0400259F RID: 9631
		private TextServicesHost _textServicesHost;

		// Token: 0x040025A0 RID: 9632
		private bool _hideCursor;
	}
}
