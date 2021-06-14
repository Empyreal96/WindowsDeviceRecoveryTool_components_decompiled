using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020000B4 RID: 180
	internal class CsdlSemanticsFunctionBase : CsdlSemanticsElement, IEdmFunctionBase, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x06000331 RID: 817 RVA: 0x00008743 File Offset: 0x00006943
		public CsdlSemanticsFunctionBase(CsdlSemanticsSchema context, CsdlFunctionBase functionBase) : base(functionBase)
		{
			this.context = context;
			this.functionBase = functionBase;
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000332 RID: 818 RVA: 0x00008770 File Offset: 0x00006970
		public string Name
		{
			get
			{
				return this.functionBase.Name;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000877D File Offset: 0x0000697D
		public IEdmTypeReference ReturnType
		{
			get
			{
				return this.returnTypeCache.GetValue(this, CsdlSemanticsFunctionBase.ComputeReturnTypeFunc, null);
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00008791 File Offset: 0x00006991
		public IEnumerable<IEdmFunctionParameter> Parameters
		{
			get
			{
				return this.parametersCache.GetValue(this, CsdlSemanticsFunctionBase.ComputeParametersFunc, null);
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000335 RID: 821 RVA: 0x000087A5 File Offset: 0x000069A5
		public CsdlSemanticsSchema Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000336 RID: 822 RVA: 0x000087AD File Offset: 0x000069AD
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.Context.Model;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000337 RID: 823 RVA: 0x000087BA File Offset: 0x000069BA
		public override CsdlElement Element
		{
			get
			{
				return this.functionBase;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000338 RID: 824 RVA: 0x000087C2 File Offset: 0x000069C2
		public EdmContainerElementKind ContainerElementKind
		{
			get
			{
				return EdmContainerElementKind.FunctionImport;
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x000087E0 File Offset: 0x000069E0
		public IEdmFunctionParameter FindParameter(string name)
		{
			return this.Parameters.SingleOrDefault((IEdmFunctionParameter p) => p.Name == name);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00008811 File Offset: 0x00006A11
		protected override IEnumerable<IEdmVocabularyAnnotation> ComputeInlineVocabularyAnnotations()
		{
			return this.Model.WrapInlineVocabularyAnnotations(this, this.Context);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00008825 File Offset: 0x00006A25
		private IEdmTypeReference ComputeReturnType()
		{
			return CsdlSemanticsModel.WrapTypeReference(this.Context, this.functionBase.ReturnType);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00008840 File Offset: 0x00006A40
		private IEnumerable<IEdmFunctionParameter> ComputeParameters()
		{
			List<IEdmFunctionParameter> list = new List<IEdmFunctionParameter>();
			foreach (CsdlFunctionParameter parameter in this.functionBase.Parameters)
			{
				list.Add(new CsdlSemanticsFunctionParameter(this, parameter));
			}
			return list;
		}

		// Token: 0x04000169 RID: 361
		private readonly CsdlSemanticsSchema context;

		// Token: 0x0400016A RID: 362
		private readonly CsdlFunctionBase functionBase;

		// Token: 0x0400016B RID: 363
		private readonly Cache<CsdlSemanticsFunctionBase, IEdmTypeReference> returnTypeCache = new Cache<CsdlSemanticsFunctionBase, IEdmTypeReference>();

		// Token: 0x0400016C RID: 364
		private static readonly Func<CsdlSemanticsFunctionBase, IEdmTypeReference> ComputeReturnTypeFunc = (CsdlSemanticsFunctionBase me) => me.ComputeReturnType();

		// Token: 0x0400016D RID: 365
		private readonly Cache<CsdlSemanticsFunctionBase, IEnumerable<IEdmFunctionParameter>> parametersCache = new Cache<CsdlSemanticsFunctionBase, IEnumerable<IEdmFunctionParameter>>();

		// Token: 0x0400016E RID: 366
		private static readonly Func<CsdlSemanticsFunctionBase, IEnumerable<IEdmFunctionParameter>> ComputeParametersFunc = (CsdlSemanticsFunctionBase me) => me.ComputeParameters();
	}
}
