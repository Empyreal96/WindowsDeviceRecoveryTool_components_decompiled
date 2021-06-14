using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see langword="MeasureItem" /> event of the <see cref="T:System.Windows.Forms.ListBox" />, <see cref="T:System.Windows.Forms.ComboBox" />, <see cref="T:System.Windows.Forms.CheckedListBox" />, and <see cref="T:System.Windows.Forms.MenuItem" /> controls.</summary>
	// Token: 0x020002E0 RID: 736
	public class MeasureItemEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MeasureItemEventArgs" /> class providing a parameter for the item height.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> object being written to. </param>
		/// <param name="index">The index of the item for which you need the height or width. </param>
		/// <param name="itemHeight">The height of the item to measure relative to the <paramref name="graphics" /> object. </param>
		// Token: 0x06002C3E RID: 11326 RVA: 0x000CE915 File Offset: 0x000CCB15
		public MeasureItemEventArgs(Graphics graphics, int index, int itemHeight)
		{
			this.graphics = graphics;
			this.index = index;
			this.itemHeight = itemHeight;
			this.itemWidth = 0;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MeasureItemEventArgs" /> class.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> object being written to. </param>
		/// <param name="index">The index of the item for which you need the height or width. </param>
		// Token: 0x06002C3F RID: 11327 RVA: 0x000CE939 File Offset: 0x000CCB39
		public MeasureItemEventArgs(Graphics graphics, int index)
		{
			this.graphics = graphics;
			this.index = index;
			this.itemHeight = 0;
			this.itemWidth = 0;
		}

		/// <summary>Gets the <see cref="T:System.Drawing.Graphics" /> object to measure against.</summary>
		/// <returns>The <see cref="T:System.Drawing.Graphics" /> object to use to determine the scale of the item you are drawing.</returns>
		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06002C40 RID: 11328 RVA: 0x000CE95D File Offset: 0x000CCB5D
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		/// <summary>Gets the index of the item for which the height and width is needed.</summary>
		/// <returns>The index of the item to be measured.</returns>
		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06002C41 RID: 11329 RVA: 0x000CE965 File Offset: 0x000CCB65
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		/// <summary>Gets or sets the height of the item specified by the <see cref="P:System.Windows.Forms.MeasureItemEventArgs.Index" />.</summary>
		/// <returns>The height of the item measured.</returns>
		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06002C42 RID: 11330 RVA: 0x000CE96D File Offset: 0x000CCB6D
		// (set) Token: 0x06002C43 RID: 11331 RVA: 0x000CE975 File Offset: 0x000CCB75
		public int ItemHeight
		{
			get
			{
				return this.itemHeight;
			}
			set
			{
				this.itemHeight = value;
			}
		}

		/// <summary>Gets or sets the width of the item specified by the <see cref="P:System.Windows.Forms.MeasureItemEventArgs.Index" />.</summary>
		/// <returns>The width of the item measured.</returns>
		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06002C44 RID: 11332 RVA: 0x000CE97E File Offset: 0x000CCB7E
		// (set) Token: 0x06002C45 RID: 11333 RVA: 0x000CE986 File Offset: 0x000CCB86
		public int ItemWidth
		{
			get
			{
				return this.itemWidth;
			}
			set
			{
				this.itemWidth = value;
			}
		}

		// Token: 0x040012EA RID: 4842
		private int itemHeight;

		// Token: 0x040012EB RID: 4843
		private int itemWidth;

		// Token: 0x040012EC RID: 4844
		private int index;

		// Token: 0x040012ED RID: 4845
		private readonly Graphics graphics;
	}
}
