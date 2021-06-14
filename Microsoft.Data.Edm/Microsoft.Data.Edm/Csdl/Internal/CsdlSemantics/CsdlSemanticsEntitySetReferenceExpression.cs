using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200004C RID: 76
	internal class CsdlSemanticsEntitySetReferenceExpression : CsdlSemanticsExpression, IEdmEntitySetReferenceExpression, IEdmExpression, IEdmElement, IEdmCheckable
	{
		// Token: 0x0600011C RID: 284 RVA: 0x0000449F File Offset: 0x0000269F
		public CsdlSemanticsEntitySetReferenceExpression(CsdlEntitySetReferenceExpression expression, IEdmEntityType bindingContext, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
			this.bindingContext = bindingContext;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600011D RID: 285 RVA: 0x000044C2 File Offset: 0x000026C2
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600011E RID: 286 RVA: 0x000044CA File Offset: 0x000026CA
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.EntitySetReference;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600011F RID: 287 RVA: 0x000044CE File Offset: 0x000026CE
		public IEdmEntitySet ReferencedEntitySet
		{
			get
			{
				return this.referencedCache.GetValue(this, CsdlSemanticsEntitySetReferenceExpression.ComputeReferencedFunc, null);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000120 RID: 288 RVA: 0x000044E2 File Offset: 0x000026E2
		public IEnumerable<EdmError> Errors
		{
			get
			{
				if (this.ReferencedEntitySet is IUnresolvedElement)
				{
					return this.ReferencedEntitySet.Errors();
				}
				return Enumerable.Empty<EdmError>();
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00004504 File Offset: 0x00002704
		private IEdmEntitySet ComputeReferenced()
		{
			string[] array = this.expression.EntitySetPath.Split(new char[]
			{
				'/'
			});
			return new UnresolvedEntitySet(array[1], new UnresolvedEntityContainer(array[0], base.Location), base.Location);
		}

		// Token: 0x04000069 RID: 105
		private readonly CsdlEntitySetReferenceExpression expression;

		// Token: 0x0400006A RID: 106
		private readonly IEdmEntityType bindingContext;

		// Token: 0x0400006B RID: 107
		private readonly Cache<CsdlSemanticsEntitySetReferenceExpression, IEdmEntitySet> referencedCache = new Cache<CsdlSemanticsEntitySetReferenceExpression, IEdmEntitySet>();

		// Token: 0x0400006C RID: 108
		private static readonly Func<CsdlSemanticsEntitySetReferenceExpression, IEdmEntitySet> ComputeReferencedFunc = (CsdlSemanticsEntitySetReferenceExpression me) => me.ComputeReferenced();
	}
}
