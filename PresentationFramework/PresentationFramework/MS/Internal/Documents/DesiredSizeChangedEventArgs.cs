using System;
using System.Windows;

namespace MS.Internal.Documents
{
	// Token: 0x020006FA RID: 1786
	internal class DesiredSizeChangedEventArgs : EventArgs
	{
		// Token: 0x06007314 RID: 29460 RVA: 0x002102F0 File Offset: 0x0020E4F0
		internal DesiredSizeChangedEventArgs(UIElement child)
		{
			this._child = child;
		}

		// Token: 0x17001B4A RID: 6986
		// (get) Token: 0x06007315 RID: 29461 RVA: 0x002102FF File Offset: 0x0020E4FF
		internal UIElement Child
		{
			get
			{
				return this._child;
			}
		}

		// Token: 0x04003771 RID: 14193
		private readonly UIElement _child;
	}
}
