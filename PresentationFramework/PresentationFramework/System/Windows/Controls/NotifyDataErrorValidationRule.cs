using System;
using System.Globalization;

namespace System.Windows.Controls
{
	/// <summary>Represents a rule that checks for errors that are raised by a data source that implements <see cref="T:System.ComponentModel.INotifyDataErrorInfo" />.</summary>
	// Token: 0x02000506 RID: 1286
	public sealed class NotifyDataErrorValidationRule : ValidationRule
	{
		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Controls.NotifyDataErrorValidationRule" /> class.</summary>
		// Token: 0x0600528E RID: 21134 RVA: 0x0013885E File Offset: 0x00136A5E
		public NotifyDataErrorValidationRule() : base(ValidationStep.UpdatedValue, true)
		{
		}

		/// <summary>Performs validation checks on a value.</summary>
		/// <param name="value">The value (from the binding target) to check.</param>
		/// <param name="cultureInfo">The culture to use in this rule.</param>
		/// <returns>A <see cref="T:System.Windows.Controls.ValidationResult" /> object.</returns>
		// Token: 0x0600528F RID: 21135 RVA: 0x0013851C File Offset: 0x0013671C
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			return ValidationResult.ValidResult;
		}

		// Token: 0x04002CB5 RID: 11445
		internal static readonly NotifyDataErrorValidationRule Instance = new NotifyDataErrorValidationRule();
	}
}
