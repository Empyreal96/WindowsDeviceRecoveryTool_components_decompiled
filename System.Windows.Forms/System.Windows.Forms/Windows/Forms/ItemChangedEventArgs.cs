using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.CurrencyManager.ItemChanged" /> event.</summary>
	// Token: 0x02000297 RID: 663
	public class ItemChangedEventArgs : EventArgs
	{
		// Token: 0x060026E8 RID: 9960 RVA: 0x000B769E File Offset: 0x000B589E
		internal ItemChangedEventArgs(int index)
		{
			this.index = index;
		}

		/// <summary>Indicates the position of the item being changed within the list.</summary>
		/// <returns>The zero-based index to the item being changed.</returns>
		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x060026E9 RID: 9961 RVA: 0x000B76AD File Offset: 0x000B58AD
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x04001071 RID: 4209
		private int index;
	}
}
