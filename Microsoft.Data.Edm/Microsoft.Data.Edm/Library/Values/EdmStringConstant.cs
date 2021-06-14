using System;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Library.Values
{
	// Token: 0x020001DC RID: 476
	public class EdmStringConstant : EdmValue, IEdmStringConstantExpression, IEdmExpression, IEdmStringValue, IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x06000B4E RID: 2894 RVA: 0x00020D01 File Offset: 0x0001EF01
		public EdmStringConstant(string value) : this(null, value)
		{
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x00020D0B File Offset: 0x0001EF0B
		public EdmStringConstant(IEdmStringTypeReference type, string value) : base(type)
		{
			EdmUtil.CheckArgumentNull<string>(value, "value");
			this.value = value;
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x00020D27 File Offset: 0x0001EF27
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06000B51 RID: 2897 RVA: 0x00020D2F File Offset: 0x0001EF2F
		public EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.StringConstant;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x00020D33 File Offset: 0x0001EF33
		public override EdmValueKind ValueKind
		{
			get
			{
				return EdmValueKind.String;
			}
		}

		// Token: 0x0400054C RID: 1356
		private readonly string value;
	}
}
