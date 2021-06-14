using System;

namespace System.Windows.Documents
{
	/// <summary>Specifies how a <see cref="T:System.Windows.Controls.RichTextBox" /> should handle a custom text element.</summary>
	// Token: 0x02000403 RID: 1027
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class TextElementEditingBehaviorAttribute : Attribute
	{
		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Controls.RichTextBox" /> can merge two adjacent text elements.</summary>
		/// <returns>
		///     <see langword="true" /> if a <see cref="T:System.Windows.Controls.RichTextBox" /> is free to merge adjacent custom text elements that have identical property values; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x060039A9 RID: 14761 RVA: 0x001058CD File Offset: 0x00103ACD
		// (set) Token: 0x060039AA RID: 14762 RVA: 0x001058D5 File Offset: 0x00103AD5
		public bool IsMergeable
		{
			get
			{
				return this._isMergeable;
			}
			set
			{
				this._isMergeable = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the text element provides formatting on a character basis, or if the formatting applies to the entire element. </summary>
		/// <returns>
		///     <see langword="true" /> if formatting should apply to the individual characters; <see langword="false" /> if the formatting should apply to the entire element.</returns>
		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x060039AB RID: 14763 RVA: 0x001058DE File Offset: 0x00103ADE
		// (set) Token: 0x060039AC RID: 14764 RVA: 0x001058E6 File Offset: 0x00103AE6
		public bool IsTypographicOnly
		{
			get
			{
				return this._isTypographicOnly;
			}
			set
			{
				this._isTypographicOnly = value;
			}
		}

		// Token: 0x040025B2 RID: 9650
		private bool _isMergeable;

		// Token: 0x040025B3 RID: 9651
		private bool _isTypographicOnly;
	}
}
