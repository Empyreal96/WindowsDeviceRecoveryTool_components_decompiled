using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000016 RID: 22
	internal sealed class FunctionParameterToken : QueryToken
	{
		// Token: 0x06000080 RID: 128 RVA: 0x000036AB File Offset: 0x000018AB
		public FunctionParameterToken(string parameterName, QueryToken valueToken)
		{
			this.parameterName = parameterName;
			this.valueToken = valueToken;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000081 RID: 129 RVA: 0x000036C1 File Offset: 0x000018C1
		public string ParameterName
		{
			get
			{
				return this.parameterName;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000036C9 File Offset: 0x000018C9
		public QueryToken ValueToken
		{
			get
			{
				return this.valueToken;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000083 RID: 131 RVA: 0x000036D1 File Offset: 0x000018D1
		public override QueryTokenKind Kind
		{
			get
			{
				return QueryTokenKind.FunctionParameter;
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000036D5 File Offset: 0x000018D5
		public override T Accept<T>(ISyntacticTreeVisitor<T> visitor)
		{
			return visitor.Visit(this);
		}

		// Token: 0x04000033 RID: 51
		public static FunctionParameterToken[] EmptyParameterList = new FunctionParameterToken[0];

		// Token: 0x04000034 RID: 52
		private readonly string parameterName;

		// Token: 0x04000035 RID: 53
		private readonly QueryToken valueToken;
	}
}
