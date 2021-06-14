using System;
using MS.Utility;

namespace MS.Internal.Data
{
	// Token: 0x0200073B RID: 1851
	internal struct SourceValueInfo
	{
		// Token: 0x06007625 RID: 30245 RVA: 0x0021AE1B File Offset: 0x0021901B
		public SourceValueInfo(SourceValueType t, DrillIn d, string n)
		{
			this.type = t;
			this.drillIn = d;
			this.name = n;
			this.paramList = null;
			this.propertyName = null;
		}

		// Token: 0x06007626 RID: 30246 RVA: 0x0021AE40 File Offset: 0x00219040
		public SourceValueInfo(SourceValueType t, DrillIn d, FrugalObjectList<IndexerParamInfo> list)
		{
			this.type = t;
			this.drillIn = d;
			this.name = null;
			this.paramList = list;
			this.propertyName = null;
		}

		// Token: 0x04003861 RID: 14433
		public SourceValueType type;

		// Token: 0x04003862 RID: 14434
		public DrillIn drillIn;

		// Token: 0x04003863 RID: 14435
		public string name;

		// Token: 0x04003864 RID: 14436
		public FrugalObjectList<IndexerParamInfo> paramList;

		// Token: 0x04003865 RID: 14437
		public string propertyName;
	}
}
