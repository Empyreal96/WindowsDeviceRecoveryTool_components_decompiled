using System;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Annotations.Storage;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020007D3 RID: 2003
	internal class AttachedAnnotation : IAttachedAnnotation, IAnchorInfo
	{
		// Token: 0x06007BE3 RID: 31715 RVA: 0x0022D597 File Offset: 0x0022B797
		internal AttachedAnnotation(LocatorManager manager, Annotation annotation, AnnotationResource anchor, object attachedAnchor, AttachmentLevel attachmentLevel) : this(manager, annotation, anchor, attachedAnchor, attachmentLevel, null)
		{
		}

		// Token: 0x06007BE4 RID: 31716 RVA: 0x0022D5A7 File Offset: 0x0022B7A7
		internal AttachedAnnotation(LocatorManager manager, Annotation annotation, AnnotationResource anchor, object attachedAnchor, AttachmentLevel attachmentLevel, DependencyObject parent)
		{
			this._annotation = annotation;
			this._anchor = anchor;
			this._locatorManager = manager;
			this.Update(attachedAnchor, attachmentLevel, parent);
		}

		// Token: 0x06007BE5 RID: 31717 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsAnchorEqual(object o)
		{
			return false;
		}

		// Token: 0x17001CDF RID: 7391
		// (get) Token: 0x06007BE6 RID: 31718 RVA: 0x0022D5D0 File Offset: 0x0022B7D0
		public Annotation Annotation
		{
			get
			{
				return this._annotation;
			}
		}

		// Token: 0x17001CE0 RID: 7392
		// (get) Token: 0x06007BE7 RID: 31719 RVA: 0x0022D5D8 File Offset: 0x0022B7D8
		public AnnotationResource Anchor
		{
			get
			{
				return this._anchor;
			}
		}

		// Token: 0x17001CE1 RID: 7393
		// (get) Token: 0x06007BE8 RID: 31720 RVA: 0x0022D5E0 File Offset: 0x0022B7E0
		public object AttachedAnchor
		{
			get
			{
				return this._attachedAnchor;
			}
		}

		// Token: 0x17001CE2 RID: 7394
		// (get) Token: 0x06007BE9 RID: 31721 RVA: 0x0022D5E8 File Offset: 0x0022B7E8
		public object ResolvedAnchor
		{
			get
			{
				return this.FullyAttachedAnchor;
			}
		}

		// Token: 0x17001CE3 RID: 7395
		// (get) Token: 0x06007BEA RID: 31722 RVA: 0x0022D5F0 File Offset: 0x0022B7F0
		public object FullyAttachedAnchor
		{
			get
			{
				if (this._attachmentLevel == AttachmentLevel.Full)
				{
					return this._attachedAnchor;
				}
				return this._fullyAttachedAnchor;
			}
		}

		// Token: 0x17001CE4 RID: 7396
		// (get) Token: 0x06007BEB RID: 31723 RVA: 0x0022D608 File Offset: 0x0022B808
		public AttachmentLevel AttachmentLevel
		{
			get
			{
				return this._attachmentLevel;
			}
		}

		// Token: 0x17001CE5 RID: 7397
		// (get) Token: 0x06007BEC RID: 31724 RVA: 0x0022D610 File Offset: 0x0022B810
		public DependencyObject Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x17001CE6 RID: 7398
		// (get) Token: 0x06007BED RID: 31725 RVA: 0x0022D618 File Offset: 0x0022B818
		public Point AnchorPoint
		{
			get
			{
				Point anchorPoint = this._selectionProcessor.GetAnchorPoint(this._attachedAnchor);
				if (!double.IsInfinity(anchorPoint.X) && !double.IsInfinity(anchorPoint.Y))
				{
					this._cachedPoint = anchorPoint;
				}
				return this._cachedPoint;
			}
		}

		// Token: 0x17001CE7 RID: 7399
		// (get) Token: 0x06007BEE RID: 31726 RVA: 0x0022D660 File Offset: 0x0022B860
		public AnnotationStore Store
		{
			get
			{
				return this.GetStore();
			}
		}

		// Token: 0x06007BEF RID: 31727 RVA: 0x0022D668 File Offset: 0x0022B868
		internal void Update(object attachedAnchor, AttachmentLevel attachmentLevel, DependencyObject parent)
		{
			this._attachedAnchor = attachedAnchor;
			this._attachmentLevel = attachmentLevel;
			this._selectionProcessor = this._locatorManager.GetSelectionProcessor(attachedAnchor.GetType());
			if (parent != null)
			{
				this._parent = parent;
				return;
			}
			this._parent = this._selectionProcessor.GetParent(this._attachedAnchor);
		}

		// Token: 0x06007BF0 RID: 31728 RVA: 0x0022D6BC File Offset: 0x0022B8BC
		internal void SetFullyAttachedAnchor(object fullyAttachedAnchor)
		{
			this._fullyAttachedAnchor = fullyAttachedAnchor;
		}

		// Token: 0x06007BF1 RID: 31729 RVA: 0x0022D6C8 File Offset: 0x0022B8C8
		private AnnotationStore GetStore()
		{
			if (this.Parent != null)
			{
				AnnotationService service = AnnotationService.GetService(this.Parent);
				if (service != null)
				{
					return service.Store;
				}
			}
			return null;
		}

		// Token: 0x04003A41 RID: 14913
		private Annotation _annotation;

		// Token: 0x04003A42 RID: 14914
		private AnnotationResource _anchor;

		// Token: 0x04003A43 RID: 14915
		private object _attachedAnchor;

		// Token: 0x04003A44 RID: 14916
		private object _fullyAttachedAnchor;

		// Token: 0x04003A45 RID: 14917
		private AttachmentLevel _attachmentLevel;

		// Token: 0x04003A46 RID: 14918
		private DependencyObject _parent;

		// Token: 0x04003A47 RID: 14919
		private SelectionProcessor _selectionProcessor;

		// Token: 0x04003A48 RID: 14920
		private LocatorManager _locatorManager;

		// Token: 0x04003A49 RID: 14921
		private Point _cachedPoint;
	}
}
