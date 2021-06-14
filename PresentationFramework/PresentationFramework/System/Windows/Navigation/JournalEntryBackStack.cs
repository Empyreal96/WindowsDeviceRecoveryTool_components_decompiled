using System;
using System.Collections;

namespace System.Windows.Navigation
{
	// Token: 0x020002F8 RID: 760
	internal class JournalEntryBackStack : JournalEntryStack
	{
		// Token: 0x060028AF RID: 10415 RVA: 0x000BD165 File Offset: 0x000BB365
		public JournalEntryBackStack(Journal journal) : base(journal)
		{
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x000BD16E File Offset: 0x000BB36E
		public override IEnumerator GetEnumerator()
		{
			return new JournalEntryStackEnumerator(this._journal, this._journal.CurrentIndex - 1, -1, base.Filter);
		}
	}
}
