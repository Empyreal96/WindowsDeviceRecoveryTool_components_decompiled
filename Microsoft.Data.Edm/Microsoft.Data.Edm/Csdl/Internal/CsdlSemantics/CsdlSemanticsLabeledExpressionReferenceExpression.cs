using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200005E RID: 94
	internal class CsdlSemanticsLabeledExpressionReferenceExpression : CsdlSemanticsExpression, IEdmLabeledExpressionReferenceExpression, IEdmExpression, IEdmElement, IEdmCheckable
	{
		// Token: 0x06000179 RID: 377 RVA: 0x00004CBC File Offset: 0x00002EBC
		public CsdlSemanticsLabeledExpressionReferenceExpression(CsdlLabeledExpressionReferenceExpression expression, IEdmEntityType bindingContext, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
			this.bindingContext = bindingContext;
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00004CDF File Offset: 0x00002EDF
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00004CE7 File Offset: 0x00002EE7
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.LabeledExpressionReference;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00004CEB File Offset: 0x00002EEB
		public IEdmLabeledExpression ReferencedLabeledExpression
		{
			get
			{
				return this.elementCache.GetValue(this, CsdlSemanticsLabeledExpressionReferenceExpression.ComputeElementFunc, null);
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00004CFF File Offset: 0x00002EFF
		public IEnumerable<EdmError> Errors
		{
			get
			{
				if (this.ReferencedLabeledExpression is IUnresolvedElement)
				{
					return this.ReferencedLabeledExpression.Errors();
				}
				return Enumerable.Empty<EdmError>();
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00004D20 File Offset: 0x00002F20
		private IEdmLabeledExpression ComputeElement()
		{
			CsdlSemanticsModel model = base.Schema.Model;
			IEdmLabeledExpression edmLabeledExpression = base.Schema.FindLabeledElement(this.expression.Label, this.bindingContext);
			if (edmLabeledExpression != null)
			{
				return edmLabeledExpression;
			}
			return new UnresolvedLabeledElement(this.expression.Label, base.Location);
		}

		// Token: 0x040000A0 RID: 160
		private readonly CsdlLabeledExpressionReferenceExpression expression;

		// Token: 0x040000A1 RID: 161
		private readonly IEdmEntityType bindingContext;

		// Token: 0x040000A2 RID: 162
		private readonly Cache<CsdlSemanticsLabeledExpressionReferenceExpression, IEdmLabeledExpression> elementCache = new Cache<CsdlSemanticsLabeledExpressionReferenceExpression, IEdmLabeledExpression>();

		// Token: 0x040000A3 RID: 163
		private static readonly Func<CsdlSemanticsLabeledExpressionReferenceExpression, IEdmLabeledExpression> ComputeElementFunc = (CsdlSemanticsLabeledExpressionReferenceExpression me) => me.ComputeElement();
	}
}
