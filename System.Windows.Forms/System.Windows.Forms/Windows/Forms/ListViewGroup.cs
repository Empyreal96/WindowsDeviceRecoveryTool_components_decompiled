using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents a group of items displayed within a <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
	// Token: 0x020002C5 RID: 709
	[TypeConverter(typeof(ListViewGroupConverter))]
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	[DefaultProperty("Header")]
	[Serializable]
	public sealed class ListViewGroup : ISerializable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewGroup" /> class using the default header text of "ListViewGroup" and the default left header alignment.</summary>
		// Token: 0x06002A82 RID: 10882 RVA: 0x000C864D File Offset: 0x000C684D
		public ListViewGroup() : this(SR.GetString("ListViewGroupDefaultHeader", new object[]
		{
			ListViewGroup.nextHeader++
		}))
		{
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x000C867A File Offset: 0x000C687A
		private ListViewGroup(SerializationInfo info, StreamingContext context) : this()
		{
			this.Deserialize(info, context);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewGroup" /> class using the specified values to initialize the <see cref="P:System.Windows.Forms.ListViewGroup.Name" /> and <see cref="P:System.Windows.Forms.ListViewGroup.Header" /> properties. </summary>
		/// <param name="key">The initial value of the <see cref="P:System.Windows.Forms.ListViewGroup.Name" /> property.</param>
		/// <param name="headerText">The initial value of the <see cref="P:System.Windows.Forms.ListViewGroup.Header" /> property.</param>
		// Token: 0x06002A84 RID: 10884 RVA: 0x000C868A File Offset: 0x000C688A
		public ListViewGroup(string key, string headerText) : this()
		{
			this.name = key;
			this.header = headerText;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewGroup" /> class using the specified value to initialize the <see cref="P:System.Windows.Forms.ListViewGroup.Header" /> property and using the default left header alignment.</summary>
		/// <param name="header">The text to display for the group header. </param>
		// Token: 0x06002A85 RID: 10885 RVA: 0x000C86A0 File Offset: 0x000C68A0
		public ListViewGroup(string header)
		{
			this.header = header;
			this.id = ListViewGroup.nextID++;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewGroup" /> class using the specified header text and the specified header alignment.</summary>
		/// <param name="header">The text to display for the group header. </param>
		/// <param name="headerAlignment">One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values that specifies the alignment of the header text. </param>
		// Token: 0x06002A86 RID: 10886 RVA: 0x000C86C2 File Offset: 0x000C68C2
		public ListViewGroup(string header, HorizontalAlignment headerAlignment) : this(header)
		{
			this.headerAlignment = headerAlignment;
		}

		/// <summary>Gets or sets the header text for the group.</summary>
		/// <returns>The text to display for the group header. The default is "ListViewGroup".</returns>
		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06002A87 RID: 10887 RVA: 0x000C86D2 File Offset: 0x000C68D2
		// (set) Token: 0x06002A88 RID: 10888 RVA: 0x000C86E8 File Offset: 0x000C68E8
		[SRCategory("CatAppearance")]
		public string Header
		{
			get
			{
				if (this.header != null)
				{
					return this.header;
				}
				return "";
			}
			set
			{
				if (this.header != value)
				{
					this.header = value;
					if (this.listView != null)
					{
						this.listView.RecreateHandleInternal();
					}
				}
			}
		}

		/// <summary>Gets or sets the alignment of the group header text.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values that specifies the alignment of the header text. The default is <see cref="F:System.Windows.Forms.HorizontalAlignment.Left" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid <see cref="T:System.Windows.Forms.HorizontalAlignment" /> value.</exception>
		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06002A89 RID: 10889 RVA: 0x000C8712 File Offset: 0x000C6912
		// (set) Token: 0x06002A8A RID: 10890 RVA: 0x000C871A File Offset: 0x000C691A
		[DefaultValue(HorizontalAlignment.Left)]
		[SRCategory("CatAppearance")]
		public HorizontalAlignment HeaderAlignment
		{
			get
			{
				return this.headerAlignment;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(HorizontalAlignment));
				}
				if (this.headerAlignment != value)
				{
					this.headerAlignment = value;
					this.UpdateListView();
				}
			}
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06002A8B RID: 10891 RVA: 0x000C8758 File Offset: 0x000C6958
		internal int ID
		{
			get
			{
				return this.id;
			}
		}

		/// <summary>Gets a collection containing all items associated with this group.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> that contains all the items in the group. If there are no items in the group, an empty <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> object is returned.</returns>
		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06002A8C RID: 10892 RVA: 0x000C8760 File Offset: 0x000C6960
		[Browsable(false)]
		public ListView.ListViewItemCollection Items
		{
			get
			{
				if (this.items == null)
				{
					this.items = new ListView.ListViewItemCollection(new ListViewGroupItemCollection(this));
				}
				return this.items;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ListView" /> control that contains this group. </summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ListView" /> control that contains this group.</returns>
		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06002A8D RID: 10893 RVA: 0x000C8781 File Offset: 0x000C6981
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ListView ListView
		{
			get
			{
				return this.listView;
			}
		}

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06002A8E RID: 10894 RVA: 0x000C8781 File Offset: 0x000C6981
		// (set) Token: 0x06002A8F RID: 10895 RVA: 0x000C8789 File Offset: 0x000C6989
		internal ListView ListViewInternal
		{
			get
			{
				return this.listView;
			}
			set
			{
				if (this.listView != value)
				{
					this.listView = value;
				}
			}
		}

		/// <summary>Gets or sets the name of the group.</summary>
		/// <returns>The name of the group.</returns>
		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06002A90 RID: 10896 RVA: 0x000C879B File Offset: 0x000C699B
		// (set) Token: 0x06002A91 RID: 10897 RVA: 0x000C87A3 File Offset: 0x000C69A3
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewGroupNameDescr")]
		[Browsable(true)]
		[DefaultValue("")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		/// <summary>Gets or sets the object that contains data about the group.</summary>
		/// <returns>An <see cref="T:System.Object" /> for storing the additional data. </returns>
		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06002A92 RID: 10898 RVA: 0x000C87AC File Offset: 0x000C69AC
		// (set) Token: 0x06002A93 RID: 10899 RVA: 0x000C87B4 File Offset: 0x000C69B4
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

		// Token: 0x06002A94 RID: 10900 RVA: 0x000C87C0 File Offset: 0x000C69C0
		private void Deserialize(SerializationInfo info, StreamingContext context)
		{
			int num = 0;
			foreach (SerializationEntry serializationEntry in info)
			{
				if (serializationEntry.Name == "Header")
				{
					this.Header = (string)serializationEntry.Value;
				}
				else if (serializationEntry.Name == "HeaderAlignment")
				{
					this.HeaderAlignment = (HorizontalAlignment)serializationEntry.Value;
				}
				else if (serializationEntry.Name == "Tag")
				{
					this.Tag = serializationEntry.Value;
				}
				else if (serializationEntry.Name == "ItemsCount")
				{
					num = (int)serializationEntry.Value;
				}
				else if (serializationEntry.Name == "Name")
				{
					this.Name = (string)serializationEntry.Value;
				}
			}
			if (num > 0)
			{
				ListViewItem[] array = new ListViewItem[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = (ListViewItem)info.GetValue("Item" + i, typeof(ListViewItem));
				}
				this.Items.AddRange(array);
			}
		}

		/// <summary>Returns a string that represents the current object.</summary>
		/// <returns>A string that represents the current object.</returns>
		// Token: 0x06002A95 RID: 10901 RVA: 0x000C88F8 File Offset: 0x000C6AF8
		public override string ToString()
		{
			return this.Header;
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x000C8900 File Offset: 0x000C6B00
		private void UpdateListView()
		{
			if (this.listView != null && this.listView.IsHandleCreated)
			{
				this.listView.UpdateGroupNative(this);
			}
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
		/// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
		// Token: 0x06002A97 RID: 10903 RVA: 0x000C8924 File Offset: 0x000C6B24
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Header", this.Header);
			info.AddValue("HeaderAlignment", this.HeaderAlignment);
			info.AddValue("Tag", this.Tag);
			if (!string.IsNullOrEmpty(this.Name))
			{
				info.AddValue("Name", this.Name);
			}
			if (this.items != null && this.items.Count > 0)
			{
				info.AddValue("ItemsCount", this.Items.Count);
				for (int i = 0; i < this.Items.Count; i++)
				{
					info.AddValue("Item" + i.ToString(CultureInfo.InvariantCulture), this.Items[i], typeof(ListViewItem));
				}
			}
		}

		// Token: 0x04001253 RID: 4691
		private ListView listView;

		// Token: 0x04001254 RID: 4692
		private int id;

		// Token: 0x04001255 RID: 4693
		private string header;

		// Token: 0x04001256 RID: 4694
		private HorizontalAlignment headerAlignment;

		// Token: 0x04001257 RID: 4695
		private ListView.ListViewItemCollection items;

		// Token: 0x04001258 RID: 4696
		private static int nextID;

		// Token: 0x04001259 RID: 4697
		private static int nextHeader = 1;

		// Token: 0x0400125A RID: 4698
		private object userData;

		// Token: 0x0400125B RID: 4699
		private string name;
	}
}
