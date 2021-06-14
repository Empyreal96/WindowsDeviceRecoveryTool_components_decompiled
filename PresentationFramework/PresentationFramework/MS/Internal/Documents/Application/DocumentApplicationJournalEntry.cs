using System;
using System.Security;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace MS.Internal.Documents.Application
{
	// Token: 0x020006FE RID: 1790
	[Serializable]
	internal sealed class DocumentApplicationJournalEntry : CustomContentState
	{
		// Token: 0x0600733F RID: 29503 RVA: 0x00210F44 File Offset: 0x0020F144
		public DocumentApplicationJournalEntry(object state, string name)
		{
			Invariant.Assert(state is DocumentApplicationState, "state should be of type DocumentApplicationState");
			this._state = state;
			this._displayName = name;
		}

		// Token: 0x06007340 RID: 29504 RVA: 0x00210F6D File Offset: 0x0020F16D
		public DocumentApplicationJournalEntry(object state) : this(state, null)
		{
		}

		// Token: 0x06007341 RID: 29505 RVA: 0x00210F78 File Offset: 0x0020F178
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public override void Replay(NavigationService navigationService, NavigationMode mode)
		{
			ContentControl contentControl = (ContentControl)navigationService.INavigatorHost;
			contentControl.ApplyTemplate();
			DocumentApplicationDocumentViewer documentApplicationDocumentViewer = contentControl.Template.FindName("PUIDocumentApplicationDocumentViewer", contentControl) as DocumentApplicationDocumentViewer;
			if (documentApplicationDocumentViewer != null)
			{
				if (this._state is DocumentApplicationState)
				{
					documentApplicationDocumentViewer.StoredDocumentApplicationState = (DocumentApplicationState)this._state;
				}
				if (navigationService.Content != null)
				{
					IDocumentPaginatorSource documentPaginatorSource = navigationService.Content as IDocumentPaginatorSource;
					if (documentPaginatorSource != null && documentPaginatorSource.DocumentPaginator.IsPageCountValid)
					{
						documentApplicationDocumentViewer.SetUIToStoredState();
					}
				}
			}
		}

		// Token: 0x17001B59 RID: 7001
		// (get) Token: 0x06007342 RID: 29506 RVA: 0x00210FFA File Offset: 0x0020F1FA
		public override string JournalEntryName
		{
			get
			{
				return this._displayName;
			}
		}

		// Token: 0x0400378A RID: 14218
		private object _state;

		// Token: 0x0400378B RID: 14219
		private string _displayName;
	}
}
