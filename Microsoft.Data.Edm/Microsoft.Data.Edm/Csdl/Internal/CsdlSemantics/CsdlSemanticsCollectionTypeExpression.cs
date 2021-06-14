using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000071 RID: 113
	internal class CsdlSemanticsCollectionTypeExpression : CsdlSemanticsTypeExpression, IEdmCollectionTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x060001D2 RID: 466 RVA: 0x0000548C File Offset: 0x0000368C
		public CsdlSemanticsCollectionTypeExpression(CsdlExpressionTypeReference expressionUsage, CsdlSemanticsTypeDefinition type) : base(expressionUsage, type)
		{
		}
	}
}
