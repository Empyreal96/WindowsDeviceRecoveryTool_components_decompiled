using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows list view control, which displays a collection of items that can be displayed using one of four different views. </summary>
	// Token: 0x020002C3 RID: 707
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Docking(DockingBehavior.Ask)]
	[Designer("System.Windows.Forms.Design.ListViewDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("Items")]
	[DefaultEvent("SelectedIndexChanged")]
	[SRDescription("DescriptionListView")]
	public class ListView : Control
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListView" /> class.</summary>
		// Token: 0x06002967 RID: 10599 RVA: 0x000C0C48 File Offset: 0x000BEE48
		public ListView()
		{
			int num = 8392196;
			if (!AccessibilityImprovements.Level3)
			{
				num |= 64;
			}
			this.listViewState = new BitVector32(num);
			this.listViewState1 = new BitVector32(8);
			base.SetStyle(ControlStyles.UserPaint, false);
			base.SetStyle(ControlStyles.StandardClick, false);
			base.SetStyle(ControlStyles.UseTextForAccessibility, false);
			this.odCacheFont = this.Font;
			this.odCacheFontHandle = base.FontHandle;
			base.SetBounds(0, 0, 121, 97);
			this.listItemCollection = new ListView.ListViewItemCollection(new ListView.ListViewNativeItemCollection(this));
			this.columnHeaderCollection = new ListView.ColumnHeaderCollection(this);
		}

		/// <summary>Gets or sets the type of action the user must take to activate an item.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ItemActivation" /> values. The default is <see cref="F:System.Windows.Forms.ItemActivation.Standard" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is not one of the <see cref="T:System.Windows.Forms.ItemActivation" /> members. </exception>
		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x06002968 RID: 10600 RVA: 0x000C0D61 File Offset: 0x000BEF61
		// (set) Token: 0x06002969 RID: 10601 RVA: 0x000C0D6C File Offset: 0x000BEF6C
		[SRCategory("CatBehavior")]
		[DefaultValue(ItemActivation.Standard)]
		[SRDescription("ListViewActivationDescr")]
		public ItemActivation Activation
		{
			get
			{
				return this.activation;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ItemActivation));
				}
				if (this.HotTracking && value != ItemActivation.OneClick)
				{
					throw new ArgumentException(SR.GetString("ListViewActivationMustBeOnWhenHotTrackingIsOn"), "value");
				}
				if (this.activation != value)
				{
					this.activation = value;
					this.UpdateExtendedStyles();
				}
			}
		}

		/// <summary>Gets or sets the alignment of items in the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ListViewAlignment" /> values. The default is <see cref="F:System.Windows.Forms.ListViewAlignment.Top" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is not one of the <see cref="T:System.Windows.Forms.ListViewAlignment" /> values. </exception>
		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x0600296A RID: 10602 RVA: 0x000C0DD6 File Offset: 0x000BEFD6
		// (set) Token: 0x0600296B RID: 10603 RVA: 0x000C0DE0 File Offset: 0x000BEFE0
		[SRCategory("CatBehavior")]
		[DefaultValue(ListViewAlignment.Top)]
		[Localizable(true)]
		[SRDescription("ListViewAlignmentDescr")]
		public ListViewAlignment Alignment
		{
			get
			{
				return this.alignStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid_NotSequential(value, (int)value, new int[]
				{
					0,
					2,
					1,
					5
				}))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ListViewAlignment));
				}
				if (this.alignStyle != value)
				{
					this.alignStyle = value;
					this.RecreateHandleInternal();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the user can drag column headers to reorder columns in the control.</summary>
		/// <returns>
		///     <see langword="true" /> if drag-and-drop column reordering is allowed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x0600296C RID: 10604 RVA: 0x000C0E38 File Offset: 0x000BF038
		// (set) Token: 0x0600296D RID: 10605 RVA: 0x000C0E46 File Offset: 0x000BF046
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListViewAllowColumnReorderDescr")]
		public bool AllowColumnReorder
		{
			get
			{
				return this.listViewState[2];
			}
			set
			{
				if (this.AllowColumnReorder != value)
				{
					this.listViewState[2] = value;
					this.UpdateExtendedStyles();
				}
			}
		}

		/// <summary>Gets or sets whether icons are automatically kept arranged.</summary>
		/// <returns>
		///     <see langword="true" /> if icons are automatically kept arranged and snapped to the grid; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x0600296E RID: 10606 RVA: 0x000C0E64 File Offset: 0x000BF064
		// (set) Token: 0x0600296F RID: 10607 RVA: 0x000C0E72 File Offset: 0x000BF072
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("ListViewAutoArrangeDescr")]
		public bool AutoArrange
		{
			get
			{
				return this.listViewState[4];
			}
			set
			{
				if (this.AutoArrange != value)
				{
					this.listViewState[4] = value;
					base.UpdateStyles();
				}
			}
		}

		/// <summary>Gets or sets the background color.</summary>
		/// <returns>The <see cref="T:System.Drawing.Color" /> of the background.</returns>
		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x06002970 RID: 10608 RVA: 0x0001FD6B File Offset: 0x0001DF6B
		// (set) Token: 0x06002971 RID: 10609 RVA: 0x000C0E90 File Offset: 0x000BF090
		public override Color BackColor
		{
			get
			{
				if (this.ShouldSerializeBackColor())
				{
					return base.BackColor;
				}
				return SystemColors.Window;
			}
			set
			{
				base.BackColor = value;
				if (base.IsHandleCreated)
				{
					base.SendMessage(4097, 0, ColorTranslator.ToWin32(this.BackColor));
				}
			}
		}

		/// <summary>Gets or sets an <see cref="T:System.Windows.Forms.ImageLayout" /> value.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImageLayout" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is not one of the <see cref="T:System.Windows.Forms.ImageLayout" /> values.</exception>
		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x06002972 RID: 10610 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x06002973 RID: 10611 RVA: 0x00011FDB File Offset: 0x000101DB
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ListView.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x140001ED RID: 493
		// (add) Token: 0x06002974 RID: 10612 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x06002975 RID: 10613 RVA: 0x0001FD9C File Offset: 0x0001DF9C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				base.BackgroundImageLayoutChanged -= value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the background image of the <see cref="T:System.Windows.Forms.ListView" /> should be tiled.</summary>
		/// <returns>
		///     <see langword="true" /> if the background image of the <see cref="T:System.Windows.Forms.ListView" /> should be tiled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x06002976 RID: 10614 RVA: 0x000C0EB9 File Offset: 0x000BF0B9
		// (set) Token: 0x06002977 RID: 10615 RVA: 0x000C0ECC File Offset: 0x000BF0CC
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		[SRDescription("ListViewBackgroundImageTiledDescr")]
		public bool BackgroundImageTiled
		{
			get
			{
				return this.listViewState[65536];
			}
			set
			{
				if (this.BackgroundImageTiled != value)
				{
					this.listViewState[65536] = value;
					if (base.IsHandleCreated && this.BackgroundImage != null)
					{
						NativeMethods.LVBKIMAGE lvbkimage = new NativeMethods.LVBKIMAGE();
						lvbkimage.xOffset = 0;
						lvbkimage.yOffset = 0;
						if (this.BackgroundImageTiled)
						{
							lvbkimage.ulFlags = 16;
						}
						else
						{
							lvbkimage.ulFlags = 0;
						}
						lvbkimage.ulFlags |= 2;
						lvbkimage.pszImage = this.backgroundImageFileName;
						lvbkimage.cchImageMax = this.backgroundImageFileName.Length + 1;
						UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_SETBKIMAGE, 0, lvbkimage);
					}
				}
			}
		}

		/// <summary>Gets or sets the border style of the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. The default is <see cref="F:System.Windows.Forms.BorderStyle.Fixed3D" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is not one of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. </exception>
		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x06002978 RID: 10616 RVA: 0x000C0F79 File Offset: 0x000BF179
		// (set) Token: 0x06002979 RID: 10617 RVA: 0x000C0F81 File Offset: 0x000BF181
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.Fixed3D)]
		[DispId(-504)]
		[SRDescription("borderStyleDescr")]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
				}
				if (this.borderStyle != value)
				{
					this.borderStyle = value;
					base.UpdateStyles();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether a check box appears next to each item in the control.</summary>
		/// <returns>
		///     <see langword="true" /> if a check box appears next to each item in the <see cref="T:System.Windows.Forms.ListView" /> control; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x0600297A RID: 10618 RVA: 0x000C0FBF File Offset: 0x000BF1BF
		// (set) Token: 0x0600297B RID: 10619 RVA: 0x000C0FD0 File Offset: 0x000BF1D0
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		[SRDescription("ListViewCheckBoxesDescr")]
		public bool CheckBoxes
		{
			get
			{
				return this.listViewState[8];
			}
			set
			{
				if (this.UseCompatibleStateImageBehavior)
				{
					if (this.CheckBoxes != value)
					{
						if (value && this.View == View.Tile)
						{
							throw new NotSupportedException(SR.GetString("ListViewCheckBoxesNotSupportedInTileView"));
						}
						if (this.CheckBoxes)
						{
							this.savedCheckedItems = new List<ListViewItem>(this.CheckedItems.Count);
							ListViewItem[] array = new ListViewItem[this.CheckedItems.Count];
							this.CheckedItems.CopyTo(array, 0);
							for (int i = 0; i < array.Length; i++)
							{
								this.savedCheckedItems.Add(array[i]);
							}
						}
						this.listViewState[8] = value;
						this.UpdateExtendedStyles();
						if (this.CheckBoxes && this.savedCheckedItems != null)
						{
							if (this.savedCheckedItems.Count > 0)
							{
								foreach (ListViewItem listViewItem in this.savedCheckedItems)
								{
									listViewItem.Checked = true;
								}
							}
							this.savedCheckedItems = null;
						}
						if (this.AutoArrange)
						{
							this.ArrangeIcons(this.Alignment);
							return;
						}
					}
				}
				else if (this.CheckBoxes != value)
				{
					if (value && this.View == View.Tile)
					{
						throw new NotSupportedException(SR.GetString("ListViewCheckBoxesNotSupportedInTileView"));
					}
					if (this.CheckBoxes)
					{
						this.savedCheckedItems = new List<ListViewItem>(this.CheckedItems.Count);
						ListViewItem[] array2 = new ListViewItem[this.CheckedItems.Count];
						this.CheckedItems.CopyTo(array2, 0);
						for (int j = 0; j < array2.Length; j++)
						{
							this.savedCheckedItems.Add(array2[j]);
						}
					}
					this.listViewState[8] = value;
					if ((!value && this.StateImageList != null && base.IsHandleCreated) || (!value && this.Alignment == ListViewAlignment.Left && base.IsHandleCreated) || (value && this.View == View.List && base.IsHandleCreated) || (value && (this.View == View.SmallIcon || this.View == View.LargeIcon) && base.IsHandleCreated))
					{
						this.RecreateHandleInternal();
					}
					else
					{
						this.UpdateExtendedStyles();
					}
					if (this.CheckBoxes && this.savedCheckedItems != null)
					{
						if (this.savedCheckedItems.Count > 0)
						{
							foreach (ListViewItem listViewItem2 in this.savedCheckedItems)
							{
								listViewItem2.Checked = true;
							}
						}
						this.savedCheckedItems = null;
					}
					if (base.IsHandleCreated && this.imageListState != null)
					{
						if (this.CheckBoxes)
						{
							base.SendMessage(4099, 2, this.imageListState.Handle);
						}
						else
						{
							base.SendMessage(4099, 2, IntPtr.Zero);
						}
					}
					if (this.AutoArrange)
					{
						this.ArrangeIcons(this.Alignment);
					}
				}
			}
		}

		/// <summary>Gets the indexes of the currently checked items in the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListView.CheckedIndexCollection" /> that contains the indexes of the currently checked items. If no items are currently checked, an empty <see cref="T:System.Windows.Forms.ListView.CheckedIndexCollection" /> is returned.</returns>
		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x0600297C RID: 10620 RVA: 0x000C12BC File Offset: 0x000BF4BC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ListView.CheckedIndexCollection CheckedIndices
		{
			get
			{
				if (this.checkedIndexCollection == null)
				{
					this.checkedIndexCollection = new ListView.CheckedIndexCollection(this);
				}
				return this.checkedIndexCollection;
			}
		}

		/// <summary>Gets the currently checked items in the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListView.CheckedListViewItemCollection" /> that contains the currently checked items. If no items are currently checked, an empty <see cref="T:System.Windows.Forms.ListView.CheckedListViewItemCollection" /> is returned.</returns>
		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x0600297D RID: 10621 RVA: 0x000C12D8 File Offset: 0x000BF4D8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ListView.CheckedListViewItemCollection CheckedItems
		{
			get
			{
				if (this.checkedListViewItemCollection == null)
				{
					this.checkedListViewItemCollection = new ListView.CheckedListViewItemCollection(this);
				}
				return this.checkedListViewItemCollection;
			}
		}

		/// <summary>Gets the collection of all column headers that appear in the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" /> that represents the column headers that appear when the <see cref="P:System.Windows.Forms.ListView.View" /> property is set to <see cref="F:System.Windows.Forms.View.Details" />.</returns>
		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x0600297E RID: 10622 RVA: 0x000C12F4 File Offset: 0x000BF4F4
		[SRCategory("CatBehavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("System.Windows.Forms.Design.ColumnHeaderCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("ListViewColumnsDescr")]
		[Localizable(true)]
		[MergableProperty(false)]
		public ListView.ColumnHeaderCollection Columns
		{
			get
			{
				return this.columnHeaderCollection;
			}
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x0600297F RID: 10623 RVA: 0x000C12FC File Offset: 0x000BF4FC
		private bool ComctlSupportsVisualStyles
		{
			get
			{
				if (!this.listViewState[4194304])
				{
					this.listViewState[4194304] = true;
					this.listViewState[2097152] = Application.ComCtlSupportsVisualStyles;
				}
				return this.listViewState[2097152];
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>
		///     <see langword="null" /> in all cases.</returns>
		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x06002980 RID: 10624 RVA: 0x000C1354 File Offset: 0x000BF554
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "SysListView32";
				if (base.IsHandleCreated)
				{
					int num = (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -16));
					createParams.Style |= (num & 3145728);
				}
				createParams.Style |= 64;
				ListViewAlignment listViewAlignment = this.alignStyle;
				if (listViewAlignment != ListViewAlignment.Left)
				{
					if (listViewAlignment == ListViewAlignment.Top)
					{
						createParams.Style |= 0;
					}
				}
				else
				{
					createParams.Style |= 2048;
				}
				if (this.AutoArrange)
				{
					createParams.Style |= 256;
				}
				BorderStyle borderStyle = this.borderStyle;
				if (borderStyle != BorderStyle.FixedSingle)
				{
					if (borderStyle == BorderStyle.Fixed3D)
					{
						createParams.ExStyle |= 512;
					}
				}
				else
				{
					createParams.Style |= 8388608;
				}
				ColumnHeaderStyle columnHeaderStyle = this.headerStyle;
				if (columnHeaderStyle != ColumnHeaderStyle.None)
				{
					if (columnHeaderStyle == ColumnHeaderStyle.Nonclickable)
					{
						createParams.Style |= 32768;
					}
				}
				else
				{
					createParams.Style |= 16384;
				}
				if (this.LabelEdit)
				{
					createParams.Style |= 512;
				}
				if (!this.LabelWrap)
				{
					createParams.Style |= 128;
				}
				if (!this.HideSelection)
				{
					createParams.Style |= 8;
				}
				if (!this.MultiSelect)
				{
					createParams.Style |= 4;
				}
				if (this.listItemSorter == null)
				{
					SortOrder sortOrder = this.sorting;
					if (sortOrder != SortOrder.Ascending)
					{
						if (sortOrder == SortOrder.Descending)
						{
							createParams.Style |= 32;
						}
					}
					else
					{
						createParams.Style |= 16;
					}
				}
				if (this.VirtualMode)
				{
					createParams.Style |= 4096;
				}
				if (this.viewStyle != View.Tile)
				{
					createParams.Style |= (int)this.viewStyle;
				}
				if (this.RightToLeft == RightToLeft.Yes && this.RightToLeftLayout)
				{
					createParams.ExStyle |= 4194304;
					createParams.ExStyle &= -28673;
				}
				return createParams;
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06002981 RID: 10625 RVA: 0x000C1579 File Offset: 0x000BF779
		internal ListViewGroup DefaultGroup
		{
			get
			{
				if (this.defaultGroup == null)
				{
					this.defaultGroup = new ListViewGroup(SR.GetString("ListViewGroupDefaultGroup", new object[]
					{
						"1"
					}));
				}
				return this.defaultGroup;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06002982 RID: 10626 RVA: 0x000C15AC File Offset: 0x000BF7AC
		protected override Size DefaultSize
		{
			get
			{
				return new Size(121, 97);
			}
		}

		/// <summary>Gets or sets a value indicating whether this control should redraw its surface using a secondary buffer to reduce or prevent flicker.</summary>
		/// <returns>
		///     <see langword="true" /> if the surface of the control should be drawn using double buffering; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x06002983 RID: 10627 RVA: 0x000A2CB2 File Offset: 0x000A0EB2
		// (set) Token: 0x06002984 RID: 10628 RVA: 0x000C15B7 File Offset: 0x000BF7B7
		protected override bool DoubleBuffered
		{
			get
			{
				return base.DoubleBuffered;
			}
			set
			{
				if (this.DoubleBuffered != value)
				{
					base.DoubleBuffered = value;
					this.UpdateExtendedStyles();
				}
			}
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06002985 RID: 10629 RVA: 0x000C15CF File Offset: 0x000BF7CF
		internal bool ExpectingMouseUp
		{
			get
			{
				return this.listViewState[1048576];
			}
		}

		/// <summary>Gets or sets the item in the control that currently has focus.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListViewItem" /> that represents the item that has focus, or <see langword="null" /> if no item has the focus in the <see cref="T:System.Windows.Forms.ListView" />.</returns>
		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06002986 RID: 10630 RVA: 0x000C15E4 File Offset: 0x000BF7E4
		// (set) Token: 0x06002987 RID: 10631 RVA: 0x000C161F File Offset: 0x000BF81F
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListViewFocusedItemDescr")]
		public ListViewItem FocusedItem
		{
			get
			{
				if (base.IsHandleCreated)
				{
					int num = (int)((long)base.SendMessage(4108, -1, 1));
					if (num > -1)
					{
						return this.Items[num];
					}
				}
				return null;
			}
			set
			{
				if (base.IsHandleCreated && value != null)
				{
					value.Focused = true;
				}
			}
		}

		/// <summary>Gets or sets the foreground color.</summary>
		/// <returns>The <see cref="T:System.Drawing.Color" /> that is the foreground color.</returns>
		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06002988 RID: 10632 RVA: 0x000201D0 File Offset: 0x0001E3D0
		// (set) Token: 0x06002989 RID: 10633 RVA: 0x000C1633 File Offset: 0x000BF833
		public override Color ForeColor
		{
			get
			{
				if (this.ShouldSerializeForeColor())
				{
					return base.ForeColor;
				}
				return SystemColors.WindowText;
			}
			set
			{
				base.ForeColor = value;
				if (base.IsHandleCreated)
				{
					base.SendMessage(4132, 0, ColorTranslator.ToWin32(this.ForeColor));
				}
			}
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x0600298A RID: 10634 RVA: 0x000C165C File Offset: 0x000BF85C
		// (set) Token: 0x0600298B RID: 10635 RVA: 0x000C166E File Offset: 0x000BF86E
		private bool FlipViewToLargeIconAndSmallIcon
		{
			get
			{
				return this.listViewState[268435456];
			}
			set
			{
				this.listViewState[268435456] = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether clicking an item selects all its subitems.</summary>
		/// <returns>
		///     <see langword="true" /> if clicking an item selects the item and all its subitems; <see langword="false" /> if clicking an item selects only the item itself. The default is <see langword="false" />.</returns>
		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x0600298C RID: 10636 RVA: 0x000C1681 File Offset: 0x000BF881
		// (set) Token: 0x0600298D RID: 10637 RVA: 0x000C1690 File Offset: 0x000BF890
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		[SRDescription("ListViewFullRowSelectDescr")]
		public bool FullRowSelect
		{
			get
			{
				return this.listViewState[16];
			}
			set
			{
				if (this.FullRowSelect != value)
				{
					this.listViewState[16] = value;
					this.UpdateExtendedStyles();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether grid lines appear between the rows and columns containing the items and subitems in the control.</summary>
		/// <returns>
		///     <see langword="true" /> if grid lines are drawn around items and subitems; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x0600298E RID: 10638 RVA: 0x000C16AF File Offset: 0x000BF8AF
		// (set) Token: 0x0600298F RID: 10639 RVA: 0x000C16BE File Offset: 0x000BF8BE
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		[SRDescription("ListViewGridLinesDescr")]
		public bool GridLines
		{
			get
			{
				return this.listViewState[32];
			}
			set
			{
				if (this.GridLines != value)
				{
					this.listViewState[32] = value;
					this.UpdateExtendedStyles();
				}
			}
		}

		/// <summary>Gets the collection of <see cref="T:System.Windows.Forms.ListViewGroup" /> objects assigned to the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListViewGroupCollection" /> that contains all the groups in the <see cref="T:System.Windows.Forms.ListView" /> control.</returns>
		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06002990 RID: 10640 RVA: 0x000C16DD File Offset: 0x000BF8DD
		[SRCategory("CatBehavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[Editor("System.Windows.Forms.Design.ListViewGroupCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("ListViewGroupsDescr")]
		[MergableProperty(false)]
		public ListViewGroupCollection Groups
		{
			get
			{
				if (this.groups == null)
				{
					this.groups = new ListViewGroupCollection(this);
				}
				return this.groups;
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06002991 RID: 10641 RVA: 0x000C16F9 File Offset: 0x000BF8F9
		internal bool GroupsEnabled
		{
			get
			{
				return this.ShowGroups && this.groups != null && this.groups.Count > 0 && this.ComctlSupportsVisualStyles && !this.VirtualMode;
			}
		}

		/// <summary>Gets or sets the column header style.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ColumnHeaderStyle" /> values. The default is <see cref="F:System.Windows.Forms.ColumnHeaderStyle.Clickable" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is not one of the <see cref="T:System.Windows.Forms.ColumnHeaderStyle" /> values. </exception>
		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x06002992 RID: 10642 RVA: 0x000C172C File Offset: 0x000BF92C
		// (set) Token: 0x06002993 RID: 10643 RVA: 0x000C1734 File Offset: 0x000BF934
		[SRCategory("CatBehavior")]
		[DefaultValue(ColumnHeaderStyle.Clickable)]
		[SRDescription("ListViewHeaderStyleDescr")]
		public ColumnHeaderStyle HeaderStyle
		{
			get
			{
				return this.headerStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ColumnHeaderStyle));
				}
				if (this.headerStyle != value)
				{
					this.headerStyle = value;
					if ((this.listViewState[8192] && value == ColumnHeaderStyle.Clickable) || (!this.listViewState[8192] && value == ColumnHeaderStyle.Nonclickable))
					{
						this.listViewState[8192] = !this.listViewState[8192];
						this.RecreateHandleInternal();
						return;
					}
					base.UpdateStyles();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the selected item in the control remains highlighted when the control loses focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the selected item does not appear highlighted when the control loses focus; <see langword="false" /> if the selected item still appears highlighted when the control loses focus. The default is <see langword="true" />.</returns>
		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x06002994 RID: 10644 RVA: 0x000C17D3 File Offset: 0x000BF9D3
		// (set) Token: 0x06002995 RID: 10645 RVA: 0x000C17E2 File Offset: 0x000BF9E2
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("ListViewHideSelectionDescr")]
		public bool HideSelection
		{
			get
			{
				return this.listViewState[64];
			}
			set
			{
				if (this.HideSelection != value)
				{
					this.listViewState[64] = value;
					base.UpdateStyles();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the text of an item or subitem has the appearance of a hyperlink when the mouse pointer passes over it.</summary>
		/// <returns>
		///     <see langword="true" /> if the item text has the appearance of a hyperlink when the mouse passes over it; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x06002996 RID: 10646 RVA: 0x000C1801 File Offset: 0x000BFA01
		// (set) Token: 0x06002997 RID: 10647 RVA: 0x000C1813 File Offset: 0x000BFA13
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListViewHotTrackingDescr")]
		public bool HotTracking
		{
			get
			{
				return this.listViewState[128];
			}
			set
			{
				if (this.HotTracking != value)
				{
					this.listViewState[128] = value;
					if (value)
					{
						this.HoverSelection = true;
						this.Activation = ItemActivation.OneClick;
					}
					this.UpdateExtendedStyles();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether an item is automatically selected when the mouse pointer remains over the item for a few seconds.</summary>
		/// <returns>
		///     <see langword="true" /> if an item is automatically selected when the mouse pointer hovers over it; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06002998 RID: 10648 RVA: 0x000C1846 File Offset: 0x000BFA46
		// (set) Token: 0x06002999 RID: 10649 RVA: 0x000C1858 File Offset: 0x000BFA58
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListViewHoverSelectDescr")]
		public bool HoverSelection
		{
			get
			{
				return this.listViewState[4096];
			}
			set
			{
				if (this.HoverSelection != value)
				{
					if (this.HotTracking && !value)
					{
						throw new ArgumentException(SR.GetString("ListViewHoverMustBeOnWhenHotTrackingIsOn"), "value");
					}
					this.listViewState[4096] = value;
					this.UpdateExtendedStyles();
				}
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x0600299A RID: 10650 RVA: 0x000C18A5 File Offset: 0x000BFAA5
		internal bool InsertingItemsNatively
		{
			get
			{
				return this.listViewState1[1];
			}
		}

		/// <summary>Gets an object used to indicate the expected drop location when an item is dragged within a <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListViewInsertionMark" /> object representing the insertion mark.</returns>
		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x0600299B RID: 10651 RVA: 0x000C18B3 File Offset: 0x000BFAB3
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListViewInsertionMarkDescr")]
		public ListViewInsertionMark InsertionMark
		{
			get
			{
				if (this.insertionMark == null)
				{
					this.insertionMark = new ListViewInsertionMark(this);
				}
				return this.insertionMark;
			}
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x0600299C RID: 10652 RVA: 0x000C18CF File Offset: 0x000BFACF
		// (set) Token: 0x0600299D RID: 10653 RVA: 0x000C18E1 File Offset: 0x000BFAE1
		private bool ItemCollectionChangedInMouseDown
		{
			get
			{
				return this.listViewState[134217728];
			}
			set
			{
				this.listViewState[134217728] = value;
			}
		}

		/// <summary>Gets a collection containing all items in the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> that contains all the items in the <see cref="T:System.Windows.Forms.ListView" /> control.</returns>
		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x0600299E RID: 10654 RVA: 0x000C18F4 File Offset: 0x000BFAF4
		[SRCategory("CatBehavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[Editor("System.Windows.Forms.Design.ListViewItemCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("ListViewItemsDescr")]
		[MergableProperty(false)]
		public ListView.ListViewItemCollection Items
		{
			get
			{
				return this.listItemCollection;
			}
		}

		/// <summary>Gets or sets a value indicating whether the user can edit the labels of items in the control.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can edit the labels of items at run time; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x0600299F RID: 10655 RVA: 0x000C18FC File Offset: 0x000BFAFC
		// (set) Token: 0x060029A0 RID: 10656 RVA: 0x000C190E File Offset: 0x000BFB0E
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListViewLabelEditDescr")]
		public bool LabelEdit
		{
			get
			{
				return this.listViewState[256];
			}
			set
			{
				if (this.LabelEdit != value)
				{
					this.listViewState[256] = value;
					base.UpdateStyles();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether item labels wrap when items are displayed in the control as icons.</summary>
		/// <returns>
		///     <see langword="true" /> if item labels wrap when items are displayed as icons; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x060029A1 RID: 10657 RVA: 0x000C1930 File Offset: 0x000BFB30
		// (set) Token: 0x060029A2 RID: 10658 RVA: 0x000C1942 File Offset: 0x000BFB42
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[Localizable(true)]
		[SRDescription("ListViewLabelWrapDescr")]
		public bool LabelWrap
		{
			get
			{
				return this.listViewState[512];
			}
			set
			{
				if (this.LabelWrap != value)
				{
					this.listViewState[512] = value;
					base.UpdateStyles();
				}
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ImageList" /> to use when displaying items as large icons in the control.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.ImageList" /> that contains the icons to use when the <see cref="P:System.Windows.Forms.ListView.View" /> property is set to <see cref="F:System.Windows.Forms.View.LargeIcon" />. The default is <see langword="null" />.</returns>
		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x060029A3 RID: 10659 RVA: 0x000C1964 File Offset: 0x000BFB64
		// (set) Token: 0x060029A4 RID: 10660 RVA: 0x000C196C File Offset: 0x000BFB6C
		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[SRDescription("ListViewLargeImageListDescr")]
		public ImageList LargeImageList
		{
			get
			{
				return this.imageListLarge;
			}
			set
			{
				if (value != this.imageListLarge)
				{
					EventHandler value2 = new EventHandler(this.LargeImageListRecreateHandle);
					EventHandler value3 = new EventHandler(this.DetachImageList);
					EventHandler value4 = new EventHandler(this.LargeImageListChangedHandle);
					if (this.imageListLarge != null)
					{
						this.imageListLarge.RecreateHandle -= value2;
						this.imageListLarge.Disposed -= value3;
						this.imageListLarge.ChangeHandle -= value4;
					}
					this.imageListLarge = value;
					if (value != null)
					{
						value.RecreateHandle += value2;
						value.Disposed += value3;
						value.ChangeHandle += value4;
					}
					if (base.IsHandleCreated)
					{
						base.SendMessage(4099, (IntPtr)0, (value == null) ? IntPtr.Zero : value.Handle);
						if (this.AutoArrange && !this.listViewState1[4])
						{
							this.UpdateListViewItemsLocations();
						}
					}
				}
			}
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x060029A5 RID: 10661 RVA: 0x000C1A3D File Offset: 0x000BFC3D
		// (set) Token: 0x060029A6 RID: 10662 RVA: 0x000C1A4F File Offset: 0x000BFC4F
		internal bool ListViewHandleDestroyed
		{
			get
			{
				return this.listViewState[16777216];
			}
			set
			{
				this.listViewState[16777216] = value;
			}
		}

		/// <summary>Gets or sets the sorting comparer for the control.</summary>
		/// <returns>An <see cref="T:System.Collections.IComparer" /> that represents the sorting comparer for the control.</returns>
		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x060029A7 RID: 10663 RVA: 0x000C1A62 File Offset: 0x000BFC62
		// (set) Token: 0x060029A8 RID: 10664 RVA: 0x000C1A6A File Offset: 0x000BFC6A
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListViewItemSorterDescr")]
		public IComparer ListViewItemSorter
		{
			get
			{
				return this.listItemSorter;
			}
			set
			{
				if (this.listItemSorter != value)
				{
					this.listItemSorter = value;
					if (!this.VirtualMode)
					{
						this.Sort();
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether multiple items can be selected.</summary>
		/// <returns>
		///     <see langword="true" /> if multiple items in the control can be selected at one time; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x060029A9 RID: 10665 RVA: 0x000C1A8A File Offset: 0x000BFC8A
		// (set) Token: 0x060029AA RID: 10666 RVA: 0x000C1A9C File Offset: 0x000BFC9C
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("ListViewMultiSelectDescr")]
		public bool MultiSelect
		{
			get
			{
				return this.listViewState[1024];
			}
			set
			{
				if (this.MultiSelect != value)
				{
					this.listViewState[1024] = value;
					base.UpdateStyles();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ListView" /> control is drawn by the operating system or by code that you provide.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ListView" /> control is drawn by code that you provide; <see langword="false" /> if the <see cref="T:System.Windows.Forms.ListView" /> control is drawn by the operating system. The default is <see langword="false" />.</returns>
		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x060029AB RID: 10667 RVA: 0x000C1ABE File Offset: 0x000BFCBE
		// (set) Token: 0x060029AC RID: 10668 RVA: 0x000C1ACC File Offset: 0x000BFCCC
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListViewOwnerDrawDescr")]
		public bool OwnerDraw
		{
			get
			{
				return this.listViewState[1];
			}
			set
			{
				if (this.OwnerDraw != value)
				{
					this.listViewState[1] = value;
					base.Invalidate(true);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the control is laid out from right to left.</summary>
		/// <returns>
		///     <see langword="true" /> to indicate the <see cref="T:System.Windows.Forms.ListView" /> control is laid out from right to left; otherwise, <see langword="false" />. </returns>
		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x060029AD RID: 10669 RVA: 0x000C1AEB File Offset: 0x000BFCEB
		// (set) Token: 0x060029AE RID: 10670 RVA: 0x000C1AF4 File Offset: 0x000BFCF4
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue(false)]
		[SRDescription("ControlRightToLeftLayoutDescr")]
		public virtual bool RightToLeftLayout
		{
			get
			{
				return this.rightToLeftLayout;
			}
			set
			{
				if (value != this.rightToLeftLayout)
				{
					this.rightToLeftLayout = value;
					using (new LayoutTransaction(this, this, PropertyNames.RightToLeftLayout))
					{
						this.OnRightToLeftLayoutChanged(EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ListView.RightToLeftLayout" /> property changes.</summary>
		// Token: 0x140001EE RID: 494
		// (add) Token: 0x060029AF RID: 10671 RVA: 0x000C1B48 File Offset: 0x000BFD48
		// (remove) Token: 0x060029B0 RID: 10672 RVA: 0x000C1B5B File Offset: 0x000BFD5B
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		public event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_RIGHTTOLEFTLAYOUTCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_RIGHTTOLEFTLAYOUTCHANGED, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether a scroll bar is added to the control when there is not enough room to display all items.</summary>
		/// <returns>
		///     <see langword="true" /> if scroll bars are added to the control when necessary to allow the user to see all the items; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x060029B1 RID: 10673 RVA: 0x000C1B6E File Offset: 0x000BFD6E
		// (set) Token: 0x060029B2 RID: 10674 RVA: 0x000C1B80 File Offset: 0x000BFD80
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("ListViewScrollableDescr")]
		public bool Scrollable
		{
			get
			{
				return this.listViewState[2048];
			}
			set
			{
				if (this.Scrollable != value)
				{
					this.listViewState[2048] = value;
					this.RecreateHandleInternal();
				}
			}
		}

		/// <summary>Gets the indexes of the selected items in the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" /> that contains the indexes of the selected items. If no items are currently selected, an empty <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" /> is returned.</returns>
		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x060029B3 RID: 10675 RVA: 0x000C1BA2 File Offset: 0x000BFDA2
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ListView.SelectedIndexCollection SelectedIndices
		{
			get
			{
				if (this.selectedIndexCollection == null)
				{
					this.selectedIndexCollection = new ListView.SelectedIndexCollection(this);
				}
				return this.selectedIndexCollection;
			}
		}

		/// <summary>Gets the items that are selected in the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListView.SelectedListViewItemCollection" /> that contains the items that are selected in the control. If no items are currently selected, an empty <see cref="T:System.Windows.Forms.ListView.SelectedListViewItemCollection" /> is returned.</returns>
		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x000C1BBE File Offset: 0x000BFDBE
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListViewSelectedItemsDescr")]
		public ListView.SelectedListViewItemCollection SelectedItems
		{
			get
			{
				if (this.selectedListViewItemCollection == null)
				{
					this.selectedListViewItemCollection = new ListView.SelectedListViewItemCollection(this);
				}
				return this.selectedListViewItemCollection;
			}
		}

		/// <summary>Gets or sets a value indicating whether items are displayed in groups.</summary>
		/// <returns>
		///     <see langword="true" /> to display items in groups; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x060029B5 RID: 10677 RVA: 0x000C1BDA File Offset: 0x000BFDDA
		// (set) Token: 0x060029B6 RID: 10678 RVA: 0x000C1BEC File Offset: 0x000BFDEC
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("ListViewShowGroupsDescr")]
		public bool ShowGroups
		{
			get
			{
				return this.listViewState[8388608];
			}
			set
			{
				if (value != this.ShowGroups)
				{
					this.listViewState[8388608] = value;
					if (base.IsHandleCreated)
					{
						this.UpdateGroupView();
					}
				}
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ImageList" /> to use when displaying items as small icons in the control.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.ImageList" /> that contains the icons to use when the <see cref="P:System.Windows.Forms.ListView.View" /> property is set to <see cref="F:System.Windows.Forms.View.SmallIcon" />. The default is <see langword="null" />.</returns>
		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x060029B7 RID: 10679 RVA: 0x000C1C16 File Offset: 0x000BFE16
		// (set) Token: 0x060029B8 RID: 10680 RVA: 0x000C1C20 File Offset: 0x000BFE20
		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[SRDescription("ListViewSmallImageListDescr")]
		public ImageList SmallImageList
		{
			get
			{
				return this.imageListSmall;
			}
			set
			{
				if (this.imageListSmall != value)
				{
					EventHandler value2 = new EventHandler(this.SmallImageListRecreateHandle);
					EventHandler value3 = new EventHandler(this.DetachImageList);
					if (this.imageListSmall != null)
					{
						this.imageListSmall.RecreateHandle -= value2;
						this.imageListSmall.Disposed -= value3;
					}
					this.imageListSmall = value;
					if (value != null)
					{
						value.RecreateHandle += value2;
						value.Disposed += value3;
					}
					if (base.IsHandleCreated)
					{
						base.SendMessage(4099, (IntPtr)1, (value == null) ? IntPtr.Zero : value.Handle);
						if (this.View == View.SmallIcon)
						{
							this.View = View.LargeIcon;
							this.View = View.SmallIcon;
						}
						else if (!this.listViewState1[4])
						{
							this.UpdateListViewItemsLocations();
						}
						if (this.View == View.Details)
						{
							base.Invalidate(true);
						}
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether ToolTips are shown for the <see cref="T:System.Windows.Forms.ListViewItem" /> objects contained in the <see cref="T:System.Windows.Forms.ListView" />.</summary>
		/// <returns>
		///     <see langword="true" /> if <see cref="T:System.Windows.Forms.ListViewItem" /> ToolTips should be shown; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x060029B9 RID: 10681 RVA: 0x000C1CF2 File Offset: 0x000BFEF2
		// (set) Token: 0x060029BA RID: 10682 RVA: 0x000C1D04 File Offset: 0x000BFF04
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListViewShowItemToolTipsDescr")]
		public bool ShowItemToolTips
		{
			get
			{
				return this.listViewState[32768];
			}
			set
			{
				if (this.ShowItemToolTips != value)
				{
					this.listViewState[32768] = value;
					this.RecreateHandleInternal();
				}
			}
		}

		/// <summary>Gets or sets the sort order for items in the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.SortOrder" /> values. The default is <see cref="F:System.Windows.Forms.SortOrder.None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is not one of the <see cref="T:System.Windows.Forms.SortOrder" /> values. </exception>
		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x060029BB RID: 10683 RVA: 0x000C1D26 File Offset: 0x000BFF26
		// (set) Token: 0x060029BC RID: 10684 RVA: 0x000C1D30 File Offset: 0x000BFF30
		[SRCategory("CatBehavior")]
		[DefaultValue(SortOrder.None)]
		[SRDescription("ListViewSortingDescr")]
		public SortOrder Sorting
		{
			get
			{
				return this.sorting;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SortOrder));
				}
				if (this.sorting != value)
				{
					this.sorting = value;
					if (this.View == View.LargeIcon || this.View == View.SmallIcon)
					{
						if (this.listItemSorter == null)
						{
							this.listItemSorter = new ListView.IconComparer(this.sorting);
						}
						else if (this.listItemSorter is ListView.IconComparer)
						{
							((ListView.IconComparer)this.listItemSorter).SortOrder = this.sorting;
						}
					}
					else if (value == SortOrder.None)
					{
						this.listItemSorter = null;
					}
					if (value == SortOrder.None)
					{
						base.UpdateStyles();
						return;
					}
					this.RecreateHandleInternal();
				}
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ImageList" /> associated with application-defined states in the control.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.ImageList" /> that contains a set of state images that can be used to indicate an application-defined state of an item. The default is <see langword="null" />.</returns>
		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x060029BD RID: 10685 RVA: 0x000C1DDE File Offset: 0x000BFFDE
		// (set) Token: 0x060029BE RID: 10686 RVA: 0x000C1DE8 File Offset: 0x000BFFE8
		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[SRDescription("ListViewStateImageListDescr")]
		public ImageList StateImageList
		{
			get
			{
				return this.imageListState;
			}
			set
			{
				if (this.UseCompatibleStateImageBehavior)
				{
					if (this.imageListState != value)
					{
						EventHandler value2 = new EventHandler(this.StateImageListRecreateHandle);
						EventHandler value3 = new EventHandler(this.DetachImageList);
						if (this.imageListState != null)
						{
							this.imageListState.RecreateHandle -= value2;
							this.imageListState.Disposed -= value3;
						}
						this.imageListState = value;
						if (value != null)
						{
							value.RecreateHandle += value2;
							value.Disposed += value3;
						}
						if (base.IsHandleCreated)
						{
							base.SendMessage(4099, 2, (value == null) ? IntPtr.Zero : value.Handle);
							return;
						}
					}
				}
				else if (this.imageListState != value)
				{
					EventHandler value4 = new EventHandler(this.StateImageListRecreateHandle);
					EventHandler value5 = new EventHandler(this.DetachImageList);
					if (this.imageListState != null)
					{
						this.imageListState.RecreateHandle -= value4;
						this.imageListState.Disposed -= value5;
					}
					if (base.IsHandleCreated && this.imageListState != null && this.CheckBoxes)
					{
						base.SendMessage(4099, 2, IntPtr.Zero);
					}
					this.imageListState = value;
					if (value != null)
					{
						value.RecreateHandle += value4;
						value.Disposed += value5;
					}
					if (base.IsHandleCreated)
					{
						if (this.CheckBoxes)
						{
							this.RecreateHandleInternal();
						}
						else
						{
							base.SendMessage(4099, 2, (this.imageListState == null || this.imageListState.Images.Count == 0) ? IntPtr.Zero : this.imageListState.Handle);
						}
						if (!this.listViewState1[4])
						{
							this.UpdateListViewItemsLocations();
						}
					}
				}
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The text to display in the <see cref="T:System.Windows.Forms.ListView" /> control.</returns>
		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x060029BF RID: 10687 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x060029C0 RID: 10688 RVA: 0x0001BFAD File Offset: 0x0001A1AD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ListView.Text" /> property changes.</summary>
		// Token: 0x140001EF RID: 495
		// (add) Token: 0x060029C1 RID: 10689 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x060029C2 RID: 10690 RVA: 0x0003E43E File Offset: 0x0003C63E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		/// <summary>Gets or sets the size of the tiles shown in tile view.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that contains the new tile size.</returns>
		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x060029C3 RID: 10691 RVA: 0x000C1F74 File Offset: 0x000C0174
		// (set) Token: 0x060029C4 RID: 10692 RVA: 0x000C1FE4 File Offset: 0x000C01E4
		[SRCategory("CatAppearance")]
		[Browsable(true)]
		[SRDescription("ListViewTileSizeDescr")]
		public Size TileSize
		{
			get
			{
				if (!this.tileSize.IsEmpty)
				{
					return this.tileSize;
				}
				if (base.IsHandleCreated)
				{
					NativeMethods.LVTILEVIEWINFO lvtileviewinfo = new NativeMethods.LVTILEVIEWINFO();
					lvtileviewinfo.dwMask = 1;
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4259, 0, lvtileviewinfo);
					return new Size(lvtileviewinfo.sizeTile.cx, lvtileviewinfo.sizeTile.cy);
				}
				return Size.Empty;
			}
			set
			{
				if (this.tileSize != value)
				{
					if (value.IsEmpty || value.Height <= 0 || value.Width <= 0)
					{
						throw new ArgumentOutOfRangeException("TileSize", SR.GetString("ListViewTileSizeMustBePositive"));
					}
					this.tileSize = value;
					if (base.IsHandleCreated)
					{
						NativeMethods.LVTILEVIEWINFO lvtileviewinfo = new NativeMethods.LVTILEVIEWINFO();
						lvtileviewinfo.dwMask = 1;
						lvtileviewinfo.dwFlags = 3;
						lvtileviewinfo.sizeTile = new NativeMethods.SIZE(this.tileSize.Width, this.tileSize.Height);
						bool flag = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4258, 0, lvtileviewinfo);
						if (this.AutoArrange)
						{
							this.UpdateListViewItemsLocations();
						}
					}
				}
			}
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x000C209F File Offset: 0x000C029F
		private bool ShouldSerializeTileSize()
		{
			return !this.tileSize.Equals(Size.Empty);
		}

		/// <summary>Gets or sets the first visible item in the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListViewItem" /> that represents the first visible item in the control.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Forms.ListView.View" /> property is set to <see cref="F:System.Windows.Forms.View.LargeIcon" />,  <see cref="F:System.Windows.Forms.View.SmallIcon" />, or <see cref="F:System.Windows.Forms.View.Tile" />.</exception>
		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x060029C6 RID: 10694 RVA: 0x000C20C0 File Offset: 0x000C02C0
		// (set) Token: 0x060029C7 RID: 10695 RVA: 0x000C2164 File Offset: 0x000C0364
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListViewTopItemDescr")]
		public ListViewItem TopItem
		{
			get
			{
				if (this.viewStyle == View.LargeIcon || this.viewStyle == View.SmallIcon || this.viewStyle == View.Tile)
				{
					throw new InvalidOperationException(SR.GetString("ListViewGetTopItem"));
				}
				if (!base.IsHandleCreated)
				{
					if (this.Items.Count > 0)
					{
						return this.Items[0];
					}
					return null;
				}
				else
				{
					this.topIndex = (int)((long)base.SendMessage(4135, 0, 0));
					if (this.topIndex >= 0 && this.topIndex < this.Items.Count)
					{
						return this.Items[this.topIndex];
					}
					return null;
				}
			}
			set
			{
				if (this.viewStyle == View.LargeIcon || this.viewStyle == View.SmallIcon || this.viewStyle == View.Tile)
				{
					throw new InvalidOperationException(SR.GetString("ListViewSetTopItem"));
				}
				if (value == null)
				{
					return;
				}
				if (value.ListView != this)
				{
					return;
				}
				if (!base.IsHandleCreated)
				{
					this.CreateHandle();
				}
				if (value == this.TopItem)
				{
					return;
				}
				this.EnsureVisible(value.Index);
				ListViewItem topItem = this.TopItem;
				if (topItem == null && this.topIndex == this.Items.Count)
				{
					if (this.Scrollable)
					{
						this.EnsureVisible(0);
						this.Scroll(0, value.Index);
					}
					return;
				}
				if (value.Index == topItem.Index)
				{
					return;
				}
				if (this.Scrollable)
				{
					this.Scroll(topItem.Index, value.Index);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ListView" /> uses state image behavior that is compatible with the .NET Framework 1.1 or the .NET Framework 2.0.</summary>
		/// <returns>
		///     <see langword="true" /> if the state image behavior is compatible with the .NET Framework 1.1; <see langword="false" /> if the behavior is compatible with the .NET Framework 2.0. The default is <see langword="true" />.</returns>
		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x060029C8 RID: 10696 RVA: 0x000C2232 File Offset: 0x000C0432
		// (set) Token: 0x060029C9 RID: 10697 RVA: 0x000C2240 File Offset: 0x000C0440
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DefaultValue(true)]
		public bool UseCompatibleStateImageBehavior
		{
			get
			{
				return this.listViewState1[8];
			}
			set
			{
				this.listViewState1[8] = value;
			}
		}

		/// <summary>Gets or sets how items are displayed in the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.View" /> values. The default is <see cref="F:System.Windows.Forms.View.LargeIcon" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is not one of the <see cref="T:System.Windows.Forms.View" /> values. </exception>
		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x060029CA RID: 10698 RVA: 0x000C224F File Offset: 0x000C044F
		// (set) Token: 0x060029CB RID: 10699 RVA: 0x000C2258 File Offset: 0x000C0458
		[SRCategory("CatAppearance")]
		[DefaultValue(View.LargeIcon)]
		[SRDescription("ListViewViewDescr")]
		public View View
		{
			get
			{
				return this.viewStyle;
			}
			set
			{
				if (value == View.Tile && this.CheckBoxes)
				{
					throw new NotSupportedException(SR.GetString("ListViewTileViewDoesNotSupportCheckBoxes"));
				}
				this.FlipViewToLargeIconAndSmallIcon = false;
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(View));
				}
				if (value == View.Tile && this.VirtualMode)
				{
					throw new NotSupportedException(SR.GetString("ListViewCantSetViewToTileViewInVirtualMode"));
				}
				if (this.viewStyle != value)
				{
					this.viewStyle = value;
					if (base.IsHandleCreated && this.ComctlSupportsVisualStyles)
					{
						base.SendMessage(4238, (int)this.viewStyle, 0);
						this.UpdateGroupView();
						if (this.viewStyle == View.Tile)
						{
							this.UpdateTileView();
						}
					}
					else
					{
						base.UpdateStyles();
					}
					this.UpdateListViewItemsLocations();
				}
			}
		}

		/// <summary>Gets or sets the number of <see cref="T:System.Windows.Forms.ListViewItem" /> objects contained in the list when in virtual mode.</summary>
		/// <returns>The number of <see cref="T:System.Windows.Forms.ListViewItem" /> objects contained in the <see cref="T:System.Windows.Forms.ListView" /> when in virtual mode.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <see cref="P:System.Windows.Forms.ListView.VirtualListSize" /> is set to a value less than 0.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.ListView.VirtualMode" /> is set to <see langword="true" />, <see cref="P:System.Windows.Forms.ListView.VirtualListSize" /> is greater than 0, and <see cref="E:System.Windows.Forms.ListView.RetrieveVirtualItem" /> is not handled.</exception>
		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x060029CC RID: 10700 RVA: 0x000C2320 File Offset: 0x000C0520
		// (set) Token: 0x060029CD RID: 10701 RVA: 0x000C2328 File Offset: 0x000C0528
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ListViewVirtualListSizeDescr")]
		public int VirtualListSize
		{
			get
			{
				return this.virtualListSize;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("ListViewVirtualListSizeInvalidArgument", new object[]
					{
						"value",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (value == this.virtualListSize)
				{
					return;
				}
				bool flag = base.IsHandleCreated && this.VirtualMode && this.View == View.Details && !base.DesignMode;
				int num = -1;
				if (flag)
				{
					num = (int)((long)base.SendMessage(4135, 0, 0));
				}
				this.virtualListSize = value;
				if (base.IsHandleCreated && this.VirtualMode && !base.DesignMode)
				{
					base.SendMessage(4143, this.virtualListSize, 0);
				}
				if (flag)
				{
					num = Math.Min(num, this.VirtualListSize - 1);
					if (num > 0)
					{
						ListViewItem topItem = this.Items[num];
						this.TopItem = topItem;
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether you have provided your own data-management operations for the <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
		/// <returns>
		///     <see langword="true" /> if <see cref="T:System.Windows.Forms.ListView" /> uses data-management operations that you provide; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.ListView.VirtualMode" /> is set to <see langword="true" /> and one of the following conditions exist:
		///             <see cref="P:System.Windows.Forms.ListView.VirtualListSize" /> is greater than 0 and <see cref="E:System.Windows.Forms.ListView.RetrieveVirtualItem" /> is not handled.-or-
		///             <see cref="P:System.Windows.Forms.ListView.Items" />, <see cref="P:System.Windows.Forms.ListView.CheckedItems" />, or <see cref="P:System.Windows.Forms.ListView.SelectedItems" /> contains items.-or-Edits are made to <see cref="P:System.Windows.Forms.ListView.Items" />.</exception>
		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x060029CE RID: 10702 RVA: 0x000C240B File Offset: 0x000C060B
		// (set) Token: 0x060029CF RID: 10703 RVA: 0x000C2420 File Offset: 0x000C0620
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ListViewVirtualModeDescr")]
		public bool VirtualMode
		{
			get
			{
				return this.listViewState[33554432];
			}
			set
			{
				if (value == this.VirtualMode)
				{
					return;
				}
				if (value && this.Items.Count > 0)
				{
					throw new InvalidOperationException(SR.GetString("ListViewVirtualListViewRequiresNoItems"));
				}
				if (value && this.CheckedItems.Count > 0)
				{
					throw new InvalidOperationException(SR.GetString("ListViewVirtualListViewRequiresNoCheckedItems"));
				}
				if (value && this.SelectedItems.Count > 0)
				{
					throw new InvalidOperationException(SR.GetString("ListViewVirtualListViewRequiresNoSelectedItems"));
				}
				if (value && this.View == View.Tile)
				{
					throw new NotSupportedException(SR.GetString("ListViewCantSetVirtualModeWhenInTileView"));
				}
				this.listViewState[33554432] = value;
				this.RecreateHandleInternal();
			}
		}

		/// <summary>Occurs when the label for an item is edited by the user.</summary>
		// Token: 0x140001F0 RID: 496
		// (add) Token: 0x060029D0 RID: 10704 RVA: 0x000C24CD File Offset: 0x000C06CD
		// (remove) Token: 0x060029D1 RID: 10705 RVA: 0x000C24E6 File Offset: 0x000C06E6
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewAfterLabelEditDescr")]
		public event LabelEditEventHandler AfterLabelEdit
		{
			add
			{
				this.onAfterLabelEdit = (LabelEditEventHandler)Delegate.Combine(this.onAfterLabelEdit, value);
			}
			remove
			{
				this.onAfterLabelEdit = (LabelEditEventHandler)Delegate.Remove(this.onAfterLabelEdit, value);
			}
		}

		/// <summary>Occurs when the user starts editing the label of an item.</summary>
		// Token: 0x140001F1 RID: 497
		// (add) Token: 0x060029D2 RID: 10706 RVA: 0x000C24FF File Offset: 0x000C06FF
		// (remove) Token: 0x060029D3 RID: 10707 RVA: 0x000C2518 File Offset: 0x000C0718
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewBeforeLabelEditDescr")]
		public event LabelEditEventHandler BeforeLabelEdit
		{
			add
			{
				this.onBeforeLabelEdit = (LabelEditEventHandler)Delegate.Combine(this.onBeforeLabelEdit, value);
			}
			remove
			{
				this.onBeforeLabelEdit = (LabelEditEventHandler)Delegate.Remove(this.onBeforeLabelEdit, value);
			}
		}

		/// <summary>Occurs when the contents of the display area for a <see cref="T:System.Windows.Forms.ListView" /> in virtual mode has changed, and the <see cref="T:System.Windows.Forms.ListView" /> determines that a new range of items is needed.</summary>
		// Token: 0x140001F2 RID: 498
		// (add) Token: 0x060029D4 RID: 10708 RVA: 0x000C2531 File Offset: 0x000C0731
		// (remove) Token: 0x060029D5 RID: 10709 RVA: 0x000C2544 File Offset: 0x000C0744
		[SRCategory("CatAction")]
		[SRDescription("ListViewCacheVirtualItemsEventDescr")]
		public event CacheVirtualItemsEventHandler CacheVirtualItems
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_CACHEVIRTUALITEMS, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_CACHEVIRTUALITEMS, value);
			}
		}

		/// <summary>Occurs when the user clicks a column header within the list view control.</summary>
		// Token: 0x140001F3 RID: 499
		// (add) Token: 0x060029D6 RID: 10710 RVA: 0x000C2557 File Offset: 0x000C0757
		// (remove) Token: 0x060029D7 RID: 10711 RVA: 0x000C2570 File Offset: 0x000C0770
		[SRCategory("CatAction")]
		[SRDescription("ListViewColumnClickDescr")]
		public event ColumnClickEventHandler ColumnClick
		{
			add
			{
				this.onColumnClick = (ColumnClickEventHandler)Delegate.Combine(this.onColumnClick, value);
			}
			remove
			{
				this.onColumnClick = (ColumnClickEventHandler)Delegate.Remove(this.onColumnClick, value);
			}
		}

		/// <summary>Occurs when the column header order is changed.</summary>
		// Token: 0x140001F4 RID: 500
		// (add) Token: 0x060029D8 RID: 10712 RVA: 0x000C2589 File Offset: 0x000C0789
		// (remove) Token: 0x060029D9 RID: 10713 RVA: 0x000C259C File Offset: 0x000C079C
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListViewColumnReorderedDscr")]
		public event ColumnReorderedEventHandler ColumnReordered
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_COLUMNREORDERED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_COLUMNREORDERED, value);
			}
		}

		/// <summary>Occurs after the width of a column is successfully changed.</summary>
		// Token: 0x140001F5 RID: 501
		// (add) Token: 0x060029DA RID: 10714 RVA: 0x000C25AF File Offset: 0x000C07AF
		// (remove) Token: 0x060029DB RID: 10715 RVA: 0x000C25C2 File Offset: 0x000C07C2
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListViewColumnWidthChangedDscr")]
		public event ColumnWidthChangedEventHandler ColumnWidthChanged
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_COLUMNWIDTHCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_COLUMNWIDTHCHANGED, value);
			}
		}

		/// <summary>Occurs when the width of a column is changing.</summary>
		// Token: 0x140001F6 RID: 502
		// (add) Token: 0x060029DC RID: 10716 RVA: 0x000C25D5 File Offset: 0x000C07D5
		// (remove) Token: 0x060029DD RID: 10717 RVA: 0x000C25E8 File Offset: 0x000C07E8
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListViewColumnWidthChangingDscr")]
		public event ColumnWidthChangingEventHandler ColumnWidthChanging
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_COLUMNWIDTHCHANGING, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_COLUMNWIDTHCHANGING, value);
			}
		}

		/// <summary>Occurs when the details view of a <see cref="T:System.Windows.Forms.ListView" /> is drawn and the <see cref="P:System.Windows.Forms.ListView.OwnerDraw" /> property is set to <see langword="true" />. </summary>
		// Token: 0x140001F7 RID: 503
		// (add) Token: 0x060029DE RID: 10718 RVA: 0x000C25FB File Offset: 0x000C07FB
		// (remove) Token: 0x060029DF RID: 10719 RVA: 0x000C260E File Offset: 0x000C080E
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewDrawColumnHeaderEventDescr")]
		public event DrawListViewColumnHeaderEventHandler DrawColumnHeader
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_DRAWCOLUMNHEADER, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_DRAWCOLUMNHEADER, value);
			}
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Forms.ListView" /> is drawn and the <see cref="P:System.Windows.Forms.ListView.OwnerDraw" /> property is set to <see langword="true" />.</summary>
		// Token: 0x140001F8 RID: 504
		// (add) Token: 0x060029E0 RID: 10720 RVA: 0x000C2621 File Offset: 0x000C0821
		// (remove) Token: 0x060029E1 RID: 10721 RVA: 0x000C2634 File Offset: 0x000C0834
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewDrawItemEventDescr")]
		public event DrawListViewItemEventHandler DrawItem
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_DRAWITEM, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_DRAWITEM, value);
			}
		}

		/// <summary>Occurs when the details view of a <see cref="T:System.Windows.Forms.ListView" /> is drawn and the <see cref="P:System.Windows.Forms.ListView.OwnerDraw" /> property is set to <see langword="true" />.</summary>
		// Token: 0x140001F9 RID: 505
		// (add) Token: 0x060029E2 RID: 10722 RVA: 0x000C2647 File Offset: 0x000C0847
		// (remove) Token: 0x060029E3 RID: 10723 RVA: 0x000C265A File Offset: 0x000C085A
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewDrawSubItemEventDescr")]
		public event DrawListViewSubItemEventHandler DrawSubItem
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_DRAWSUBITEM, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_DRAWSUBITEM, value);
			}
		}

		/// <summary>Occurs when an item is activated.</summary>
		// Token: 0x140001FA RID: 506
		// (add) Token: 0x060029E4 RID: 10724 RVA: 0x000C266D File Offset: 0x000C086D
		// (remove) Token: 0x060029E5 RID: 10725 RVA: 0x000C2686 File Offset: 0x000C0886
		[SRCategory("CatAction")]
		[SRDescription("ListViewItemClickDescr")]
		public event EventHandler ItemActivate
		{
			add
			{
				this.onItemActivate = (EventHandler)Delegate.Combine(this.onItemActivate, value);
			}
			remove
			{
				this.onItemActivate = (EventHandler)Delegate.Remove(this.onItemActivate, value);
			}
		}

		/// <summary>Occurs when the check state of an item changes.</summary>
		// Token: 0x140001FB RID: 507
		// (add) Token: 0x060029E6 RID: 10726 RVA: 0x000C269F File Offset: 0x000C089F
		// (remove) Token: 0x060029E7 RID: 10727 RVA: 0x000C26B8 File Offset: 0x000C08B8
		[SRCategory("CatBehavior")]
		[SRDescription("CheckedListBoxItemCheckDescr")]
		public event ItemCheckEventHandler ItemCheck
		{
			add
			{
				this.onItemCheck = (ItemCheckEventHandler)Delegate.Combine(this.onItemCheck, value);
			}
			remove
			{
				this.onItemCheck = (ItemCheckEventHandler)Delegate.Remove(this.onItemCheck, value);
			}
		}

		/// <summary>Occurs when the checked state of an item changes.</summary>
		// Token: 0x140001FC RID: 508
		// (add) Token: 0x060029E8 RID: 10728 RVA: 0x000C26D1 File Offset: 0x000C08D1
		// (remove) Token: 0x060029E9 RID: 10729 RVA: 0x000C26EA File Offset: 0x000C08EA
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewItemCheckedDescr")]
		public event ItemCheckedEventHandler ItemChecked
		{
			add
			{
				this.onItemChecked = (ItemCheckedEventHandler)Delegate.Combine(this.onItemChecked, value);
			}
			remove
			{
				this.onItemChecked = (ItemCheckedEventHandler)Delegate.Remove(this.onItemChecked, value);
			}
		}

		/// <summary>Occurs when the user begins dragging an item.</summary>
		// Token: 0x140001FD RID: 509
		// (add) Token: 0x060029EA RID: 10730 RVA: 0x000C2703 File Offset: 0x000C0903
		// (remove) Token: 0x060029EB RID: 10731 RVA: 0x000C271C File Offset: 0x000C091C
		[SRCategory("CatAction")]
		[SRDescription("ListViewItemDragDescr")]
		public event ItemDragEventHandler ItemDrag
		{
			add
			{
				this.onItemDrag = (ItemDragEventHandler)Delegate.Combine(this.onItemDrag, value);
			}
			remove
			{
				this.onItemDrag = (ItemDragEventHandler)Delegate.Remove(this.onItemDrag, value);
			}
		}

		/// <summary>Occurs when the mouse hovers over an item.</summary>
		// Token: 0x140001FE RID: 510
		// (add) Token: 0x060029EC RID: 10732 RVA: 0x000C2735 File Offset: 0x000C0935
		// (remove) Token: 0x060029ED RID: 10733 RVA: 0x000C274E File Offset: 0x000C094E
		[SRCategory("CatAction")]
		[SRDescription("ListViewItemMouseHoverDescr")]
		public event ListViewItemMouseHoverEventHandler ItemMouseHover
		{
			add
			{
				this.onItemMouseHover = (ListViewItemMouseHoverEventHandler)Delegate.Combine(this.onItemMouseHover, value);
			}
			remove
			{
				this.onItemMouseHover = (ListViewItemMouseHoverEventHandler)Delegate.Remove(this.onItemMouseHover, value);
			}
		}

		/// <summary>Occurs when the selection state of an item changes.</summary>
		// Token: 0x140001FF RID: 511
		// (add) Token: 0x060029EE RID: 10734 RVA: 0x000C2767 File Offset: 0x000C0967
		// (remove) Token: 0x060029EF RID: 10735 RVA: 0x000C277A File Offset: 0x000C097A
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewItemSelectionChangedDescr")]
		public event ListViewItemSelectionChangedEventHandler ItemSelectionChanged
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_ITEMSELECTIONCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_ITEMSELECTIONCHANGED, value);
			}
		}

		/// <summary>Gets or sets the space between the <see cref="T:System.Windows.Forms.ListView" /> control and its contents.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Padding" /> that specifies the space between the <see cref="T:System.Windows.Forms.ListView" /> control and its contents.</returns>
		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x060029F0 RID: 10736 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x060029F1 RID: 10737 RVA: 0x000204A2 File Offset: 0x0001E6A2
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ListView.Padding" /> property changes.</summary>
		// Token: 0x14000200 RID: 512
		// (add) Token: 0x060029F2 RID: 10738 RVA: 0x000204AB File Offset: 0x0001E6AB
		// (remove) Token: 0x060029F3 RID: 10739 RVA: 0x000204B4 File Offset: 0x0001E6B4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler PaddingChanged
		{
			add
			{
				base.PaddingChanged += value;
			}
			remove
			{
				base.PaddingChanged -= value;
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.ListView" /> control is painted.</summary>
		// Token: 0x14000201 RID: 513
		// (add) Token: 0x060029F4 RID: 10740 RVA: 0x00020D37 File Offset: 0x0001EF37
		// (remove) Token: 0x060029F5 RID: 10741 RVA: 0x00020D40 File Offset: 0x0001EF40
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event PaintEventHandler Paint
		{
			add
			{
				base.Paint += value;
			}
			remove
			{
				base.Paint -= value;
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.ListView" /> is in virtual mode and requires a <see cref="T:System.Windows.Forms.ListViewItem" />.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Forms.RetrieveVirtualItemEventArgs.Item" /> property is not set to an item when the <see cref="E:System.Windows.Forms.ListView.RetrieveVirtualItem" /> event is handled.  </exception>
		// Token: 0x14000202 RID: 514
		// (add) Token: 0x060029F6 RID: 10742 RVA: 0x000C278D File Offset: 0x000C098D
		// (remove) Token: 0x060029F7 RID: 10743 RVA: 0x000C27A0 File Offset: 0x000C09A0
		[SRCategory("CatAction")]
		[SRDescription("ListViewRetrieveVirtualItemEventDescr")]
		public event RetrieveVirtualItemEventHandler RetrieveVirtualItem
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_RETRIEVEVIRTUALITEM, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_RETRIEVEVIRTUALITEM, value);
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.ListView" /> is in virtual mode and a search is taking place.</summary>
		// Token: 0x14000203 RID: 515
		// (add) Token: 0x060029F8 RID: 10744 RVA: 0x000C27B3 File Offset: 0x000C09B3
		// (remove) Token: 0x060029F9 RID: 10745 RVA: 0x000C27C6 File Offset: 0x000C09C6
		[SRCategory("CatAction")]
		[SRDescription("ListViewSearchForVirtualItemDescr")]
		public event SearchForVirtualItemEventHandler SearchForVirtualItem
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_SEARCHFORVIRTUALITEM, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_SEARCHFORVIRTUALITEM, value);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ListView.SelectedIndices" /> collection changes. </summary>
		// Token: 0x14000204 RID: 516
		// (add) Token: 0x060029FA RID: 10746 RVA: 0x000C27D9 File Offset: 0x000C09D9
		// (remove) Token: 0x060029FB RID: 10747 RVA: 0x000C27EC File Offset: 0x000C09EC
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewSelectedIndexChangedDescr")]
		public event EventHandler SelectedIndexChanged
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_SELECTEDINDEXCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_SELECTEDINDEXCHANGED, value);
			}
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Forms.ListView" /> is in virtual mode and the selection state of a range of items has changed.</summary>
		// Token: 0x14000205 RID: 517
		// (add) Token: 0x060029FC RID: 10748 RVA: 0x000C27FF File Offset: 0x000C09FF
		// (remove) Token: 0x060029FD RID: 10749 RVA: 0x000C2812 File Offset: 0x000C0A12
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewVirtualItemsSelectionRangeChangedDescr")]
		public event ListViewVirtualItemsSelectionRangeChangedEventHandler VirtualItemsSelectionRangeChanged
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_VIRTUALITEMSSELECTIONRANGECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_VIRTUALITEMSSELECTIONRANGECHANGED, value);
			}
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x000C2828 File Offset: 0x000C0A28
		private void ApplyUpdateCachedItems()
		{
			ArrayList arrayList = (ArrayList)base.Properties.GetObject(ListView.PropDelayedUpdateItems);
			if (arrayList != null)
			{
				base.Properties.SetObject(ListView.PropDelayedUpdateItems, null);
				ListViewItem[] array = (ListViewItem[])arrayList.ToArray(typeof(ListViewItem));
				if (array.Length != 0)
				{
					this.InsertItems(this.itemCount, array, false);
				}
			}
		}

		/// <summary>Arranges items in the control when they are displayed as icons with a specified alignment setting.</summary>
		/// <param name="value">One of the <see cref="T:System.Windows.Forms.ListViewAlignment" /> values. </param>
		/// <exception cref="T:System.ArgumentException">The value specified in the <paramref name="value" /> parameter is not a member of the <see cref="T:System.Windows.Forms.ListViewAlignment" /> enumeration. </exception>
		// Token: 0x060029FF RID: 10751 RVA: 0x000C2888 File Offset: 0x000C0A88
		public void ArrangeIcons(ListViewAlignment value)
		{
			if (this.viewStyle != View.SmallIcon)
			{
				return;
			}
			int num = (int)value;
			if (num <= 2 || num == 5)
			{
				if (base.IsHandleCreated)
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 4118, (int)value, 0);
				}
				if (!this.VirtualMode && this.sorting != SortOrder.None)
				{
					this.Sort();
				}
				return;
			}
			throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
			{
				"value",
				value.ToString()
			}));
		}

		/// <summary>Arranges items in the control when they are displayed as icons based on the value of the <see cref="P:System.Windows.Forms.ListView.Alignment" /> property.</summary>
		// Token: 0x06002A00 RID: 10752 RVA: 0x000C2910 File Offset: 0x000C0B10
		public void ArrangeIcons()
		{
			this.ArrangeIcons(ListViewAlignment.Default);
		}

		/// <summary>Resizes the width of the columns as indicated by the resize style.</summary>
		/// <param name="headerAutoResize">One of the <see cref="T:System.Windows.Forms.ColumnHeaderAutoResizeStyle" /> values.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="M:System.Windows.Forms.ListView.AutoResizeColumn(System.Int32,System.Windows.Forms.ColumnHeaderAutoResizeStyle)" /> is called with a value other than <see cref="F:System.Windows.Forms.ColumnHeaderAutoResizeStyle.None" /> when <see cref="P:System.Windows.Forms.ListView.View" /> is not set to <see cref="F:System.Windows.Forms.View.Details" />.</exception>
		// Token: 0x06002A01 RID: 10753 RVA: 0x000C2919 File Offset: 0x000C0B19
		public void AutoResizeColumns(ColumnHeaderAutoResizeStyle headerAutoResize)
		{
			if (!base.IsHandleCreated)
			{
				this.CreateHandle();
			}
			this.UpdateColumnWidths(headerAutoResize);
		}

		/// <summary>Resizes the width of the given column as indicated by the resize style.</summary>
		/// <param name="columnIndex">The zero-based index of the column to resize.</param>
		/// <param name="headerAutoResize">One of the <see cref="T:System.Windows.Forms.ColumnHeaderAutoResizeStyle" /> values.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="columnIndex" /> is greater than 0 when <see cref="P:System.Windows.Forms.ListView.Columns" /> is <see langword="null" />-or-
		///         <paramref name="columnIndex" /> is less than 0 or greater than the number of columns set.</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="headerAutoResize" /> is not a member of the <see cref="T:System.Windows.Forms.ColumnHeaderAutoResizeStyle" /> enumeration.</exception>
		// Token: 0x06002A02 RID: 10754 RVA: 0x000C2930 File Offset: 0x000C0B30
		public void AutoResizeColumn(int columnIndex, ColumnHeaderAutoResizeStyle headerAutoResize)
		{
			if (!base.IsHandleCreated)
			{
				this.CreateHandle();
			}
			this.SetColumnWidth(columnIndex, headerAutoResize);
		}

		/// <summary>Prevents the control from drawing until the <see cref="M:System.Windows.Forms.ListView.EndUpdate" /> method is called.</summary>
		// Token: 0x06002A03 RID: 10755 RVA: 0x000C2948 File Offset: 0x000C0B48
		public void BeginUpdate()
		{
			base.BeginUpdateInternal();
			int num = this.updateCounter;
			this.updateCounter = num + 1;
			if (num == 0 && base.Properties.GetObject(ListView.PropDelayedUpdateItems) == null)
			{
				base.Properties.SetObject(ListView.PropDelayedUpdateItems, new ArrayList());
			}
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x000C2998 File Offset: 0x000C0B98
		internal void CacheSelectedStateForItem(ListViewItem lvi, bool selected)
		{
			if (selected)
			{
				if (this.savedSelectedItems == null)
				{
					this.savedSelectedItems = new List<ListViewItem>();
				}
				if (!this.savedSelectedItems.Contains(lvi))
				{
					this.savedSelectedItems.Add(lvi);
					return;
				}
			}
			else if (this.savedSelectedItems != null && this.savedSelectedItems.Contains(lvi))
			{
				this.savedSelectedItems.Remove(lvi);
			}
		}

		// Token: 0x06002A05 RID: 10757 RVA: 0x000C29FC File Offset: 0x000C0BFC
		private void CleanPreviousBackgroundImageFiles()
		{
			if (this.bkImgFileNames == null)
			{
				return;
			}
			FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.Unrestricted);
			fileIOPermission.Assert();
			try
			{
				for (int i = 0; i <= this.bkImgFileNamesCount; i++)
				{
					FileInfo fileInfo = new FileInfo(this.bkImgFileNames[i]);
					if (fileInfo.Exists)
					{
						try
						{
							fileInfo.Delete();
						}
						catch (IOException)
						{
						}
					}
				}
			}
			finally
			{
				PermissionSet.RevertAssert();
			}
			this.bkImgFileNames = null;
			this.bkImgFileNamesCount = -1;
		}

		/// <summary>Removes all items and columns from the control.</summary>
		// Token: 0x06002A06 RID: 10758 RVA: 0x000C2A84 File Offset: 0x000C0C84
		public void Clear()
		{
			this.Items.Clear();
			this.Columns.Clear();
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x000C2A9C File Offset: 0x000C0C9C
		private int CompareFunc(IntPtr lparam1, IntPtr lparam2, IntPtr lparamSort)
		{
			if (this.listItemSorter != null)
			{
				return this.listItemSorter.Compare(this.listItemsTable[(int)lparam1], this.listItemsTable[(int)lparam2]);
			}
			return 0;
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x000C2AEC File Offset: 0x000C0CEC
		private int CompensateColumnHeaderResize(Message m, bool columnResizeCancelled)
		{
			if (this.ComctlSupportsVisualStyles && this.View == View.Details && !columnResizeCancelled && this.Items.Count > 0)
			{
				NativeMethods.NMHEADER nmheader = (NativeMethods.NMHEADER)m.GetLParam(typeof(NativeMethods.NMHEADER));
				return this.CompensateColumnHeaderResize(nmheader.iItem, columnResizeCancelled);
			}
			return 0;
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x000C2B44 File Offset: 0x000C0D44
		private int CompensateColumnHeaderResize(int columnIndex, bool columnResizeCancelled)
		{
			if (this.ComctlSupportsVisualStyles && this.View == View.Details && !columnResizeCancelled && this.Items.Count > 0 && columnIndex == 0)
			{
				ColumnHeader columnHeader = (this.columnHeaders != null && this.columnHeaders.Length != 0) ? this.columnHeaders[0] : null;
				if (columnHeader != null)
				{
					if (this.SmallImageList == null)
					{
						return 2;
					}
					bool flag = true;
					for (int i = 0; i < this.Items.Count; i++)
					{
						if (this.Items[i].ImageIndexer.ActualIndex > -1)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return 18;
					}
				}
			}
			return 0;
		}

		/// <summary>Creates a handle for the control.</summary>
		// Token: 0x06002A0A RID: 10762 RVA: 0x000C2BE0 File Offset: 0x000C0DE0
		protected override void CreateHandle()
		{
			if (!base.RecreatingHandle)
			{
				IntPtr userCookie = UnsafeNativeMethods.ThemingScope.Activate();
				try
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 1
					});
				}
				finally
				{
					UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
				}
			}
			base.CreateHandle();
			if (this.BackgroundImage != null)
			{
				this.SetBackgroundImage();
			}
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x000C2C40 File Offset: 0x000C0E40
		private unsafe void CustomDraw(ref Message m)
		{
			bool flag = false;
			bool flag2 = false;
			try
			{
				NativeMethods.NMLVCUSTOMDRAW* ptr = (NativeMethods.NMLVCUSTOMDRAW*)((void*)m.LParam);
				int dwDrawStage = ptr->nmcd.dwDrawStage;
				if (dwDrawStage != 1)
				{
					int num;
					Rectangle itemRectOrEmpty;
					if (dwDrawStage != 65537)
					{
						if (dwDrawStage != 196609)
						{
							m.Result = (IntPtr)0;
							return;
						}
					}
					else
					{
						num = (int)ptr->nmcd.dwItemSpec;
						itemRectOrEmpty = this.GetItemRectOrEmpty(num);
						if (!base.ClientRectangle.IntersectsWith(itemRectOrEmpty))
						{
							return;
						}
						if (this.OwnerDraw)
						{
							Graphics graphics = Graphics.FromHdcInternal(ptr->nmcd.hdc);
							DrawListViewItemEventArgs drawListViewItemEventArgs = null;
							try
							{
								drawListViewItemEventArgs = new DrawListViewItemEventArgs(graphics, this.Items[(int)ptr->nmcd.dwItemSpec], itemRectOrEmpty, (int)ptr->nmcd.dwItemSpec, (ListViewItemStates)ptr->nmcd.uItemState);
								this.OnDrawItem(drawListViewItemEventArgs);
							}
							finally
							{
								graphics.Dispose();
							}
							flag2 = drawListViewItemEventArgs.DrawDefault;
							if (this.viewStyle == View.Details)
							{
								m.Result = (IntPtr)32;
							}
							else if (!drawListViewItemEventArgs.DrawDefault)
							{
								m.Result = (IntPtr)4;
							}
							if (!drawListViewItemEventArgs.DrawDefault)
							{
								return;
							}
						}
						if (this.viewStyle == View.Details || this.viewStyle == View.Tile)
						{
							m.Result = (IntPtr)34;
							flag = true;
						}
					}
					num = (int)ptr->nmcd.dwItemSpec;
					itemRectOrEmpty = this.GetItemRectOrEmpty(num);
					if (base.ClientRectangle.IntersectsWith(itemRectOrEmpty))
					{
						if (this.OwnerDraw && !flag2)
						{
							Graphics graphics2 = Graphics.FromHdcInternal(ptr->nmcd.hdc);
							bool flag3 = true;
							try
							{
								if (ptr->iSubItem < this.Items[num].SubItems.Count)
								{
									Rectangle subItemRect = this.GetSubItemRect(num, ptr->iSubItem);
									if (ptr->iSubItem == 0 && this.Items[num].SubItems.Count > 1)
									{
										subItemRect.Width = this.columnHeaders[0].Width;
									}
									if (base.ClientRectangle.IntersectsWith(subItemRect))
									{
										DrawListViewSubItemEventArgs drawListViewSubItemEventArgs = new DrawListViewSubItemEventArgs(graphics2, subItemRect, this.Items[num], this.Items[num].SubItems[ptr->iSubItem], num, ptr->iSubItem, this.columnHeaders[ptr->iSubItem], (ListViewItemStates)ptr->nmcd.uItemState);
										this.OnDrawSubItem(drawListViewSubItemEventArgs);
										flag3 = !drawListViewSubItemEventArgs.DrawDefault;
									}
								}
							}
							finally
							{
								graphics2.Dispose();
							}
							if (flag3)
							{
								m.Result = (IntPtr)4;
								return;
							}
						}
						ListViewItem listViewItem = this.Items[(int)ptr->nmcd.dwItemSpec];
						if (flag && listViewItem.UseItemStyleForSubItems)
						{
							m.Result = (IntPtr)2;
						}
						int num2 = ptr->nmcd.uItemState;
						if (!this.HideSelection)
						{
							int itemState = this.GetItemState((int)ptr->nmcd.dwItemSpec);
							if ((itemState & 2) == 0)
							{
								num2 &= -2;
							}
						}
						int num3 = ((ptr->nmcd.dwDrawStage & 131072) != 0) ? ptr->iSubItem : 0;
						Font font = null;
						Color color = Color.Empty;
						Color color2 = Color.Empty;
						bool flag4 = false;
						bool flag5 = false;
						if (listViewItem != null && num3 < listViewItem.SubItems.Count)
						{
							flag4 = true;
							if (num3 == 0 && (num2 & 64) != 0 && this.HotTracking)
							{
								flag5 = true;
								font = new Font(listViewItem.SubItems[0].Font, FontStyle.Underline);
							}
							else
							{
								font = listViewItem.SubItems[num3].Font;
							}
							if (num3 > 0 || (num2 & 71) == 0)
							{
								color = listViewItem.SubItems[num3].ForeColor;
								color2 = listViewItem.SubItems[num3].BackColor;
							}
						}
						Color c = Color.Empty;
						Color c2 = Color.Empty;
						if (flag4)
						{
							c = color;
							c2 = color2;
						}
						bool flag6 = true;
						if (!base.Enabled)
						{
							flag6 = false;
						}
						else if ((this.activation == ItemActivation.OneClick || this.activation == ItemActivation.TwoClick) && (num2 & 71) != 0)
						{
							flag6 = false;
						}
						if (flag6)
						{
							if (!flag4 || c.IsEmpty)
							{
								ptr->clrText = ColorTranslator.ToWin32(this.odCacheForeColor);
							}
							else
							{
								ptr->clrText = ColorTranslator.ToWin32(c);
							}
							if (ptr->clrText == ColorTranslator.ToWin32(SystemColors.HotTrack))
							{
								int num4 = 0;
								bool flag7 = false;
								int num5 = 16711680;
								do
								{
									int num6 = ptr->clrText & num5;
									if (num6 != 0 || num5 == 255)
									{
										int num7 = 16 - num4;
										if (num6 == num5)
										{
											num6 = (num6 >> num7) - 1 << num7;
										}
										else
										{
											num6 = (num6 >> num7) + 1 << num7;
										}
										ptr->clrText = ((ptr->clrText & ~num5) | num6);
										flag7 = true;
									}
									else
									{
										num5 >>= 8;
										num4 += 8;
									}
								}
								while (!flag7);
							}
							if (!flag4 || c2.IsEmpty)
							{
								ptr->clrTextBk = ColorTranslator.ToWin32(this.odCacheBackColor);
							}
							else
							{
								ptr->clrTextBk = ColorTranslator.ToWin32(c2);
							}
						}
						if (!flag4 || font == null)
						{
							if (this.odCacheFont != null)
							{
								SafeNativeMethods.SelectObject(new HandleRef(ptr->nmcd, ptr->nmcd.hdc), new HandleRef(null, this.odCacheFontHandle));
							}
						}
						else
						{
							if (this.odCacheFontHandleWrapper != null)
							{
								this.odCacheFontHandleWrapper.Dispose();
							}
							this.odCacheFontHandleWrapper = new Control.FontHandleWrapper(font);
							SafeNativeMethods.SelectObject(new HandleRef(ptr->nmcd, ptr->nmcd.hdc), new HandleRef(this.odCacheFontHandleWrapper, this.odCacheFontHandleWrapper.Handle));
						}
						if (!flag)
						{
							m.Result = (IntPtr)2;
						}
						if (flag5)
						{
							font.Dispose();
						}
					}
				}
				else if (this.OwnerDraw)
				{
					m.Result = (IntPtr)32;
				}
				else
				{
					m.Result = (IntPtr)34;
					this.odCacheBackColor = this.BackColor;
					this.odCacheForeColor = this.ForeColor;
					this.odCacheFont = this.Font;
					this.odCacheFontHandle = base.FontHandle;
					if (ptr->dwItemType == 1)
					{
						if (this.odCacheFontHandleWrapper != null)
						{
							this.odCacheFontHandleWrapper.Dispose();
						}
						this.odCacheFont = new Font(this.odCacheFont, FontStyle.Bold);
						this.odCacheFontHandleWrapper = new Control.FontHandleWrapper(this.odCacheFont);
						this.odCacheFontHandle = this.odCacheFontHandleWrapper.Handle;
						SafeNativeMethods.SelectObject(new HandleRef(ptr->nmcd, ptr->nmcd.hdc), new HandleRef(this.odCacheFontHandleWrapper, this.odCacheFontHandleWrapper.Handle));
						m.Result = (IntPtr)2;
					}
				}
			}
			catch (Exception ex)
			{
				m.Result = (IntPtr)0;
			}
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x000C3380 File Offset: 0x000C1580
		private void DeleteFileName(string fileName)
		{
			if (!string.IsNullOrEmpty(fileName))
			{
				FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.Unrestricted);
				fileIOPermission.Assert();
				try
				{
					FileInfo fileInfo = new FileInfo(fileName);
					if (fileInfo.Exists)
					{
						try
						{
							fileInfo.Delete();
						}
						catch (IOException)
						{
						}
					}
				}
				finally
				{
					PermissionSet.RevertAssert();
				}
			}
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x000C33E0 File Offset: 0x000C15E0
		private void DestroyLVGROUP(NativeMethods.LVGROUP lvgroup)
		{
			if (lvgroup.pszHeader != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(lvgroup.pszHeader);
			}
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x000C3400 File Offset: 0x000C1600
		private void DetachImageList(object sender, EventArgs e)
		{
			this.listViewState1[4] = true;
			try
			{
				if (sender == this.imageListSmall)
				{
					this.SmallImageList = null;
				}
				if (sender == this.imageListLarge)
				{
					this.LargeImageList = null;
				}
				if (sender == this.imageListState)
				{
					this.StateImageList = null;
				}
			}
			finally
			{
				this.listViewState1[4] = false;
			}
			this.UpdateListViewItemsLocations();
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.ListView" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x06002A0F RID: 10767 RVA: 0x000C3470 File Offset: 0x000C1670
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.imageListSmall != null)
				{
					this.imageListSmall.Disposed -= this.DetachImageList;
					this.imageListSmall = null;
				}
				if (this.imageListLarge != null)
				{
					this.imageListLarge.Disposed -= this.DetachImageList;
					this.imageListLarge = null;
				}
				if (this.imageListState != null)
				{
					this.imageListState.Disposed -= this.DetachImageList;
					this.imageListState = null;
				}
				if (this.columnHeaders != null)
				{
					for (int i = this.columnHeaders.Length - 1; i >= 0; i--)
					{
						this.columnHeaders[i].OwnerListview = null;
						this.columnHeaders[i].Dispose();
					}
					this.columnHeaders = null;
				}
				this.Items.Clear();
				if (this.odCacheFontHandleWrapper != null)
				{
					this.odCacheFontHandleWrapper.Dispose();
					this.odCacheFontHandleWrapper = null;
				}
				if (!string.IsNullOrEmpty(this.backgroundImageFileName) || this.bkImgFileNames != null)
				{
					FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.Unrestricted);
					fileIOPermission.Assert();
					try
					{
						if (!string.IsNullOrEmpty(this.backgroundImageFileName))
						{
							FileInfo fileInfo = new FileInfo(this.backgroundImageFileName);
							try
							{
								fileInfo.Delete();
							}
							catch (IOException)
							{
							}
							this.backgroundImageFileName = string.Empty;
						}
						for (int j = 0; j <= this.bkImgFileNamesCount; j++)
						{
							FileInfo fileInfo = new FileInfo(this.bkImgFileNames[j]);
							try
							{
								fileInfo.Delete();
							}
							catch (IOException)
							{
							}
						}
						this.bkImgFileNames = null;
						this.bkImgFileNamesCount = -1;
					}
					finally
					{
						PermissionSet.RevertAssert();
					}
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>Resumes drawing of the list view control after drawing is suspended by the <see cref="M:System.Windows.Forms.ListView.BeginUpdate" /> method.</summary>
		// Token: 0x06002A10 RID: 10768 RVA: 0x000C361C File Offset: 0x000C181C
		public void EndUpdate()
		{
			int num = this.updateCounter - 1;
			this.updateCounter = num;
			if (num == 0 && base.Properties.GetObject(ListView.PropDelayedUpdateItems) != null)
			{
				this.ApplyUpdateCachedItems();
			}
			base.EndUpdateInternal();
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x000C365C File Offset: 0x000C185C
		private void EnsureDefaultGroup()
		{
			if (base.IsHandleCreated && this.ComctlSupportsVisualStyles && this.GroupsEnabled && base.SendMessage(4257, this.DefaultGroup.ID, 0) == IntPtr.Zero)
			{
				this.UpdateGroupView();
				this.InsertGroupNative(0, this.DefaultGroup);
			}
		}

		/// <summary>Ensures that the specified item is visible within the control, scrolling the contents of the control if necessary.</summary>
		/// <param name="index">The zero-based index of the item to scroll into view. </param>
		// Token: 0x06002A12 RID: 10770 RVA: 0x000C36B8 File Offset: 0x000C18B8
		public void EnsureVisible(int index)
		{
			if (index < 0 || index >= this.Items.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (base.IsHandleCreated)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4115, index, 0);
			}
		}

		/// <summary>Finds the first <see cref="T:System.Windows.Forms.ListViewItem" /> that begins with the specified text value.</summary>
		/// <param name="text">The text to search for.</param>
		/// <returns>The first <see cref="T:System.Windows.Forms.ListViewItem" /> that begins with the specified text value.</returns>
		// Token: 0x06002A13 RID: 10771 RVA: 0x000C372A File Offset: 0x000C192A
		public ListViewItem FindItemWithText(string text)
		{
			if (this.Items.Count == 0)
			{
				return null;
			}
			return this.FindItemWithText(text, true, 0, true);
		}

		/// <summary>Finds the first <see cref="T:System.Windows.Forms.ListViewItem" /> or <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" />, if indicated, that begins with the specified text value. The search starts at the specified index.</summary>
		/// <param name="text">The text to search for.</param>
		/// <param name="includeSubItemsInSearch">
		///       <see langword="true" /> to include subitems in the search; otherwise, <see langword="false" />. </param>
		/// <param name="startIndex">The index of the item at which to start the search.</param>
		/// <returns>The first <see cref="T:System.Windows.Forms.ListViewItem" /> that begins with the specified text value.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="startIndex" /> is less 0 or more than the number items in the <see cref="T:System.Windows.Forms.ListView" />. </exception>
		// Token: 0x06002A14 RID: 10772 RVA: 0x000C3745 File Offset: 0x000C1945
		public ListViewItem FindItemWithText(string text, bool includeSubItemsInSearch, int startIndex)
		{
			return this.FindItemWithText(text, includeSubItemsInSearch, startIndex, true);
		}

		/// <summary>Finds the first <see cref="T:System.Windows.Forms.ListViewItem" /> or <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" />, if indicated, that begins with the specified text value. The search starts at the specified index.</summary>
		/// <param name="text">The text to search for.</param>
		/// <param name="includeSubItemsInSearch">
		///       <see langword="true" /> to include subitems in the search; otherwise, <see langword="false" />. </param>
		/// <param name="startIndex">The index of the item at which to start the search.</param>
		/// <param name="isPrefixSearch">
		///       <see langword="true" /> to allow partial matches; otherwise, <see langword="false" />.</param>
		/// <returns>The first <see cref="T:System.Windows.Forms.ListViewItem" /> that begins with the specified text value.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="startIndex" /> is less than 0 or more than the number of items in the <see cref="T:System.Windows.Forms.ListView" />. </exception>
		// Token: 0x06002A15 RID: 10773 RVA: 0x000C3754 File Offset: 0x000C1954
		public ListViewItem FindItemWithText(string text, bool includeSubItemsInSearch, int startIndex, bool isPrefixSearch)
		{
			if (startIndex < 0 || startIndex >= this.Items.Count)
			{
				throw new ArgumentOutOfRangeException("startIndex", SR.GetString("InvalidArgument", new object[]
				{
					"startIndex",
					startIndex.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return this.FindItem(true, text, isPrefixSearch, new Point(0, 0), SearchDirectionHint.Down, startIndex, includeSubItemsInSearch);
		}

		/// <summary>Finds the next item from the given point, searching in the specified direction</summary>
		/// <param name="dir">One of the <see cref="T:System.Windows.Forms.SearchDirectionHint" /> values.</param>
		/// <param name="point">The point at which to begin searching.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> that is closest to the given point, searching in the specified direction.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.ListView.View" /> is set to a value other than <see cref="F:System.Windows.Forms.View.SmallIcon" /> or <see cref="F:System.Windows.Forms.View.LargeIcon" />. </exception>
		// Token: 0x06002A16 RID: 10774 RVA: 0x000C37BA File Offset: 0x000C19BA
		public ListViewItem FindNearestItem(SearchDirectionHint dir, Point point)
		{
			return this.FindNearestItem(dir, point.X, point.Y);
		}

		/// <summary>Finds the next item from the given x- and y-coordinates, searching in the specified direction. </summary>
		/// <param name="searchDirection">One of the <see cref="T:System.Windows.Forms.SearchDirectionHint" /> values.</param>
		/// <param name="x">The x-coordinate for the point at which to begin searching.</param>
		/// <param name="y">The y-coordinate for the point at which to begin searching.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> that is closest to the given coordinates, searching in the specified direction.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.ListView.View" /> is set to a value other than <see cref="F:System.Windows.Forms.View.SmallIcon" /> or <see cref="F:System.Windows.Forms.View.LargeIcon" />. </exception>
		// Token: 0x06002A17 RID: 10775 RVA: 0x000C37D4 File Offset: 0x000C19D4
		public ListViewItem FindNearestItem(SearchDirectionHint searchDirection, int x, int y)
		{
			if (this.View != View.SmallIcon && this.View != View.LargeIcon)
			{
				throw new InvalidOperationException(SR.GetString("ListViewFindNearestItemWorksOnlyInIconView"));
			}
			if (searchDirection < SearchDirectionHint.Left || searchDirection > SearchDirectionHint.Down)
			{
				throw new ArgumentOutOfRangeException("searchDirection", SR.GetString("InvalidArgument", new object[]
				{
					"searchDirection",
					searchDirection.ToString()
				}));
			}
			ListViewItem itemAt = this.GetItemAt(x, y);
			if (itemAt != null)
			{
				Rectangle bounds = itemAt.Bounds;
				Rectangle itemRect = this.GetItemRect(itemAt.Index, ItemBoundsPortion.Icon);
				switch (searchDirection)
				{
				case SearchDirectionHint.Left:
					x = Math.Max(bounds.Left, itemRect.Left) - 1;
					break;
				case SearchDirectionHint.Up:
					y = Math.Max(bounds.Top, itemRect.Top) - 1;
					break;
				case SearchDirectionHint.Right:
					x = Math.Max(bounds.Left, itemRect.Left) + 1;
					break;
				case SearchDirectionHint.Down:
					y = Math.Max(bounds.Top, itemRect.Top) + 1;
					break;
				}
			}
			return this.FindItem(false, string.Empty, false, new Point(x, y), searchDirection, -1, false);
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x000C38F8 File Offset: 0x000C1AF8
		private ListViewItem FindItem(bool isTextSearch, string text, bool isPrefixSearch, Point pt, SearchDirectionHint dir, int startIndex, bool includeSubItemsInSearch)
		{
			if (this.Items.Count == 0)
			{
				return null;
			}
			if (!base.IsHandleCreated)
			{
				this.CreateHandle();
			}
			if (this.VirtualMode)
			{
				SearchForVirtualItemEventArgs searchForVirtualItemEventArgs = new SearchForVirtualItemEventArgs(isTextSearch, isPrefixSearch, includeSubItemsInSearch, text, pt, dir, startIndex);
				this.OnSearchForVirtualItem(searchForVirtualItemEventArgs);
				if (searchForVirtualItemEventArgs.Index != -1)
				{
					return this.Items[searchForVirtualItemEventArgs.Index];
				}
				return null;
			}
			else
			{
				NativeMethods.LVFINDINFO lvfindinfo = default(NativeMethods.LVFINDINFO);
				if (isTextSearch)
				{
					lvfindinfo.flags = 2;
					lvfindinfo.flags |= (isPrefixSearch ? 8 : 0);
					lvfindinfo.psz = text;
				}
				else
				{
					lvfindinfo.flags = 64;
					lvfindinfo.ptX = pt.X;
					lvfindinfo.ptY = pt.Y;
					lvfindinfo.vkDirection = (int)dir;
				}
				lvfindinfo.lParam = IntPtr.Zero;
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_FINDITEM, startIndex - 1, ref lvfindinfo);
				if (num >= 0)
				{
					return this.Items[num];
				}
				if (isTextSearch && includeSubItemsInSearch)
				{
					for (int i = startIndex; i < this.Items.Count; i++)
					{
						ListViewItem listViewItem = this.Items[i];
						for (int j = 0; j < listViewItem.SubItems.Count; j++)
						{
							ListViewItem.ListViewSubItem listViewSubItem = listViewItem.SubItems[j];
							if (string.Equals(text, listViewSubItem.Text, StringComparison.OrdinalIgnoreCase))
							{
								return listViewItem;
							}
							if (isPrefixSearch && CultureInfo.CurrentCulture.CompareInfo.IsPrefix(listViewSubItem.Text, text, CompareOptions.IgnoreCase))
							{
								return listViewItem;
							}
						}
					}
					return null;
				}
				return null;
			}
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x000C3A8C File Offset: 0x000C1C8C
		private void ForceCheckBoxUpdate()
		{
			if (this.CheckBoxes && base.IsHandleCreated)
			{
				base.SendMessage(4150, 4, 0);
				base.SendMessage(4150, 4, 4);
				if (this.AutoArrange)
				{
					this.ArrangeIcons(this.Alignment);
				}
			}
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x000C3ADC File Offset: 0x000C1CDC
		private string GenerateRandomName()
		{
			Bitmap bitmap = new Bitmap(this.BackgroundImage);
			int num = 0;
			try
			{
				num = (int)((long)bitmap.GetHicon());
			}
			catch
			{
				bitmap.Dispose();
			}
			Random random;
			if (num == 0)
			{
				random = new Random((int)DateTime.Now.Ticks);
			}
			else
			{
				random = new Random(num);
			}
			return random.Next().ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x000C3B54 File Offset: 0x000C1D54
		private int GenerateUniqueID()
		{
			int num = this.nextID;
			this.nextID = num + 1;
			int num2 = num;
			if (num2 == -1)
			{
				num2 = 0;
				this.nextID = 1;
			}
			return num2;
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x000C3B84 File Offset: 0x000C1D84
		internal int GetDisplayIndex(ListViewItem item, int lastIndex)
		{
			this.ApplyUpdateCachedItems();
			if (base.IsHandleCreated && !this.ListViewHandleDestroyed)
			{
				NativeMethods.LVFINDINFO lvfindinfo = default(NativeMethods.LVFINDINFO);
				lvfindinfo.lParam = (IntPtr)item.ID;
				lvfindinfo.flags = 1;
				int num = -1;
				if (lastIndex != -1)
				{
					num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_FINDITEM, lastIndex - 1, ref lvfindinfo);
				}
				if (num == -1)
				{
					num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_FINDITEM, -1, ref lvfindinfo);
				}
				return num;
			}
			int num2 = 0;
			foreach (object obj in this.listItemsArray)
			{
				if (obj == item)
				{
					return num2;
				}
				num2++;
			}
			return -1;
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x000C3C70 File Offset: 0x000C1E70
		internal int GetColumnIndex(ColumnHeader ch)
		{
			if (this.columnHeaders == null)
			{
				return -1;
			}
			for (int i = 0; i < this.columnHeaders.Length; i++)
			{
				if (this.columnHeaders[i] == ch)
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>Retrieves the item at the specified location.</summary>
		/// <param name="x">The x-coordinate of the location to search for an item (expressed in client coordinates). </param>
		/// <param name="y">The y-coordinate of the location to search for an item (expressed in client coordinates). </param>
		/// <returns>A <see cref="T:System.Windows.Forms.ListViewItem" /> that represents the item at the specified position. If there is no item at the specified location, the method returns <see langword="null" />.</returns>
		// Token: 0x06002A1E RID: 10782 RVA: 0x000C3CA8 File Offset: 0x000C1EA8
		public ListViewItem GetItemAt(int x, int y)
		{
			NativeMethods.LVHITTESTINFO lvhittestinfo = new NativeMethods.LVHITTESTINFO();
			lvhittestinfo.pt_x = x;
			lvhittestinfo.pt_y = y;
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4114, 0, lvhittestinfo);
			ListViewItem result = null;
			if (num >= 0 && (lvhittestinfo.flags & 14) != 0)
			{
				result = this.Items[num];
			}
			return result;
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x000C3D06 File Offset: 0x000C1F06
		internal int GetNativeGroupId(ListViewItem item)
		{
			item.UpdateGroupFromName();
			if (item.Group != null && this.Groups.Contains(item.Group))
			{
				return item.Group.ID;
			}
			this.EnsureDefaultGroup();
			return this.DefaultGroup.ID;
		}

		// Token: 0x06002A20 RID: 10784 RVA: 0x000C3D48 File Offset: 0x000C1F48
		internal void GetSubItemAt(int x, int y, out int iItem, out int iSubItem)
		{
			NativeMethods.LVHITTESTINFO lvhittestinfo = new NativeMethods.LVHITTESTINFO();
			lvhittestinfo.pt_x = x;
			lvhittestinfo.pt_y = y;
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4153, 0, lvhittestinfo);
			if (num > -1)
			{
				iItem = lvhittestinfo.iItem;
				iSubItem = lvhittestinfo.iSubItem;
				return;
			}
			iItem = -1;
			iSubItem = -1;
		}

		// Token: 0x06002A21 RID: 10785 RVA: 0x000C3DA4 File Offset: 0x000C1FA4
		internal Point GetItemPosition(int index)
		{
			NativeMethods.POINT point = new NativeMethods.POINT();
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4112, index, point);
			return new Point(point.x, point.y);
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x000C3DE1 File Offset: 0x000C1FE1
		internal int GetItemState(int index)
		{
			return this.GetItemState(index, 65295);
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x000C3DF0 File Offset: 0x000C1FF0
		internal int GetItemState(int index, int mask)
		{
			if (index < 0 || (this.VirtualMode && index >= this.VirtualListSize) || (!this.VirtualMode && index >= this.itemCount))
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return (int)((long)base.SendMessage(4140, index, mask));
		}

		/// <summary>Retrieves the bounding rectangle for a specific item within the list view control.</summary>
		/// <param name="index">The zero-based index of the item within the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> whose bounding rectangle you want to return. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounding rectangle of the specified <see cref="T:System.Windows.Forms.ListViewItem" />.</returns>
		// Token: 0x06002A24 RID: 10788 RVA: 0x000C3E68 File Offset: 0x000C2068
		public Rectangle GetItemRect(int index)
		{
			return this.GetItemRect(index, ItemBoundsPortion.Entire);
		}

		/// <summary>Retrieves the specified portion of the bounding rectangle for a specific item within the list view control.</summary>
		/// <param name="index">The zero-based index of the item within the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> whose bounding rectangle you want to return. </param>
		/// <param name="portion">One of the <see cref="T:System.Windows.Forms.ItemBoundsPortion" /> values that represents a portion of the <see cref="T:System.Windows.Forms.ListViewItem" /> for which to retrieve the bounding rectangle. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounding rectangle for the specified portion of the specified <see cref="T:System.Windows.Forms.ListViewItem" />.</returns>
		// Token: 0x06002A25 RID: 10789 RVA: 0x000C3E74 File Offset: 0x000C2074
		public Rectangle GetItemRect(int index, ItemBoundsPortion portion)
		{
			if (index < 0 || index >= this.Items.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (!ClientUtils.IsEnumValid(portion, (int)portion, 0, 3))
			{
				throw new InvalidEnumArgumentException("portion", (int)portion, typeof(ItemBoundsPortion));
			}
			if (this.View == View.Details && this.Columns.Count == 0)
			{
				return Rectangle.Empty;
			}
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			rect.left = (int)portion;
			if ((int)((long)base.SendMessage(4110, index, ref rect)) == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x000C3F78 File Offset: 0x000C2178
		private Rectangle GetItemRectOrEmpty(int index)
		{
			if (index < 0 || index >= this.Items.Count)
			{
				return Rectangle.Empty;
			}
			if (this.View == View.Details && this.Columns.Count == 0)
			{
				return Rectangle.Empty;
			}
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			rect.left = 0;
			if ((int)((long)base.SendMessage(4110, index, ref rect)) == 0)
			{
				return Rectangle.Empty;
			}
			return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x000C4004 File Offset: 0x000C2204
		private NativeMethods.LVGROUP GetLVGROUP(ListViewGroup group)
		{
			NativeMethods.LVGROUP lvgroup = new NativeMethods.LVGROUP();
			lvgroup.mask = 25U;
			string header = group.Header;
			lvgroup.pszHeader = Marshal.StringToHGlobalAuto(header);
			lvgroup.cchHeader = header.Length;
			lvgroup.iGroupId = group.ID;
			switch (group.HeaderAlignment)
			{
			case HorizontalAlignment.Left:
				lvgroup.uAlign = 1U;
				break;
			case HorizontalAlignment.Right:
				lvgroup.uAlign = 4U;
				break;
			case HorizontalAlignment.Center:
				lvgroup.uAlign = 2U;
				break;
			}
			return lvgroup;
		}

		// Token: 0x06002A28 RID: 10792 RVA: 0x000C407F File Offset: 0x000C227F
		internal Rectangle GetSubItemRect(int itemIndex, int subItemIndex)
		{
			return this.GetSubItemRect(itemIndex, subItemIndex, ItemBoundsPortion.Entire);
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x000C408C File Offset: 0x000C228C
		internal Rectangle GetSubItemRect(int itemIndex, int subItemIndex, ItemBoundsPortion portion)
		{
			if (this.View != View.Details)
			{
				return Rectangle.Empty;
			}
			if (itemIndex < 0 || itemIndex >= this.Items.Count)
			{
				throw new ArgumentOutOfRangeException("itemIndex", SR.GetString("InvalidArgument", new object[]
				{
					"itemIndex",
					itemIndex.ToString(CultureInfo.CurrentCulture)
				}));
			}
			int count = this.Items[itemIndex].SubItems.Count;
			if (subItemIndex < 0 || subItemIndex >= count)
			{
				throw new ArgumentOutOfRangeException("subItemIndex", SR.GetString("InvalidArgument", new object[]
				{
					"subItemIndex",
					subItemIndex.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (!ClientUtils.IsEnumValid(portion, (int)portion, 0, 3))
			{
				throw new InvalidEnumArgumentException("portion", (int)portion, typeof(ItemBoundsPortion));
			}
			if (this.Columns.Count == 0)
			{
				return Rectangle.Empty;
			}
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			rect.left = (int)portion;
			rect.top = subItemIndex;
			if ((int)((long)base.SendMessage(4152, itemIndex, ref rect)) == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"itemIndex",
					itemIndex.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
		}

		/// <summary>Provides item information, given a point.</summary>
		/// <param name="point">The <see cref="T:System.Drawing.Point" /> at which to retrieve the item information. The coordinates are relative to the upper-left corner of the control.</param>
		/// <returns>The item information, given a point.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The point contains coordinates that are less than 0.</exception>
		// Token: 0x06002A2A RID: 10794 RVA: 0x000C41F0 File Offset: 0x000C23F0
		public ListViewHitTestInfo HitTest(Point point)
		{
			return this.HitTest(point.X, point.Y);
		}

		/// <summary>Provides item information, given x- and y-coordinates.</summary>
		/// <param name="x">The x-coordinate at which to retrieve the item information. The coordinate is relative to the upper-left corner of the control.</param>
		/// <param name="y">The y-coordinate at which to retrieve the item information. The coordinate is relative to the upper-left corner of the control.</param>
		/// <returns>The item information, given x- and y- coordinates.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The x- or y-coordinate is less than 0.</exception>
		// Token: 0x06002A2B RID: 10795 RVA: 0x000C4208 File Offset: 0x000C2408
		public ListViewHitTestInfo HitTest(int x, int y)
		{
			if (!base.ClientRectangle.Contains(x, y))
			{
				return new ListViewHitTestInfo(null, null, ListViewHitTestLocations.None);
			}
			NativeMethods.LVHITTESTINFO lvhittestinfo = new NativeMethods.LVHITTESTINFO();
			lvhittestinfo.pt_x = x;
			lvhittestinfo.pt_y = y;
			int num;
			if (this.View == View.Details)
			{
				num = (int)((long)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4153, 0, lvhittestinfo));
			}
			else
			{
				num = (int)((long)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4114, 0, lvhittestinfo));
			}
			ListViewItem listViewItem = (num == -1) ? null : this.Items[num];
			ListViewHitTestLocations hitLocation;
			if (listViewItem == null && (8 & lvhittestinfo.flags) == 8)
			{
				hitLocation = (ListViewHitTestLocations)((247 & lvhittestinfo.flags) | 256);
			}
			else if (listViewItem != null && (8 & lvhittestinfo.flags) == 8)
			{
				hitLocation = (ListViewHitTestLocations)((247 & lvhittestinfo.flags) | 512);
			}
			else
			{
				hitLocation = (ListViewHitTestLocations)lvhittestinfo.flags;
			}
			if (this.View != View.Details || listViewItem == null)
			{
				return new ListViewHitTestInfo(listViewItem, null, hitLocation);
			}
			if (lvhittestinfo.iSubItem < listViewItem.SubItems.Count)
			{
				return new ListViewHitTestInfo(listViewItem, listViewItem.SubItems[lvhittestinfo.iSubItem], hitLocation);
			}
			return new ListViewHitTestInfo(listViewItem, null, hitLocation);
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x000C433C File Offset: 0x000C253C
		private void InvalidateColumnHeaders()
		{
			if (this.viewStyle == View.Details && base.IsHandleCreated)
			{
				IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4127, 0, 0);
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.InvalidateRect(new HandleRef(this, intPtr), null, true);
				}
			}
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x000C438F File Offset: 0x000C258F
		internal ColumnHeader InsertColumn(int index, ColumnHeader ch)
		{
			return this.InsertColumn(index, ch, true);
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x000C439C File Offset: 0x000C259C
		internal ColumnHeader InsertColumn(int index, ColumnHeader ch, bool refreshSubItems)
		{
			if (ch == null)
			{
				throw new ArgumentNullException("ch");
			}
			if (ch.OwnerListview != null)
			{
				throw new ArgumentException(SR.GetString("OnlyOneControl", new object[]
				{
					ch.Text
				}), "ch");
			}
			int num;
			if (base.IsHandleCreated && this.View != View.Tile)
			{
				num = this.InsertColumnNative(index, ch);
			}
			else
			{
				num = index;
			}
			if (-1 == num)
			{
				throw new InvalidOperationException(SR.GetString("ListViewAddColumnFailed"));
			}
			int num2 = (this.columnHeaders == null) ? 0 : this.columnHeaders.Length;
			if (num2 > 0)
			{
				ColumnHeader[] destinationArray = new ColumnHeader[num2 + 1];
				if (num2 > 0)
				{
					Array.Copy(this.columnHeaders, 0, destinationArray, 0, num2);
				}
				this.columnHeaders = destinationArray;
			}
			else
			{
				this.columnHeaders = new ColumnHeader[1];
			}
			if (num < num2)
			{
				Array.Copy(this.columnHeaders, num, this.columnHeaders, num + 1, num2 - num);
			}
			this.columnHeaders[num] = ch;
			ch.OwnerListview = this;
			if (ch.ActualImageIndex_Internal != -1 && base.IsHandleCreated && this.View != View.Tile)
			{
				this.SetColumnInfo(16, ch);
			}
			int[] array = new int[this.Columns.Count];
			for (int i = 0; i < this.Columns.Count; i++)
			{
				ColumnHeader columnHeader = this.Columns[i];
				if (columnHeader == ch)
				{
					columnHeader.DisplayIndexInternal = index;
				}
				else if (columnHeader.DisplayIndex >= index)
				{
					ColumnHeader columnHeader2 = columnHeader;
					int displayIndexInternal = columnHeader2.DisplayIndexInternal;
					columnHeader2.DisplayIndexInternal = displayIndexInternal + 1;
				}
				array[i] = columnHeader.DisplayIndexInternal;
			}
			this.SetDisplayIndices(array);
			if (base.IsHandleCreated && this.View == View.Tile)
			{
				this.RecreateHandleInternal();
			}
			else if (base.IsHandleCreated && refreshSubItems)
			{
				this.RealizeAllSubItems();
			}
			return ch;
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x000C4550 File Offset: 0x000C2750
		private int InsertColumnNative(int index, ColumnHeader ch)
		{
			NativeMethods.LVCOLUMN_T lvcolumn_T = new NativeMethods.LVCOLUMN_T();
			lvcolumn_T.mask = 7;
			if (ch.OwnerListview != null && ch.ActualImageIndex_Internal != -1)
			{
				lvcolumn_T.mask |= 16;
				lvcolumn_T.iImage = ch.ActualImageIndex_Internal;
			}
			lvcolumn_T.fmt = (int)ch.TextAlign;
			lvcolumn_T.cx = ch.Width;
			lvcolumn_T.pszText = ch.Text;
			return (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_INSERTCOLUMN, index, lvcolumn_T);
		}

		// Token: 0x06002A30 RID: 10800 RVA: 0x000C45D8 File Offset: 0x000C27D8
		internal void InsertGroupInListView(int index, ListViewGroup group)
		{
			bool flag = this.groups.Count == 1 && this.GroupsEnabled;
			this.UpdateGroupView();
			this.EnsureDefaultGroup();
			this.InsertGroupNative(index, group);
			if (flag)
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					ListViewItem listViewItem = this.Items[i];
					if (listViewItem.Group == null)
					{
						listViewItem.UpdateStateToListView(listViewItem.Index);
					}
				}
			}
		}

		// Token: 0x06002A31 RID: 10801 RVA: 0x000C464C File Offset: 0x000C284C
		private void InsertGroupNative(int index, ListViewGroup group)
		{
			NativeMethods.LVGROUP lvgroup = new NativeMethods.LVGROUP();
			try
			{
				lvgroup = this.GetLVGROUP(group);
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4241, index, lvgroup);
			}
			finally
			{
				this.DestroyLVGROUP(lvgroup);
			}
		}

		// Token: 0x06002A32 RID: 10802 RVA: 0x000C46A0 File Offset: 0x000C28A0
		private void InsertItems(int displayIndex, ListViewItem[] items, bool checkHosting)
		{
			if (items == null || items.Length == 0)
			{
				return;
			}
			if (base.IsHandleCreated && this.Items.Count == 0 && this.View == View.SmallIcon && this.ComctlSupportsVisualStyles)
			{
				this.FlipViewToLargeIconAndSmallIcon = true;
			}
			if (this.updateCounter > 0 && base.Properties.GetObject(ListView.PropDelayedUpdateItems) != null)
			{
				if (checkHosting)
				{
					for (int i = 0; i < items.Length; i++)
					{
						if (items[i].listView != null)
						{
							throw new ArgumentException(SR.GetString("OnlyOneControl", new object[]
							{
								items[i].Text
							}), "item");
						}
					}
				}
				ArrayList arrayList = (ArrayList)base.Properties.GetObject(ListView.PropDelayedUpdateItems);
				if (arrayList != null)
				{
					arrayList.AddRange(items);
				}
				for (int j = 0; j < items.Length; j++)
				{
					items[j].Host(this, this.GenerateUniqueID(), -1);
				}
				this.FlipViewToLargeIconAndSmallIcon = false;
				return;
			}
			for (int k = 0; k < items.Length; k++)
			{
				ListViewItem listViewItem = items[k];
				if (checkHosting && listViewItem.listView != null)
				{
					throw new ArgumentException(SR.GetString("OnlyOneControl", new object[]
					{
						listViewItem.Text
					}), "item");
				}
				int num = this.GenerateUniqueID();
				this.listItemsTable.Add(num, listViewItem);
				this.itemCount++;
				listViewItem.Host(this, num, -1);
				if (!base.IsHandleCreated)
				{
					this.listItemsArray.Insert(displayIndex + k, listViewItem);
				}
			}
			if (base.IsHandleCreated)
			{
				this.InsertItemsNative(displayIndex, items);
			}
			base.Invalidate();
			this.ArrangeIcons(this.alignStyle);
			if (!this.VirtualMode)
			{
				this.Sort();
			}
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x000C4854 File Offset: 0x000C2A54
		private int InsertItemsNative(int index, ListViewItem[] items)
		{
			if (items == null || items.Length == 0)
			{
				return 0;
			}
			if (index == this.itemCount - 1)
			{
				index++;
			}
			NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
			int num = -1;
			IntPtr intPtr = IntPtr.Zero;
			int num2 = 0;
			this.listViewState1[1] = true;
			try
			{
				base.SendMessage(4143, this.itemCount, 0);
				for (int i = 0; i < items.Length; i++)
				{
					ListViewItem listViewItem = items[i];
					lvitem.Reset();
					lvitem.mask = 23;
					lvitem.iItem = index + i;
					lvitem.pszText = listViewItem.Text;
					lvitem.iImage = listViewItem.ImageIndexer.ActualIndex;
					lvitem.iIndent = listViewItem.IndentCount;
					lvitem.lParam = (IntPtr)listViewItem.ID;
					if (this.GroupsEnabled)
					{
						lvitem.mask |= 256;
						lvitem.iGroupId = this.GetNativeGroupId(listViewItem);
					}
					lvitem.mask |= 512;
					lvitem.cColumns = ((this.columnHeaders != null) ? Math.Min(20, this.columnHeaders.Length) : 0);
					if (lvitem.cColumns > num2 || intPtr == IntPtr.Zero)
					{
						if (intPtr != IntPtr.Zero)
						{
							Marshal.FreeHGlobal(intPtr);
						}
						intPtr = Marshal.AllocHGlobal(lvitem.cColumns * Marshal.SizeOf(typeof(int)));
						num2 = lvitem.cColumns;
					}
					lvitem.puColumns = intPtr;
					int[] array = new int[lvitem.cColumns];
					for (int j = 0; j < lvitem.cColumns; j++)
					{
						array[j] = j + 1;
					}
					Marshal.Copy(array, 0, lvitem.puColumns, lvitem.cColumns);
					ItemCheckEventHandler itemCheckEventHandler = this.onItemCheck;
					this.onItemCheck = null;
					int num3;
					try
					{
						listViewItem.UpdateStateToListView(lvitem.iItem, ref lvitem, false);
						num3 = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_INSERTITEM, 0, ref lvitem);
						if (num == -1)
						{
							num = num3;
							index = num;
						}
					}
					finally
					{
						this.onItemCheck = itemCheckEventHandler;
					}
					if (-1 == num3)
					{
						throw new InvalidOperationException(SR.GetString("ListViewAddItemFailed"));
					}
					for (int k = 1; k < listViewItem.SubItems.Count; k++)
					{
						this.SetItemText(num3, k, listViewItem.SubItems[k].Text, ref lvitem);
					}
					if (listViewItem.StateImageSet || listViewItem.StateSelected)
					{
						this.SetItemState(num3, lvitem.state, lvitem.stateMask);
					}
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				this.listViewState1[1] = false;
			}
			if (this.listViewState1[16])
			{
				this.listViewState1[16] = false;
				this.OnSelectedIndexChanged(EventArgs.Empty);
			}
			if (this.FlipViewToLargeIconAndSmallIcon)
			{
				this.FlipViewToLargeIconAndSmallIcon = false;
				this.View = View.LargeIcon;
				this.View = View.SmallIcon;
			}
			return num;
		}

		/// <summary>Determines whether the specified key is a regular input key or a special key that requires preprocessing.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values.</param>
		/// <returns>
		///     <see langword="true" /> if the specified key is a regular input key; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002A34 RID: 10804 RVA: 0x000C4B78 File Offset: 0x000C2D78
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.Alt)
			{
				return false;
			}
			Keys keys = keyData & Keys.KeyCode;
			if (keys - Keys.Prior <= 3)
			{
				return true;
			}
			bool flag = base.IsInputKey(keyData);
			if (flag)
			{
				return true;
			}
			if (this.listViewState[16384])
			{
				keys = (keyData & Keys.KeyCode);
				if (keys == Keys.Return || keys == Keys.Escape)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x000C4BDC File Offset: 0x000C2DDC
		private void LargeImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				IntPtr lparam = (this.LargeImageList == null) ? IntPtr.Zero : this.LargeImageList.Handle;
				base.SendMessage(4099, (IntPtr)0, lparam);
				this.ForceCheckBoxUpdate();
			}
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x000C4C28 File Offset: 0x000C2E28
		private void LargeImageListChangedHandle(object sender, EventArgs e)
		{
			if (!this.VirtualMode && sender != null && sender == this.imageListLarge && base.IsHandleCreated)
			{
				foreach (object obj in this.Items)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					if (listViewItem.ImageIndexer.ActualIndex != -1 && listViewItem.ImageIndexer.ActualIndex >= this.imageListLarge.Images.Count)
					{
						this.SetItemImage(listViewItem.Index, this.imageListLarge.Images.Count - 1);
					}
					else
					{
						this.SetItemImage(listViewItem.Index, listViewItem.ImageIndexer.ActualIndex);
					}
				}
			}
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x000C4D04 File Offset: 0x000C2F04
		internal void ListViewItemToolTipChanged(ListViewItem item)
		{
			if (base.IsHandleCreated)
			{
				this.SetItemText(item.Index, 0, item.Text);
			}
		}

		// Token: 0x06002A38 RID: 10808 RVA: 0x000C4D24 File Offset: 0x000C2F24
		private void LvnBeginDrag(MouseButtons buttons, NativeMethods.NMLISTVIEW nmlv)
		{
			ListViewItem item = this.Items[nmlv.iItem];
			this.OnItemDrag(new ItemDragEventArgs(buttons, item));
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.AfterLabelEdit" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.LabelEditEventArgs" /> that contains the event data. </param>
		// Token: 0x06002A39 RID: 10809 RVA: 0x000C4D50 File Offset: 0x000C2F50
		protected virtual void OnAfterLabelEdit(LabelEditEventArgs e)
		{
			if (this.onAfterLabelEdit != null)
			{
				this.onAfterLabelEdit(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackgroundImageChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002A3A RID: 10810 RVA: 0x000C4D67 File Offset: 0x000C2F67
		protected override void OnBackgroundImageChanged(EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				this.SetBackgroundImage();
			}
			base.OnBackgroundImageChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002A3B RID: 10811 RVA: 0x000C4D7E File Offset: 0x000C2F7E
		protected override void OnMouseLeave(EventArgs e)
		{
			this.hoveredAlready = false;
			base.OnMouseLeave(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseHover" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002A3C RID: 10812 RVA: 0x000C4D90 File Offset: 0x000C2F90
		protected override void OnMouseHover(EventArgs e)
		{
			ListViewItem listViewItem = null;
			if (this.Items.Count > 0)
			{
				Point p = Cursor.Position;
				p = base.PointToClientInternal(p);
				listViewItem = this.GetItemAt(p.X, p.Y);
			}
			if (listViewItem != this.prevHoveredItem && listViewItem != null)
			{
				this.OnItemMouseHover(new ListViewItemMouseHoverEventArgs(listViewItem));
				this.prevHoveredItem = listViewItem;
			}
			if (!this.hoveredAlready)
			{
				base.OnMouseHover(e);
				this.hoveredAlready = true;
			}
			base.ResetMouseEventArgs();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.BeforeLabelEdit" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.LabelEditEventArgs" /> that contains the event data. </param>
		// Token: 0x06002A3D RID: 10813 RVA: 0x000C4E0B File Offset: 0x000C300B
		protected virtual void OnBeforeLabelEdit(LabelEditEventArgs e)
		{
			if (this.onBeforeLabelEdit != null)
			{
				this.onBeforeLabelEdit(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.CacheVirtualItems" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.CacheVirtualItemsEventArgs" /> that contains the event data. </param>
		// Token: 0x06002A3E RID: 10814 RVA: 0x000C4E24 File Offset: 0x000C3024
		protected virtual void OnCacheVirtualItems(CacheVirtualItemsEventArgs e)
		{
			CacheVirtualItemsEventHandler cacheVirtualItemsEventHandler = (CacheVirtualItemsEventHandler)base.Events[ListView.EVENT_CACHEVIRTUALITEMS];
			if (cacheVirtualItemsEventHandler != null)
			{
				cacheVirtualItemsEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.ColumnClick" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ColumnClickEventArgs" /> that contains the event data. </param>
		// Token: 0x06002A3F RID: 10815 RVA: 0x000C4E52 File Offset: 0x000C3052
		protected virtual void OnColumnClick(ColumnClickEventArgs e)
		{
			if (this.onColumnClick != null)
			{
				this.onColumnClick(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.ColumnReordered" /> event. </summary>
		/// <param name="e">The <see cref="T:System.Windows.Forms.ColumnReorderedEventArgs" /> that contains the event data.</param>
		// Token: 0x06002A40 RID: 10816 RVA: 0x000C4E6C File Offset: 0x000C306C
		protected virtual void OnColumnReordered(ColumnReorderedEventArgs e)
		{
			ColumnReorderedEventHandler columnReorderedEventHandler = (ColumnReorderedEventHandler)base.Events[ListView.EVENT_COLUMNREORDERED];
			if (columnReorderedEventHandler != null)
			{
				columnReorderedEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.ColumnWidthChanged" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ColumnWidthChangedEventArgs" /> that contains the event data. </param>
		// Token: 0x06002A41 RID: 10817 RVA: 0x000C4E9C File Offset: 0x000C309C
		protected virtual void OnColumnWidthChanged(ColumnWidthChangedEventArgs e)
		{
			ColumnWidthChangedEventHandler columnWidthChangedEventHandler = (ColumnWidthChangedEventHandler)base.Events[ListView.EVENT_COLUMNWIDTHCHANGED];
			if (columnWidthChangedEventHandler != null)
			{
				columnWidthChangedEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.ColumnWidthChanging" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ColumnWidthChangingEventArgs" />  that contains the event data. </param>
		// Token: 0x06002A42 RID: 10818 RVA: 0x000C4ECC File Offset: 0x000C30CC
		protected virtual void OnColumnWidthChanging(ColumnWidthChangingEventArgs e)
		{
			ColumnWidthChangingEventHandler columnWidthChangingEventHandler = (ColumnWidthChangingEventHandler)base.Events[ListView.EVENT_COLUMNWIDTHCHANGING];
			if (columnWidthChangingEventHandler != null)
			{
				columnWidthChangingEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.DrawColumnHeader" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DrawListViewColumnHeaderEventArgs" /> that contains the event data. </param>
		// Token: 0x06002A43 RID: 10819 RVA: 0x000C4EFC File Offset: 0x000C30FC
		protected virtual void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
		{
			DrawListViewColumnHeaderEventHandler drawListViewColumnHeaderEventHandler = (DrawListViewColumnHeaderEventHandler)base.Events[ListView.EVENT_DRAWCOLUMNHEADER];
			if (drawListViewColumnHeaderEventHandler != null)
			{
				drawListViewColumnHeaderEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.DrawItem" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DrawListViewItemEventArgs" /> that contains the event data. </param>
		// Token: 0x06002A44 RID: 10820 RVA: 0x000C4F2C File Offset: 0x000C312C
		protected virtual void OnDrawItem(DrawListViewItemEventArgs e)
		{
			DrawListViewItemEventHandler drawListViewItemEventHandler = (DrawListViewItemEventHandler)base.Events[ListView.EVENT_DRAWITEM];
			if (drawListViewItemEventHandler != null)
			{
				drawListViewItemEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.DrawSubItem" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DrawListViewSubItemEventArgs" /> that contains the event data. </param>
		// Token: 0x06002A45 RID: 10821 RVA: 0x000C4F5C File Offset: 0x000C315C
		protected virtual void OnDrawSubItem(DrawListViewSubItemEventArgs e)
		{
			DrawListViewSubItemEventHandler drawListViewSubItemEventHandler = (DrawListViewSubItemEventHandler)base.Events[ListView.EVENT_DRAWSUBITEM];
			if (drawListViewSubItemEventHandler != null)
			{
				drawListViewSubItemEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see langword="FontChanged" /> event.</summary>
		/// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002A46 RID: 10822 RVA: 0x000C4F8C File Offset: 0x000C318C
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			if (!this.VirtualMode && base.IsHandleCreated && this.AutoArrange)
			{
				this.BeginUpdate();
				try
				{
					base.SendMessage(4138, -1, 0);
				}
				finally
				{
					this.EndUpdate();
				}
			}
			this.InvalidateColumnHeaders();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002A47 RID: 10823 RVA: 0x000C4FEC File Offset: 0x000C31EC
		protected override void OnHandleCreated(EventArgs e)
		{
			this.listViewState[4194304] = false;
			this.FlipViewToLargeIconAndSmallIcon = false;
			base.OnHandleCreated(e);
			int num = (int)((long)base.SendMessage(8200, 0, 0));
			if (num < 5)
			{
				base.SendMessage(8199, 5, 0);
			}
			this.UpdateExtendedStyles();
			this.RealizeProperties();
			int lparam = ColorTranslator.ToWin32(this.BackColor);
			base.SendMessage(4097, 0, lparam);
			base.SendMessage(4132, 0, ColorTranslator.ToWin32(base.ForeColor));
			base.SendMessage(4134, 0, -1);
			if (!this.Scrollable)
			{
				int num2 = (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -16));
				num2 |= 8192;
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -16, new HandleRef(null, (IntPtr)num2));
			}
			if (this.VirtualMode)
			{
				int num3 = (int)((long)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4106, 0, 0));
				num3 |= 61440;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4107, num3, 0);
			}
			if (this.ComctlSupportsVisualStyles)
			{
				base.SendMessage(4238, (int)this.viewStyle, 0);
				this.UpdateGroupView();
				if (this.groups != null)
				{
					for (int i = 0; i < this.groups.Count; i++)
					{
						this.InsertGroupNative(i, this.groups[i]);
					}
				}
				if (this.viewStyle == View.Tile)
				{
					this.UpdateTileView();
				}
			}
			this.ListViewHandleDestroyed = false;
			ListViewItem[] array = null;
			if (this.listItemsArray != null)
			{
				array = (ListViewItem[])this.listItemsArray.ToArray(typeof(ListViewItem));
				this.listItemsArray = null;
			}
			int num4 = (this.columnHeaders == null) ? 0 : this.columnHeaders.Length;
			if (num4 > 0)
			{
				int[] array2 = new int[this.columnHeaders.Length];
				int num5 = 0;
				foreach (ColumnHeader columnHeader in this.columnHeaders)
				{
					array2[num5] = columnHeader.DisplayIndex;
					this.InsertColumnNative(num5++, columnHeader);
				}
				this.SetDisplayIndices(array2);
			}
			if (this.itemCount > 0 && array != null)
			{
				this.InsertItemsNative(0, array);
			}
			if (this.VirtualMode && this.VirtualListSize > -1 && !base.DesignMode)
			{
				base.SendMessage(4143, this.VirtualListSize, 0);
			}
			if (num4 > 0)
			{
				this.UpdateColumnWidths(ColumnHeaderAutoResizeStyle.None);
			}
			this.ArrangeIcons(this.alignStyle);
			this.UpdateListViewItemsLocations();
			if (!this.VirtualMode)
			{
				this.Sort();
			}
			if (this.ComctlSupportsVisualStyles && this.InsertionMark.Index > 0)
			{
				this.InsertionMark.UpdateListView();
			}
			this.savedCheckedItems = null;
			if (!this.CheckBoxes && !this.VirtualMode)
			{
				for (int k = 0; k < this.Items.Count; k++)
				{
					if (this.Items[k].Checked)
					{
						this.UpdateSavedCheckedItems(this.Items[k], true);
					}
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002A48 RID: 10824 RVA: 0x000C5314 File Offset: 0x000C3514
		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (!base.Disposing && !this.VirtualMode)
			{
				int count = this.Items.Count;
				for (int i = 0; i < count; i++)
				{
					this.Items[i].UpdateStateFromListView(i, true);
				}
				if (this.SelectedItems != null && !this.VirtualMode)
				{
					ListViewItem[] array = new ListViewItem[this.SelectedItems.Count];
					this.SelectedItems.CopyTo(array, 0);
					this.savedSelectedItems = new List<ListViewItem>(array.Length);
					for (int j = 0; j < array.Length; j++)
					{
						this.savedSelectedItems.Add(array[j]);
					}
				}
				ListViewItem[] array2 = null;
				ListView.ListViewItemCollection items = this.Items;
				if (items != null)
				{
					array2 = new ListViewItem[items.Count];
					items.CopyTo(array2, 0);
				}
				if (array2 != null)
				{
					this.listItemsArray = new ArrayList(array2.Length);
					this.listItemsArray.AddRange(array2);
				}
				this.ListViewHandleDestroyed = true;
			}
			base.OnHandleDestroyed(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.ItemActivate" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002A49 RID: 10825 RVA: 0x000C540F File Offset: 0x000C360F
		protected virtual void OnItemActivate(EventArgs e)
		{
			if (this.onItemActivate != null)
			{
				this.onItemActivate(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.ItemCheck" /> event.</summary>
		/// <param name="ice">An <see cref="T:System.Windows.Forms.ItemCheckEventArgs" /> that contains the event data. </param>
		// Token: 0x06002A4A RID: 10826 RVA: 0x000C5426 File Offset: 0x000C3626
		protected virtual void OnItemCheck(ItemCheckEventArgs ice)
		{
			if (this.onItemCheck != null)
			{
				this.onItemCheck(this, ice);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.ItemChecked" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Windows.Forms.ItemCheckedEventArgs" /> that contains the event data.</param>
		// Token: 0x06002A4B RID: 10827 RVA: 0x000C543D File Offset: 0x000C363D
		protected virtual void OnItemChecked(ItemCheckedEventArgs e)
		{
			if (this.onItemChecked != null)
			{
				this.onItemChecked(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.ItemDrag" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Windows.Forms.ItemDragEventArgs" /> that contains the event data. </param>
		// Token: 0x06002A4C RID: 10828 RVA: 0x000C5454 File Offset: 0x000C3654
		protected virtual void OnItemDrag(ItemDragEventArgs e)
		{
			if (this.onItemDrag != null)
			{
				this.onItemDrag(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.ItemMouseHover" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ListViewItemMouseHoverEventArgs" /> that contains the event data. </param>
		// Token: 0x06002A4D RID: 10829 RVA: 0x000C546B File Offset: 0x000C366B
		protected virtual void OnItemMouseHover(ListViewItemMouseHoverEventArgs e)
		{
			if (this.onItemMouseHover != null)
			{
				this.onItemMouseHover(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.ItemSelectionChanged" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ListViewItemSelectionChangedEventArgs" /> that contains the event data. </param>
		// Token: 0x06002A4E RID: 10830 RVA: 0x000C5484 File Offset: 0x000C3684
		protected virtual void OnItemSelectionChanged(ListViewItemSelectionChangedEventArgs e)
		{
			ListViewItemSelectionChangedEventHandler listViewItemSelectionChangedEventHandler = (ListViewItemSelectionChangedEventHandler)base.Events[ListView.EVENT_ITEMSELECTIONCHANGED];
			if (listViewItemSelectionChangedEventHandler != null)
			{
				listViewItemSelectionChangedEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002A4F RID: 10831 RVA: 0x000C54B2 File Offset: 0x000C36B2
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			if (base.IsHandleCreated)
			{
				this.RecreateHandleInternal();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002A50 RID: 10832 RVA: 0x000C54C9 File Offset: 0x000C36C9
		protected override void OnResize(EventArgs e)
		{
			if (this.View == View.Details && !this.Scrollable && base.IsHandleCreated)
			{
				this.PositionHeader();
			}
			base.OnResize(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.RetrieveVirtualItem" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.RetrieveVirtualItemEventArgs" /> that contains the event data. </param>
		// Token: 0x06002A51 RID: 10833 RVA: 0x000C54F4 File Offset: 0x000C36F4
		protected virtual void OnRetrieveVirtualItem(RetrieveVirtualItemEventArgs e)
		{
			RetrieveVirtualItemEventHandler retrieveVirtualItemEventHandler = (RetrieveVirtualItemEventHandler)base.Events[ListView.EVENT_RETRIEVEVIRTUALITEM];
			if (retrieveVirtualItemEventHandler != null)
			{
				retrieveVirtualItemEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.RightToLeftLayoutChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002A52 RID: 10834 RVA: 0x000C5524 File Offset: 0x000C3724
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRightToLeftLayoutChanged(EventArgs e)
		{
			if (base.GetAnyDisposingInHierarchy())
			{
				return;
			}
			if (this.RightToLeft == RightToLeft.Yes)
			{
				this.RecreateHandleInternal();
			}
			EventHandler eventHandler = base.Events[ListView.EVENT_RIGHTTOLEFTLAYOUTCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.SearchForVirtualItem" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.SearchForVirtualItemEventArgs" /> that contains the event data. </param>
		// Token: 0x06002A53 RID: 10835 RVA: 0x000C556C File Offset: 0x000C376C
		protected virtual void OnSearchForVirtualItem(SearchForVirtualItemEventArgs e)
		{
			SearchForVirtualItemEventHandler searchForVirtualItemEventHandler = (SearchForVirtualItemEventHandler)base.Events[ListView.EVENT_SEARCHFORVIRTUALITEM];
			if (searchForVirtualItemEventHandler != null)
			{
				searchForVirtualItemEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.SelectedIndexChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002A54 RID: 10836 RVA: 0x000C559C File Offset: 0x000C379C
		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ListView.EVENT_SELECTEDINDEXCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.SystemColorsChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002A55 RID: 10837 RVA: 0x000C55CC File Offset: 0x000C37CC
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			if (base.IsHandleCreated)
			{
				int lparam = ColorTranslator.ToWin32(this.BackColor);
				base.SendMessage(4097, 0, lparam);
				base.SendMessage(4134, 0, -1);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListView.VirtualItemsSelectionRangeChanged" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ListViewVirtualItemsSelectionRangeChangedEventArgs" /> that contains the event data. </param>
		// Token: 0x06002A56 RID: 10838 RVA: 0x000C5610 File Offset: 0x000C3810
		protected virtual void OnVirtualItemsSelectionRangeChanged(ListViewVirtualItemsSelectionRangeChangedEventArgs e)
		{
			ListViewVirtualItemsSelectionRangeChangedEventHandler listViewVirtualItemsSelectionRangeChangedEventHandler = (ListViewVirtualItemsSelectionRangeChangedEventHandler)base.Events[ListView.EVENT_VIRTUALITEMSSELECTIONRANGECHANGED];
			if (listViewVirtualItemsSelectionRangeChangedEventHandler != null)
			{
				listViewVirtualItemsSelectionRangeChangedEventHandler(this, e);
			}
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x000C5640 File Offset: 0x000C3840
		private void PositionHeader()
		{
			IntPtr window = UnsafeNativeMethods.GetWindow(new HandleRef(this, base.Handle), 5);
			if (window != IntPtr.Zero)
			{
				IntPtr intPtr = IntPtr.Zero;
				IntPtr intPtr2 = IntPtr.Zero;
				intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.RECT)));
				if (intPtr == IntPtr.Zero)
				{
					return;
				}
				try
				{
					intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.WINDOWPOS)));
					if (!(intPtr == IntPtr.Zero))
					{
						UnsafeNativeMethods.GetClientRect(new HandleRef(this, base.Handle), intPtr);
						NativeMethods.HDLAYOUT hdlayout = default(NativeMethods.HDLAYOUT);
						hdlayout.prc = intPtr;
						hdlayout.pwpos = intPtr2;
						UnsafeNativeMethods.SendMessage(new HandleRef(this, window), 4613, 0, ref hdlayout);
						NativeMethods.WINDOWPOS windowpos = (NativeMethods.WINDOWPOS)Marshal.PtrToStructure(intPtr2, typeof(NativeMethods.WINDOWPOS));
						SafeNativeMethods.SetWindowPos(new HandleRef(this, window), new HandleRef(this, windowpos.hwndInsertAfter), windowpos.x, windowpos.y, windowpos.cx, windowpos.cy, windowpos.flags | 64);
					}
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(intPtr);
					}
					if (intPtr2 != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(intPtr2);
					}
				}
			}
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x000C5798 File Offset: 0x000C3998
		private void RealizeAllSubItems()
		{
			NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
			for (int i = 0; i < this.itemCount; i++)
			{
				int count = this.Items[i].SubItems.Count;
				for (int j = 0; j < count; j++)
				{
					this.SetItemText(i, j, this.Items[i].SubItems[j].Text, ref lvitem);
				}
			}
		}

		/// <summary>Initializes the properties of the <see cref="T:System.Windows.Forms.ListView" /> control that manage the appearance of the control.</summary>
		// Token: 0x06002A59 RID: 10841 RVA: 0x000C5808 File Offset: 0x000C3A08
		protected void RealizeProperties()
		{
			Color color = this.BackColor;
			if (color != SystemColors.Window)
			{
				base.SendMessage(4097, 0, ColorTranslator.ToWin32(color));
			}
			color = this.ForeColor;
			if (color != SystemColors.WindowText)
			{
				base.SendMessage(4132, 0, ColorTranslator.ToWin32(color));
			}
			if (this.imageListLarge != null)
			{
				base.SendMessage(4099, 0, this.imageListLarge.Handle);
			}
			if (this.imageListSmall != null)
			{
				base.SendMessage(4099, 1, this.imageListSmall.Handle);
			}
			if (this.imageListState != null)
			{
				base.SendMessage(4099, 2, this.imageListState.Handle);
			}
		}

		/// <summary>Forces a range of <see cref="T:System.Windows.Forms.ListViewItem" /> objects to be redrawn.</summary>
		/// <param name="startIndex">The index for the first item in the range to be redrawn.</param>
		/// <param name="endIndex">The index for the last item of the range to be redrawn.</param>
		/// <param name="invalidateOnly">
		///       <see langword="true" /> to invalidate the range of items; <see langword="false" /> to invalidate and repaint the items.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="startIndex" /> or <paramref name="endIndex" /> is less than 0, greater than or equal to the number of items in the <see cref="T:System.Windows.Forms.ListView" /> or, if in virtual mode, greater than the value of <see cref="P:System.Windows.Forms.ListView.VirtualListSize" />.-or-The given <paramref name="startIndex" /> is greater than the <paramref name="endIndex." /></exception>
		// Token: 0x06002A5A RID: 10842 RVA: 0x000C58C4 File Offset: 0x000C3AC4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void RedrawItems(int startIndex, int endIndex, bool invalidateOnly)
		{
			if (this.VirtualMode)
			{
				if (startIndex < 0 || startIndex >= this.VirtualListSize)
				{
					throw new ArgumentOutOfRangeException("startIndex", SR.GetString("InvalidArgument", new object[]
					{
						"startIndex",
						startIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (endIndex < 0 || endIndex >= this.VirtualListSize)
				{
					throw new ArgumentOutOfRangeException("endIndex", SR.GetString("InvalidArgument", new object[]
					{
						"endIndex",
						endIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
			}
			else
			{
				if (startIndex < 0 || startIndex >= this.Items.Count)
				{
					throw new ArgumentOutOfRangeException("startIndex", SR.GetString("InvalidArgument", new object[]
					{
						"startIndex",
						startIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (endIndex < 0 || endIndex >= this.Items.Count)
				{
					throw new ArgumentOutOfRangeException("endIndex", SR.GetString("InvalidArgument", new object[]
					{
						"endIndex",
						endIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
			}
			if (startIndex > endIndex)
			{
				throw new ArgumentException(SR.GetString("ListViewStartIndexCannotBeLargerThanEndIndex"));
			}
			if (base.IsHandleCreated)
			{
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4117, startIndex, endIndex);
				if (this.View == View.LargeIcon || this.View == View.SmallIcon)
				{
					Rectangle rectangle = this.Items[startIndex].Bounds;
					for (int i = startIndex + 1; i <= endIndex; i++)
					{
						rectangle = Rectangle.Union(rectangle, this.Items[i].Bounds);
					}
					if (startIndex > 0)
					{
						rectangle = Rectangle.Union(rectangle, this.Items[startIndex - 1].Bounds);
					}
					else
					{
						rectangle.Width += rectangle.X;
						rectangle.Height += rectangle.Y;
						rectangle.X = (rectangle.Y = 0);
					}
					if (endIndex < this.Items.Count - 1)
					{
						rectangle = Rectangle.Union(rectangle, this.Items[endIndex + 1].Bounds);
					}
					else
					{
						rectangle.Height += base.ClientRectangle.Bottom - rectangle.Bottom;
						rectangle.Width += base.ClientRectangle.Right - rectangle.Right;
					}
					if (this.View == View.LargeIcon)
					{
						rectangle.Inflate(1, this.Font.Height + 1);
					}
					base.Invalidate(rectangle);
				}
				if (!invalidateOnly)
				{
					base.Update();
				}
			}
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x000C5B70 File Offset: 0x000C3D70
		internal void RemoveGroupFromListView(ListViewGroup group)
		{
			this.EnsureDefaultGroup();
			foreach (object obj in group.Items)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				if (listViewItem.ListView == this)
				{
					listViewItem.UpdateStateToListView(listViewItem.Index);
				}
			}
			this.RemoveGroupNative(group);
			this.UpdateGroupView();
		}

		// Token: 0x06002A5C RID: 10844 RVA: 0x000C5BEC File Offset: 0x000C3DEC
		private void RemoveGroupNative(ListViewGroup group)
		{
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4246, group.ID, IntPtr.Zero);
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x000C5C20 File Offset: 0x000C3E20
		private void Scroll(int fromLVItem, int toLVItem)
		{
			int lParam = this.GetItemPosition(toLVItem).Y - this.GetItemPosition(fromLVItem).Y;
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4116, 0, lParam);
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x000C5C68 File Offset: 0x000C3E68
		private void SetBackgroundImage()
		{
			Application.OleRequired();
			NativeMethods.LVBKIMAGE lvbkimage = new NativeMethods.LVBKIMAGE();
			lvbkimage.xOffset = 0;
			lvbkimage.yOffset = 0;
			string text = this.backgroundImageFileName;
			if (this.BackgroundImage != null)
			{
				EnvironmentPermission perm = new EnvironmentPermission(EnvironmentPermissionAccess.Read, "TEMP");
				FileIOPermission perm2 = new FileIOPermission(PermissionState.Unrestricted);
				PermissionSet permissionSet = new PermissionSet(PermissionState.Unrestricted);
				permissionSet.AddPermission(perm);
				permissionSet.AddPermission(perm2);
				permissionSet.Assert();
				try
				{
					string tempPath = Path.GetTempPath();
					StringBuilder stringBuilder = new StringBuilder(1024);
					UnsafeNativeMethods.GetTempFileName(tempPath, this.GenerateRandomName(), 0, stringBuilder);
					this.backgroundImageFileName = stringBuilder.ToString();
					this.BackgroundImage.Save(this.backgroundImageFileName, ImageFormat.Bmp);
				}
				finally
				{
					PermissionSet.RevertAssert();
				}
				lvbkimage.pszImage = this.backgroundImageFileName;
				lvbkimage.cchImageMax = this.backgroundImageFileName.Length + 1;
				lvbkimage.ulFlags = 2;
				if (this.BackgroundImageTiled)
				{
					lvbkimage.ulFlags |= 16;
				}
				else
				{
					lvbkimage.ulFlags |= 0;
				}
			}
			else
			{
				lvbkimage.ulFlags = 0;
				this.backgroundImageFileName = string.Empty;
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_SETBKIMAGE, 0, lvbkimage);
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (this.bkImgFileNames == null)
			{
				this.bkImgFileNames = new string[8];
				this.bkImgFileNamesCount = -1;
			}
			if (this.bkImgFileNamesCount == 7)
			{
				this.DeleteFileName(this.bkImgFileNames[0]);
				this.bkImgFileNames[0] = this.bkImgFileNames[1];
				this.bkImgFileNames[1] = this.bkImgFileNames[2];
				this.bkImgFileNames[2] = this.bkImgFileNames[3];
				this.bkImgFileNames[3] = this.bkImgFileNames[4];
				this.bkImgFileNames[4] = this.bkImgFileNames[5];
				this.bkImgFileNames[5] = this.bkImgFileNames[6];
				this.bkImgFileNames[6] = this.bkImgFileNames[7];
				this.bkImgFileNames[7] = null;
				this.bkImgFileNamesCount--;
			}
			this.bkImgFileNamesCount++;
			this.bkImgFileNames[this.bkImgFileNamesCount] = text;
			this.Refresh();
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x000C5E94 File Offset: 0x000C4094
		internal void SetColumnInfo(int mask, ColumnHeader ch)
		{
			if (base.IsHandleCreated)
			{
				NativeMethods.LVCOLUMN lvcolumn = new NativeMethods.LVCOLUMN();
				lvcolumn.mask = mask;
				if ((mask & 16) != 0 || (mask & 1) != 0)
				{
					lvcolumn.mask |= 1;
					if (ch.ActualImageIndex_Internal > -1)
					{
						lvcolumn.iImage = ch.ActualImageIndex_Internal;
						lvcolumn.fmt |= 2048;
					}
					lvcolumn.fmt |= (int)ch.TextAlign;
				}
				if ((mask & 4) != 0)
				{
					lvcolumn.pszText = Marshal.StringToHGlobalAuto(ch.Text);
				}
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_SETCOLUMN, ch.Index, lvcolumn);
				if ((mask & 4) != 0)
				{
					Marshal.FreeHGlobal(lvcolumn.pszText);
				}
				if (num == 0)
				{
					throw new InvalidOperationException(SR.GetString("ListViewColumnInfoSet"));
				}
				this.InvalidateColumnHeaders();
			}
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x000C5F70 File Offset: 0x000C4170
		internal void SetColumnWidth(int columnIndex, ColumnHeaderAutoResizeStyle headerAutoResize)
		{
			if (columnIndex < 0 || (columnIndex >= 0 && this.columnHeaders == null) || columnIndex >= this.columnHeaders.Length)
			{
				throw new ArgumentOutOfRangeException("columnIndex", SR.GetString("InvalidArgument", new object[]
				{
					"columnIndex",
					columnIndex.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (!ClientUtils.IsEnumValid(headerAutoResize, (int)headerAutoResize, 0, 2))
			{
				throw new InvalidEnumArgumentException("headerAutoResize", (int)headerAutoResize, typeof(ColumnHeaderAutoResizeStyle));
			}
			int num = 0;
			int num2 = 0;
			if (headerAutoResize == ColumnHeaderAutoResizeStyle.None)
			{
				num = this.columnHeaders[columnIndex].WidthInternal;
				if (num == -2)
				{
					headerAutoResize = ColumnHeaderAutoResizeStyle.HeaderSize;
				}
				else if (num == -1)
				{
					headerAutoResize = ColumnHeaderAutoResizeStyle.ColumnContent;
				}
			}
			if (headerAutoResize == ColumnHeaderAutoResizeStyle.HeaderSize)
			{
				num2 = this.CompensateColumnHeaderResize(columnIndex, false);
				num = -2;
			}
			else if (headerAutoResize == ColumnHeaderAutoResizeStyle.ColumnContent)
			{
				num2 = this.CompensateColumnHeaderResize(columnIndex, false);
				num = -1;
			}
			if (base.IsHandleCreated)
			{
				base.SendMessage(4126, columnIndex, NativeMethods.Util.MAKELPARAM(num, 0));
			}
			if (base.IsHandleCreated && (headerAutoResize == ColumnHeaderAutoResizeStyle.ColumnContent || headerAutoResize == ColumnHeaderAutoResizeStyle.HeaderSize) && num2 != 0)
			{
				int low = this.columnHeaders[columnIndex].Width + num2;
				base.SendMessage(4126, columnIndex, NativeMethods.Util.MAKELPARAM(low, 0));
			}
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x000C608A File Offset: 0x000C428A
		private void SetColumnWidth(int index, int width)
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(4126, index, NativeMethods.Util.MAKELPARAM(width, 0));
			}
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x000C60A8 File Offset: 0x000C42A8
		private void SetDisplayIndices(int[] indices)
		{
			int[] array = new int[indices.Length];
			for (int i = 0; i < indices.Length; i++)
			{
				this.Columns[i].DisplayIndexInternal = indices[i];
				array[indices[i]] = i;
			}
			if (base.IsHandleCreated && !base.Disposing)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4154, array.Length, array);
			}
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x000C6111 File Offset: 0x000C4311
		internal void UpdateSavedCheckedItems(ListViewItem item, bool addItem)
		{
			if (addItem && this.savedCheckedItems == null)
			{
				this.savedCheckedItems = new List<ListViewItem>();
			}
			if (addItem)
			{
				this.savedCheckedItems.Add(item);
				return;
			}
			if (this.savedCheckedItems != null)
			{
				this.savedCheckedItems.Remove(item);
			}
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x000C6150 File Offset: 0x000C4350
		internal void SetToolTip(ToolTip toolTip, string toolTipCaption)
		{
			this.toolTipCaption = toolTipCaption;
			IntPtr handle = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4170, new HandleRef(toolTip, toolTip.Handle), 0);
			UnsafeNativeMethods.DestroyWindow(new HandleRef(null, handle));
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x000C6198 File Offset: 0x000C4398
		internal void SetItemImage(int index, int image)
		{
			if (index < 0 || (this.VirtualMode && index >= this.VirtualListSize) || (!this.VirtualMode && index >= this.itemCount))
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (base.IsHandleCreated)
			{
				NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
				lvitem.mask = 2;
				lvitem.iItem = index;
				lvitem.iImage = image;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_SETITEM, 0, ref lvitem);
			}
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x000C6240 File Offset: 0x000C4440
		internal void SetItemIndentCount(int index, int indentCount)
		{
			if (index < 0 || (this.VirtualMode && index >= this.VirtualListSize) || (!this.VirtualMode && index >= this.itemCount))
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (base.IsHandleCreated)
			{
				NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
				lvitem.mask = 16;
				lvitem.iItem = index;
				lvitem.iIndent = indentCount;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_SETITEM, 0, ref lvitem);
			}
		}

		// Token: 0x06002A67 RID: 10855 RVA: 0x000C62E8 File Offset: 0x000C44E8
		internal void SetItemPosition(int index, int x, int y)
		{
			if (this.VirtualMode)
			{
				return;
			}
			if (index < 0 || index >= this.itemCount)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			NativeMethods.POINT point = new NativeMethods.POINT();
			point.x = x;
			point.y = y;
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4145, index, point);
		}

		// Token: 0x06002A68 RID: 10856 RVA: 0x000C636C File Offset: 0x000C456C
		internal void SetItemState(int index, int state, int mask)
		{
			if (index < -1 || (this.VirtualMode && index >= this.VirtualListSize) || (!this.VirtualMode && index >= this.itemCount))
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (base.IsHandleCreated)
			{
				NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
				lvitem.mask = 8;
				lvitem.state = state;
				lvitem.stateMask = mask;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4139, index, ref lvitem);
			}
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x000C6414 File Offset: 0x000C4614
		internal void SetItemText(int itemIndex, int subItemIndex, string text)
		{
			NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
			this.SetItemText(itemIndex, subItemIndex, text, ref lvitem);
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x000C6434 File Offset: 0x000C4634
		private void SetItemText(int itemIndex, int subItemIndex, string text, ref NativeMethods.LVITEM lvItem)
		{
			if (this.View == View.List && subItemIndex == 0)
			{
				int num = (int)((long)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4125, 0, 0));
				Graphics graphics = base.CreateGraphicsInternal();
				int num2 = 0;
				try
				{
					num2 = Size.Ceiling(graphics.MeasureString(text, this.Font)).Width;
				}
				finally
				{
					graphics.Dispose();
				}
				if (num2 > num)
				{
					this.SetColumnWidth(0, num2);
				}
			}
			lvItem.mask = 1;
			lvItem.iItem = itemIndex;
			lvItem.iSubItem = subItemIndex;
			lvItem.pszText = text;
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_SETITEMTEXT, itemIndex, ref lvItem);
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x000C64F0 File Offset: 0x000C46F0
		internal void SetSelectionMark(int itemIndex)
		{
			if (itemIndex < 0 || itemIndex >= this.Items.Count)
			{
				return;
			}
			base.SendMessage(4163, 0, itemIndex);
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x000C6514 File Offset: 0x000C4714
		private void SmallImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				IntPtr lparam = (this.SmallImageList == null) ? IntPtr.Zero : this.SmallImageList.Handle;
				base.SendMessage(4099, (IntPtr)1, lparam);
				this.ForceCheckBoxUpdate();
			}
		}

		/// <summary>Sorts the items of the list view.</summary>
		// Token: 0x06002A6D RID: 10861 RVA: 0x000C6560 File Offset: 0x000C4760
		public void Sort()
		{
			if (this.VirtualMode)
			{
				throw new InvalidOperationException(SR.GetString("ListViewSortNotAllowedInVirtualListView"));
			}
			this.ApplyUpdateCachedItems();
			if (base.IsHandleCreated && this.listItemSorter != null)
			{
				NativeMethods.ListViewCompareCallback pfnCompare = new NativeMethods.ListViewCompareCallback(this.CompareFunc);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4144, IntPtr.Zero, pfnCompare);
			}
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x000C65C8 File Offset: 0x000C47C8
		private void StateImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				IntPtr lparam = IntPtr.Zero;
				if (this.StateImageList != null)
				{
					lparam = this.imageListState.Handle;
				}
				base.SendMessage(4099, (IntPtr)2, lparam);
			}
		}

		/// <summary>Returns a string representation of the <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
		/// <returns>A string that states the control type, the count of items in the <see cref="T:System.Windows.Forms.ListView" /> control, and the type of the first item in the <see cref="T:System.Windows.Forms.ListView" />, if the count is not 0.</returns>
		// Token: 0x06002A6F RID: 10863 RVA: 0x000C660C File Offset: 0x000C480C
		public override string ToString()
		{
			string text = base.ToString();
			if (this.listItemsArray != null)
			{
				text = text + ", Items.Count: " + this.listItemsArray.Count.ToString(CultureInfo.CurrentCulture);
				if (this.listItemsArray.Count > 0)
				{
					string text2 = this.listItemsArray[0].ToString();
					string str = (text2.Length > 40) ? text2.Substring(0, 40) : text2;
					text = text + ", Items[0]: " + str;
				}
			}
			else if (this.Items != null)
			{
				text = text + ", Items.Count: " + this.Items.Count.ToString(CultureInfo.CurrentCulture);
				if (this.Items.Count > 0 && !this.VirtualMode)
				{
					string text3 = (this.Items[0] == null) ? "null" : this.Items[0].ToString();
					string str2 = (text3.Length > 40) ? text3.Substring(0, 40) : text3;
					text = text + ", Items[0]: " + str2;
				}
			}
			return text;
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x000C6730 File Offset: 0x000C4930
		internal void UpdateListViewItemsLocations()
		{
			if (!this.VirtualMode && base.IsHandleCreated && this.AutoArrange && (this.View == View.LargeIcon || this.View == View.SmallIcon))
			{
				try
				{
					this.BeginUpdate();
					base.SendMessage(4138, -1, 0);
				}
				finally
				{
					this.EndUpdate();
				}
			}
		}

		// Token: 0x06002A71 RID: 10865 RVA: 0x000C6794 File Offset: 0x000C4994
		private void UpdateColumnWidths(ColumnHeaderAutoResizeStyle headerAutoResize)
		{
			if (this.columnHeaders != null)
			{
				for (int i = 0; i < this.columnHeaders.Length; i++)
				{
					this.SetColumnWidth(i, headerAutoResize);
				}
			}
		}

		/// <summary>Updates the extended styles applied to the list view control.</summary>
		// Token: 0x06002A72 RID: 10866 RVA: 0x000C67C4 File Offset: 0x000C49C4
		protected void UpdateExtendedStyles()
		{
			if (base.IsHandleCreated)
			{
				int num = 0;
				int wparam = 68861;
				ItemActivation itemActivation = this.activation;
				if (itemActivation != ItemActivation.OneClick)
				{
					if (itemActivation == ItemActivation.TwoClick)
					{
						num |= 128;
					}
				}
				else
				{
					num |= 64;
				}
				if (this.AllowColumnReorder)
				{
					num |= 16;
				}
				if (this.CheckBoxes)
				{
					num |= 4;
				}
				if (this.DoubleBuffered)
				{
					num |= 65536;
				}
				if (this.FullRowSelect)
				{
					num |= 32;
				}
				if (this.GridLines)
				{
					num |= 1;
				}
				if (this.HoverSelection)
				{
					num |= 8;
				}
				if (this.HotTracking)
				{
					num |= 2048;
				}
				if (this.ShowItemToolTips)
				{
					num |= 1024;
				}
				base.SendMessage(4150, wparam, num);
				base.Invalidate();
			}
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x000C6888 File Offset: 0x000C4A88
		internal void UpdateGroupNative(ListViewGroup group)
		{
			NativeMethods.LVGROUP lvgroup = new NativeMethods.LVGROUP();
			try
			{
				lvgroup = this.GetLVGROUP(group);
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4243, group.ID, lvgroup);
			}
			finally
			{
				this.DestroyLVGROUP(lvgroup);
			}
			base.Invalidate();
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x000C68E8 File Offset: 0x000C4AE8
		internal void UpdateGroupView()
		{
			if (base.IsHandleCreated && this.ComctlSupportsVisualStyles && !this.VirtualMode)
			{
				int num = (int)((long)base.SendMessage(4253, this.GroupsEnabled ? 1 : 0, 0));
			}
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x000C692C File Offset: 0x000C4B2C
		private void UpdateTileView()
		{
			NativeMethods.LVTILEVIEWINFO lvtileviewinfo = new NativeMethods.LVTILEVIEWINFO();
			lvtileviewinfo.dwMask = 2;
			lvtileviewinfo.cLines = ((this.columnHeaders != null) ? this.columnHeaders.Length : 0);
			lvtileviewinfo.dwMask |= 1;
			lvtileviewinfo.dwFlags = 3;
			lvtileviewinfo.sizeTile = new NativeMethods.SIZE(this.TileSize.Width, this.TileSize.Height);
			bool flag = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4258, 0, lvtileviewinfo);
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x000C69B4 File Offset: 0x000C4BB4
		private void WmNmClick(ref Message m)
		{
			if (this.CheckBoxes)
			{
				Point p = Cursor.Position;
				p = base.PointToClientInternal(p);
				NativeMethods.LVHITTESTINFO lvhittestinfo = new NativeMethods.LVHITTESTINFO();
				lvhittestinfo.pt_x = p.X;
				lvhittestinfo.pt_y = p.Y;
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4114, 0, lvhittestinfo);
				if (num != -1 && (lvhittestinfo.flags & 8) != 0)
				{
					ListViewItem listViewItem = this.Items[num];
					if (listViewItem.Selected)
					{
						bool @checked = !listViewItem.Checked;
						if (!this.VirtualMode)
						{
							foreach (object obj in this.SelectedItems)
							{
								ListViewItem listViewItem2 = (ListViewItem)obj;
								if (listViewItem2 != listViewItem)
								{
									listViewItem2.Checked = @checked;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x000C6AAC File Offset: 0x000C4CAC
		private void WmNmDblClick(ref Message m)
		{
			if (this.CheckBoxes)
			{
				Point p = Cursor.Position;
				p = base.PointToClientInternal(p);
				NativeMethods.LVHITTESTINFO lvhittestinfo = new NativeMethods.LVHITTESTINFO();
				lvhittestinfo.pt_x = p.X;
				lvhittestinfo.pt_y = p.Y;
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4114, 0, lvhittestinfo);
				if (num != -1 && (lvhittestinfo.flags & 14) != 0)
				{
					ListViewItem listViewItem = this.Items[num];
					listViewItem.Checked = !listViewItem.Checked;
				}
			}
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x000C6B38 File Offset: 0x000C4D38
		private void WmMouseDown(ref Message m, MouseButtons button, int clicks)
		{
			this.listViewState[524288] = false;
			this.listViewState[1048576] = true;
			this.FocusInternal();
			int x = NativeMethods.Util.SignedLOWORD(m.LParam);
			int y = NativeMethods.Util.SignedHIWORD(m.LParam);
			this.OnMouseDown(new MouseEventArgs(button, clicks, x, y, 0));
			if (!base.ValidationCancelled)
			{
				if (this.CheckBoxes)
				{
					ListViewHitTestInfo listViewHitTestInfo = this.HitTest(x, y);
					if (this.imageListState == null || this.imageListState.Images.Count >= 2)
					{
						if (AccessibilityImprovements.Level2 && listViewHitTestInfo.Item != null && listViewHitTestInfo.Location == ListViewHitTestLocations.StateImage)
						{
							listViewHitTestInfo.Item.Focused = true;
						}
						this.DefWndProc(ref m);
						return;
					}
					if (listViewHitTestInfo.Location != ListViewHitTestLocations.StateImage)
					{
						this.DefWndProc(ref m);
						return;
					}
				}
				else
				{
					this.DefWndProc(ref m);
				}
			}
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x000C6C18 File Offset: 0x000C4E18
		private unsafe bool WmNotify(ref Message m)
		{
			NativeMethods.NMHDR* ptr = (NativeMethods.NMHDR*)((void*)m.LParam);
			if (ptr->code == -12 && this.OwnerDraw)
			{
				try
				{
					NativeMethods.NMCUSTOMDRAW* ptr2 = (NativeMethods.NMCUSTOMDRAW*)((void*)m.LParam);
					int dwDrawStage = ptr2->dwDrawStage;
					if (dwDrawStage == 1)
					{
						m.Result = (IntPtr)32;
						return true;
					}
					if (dwDrawStage != 65537)
					{
						return false;
					}
					Graphics graphics = Graphics.FromHdcInternal(ptr2->hdc);
					Rectangle bounds = Rectangle.FromLTRB(ptr2->rc.left, ptr2->rc.top, ptr2->rc.right, ptr2->rc.bottom);
					DrawListViewColumnHeaderEventArgs drawListViewColumnHeaderEventArgs = null;
					try
					{
						Color foreColor = ColorTranslator.FromWin32(SafeNativeMethods.GetTextColor(new HandleRef(this, ptr2->hdc)));
						Color backColor = ColorTranslator.FromWin32(SafeNativeMethods.GetBkColor(new HandleRef(this, ptr2->hdc)));
						Font listHeaderFont = this.GetListHeaderFont();
						drawListViewColumnHeaderEventArgs = new DrawListViewColumnHeaderEventArgs(graphics, bounds, (int)ptr2->dwItemSpec, this.columnHeaders[(int)ptr2->dwItemSpec], (ListViewItemStates)ptr2->uItemState, foreColor, backColor, listHeaderFont);
						this.OnDrawColumnHeader(drawListViewColumnHeaderEventArgs);
					}
					finally
					{
						graphics.Dispose();
					}
					if (drawListViewColumnHeaderEventArgs.DrawDefault)
					{
						m.Result = (IntPtr)0;
						return false;
					}
					m.Result = (IntPtr)4;
					return true;
				}
				catch (Exception ex)
				{
					m.Result = (IntPtr)0;
				}
			}
			if (ptr->code == -16 && this.listViewState[131072])
			{
				this.listViewState[131072] = false;
				this.OnColumnClick(new ColumnClickEventArgs(this.columnIndex));
			}
			if (ptr->code == -306 || ptr->code == -326)
			{
				this.listViewState[67108864] = true;
				this.listViewState1[2] = false;
				this.newWidthForColumnWidthChangingCancelled = -1;
				this.listViewState1[2] = false;
				NativeMethods.NMHEADER nmheader = (NativeMethods.NMHEADER)m.GetLParam(typeof(NativeMethods.NMHEADER));
				if (this.columnHeaders != null && this.columnHeaders.Length > nmheader.iItem)
				{
					this.columnHeaderClicked = this.columnHeaders[nmheader.iItem];
					this.columnHeaderClickedWidth = this.columnHeaderClicked.Width;
				}
				else
				{
					this.columnHeaderClickedWidth = -1;
					this.columnHeaderClicked = null;
				}
			}
			if (ptr->code == -300 || ptr->code == -320)
			{
				NativeMethods.NMHEADER nmheader2 = (NativeMethods.NMHEADER)m.GetLParam(typeof(NativeMethods.NMHEADER));
				if (this.columnHeaders != null && nmheader2.iItem < this.columnHeaders.Length && (this.listViewState[67108864] || this.listViewState[536870912]))
				{
					NativeMethods.HDITEM2 hditem = (NativeMethods.HDITEM2)UnsafeNativeMethods.PtrToStructure(nmheader2.pItem, typeof(NativeMethods.HDITEM2));
					int newWidth = ((hditem.mask & 1) != 0) ? hditem.cxy : -1;
					ColumnWidthChangingEventArgs columnWidthChangingEventArgs = new ColumnWidthChangingEventArgs(nmheader2.iItem, newWidth);
					this.OnColumnWidthChanging(columnWidthChangingEventArgs);
					m.Result = (IntPtr)(columnWidthChangingEventArgs.Cancel ? 1 : 0);
					if (columnWidthChangingEventArgs.Cancel)
					{
						hditem.cxy = columnWidthChangingEventArgs.NewWidth;
						if (this.listViewState[536870912])
						{
							this.listViewState[1073741824] = true;
						}
						this.listViewState1[2] = true;
						this.newWidthForColumnWidthChangingCancelled = columnWidthChangingEventArgs.NewWidth;
						return true;
					}
					return false;
				}
			}
			if ((ptr->code == -301 || ptr->code == -321) && !this.listViewState[67108864])
			{
				NativeMethods.NMHEADER nmheader3 = (NativeMethods.NMHEADER)m.GetLParam(typeof(NativeMethods.NMHEADER));
				if (this.columnHeaders != null && nmheader3.iItem < this.columnHeaders.Length)
				{
					int width = this.columnHeaders[nmheader3.iItem].Width;
					if (this.columnHeaderClicked == null || (this.columnHeaderClicked == this.columnHeaders[nmheader3.iItem] && this.columnHeaderClickedWidth != -1 && this.columnHeaderClickedWidth != width))
					{
						if (this.listViewState[536870912])
						{
							if (this.CompensateColumnHeaderResize(m, this.listViewState[1073741824]) == 0)
							{
								this.OnColumnWidthChanged(new ColumnWidthChangedEventArgs(nmheader3.iItem));
							}
						}
						else
						{
							this.OnColumnWidthChanged(new ColumnWidthChangedEventArgs(nmheader3.iItem));
						}
					}
				}
				this.columnHeaderClicked = null;
				this.columnHeaderClickedWidth = -1;
				ISite site = this.Site;
				if (site != null)
				{
					IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						try
						{
							componentChangeService.OnComponentChanging(this, null);
						}
						catch (CheckoutException ex2)
						{
							if (ex2 == CheckoutException.Canceled)
							{
								return false;
							}
							throw ex2;
						}
					}
				}
			}
			if (ptr->code == -307 || ptr->code == -327)
			{
				this.listViewState[67108864] = false;
				if (this.listViewState1[2])
				{
					m.Result = (IntPtr)1;
					if (this.newWidthForColumnWidthChangingCancelled != -1)
					{
						NativeMethods.NMHEADER nmheader4 = (NativeMethods.NMHEADER)m.GetLParam(typeof(NativeMethods.NMHEADER));
						if (this.columnHeaders != null && this.columnHeaders.Length > nmheader4.iItem)
						{
							this.columnHeaders[nmheader4.iItem].Width = this.newWidthForColumnWidthChangingCancelled;
						}
					}
					this.listViewState1[2] = false;
					this.newWidthForColumnWidthChangingCancelled = -1;
					return true;
				}
				return false;
			}
			else
			{
				if (ptr->code == -311)
				{
					NativeMethods.NMHEADER nmheader5 = (NativeMethods.NMHEADER)m.GetLParam(typeof(NativeMethods.NMHEADER));
					if (nmheader5.pItem != IntPtr.Zero)
					{
						NativeMethods.HDITEM2 hditem2 = (NativeMethods.HDITEM2)UnsafeNativeMethods.PtrToStructure(nmheader5.pItem, typeof(NativeMethods.HDITEM2));
						if ((hditem2.mask & 128) == 128)
						{
							int displayIndex = this.Columns[nmheader5.iItem].DisplayIndex;
							int iOrder = hditem2.iOrder;
							if (displayIndex == iOrder)
							{
								return false;
							}
							if (iOrder < 0)
							{
								return false;
							}
							ColumnReorderedEventArgs columnReorderedEventArgs = new ColumnReorderedEventArgs(displayIndex, iOrder, this.Columns[nmheader5.iItem]);
							this.OnColumnReordered(columnReorderedEventArgs);
							if (columnReorderedEventArgs.Cancel)
							{
								m.Result = new IntPtr(1);
								return true;
							}
							int num = Math.Min(displayIndex, iOrder);
							int num2 = Math.Max(displayIndex, iOrder);
							bool flag = iOrder > displayIndex;
							ColumnHeader columnHeader = null;
							int[] array = new int[this.Columns.Count];
							for (int i = 0; i < this.Columns.Count; i++)
							{
								ColumnHeader columnHeader2 = this.Columns[i];
								if (columnHeader2.DisplayIndex == displayIndex)
								{
									columnHeader = columnHeader2;
								}
								else if (columnHeader2.DisplayIndex >= num && columnHeader2.DisplayIndex <= num2)
								{
									columnHeader2.DisplayIndexInternal -= (flag ? 1 : -1);
								}
								array[i] = columnHeader2.DisplayIndexInternal;
							}
							columnHeader.DisplayIndexInternal = iOrder;
							array[columnHeader.Index] = columnHeader.DisplayIndexInternal;
							this.SetDisplayIndices(array);
						}
					}
				}
				if (ptr->code == -305 || ptr->code == -325)
				{
					this.listViewState[536870912] = true;
					this.listViewState[1073741824] = false;
					bool flag2 = false;
					try
					{
						this.DefWndProc(ref m);
					}
					finally
					{
						this.listViewState[536870912] = false;
						flag2 = this.listViewState[1073741824];
						this.listViewState[1073741824] = false;
					}
					this.columnHeaderClicked = null;
					this.columnHeaderClickedWidth = -1;
					if (flag2)
					{
						if (this.newWidthForColumnWidthChangingCancelled != -1)
						{
							NativeMethods.NMHEADER nmheader6 = (NativeMethods.NMHEADER)m.GetLParam(typeof(NativeMethods.NMHEADER));
							if (this.columnHeaders != null && this.columnHeaders.Length > nmheader6.iItem)
							{
								this.columnHeaders[nmheader6.iItem].Width = this.newWidthForColumnWidthChangingCancelled;
							}
						}
						m.Result = (IntPtr)1;
					}
					else
					{
						int num3 = this.CompensateColumnHeaderResize(m, flag2);
						if (num3 != 0)
						{
							ColumnHeader columnHeader3 = this.columnHeaders[0];
							columnHeader3.Width += num3;
						}
					}
					return true;
				}
				return false;
			}
			bool result;
			return result;
		}

		// Token: 0x06002A7A RID: 10874 RVA: 0x000C74EC File Offset: 0x000C56EC
		private Font GetListHeaderFont()
		{
			IntPtr handle = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4127, 0, 0);
			IntPtr hfont = UnsafeNativeMethods.SendMessage(new HandleRef(this, handle), 49, 0, 0);
			IntSecurity.ObjectFromWin32Handle.Assert();
			return Font.FromHfont(hfont);
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x000C7534 File Offset: 0x000C5734
		private int GetIndexOfClickedItem(NativeMethods.LVHITTESTINFO lvhi)
		{
			Point p = Cursor.Position;
			p = base.PointToClientInternal(p);
			lvhi.pt_x = p.X;
			lvhi.pt_y = p.Y;
			return (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4114, 0, lvhi);
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x000C7586 File Offset: 0x000C5786
		internal void RecreateHandleInternal()
		{
			if (base.IsHandleCreated && this.StateImageList != null)
			{
				base.SendMessage(4099, 2, IntPtr.Zero);
			}
			base.RecreateHandle();
		}

		// Token: 0x06002A7D RID: 10877 RVA: 0x000C75B0 File Offset: 0x000C57B0
		private unsafe void WmReflectNotify(ref Message m)
		{
			NativeMethods.NMHDR* ptr = (NativeMethods.NMHDR*)((void*)m.LParam);
			int code = ptr->code;
			if (code <= -155)
			{
				if (code == -176)
				{
					goto IL_150;
				}
				if (code != -175)
				{
					if (code != -155)
					{
						goto IL_650;
					}
					if (!this.CheckBoxes)
					{
						return;
					}
					NativeMethods.NMLVKEYDOWN nmlvkeydown = (NativeMethods.NMLVKEYDOWN)m.GetLParam(typeof(NativeMethods.NMLVKEYDOWN));
					if (nmlvkeydown.wVKey != 32)
					{
						return;
					}
					ListViewItem focusedItem = this.FocusedItem;
					if (focusedItem == null)
					{
						return;
					}
					bool @checked = !focusedItem.Checked;
					if (!this.VirtualMode)
					{
						using (IEnumerator enumerator = this.SelectedItems.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								ListViewItem listViewItem = (ListViewItem)obj;
								if (listViewItem != focusedItem)
								{
									listViewItem.Checked = @checked;
								}
							}
							return;
						}
						goto IL_61F;
					}
					return;
				}
			}
			else
			{
				switch (code)
				{
				case -114:
					this.OnItemActivate(EventArgs.Empty);
					return;
				case -113:
					goto IL_61F;
				case -112:
				case -110:
				case -107:
				case -104:
				case -103:
				case -102:
					goto IL_650;
				case -111:
					if (!this.ItemCollectionChangedInMouseDown)
					{
						NativeMethods.NMLISTVIEW nmlv = (NativeMethods.NMLISTVIEW)m.GetLParam(typeof(NativeMethods.NMLISTVIEW));
						this.LvnBeginDrag(MouseButtons.Right, nmlv);
						return;
					}
					return;
				case -109:
					if (!this.ItemCollectionChangedInMouseDown)
					{
						NativeMethods.NMLISTVIEW nmlv2 = (NativeMethods.NMLISTVIEW)m.GetLParam(typeof(NativeMethods.NMLISTVIEW));
						this.LvnBeginDrag(MouseButtons.Left, nmlv2);
						return;
					}
					return;
				case -108:
				{
					NativeMethods.NMLISTVIEW nmlistview = (NativeMethods.NMLISTVIEW)m.GetLParam(typeof(NativeMethods.NMLISTVIEW));
					this.listViewState[131072] = true;
					this.columnIndex = nmlistview.iSubItem;
					return;
				}
				case -106:
					goto IL_150;
				case -105:
					break;
				case -101:
				{
					NativeMethods.NMLISTVIEW* ptr2 = (NativeMethods.NMLISTVIEW*)((void*)m.LParam);
					if ((ptr2->uChanged & 8) == 0)
					{
						return;
					}
					CheckState checkState = ((ptr2->uOldState & 61440) >> 12 == 1) ? CheckState.Unchecked : CheckState.Checked;
					CheckState checkState2 = ((ptr2->uNewState & 61440) >> 12 == 1) ? CheckState.Unchecked : CheckState.Checked;
					if (checkState2 != checkState)
					{
						ItemCheckedEventArgs e = new ItemCheckedEventArgs(this.Items[ptr2->iItem]);
						this.OnItemChecked(e);
						if (AccessibilityImprovements.Level1)
						{
							base.AccessibilityNotifyClients(AccessibleEvents.StateChange, ptr2->iItem);
							base.AccessibilityNotifyClients(AccessibleEvents.NameChange, ptr2->iItem);
						}
					}
					int num = ptr2->uOldState & 2;
					int num2 = ptr2->uNewState & 2;
					if (num2 == num)
					{
						return;
					}
					if (this.VirtualMode && ptr2->iItem == -1)
					{
						if (this.VirtualListSize > 0)
						{
							ListViewVirtualItemsSelectionRangeChangedEventArgs e2 = new ListViewVirtualItemsSelectionRangeChangedEventArgs(0, this.VirtualListSize - 1, num2 != 0);
							this.OnVirtualItemsSelectionRangeChanged(e2);
						}
					}
					else if (this.Items.Count > 0)
					{
						ListViewItemSelectionChangedEventArgs e3 = new ListViewItemSelectionChangedEventArgs(this.Items[ptr2->iItem], ptr2->iItem, num2 != 0);
						this.OnItemSelectionChanged(e3);
					}
					if (this.Items.Count == 0 || this.Items[this.Items.Count - 1] != null)
					{
						this.listViewState1[16] = false;
						this.OnSelectedIndexChanged(EventArgs.Empty);
						return;
					}
					this.listViewState1[16] = true;
					return;
				}
				case -100:
				{
					NativeMethods.NMLISTVIEW* ptr3 = (NativeMethods.NMLISTVIEW*)((void*)m.LParam);
					if ((ptr3->uChanged & 8) == 0)
					{
						return;
					}
					CheckState checkState3 = ((ptr3->uOldState & 61440) >> 12 == 1) ? CheckState.Unchecked : CheckState.Checked;
					CheckState checkState4 = ((ptr3->uNewState & 61440) >> 12 == 1) ? CheckState.Unchecked : CheckState.Checked;
					if (checkState3 != checkState4)
					{
						ItemCheckEventArgs itemCheckEventArgs = new ItemCheckEventArgs(ptr3->iItem, checkState4, checkState3);
						this.OnItemCheck(itemCheckEventArgs);
						m.Result = (IntPtr)((((itemCheckEventArgs.NewValue == CheckState.Unchecked) ? CheckState.Unchecked : CheckState.Checked) == checkState3) ? 1 : 0);
						return;
					}
					return;
				}
				default:
					if (code != -12)
					{
						switch (code)
						{
						case -6:
							goto IL_53A;
						case -5:
							break;
						case -4:
							goto IL_650;
						case -3:
							this.WmNmDblClick(ref m);
							goto IL_53A;
						case -2:
							this.WmNmClick(ref m);
							break;
						default:
							goto IL_650;
						}
						NativeMethods.LVHITTESTINFO lvhi = new NativeMethods.LVHITTESTINFO();
						int indexOfClickedItem = this.GetIndexOfClickedItem(lvhi);
						MouseButtons button = (ptr->code == -2) ? MouseButtons.Left : MouseButtons.Right;
						Point p = Cursor.Position;
						p = base.PointToClientInternal(p);
						if (!base.ValidationCancelled && indexOfClickedItem != -1)
						{
							this.OnClick(EventArgs.Empty);
							this.OnMouseClick(new MouseEventArgs(button, 1, p.X, p.Y, 0));
						}
						if (!this.listViewState[524288])
						{
							this.OnMouseUp(new MouseEventArgs(button, 1, p.X, p.Y, 0));
							this.listViewState[524288] = true;
							return;
						}
						return;
						IL_53A:
						NativeMethods.LVHITTESTINFO lvhi2 = new NativeMethods.LVHITTESTINFO();
						int indexOfClickedItem2 = this.GetIndexOfClickedItem(lvhi2);
						if (indexOfClickedItem2 != -1)
						{
							this.listViewState[262144] = true;
						}
						this.listViewState[524288] = false;
						base.CaptureInternal = true;
						return;
					}
					this.CustomDraw(ref m);
					return;
				}
			}
			NativeMethods.NMLVDISPINFO_NOTEXT nmlvdispinfo_NOTEXT = (NativeMethods.NMLVDISPINFO_NOTEXT)m.GetLParam(typeof(NativeMethods.NMLVDISPINFO_NOTEXT));
			LabelEditEventArgs labelEditEventArgs = new LabelEditEventArgs(nmlvdispinfo_NOTEXT.item.iItem);
			this.OnBeforeLabelEdit(labelEditEventArgs);
			m.Result = (IntPtr)(labelEditEventArgs.CancelEdit ? 1 : 0);
			this.listViewState[16384] = !labelEditEventArgs.CancelEdit;
			return;
			IL_150:
			this.listViewState[16384] = false;
			NativeMethods.NMLVDISPINFO nmlvdispinfo = (NativeMethods.NMLVDISPINFO)m.GetLParam(typeof(NativeMethods.NMLVDISPINFO));
			LabelEditEventArgs labelEditEventArgs2 = new LabelEditEventArgs(nmlvdispinfo.item.iItem, nmlvdispinfo.item.pszText);
			this.OnAfterLabelEdit(labelEditEventArgs2);
			m.Result = (IntPtr)(labelEditEventArgs2.CancelEdit ? 0 : 1);
			if (!labelEditEventArgs2.CancelEdit && nmlvdispinfo.item.pszText != null)
			{
				this.Items[nmlvdispinfo.item.iItem].Text = nmlvdispinfo.item.pszText;
				return;
			}
			return;
			IL_61F:
			NativeMethods.NMLVCACHEHINT nmlvcachehint = (NativeMethods.NMLVCACHEHINT)m.GetLParam(typeof(NativeMethods.NMLVCACHEHINT));
			this.OnCacheVirtualItems(new CacheVirtualItemsEventArgs(nmlvcachehint.iFrom, nmlvcachehint.iTo));
			return;
			IL_650:
			if (ptr->code == NativeMethods.LVN_GETDISPINFO)
			{
				if (this.VirtualMode && m.LParam != IntPtr.Zero)
				{
					NativeMethods.NMLVDISPINFO_NOTEXT nmlvdispinfo_NOTEXT2 = (NativeMethods.NMLVDISPINFO_NOTEXT)m.GetLParam(typeof(NativeMethods.NMLVDISPINFO_NOTEXT));
					RetrieveVirtualItemEventArgs retrieveVirtualItemEventArgs = new RetrieveVirtualItemEventArgs(nmlvdispinfo_NOTEXT2.item.iItem);
					this.OnRetrieveVirtualItem(retrieveVirtualItemEventArgs);
					ListViewItem item = retrieveVirtualItemEventArgs.Item;
					if (item == null)
					{
						throw new InvalidOperationException(SR.GetString("ListViewVirtualItemRequired"));
					}
					item.SetItemIndex(this, nmlvdispinfo_NOTEXT2.item.iItem);
					if ((nmlvdispinfo_NOTEXT2.item.mask & 1) != 0)
					{
						string text;
						if (nmlvdispinfo_NOTEXT2.item.iSubItem == 0)
						{
							text = item.Text;
						}
						else
						{
							if (item.SubItems.Count <= nmlvdispinfo_NOTEXT2.item.iSubItem)
							{
								throw new InvalidOperationException(SR.GetString("ListViewVirtualModeCantAccessSubItem"));
							}
							text = item.SubItems[nmlvdispinfo_NOTEXT2.item.iSubItem].Text;
						}
						if (nmlvdispinfo_NOTEXT2.item.cchTextMax <= text.Length)
						{
							text = text.Substring(0, nmlvdispinfo_NOTEXT2.item.cchTextMax - 1);
						}
						if (Marshal.SystemDefaultCharSize == 1)
						{
							byte[] bytes = Encoding.Default.GetBytes(text + "\0");
							Marshal.Copy(bytes, 0, nmlvdispinfo_NOTEXT2.item.pszText, text.Length + 1);
						}
						else
						{
							char[] source = (text + "\0").ToCharArray();
							Marshal.Copy(source, 0, nmlvdispinfo_NOTEXT2.item.pszText, text.Length + 1);
						}
					}
					if ((nmlvdispinfo_NOTEXT2.item.mask & 2) != 0 && item.ImageIndex != -1)
					{
						nmlvdispinfo_NOTEXT2.item.iImage = item.ImageIndex;
					}
					if ((nmlvdispinfo_NOTEXT2.item.mask & 16) != 0)
					{
						nmlvdispinfo_NOTEXT2.item.iIndent = item.IndentCount;
					}
					if ((nmlvdispinfo_NOTEXT2.item.stateMask & 61440) != 0)
					{
						NativeMethods.NMLVDISPINFO_NOTEXT nmlvdispinfo_NOTEXT3 = nmlvdispinfo_NOTEXT2;
						nmlvdispinfo_NOTEXT3.item.state = (nmlvdispinfo_NOTEXT3.item.state | item.RawStateImageIndex);
					}
					Marshal.StructureToPtr(nmlvdispinfo_NOTEXT2, m.LParam, false);
					return;
				}
			}
			else if (ptr->code == -115)
			{
				if (this.VirtualMode && m.LParam != IntPtr.Zero)
				{
					NativeMethods.NMLVODSTATECHANGE nmlvodstatechange = (NativeMethods.NMLVODSTATECHANGE)m.GetLParam(typeof(NativeMethods.NMLVODSTATECHANGE));
					bool flag = (nmlvodstatechange.uNewState & 2) != (nmlvodstatechange.uOldState & 2);
					if (flag)
					{
						int num3 = nmlvodstatechange.iTo;
						if (!UnsafeNativeMethods.IsVista)
						{
							num3--;
						}
						ListViewVirtualItemsSelectionRangeChangedEventArgs e4 = new ListViewVirtualItemsSelectionRangeChangedEventArgs(nmlvodstatechange.iFrom, num3, (nmlvodstatechange.uNewState & 2) != 0);
						this.OnVirtualItemsSelectionRangeChanged(e4);
						return;
					}
				}
			}
			else if (ptr->code == NativeMethods.LVN_GETINFOTIP)
			{
				if (this.ShowItemToolTips && m.LParam != IntPtr.Zero)
				{
					NativeMethods.NMLVGETINFOTIP nmlvgetinfotip = (NativeMethods.NMLVGETINFOTIP)m.GetLParam(typeof(NativeMethods.NMLVGETINFOTIP));
					ListViewItem listViewItem2 = this.Items[nmlvgetinfotip.item];
					if (listViewItem2 != null && !string.IsNullOrEmpty(listViewItem2.ToolTipText))
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, ptr->hwndFrom), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
						if (Marshal.SystemDefaultCharSize == 1)
						{
							byte[] bytes2 = Encoding.Default.GetBytes(listViewItem2.ToolTipText + "\0");
							Marshal.Copy(bytes2, 0, nmlvgetinfotip.lpszText, Math.Min(bytes2.Length, nmlvgetinfotip.cchTextMax));
						}
						else
						{
							char[] array = (listViewItem2.ToolTipText + "\0").ToCharArray();
							Marshal.Copy(array, 0, nmlvgetinfotip.lpszText, Math.Min(array.Length, nmlvgetinfotip.cchTextMax));
						}
						Marshal.StructureToPtr(nmlvgetinfotip, m.LParam, false);
						return;
					}
				}
			}
			else if (ptr->code == NativeMethods.LVN_ODFINDITEM && this.VirtualMode)
			{
				NativeMethods.NMLVFINDITEM nmlvfinditem = (NativeMethods.NMLVFINDITEM)m.GetLParam(typeof(NativeMethods.NMLVFINDITEM));
				if ((nmlvfinditem.lvfi.flags & 1) != 0)
				{
					m.Result = (IntPtr)(-1);
					return;
				}
				bool flag2 = (nmlvfinditem.lvfi.flags & 2) != 0 || (nmlvfinditem.lvfi.flags & 8) != 0;
				bool isPrefixSearch = (nmlvfinditem.lvfi.flags & 8) != 0;
				string text2 = string.Empty;
				if (flag2)
				{
					text2 = nmlvfinditem.lvfi.psz;
				}
				Point empty = Point.Empty;
				if ((nmlvfinditem.lvfi.flags & 64) != 0)
				{
					empty = new Point(nmlvfinditem.lvfi.ptX, nmlvfinditem.lvfi.ptY);
				}
				SearchDirectionHint direction = SearchDirectionHint.Down;
				if ((nmlvfinditem.lvfi.flags & 64) != 0)
				{
					direction = (SearchDirectionHint)nmlvfinditem.lvfi.vkDirection;
				}
				int iStart = nmlvfinditem.iStart;
				if (iStart >= this.VirtualListSize)
				{
				}
				SearchForVirtualItemEventArgs searchForVirtualItemEventArgs = new SearchForVirtualItemEventArgs(flag2, isPrefixSearch, false, text2, empty, direction, nmlvfinditem.iStart);
				this.OnSearchForVirtualItem(searchForVirtualItemEventArgs);
				if (searchForVirtualItemEventArgs.Index != -1)
				{
					m.Result = (IntPtr)searchForVirtualItemEventArgs.Index;
					return;
				}
				m.Result = (IntPtr)(-1);
			}
		}

		// Token: 0x06002A7E RID: 10878 RVA: 0x000C816C File Offset: 0x000C636C
		private void WmPrint(ref Message m)
		{
			base.WndProc(ref m);
			if ((2 & (int)m.LParam) != 0 && Application.RenderWithVisualStyles && this.BorderStyle == BorderStyle.Fixed3D)
			{
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					using (Graphics graphics = Graphics.FromHdc(m.WParam))
					{
						Rectangle rect = new Rectangle(0, 0, base.Size.Width - 1, base.Size.Height - 1);
						graphics.DrawRectangle(new Pen(VisualStyleInformation.TextControlBorder), rect);
						rect.Inflate(-1, -1);
						graphics.DrawRectangle(SystemPens.Window, rect);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		/// <summary>Overrides <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" />.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06002A7F RID: 10879 RVA: 0x000C8238 File Offset: 0x000C6438
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 275)
			{
				if (msg <= 15)
				{
					if (msg != 7)
					{
						if (msg == 15)
						{
							base.WndProc(ref m);
							base.BeginInvoke(new MethodInvoker(this.CleanPreviousBackgroundImageFiles));
							return;
						}
					}
					else
					{
						base.WndProc(ref m);
						if (!base.RecreatingHandle && !this.ListViewHandleDestroyed && this.FocusedItem == null && this.Items.Count > 0)
						{
							this.Items[0].Focused = true;
							return;
						}
						return;
					}
				}
				else if (msg != 78)
				{
					if (msg == 275)
					{
						if ((int)((long)m.WParam) != 48 || !this.ComctlSupportsVisualStyles)
						{
							base.WndProc(ref m);
							return;
						}
						return;
					}
				}
				else if (this.WmNotify(ref m))
				{
					return;
				}
			}
			else if (msg <= 673)
			{
				switch (msg)
				{
				case 512:
					if (this.listViewState[1048576] && !this.listViewState[524288] && Control.MouseButtons == MouseButtons.None)
					{
						this.OnMouseUp(new MouseEventArgs(this.downButton, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
						this.listViewState[524288] = true;
					}
					base.CaptureInternal = false;
					base.WndProc(ref m);
					return;
				case 513:
					this.ItemCollectionChangedInMouseDown = false;
					this.WmMouseDown(ref m, MouseButtons.Left, 1);
					this.downButton = MouseButtons.Left;
					return;
				case 514:
				case 517:
				case 520:
				{
					NativeMethods.LVHITTESTINFO lvhi = new NativeMethods.LVHITTESTINFO();
					int indexOfClickedItem = this.GetIndexOfClickedItem(lvhi);
					if (!base.ValidationCancelled && this.listViewState[262144] && indexOfClickedItem != -1)
					{
						this.listViewState[262144] = false;
						this.OnDoubleClick(EventArgs.Empty);
						this.OnMouseDoubleClick(new MouseEventArgs(this.downButton, 2, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
					}
					if (!this.listViewState[524288])
					{
						this.OnMouseUp(new MouseEventArgs(this.downButton, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
						this.listViewState[1048576] = false;
					}
					this.ItemCollectionChangedInMouseDown = false;
					this.listViewState[524288] = true;
					base.CaptureInternal = false;
					return;
				}
				case 515:
					this.ItemCollectionChangedInMouseDown = false;
					base.CaptureInternal = true;
					this.WmMouseDown(ref m, MouseButtons.Left, 2);
					return;
				case 516:
					this.WmMouseDown(ref m, MouseButtons.Right, 1);
					this.downButton = MouseButtons.Right;
					return;
				case 518:
					this.WmMouseDown(ref m, MouseButtons.Right, 2);
					return;
				case 519:
					this.WmMouseDown(ref m, MouseButtons.Middle, 1);
					this.downButton = MouseButtons.Middle;
					return;
				case 521:
					this.WmMouseDown(ref m, MouseButtons.Middle, 2);
					return;
				default:
					if (msg == 673)
					{
						if (this.HoverSelection)
						{
							base.WndProc(ref m);
							return;
						}
						this.OnMouseHover(EventArgs.Empty);
						return;
					}
					break;
				}
			}
			else
			{
				if (msg == 675)
				{
					this.prevHoveredItem = null;
					base.WndProc(ref m);
					return;
				}
				if (msg == 791)
				{
					this.WmPrint(ref m);
					return;
				}
				if (msg == 8270)
				{
					this.WmReflectNotify(ref m);
					return;
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x06002A80 RID: 10880 RVA: 0x000C859D File Offset: 0x000C679D
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level3)
			{
				return new ListView.ListViewAccessibleObject(this);
			}
			return base.CreateAccessibilityInstance();
		}

		// Token: 0x040011DF RID: 4575
		private const int MASK_HITTESTFLAG = 247;

		// Token: 0x040011E0 RID: 4576
		private static readonly object EVENT_CACHEVIRTUALITEMS = new object();

		// Token: 0x040011E1 RID: 4577
		private static readonly object EVENT_COLUMNREORDERED = new object();

		// Token: 0x040011E2 RID: 4578
		private static readonly object EVENT_COLUMNWIDTHCHANGED = new object();

		// Token: 0x040011E3 RID: 4579
		private static readonly object EVENT_COLUMNWIDTHCHANGING = new object();

		// Token: 0x040011E4 RID: 4580
		private static readonly object EVENT_DRAWCOLUMNHEADER = new object();

		// Token: 0x040011E5 RID: 4581
		private static readonly object EVENT_DRAWITEM = new object();

		// Token: 0x040011E6 RID: 4582
		private static readonly object EVENT_DRAWSUBITEM = new object();

		// Token: 0x040011E7 RID: 4583
		private static readonly object EVENT_ITEMSELECTIONCHANGED = new object();

		// Token: 0x040011E8 RID: 4584
		private static readonly object EVENT_RETRIEVEVIRTUALITEM = new object();

		// Token: 0x040011E9 RID: 4585
		private static readonly object EVENT_SEARCHFORVIRTUALITEM = new object();

		// Token: 0x040011EA RID: 4586
		private static readonly object EVENT_SELECTEDINDEXCHANGED = new object();

		// Token: 0x040011EB RID: 4587
		private static readonly object EVENT_VIRTUALITEMSSELECTIONRANGECHANGED = new object();

		// Token: 0x040011EC RID: 4588
		private static readonly object EVENT_RIGHTTOLEFTLAYOUTCHANGED = new object();

		// Token: 0x040011ED RID: 4589
		private ItemActivation activation;

		// Token: 0x040011EE RID: 4590
		private ListViewAlignment alignStyle = ListViewAlignment.Top;

		// Token: 0x040011EF RID: 4591
		private BorderStyle borderStyle = BorderStyle.Fixed3D;

		// Token: 0x040011F0 RID: 4592
		private ColumnHeaderStyle headerStyle = ColumnHeaderStyle.Clickable;

		// Token: 0x040011F1 RID: 4593
		private SortOrder sorting;

		// Token: 0x040011F2 RID: 4594
		private View viewStyle;

		// Token: 0x040011F3 RID: 4595
		private string toolTipCaption = string.Empty;

		// Token: 0x040011F4 RID: 4596
		private const int LISTVIEWSTATE_ownerDraw = 1;

		// Token: 0x040011F5 RID: 4597
		private const int LISTVIEWSTATE_allowColumnReorder = 2;

		// Token: 0x040011F6 RID: 4598
		private const int LISTVIEWSTATE_autoArrange = 4;

		// Token: 0x040011F7 RID: 4599
		private const int LISTVIEWSTATE_checkBoxes = 8;

		// Token: 0x040011F8 RID: 4600
		private const int LISTVIEWSTATE_fullRowSelect = 16;

		// Token: 0x040011F9 RID: 4601
		private const int LISTVIEWSTATE_gridLines = 32;

		// Token: 0x040011FA RID: 4602
		private const int LISTVIEWSTATE_hideSelection = 64;

		// Token: 0x040011FB RID: 4603
		private const int LISTVIEWSTATE_hotTracking = 128;

		// Token: 0x040011FC RID: 4604
		private const int LISTVIEWSTATE_labelEdit = 256;

		// Token: 0x040011FD RID: 4605
		private const int LISTVIEWSTATE_labelWrap = 512;

		// Token: 0x040011FE RID: 4606
		private const int LISTVIEWSTATE_multiSelect = 1024;

		// Token: 0x040011FF RID: 4607
		private const int LISTVIEWSTATE_scrollable = 2048;

		// Token: 0x04001200 RID: 4608
		private const int LISTVIEWSTATE_hoverSelection = 4096;

		// Token: 0x04001201 RID: 4609
		private const int LISTVIEWSTATE_nonclickHdr = 8192;

		// Token: 0x04001202 RID: 4610
		private const int LISTVIEWSTATE_inLabelEdit = 16384;

		// Token: 0x04001203 RID: 4611
		private const int LISTVIEWSTATE_showItemToolTips = 32768;

		// Token: 0x04001204 RID: 4612
		private const int LISTVIEWSTATE_backgroundImageTiled = 65536;

		// Token: 0x04001205 RID: 4613
		private const int LISTVIEWSTATE_columnClicked = 131072;

		// Token: 0x04001206 RID: 4614
		private const int LISTVIEWSTATE_doubleclickFired = 262144;

		// Token: 0x04001207 RID: 4615
		private const int LISTVIEWSTATE_mouseUpFired = 524288;

		// Token: 0x04001208 RID: 4616
		private const int LISTVIEWSTATE_expectingMouseUp = 1048576;

		// Token: 0x04001209 RID: 4617
		private const int LISTVIEWSTATE_comctlSupportsVisualStyles = 2097152;

		// Token: 0x0400120A RID: 4618
		private const int LISTVIEWSTATE_comctlSupportsVisualStylesTested = 4194304;

		// Token: 0x0400120B RID: 4619
		private const int LISTVIEWSTATE_showGroups = 8388608;

		// Token: 0x0400120C RID: 4620
		private const int LISTVIEWSTATE_handleDestroyed = 16777216;

		// Token: 0x0400120D RID: 4621
		private const int LISTVIEWSTATE_virtualMode = 33554432;

		// Token: 0x0400120E RID: 4622
		private const int LISTVIEWSTATE_headerControlTracking = 67108864;

		// Token: 0x0400120F RID: 4623
		private const int LISTVIEWSTATE_itemCollectionChangedInMouseDown = 134217728;

		// Token: 0x04001210 RID: 4624
		private const int LISTVIEWSTATE_flipViewToLargeIconAndSmallIcon = 268435456;

		// Token: 0x04001211 RID: 4625
		private const int LISTVIEWSTATE_headerDividerDblClick = 536870912;

		// Token: 0x04001212 RID: 4626
		private const int LISTVIEWSTATE_columnResizeCancelled = 1073741824;

		// Token: 0x04001213 RID: 4627
		private const int LISTVIEWSTATE1_insertingItemsNatively = 1;

		// Token: 0x04001214 RID: 4628
		private const int LISTVIEWSTATE1_cancelledColumnWidthChanging = 2;

		// Token: 0x04001215 RID: 4629
		private const int LISTVIEWSTATE1_disposingImageLists = 4;

		// Token: 0x04001216 RID: 4630
		private const int LISTVIEWSTATE1_useCompatibleStateImageBehavior = 8;

		// Token: 0x04001217 RID: 4631
		private const int LISTVIEWSTATE1_selectedIndexChangedSkipped = 16;

		// Token: 0x04001218 RID: 4632
		private const int LVTOOLTIPTRACKING = 48;

		// Token: 0x04001219 RID: 4633
		private const int MAXTILECOLUMNS = 20;

		// Token: 0x0400121A RID: 4634
		private BitVector32 listViewState;

		// Token: 0x0400121B RID: 4635
		private BitVector32 listViewState1;

		// Token: 0x0400121C RID: 4636
		private Color odCacheForeColor = SystemColors.WindowText;

		// Token: 0x0400121D RID: 4637
		private Color odCacheBackColor = SystemColors.Window;

		// Token: 0x0400121E RID: 4638
		private Font odCacheFont;

		// Token: 0x0400121F RID: 4639
		private IntPtr odCacheFontHandle = IntPtr.Zero;

		// Token: 0x04001220 RID: 4640
		private Control.FontHandleWrapper odCacheFontHandleWrapper;

		// Token: 0x04001221 RID: 4641
		private ImageList imageListLarge;

		// Token: 0x04001222 RID: 4642
		private ImageList imageListSmall;

		// Token: 0x04001223 RID: 4643
		private ImageList imageListState;

		// Token: 0x04001224 RID: 4644
		private MouseButtons downButton;

		// Token: 0x04001225 RID: 4645
		private int itemCount;

		// Token: 0x04001226 RID: 4646
		private int columnIndex;

		// Token: 0x04001227 RID: 4647
		private int topIndex;

		// Token: 0x04001228 RID: 4648
		private bool hoveredAlready;

		// Token: 0x04001229 RID: 4649
		private bool rightToLeftLayout;

		// Token: 0x0400122A RID: 4650
		private int virtualListSize;

		// Token: 0x0400122B RID: 4651
		private ListViewGroup defaultGroup;

		// Token: 0x0400122C RID: 4652
		private Hashtable listItemsTable = new Hashtable();

		// Token: 0x0400122D RID: 4653
		private ArrayList listItemsArray = new ArrayList();

		// Token: 0x0400122E RID: 4654
		private Size tileSize = Size.Empty;

		// Token: 0x0400122F RID: 4655
		private static readonly int PropDelayedUpdateItems = PropertyStore.CreateKey();

		// Token: 0x04001230 RID: 4656
		private int updateCounter;

		// Token: 0x04001231 RID: 4657
		private ColumnHeader[] columnHeaders;

		// Token: 0x04001232 RID: 4658
		private ListView.ListViewItemCollection listItemCollection;

		// Token: 0x04001233 RID: 4659
		private ListView.ColumnHeaderCollection columnHeaderCollection;

		// Token: 0x04001234 RID: 4660
		private ListView.CheckedIndexCollection checkedIndexCollection;

		// Token: 0x04001235 RID: 4661
		private ListView.CheckedListViewItemCollection checkedListViewItemCollection;

		// Token: 0x04001236 RID: 4662
		private ListView.SelectedListViewItemCollection selectedListViewItemCollection;

		// Token: 0x04001237 RID: 4663
		private ListView.SelectedIndexCollection selectedIndexCollection;

		// Token: 0x04001238 RID: 4664
		private ListViewGroupCollection groups;

		// Token: 0x04001239 RID: 4665
		private ListViewInsertionMark insertionMark;

		// Token: 0x0400123A RID: 4666
		private LabelEditEventHandler onAfterLabelEdit;

		// Token: 0x0400123B RID: 4667
		private LabelEditEventHandler onBeforeLabelEdit;

		// Token: 0x0400123C RID: 4668
		private ColumnClickEventHandler onColumnClick;

		// Token: 0x0400123D RID: 4669
		private EventHandler onItemActivate;

		// Token: 0x0400123E RID: 4670
		private ItemCheckedEventHandler onItemChecked;

		// Token: 0x0400123F RID: 4671
		private ItemDragEventHandler onItemDrag;

		// Token: 0x04001240 RID: 4672
		private ItemCheckEventHandler onItemCheck;

		// Token: 0x04001241 RID: 4673
		private ListViewItemMouseHoverEventHandler onItemMouseHover;

		// Token: 0x04001242 RID: 4674
		private int nextID;

		// Token: 0x04001243 RID: 4675
		private List<ListViewItem> savedSelectedItems;

		// Token: 0x04001244 RID: 4676
		private List<ListViewItem> savedCheckedItems;

		// Token: 0x04001245 RID: 4677
		private IComparer listItemSorter;

		// Token: 0x04001246 RID: 4678
		private ListViewItem prevHoveredItem;

		// Token: 0x04001247 RID: 4679
		private string backgroundImageFileName = string.Empty;

		// Token: 0x04001248 RID: 4680
		private int bkImgFileNamesCount = -1;

		// Token: 0x04001249 RID: 4681
		private string[] bkImgFileNames;

		// Token: 0x0400124A RID: 4682
		private const int BKIMGARRAYSIZE = 8;

		// Token: 0x0400124B RID: 4683
		private ColumnHeader columnHeaderClicked;

		// Token: 0x0400124C RID: 4684
		private int columnHeaderClickedWidth;

		// Token: 0x0400124D RID: 4685
		private int newWidthForColumnWidthChangingCancelled = -1;

		// Token: 0x0200060A RID: 1546
		internal class IconComparer : IComparer
		{
			// Token: 0x06005CC9 RID: 23753 RVA: 0x0018224E File Offset: 0x0018044E
			public IconComparer(SortOrder currentSortOrder)
			{
				this.sortOrder = currentSortOrder;
			}

			// Token: 0x1700163A RID: 5690
			// (set) Token: 0x06005CCA RID: 23754 RVA: 0x0018225D File Offset: 0x0018045D
			public SortOrder SortOrder
			{
				set
				{
					this.sortOrder = value;
				}
			}

			// Token: 0x06005CCB RID: 23755 RVA: 0x00182268 File Offset: 0x00180468
			public int Compare(object obj1, object obj2)
			{
				ListViewItem listViewItem = (ListViewItem)obj1;
				ListViewItem listViewItem2 = (ListViewItem)obj2;
				if (this.sortOrder == SortOrder.Ascending)
				{
					return string.Compare(listViewItem.Text, listViewItem2.Text, false, CultureInfo.CurrentCulture);
				}
				return string.Compare(listViewItem2.Text, listViewItem.Text, false, CultureInfo.CurrentCulture);
			}

			// Token: 0x04003A00 RID: 14848
			private SortOrder sortOrder;
		}

		/// <summary>Represents the collection containing the indexes to the checked items in a list view control.</summary>
		// Token: 0x0200060B RID: 1547
		[ListBindable(false)]
		public class CheckedIndexCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListView.CheckedIndexCollection" /> class.</summary>
			/// <param name="owner">A <see cref="T:System.Windows.Forms.ListView" /> control that owns the collection. </param>
			// Token: 0x06005CCC RID: 23756 RVA: 0x001822BB File Offset: 0x001804BB
			public CheckedIndexCollection(ListView owner)
			{
				this.owner = owner;
			}

			/// <summary>Gets the number of items in the collection.</summary>
			/// <returns>The number of items in the collection.</returns>
			// Token: 0x1700163B RID: 5691
			// (get) Token: 0x06005CCD RID: 23757 RVA: 0x001822CC File Offset: 0x001804CC
			[Browsable(false)]
			public int Count
			{
				get
				{
					if (!this.owner.CheckBoxes)
					{
						return 0;
					}
					int num = 0;
					foreach (object obj in this.owner.Items)
					{
						ListViewItem listViewItem = (ListViewItem)obj;
						if (listViewItem != null && listViewItem.Checked)
						{
							num++;
						}
					}
					return num;
				}
			}

			// Token: 0x1700163C RID: 5692
			// (get) Token: 0x06005CCE RID: 23758 RVA: 0x00182344 File Offset: 0x00180544
			private int[] IndicesArray
			{
				get
				{
					int[] array = new int[this.Count];
					int num = 0;
					int num2 = 0;
					while (num2 < this.owner.Items.Count && num < array.Length)
					{
						if (this.owner.Items[num2].Checked)
						{
							array[num++] = num2;
						}
						num2++;
					}
					return array;
				}
			}

			/// <summary>Gets the index value at the specified index within the collection.</summary>
			/// <param name="index">The index of the item in the collection to retrieve. </param>
			/// <returns>The index value from the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> that is stored at the specified location.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than 0 or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListView.CheckedIndexCollection.Count" /> property of <see cref="T:System.Windows.Forms.ListView.CheckedIndexCollection" />. </exception>
			// Token: 0x1700163D RID: 5693
			public int this[int index]
			{
				get
				{
					if (index < 0)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					int count = this.owner.Items.Count;
					int num = 0;
					for (int i = 0; i < count; i++)
					{
						ListViewItem listViewItem = this.owner.Items[i];
						if (listViewItem.Checked)
						{
							if (num == index)
							{
								return i;
							}
							num++;
						}
					}
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
			}

			/// <summary>Gets or sets an object in the <see cref="T:System.Windows.Forms.ListView.CheckedIndexCollection" />.</summary>
			/// <param name="index">The zero-based index of the element to get or set.</param>
			/// <returns>The object from the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> that is stored at the specified location.</returns>
			// Token: 0x1700163E RID: 5694
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the collection of controls.</summary>
			/// <returns>The object used to synchronize the collection.</returns>
			// Token: 0x1700163F RID: 5695
			// (get) Token: 0x06005CD2 RID: 23762 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x17001640 RID: 5696
			// (get) Token: 0x06005CD3 RID: 23763 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.ListView.CheckedIndexCollection" /> has a fixed size.</summary>
			/// <returns>
			///     <see langword="true" /> in all cases.</returns>
			// Token: 0x17001641 RID: 5697
			// (get) Token: 0x06005CD4 RID: 23764 RVA: 0x0000E214 File Offset: 0x0000C414
			bool IList.IsFixedSize
			{
				get
				{
					return true;
				}
			}

			/// <summary>Gets a value indicating whether the collection is read-only.</summary>
			/// <returns>
			///     <see langword="true" /> if the collection is read-only; otherwise, <see langword="false" />.</returns>
			// Token: 0x17001642 RID: 5698
			// (get) Token: 0x06005CD5 RID: 23765 RVA: 0x0000E214 File Offset: 0x0000C414
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			/// <summary>Determines whether the specified index is located in the collection.</summary>
			/// <param name="checkedIndex">The index to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the specified index from the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> for the <see cref="T:System.Windows.Forms.ListView" /> is an item in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005CD6 RID: 23766 RVA: 0x00182469 File Offset: 0x00180669
			public bool Contains(int checkedIndex)
			{
				return this.owner.Items[checkedIndex].Checked;
			}

			/// <summary>Checks whether the index corresponding with the <see cref="T:System.Windows.Forms.ListViewItem" /> is checked.</summary>
			/// <param name="checkedIndex">An index to locate in the <see cref="T:System.Windows.Forms.ListView.CheckedIndexCollection" />.</param>
			/// <returns>
			///     <see langword="true" /> if the index is found in the <see cref="T:System.Windows.Forms.ListView.CheckedIndexCollection" />; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005CD7 RID: 23767 RVA: 0x00182486 File Offset: 0x00180686
			bool IList.Contains(object checkedIndex)
			{
				return checkedIndex is int && this.Contains((int)checkedIndex);
			}

			/// <summary>Returns the index within the <see cref="T:System.Windows.Forms.ListView.CheckedIndexCollection" /> of the specified index from the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> of the list view control.</summary>
			/// <param name="checkedIndex">The zero-based index from the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> to locate in the collection. </param>
			/// <returns>The zero-based index in the collection where the specified index of the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> is located within the <see cref="T:System.Windows.Forms.ListView.CheckedIndexCollection" />; otherwise, -1 if the index is not located in the collection.</returns>
			// Token: 0x06005CD8 RID: 23768 RVA: 0x001824A0 File Offset: 0x001806A0
			public int IndexOf(int checkedIndex)
			{
				int[] indicesArray = this.IndicesArray;
				for (int i = 0; i < indicesArray.Length; i++)
				{
					if (indicesArray[i] == checkedIndex)
					{
						return i;
					}
				}
				return -1;
			}

			/// <summary>Returns the index of the specified object in the <see cref="T:System.Windows.Forms.ListView.CheckedIndexCollection" />. </summary>
			/// <param name="checkedIndex">The zero-based index from the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> to locate in the collection.</param>
			/// <returns>The zero-based index in the collection where the specified index of the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> is located if it is in the <see cref="T:System.Windows.Forms.ListView.CheckedIndexCollection" />; otherwise, -1.</returns>
			// Token: 0x06005CD9 RID: 23769 RVA: 0x001824CB File Offset: 0x001806CB
			int IList.IndexOf(object checkedIndex)
			{
				if (checkedIndex is int)
				{
					return this.IndexOf((int)checkedIndex);
				}
				return -1;
			}

			/// <summary>Adds an item to the collection.</summary>
			/// <param name="value">The object to add to the <see cref="T:System.Windows.Forms.ListView.CheckedIndexCollection" />.</param>
			/// <returns>The zero-based index where <paramref name="value" /> is located in the collection.</returns>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005CDA RID: 23770 RVA: 0x0000A2AB File Offset: 0x000084AB
			int IList.Add(object value)
			{
				throw new NotSupportedException();
			}

			/// <summary>Removes all items from the collection.</summary>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005CDB RID: 23771 RVA: 0x0000A2AB File Offset: 0x000084AB
			void IList.Clear()
			{
				throw new NotSupportedException();
			}

			/// <summary>Inserts an item into the collection at a specified index.</summary>
			/// <param name="index">The index at which <paramref name="value" /> should be inserted.</param>
			/// <param name="value">The object to be added to the <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005CDC RID: 23772 RVA: 0x0000A2AB File Offset: 0x000084AB
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			/// <summary>Removes the first occurrence of an item from the collection.</summary>
			/// <param name="value">The object to be removed from the <see cref="T:System.Windows.Forms.ListView.CheckedIndexCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005CDD RID: 23773 RVA: 0x0000A2AB File Offset: 0x000084AB
			void IList.Remove(object value)
			{
				throw new NotSupportedException();
			}

			/// <summary>Removes an item from the collection at a specified index.</summary>
			/// <param name="index">The zero-based index of the item to remove.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005CDE RID: 23774 RVA: 0x0000A2AB File Offset: 0x000084AB
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			/// <summary>Copies the collection of checked-item indexes into an array.</summary>
			/// <param name="dest">An array of type <see cref="T:System.Int32" />.</param>
			/// <param name="index">The zero-based index in the array at which copying begins. </param>
			/// <exception cref="T:System.ArrayTypeMismatchException">The array type cannot be cast to an <see cref="T:System.Int32" />.</exception>
			// Token: 0x06005CDF RID: 23775 RVA: 0x001824E3 File Offset: 0x001806E3
			void ICollection.CopyTo(Array dest, int index)
			{
				if (this.Count > 0)
				{
					Array.Copy(this.IndicesArray, 0, dest, index, this.Count);
				}
			}

			/// <summary>Returns an enumerator that can be used to iterate through the checked index collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the checked index collection.</returns>
			// Token: 0x06005CE0 RID: 23776 RVA: 0x00182504 File Offset: 0x00180704
			public IEnumerator GetEnumerator()
			{
				int[] indicesArray = this.IndicesArray;
				if (indicesArray != null)
				{
					return indicesArray.GetEnumerator();
				}
				return new int[0].GetEnumerator();
			}

			// Token: 0x04003A01 RID: 14849
			private ListView owner;
		}

		/// <summary>Represents the collection of checked items in a list view control.</summary>
		// Token: 0x0200060C RID: 1548
		[ListBindable(false)]
		public class CheckedListViewItemCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListView.CheckedListViewItemCollection" /> class.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.ListView" /> control that owns the collection. </param>
			// Token: 0x06005CE1 RID: 23777 RVA: 0x0018252D File Offset: 0x0018072D
			public CheckedListViewItemCollection(ListView owner)
			{
				this.owner = owner;
			}

			/// <summary>Gets the number of items in the collection.</summary>
			/// <returns>The number of items in the collection.</returns>
			// Token: 0x17001643 RID: 5699
			// (get) Token: 0x06005CE2 RID: 23778 RVA: 0x00182543 File Offset: 0x00180743
			[Browsable(false)]
			public int Count
			{
				get
				{
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
					}
					return this.owner.CheckedIndices.Count;
				}
			}

			// Token: 0x17001644 RID: 5700
			// (get) Token: 0x06005CE3 RID: 23779 RVA: 0x00182574 File Offset: 0x00180774
			private ListViewItem[] ItemArray
			{
				get
				{
					ListViewItem[] array = new ListViewItem[this.Count];
					int num = 0;
					int num2 = 0;
					while (num2 < this.owner.Items.Count && num < array.Length)
					{
						if (this.owner.Items[num2].Checked)
						{
							array[num++] = this.owner.Items[num2];
						}
						num2++;
					}
					return array;
				}
			}

			/// <summary>Gets the item at the specified index within the collection.</summary>
			/// <param name="index">The index of the item in the collection to retrieve. </param>
			/// <returns>A <see cref="T:System.Windows.Forms.ListViewItem" /> representing the item located at the specified index within the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than 0 or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListView.CheckedListViewItemCollection.Count" /> property of <see cref="T:System.Windows.Forms.ListView.CheckedListViewItemCollection" />. </exception>
			/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Forms.ListView" /> is in virtual mode.</exception>
			// Token: 0x17001645 RID: 5701
			public ListViewItem this[int index]
			{
				get
				{
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
					}
					int index2 = this.owner.CheckedIndices[index];
					return this.owner.Items[index2];
				}
			}

			/// <summary>Gets or sets an object from the collection.</summary>
			/// <param name="index">The zero-based index of the element to get or set.</param>
			/// <returns>A <see cref="T:System.Windows.Forms.ListViewItem" /> that represents the item located at the specified index within the collection.</returns>
			/// <exception cref="T:System.NotSupportedException">This property cannot be set.</exception>
			// Token: 0x17001646 RID: 5702
			object IList.this[int index]
			{
				get
				{
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
					}
					return this[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			/// <summary>Gets an item with the specified key within the collection.</summary>
			/// <param name="key">The key of the item in the collection to retrieve.</param>
			/// <returns>A <see cref="T:System.Windows.Forms.ListViewItem" /> representing the item with the specified index within the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The owner <see cref="T:System.Windows.Forms.ListView" /> is in virtual mode.</exception>
			// Token: 0x17001647 RID: 5703
			public virtual ListViewItem this[string key]
			{
				get
				{
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
					}
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

			/// <summary>Gets an object that can be used to synchronize access to the collection of controls.</summary>
			/// <returns>The object used to synchronize the collection.</returns>
			// Token: 0x17001648 RID: 5704
			// (get) Token: 0x06005CE8 RID: 23784 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>Gets a value indicating whether access to the <see cref="T:System.Windows.Forms.ListView.CheckedListViewItemCollection" /> is synchronized (thread safe).</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x17001649 RID: 5705
			// (get) Token: 0x06005CE9 RID: 23785 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
			/// <returns>
			///     <see langword="true" /> in all cases.</returns>
			// Token: 0x1700164A RID: 5706
			// (get) Token: 0x06005CEA RID: 23786 RVA: 0x0000E214 File Offset: 0x0000C414
			bool IList.IsFixedSize
			{
				get
				{
					return true;
				}
			}

			/// <summary>Gets a value indicating whether the collection is read-only.</summary>
			/// <returns>
			///     <see langword="true" /> if the collection is read-only; otherwise, <see langword="false" />.</returns>
			// Token: 0x1700164B RID: 5707
			// (get) Token: 0x06005CEB RID: 23787 RVA: 0x0000E214 File Offset: 0x0000C414
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			/// <summary>Determines whether the specified item is located in the collection.</summary>
			/// <param name="item">A <see cref="T:System.Windows.Forms.ListViewItem" /> representing the item to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the specified item is located in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005CEC RID: 23788 RVA: 0x001826A6 File Offset: 0x001808A6
			public bool Contains(ListViewItem item)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
				}
				return item != null && item.ListView == this.owner && item.Checked;
			}

			/// <summary>Verifies whether the item is checked.</summary>
			/// <param name="item">A <see cref="T:System.Windows.Forms.ListViewItem" /> to locate in the <see cref="T:System.Windows.Forms.ListView.CheckedListViewItemCollection" />.</param>
			/// <returns>
			///     <see langword="true" /> if item is found in the <see cref="T:System.Windows.Forms.ListView.CheckedListViewItemCollection" />; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005CED RID: 23789 RVA: 0x001826E1 File Offset: 0x001808E1
			bool IList.Contains(object item)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
				}
				return item is ListViewItem && this.Contains((ListViewItem)item);
			}

			/// <summary>Determines if a column with the specified key is contained in the collection.</summary>
			/// <param name="key">The name of the item to search for.</param>
			/// <returns>
			///     <see langword="true" /> if an item with the specified key is contained in the collection; otherwise, <see langword="false." /></returns>
			/// <exception cref="T:System.InvalidOperationException">The owner <see cref="T:System.Windows.Forms.ListView" /> is in virtual mode.</exception>
			// Token: 0x06005CEE RID: 23790 RVA: 0x00182716 File Offset: 0x00180916
			public virtual bool ContainsKey(string key)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
				}
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			/// <summary>Returns the index within the collection of the specified item.</summary>
			/// <param name="item">A <see cref="T:System.Windows.Forms.ListViewItem" /> representing the item to locate in the collection. </param>
			/// <returns>The zero-based index of the item in the collection; otherwise, -1.</returns>
			// Token: 0x06005CEF RID: 23791 RVA: 0x00182744 File Offset: 0x00180944
			public int IndexOf(ListViewItem item)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
				}
				ListViewItem[] itemArray = this.ItemArray;
				for (int i = 0; i < itemArray.Length; i++)
				{
					if (itemArray[i] == item)
					{
						return i;
					}
				}
				return -1;
			}

			/// <summary>Determines the index for an item with the specified key.</summary>
			/// <param name="key">The name of the item to retrieve the index for.</param>
			/// <returns>The zero-based index for the <see cref="T:System.Windows.Forms.ListViewItem" /> with the specified name, if found; otherwise, -1.</returns>
			/// <exception cref="T:System.InvalidOperationException">The owner <see cref="T:System.Windows.Forms.ListView" /> is in virtual mode.</exception>
			// Token: 0x06005CF0 RID: 23792 RVA: 0x0018278C File Offset: 0x0018098C
			public virtual int IndexOfKey(string key)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
				}
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

			// Token: 0x06005CF1 RID: 23793 RVA: 0x00182826 File Offset: 0x00180A26
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			/// <summary>Returns the index within the collection of the specified item.</summary>
			/// <param name="item">A <see cref="T:System.Windows.Forms.ListViewItem" /> that represents the item to locate in the collection.</param>
			/// <returns>The zero-based index of the item if it is in the collection; otherwise, -1.</returns>
			// Token: 0x06005CF2 RID: 23794 RVA: 0x00182837 File Offset: 0x00180A37
			int IList.IndexOf(object item)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
				}
				if (item is ListViewItem)
				{
					return this.IndexOf((ListViewItem)item);
				}
				return -1;
			}

			/// <summary>Adds an item to the collection.</summary>
			/// <param name="value">The item to add to the collection.</param>
			/// <returns>The zero-based index where value is located in the collection.</returns>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005CF3 RID: 23795 RVA: 0x0000A2AB File Offset: 0x000084AB
			int IList.Add(object value)
			{
				throw new NotSupportedException();
			}

			/// <summary>Removes all items from the collection.</summary>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005CF4 RID: 23796 RVA: 0x0000A2AB File Offset: 0x000084AB
			void IList.Clear()
			{
				throw new NotSupportedException();
			}

			/// <summary>Inserts an item into the collection at a specified index.</summary>
			/// <param name="index">The index at which value should be inserted.</param>
			/// <param name="value">The object to be added to the <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005CF5 RID: 23797 RVA: 0x0000A2AB File Offset: 0x000084AB
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			/// <summary>Removes the first occurrence of an item from the collection.</summary>
			/// <param name="value">The object to be removed from the <see cref="T:System.Windows.Forms.ListView.CheckedListViewItemCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005CF6 RID: 23798 RVA: 0x0000A2AB File Offset: 0x000084AB
			void IList.Remove(object value)
			{
				throw new NotSupportedException();
			}

			/// <summary>Removes an item from the collection at the specified index.</summary>
			/// <param name="index">The zero-based index of the item to remove.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005CF7 RID: 23799 RVA: 0x0000A2AB File Offset: 0x000084AB
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			/// <summary>Copies the entire collection into an existing array at a specified location within the array.</summary>
			/// <param name="dest">An <see cref="T:System.Array" /> representing the array to copy the contents of the collection to. </param>
			/// <param name="index">The location within the destination array to copy the items from the collection to. </param>
			// Token: 0x06005CF8 RID: 23800 RVA: 0x0018286C File Offset: 0x00180A6C
			public void CopyTo(Array dest, int index)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
				}
				if (this.Count > 0)
				{
					Array.Copy(this.ItemArray, 0, dest, index, this.Count);
				}
			}

			/// <summary>Returns an enumerator that can be used to iterate through the checked item collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the checked item collection.</returns>
			// Token: 0x06005CF9 RID: 23801 RVA: 0x001828A8 File Offset: 0x00180AA8
			public IEnumerator GetEnumerator()
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
				}
				ListViewItem[] itemArray = this.ItemArray;
				if (itemArray != null)
				{
					return itemArray.GetEnumerator();
				}
				return new ListViewItem[0].GetEnumerator();
			}

			// Token: 0x04003A02 RID: 14850
			private ListView owner;

			// Token: 0x04003A03 RID: 14851
			private int lastAccessedIndex = -1;
		}

		/// <summary>Represents the collection that contains the indexes to the selected items in a <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
		// Token: 0x0200060D RID: 1549
		[ListBindable(false)]
		public class SelectedIndexCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" /> class.</summary>
			/// <param name="owner">A <see cref="T:System.Windows.Forms.ListView" /> control that owns the collection. </param>
			// Token: 0x06005CFA RID: 23802 RVA: 0x001828EE File Offset: 0x00180AEE
			public SelectedIndexCollection(ListView owner)
			{
				this.owner = owner;
			}

			/// <summary>Gets the number of items in the collection.</summary>
			/// <returns>The number of items in the collection.</returns>
			// Token: 0x1700164C RID: 5708
			// (get) Token: 0x06005CFB RID: 23803 RVA: 0x00182900 File Offset: 0x00180B00
			[Browsable(false)]
			public int Count
			{
				get
				{
					if (this.owner.IsHandleCreated)
					{
						return (int)((long)this.owner.SendMessage(4146, 0, 0));
					}
					if (this.owner.savedSelectedItems != null)
					{
						return this.owner.savedSelectedItems.Count;
					}
					return 0;
				}
			}

			// Token: 0x1700164D RID: 5709
			// (get) Token: 0x06005CFC RID: 23804 RVA: 0x00182954 File Offset: 0x00180B54
			private int[] IndicesArray
			{
				get
				{
					int count = this.Count;
					int[] array = new int[count];
					if (this.owner.IsHandleCreated)
					{
						int wparam = -1;
						for (int i = 0; i < count; i++)
						{
							int num = (int)((long)this.owner.SendMessage(4108, wparam, 2));
							if (num <= -1)
							{
								throw new InvalidOperationException(SR.GetString("SelectedNotEqualActual"));
							}
							array[i] = num;
							wparam = num;
						}
					}
					else
					{
						for (int j = 0; j < count; j++)
						{
							array[j] = this.owner.savedSelectedItems[j].Index;
						}
					}
					return array;
				}
			}

			/// <summary>Gets the index value at the specified index within the collection.</summary>
			/// <param name="index">The index of the item in the collection to retrieve. </param>
			/// <returns>The index value from the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> that is stored at the specified location.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than 0 or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListView.SelectedIndexCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" />. </exception>
			// Token: 0x1700164E RID: 5710
			public int this[int index]
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
					if (this.owner.IsHandleCreated)
					{
						int num = -1;
						for (int i = 0; i <= index; i++)
						{
							num = (int)((long)this.owner.SendMessage(4108, num, 2));
						}
						return num;
					}
					return this.owner.savedSelectedItems[index].Index;
				}
			}

			/// <summary>Gets or sets an object in the collection.</summary>
			/// <param name="index">The zero-based index of the element to get or set.</param>
			/// <returns>The index value from the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> that is stored at the specified location.</returns>
			// Token: 0x1700164F RID: 5711
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the collection of controls.</summary>
			/// <returns>The object used to synchronize the collection.</returns>
			// Token: 0x17001650 RID: 5712
			// (get) Token: 0x06005D00 RID: 23808 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x17001651 RID: 5713
			// (get) Token: 0x06005D01 RID: 23809 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" /> has a fixed size.</summary>
			/// <returns>
			///     <see langword="true" /> in all cases.</returns>
			// Token: 0x17001652 RID: 5714
			// (get) Token: 0x06005D02 RID: 23810 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
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
			// Token: 0x17001653 RID: 5715
			// (get) Token: 0x06005D03 RID: 23811 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			/// <summary>Determines whether the specified index is located in the collection.</summary>
			/// <param name="selectedIndex">The index to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the specified index from the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> for the <see cref="T:System.Windows.Forms.ListView" /> is an item in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005D04 RID: 23812 RVA: 0x00182A9A File Offset: 0x00180C9A
			public bool Contains(int selectedIndex)
			{
				return this.owner.Items[selectedIndex].Selected;
			}

			/// <summary>Determines whether the specified item is located in the collection.</summary>
			/// <param name="selectedIndex">The index to locate in the collection.</param>
			/// <returns>
			///     <see langword="true" /> if the specified index from the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> for the <see cref="T:System.Windows.Forms.ListView" /> is an item in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005D05 RID: 23813 RVA: 0x00182AB2 File Offset: 0x00180CB2
			bool IList.Contains(object selectedIndex)
			{
				return selectedIndex is int && this.Contains((int)selectedIndex);
			}

			/// <summary>Returns the index within the <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" /> of the specified index from the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> of the <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
			/// <param name="selectedIndex">The zero-based index from the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> to locate in the collection. </param>
			/// <returns>The zero-based index in the collection where the specified index of the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> is located within the <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" />, or -1 if the index is not located in the collection.</returns>
			// Token: 0x06005D06 RID: 23814 RVA: 0x00182ACC File Offset: 0x00180CCC
			public int IndexOf(int selectedIndex)
			{
				int[] indicesArray = this.IndicesArray;
				for (int i = 0; i < indicesArray.Length; i++)
				{
					if (indicesArray[i] == selectedIndex)
					{
						return i;
					}
				}
				return -1;
			}

			/// <summary>Returns the index in the <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" />. The <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" /> contains the indexes of selected items in the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> of the <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
			/// <param name="selectedIndex">The zero-based index from the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> to locate in the collection.</param>
			// Token: 0x06005D07 RID: 23815 RVA: 0x00182AF7 File Offset: 0x00180CF7
			int IList.IndexOf(object selectedIndex)
			{
				if (selectedIndex is int)
				{
					return this.IndexOf((int)selectedIndex);
				}
				return -1;
			}

			/// <summary>Adds an item to the collection.</summary>
			/// <param name="value">An object to be added to the <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" />.</param>
			/// <returns>The location of the added item.</returns>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005D08 RID: 23816 RVA: 0x00182B0F File Offset: 0x00180D0F
			int IList.Add(object value)
			{
				if (value is int)
				{
					return this.Add((int)value);
				}
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"value",
					value.ToString()
				}));
			}

			/// <summary>Removes all items from the collection.</summary>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005D09 RID: 23817 RVA: 0x00182B4C File Offset: 0x00180D4C
			void IList.Clear()
			{
				this.Clear();
			}

			/// <summary>Inserts an item into the collection at a specified index.</summary>
			/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted..</param>
			/// <param name="value">The object to be inserted into the <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005D0A RID: 23818 RVA: 0x0000A2AB File Offset: 0x000084AB
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			/// <summary>Removes the first occurrence of a specified item from the collection.</summary>
			/// <param name="value">The object to remove from the <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005D0B RID: 23819 RVA: 0x00182B54 File Offset: 0x00180D54
			void IList.Remove(object value)
			{
				if (value is int)
				{
					this.Remove((int)value);
					return;
				}
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"value",
					value.ToString()
				}));
			}

			/// <summary>Removes an item from the collection at a specified index.</summary>
			/// <param name="index">The zero-based index of the item to remove.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005D0C RID: 23820 RVA: 0x0000A2AB File Offset: 0x000084AB
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			/// <summary>Adds the item at the specified index in the <see cref="P:System.Windows.Forms.ListView.Items" /> array to the collection.</summary>
			/// <param name="itemIndex">The index of the item in the <see cref="P:System.Windows.Forms.ListView.Items" /> collection to be added to the <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" />. </param>
			/// <returns>The number of items in the <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" />.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The specified index is less than 0 or greater than or equal to the number of items in the owner <see cref="T:System.Windows.Forms.ListView" />.-or-The owner <see cref="T:System.Windows.Forms.ListView" /> is in virtual mode, and the specified index is less than 0 or greater than or equal to the value of <see cref="P:System.Windows.Forms.ListView.VirtualListSize" />.</exception>
			// Token: 0x06005D0D RID: 23821 RVA: 0x00182B94 File Offset: 0x00180D94
			public int Add(int itemIndex)
			{
				if (this.owner.VirtualMode)
				{
					if (itemIndex < 0 || itemIndex >= this.owner.VirtualListSize)
					{
						throw new ArgumentOutOfRangeException("itemIndex", SR.GetString("InvalidArgument", new object[]
						{
							"itemIndex",
							itemIndex.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.owner.IsHandleCreated)
					{
						this.owner.SetItemState(itemIndex, 2, 2);
						return this.Count;
					}
					return -1;
				}
				else
				{
					if (itemIndex < 0 || itemIndex >= this.owner.Items.Count)
					{
						throw new ArgumentOutOfRangeException("itemIndex", SR.GetString("InvalidArgument", new object[]
						{
							"itemIndex",
							itemIndex.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.owner.Items[itemIndex].Selected = true;
					return this.Count;
				}
			}

			/// <summary>Clears the items in the collection.</summary>
			// Token: 0x06005D0E RID: 23822 RVA: 0x00182C7C File Offset: 0x00180E7C
			public void Clear()
			{
				if (!this.owner.VirtualMode)
				{
					this.owner.savedSelectedItems = null;
				}
				if (this.owner.IsHandleCreated)
				{
					this.owner.SetItemState(-1, 0, 2);
				}
			}

			/// <summary>Copies the entire collection into an existing array at a specified location within the array.</summary>
			/// <param name="dest">An <see cref="T:System.Array" /> representing the array to copy the contents of the collection to. </param>
			/// <param name="index">The location within the destination array to copy the items from the collection to. </param>
			// Token: 0x06005D0F RID: 23823 RVA: 0x00182CB2 File Offset: 0x00180EB2
			public void CopyTo(Array dest, int index)
			{
				if (this.Count > 0)
				{
					Array.Copy(this.IndicesArray, 0, dest, index, this.Count);
				}
			}

			/// <summary>Returns an enumerator that can be used to iterate through the selected index collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the selected index collection.</returns>
			// Token: 0x06005D10 RID: 23824 RVA: 0x00182CD4 File Offset: 0x00180ED4
			public IEnumerator GetEnumerator()
			{
				int[] indicesArray = this.IndicesArray;
				if (indicesArray != null)
				{
					return indicesArray.GetEnumerator();
				}
				return new int[0].GetEnumerator();
			}

			/// <summary>Removes the item at the specified index in the <see cref="P:System.Windows.Forms.ListView.Items" /> collection from the <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" />.</summary>
			/// <param name="itemIndex">The index of the item in the <see cref="P:System.Windows.Forms.ListView.Items" /> collection to remove from the <see cref="T:System.Windows.Forms.ListView.SelectedIndexCollection" />.</param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The specified index is less than 0 or greater than or equal to the number of items in the owner <see cref="T:System.Windows.Forms.ListView" />.-or-The owner <see cref="T:System.Windows.Forms.ListView" /> is in virtual mode, and the specified index is less than 0 or greater than or equal to the value of <see cref="P:System.Windows.Forms.ListView.VirtualListSize" />.</exception>
			// Token: 0x06005D11 RID: 23825 RVA: 0x00182D00 File Offset: 0x00180F00
			public void Remove(int itemIndex)
			{
				if (this.owner.VirtualMode)
				{
					if (itemIndex < 0 || itemIndex >= this.owner.VirtualListSize)
					{
						throw new ArgumentOutOfRangeException("itemIndex", SR.GetString("InvalidArgument", new object[]
						{
							"itemIndex",
							itemIndex.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.owner.IsHandleCreated)
					{
						this.owner.SetItemState(itemIndex, 0, 2);
						return;
					}
				}
				else
				{
					if (itemIndex < 0 || itemIndex >= this.owner.Items.Count)
					{
						throw new ArgumentOutOfRangeException("itemIndex", SR.GetString("InvalidArgument", new object[]
						{
							"itemIndex",
							itemIndex.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.owner.Items[itemIndex].Selected = false;
				}
			}

			// Token: 0x04003A04 RID: 14852
			private ListView owner;
		}

		/// <summary>Represents the collection of selected items in a list view control.</summary>
		// Token: 0x0200060E RID: 1550
		[ListBindable(false)]
		public class SelectedListViewItemCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListView.SelectedListViewItemCollection" /> class.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.ListView" /> control that owns the collection. </param>
			// Token: 0x06005D12 RID: 23826 RVA: 0x00182DDA File Offset: 0x00180FDA
			public SelectedListViewItemCollection(ListView owner)
			{
				this.owner = owner;
			}

			// Token: 0x17001654 RID: 5716
			// (get) Token: 0x06005D13 RID: 23827 RVA: 0x00182DF0 File Offset: 0x00180FF0
			private ListViewItem[] SelectedItemArray
			{
				get
				{
					if (this.owner.IsHandleCreated)
					{
						int num = (int)((long)this.owner.SendMessage(4146, 0, 0));
						ListViewItem[] array = new ListViewItem[num];
						int wparam = -1;
						for (int i = 0; i < num; i++)
						{
							int num2 = (int)((long)this.owner.SendMessage(4108, wparam, 2));
							if (num2 <= -1)
							{
								throw new InvalidOperationException(SR.GetString("SelectedNotEqualActual"));
							}
							array[i] = this.owner.Items[num2];
							wparam = num2;
						}
						return array;
					}
					if (this.owner.savedSelectedItems != null)
					{
						ListViewItem[] array2 = new ListViewItem[this.owner.savedSelectedItems.Count];
						for (int j = 0; j < this.owner.savedSelectedItems.Count; j++)
						{
							array2[j] = this.owner.savedSelectedItems[j];
						}
						return array2;
					}
					return new ListViewItem[0];
				}
			}

			/// <summary>Gets the number of items in the collection.</summary>
			/// <returns>The number of items in the collection.</returns>
			// Token: 0x17001655 RID: 5717
			// (get) Token: 0x06005D14 RID: 23828 RVA: 0x00182EE8 File Offset: 0x001810E8
			[Browsable(false)]
			public int Count
			{
				get
				{
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
					}
					if (this.owner.IsHandleCreated)
					{
						return (int)((long)this.owner.SendMessage(4146, 0, 0));
					}
					if (this.owner.savedSelectedItems != null)
					{
						return this.owner.savedSelectedItems.Count;
					}
					return 0;
				}
			}

			/// <summary>Gets the item at the specified index within the collection.</summary>
			/// <param name="index">The index of the item in the collection to retrieve. </param>
			/// <returns>A <see cref="T:System.Windows.Forms.ListViewItem" /> representing the item located at the specified index within the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than zero or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListView.ListViewItemCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.SelectedListViewItemCollection" />. </exception>
			// Token: 0x17001656 RID: 5718
			public ListViewItem this[int index]
			{
				get
				{
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
					}
					if (index < 0 || index >= this.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.owner.IsHandleCreated)
					{
						int num = -1;
						for (int i = 0; i <= index; i++)
						{
							num = (int)((long)this.owner.SendMessage(4108, num, 2));
						}
						return this.owner.Items[num];
					}
					return this.owner.savedSelectedItems[index];
				}
			}

			/// <summary>Gets an item with the specified key from the collection.</summary>
			/// <param name="key">The name of the item to retrieve from the collection.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> with the specified key.</returns>
			// Token: 0x17001657 RID: 5719
			public virtual ListViewItem this[string key]
			{
				get
				{
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
					}
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

			/// <summary>Gets or sets an an object from the collection.</summary>
			/// <param name="index">The zero-based index of the element to get.</param>
			/// <returns>A <see cref="T:System.Windows.Forms.ListViewItem" /> that represents the item located at the specified index within the collection.</returns>
			// Token: 0x17001658 RID: 5720
			object IList.this[int index]
			{
				get
				{
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
					}
					return this[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
			/// <returns>
			///     <see langword="true" /> in all cases.</returns>
			// Token: 0x17001659 RID: 5721
			// (get) Token: 0x06005D19 RID: 23833 RVA: 0x0000E214 File Offset: 0x0000C414
			bool IList.IsFixedSize
			{
				get
				{
					return true;
				}
			}

			/// <summary>Gets a value indicating whether the collection is read-only.</summary>
			/// <returns>
			///     <see langword="true" /> if the collection is read-only; otherwise, <see langword="false" />.</returns>
			// Token: 0x1700165A RID: 5722
			// (get) Token: 0x06005D1A RID: 23834 RVA: 0x0000E214 File Offset: 0x0000C414
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the collection of controls.</summary>
			/// <returns>The object used to synchronize the collection.</returns>
			// Token: 0x1700165B RID: 5723
			// (get) Token: 0x06005D1B RID: 23835 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x1700165C RID: 5724
			// (get) Token: 0x06005D1C RID: 23836 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Adds an item to the collection.</summary>
			/// <param name="value">An object to be added to the <see cref="T:System.Windows.Forms.ListView.SelectedListViewItemCollection" />.</param>
			/// <returns>The location of the added item.</returns>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005D1D RID: 23837 RVA: 0x0000A2AB File Offset: 0x000084AB
			int IList.Add(object value)
			{
				throw new NotSupportedException();
			}

			/// <summary>Inserts an item into the collection at a specified index.</summary>
			/// <param name="index">The zero-based index of the item to be inserted.</param>
			/// <param name="value">An object to be added to the <see cref="T:System.Windows.Forms.ListView.SelectedListViewItemCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005D1E RID: 23838 RVA: 0x0000A2AB File Offset: 0x000084AB
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06005D1F RID: 23839 RVA: 0x0018308C File Offset: 0x0018128C
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			/// <summary>Removes the first occurrence of a specified item from the collection.</summary>
			/// <param name="value">The object to remove from the <see cref="T:System.Windows.Forms.ListView.SelectedListViewItemCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005D20 RID: 23840 RVA: 0x0000A2AB File Offset: 0x000084AB
			void IList.Remove(object value)
			{
				throw new NotSupportedException();
			}

			/// <summary>Removes an item from the collection at a specified index.</summary>
			/// <param name="index">The zero-based index of the item to remove.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005D21 RID: 23841 RVA: 0x0000A2AB File Offset: 0x000084AB
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			/// <summary>Removes all items from the collection.</summary>
			// Token: 0x06005D22 RID: 23842 RVA: 0x001830A0 File Offset: 0x001812A0
			public void Clear()
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				ListViewItem[] selectedItemArray = this.SelectedItemArray;
				for (int i = 0; i < selectedItemArray.Length; i++)
				{
					selectedItemArray[i].Selected = false;
				}
			}

			/// <summary>Determines whether an item with the specified key is contained in the collection.</summary>
			/// <param name="key">The name of the item to find in the collection.</param>
			/// <returns>
			///     <see langword="true" /> to indicate the specified item is contained in the collection; otherwise, <see langword="false" />. </returns>
			// Token: 0x06005D23 RID: 23843 RVA: 0x001830E8 File Offset: 0x001812E8
			public virtual bool ContainsKey(string key)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			/// <summary>Determines whether the specified item is located in the collection.</summary>
			/// <param name="item">A <see cref="T:System.Windows.Forms.ListViewItem" /> representing the item to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the specified item is located in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005D24 RID: 23844 RVA: 0x00183114 File Offset: 0x00181314
			public bool Contains(ListViewItem item)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				return this.IndexOf(item) != -1;
			}

			/// <summary>Determines whether the specified item is located in the collection.</summary>
			/// <param name="item">An object that represents the item to locate in the collection.</param>
			/// <returns>
			///     <see langword="true" /> if the specified item is located in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005D25 RID: 23845 RVA: 0x00183140 File Offset: 0x00181340
			bool IList.Contains(object item)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				return item is ListViewItem && this.Contains((ListViewItem)item);
			}

			/// <summary>Copies the entire collection into an existing array at a specified location within the array.</summary>
			/// <param name="dest">An <see cref="T:System.Array" /> representing the array to copy the contents of the collection to. </param>
			/// <param name="index">The location within the destination array to copy the items from the collection to. </param>
			// Token: 0x06005D26 RID: 23846 RVA: 0x00183175 File Offset: 0x00181375
			public void CopyTo(Array dest, int index)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				if (this.Count > 0)
				{
					Array.Copy(this.SelectedItemArray, 0, dest, index, this.Count);
				}
			}

			/// <summary>Returns an enumerator that can be used to iterate through the selected item collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the collection of selected items.</returns>
			// Token: 0x06005D27 RID: 23847 RVA: 0x001831B4 File Offset: 0x001813B4
			public IEnumerator GetEnumerator()
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				ListViewItem[] selectedItemArray = this.SelectedItemArray;
				if (selectedItemArray != null)
				{
					return selectedItemArray.GetEnumerator();
				}
				return new ListViewItem[0].GetEnumerator();
			}

			/// <summary>Returns the index within the collection of the specified item.</summary>
			/// <param name="item">A <see cref="T:System.Windows.Forms.ListViewItem" /> representing the item to locate in the collection. </param>
			/// <returns>The zero-based index of the item in the collection. If the item is not located in the collection, the return value is negative one (-1).</returns>
			// Token: 0x06005D28 RID: 23848 RVA: 0x001831FC File Offset: 0x001813FC
			public int IndexOf(ListViewItem item)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				ListViewItem[] selectedItemArray = this.SelectedItemArray;
				for (int i = 0; i < selectedItemArray.Length; i++)
				{
					if (selectedItemArray[i] == item)
					{
						return i;
					}
				}
				return -1;
			}

			/// <summary>Returns the index, within the collection, of the specified item.</summary>
			/// <param name="item">An object that represents the item to locate in the collection.</param>
			/// <returns>The zero-based index of the item if it is in the collection; otherwise, -1</returns>
			// Token: 0x06005D29 RID: 23849 RVA: 0x00183244 File Offset: 0x00181444
			int IList.IndexOf(object item)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				if (item is ListViewItem)
				{
					return this.IndexOf((ListViewItem)item);
				}
				return -1;
			}

			/// <summary>Returns the index of the first occurrence of the item with the specified key.</summary>
			/// <param name="key">The name of the item to find in the collection.</param>
			/// <returns>The zero-based index of the first item with the specified key.</returns>
			// Token: 0x06005D2A RID: 23850 RVA: 0x0018327C File Offset: 0x0018147C
			public virtual int IndexOfKey(string key)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
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

			// Token: 0x04003A05 RID: 14853
			private ListView owner;

			// Token: 0x04003A06 RID: 14854
			private int lastAccessedIndex = -1;
		}

		/// <summary>Represents the collection of column headers in a <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
		// Token: 0x0200060F RID: 1551
		[ListBindable(false)]
		public class ColumnHeaderCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" /> class.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.ListView" /> that owns this collection. </param>
			// Token: 0x06005D2B RID: 23851 RVA: 0x00183316 File Offset: 0x00181516
			public ColumnHeaderCollection(ListView owner)
			{
				this.owner = owner;
			}

			/// <summary>Gets the column header at the specified index within the collection.</summary>
			/// <param name="index">The index of the column header to retrieve from the collection.</param>
			/// <returns>A <see cref="T:System.Windows.Forms.ColumnHeader" /> representing the column header located at the specified index within the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than 0 or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListView.ColumnHeaderCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" />. </exception>
			// Token: 0x1700165D RID: 5725
			public virtual ColumnHeader this[int index]
			{
				get
				{
					if (this.owner.columnHeaders == null || index < 0 || index >= this.owner.columnHeaders.Length)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.owner.columnHeaders[index];
				}
			}

			/// <summary>Gets or sets the column header at the specified index within the collection.</summary>
			/// <param name="index">The zero-based index of the element to get or set.</param>
			/// <returns>A <see cref="T:System.Windows.Forms.ColumnHeader" /> that represents the column header located at the specified index within the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than 0 or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListView.ColumnHeaderCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" />.</exception>
			// Token: 0x1700165E RID: 5726
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			/// <summary>Gets the column header with the specified key from the collection.</summary>
			/// <param name="key">The name of the column header to retrieve from the collection.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ColumnHeader" /> with the specified key.</returns>
			// Token: 0x1700165F RID: 5727
			public virtual ColumnHeader this[string key]
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

			/// <summary>Gets the number of items in the collection.</summary>
			/// <returns>The number of items in the collection.</returns>
			// Token: 0x17001660 RID: 5728
			// (get) Token: 0x06005D30 RID: 23856 RVA: 0x001833D5 File Offset: 0x001815D5
			[Browsable(false)]
			public int Count
			{
				get
				{
					if (this.owner.columnHeaders != null)
					{
						return this.owner.columnHeaders.Length;
					}
					return 0;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the collection of controls.</summary>
			/// <returns>The object used to synchronize the collection.</returns>
			// Token: 0x17001661 RID: 5729
			// (get) Token: 0x06005D31 RID: 23857 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>Gets a value indicating whether access to the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" /> is synchronized (thread safe).</summary>
			/// <returns>
			///     <see langword="true" /> in all cases.</returns>
			// Token: 0x17001662 RID: 5730
			// (get) Token: 0x06005D32 RID: 23858 RVA: 0x0000E214 File Offset: 0x0000C414
			bool ICollection.IsSynchronized
			{
				get
				{
					return true;
				}
			}

			/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" /> has a fixed size.</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x17001663 RID: 5731
			// (get) Token: 0x06005D33 RID: 23859 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
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
			// Token: 0x17001664 RID: 5732
			// (get) Token: 0x06005D34 RID: 23860 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			/// <summary>Removes the column with the specified key from the collection.</summary>
			/// <param name="key">The name of the column to remove from the collection.</param>
			// Token: 0x06005D35 RID: 23861 RVA: 0x001833F4 File Offset: 0x001815F4
			public virtual void RemoveByKey(string key)
			{
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					this.RemoveAt(index);
				}
			}

			/// <summary>Determines the index for a column with the specified key.</summary>
			/// <param name="key">The name of the column to retrieve the index for.</param>
			/// <returns>The zero-based index for the first occurrence of the column with the specified name, if found; otherwise, -1.</returns>
			// Token: 0x06005D36 RID: 23862 RVA: 0x0018341C File Offset: 0x0018161C
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

			// Token: 0x06005D37 RID: 23863 RVA: 0x00183499 File Offset: 0x00181699
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			/// <summary>Adds a column header to the collection with specified text, width, and alignment settings.</summary>
			/// <param name="text">The text to display in the column header. </param>
			/// <param name="width">The initial width of the column header. </param>
			/// <param name="textAlign">One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values. </param>
			/// <returns>The <see cref="T:System.Windows.Forms.ColumnHeader" /> that was created and added to the collection.</returns>
			// Token: 0x06005D38 RID: 23864 RVA: 0x001834AC File Offset: 0x001816AC
			public virtual ColumnHeader Add(string text, int width, HorizontalAlignment textAlign)
			{
				ColumnHeader columnHeader = new ColumnHeader();
				columnHeader.Text = text;
				columnHeader.Width = width;
				columnHeader.TextAlign = textAlign;
				return this.owner.InsertColumn(this.Count, columnHeader);
			}

			/// <summary>Adds an existing <see cref="T:System.Windows.Forms.ColumnHeader" /> to the collection.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.ColumnHeader" /> to add to the collection. </param>
			/// <returns>The zero-based index into the collection where the item was added.</returns>
			// Token: 0x06005D39 RID: 23865 RVA: 0x001834E8 File Offset: 0x001816E8
			public virtual int Add(ColumnHeader value)
			{
				int count = this.Count;
				this.owner.InsertColumn(count, value);
				return count;
			}

			/// <summary>Creates and adds a column with the specified text to the collection.</summary>
			/// <param name="text">The text to display in the column header.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ColumnHeader" /> with the specified text that was added to the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" />. </returns>
			// Token: 0x06005D3A RID: 23866 RVA: 0x0018350C File Offset: 0x0018170C
			public virtual ColumnHeader Add(string text)
			{
				ColumnHeader columnHeader = new ColumnHeader();
				columnHeader.Text = text;
				return this.owner.InsertColumn(this.Count, columnHeader);
			}

			/// <summary>Creates and adds a column with the specified text and width to the collection.</summary>
			/// <param name="text">The text of the <see cref="T:System.Windows.Forms.ColumnHeader" /> to add to the collection.</param>
			/// <param name="width">The width of the <see cref="T:System.Windows.Forms.ColumnHeader" /> to add to the collection.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ColumnHeader" /> with the specified text and width that was added to the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" />.</returns>
			// Token: 0x06005D3B RID: 23867 RVA: 0x00183538 File Offset: 0x00181738
			public virtual ColumnHeader Add(string text, int width)
			{
				ColumnHeader columnHeader = new ColumnHeader();
				columnHeader.Text = text;
				columnHeader.Width = width;
				return this.owner.InsertColumn(this.Count, columnHeader);
			}

			/// <summary>Creates and adds a column with the specified text and key to the collection.</summary>
			/// <param name="key">The key of the <see cref="T:System.Windows.Forms.ColumnHeader" /> to add to the collection.</param>
			/// <param name="text">The text of the <see cref="T:System.Windows.Forms.ColumnHeader" /> to add to the collection.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ColumnHeader" /> with the specified key and text that was added to the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" />.</returns>
			// Token: 0x06005D3C RID: 23868 RVA: 0x0018356C File Offset: 0x0018176C
			public virtual ColumnHeader Add(string key, string text)
			{
				ColumnHeader columnHeader = new ColumnHeader();
				columnHeader.Name = key;
				columnHeader.Text = text;
				return this.owner.InsertColumn(this.Count, columnHeader);
			}

			/// <summary>Creates and adds a column with the specified text, key, and width to the collection.</summary>
			/// <param name="key">The key of the column header.</param>
			/// <param name="text">The text to display in the column header.</param>
			/// <param name="width">The initial width of the <see cref="T:System.Windows.Forms.ColumnHeader" />.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ColumnHeader" /> with the given text, key, and width that was added to the collection.</returns>
			// Token: 0x06005D3D RID: 23869 RVA: 0x001835A0 File Offset: 0x001817A0
			public virtual ColumnHeader Add(string key, string text, int width)
			{
				ColumnHeader columnHeader = new ColumnHeader();
				columnHeader.Name = key;
				columnHeader.Text = text;
				columnHeader.Width = width;
				return this.owner.InsertColumn(this.Count, columnHeader);
			}

			/// <summary>Creates and adds a column with the specified key, aligned text, width, and image key to the collection.</summary>
			/// <param name="key">The key of the column header.</param>
			/// <param name="text">The text to display in the column header.</param>
			/// <param name="width">The initial width of the column header.</param>
			/// <param name="textAlign">One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values.</param>
			/// <param name="imageKey">The key value of the image to display in the column header.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ColumnHeader" /> with the specified key, aligned text, width, and image key that has been added to the collection.</returns>
			// Token: 0x06005D3E RID: 23870 RVA: 0x001835DC File Offset: 0x001817DC
			public virtual ColumnHeader Add(string key, string text, int width, HorizontalAlignment textAlign, string imageKey)
			{
				ColumnHeader columnHeader = new ColumnHeader(imageKey);
				columnHeader.Name = key;
				columnHeader.Text = text;
				columnHeader.Width = width;
				columnHeader.TextAlign = textAlign;
				return this.owner.InsertColumn(this.Count, columnHeader);
			}

			/// <summary>Creates and adds a column with the specified key, aligned text, width, and image index to the collection.</summary>
			/// <param name="key">The key of the column header.</param>
			/// <param name="text">The text to display in the column header.</param>
			/// <param name="width">The initial width of the column header.</param>
			/// <param name="textAlign">One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values.</param>
			/// <param name="imageIndex">The index value of the image to display in the column. </param>
			/// <returns>The <see cref="T:System.Windows.Forms.ColumnHeader" /> with the specified key, aligned text, width, and image index that has been added to the collection.</returns>
			// Token: 0x06005D3F RID: 23871 RVA: 0x00183620 File Offset: 0x00181820
			public virtual ColumnHeader Add(string key, string text, int width, HorizontalAlignment textAlign, int imageIndex)
			{
				ColumnHeader columnHeader = new ColumnHeader(imageIndex);
				columnHeader.Name = key;
				columnHeader.Text = text;
				columnHeader.Width = width;
				columnHeader.TextAlign = textAlign;
				return this.owner.InsertColumn(this.Count, columnHeader);
			}

			/// <summary>Adds an array of column headers to the collection.</summary>
			/// <param name="values">An array of <see cref="T:System.Windows.Forms.ColumnHeader" /> objects to add to the collection. </param>
			// Token: 0x06005D40 RID: 23872 RVA: 0x00183664 File Offset: 0x00181864
			public virtual void AddRange(ColumnHeader[] values)
			{
				if (values == null)
				{
					throw new ArgumentNullException("values");
				}
				Hashtable hashtable = new Hashtable();
				int[] array = new int[values.Length];
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i].DisplayIndex == -1)
					{
						values[i].DisplayIndexInternal = i;
					}
					if (!hashtable.ContainsKey(values[i].DisplayIndex) && values[i].DisplayIndex >= 0 && values[i].DisplayIndex < values.Length)
					{
						hashtable.Add(values[i].DisplayIndex, i);
					}
					array[i] = values[i].DisplayIndex;
					this.Add(values[i]);
				}
				if (hashtable.Count == values.Length)
				{
					this.owner.SetDisplayIndices(array);
				}
			}

			/// <summary>Adds a <see cref="T:System.Windows.Forms.ColumnHeader" /> to the <see cref="T:System.Windows.Forms.ListView" />.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.ColumnHeader" /> to be added to the <see cref="T:System.Windows.Forms.ListView" />.</param>
			/// <returns>The zero-based index indicating the location of the object that was added to the collection</returns>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="value" /> is not a <see cref="T:System.Windows.Forms.ColumnHeader" />.</exception>
			// Token: 0x06005D41 RID: 23873 RVA: 0x00183722 File Offset: 0x00181922
			int IList.Add(object value)
			{
				if (value is ColumnHeader)
				{
					return this.Add((ColumnHeader)value);
				}
				throw new ArgumentException(SR.GetString("ColumnHeaderCollectionInvalidArgument"));
			}

			/// <summary>Removes all column headers from the collection.</summary>
			// Token: 0x06005D42 RID: 23874 RVA: 0x00183748 File Offset: 0x00181948
			public virtual void Clear()
			{
				if (this.owner.columnHeaders != null)
				{
					if (this.owner.View == View.Tile)
					{
						for (int i = this.owner.columnHeaders.Length - 1; i >= 0; i--)
						{
							int width = this.owner.columnHeaders[i].Width;
							this.owner.columnHeaders[i].OwnerListview = null;
						}
						this.owner.columnHeaders = null;
						if (this.owner.IsHandleCreated)
						{
							this.owner.RecreateHandleInternal();
							return;
						}
					}
					else
					{
						for (int j = this.owner.columnHeaders.Length - 1; j >= 0; j--)
						{
							int width2 = this.owner.columnHeaders[j].Width;
							if (this.owner.IsHandleCreated)
							{
								this.owner.SendMessage(4124, j, 0);
							}
							this.owner.columnHeaders[j].OwnerListview = null;
						}
						this.owner.columnHeaders = null;
					}
				}
			}

			/// <summary>Determines whether the specified column header is located in the collection.</summary>
			/// <param name="value">A <see cref="T:System.Windows.Forms.ColumnHeader" /> representing the column header to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the column header is contained in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005D43 RID: 23875 RVA: 0x00183844 File Offset: 0x00181A44
			public bool Contains(ColumnHeader value)
			{
				return this.IndexOf(value) != -1;
			}

			/// <summary>Determines whether the specified column header is located in the collection.</summary>
			/// <param name="value">An object that represents the column header to locate in the collection.</param>
			/// <returns>
			///     <see langword="true" /> if the object is a column header that is contained in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005D44 RID: 23876 RVA: 0x00183853 File Offset: 0x00181A53
			bool IList.Contains(object value)
			{
				return value is ColumnHeader && this.Contains((ColumnHeader)value);
			}

			/// <summary>Determines if a column with the specified key is contained in the collection.</summary>
			/// <param name="key">The name of the column to search for.</param>
			/// <returns>
			///     <see langword="true" /> if a column with the specified name is contained in the collection; otherwise, <see langword="false" />. </returns>
			// Token: 0x06005D45 RID: 23877 RVA: 0x0018386B File Offset: 0x00181A6B
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			/// <summary>Copies the <see cref="T:System.Windows.Forms.ColumnHeader" /> objects in the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" /> to an array, starting at a particular array index.</summary>
			/// <param name="dest">The one-dimensional array that is the destination of the elements copied from the collection. The array must have zero-based indexing. </param>
			/// <param name="index">The zero-based index in the array at which copying begins.</param>
			// Token: 0x06005D46 RID: 23878 RVA: 0x0018387A File Offset: 0x00181A7A
			void ICollection.CopyTo(Array dest, int index)
			{
				if (this.Count > 0)
				{
					Array.Copy(this.owner.columnHeaders, 0, dest, index, this.Count);
				}
			}

			/// <summary>Returns the index, within the collection, of the specified column header.</summary>
			/// <param name="value">A <see cref="T:System.Windows.Forms.ColumnHeader" /> representing the column header to locate in the collection. </param>
			/// <returns>The zero-based index of the column header's location in the collection. If the column header is not located in the collection, the return value is -1.</returns>
			// Token: 0x06005D47 RID: 23879 RVA: 0x001838A0 File Offset: 0x00181AA0
			public int IndexOf(ColumnHeader value)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this[i] == value)
					{
						return i;
					}
				}
				return -1;
			}

			/// <summary>Returns the index, within the collection, of the specified column header.</summary>
			/// <param name="value">An object that represents the column header to locate in the collection.</param>
			// Token: 0x06005D48 RID: 23880 RVA: 0x001838CB File Offset: 0x00181ACB
			int IList.IndexOf(object value)
			{
				if (value is ColumnHeader)
				{
					return this.IndexOf((ColumnHeader)value);
				}
				return -1;
			}

			/// <summary>Inserts an existing column header into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the column header is inserted. </param>
			/// <param name="value">The <see cref="T:System.Windows.Forms.ColumnHeader" /> to insert into the collection. </param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than 0 or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListView.ColumnHeaderCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" />. </exception>
			// Token: 0x06005D49 RID: 23881 RVA: 0x001838E4 File Offset: 0x00181AE4
			public void Insert(int index, ColumnHeader value)
			{
				if (index < 0 || index > this.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.owner.InsertColumn(index, value);
			}

			/// <summary>Inserts an existing column header into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the column header is inserted.</param>
			/// <param name="value">The <see cref="T:System.Windows.Forms.ColumnHeader" /> to insert into the collection.</param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than 0 or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListView.ColumnHeaderCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" />.</exception>
			// Token: 0x06005D4A RID: 23882 RVA: 0x0018393E File Offset: 0x00181B3E
			void IList.Insert(int index, object value)
			{
				if (value is ColumnHeader)
				{
					this.Insert(index, (ColumnHeader)value);
				}
			}

			/// <summary>Creates a new column header and inserts it into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the column header is inserted. </param>
			/// <param name="text">The text to display in the column header. </param>
			/// <param name="width">The initial width of the column header. Set to -1 to autosize the column header to the size of the largest subitem text in the column or -2 to autosize the column header to the size of the text of the column header. </param>
			/// <param name="textAlign">One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values. </param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than 0 or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListView.ColumnHeaderCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" />. </exception>
			// Token: 0x06005D4B RID: 23883 RVA: 0x00183958 File Offset: 0x00181B58
			public void Insert(int index, string text, int width, HorizontalAlignment textAlign)
			{
				this.Insert(index, new ColumnHeader
				{
					Text = text,
					Width = width,
					TextAlign = textAlign
				});
			}

			/// <summary>Creates a new column header with the specified text, and inserts the header into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the column header is inserted.</param>
			/// <param name="text">The text to display in the column header. </param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than 0 or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListView.ColumnHeaderCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" />. </exception>
			// Token: 0x06005D4C RID: 23884 RVA: 0x0018398C File Offset: 0x00181B8C
			public void Insert(int index, string text)
			{
				this.Insert(index, new ColumnHeader
				{
					Text = text
				});
			}

			/// <summary>Creates a new column header with the specified text and initial width, and inserts the header into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the column header is inserted.</param>
			/// <param name="text">The text to display in the column header. </param>
			/// <param name="width">The initial width, in pixels, of the column header.</param>
			// Token: 0x06005D4D RID: 23885 RVA: 0x001839B0 File Offset: 0x00181BB0
			public void Insert(int index, string text, int width)
			{
				this.Insert(index, new ColumnHeader
				{
					Text = text,
					Width = width
				});
			}

			/// <summary>Creates a new column header with the specified text and key, and inserts the header into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the column header is inserted.</param>
			/// <param name="key">The name of the column header. </param>
			/// <param name="text">The text to display in the column header. </param>
			// Token: 0x06005D4E RID: 23886 RVA: 0x001839DC File Offset: 0x00181BDC
			public void Insert(int index, string key, string text)
			{
				this.Insert(index, new ColumnHeader
				{
					Name = key,
					Text = text
				});
			}

			/// <summary>Creates a new column header with the specified text, key, and width, and inserts the header into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the column header is inserted.</param>
			/// <param name="key">The name of the column header. </param>
			/// <param name="text">The text to display in the column header. </param>
			/// <param name="width">The initial width, in pixels, of the column header.</param>
			// Token: 0x06005D4F RID: 23887 RVA: 0x00183A08 File Offset: 0x00181C08
			public void Insert(int index, string key, string text, int width)
			{
				this.Insert(index, new ColumnHeader
				{
					Name = key,
					Text = text,
					Width = width
				});
			}

			/// <summary>Creates a new column header with the specified aligned text, key, width, and image key, and inserts the header into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the column header is inserted.</param>
			/// <param name="key">The name of the column header. </param>
			/// <param name="text">The text to display in the column header. </param>
			/// <param name="width">The initial width, in pixels, of the column header.</param>
			/// <param name="textAlign">One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values.</param>
			/// <param name="imageKey">The key of the image to display in the column header.</param>
			// Token: 0x06005D50 RID: 23888 RVA: 0x00183A3C File Offset: 0x00181C3C
			public void Insert(int index, string key, string text, int width, HorizontalAlignment textAlign, string imageKey)
			{
				this.Insert(index, new ColumnHeader(imageKey)
				{
					Name = key,
					Text = text,
					Width = width,
					TextAlign = textAlign
				});
			}

			/// <summary>Creates a new column header with the specified aligned text, key, width, and image index, and inserts the header into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the column header is inserted.</param>
			/// <param name="key">The name of the column header. </param>
			/// <param name="text">The text to display in the column header. </param>
			/// <param name="width">The initial width, in pixels, of the column header.</param>
			/// <param name="textAlign">One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values.</param>
			/// <param name="imageIndex">The index of the image to display in the column header.</param>
			// Token: 0x06005D51 RID: 23889 RVA: 0x00183A78 File Offset: 0x00181C78
			public void Insert(int index, string key, string text, int width, HorizontalAlignment textAlign, int imageIndex)
			{
				this.Insert(index, new ColumnHeader(imageIndex)
				{
					Name = key,
					Text = text,
					Width = width,
					TextAlign = textAlign
				});
			}

			/// <summary>Removes the column header at the specified index within the collection.</summary>
			/// <param name="index">The zero-based index of the column header to remove. </param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than 0 or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListView.ColumnHeaderCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" />. </exception>
			// Token: 0x06005D52 RID: 23890 RVA: 0x00183AB4 File Offset: 0x00181CB4
			public virtual void RemoveAt(int index)
			{
				if (index < 0 || index >= this.owner.columnHeaders.Length)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				int width = this.owner.columnHeaders[index].Width;
				if (this.owner.IsHandleCreated && this.owner.View != View.Tile && (int)((long)this.owner.SendMessage(4124, index, 0)) == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				int[] array = new int[this.Count - 1];
				ColumnHeader columnHeader = this[index];
				for (int i = 0; i < this.Count; i++)
				{
					ColumnHeader columnHeader2 = this[i];
					if (i != index)
					{
						if (columnHeader2.DisplayIndex >= columnHeader.DisplayIndex)
						{
							ColumnHeader columnHeader3 = columnHeader2;
							int displayIndexInternal = columnHeader3.DisplayIndexInternal;
							columnHeader3.DisplayIndexInternal = displayIndexInternal - 1;
						}
						array[(i > index) ? (i - 1) : i] = columnHeader2.DisplayIndexInternal;
					}
				}
				columnHeader.DisplayIndexInternal = -1;
				this.owner.columnHeaders[index].OwnerListview = null;
				int num = this.owner.columnHeaders.Length;
				if (num == 1)
				{
					this.owner.columnHeaders = null;
				}
				else
				{
					ColumnHeader[] array2 = new ColumnHeader[--num];
					if (index > 0)
					{
						Array.Copy(this.owner.columnHeaders, 0, array2, 0, index);
					}
					if (index < num)
					{
						Array.Copy(this.owner.columnHeaders, index + 1, array2, index, num - index);
					}
					this.owner.columnHeaders = array2;
				}
				if (this.owner.IsHandleCreated && this.owner.View == View.Tile)
				{
					this.owner.RecreateHandleInternal();
				}
				this.owner.SetDisplayIndices(array);
			}

			/// <summary>Removes the specified column header from the collection.</summary>
			/// <param name="column">A <see cref="T:System.Windows.Forms.ColumnHeader" /> representing the column header to remove from the collection. </param>
			// Token: 0x06005D53 RID: 23891 RVA: 0x00183CB0 File Offset: 0x00181EB0
			public virtual void Remove(ColumnHeader column)
			{
				int num = this.IndexOf(column);
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			/// <summary>Removes the specified column header from the collection.</summary>
			/// <param name="value">A <see cref="T:System.Windows.Forms.ColumnHeader" /> that represents the column header to remove from the collection.</param>
			// Token: 0x06005D54 RID: 23892 RVA: 0x00183CD0 File Offset: 0x00181ED0
			void IList.Remove(object value)
			{
				if (value is ColumnHeader)
				{
					this.Remove((ColumnHeader)value);
				}
			}

			/// <summary>Returns an enumerator to use to iterate through the column header collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the column header collection.</returns>
			// Token: 0x06005D55 RID: 23893 RVA: 0x00183CE6 File Offset: 0x00181EE6
			public IEnumerator GetEnumerator()
			{
				if (this.owner.columnHeaders != null)
				{
					return this.owner.columnHeaders.GetEnumerator();
				}
				return new ColumnHeader[0].GetEnumerator();
			}

			// Token: 0x04003A07 RID: 14855
			private ListView owner;

			// Token: 0x04003A08 RID: 14856
			private int lastAccessedIndex = -1;
		}

		/// <summary>Represents the collection of items in a <see cref="T:System.Windows.Forms.ListView" /> control or assigned to a <see cref="T:System.Windows.Forms.ListViewGroup" />. </summary>
		// Token: 0x02000610 RID: 1552
		[ListBindable(false)]
		public class ListViewItemCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> class. </summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.ListView" /> that owns the collection. </param>
			// Token: 0x06005D56 RID: 23894 RVA: 0x00183D11 File Offset: 0x00181F11
			public ListViewItemCollection(ListView owner)
			{
				this.innerList = new ListView.ListViewNativeItemCollection(owner);
			}

			// Token: 0x06005D57 RID: 23895 RVA: 0x00183D2C File Offset: 0x00181F2C
			internal ListViewItemCollection(ListView.ListViewItemCollection.IInnerList innerList)
			{
				this.innerList = innerList;
			}

			// Token: 0x17001665 RID: 5733
			// (get) Token: 0x06005D58 RID: 23896 RVA: 0x00183D42 File Offset: 0x00181F42
			private ListView.ListViewItemCollection.IInnerList InnerList
			{
				get
				{
					return this.innerList;
				}
			}

			/// <summary>Gets the number of items in the collection.</summary>
			/// <returns>The number of items in the collection.</returns>
			// Token: 0x17001666 RID: 5734
			// (get) Token: 0x06005D59 RID: 23897 RVA: 0x00183D4A File Offset: 0x00181F4A
			[Browsable(false)]
			public int Count
			{
				get
				{
					return this.InnerList.Count;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the collection of controls.</summary>
			/// <returns>The object used to synchronize the collection.</returns>
			// Token: 0x17001667 RID: 5735
			// (get) Token: 0x06005D5A RID: 23898 RVA: 0x000069BD File Offset: 0x00004BBD
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
			// Token: 0x17001668 RID: 5736
			// (get) Token: 0x06005D5B RID: 23899 RVA: 0x0000E214 File Offset: 0x0000C414
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
			// Token: 0x17001669 RID: 5737
			// (get) Token: 0x06005D5C RID: 23900 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
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
			// Token: 0x1700166A RID: 5738
			// (get) Token: 0x06005D5D RID: 23901 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets or sets the item at the specified index within the collection.</summary>
			/// <param name="index">The index of the item in the collection to get or set. </param>
			/// <returns>A <see cref="T:System.Windows.Forms.ListViewItem" /> representing the item located at the specified index within the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than 0 or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListView.ListViewItemCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" />. </exception>
			// Token: 0x1700166B RID: 5739
			public virtual ListViewItem this[int index]
			{
				get
				{
					if (index < 0 || index >= this.InnerList.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.InnerList[index];
				}
				set
				{
					if (index < 0 || index >= this.InnerList.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.InnerList[index] = value;
				}
			}

			/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ListViewItem" /> at the specified index within the collection.</summary>
			/// <param name="index">The zero-based index of the element to get.</param>
			/// <returns>A <see cref="T:System.Windows.Forms.ListViewItem" /> that represents the item located at the specified index within the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The index parameter is less than 0 or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListView.ListViewItemCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" />.</exception>
			// Token: 0x1700166C RID: 5740
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (value is ListViewItem)
					{
						this[index] = (ListViewItem)value;
						return;
					}
					if (value != null)
					{
						this[index] = new ListViewItem(value.ToString(), -1);
					}
				}
			}

			/// <summary>Retrieves the item with the specified key.</summary>
			/// <param name="key">The name of the item to retrieve.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> whose <see cref="P:System.Windows.Forms.ListViewItem.Name" /> property matches the specified key.</returns>
			// Token: 0x1700166D RID: 5741
			public virtual ListViewItem this[string key]
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

			/// <summary>Creates an item with the specified text and adds it to the collection.</summary>
			/// <param name="text">The text to display for the item. </param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> that was added to the collection.</returns>
			// Token: 0x06005D63 RID: 23907 RVA: 0x00183E81 File Offset: 0x00182081
			public virtual ListViewItem Add(string text)
			{
				return this.Add(text, -1);
			}

			/// <summary>Adds an existing object to the collection.</summary>
			/// <param name="item">The object to add to the collection.</param>
			/// <returns>The zero-based index indicating the location of the object if it was added to the collection; otherwise, -1.</returns>
			// Token: 0x06005D64 RID: 23908 RVA: 0x00183E8B File Offset: 0x0018208B
			int IList.Add(object item)
			{
				if (item is ListViewItem)
				{
					return this.IndexOf(this.Add((ListViewItem)item));
				}
				if (item != null)
				{
					return this.IndexOf(this.Add(item.ToString()));
				}
				return -1;
			}

			/// <summary>Creates an item with the specified text and image and adds it to the collection.</summary>
			/// <param name="text">The text of the item. </param>
			/// <param name="imageIndex">The index of the image to display for the item. </param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> that was added to the collection.</returns>
			// Token: 0x06005D65 RID: 23909 RVA: 0x00183EC0 File Offset: 0x001820C0
			public virtual ListViewItem Add(string text, int imageIndex)
			{
				ListViewItem listViewItem = new ListViewItem(text, imageIndex);
				this.Add(listViewItem);
				return listViewItem;
			}

			/// <summary>Adds an existing <see cref="T:System.Windows.Forms.ListViewItem" /> to the collection.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.ListViewItem" /> to add to the collection. </param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> that was added to the collection.</returns>
			// Token: 0x06005D66 RID: 23910 RVA: 0x00183EDE File Offset: 0x001820DE
			public virtual ListViewItem Add(ListViewItem value)
			{
				this.InnerList.Add(value);
				return value;
			}

			/// <summary>Creates an item with the specified text and image and adds it to the collection.</summary>
			/// <param name="text">The text of the item.</param>
			/// <param name="imageKey">The key of the image to display for the item.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> added to the collection.</returns>
			// Token: 0x06005D67 RID: 23911 RVA: 0x00183EF0 File Offset: 0x001820F0
			public virtual ListViewItem Add(string text, string imageKey)
			{
				ListViewItem listViewItem = new ListViewItem(text, imageKey);
				this.Add(listViewItem);
				return listViewItem;
			}

			/// <summary>Creates an item with the specified key, text, and image, and adds it to the collection.</summary>
			/// <param name="key">The name of the item.</param>
			/// <param name="text">The text of the item.</param>
			/// <param name="imageKey">The key of the image to display for the item.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> added to the collection.</returns>
			// Token: 0x06005D68 RID: 23912 RVA: 0x00183F10 File Offset: 0x00182110
			public virtual ListViewItem Add(string key, string text, string imageKey)
			{
				ListViewItem listViewItem = new ListViewItem(text, imageKey);
				listViewItem.Name = key;
				this.Add(listViewItem);
				return listViewItem;
			}

			/// <summary>Creates an item with the specified key, text, and image and adds an item to the collection.</summary>
			/// <param name="key">The name of the item.</param>
			/// <param name="text">The text of the item.</param>
			/// <param name="imageIndex">The index of the image to display for the item.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> added to the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The containing <see cref="T:System.Windows.Forms.ListView" /> is in virtual mode.</exception>
			// Token: 0x06005D69 RID: 23913 RVA: 0x00183F38 File Offset: 0x00182138
			public virtual ListViewItem Add(string key, string text, int imageIndex)
			{
				ListViewItem listViewItem = new ListViewItem(text, imageIndex);
				listViewItem.Name = key;
				this.Add(listViewItem);
				return listViewItem;
			}

			/// <summary>Adds an array of <see cref="T:System.Windows.Forms.ListViewItem" /> objects to the collection.</summary>
			/// <param name="items">An array of <see cref="T:System.Windows.Forms.ListViewItem" /> objects to add to the collection. </param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="items" /> is <see langword="null" />.</exception>
			// Token: 0x06005D6A RID: 23914 RVA: 0x00183F5D File Offset: 0x0018215D
			public void AddRange(ListViewItem[] items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				this.InnerList.AddRange(items);
			}

			/// <summary>Adds a collection of items to the collection.</summary>
			/// <param name="items">The <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" /> to add to the collection.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="items" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.InvalidOperationException">The containing <see cref="T:System.Windows.Forms.ListView" /> is in virtual mode.</exception>
			// Token: 0x06005D6B RID: 23915 RVA: 0x00183F7C File Offset: 0x0018217C
			public void AddRange(ListView.ListViewItemCollection items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				ListViewItem[] array = new ListViewItem[items.Count];
				items.CopyTo(array, 0);
				this.InnerList.AddRange(array);
			}

			/// <summary>Removes all items from the collection.</summary>
			// Token: 0x06005D6C RID: 23916 RVA: 0x00183FB7 File Offset: 0x001821B7
			public virtual void Clear()
			{
				this.InnerList.Clear();
			}

			/// <summary>Determines whether the specified item is located in the collection.</summary>
			/// <param name="item">A <see cref="T:System.Windows.Forms.ListViewItem" /> representing the item to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the item is contained in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005D6D RID: 23917 RVA: 0x00183FC4 File Offset: 0x001821C4
			public bool Contains(ListViewItem item)
			{
				return this.InnerList.Contains(item);
			}

			/// <summary>Determines whether the specified item is in the collection.</summary>
			/// <param name="item">An object that represents the item to locate in the collection.</param>
			/// <returns>
			///     <see langword="true" /> if the specified item is located in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005D6E RID: 23918 RVA: 0x00183FD2 File Offset: 0x001821D2
			bool IList.Contains(object item)
			{
				return item is ListViewItem && this.Contains((ListViewItem)item);
			}

			/// <summary>Determines whether the collection contains an item with the specified key.</summary>
			/// <param name="key">The name of the item to search for.</param>
			/// <returns>
			///     <see langword="true" /> to indicate the collection contains an item with the specified key; otherwise, <see langword="false" />. </returns>
			// Token: 0x06005D6F RID: 23919 RVA: 0x00183FEA File Offset: 0x001821EA
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			/// <summary>Copies the entire collection into an existing array at a specified location within the array.</summary>
			/// <param name="dest">An <see cref="T:System.Array" /> representing the array to copy the contents of the collection to. </param>
			/// <param name="index">The location within the destination array to copy the items from the collection to. </param>
			// Token: 0x06005D70 RID: 23920 RVA: 0x00183FF9 File Offset: 0x001821F9
			public void CopyTo(Array dest, int index)
			{
				this.InnerList.CopyTo(dest, index);
			}

			/// <summary>Searches for items whose name matches the specified key, optionally searching subitems.</summary>
			/// <param name="key">The item name to search for.</param>
			/// <param name="searchAllSubItems">
			///       <see langword="true" /> to search subitems; otherwise, <see langword="false" />. </param>
			/// <returns>An array of  <see cref="T:System.Windows.Forms.ListViewItem" /> objects containing the matching items, or an empty array if no items matched.</returns>
			// Token: 0x06005D71 RID: 23921 RVA: 0x00184008 File Offset: 0x00182208
			public ListViewItem[] Find(string key, bool searchAllSubItems)
			{
				ArrayList arrayList = this.FindInternal(key, searchAllSubItems, this, new ArrayList());
				ListViewItem[] array = new ListViewItem[arrayList.Count];
				arrayList.CopyTo(array, 0);
				return array;
			}

			// Token: 0x06005D72 RID: 23922 RVA: 0x0018403C File Offset: 0x0018223C
			private ArrayList FindInternal(string key, bool searchAllSubItems, ListView.ListViewItemCollection listViewItems, ArrayList foundItems)
			{
				if (listViewItems == null || foundItems == null)
				{
					return null;
				}
				for (int i = 0; i < listViewItems.Count; i++)
				{
					if (WindowsFormsUtils.SafeCompareStrings(listViewItems[i].Name, key, true))
					{
						foundItems.Add(listViewItems[i]);
					}
					else if (searchAllSubItems)
					{
						for (int j = 1; j < listViewItems[i].SubItems.Count; j++)
						{
							if (WindowsFormsUtils.SafeCompareStrings(listViewItems[i].SubItems[j].Name, key, true))
							{
								foundItems.Add(listViewItems[i]);
								break;
							}
						}
					}
				}
				return foundItems;
			}

			/// <summary>Returns an enumerator to use to iterate through the item collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the item collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The owner <see cref="T:System.Windows.Forms.ListView" /> is in virtual mode.</exception>
			// Token: 0x06005D73 RID: 23923 RVA: 0x001840DE File Offset: 0x001822DE
			public IEnumerator GetEnumerator()
			{
				if (this.InnerList.OwnerIsVirtualListView && !this.InnerList.OwnerIsDesignMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantGetEnumeratorInVirtualMode"));
				}
				return this.InnerList.GetEnumerator();
			}

			/// <summary>Returns the index within the collection of the specified item.</summary>
			/// <param name="item">A <see cref="T:System.Windows.Forms.ListViewItem" /> representing the item to locate in the collection. </param>
			/// <returns>The zero-based index of the item's location in the collection; otherwise, -1 if the item is not located in the collection.</returns>
			// Token: 0x06005D74 RID: 23924 RVA: 0x00184118 File Offset: 0x00182318
			public int IndexOf(ListViewItem item)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this[i] == item)
					{
						return i;
					}
				}
				return -1;
			}

			/// <summary>Returns the index within the collection of the specified item.</summary>
			/// <param name="item">A <see cref="T:System.Windows.Forms.ListViewItem" /> that represents the item to locate in the collection.</param>
			/// <returns>The zero-based index of the item if it is in the collection; otherwise, -1.</returns>
			// Token: 0x06005D75 RID: 23925 RVA: 0x00184143 File Offset: 0x00182343
			int IList.IndexOf(object item)
			{
				if (item is ListViewItem)
				{
					return this.IndexOf((ListViewItem)item);
				}
				return -1;
			}

			/// <summary>Retrieves the index of the item with the specified key.</summary>
			/// <param name="key">The name of the item to find in the collection.</param>
			/// <returns>The zero-based index of the first occurrence of the item with the specified key, if found; otherwise, -1.</returns>
			// Token: 0x06005D76 RID: 23926 RVA: 0x0018415C File Offset: 0x0018235C
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

			// Token: 0x06005D77 RID: 23927 RVA: 0x001841D9 File Offset: 0x001823D9
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			/// <summary>Inserts an existing <see cref="T:System.Windows.Forms.ListViewItem" /> into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the item is inserted. </param>
			/// <param name="item">The <see cref="T:System.Windows.Forms.ListViewItem" /> that represents the item to insert. </param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> that was inserted into the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than 0 or greater than the value of the <see cref="P:System.Windows.Forms.ListView.ListViewItemCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" />. </exception>
			// Token: 0x06005D78 RID: 23928 RVA: 0x001841EC File Offset: 0x001823EC
			public ListViewItem Insert(int index, ListViewItem item)
			{
				if (index < 0 || index > this.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.InnerList.Insert(index, item);
				return item;
			}

			/// <summary>Creates a new item and inserts it into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the item is inserted. </param>
			/// <param name="text">The text to display for the item. </param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> that was inserted into the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than 0 or greater than the value of the <see cref="P:System.Windows.Forms.ListView.ListViewItemCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" />. </exception>
			// Token: 0x06005D79 RID: 23929 RVA: 0x00184247 File Offset: 0x00182447
			public ListViewItem Insert(int index, string text)
			{
				return this.Insert(index, new ListViewItem(text));
			}

			/// <summary>Creates a new item with the specified image index and inserts it into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the item is inserted. </param>
			/// <param name="text">The text to display for the item. </param>
			/// <param name="imageIndex">The index of the image to display for the item. </param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> that was inserted into the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than 0 or greater than the value of the <see cref="P:System.Windows.Forms.ListView.ListViewItemCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" />. </exception>
			// Token: 0x06005D7A RID: 23930 RVA: 0x00184256 File Offset: 0x00182456
			public ListViewItem Insert(int index, string text, int imageIndex)
			{
				return this.Insert(index, new ListViewItem(text, imageIndex));
			}

			/// <summary>Inserts an object into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the item is inserted.</param>
			/// <param name="item">The object that represents the item to insert.</param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The index parameter is less than 0 or greater than the value of the <see cref="P:System.Windows.Forms.ListView.ListViewItemCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" />.</exception>
			// Token: 0x06005D7B RID: 23931 RVA: 0x00184266 File Offset: 0x00182466
			void IList.Insert(int index, object item)
			{
				if (item is ListViewItem)
				{
					this.Insert(index, (ListViewItem)item);
					return;
				}
				if (item != null)
				{
					this.Insert(index, item.ToString());
				}
			}

			/// <summary>Creates a new item with the specified text and image and inserts it in the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the item is inserted. </param>
			/// <param name="text">The text of the <see cref="T:System.Windows.Forms.ListViewItem" />.</param>
			/// <param name="imageKey">The key of the image to display for the item.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> added to the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than 0 or greater than the value of the <see cref="P:System.Windows.Forms.ListView.ListViewItemCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" />. </exception>
			// Token: 0x06005D7C RID: 23932 RVA: 0x00184290 File Offset: 0x00182490
			public ListViewItem Insert(int index, string text, string imageKey)
			{
				return this.Insert(index, new ListViewItem(text, imageKey));
			}

			/// <summary>Creates a new item with the specified key, text, and image, and adds it to the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the item is inserted.</param>
			/// <param name="key">The <see cref="P:System.Windows.Forms.ListViewItem.Name" /> of the item. </param>
			/// <param name="text">The text of the item.</param>
			/// <param name="imageKey">The key of the image to display for the item.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> added to the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than 0 or greater than the value of the <see cref="P:System.Windows.Forms.ListView.ListViewItemCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" />. </exception>
			// Token: 0x06005D7D RID: 23933 RVA: 0x001842A0 File Offset: 0x001824A0
			public virtual ListViewItem Insert(int index, string key, string text, string imageKey)
			{
				return this.Insert(index, new ListViewItem(text, imageKey)
				{
					Name = key
				});
			}

			/// <summary>Creates a new item with the specified key, text, and image, and inserts it in the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the item is inserted</param>
			/// <param name="key">The <see cref="P:System.Windows.Forms.ListViewItem.Name" /> of the item.</param>
			/// <param name="text">The text of the item.</param>
			/// <param name="imageIndex">The index of the image to display for the item.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> added to the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than 0 or greater than the value of the <see cref="P:System.Windows.Forms.ListView.ListViewItemCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" />. </exception>
			// Token: 0x06005D7E RID: 23934 RVA: 0x001842C8 File Offset: 0x001824C8
			public virtual ListViewItem Insert(int index, string key, string text, int imageIndex)
			{
				return this.Insert(index, new ListViewItem(text, imageIndex)
				{
					Name = key
				});
			}

			/// <summary>Removes the specified item from the collection.</summary>
			/// <param name="item">A <see cref="T:System.Windows.Forms.ListViewItem" /> representing the item to remove from the collection. </param>
			/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Windows.Forms.ListViewItem" /> assigned to the <paramref name="item" /> parameter is <see langword="null" />. </exception>
			// Token: 0x06005D7F RID: 23935 RVA: 0x001842ED File Offset: 0x001824ED
			public virtual void Remove(ListViewItem item)
			{
				this.InnerList.Remove(item);
			}

			/// <summary>Removes the item at the specified index within the collection.</summary>
			/// <param name="index">The zero-based index of the item to remove. </param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than 0 or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListView.ListViewItemCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListView.ListViewItemCollection" />. </exception>
			// Token: 0x06005D80 RID: 23936 RVA: 0x001842FC File Offset: 0x001824FC
			public virtual void RemoveAt(int index)
			{
				if (index < 0 || index >= this.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.InnerList.RemoveAt(index);
			}

			/// <summary>Removes the item with the specified key from the collection.</summary>
			/// <param name="key">The name of the item to remove from the collection.</param>
			// Token: 0x06005D81 RID: 23937 RVA: 0x00184354 File Offset: 0x00182554
			public virtual void RemoveByKey(string key)
			{
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					this.RemoveAt(index);
				}
			}

			/// <summary>Removes the specified item from the collection.</summary>
			/// <param name="item">A <see cref="T:System.Windows.Forms.ListViewItem" /> that represents the item to remove from the collection.</param>
			// Token: 0x06005D82 RID: 23938 RVA: 0x00184379 File Offset: 0x00182579
			void IList.Remove(object item)
			{
				if (item == null || !(item is ListViewItem))
				{
					return;
				}
				this.Remove((ListViewItem)item);
			}

			// Token: 0x04003A09 RID: 14857
			private int lastAccessedIndex = -1;

			// Token: 0x04003A0A RID: 14858
			private ListView.ListViewItemCollection.IInnerList innerList;

			// Token: 0x0200089A RID: 2202
			internal interface IInnerList
			{
				// Token: 0x1700187E RID: 6270
				// (get) Token: 0x060070C1 RID: 28865
				int Count { get; }

				// Token: 0x1700187F RID: 6271
				// (get) Token: 0x060070C2 RID: 28866
				bool OwnerIsVirtualListView { get; }

				// Token: 0x17001880 RID: 6272
				// (get) Token: 0x060070C3 RID: 28867
				bool OwnerIsDesignMode { get; }

				// Token: 0x17001881 RID: 6273
				ListViewItem this[int index]
				{
					get;
					set;
				}

				// Token: 0x060070C6 RID: 28870
				ListViewItem Add(ListViewItem item);

				// Token: 0x060070C7 RID: 28871
				void AddRange(ListViewItem[] items);

				// Token: 0x060070C8 RID: 28872
				void Clear();

				// Token: 0x060070C9 RID: 28873
				bool Contains(ListViewItem item);

				// Token: 0x060070CA RID: 28874
				void CopyTo(Array dest, int index);

				// Token: 0x060070CB RID: 28875
				IEnumerator GetEnumerator();

				// Token: 0x060070CC RID: 28876
				int IndexOf(ListViewItem item);

				// Token: 0x060070CD RID: 28877
				ListViewItem Insert(int index, ListViewItem item);

				// Token: 0x060070CE RID: 28878
				void Remove(ListViewItem item);

				// Token: 0x060070CF RID: 28879
				void RemoveAt(int index);
			}
		}

		// Token: 0x02000611 RID: 1553
		internal class ListViewNativeItemCollection : ListView.ListViewItemCollection.IInnerList
		{
			// Token: 0x06005D83 RID: 23939 RVA: 0x00184393 File Offset: 0x00182593
			public ListViewNativeItemCollection(ListView owner)
			{
				this.owner = owner;
			}

			// Token: 0x1700166E RID: 5742
			// (get) Token: 0x06005D84 RID: 23940 RVA: 0x001843A2 File Offset: 0x001825A2
			public int Count
			{
				get
				{
					this.owner.ApplyUpdateCachedItems();
					if (this.owner.VirtualMode)
					{
						return this.owner.VirtualListSize;
					}
					return this.owner.itemCount;
				}
			}

			// Token: 0x1700166F RID: 5743
			// (get) Token: 0x06005D85 RID: 23941 RVA: 0x001843D3 File Offset: 0x001825D3
			public bool OwnerIsVirtualListView
			{
				get
				{
					return this.owner.VirtualMode;
				}
			}

			// Token: 0x17001670 RID: 5744
			// (get) Token: 0x06005D86 RID: 23942 RVA: 0x001843E0 File Offset: 0x001825E0
			public bool OwnerIsDesignMode
			{
				get
				{
					return this.owner.DesignMode;
				}
			}

			// Token: 0x17001671 RID: 5745
			public ListViewItem this[int displayIndex]
			{
				get
				{
					this.owner.ApplyUpdateCachedItems();
					if (this.owner.VirtualMode)
					{
						RetrieveVirtualItemEventArgs retrieveVirtualItemEventArgs = new RetrieveVirtualItemEventArgs(displayIndex);
						this.owner.OnRetrieveVirtualItem(retrieveVirtualItemEventArgs);
						retrieveVirtualItemEventArgs.Item.SetItemIndex(this.owner, displayIndex);
						return retrieveVirtualItemEventArgs.Item;
					}
					if (displayIndex < 0 || displayIndex >= this.owner.itemCount)
					{
						throw new ArgumentOutOfRangeException("displayIndex", SR.GetString("InvalidArgument", new object[]
						{
							"displayIndex",
							displayIndex.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.owner.IsHandleCreated && !this.owner.ListViewHandleDestroyed)
					{
						return (ListViewItem)this.owner.listItemsTable[this.DisplayIndexToID(displayIndex)];
					}
					return (ListViewItem)this.owner.listItemsArray[displayIndex];
				}
				set
				{
					this.owner.ApplyUpdateCachedItems();
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantModifyTheItemCollInAVirtualListView"));
					}
					if (displayIndex < 0 || displayIndex >= this.owner.itemCount)
					{
						throw new ArgumentOutOfRangeException("displayIndex", SR.GetString("InvalidArgument", new object[]
						{
							"displayIndex",
							displayIndex.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.owner.ExpectingMouseUp)
					{
						this.owner.ItemCollectionChangedInMouseDown = true;
					}
					this.RemoveAt(displayIndex);
					this.Insert(displayIndex, value);
				}
			}

			// Token: 0x06005D89 RID: 23945 RVA: 0x0018457C File Offset: 0x0018277C
			public ListViewItem Add(ListViewItem value)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAddItemsToAVirtualListView"));
				}
				bool @checked = value.Checked;
				this.owner.InsertItems(this.owner.itemCount, new ListViewItem[]
				{
					value
				}, true);
				if (this.owner.IsHandleCreated && !this.owner.CheckBoxes && @checked)
				{
					this.owner.UpdateSavedCheckedItems(value, true);
				}
				if (this.owner.ExpectingMouseUp)
				{
					this.owner.ItemCollectionChangedInMouseDown = true;
				}
				return value;
			}

			// Token: 0x06005D8A RID: 23946 RVA: 0x00184618 File Offset: 0x00182818
			public void AddRange(ListViewItem[] values)
			{
				if (values == null)
				{
					throw new ArgumentNullException("values");
				}
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAddItemsToAVirtualListView"));
				}
				IComparer listItemSorter = this.owner.listItemSorter;
				this.owner.listItemSorter = null;
				bool[] array = null;
				if (this.owner.IsHandleCreated && !this.owner.CheckBoxes)
				{
					array = new bool[values.Length];
					for (int i = 0; i < values.Length; i++)
					{
						array[i] = values[i].Checked;
					}
				}
				try
				{
					this.owner.BeginUpdate();
					this.owner.InsertItems(this.owner.itemCount, values, true);
					if (this.owner.IsHandleCreated && !this.owner.CheckBoxes)
					{
						for (int j = 0; j < values.Length; j++)
						{
							if (array[j])
							{
								this.owner.UpdateSavedCheckedItems(values[j], true);
							}
						}
					}
				}
				finally
				{
					this.owner.listItemSorter = listItemSorter;
					this.owner.EndUpdate();
				}
				if (this.owner.ExpectingMouseUp)
				{
					this.owner.ItemCollectionChangedInMouseDown = true;
				}
				if (listItemSorter != null || (this.owner.Sorting != SortOrder.None && !this.owner.VirtualMode))
				{
					this.owner.Sort();
				}
			}

			// Token: 0x06005D8B RID: 23947 RVA: 0x00184774 File Offset: 0x00182974
			private int DisplayIndexToID(int displayIndex)
			{
				if (this.owner.IsHandleCreated && !this.owner.ListViewHandleDestroyed)
				{
					NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
					lvitem.mask = 4;
					lvitem.iItem = displayIndex;
					UnsafeNativeMethods.SendMessage(new HandleRef(this.owner, this.owner.Handle), NativeMethods.LVM_GETITEM, 0, ref lvitem);
					return (int)lvitem.lParam;
				}
				return this[displayIndex].ID;
			}

			// Token: 0x06005D8C RID: 23948 RVA: 0x001847F0 File Offset: 0x001829F0
			public void Clear()
			{
				if (this.owner.itemCount > 0)
				{
					this.owner.ApplyUpdateCachedItems();
					if (this.owner.IsHandleCreated && !this.owner.ListViewHandleDestroyed)
					{
						int count = this.owner.Items.Count;
						int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this.owner, this.owner.Handle), 4108, -1, 2);
						for (int i = 0; i < count; i++)
						{
							ListViewItem listViewItem = this.owner.Items[i];
							if (listViewItem != null)
							{
								if (i == num)
								{
									listViewItem.StateSelected = true;
									num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this.owner, this.owner.Handle), 4108, num, 2);
								}
								else
								{
									listViewItem.StateSelected = false;
								}
								listViewItem.UnHost(i, false);
							}
						}
						UnsafeNativeMethods.SendMessage(new HandleRef(this.owner, this.owner.Handle), 4105, 0, 0);
						if (this.owner.View == View.SmallIcon)
						{
							if (this.owner.ComctlSupportsVisualStyles)
							{
								this.owner.FlipViewToLargeIconAndSmallIcon = true;
							}
							else
							{
								this.owner.View = View.LargeIcon;
								this.owner.View = View.SmallIcon;
							}
						}
					}
					else
					{
						int count2 = this.owner.Items.Count;
						for (int j = 0; j < count2; j++)
						{
							ListViewItem listViewItem2 = this.owner.Items[j];
							if (listViewItem2 != null)
							{
								listViewItem2.UnHost(j, true);
							}
						}
						this.owner.listItemsArray.Clear();
					}
					this.owner.listItemsTable.Clear();
					if (this.owner.IsHandleCreated && !this.owner.CheckBoxes)
					{
						this.owner.savedCheckedItems = null;
					}
					this.owner.itemCount = 0;
					if (this.owner.ExpectingMouseUp)
					{
						this.owner.ItemCollectionChangedInMouseDown = true;
					}
				}
			}

			// Token: 0x06005D8D RID: 23949 RVA: 0x001849F0 File Offset: 0x00182BF0
			public bool Contains(ListViewItem item)
			{
				this.owner.ApplyUpdateCachedItems();
				if (this.owner.IsHandleCreated && !this.owner.ListViewHandleDestroyed)
				{
					return this.owner.listItemsTable[item.ID] == item;
				}
				return this.owner.listItemsArray.Contains(item);
			}

			// Token: 0x06005D8E RID: 23950 RVA: 0x00184A54 File Offset: 0x00182C54
			public ListViewItem Insert(int index, ListViewItem item)
			{
				int num;
				if (this.owner.VirtualMode)
				{
					num = this.Count;
				}
				else
				{
					num = this.owner.itemCount;
				}
				if (index < 0 || index > num)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAddItemsToAVirtualListView"));
				}
				if (index < num)
				{
					this.owner.ApplyUpdateCachedItems();
				}
				this.owner.InsertItems(index, new ListViewItem[]
				{
					item
				}, true);
				if (this.owner.IsHandleCreated && !this.owner.CheckBoxes && item.Checked)
				{
					this.owner.UpdateSavedCheckedItems(item, true);
				}
				if (this.owner.ExpectingMouseUp)
				{
					this.owner.ItemCollectionChangedInMouseDown = true;
				}
				return item;
			}

			// Token: 0x06005D8F RID: 23951 RVA: 0x00184B4C File Offset: 0x00182D4C
			public int IndexOf(ListViewItem item)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (item == this[i])
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06005D90 RID: 23952 RVA: 0x00184B78 File Offset: 0x00182D78
			public void Remove(ListViewItem item)
			{
				int num = this.owner.VirtualMode ? (this.Count - 1) : this.IndexOf(item);
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantRemoveItemsFromAVirtualListView"));
				}
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x06005D91 RID: 23953 RVA: 0x00184BCC File Offset: 0x00182DCC
			public void RemoveAt(int index)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantRemoveItemsFromAVirtualListView"));
				}
				if (index < 0 || index >= this.owner.itemCount)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owner.IsHandleCreated && !this.owner.CheckBoxes && this[index].Checked)
				{
					this.owner.UpdateSavedCheckedItems(this[index], false);
				}
				this.owner.ApplyUpdateCachedItems();
				int num = this.DisplayIndexToID(index);
				this[index].UnHost(true);
				if (this.owner.IsHandleCreated)
				{
					if ((int)((long)this.owner.SendMessage(4104, index, 0)) == 0)
					{
						throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
				}
				else
				{
					this.owner.listItemsArray.RemoveAt(index);
				}
				this.owner.itemCount--;
				this.owner.listItemsTable.Remove(num);
				if (this.owner.ExpectingMouseUp)
				{
					this.owner.ItemCollectionChangedInMouseDown = true;
				}
			}

			// Token: 0x06005D92 RID: 23954 RVA: 0x00184D40 File Offset: 0x00182F40
			public void CopyTo(Array dest, int index)
			{
				if (this.owner.itemCount > 0)
				{
					for (int i = 0; i < this.Count; i++)
					{
						dest.SetValue(this[i], index++);
					}
				}
			}

			// Token: 0x06005D93 RID: 23955 RVA: 0x00184D80 File Offset: 0x00182F80
			public IEnumerator GetEnumerator()
			{
				ListViewItem[] array = new ListViewItem[this.owner.itemCount];
				this.CopyTo(array, 0);
				return array.GetEnumerator();
			}

			// Token: 0x04003A0B RID: 14859
			private ListView owner;
		}

		// Token: 0x02000612 RID: 1554
		internal class ListViewAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06005D94 RID: 23956 RVA: 0x00184DAC File Offset: 0x00182FAC
			internal ListViewAccessibleObject(ListView owner) : base(owner)
			{
				this.owner = owner;
			}

			// Token: 0x06005D95 RID: 23957 RVA: 0x00184DBC File Offset: 0x00182FBC
			internal override bool IsIAccessibleExSupported()
			{
				return this.owner != null || base.IsIAccessibleExSupported();
			}

			// Token: 0x06005D96 RID: 23958 RVA: 0x00184DD0 File Offset: 0x00182FD0
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30026)
				{
					switch (this.owner.Sorting)
					{
					case SortOrder.None:
						return SR.GetString("NotSortedAccessibleStatus");
					case SortOrder.Ascending:
						return SR.GetString("SortedAscendingAccessibleStatus");
					case SortOrder.Descending:
						return SR.GetString("SortedDescendingAccessibleStatus");
					}
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x04003A0C RID: 14860
			private ListView owner;
		}
	}
}
