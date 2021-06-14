using System;

namespace MS.Internal.Data
{
	// Token: 0x02000709 RID: 1801
	internal class BindingValueChangedEventArgs : EventArgs
	{
		// Token: 0x06007374 RID: 29556 RVA: 0x002114CD File Offset: 0x0020F6CD
		internal BindingValueChangedEventArgs(object oldValue, object newValue)
		{
			this._oldValue = oldValue;
			this._newValue = newValue;
		}

		// Token: 0x17001B67 RID: 7015
		// (get) Token: 0x06007375 RID: 29557 RVA: 0x002114E3 File Offset: 0x0020F6E3
		public object OldValue
		{
			get
			{
				return this._oldValue;
			}
		}

		// Token: 0x17001B68 RID: 7016
		// (get) Token: 0x06007376 RID: 29558 RVA: 0x002114EB File Offset: 0x0020F6EB
		public object NewValue
		{
			get
			{
				return this._newValue;
			}
		}

		// Token: 0x040037AA RID: 14250
		private object _oldValue;

		// Token: 0x040037AB RID: 14251
		private object _newValue;
	}
}
