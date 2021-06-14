using System;

namespace System.Windows.Markup
{
	// Token: 0x02000206 RID: 518
	internal class BamlKeyElementEndRecord : BamlElementEndRecord
	{
		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x0600205D RID: 8285 RVA: 0x000961D8 File Offset: 0x000943D8
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.KeyElementEnd;
			}
		}
	}
}
