using System;

namespace System.Windows.Markup
{
	// Token: 0x020001F2 RID: 498
	internal class BamlPropertyArrayStartRecord : BamlPropertyComplexStartRecord
	{
		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06001FD4 RID: 8148 RVA: 0x0009580F File Offset: 0x00093A0F
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyArrayStart;
			}
		}
	}
}
