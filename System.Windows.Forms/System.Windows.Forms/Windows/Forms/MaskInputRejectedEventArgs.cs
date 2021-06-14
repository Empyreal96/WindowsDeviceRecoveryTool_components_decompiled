using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.MaskedTextBox.MaskInputRejected" /> event.</summary>
	// Token: 0x020002D8 RID: 728
	public class MaskInputRejectedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MaskInputRejectedEventArgs" /> class.</summary>
		/// <param name="position">An <see cref="T:System.Int32" /> value that contains the zero-based position of the character that failed the mask. The position includes literal characters.</param>
		/// <param name="rejectionHint">A <see cref="T:System.ComponentModel.MaskedTextResultHint" /> that generally describes why the character was rejected.</param>
		// Token: 0x06002C07 RID: 11271 RVA: 0x000CD5DE File Offset: 0x000CB7DE
		public MaskInputRejectedEventArgs(int position, MaskedTextResultHint rejectionHint)
		{
			this.position = position;
			this.hint = rejectionHint;
		}

		/// <summary>Gets the position in the mask corresponding to the invalid input character.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that contains the zero-based position of the character that failed the mask. The position includes literal characters.</returns>
		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06002C08 RID: 11272 RVA: 0x000CD5F4 File Offset: 0x000CB7F4
		public int Position
		{
			get
			{
				return this.position;
			}
		}

		/// <summary>Gets an enumerated value that describes why the input character was rejected.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.MaskedTextResultHint" /> that generally describes why the character was rejected.</returns>
		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06002C09 RID: 11273 RVA: 0x000CD5FC File Offset: 0x000CB7FC
		public MaskedTextResultHint RejectionHint
		{
			get
			{
				return this.hint;
			}
		}

		// Token: 0x040012D4 RID: 4820
		private int position;

		// Token: 0x040012D5 RID: 4821
		private MaskedTextResultHint hint;
	}
}
