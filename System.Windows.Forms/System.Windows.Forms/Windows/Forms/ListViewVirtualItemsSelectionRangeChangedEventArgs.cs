using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListView.VirtualItemsSelectionRangeChanged" /> event. </summary>
	// Token: 0x020002D2 RID: 722
	public class ListViewVirtualItemsSelectionRangeChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewVirtualItemsSelectionRangeChangedEventArgs" /> class. </summary>
		/// <param name="startIndex">The index of the first item in the range that has changed.</param>
		/// <param name="endIndex">The index of the last item in the range that has changed.</param>
		/// <param name="isSelected">
		///       <see langword="true" /> to indicate the items are selected; <see langword="false" /> to indicate the items are deselected.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="startIndex" /> is larger than <paramref name="endIndex." /></exception>
		// Token: 0x06002B51 RID: 11089 RVA: 0x000CAE6D File Offset: 0x000C906D
		public ListViewVirtualItemsSelectionRangeChangedEventArgs(int startIndex, int endIndex, bool isSelected)
		{
			if (startIndex > endIndex)
			{
				throw new ArgumentException(SR.GetString("ListViewStartIndexCannotBeLargerThanEndIndex"));
			}
			this.startIndex = startIndex;
			this.endIndex = endIndex;
			this.isSelected = isSelected;
		}

		/// <summary>Gets the index for the last item in the range of items whose selection state has changed</summary>
		/// <returns>The index of the last item in the range of items whose selection state has changed.</returns>
		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06002B52 RID: 11090 RVA: 0x000CAE9E File Offset: 0x000C909E
		public int EndIndex
		{
			get
			{
				return this.endIndex;
			}
		}

		/// <summary>Gets a value indicating whether the range of items is selected. </summary>
		/// <returns>
		///     <see langword="true" /> if the range of items is selected; <see langword="false" /> if the range of items is deselected.</returns>
		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06002B53 RID: 11091 RVA: 0x000CAEA6 File Offset: 0x000C90A6
		public bool IsSelected
		{
			get
			{
				return this.isSelected;
			}
		}

		/// <summary>Gets the index for the first item in the range of items whose selection state has changed.</summary>
		/// <returns>The index of the first item in the range of items whose selection state has changed.</returns>
		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06002B54 RID: 11092 RVA: 0x000CAEAE File Offset: 0x000C90AE
		public int StartIndex
		{
			get
			{
				return this.startIndex;
			}
		}

		// Token: 0x04001291 RID: 4753
		private int startIndex;

		// Token: 0x04001292 RID: 4754
		private int endIndex;

		// Token: 0x04001293 RID: 4755
		private bool isSelected;
	}
}
