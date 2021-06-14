using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Displays a <see cref="T:System.Windows.Forms.ListBox" /> in which a check box is displayed to the left of each item.</summary>
	// Token: 0x0200013D RID: 317
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[LookupBindingProperties]
	[SRDescription("DescriptionCheckedListBox")]
	public class CheckedListBox : ListBox
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.CheckedListBox" /> class.</summary>
		// Token: 0x060009D3 RID: 2515 RVA: 0x0001D8CE File Offset: 0x0001BACE
		public CheckedListBox()
		{
			base.SetStyle(ControlStyles.ResizeRedraw, true);
		}

		/// <summary>Gets or sets a value indicating whether the check box should be toggled when an item is selected.</summary>
		/// <returns>
		///     <see langword="true" /> if the check mark is applied immediately; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x0001D8F5 File Offset: 0x0001BAF5
		// (set) Token: 0x060009D5 RID: 2517 RVA: 0x0001D8FD File Offset: 0x0001BAFD
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("CheckedListBoxCheckOnClickDescr")]
		public bool CheckOnClick
		{
			get
			{
				return this.checkOnClick;
			}
			set
			{
				this.checkOnClick = value;
			}
		}

		/// <summary>Collection of checked indexes in this <see cref="T:System.Windows.Forms.CheckedListBox" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.CheckedListBox.CheckedIndexCollection" /> collection for the <see cref="T:System.Windows.Forms.CheckedListBox" />.</returns>
		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x060009D6 RID: 2518 RVA: 0x0001D906 File Offset: 0x0001BB06
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CheckedListBox.CheckedIndexCollection CheckedIndices
		{
			get
			{
				if (this.checkedIndexCollection == null)
				{
					this.checkedIndexCollection = new CheckedListBox.CheckedIndexCollection(this);
				}
				return this.checkedIndexCollection;
			}
		}

		/// <summary>Collection of checked items in this <see cref="T:System.Windows.Forms.CheckedListBox" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.CheckedListBox.CheckedItemCollection" /> collection for the <see cref="T:System.Windows.Forms.CheckedListBox" />.</returns>
		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x0001D922 File Offset: 0x0001BB22
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CheckedListBox.CheckedItemCollection CheckedItems
		{
			get
			{
				if (this.checkedItemCollection == null)
				{
					this.checkedItemCollection = new CheckedListBox.CheckedItemCollection(this);
				}
				return this.checkedItemCollection;
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required parameters.</returns>
		// Token: 0x170002CA RID: 714
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x0001D940 File Offset: 0x0001BB40
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style |= 1040;
				return createParams;
			}
		}

		/// <summary>Gets or sets the data source for the control.</summary>
		/// <returns>An object representing the source of the data.</returns>
		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x0001D967 File Offset: 0x0001BB67
		// (set) Token: 0x060009DA RID: 2522 RVA: 0x0001D96F File Offset: 0x0001BB6F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new object DataSource
		{
			get
			{
				return base.DataSource;
			}
			set
			{
				base.DataSource = value;
			}
		}

		/// <summary>Gets or sets a string that specifies a property of the objects contained in the list box whose contents you want to display.</summary>
		/// <returns>A string that specifies the name of a property of the objects contained in the list box. The default is an empty string ("").</returns>
		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x0001D978 File Offset: 0x0001BB78
		// (set) Token: 0x060009DC RID: 2524 RVA: 0x0001D980 File Offset: 0x0001BB80
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new string DisplayMember
		{
			get
			{
				return base.DisplayMember;
			}
			set
			{
				base.DisplayMember = value;
			}
		}

		/// <summary>Gets a value indicating the mode for drawing elements of the <see cref="T:System.Windows.Forms.CheckedListBox" />. This property is not relevant to this class.</summary>
		/// <returns>Always a <see cref="T:System.Windows.Forms.DrawMode" /> of <see langword="Normal" />.</returns>
		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		// (set) Token: 0x060009DE RID: 2526 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override DrawMode DrawMode
		{
			get
			{
				return DrawMode.Normal;
			}
			set
			{
			}
		}

		/// <summary>Gets the height of the item area.</summary>
		/// <returns>The height, in pixels, of the item area.</returns>
		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x0001D989 File Offset: 0x0001BB89
		// (set) Token: 0x060009E0 RID: 2528 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override int ItemHeight
		{
			get
			{
				return this.Font.Height + this.scaledListItemBordersHeight;
			}
			set
			{
			}
		}

		/// <summary>Gets the collection of items in this <see cref="T:System.Windows.Forms.CheckedListBox" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.CheckedListBox.ObjectCollection" /> collection representing the items in the <see cref="T:System.Windows.Forms.CheckedListBox" />.</returns>
		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x0001D99D File Offset: 0x0001BB9D
		[SRCategory("CatData")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRDescription("ListBoxItemsDescr")]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public new CheckedListBox.ObjectCollection Items
		{
			get
			{
				return (CheckedListBox.ObjectCollection)base.Items;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x0001D9AA File Offset: 0x0001BBAA
		internal override int MaxItemWidth
		{
			get
			{
				return base.MaxItemWidth + this.idealCheckSize + this.scaledListItemPaddingBuffer;
			}
		}

		/// <summary>Gets or sets a value specifying the selection mode.</summary>
		/// <returns>Either the <see langword="One" /> or <see langword="None" /> value of <see cref="T:System.Windows.Forms.SelectionMode" />.</returns>
		/// <exception cref="T:System.ArgumentException">An attempt was made to assign a value that is not a <see cref="T:System.Windows.Forms.SelectionMode" /> value of <see langword="One" /> or <see langword="None" />. </exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">An attempt was made to assign the <see langword="MultiExtended" /> value of <see cref="T:System.Windows.Forms.SelectionMode" /> to the control.</exception>
		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x0001D9C0 File Offset: 0x0001BBC0
		// (set) Token: 0x060009E4 RID: 2532 RVA: 0x0001D9C8 File Offset: 0x0001BBC8
		public override SelectionMode SelectionMode
		{
			get
			{
				return base.SelectionMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SelectionMode));
				}
				if (value != SelectionMode.One && value != SelectionMode.None)
				{
					throw new ArgumentException(SR.GetString("CheckedListBoxInvalidSelectionMode"));
				}
				if (value != this.SelectionMode)
				{
					base.SelectionMode = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the check boxes have a <see cref="T:System.Windows.Forms.ButtonState" /> of <see langword="Flat" /> or <see langword="Normal" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the check box has a flat appearance; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0001DA28 File Offset: 0x0001BC28
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x0001DA34 File Offset: 0x0001BC34
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		[SRDescription("CheckedListBoxThreeDCheckBoxesDescr")]
		public bool ThreeDCheckBoxes
		{
			get
			{
				return !this.flat;
			}
			set
			{
				if (this.flat == value)
				{
					this.flat = !value;
					CheckedListBox.ObjectCollection items = this.Items;
					if (items != null && items.Count > 0)
					{
						base.Invalidate();
					}
				}
			}
		}

		/// <summary>Gets or sets a value that determines whether to use the <see cref="T:System.Drawing.Graphics" /> class (GDI+) or the <see cref="T:System.Windows.Forms.TextRenderer" /> class (GDI) to render text.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Drawing.Graphics" /> class should be used to perform text rendering for compatibility with versions 1.0 and 1.1. of the .NET Framework; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x0001C7CB File Offset: 0x0001A9CB
		// (set) Token: 0x060009E8 RID: 2536 RVA: 0x0001C7D3 File Offset: 0x0001A9D3
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("UseCompatibleTextRenderingDescr")]
		public bool UseCompatibleTextRendering
		{
			get
			{
				return base.UseCompatibleTextRenderingInt;
			}
			set
			{
				base.UseCompatibleTextRenderingInt = value;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override bool SupportsUseCompatibleTextRendering
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets a string that specifies the property of the data source from which to draw the value. </summary>
		/// <returns>A string that specifies the property of the data source from which to draw the value.</returns>
		/// <exception cref="T:System.ArgumentException">The specified property cannot be found on the object specified by the <see cref="P:System.Windows.Forms.CheckedListBox.DataSource" /> property.</exception>
		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x0001DA6D File Offset: 0x0001BC6D
		// (set) Token: 0x060009EB RID: 2539 RVA: 0x0001DA75 File Offset: 0x0001BC75
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new string ValueMember
		{
			get
			{
				return base.ValueMember;
			}
			set
			{
				base.ValueMember = value;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.CheckedListBox.DataSource" /> property changes.</summary>
		// Token: 0x14000050 RID: 80
		// (add) Token: 0x060009EC RID: 2540 RVA: 0x0001DA7E File Offset: 0x0001BC7E
		// (remove) Token: 0x060009ED RID: 2541 RVA: 0x0001DA87 File Offset: 0x0001BC87
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DataSourceChanged
		{
			add
			{
				base.DataSourceChanged += value;
			}
			remove
			{
				base.DataSourceChanged -= value;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.CheckedListBox.DisplayMember" /> property changes. </summary>
		// Token: 0x14000051 RID: 81
		// (add) Token: 0x060009EE RID: 2542 RVA: 0x0001DA90 File Offset: 0x0001BC90
		// (remove) Token: 0x060009EF RID: 2543 RVA: 0x0001DA99 File Offset: 0x0001BC99
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DisplayMemberChanged
		{
			add
			{
				base.DisplayMemberChanged += value;
			}
			remove
			{
				base.DisplayMemberChanged -= value;
			}
		}

		/// <summary>Occurs when the checked state of an item changes.</summary>
		// Token: 0x14000052 RID: 82
		// (add) Token: 0x060009F0 RID: 2544 RVA: 0x0001DAA2 File Offset: 0x0001BCA2
		// (remove) Token: 0x060009F1 RID: 2545 RVA: 0x0001DABB File Offset: 0x0001BCBB
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

		/// <summary>Occurs when the user clicks the <see cref="T:System.Windows.Forms.CheckedListBox" /> control.</summary>
		// Token: 0x14000053 RID: 83
		// (add) Token: 0x060009F2 RID: 2546 RVA: 0x0001DAD4 File Offset: 0x0001BCD4
		// (remove) Token: 0x060009F3 RID: 2547 RVA: 0x0001DADD File Offset: 0x0001BCDD
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event EventHandler Click
		{
			add
			{
				base.Click += value;
			}
			remove
			{
				base.Click -= value;
			}
		}

		/// <summary>Occurs when the user clicks the <see cref="T:System.Windows.Forms.CheckedListBox" /> control with the mouse.</summary>
		// Token: 0x14000054 RID: 84
		// (add) Token: 0x060009F4 RID: 2548 RVA: 0x0001DAE6 File Offset: 0x0001BCE6
		// (remove) Token: 0x060009F5 RID: 2549 RVA: 0x0001DAEF File Offset: 0x0001BCEF
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event MouseEventHandler MouseClick
		{
			add
			{
				base.MouseClick += value;
			}
			remove
			{
				base.MouseClick -= value;
			}
		}

		/// <summary>Occurs when a visual aspect of an owner-drawn <see cref="T:System.Windows.Forms.CheckedListBox" /> changes. This event is not relevant to this class.</summary>
		// Token: 0x14000055 RID: 85
		// (add) Token: 0x060009F6 RID: 2550 RVA: 0x0001DAF8 File Offset: 0x0001BCF8
		// (remove) Token: 0x060009F7 RID: 2551 RVA: 0x0001DB01 File Offset: 0x0001BD01
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event DrawItemEventHandler DrawItem
		{
			add
			{
				base.DrawItem += value;
			}
			remove
			{
				base.DrawItem -= value;
			}
		}

		/// <summary>Occurs when an owner-drawn <see cref="T:System.Windows.Forms.ListBox" /> is created and the sizes of the list items are determined. This event is not relevant to this class.</summary>
		// Token: 0x14000056 RID: 86
		// (add) Token: 0x060009F8 RID: 2552 RVA: 0x0001DB0A File Offset: 0x0001BD0A
		// (remove) Token: 0x060009F9 RID: 2553 RVA: 0x0001DB13 File Offset: 0x0001BD13
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MeasureItemEventHandler MeasureItem
		{
			add
			{
				base.MeasureItem += value;
			}
			remove
			{
				base.MeasureItem -= value;
			}
		}

		/// <summary>Gets or sets padding within the <see cref="T:System.Windows.Forms.CheckedListBox" />. This property is not relevant to this class.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> representing the control's internal spacing characteristics.</returns>
		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x0001DB1C File Offset: 0x0001BD1C
		// (set) Token: 0x060009FB RID: 2555 RVA: 0x0001DB24 File Offset: 0x0001BD24
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.CheckedListBox.ValueMember" /> property changes.</summary>
		// Token: 0x14000057 RID: 87
		// (add) Token: 0x060009FC RID: 2556 RVA: 0x0001DB2D File Offset: 0x0001BD2D
		// (remove) Token: 0x060009FD RID: 2557 RVA: 0x0001DB36 File Offset: 0x0001BD36
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ValueMemberChanged
		{
			add
			{
				base.ValueMemberChanged += value;
			}
			remove
			{
				base.ValueMemberChanged -= value;
			}
		}

		/// <summary>Creates a new accessibility object for the <see cref="T:System.Windows.Forms.CheckedListBox" /> control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the control.</returns>
		// Token: 0x060009FE RID: 2558 RVA: 0x0001DB3F File Offset: 0x0001BD3F
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new CheckedListBox.CheckedListBoxAccessibleObject(this);
		}

		/// <summary>Creates a new instance of the item collection.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> that represents the new item collection.</returns>
		// Token: 0x060009FF RID: 2559 RVA: 0x0001DB47 File Offset: 0x0001BD47
		protected override ListBox.ObjectCollection CreateItemCollection()
		{
			return new CheckedListBox.ObjectCollection(this);
		}

		/// <summary>Returns a value indicating the check state of the current item.</summary>
		/// <param name="index">The index of the item to get the checked value of. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.CheckState" /> values.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> specified is less than zero.-or- The <paramref name="index" /> specified is greater than or equal to the count of items in the list. </exception>
		// Token: 0x06000A00 RID: 2560 RVA: 0x0001DB50 File Offset: 0x0001BD50
		public CheckState GetItemCheckState(int index)
		{
			if (index < 0 || index >= this.Items.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return this.CheckedItems.GetCheckedState(index);
		}

		/// <summary>Returns a value indicating whether the specified item is checked.</summary>
		/// <param name="index">The index of the item. </param>
		/// <returns>
		///     <see langword="true" /> if the item is checked; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">The <paramref name="index" /> specified is less than zero.-or- The <paramref name="index" /> specified is greater than or equal to the count of items in the list. </exception>
		// Token: 0x06000A01 RID: 2561 RVA: 0x0001DBAD File Offset: 0x0001BDAD
		public bool GetItemChecked(int index)
		{
			return this.GetItemCheckState(index) > CheckState.Unchecked;
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0001DBBC File Offset: 0x0001BDBC
		private void InvalidateItem(int index)
		{
			if (base.IsHandleCreated)
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				base.SendMessage(408, index, ref rect);
				SafeNativeMethods.InvalidateRect(new HandleRef(this, base.Handle), ref rect, false);
			}
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0001DC00 File Offset: 0x0001BE00
		private void LbnSelChange()
		{
			int selectedIndex = this.SelectedIndex;
			if (selectedIndex < 0 || selectedIndex >= this.Items.Count)
			{
				return;
			}
			base.AccessibilityNotifyClients(AccessibleEvents.Focus, selectedIndex);
			base.AccessibilityNotifyClients(AccessibleEvents.Selection, selectedIndex);
			if (!this.killnextselect && (selectedIndex == this.lastSelected || this.checkOnClick))
			{
				CheckState checkedState = this.CheckedItems.GetCheckedState(selectedIndex);
				CheckState newCheckValue = (checkedState != CheckState.Unchecked) ? CheckState.Unchecked : CheckState.Checked;
				ItemCheckEventArgs itemCheckEventArgs = new ItemCheckEventArgs(selectedIndex, newCheckValue, checkedState);
				this.OnItemCheck(itemCheckEventArgs);
				this.CheckedItems.SetCheckedState(selectedIndex, itemCheckEventArgs.NewValue);
				if (AccessibilityImprovements.Level1)
				{
					base.AccessibilityNotifyClients(AccessibleEvents.StateChange, selectedIndex);
					base.AccessibilityNotifyClients(AccessibleEvents.NameChange, selectedIndex);
				}
			}
			this.lastSelected = selectedIndex;
			this.InvalidateItem(selectedIndex);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.CheckedListBox.Click" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000A04 RID: 2564 RVA: 0x0001DCBC File Offset: 0x0001BEBC
		protected override void OnClick(EventArgs e)
		{
			this.killnextselect = false;
			base.OnClick(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000A05 RID: 2565 RVA: 0x0001DCCC File Offset: 0x0001BECC
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			base.SendMessage(416, 0, this.ItemHeight);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.CheckedListBox.DrawItem" /> event.</summary>
		/// <param name="e">The <see cref="T:System.Windows.Forms.DrawItemEventArgs" /> object with the details </param>
		// Token: 0x06000A06 RID: 2566 RVA: 0x0001DCE8 File Offset: 0x0001BEE8
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (this.Font.Height < 0)
			{
				this.Font = Control.DefaultFont;
			}
			if (e.Index >= 0)
			{
				object item;
				if (e.Index < this.Items.Count)
				{
					item = this.Items[e.Index];
				}
				else
				{
					item = base.NativeGetItemText(e.Index);
				}
				Rectangle bounds = e.Bounds;
				int itemHeight = this.ItemHeight;
				ButtonState buttonState = ButtonState.Normal;
				if (this.flat)
				{
					buttonState |= ButtonState.Flat;
				}
				if (e.Index < this.Items.Count)
				{
					CheckState checkedState = this.CheckedItems.GetCheckedState(e.Index);
					if (checkedState != CheckState.Checked)
					{
						if (checkedState == CheckState.Indeterminate)
						{
							buttonState |= (ButtonState.Checked | ButtonState.Inactive);
						}
					}
					else
					{
						buttonState |= ButtonState.Checked;
					}
				}
				if (Application.RenderWithVisualStyles)
				{
					CheckBoxState state = CheckBoxRenderer.ConvertFromButtonState(buttonState, false, (e.State & DrawItemState.HotLight) == DrawItemState.HotLight);
					this.idealCheckSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, state, base.HandleInternal).Width;
				}
				int num = Math.Max((itemHeight - this.idealCheckSize) / 2, 0);
				if (num + this.idealCheckSize > bounds.Height)
				{
					num = bounds.Height - this.idealCheckSize;
				}
				Rectangle rectangle = new Rectangle(bounds.X + this.scaledListItemStartPosition, bounds.Y + num, this.idealCheckSize, this.idealCheckSize);
				if (this.RightToLeft == RightToLeft.Yes)
				{
					rectangle.X = bounds.X + bounds.Width - this.idealCheckSize - this.scaledListItemStartPosition;
				}
				if (Application.RenderWithVisualStyles)
				{
					CheckBoxState state2 = CheckBoxRenderer.ConvertFromButtonState(buttonState, false, (e.State & DrawItemState.HotLight) == DrawItemState.HotLight);
					CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(rectangle.X, rectangle.Y), state2, base.HandleInternal);
				}
				else
				{
					ControlPaint.DrawCheckBox(e.Graphics, rectangle, buttonState);
				}
				Rectangle rectangle2 = new Rectangle(bounds.X + this.idealCheckSize + this.scaledListItemStartPosition * 2, bounds.Y, bounds.Width - (this.idealCheckSize + this.scaledListItemStartPosition * 2), bounds.Height);
				if (this.RightToLeft == RightToLeft.Yes)
				{
					rectangle2.X = bounds.X;
				}
				string text = "";
				Color color = (this.SelectionMode != SelectionMode.None) ? e.BackColor : this.BackColor;
				Color color2 = (this.SelectionMode != SelectionMode.None) ? e.ForeColor : this.ForeColor;
				if (!base.Enabled)
				{
					color2 = SystemColors.GrayText;
				}
				Font font = this.Font;
				text = base.GetItemText(item);
				if (this.SelectionMode != SelectionMode.None && (e.State & DrawItemState.Selected) == DrawItemState.Selected)
				{
					if (base.Enabled)
					{
						color = SystemColors.Highlight;
						color2 = SystemColors.HighlightText;
					}
					else
					{
						color = SystemColors.InactiveBorder;
						color2 = SystemColors.GrayText;
					}
				}
				using (Brush brush = new SolidBrush(color))
				{
					e.Graphics.FillRectangle(brush, rectangle2);
				}
				Rectangle rectangle3 = new Rectangle(rectangle2.X + 1, rectangle2.Y, rectangle2.Width - 1, rectangle2.Height - 2);
				if (this.UseCompatibleTextRendering)
				{
					using (StringFormat stringFormat = new StringFormat())
					{
						if (base.UseTabStops)
						{
							float num2 = 3.6f * (float)this.Font.Height;
							float[] array = new float[15];
							float num3 = (float)(-(float)(this.idealCheckSize + this.scaledListItemStartPosition * 2));
							for (int i = 1; i < array.Length; i++)
							{
								array[i] = num2;
							}
							if (Math.Abs(num3) < num2)
							{
								array[0] = num2 + num3;
							}
							else
							{
								array[0] = num2;
							}
							stringFormat.SetTabStops(0f, array);
						}
						else if (base.UseCustomTabOffsets)
						{
							int count = base.CustomTabOffsets.Count;
							float[] array2 = new float[count];
							base.CustomTabOffsets.CopyTo(array2, 0);
							stringFormat.SetTabStops(0f, array2);
						}
						if (this.RightToLeft == RightToLeft.Yes)
						{
							stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
						}
						stringFormat.FormatFlags |= StringFormatFlags.NoWrap;
						stringFormat.Trimming = StringTrimming.None;
						using (SolidBrush solidBrush = new SolidBrush(color2))
						{
							e.Graphics.DrawString(text, font, solidBrush, rectangle3, stringFormat);
							goto IL_4A0;
						}
					}
				}
				TextFormatFlags textFormatFlags = TextFormatFlags.Default;
				textFormatFlags |= TextFormatFlags.NoPrefix;
				if (base.UseTabStops || base.UseCustomTabOffsets)
				{
					textFormatFlags |= TextFormatFlags.ExpandTabs;
				}
				if (this.RightToLeft == RightToLeft.Yes)
				{
					textFormatFlags |= TextFormatFlags.RightToLeft;
					textFormatFlags |= TextFormatFlags.Right;
				}
				TextRenderer.DrawText(e.Graphics, text, font, rectangle3, color2, textFormatFlags);
				IL_4A0:
				if ((e.State & DrawItemState.Focus) == DrawItemState.Focus && (e.State & DrawItemState.NoFocusRect) != DrawItemState.NoFocusRect)
				{
					ControlPaint.DrawFocusRectangle(e.Graphics, rectangle2, color2, color);
				}
			}
			if (this.Items.Count == 0 && AccessibilityImprovements.Level3 && e.Bounds.Width > 2 && e.Bounds.Height > 2)
			{
				Color color3 = (this.SelectionMode != SelectionMode.None) ? e.BackColor : this.BackColor;
				Rectangle bounds2 = e.Bounds;
				Rectangle rectangle4 = new Rectangle(bounds2.X + 1, bounds2.Y, bounds2.Width - 1, bounds2.Height - 2);
				if (this.Focused)
				{
					Color foreColor = (this.SelectionMode != SelectionMode.None) ? e.ForeColor : this.ForeColor;
					if (!base.Enabled)
					{
						foreColor = SystemColors.GrayText;
					}
					ControlPaint.DrawFocusRectangle(e.Graphics, rectangle4, foreColor, color3);
					return;
				}
				if (!Application.RenderWithVisualStyles)
				{
					using (Brush brush2 = new SolidBrush(color3))
					{
						e.Graphics.FillRectangle(brush2, rectangle4);
					}
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000A07 RID: 2567 RVA: 0x0001E328 File Offset: 0x0001C528
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			if (base.IsHandleCreated)
			{
				SafeNativeMethods.InvalidateRect(new HandleRef(this, base.Handle), null, true);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000A08 RID: 2568 RVA: 0x0001E34D File Offset: 0x0001C54D
		protected override void OnFontChanged(EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(416, 0, this.ItemHeight);
			}
			base.OnFontChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyPress" /> event.</summary>
		/// <param name="e">The <see cref="T:System.Windows.Forms.KeyPressEventArgs" /> that was raised. </param>
		// Token: 0x06000A09 RID: 2569 RVA: 0x0001E371 File Offset: 0x0001C571
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if (e.KeyChar == ' ' && this.SelectionMode != SelectionMode.None)
			{
				this.LbnSelChange();
			}
			if (base.FormattingEnabled)
			{
				base.OnKeyPress(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.CheckedListBox.ItemCheck" /> event.</summary>
		/// <param name="ice">An <see cref="T:System.Windows.Forms.ItemCheckEventArgs" /> that contains the event data.</param>
		// Token: 0x06000A0A RID: 2570 RVA: 0x0001E39A File Offset: 0x0001C59A
		protected virtual void OnItemCheck(ItemCheckEventArgs ice)
		{
			if (this.onItemCheck != null)
			{
				this.onItemCheck(this, ice);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.CheckedListBox.MeasureItem" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MeasureItemEventArgs" /> that contains the event data. </param>
		// Token: 0x06000A0B RID: 2571 RVA: 0x0001E3B1 File Offset: 0x0001C5B1
		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			base.OnMeasureItem(e);
			if (e.ItemHeight < this.idealCheckSize + 2)
			{
				e.ItemHeight = this.idealCheckSize + 2;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListBox.SelectedIndexChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000A0C RID: 2572 RVA: 0x0001E3D8 File Offset: 0x0001C5D8
		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged(e);
			this.lastSelected = this.SelectedIndex;
		}

		/// <summary>Parses all <see cref="T:System.Windows.Forms.CheckedListBox" /> items again and gets new text strings for the items.</summary>
		// Token: 0x06000A0D RID: 2573 RVA: 0x0001E3F0 File Offset: 0x0001C5F0
		protected override void RefreshItems()
		{
			Hashtable hashtable = new Hashtable();
			for (int i = 0; i < this.Items.Count; i++)
			{
				hashtable[i] = this.CheckedItems.GetCheckedState(i);
			}
			base.RefreshItems();
			for (int j = 0; j < this.Items.Count; j++)
			{
				this.CheckedItems.SetCheckedState(j, (CheckState)hashtable[j]);
			}
		}

		/// <summary>Sets the check state of the item at the specified index.</summary>
		/// <param name="index">The index of the item to set the state for. </param>
		/// <param name="value">One of the <see cref="T:System.Windows.Forms.CheckState" /> values. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> specified is less than zero.-or- The <paramref name="index" /> is greater than or equal to the count of items in the list. </exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The <paramref name="value" /> is not one of the <see cref="T:System.Windows.Forms.CheckState" /> values. </exception>
		// Token: 0x06000A0E RID: 2574 RVA: 0x0001E470 File Offset: 0x0001C670
		public void SetItemCheckState(int index, CheckState value)
		{
			if (index < 0 || index >= this.Items.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
			{
				throw new InvalidEnumArgumentException("value", (int)value, typeof(CheckState));
			}
			CheckState checkedState = this.CheckedItems.GetCheckedState(index);
			if (value != checkedState)
			{
				ItemCheckEventArgs itemCheckEventArgs = new ItemCheckEventArgs(index, value, checkedState);
				this.OnItemCheck(itemCheckEventArgs);
				if (itemCheckEventArgs.NewValue != checkedState)
				{
					this.CheckedItems.SetCheckedState(index, itemCheckEventArgs.NewValue);
					this.InvalidateItem(index);
				}
			}
		}

		/// <summary>Sets <see cref="T:System.Windows.Forms.CheckState" /> for the item at the specified index to <see langword="Checked" />.</summary>
		/// <param name="index">The index of the item to set the check state for. </param>
		/// <param name="value">
		///       <see langword="true" /> to set the item as checked; otherwise, <see langword="false" />. </param>
		/// <exception cref="T:System.ArgumentException">The index specified is less than zero.-or- The index is greater than the count of items in the list. </exception>
		// Token: 0x06000A0F RID: 2575 RVA: 0x0001E52A File Offset: 0x0001C72A
		public void SetItemChecked(int index, bool value)
		{
			this.SetItemCheckState(index, value ? CheckState.Checked : CheckState.Unchecked);
		}

		/// <summary>Processes the command message the <see cref="T:System.Windows.Forms.CheckedListBox" /> control receives from the top-level window.</summary>
		/// <param name="m">The <see cref="T:System.Windows.Forms.Message" /> the top-level window sent to the <see cref="T:System.Windows.Forms.CheckedListBox" /> control.</param>
		// Token: 0x06000A10 RID: 2576 RVA: 0x0001E53C File Offset: 0x0001C73C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WmReflectCommand(ref Message m)
		{
			int num = NativeMethods.Util.HIWORD(m.WParam);
			if (num == 1)
			{
				this.LbnSelChange();
				base.WmReflectCommand(ref m);
				return;
			}
			if (num != 2)
			{
				base.WmReflectCommand(ref m);
				return;
			}
			this.LbnSelChange();
			base.WmReflectCommand(ref m);
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0001E584 File Offset: 0x0001C784
		private void WmReflectVKeyToItem(ref Message m)
		{
			int num = NativeMethods.Util.LOWORD(m.WParam);
			Keys keys = (Keys)num;
			if (keys - Keys.Prior <= 7)
			{
				this.killnextselect = true;
			}
			else
			{
				this.killnextselect = false;
			}
			m.Result = NativeMethods.InvalidIntPtr;
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06000A12 RID: 2578 RVA: 0x0001E5C4 File Offset: 0x0001C7C4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 8238)
			{
				this.WmReflectVKeyToItem(ref m);
				return;
			}
			if (msg == 8239)
			{
				m.Result = NativeMethods.InvalidIntPtr;
				return;
			}
			if (m.Msg == CheckedListBox.LBC_GETCHECKSTATE)
			{
				int num = (int)((long)m.WParam);
				if (num < 0 || num >= this.Items.Count)
				{
					m.Result = (IntPtr)(-1);
					return;
				}
				m.Result = (IntPtr)(this.GetItemChecked(num) ? 1 : 0);
				return;
			}
			else
			{
				if (m.Msg != CheckedListBox.LBC_SETCHECKSTATE)
				{
					base.WndProc(ref m);
					return;
				}
				int num2 = (int)((long)m.WParam);
				int num3 = (int)((long)m.LParam);
				if (num2 < 0 || num2 >= this.Items.Count || (num3 != 1 && num3 != 0))
				{
					m.Result = IntPtr.Zero;
					return;
				}
				this.SetItemChecked(num2, num3 == 1);
				m.Result = (IntPtr)1;
				return;
			}
		}

		// Token: 0x040006BB RID: 1723
		private int idealCheckSize = 13;

		// Token: 0x040006BC RID: 1724
		private const int LB_CHECKED = 1;

		// Token: 0x040006BD RID: 1725
		private const int LB_UNCHECKED = 0;

		// Token: 0x040006BE RID: 1726
		private const int LB_ERROR = -1;

		// Token: 0x040006BF RID: 1727
		private const int BORDER_SIZE = 1;

		// Token: 0x040006C0 RID: 1728
		private bool killnextselect;

		// Token: 0x040006C1 RID: 1729
		private ItemCheckEventHandler onItemCheck;

		// Token: 0x040006C2 RID: 1730
		private bool checkOnClick;

		// Token: 0x040006C3 RID: 1731
		private bool flat = true;

		// Token: 0x040006C4 RID: 1732
		private int lastSelected = -1;

		// Token: 0x040006C5 RID: 1733
		private CheckedListBox.CheckedItemCollection checkedItemCollection;

		// Token: 0x040006C6 RID: 1734
		private CheckedListBox.CheckedIndexCollection checkedIndexCollection;

		// Token: 0x040006C7 RID: 1735
		private static int LBC_GETCHECKSTATE = SafeNativeMethods.RegisterWindowMessage("LBC_GETCHECKSTATE");

		// Token: 0x040006C8 RID: 1736
		private static int LBC_SETCHECKSTATE = SafeNativeMethods.RegisterWindowMessage("LBC_SETCHECKSTATE");

		/// <summary>Represents the collection of items in a <see cref="T:System.Windows.Forms.CheckedListBox" />.</summary>
		// Token: 0x02000561 RID: 1377
		public new class ObjectCollection : ListBox.ObjectCollection
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.CheckedListBox.ObjectCollection" /> class.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.CheckedListBox" /> that owns the collection. </param>
			// Token: 0x06005632 RID: 22066 RVA: 0x001699EC File Offset: 0x00167BEC
			public ObjectCollection(CheckedListBox owner) : base(owner)
			{
				this.owner = owner;
			}

			/// <summary>Adds an item to the list of items for a <see cref="T:System.Windows.Forms.CheckedListBox" />, specifying the object to add and whether it is checked.</summary>
			/// <param name="item">An object representing the item to add to the collection. </param>
			/// <param name="isChecked">
			///       <see langword="true" /> to check the item; otherwise, <see langword="false" />. </param>
			/// <returns>The index of the newly added item.</returns>
			// Token: 0x06005633 RID: 22067 RVA: 0x001699FC File Offset: 0x00167BFC
			public int Add(object item, bool isChecked)
			{
				return this.Add(item, isChecked ? CheckState.Checked : CheckState.Unchecked);
			}

			/// <summary>Adds an item to the list of items for a <see cref="T:System.Windows.Forms.CheckedListBox" />, specifying the object to add and the initial checked value.</summary>
			/// <param name="item">An object representing the item to add to the collection. </param>
			/// <param name="check">The initial <see cref="T:System.Windows.Forms.CheckState" /> for the checked portion of the item. </param>
			/// <returns>The index of the newly added item.</returns>
			/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The <paramref name="check" /> parameter is not one of the valid <see cref="T:System.Windows.Forms.CheckState" /> values. </exception>
			// Token: 0x06005634 RID: 22068 RVA: 0x00169A0C File Offset: 0x00167C0C
			public int Add(object item, CheckState check)
			{
				if (!ClientUtils.IsEnumValid(check, (int)check, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)check, typeof(CheckState));
				}
				int num = base.Add(item);
				this.owner.SetItemCheckState(num, check);
				return num;
			}

			// Token: 0x040037E4 RID: 14308
			private CheckedListBox owner;
		}

		/// <summary>Encapsulates the collection of indexes of checked items (including items in an indeterminate state) in a <see cref="T:System.Windows.Forms.CheckedListBox" />.</summary>
		// Token: 0x02000562 RID: 1378
		public class CheckedIndexCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x06005635 RID: 22069 RVA: 0x00169A55 File Offset: 0x00167C55
			internal CheckedIndexCollection(CheckedListBox owner)
			{
				this.owner = owner;
			}

			/// <summary>Gets the number of checked items.</summary>
			/// <returns>The number of indexes in the collection.</returns>
			// Token: 0x1700147C RID: 5244
			// (get) Token: 0x06005636 RID: 22070 RVA: 0x00169A64 File Offset: 0x00167C64
			public int Count
			{
				get
				{
					return this.owner.CheckedItems.Count;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the collection of controls. For a description of this member, see <see cref="P:System.Collections.ICollection.SyncRoot" />.</summary>
			/// <returns>The <see cref="T:System.Object" /> used to synchronize to the collection.</returns>
			// Token: 0x1700147D RID: 5245
			// (get) Token: 0x06005637 RID: 22071 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>Gets a value indicating whether access to the <see cref="T:System.Windows.Forms.CheckedListBox.CheckedIndexCollection" /> is synchronized (thread safe).</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x1700147E RID: 5246
			// (get) Token: 0x06005638 RID: 22072 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsFixedSize" />.</summary>
			/// <returns>
			///     <see langword="true" /> in all cases.</returns>
			// Token: 0x1700147F RID: 5247
			// (get) Token: 0x06005639 RID: 22073 RVA: 0x0000E214 File Offset: 0x0000C414
			bool IList.IsFixedSize
			{
				get
				{
					return true;
				}
			}

			/// <summary>Gets a value indicating whether the collection is read-only.</summary>
			/// <returns>
			///     <see langword="true" /> in all cases.</returns>
			// Token: 0x17001480 RID: 5248
			// (get) Token: 0x0600563A RID: 22074 RVA: 0x0000E214 File Offset: 0x0000C414
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			/// <summary>Gets the index of a checked item in the <see cref="T:System.Windows.Forms.CheckedListBox" /> control.</summary>
			/// <param name="index">An index into the checked indexes collection. This index specifies the index of the checked item you want to retrieve. </param>
			/// <returns>The index of the checked item. For more information, see the examples in the <see cref="T:System.Windows.Forms.CheckedListBox.CheckedIndexCollection" /> class overview.</returns>
			/// <exception cref="T:System.ArgumentException">The <paramref name="index" /> is less than zero.-or- The <paramref name="index" /> is not in the collection. </exception>
			// Token: 0x17001481 RID: 5249
			[Browsable(false)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public int this[int index]
			{
				get
				{
					object entryObject = this.InnerArray.GetEntryObject(index, CheckedListBox.CheckedItemCollection.AnyMask);
					return this.InnerArray.IndexOfIdentifier(entryObject, 0);
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.Item(System.Int32)" />.</summary>
			/// <param name="index">The zero-based index of the item to get.</param>
			/// <returns>The index value from the <see cref="T:System.Windows.Forms.CheckedListBox.ObjectCollection" /> that is stored at the specified location.</returns>
			// Token: 0x17001482 RID: 5250
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedIndexCollectionIsReadOnly"));
				}
			}

			/// <summary>Adds an item to the <see cref="T:System.Windows.Forms.CheckedListBox.CheckedIndexCollection" />. For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />.</summary>
			/// <param name="value">The object to be added to the <see cref="T:System.Windows.Forms.CheckedListBox.CheckedIndexCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x0600563E RID: 22078 RVA: 0x00169AB2 File Offset: 0x00167CB2
			int IList.Add(object value)
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedIndexCollectionIsReadOnly"));
			}

			/// <summary>Removes all items from the <see cref="T:System.Windows.Forms.CheckedListBox.CheckedIndexCollection" />. For a description of this member, see <see cref="M:System.Collections.IList.Clear" />.</summary>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x0600563F RID: 22079 RVA: 0x00169AB2 File Offset: 0x00167CB2
			void IList.Clear()
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedIndexCollectionIsReadOnly"));
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" />.</summary>
			/// <param name="index">The index at which value should be inserted.</param>
			/// <param name="value">The object to be added to the <see cref="T:System.Windows.Forms.CheckedListBox.CheckedIndexCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005640 RID: 22080 RVA: 0x00169AB2 File Offset: 0x00167CB2
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedIndexCollectionIsReadOnly"));
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Remove(System.Object)" />.</summary>
			/// <param name="value">The object to be removed from the <see cref="T:System.Windows.Forms.CheckedListBox.CheckedIndexCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005641 RID: 22081 RVA: 0x00169AB2 File Offset: 0x00167CB2
			void IList.Remove(object value)
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedIndexCollectionIsReadOnly"));
			}

			/// <summary>or a description of this member, see <see cref="M:System.Collections.IList.RemoveAt(System.Int32)" />.</summary>
			/// <param name="index">The zero-based index of the item to remove.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005642 RID: 22082 RVA: 0x00169AB2 File Offset: 0x00167CB2
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedIndexCollectionIsReadOnly"));
			}

			/// <summary>Determines whether the specified index is located in the collection.</summary>
			/// <param name="index">The index to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the specified index from the <see cref="T:System.Windows.Forms.CheckedListBox.ObjectCollection" /> is an item in this collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005643 RID: 22083 RVA: 0x00169AC3 File Offset: 0x00167CC3
			public bool Contains(int index)
			{
				return this.IndexOf(index) != -1;
			}

			/// <summary>Determines whether the specified index is located within the <see cref="T:System.Windows.Forms.CheckedListBox.CheckedIndexCollection" />. For a description of this member, see <see cref="M:System.Collections.IList.Contains(System.Object)" />.</summary>
			/// <param name="index">The index to locate in the collection.</param>
			/// <returns>
			///     <see langword="true" /> if the specified index from the <see cref="T:System.Windows.Forms.CheckedListBox.ObjectCollection" /> for the <see cref="T:System.Windows.Forms.CheckedListBox" /> is an item in this collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005644 RID: 22084 RVA: 0x00169AD2 File Offset: 0x00167CD2
			bool IList.Contains(object index)
			{
				return index is int && this.Contains((int)index);
			}

			/// <summary>Copies the entire collection into an existing array at a specified location within the array.</summary>
			/// <param name="dest">The destination array. </param>
			/// <param name="index">The zero-based relative index in <paramref name="dest" /> at which copying begins. </param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="array" /> is <see langword="null" />. </exception>
			/// <exception cref="T:System.RankException">
			///         <paramref name="array" /> is multidimensional. </exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than zero. </exception>
			/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Array" /> is greater than the available space from index to the end of the destination <see cref="T:System.Array" />. </exception>
			/// <exception cref="T:System.ArrayTypeMismatchException">The type of the source <see cref="T:System.Array" /> cannot be cast automatically to the type of the destination <see cref="T:System.Array" />. </exception>
			// Token: 0x06005645 RID: 22085 RVA: 0x00169AEC File Offset: 0x00167CEC
			public void CopyTo(Array dest, int index)
			{
				int count = this.owner.CheckedItems.Count;
				for (int i = 0; i < count; i++)
				{
					dest.SetValue(this[i], i + index);
				}
			}

			// Token: 0x17001483 RID: 5251
			// (get) Token: 0x06005646 RID: 22086 RVA: 0x00169B2B File Offset: 0x00167D2B
			private ListBox.ItemArray InnerArray
			{
				get
				{
					return this.owner.Items.InnerArray;
				}
			}

			/// <summary>Returns an enumerator that can be used to iterate through the <see cref="P:System.Windows.Forms.CheckedListBox.CheckedIndices" /> collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for navigating through the list.</returns>
			// Token: 0x06005647 RID: 22087 RVA: 0x00169B40 File Offset: 0x00167D40
			public IEnumerator GetEnumerator()
			{
				int[] array = new int[this.Count];
				this.CopyTo(array, 0);
				return array.GetEnumerator();
			}

			/// <summary>Returns an index into the collection of checked indexes.</summary>
			/// <param name="index">The index of the checked item. </param>
			/// <returns>The index that specifies the index of the checked item or -1 if the <paramref name="index" /> parameter is not in the checked indexes collection. For more information, see the examples in the <see cref="T:System.Windows.Forms.CheckedListBox.CheckedIndexCollection" /> class overview.</returns>
			// Token: 0x06005648 RID: 22088 RVA: 0x00169B68 File Offset: 0x00167D68
			public int IndexOf(int index)
			{
				if (index >= 0 && index < this.owner.Items.Count)
				{
					object entryObject = this.InnerArray.GetEntryObject(index, 0);
					return this.owner.CheckedItems.IndexOfIdentifier(entryObject);
				}
				return -1;
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.IndexOf(System.Object)" />.</summary>
			/// <param name="index">The zero-based index from the <see cref="T:System.Windows.Forms.CheckedListBox.ObjectCollection" /> to locate in this collection.</param>
			/// <returns>This member is an explicit interface member implementation. It can be used only when the <see cref="T:System.Windows.Forms.CheckedListBox.CheckedIndexCollection" /> instance is cast to an <see cref="T:System.Collections.IList" /> interface.</returns>
			// Token: 0x06005649 RID: 22089 RVA: 0x00169BAD File Offset: 0x00167DAD
			int IList.IndexOf(object index)
			{
				if (index is int)
				{
					return this.IndexOf((int)index);
				}
				return -1;
			}

			// Token: 0x040037E5 RID: 14309
			private CheckedListBox owner;
		}

		/// <summary>Encapsulates the collection of checked items, including items in an indeterminate state, in a <see cref="T:System.Windows.Forms.CheckedListBox" /> control.</summary>
		// Token: 0x02000563 RID: 1379
		public class CheckedItemCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x0600564A RID: 22090 RVA: 0x00169BC5 File Offset: 0x00167DC5
			internal CheckedItemCollection(CheckedListBox owner)
			{
				this.owner = owner;
			}

			/// <summary>Gets the number of items in the collection.</summary>
			/// <returns>The number of items in the collection.</returns>
			// Token: 0x17001484 RID: 5252
			// (get) Token: 0x0600564B RID: 22091 RVA: 0x00169BD4 File Offset: 0x00167DD4
			public int Count
			{
				get
				{
					return this.InnerArray.GetCount(CheckedListBox.CheckedItemCollection.AnyMask);
				}
			}

			// Token: 0x17001485 RID: 5253
			// (get) Token: 0x0600564C RID: 22092 RVA: 0x00169BE6 File Offset: 0x00167DE6
			private ListBox.ItemArray InnerArray
			{
				get
				{
					return this.owner.Items.InnerArray;
				}
			}

			/// <summary>Gets an object in the checked items collection.</summary>
			/// <param name="index">An index into the collection of checked items. This collection index corresponds to the index of the checked item. </param>
			/// <returns>The object at the specified index. For more information, see the examples in the <see cref="T:System.Windows.Forms.CheckedListBox.CheckedItemCollection" /> class overview.</returns>
			/// <exception cref="T:System.NotSupportedException">The object cannot be set.</exception>
			// Token: 0x17001486 RID: 5254
			[Browsable(false)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public object this[int index]
			{
				get
				{
					return this.InnerArray.GetItem(index, CheckedListBox.CheckedItemCollection.AnyMask);
				}
				set
				{
					throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedItemCollectionIsReadOnly"));
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.SyncRoot" />.</summary>
			/// <returns>The <see cref="T:System.Object" /> used to synchronize to the collection.</returns>
			// Token: 0x17001487 RID: 5255
			// (get) Token: 0x0600564F RID: 22095 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.IsSynchronized" />.</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x17001488 RID: 5256
			// (get) Token: 0x06005650 RID: 22096 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsFixedSize" />.</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x17001489 RID: 5257
			// (get) Token: 0x06005651 RID: 22097 RVA: 0x0000E214 File Offset: 0x0000C414
			bool IList.IsFixedSize
			{
				get
				{
					return true;
				}
			}

			/// <summary>Gets a value indicating if the collection is read-only.</summary>
			/// <returns>Always <see langword="true" />.</returns>
			// Token: 0x1700148A RID: 5258
			// (get) Token: 0x06005652 RID: 22098 RVA: 0x0000E214 File Offset: 0x0000C414
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			/// <summary>Determines whether the specified item is located in the collection.</summary>
			/// <param name="item">An object from the items collection. </param>
			/// <returns>
			///     <see langword="true" /> if item is in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005653 RID: 22099 RVA: 0x00169C1C File Offset: 0x00167E1C
			public bool Contains(object item)
			{
				return this.IndexOf(item) != -1;
			}

			/// <summary>Returns an index into the collection of checked items.</summary>
			/// <param name="item">The object whose index you want to retrieve. This object must belong to the checked items collection. </param>
			/// <returns>The index of the object in the checked item collection or -1 if the object is not in the collection. For more information, see the examples in the <see cref="T:System.Windows.Forms.CheckedListBox.CheckedItemCollection" /> class overview.</returns>
			// Token: 0x06005654 RID: 22100 RVA: 0x00169C2B File Offset: 0x00167E2B
			public int IndexOf(object item)
			{
				return this.InnerArray.IndexOf(item, CheckedListBox.CheckedItemCollection.AnyMask);
			}

			// Token: 0x06005655 RID: 22101 RVA: 0x00169C3E File Offset: 0x00167E3E
			internal int IndexOfIdentifier(object item)
			{
				return this.InnerArray.IndexOfIdentifier(item, CheckedListBox.CheckedItemCollection.AnyMask);
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />.</summary>
			/// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Collections.IList" />.</param>
			/// <returns>The zero-based index of the item to add.</returns>
			// Token: 0x06005656 RID: 22102 RVA: 0x00169C0B File Offset: 0x00167E0B
			int IList.Add(object value)
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedItemCollectionIsReadOnly"));
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Clear" />.</summary>
			// Token: 0x06005657 RID: 22103 RVA: 0x00169C0B File Offset: 0x00167E0B
			void IList.Clear()
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedItemCollectionIsReadOnly"));
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" />.</summary>
			/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
			/// <param name="value">The item to insert into the <see cref="T:System.Windows.Forms.CheckedListBox.CheckedItemCollection" />.</param>
			// Token: 0x06005658 RID: 22104 RVA: 0x00169C0B File Offset: 0x00167E0B
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedItemCollectionIsReadOnly"));
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Remove(System.Object)" />.</summary>
			/// <param name="value">The item to remove from the <see cref="T:System.Windows.Forms.CheckedListBox.CheckedItemCollection" />.</param>
			// Token: 0x06005659 RID: 22105 RVA: 0x00169C0B File Offset: 0x00167E0B
			void IList.Remove(object value)
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedItemCollectionIsReadOnly"));
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.RemoveAt(System.Int32)" />.</summary>
			/// <param name="index">The zero-based index of the item to remove.</param>
			// Token: 0x0600565A RID: 22106 RVA: 0x00169C0B File Offset: 0x00167E0B
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException(SR.GetString("CheckedListBoxCheckedItemCollectionIsReadOnly"));
			}

			/// <summary>Copies the entire collection into an existing array at a specified location within the array.</summary>
			/// <param name="dest">The destination array. </param>
			/// <param name="index">The zero-based relative index in <paramref name="dest" /> at which copying begins. </param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="array" /> is <see langword="null" />. </exception>
			/// <exception cref="T:System.RankException">
			///         <paramref name="array" /> is multidimensional. </exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than zero. </exception>
			/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Array" /> is greater than the available space from index to the end of the destination <see cref="T:System.Array" />. </exception>
			/// <exception cref="T:System.ArrayTypeMismatchException">The type of the source <see cref="T:System.Array" /> cannot be cast automatically to the type of the destination <see cref="T:System.Array" />. </exception>
			// Token: 0x0600565B RID: 22107 RVA: 0x00169C54 File Offset: 0x00167E54
			public void CopyTo(Array dest, int index)
			{
				int count = this.InnerArray.GetCount(CheckedListBox.CheckedItemCollection.AnyMask);
				for (int i = 0; i < count; i++)
				{
					dest.SetValue(this.InnerArray.GetItem(i, CheckedListBox.CheckedItemCollection.AnyMask), i + index);
				}
			}

			// Token: 0x0600565C RID: 22108 RVA: 0x00169C98 File Offset: 0x00167E98
			internal CheckState GetCheckedState(int index)
			{
				bool state = this.InnerArray.GetState(index, CheckedListBox.CheckedItemCollection.CheckedItemMask);
				bool state2 = this.InnerArray.GetState(index, CheckedListBox.CheckedItemCollection.IndeterminateItemMask);
				if (state2)
				{
					return CheckState.Indeterminate;
				}
				if (state)
				{
					return CheckState.Checked;
				}
				return CheckState.Unchecked;
			}

			/// <summary>Returns an enumerator that can be used to iterate through the <see cref="P:System.Windows.Forms.CheckedListBox.CheckedItems" /> collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for navigating through the list.</returns>
			// Token: 0x0600565D RID: 22109 RVA: 0x00169CD4 File Offset: 0x00167ED4
			public IEnumerator GetEnumerator()
			{
				return this.InnerArray.GetEnumerator(CheckedListBox.CheckedItemCollection.AnyMask, true);
			}

			// Token: 0x0600565E RID: 22110 RVA: 0x00169CE8 File Offset: 0x00167EE8
			internal void SetCheckedState(int index, CheckState value)
			{
				bool flag;
				bool flag2;
				if (value != CheckState.Checked)
				{
					if (value != CheckState.Indeterminate)
					{
						flag = false;
						flag2 = false;
					}
					else
					{
						flag = false;
						flag2 = true;
					}
				}
				else
				{
					flag = true;
					flag2 = false;
				}
				bool state = this.InnerArray.GetState(index, CheckedListBox.CheckedItemCollection.CheckedItemMask);
				bool state2 = this.InnerArray.GetState(index, CheckedListBox.CheckedItemCollection.IndeterminateItemMask);
				this.InnerArray.SetState(index, CheckedListBox.CheckedItemCollection.CheckedItemMask, flag);
				this.InnerArray.SetState(index, CheckedListBox.CheckedItemCollection.IndeterminateItemMask, flag2);
				if (state != flag || state2 != flag2)
				{
					this.owner.AccessibilityNotifyClients(AccessibleEvents.StateChange, index);
				}
			}

			// Token: 0x040037E6 RID: 14310
			internal static int CheckedItemMask = ListBox.ItemArray.CreateMask();

			// Token: 0x040037E7 RID: 14311
			internal static int IndeterminateItemMask = ListBox.ItemArray.CreateMask();

			// Token: 0x040037E8 RID: 14312
			internal static int AnyMask = CheckedListBox.CheckedItemCollection.CheckedItemMask | CheckedListBox.CheckedItemCollection.IndeterminateItemMask;

			// Token: 0x040037E9 RID: 14313
			private CheckedListBox owner;
		}

		// Token: 0x02000564 RID: 1380
		[ComVisible(true)]
		internal class CheckedListBoxAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06005660 RID: 22112 RVA: 0x00093572 File Offset: 0x00091772
			public CheckedListBoxAccessibleObject(CheckedListBox owner) : base(owner)
			{
			}

			// Token: 0x1700148B RID: 5259
			// (get) Token: 0x06005661 RID: 22113 RVA: 0x00169D96 File Offset: 0x00167F96
			private CheckedListBox CheckedListBox
			{
				get
				{
					return (CheckedListBox)base.Owner;
				}
			}

			// Token: 0x06005662 RID: 22114 RVA: 0x00169DA3 File Offset: 0x00167FA3
			public override AccessibleObject GetChild(int index)
			{
				if (index >= 0 && index < this.CheckedListBox.Items.Count)
				{
					return new CheckedListBox.CheckedListBoxItemAccessibleObject(this.CheckedListBox.GetItemText(this.CheckedListBox.Items[index]), index, this);
				}
				return null;
			}

			// Token: 0x06005663 RID: 22115 RVA: 0x00169DE1 File Offset: 0x00167FE1
			public override int GetChildCount()
			{
				return this.CheckedListBox.Items.Count;
			}

			// Token: 0x06005664 RID: 22116 RVA: 0x00169DF4 File Offset: 0x00167FF4
			public override AccessibleObject GetFocused()
			{
				int focusedIndex = this.CheckedListBox.FocusedIndex;
				if (focusedIndex >= 0)
				{
					return this.GetChild(focusedIndex);
				}
				return null;
			}

			// Token: 0x06005665 RID: 22117 RVA: 0x00169E1C File Offset: 0x0016801C
			public override AccessibleObject GetSelected()
			{
				int selectedIndex = this.CheckedListBox.SelectedIndex;
				if (selectedIndex >= 0)
				{
					return this.GetChild(selectedIndex);
				}
				return null;
			}

			// Token: 0x06005666 RID: 22118 RVA: 0x00169E44 File Offset: 0x00168044
			public override AccessibleObject HitTest(int x, int y)
			{
				int childCount = this.GetChildCount();
				for (int i = 0; i < childCount; i++)
				{
					AccessibleObject child = this.GetChild(i);
					if (child.Bounds.Contains(x, y))
					{
						return child;
					}
				}
				if (this.Bounds.Contains(x, y))
				{
					return this;
				}
				return null;
			}

			// Token: 0x06005667 RID: 22119 RVA: 0x00169E95 File Offset: 0x00168095
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation direction)
			{
				if (this.GetChildCount() > 0)
				{
					if (direction == AccessibleNavigation.FirstChild)
					{
						return this.GetChild(0);
					}
					if (direction == AccessibleNavigation.LastChild)
					{
						return this.GetChild(this.GetChildCount() - 1);
					}
				}
				return base.Navigate(direction);
			}
		}

		// Token: 0x02000565 RID: 1381
		[ComVisible(true)]
		internal class CheckedListBoxItemAccessibleObject : AccessibleObject
		{
			// Token: 0x06005668 RID: 22120 RVA: 0x00169EC6 File Offset: 0x001680C6
			public CheckedListBoxItemAccessibleObject(string name, int index, CheckedListBox.CheckedListBoxAccessibleObject parent)
			{
				this.name = name;
				this.parent = parent;
				this.index = index;
			}

			// Token: 0x1700148C RID: 5260
			// (get) Token: 0x06005669 RID: 22121 RVA: 0x00169EE4 File Offset: 0x001680E4
			public override Rectangle Bounds
			{
				get
				{
					Rectangle itemRectangle = this.ParentCheckedListBox.GetItemRectangle(this.index);
					NativeMethods.POINT point = new NativeMethods.POINT(itemRectangle.X, itemRectangle.Y);
					UnsafeNativeMethods.ClientToScreen(new HandleRef(this.ParentCheckedListBox, this.ParentCheckedListBox.Handle), point);
					return new Rectangle(point.x, point.y, itemRectangle.Width, itemRectangle.Height);
				}
			}

			// Token: 0x1700148D RID: 5261
			// (get) Token: 0x0600566A RID: 22122 RVA: 0x00169F53 File Offset: 0x00168153
			public override string DefaultAction
			{
				get
				{
					if (this.ParentCheckedListBox.GetItemChecked(this.index))
					{
						return SR.GetString("AccessibleActionUncheck");
					}
					return SR.GetString("AccessibleActionCheck");
				}
			}

			// Token: 0x1700148E RID: 5262
			// (get) Token: 0x0600566B RID: 22123 RVA: 0x00169F7D File Offset: 0x0016817D
			private CheckedListBox ParentCheckedListBox
			{
				get
				{
					return (CheckedListBox)this.parent.Owner;
				}
			}

			// Token: 0x1700148F RID: 5263
			// (get) Token: 0x0600566C RID: 22124 RVA: 0x00169F8F File Offset: 0x0016818F
			// (set) Token: 0x0600566D RID: 22125 RVA: 0x00169F97 File Offset: 0x00168197
			public override string Name
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

			// Token: 0x17001490 RID: 5264
			// (get) Token: 0x0600566E RID: 22126 RVA: 0x00169FA0 File Offset: 0x001681A0
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.parent;
				}
			}

			// Token: 0x17001491 RID: 5265
			// (get) Token: 0x0600566F RID: 22127 RVA: 0x00169FA8 File Offset: 0x001681A8
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.CheckButton;
				}
			}

			// Token: 0x17001492 RID: 5266
			// (get) Token: 0x06005670 RID: 22128 RVA: 0x00169FAC File Offset: 0x001681AC
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.Focusable | AccessibleStates.Selectable;
					switch (this.ParentCheckedListBox.GetItemCheckState(this.index))
					{
					case CheckState.Checked:
						accessibleStates |= AccessibleStates.Checked;
						break;
					case CheckState.Indeterminate:
						accessibleStates |= AccessibleStates.Mixed;
						break;
					}
					if (this.ParentCheckedListBox.SelectedIndex == this.index)
					{
						accessibleStates |= (AccessibleStates.Selected | AccessibleStates.Focused);
					}
					if (AccessibilityImprovements.Level3 && this.ParentCheckedListBox.Focused && this.ParentCheckedListBox.SelectedIndex == -1)
					{
						accessibleStates |= AccessibleStates.Focused;
					}
					return accessibleStates;
				}
			}

			// Token: 0x17001493 RID: 5267
			// (get) Token: 0x06005671 RID: 22129 RVA: 0x0016A030 File Offset: 0x00168230
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.ParentCheckedListBox.GetItemChecked(this.index).ToString();
				}
			}

			// Token: 0x06005672 RID: 22130 RVA: 0x0016A056 File Offset: 0x00168256
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				this.ParentCheckedListBox.SetItemChecked(this.index, !this.ParentCheckedListBox.GetItemChecked(this.index));
			}

			// Token: 0x06005673 RID: 22131 RVA: 0x0016A080 File Offset: 0x00168280
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation direction)
			{
				if ((direction == AccessibleNavigation.Down || direction == AccessibleNavigation.Next) && this.index < this.parent.GetChildCount() - 1)
				{
					return this.parent.GetChild(this.index + 1);
				}
				if ((direction == AccessibleNavigation.Up || direction == AccessibleNavigation.Previous) && this.index > 0)
				{
					return this.parent.GetChild(this.index - 1);
				}
				return base.Navigate(direction);
			}

			// Token: 0x06005674 RID: 22132 RVA: 0x0016A0EC File Offset: 0x001682EC
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				try
				{
					this.ParentCheckedListBox.AccessibilityObject.GetSystemIAccessibleInternal().accSelect((int)flags, this.index + 1);
				}
				catch (ArgumentException)
				{
				}
			}

			// Token: 0x040037EA RID: 14314
			private string name;

			// Token: 0x040037EB RID: 14315
			private int index;

			// Token: 0x040037EC RID: 14316
			private CheckedListBox.CheckedListBoxAccessibleObject parent;
		}
	}
}
