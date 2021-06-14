using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x020003C9 RID: 969
	internal class ListLevelTable : ArrayList
	{
		// Token: 0x06003411 RID: 13329 RVA: 0x000E7C6C File Offset: 0x000E5E6C
		internal ListLevelTable() : base(1)
		{
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x000E7C75 File Offset: 0x000E5E75
		internal ListLevel EntryAt(int index)
		{
			if (index > this.Count)
			{
				index = this.Count - 1;
			}
			return (ListLevel)((this.Count > index && index >= 0) ? this[index] : null);
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x000E7CA8 File Offset: 0x000E5EA8
		internal ListLevel AddEntry()
		{
			ListLevel listLevel = new ListLevel();
			this.Add(listLevel);
			return listLevel;
		}

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x06003414 RID: 13332 RVA: 0x000E7CC4 File Offset: 0x000E5EC4
		internal ListLevel CurrentEntry
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
