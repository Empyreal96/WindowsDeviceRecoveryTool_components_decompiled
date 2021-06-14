using System;
using System.Windows.Controls;
using MS.Internal.Annotations;
using MS.Internal.Annotations.Component;

namespace System.Windows.Annotations
{
	// Token: 0x020005C8 RID: 1480
	internal sealed class AnnotationComponentChooser
	{
		// Token: 0x0600628D RID: 25229 RVA: 0x001BA854 File Offset: 0x001B8A54
		public IAnnotationComponent ChooseAnnotationComponent(IAttachedAnnotation attachedAnnotation)
		{
			if (attachedAnnotation == null)
			{
				throw new ArgumentNullException("attachedAnnotation");
			}
			IAnnotationComponent result = null;
			if (attachedAnnotation.Annotation.AnnotationType == StickyNoteControl.TextSchemaName)
			{
				result = new StickyNoteControl(StickyNoteType.Text);
			}
			else if (attachedAnnotation.Annotation.AnnotationType == StickyNoteControl.InkSchemaName)
			{
				result = new StickyNoteControl(StickyNoteType.Ink);
			}
			else if (attachedAnnotation.Annotation.AnnotationType == HighlightComponent.TypeName)
			{
				result = new HighlightComponent();
			}
			return result;
		}
	}
}
