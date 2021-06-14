using System;

namespace System.Windows.Markup
{
	// Token: 0x02000204 RID: 516
	internal class BamlElementEndRecord : BamlRecord
	{
		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06002059 RID: 8281 RVA: 0x00094CFC File Offset: 0x00092EFC
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.ElementEnd;
			}
		}
	}
}
