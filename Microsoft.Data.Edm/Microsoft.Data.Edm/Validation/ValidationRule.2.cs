using System;

namespace Microsoft.Data.Edm.Validation
{
	// Token: 0x0200023C RID: 572
	public sealed class ValidationRule<TItem> : ValidationRule where TItem : IEdmElement
	{
		// Token: 0x06000D12 RID: 3346 RVA: 0x000294BD File Offset: 0x000276BD
		public ValidationRule(Action<ValidationContext, TItem> validate)
		{
			this.validate = validate;
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06000D13 RID: 3347 RVA: 0x000294CC File Offset: 0x000276CC
		internal override Type ValidatedType
		{
			get
			{
				return typeof(TItem);
			}
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x000294D8 File Offset: 0x000276D8
		internal override void Evaluate(ValidationContext context, object item)
		{
			TItem arg = (TItem)((object)item);
			this.validate(context, arg);
		}

		// Token: 0x0400067C RID: 1660
		private readonly Action<ValidationContext, TItem> validate;
	}
}
