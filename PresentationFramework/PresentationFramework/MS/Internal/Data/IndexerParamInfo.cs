using System;

namespace MS.Internal.Data
{
	// Token: 0x0200073C RID: 1852
	internal struct IndexerParamInfo
	{
		// Token: 0x06007627 RID: 30247 RVA: 0x0021AE65 File Offset: 0x00219065
		public IndexerParamInfo(string paren, string value)
		{
			this.parenString = paren;
			this.valueString = value;
		}

		// Token: 0x04003866 RID: 14438
		public string parenString;

		// Token: 0x04003867 RID: 14439
		public string valueString;
	}
}
