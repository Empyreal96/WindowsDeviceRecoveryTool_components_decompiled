using System;
using System.Globalization;
using System.Windows.Controls;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls.TextBoxes.Validation
{
	// Token: 0x02000041 RID: 65
	public class NonZeroNumericValidationRule : ValidationRule
	{
		// Token: 0x06000254 RID: 596 RVA: 0x0000ECE4 File Offset: 0x0000CEE4
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string text = value as string;
			if (!string.IsNullOrEmpty(text) && !text.StartsWith("0", StringComparison.Ordinal))
			{
				uint num;
				if (uint.TryParse(text, out num))
				{
					if (num != 0U)
					{
						return new ValidationResult(true, null);
					}
				}
			}
			return new ValidationResult(false, null);
		}
	}
}
