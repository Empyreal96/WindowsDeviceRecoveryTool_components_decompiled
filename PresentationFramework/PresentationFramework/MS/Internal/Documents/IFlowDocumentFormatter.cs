using System;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006CD RID: 1741
	internal interface IFlowDocumentFormatter
	{
		// Token: 0x06007081 RID: 28801
		void OnContentInvalidated(bool affectsLayout);

		// Token: 0x06007082 RID: 28802
		void OnContentInvalidated(bool affectsLayout, ITextPointer start, ITextPointer end);

		// Token: 0x06007083 RID: 28803
		void Suspend();

		// Token: 0x17001ABD RID: 6845
		// (get) Token: 0x06007084 RID: 28804
		bool IsLayoutDataValid { get; }
	}
}
