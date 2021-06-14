using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x02000375 RID: 885
	internal abstract class HighlightChangedEventArgs
	{
		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x06002FF6 RID: 12278
		internal abstract IList Ranges { get; }

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x06002FF7 RID: 12279
		internal abstract Type OwnerType { get; }
	}
}
