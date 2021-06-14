using System;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006CE RID: 1742
	internal interface IFlowDocumentViewer
	{
		// Token: 0x06007085 RID: 28805
		void PreviousPage();

		// Token: 0x06007086 RID: 28806
		void NextPage();

		// Token: 0x06007087 RID: 28807
		void FirstPage();

		// Token: 0x06007088 RID: 28808
		void LastPage();

		// Token: 0x06007089 RID: 28809
		void Print();

		// Token: 0x0600708A RID: 28810
		void CancelPrint();

		// Token: 0x0600708B RID: 28811
		void ShowFindResult(ITextRange findResult);

		// Token: 0x0600708C RID: 28812
		bool CanGoToPage(int pageNumber);

		// Token: 0x0600708D RID: 28813
		void GoToPage(int pageNumber);

		// Token: 0x0600708E RID: 28814
		void SetDocument(FlowDocument document);

		// Token: 0x17001ABE RID: 6846
		// (get) Token: 0x0600708F RID: 28815
		// (set) Token: 0x06007090 RID: 28816
		ContentPosition ContentPosition { get; set; }

		// Token: 0x17001ABF RID: 6847
		// (get) Token: 0x06007091 RID: 28817
		// (set) Token: 0x06007092 RID: 28818
		ITextSelection TextSelection { get; set; }

		// Token: 0x17001AC0 RID: 6848
		// (get) Token: 0x06007093 RID: 28819
		bool CanGoToPreviousPage { get; }

		// Token: 0x17001AC1 RID: 6849
		// (get) Token: 0x06007094 RID: 28820
		bool CanGoToNextPage { get; }

		// Token: 0x17001AC2 RID: 6850
		// (get) Token: 0x06007095 RID: 28821
		int PageNumber { get; }

		// Token: 0x17001AC3 RID: 6851
		// (get) Token: 0x06007096 RID: 28822
		int PageCount { get; }

		// Token: 0x1400013C RID: 316
		// (add) Token: 0x06007097 RID: 28823
		// (remove) Token: 0x06007098 RID: 28824
		event EventHandler PageNumberChanged;

		// Token: 0x1400013D RID: 317
		// (add) Token: 0x06007099 RID: 28825
		// (remove) Token: 0x0600709A RID: 28826
		event EventHandler PageCountChanged;

		// Token: 0x1400013E RID: 318
		// (add) Token: 0x0600709B RID: 28827
		// (remove) Token: 0x0600709C RID: 28828
		event EventHandler PrintStarted;

		// Token: 0x1400013F RID: 319
		// (add) Token: 0x0600709D RID: 28829
		// (remove) Token: 0x0600709E RID: 28830
		event EventHandler PrintCompleted;
	}
}
