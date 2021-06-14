using System;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000017 RID: 23
	internal class CsdlPathExpression : CsdlExpressionBase
	{
		// Token: 0x06000065 RID: 101 RVA: 0x00002B68 File Offset: 0x00000D68
		public CsdlPathExpression(string path, CsdlLocation location) : base(location)
		{
			this.path = path;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002B78 File Offset: 0x00000D78
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.Path;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00002B7C File Offset: 0x00000D7C
		public string Path
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x04000023 RID: 35
		private readonly string path;
	}
}
