using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000170 RID: 368
	internal class CsdlSemanticsFunction : CsdlSemanticsFunctionBase, IEdmFunction, IEdmFunctionBase, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x060007F7 RID: 2039 RVA: 0x00015D17 File Offset: 0x00013F17
		public CsdlSemanticsFunction(CsdlSemanticsSchema context, CsdlFunction function) : base(context, function)
		{
			this.function = function;
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x00015D28 File Offset: 0x00013F28
		public override CsdlSemanticsModel Model
		{
			get
			{
				return base.Context.Model;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x00015D35 File Offset: 0x00013F35
		public override CsdlElement Element
		{
			get
			{
				return this.function;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x00015D3D File Offset: 0x00013F3D
		public string Namespace
		{
			get
			{
				return base.Context.Namespace;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x00015D4A File Offset: 0x00013F4A
		public string DefiningExpression
		{
			get
			{
				return this.function.DefiningExpression;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x00015D57 File Offset: 0x00013F57
		public EdmSchemaElementKind SchemaElementKind
		{
			get
			{
				return EdmSchemaElementKind.Function;
			}
		}

		// Token: 0x04000405 RID: 1029
		private readonly CsdlFunction function;
	}
}
