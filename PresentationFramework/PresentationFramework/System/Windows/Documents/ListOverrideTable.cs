using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x020003CD RID: 973
	internal class ListOverrideTable : ArrayList
	{
		// Token: 0x06003429 RID: 13353 RVA: 0x000E3C42 File Offset: 0x000E1E42
		internal ListOverrideTable() : base(20)
		{
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x000E7E12 File Offset: 0x000E6012
		internal ListOverride EntryAt(int index)
		{
			return (ListOverride)this[index];
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x000E7E20 File Offset: 0x000E6020
		internal ListOverride FindEntry(int index)
		{
			for (int i = 0; i < this.Count; i++)
			{
				ListOverride listOverride = this.EntryAt(i);
				if (listOverride.Index == (long)index)
				{
					return listOverride;
				}
			}
			return null;
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x000E7E54 File Offset: 0x000E6054
		internal ListOverride AddEntry()
		{
			ListOverride listOverride = new ListOverride();
			this.Add(listOverride);
			return listOverride;
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x0600342D RID: 13357 RVA: 0x000E7E70 File Offset: 0x000E6070
		internal ListOverride CurrentEntry
		{
			get
			{
				if (this.Count <= 0)
				{
					return null;
				}
				return this.EntryAt(this.Count - 1);
			}
		}
	}
}
