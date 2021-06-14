using System;
using System.Collections;

namespace System.Windows.Navigation
{
	// Token: 0x020002F9 RID: 761
	internal class JournalEntryForwardStack : JournalEntryStack
	{
		// Token: 0x060028B1 RID: 10417 RVA: 0x000BD165 File Offset: 0x000BB365
		public JournalEntryForwardStack(Journal journal) : base(journal)
		{
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x000BD18F File Offset: 0x000BB38F
		public override IEnumerator GetEnumerator()
		{
			return new JournalEntryStackEnumerator(this._journal, this._journal.CurrentIndex + 1, 1, base.Filter);
		}
	}
}
