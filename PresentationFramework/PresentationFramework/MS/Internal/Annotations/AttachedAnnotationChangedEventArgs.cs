using System;

namespace MS.Internal.Annotations
{
	// Token: 0x020007CA RID: 1994
	internal class AttachedAnnotationChangedEventArgs : EventArgs
	{
		// Token: 0x06007B8E RID: 31630 RVA: 0x0022BBF5 File Offset: 0x00229DF5
		internal AttachedAnnotationChangedEventArgs(AttachedAnnotationAction action, IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel)
		{
			Invariant.Assert(attachedAnnotation != null);
			this._action = action;
			this._attachedAnnotation = attachedAnnotation;
			this._previousAttachedAnchor = previousAttachedAnchor;
			this._previousAttachmentLevel = previousAttachmentLevel;
		}

		// Token: 0x17001CD4 RID: 7380
		// (get) Token: 0x06007B8F RID: 31631 RVA: 0x0022BC23 File Offset: 0x00229E23
		public AttachedAnnotationAction Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x17001CD5 RID: 7381
		// (get) Token: 0x06007B90 RID: 31632 RVA: 0x0022BC2B File Offset: 0x00229E2B
		public IAttachedAnnotation AttachedAnnotation
		{
			get
			{
				return this._attachedAnnotation;
			}
		}

		// Token: 0x17001CD6 RID: 7382
		// (get) Token: 0x06007B91 RID: 31633 RVA: 0x0022BC33 File Offset: 0x00229E33
		public object PreviousAttachedAnchor
		{
			get
			{
				return this._previousAttachedAnchor;
			}
		}

		// Token: 0x17001CD7 RID: 7383
		// (get) Token: 0x06007B92 RID: 31634 RVA: 0x0022BC3B File Offset: 0x00229E3B
		public AttachmentLevel PreviousAttachmentLevel
		{
			get
			{
				return this._previousAttachmentLevel;
			}
		}

		// Token: 0x06007B93 RID: 31635 RVA: 0x0022BC43 File Offset: 0x00229E43
		internal static AttachedAnnotationChangedEventArgs Added(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null);
			return new AttachedAnnotationChangedEventArgs(AttachedAnnotationAction.Added, attachedAnnotation, null, AttachmentLevel.Unresolved);
		}

		// Token: 0x06007B94 RID: 31636 RVA: 0x0022BC57 File Offset: 0x00229E57
		internal static AttachedAnnotationChangedEventArgs Loaded(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null);
			return new AttachedAnnotationChangedEventArgs(AttachedAnnotationAction.Loaded, attachedAnnotation, null, AttachmentLevel.Unresolved);
		}

		// Token: 0x06007B95 RID: 31637 RVA: 0x0022BC6B File Offset: 0x00229E6B
		internal static AttachedAnnotationChangedEventArgs Deleted(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null);
			return new AttachedAnnotationChangedEventArgs(AttachedAnnotationAction.Deleted, attachedAnnotation, null, AttachmentLevel.Unresolved);
		}

		// Token: 0x06007B96 RID: 31638 RVA: 0x0022BC7F File Offset: 0x00229E7F
		internal static AttachedAnnotationChangedEventArgs Unloaded(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null);
			return new AttachedAnnotationChangedEventArgs(AttachedAnnotationAction.Unloaded, attachedAnnotation, null, AttachmentLevel.Unresolved);
		}

		// Token: 0x06007B97 RID: 31639 RVA: 0x0022BC93 File Offset: 0x00229E93
		internal static AttachedAnnotationChangedEventArgs Modified(IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel)
		{
			Invariant.Assert(attachedAnnotation != null && previousAttachedAnchor != null);
			return new AttachedAnnotationChangedEventArgs(AttachedAnnotationAction.AnchorModified, attachedAnnotation, previousAttachedAnchor, previousAttachmentLevel);
		}

		// Token: 0x04003A2D RID: 14893
		private AttachedAnnotationAction _action;

		// Token: 0x04003A2E RID: 14894
		private IAttachedAnnotation _attachedAnnotation;

		// Token: 0x04003A2F RID: 14895
		private object _previousAttachedAnchor;

		// Token: 0x04003A30 RID: 14896
		private AttachmentLevel _previousAttachmentLevel;
	}
}
