using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Displays a single column header in a <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
	// Token: 0x02000145 RID: 325
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	[DefaultProperty("Text")]
	[TypeConverter(typeof(ColumnHeaderConverter))]
	public class ColumnHeader : Component, ICloneable
	{
		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x0001F1AA File Offset: 0x0001D3AA
		// (set) Token: 0x06000A51 RID: 2641 RVA: 0x0001F1B4 File Offset: 0x0001D3B4
		internal ListView OwnerListview
		{
			get
			{
				return this.listview;
			}
			set
			{
				int num = this.Width;
				this.listview = value;
				this.Width = num;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ColumnHeader" /> class.</summary>
		// Token: 0x06000A52 RID: 2642 RVA: 0x0001F1D6 File Offset: 0x0001D3D6
		public ColumnHeader()
		{
			this.imageIndexer = new ColumnHeader.ColumnHeaderImageListIndexer(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ColumnHeader" /> class with the image specified.</summary>
		/// <param name="imageIndex">The index of the image to display in the <see cref="T:System.Windows.Forms.ColumnHeader" />.</param>
		// Token: 0x06000A53 RID: 2643 RVA: 0x0001F200 File Offset: 0x0001D400
		public ColumnHeader(int imageIndex) : this()
		{
			this.ImageIndex = imageIndex;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ColumnHeader" /> class with the image specified.</summary>
		/// <param name="imageKey">The key of the image to display in the <see cref="T:System.Windows.Forms.ColumnHeader" />.</param>
		// Token: 0x06000A54 RID: 2644 RVA: 0x0001F20F File Offset: 0x0001D40F
		public ColumnHeader(string imageKey) : this()
		{
			this.ImageKey = imageKey;
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0001F220 File Offset: 0x0001D420
		internal int ActualImageIndex_Internal
		{
			get
			{
				int actualIndex = this.imageIndexer.ActualIndex;
				if (this.ImageList == null || this.ImageList.Images == null || actualIndex >= this.ImageList.Images.Count)
				{
					return -1;
				}
				return actualIndex;
			}
		}

		/// <summary>Gets or sets the display order of the column relative to the currently displayed columns.</summary>
		/// <returns>The display order of the column, relative to the currently displayed columns.</returns>
		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0001F264 File Offset: 0x0001D464
		// (set) Token: 0x06000A57 RID: 2647 RVA: 0x0001F26C File Offset: 0x0001D46C
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatBehavior")]
		[SRDescription("ColumnHeaderDisplayIndexDescr")]
		public int DisplayIndex
		{
			get
			{
				return this.DisplayIndexInternal;
			}
			set
			{
				if (this.listview == null)
				{
					this.DisplayIndexInternal = value;
					return;
				}
				if (value < 0 || value > this.listview.Columns.Count - 1)
				{
					throw new ArgumentOutOfRangeException("DisplayIndex", SR.GetString("ColumnHeaderBadDisplayIndex"));
				}
				int num = Math.Min(this.DisplayIndexInternal, value);
				int num2 = Math.Max(this.DisplayIndexInternal, value);
				int[] array = new int[this.listview.Columns.Count];
				bool flag = value > this.DisplayIndexInternal;
				ColumnHeader columnHeader = null;
				for (int i = 0; i < this.listview.Columns.Count; i++)
				{
					ColumnHeader columnHeader2 = this.listview.Columns[i];
					if (columnHeader2.DisplayIndex == this.DisplayIndexInternal)
					{
						columnHeader = columnHeader2;
					}
					else if (columnHeader2.DisplayIndex >= num && columnHeader2.DisplayIndex <= num2)
					{
						columnHeader2.DisplayIndexInternal -= (flag ? 1 : -1);
					}
					if (i != this.Index)
					{
						array[columnHeader2.DisplayIndexInternal] = i;
					}
				}
				columnHeader.DisplayIndexInternal = value;
				array[columnHeader.DisplayIndexInternal] = columnHeader.Index;
				this.SetDisplayIndices(array);
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000A58 RID: 2648 RVA: 0x0001F39C File Offset: 0x0001D59C
		// (set) Token: 0x06000A59 RID: 2649 RVA: 0x0001F3A4 File Offset: 0x0001D5A4
		internal int DisplayIndexInternal
		{
			get
			{
				return this.displayIndexInternal;
			}
			set
			{
				this.displayIndexInternal = value;
			}
		}

		/// <summary>Gets the location with the <see cref="T:System.Windows.Forms.ListView" /> control's <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" /> of this column.</summary>
		/// <returns>The zero-based index of the column header within the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" /> of the <see cref="T:System.Windows.Forms.ListView" /> control it is contained in.</returns>
		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x0001F3AD File Offset: 0x0001D5AD
		[Browsable(false)]
		public int Index
		{
			get
			{
				if (this.listview != null)
				{
					return this.listview.GetColumnIndex(this);
				}
				return -1;
			}
		}

		/// <summary>Gets or sets the index of the image displayed in the <see cref="T:System.Windows.Forms.ColumnHeader" />. </summary>
		/// <returns>The index of the image displayed in the <see cref="T:System.Windows.Forms.ColumnHeader" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <see cref="P:System.Windows.Forms.ColumnHeader.ImageIndex" /> is less than -1.</exception>
		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x0001F3C8 File Offset: 0x0001D5C8
		// (set) Token: 0x06000A5C RID: 2652 RVA: 0x0001F428 File Offset: 0x0001D628
		[DefaultValue(-1)]
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ImageIndex
		{
			get
			{
				if (this.imageIndexer.Index != -1 && this.ImageList != null && this.imageIndexer.Index >= this.ImageList.Images.Count)
				{
					return this.ImageList.Images.Count - 1;
				}
				return this.imageIndexer.Index;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("ImageIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"ImageIndex",
						value.ToString(CultureInfo.CurrentCulture),
						-1.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.imageIndexer.Index != value)
				{
					this.imageIndexer.Index = value;
					if (this.ListView != null && this.ListView.IsHandleCreated)
					{
						this.ListView.SetColumnInfo(16, this);
					}
				}
			}
		}

		/// <summary>Gets the image list associated with the <see cref="T:System.Windows.Forms.ColumnHeader" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ImageList" /> associated with the <see cref="T:System.Windows.Forms.ColumnHeader" />. </returns>
		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x0001F4B9 File Offset: 0x0001D6B9
		[Browsable(false)]
		public ImageList ImageList
		{
			get
			{
				return this.imageIndexer.ImageList;
			}
		}

		/// <summary>Gets or sets the key of the image displayed in the column.</summary>
		/// <returns>The key of the image displayed in the column.</returns>
		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x0001F4C6 File Offset: 0x0001D6C6
		// (set) Token: 0x06000A5F RID: 2655 RVA: 0x0001F4D4 File Offset: 0x0001D6D4
		[DefaultValue("")]
		[TypeConverter(typeof(ImageKeyConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string ImageKey
		{
			get
			{
				return this.imageIndexer.Key;
			}
			set
			{
				if (value != this.imageIndexer.Key)
				{
					this.imageIndexer.Key = value;
					if (this.ListView != null && this.ListView.IsHandleCreated)
					{
						this.ListView.SetColumnInfo(16, this);
					}
				}
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ListView" /> control the <see cref="T:System.Windows.Forms.ColumnHeader" /> is located in.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListView" /> control that represents the control that contains the <see cref="T:System.Windows.Forms.ColumnHeader" />.</returns>
		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000A60 RID: 2656 RVA: 0x0001F1AA File Offset: 0x0001D3AA
		[Browsable(false)]
		public ListView ListView
		{
			get
			{
				return this.listview;
			}
		}

		/// <summary>Gets or sets the name of the <see cref="T:System.Windows.Forms.ColumnHeader" />. </summary>
		/// <returns>The name of the <see cref="T:System.Windows.Forms.ColumnHeader" />. </returns>
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0001F523 File Offset: 0x0001D723
		// (set) Token: 0x06000A62 RID: 2658 RVA: 0x0001F531 File Offset: 0x0001D731
		[Browsable(false)]
		[SRDescription("ColumnHeaderNameDescr")]
		public string Name
		{
			get
			{
				return WindowsFormsUtils.GetComponentName(this, this.name);
			}
			set
			{
				if (value == null)
				{
					this.name = "";
				}
				else
				{
					this.name = value;
				}
				if (this.Site != null)
				{
					this.Site.Name = value;
				}
			}
		}

		/// <summary>Gets or sets the text displayed in the column header.</summary>
		/// <returns>The text displayed in the column header.</returns>
		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x0001F55E File Offset: 0x0001D75E
		// (set) Token: 0x06000A64 RID: 2660 RVA: 0x0001F574 File Offset: 0x0001D774
		[Localizable(true)]
		[SRDescription("ColumnCaption")]
		public string Text
		{
			get
			{
				if (this.text == null)
				{
					return "ColumnHeader";
				}
				return this.text;
			}
			set
			{
				if (value == null)
				{
					this.text = "";
				}
				else
				{
					this.text = value;
				}
				if (this.listview != null)
				{
					this.listview.SetColumnInfo(4, this);
				}
			}
		}

		/// <summary>Gets or sets the horizontal alignment of the text displayed in the <see cref="T:System.Windows.Forms.ColumnHeader" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values. The default is <see cref="F:System.Windows.Forms.HorizontalAlignment.Left" />.</returns>
		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0001F5A4 File Offset: 0x0001D7A4
		// (set) Token: 0x06000A66 RID: 2662 RVA: 0x0001F5F8 File Offset: 0x0001D7F8
		[SRDescription("ColumnAlignment")]
		[Localizable(true)]
		[DefaultValue(HorizontalAlignment.Left)]
		public HorizontalAlignment TextAlign
		{
			get
			{
				if (!this.textAlignInitialized && this.listview != null)
				{
					this.textAlignInitialized = true;
					if (this.Index != 0 && this.listview.RightToLeft == RightToLeft.Yes && !this.listview.IsMirrored)
					{
						this.textAlign = HorizontalAlignment.Right;
					}
				}
				return this.textAlign;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(HorizontalAlignment));
				}
				this.textAlign = value;
				if (this.Index == 0 && this.textAlign != HorizontalAlignment.Left)
				{
					this.textAlign = HorizontalAlignment.Left;
				}
				if (this.listview != null)
				{
					this.listview.SetColumnInfo(1, this);
					this.listview.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets an object that contains data to associate with the <see cref="T:System.Windows.Forms.ColumnHeader" />.</summary>
		/// <returns>An <see cref="T:System.Object" /> that contains data to associate with the <see cref="T:System.Windows.Forms.ColumnHeader" />.</returns>
		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x0001F669 File Offset: 0x0001D869
		// (set) Token: 0x06000A68 RID: 2664 RVA: 0x0001F671 File Offset: 0x0001D871
		[SRCategory("CatData")]
		[Localizable(false)]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0001F67A File Offset: 0x0001D87A
		internal int WidthInternal
		{
			get
			{
				return this.width;
			}
		}

		/// <summary>Gets or sets the width of the column.</summary>
		/// <returns>The width of the column, in pixels.</returns>
		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0001F684 File Offset: 0x0001D884
		// (set) Token: 0x06000A6B RID: 2667 RVA: 0x0001F75C File Offset: 0x0001D95C
		[SRDescription("ColumnWidth")]
		[Localizable(true)]
		[DefaultValue(60)]
		public int Width
		{
			get
			{
				if (this.listview != null && this.listview.IsHandleCreated && !this.listview.Disposing && this.listview.View == View.Details)
				{
					IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this.listview, this.listview.Handle), 4127, 0, 0);
					if (intPtr != IntPtr.Zero)
					{
						int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this.listview, intPtr), 4608, 0, 0);
						if (this.Index < num)
						{
							this.width = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this.listview, this.listview.Handle), 4125, this.Index, 0);
						}
					}
				}
				return this.width;
			}
			set
			{
				this.width = value;
				if (this.listview != null)
				{
					this.listview.SetColumnWidth(this.Index, ColumnHeaderAutoResizeStyle.None);
				}
			}
		}

		/// <summary>Resizes the width of the column as indicated by the resize style.</summary>
		/// <param name="headerAutoResize">One of the <see cref="T:System.Windows.Forms.ColumnHeaderAutoResizeStyle" />  values.</param>
		/// <exception cref="T:System.InvalidOperationException">A value other than <see cref="F:System.Windows.Forms.ColumnHeaderAutoResizeStyle.None" /> is passed to the <see cref="M:System.Windows.Forms.ColumnHeader.AutoResize(System.Windows.Forms.ColumnHeaderAutoResizeStyle)" /> method when the <see cref="P:System.Windows.Forms.ListView.View" /> property is a value other than <see cref="F:System.Windows.Forms.View.Details" />.</exception>
		// Token: 0x06000A6C RID: 2668 RVA: 0x0001F77F File Offset: 0x0001D97F
		public void AutoResize(ColumnHeaderAutoResizeStyle headerAutoResize)
		{
			if (headerAutoResize < ColumnHeaderAutoResizeStyle.None || headerAutoResize > ColumnHeaderAutoResizeStyle.ColumnContent)
			{
				throw new InvalidEnumArgumentException("headerAutoResize", (int)headerAutoResize, typeof(ColumnHeaderAutoResizeStyle));
			}
			if (this.listview != null)
			{
				this.listview.AutoResizeColumn(this.Index, headerAutoResize);
			}
		}

		/// <summary>Creates an identical copy of the current <see cref="T:System.Windows.Forms.ColumnHeader" /> that is not attached to any list view control.</summary>
		/// <returns>An object representing a copy of this <see cref="T:System.Windows.Forms.ColumnHeader" /> object.</returns>
		// Token: 0x06000A6D RID: 2669 RVA: 0x0001F7BC File Offset: 0x0001D9BC
		public object Clone()
		{
			Type type = base.GetType();
			ColumnHeader columnHeader;
			if (type == typeof(ColumnHeader))
			{
				columnHeader = new ColumnHeader();
			}
			else
			{
				columnHeader = (ColumnHeader)Activator.CreateInstance(type);
			}
			columnHeader.text = this.text;
			columnHeader.Width = this.width;
			columnHeader.textAlign = this.TextAlign;
			return columnHeader;
		}

		/// <summary>Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.ColumnHeader" />.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x06000A6E RID: 2670 RVA: 0x0001F820 File Offset: 0x0001DA20
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.listview != null)
			{
				int num = this.Index;
				if (num != -1)
				{
					this.listview.Columns.RemoveAt(num);
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0001F85B File Offset: 0x0001DA5B
		private void ResetText()
		{
			this.Text = null;
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x0001F864 File Offset: 0x0001DA64
		private void SetDisplayIndices(int[] cols)
		{
			if (this.listview.IsHandleCreated && !this.listview.Disposing)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this.listview, this.listview.Handle), 4154, cols.Length, cols);
			}
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x0001F8B0 File Offset: 0x0001DAB0
		private bool ShouldSerializeName()
		{
			return !string.IsNullOrEmpty(this.name);
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x0001F8C0 File Offset: 0x0001DAC0
		private bool ShouldSerializeDisplayIndex()
		{
			return this.DisplayIndex != this.Index;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x0001F8D3 File Offset: 0x0001DAD3
		internal bool ShouldSerializeText()
		{
			return this.text != null;
		}

		/// <summary>Returns a <see cref="T:System.String" /> containing the name of the <see cref="T:System.ComponentModel.Component" />, if any. This method should not be overridden.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the name of the <see cref="T:System.ComponentModel.Component" />, if any, or <see langword="null" /> if the <see cref="T:System.ComponentModel.Component" /> is unnamed.</returns>
		// Token: 0x06000A74 RID: 2676 RVA: 0x0001F8DE File Offset: 0x0001DADE
		public override string ToString()
		{
			return "ColumnHeader: Text: " + this.Text;
		}

		// Token: 0x040006DF RID: 1759
		internal int index = -1;

		// Token: 0x040006E0 RID: 1760
		internal string text;

		// Token: 0x040006E1 RID: 1761
		internal string name;

		// Token: 0x040006E2 RID: 1762
		internal int width = 60;

		// Token: 0x040006E3 RID: 1763
		private HorizontalAlignment textAlign;

		// Token: 0x040006E4 RID: 1764
		private bool textAlignInitialized;

		// Token: 0x040006E5 RID: 1765
		private int displayIndexInternal = -1;

		// Token: 0x040006E6 RID: 1766
		private ColumnHeader.ColumnHeaderImageListIndexer imageIndexer;

		// Token: 0x040006E7 RID: 1767
		private object userData;

		// Token: 0x040006E8 RID: 1768
		private ListView listview;

		// Token: 0x02000566 RID: 1382
		internal class ColumnHeaderImageListIndexer : ImageList.Indexer
		{
			// Token: 0x06005675 RID: 22133 RVA: 0x0016A134 File Offset: 0x00168334
			public ColumnHeaderImageListIndexer(ColumnHeader ch)
			{
				this.owner = ch;
			}

			// Token: 0x17001494 RID: 5268
			// (get) Token: 0x06005676 RID: 22134 RVA: 0x0016A143 File Offset: 0x00168343
			// (set) Token: 0x06005677 RID: 22135 RVA: 0x0000701A File Offset: 0x0000521A
			public override ImageList ImageList
			{
				get
				{
					if (this.owner != null && this.owner.ListView != null)
					{
						return this.owner.ListView.SmallImageList;
					}
					return null;
				}
				set
				{
				}
			}

			// Token: 0x040037ED RID: 14317
			private ColumnHeader owner;
		}
	}
}
