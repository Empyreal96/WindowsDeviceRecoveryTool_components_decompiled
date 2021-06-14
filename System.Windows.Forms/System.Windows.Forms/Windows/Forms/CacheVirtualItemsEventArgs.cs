using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListView.CacheVirtualItems" /> event. </summary>
	// Token: 0x02000137 RID: 311
	public class CacheVirtualItemsEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.CacheVirtualItemsEventArgs" /> class with the specified starting and ending indices.</summary>
		/// <param name="startIndex">The starting index of a range of items needed by the <see cref="T:System.Windows.Forms.ListView" /> for the next <see cref="E:System.Windows.Forms.ListView.RetrieveVirtualItem" /> event that occurs.</param>
		/// <param name="endIndex">The ending index of a range of items needed by the <see cref="T:System.Windows.Forms.ListView" /> for the next <see cref="E:System.Windows.Forms.ListView.RetrieveVirtualItem" /> event that occurs.</param>
		// Token: 0x06000987 RID: 2439 RVA: 0x0001CCB7 File Offset: 0x0001AEB7
		public CacheVirtualItemsEventArgs(int startIndex, int endIndex)
		{
			this.startIndex = startIndex;
			this.endIndex = endIndex;
		}

		/// <summary>Gets the starting index for a range of values needed by a <see cref="T:System.Windows.Forms.ListView" /> control in virtual mode.</summary>
		/// <returns>The index at the start of the range of values needed by the <see cref="T:System.Windows.Forms.ListView" /> control.</returns>
		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x0001CCCD File Offset: 0x0001AECD
		public int StartIndex
		{
			get
			{
				return this.startIndex;
			}
		}

		/// <summary>Gets the ending index for the range of values needed by a <see cref="T:System.Windows.Forms.ListView" /> control in virtual mode.</summary>
		/// <returns>The index at the end of the range of values needed by the <see cref="T:System.Windows.Forms.ListView" /> control.</returns>
		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x0001CCD5 File Offset: 0x0001AED5
		public int EndIndex
		{
			get
			{
				return this.endIndex;
			}
		}

		// Token: 0x0400069E RID: 1694
		private int startIndex;

		// Token: 0x0400069F RID: 1695
		private int endIndex;
	}
}
