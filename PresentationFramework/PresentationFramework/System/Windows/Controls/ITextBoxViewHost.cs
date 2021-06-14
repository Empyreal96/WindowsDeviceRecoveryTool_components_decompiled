using System;
using System.Windows.Documents;

namespace System.Windows.Controls
{
	// Token: 0x02000464 RID: 1124
	internal interface ITextBoxViewHost
	{
		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x06004132 RID: 16690
		ITextContainer TextContainer { get; }

		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x06004133 RID: 16691
		bool IsTypographyDefaultValue { get; }
	}
}
