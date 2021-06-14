using System;
using System.Globalization;
using System.Windows.Controls;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls.TextBoxes
{
	// Token: 0x0200003E RID: 62
	public class ProxyAddressValidationRule : ValidationRule
	{
		// Token: 0x06000234 RID: 564 RVA: 0x0000E6DC File Offset: 0x0000C8DC
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string text = value as string;
			ValidationResult result;
			if (text != null)
			{
				if (text.Length > 0 && !ProxyAddressValidationRule.IsStringValidUri(text))
				{
					result = new ValidationResult(false, null);
				}
				else
				{
					result = new ValidationResult(true, null);
				}
			}
			else
			{
				result = new ValidationResult(false, null);
			}
			return result;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000E734 File Offset: 0x0000C934
		private static bool IsStringValidUri(string str)
		{
			return Uri.IsWellFormedUriString(Uri.EscapeUriString(str), UriKind.RelativeOrAbsolute);
		}
	}
}
