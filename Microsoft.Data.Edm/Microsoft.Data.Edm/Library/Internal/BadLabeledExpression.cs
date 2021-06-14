using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library.Values;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000FE RID: 254
	internal class BadLabeledExpression : BadElement, IEdmLabeledExpression, IEdmNamedElement, IEdmExpression, IEdmElement
	{
		// Token: 0x060004E8 RID: 1256 RVA: 0x0000C700 File Offset: 0x0000A900
		public BadLabeledExpression(string name, IEnumerable<EdmError> errors) : base(errors)
		{
			this.name = (name ?? string.Empty);
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x0000C724 File Offset: 0x0000A924
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x0000C72C File Offset: 0x0000A92C
		public EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.Labeled;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0000C730 File Offset: 0x0000A930
		public IEdmExpression Expression
		{
			get
			{
				return this.expressionCache.GetValue(this, BadLabeledExpression.ComputeExpressionFunc, null);
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0000C744 File Offset: 0x0000A944
		private IEdmExpression ComputeExpression()
		{
			return EdmNullExpression.Instance;
		}

		// Token: 0x040001CF RID: 463
		private readonly string name;

		// Token: 0x040001D0 RID: 464
		private readonly Cache<BadLabeledExpression, IEdmExpression> expressionCache = new Cache<BadLabeledExpression, IEdmExpression>();

		// Token: 0x040001D1 RID: 465
		private static readonly Func<BadLabeledExpression, IEdmExpression> ComputeExpressionFunc = (BadLabeledExpression me) => me.ComputeExpression();
	}
}
