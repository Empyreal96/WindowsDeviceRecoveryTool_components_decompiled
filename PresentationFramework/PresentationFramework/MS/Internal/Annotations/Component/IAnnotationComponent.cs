using System;
using System.Collections;
using System.Windows;
using System.Windows.Media;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020007E4 RID: 2020
	internal interface IAnnotationComponent
	{
		// Token: 0x17001D0E RID: 7438
		// (get) Token: 0x06007CF7 RID: 31991
		IList AttachedAnnotations { get; }

		// Token: 0x17001D0F RID: 7439
		// (get) Token: 0x06007CF8 RID: 31992
		// (set) Token: 0x06007CF9 RID: 31993
		PresentationContext PresentationContext { get; set; }

		// Token: 0x17001D10 RID: 7440
		// (get) Token: 0x06007CFA RID: 31994
		// (set) Token: 0x06007CFB RID: 31995
		int ZOrder { get; set; }

		// Token: 0x17001D11 RID: 7441
		// (get) Token: 0x06007CFC RID: 31996
		// (set) Token: 0x06007CFD RID: 31997
		bool IsDirty { get; set; }

		// Token: 0x06007CFE RID: 31998
		GeneralTransform GetDesiredTransform(GeneralTransform transform);

		// Token: 0x17001D12 RID: 7442
		// (get) Token: 0x06007CFF RID: 31999
		UIElement AnnotatedElement { get; }

		// Token: 0x06007D00 RID: 32000
		void AddAttachedAnnotation(IAttachedAnnotation attachedAnnotation);

		// Token: 0x06007D01 RID: 32001
		void RemoveAttachedAnnotation(IAttachedAnnotation attachedAnnotation);

		// Token: 0x06007D02 RID: 32002
		void ModifyAttachedAnnotation(IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel);
	}
}
