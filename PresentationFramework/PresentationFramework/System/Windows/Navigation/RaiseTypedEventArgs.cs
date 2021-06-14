using System;

namespace System.Windows.Navigation
{
	// Token: 0x02000321 RID: 801
	internal class RaiseTypedEventArgs : EventArgs
	{
		// Token: 0x06002A74 RID: 10868 RVA: 0x000C2809 File Offset: 0x000C0A09
		internal RaiseTypedEventArgs(Delegate d, object o)
		{
			this.D = d;
			this.O = o;
		}

		// Token: 0x04001C40 RID: 7232
		internal Delegate D;

		// Token: 0x04001C41 RID: 7233
		internal object O;
	}
}
