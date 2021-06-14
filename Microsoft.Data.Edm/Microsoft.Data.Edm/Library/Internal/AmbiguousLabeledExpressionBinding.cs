using System;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library.Values;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000E1 RID: 225
	internal class AmbiguousLabeledExpressionBinding : AmbiguousBinding<IEdmLabeledExpression>, IEdmLabeledExpression, IEdmNamedElement, IEdmExpression, IEdmElement
	{
		// Token: 0x06000484 RID: 1156 RVA: 0x0000C0B1 File Offset: 0x0000A2B1
		public AmbiguousLabeledExpressionBinding(IEdmLabeledExpression first, IEdmLabeledExpression second) : base(first, second)
		{
			this.first = first;
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x0000C0CD File Offset: 0x0000A2CD
		public IEdmExpression Expression
		{
			get
			{
				return this.expressionCache.GetValue(this, AmbiguousLabeledExpressionBinding.ComputeExpressionFunc, null);
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x0000C0E1 File Offset: 0x0000A2E1
		public EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.Labeled;
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000C0E5 File Offset: 0x0000A2E5
		private IEdmExpression ComputeExpression()
		{
			return EdmNullExpression.Instance;
		}

		// Token: 0x040001AD RID: 429
		private readonly IEdmLabeledExpression first;

		// Token: 0x040001AE RID: 430
		private readonly Cache<AmbiguousLabeledExpressionBinding, IEdmExpression> expressionCache = new Cache<AmbiguousLabeledExpressionBinding, IEdmExpression>();

		// Token: 0x040001AF RID: 431
		private static readonly Func<AmbiguousLabeledExpressionBinding, IEdmExpression> ComputeExpressionFunc = (AmbiguousLabeledExpressionBinding me) => me.ComputeExpression();
	}
}
