using System;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Library.Values
{
	// Token: 0x020001A2 RID: 418
	public class EdmGuidConstant : EdmValue, IEdmGuidConstantExpression, IEdmExpression, IEdmGuidValue, IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x0600091B RID: 2331 RVA: 0x00018786 File Offset: 0x00016986
		public EdmGuidConstant(Guid value) : this(null, value)
		{
			this.value = value;
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00018797 File Offset: 0x00016997
		public EdmGuidConstant(IEdmPrimitiveTypeReference type, Guid value) : base(type)
		{
			this.value = value;
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x000187A7 File Offset: 0x000169A7
		public Guid Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x000187AF File Offset: 0x000169AF
		public EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.GuidConstant;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x0600091F RID: 2335 RVA: 0x000187B2 File Offset: 0x000169B2
		public override EdmValueKind ValueKind
		{
			get
			{
				return EdmValueKind.Guid;
			}
		}

		// Token: 0x04000473 RID: 1139
		private readonly Guid value;
	}
}
