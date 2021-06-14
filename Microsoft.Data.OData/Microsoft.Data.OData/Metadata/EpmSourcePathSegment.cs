using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Metadata
{
	// Token: 0x0200020F RID: 527
	internal sealed class EpmSourcePathSegment
	{
		// Token: 0x0600103D RID: 4157 RVA: 0x0003B431 File Offset: 0x00039631
		internal EpmSourcePathSegment() : this(null)
		{
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0003B43A File Offset: 0x0003963A
		internal EpmSourcePathSegment(string propertyName)
		{
			this.propertyName = propertyName;
			this.subProperties = new List<EpmSourcePathSegment>();
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x0600103F RID: 4159 RVA: 0x0003B454 File Offset: 0x00039654
		internal string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x0003B45C File Offset: 0x0003965C
		internal List<EpmSourcePathSegment> SubProperties
		{
			get
			{
				return this.subProperties;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x0003B464 File Offset: 0x00039664
		// (set) Token: 0x06001042 RID: 4162 RVA: 0x0003B46C File Offset: 0x0003966C
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

		// Token: 0x040005F2 RID: 1522
		private readonly string propertyName;

		// Token: 0x040005F3 RID: 1523
		private readonly List<EpmSourcePathSegment> subProperties;

		// Token: 0x040005F4 RID: 1524
		private EntityPropertyMappingInfo epmInfo;
	}
}
