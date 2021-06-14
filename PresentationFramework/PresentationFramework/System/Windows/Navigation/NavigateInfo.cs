using System;

namespace System.Windows.Navigation
{
	// Token: 0x0200031A RID: 794
	internal class NavigateInfo
	{
		// Token: 0x060029F9 RID: 10745 RVA: 0x000C1B25 File Offset: 0x000BFD25
		internal NavigateInfo(Uri source)
		{
			this._source = source;
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x000C1B34 File Offset: 0x000BFD34
		internal NavigateInfo(Uri source, NavigationMode navigationMode)
		{
			this._source = source;
			this._navigationMode = navigationMode;
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x000C1B4A File Offset: 0x000BFD4A
		internal NavigateInfo(Uri source, NavigationMode navigationMode, JournalEntry journalEntry)
		{
			this._source = source;
			this._navigationMode = navigationMode;
			this._journalEntry = journalEntry;
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x060029FC RID: 10748 RVA: 0x000C1B67 File Offset: 0x000BFD67
		internal Uri Source
		{
			get
			{
				return this._source;
			}
		}

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x060029FD RID: 10749 RVA: 0x000C1B6F File Offset: 0x000BFD6F
		internal NavigationMode NavigationMode
		{
			get
			{
				return this._navigationMode;
			}
		}

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x060029FE RID: 10750 RVA: 0x000C1B77 File Offset: 0x000BFD77
		internal JournalEntry JournalEntry
		{
			get
			{
				return this._journalEntry;
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x060029FF RID: 10751 RVA: 0x000C1B7F File Offset: 0x000BFD7F
		internal bool IsConsistent
		{
			get
			{
				return (this._navigationMode == NavigationMode.New ^ this._journalEntry != null) || this._navigationMode == NavigationMode.Refresh;
			}
		}

		// Token: 0x04001C20 RID: 7200
		private Uri _source;

		// Token: 0x04001C21 RID: 7201
		private NavigationMode _navigationMode;

		// Token: 0x04001C22 RID: 7202
		private JournalEntry _journalEntry;
	}
}
