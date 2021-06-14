using System;
using System.Globalization;

namespace System.Windows.Controls
{
	// Token: 0x0200048F RID: 1167
	internal sealed class ConversionValidationRule : ValidationRule
	{
		// Token: 0x060044C4 RID: 17604 RVA: 0x00138512 File Offset: 0x00136712
		internal ConversionValidationRule() : base(ValidationStep.ConvertedProposedValue, false)
		{
		}

		// Token: 0x060044C5 RID: 17605 RVA: 0x0013851C File Offset: 0x0013671C
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			return ValidationResult.ValidResult;
		}

		// Token: 0x040028B1 RID: 10417
		internal static readonly ConversionValidationRule Instance = new ConversionValidationRule();
	}
}
