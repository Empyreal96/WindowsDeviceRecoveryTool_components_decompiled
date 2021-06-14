using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListView.SearchForVirtualItem" /> event. </summary>
	// Token: 0x0200034F RID: 847
	public class SearchForVirtualItemEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.SearchForVirtualItemEventArgs" /> class. </summary>
		/// <param name="isTextSearch">A value indicating whether the search is a text search.</param>
		/// <param name="isPrefixSearch">A value indicating whether the search is a prefix search.</param>
		/// <param name="includeSubItemsInSearch">A value indicating whether to include subitems of list items in the search.</param>
		/// <param name="text">The text of the item to search for.</param>
		/// <param name="startingPoint">The <see cref="T:System.Drawing.Point" /> at which to start the search.</param>
		/// <param name="direction">One of the <see cref="T:System.Windows.Forms.SearchDirectionHint" /> values.</param>
		/// <param name="startIndex">The index of the <see cref="T:System.Windows.Forms.ListViewItem" /> at which to start the search.</param>
		// Token: 0x0600350A RID: 13578 RVA: 0x000F2094 File Offset: 0x000F0294
		public SearchForVirtualItemEventArgs(bool isTextSearch, bool isPrefixSearch, bool includeSubItemsInSearch, string text, Point startingPoint, SearchDirectionHint direction, int startIndex)
		{
			this.isTextSearch = isTextSearch;
			this.isPrefixSearch = isPrefixSearch;
			this.includeSubItemsInSearch = includeSubItemsInSearch;
			this.text = text;
			this.startingPoint = startingPoint;
			this.direction = direction;
			this.startIndex = startIndex;
		}

		/// <summary>Gets a value indicating whether the search is a text search.</summary>
		/// <returns>
		///     <see langword="true" /> if the search is a text search; <see langword="false" /> if the search is a location search.</returns>
		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x0600350B RID: 13579 RVA: 0x000F20E3 File Offset: 0x000F02E3
		public bool IsTextSearch
		{
			get
			{
				return this.isTextSearch;
			}
		}

		/// <summary>Gets a value indicating whether the search should include subitems of list items.</summary>
		/// <returns>
		///     <see langword="true" /> if subitems should be included in the search; otherwise, <see langword="false" />. </returns>
		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x0600350C RID: 13580 RVA: 0x000F20EB File Offset: 0x000F02EB
		public bool IncludeSubItemsInSearch
		{
			get
			{
				return this.includeSubItemsInSearch;
			}
		}

		/// <summary>Gets or sets the index of the <see cref="T:System.Windows.Forms.ListViewItem" /> found in the <see cref="T:System.Windows.Forms.ListView" /> .</summary>
		/// <returns>The index of the <see cref="T:System.Windows.Forms.ListViewItem" /> found in the <see cref="T:System.Windows.Forms.ListView" />.</returns>
		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x0600350D RID: 13581 RVA: 0x000F20F3 File Offset: 0x000F02F3
		// (set) Token: 0x0600350E RID: 13582 RVA: 0x000F20FB File Offset: 0x000F02FB
		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		/// <summary>Gets a value indicating whether the search should return an item if its text starts with the search text.</summary>
		/// <returns>
		///     <see langword="true" /> if the search should match item text that starts with the search text; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x0600350F RID: 13583 RVA: 0x000F2104 File Offset: 0x000F0304
		public bool IsPrefixSearch
		{
			get
			{
				return this.isPrefixSearch;
			}
		}

		/// <summary>Gets the text used to find an item in the <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
		/// <returns>The text used to find an item in the <see cref="T:System.Windows.Forms.ListView" /> control.</returns>
		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06003510 RID: 13584 RVA: 0x000F210C File Offset: 0x000F030C
		public string Text
		{
			get
			{
				return this.text;
			}
		}

		/// <summary>Gets the starting location of the search.</summary>
		/// <returns>The <see cref="T:System.Drawing.Point" /> that indicates the starting location of the search.</returns>
		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06003511 RID: 13585 RVA: 0x000F2114 File Offset: 0x000F0314
		public Point StartingPoint
		{
			get
			{
				return this.startingPoint;
			}
		}

		/// <summary>Gets the direction from the current item that the search should take place.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.SearchDirectionHint" /> values.</returns>
		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06003512 RID: 13586 RVA: 0x000F211C File Offset: 0x000F031C
		public SearchDirectionHint Direction
		{
			get
			{
				return this.direction;
			}
		}

		/// <summary>Gets the index of the <see cref="T:System.Windows.Forms.ListViewItem" /> where the search starts.</summary>
		/// <returns>The index of the <see cref="T:System.Windows.Forms.ListViewItem" /> indicating where the search starts</returns>
		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06003513 RID: 13587 RVA: 0x000F2124 File Offset: 0x000F0324
		public int StartIndex
		{
			get
			{
				return this.startIndex;
			}
		}

		// Token: 0x04002098 RID: 8344
		private bool isTextSearch;

		// Token: 0x04002099 RID: 8345
		private bool isPrefixSearch;

		// Token: 0x0400209A RID: 8346
		private bool includeSubItemsInSearch;

		// Token: 0x0400209B RID: 8347
		private string text;

		// Token: 0x0400209C RID: 8348
		private Point startingPoint;

		// Token: 0x0400209D RID: 8349
		private SearchDirectionHint direction;

		// Token: 0x0400209E RID: 8350
		private int startIndex;

		// Token: 0x0400209F RID: 8351
		private int index = -1;
	}
}
