using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents an item in a <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
	// Token: 0x020002CC RID: 716
	[TypeConverter(typeof(ListViewItemConverter))]
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	[DefaultProperty("Text")]
	[Serializable]
	public class ListViewItem : ICloneable, ISerializable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with default values.</summary>
		// Token: 0x06002AE3 RID: 10979 RVA: 0x000C96A0 File Offset: 0x000C78A0
		public ListViewItem()
		{
			this.StateSelected = false;
			this.UseItemStyleForSubItems = true;
			this.SavedStateImageIndex = -1;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the specified serialization information and streaming context.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> containing information about the <see cref="T:System.Windows.Forms.ListViewItem" /> to be initialized.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that indicates the source destination and context information of a serialized stream.</param>
		// Token: 0x06002AE4 RID: 10980 RVA: 0x000C96EE File Offset: 0x000C78EE
		protected ListViewItem(SerializationInfo info, StreamingContext context) : this()
		{
			this.Deserialize(info, context);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the specified item text.</summary>
		/// <param name="text">The text to display for the item. This should not exceed 259 characters.</param>
		// Token: 0x06002AE5 RID: 10981 RVA: 0x000C96FE File Offset: 0x000C78FE
		public ListViewItem(string text) : this(text, -1)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the specified item text and the image index position of the item's icon.</summary>
		/// <param name="text">The text to display for the item. This should not exceed 259 characters.</param>
		/// <param name="imageIndex">The zero-based index of the image within the <see cref="T:System.Windows.Forms.ImageList" /> associated with the <see cref="T:System.Windows.Forms.ListView" /> that contains the item. </param>
		// Token: 0x06002AE6 RID: 10982 RVA: 0x000C9708 File Offset: 0x000C7908
		public ListViewItem(string text, int imageIndex) : this()
		{
			this.ImageIndexer.Index = imageIndex;
			this.Text = text;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with an array of strings representing subitems.</summary>
		/// <param name="items">An array of strings that represent the subitems of the new item. </param>
		// Token: 0x06002AE7 RID: 10983 RVA: 0x000C9723 File Offset: 0x000C7923
		public ListViewItem(string[] items) : this(items, -1)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the image index position of the item's icon and an array of strings representing subitems.</summary>
		/// <param name="items">An array of strings that represent the subitems of the new item. </param>
		/// <param name="imageIndex">The zero-based index of the image within the <see cref="T:System.Windows.Forms.ImageList" /> associated with the <see cref="T:System.Windows.Forms.ListView" /> that contains the item. </param>
		// Token: 0x06002AE8 RID: 10984 RVA: 0x000C9730 File Offset: 0x000C7930
		public ListViewItem(string[] items, int imageIndex) : this()
		{
			this.ImageIndexer.Index = imageIndex;
			if (items != null && items.Length != 0)
			{
				this.subItems = new ListViewItem.ListViewSubItem[items.Length];
				for (int i = 0; i < items.Length; i++)
				{
					this.subItems[i] = new ListViewItem.ListViewSubItem(this, items[i]);
				}
				this.SubItemCount = items.Length;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the image index position of the item's icon; the foreground color, background color, and font of the item; and an array of strings representing subitems.</summary>
		/// <param name="items">An array of strings that represent the subitems of the new item. </param>
		/// <param name="imageIndex">The zero-based index of the image within the <see cref="T:System.Windows.Forms.ImageList" /> associated with the <see cref="T:System.Windows.Forms.ListView" /> that contains the item. </param>
		/// <param name="foreColor">A <see cref="T:System.Drawing.Color" /> that represents the foreground color of the item. </param>
		/// <param name="backColor">A <see cref="T:System.Drawing.Color" /> that represents the background color of the item. </param>
		/// <param name="font">A <see cref="T:System.Drawing.Font" /> that represents the font to display the item's text in. </param>
		// Token: 0x06002AE9 RID: 10985 RVA: 0x000C978C File Offset: 0x000C798C
		public ListViewItem(string[] items, int imageIndex, Color foreColor, Color backColor, Font font) : this(items, imageIndex)
		{
			this.ForeColor = foreColor;
			this.BackColor = backColor;
			this.Font = font;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the image index position of the item's icon and an array of <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> objects.</summary>
		/// <param name="subItems">An array of type <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> that represents the subitems of the item. </param>
		/// <param name="imageIndex">The zero-based index of the image within the <see cref="T:System.Windows.Forms.ImageList" /> associated with the <see cref="T:System.Windows.Forms.ListView" /> that contains the item. </param>
		// Token: 0x06002AEA RID: 10986 RVA: 0x000C97B0 File Offset: 0x000C79B0
		public ListViewItem(ListViewItem.ListViewSubItem[] subItems, int imageIndex) : this()
		{
			this.ImageIndexer.Index = imageIndex;
			this.subItems = subItems;
			this.SubItemCount = this.subItems.Length;
			for (int i = 0; i < subItems.Length; i++)
			{
				subItems[i].owner = this;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class and assigns it to the specified group.</summary>
		/// <param name="group">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to assign the item to. </param>
		// Token: 0x06002AEB RID: 10987 RVA: 0x000C97FB File Offset: 0x000C79FB
		public ListViewItem(ListViewGroup group) : this()
		{
			this.Group = group;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the specified item text and assigns it to the specified group.</summary>
		/// <param name="text">The text to display for the item. This should not exceed 259 characters.</param>
		/// <param name="group">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to assign the item to. </param>
		// Token: 0x06002AEC RID: 10988 RVA: 0x000C980A File Offset: 0x000C7A0A
		public ListViewItem(string text, ListViewGroup group) : this(text)
		{
			this.Group = group;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the specified item text and the image index position of the item's icon, and assigns the item to the specified group.</summary>
		/// <param name="text">The text to display for the item. This should not exceed 259 characters.</param>
		/// <param name="imageIndex">The zero-based index of the image within the <see cref="T:System.Windows.Forms.ImageList" /> associated with the <see cref="T:System.Windows.Forms.ListView" /> that contains the item. </param>
		/// <param name="group">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to assign the item to. </param>
		// Token: 0x06002AED RID: 10989 RVA: 0x000C981A File Offset: 0x000C7A1A
		public ListViewItem(string text, int imageIndex, ListViewGroup group) : this(text, imageIndex)
		{
			this.Group = group;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with an array of strings representing subitems, and assigns the item to the specified group.</summary>
		/// <param name="items">An array of strings that represent the subitems of the new item. </param>
		/// <param name="group">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to assign the item to. </param>
		// Token: 0x06002AEE RID: 10990 RVA: 0x000C982B File Offset: 0x000C7A2B
		public ListViewItem(string[] items, ListViewGroup group) : this(items)
		{
			this.Group = group;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the image index position of the item's icon and an array of strings representing subitems, and assigns the item to the specified group.</summary>
		/// <param name="items">An array of strings that represent the subitems of the new item. </param>
		/// <param name="imageIndex">The zero-based index of the image within the <see cref="T:System.Windows.Forms.ImageList" /> associated with the <see cref="T:System.Windows.Forms.ListView" /> that contains the item. </param>
		/// <param name="group">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to assign the item to. </param>
		// Token: 0x06002AEF RID: 10991 RVA: 0x000C983B File Offset: 0x000C7A3B
		public ListViewItem(string[] items, int imageIndex, ListViewGroup group) : this(items, imageIndex)
		{
			this.Group = group;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the image index position of the item's icon; the foreground color, background color, and font of the item; and an array of strings representing subitems. Assigns the item to the specified group.</summary>
		/// <param name="items">An array of strings that represent the subitems of the new item. </param>
		/// <param name="imageIndex">The zero-based index of the image within the <see cref="T:System.Windows.Forms.ImageList" /> associated with the <see cref="T:System.Windows.Forms.ListView" /> that contains the item. </param>
		/// <param name="foreColor">A <see cref="T:System.Drawing.Color" /> that represents the foreground color of the item. </param>
		/// <param name="backColor">A <see cref="T:System.Drawing.Color" /> that represents the background color of the item. </param>
		/// <param name="font">A <see cref="T:System.Drawing.Font" /> that represents the font to display the item's text in. </param>
		/// <param name="group">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to assign the item to. </param>
		// Token: 0x06002AF0 RID: 10992 RVA: 0x000C984C File Offset: 0x000C7A4C
		public ListViewItem(string[] items, int imageIndex, Color foreColor, Color backColor, Font font, ListViewGroup group) : this(items, imageIndex, foreColor, backColor, font)
		{
			this.Group = group;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the image index position of the item's icon and an array of <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> objects, and assigns the item to the specified group.</summary>
		/// <param name="subItems">An array of type <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> that represents the subitems of the item. </param>
		/// <param name="imageIndex">The zero-based index of the image within the <see cref="T:System.Windows.Forms.ImageList" /> associated with the <see cref="T:System.Windows.Forms.ListView" /> that contains the item. </param>
		/// <param name="group">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to assign the item to. </param>
		// Token: 0x06002AF1 RID: 10993 RVA: 0x000C9863 File Offset: 0x000C7A63
		public ListViewItem(ListViewItem.ListViewSubItem[] subItems, int imageIndex, ListViewGroup group) : this(subItems, imageIndex)
		{
			this.Group = group;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the specified text and image.</summary>
		/// <param name="text">The text to display for the item. This should not exceed 259 characters.</param>
		/// <param name="imageKey">The name of the image within the <see cref="P:System.Windows.Forms.ListViewItem.ImageList" /> of the owning <see cref="T:System.Windows.Forms.ListView" /> to display in the <see cref="T:System.Windows.Forms.ListViewItem" />.</param>
		// Token: 0x06002AF2 RID: 10994 RVA: 0x000C9874 File Offset: 0x000C7A74
		public ListViewItem(string text, string imageKey) : this()
		{
			this.ImageIndexer.Key = imageKey;
			this.Text = text;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the specified item and subitem text and image.</summary>
		/// <param name="items">An array containing the text of the subitems of the <see cref="T:System.Windows.Forms.ListViewItem" />.</param>
		/// <param name="imageKey">The name of the image within the <see cref="P:System.Windows.Forms.ListViewItem.ImageList" /> of the owning <see cref="T:System.Windows.Forms.ListView" /> to display in the <see cref="T:System.Windows.Forms.ListViewItem" />.</param>
		// Token: 0x06002AF3 RID: 10995 RVA: 0x000C9890 File Offset: 0x000C7A90
		public ListViewItem(string[] items, string imageKey) : this()
		{
			this.ImageIndexer.Key = imageKey;
			if (items != null && items.Length != 0)
			{
				this.subItems = new ListViewItem.ListViewSubItem[items.Length];
				for (int i = 0; i < items.Length; i++)
				{
					this.subItems[i] = new ListViewItem.ListViewSubItem(this, items[i]);
				}
				this.SubItemCount = items.Length;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the subitems containing the specified text, image, colors, and font.</summary>
		/// <param name="items">An array of strings that represent the text of the subitems for the <see cref="T:System.Windows.Forms.ListViewItem" />.</param>
		/// <param name="imageKey">The name of the image within the <see cref="P:System.Windows.Forms.ListViewItem.ImageList" /> of the owning <see cref="T:System.Windows.Forms.ListView" /> to display in the item.</param>
		/// <param name="foreColor">A <see cref="T:System.Drawing.Color" /> that represents the foreground color of the item.</param>
		/// <param name="backColor">A <see cref="T:System.Drawing.Color" /> that represents the background color of the item.</param>
		/// <param name="font">A <see cref="T:System.Drawing.Font" /> to apply to the item text.</param>
		// Token: 0x06002AF4 RID: 10996 RVA: 0x000C98EC File Offset: 0x000C7AEC
		public ListViewItem(string[] items, string imageKey, Color foreColor, Color backColor, Font font) : this(items, imageKey)
		{
			this.ForeColor = foreColor;
			this.BackColor = backColor;
			this.Font = font;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the specified subitems and image.</summary>
		/// <param name="subItems">An array of <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> objects.</param>
		/// <param name="imageKey">The name of the image within the <see cref="P:System.Windows.Forms.ListViewItem.ImageList" /> of the owning <see cref="T:System.Windows.Forms.ListView" /> to display in the <see cref="T:System.Windows.Forms.ListViewItem" />.</param>
		// Token: 0x06002AF5 RID: 10997 RVA: 0x000C9910 File Offset: 0x000C7B10
		public ListViewItem(ListViewItem.ListViewSubItem[] subItems, string imageKey) : this()
		{
			this.ImageIndexer.Key = imageKey;
			this.subItems = subItems;
			this.SubItemCount = this.subItems.Length;
			for (int i = 0; i < subItems.Length; i++)
			{
				subItems[i].owner = this;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the specified text, image, and group.</summary>
		/// <param name="text">The text to display for the item. This should not exceed 259 characters.</param>
		/// <param name="imageKey">The name of the image within the <see cref="P:System.Windows.Forms.ListViewItem.ImageList" /> of the owning <see cref="T:System.Windows.Forms.ListView" /> to display in the item.</param>
		/// <param name="group">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to assign the item to.</param>
		// Token: 0x06002AF6 RID: 10998 RVA: 0x000C995B File Offset: 0x000C7B5B
		public ListViewItem(string text, string imageKey, ListViewGroup group) : this(text, imageKey)
		{
			this.Group = group;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with subitems containing the specified text, image, and group.</summary>
		/// <param name="items">An array of strings that represents the text for subitems of the <see cref="T:System.Windows.Forms.ListViewItem" />.</param>
		/// <param name="imageKey">The name of the image within the <see cref="P:System.Windows.Forms.ListViewItem.ImageList" /> of the owning <see cref="T:System.Windows.Forms.ListView" /> to display in the item.</param>
		/// <param name="group">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to assign the item to.</param>
		// Token: 0x06002AF7 RID: 10999 RVA: 0x000C996C File Offset: 0x000C7B6C
		public ListViewItem(string[] items, string imageKey, ListViewGroup group) : this(items, imageKey)
		{
			this.Group = group;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the subitems containing the specified text, image, colors, font, and group.</summary>
		/// <param name="items">An array of strings that represents the text of the subitems for the <see cref="T:System.Windows.Forms.ListViewItem" />.</param>
		/// <param name="imageKey">The name of the image within the <see cref="P:System.Windows.Forms.ListViewItem.ImageList" /> of the owning <see cref="T:System.Windows.Forms.ListView" /> to display in the item.</param>
		/// <param name="foreColor">A <see cref="T:System.Drawing.Color" /> that represents the foreground color of the item.</param>
		/// <param name="backColor">A <see cref="T:System.Drawing.Color" /> that represents the background color of the item.</param>
		/// <param name="font">A <see cref="T:System.Drawing.Font" /> to apply to the item text.</param>
		/// <param name="group">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to assign the item to.</param>
		// Token: 0x06002AF8 RID: 11000 RVA: 0x000C997D File Offset: 0x000C7B7D
		public ListViewItem(string[] items, string imageKey, Color foreColor, Color backColor, Font font, ListViewGroup group) : this(items, imageKey, foreColor, backColor, font)
		{
			this.Group = group;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem" /> class with the specified subitems, image, and group.</summary>
		/// <param name="subItems">An array of <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> objects that represent the subitems of the <see cref="T:System.Windows.Forms.ListViewItem" />.</param>
		/// <param name="imageKey">The name of the image within the <see cref="P:System.Windows.Forms.ListViewItem.ImageList" /> of the owning <see cref="T:System.Windows.Forms.ListView" /> to display in the item.</param>
		/// <param name="group">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to assign the item to.</param>
		// Token: 0x06002AF9 RID: 11001 RVA: 0x000C9994 File Offset: 0x000C7B94
		public ListViewItem(ListViewItem.ListViewSubItem[] subItems, string imageKey, ListViewGroup group) : this(subItems, imageKey)
		{
			this.Group = group;
		}

		/// <summary>Gets or sets the background color of the item's text.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the item's text.</returns>
		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06002AFA RID: 11002 RVA: 0x000C99A5 File Offset: 0x000C7BA5
		// (set) Token: 0x06002AFB RID: 11003 RVA: 0x000C99D6 File Offset: 0x000C7BD6
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatAppearance")]
		public Color BackColor
		{
			get
			{
				if (this.SubItemCount != 0)
				{
					return this.subItems[0].BackColor;
				}
				if (this.listView != null)
				{
					return this.listView.BackColor;
				}
				return SystemColors.Window;
			}
			set
			{
				this.SubItems[0].BackColor = value;
			}
		}

		/// <summary>Gets the bounding rectangle of the item, including subitems.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounding rectangle of the item.</returns>
		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06002AFC RID: 11004 RVA: 0x000C99EC File Offset: 0x000C7BEC
		[Browsable(false)]
		public Rectangle Bounds
		{
			get
			{
				if (this.listView != null)
				{
					return this.listView.GetItemRect(this.Index);
				}
				return default(Rectangle);
			}
		}

		/// <summary>Gets or sets a value indicating whether the item is checked.</summary>
		/// <returns>
		///     <see langword="true" /> if the item is checked; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06002AFD RID: 11005 RVA: 0x000C9A1C File Offset: 0x000C7C1C
		// (set) Token: 0x06002AFE RID: 11006 RVA: 0x000C9A28 File Offset: 0x000C7C28
		[DefaultValue(false)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatAppearance")]
		public bool Checked
		{
			get
			{
				return this.StateImageIndex > 0;
			}
			set
			{
				if (this.Checked != value)
				{
					if (this.listView != null && this.listView.IsHandleCreated)
					{
						this.StateImageIndex = (value ? 1 : 0);
						if (this.listView != null && !this.listView.UseCompatibleStateImageBehavior && !this.listView.CheckBoxes)
						{
							this.listView.UpdateSavedCheckedItems(this, value);
							return;
						}
					}
					else
					{
						this.SavedStateImageIndex = (value ? 1 : 0);
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the item has focus within the <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
		/// <returns>
		///     <see langword="true" /> if the item has focus; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06002AFF RID: 11007 RVA: 0x000C9A9D File Offset: 0x000C7C9D
		// (set) Token: 0x06002B00 RID: 11008 RVA: 0x000C9ACB File Offset: 0x000C7CCB
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool Focused
		{
			get
			{
				return this.listView != null && this.listView.IsHandleCreated && this.listView.GetItemState(this.Index, 1) != 0;
			}
			set
			{
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.listView.SetItemState(this.Index, value ? 1 : 0, 1);
				}
			}
		}

		/// <summary>Gets or sets the font of the text displayed by the item.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont" /> property if the <see cref="T:System.Windows.Forms.ListViewItem" /> is not associated with a <see cref="T:System.Windows.Forms.ListView" /> control; otherwise, the font specified in the <see cref="P:System.Windows.Forms.Control.Font" /> property for the <see cref="T:System.Windows.Forms.ListView" /> control is used.</returns>
		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06002B01 RID: 11009 RVA: 0x000C9AFB File Offset: 0x000C7CFB
		// (set) Token: 0x06002B02 RID: 11010 RVA: 0x000C9B2C File Offset: 0x000C7D2C
		[Localizable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatAppearance")]
		public Font Font
		{
			get
			{
				if (this.SubItemCount != 0)
				{
					return this.subItems[0].Font;
				}
				if (this.listView != null)
				{
					return this.listView.Font;
				}
				return Control.DefaultFont;
			}
			set
			{
				this.SubItems[0].Font = value;
			}
		}

		/// <summary>Gets or sets the foreground color of the item's text.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the foreground color of the item's text.</returns>
		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06002B03 RID: 11011 RVA: 0x000C9B40 File Offset: 0x000C7D40
		// (set) Token: 0x06002B04 RID: 11012 RVA: 0x000C9B71 File Offset: 0x000C7D71
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatAppearance")]
		public Color ForeColor
		{
			get
			{
				if (this.SubItemCount != 0)
				{
					return this.subItems[0].ForeColor;
				}
				if (this.listView != null)
				{
					return this.listView.ForeColor;
				}
				return SystemColors.WindowText;
			}
			set
			{
				this.SubItems[0].ForeColor = value;
			}
		}

		/// <summary>Gets or sets the group to which the item is assigned.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ListViewGroup" /> to which the item is assigned.</returns>
		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06002B05 RID: 11013 RVA: 0x000C9B85 File Offset: 0x000C7D85
		// (set) Token: 0x06002B06 RID: 11014 RVA: 0x000C9B8D File Offset: 0x000C7D8D
		[DefaultValue(null)]
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		public ListViewGroup Group
		{
			get
			{
				return this.group;
			}
			set
			{
				if (this.group != value)
				{
					if (value != null)
					{
						value.Items.Add(this);
					}
					else
					{
						this.group.Items.Remove(this);
					}
				}
				this.groupName = null;
			}
		}

		/// <summary>Gets or sets the index of the image that is displayed for the item.</summary>
		/// <returns>The zero-based index of the image in the <see cref="T:System.Windows.Forms.ImageList" /> that is displayed for the item. The default is -1.</returns>
		/// <exception cref="T:System.ArgumentException">The value specified is less than -1. </exception>
		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06002B07 RID: 11015 RVA: 0x000C9BC4 File Offset: 0x000C7DC4
		// (set) Token: 0x06002B08 RID: 11016 RVA: 0x000C9C24 File Offset: 0x000C7E24
		[DefaultValue(-1)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewItemImageIndexDescr")]
		[TypeConverter(typeof(NoneExcludedImageIndexConverter))]
		public int ImageIndex
		{
			get
			{
				if (this.ImageIndexer.Index != -1 && this.ImageList != null && this.ImageIndexer.Index >= this.ImageList.Images.Count)
				{
					return this.ImageList.Images.Count - 1;
				}
				return this.ImageIndexer.Index;
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
				this.ImageIndexer.Index = value;
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.listView.SetItemImage(this.Index, this.ImageIndexer.ActualIndex);
				}
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06002B09 RID: 11017 RVA: 0x000C9CB5 File Offset: 0x000C7EB5
		internal ListViewItem.ListViewItemImageIndexer ImageIndexer
		{
			get
			{
				if (this.imageIndexer == null)
				{
					this.imageIndexer = new ListViewItem.ListViewItemImageIndexer(this);
				}
				return this.imageIndexer;
			}
		}

		/// <summary>Gets or sets the key for the image that is displayed for the item.</summary>
		/// <returns>The key for the image that is displayed for the <see cref="T:System.Windows.Forms.ListViewItem" />.</returns>
		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06002B0A RID: 11018 RVA: 0x000C9CD1 File Offset: 0x000C7ED1
		// (set) Token: 0x06002B0B RID: 11019 RVA: 0x000C9CDE File Offset: 0x000C7EDE
		[DefaultValue("")]
		[TypeConverter(typeof(ImageKeyConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		public string ImageKey
		{
			get
			{
				return this.ImageIndexer.Key;
			}
			set
			{
				this.ImageIndexer.Key = value;
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.listView.SetItemImage(this.Index, this.ImageIndexer.ActualIndex);
				}
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ImageList" /> that contains the image displayed with the item.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ImageList" /> used by the <see cref="T:System.Windows.Forms.ListView" /> control that contains the image displayed with the item.</returns>
		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06002B0C RID: 11020 RVA: 0x000C9D20 File Offset: 0x000C7F20
		[Browsable(false)]
		public ImageList ImageList
		{
			get
			{
				if (this.listView != null)
				{
					switch (this.listView.View)
					{
					case View.LargeIcon:
					case View.Tile:
						return this.listView.LargeImageList;
					case View.Details:
					case View.SmallIcon:
					case View.List:
						return this.listView.SmallImageList;
					}
				}
				return null;
			}
		}

		/// <summary>Gets or sets the number of small image widths by which to indent the <see cref="T:System.Windows.Forms.ListViewItem" />.</summary>
		/// <returns>The number of small image widths by which to indent the <see cref="T:System.Windows.Forms.ListViewItem" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">When setting <see cref="P:System.Windows.Forms.ListViewItem.IndentCount" />, the number specified is less than 0.</exception>
		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06002B0D RID: 11021 RVA: 0x000C9D76 File Offset: 0x000C7F76
		// (set) Token: 0x06002B0E RID: 11022 RVA: 0x000C9D80 File Offset: 0x000C7F80
		[DefaultValue(0)]
		[SRDescription("ListViewItemIndentCountDescr")]
		[SRCategory("CatDisplay")]
		public int IndentCount
		{
			get
			{
				return this.indentCount;
			}
			set
			{
				if (value == this.indentCount)
				{
					return;
				}
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("IndentCount", SR.GetString("ListViewIndentCountCantBeNegative"));
				}
				this.indentCount = value;
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.listView.SetItemIndentCount(this.Index, this.indentCount);
				}
			}
		}

		/// <summary>Gets the zero-based index of the item within the <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
		/// <returns>The zero-based index of the item within the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> of the <see cref="T:System.Windows.Forms.ListView" /> control, or -1 if the item is not associated with a <see cref="T:System.Windows.Forms.ListView" /> control.</returns>
		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06002B0F RID: 11023 RVA: 0x000C9DE3 File Offset: 0x000C7FE3
		[Browsable(false)]
		public int Index
		{
			get
			{
				if (this.listView != null)
				{
					if (!this.listView.VirtualMode)
					{
						this.lastIndex = this.listView.GetDisplayIndex(this, this.lastIndex);
					}
					return this.lastIndex;
				}
				return -1;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ListView" /> control that contains the item.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListView" /> that contains the <see cref="T:System.Windows.Forms.ListViewItem" />.</returns>
		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06002B10 RID: 11024 RVA: 0x000C9E1A File Offset: 0x000C801A
		[Browsable(false)]
		public ListView ListView
		{
			get
			{
				return this.listView;
			}
		}

		/// <summary>Gets or sets the name associated with this <see cref="T:System.Windows.Forms.ListViewItem" />. </summary>
		/// <returns>The name of the <see cref="T:System.Windows.Forms.ListViewItem" />. The default is an empty string ("").</returns>
		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06002B11 RID: 11025 RVA: 0x000C9E22 File Offset: 0x000C8022
		// (set) Token: 0x06002B12 RID: 11026 RVA: 0x000C9E3F File Offset: 0x000C803F
		[Localizable(true)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Name
		{
			get
			{
				if (this.SubItemCount == 0)
				{
					return string.Empty;
				}
				return this.subItems[0].Name;
			}
			set
			{
				this.SubItems[0].Name = value;
			}
		}

		/// <summary>Gets or sets the position of the upper-left corner of the <see cref="T:System.Windows.Forms.ListViewItem" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Point" /> at the upper-left corner of the <see cref="T:System.Windows.Forms.ListViewItem" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Forms.ListViewItem.Position" /> is set when the containing <see cref="T:System.Windows.Forms.ListView" /> is in virtual mode.</exception>
		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06002B13 RID: 11027 RVA: 0x000C9E53 File Offset: 0x000C8053
		// (set) Token: 0x06002B14 RID: 11028 RVA: 0x000C9E88 File Offset: 0x000C8088
		[SRCategory("CatDisplay")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public Point Position
		{
			get
			{
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.position = this.listView.GetItemPosition(this.Index);
				}
				return this.position;
			}
			set
			{
				if (value.Equals(this.position))
				{
					return;
				}
				this.position = value;
				if (this.listView != null && this.listView.IsHandleCreated && !this.listView.VirtualMode)
				{
					this.listView.SetItemPosition(this.Index, this.position.X, this.position.Y);
				}
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06002B15 RID: 11029 RVA: 0x000C9F00 File Offset: 0x000C8100
		internal int RawStateImageIndex
		{
			get
			{
				return this.SavedStateImageIndex + 1 << 12;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06002B16 RID: 11030 RVA: 0x000C9F0D File Offset: 0x000C810D
		// (set) Token: 0x06002B17 RID: 11031 RVA: 0x000C9F21 File Offset: 0x000C8121
		private int SavedStateImageIndex
		{
			get
			{
				return this.state[ListViewItem.SavedStateImageIndexSection] - 1;
			}
			set
			{
				this.state[ListViewItem.StateImageMaskSet] = ((value == -1) ? 0 : 1);
				this.state[ListViewItem.SavedStateImageIndexSection] = value + 1;
			}
		}

		/// <summary>Gets or sets a value indicating whether the item is selected.</summary>
		/// <returns>
		///     <see langword="true" /> if the item is selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06002B18 RID: 11032 RVA: 0x000C9F4E File Offset: 0x000C814E
		// (set) Token: 0x06002B19 RID: 11033 RVA: 0x000C9F84 File Offset: 0x000C8184
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Selected
		{
			get
			{
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					return this.listView.GetItemState(this.Index, 2) != 0;
				}
				return this.StateSelected;
			}
			set
			{
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.listView.SetItemState(this.Index, value ? 2 : 0, 2);
					this.listView.SetSelectionMark(this.Index);
					return;
				}
				this.StateSelected = value;
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.listView.CacheSelectedStateForItem(this, value);
				}
			}
		}

		/// <summary>Gets or sets the index of the state image (an image such as a selected or cleared check box that indicates the state of the item) that is displayed for the item.</summary>
		/// <returns>The zero-based index of the state image in the <see cref="T:System.Windows.Forms.ImageList" /> that is displayed for the item.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for this property is less than -1.-or- The value specified for this property is greater than 14. </exception>
		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06002B1A RID: 11034 RVA: 0x000C9FFC File Offset: 0x000C81FC
		// (set) Token: 0x06002B1B RID: 11035 RVA: 0x000CA044 File Offset: 0x000C8244
		[Localizable(true)]
		[TypeConverter(typeof(NoneExcludedImageIndexConverter))]
		[DefaultValue(-1)]
		[SRDescription("ListViewItemStateImageIndexDescr")]
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RelatedImageList("ListView.StateImageList")]
		public int StateImageIndex
		{
			get
			{
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					int itemState = this.listView.GetItemState(this.Index, 61440);
					return (itemState >> 12) - 1;
				}
				return this.SavedStateImageIndex;
			}
			set
			{
				if (value < -1 || value > 14)
				{
					throw new ArgumentOutOfRangeException("StateImageIndex", SR.GetString("InvalidArgument", new object[]
					{
						"StateImageIndex",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.state[ListViewItem.StateImageMaskSet] = ((value == -1) ? 0 : 1);
					int num = value + 1 << 12;
					this.listView.SetItemState(this.Index, num, 61440);
				}
				this.SavedStateImageIndex = value;
			}
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06002B1C RID: 11036 RVA: 0x000CA0DE File Offset: 0x000C82DE
		internal bool StateImageSet
		{
			get
			{
				return this.state[ListViewItem.StateImageMaskSet] != 0;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06002B1D RID: 11037 RVA: 0x000CA0F3 File Offset: 0x000C82F3
		// (set) Token: 0x06002B1E RID: 11038 RVA: 0x000CA108 File Offset: 0x000C8308
		internal bool StateSelected
		{
			get
			{
				return this.state[ListViewItem.StateSelectedSection] == 1;
			}
			set
			{
				this.state[ListViewItem.StateSelectedSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06002B1F RID: 11039 RVA: 0x000CA121 File Offset: 0x000C8321
		// (set) Token: 0x06002B20 RID: 11040 RVA: 0x000CA133 File Offset: 0x000C8333
		private int SubItemCount
		{
			get
			{
				return this.state[ListViewItem.SubItemCountSection];
			}
			set
			{
				this.state[ListViewItem.SubItemCountSection] = value;
			}
		}

		/// <summary>Gets a collection containing all subitems of the item.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItemCollection" /> that contains the subitems.</returns>
		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06002B21 RID: 11041 RVA: 0x000CA148 File Offset: 0x000C8348
		[SRCategory("CatData")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListViewItemSubItemsDescr")]
		[Editor("System.Windows.Forms.Design.ListViewSubItemCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public ListViewItem.ListViewSubItemCollection SubItems
		{
			get
			{
				if (this.SubItemCount == 0)
				{
					this.subItems = new ListViewItem.ListViewSubItem[1];
					this.subItems[0] = new ListViewItem.ListViewSubItem(this, string.Empty);
					this.SubItemCount = 1;
				}
				if (this.listViewSubItemCollection == null)
				{
					this.listViewSubItemCollection = new ListViewItem.ListViewSubItemCollection(this);
				}
				return this.listViewSubItemCollection;
			}
		}

		/// <summary>Gets or sets an object that contains data to associate with the item.</summary>
		/// <returns>An object that contains information that is associated with the item.</returns>
		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06002B22 RID: 11042 RVA: 0x000CA19D File Offset: 0x000C839D
		// (set) Token: 0x06002B23 RID: 11043 RVA: 0x000CA1A5 File Offset: 0x000C83A5
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

		/// <summary>Gets or sets the text of the item.</summary>
		/// <returns>The text to display for the item. This should not exceed 259 characters.</returns>
		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06002B24 RID: 11044 RVA: 0x000CA1AE File Offset: 0x000C83AE
		// (set) Token: 0x06002B25 RID: 11045 RVA: 0x000CA1CB File Offset: 0x000C83CB
		[Localizable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatAppearance")]
		public string Text
		{
			get
			{
				if (this.SubItemCount == 0)
				{
					return string.Empty;
				}
				return this.subItems[0].Text;
			}
			set
			{
				this.SubItems[0].Text = value;
			}
		}

		/// <summary>Gets or sets the text shown when the mouse pointer rests on the <see cref="T:System.Windows.Forms.ListViewItem" />.</summary>
		/// <returns>The text shown when the mouse pointer rests on the <see cref="T:System.Windows.Forms.ListViewItem" />.</returns>
		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06002B26 RID: 11046 RVA: 0x000CA1DF File Offset: 0x000C83DF
		// (set) Token: 0x06002B27 RID: 11047 RVA: 0x000CA1E8 File Offset: 0x000C83E8
		[SRCategory("CatAppearance")]
		[DefaultValue("")]
		public string ToolTipText
		{
			get
			{
				return this.toolTipText;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (WindowsFormsUtils.SafeCompareStrings(this.toolTipText, value, false))
				{
					return;
				}
				this.toolTipText = value;
				if (this.listView != null && this.listView.IsHandleCreated)
				{
					this.listView.ListViewItemToolTipChanged(this);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="P:System.Windows.Forms.ListViewItem.Font" />, <see cref="P:System.Windows.Forms.ListViewItem.ForeColor" />, and <see cref="P:System.Windows.Forms.ListViewItem.BackColor" /> properties for the item are used for all its subitems.</summary>
		/// <returns>
		///     <see langword="true" /> if all subitems use the font, foreground color, and background color settings of the item; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06002B28 RID: 11048 RVA: 0x000CA237 File Offset: 0x000C8437
		// (set) Token: 0x06002B29 RID: 11049 RVA: 0x000CA24C File Offset: 0x000C844C
		[DefaultValue(true)]
		[SRCategory("CatAppearance")]
		public bool UseItemStyleForSubItems
		{
			get
			{
				return this.state[ListViewItem.StateWholeRowOneStyleSection] == 1;
			}
			set
			{
				this.state[ListViewItem.StateWholeRowOneStyleSection] = (value ? 1 : 0);
			}
		}

		/// <summary>Places the item text into edit mode.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Forms.ListView.LabelEdit" /> property of the associated <see cref="T:System.Windows.Forms.ListView" /> is not set to <see langword="true" />. </exception>
		// Token: 0x06002B2A RID: 11050 RVA: 0x000CA268 File Offset: 0x000C8468
		public void BeginEdit()
		{
			if (this.Index >= 0)
			{
				ListView listView = this.ListView;
				if (!listView.LabelEdit)
				{
					throw new InvalidOperationException(SR.GetString("ListViewBeginEditFailed"));
				}
				if (!listView.Focused)
				{
					listView.FocusInternal();
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(listView, listView.Handle), NativeMethods.LVM_EDITLABEL, this.Index, 0);
			}
		}

		/// <summary>Creates an identical copy of the item.</summary>
		/// <returns>An object that represents an item that has the same text, image, and subitems associated with it as the cloned item.</returns>
		// Token: 0x06002B2B RID: 11051 RVA: 0x000CA2CC File Offset: 0x000C84CC
		public virtual object Clone()
		{
			ListViewItem.ListViewSubItem[] array = new ListViewItem.ListViewSubItem[this.SubItems.Count];
			for (int i = 0; i < this.SubItems.Count; i++)
			{
				ListViewItem.ListViewSubItem listViewSubItem = this.SubItems[i];
				array[i] = new ListViewItem.ListViewSubItem(null, listViewSubItem.Text, listViewSubItem.ForeColor, listViewSubItem.BackColor, listViewSubItem.Font);
				array[i].Tag = listViewSubItem.Tag;
			}
			Type type = base.GetType();
			ListViewItem listViewItem;
			if (type == typeof(ListViewItem))
			{
				listViewItem = new ListViewItem(array, this.ImageIndexer.Index);
			}
			else
			{
				listViewItem = (ListViewItem)Activator.CreateInstance(type);
			}
			listViewItem.subItems = array;
			listViewItem.ImageIndexer.Index = this.ImageIndexer.Index;
			listViewItem.SubItemCount = this.SubItemCount;
			listViewItem.Checked = this.Checked;
			listViewItem.UseItemStyleForSubItems = this.UseItemStyleForSubItems;
			listViewItem.Tag = this.Tag;
			if (!string.IsNullOrEmpty(this.ImageIndexer.Key))
			{
				listViewItem.ImageIndexer.Key = this.ImageIndexer.Key;
			}
			listViewItem.indentCount = this.indentCount;
			listViewItem.StateImageIndex = this.StateImageIndex;
			listViewItem.toolTipText = this.toolTipText;
			listViewItem.BackColor = this.BackColor;
			listViewItem.ForeColor = this.ForeColor;
			listViewItem.Font = this.Font;
			listViewItem.Text = this.Text;
			listViewItem.Group = this.Group;
			return listViewItem;
		}

		/// <summary>Ensures that the item is visible within the control, scrolling the contents of the control, if necessary.</summary>
		// Token: 0x06002B2C RID: 11052 RVA: 0x000CA453 File Offset: 0x000C8653
		public virtual void EnsureVisible()
		{
			if (this.listView != null && this.listView.IsHandleCreated)
			{
				this.listView.EnsureVisible(this.Index);
			}
		}

		/// <summary>Finds the next item from the <see cref="T:System.Windows.Forms.ListViewItem" />, searching in the specified direction.</summary>
		/// <param name="searchDirection">One of the <see cref="T:System.Windows.Forms.SearchDirectionHint" /> values.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> that is closest to the given coordinates, searching in the specified direction.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Forms.ListView.View" /> property of the containing <see cref="T:System.Windows.Forms.ListView" /> is set to a value other than <see cref="F:System.Windows.Forms.View.SmallIcon" /> or <see cref="F:System.Windows.Forms.View.LargeIcon" />. </exception>
		// Token: 0x06002B2D RID: 11053 RVA: 0x000CA47C File Offset: 0x000C867C
		public ListViewItem FindNearestItem(SearchDirectionHint searchDirection)
		{
			Rectangle bounds = this.Bounds;
			switch (searchDirection)
			{
			case SearchDirectionHint.Left:
				return this.ListView.FindNearestItem(searchDirection, bounds.Left, bounds.Top);
			case SearchDirectionHint.Up:
				return this.ListView.FindNearestItem(searchDirection, bounds.Left, bounds.Top);
			case SearchDirectionHint.Right:
				return this.ListView.FindNearestItem(searchDirection, bounds.Right, bounds.Top);
			case SearchDirectionHint.Down:
				return this.ListView.FindNearestItem(searchDirection, bounds.Left, bounds.Bottom);
			default:
				return null;
			}
		}

		/// <summary>Retrieves the specified portion of the bounding rectangle for the item.</summary>
		/// <param name="portion">One of the <see cref="T:System.Windows.Forms.ItemBoundsPortion" /> values that represents a portion of the item for which to retrieve the bounding rectangle. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounding rectangle for the specified portion of the item.</returns>
		// Token: 0x06002B2E RID: 11054 RVA: 0x000CA518 File Offset: 0x000C8718
		public Rectangle GetBounds(ItemBoundsPortion portion)
		{
			if (this.listView != null && this.listView.IsHandleCreated)
			{
				return this.listView.GetItemRect(this.Index, portion);
			}
			return default(Rectangle);
		}

		/// <summary>Returns the subitem of the <see cref="T:System.Windows.Forms.ListViewItem" /> at the specified coordinates.</summary>
		/// <param name="x">The x-coordinate. </param>
		/// <param name="y">The y-coordinate.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> at the specified x- and y-coordinates.</returns>
		// Token: 0x06002B2F RID: 11055 RVA: 0x000CA558 File Offset: 0x000C8758
		public ListViewItem.ListViewSubItem GetSubItemAt(int x, int y)
		{
			if (this.listView == null || !this.listView.IsHandleCreated || this.listView.View != View.Details)
			{
				return null;
			}
			int num = -1;
			int num2 = -1;
			this.listView.GetSubItemAt(x, y, out num, out num2);
			if (num == this.Index && num2 != -1 && num2 < this.SubItems.Count)
			{
				return this.SubItems[num2];
			}
			return null;
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x000CA5C8 File Offset: 0x000C87C8
		internal void Host(ListView parent, int ID, int index)
		{
			this.ID = ID;
			this.listView = parent;
			if (index != -1)
			{
				this.UpdateStateToListView(index);
			}
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x000CA5E4 File Offset: 0x000C87E4
		internal void UpdateGroupFromName()
		{
			if (string.IsNullOrEmpty(this.groupName))
			{
				return;
			}
			ListViewGroup listViewGroup = this.listView.Groups[this.groupName];
			this.Group = listViewGroup;
			this.groupName = null;
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x000CA624 File Offset: 0x000C8824
		internal void UpdateStateToListView(int index)
		{
			NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
			this.UpdateStateToListView(index, ref lvitem, true);
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x000CA644 File Offset: 0x000C8844
		internal void UpdateStateToListView(int index, ref NativeMethods.LVITEM lvItem, bool updateOwner)
		{
			if (index == -1)
			{
				index = this.Index;
			}
			else
			{
				this.lastIndex = index;
			}
			int num = 0;
			int num2 = 0;
			if (this.StateSelected)
			{
				num |= 2;
				num2 |= 2;
			}
			if (this.SavedStateImageIndex > -1)
			{
				num |= this.SavedStateImageIndex + 1 << 12;
				num2 |= 61440;
			}
			lvItem.mask |= 8;
			lvItem.iItem = index;
			lvItem.stateMask |= num2;
			lvItem.state |= num;
			if (this.listView.GroupsEnabled)
			{
				lvItem.mask |= 256;
				lvItem.iGroupId = this.listView.GetNativeGroupId(this);
			}
			if (updateOwner)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this.listView, this.listView.Handle), NativeMethods.LVM_SETITEM, 0, ref lvItem);
			}
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x000CA718 File Offset: 0x000C8918
		internal void UpdateStateFromListView(int displayIndex, bool checkSelection)
		{
			if (this.listView != null && this.listView.IsHandleCreated && displayIndex != -1)
			{
				NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
				lvitem.mask = 268;
				if (checkSelection)
				{
					lvitem.stateMask = 2;
				}
				lvitem.stateMask |= 61440;
				if (lvitem.stateMask == 0)
				{
					return;
				}
				lvitem.iItem = displayIndex;
				UnsafeNativeMethods.SendMessage(new HandleRef(this.listView, this.listView.Handle), NativeMethods.LVM_GETITEM, 0, ref lvitem);
				if (checkSelection)
				{
					this.StateSelected = ((lvitem.state & 2) != 0);
				}
				this.SavedStateImageIndex = ((lvitem.state & 61440) >> 12) - 1;
				this.group = null;
				foreach (object obj in this.ListView.Groups)
				{
					ListViewGroup listViewGroup = (ListViewGroup)obj;
					if (listViewGroup.ID == lvitem.iGroupId)
					{
						this.group = listViewGroup;
						break;
					}
				}
			}
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x000CA840 File Offset: 0x000C8A40
		internal void UnHost(bool checkSelection)
		{
			this.UnHost(this.Index, checkSelection);
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x000CA850 File Offset: 0x000C8A50
		internal void UnHost(int displayIndex, bool checkSelection)
		{
			this.UpdateStateFromListView(displayIndex, checkSelection);
			if (this.listView != null && (this.listView.Site == null || !this.listView.Site.DesignMode) && this.group != null)
			{
				this.group.Items.Remove(this);
			}
			this.ID = -1;
			this.listView = null;
		}

		/// <summary>Removes the item from its associated <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
		// Token: 0x06002B37 RID: 11063 RVA: 0x000CA8B3 File Offset: 0x000C8AB3
		public virtual void Remove()
		{
			if (this.listView != null)
			{
				this.listView.Items.Remove(this);
			}
		}

		/// <summary>Deserializes the item.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the data needed to deserialize the item. </param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that represents the source and destination of the stream being deserialized. </param>
		// Token: 0x06002B38 RID: 11064 RVA: 0x000CA8D0 File Offset: 0x000C8AD0
		protected virtual void Deserialize(SerializationInfo info, StreamingContext context)
		{
			bool flag = false;
			string text = null;
			int num = -1;
			foreach (SerializationEntry serializationEntry in info)
			{
				if (serializationEntry.Name == "Text")
				{
					this.Text = info.GetString(serializationEntry.Name);
				}
				else if (serializationEntry.Name == "ImageIndex")
				{
					num = info.GetInt32(serializationEntry.Name);
				}
				else if (serializationEntry.Name == "ImageKey")
				{
					text = info.GetString(serializationEntry.Name);
				}
				else if (serializationEntry.Name == "SubItemCount")
				{
					this.SubItemCount = info.GetInt32(serializationEntry.Name);
					if (this.SubItemCount > 0)
					{
						flag = true;
					}
				}
				else if (serializationEntry.Name == "BackColor")
				{
					this.BackColor = (Color)info.GetValue(serializationEntry.Name, typeof(Color));
				}
				else if (serializationEntry.Name == "Checked")
				{
					this.Checked = info.GetBoolean(serializationEntry.Name);
				}
				else if (serializationEntry.Name == "Font")
				{
					this.Font = (Font)info.GetValue(serializationEntry.Name, typeof(Font));
				}
				else if (serializationEntry.Name == "ForeColor")
				{
					this.ForeColor = (Color)info.GetValue(serializationEntry.Name, typeof(Color));
				}
				else if (serializationEntry.Name == "UseItemStyleForSubItems")
				{
					this.UseItemStyleForSubItems = info.GetBoolean(serializationEntry.Name);
				}
				else if (serializationEntry.Name == "Group")
				{
					ListViewGroup listViewGroup = (ListViewGroup)info.GetValue(serializationEntry.Name, typeof(ListViewGroup));
					this.groupName = listViewGroup.Name;
				}
			}
			if (text != null)
			{
				this.ImageKey = text;
			}
			else if (num != -1)
			{
				this.ImageIndex = num;
			}
			if (flag)
			{
				ListViewItem.ListViewSubItem[] array = new ListViewItem.ListViewSubItem[this.SubItemCount];
				for (int i = 1; i < this.SubItemCount; i++)
				{
					ListViewItem.ListViewSubItem listViewSubItem = (ListViewItem.ListViewSubItem)info.GetValue("SubItem" + i.ToString(CultureInfo.InvariantCulture), typeof(ListViewItem.ListViewSubItem));
					listViewSubItem.owner = this;
					array[i] = listViewSubItem;
				}
				array[0] = this.subItems[0];
				this.subItems = array;
			}
		}

		/// <summary>Serializes the item.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the data needed to serialize the item. </param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that represents the source and destination of the stream being serialized. </param>
		// Token: 0x06002B39 RID: 11065 RVA: 0x000CAB7C File Offset: 0x000C8D7C
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		protected virtual void Serialize(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Text", this.Text);
			info.AddValue("ImageIndex", this.ImageIndexer.Index);
			if (!string.IsNullOrEmpty(this.ImageIndexer.Key))
			{
				info.AddValue("ImageKey", this.ImageIndexer.Key);
			}
			if (this.SubItemCount > 1)
			{
				info.AddValue("SubItemCount", this.SubItemCount);
				for (int i = 1; i < this.SubItemCount; i++)
				{
					info.AddValue("SubItem" + i.ToString(CultureInfo.InvariantCulture), this.subItems[i], typeof(ListViewItem.ListViewSubItem));
				}
			}
			info.AddValue("BackColor", this.BackColor);
			info.AddValue("Checked", this.Checked);
			info.AddValue("Font", this.Font);
			info.AddValue("ForeColor", this.ForeColor);
			info.AddValue("UseItemStyleForSubItems", this.UseItemStyleForSubItems);
			if (this.Group != null)
			{
				info.AddValue("Group", this.Group);
			}
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x000CACA9 File Offset: 0x000C8EA9
		internal void SetItemIndex(ListView listView, int index)
		{
			this.listView = listView;
			this.lastIndex = index;
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal bool ShouldSerializeText()
		{
			return false;
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x000CACB9 File Offset: 0x000C8EB9
		private bool ShouldSerializePosition()
		{
			return !this.position.Equals(new Point(-1, -1));
		}

		/// <summary>Returns a string that represents the current object.</summary>
		/// <returns>A string that represents the current object.</returns>
		// Token: 0x06002B3D RID: 11069 RVA: 0x000CACDB File Offset: 0x000C8EDB
		public override string ToString()
		{
			return "ListViewItem: {" + this.Text + "}";
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x000CACF2 File Offset: 0x000C8EF2
		internal void InvalidateListView()
		{
			if (this.listView != null && this.listView.IsHandleCreated)
			{
				this.listView.Invalidate();
			}
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x000CAD14 File Offset: 0x000C8F14
		internal void UpdateSubItems(int index)
		{
			this.UpdateSubItems(index, this.SubItemCount);
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x000CAD24 File Offset: 0x000C8F24
		internal void UpdateSubItems(int index, int oldCount)
		{
			if (this.listView != null && this.listView.IsHandleCreated)
			{
				int subItemCount = this.SubItemCount;
				int index2 = this.Index;
				if (index != -1)
				{
					this.listView.SetItemText(index2, index, this.subItems[index].Text);
				}
				else
				{
					for (int i = 0; i < subItemCount; i++)
					{
						this.listView.SetItemText(index2, i, this.subItems[i].Text);
					}
				}
				for (int j = subItemCount; j < oldCount; j++)
				{
					this.listView.SetItemText(index2, j, string.Empty);
				}
			}
		}

		/// <summary>Serializes the item.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the data needed to serialize the item.  </param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that represents the source and destination of the stream being serialized.</param>
		// Token: 0x06002B41 RID: 11073 RVA: 0x000CADB8 File Offset: 0x000C8FB8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			this.Serialize(info, context);
		}

		// Token: 0x04001270 RID: 4720
		private const int MAX_SUBITEMS = 4096;

		// Token: 0x04001271 RID: 4721
		private static readonly BitVector32.Section StateSelectedSection = BitVector32.CreateSection(1);

		// Token: 0x04001272 RID: 4722
		private static readonly BitVector32.Section StateImageMaskSet = BitVector32.CreateSection(1, ListViewItem.StateSelectedSection);

		// Token: 0x04001273 RID: 4723
		private static readonly BitVector32.Section StateWholeRowOneStyleSection = BitVector32.CreateSection(1, ListViewItem.StateImageMaskSet);

		// Token: 0x04001274 RID: 4724
		private static readonly BitVector32.Section SavedStateImageIndexSection = BitVector32.CreateSection(15, ListViewItem.StateWholeRowOneStyleSection);

		// Token: 0x04001275 RID: 4725
		private static readonly BitVector32.Section SubItemCountSection = BitVector32.CreateSection(4096, ListViewItem.SavedStateImageIndexSection);

		// Token: 0x04001276 RID: 4726
		private int indentCount;

		// Token: 0x04001277 RID: 4727
		private Point position = new Point(-1, -1);

		// Token: 0x04001278 RID: 4728
		internal ListView listView;

		// Token: 0x04001279 RID: 4729
		internal ListViewGroup group;

		// Token: 0x0400127A RID: 4730
		private string groupName;

		// Token: 0x0400127B RID: 4731
		private ListViewItem.ListViewSubItemCollection listViewSubItemCollection;

		// Token: 0x0400127C RID: 4732
		private ListViewItem.ListViewSubItem[] subItems;

		// Token: 0x0400127D RID: 4733
		private int lastIndex = -1;

		// Token: 0x0400127E RID: 4734
		internal int ID = -1;

		// Token: 0x0400127F RID: 4735
		private BitVector32 state;

		// Token: 0x04001280 RID: 4736
		private ListViewItem.ListViewItemImageIndexer imageIndexer;

		// Token: 0x04001281 RID: 4737
		private string toolTipText = string.Empty;

		// Token: 0x04001282 RID: 4738
		private object userData;

		// Token: 0x02000613 RID: 1555
		internal class ListViewItemImageIndexer : ImageList.Indexer
		{
			// Token: 0x06005D97 RID: 23959 RVA: 0x00184E2D File Offset: 0x0018302D
			public ListViewItemImageIndexer(ListViewItem item)
			{
				this.owner = item;
			}

			// Token: 0x17001672 RID: 5746
			// (get) Token: 0x06005D98 RID: 23960 RVA: 0x00184E3C File Offset: 0x0018303C
			// (set) Token: 0x06005D99 RID: 23961 RVA: 0x0000701A File Offset: 0x0000521A
			public override ImageList ImageList
			{
				get
				{
					if (this.owner != null)
					{
						return this.owner.ImageList;
					}
					return null;
				}
				set
				{
				}
			}

			// Token: 0x04003A0D RID: 14861
			private ListViewItem owner;
		}

		/// <summary>Represents a subitem of a <see cref="T:System.Windows.Forms.ListViewItem" />.</summary>
		// Token: 0x02000614 RID: 1556
		[TypeConverter(typeof(ListViewSubItemConverter))]
		[ToolboxItem(false)]
		[DesignTimeVisible(false)]
		[DefaultProperty("Text")]
		[Serializable]
		public class ListViewSubItem
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> class with default values.</summary>
			// Token: 0x06005D9A RID: 23962 RVA: 0x000027DB File Offset: 0x000009DB
			public ListViewSubItem()
			{
			}

			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> class with the specified owner and text.</summary>
			/// <param name="owner">A <see cref="T:System.Windows.Forms.ListViewItem" /> that represents the item that owns the subitem. </param>
			/// <param name="text">The text to display for the subitem. </param>
			// Token: 0x06005D9B RID: 23963 RVA: 0x00184E53 File Offset: 0x00183053
			public ListViewSubItem(ListViewItem owner, string text)
			{
				this.owner = owner;
				this.text = text;
			}

			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> class with the specified owner, text, foreground color, background color, and font values.</summary>
			/// <param name="owner">A <see cref="T:System.Windows.Forms.ListViewItem" /> that represents the item that owns the subitem. </param>
			/// <param name="text">The text to display for the subitem. </param>
			/// <param name="foreColor">A <see cref="T:System.Drawing.Color" /> that represents the foreground color of the subitem. </param>
			/// <param name="backColor">A <see cref="T:System.Drawing.Color" /> that represents the background color of the subitem. </param>
			/// <param name="font">A <see cref="T:System.Drawing.Font" /> that represents the font to display the subitem's text in. </param>
			// Token: 0x06005D9C RID: 23964 RVA: 0x00184E6C File Offset: 0x0018306C
			public ListViewSubItem(ListViewItem owner, string text, Color foreColor, Color backColor, Font font)
			{
				this.owner = owner;
				this.text = text;
				this.style = new ListViewItem.ListViewSubItem.SubItemStyle();
				this.style.foreColor = foreColor;
				this.style.backColor = backColor;
				this.style.font = font;
			}

			/// <summary>Gets or sets the background color of the subitem's text.</summary>
			/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the subitem's text.</returns>
			// Token: 0x17001673 RID: 5747
			// (get) Token: 0x06005D9D RID: 23965 RVA: 0x00184EC0 File Offset: 0x001830C0
			// (set) Token: 0x06005D9E RID: 23966 RVA: 0x00184F24 File Offset: 0x00183124
			public Color BackColor
			{
				get
				{
					if (this.style != null && this.style.backColor != Color.Empty)
					{
						return this.style.backColor;
					}
					if (this.owner != null && this.owner.listView != null)
					{
						return this.owner.listView.BackColor;
					}
					return SystemColors.Window;
				}
				set
				{
					if (this.style == null)
					{
						this.style = new ListViewItem.ListViewSubItem.SubItemStyle();
					}
					if (this.style.backColor != value)
					{
						this.style.backColor = value;
						if (this.owner != null)
						{
							this.owner.InvalidateListView();
						}
					}
				}
			}

			/// <summary>Gets the bounding rectangle of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" />.</summary>
			/// <returns>The bounding <see cref="T:System.Drawing.Rectangle" /> of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" />.</returns>
			// Token: 0x17001674 RID: 5748
			// (get) Token: 0x06005D9F RID: 23967 RVA: 0x00184F78 File Offset: 0x00183178
			[Browsable(false)]
			public Rectangle Bounds
			{
				get
				{
					if (this.owner != null && this.owner.listView != null && this.owner.listView.IsHandleCreated)
					{
						return this.owner.listView.GetSubItemRect(this.owner.Index, this.owner.SubItems.IndexOf(this));
					}
					return Rectangle.Empty;
				}
			}

			// Token: 0x17001675 RID: 5749
			// (get) Token: 0x06005DA0 RID: 23968 RVA: 0x00184FDE File Offset: 0x001831DE
			internal bool CustomBackColor
			{
				get
				{
					return this.style != null && !this.style.backColor.IsEmpty;
				}
			}

			// Token: 0x17001676 RID: 5750
			// (get) Token: 0x06005DA1 RID: 23969 RVA: 0x00184FFD File Offset: 0x001831FD
			internal bool CustomFont
			{
				get
				{
					return this.style != null && this.style.font != null;
				}
			}

			// Token: 0x17001677 RID: 5751
			// (get) Token: 0x06005DA2 RID: 23970 RVA: 0x00185017 File Offset: 0x00183217
			internal bool CustomForeColor
			{
				get
				{
					return this.style != null && !this.style.foreColor.IsEmpty;
				}
			}

			// Token: 0x17001678 RID: 5752
			// (get) Token: 0x06005DA3 RID: 23971 RVA: 0x00185036 File Offset: 0x00183236
			internal bool CustomStyle
			{
				get
				{
					return this.style != null;
				}
			}

			/// <summary>Gets or sets the font of the text displayed by the subitem.</summary>
			/// <returns>The <see cref="T:System.Drawing.Font" /> to apply to the text displayed by the control.</returns>
			// Token: 0x17001679 RID: 5753
			// (get) Token: 0x06005DA4 RID: 23972 RVA: 0x00185044 File Offset: 0x00183244
			// (set) Token: 0x06005DA5 RID: 23973 RVA: 0x001850A0 File Offset: 0x001832A0
			[Localizable(true)]
			public Font Font
			{
				get
				{
					if (this.style != null && this.style.font != null)
					{
						return this.style.font;
					}
					if (this.owner != null && this.owner.listView != null)
					{
						return this.owner.listView.Font;
					}
					return Control.DefaultFont;
				}
				set
				{
					if (this.style == null)
					{
						this.style = new ListViewItem.ListViewSubItem.SubItemStyle();
					}
					if (this.style.font != value)
					{
						this.style.font = value;
						if (this.owner != null)
						{
							this.owner.InvalidateListView();
						}
					}
				}
			}

			/// <summary>Gets or sets the foreground color of the subitem's text.</summary>
			/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the foreground color of the subitem's text.</returns>
			// Token: 0x1700167A RID: 5754
			// (get) Token: 0x06005DA6 RID: 23974 RVA: 0x001850F0 File Offset: 0x001832F0
			// (set) Token: 0x06005DA7 RID: 23975 RVA: 0x00185154 File Offset: 0x00183354
			public Color ForeColor
			{
				get
				{
					if (this.style != null && this.style.foreColor != Color.Empty)
					{
						return this.style.foreColor;
					}
					if (this.owner != null && this.owner.listView != null)
					{
						return this.owner.listView.ForeColor;
					}
					return SystemColors.WindowText;
				}
				set
				{
					if (this.style == null)
					{
						this.style = new ListViewItem.ListViewSubItem.SubItemStyle();
					}
					if (this.style.foreColor != value)
					{
						this.style.foreColor = value;
						if (this.owner != null)
						{
							this.owner.InvalidateListView();
						}
					}
				}
			}

			/// <summary>Gets or sets an object that contains data about the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" />. </summary>
			/// <returns>An <see cref="T:System.Object" /> that contains data about the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" />. The default is <see langword="null" />.</returns>
			// Token: 0x1700167B RID: 5755
			// (get) Token: 0x06005DA8 RID: 23976 RVA: 0x001851A6 File Offset: 0x001833A6
			// (set) Token: 0x06005DA9 RID: 23977 RVA: 0x001851AE File Offset: 0x001833AE
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

			/// <summary>Gets or sets the text of the subitem.</summary>
			/// <returns>The text to display for the subitem.</returns>
			// Token: 0x1700167C RID: 5756
			// (get) Token: 0x06005DAA RID: 23978 RVA: 0x001851B7 File Offset: 0x001833B7
			// (set) Token: 0x06005DAB RID: 23979 RVA: 0x001851CD File Offset: 0x001833CD
			[Localizable(true)]
			public string Text
			{
				get
				{
					if (this.text != null)
					{
						return this.text;
					}
					return "";
				}
				set
				{
					this.text = value;
					if (this.owner != null)
					{
						this.owner.UpdateSubItems(-1);
					}
				}
			}

			/// <summary>Gets or sets the name of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" />.</summary>
			/// <returns>The name of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" />, or an empty string ("") if a name has not been set.</returns>
			// Token: 0x1700167D RID: 5757
			// (get) Token: 0x06005DAC RID: 23980 RVA: 0x001851EA File Offset: 0x001833EA
			// (set) Token: 0x06005DAD RID: 23981 RVA: 0x00185200 File Offset: 0x00183400
			[Localizable(true)]
			public string Name
			{
				get
				{
					if (this.name != null)
					{
						return this.name;
					}
					return "";
				}
				set
				{
					this.name = value;
					if (this.owner != null)
					{
						this.owner.UpdateSubItems(-1);
					}
				}
			}

			// Token: 0x06005DAE RID: 23982 RVA: 0x0000701A File Offset: 0x0000521A
			[OnDeserializing]
			private void OnDeserializing(StreamingContext ctx)
			{
			}

			// Token: 0x06005DAF RID: 23983 RVA: 0x0018521D File Offset: 0x0018341D
			[OnDeserialized]
			private void OnDeserialized(StreamingContext ctx)
			{
				this.name = null;
				this.userData = null;
			}

			// Token: 0x06005DB0 RID: 23984 RVA: 0x0000701A File Offset: 0x0000521A
			[OnSerializing]
			private void OnSerializing(StreamingContext ctx)
			{
			}

			// Token: 0x06005DB1 RID: 23985 RVA: 0x0000701A File Offset: 0x0000521A
			[OnSerialized]
			private void OnSerialized(StreamingContext ctx)
			{
			}

			/// <summary>Resets the styles applied to the subitem to the default font and colors.</summary>
			// Token: 0x06005DB2 RID: 23986 RVA: 0x0018522D File Offset: 0x0018342D
			public void ResetStyle()
			{
				if (this.style != null)
				{
					this.style = null;
					if (this.owner != null)
					{
						this.owner.InvalidateListView();
					}
				}
			}

			/// <summary>Returns a string that represents the current object.</summary>
			/// <returns>A string that represents the current object.</returns>
			// Token: 0x06005DB3 RID: 23987 RVA: 0x00185251 File Offset: 0x00183451
			public override string ToString()
			{
				return "ListViewSubItem: {" + this.Text + "}";
			}

			// Token: 0x04003A0E RID: 14862
			[NonSerialized]
			internal ListViewItem owner;

			// Token: 0x04003A0F RID: 14863
			private string text;

			// Token: 0x04003A10 RID: 14864
			[OptionalField(VersionAdded = 2)]
			private string name;

			// Token: 0x04003A11 RID: 14865
			private ListViewItem.ListViewSubItem.SubItemStyle style;

			// Token: 0x04003A12 RID: 14866
			[OptionalField(VersionAdded = 2)]
			private object userData;

			// Token: 0x0200089B RID: 2203
			[Serializable]
			private class SubItemStyle
			{
				// Token: 0x040043FE RID: 17406
				public Color backColor = Color.Empty;

				// Token: 0x040043FF RID: 17407
				public Color foreColor = Color.Empty;

				// Token: 0x04004400 RID: 17408
				public Font font;
			}
		}

		/// <summary>Represents a collection of <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> objects stored in a <see cref="T:System.Windows.Forms.ListViewItem" />.</summary>
		// Token: 0x02000615 RID: 1557
		public class ListViewSubItemCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItemCollection" /> class.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.ListViewItem" /> that owns the collection. </param>
			// Token: 0x06005DB4 RID: 23988 RVA: 0x00185268 File Offset: 0x00183468
			public ListViewSubItemCollection(ListViewItem owner)
			{
				this.owner = owner;
			}

			/// <summary>Gets the number of subitems in the collection.</summary>
			/// <returns>The number of subitems in the collection.</returns>
			// Token: 0x1700167E RID: 5758
			// (get) Token: 0x06005DB5 RID: 23989 RVA: 0x0018527E File Offset: 0x0018347E
			[Browsable(false)]
			public int Count
			{
				get
				{
					return this.owner.SubItemCount;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
			/// <returns>The object used to synchronize the collection.</returns>
			// Token: 0x1700167F RID: 5759
			// (get) Token: 0x06005DB6 RID: 23990 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
			/// <returns>
			///     <see langword="true" /> in all cases.</returns>
			// Token: 0x17001680 RID: 5760
			// (get) Token: 0x06005DB7 RID: 23991 RVA: 0x0000E214 File Offset: 0x0000C414
			bool ICollection.IsSynchronized
			{
				get
				{
					return true;
				}
			}

			/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x17001681 RID: 5761
			// (get) Token: 0x06005DB8 RID: 23992 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether the collection is read-only.</summary>
			/// <returns>
			///     <see langword="true" /> if the collection is read-only; otherwise, <see langword="false" />.</returns>
			// Token: 0x17001682 RID: 5762
			// (get) Token: 0x06005DB9 RID: 23993 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets or sets the subitem at the specified index within the collection.</summary>
			/// <param name="index">The index of the item in the collection to retrieve. </param>
			/// <returns>A <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> representing the subitem located at the specified index within the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than zero or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListViewItem.ListViewSubItemCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItemCollection" />. </exception>
			// Token: 0x17001683 RID: 5763
			public ListViewItem.ListViewSubItem this[int index]
			{
				get
				{
					if (index < 0 || index >= this.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.owner.subItems[index];
				}
				set
				{
					if (index < 0 || index >= this.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.owner.subItems[index] = value;
					this.owner.UpdateSubItems(index);
				}
			}

			/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> at the specified index within the collection.</summary>
			/// <param name="index">The zero-based index of the element to get or set.</param>
			/// <returns>A <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> that represents the item located at the specified index within the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The index parameter is less than 0 or greater than or equal to the value of the Count property of the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" />.</exception>
			/// <exception cref="T:System.ArgumentException">The object is not a <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" />.</exception>
			// Token: 0x17001684 RID: 5764
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (value is ListViewItem.ListViewSubItem)
					{
						this[index] = (ListViewItem.ListViewSubItem)value;
						return;
					}
					throw new ArgumentException(SR.GetString("ListViewBadListViewSubItem"), "value");
				}
			}

			/// <summary>Gets an item with the specified key from the collection.</summary>
			/// <param name="key">The name of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to retrieve.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> with the specified key.</returns>
			// Token: 0x17001685 RID: 5765
			public virtual ListViewItem.ListViewSubItem this[string key]
			{
				get
				{
					if (string.IsNullOrEmpty(key))
					{
						return null;
					}
					int index = this.IndexOfKey(key);
					if (this.IsValidIndex(index))
					{
						return this[index];
					}
					return null;
				}
			}

			/// <summary>Adds an existing <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to the collection.</summary>
			/// <param name="item">The <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to add to the collection. </param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> that was added to the collection.</returns>
			// Token: 0x06005DBF RID: 23999 RVA: 0x001853B8 File Offset: 0x001835B8
			public ListViewItem.ListViewSubItem Add(ListViewItem.ListViewSubItem item)
			{
				this.EnsureSubItemSpace(1, -1);
				item.owner = this.owner;
				this.owner.subItems[this.owner.SubItemCount] = item;
				ListViewItem listViewItem = this.owner;
				ListViewItem listViewItem2 = this.owner;
				int subItemCount = listViewItem2.SubItemCount;
				listViewItem2.SubItemCount = subItemCount + 1;
				listViewItem.UpdateSubItems(subItemCount);
				return item;
			}

			/// <summary>Adds a subitem to the collection with specified text.</summary>
			/// <param name="text">The text to display for the subitem. </param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> that was added to the collection.</returns>
			// Token: 0x06005DC0 RID: 24000 RVA: 0x00185414 File Offset: 0x00183614
			public ListViewItem.ListViewSubItem Add(string text)
			{
				ListViewItem.ListViewSubItem listViewSubItem = new ListViewItem.ListViewSubItem(this.owner, text);
				this.Add(listViewSubItem);
				return listViewSubItem;
			}

			/// <summary>Adds a subitem to the collection with specified text, foreground color, background color, and font settings.</summary>
			/// <param name="text">The text to display for the subitem. </param>
			/// <param name="foreColor">A <see cref="T:System.Drawing.Color" /> that represents the foreground color of the subitem. </param>
			/// <param name="backColor">A <see cref="T:System.Drawing.Color" /> that represents the background color of the subitem. </param>
			/// <param name="font">A <see cref="T:System.Drawing.Font" /> that represents the typeface to display the subitem's text in. </param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> that was added to the collection.</returns>
			// Token: 0x06005DC1 RID: 24001 RVA: 0x00185438 File Offset: 0x00183638
			public ListViewItem.ListViewSubItem Add(string text, Color foreColor, Color backColor, Font font)
			{
				ListViewItem.ListViewSubItem listViewSubItem = new ListViewItem.ListViewSubItem(this.owner, text, foreColor, backColor, font);
				this.Add(listViewSubItem);
				return listViewSubItem;
			}

			/// <summary>Adds an array of <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> objects to the collection.</summary>
			/// <param name="items">An array of <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> objects to add to the collection. </param>
			// Token: 0x06005DC2 RID: 24002 RVA: 0x00185460 File Offset: 0x00183660
			public void AddRange(ListViewItem.ListViewSubItem[] items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				this.EnsureSubItemSpace(items.Length, -1);
				foreach (ListViewItem.ListViewSubItem listViewSubItem in items)
				{
					if (listViewSubItem != null)
					{
						ListViewItem.ListViewSubItem[] subItems = this.owner.subItems;
						ListViewItem listViewItem = this.owner;
						int subItemCount = listViewItem.SubItemCount;
						listViewItem.SubItemCount = subItemCount + 1;
						subItems[subItemCount] = listViewSubItem;
					}
				}
				this.owner.UpdateSubItems(-1);
			}

			/// <summary>Creates new subitems based on an array and adds them to the collection.</summary>
			/// <param name="items">An array of strings representing the text of each subitem to add to the collection. </param>
			// Token: 0x06005DC3 RID: 24003 RVA: 0x001854CC File Offset: 0x001836CC
			public void AddRange(string[] items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				this.EnsureSubItemSpace(items.Length, -1);
				foreach (string text in items)
				{
					if (text != null)
					{
						ListViewItem.ListViewSubItem[] subItems = this.owner.subItems;
						ListViewItem listViewItem = this.owner;
						int subItemCount = listViewItem.SubItemCount;
						listViewItem.SubItemCount = subItemCount + 1;
						subItems[subItemCount] = new ListViewItem.ListViewSubItem(this.owner, text);
					}
				}
				this.owner.UpdateSubItems(-1);
			}

			/// <summary>Creates new subitems based on an array and adds them to the collection with specified foreground color, background color, and font.</summary>
			/// <param name="items">An array of strings representing the text of each subitem to add to the collection. </param>
			/// <param name="foreColor">A <see cref="T:System.Drawing.Color" /> that represents the foreground color of the subitem. </param>
			/// <param name="backColor">A <see cref="T:System.Drawing.Color" /> that represents the background color of the subitem. </param>
			/// <param name="font">A <see cref="T:System.Drawing.Font" /> that represents the typeface to display the subitem's text in. </param>
			// Token: 0x06005DC4 RID: 24004 RVA: 0x00185544 File Offset: 0x00183744
			public void AddRange(string[] items, Color foreColor, Color backColor, Font font)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				this.EnsureSubItemSpace(items.Length, -1);
				foreach (string text in items)
				{
					if (text != null)
					{
						ListViewItem.ListViewSubItem[] subItems = this.owner.subItems;
						ListViewItem listViewItem = this.owner;
						int subItemCount = listViewItem.SubItemCount;
						listViewItem.SubItemCount = subItemCount + 1;
						subItems[subItemCount] = new ListViewItem.ListViewSubItem(this.owner, text, foreColor, backColor, font);
					}
				}
				this.owner.UpdateSubItems(-1);
			}

			/// <summary>Adds an existing <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to the collection.</summary>
			/// <param name="item">The <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to add to the collection.</param>
			/// <returns>The zero-based index that indicates the location of the object that was added to the collection.</returns>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="item" /> is not a <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" />.</exception>
			// Token: 0x06005DC5 RID: 24005 RVA: 0x001855BE File Offset: 0x001837BE
			int IList.Add(object item)
			{
				if (item is ListViewItem.ListViewSubItem)
				{
					return this.IndexOf(this.Add((ListViewItem.ListViewSubItem)item));
				}
				throw new ArgumentException(SR.GetString("ListViewSubItemCollectionInvalidArgument"));
			}

			/// <summary>Removes all subitems and the parent <see cref="T:System.Windows.Forms.ListViewItem" /> from the collection.</summary>
			// Token: 0x06005DC6 RID: 24006 RVA: 0x001855EC File Offset: 0x001837EC
			public void Clear()
			{
				int subItemCount = this.owner.SubItemCount;
				if (subItemCount > 0)
				{
					this.owner.SubItemCount = 0;
					this.owner.UpdateSubItems(-1, subItemCount);
				}
			}

			/// <summary>Determines whether the specified subitem is located in the collection.</summary>
			/// <param name="subItem">A <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> representing the subitem to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the subitem is contained in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005DC7 RID: 24007 RVA: 0x00185622 File Offset: 0x00183822
			public bool Contains(ListViewItem.ListViewSubItem subItem)
			{
				return this.IndexOf(subItem) != -1;
			}

			/// <summary>Determines whether the specified subitem is located in the collection.</summary>
			/// <param name="subItem">An object that represents the subitem to locate in the collection.</param>
			/// <returns>
			///     <see langword="true" /> if the subitem is contained in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005DC8 RID: 24008 RVA: 0x00185631 File Offset: 0x00183831
			bool IList.Contains(object subItem)
			{
				return subItem is ListViewItem.ListViewSubItem && this.Contains((ListViewItem.ListViewSubItem)subItem);
			}

			/// <summary>Determines if the collection contains an item with the specified key.</summary>
			/// <param name="key">The name of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to look for.</param>
			/// <returns>
			///     <see langword="true" /> to indicate the collection contains an item with the specified key; otherwise, <see langword="false" />. </returns>
			// Token: 0x06005DC9 RID: 24009 RVA: 0x00185649 File Offset: 0x00183849
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			// Token: 0x06005DCA RID: 24010 RVA: 0x00185658 File Offset: 0x00183858
			private void EnsureSubItemSpace(int size, int index)
			{
				if (this.owner.SubItemCount == 4096)
				{
					throw new InvalidOperationException(SR.GetString("ErrorCollectionFull"));
				}
				if (this.owner.SubItemCount + size <= this.owner.subItems.Length)
				{
					if (index != -1)
					{
						for (int i = this.owner.SubItemCount - 1; i >= index; i--)
						{
							this.owner.subItems[i + size] = this.owner.subItems[i];
						}
					}
					return;
				}
				if (this.owner.subItems == null)
				{
					int num = (size > 4) ? size : 4;
					this.owner.subItems = new ListViewItem.ListViewSubItem[num];
					return;
				}
				int num2 = this.owner.subItems.Length * 2;
				while (num2 - this.owner.SubItemCount < size)
				{
					num2 *= 2;
				}
				ListViewItem.ListViewSubItem[] array = new ListViewItem.ListViewSubItem[num2];
				if (index != -1)
				{
					Array.Copy(this.owner.subItems, 0, array, 0, index);
					Array.Copy(this.owner.subItems, index, array, index + size, this.owner.SubItemCount - index);
				}
				else
				{
					Array.Copy(this.owner.subItems, array, this.owner.SubItemCount);
				}
				this.owner.subItems = array;
			}

			/// <summary>Returns the index within the collection of the specified subitem.</summary>
			/// <param name="subItem">A <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> representing the subitem to locate in the collection. </param>
			/// <returns>The zero-based index of the subitem's location in the collection. If the subitem is not located in the collection, the return value is negative one (-1).</returns>
			// Token: 0x06005DCB RID: 24011 RVA: 0x00185798 File Offset: 0x00183998
			public int IndexOf(ListViewItem.ListViewSubItem subItem)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.owner.subItems[i] == subItem)
					{
						return i;
					}
				}
				return -1;
			}

			/// <summary>Returns the index within the collection of the specified subitem.</summary>
			/// <param name="subItem">An object that represents the subitem to locate in the collection.</param>
			/// <returns>The zero-based index of the subitem if it is in the collection; otherwise, -1.</returns>
			// Token: 0x06005DCC RID: 24012 RVA: 0x001857C9 File Offset: 0x001839C9
			int IList.IndexOf(object subItem)
			{
				if (subItem is ListViewItem.ListViewSubItem)
				{
					return this.IndexOf((ListViewItem.ListViewSubItem)subItem);
				}
				return -1;
			}

			/// <summary>Returns the index of the first occurrence of an item with the specified key within the collection.</summary>
			/// <param name="key">The name of the item to retrieve the index for.</param>
			/// <returns>The zero-based index of the first occurrence of an item with the specified key.</returns>
			// Token: 0x06005DCD RID: 24013 RVA: 0x001857E4 File Offset: 0x001839E4
			public virtual int IndexOfKey(string key)
			{
				if (string.IsNullOrEmpty(key))
				{
					return -1;
				}
				if (this.IsValidIndex(this.lastAccessedIndex) && WindowsFormsUtils.SafeCompareStrings(this[this.lastAccessedIndex].Name, key, true))
				{
					return this.lastAccessedIndex;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (WindowsFormsUtils.SafeCompareStrings(this[i].Name, key, true))
					{
						this.lastAccessedIndex = i;
						return i;
					}
				}
				this.lastAccessedIndex = -1;
				return -1;
			}

			// Token: 0x06005DCE RID: 24014 RVA: 0x00185861 File Offset: 0x00183A61
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			/// <summary>Inserts a subitem into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the item is inserted. </param>
			/// <param name="item">A <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> representing the subitem to insert into the collection. </param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than zero or greater than the value of the <see cref="P:System.Windows.Forms.ListViewItem.ListViewSubItemCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItemCollection" />. </exception>
			// Token: 0x06005DCF RID: 24015 RVA: 0x00185874 File Offset: 0x00183A74
			public void Insert(int index, ListViewItem.ListViewSubItem item)
			{
				if (index < 0 || index > this.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				item.owner = this.owner;
				this.EnsureSubItemSpace(1, index);
				this.owner.subItems[index] = item;
				ListViewItem listViewItem = this.owner;
				int subItemCount = listViewItem.SubItemCount;
				listViewItem.SubItemCount = subItemCount + 1;
				this.owner.UpdateSubItems(-1);
			}

			/// <summary>Inserts a subitem into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the item is inserted.</param>
			/// <param name="item">An object that represents the subitem to insert into the collection.</param>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="item" /> is not a <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The index parameter is less than 0 or greater than or equal to the value of the Count property of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItemCollection" />.</exception>
			// Token: 0x06005DD0 RID: 24016 RVA: 0x001858DC File Offset: 0x00183ADC
			void IList.Insert(int index, object item)
			{
				if (item is ListViewItem.ListViewSubItem)
				{
					this.Insert(index, (ListViewItem.ListViewSubItem)item);
					return;
				}
				throw new ArgumentException(SR.GetString("ListViewBadListViewSubItem"), "item");
			}

			/// <summary>Removes a specified item from the collection.</summary>
			/// <param name="item">The item to remove from the collection.</param>
			// Token: 0x06005DD1 RID: 24017 RVA: 0x00185908 File Offset: 0x00183B08
			public void Remove(ListViewItem.ListViewSubItem item)
			{
				int num = this.IndexOf(item);
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			/// <summary>Removes a specified item from the collection.</summary>
			/// <param name="item">The item to remove from the collection.</param>
			// Token: 0x06005DD2 RID: 24018 RVA: 0x00185928 File Offset: 0x00183B28
			void IList.Remove(object item)
			{
				if (item is ListViewItem.ListViewSubItem)
				{
					this.Remove((ListViewItem.ListViewSubItem)item);
				}
			}

			/// <summary>Removes the subitem at the specified index within the collection.</summary>
			/// <param name="index">The zero-based index of the subitem to remove. </param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than zero or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListViewItem.ListViewSubItemCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItemCollection" />. </exception>
			// Token: 0x06005DD3 RID: 24019 RVA: 0x00185940 File Offset: 0x00183B40
			public void RemoveAt(int index)
			{
				if (index < 0 || index >= this.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				for (int i = index + 1; i < this.owner.SubItemCount; i++)
				{
					this.owner.subItems[i - 1] = this.owner.subItems[i];
				}
				int subItemCount = this.owner.SubItemCount;
				ListViewItem listViewItem = this.owner;
				int subItemCount2 = listViewItem.SubItemCount;
				listViewItem.SubItemCount = subItemCount2 - 1;
				this.owner.subItems[this.owner.SubItemCount] = null;
				this.owner.UpdateSubItems(-1, subItemCount);
			}

			/// <summary>Removes an item with the specified key from the collection.</summary>
			/// <param name="key">The name of the item to remove from the collection.</param>
			// Token: 0x06005DD4 RID: 24020 RVA: 0x001859E0 File Offset: 0x00183BE0
			public virtual void RemoveByKey(string key)
			{
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					this.RemoveAt(index);
				}
			}

			/// <summary>Copies the item and collection of subitems into an array.</summary>
			/// <param name="dest">An array of <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" />.</param>
			/// <param name="index">The zero-based index in array at which copying begins.</param>
			/// <exception cref="T:System.ArrayTypeMismatchException">The array type is not compatible with <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" />.</exception>
			// Token: 0x06005DD5 RID: 24021 RVA: 0x00185A05 File Offset: 0x00183C05
			void ICollection.CopyTo(Array dest, int index)
			{
				if (this.Count > 0)
				{
					Array.Copy(this.owner.subItems, 0, dest, index, this.Count);
				}
			}

			/// <summary>Returns an enumerator to use to iterate through the subitem collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the subitem collection.</returns>
			// Token: 0x06005DD6 RID: 24022 RVA: 0x00185A29 File Offset: 0x00183C29
			public IEnumerator GetEnumerator()
			{
				if (this.owner.subItems != null)
				{
					return new WindowsFormsUtils.ArraySubsetEnumerator(this.owner.subItems, this.owner.SubItemCount);
				}
				return new ListViewItem.ListViewSubItem[0].GetEnumerator();
			}

			// Token: 0x04003A13 RID: 14867
			private ListViewItem owner;

			// Token: 0x04003A14 RID: 14868
			private int lastAccessedIndex = -1;
		}
	}
}
