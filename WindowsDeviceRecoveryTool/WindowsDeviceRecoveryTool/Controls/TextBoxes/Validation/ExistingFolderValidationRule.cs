using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls.TextBoxes.Validation
{
	// Token: 0x02000040 RID: 64
	public class ExistingFolderValidationRule : ValidationRule
	{
		// Token: 0x06000251 RID: 593 RVA: 0x0000EC08 File Offset: 0x0000CE08
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string text = value as string;
			if (!string.IsNullOrEmpty(text))
			{
				if (Directory.Exists(text) && ExistingFolderValidationRule.IsValidPath(text))
				{
					return new ValidationResult(true, null);
				}
			}
			return new ValidationResult(false, null);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000EC58 File Offset: 0x0000CE58
		private static bool IsValidPath(string path)
		{
			return !path.Contains(" \\") && !path.Contains("\\\\") && !path.Contains("/") && !path.StartsWith(" ", StringComparison.Ordinal) && !path.StartsWith(".", StringComparison.Ordinal) && !path.StartsWith("\\", StringComparison.Ordinal) && !path.EndsWith(" ", StringComparison.Ordinal) && !path.EndsWith(".", StringComparison.Ordinal);
		}
	}
}
