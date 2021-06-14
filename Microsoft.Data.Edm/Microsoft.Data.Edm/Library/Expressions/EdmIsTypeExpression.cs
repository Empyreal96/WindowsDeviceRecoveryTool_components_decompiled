using System;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Library.Expressions
{
	// Token: 0x0200019A RID: 410
	public class EdmIsTypeExpression : EdmElement, IEdmIsTypeExpression, IEdmExpression, IEdmElement
	{
		// Token: 0x060008F5 RID: 2293 RVA: 0x00018542 File Offset: 0x00016742
		public EdmIsTypeExpression(IEdmExpression operand, IEdmTypeReference type)
		{
			EdmUtil.CheckArgumentNull<IEdmExpression>(operand, "operand");
			EdmUtil.CheckArgumentNull<IEdmTypeReference>(type, "type");
			this.operand = operand;
			this.type = type;
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00018570 File Offset: 0x00016770
		public IEdmExpression Operand
		{
			get
			{
				return this.operand;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00018578 File Offset: 0x00016778
		public IEdmTypeReference Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x00018580 File Offset: 0x00016780
		public EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.IsType;
			}
		}

		// Token: 0x04000464 RID: 1124
		private readonly IEdmExpression operand;

		// Token: 0x04000465 RID: 1125
		private readonly IEdmTypeReference type;
	}
}
