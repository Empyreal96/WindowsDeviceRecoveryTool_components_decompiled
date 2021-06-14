using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200005C RID: 92
	internal class CsdlSemanticsLabeledExpression : CsdlSemanticsElement, IEdmLabeledExpression, IEdmNamedElement, IEdmExpression, IEdmElement
	{
		// Token: 0x0600016E RID: 366 RVA: 0x00004C09 File Offset: 0x00002E09
		public CsdlSemanticsLabeledExpression(string name, CsdlExpressionBase element, IEdmEntityType bindingContext, CsdlSemanticsSchema schema) : base(element)
		{
			this.name = name;
			this.sourceElement = element;
			this.bindingContext = bindingContext;
			this.schema = schema;
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00004C3A File Offset: 0x00002E3A
		public override CsdlElement Element
		{
			get
			{
				return this.sourceElement;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00004C42 File Offset: 0x00002E42
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.schema.Model;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00004C4F File Offset: 0x00002E4F
		public IEdmEntityType BindingContext
		{
			get
			{
				return this.bindingContext;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00004C57 File Offset: 0x00002E57
		public IEdmExpression Expression
		{
			get
			{
				return this.expressionCache.GetValue(this, CsdlSemanticsLabeledExpression.ComputeExpressionFunc, null);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00004C6B File Offset: 0x00002E6B
		public EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.Labeled;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00004C6F File Offset: 0x00002E6F
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00004C77 File Offset: 0x00002E77
		private IEdmExpression ComputeExpression()
		{
			return CsdlSemanticsModel.WrapExpression(this.sourceElement, this.BindingContext, this.schema);
		}

		// Token: 0x04000099 RID: 153
		private readonly string name;

		// Token: 0x0400009A RID: 154
		private readonly CsdlExpressionBase sourceElement;

		// Token: 0x0400009B RID: 155
		private readonly CsdlSemanticsSchema schema;

		// Token: 0x0400009C RID: 156
		private readonly IEdmEntityType bindingContext;

		// Token: 0x0400009D RID: 157
		private readonly Cache<CsdlSemanticsLabeledExpression, IEdmExpression> expressionCache = new Cache<CsdlSemanticsLabeledExpression, IEdmExpression>();

		// Token: 0x0400009E RID: 158
		private static readonly Func<CsdlSemanticsLabeledExpression, IEdmExpression> ComputeExpressionFunc = (CsdlSemanticsLabeledExpression me) => me.ComputeExpression();
	}
}
