using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200000A RID: 10
	internal class CsdlApplyExpression : CsdlExpressionBase
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002951 File Offset: 0x00000B51
		public CsdlApplyExpression(string function, IEnumerable<CsdlExpressionBase> arguments, CsdlLocation location) : base(location)
		{
			this.function = function;
			this.arguments = new List<CsdlExpressionBase>(arguments);
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000038 RID: 56 RVA: 0x0000296D File Offset: 0x00000B6D
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.FunctionApplication;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002971 File Offset: 0x00000B71
		public string Function
		{
			get
			{
				return this.function;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002979 File Offset: 0x00000B79
		public IEnumerable<CsdlExpressionBase> Arguments
		{
			get
			{
				return this.arguments;
			}
		}

		// Token: 0x0400000D RID: 13
		private readonly string function;

		// Token: 0x0400000E RID: 14
		private readonly List<CsdlExpressionBase> arguments;
	}
}
