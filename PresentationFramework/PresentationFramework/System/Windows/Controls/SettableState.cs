using System;

namespace System.Windows.Controls
{
	// Token: 0x0200046E RID: 1134
	internal struct SettableState<T>
	{
		// Token: 0x06004234 RID: 16948 RVA: 0x0012EDDC File Offset: 0x0012CFDC
		internal SettableState(T value)
		{
			this._value = value;
			this._isSet = (this._wasSet = false);
		}

		// Token: 0x040027D9 RID: 10201
		internal T _value;

		// Token: 0x040027DA RID: 10202
		internal bool _isSet;

		// Token: 0x040027DB RID: 10203
		internal bool _wasSet;
	}
}
