using System;

namespace MS.Internal.Globalization
{
	// Token: 0x02000699 RID: 1689
	internal struct BamlStringToken
	{
		// Token: 0x06006E12 RID: 28178 RVA: 0x001FAA9B File Offset: 0x001F8C9B
		internal BamlStringToken(BamlStringToken.TokenType type, string value)
		{
			this.Type = type;
			this.Value = value;
		}

		// Token: 0x04003629 RID: 13865
		internal readonly BamlStringToken.TokenType Type;

		// Token: 0x0400362A RID: 13866
		internal readonly string Value;

		// Token: 0x02000B28 RID: 2856
		internal enum TokenType
		{
			// Token: 0x04004A74 RID: 19060
			Text,
			// Token: 0x04004A75 RID: 19061
			ChildPlaceHolder
		}
	}
}
