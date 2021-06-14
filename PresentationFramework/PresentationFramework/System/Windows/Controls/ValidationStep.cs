using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies when a <see cref="T:System.Windows.Controls.ValidationRule" /> runs.</summary>
	// Token: 0x02000556 RID: 1366
	public enum ValidationStep
	{
		/// <summary>Runs the <see cref="T:System.Windows.Controls.ValidationRule" /> before any conversion occurs.</summary>
		// Token: 0x04002F19 RID: 12057
		RawProposedValue,
		/// <summary>Runs the <see cref="T:System.Windows.Controls.ValidationRule" /> after the value is converted.</summary>
		// Token: 0x04002F1A RID: 12058
		ConvertedProposedValue,
		/// <summary>Runs the <see cref="T:System.Windows.Controls.ValidationRule" /> after the source is updated.</summary>
		// Token: 0x04002F1B RID: 12059
		UpdatedValue,
		/// <summary>Runs the <see cref="T:System.Windows.Controls.ValidationRule" /> after the value has been committed to the source.</summary>
		// Token: 0x04002F1C RID: 12060
		CommittedValue
	}
}
