using System;
using System.ComponentModel;

namespace MS.Internal.Data
{
	// Token: 0x0200074B RID: 1867
	internal class ValueChangedEventArgs : EventArgs
	{
		// Token: 0x06007719 RID: 30489 RVA: 0x0022090C File Offset: 0x0021EB0C
		internal ValueChangedEventArgs(PropertyDescriptor pd)
		{
			this._pd = pd;
		}

		// Token: 0x17001C4D RID: 7245
		// (get) Token: 0x0600771A RID: 30490 RVA: 0x0022091B File Offset: 0x0021EB1B
		internal PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this._pd;
			}
		}

		// Token: 0x040038AB RID: 14507
		private PropertyDescriptor _pd;
	}
}
