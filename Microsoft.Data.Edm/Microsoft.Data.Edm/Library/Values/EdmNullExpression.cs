using System;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Library.Values
{
	// Token: 0x02000186 RID: 390
	public class EdmNullExpression : EdmValue, IEdmNullExpression, IEdmExpression, IEdmNullValue, IEdmValue, IEdmElement
	{
		// Token: 0x0600089B RID: 2203 RVA: 0x00018050 File Offset: 0x00016250
		private EdmNullExpression() : base(null)
		{
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x00018059 File Offset: 0x00016259
		public EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.Null;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x0001805D File Offset: 0x0001625D
		public override EdmValueKind ValueKind
		{
			get
			{
				return EdmValueKind.Null;
			}
		}

		// Token: 0x04000444 RID: 1092
		public static EdmNullExpression Instance = new EdmNullExpression();
	}
}
