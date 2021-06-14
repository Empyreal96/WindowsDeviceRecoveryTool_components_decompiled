using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.CheckedListBox.ItemCheck" /> event of the <see cref="T:System.Windows.Forms.CheckedListBox" /> and <see cref="T:System.Windows.Forms.ListView" /> controls. </summary>
	// Token: 0x0200029B RID: 667
	[ComVisible(true)]
	public class ItemCheckEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ItemCheckEventArgs" /> class.</summary>
		/// <param name="index">The zero-based index of the item to change. </param>
		/// <param name="newCheckValue">One of the <see cref="T:System.Windows.Forms.CheckState" /> values that indicates whether to change the check box for the item to be checked, unchecked, or indeterminate. </param>
		/// <param name="currentValue">One of the <see cref="T:System.Windows.Forms.CheckState" /> values that indicates whether the check box for the item is currently checked, unchecked, or indeterminate. </param>
		// Token: 0x060026F4 RID: 9972 RVA: 0x000B76CC File Offset: 0x000B58CC
		public ItemCheckEventArgs(int index, CheckState newCheckValue, CheckState currentValue)
		{
			this.index = index;
			this.newValue = newCheckValue;
			this.currentValue = currentValue;
		}

		/// <summary>Gets the zero-based index of the item to change.</summary>
		/// <returns>The zero-based index of the item to change.</returns>
		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x060026F5 RID: 9973 RVA: 0x000B76E9 File Offset: 0x000B58E9
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		/// <summary>Gets or sets a value indicating whether to set the check box for the item to be checked, unchecked, or indeterminate.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.CheckState" /> values.</returns>
		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x060026F6 RID: 9974 RVA: 0x000B76F1 File Offset: 0x000B58F1
		// (set) Token: 0x060026F7 RID: 9975 RVA: 0x000B76F9 File Offset: 0x000B58F9
		public CheckState NewValue
		{
			get
			{
				return this.newValue;
			}
			set
			{
				this.newValue = value;
			}
		}

		/// <summary>Gets a value indicating the current state of the item's check box.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.CheckState" /> values.</returns>
		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x060026F8 RID: 9976 RVA: 0x000B7702 File Offset: 0x000B5902
		public CheckState CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x04001073 RID: 4211
		private readonly int index;

		// Token: 0x04001074 RID: 4212
		private CheckState newValue;

		// Token: 0x04001075 RID: 4213
		private readonly CheckState currentValue;
	}
}
