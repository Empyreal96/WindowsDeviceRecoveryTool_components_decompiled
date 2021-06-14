using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000172 RID: 370
	internal class CsdlSemanticsFunctionParameter : CsdlSemanticsElement, IEdmFunctionParameter, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x06000800 RID: 2048 RVA: 0x00015D5A File Offset: 0x00013F5A
		public CsdlSemanticsFunctionParameter(CsdlSemanticsFunctionBase declaringFunction, CsdlFunctionParameter parameter) : base(parameter)
		{
			this.parameter = parameter;
			this.declaringFunction = declaringFunction;
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x00015D7C File Offset: 0x00013F7C
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.declaringFunction.Model;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x00015D89 File Offset: 0x00013F89
		public override CsdlElement Element
		{
			get
			{
				return this.parameter;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x00015D91 File Offset: 0x00013F91
		public IEdmTypeReference Type
		{
			get
			{
				return this.typeCache.GetValue(this, CsdlSemanticsFunctionParameter.ComputeTypeFunc, null);
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x00015DA5 File Offset: 0x00013FA5
		public EdmFunctionParameterMode Mode
		{
			get
			{
				return this.parameter.Mode;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x00015DB2 File Offset: 0x00013FB2
		public string Name
		{
			get
			{
				return this.parameter.Name;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x00015DBF File Offset: 0x00013FBF
		public IEdmFunctionBase DeclaringFunction
		{
			get
			{
				return this.declaringFunction;
			}
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00015DC7 File Offset: 0x00013FC7
		protected override IEnumerable<IEdmVocabularyAnnotation> ComputeInlineVocabularyAnnotations()
		{
			return this.Model.WrapInlineVocabularyAnnotations(this, this.declaringFunction.Context);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00015DE0 File Offset: 0x00013FE0
		private IEdmTypeReference ComputeType()
		{
			return CsdlSemanticsModel.WrapTypeReference(this.declaringFunction.Context, this.parameter.Type);
		}

		// Token: 0x04000406 RID: 1030
		private readonly CsdlSemanticsFunctionBase declaringFunction;

		// Token: 0x04000407 RID: 1031
		private readonly CsdlFunctionParameter parameter;

		// Token: 0x04000408 RID: 1032
		private readonly Cache<CsdlSemanticsFunctionParameter, IEdmTypeReference> typeCache = new Cache<CsdlSemanticsFunctionParameter, IEdmTypeReference>();

		// Token: 0x04000409 RID: 1033
		private static readonly Func<CsdlSemanticsFunctionParameter, IEdmTypeReference> ComputeTypeFunc = (CsdlSemanticsFunctionParameter me) => me.ComputeType();
	}
}
