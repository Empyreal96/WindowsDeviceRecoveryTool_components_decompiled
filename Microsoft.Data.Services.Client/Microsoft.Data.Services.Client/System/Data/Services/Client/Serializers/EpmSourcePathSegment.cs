using System;
using System.Collections.Generic;

namespace System.Data.Services.Client.Serializers
{
	// Token: 0x02000018 RID: 24
	internal class EpmSourcePathSegment
	{
		// Token: 0x0600007C RID: 124 RVA: 0x00003C16 File Offset: 0x00001E16
		internal EpmSourcePathSegment()
		{
			this.propertyName = null;
			this.subProperties = new List<EpmSourcePathSegment>();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003C30 File Offset: 0x00001E30
		internal EpmSourcePathSegment(string propertyName)
		{
			this.propertyName = propertyName;
			this.subProperties = new List<EpmSourcePathSegment>();
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00003C4A File Offset: 0x00001E4A
		internal string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003C52 File Offset: 0x00001E52
		internal List<EpmSourcePathSegment> SubProperties
		{
			get
			{
				return this.subProperties;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003C5A File Offset: 0x00001E5A
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00003C62 File Offset: 0x00001E62
		internal EntityPropertyMappingInfo EpmInfo { get; set; }

		// Token: 0x0400016D RID: 365
		private readonly string propertyName;

		// Token: 0x0400016E RID: 366
		private readonly List<EpmSourcePathSegment> subProperties;
	}
}
