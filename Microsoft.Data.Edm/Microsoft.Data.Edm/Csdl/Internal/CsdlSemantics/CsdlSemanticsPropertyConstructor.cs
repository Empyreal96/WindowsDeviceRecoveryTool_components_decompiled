using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000087 RID: 135
	internal class CsdlSemanticsPropertyConstructor : CsdlSemanticsElement, IEdmPropertyConstructor, IEdmElement
	{
		// Token: 0x06000228 RID: 552 RVA: 0x00005C47 File Offset: 0x00003E47
		public CsdlSemanticsPropertyConstructor(CsdlPropertyValue property, CsdlSemanticsRecordExpression context) : base(property)
		{
			this.property = property;
			this.context = context;
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000229 RID: 553 RVA: 0x00005C69 File Offset: 0x00003E69
		public string Name
		{
			get
			{
				return this.property.Property;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600022A RID: 554 RVA: 0x00005C76 File Offset: 0x00003E76
		public IEdmExpression Value
		{
			get
			{
				return this.valueCache.GetValue(this, CsdlSemanticsPropertyConstructor.ComputeValueFunc, null);
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00005C8A File Offset: 0x00003E8A
		public override CsdlElement Element
		{
			get
			{
				return this.property;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00005C92 File Offset: 0x00003E92
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.context.Model;
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00005C9F File Offset: 0x00003E9F
		private IEdmExpression ComputeValue()
		{
			return CsdlSemanticsModel.WrapExpression(this.property.Expression, this.context.BindingContext, this.context.Schema);
		}

		// Token: 0x040000F6 RID: 246
		private readonly CsdlPropertyValue property;

		// Token: 0x040000F7 RID: 247
		private readonly CsdlSemanticsRecordExpression context;

		// Token: 0x040000F8 RID: 248
		private readonly Cache<CsdlSemanticsPropertyConstructor, IEdmExpression> valueCache = new Cache<CsdlSemanticsPropertyConstructor, IEdmExpression>();

		// Token: 0x040000F9 RID: 249
		private static readonly Func<CsdlSemanticsPropertyConstructor, IEdmExpression> ComputeValueFunc = (CsdlSemanticsPropertyConstructor me) => me.ComputeValue();
	}
}
