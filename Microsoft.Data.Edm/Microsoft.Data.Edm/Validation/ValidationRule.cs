using System;

namespace Microsoft.Data.Edm.Validation
{
	// Token: 0x0200023B RID: 571
	public abstract class ValidationRule
	{
		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06000D0F RID: 3343
		internal abstract Type ValidatedType { get; }

		// Token: 0x06000D10 RID: 3344
		internal abstract void Evaluate(ValidationContext context, object item);
	}
}
