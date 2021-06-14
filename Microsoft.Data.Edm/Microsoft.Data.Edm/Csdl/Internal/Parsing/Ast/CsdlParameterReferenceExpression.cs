using System;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000016 RID: 22
	internal class CsdlParameterReferenceExpression : CsdlExpressionBase
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00002B4C File Offset: 0x00000D4C
		public CsdlParameterReferenceExpression(string parameter, CsdlLocation location) : base(location)
		{
			this.parameter = parameter;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002B5C File Offset: 0x00000D5C
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.ParameterReference;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002B60 File Offset: 0x00000D60
		public string Parameter
		{
			get
			{
				return this.parameter;
			}
		}

		// Token: 0x04000022 RID: 34
		private readonly string parameter;
	}
}
