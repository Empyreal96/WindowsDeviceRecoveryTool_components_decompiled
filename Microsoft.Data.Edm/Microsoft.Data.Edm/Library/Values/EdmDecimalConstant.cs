using System;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Library.Values
{
	// Token: 0x02000194 RID: 404
	public class EdmDecimalConstant : EdmValue, IEdmDecimalConstantExpression, IEdmExpression, IEdmDecimalValue, IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x060008DB RID: 2267 RVA: 0x000183D7 File Offset: 0x000165D7
		public EdmDecimalConstant(decimal value) : this(null, value)
		{
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x000183E1 File Offset: 0x000165E1
		public EdmDecimalConstant(IEdmDecimalTypeReference type, decimal value) : base(type)
		{
			this.value = value;
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x000183F1 File Offset: 0x000165F1
		public decimal Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x000183F9 File Offset: 0x000165F9
		public EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.DecimalConstant;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x000183FC File Offset: 0x000165FC
		public override EdmValueKind ValueKind
		{
			get
			{
				return EdmValueKind.Decimal;
			}
		}

		// Token: 0x0400045A RID: 1114
		private readonly decimal value;
	}
}
