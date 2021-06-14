using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000787 RID: 1927
	[Serializable]
	internal class JournalEntryGroupState
	{
		// Token: 0x0600794A RID: 31050 RVA: 0x0000326D File Offset: 0x0000146D
		internal JournalEntryGroupState()
		{
		}

		// Token: 0x0600794B RID: 31051 RVA: 0x00227131 File Offset: 0x00225331
		internal JournalEntryGroupState(Guid navSvcId, uint contentId)
		{
			this._navigationServiceId = navSvcId;
			this._contentId = contentId;
		}

		// Token: 0x17001CA6 RID: 7334
		// (get) Token: 0x0600794C RID: 31052 RVA: 0x00227147 File Offset: 0x00225347
		// (set) Token: 0x0600794D RID: 31053 RVA: 0x0022714F File Offset: 0x0022534F
		internal Guid NavigationServiceId
		{
			get
			{
				return this._navigationServiceId;
			}
			set
			{
				this._navigationServiceId = value;
			}
		}

		// Token: 0x17001CA7 RID: 7335
		// (get) Token: 0x0600794E RID: 31054 RVA: 0x00227158 File Offset: 0x00225358
		// (set) Token: 0x0600794F RID: 31055 RVA: 0x00227160 File Offset: 0x00225360
		internal uint ContentId
		{
			get
			{
				return this._contentId;
			}
			set
			{
				this._contentId = value;
			}
		}

		// Token: 0x17001CA8 RID: 7336
		// (get) Token: 0x06007950 RID: 31056 RVA: 0x00227169 File Offset: 0x00225369
		// (set) Token: 0x06007951 RID: 31057 RVA: 0x00227171 File Offset: 0x00225371
		internal DataStreams JournalDataStreams
		{
			get
			{
				return this._journalDataStreams;
			}
			set
			{
				this._journalDataStreams = value;
			}
		}

		// Token: 0x17001CA9 RID: 7337
		// (get) Token: 0x06007952 RID: 31058 RVA: 0x0022717A File Offset: 0x0022537A
		// (set) Token: 0x06007953 RID: 31059 RVA: 0x00227182 File Offset: 0x00225382
		internal JournalEntry GroupExitEntry
		{
			get
			{
				return this._groupExitEntry;
			}
			set
			{
				this._groupExitEntry = value;
			}
		}

		// Token: 0x0400397B RID: 14715
		private Guid _navigationServiceId;

		// Token: 0x0400397C RID: 14716
		private uint _contentId;

		// Token: 0x0400397D RID: 14717
		private DataStreams _journalDataStreams;

		// Token: 0x0400397E RID: 14718
		private JournalEntry _groupExitEntry;
	}
}
