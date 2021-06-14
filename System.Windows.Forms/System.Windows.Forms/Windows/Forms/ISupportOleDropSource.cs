using System;

namespace System.Windows.Forms
{
	// Token: 0x02000292 RID: 658
	internal interface ISupportOleDropSource
	{
		// Token: 0x060026DA RID: 9946
		void OnQueryContinueDrag(QueryContinueDragEventArgs qcdevent);

		// Token: 0x060026DB RID: 9947
		void OnGiveFeedback(GiveFeedbackEventArgs gfbevent);
	}
}
