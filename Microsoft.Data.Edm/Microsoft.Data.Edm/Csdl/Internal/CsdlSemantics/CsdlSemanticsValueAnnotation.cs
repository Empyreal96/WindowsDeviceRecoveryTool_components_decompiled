using System;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020000A1 RID: 161
	internal class CsdlSemanticsValueAnnotation : CsdlSemanticsVocabularyAnnotation, IEdmValueAnnotation, IEdmVocabularyAnnotation, IEdmElement
	{
		// Token: 0x060002A3 RID: 675 RVA: 0x00006C0C File Offset: 0x00004E0C
		public CsdlSemanticsValueAnnotation(CsdlSemanticsSchema schema, IEdmVocabularyAnnotatable targetContext, CsdlSemanticsAnnotations annotationsContext, CsdlValueAnnotation annotation, string externalQualifier) : base(schema, targetContext, annotationsContext, annotation, externalQualifier)
		{
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x00006C26 File Offset: 0x00004E26
		public IEdmExpression Value
		{
			get
			{
				return this.valueCache.GetValue(this, CsdlSemanticsValueAnnotation.ComputeValueFunc, null);
			}
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00006C3A File Offset: 0x00004E3A
		protected override IEdmTerm ComputeTerm()
		{
			return base.Schema.FindValueTerm(this.Annotation.Term) ?? new UnresolvedValueTerm(base.Schema.UnresolvedName(this.Annotation.Term));
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00006C71 File Offset: 0x00004E71
		private IEdmExpression ComputeValue()
		{
			return CsdlSemanticsModel.WrapExpression(((CsdlValueAnnotation)this.Annotation).Expression, base.TargetBindingContext, base.Schema);
		}

		// Token: 0x04000132 RID: 306
		private readonly Cache<CsdlSemanticsValueAnnotation, IEdmExpression> valueCache = new Cache<CsdlSemanticsValueAnnotation, IEdmExpression>();

		// Token: 0x04000133 RID: 307
		private static readonly Func<CsdlSemanticsValueAnnotation, IEdmExpression> ComputeValueFunc = (CsdlSemanticsValueAnnotation me) => me.ComputeValue();
	}
}
