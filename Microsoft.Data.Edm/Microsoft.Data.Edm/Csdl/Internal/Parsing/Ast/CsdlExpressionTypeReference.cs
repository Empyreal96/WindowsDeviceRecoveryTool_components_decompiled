using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200013D RID: 317
	internal class CsdlExpressionTypeReference : CsdlTypeReference
	{
		// Token: 0x060005FD RID: 1533 RVA: 0x0000F5BF File Offset: 0x0000D7BF
		public CsdlExpressionTypeReference(ICsdlTypeExpression typeExpression, bool isNullable, CsdlLocation location) : base(isNullable, location)
		{
			this.typeExpression = typeExpression;
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x0000F5D0 File Offset: 0x0000D7D0
		public ICsdlTypeExpression TypeExpression
		{
			get
			{
				return this.typeExpression;
			}
		}

		// Token: 0x0400032E RID: 814
		private readonly ICsdlTypeExpression typeExpression;
	}
}
