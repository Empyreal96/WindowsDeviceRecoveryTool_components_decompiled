using System;

namespace MS.Internal.AppModel
{
	// Token: 0x02000786 RID: 1926
	internal interface IJournalState
	{
		// Token: 0x06007948 RID: 31048
		CustomJournalStateInternal GetJournalState(JournalReason journalReason);

		// Token: 0x06007949 RID: 31049
		void RestoreJournalState(CustomJournalStateInternal state);
	}
}
