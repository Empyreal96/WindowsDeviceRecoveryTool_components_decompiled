using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200009F RID: 159
	internal class CsdlSemanticsTypeAnnotation : CsdlSemanticsVocabularyAnnotation, IEdmTypeAnnotation, IEdmVocabularyAnnotation, IEdmElement
	{
		// Token: 0x0600029C RID: 668 RVA: 0x00006B01 File Offset: 0x00004D01
		public CsdlSemanticsTypeAnnotation(CsdlSemanticsSchema schema, IEdmVocabularyAnnotatable targetContext, CsdlSemanticsAnnotations annotationsContext, CsdlTypeAnnotation annotation, string externalQualifier) : base(schema, targetContext, annotationsContext, annotation, externalQualifier)
		{
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00006B1B File Offset: 0x00004D1B
		public IEnumerable<IEdmPropertyValueBinding> PropertyValueBindings
		{
			get
			{
				return this.propertiesCache.GetValue(this, CsdlSemanticsTypeAnnotation.ComputePropertiesFunc, null);
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00006B30 File Offset: 0x00004D30
		protected override IEdmTerm ComputeTerm()
		{
			return (IEdmTerm)((base.Schema.FindType(this.Annotation.Term) as IEdmStructuredType) ?? new UnresolvedTypeTerm(base.Schema.UnresolvedName(this.Annotation.Term)));
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00006B7C File Offset: 0x00004D7C
		private IEnumerable<IEdmPropertyValueBinding> ComputeProperties()
		{
			List<IEdmPropertyValueBinding> list = new List<IEdmPropertyValueBinding>();
			foreach (CsdlPropertyValue property in ((CsdlTypeAnnotation)this.Annotation).Properties)
			{
				list.Add(new CsdlSemanticsPropertyValueBinding(this, property));
			}
			return list;
		}

		// Token: 0x0400012F RID: 303
		private readonly Cache<CsdlSemanticsTypeAnnotation, IEnumerable<IEdmPropertyValueBinding>> propertiesCache = new Cache<CsdlSemanticsTypeAnnotation, IEnumerable<IEdmPropertyValueBinding>>();

		// Token: 0x04000130 RID: 304
		private static readonly Func<CsdlSemanticsTypeAnnotation, IEnumerable<IEdmPropertyValueBinding>> ComputePropertiesFunc = (CsdlSemanticsTypeAnnotation me) => me.ComputeProperties();
	}
}
