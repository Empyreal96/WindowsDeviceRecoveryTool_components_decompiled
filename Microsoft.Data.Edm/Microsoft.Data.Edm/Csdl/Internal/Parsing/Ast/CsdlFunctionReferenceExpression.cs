using System;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000025 RID: 37
	internal class CsdlFunctionReferenceExpression : CsdlExpressionBase
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00002D9A File Offset: 0x00000F9A
		public CsdlFunctionReferenceExpression(string function, CsdlLocation location) : base(location)
		{
			this.function = function;
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00002DAA File Offset: 0x00000FAA
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.FunctionReference;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00002DAE File Offset: 0x00000FAE
		public string Function
		{
			get
			{
				return this.function;
			}
		}

		// Token: 0x04000037 RID: 55
		private readonly string function;
	}
}
