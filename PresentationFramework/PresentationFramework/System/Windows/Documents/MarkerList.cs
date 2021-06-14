using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x020003C3 RID: 963
	internal class MarkerList : ArrayList
	{
		// Token: 0x060033DC RID: 13276 RVA: 0x000E70C5 File Offset: 0x000E52C5
		internal MarkerList() : base(5)
		{
		}

		// Token: 0x060033DD RID: 13277 RVA: 0x000E70CE File Offset: 0x000E52CE
		internal MarkerListEntry EntryAt(int index)
		{
			return (MarkerListEntry)this[index];
		}

		// Token: 0x060033DE RID: 13278 RVA: 0x000E70DC File Offset: 0x000E52DC
		internal void AddEntry(MarkerStyle m, long nILS, long nStartIndexOverride, long nStartIndexDefault, long nLevel)
		{
			this.Add(new MarkerListEntry
			{
				Marker = m,
				StartIndexOverride = nStartIndexOverride,
				StartIndexDefault = nStartIndexDefault,
				VirtualListLevel = nLevel,
				ILS = nILS
			});
		}
	}
}
