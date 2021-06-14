using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Data.OData.Metadata
{
	// Token: 0x02000211 RID: 529
	[DebuggerDisplay("EpmTargetPathSegment {SegmentName} HasContent={HasContent}")]
	internal sealed class EpmTargetPathSegment
	{
		// Token: 0x06001049 RID: 4169 RVA: 0x0003B77F File Offset: 0x0003997F
		internal EpmTargetPathSegment()
		{
			this.subSegments = new List<EpmTargetPathSegment>();
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0003B794 File Offset: 0x00039994
		internal EpmTargetPathSegment(string segmentName, string segmentNamespaceUri, string segmentNamespacePrefix, EpmTargetPathSegment parentSegment) : this()
		{
			this.segmentName = segmentName;
			this.segmentNamespaceUri = segmentNamespaceUri;
			this.segmentNamespacePrefix = segmentNamespacePrefix;
			this.parentSegment = parentSegment;
			if (!string.IsNullOrEmpty(segmentName) && segmentName[0] == '@')
			{
				this.segmentAttributeName = segmentName.Substring(1);
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x0600104B RID: 4171 RVA: 0x0003B7E4 File Offset: 0x000399E4
		internal string SegmentName
		{
			get
			{
				return this.segmentName;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x0600104C RID: 4172 RVA: 0x0003B7EC File Offset: 0x000399EC
		internal string AttributeName
		{
			get
			{
				return this.segmentAttributeName;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x0600104D RID: 4173 RVA: 0x0003B7F4 File Offset: 0x000399F4
		internal string SegmentNamespaceUri
		{
			get
			{
				return this.segmentNamespaceUri;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x0600104E RID: 4174 RVA: 0x0003B7FC File Offset: 0x000399FC
		internal string SegmentNamespacePrefix
		{
			get
			{
				return this.segmentNamespacePrefix;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x0600104F RID: 4175 RVA: 0x0003B804 File Offset: 0x00039A04
		// (set) Token: 0x06001050 RID: 4176 RVA: 0x0003B80C File Offset: 0x00039A0C
		internal EntityPropertyMappingInfo EpmInfo
		{
			get
			{
				return this.epmInfo;
			}
			set
			{
				this.epmInfo = value;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001051 RID: 4177 RVA: 0x0003B815 File Offset: 0x00039A15
		internal bool HasContent
		{
			get
			{
				return this.EpmInfo != null;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001052 RID: 4178 RVA: 0x0003B823 File Offset: 0x00039A23
		internal bool IsAttribute
		{
			get
			{
				return this.segmentAttributeName != null;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001053 RID: 4179 RVA: 0x0003B831 File Offset: 0x00039A31
		internal EpmTargetPathSegment ParentSegment
		{
			get
			{
				return this.parentSegment;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001054 RID: 4180 RVA: 0x0003B839 File Offset: 0x00039A39
		internal List<EpmTargetPathSegment> SubSegments
		{
			get
			{
				return this.subSegments;
			}
		}

		// Token: 0x040005F7 RID: 1527
		private readonly string segmentName;

		// Token: 0x040005F8 RID: 1528
		private readonly string segmentAttributeName;

		// Token: 0x040005F9 RID: 1529
		private readonly string segmentNamespaceUri;

		// Token: 0x040005FA RID: 1530
		private readonly string segmentNamespacePrefix;

		// Token: 0x040005FB RID: 1531
		private readonly List<EpmTargetPathSegment> subSegments;

		// Token: 0x040005FC RID: 1532
		private readonly EpmTargetPathSegment parentSegment;

		// Token: 0x040005FD RID: 1533
		private EntityPropertyMappingInfo epmInfo;
	}
}
