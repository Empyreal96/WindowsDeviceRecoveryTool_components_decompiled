using System;
using System.Collections.Generic;

namespace MS.Internal.Annotations
{
	// Token: 0x020007C2 RID: 1986
	internal class AnnotationMap
	{
		// Token: 0x06007B6C RID: 31596 RVA: 0x0022B70C File Offset: 0x0022990C
		internal void AddAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			List<IAttachedAnnotation> list = null;
			if (!this._annotationIdToAttachedAnnotations.TryGetValue(attachedAnnotation.Annotation.Id, out list))
			{
				list = new List<IAttachedAnnotation>(1);
				this._annotationIdToAttachedAnnotations.Add(attachedAnnotation.Annotation.Id, list);
			}
			list.Add(attachedAnnotation);
		}

		// Token: 0x06007B6D RID: 31597 RVA: 0x0022B75C File Offset: 0x0022995C
		internal void RemoveAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			List<IAttachedAnnotation> list = null;
			if (this._annotationIdToAttachedAnnotations.TryGetValue(attachedAnnotation.Annotation.Id, out list))
			{
				list.Remove(attachedAnnotation);
				if (list.Count == 0)
				{
					this._annotationIdToAttachedAnnotations.Remove(attachedAnnotation.Annotation.Id);
				}
			}
		}

		// Token: 0x17001CCD RID: 7373
		// (get) Token: 0x06007B6E RID: 31598 RVA: 0x0022B7AC File Offset: 0x002299AC
		internal bool IsEmpty
		{
			get
			{
				return this._annotationIdToAttachedAnnotations.Count == 0;
			}
		}

		// Token: 0x06007B6F RID: 31599 RVA: 0x0022B7BC File Offset: 0x002299BC
		internal List<IAttachedAnnotation> GetAttachedAnnotations(Guid annotationId)
		{
			List<IAttachedAnnotation> result = null;
			if (!this._annotationIdToAttachedAnnotations.TryGetValue(annotationId, out result))
			{
				return AnnotationMap._emptyList;
			}
			return result;
		}

		// Token: 0x06007B70 RID: 31600 RVA: 0x0022B7E4 File Offset: 0x002299E4
		internal List<IAttachedAnnotation> GetAllAttachedAnnotations()
		{
			List<IAttachedAnnotation> list = new List<IAttachedAnnotation>(this._annotationIdToAttachedAnnotations.Keys.Count);
			foreach (Guid key in this._annotationIdToAttachedAnnotations.Keys)
			{
				List<IAttachedAnnotation> collection = this._annotationIdToAttachedAnnotations[key];
				list.AddRange(collection);
			}
			if (list.Count == 0)
			{
				return AnnotationMap._emptyList;
			}
			return list;
		}

		// Token: 0x04003A1A RID: 14874
		private Dictionary<Guid, List<IAttachedAnnotation>> _annotationIdToAttachedAnnotations = new Dictionary<Guid, List<IAttachedAnnotation>>();

		// Token: 0x04003A1B RID: 14875
		private static readonly List<IAttachedAnnotation> _emptyList = new List<IAttachedAnnotation>(0);
	}
}
