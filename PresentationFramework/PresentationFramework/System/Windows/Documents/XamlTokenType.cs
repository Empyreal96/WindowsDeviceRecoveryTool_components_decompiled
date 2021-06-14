using System;

namespace System.Windows.Documents
{
	// Token: 0x02000435 RID: 1077
	internal enum XamlTokenType
	{
		// Token: 0x04002714 RID: 10004
		XTokInvalid,
		// Token: 0x04002715 RID: 10005
		XTokEOF,
		// Token: 0x04002716 RID: 10006
		XTokCharacters,
		// Token: 0x04002717 RID: 10007
		XTokEntity,
		// Token: 0x04002718 RID: 10008
		XTokStartElement,
		// Token: 0x04002719 RID: 10009
		XTokEndElement,
		// Token: 0x0400271A RID: 10010
		XTokCData,
		// Token: 0x0400271B RID: 10011
		XTokPI,
		// Token: 0x0400271C RID: 10012
		XTokComment,
		// Token: 0x0400271D RID: 10013
		XTokWS
	}
}
