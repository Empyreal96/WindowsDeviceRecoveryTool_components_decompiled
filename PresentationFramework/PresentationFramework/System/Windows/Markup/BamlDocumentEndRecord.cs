using System;

namespace System.Windows.Markup
{
	// Token: 0x02000207 RID: 519
	internal class BamlDocumentEndRecord : BamlRecord
	{
		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x0600205F RID: 8287 RVA: 0x00094C44 File Offset: 0x00092E44
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.DocumentEnd;
			}
		}
	}
}
