using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200004E RID: 78
	internal class CsdlSemanticsEnumMemberReferenceExpression : CsdlSemanticsExpression, IEdmEnumMemberReferenceExpression, IEdmExpression, IEdmElement, IEdmCheckable
	{
		// Token: 0x06000125 RID: 293 RVA: 0x00004577 File Offset: 0x00002777
		public CsdlSemanticsEnumMemberReferenceExpression(CsdlEnumMemberReferenceExpression expression, IEdmEntityType bindingContext, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
			this.bindingContext = bindingContext;
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000126 RID: 294 RVA: 0x0000459A File Offset: 0x0000279A
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000127 RID: 295 RVA: 0x000045A2 File Offset: 0x000027A2
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.EnumMemberReference;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000128 RID: 296 RVA: 0x000045A6 File Offset: 0x000027A6
		public IEdmEnumMember ReferencedEnumMember
		{
			get
			{
				return this.referencedCache.GetValue(this, CsdlSemanticsEnumMemberReferenceExpression.ComputeReferencedFunc, null);
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000129 RID: 297 RVA: 0x000045BA File Offset: 0x000027BA
		public IEnumerable<EdmError> Errors
		{
			get
			{
				if (this.ReferencedEnumMember is IUnresolvedElement)
				{
					return this.ReferencedEnumMember.Errors();
				}
				return Enumerable.Empty<EdmError>();
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000045DC File Offset: 0x000027DC
		private IEdmEnumMember ComputeReferenced()
		{
			string[] array = this.expression.EnumMemberPath.Split(new char[]
			{
				'/'
			});
			return new UnresolvedEnumMember(array[1], new UnresolvedEnumType(array[0], base.Location), base.Location);
		}

		// Token: 0x0400006E RID: 110
		private readonly CsdlEnumMemberReferenceExpression expression;

		// Token: 0x0400006F RID: 111
		private readonly IEdmEntityType bindingContext;

		// Token: 0x04000070 RID: 112
		private readonly Cache<CsdlSemanticsEnumMemberReferenceExpression, IEdmEnumMember> referencedCache = new Cache<CsdlSemanticsEnumMemberReferenceExpression, IEdmEnumMember>();

		// Token: 0x04000071 RID: 113
		private static readonly Func<CsdlSemanticsEnumMemberReferenceExpression, IEdmEnumMember> ComputeReferencedFunc = (CsdlSemanticsEnumMemberReferenceExpression me) => me.ComputeReferenced();
	}
}
