using System;
using System.Globalization;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x0200005C RID: 92
	public class LanguageChangedMessage
	{
		// Token: 0x060002D7 RID: 727 RVA: 0x0000FDC4 File Offset: 0x0000DFC4
		public LanguageChangedMessage(CultureInfo language)
		{
			this.Language = language;
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000FDD8 File Offset: 0x0000DFD8
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x0000FDEF File Offset: 0x0000DFEF
		public CultureInfo Language { get; private set; }
	}
}
