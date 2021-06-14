using System;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000622 RID: 1570
	internal abstract class FloaterBaseParaClient : BaseParaClient
	{
		// Token: 0x06006809 RID: 26633 RVA: 0x001CF362 File Offset: 0x001CD562
		protected FloaterBaseParaClient(FloaterBaseParagraph paragraph) : base(paragraph)
		{
		}

		// Token: 0x0600680A RID: 26634 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void ArrangeFloater(PTS.FSRECT rcFloater, PTS.FSRECT rcHostPara, uint fswdirParent, PageContext pageContext)
		{
		}

		// Token: 0x0600680B RID: 26635
		internal abstract override TextContentRange GetTextContentRange();
	}
}
