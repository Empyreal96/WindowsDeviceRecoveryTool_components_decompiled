using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Documents;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020007DF RID: 2015
	internal class AnnotationComponentManager : DependencyObject
	{
		// Token: 0x06007C8D RID: 31885 RVA: 0x00230735 File Offset: 0x0022E935
		internal AnnotationComponentManager(AnnotationService service)
		{
			if (service != null)
			{
				service.AttachedAnnotationChanged += this.AttachedAnnotationUpdateEventHandler;
			}
		}

		// Token: 0x06007C8E RID: 31886 RVA: 0x00230760 File Offset: 0x0022E960
		internal void AddAttachedAnnotation(IAttachedAnnotation attachedAnnotation, bool reorder)
		{
			IAnnotationComponent annotationComponent = this.FindComponent(attachedAnnotation);
			if (annotationComponent == null)
			{
				return;
			}
			this.AddComponent(attachedAnnotation, annotationComponent, reorder);
		}

		// Token: 0x06007C8F RID: 31887 RVA: 0x00230784 File Offset: 0x0022E984
		internal void RemoveAttachedAnnotation(IAttachedAnnotation attachedAnnotation, bool reorder)
		{
			if (!this._attachedAnnotations.ContainsKey(attachedAnnotation))
			{
				return;
			}
			IList<IAnnotationComponent> list = this._attachedAnnotations[attachedAnnotation];
			this._attachedAnnotations.Remove(attachedAnnotation);
			foreach (IAnnotationComponent annotationComponent in list)
			{
				annotationComponent.RemoveAttachedAnnotation(attachedAnnotation);
				if (annotationComponent.AttachedAnnotations.Count == 0 && annotationComponent.PresentationContext != null)
				{
					annotationComponent.PresentationContext.RemoveFromHost(annotationComponent, reorder);
				}
			}
		}

		// Token: 0x06007C90 RID: 31888 RVA: 0x00230818 File Offset: 0x0022EA18
		private void AttachedAnnotationUpdateEventHandler(object sender, AttachedAnnotationChangedEventArgs e)
		{
			switch (e.Action)
			{
			case AttachedAnnotationAction.Loaded:
				this.AddAttachedAnnotation(e.AttachedAnnotation, false);
				return;
			case AttachedAnnotationAction.Unloaded:
				this.RemoveAttachedAnnotation(e.AttachedAnnotation, false);
				return;
			case AttachedAnnotationAction.AnchorModified:
				this.ModifyAttachedAnnotation(e.AttachedAnnotation, e.PreviousAttachedAnchor, e.PreviousAttachmentLevel);
				return;
			case AttachedAnnotationAction.Added:
				this.AddAttachedAnnotation(e.AttachedAnnotation, true);
				return;
			case AttachedAnnotationAction.Deleted:
				this.RemoveAttachedAnnotation(e.AttachedAnnotation, true);
				return;
			default:
				return;
			}
		}

		// Token: 0x06007C91 RID: 31889 RVA: 0x00230898 File Offset: 0x0022EA98
		private IAnnotationComponent FindComponent(IAttachedAnnotation attachedAnnotation)
		{
			UIElement d = attachedAnnotation.Parent as UIElement;
			AnnotationComponentChooser chooser = AnnotationService.GetChooser(d);
			return chooser.ChooseAnnotationComponent(attachedAnnotation);
		}

		// Token: 0x06007C92 RID: 31890 RVA: 0x002308C4 File Offset: 0x0022EAC4
		private void AddComponent(IAttachedAnnotation attachedAnnotation, IAnnotationComponent component, bool reorder)
		{
			UIElement uielement = attachedAnnotation.Parent as UIElement;
			if (component.PresentationContext != null)
			{
				return;
			}
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(uielement);
			if (adornerLayer != null)
			{
				this.AddToAttachedAnnotations(attachedAnnotation, component);
				component.AddAttachedAnnotation(attachedAnnotation);
				AdornerPresentationContext.HostComponent(adornerLayer, component, uielement, reorder);
				return;
			}
			if (PresentationSource.FromVisual(uielement) == null)
			{
				return;
			}
			throw new InvalidOperationException(SR.Get("NoPresentationContextForGivenElement", new object[]
			{
				uielement
			}));
		}

		// Token: 0x06007C93 RID: 31891 RVA: 0x0023092C File Offset: 0x0022EB2C
		private void ModifyAttachedAnnotation(IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel)
		{
			if (!this._attachedAnnotations.ContainsKey(attachedAnnotation))
			{
				this.AddAttachedAnnotation(attachedAnnotation, true);
				return;
			}
			IAnnotationComponent annotationComponent = this.FindComponent(attachedAnnotation);
			if (annotationComponent == null)
			{
				this.RemoveAttachedAnnotation(attachedAnnotation, true);
				return;
			}
			IList<IAnnotationComponent> list = this._attachedAnnotations[attachedAnnotation];
			if (list.Contains(annotationComponent))
			{
				using (IEnumerator<IAnnotationComponent> enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IAnnotationComponent annotationComponent2 = enumerator.Current;
						annotationComponent2.ModifyAttachedAnnotation(attachedAnnotation, previousAttachedAnchor, previousAttachmentLevel);
						if (annotationComponent2.AttachedAnnotations.Count == 0)
						{
							annotationComponent2.PresentationContext.RemoveFromHost(annotationComponent2, true);
						}
					}
					return;
				}
			}
			this.RemoveAttachedAnnotation(attachedAnnotation, true);
			this.AddComponent(attachedAnnotation, annotationComponent, true);
		}

		// Token: 0x06007C94 RID: 31892 RVA: 0x002309E4 File Offset: 0x0022EBE4
		private void AddToAttachedAnnotations(IAttachedAnnotation attachedAnnotation, IAnnotationComponent component)
		{
			IList<IAnnotationComponent> list;
			if (!this._attachedAnnotations.TryGetValue(attachedAnnotation, out list))
			{
				list = new List<IAnnotationComponent>();
				this._attachedAnnotations[attachedAnnotation] = list;
			}
			list.Add(component);
		}

		// Token: 0x04003A6A RID: 14954
		private Dictionary<IAttachedAnnotation, IList<IAnnotationComponent>> _attachedAnnotations = new Dictionary<IAttachedAnnotation, IList<IAnnotationComponent>>();
	}
}
