using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Data.Services.Client.Serializers
{
	// Token: 0x0200001A RID: 26
	[DebuggerDisplay("EpmTargetPathSegment {SegmentName} HasContent={HasContent}")]
	internal class EpmTargetPathSegment
	{
		// Token: 0x0600008E RID: 142 RVA: 0x0000416C File Offset: 0x0000236C
		internal EpmTargetPathSegment()
		{
			this.subSegments = new List<EpmTargetPathSegment>();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000417F File Offset: 0x0000237F
		internal EpmTargetPathSegment(string segmentName, string segmentNamespaceUri, EpmTargetPathSegment parentSegment) : this()
		{
			this.segmentName = segmentName;
			this.segmentNamespaceUri = segmentNamespaceUri;
			this.parentSegment = parentSegment;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0000419C File Offset: 0x0000239C
		internal string SegmentName
		{
			get
			{
				return this.segmentName;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000041A4 File Offset: 0x000023A4
		internal string SegmentNamespaceUri
		{
			get
			{
				return this.segmentNamespaceUri;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000092 RID: 146 RVA: 0x000041AC File Offset: 0x000023AC
		// (set) Token: 0x06000093 RID: 147 RVA: 0x000041B4 File Offset: 0x000023B4
		internal EntityPropertyMappingInfo EpmInfo { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000094 RID: 148 RVA: 0x000041BD File Offset: 0x000023BD
		internal bool HasContent
		{
			get
			{
				return this.EpmInfo != null;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000041CB File Offset: 0x000023CB
		internal bool IsAttribute
		{
			get
			{
				return !string.IsNullOrEmpty(this.SegmentName) && this.SegmentName[0] == '@';
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000041EC File Offset: 0x000023EC
		internal EpmTargetPathSegment ParentSegment
		{
			get
			{
				return this.parentSegment;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000041F4 File Offset: 0x000023F4
		internal List<EpmTargetPathSegment> SubSegments
		{
			get
			{
				return this.subSegments;
			}
		}

		// Token: 0x04000174 RID: 372
		private readonly string segmentName;

		// Token: 0x04000175 RID: 373
		private readonly string segmentNamespaceUri;

		// Token: 0x04000176 RID: 374
		private readonly List<EpmTargetPathSegment> subSegments;

		// Token: 0x04000177 RID: 375
		private readonly EpmTargetPathSegment parentSegment;
	}
}
