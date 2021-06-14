using System;

namespace System.Windows.Markup
{
	// Token: 0x020001FB RID: 507
	internal class BamlStaticResourceStartRecord : BamlElementStartRecord
	{
		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06002011 RID: 8209 RVA: 0x00095C9C File Offset: 0x00093E9C
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.StaticResourceStart;
			}
		}
	}
}
