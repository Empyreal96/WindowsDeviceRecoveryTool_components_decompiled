using System;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Library.Values
{
	// Token: 0x020000D8 RID: 216
	public class EdmTimeConstant : EdmValue, IEdmTimeConstantExpression, IEdmExpression, IEdmTimeValue, IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x06000457 RID: 1111 RVA: 0x0000BE3C File Offset: 0x0000A03C
		public EdmTimeConstant(TimeSpan value) : this(null, value)
		{
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000BE46 File Offset: 0x0000A046
		public EdmTimeConstant(IEdmTemporalTypeReference type, TimeSpan value) : base(type)
		{
			this.value = value;
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x0000BE56 File Offset: 0x0000A056
		public TimeSpan Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x0000BE5E File Offset: 0x0000A05E
		public EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.TimeConstant;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x0000BE62 File Offset: 0x0000A062
		public override EdmValueKind ValueKind
		{
			get
			{
				return EdmValueKind.Time;
			}
		}

		// Token: 0x040001AA RID: 426
		private readonly TimeSpan value;
	}
}
