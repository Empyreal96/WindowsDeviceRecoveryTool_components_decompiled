using System;

namespace System.Windows.Markup
{
	// Token: 0x02000200 RID: 512
	internal class BamlTextRecord : BamlStringValueRecord
	{
		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x0600203A RID: 8250 RVA: 0x00095FAB File Offset: 0x000941AB
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.Text;
			}
		}
	}
}
