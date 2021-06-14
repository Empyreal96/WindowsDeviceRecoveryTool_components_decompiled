using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000014 RID: 20
	internal sealed class RawFunctionParameterValueToken : QueryToken
	{
		// Token: 0x06000076 RID: 118 RVA: 0x0000364A File Offset: 0x0000184A
		public RawFunctionParameterValueToken(string rawText)
		{
			this.RawText = rawText;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003659 File Offset: 0x00001859
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00003661 File Offset: 0x00001861
		public string RawText { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000079 RID: 121 RVA: 0x0000366A File Offset: 0x0000186A
		public override QueryTokenKind Kind
		{
			get
			{
				return QueryTokenKind.RawFunctionParameterValue;
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000366E File Offset: 0x0000186E
		public override T Accept<T>(ISyntacticTreeVisitor<T> visitor)
		{
			throw new NotImplementedException();
		}
	}
}
