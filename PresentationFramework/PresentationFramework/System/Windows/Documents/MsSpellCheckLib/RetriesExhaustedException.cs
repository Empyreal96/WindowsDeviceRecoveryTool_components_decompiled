using System;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x02000461 RID: 1121
	internal class RetriesExhaustedException : Exception
	{
		// Token: 0x060040B5 RID: 16565 RVA: 0x00127A0B File Offset: 0x00125C0B
		internal RetriesExhaustedException()
		{
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x00127A13 File Offset: 0x00125C13
		internal RetriesExhaustedException(string message) : base(message)
		{
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x00127A1C File Offset: 0x00125C1C
		internal RetriesExhaustedException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
