using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Provides a menu system for a form.</summary>
	// Token: 0x020002E6 RID: 742
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[SRDescription("DescriptionMenuStrip")]
	public class MenuStrip : ToolStrip
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MenuStrip" /> class. </summary>
		// Token: 0x06002CC6 RID: 11462 RVA: 0x000D097B File Offset: 0x000CEB7B
		public MenuStrip()
		{
			this.CanOverflow = false;
			this.GripStyle = ToolStripGripStyle.Hidden;
			this.Stretch = true;
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06002CC7 RID: 11463 RVA: 0x000D0998 File Offset: 0x000CEB98
		// (set) Token: 0x06002CC8 RID: 11464 RVA: 0x000D09A0 File Offset: 0x000CEBA0
		internal override bool KeyboardActive
		{
			get
			{
				return base.KeyboardActive;
			}
			set
			{
				if (base.KeyboardActive != value)
				{
					base.KeyboardActive = value;
					if (value)
					{
						this.OnMenuActivate(EventArgs.Empty);
						return;
					}
					this.OnMenuDeactivate(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.MenuStrip" /> supports overflow functionality. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.MenuStrip" /> supports overflow functionality; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06002CC9 RID: 11465 RVA: 0x000D09CC File Offset: 0x000CEBCC
		// (set) Token: 0x06002CCA RID: 11466 RVA: 0x000D09D4 File Offset: 0x000CEBD4
		[DefaultValue(false)]
		[SRDescription("ToolStripCanOverflowDescr")]
		[SRCategory("CatLayout")]
		[Browsable(false)]
		public new bool CanOverflow
		{
			get
			{
				return base.CanOverflow;
			}
			set
			{
				base.CanOverflow = value;
			}
		}

		/// <summary>Gets a value indicating whether ToolTips are shown for the <see cref="T:System.Windows.Forms.MenuStrip" /> by default.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06002CCB RID: 11467 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected override bool DefaultShowItemToolTips
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets the default spacing, in pixels, between the sizing grip and the edges of the <see cref="T:System.Windows.Forms.MenuStrip" />.</summary>
		/// <returns>
		///     <see cref="T:System.Windows.Forms.Padding" /> values representing the spacing, in pixels.</returns>
		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06002CCC RID: 11468 RVA: 0x000D09DD File Offset: 0x000CEBDD
		protected override Padding DefaultGripMargin
		{
			get
			{
				if (!DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements)
				{
					return new Padding(2, 2, 0, 2);
				}
				return DpiHelper.LogicalToDeviceUnits(new Padding(2, 2, 0, 2), base.DeviceDpi);
			}
		}

		/// <summary>Gets the horizontal and vertical dimensions, in pixels, of the <see cref="T:System.Windows.Forms.MenuStrip" /> when it is first created.</summary>
		/// <returns>A <see cref="M:System.Drawing.Point.#ctor(System.Drawing.Size)" /> value representing the <see cref="T:System.Windows.Forms.MenuStrip" /> horizontal and vertical dimensions, in pixels. The default is 200 x 21 pixels.</returns>
		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06002CCD RID: 11469 RVA: 0x000D0A04 File Offset: 0x000CEC04
		protected override Size DefaultSize
		{
			get
			{
				if (!DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements)
				{
					return new Size(200, 24);
				}
				return DpiHelper.LogicalToDeviceUnits(new Size(200, 24), base.DeviceDpi);
			}
		}

		/// <summary>Gets the spacing, in pixels, between the left, right, top, and bottom edges of the <see cref="T:System.Windows.Forms.MenuStrip" /> from the edges of the form.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> that represents the spacing. The default is {Left=6, Top=2, Right=0, Bottom=2}.</returns>
		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06002CCE RID: 11470 RVA: 0x000D0A34 File Offset: 0x000CEC34
		protected override Padding DefaultPadding
		{
			get
			{
				if (this.GripStyle == ToolStripGripStyle.Visible)
				{
					if (!DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements)
					{
						return new Padding(3, 2, 0, 2);
					}
					return DpiHelper.LogicalToDeviceUnits(new Padding(3, 2, 0, 2), base.DeviceDpi);
				}
				else
				{
					if (!DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements)
					{
						return new Padding(6, 2, 0, 2);
					}
					return DpiHelper.LogicalToDeviceUnits(new Padding(6, 2, 0, 2), base.DeviceDpi);
				}
			}
		}

		/// <summary>Gets or sets the visibility of the grip used to reposition the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripGripStyle" /> values. The default is <see cref="F:System.Windows.Forms.ToolStripGripStyle.Hidden" />.</returns>
		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06002CCF RID: 11471 RVA: 0x000D0A95 File Offset: 0x000CEC95
		// (set) Token: 0x06002CD0 RID: 11472 RVA: 0x000D0A9D File Offset: 0x000CEC9D
		[DefaultValue(ToolStripGripStyle.Hidden)]
		public new ToolStripGripStyle GripStyle
		{
			get
			{
				return base.GripStyle;
			}
			set
			{
				base.GripStyle = value;
			}
		}

		/// <summary>Occurs when the user accesses the menu with the keyboard or mouse. </summary>
		// Token: 0x14000213 RID: 531
		// (add) Token: 0x06002CD1 RID: 11473 RVA: 0x000D0AA6 File Offset: 0x000CECA6
		// (remove) Token: 0x06002CD2 RID: 11474 RVA: 0x000D0AB9 File Offset: 0x000CECB9
		[SRCategory("CatBehavior")]
		[SRDescription("MenuStripMenuActivateDescr")]
		public event EventHandler MenuActivate
		{
			add
			{
				base.Events.AddHandler(MenuStrip.EventMenuActivate, value);
			}
			remove
			{
				base.Events.RemoveHandler(MenuStrip.EventMenuActivate, value);
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.MenuStrip" /> is deactivated.</summary>
		// Token: 0x14000214 RID: 532
		// (add) Token: 0x06002CD3 RID: 11475 RVA: 0x000D0ACC File Offset: 0x000CECCC
		// (remove) Token: 0x06002CD4 RID: 11476 RVA: 0x000D0ADF File Offset: 0x000CECDF
		[SRCategory("CatBehavior")]
		[SRDescription("MenuStripMenuDeactivateDescr")]
		public event EventHandler MenuDeactivate
		{
			add
			{
				base.Events.AddHandler(MenuStrip.EventMenuDeactivate, value);
			}
			remove
			{
				base.Events.RemoveHandler(MenuStrip.EventMenuDeactivate, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether ToolTips are shown for the <see cref="T:System.Windows.Forms.MenuStrip" />. </summary>
		/// <returns>
		///     <see langword="true" /> if ToolTips are shown for the <see cref="T:System.Windows.Forms.MenuStrip" />; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06002CD5 RID: 11477 RVA: 0x000D0AF2 File Offset: 0x000CECF2
		// (set) Token: 0x06002CD6 RID: 11478 RVA: 0x000D0AFA File Offset: 0x000CECFA
		[DefaultValue(false)]
		[SRDescription("ToolStripShowItemToolTipsDescr")]
		[SRCategory("CatBehavior")]
		public new bool ShowItemToolTips
		{
			get
			{
				return base.ShowItemToolTips;
			}
			set
			{
				base.ShowItemToolTips = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.MenuStrip" /> stretches from end to end in its container. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.MenuStrip" /> stretches from end to end in its container; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06002CD7 RID: 11479 RVA: 0x000D0B03 File Offset: 0x000CED03
		// (set) Token: 0x06002CD8 RID: 11480 RVA: 0x000D0B0B File Offset: 0x000CED0B
		[DefaultValue(true)]
		[SRCategory("CatLayout")]
		[SRDescription("ToolStripStretchDescr")]
		public new bool Stretch
		{
			get
			{
				return base.Stretch;
			}
			set
			{
				base.Stretch = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> that is used to display a list of Multiple-document interface (MDI) child forms.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> that represents the menu item displaying a list of MDI child forms that are open in the application.</returns>
		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06002CD9 RID: 11481 RVA: 0x000D0B14 File Offset: 0x000CED14
		// (set) Token: 0x06002CDA RID: 11482 RVA: 0x000D0B1C File Offset: 0x000CED1C
		[DefaultValue(null)]
		[MergableProperty(false)]
		[SRDescription("MenuStripMdiWindowListItem")]
		[SRCategory("CatBehavior")]
		[TypeConverter(typeof(MdiWindowListItemConverter))]
		public ToolStripMenuItem MdiWindowListItem
		{
			get
			{
				return this.mdiWindowListItem;
			}
			set
			{
				this.mdiWindowListItem = value;
			}
		}

		/// <summary>Creates a new accessibility object for the control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the control.</returns>
		// Token: 0x06002CDB RID: 11483 RVA: 0x000D0B25 File Offset: 0x000CED25
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new MenuStrip.MenuStripAccessibleObject(this);
		}

		/// <summary>Creates a <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> with the specified text, image, and event handler on a new <see cref="T:System.Windows.Forms.MenuStrip" />.</summary>
		/// <param name="text">The text to use for the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />. If the <paramref name="text" /> parameter is a hyphen (-), this method creates a <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.</param>
		/// <param name="onClick">An event handler that raises the <see cref="E:System.Windows.Forms.Control.Click" /> event when the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is clicked.</param>
		/// <returns>A <see cref="M:System.Windows.Forms.ToolStripMenuItem.#ctor(System.String,System.Drawing.Image,System.EventHandler)" />, or a <see cref="T:System.Windows.Forms.ToolStripSeparator" /> if the <paramref name="text" /> parameter is a hyphen (-).</returns>
		// Token: 0x06002CDC RID: 11484 RVA: 0x000D0B2D File Offset: 0x000CED2D
		protected internal override ToolStripItem CreateDefaultItem(string text, Image image, EventHandler onClick)
		{
			if (text == "-")
			{
				return new ToolStripSeparator();
			}
			return new ToolStripMenuItem(text, image, onClick);
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x000D0B4C File Offset: 0x000CED4C
		internal override ToolStripItem GetNextItem(ToolStripItem start, ArrowDirection direction, bool rtlAware)
		{
			ToolStripItem nextItem = base.GetNextItem(start, direction, rtlAware);
			if (nextItem is MdiControlStrip.SystemMenuItem && AccessibilityImprovements.Level2)
			{
				nextItem = base.GetNextItem(nextItem, direction, rtlAware);
			}
			return nextItem;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.MenuStrip.MenuActivate" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002CDE RID: 11486 RVA: 0x000D0B80 File Offset: 0x000CED80
		protected virtual void OnMenuActivate(EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				base.AccessibilityNotifyClients(AccessibleEvents.SystemMenuStart, -1);
			}
			EventHandler eventHandler = (EventHandler)base.Events[MenuStrip.EventMenuActivate];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.MenuStrip.MenuDeactivate" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002CDF RID: 11487 RVA: 0x000D0BC0 File Offset: 0x000CEDC0
		protected virtual void OnMenuDeactivate(EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				base.AccessibilityNotifyClients(AccessibleEvents.SystemMenuEnd, -1);
			}
			EventHandler eventHandler = (EventHandler)base.Events[MenuStrip.EventMenuDeactivate];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x000D0C00 File Offset: 0x000CEE00
		internal bool OnMenuKey()
		{
			if (!this.Focused && !base.ContainsFocus)
			{
				ToolStripManager.ModalMenuFilter.SetActiveToolStrip(this, true);
				if (this.DisplayedItems.Count > 0)
				{
					if (this.DisplayedItems[0] is MdiControlStrip.SystemMenuItem)
					{
						base.SelectNextToolStripItem(this.DisplayedItems[0], true);
					}
					else
					{
						base.SelectNextToolStripItem(null, this.RightToLeft == RightToLeft.No);
					}
				}
				return true;
			}
			return false;
		}

		/// <summary>Processes a command key.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the window message to process.</param>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process.</param>
		/// <returns>
		///     <see langword="true" /> if the character was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002CE1 RID: 11489 RVA: 0x000D0C70 File Offset: 0x000CEE70
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			if (ToolStripManager.ModalMenuFilter.InMenuMode && keyData == Keys.Space && (this.Focused || !base.ContainsFocus))
			{
				base.NotifySelectionChange(null);
				ToolStripManager.ModalMenuFilter.ExitMenuMode();
				UnsafeNativeMethods.PostMessage(WindowsFormsUtils.GetRootHWnd(this), 274, 61696, 32);
				return true;
			}
			return base.ProcessCmdKey(ref m, keyData);
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06002CE2 RID: 11490 RVA: 0x000D0CC8 File Offset: 0x000CEEC8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 33 && base.ActiveDropDowns.Count == 0)
			{
				Point point = base.PointToClient(WindowsFormsUtils.LastCursorPoint);
				ToolStripItem itemAt = base.GetItemAt(point);
				if (itemAt != null && !(itemAt is ToolStripControlHost))
				{
					this.KeyboardActive = true;
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x0400131E RID: 4894
		private ToolStripMenuItem mdiWindowListItem;

		// Token: 0x0400131F RID: 4895
		private static readonly object EventMenuActivate = new object();

		// Token: 0x04001320 RID: 4896
		private static readonly object EventMenuDeactivate = new object();

		// Token: 0x02000621 RID: 1569
		[ComVisible(true)]
		internal class MenuStripAccessibleObject : ToolStrip.ToolStripAccessibleObject
		{
			// Token: 0x06005E26 RID: 24102 RVA: 0x001868E4 File Offset: 0x00184AE4
			public MenuStripAccessibleObject(MenuStrip owner) : base(owner)
			{
			}

			// Token: 0x17001697 RID: 5783
			// (get) Token: 0x06005E27 RID: 24103 RVA: 0x001868F0 File Offset: 0x00184AF0
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.MenuBar;
				}
			}

			// Token: 0x06005E28 RID: 24104 RVA: 0x00186910 File Offset: 0x00184B10
			internal override object GetPropertyValue(int propertyID)
			{
				if (AccessibilityImprovements.Level3 && propertyID == 30003)
				{
					return 50010;
				}
				return base.GetPropertyValue(propertyID);
			}
		}
	}
}
