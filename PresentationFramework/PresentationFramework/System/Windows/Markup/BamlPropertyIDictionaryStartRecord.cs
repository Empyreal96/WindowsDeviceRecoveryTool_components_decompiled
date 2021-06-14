using System;

namespace System.Windows.Markup
{
	// Token: 0x020001F4 RID: 500
	internal class BamlPropertyIDictionaryStartRecord : BamlPropertyComplexStartRecord
	{
		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x0003B40B File Offset: 0x0003960B
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyIDictionaryStart;
			}
		}
	}
}
