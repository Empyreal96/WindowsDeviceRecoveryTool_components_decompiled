using System;

namespace System.Windows.Markup
{
	// Token: 0x020001FC RID: 508
	internal class BamlStaticResourceEndRecord : BamlElementEndRecord
	{
		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06002013 RID: 8211 RVA: 0x00095CA0 File Offset: 0x00093EA0
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.StaticResourceEnd;
			}
		}
	}
}
