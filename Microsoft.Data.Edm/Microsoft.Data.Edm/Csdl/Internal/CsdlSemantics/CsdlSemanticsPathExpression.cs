using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000085 RID: 133
	internal class CsdlSemanticsPathExpression : CsdlSemanticsExpression, IEdmPathExpression, IEdmExpression, IEdmElement
	{
		// Token: 0x0600021F RID: 543 RVA: 0x00005BAB File Offset: 0x00003DAB
		public CsdlSemanticsPathExpression(CsdlPathExpression expression, IEdmEntityType bindingContext, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
			this.bindingContext = bindingContext;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000220 RID: 544 RVA: 0x00005BCE File Offset: 0x00003DCE
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00005BD6 File Offset: 0x00003DD6
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.Path;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00005BDA File Offset: 0x00003DDA
		public IEnumerable<string> Path
		{
			get
			{
				return this.pathCache.GetValue(this, CsdlSemanticsPathExpression.ComputePathFunc, null);
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00005BF0 File Offset: 0x00003DF0
		private IEnumerable<string> ComputePath()
		{
			return this.expression.Path.Split(new char[]
			{
				'/'
			}, StringSplitOptions.None);
		}

		// Token: 0x040000F1 RID: 241
		private readonly CsdlPathExpression expression;

		// Token: 0x040000F2 RID: 242
		private readonly IEdmEntityType bindingContext;

		// Token: 0x040000F3 RID: 243
		private readonly Cache<CsdlSemanticsPathExpression, IEnumerable<string>> pathCache = new Cache<CsdlSemanticsPathExpression, IEnumerable<string>>();

		// Token: 0x040000F4 RID: 244
		private static readonly Func<CsdlSemanticsPathExpression, IEnumerable<string>> ComputePathFunc = (CsdlSemanticsPathExpression me) => me.ComputePath();
	}
}
