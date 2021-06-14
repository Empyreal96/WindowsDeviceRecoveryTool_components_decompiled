using System;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000024 RID: 36
	internal class CsdlEntitySetReferenceExpression : CsdlExpressionBase
	{
		// Token: 0x06000088 RID: 136 RVA: 0x00002D7E File Offset: 0x00000F7E
		public CsdlEntitySetReferenceExpression(string entitySetPath, CsdlLocation location) : base(location)
		{
			this.entitySetPath = entitySetPath;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00002D8E File Offset: 0x00000F8E
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.EntitySetReference;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00002D92 File Offset: 0x00000F92
		public string EntitySetPath
		{
			get
			{
				return this.entitySetPath;
			}
		}

		// Token: 0x04000036 RID: 54
		private readonly string entitySetPath;
	}
}
