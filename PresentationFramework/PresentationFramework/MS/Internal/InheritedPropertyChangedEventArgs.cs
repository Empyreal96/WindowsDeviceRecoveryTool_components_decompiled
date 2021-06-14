using System;
using System.Windows;

namespace MS.Internal
{
	// Token: 0x020005E8 RID: 1512
	internal class InheritedPropertyChangedEventArgs : EventArgs
	{
		// Token: 0x060064F0 RID: 25840 RVA: 0x001C552E File Offset: 0x001C372E
		internal InheritedPropertyChangedEventArgs(ref InheritablePropertyChangeInfo info)
		{
			this._info = info;
		}

		// Token: 0x17001840 RID: 6208
		// (get) Token: 0x060064F1 RID: 25841 RVA: 0x001C5542 File Offset: 0x001C3742
		internal InheritablePropertyChangeInfo Info
		{
			get
			{
				return this._info;
			}
		}

		// Token: 0x040032B9 RID: 12985
		private InheritablePropertyChangeInfo _info;
	}
}
