using System;
using System.Windows;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020007E5 RID: 2021
	internal abstract class PresentationContext
	{
		// Token: 0x17001D13 RID: 7443
		// (get) Token: 0x06007D03 RID: 32003
		public abstract UIElement Host { get; }

		// Token: 0x17001D14 RID: 7444
		// (get) Token: 0x06007D04 RID: 32004
		public abstract PresentationContext EnclosingContext { get; }

		// Token: 0x06007D05 RID: 32005
		public abstract void AddToHost(IAnnotationComponent component);

		// Token: 0x06007D06 RID: 32006
		public abstract void RemoveFromHost(IAnnotationComponent component, bool reorder);

		// Token: 0x06007D07 RID: 32007
		public abstract void InvalidateTransform(IAnnotationComponent component);

		// Token: 0x06007D08 RID: 32008
		public abstract void BringToFront(IAnnotationComponent component);

		// Token: 0x06007D09 RID: 32009
		public abstract void SendToBack(IAnnotationComponent component);
	}
}
