using System;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Annotations.Storage;

namespace MS.Internal.Annotations
{
	// Token: 0x020007C8 RID: 1992
	internal interface IAttachedAnnotation : IAnchorInfo
	{
		// Token: 0x17001CCE RID: 7374
		// (get) Token: 0x06007B87 RID: 31623
		object AttachedAnchor { get; }

		// Token: 0x17001CCF RID: 7375
		// (get) Token: 0x06007B88 RID: 31624
		object FullyAttachedAnchor { get; }

		// Token: 0x17001CD0 RID: 7376
		// (get) Token: 0x06007B89 RID: 31625
		AttachmentLevel AttachmentLevel { get; }

		// Token: 0x17001CD1 RID: 7377
		// (get) Token: 0x06007B8A RID: 31626
		DependencyObject Parent { get; }

		// Token: 0x17001CD2 RID: 7378
		// (get) Token: 0x06007B8B RID: 31627
		Point AnchorPoint { get; }

		// Token: 0x06007B8C RID: 31628
		bool IsAnchorEqual(object o);

		// Token: 0x17001CD3 RID: 7379
		// (get) Token: 0x06007B8D RID: 31629
		AnnotationStore Store { get; }
	}
}
