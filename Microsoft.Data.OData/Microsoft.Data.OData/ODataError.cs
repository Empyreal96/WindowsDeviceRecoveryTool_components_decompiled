using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Data.OData
{
	// Token: 0x0200028E RID: 654
	[DebuggerDisplay("{ErrorCode}: {Message}")]
	[Serializable]
	public sealed class ODataError : ODataAnnotatable
	{
		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001606 RID: 5638 RVA: 0x0005072D File Offset: 0x0004E92D
		// (set) Token: 0x06001607 RID: 5639 RVA: 0x00050735 File Offset: 0x0004E935
		public string ErrorCode { get; set; }

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001608 RID: 5640 RVA: 0x0005073E File Offset: 0x0004E93E
		// (set) Token: 0x06001609 RID: 5641 RVA: 0x00050746 File Offset: 0x0004E946
		public string Message { get; set; }

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x0600160A RID: 5642 RVA: 0x0005074F File Offset: 0x0004E94F
		// (set) Token: 0x0600160B RID: 5643 RVA: 0x00050757 File Offset: 0x0004E957
		public string MessageLanguage { get; set; }

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x0600160C RID: 5644 RVA: 0x00050760 File Offset: 0x0004E960
		// (set) Token: 0x0600160D RID: 5645 RVA: 0x00050768 File Offset: 0x0004E968
		public ODataInnerError InnerError { get; set; }

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x0600160E RID: 5646 RVA: 0x00050771 File Offset: 0x0004E971
		// (set) Token: 0x0600160F RID: 5647 RVA: 0x00050779 File Offset: 0x0004E979
		public ICollection<ODataInstanceAnnotation> InstanceAnnotations
		{
			get
			{
				return base.GetInstanceAnnotations();
			}
			set
			{
				base.SetInstanceAnnotations(value);
			}
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x00050782 File Offset: 0x0004E982
		internal override void VerifySetAnnotation(object annotation)
		{
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x0005079C File Offset: 0x0004E99C
		internal IEnumerable<ODataInstanceAnnotation> GetInstanceAnnotationsForWriting()
		{
			if (this.InstanceAnnotations.Count > 0)
			{
				return this.InstanceAnnotations;
			}
			InstanceAnnotationCollection annotation = base.GetAnnotation<InstanceAnnotationCollection>();
			if (annotation != null && annotation.Count > 0)
			{
				return from ia in annotation
				select new ODataInstanceAnnotation(ia.Key, ia.Value);
			}
			return Enumerable.Empty<ODataInstanceAnnotation>();
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x000507FC File Offset: 0x0004E9FC
		internal void AddInstanceAnnotationForReading(string instanceAnnotationName, object instanceAnnotationValue)
		{
			ODataValue value = instanceAnnotationValue.ToODataValue();
			base.GetOrCreateAnnotation<InstanceAnnotationCollection>().Add(instanceAnnotationName, value);
			this.InstanceAnnotations.Add(new ODataInstanceAnnotation(instanceAnnotationName, value));
		}
	}
}
