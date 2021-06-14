using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020000B2 RID: 178
	internal class CsdlSemanticsEntityReferenceTypeExpression : CsdlSemanticsTypeExpression, IEdmEntityReferenceTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x0600032D RID: 813 RVA: 0x00008739 File Offset: 0x00006939
		public CsdlSemanticsEntityReferenceTypeExpression(CsdlExpressionTypeReference expressionUsage, CsdlSemanticsTypeDefinition type) : base(expressionUsage, type)
		{
		}
	}
}
