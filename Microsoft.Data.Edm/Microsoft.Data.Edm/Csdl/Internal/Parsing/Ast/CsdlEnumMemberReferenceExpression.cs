using System;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000010 RID: 16
	internal class CsdlEnumMemberReferenceExpression : CsdlExpressionBase
	{
		// Token: 0x0600004B RID: 75 RVA: 0x00002A43 File Offset: 0x00000C43
		public CsdlEnumMemberReferenceExpression(string enumMemberPath, CsdlLocation location) : base(location)
		{
			this.enumMemberPath = enumMemberPath;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002A53 File Offset: 0x00000C53
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.EnumMemberReference;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002A57 File Offset: 0x00000C57
		public string EnumMemberPath
		{
			get
			{
				return this.enumMemberPath;
			}
		}

		// Token: 0x04000016 RID: 22
		private readonly string enumMemberPath;
	}
}
