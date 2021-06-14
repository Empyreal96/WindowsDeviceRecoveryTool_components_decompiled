using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Automation;
using System.Windows.Forms.ComponentModel.Com2Interop;
using System.Windows.Forms.Design;
using System.Windows.Forms.PropertyGridInternal;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	/// <summary>Provides a user interface for browsing the properties of an object.</summary>
	// Token: 0x0200031A RID: 794
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.PropertyGridDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionPropertyGrid")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class PropertyGrid : ContainerControl, IComPropertyBrowser, UnsafeNativeMethods.IPropertyNotifySink
	{
		// Token: 0x0600309E RID: 12446 RVA: 0x000E3387 File Offset: 0x000E1587
		private bool GetFlag(ushort flag)
		{
			return (this.flags & flag) > 0;
		}

		// Token: 0x0600309F RID: 12447 RVA: 0x000E3394 File Offset: 0x000E1594
		private void SetFlag(ushort flag, bool value)
		{
			if (value)
			{
				this.flags |= flag;
				return;
			}
			this.flags &= ~flag;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.PropertyGrid" /> class.</summary>
		// Token: 0x060030A0 RID: 12448 RVA: 0x000E33BC File Offset: 0x000E15BC
		public PropertyGrid()
		{
			this.onComponentAdd = new ComponentEventHandler(this.OnComponentAdd);
			this.onComponentRemove = new ComponentEventHandler(this.OnComponentRemove);
			this.onComponentChanged = new ComponentChangedEventHandler(this.OnComponentChanged);
			base.SuspendLayout();
			base.AutoScaleMode = AutoScaleMode.None;
			if (DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				this.RescaleConstants();
			}
			else if (!PropertyGrid.isScalingInitialized)
			{
				if (DpiHelper.IsScalingRequired)
				{
					PropertyGrid.normalButtonSize = base.LogicalToDeviceUnits(PropertyGrid.DEFAULT_NORMAL_BUTTON_SIZE);
					PropertyGrid.largeButtonSize = base.LogicalToDeviceUnits(PropertyGrid.DEFAULT_LARGE_BUTTON_SIZE);
				}
				PropertyGrid.isScalingInitialized = true;
			}
			try
			{
				this.gridView = this.CreateGridView(null);
				this.gridView.TabStop = true;
				this.gridView.MouseMove += this.OnChildMouseMove;
				this.gridView.MouseDown += this.OnChildMouseDown;
				this.gridView.TabIndex = 2;
				this.separator1 = this.CreateSeparatorButton();
				this.separator2 = this.CreateSeparatorButton();
				this.toolStrip = (AccessibilityImprovements.Level3 ? new PropertyGridToolStrip(this) : new ToolStrip());
				this.toolStrip.SuspendLayout();
				this.toolStrip.ShowItemToolTips = true;
				this.toolStrip.AccessibleName = (AccessibilityImprovements.Level4 ? "Property Grid" : SR.GetString("PropertyGridToolbarAccessibleName"));
				this.toolStrip.AccessibleRole = AccessibleRole.ToolBar;
				this.toolStrip.TabStop = true;
				this.toolStrip.AllowMerge = false;
				this.toolStrip.Text = "PropertyGridToolBar";
				this.toolStrip.Dock = DockStyle.None;
				this.toolStrip.AutoSize = false;
				this.toolStrip.TabIndex = 1;
				this.toolStrip.ImageScalingSize = PropertyGrid.normalButtonSize;
				this.toolStrip.CanOverflow = false;
				this.toolStrip.GripStyle = ToolStripGripStyle.Hidden;
				Padding padding = this.toolStrip.Padding;
				padding.Left = 2;
				this.toolStrip.Padding = padding;
				this.SetToolStripRenderer();
				this.AddRefTab(this.DefaultTabType, null, PropertyTabScope.Static, true);
				this.doccomment = new DocComment(this);
				this.doccomment.SuspendLayout();
				this.doccomment.TabStop = false;
				this.doccomment.Dock = DockStyle.None;
				this.doccomment.BackColor = SystemColors.Control;
				this.doccomment.ForeColor = SystemColors.ControlText;
				this.doccomment.MouseMove += this.OnChildMouseMove;
				this.doccomment.MouseDown += this.OnChildMouseDown;
				if (AccessibilityImprovements.Level4)
				{
					this.doccomment.AccessibleName = "Description";
				}
				this.hotcommands = new HotCommands(this);
				this.hotcommands.SuspendLayout();
				this.hotcommands.TabIndex = 3;
				this.hotcommands.Dock = DockStyle.None;
				this.SetHotCommandColors(false);
				this.hotcommands.Visible = false;
				this.hotcommands.MouseMove += this.OnChildMouseMove;
				this.hotcommands.MouseDown += this.OnChildMouseDown;
				if (AccessibilityImprovements.Level4)
				{
					this.hotcommands.AccessibleName = "Hot commands";
				}
				this.Controls.AddRange(new Control[]
				{
					this.doccomment,
					this.hotcommands,
					this.gridView,
					this.toolStrip
				});
				base.SetActiveControlInternal(this.gridView);
				this.toolStrip.ResumeLayout(false);
				this.SetupToolbar();
				this.PropertySort = PropertySort.CategorizedAlphabetical;
				this.Text = "PropertyGrid";
				this.SetSelectState(0);
			}
			catch (Exception ex)
			{
			}
			finally
			{
				if (this.doccomment != null)
				{
					this.doccomment.ResumeLayout(false);
				}
				if (this.hotcommands != null)
				{
					this.hotcommands.ResumeLayout(false);
				}
				base.ResumeLayout(true);
			}
		}

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x060030A1 RID: 12449 RVA: 0x000E3878 File Offset: 0x000E1A78
		// (set) Token: 0x060030A2 RID: 12450 RVA: 0x000E38A4 File Offset: 0x000E1AA4
		internal IDesignerHost ActiveDesigner
		{
			get
			{
				if (this.designerHost == null)
				{
					this.designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				}
				return this.designerHost;
			}
			set
			{
				if (value != this.designerHost)
				{
					this.SetFlag(32, true);
					if (this.designerHost != null)
					{
						IComponentChangeService componentChangeService = (IComponentChangeService)this.designerHost.GetService(typeof(IComponentChangeService));
						if (componentChangeService != null)
						{
							componentChangeService.ComponentAdded -= this.onComponentAdd;
							componentChangeService.ComponentRemoved -= this.onComponentRemove;
							componentChangeService.ComponentChanged -= this.onComponentChanged;
						}
						IPropertyValueUIService propertyValueUIService = (IPropertyValueUIService)this.designerHost.GetService(typeof(IPropertyValueUIService));
						if (propertyValueUIService != null)
						{
							propertyValueUIService.PropertyUIValueItemsChanged -= this.OnNotifyPropertyValueUIItemsChanged;
						}
						this.designerHost.TransactionOpened -= this.OnTransactionOpened;
						this.designerHost.TransactionClosed -= this.OnTransactionClosed;
						this.SetFlag(16, false);
						this.RemoveTabs(PropertyTabScope.Document, true);
						this.designerHost = null;
					}
					if (value != null)
					{
						IComponentChangeService componentChangeService2 = (IComponentChangeService)value.GetService(typeof(IComponentChangeService));
						if (componentChangeService2 != null)
						{
							componentChangeService2.ComponentAdded += this.onComponentAdd;
							componentChangeService2.ComponentRemoved += this.onComponentRemove;
							componentChangeService2.ComponentChanged += this.onComponentChanged;
						}
						value.TransactionOpened += this.OnTransactionOpened;
						value.TransactionClosed += this.OnTransactionClosed;
						this.SetFlag(16, false);
						IPropertyValueUIService propertyValueUIService2 = (IPropertyValueUIService)value.GetService(typeof(IPropertyValueUIService));
						if (propertyValueUIService2 != null)
						{
							propertyValueUIService2.PropertyUIValueItemsChanged += this.OnNotifyPropertyValueUIItemsChanged;
						}
					}
					this.designerHost = value;
					if (this.peMain != null)
					{
						this.peMain.DesignerHost = value;
					}
					this.RefreshTabs(PropertyTabScope.Document);
				}
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x060030A3 RID: 12451 RVA: 0x000A87BB File Offset: 0x000A69BB
		// (set) Token: 0x060030A4 RID: 12452 RVA: 0x000E3A46 File Offset: 0x000E1C46
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				base.AutoScroll = value;
			}
		}

		/// <summary>Gets or sets the background color for the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor" /> property.</returns>
		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x060030A5 RID: 12453 RVA: 0x00011FB1 File Offset: 0x000101B1
		// (set) Token: 0x060030A6 RID: 12454 RVA: 0x000E3A4F File Offset: 0x000E1C4F
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				this.toolStrip.BackColor = value;
				this.toolStrip.Invalidate(true);
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The background image of the property grid.</returns>
		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x060030A7 RID: 12455 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x060030A8 RID: 12456 RVA: 0x00011FCA File Offset: 0x000101CA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PropertyGrid.BackgroundImage" /> property changes.</summary>
		// Token: 0x14000255 RID: 597
		// (add) Token: 0x060030A9 RID: 12457 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x060030AA RID: 12458 RVA: 0x0001FD8A File Offset: 0x0001DF8A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				base.BackgroundImageChanged += value;
			}
			remove
			{
				base.BackgroundImageChanged -= value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The background image layout of the property grid.</returns>
		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x060030AB RID: 12459 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x060030AC RID: 12460 RVA: 0x00011FDB File Offset: 0x000101DB
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PropertyGrid.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x14000256 RID: 598
		// (add) Token: 0x060030AD RID: 12461 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x060030AE RID: 12462 RVA: 0x0001FD9C File Offset: 0x0001DF9C
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

		/// <summary>Gets or sets the browsable attributes associated with the object that the property grid is attached to.</summary>
		/// <returns>The collection of browsable attributes associated with the object.</returns>
		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x060030B0 RID: 12464 RVA: 0x000E3AF4 File Offset: 0x000E1CF4
		// (set) Token: 0x060030AF RID: 12463 RVA: 0x000E3A70 File Offset: 0x000E1C70
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public AttributeCollection BrowsableAttributes
		{
			get
			{
				if (this.browsableAttributes == null)
				{
					this.browsableAttributes = new AttributeCollection(new Attribute[]
					{
						new BrowsableAttribute(true)
					});
				}
				return this.browsableAttributes;
			}
			set
			{
				if (value == null || value == AttributeCollection.Empty)
				{
					this.browsableAttributes = new AttributeCollection(new Attribute[]
					{
						BrowsableAttribute.Yes
					});
				}
				else
				{
					Attribute[] array = new Attribute[value.Count];
					value.CopyTo(array, 0);
					this.browsableAttributes = new AttributeCollection(array);
				}
				if (this.currentObjects != null && this.currentObjects.Length != 0 && this.peMain != null)
				{
					this.peMain.BrowsableAttributes = this.BrowsableAttributes;
					this.Refresh(true);
				}
			}
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x060030B1 RID: 12465 RVA: 0x000E3B1E File Offset: 0x000E1D1E
		private bool CanCopy
		{
			get
			{
				return this.gridView.CanCopy;
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x060030B2 RID: 12466 RVA: 0x000E3B2B File Offset: 0x000E1D2B
		private bool CanCut
		{
			get
			{
				return this.gridView.CanCut;
			}
		}

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x060030B3 RID: 12467 RVA: 0x000E3B38 File Offset: 0x000E1D38
		private bool CanPaste
		{
			get
			{
				return this.gridView.CanPaste;
			}
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x060030B4 RID: 12468 RVA: 0x000E3B45 File Offset: 0x000E1D45
		private bool CanUndo
		{
			get
			{
				return this.gridView.CanUndo;
			}
		}

		/// <summary>Gets a value indicating whether the commands pane can be made visible for the currently selected objects.</summary>
		/// <returns>
		///     <see langword="true" /> if the commands pane can be made visible; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x060030B5 RID: 12469 RVA: 0x000E3B52 File Offset: 0x000E1D52
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("PropertyGridCanShowCommandsDesc")]
		public virtual bool CanShowCommands
		{
			get
			{
				return this.hotcommands.WouldBeVisible;
			}
		}

		/// <summary>Gets or sets the text color used for category headings. </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> structure representing the text color.</returns>
		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x060030B6 RID: 12470 RVA: 0x000E3B5F File Offset: 0x000E1D5F
		// (set) Token: 0x060030B7 RID: 12471 RVA: 0x000E3B67 File Offset: 0x000E1D67
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridCategoryForeColorDesc")]
		[DefaultValue(typeof(Color), "ControlText")]
		public Color CategoryForeColor
		{
			get
			{
				return this.categoryForeColor;
			}
			set
			{
				if (this.categoryForeColor != value)
				{
					this.categoryForeColor = value;
					this.gridView.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the background color of the hot commands region.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Color" /> values. The default is the default system color for controls.</returns>
		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x060030B8 RID: 12472 RVA: 0x000E3B89 File Offset: 0x000E1D89
		// (set) Token: 0x060030B9 RID: 12473 RVA: 0x000E3B96 File Offset: 0x000E1D96
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridCommandsBackColorDesc")]
		public Color CommandsBackColor
		{
			get
			{
				return this.hotcommands.BackColor;
			}
			set
			{
				this.hotcommands.BackColor = value;
				this.hotcommands.Label.BackColor = value;
			}
		}

		/// <summary>Gets or sets the foreground color for the hot commands region.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Color" /> values. The default is the default system color for control text.</returns>
		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x060030BA RID: 12474 RVA: 0x000E3BB5 File Offset: 0x000E1DB5
		// (set) Token: 0x060030BB RID: 12475 RVA: 0x000E3BC2 File Offset: 0x000E1DC2
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridCommandsForeColorDesc")]
		public Color CommandsForeColor
		{
			get
			{
				return this.hotcommands.ForeColor;
			}
			set
			{
				this.hotcommands.ForeColor = value;
				this.hotcommands.Label.ForeColor = value;
			}
		}

		/// <summary>Gets or sets the link color for the executable commands region.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> structure representing the link color for the executable commands region.</returns>
		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x060030BC RID: 12476 RVA: 0x000E3BE1 File Offset: 0x000E1DE1
		// (set) Token: 0x060030BD RID: 12477 RVA: 0x000E3BF3 File Offset: 0x000E1DF3
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridCommandsLinkColorDesc")]
		public Color CommandsLinkColor
		{
			get
			{
				return this.hotcommands.Label.LinkColor;
			}
			set
			{
				this.hotcommands.Label.LinkColor = value;
			}
		}

		/// <summary>Gets or sets the color of active links in the executable commands region.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> structure representing the active link color.</returns>
		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x060030BE RID: 12478 RVA: 0x000E3C06 File Offset: 0x000E1E06
		// (set) Token: 0x060030BF RID: 12479 RVA: 0x000E3C18 File Offset: 0x000E1E18
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridCommandsActiveLinkColorDesc")]
		public Color CommandsActiveLinkColor
		{
			get
			{
				return this.hotcommands.Label.ActiveLinkColor;
			}
			set
			{
				this.hotcommands.Label.ActiveLinkColor = value;
			}
		}

		/// <summary>Gets or sets the unavailable link color for the executable commands region.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> structure representing the unavailable link color.</returns>
		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x060030C0 RID: 12480 RVA: 0x000E3C2B File Offset: 0x000E1E2B
		// (set) Token: 0x060030C1 RID: 12481 RVA: 0x000E3C3D File Offset: 0x000E1E3D
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridCommandsDisabledLinkColorDesc")]
		public Color CommandsDisabledLinkColor
		{
			get
			{
				return this.hotcommands.Label.DisabledLinkColor;
			}
			set
			{
				this.hotcommands.Label.DisabledLinkColor = value;
			}
		}

		/// <summary>Gets or sets the color of the border surrounding the hot commands region.</summary>
		/// <returns>The color of the commands border.</returns>
		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x060030C2 RID: 12482 RVA: 0x000E3C50 File Offset: 0x000E1E50
		// (set) Token: 0x060030C3 RID: 12483 RVA: 0x000E3C5D File Offset: 0x000E1E5D
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridCommandsBorderColorDesc")]
		[DefaultValue(typeof(Color), "ControlDark")]
		public Color CommandsBorderColor
		{
			get
			{
				return this.hotcommands.BorderColor;
			}
			set
			{
				this.hotcommands.BorderColor = value;
			}
		}

		/// <summary>Gets a value indicating whether the commands pane is visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the commands pane is visible; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x060030C4 RID: 12484 RVA: 0x000E3C6B File Offset: 0x000E1E6B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual bool CommandsVisible
		{
			get
			{
				return this.hotcommands.Visible;
			}
		}

		/// <summary>Gets or sets a value indicating whether the commands pane is visible for objects that expose verbs.</summary>
		/// <returns>
		///     <see langword="true" /> if the commands pane is visible; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x060030C5 RID: 12485 RVA: 0x000E3C78 File Offset: 0x000E1E78
		// (set) Token: 0x060030C6 RID: 12486 RVA: 0x000E3C88 File Offset: 0x000E1E88
		[SRCategory("CatAppearance")]
		[DefaultValue(true)]
		[SRDescription("PropertyGridCommandsVisibleIfAvailable")]
		public virtual bool CommandsVisibleIfAvailable
		{
			get
			{
				return this.hotcommands.AllowVisible;
			}
			set
			{
				bool visible = this.hotcommands.Visible;
				this.hotcommands.AllowVisible = value;
				if (visible != this.hotcommands.Visible)
				{
					this.OnLayoutInternal(false);
					this.hotcommands.Invalidate();
				}
			}
		}

		/// <summary>Gets the default location for the shortcut menu.</summary>
		/// <returns>The default location for the shortcut menu if the command is invoked. Typically, this is centered over the selected property.</returns>
		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x060030C7 RID: 12487 RVA: 0x000E3CCD File Offset: 0x000E1ECD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Point ContextMenuDefaultLocation
		{
			get
			{
				return this.GetPropertyGridView().ContextMenuDefaultLocation;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The controls associated with the property grid.</returns>
		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x060030C8 RID: 12488 RVA: 0x000E3CDA File Offset: 0x000E1EDA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Control.ControlCollection Controls
		{
			get
			{
				return base.Controls;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x060030C9 RID: 12489 RVA: 0x000E3CE2 File Offset: 0x000E1EE2
		protected override Size DefaultSize
		{
			get
			{
				return new Size(130, 130);
			}
		}

		/// <summary>Gets the type of the default tab.</summary>
		/// <returns>A <see cref="T:System.Type" /> representing the default tab.</returns>
		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x060030CA RID: 12490 RVA: 0x000E3CF3 File Offset: 0x000E1EF3
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected virtual Type DefaultTabType
		{
			get
			{
				return typeof(PropertiesTab);
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.PropertyGrid" /> control paints its toolbar with flat buttons.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.PropertyGrid" /> paints its toolbar with flat buttons; otherwise <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x060030CB RID: 12491 RVA: 0x000E3CFF File Offset: 0x000E1EFF
		// (set) Token: 0x060030CC RID: 12492 RVA: 0x000E3D07 File Offset: 0x000E1F07
		protected bool DrawFlatToolbar
		{
			get
			{
				return this.drawFlatToolBar;
			}
			set
			{
				if (this.drawFlatToolBar != value)
				{
					this.drawFlatToolBar = value;
					this.SetToolStripRenderer();
				}
				this.SetHotCommandColors(value && !AccessibilityImprovements.Level2);
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The foreground color of the control.</returns>
		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x060030CD RID: 12493 RVA: 0x00012082 File Offset: 0x00010282
		// (set) Token: 0x060030CE RID: 12494 RVA: 0x0001208A File Offset: 0x0001028A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PropertyGrid.ForeColor" /> property changes.</summary>
		// Token: 0x14000257 RID: 599
		// (add) Token: 0x060030CF RID: 12495 RVA: 0x00052766 File Offset: 0x00050966
		// (remove) Token: 0x060030D0 RID: 12496 RVA: 0x0005276F File Offset: 0x0005096F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ForeColorChanged
		{
			add
			{
				base.ForeColorChanged += value;
			}
			remove
			{
				base.ForeColorChanged -= value;
			}
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x060030D1 RID: 12497 RVA: 0x000E3D33 File Offset: 0x000E1F33
		// (set) Token: 0x060030D2 RID: 12498 RVA: 0x000E3D40 File Offset: 0x000E1F40
		private bool FreezePainting
		{
			get
			{
				return this.paintFrozen > 0;
			}
			set
			{
				if (value && base.IsHandleCreated && base.Visible)
				{
					int num = this.paintFrozen;
					this.paintFrozen = num + 1;
					if (num == 0)
					{
						base.SendMessage(11, 0, 0);
					}
				}
				if (!value)
				{
					if (this.paintFrozen == 0)
					{
						return;
					}
					int num = this.paintFrozen - 1;
					this.paintFrozen = num;
					if (num == 0)
					{
						base.SendMessage(11, 1, 0);
						base.Invalidate(true);
					}
				}
			}
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x060030D3 RID: 12499 RVA: 0x000E3DAF File Offset: 0x000E1FAF
		internal AccessibleObject HelpAccessibleObject
		{
			get
			{
				return this.doccomment.AccessibilityObject;
			}
		}

		/// <summary>Gets or sets the background color for the Help region.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Color" /> values. The default is the default system color for controls.</returns>
		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x060030D4 RID: 12500 RVA: 0x000E3DBC File Offset: 0x000E1FBC
		// (set) Token: 0x060030D5 RID: 12501 RVA: 0x000E3DC9 File Offset: 0x000E1FC9
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridHelpBackColorDesc")]
		[DefaultValue(typeof(Color), "Control")]
		public Color HelpBackColor
		{
			get
			{
				return this.doccomment.BackColor;
			}
			set
			{
				this.doccomment.BackColor = value;
			}
		}

		/// <summary>Gets or sets the foreground color for the Help region.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Color" /> values. The default is the default system color for control text.</returns>
		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x060030D6 RID: 12502 RVA: 0x000E3DD7 File Offset: 0x000E1FD7
		// (set) Token: 0x060030D7 RID: 12503 RVA: 0x000E3DE4 File Offset: 0x000E1FE4
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridHelpForeColorDesc")]
		[DefaultValue(typeof(Color), "ControlText")]
		public Color HelpForeColor
		{
			get
			{
				return this.doccomment.ForeColor;
			}
			set
			{
				this.doccomment.ForeColor = value;
			}
		}

		/// <summary>Gets or sets the color of the border surrounding the description pane.</summary>
		/// <returns>The color of the help border.</returns>
		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x060030D8 RID: 12504 RVA: 0x000E3DF2 File Offset: 0x000E1FF2
		// (set) Token: 0x060030D9 RID: 12505 RVA: 0x000E3DFF File Offset: 0x000E1FFF
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridHelpBorderColorDesc")]
		[DefaultValue(typeof(Color), "ControlDark")]
		public Color HelpBorderColor
		{
			get
			{
				return this.doccomment.BorderColor;
			}
			set
			{
				this.doccomment.BorderColor = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the Help text is visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the help text is visible; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x060030DA RID: 12506 RVA: 0x000E3E0D File Offset: 0x000E200D
		// (set) Token: 0x060030DB RID: 12507 RVA: 0x000E3E15 File Offset: 0x000E2015
		[SRCategory("CatAppearance")]
		[DefaultValue(true)]
		[Localizable(true)]
		[SRDescription("PropertyGridHelpVisibleDesc")]
		public virtual bool HelpVisible
		{
			get
			{
				return this.helpVisible;
			}
			set
			{
				this.helpVisible = value;
				this.doccomment.Visible = value;
				this.OnLayoutInternal(false);
				base.Invalidate();
				this.doccomment.Invalidate();
			}
		}

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x060030DC RID: 12508 RVA: 0x000E3E42 File Offset: 0x000E2042
		internal AccessibleObject HotCommandsAccessibleObject
		{
			get
			{
				return this.hotcommands.AccessibilityObject;
			}
		}

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x060030DD RID: 12509 RVA: 0x000E3E4F File Offset: 0x000E204F
		internal AccessibleObject GridViewAccessibleObject
		{
			get
			{
				return this.gridView.AccessibilityObject;
			}
		}

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x060030DE RID: 12510 RVA: 0x000E3E5C File Offset: 0x000E205C
		internal bool GridViewVisible
		{
			get
			{
				return this.gridView != null && this.gridView.Visible;
			}
		}

		/// <summary>Gets or sets the background color of selected items that have the input focus.</summary>
		/// <returns>The background color of focused, selected items.</returns>
		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x060030DF RID: 12511 RVA: 0x000E3E73 File Offset: 0x000E2073
		// (set) Token: 0x060030E0 RID: 12512 RVA: 0x000E3E7B File Offset: 0x000E207B
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridSelectedItemWithFocusBackColorDesc")]
		[DefaultValue(typeof(Color), "Highlight")]
		public Color SelectedItemWithFocusBackColor
		{
			get
			{
				return this.selectedItemWithFocusBackColor;
			}
			set
			{
				if (this.selectedItemWithFocusBackColor != value)
				{
					this.selectedItemWithFocusBackColor = value;
					this.gridView.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the foreground color of selected items that have the input focus.</summary>
		/// <returns>The foreground color of focused, selected items.</returns>
		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x060030E1 RID: 12513 RVA: 0x000E3E9D File Offset: 0x000E209D
		// (set) Token: 0x060030E2 RID: 12514 RVA: 0x000E3EA5 File Offset: 0x000E20A5
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridSelectedItemWithFocusForeColorDesc")]
		[DefaultValue(typeof(Color), "HighlightText")]
		public Color SelectedItemWithFocusForeColor
		{
			get
			{
				return this.selectedItemWithFocusForeColor;
			}
			set
			{
				if (this.selectedItemWithFocusForeColor != value)
				{
					this.selectedItemWithFocusForeColor = value;
					this.gridView.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the foreground color of disabled text in the grid area.</summary>
		/// <returns>The foreground color of disabled items.</returns>
		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x060030E3 RID: 12515 RVA: 0x000E3EC7 File Offset: 0x000E20C7
		// (set) Token: 0x060030E4 RID: 12516 RVA: 0x000E3ED4 File Offset: 0x000E20D4
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridDisabledItemForeColorDesc")]
		[DefaultValue(typeof(Color), "GrayText")]
		public Color DisabledItemForeColor
		{
			get
			{
				return this.gridView.GrayTextColor;
			}
			set
			{
				this.gridView.GrayTextColor = value;
				this.gridView.Invalidate();
			}
		}

		/// <summary>Gets or sets the color of the line that separates categories in the grid area.</summary>
		/// <returns>The color of the category splitter.</returns>
		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x060030E5 RID: 12517 RVA: 0x000E3EED File Offset: 0x000E20ED
		// (set) Token: 0x060030E6 RID: 12518 RVA: 0x000E3EF5 File Offset: 0x000E20F5
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridCategorySplitterColorDesc")]
		[DefaultValue(typeof(Color), "Control")]
		public Color CategorySplitterColor
		{
			get
			{
				return this.categorySplitterColor;
			}
			set
			{
				if (this.categorySplitterColor != value)
				{
					this.categorySplitterColor = value;
					this.gridView.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether OS-specific visual style glyphs are used for the expansion nodes in the grid area.</summary>
		/// <returns>
		///     <see langword="true" /> to use the visual style glyphs; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x060030E7 RID: 12519 RVA: 0x000E3F17 File Offset: 0x000E2117
		// (set) Token: 0x060030E8 RID: 12520 RVA: 0x000E3F1F File Offset: 0x000E211F
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridCanShowVisualStyleGlyphsDesc")]
		[DefaultValue(true)]
		public bool CanShowVisualStyleGlyphs
		{
			get
			{
				return this.canShowVisualStyleGlyphs;
			}
			set
			{
				if (this.canShowVisualStyleGlyphs != value)
				{
					this.canShowVisualStyleGlyphs = value;
					this.gridView.Invalidate();
				}
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Windows.Forms.ComponentModel.Com2Interop.IComPropertyBrowser.InPropertySet" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.PropertyGrid" /> control is currently setting one of the properties of its selected object; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x060030E9 RID: 12521 RVA: 0x000E3F3C File Offset: 0x000E213C
		bool IComPropertyBrowser.InPropertySet
		{
			get
			{
				return this.GetPropertyGridView().GetInPropertySet();
			}
		}

		/// <summary>Gets or sets the color of the gridlines and borders.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Color" /> values. The default is the default system color for scroll bars.</returns>
		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x060030EA RID: 12522 RVA: 0x000E3F49 File Offset: 0x000E2149
		// (set) Token: 0x060030EB RID: 12523 RVA: 0x000E3F54 File Offset: 0x000E2154
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridLineColorDesc")]
		[DefaultValue(typeof(Color), "InactiveBorder")]
		public Color LineColor
		{
			get
			{
				return this.lineColor;
			}
			set
			{
				if (this.lineColor != value)
				{
					this.lineColor = value;
					this.developerOverride = true;
					if (this.lineBrush != null)
					{
						this.lineBrush.Dispose();
						this.lineBrush = null;
					}
					this.gridView.Invalidate();
				}
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> value.</returns>
		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x060030EC RID: 12524 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x060030ED RID: 12525 RVA: 0x000204A2 File Offset: 0x0001E6A2
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PropertyGrid.Padding" /> property changes.</summary>
		// Token: 0x14000258 RID: 600
		// (add) Token: 0x060030EE RID: 12526 RVA: 0x000204AB File Offset: 0x0001E6AB
		// (remove) Token: 0x060030EF RID: 12527 RVA: 0x000204B4 File Offset: 0x0001E6B4
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

		/// <summary>Gets or sets the type of sorting the <see cref="T:System.Windows.Forms.PropertyGrid" /> uses to display properties.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.PropertySort" /> values. The default is <see cref="F:System.Windows.Forms.PropertySort.Categorized" /> or <see cref="F:System.Windows.Forms.PropertySort.Alphabetical" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.PropertySort" /> values.</exception>
		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x060030F0 RID: 12528 RVA: 0x000E3FA2 File Offset: 0x000E21A2
		// (set) Token: 0x060030F1 RID: 12529 RVA: 0x000E3FAC File Offset: 0x000E21AC
		[SRCategory("CatAppearance")]
		[DefaultValue(PropertySort.CategorizedAlphabetical)]
		[SRDescription("PropertyGridPropertySortDesc")]
		public PropertySort PropertySort
		{
			get
			{
				return this.propertySortValue;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(PropertySort));
				}
				ToolStripButton sender;
				if ((value & PropertySort.Categorized) != PropertySort.NoSort)
				{
					sender = this.viewSortButtons[0];
				}
				else if ((value & PropertySort.Alphabetical) != PropertySort.NoSort)
				{
					sender = this.viewSortButtons[1];
				}
				else
				{
					sender = this.viewSortButtons[2];
				}
				GridItem selectedGridItem = this.SelectedGridItem;
				this.OnViewSortButtonClick(sender, EventArgs.Empty);
				this.propertySortValue = value;
				if (selectedGridItem != null)
				{
					try
					{
						this.SelectedGridItem = selectedGridItem;
					}
					catch (ArgumentException)
					{
					}
				}
			}
		}

		/// <summary>Gets the collection of property tabs that are displayed in the grid.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.PropertyGrid.PropertyTabCollection" /> containing the collection of <see cref="T:System.Windows.Forms.Design.PropertyTab" /> objects being displayed by the <see cref="T:System.Windows.Forms.PropertyGrid" />.</returns>
		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x060030F2 RID: 12530 RVA: 0x000E4044 File Offset: 0x000E2244
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PropertyGrid.PropertyTabCollection PropertyTabs
		{
			get
			{
				return new PropertyGrid.PropertyTabCollection(this);
			}
		}

		/// <summary>Gets or sets the object for which the grid displays properties.</summary>
		/// <returns>The first object in the object list. If there is no currently selected object the return is <see langword="null" />.</returns>
		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x060030F3 RID: 12531 RVA: 0x000E404C File Offset: 0x000E224C
		// (set) Token: 0x060030F4 RID: 12532 RVA: 0x000E4069 File Offset: 0x000E2269
		[DefaultValue(null)]
		[SRDescription("PropertyGridSelectedObjectDesc")]
		[SRCategory("CatBehavior")]
		[TypeConverter(typeof(PropertyGrid.SelectedObjectConverter))]
		public object SelectedObject
		{
			get
			{
				if (this.currentObjects == null || this.currentObjects.Length == 0)
				{
					return null;
				}
				return this.currentObjects[0];
			}
			set
			{
				if (value == null)
				{
					this.SelectedObjects = new object[0];
					return;
				}
				this.SelectedObjects = new object[]
				{
					value
				};
			}
		}

		/// <summary>Gets or sets the currently selected objects.</summary>
		/// <returns>An array of type <see cref="T:System.Object" />. The default is an empty array.</returns>
		/// <exception cref="T:System.ArgumentException">One of the items in the array of objects had a null value. </exception>
		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x060030F6 RID: 12534 RVA: 0x000E45D0 File Offset: 0x000E27D0
		// (set) Token: 0x060030F5 RID: 12533 RVA: 0x000E408C File Offset: 0x000E228C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object[] SelectedObjects
		{
			get
			{
				if (this.currentObjects == null)
				{
					return new object[0];
				}
				return (object[])this.currentObjects.Clone();
			}
			set
			{
				try
				{
					this.FreezePainting = true;
					this.SetFlag(128, false);
					if (this.GetFlag(16))
					{
						this.SetFlag(256, false);
					}
					this.gridView.EnsurePendingChangesCommitted();
					bool flag = false;
					bool flag2 = false;
					bool flag3 = true;
					if (value != null && value.Length != 0)
					{
						for (int i = 0; i < value.Length; i++)
						{
							if (value[i] == null)
							{
								throw new ArgumentException(SR.GetString("PropertyGridSetNull", new object[]
								{
									i.ToString(CultureInfo.CurrentCulture),
									value.Length.ToString(CultureInfo.CurrentCulture)
								}));
							}
							if (value[i] is PropertyGrid.IUnimplemented)
							{
								throw new NotSupportedException(SR.GetString("PropertyGridRemotedObject", new object[]
								{
									value[i].GetType().FullName
								}));
							}
						}
					}
					else
					{
						flag3 = false;
					}
					if (this.currentObjects != null && value != null && this.currentObjects.Length == value.Length)
					{
						flag = true;
						flag2 = true;
						int num = 0;
						while (num < value.Length && (flag || flag2))
						{
							if (flag && this.currentObjects[num] != value[num])
							{
								flag = false;
							}
							Type type = this.GetUnwrappedObject(num).GetType();
							object obj = value[num];
							if (obj is ICustomTypeDescriptor)
							{
								obj = ((ICustomTypeDescriptor)obj).GetPropertyOwner(null);
							}
							Type type2 = obj.GetType();
							if (flag2 && (type != type2 || (type.IsCOMObject && type2.IsCOMObject)))
							{
								flag2 = false;
							}
							num++;
						}
					}
					if (!flag)
					{
						this.EnsureDesignerEventService();
						flag3 = (flag3 && this.GetFlag(2));
						this.SetStatusBox("", "");
						this.ClearCachedProps();
						this.peDefault = null;
						if (value == null)
						{
							this.currentObjects = new object[0];
						}
						else
						{
							this.currentObjects = (object[])value.Clone();
						}
						this.SinkPropertyNotifyEvents();
						this.SetFlag(1, true);
						if (this.gridView != null)
						{
							try
							{
								this.gridView.RemoveSelectedEntryHelpAttributes();
							}
							catch (COMException)
							{
							}
						}
						if (this.peMain != null)
						{
							this.peMain.Dispose();
						}
						if (!flag2 && !this.GetFlag(8) && this.selectedViewTab < this.viewTabButtons.Length)
						{
							Type type3 = (this.selectedViewTab == -1) ? null : this.viewTabs[this.selectedViewTab].GetType();
							ToolStripButton button = null;
							this.RefreshTabs(PropertyTabScope.Component);
							this.EnableTabs();
							if (type3 != null)
							{
								for (int j = 0; j < this.viewTabs.Length; j++)
								{
									if (this.viewTabs[j].GetType() == type3 && this.viewTabButtons[j].Visible)
									{
										button = this.viewTabButtons[j];
										break;
									}
								}
							}
							this.SelectViewTabButtonDefault(button);
						}
						if (flag3 && this.viewTabs != null && this.viewTabs.Length > 1 && this.viewTabs[1] is EventsTab)
						{
							flag3 = this.viewTabButtons[1].Visible;
							Attribute[] array = new Attribute[this.BrowsableAttributes.Count];
							this.BrowsableAttributes.CopyTo(array, 0);
							Hashtable hashtable = null;
							if (this.currentObjects.Length > 10)
							{
								hashtable = new Hashtable();
							}
							int num2 = 0;
							while (num2 < this.currentObjects.Length && flag3)
							{
								object obj2 = this.currentObjects[num2];
								if (obj2 is ICustomTypeDescriptor)
								{
									obj2 = ((ICustomTypeDescriptor)obj2).GetPropertyOwner(null);
								}
								Type type4 = obj2.GetType();
								if (hashtable == null || !hashtable.Contains(type4))
								{
									flag3 = (flag3 && obj2 is IComponent && ((IComponent)obj2).Site != null);
									PropertyDescriptorCollection properties = ((EventsTab)this.viewTabs[1]).GetProperties(obj2, array);
									flag3 = (flag3 && properties != null && properties.Count > 0);
									if (flag3 && hashtable != null)
									{
										hashtable[type4] = type4;
									}
								}
								num2++;
							}
						}
						this.ShowEventsButton(flag3 && this.currentObjects.Length != 0);
						this.DisplayHotCommands();
						if (this.currentObjects.Length == 1)
						{
							this.EnablePropPageButton(this.currentObjects[0]);
						}
						else
						{
							this.EnablePropPageButton(null);
						}
						this.OnSelectedObjectsChanged(EventArgs.Empty);
					}
					if (!this.GetFlag(8))
					{
						if (this.currentObjects.Length != 0 && this.GetFlag(32))
						{
							object activeDesigner = this.ActiveDesigner;
							if (activeDesigner != null && this.designerSelections != null && this.designerSelections.ContainsKey(activeDesigner.GetHashCode()))
							{
								int num3 = (int)this.designerSelections[activeDesigner.GetHashCode()];
								if (num3 < this.viewTabs.Length && (num3 == 0 || this.viewTabButtons[num3].Visible))
								{
									this.SelectViewTabButton(this.viewTabButtons[num3], true);
								}
							}
							else
							{
								this.Refresh(false);
							}
							this.SetFlag(32, false);
						}
						else
						{
							this.Refresh(true);
						}
						if (this.currentObjects.Length != 0)
						{
							this.SaveTabSelection();
						}
					}
				}
				finally
				{
					this.FreezePainting = false;
				}
			}
		}

		/// <summary>Gets the currently selected property tab.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Design.PropertyTab" /> that is providing the selected view.</returns>
		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x060030F7 RID: 12535 RVA: 0x000E45F1 File Offset: 0x000E27F1
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PropertyTab SelectedTab
		{
			get
			{
				return this.viewTabs[this.selectedViewTab];
			}
		}

		/// <summary>Gets or sets the selected grid item.</summary>
		/// <returns>The currently selected row in the property grid.</returns>
		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x060030F8 RID: 12536 RVA: 0x000E4600 File Offset: 0x000E2800
		// (set) Token: 0x060030F9 RID: 12537 RVA: 0x000E4624 File Offset: 0x000E2824
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public GridItem SelectedGridItem
		{
			get
			{
				GridItem selectedGridEntry = this.gridView.SelectedGridEntry;
				if (selectedGridEntry == null)
				{
					return this.peMain;
				}
				return selectedGridEntry;
			}
			set
			{
				this.gridView.SelectedGridEntry = (GridEntry)value;
			}
		}

		/// <summary>Gets a value indicating whether the control should display focus rectangles.</summary>
		/// <returns>
		///     <see langword="true" /> if the control should display focus rectangles; otherwise, <see langword="false" />.
		/// </returns>
		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x060030FA RID: 12538 RVA: 0x0000E214 File Offset: 0x0000C414
		protected internal override bool ShowFocusCues
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets the site of the control.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.ISite" /> associated with the Control, if any.
		/// </returns>
		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x060030FB RID: 12539 RVA: 0x00040834 File Offset: 0x0003EA34
		// (set) Token: 0x060030FC RID: 12540 RVA: 0x000E4638 File Offset: 0x000E2838
		public override ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				base.SuspendAllLayout(this);
				base.Site = value;
				this.gridView.ServiceProvider = value;
				if (value == null)
				{
					this.ActiveDesigner = null;
				}
				else
				{
					this.ActiveDesigner = (IDesignerHost)value.GetService(typeof(IDesignerHost));
				}
				base.ResumeAllLayout(this, true);
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x060030FD RID: 12541 RVA: 0x000E468E File Offset: 0x000E288E
		internal bool SortedByCategories
		{
			get
			{
				return (this.PropertySort & PropertySort.Categorized) > PropertySort.NoSort;
			}
		}

		/// <summary>Gets or sets the text associated with this control.</summary>
		/// <returns>The text associated with this control.</returns>
		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x060030FE RID: 12542 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x060030FF RID: 12543 RVA: 0x0001BFAD File Offset: 0x0001A1AD
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>Occurs when the text of the <see cref="T:System.Windows.Forms.PropertyGrid" /> changes.</summary>
		// Token: 0x14000259 RID: 601
		// (add) Token: 0x06003100 RID: 12544 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x06003101 RID: 12545 RVA: 0x0003E43E File Offset: 0x0003C63E
		[Browsable(false)]
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

		/// <summary>Gets or sets a value indicating whether buttons appear in standard size or in large size.</summary>
		/// <returns>
		///     <see langword="true" /> if buttons on the control appear large; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06003102 RID: 12546 RVA: 0x000E469B File Offset: 0x000E289B
		// (set) Token: 0x06003103 RID: 12547 RVA: 0x000E46A8 File Offset: 0x000E28A8
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridLargeButtonsDesc")]
		[DefaultValue(false)]
		public bool LargeButtons
		{
			get
			{
				return this.buttonType == 1;
			}
			set
			{
				if (value == (this.buttonType == 1))
				{
					return;
				}
				this.buttonType = (value ? 1 : 0);
				if (value)
				{
					this.EnsureLargeButtons();
					if (this.imageList != null && this.imageList[1] != null)
					{
						this.toolStrip.ImageScalingSize = this.imageList[1].ImageSize;
					}
				}
				else if (this.imageList != null && this.imageList[0] != null)
				{
					this.toolStrip.ImageScalingSize = this.imageList[0].ImageSize;
				}
				this.toolStrip.ImageList = this.imageList[this.buttonType];
				this.OnLayoutInternal(false);
				base.Invalidate();
				this.toolStrip.Invalidate();
			}
		}

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06003104 RID: 12548 RVA: 0x000E475E File Offset: 0x000E295E
		internal AccessibleObject ToolbarAccessibleObject
		{
			get
			{
				return this.toolStrip.AccessibilityObject;
			}
		}

		/// <summary>Gets or sets a value indicating whether the toolbar is visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the toolbar is visible; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06003105 RID: 12549 RVA: 0x000E476B File Offset: 0x000E296B
		// (set) Token: 0x06003106 RID: 12550 RVA: 0x000E4773 File Offset: 0x000E2973
		[SRCategory("CatAppearance")]
		[DefaultValue(true)]
		[SRDescription("PropertyGridToolbarVisibleDesc")]
		public virtual bool ToolbarVisible
		{
			get
			{
				return this.toolbarVisible;
			}
			set
			{
				this.toolbarVisible = value;
				this.toolStrip.Visible = value;
				this.OnLayoutInternal(false);
				if (value)
				{
					this.SetupToolbar(this.viewTabsDirty);
				}
				base.Invalidate();
				this.toolStrip.Invalidate();
			}
		}

		/// <summary>Gets or sets the painting functionality for <see cref="T:System.Windows.Forms.ToolStrip" /> objects.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripRenderer" /> for the <see cref="T:System.Windows.Forms.PropertyGrid" />.</returns>
		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x06003107 RID: 12551 RVA: 0x000E47AF File Offset: 0x000E29AF
		// (set) Token: 0x06003108 RID: 12552 RVA: 0x000E47C6 File Offset: 0x000E29C6
		protected ToolStripRenderer ToolStripRenderer
		{
			get
			{
				if (this.toolStrip != null)
				{
					return this.toolStrip.Renderer;
				}
				return null;
			}
			set
			{
				if (this.toolStrip != null)
				{
					this.toolStrip.Renderer = value;
				}
			}
		}

		/// <summary>Gets or sets a value indicating the background color in the grid.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Color" /> values. The default is the default system color for windows.</returns>
		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06003109 RID: 12553 RVA: 0x000E47DC File Offset: 0x000E29DC
		// (set) Token: 0x0600310A RID: 12554 RVA: 0x000E47E9 File Offset: 0x000E29E9
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridViewBackColorDesc")]
		[DefaultValue(typeof(Color), "Window")]
		public Color ViewBackColor
		{
			get
			{
				return this.gridView.BackColor;
			}
			set
			{
				this.gridView.BackColor = value;
				this.gridView.Invalidate();
			}
		}

		/// <summary>Gets or sets a value indicating the color of the text in the grid.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Color" /> values. The default is current system color for text in windows.</returns>
		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x0600310B RID: 12555 RVA: 0x000E4802 File Offset: 0x000E2A02
		// (set) Token: 0x0600310C RID: 12556 RVA: 0x000E480F File Offset: 0x000E2A0F
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridViewForeColorDesc")]
		[DefaultValue(typeof(Color), "WindowText")]
		public Color ViewForeColor
		{
			get
			{
				return this.gridView.ForeColor;
			}
			set
			{
				this.gridView.ForeColor = value;
				this.gridView.Invalidate();
			}
		}

		/// <summary>Gets or sets the color of the border surrounding the grid area.</summary>
		/// <returns>The color of the property grid border.</returns>
		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x0600310D RID: 12557 RVA: 0x000E4828 File Offset: 0x000E2A28
		// (set) Token: 0x0600310E RID: 12558 RVA: 0x000E4830 File Offset: 0x000E2A30
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridViewBorderColorDesc")]
		[DefaultValue(typeof(Color), "ControlDark")]
		public Color ViewBorderColor
		{
			get
			{
				return this.viewBorderColor;
			}
			set
			{
				if (this.viewBorderColor != value)
				{
					this.viewBorderColor = value;
					this.gridView.Invalidate();
				}
			}
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x000E4854 File Offset: 0x000E2A54
		private int AddImage(Bitmap image)
		{
			image.MakeTransparent();
			if (DpiHelper.IsScalingRequired && (image.Size.Width != PropertyGrid.normalButtonSize.Width || image.Size.Height != PropertyGrid.normalButtonSize.Height))
			{
				image = DpiHelper.CreateResizedBitmap(image, PropertyGrid.normalButtonSize);
			}
			int count = this.imageList[0].Images.Count;
			this.imageList[0].Images.Add(image);
			return count;
		}

		/// <summary>Occurs when a key is first pressed.</summary>
		// Token: 0x1400025A RID: 602
		// (add) Token: 0x06003110 RID: 12560 RVA: 0x000B0E9E File Offset: 0x000AF09E
		// (remove) Token: 0x06003111 RID: 12561 RVA: 0x000B0EA7 File Offset: 0x000AF0A7
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event KeyEventHandler KeyDown
		{
			add
			{
				base.KeyDown += value;
			}
			remove
			{
				base.KeyDown -= value;
			}
		}

		/// <summary>Occurs when a key is pressed while the control has focus.</summary>
		// Token: 0x1400025B RID: 603
		// (add) Token: 0x06003112 RID: 12562 RVA: 0x000B0EB0 File Offset: 0x000AF0B0
		// (remove) Token: 0x06003113 RID: 12563 RVA: 0x000B0EB9 File Offset: 0x000AF0B9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event KeyPressEventHandler KeyPress
		{
			add
			{
				base.KeyPress += value;
			}
			remove
			{
				base.KeyPress -= value;
			}
		}

		/// <summary>Occurs when a key is released while the control has focus.</summary>
		// Token: 0x1400025C RID: 604
		// (add) Token: 0x06003114 RID: 12564 RVA: 0x000B0E8C File Offset: 0x000AF08C
		// (remove) Token: 0x06003115 RID: 12565 RVA: 0x000B0E95 File Offset: 0x000AF095
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event KeyEventHandler KeyUp
		{
			add
			{
				base.KeyUp += value;
			}
			remove
			{
				base.KeyUp -= value;
			}
		}

		/// <summary>Occurs when the user clicks the <see cref="T:System.Windows.Forms.PropertyGrid" /> control with the mouse.</summary>
		// Token: 0x1400025D RID: 605
		// (add) Token: 0x06003116 RID: 12566 RVA: 0x000B0EC2 File Offset: 0x000AF0C2
		// (remove) Token: 0x06003117 RID: 12567 RVA: 0x000B0ECB File Offset: 0x000AF0CB
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event MouseEventHandler MouseDown
		{
			add
			{
				base.MouseDown += value;
			}
			remove
			{
				base.MouseDown -= value;
			}
		}

		/// <summary>Occurs when the mouse pointer is over the control and the user releases a mouse button.</summary>
		// Token: 0x1400025E RID: 606
		// (add) Token: 0x06003118 RID: 12568 RVA: 0x000B0ED4 File Offset: 0x000AF0D4
		// (remove) Token: 0x06003119 RID: 12569 RVA: 0x000B0EDD File Offset: 0x000AF0DD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event MouseEventHandler MouseUp
		{
			add
			{
				base.MouseUp += value;
			}
			remove
			{
				base.MouseUp -= value;
			}
		}

		/// <summary>Occurs when the mouse pointer moves over the control.</summary>
		// Token: 0x1400025F RID: 607
		// (add) Token: 0x0600311A RID: 12570 RVA: 0x000B0EE6 File Offset: 0x000AF0E6
		// (remove) Token: 0x0600311B RID: 12571 RVA: 0x000B0EEF File Offset: 0x000AF0EF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event MouseEventHandler MouseMove
		{
			add
			{
				base.MouseMove += value;
			}
			remove
			{
				base.MouseMove -= value;
			}
		}

		/// <summary>Occurs when the mouse pointer enters the control.</summary>
		// Token: 0x14000260 RID: 608
		// (add) Token: 0x0600311C RID: 12572 RVA: 0x000B0EF8 File Offset: 0x000AF0F8
		// (remove) Token: 0x0600311D RID: 12573 RVA: 0x000B0F01 File Offset: 0x000AF101
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event EventHandler MouseEnter
		{
			add
			{
				base.MouseEnter += value;
			}
			remove
			{
				base.MouseEnter -= value;
			}
		}

		/// <summary>Occurs when the mouse pointer leaves the control.</summary>
		// Token: 0x14000261 RID: 609
		// (add) Token: 0x0600311E RID: 12574 RVA: 0x000B0F0A File Offset: 0x000AF10A
		// (remove) Token: 0x0600311F RID: 12575 RVA: 0x000B0F13 File Offset: 0x000AF113
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event EventHandler MouseLeave
		{
			add
			{
				base.MouseLeave += value;
			}
			remove
			{
				base.MouseLeave -= value;
			}
		}

		/// <summary>Occurs when a property value changes.</summary>
		// Token: 0x14000262 RID: 610
		// (add) Token: 0x06003120 RID: 12576 RVA: 0x000E48D6 File Offset: 0x000E2AD6
		// (remove) Token: 0x06003121 RID: 12577 RVA: 0x000E48E9 File Offset: 0x000E2AE9
		[SRCategory("CatPropertyChanged")]
		[SRDescription("PropertyGridPropertyValueChangedDescr")]
		public event PropertyValueChangedEventHandler PropertyValueChanged
		{
			add
			{
				base.Events.AddHandler(PropertyGrid.EventPropertyValueChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(PropertyGrid.EventPropertyValueChanged, value);
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.PropertyGrid" /> control is browsing a COM object and the user renames the object.</summary>
		// Token: 0x14000263 RID: 611
		// (add) Token: 0x06003122 RID: 12578 RVA: 0x000E48FC File Offset: 0x000E2AFC
		// (remove) Token: 0x06003123 RID: 12579 RVA: 0x000E490F File Offset: 0x000E2B0F
		event ComponentRenameEventHandler IComPropertyBrowser.ComComponentNameChanged
		{
			add
			{
				base.Events.AddHandler(PropertyGrid.EventComComponentNameChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(PropertyGrid.EventComComponentNameChanged, value);
			}
		}

		/// <summary>Occurs when a property tab changes.</summary>
		// Token: 0x14000264 RID: 612
		// (add) Token: 0x06003124 RID: 12580 RVA: 0x000E4922 File Offset: 0x000E2B22
		// (remove) Token: 0x06003125 RID: 12581 RVA: 0x000E4935 File Offset: 0x000E2B35
		[SRCategory("CatPropertyChanged")]
		[SRDescription("PropertyGridPropertyTabchangedDescr")]
		public event PropertyTabChangedEventHandler PropertyTabChanged
		{
			add
			{
				base.Events.AddHandler(PropertyGrid.EventPropertyTabChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(PropertyGrid.EventPropertyTabChanged, value);
			}
		}

		/// <summary>Occurs when the sort mode is changed.</summary>
		// Token: 0x14000265 RID: 613
		// (add) Token: 0x06003126 RID: 12582 RVA: 0x000E4948 File Offset: 0x000E2B48
		// (remove) Token: 0x06003127 RID: 12583 RVA: 0x000E495B File Offset: 0x000E2B5B
		[SRCategory("CatPropertyChanged")]
		[SRDescription("PropertyGridPropertySortChangedDescr")]
		public event EventHandler PropertySortChanged
		{
			add
			{
				base.Events.AddHandler(PropertyGrid.EventPropertySortChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(PropertyGrid.EventPropertySortChanged, value);
			}
		}

		/// <summary>Occurs when the selected <see cref="T:System.Windows.Forms.GridItem" /> is changed.</summary>
		// Token: 0x14000266 RID: 614
		// (add) Token: 0x06003128 RID: 12584 RVA: 0x000E496E File Offset: 0x000E2B6E
		// (remove) Token: 0x06003129 RID: 12585 RVA: 0x000E4981 File Offset: 0x000E2B81
		[SRCategory("CatPropertyChanged")]
		[SRDescription("PropertyGridSelectedGridItemChangedDescr")]
		public event SelectedGridItemChangedEventHandler SelectedGridItemChanged
		{
			add
			{
				base.Events.AddHandler(PropertyGrid.EventSelectedGridItemChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(PropertyGrid.EventSelectedGridItemChanged, value);
			}
		}

		/// <summary>Occurs when the objects selected by the <see cref="P:System.Windows.Forms.PropertyGrid.SelectedObjects" /> property have changed.</summary>
		// Token: 0x14000267 RID: 615
		// (add) Token: 0x0600312A RID: 12586 RVA: 0x000E4994 File Offset: 0x000E2B94
		// (remove) Token: 0x0600312B RID: 12587 RVA: 0x000E49A7 File Offset: 0x000E2BA7
		[SRCategory("CatPropertyChanged")]
		[SRDescription("PropertyGridSelectedObjectsChangedDescr")]
		public event EventHandler SelectedObjectsChanged
		{
			add
			{
				base.Events.AddHandler(PropertyGrid.EventSelectedObjectsChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(PropertyGrid.EventSelectedObjectsChanged, value);
			}
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x000E49BA File Offset: 0x000E2BBA
		internal void AddTab(Type tabType, PropertyTabScope scope)
		{
			this.AddRefTab(tabType, null, scope, true);
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x000E49C8 File Offset: 0x000E2BC8
		internal void AddRefTab(Type tabType, object component, PropertyTabScope type, bool setupToolbar)
		{
			PropertyTab propertyTab = null;
			int num = -1;
			if (this.viewTabs != null)
			{
				for (int i = 0; i < this.viewTabs.Length; i++)
				{
					if (tabType == this.viewTabs[i].GetType())
					{
						propertyTab = this.viewTabs[i];
						num = i;
						break;
					}
				}
			}
			else
			{
				num = 0;
			}
			if (propertyTab == null)
			{
				IDesignerHost host = null;
				if (component != null && component is IComponent && ((IComponent)component).Site != null)
				{
					host = (IDesignerHost)((IComponent)component).Site.GetService(typeof(IDesignerHost));
				}
				try
				{
					propertyTab = this.CreateTab(tabType, host);
				}
				catch (Exception ex)
				{
					return;
				}
				if (this.viewTabs != null)
				{
					num = this.viewTabs.Length;
					if (tabType == this.DefaultTabType)
					{
						num = 0;
					}
					else if (typeof(EventsTab).IsAssignableFrom(tabType))
					{
						num = 1;
					}
					else
					{
						for (int j = 1; j < this.viewTabs.Length; j++)
						{
							if (!(this.viewTabs[j] is EventsTab) && string.Compare(propertyTab.TabName, this.viewTabs[j].TabName, false, CultureInfo.InvariantCulture) < 0)
							{
								num = j;
								break;
							}
						}
					}
				}
				PropertyTab[] array = new PropertyTab[this.viewTabs.Length + 1];
				Array.Copy(this.viewTabs, 0, array, 0, num);
				Array.Copy(this.viewTabs, num, array, num + 1, this.viewTabs.Length - num);
				array[num] = propertyTab;
				this.viewTabs = array;
				this.viewTabsDirty = true;
				PropertyTabScope[] array2 = new PropertyTabScope[this.viewTabScopes.Length + 1];
				Array.Copy(this.viewTabScopes, 0, array2, 0, num);
				Array.Copy(this.viewTabScopes, num, array2, num + 1, this.viewTabScopes.Length - num);
				array2[num] = type;
				this.viewTabScopes = array2;
			}
			if (propertyTab != null && component != null)
			{
				try
				{
					object[] components = propertyTab.Components;
					int num2 = (components == null) ? 0 : components.Length;
					object[] array3 = new object[num2 + 1];
					if (num2 > 0)
					{
						Array.Copy(components, array3, num2);
					}
					array3[num2] = component;
					propertyTab.Components = array3;
				}
				catch (Exception ex2)
				{
					this.RemoveTab(num, false);
				}
			}
			if (setupToolbar)
			{
				this.SetupToolbar();
				this.ShowEventsButton(false);
			}
		}

		/// <summary>Collapses all the categories in the <see cref="T:System.Windows.Forms.PropertyGrid" />.</summary>
		// Token: 0x0600312E RID: 12590 RVA: 0x000E4C0C File Offset: 0x000E2E0C
		public void CollapseAllGridItems()
		{
			this.gridView.RecursivelyExpand(this.peMain, false, false, -1);
		}

		// Token: 0x0600312F RID: 12591 RVA: 0x000E4C22 File Offset: 0x000E2E22
		private void ClearCachedProps()
		{
			if (this.viewTabProps != null)
			{
				this.viewTabProps.Clear();
			}
		}

		// Token: 0x06003130 RID: 12592 RVA: 0x000E4C37 File Offset: 0x000E2E37
		internal void ClearValueCaches()
		{
			if (this.peMain != null)
			{
				this.peMain.ClearCachedValues();
			}
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x000E4C4C File Offset: 0x000E2E4C
		internal void ClearTabs(PropertyTabScope tabScope)
		{
			if (tabScope < PropertyTabScope.Document)
			{
				throw new ArgumentException(SR.GetString("PropertyGridTabScope"));
			}
			this.RemoveTabs(tabScope, true);
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x000E4C6A File Offset: 0x000E2E6A
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level3)
			{
				return new PropertyGridAccessibleObject(this);
			}
			return base.CreateAccessibilityInstance();
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x000E4C80 File Offset: 0x000E2E80
		private PropertyGridView CreateGridView(IServiceProvider sp)
		{
			return new PropertyGridView(sp, this);
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x000E4C8C File Offset: 0x000E2E8C
		private ToolStripSeparator CreateSeparatorButton()
		{
			ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
			if (DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements)
			{
				toolStripSeparator.DeviceDpi = base.DeviceDpi;
			}
			return toolStripSeparator;
		}

		/// <summary>When overridden in a derived class, enables the creation of a <see cref="T:System.Windows.Forms.Design.PropertyTab" />.</summary>
		/// <param name="tabType">The type of tab to create. </param>
		/// <returns>The newly created property tab. Returns <see langword="null" /> in its default implementation.</returns>
		// Token: 0x06003135 RID: 12597 RVA: 0x0000DE5C File Offset: 0x0000C05C
		protected virtual PropertyTab CreatePropertyTab(Type tabType)
		{
			return null;
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x000E4CB4 File Offset: 0x000E2EB4
		private PropertyTab CreateTab(Type tabType, IDesignerHost host)
		{
			PropertyTab propertyTab = this.CreatePropertyTab(tabType);
			if (propertyTab == null)
			{
				ConstructorInfo constructor = tabType.GetConstructor(new Type[]
				{
					typeof(IServiceProvider)
				});
				object obj = null;
				if (constructor == null)
				{
					constructor = tabType.GetConstructor(new Type[]
					{
						typeof(IDesignerHost)
					});
					if (constructor != null)
					{
						obj = host;
					}
				}
				else
				{
					obj = this.Site;
				}
				if (obj != null && constructor != null)
				{
					propertyTab = (PropertyTab)constructor.Invoke(new object[]
					{
						obj
					});
				}
				else
				{
					propertyTab = (PropertyTab)Activator.CreateInstance(tabType);
				}
			}
			if (propertyTab != null)
			{
				Bitmap bitmap = propertyTab.Bitmap;
				if (bitmap == null)
				{
					throw new ArgumentException(SR.GetString("PropertyGridNoBitmap", new object[]
					{
						propertyTab.GetType().FullName
					}));
				}
				Size size = bitmap.Size;
				if (size.Width != 16 || size.Height != 16)
				{
					bitmap = new Bitmap(bitmap, new Size(16, 16));
				}
				string tabName = propertyTab.TabName;
				if (tabName == null || tabName.Length == 0)
				{
					throw new ArgumentException(SR.GetString("PropertyGridTabName", new object[]
					{
						propertyTab.GetType().FullName
					}));
				}
			}
			return propertyTab;
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x000E4DF0 File Offset: 0x000E2FF0
		private ToolStripButton CreatePushButton(string toolTipText, int imageIndex, EventHandler eventHandler, bool useCheckButtonRole = false)
		{
			ToolStripButton toolStripButton = new ToolStripButton();
			if (DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements)
			{
				toolStripButton.DeviceDpi = base.DeviceDpi;
			}
			toolStripButton.Text = toolTipText;
			toolStripButton.AutoToolTip = true;
			toolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			toolStripButton.ImageIndex = imageIndex;
			toolStripButton.Click += eventHandler;
			toolStripButton.ImageScaling = ToolStripItemImageScaling.SizeToFit;
			if (AccessibilityImprovements.Level1 && useCheckButtonRole)
			{
				toolStripButton.AccessibleRole = AccessibleRole.CheckButton;
			}
			return toolStripButton;
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x000E4E54 File Offset: 0x000E3054
		internal void DumpPropsToConsole()
		{
			this.gridView.DumpPropsToConsole(this.peMain, "");
		}

		// Token: 0x06003139 RID: 12601 RVA: 0x000E4E6C File Offset: 0x000E306C
		private void DisplayHotCommands()
		{
			bool visible = this.hotcommands.Visible;
			IComponent component = null;
			DesignerVerb[] array = null;
			if (this.currentObjects != null && this.currentObjects.Length != 0)
			{
				for (int i = 0; i < this.currentObjects.Length; i++)
				{
					object unwrappedObject = this.GetUnwrappedObject(i);
					if (unwrappedObject is IComponent)
					{
						component = (IComponent)unwrappedObject;
						break;
					}
				}
				if (component != null)
				{
					ISite site = component.Site;
					if (site != null)
					{
						IMenuCommandService menuCommandService = (IMenuCommandService)site.GetService(typeof(IMenuCommandService));
						if (menuCommandService != null)
						{
							array = new DesignerVerb[menuCommandService.Verbs.Count];
							menuCommandService.Verbs.CopyTo(array, 0);
						}
						else if (this.currentObjects.Length == 1 && this.GetUnwrappedObject(0) is IComponent)
						{
							IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
							if (designerHost != null)
							{
								IDesigner designer = designerHost.GetDesigner(component);
								if (designer != null)
								{
									array = new DesignerVerb[designer.Verbs.Count];
									designer.Verbs.CopyTo(array, 0);
								}
							}
						}
					}
				}
			}
			if (!base.DesignMode)
			{
				if (array != null && array.Length != 0)
				{
					this.hotcommands.SetVerbs(component, array);
				}
				else
				{
					this.hotcommands.SetVerbs(null, null);
				}
				if (visible != this.hotcommands.Visible)
				{
					this.OnLayoutInternal(false);
				}
			}
		}

		/// <summary>Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.PropertyGrid" />.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x0600313A RID: 12602 RVA: 0x000E4FC8 File Offset: 0x000E31C8
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.GetFlag(2))
				{
					if (this.designerEventService != null)
					{
						this.designerEventService.ActiveDesignerChanged -= this.OnActiveDesignerChanged;
					}
					this.designerEventService = null;
					this.SetFlag(2, false);
				}
				this.ActiveDesigner = null;
				if (this.viewTabs != null)
				{
					for (int i = 0; i < this.viewTabs.Length; i++)
					{
						this.viewTabs[i].Dispose();
					}
					this.viewTabs = null;
				}
				if (this.imageList != null)
				{
					for (int j = 0; j < this.imageList.Length; j++)
					{
						if (this.imageList[j] != null)
						{
							this.imageList[j].Dispose();
						}
					}
					this.imageList = null;
				}
				if (this.bmpAlpha != null)
				{
					this.bmpAlpha.Dispose();
					this.bmpAlpha = null;
				}
				if (this.bmpCategory != null)
				{
					this.bmpCategory.Dispose();
					this.bmpCategory = null;
				}
				if (this.bmpPropPage != null)
				{
					this.bmpPropPage.Dispose();
					this.bmpPropPage = null;
				}
				if (this.lineBrush != null)
				{
					this.lineBrush.Dispose();
					this.lineBrush = null;
				}
				if (this.peMain != null)
				{
					this.peMain.Dispose();
					this.peMain = null;
				}
				if (this.currentObjects != null)
				{
					this.currentObjects = null;
					this.SinkPropertyNotifyEvents();
				}
				this.ClearCachedProps();
				this.currentPropEntries = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600313B RID: 12603 RVA: 0x000E512C File Offset: 0x000E332C
		private void DividerDraw(int y)
		{
			if (y == -1)
			{
				return;
			}
			Rectangle bounds = this.gridView.Bounds;
			bounds.Y = y - PropertyGrid.cyDivider;
			bounds.Height = PropertyGrid.cyDivider;
			PropertyGrid.DrawXorBar(this, bounds);
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x000E516C File Offset: 0x000E336C
		private PropertyGrid.SnappableControl DividerInside(int x, int y)
		{
			int num = -1;
			if (this.hotcommands.Visible)
			{
				Point location = this.hotcommands.Location;
				if (y >= location.Y - PropertyGrid.cyDivider && y <= location.Y + 1)
				{
					return this.hotcommands;
				}
				num = 0;
			}
			if (this.doccomment.Visible)
			{
				Point location2 = this.doccomment.Location;
				if (y >= location2.Y - PropertyGrid.cyDivider && y <= location2.Y + 1)
				{
					return this.doccomment;
				}
				if (num == -1)
				{
					num = 1;
				}
			}
			if (num != -1)
			{
				int y2 = this.gridView.Location.Y;
				int num2 = y2 + this.gridView.Size.Height;
				if (Math.Abs(num2 - y) <= 1 && y > y2)
				{
					if (num == 0)
					{
						return this.hotcommands;
					}
					if (num == 1)
					{
						return this.doccomment;
					}
				}
			}
			return null;
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x000E5254 File Offset: 0x000E3454
		private int DividerLimitHigh(PropertyGrid.SnappableControl target)
		{
			int num = this.gridView.Location.Y + 20;
			if (target == this.doccomment && this.hotcommands.Visible)
			{
				num += this.hotcommands.Size.Height + 2;
			}
			return num;
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x000E52A8 File Offset: 0x000E34A8
		private int DividerLimitMove(PropertyGrid.SnappableControl target, int y)
		{
			Rectangle bounds = target.Bounds;
			int val = Math.Min(bounds.Y + bounds.Height - 15, y);
			return Math.Max(this.DividerLimitHigh(target), val);
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x000E52E8 File Offset: 0x000E34E8
		private static void DrawXorBar(Control ctlDrawTo, Rectangle rcFrame)
		{
			Rectangle rectangle = ctlDrawTo.RectangleToScreen(rcFrame);
			if (rectangle.Width < rectangle.Height)
			{
				for (int i = 0; i < rectangle.Width; i++)
				{
					ControlPaint.DrawReversibleLine(new Point(rectangle.X + i, rectangle.Y), new Point(rectangle.X + i, rectangle.Y + rectangle.Height), ctlDrawTo.BackColor);
				}
				return;
			}
			for (int j = 0; j < rectangle.Height; j++)
			{
				ControlPaint.DrawReversibleLine(new Point(rectangle.X, rectangle.Y + j), new Point(rectangle.X + rectangle.Width, rectangle.Y + j), ctlDrawTo.BackColor);
			}
		}

		/// <summary>Closes any open drop-down controls on the <see cref="T:System.Windows.Forms.PropertyGrid" /> control. For a description of this member, see <see cref="M:System.Windows.Forms.ComponentModel.Com2Interop.IComPropertyBrowser.DropDownDone" />.</summary>
		// Token: 0x06003140 RID: 12608 RVA: 0x000E53AC File Offset: 0x000E35AC
		void IComPropertyBrowser.DropDownDone()
		{
			this.GetPropertyGridView().DropDownDone();
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x000E53BC File Offset: 0x000E35BC
		private bool EnablePropPageButton(object obj)
		{
			if (obj == null)
			{
				this.btnViewPropertyPages.Enabled = false;
				return false;
			}
			IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
			bool flag;
			if (iuiservice != null)
			{
				flag = iuiservice.CanShowComponentEditor(obj);
			}
			else
			{
				flag = (TypeDescriptor.GetEditor(obj, typeof(ComponentEditor)) != null);
			}
			this.btnViewPropertyPages.Enabled = flag;
			return flag;
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x000E5420 File Offset: 0x000E3620
		private void EnableTabs()
		{
			if (this.currentObjects != null)
			{
				this.SetupToolbar();
				for (int i = 1; i < this.viewTabs.Length; i++)
				{
					bool flag = true;
					for (int j = 0; j < this.currentObjects.Length; j++)
					{
						try
						{
							if (!this.viewTabs[i].CanExtend(this.GetUnwrappedObject(j)))
							{
								flag = false;
								break;
							}
						}
						catch (Exception ex)
						{
							flag = false;
							break;
						}
					}
					if (flag != this.viewTabButtons[i].Visible)
					{
						this.viewTabButtons[i].Visible = flag;
						if (!flag && i == this.selectedViewTab)
						{
							this.SelectViewTabButton(this.viewTabButtons[0], true);
						}
					}
				}
			}
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x000E54D4 File Offset: 0x000E36D4
		private void EnsureDesignerEventService()
		{
			if (this.GetFlag(2))
			{
				return;
			}
			this.designerEventService = (IDesignerEventService)this.GetService(typeof(IDesignerEventService));
			if (this.designerEventService != null)
			{
				this.SetFlag(2, true);
				this.designerEventService.ActiveDesignerChanged += this.OnActiveDesignerChanged;
				this.OnActiveDesignerChanged(null, new ActiveDesignerEventArgs(null, this.designerEventService.ActiveDesigner));
			}
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x000E5548 File Offset: 0x000E3748
		private void EnsureLargeButtons()
		{
			if (this.imageList[1] == null)
			{
				this.imageList[1] = new ImageList();
				this.imageList[1].ImageSize = PropertyGrid.largeButtonSize;
				if (DpiHelper.IsScalingRequired)
				{
					this.AddLargeImage(this.bmpAlpha);
					this.AddLargeImage(this.bmpCategory);
					foreach (PropertyTab propertyTab in this.viewTabs)
					{
						this.AddLargeImage(propertyTab.Bitmap);
					}
					this.AddLargeImage(this.bmpPropPage);
					return;
				}
				ImageList.ImageCollection images = this.imageList[0].Images;
				for (int j = 0; j < images.Count; j++)
				{
					if (images[j] is Bitmap)
					{
						this.imageList[1].Images.Add(new Bitmap((Bitmap)images[j], PropertyGrid.largeButtonSize.Width, PropertyGrid.largeButtonSize.Height));
					}
				}
			}
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x000E563C File Offset: 0x000E383C
		private void AddLargeImage(Bitmap originalBitmap)
		{
			if (originalBitmap == null)
			{
				return;
			}
			try
			{
				Bitmap bitmap = new Bitmap(originalBitmap);
				bitmap.MakeTransparent();
				Bitmap value = DpiHelper.CreateResizedBitmap(bitmap, PropertyGrid.largeButtonSize);
				bitmap.Dispose();
				this.imageList[1].Images.Add(value);
			}
			catch (Exception ex)
			{
			}
		}

		/// <summary>Commits all pending changes to the <see cref="T:System.Windows.Forms.PropertyGrid" /> control. For a description of this member, see <see cref="M:System.Windows.Forms.ComponentModel.Com2Interop.IComPropertyBrowser.EnsurePendingChangesCommitted" />.</summary>
		/// <returns>
		///     <see langword="true" /> if all the <see cref="T:System.Windows.Forms.PropertyGrid" /> successfully commits changes; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003146 RID: 12614 RVA: 0x000E5698 File Offset: 0x000E3898
		bool IComPropertyBrowser.EnsurePendingChangesCommitted()
		{
			bool result;
			try
			{
				if (this.designerHost != null)
				{
					this.designerHost.TransactionOpened -= this.OnTransactionOpened;
					this.designerHost.TransactionClosed -= this.OnTransactionClosed;
				}
				result = this.GetPropertyGridView().EnsurePendingChangesCommitted();
			}
			finally
			{
				if (this.designerHost != null)
				{
					this.designerHost.TransactionOpened += this.OnTransactionOpened;
					this.designerHost.TransactionClosed += this.OnTransactionClosed;
				}
			}
			return result;
		}

		/// <summary>Expands all the categories in the <see cref="T:System.Windows.Forms.PropertyGrid" />.</summary>
		// Token: 0x06003147 RID: 12615 RVA: 0x000E5734 File Offset: 0x000E3934
		public void ExpandAllGridItems()
		{
			this.gridView.RecursivelyExpand(this.peMain, false, true, 10);
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x000E574C File Offset: 0x000E394C
		private static Type[] GetCommonTabs(object[] objs, PropertyTabScope tabScope)
		{
			if (objs == null || objs.Length == 0)
			{
				return new Type[0];
			}
			Type[] array = new Type[5];
			int num = 0;
			PropertyTabAttribute propertyTabAttribute = (PropertyTabAttribute)TypeDescriptor.GetAttributes(objs[0])[typeof(PropertyTabAttribute)];
			if (propertyTabAttribute == null)
			{
				return new Type[0];
			}
			int i;
			for (i = 0; i < propertyTabAttribute.TabScopes.Length; i++)
			{
				PropertyTabScope propertyTabScope = propertyTabAttribute.TabScopes[i];
				if (propertyTabScope == tabScope)
				{
					if (num == array.Length)
					{
						Type[] array2 = new Type[num * 2];
						Array.Copy(array, 0, array2, 0, num);
						array = array2;
					}
					array[num++] = propertyTabAttribute.TabClasses[i];
				}
			}
			if (num == 0)
			{
				return new Type[0];
			}
			i = 1;
			while (i < objs.Length && num > 0)
			{
				propertyTabAttribute = (PropertyTabAttribute)TypeDescriptor.GetAttributes(objs[i])[typeof(PropertyTabAttribute)];
				if (propertyTabAttribute == null)
				{
					return new Type[0];
				}
				for (int j = 0; j < num; j++)
				{
					bool flag = false;
					for (int k = 0; k < propertyTabAttribute.TabClasses.Length; k++)
					{
						if (propertyTabAttribute.TabClasses[k] == array[j])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						array[j] = array[num - 1];
						array[num - 1] = null;
						num--;
						j--;
					}
				}
				i++;
			}
			Type[] array3 = new Type[num];
			if (num > 0)
			{
				Array.Copy(array, 0, array3, 0, num);
			}
			return array3;
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x000E58A9 File Offset: 0x000E3AA9
		internal GridEntry GetDefaultGridEntry()
		{
			if (this.peDefault == null && this.currentPropEntries != null)
			{
				this.peDefault = (GridEntry)this.currentPropEntries[0];
			}
			return this.peDefault;
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x000E58D8 File Offset: 0x000E3AD8
		internal Control GetElementFromPoint(Point point)
		{
			if (this.ToolbarAccessibleObject.Bounds.Contains(point))
			{
				return this.toolStrip;
			}
			if (this.GridViewAccessibleObject.Bounds.Contains(point))
			{
				return this.gridView;
			}
			if (this.HotCommandsAccessibleObject.Bounds.Contains(point))
			{
				return this.hotcommands;
			}
			if (this.HelpAccessibleObject.Bounds.Contains(point))
			{
				return this.doccomment;
			}
			return null;
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x000E595C File Offset: 0x000E3B5C
		private object GetUnwrappedObject(int index)
		{
			if (this.currentObjects == null || index < 0 || index > this.currentObjects.Length)
			{
				return null;
			}
			object obj = this.currentObjects[index];
			if (obj is ICustomTypeDescriptor)
			{
				obj = ((ICustomTypeDescriptor)obj).GetPropertyOwner(null);
			}
			return obj;
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x000E59A1 File Offset: 0x000E3BA1
		internal GridEntryCollection GetPropEntries()
		{
			if (this.currentPropEntries == null)
			{
				this.UpdateSelection();
			}
			this.SetFlag(1, false);
			return this.currentPropEntries;
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x000E59BF File Offset: 0x000E3BBF
		private PropertyGridView GetPropertyGridView()
		{
			return this.gridView;
		}

		/// <summary>Activates the <see cref="T:System.Windows.Forms.PropertyGrid" /> control when the user chooses properties for a control in Design view. For a description of this member, see <see cref="M:System.Windows.Forms.ComponentModel.Com2Interop.IComPropertyBrowser.HandleF4" />.</summary>
		// Token: 0x0600314E RID: 12622 RVA: 0x000E59C7 File Offset: 0x000E3BC7
		void IComPropertyBrowser.HandleF4()
		{
			if (this.gridView.ContainsFocus)
			{
				return;
			}
			if (base.ActiveControl != this.gridView)
			{
				base.SetActiveControlInternal(this.gridView);
			}
			this.gridView.FocusInternal();
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x000E59FD File Offset: 0x000E3BFD
		internal bool HavePropEntriesChanged()
		{
			return this.GetFlag(1);
		}

		/// <summary>Loads user states from the registry into the <see cref="T:System.Windows.Forms.PropertyGrid" /> control. For a description of this member, see <see cref="M:System.Windows.Forms.ComponentModel.Com2Interop.IComPropertyBrowser.LoadState(Microsoft.Win32.RegistryKey)" />.</summary>
		/// <param name="optRoot">The registry key that contains the user states.</param>
		// Token: 0x06003150 RID: 12624 RVA: 0x000E5A08 File Offset: 0x000E3C08
		void IComPropertyBrowser.LoadState(RegistryKey optRoot)
		{
			if (optRoot != null)
			{
				object value = optRoot.GetValue("PbrsAlpha", "0");
				if (value != null && value.ToString().Equals("1"))
				{
					this.PropertySort = PropertySort.Alphabetical;
				}
				else
				{
					this.PropertySort = PropertySort.CategorizedAlphabetical;
				}
				value = optRoot.GetValue("PbrsShowDesc", "1");
				this.HelpVisible = (value != null && value.ToString().Equals("1"));
				value = optRoot.GetValue("PbrsShowCommands", "0");
				this.CommandsVisibleIfAvailable = (value != null && value.ToString().Equals("1"));
				value = optRoot.GetValue("PbrsDescHeightRatio", "-1");
				bool flag = false;
				if (value is string)
				{
					int num = int.Parse((string)value, CultureInfo.InvariantCulture);
					if (num > 0)
					{
						this.dcSizeRatio = num;
						flag = true;
					}
				}
				value = optRoot.GetValue("PbrsHotCommandHeightRatio", "-1");
				if (value is string)
				{
					int num2 = int.Parse((string)value, CultureInfo.InvariantCulture);
					if (num2 > 0)
					{
						this.dcSizeRatio = num2;
						flag = true;
					}
				}
				if (flag)
				{
					this.OnLayoutInternal(false);
					return;
				}
			}
			else
			{
				this.PropertySort = PropertySort.CategorizedAlphabetical;
				this.HelpVisible = true;
				this.CommandsVisibleIfAvailable = false;
			}
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x000E5B3C File Offset: 0x000E3D3C
		private void OnActiveDesignerChanged(object sender, ActiveDesignerEventArgs e)
		{
			if (e.OldDesigner != null && e.OldDesigner == this.designerHost)
			{
				this.ActiveDesigner = null;
			}
			if (e.NewDesigner != null && e.NewDesigner != this.designerHost)
			{
				this.ActiveDesigner = e.NewDesigner;
			}
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x000E5B88 File Offset: 0x000E3D88
		void UnsafeNativeMethods.IPropertyNotifySink.OnChanged(int dispID)
		{
			bool flag = false;
			PropertyDescriptorGridEntry propertyDescriptorGridEntry = this.gridView.SelectedGridEntry as PropertyDescriptorGridEntry;
			if (propertyDescriptorGridEntry != null && propertyDescriptorGridEntry.PropertyDescriptor != null && propertyDescriptorGridEntry.PropertyDescriptor.Attributes != null)
			{
				DispIdAttribute dispIdAttribute = (DispIdAttribute)propertyDescriptorGridEntry.PropertyDescriptor.Attributes[typeof(DispIdAttribute)];
				if (dispIdAttribute != null && !dispIdAttribute.IsDefaultAttribute())
				{
					flag = (dispID != dispIdAttribute.Value);
				}
			}
			if (!this.GetFlag(512))
			{
				if (!this.gridView.GetInPropertySet() || flag)
				{
					this.Refresh(flag);
				}
				object unwrappedObject = this.GetUnwrappedObject(0);
				if (ComNativeDescriptor.Instance.IsNameDispId(unwrappedObject, dispID) || dispID == -800)
				{
					this.OnComComponentNameChanged(new ComponentRenameEventArgs(unwrappedObject, null, TypeDescriptor.GetClassName(unwrappedObject)));
				}
			}
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x000E5C50 File Offset: 0x000E3E50
		private void OnChildMouseMove(object sender, MouseEventArgs me)
		{
			Point empty = Point.Empty;
			if (this.ShouldForwardChildMouseMessage((Control)sender, me, ref empty))
			{
				this.OnMouseMove(new MouseEventArgs(me.Button, me.Clicks, empty.X, empty.Y, me.Delta));
				return;
			}
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x000E5CA0 File Offset: 0x000E3EA0
		private void OnChildMouseDown(object sender, MouseEventArgs me)
		{
			Point empty = Point.Empty;
			if (this.ShouldForwardChildMouseMessage((Control)sender, me, ref empty))
			{
				this.OnMouseDown(new MouseEventArgs(me.Button, me.Clicks, empty.X, empty.Y, me.Delta));
				return;
			}
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x000E5CF0 File Offset: 0x000E3EF0
		private void OnComponentAdd(object sender, ComponentEventArgs e)
		{
			PropertyTabAttribute propertyTabAttribute = (PropertyTabAttribute)TypeDescriptor.GetAttributes(e.Component.GetType())[typeof(PropertyTabAttribute)];
			if (propertyTabAttribute == null)
			{
				return;
			}
			for (int i = 0; i < propertyTabAttribute.TabClasses.Length; i++)
			{
				if (propertyTabAttribute.TabScopes[i] == PropertyTabScope.Document)
				{
					this.AddRefTab(propertyTabAttribute.TabClasses[i], e.Component, PropertyTabScope.Document, true);
				}
			}
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x000E5D5C File Offset: 0x000E3F5C
		private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			bool flag = this.GetFlag(16);
			if (flag || this.GetFlag(4) || this.gridView.GetInPropertySet() || this.currentObjects == null || this.currentObjects.Length == 0)
			{
				if (flag && !this.gridView.GetInPropertySet())
				{
					this.SetFlag(256, true);
				}
				return;
			}
			int num = this.currentObjects.Length;
			for (int i = 0; i < num; i++)
			{
				if (this.currentObjects[i] == e.Component)
				{
					this.Refresh(false);
					return;
				}
			}
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x000E5DE8 File Offset: 0x000E3FE8
		private void OnComponentRemove(object sender, ComponentEventArgs e)
		{
			PropertyTabAttribute propertyTabAttribute = (PropertyTabAttribute)TypeDescriptor.GetAttributes(e.Component.GetType())[typeof(PropertyTabAttribute)];
			if (propertyTabAttribute == null)
			{
				return;
			}
			for (int i = 0; i < propertyTabAttribute.TabClasses.Length; i++)
			{
				if (propertyTabAttribute.TabScopes[i] == PropertyTabScope.Document)
				{
					this.ReleaseTab(propertyTabAttribute.TabClasses[i], e.Component);
				}
			}
			for (int j = 0; j < this.currentObjects.Length; j++)
			{
				if (e.Component == this.currentObjects[j])
				{
					object[] array = new object[this.currentObjects.Length - 1];
					Array.Copy(this.currentObjects, 0, array, 0, j);
					if (j < array.Length)
					{
						Array.Copy(this.currentObjects, j + 1, array, j, array.Length - j);
					}
					if (!this.GetFlag(16))
					{
						this.SelectedObjects = array;
					}
					else
					{
						this.gridView.ClearProps();
						this.currentObjects = array;
						this.SetFlag(128, true);
					}
				}
			}
			this.SetupToolbar();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003158 RID: 12632 RVA: 0x000E5EE7 File Offset: 0x000E40E7
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			this.Refresh();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003159 RID: 12633 RVA: 0x000E5EF6 File Offset: 0x000E40F6
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.Refresh();
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x0003FCD6 File Offset: 0x0003DED6
		internal void OnGridViewMouseWheel(MouseEventArgs e)
		{
			this.OnMouseWheel(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600315B RID: 12635 RVA: 0x000E5F05 File Offset: 0x000E4105
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.OnLayoutInternal(false);
			TypeDescriptor.Refreshed += this.OnTypeDescriptorRefreshed;
			if (this.currentObjects != null && this.currentObjects.Length != 0)
			{
				this.Refresh(true);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600315C RID: 12636 RVA: 0x000E5F3E File Offset: 0x000E413E
		protected override void OnHandleDestroyed(EventArgs e)
		{
			TypeDescriptor.Refreshed -= this.OnTypeDescriptorRefreshed;
			base.OnHandleDestroyed(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600315D RID: 12637 RVA: 0x000E5F58 File Offset: 0x000E4158
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if (base.ActiveControl == null)
			{
				base.SetActiveControlInternal(this.gridView);
				return;
			}
			if (!base.ActiveControl.FocusInternal())
			{
				base.SetActiveControlInternal(this.gridView);
			}
		}

		/// <summary>This method is not relevant for this class.</summary>
		/// <param name="dx">The horizontal scaling factor.</param>
		/// <param name="dy">The vertical scaling factor.</param>
		// Token: 0x0600315E RID: 12638 RVA: 0x000E5F90 File Offset: 0x000E4190
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void ScaleCore(float dx, float dy)
		{
			int num = (int)Math.Round((double)((float)base.Left * dx));
			int num2 = (int)Math.Round((double)((float)base.Top * dy));
			int width = base.Width;
			width = (int)Math.Round((double)((float)(base.Left + base.Width) * dx - (float)num));
			int height = base.Height;
			height = (int)Math.Round((double)((float)(base.Top + base.Height) * dy - (float)num2));
			base.SetBounds(num, num2, width, height, BoundsSpecified.All);
		}

		// Token: 0x0600315F RID: 12639 RVA: 0x000E6010 File Offset: 0x000E4210
		private void OnLayoutInternal(bool dividerOnly)
		{
			if (!base.IsHandleCreated || !base.Visible)
			{
				return;
			}
			try
			{
				this.FreezePainting = true;
				if (!dividerOnly)
				{
					if (!this.toolStrip.Visible && !this.doccomment.Visible && !this.hotcommands.Visible)
					{
						this.gridView.Location = new Point(0, 0);
						this.gridView.Size = base.Size;
						return;
					}
					if (this.toolStrip.Visible)
					{
						int width = base.Width;
						int height = (this.LargeButtons ? PropertyGrid.largeButtonSize : PropertyGrid.normalButtonSize).Height + this.toolStripButtonPaddingY;
						Rectangle bounds = new Rectangle(0, 1, width, height);
						this.toolStrip.Bounds = bounds;
						int y = this.gridView.Location.Y;
						this.gridView.Location = new Point(0, this.toolStrip.Height + this.toolStrip.Top);
					}
					else
					{
						this.gridView.Location = new Point(0, 0);
					}
				}
				int num = base.Size.Height;
				if (num >= 20)
				{
					int num2 = num - (this.gridView.Location.Y + 20);
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					if (dividerOnly)
					{
						num3 = (this.doccomment.Visible ? this.doccomment.Size.Height : 0);
						num4 = (this.hotcommands.Visible ? this.hotcommands.Size.Height : 0);
					}
					else
					{
						if (this.doccomment.Visible)
						{
							num5 = this.doccomment.GetOptimalHeight(base.Size.Width - PropertyGrid.cyDivider);
							if (this.doccomment.userSized)
							{
								num3 = this.doccomment.Size.Height;
							}
							else if (this.dcSizeRatio != -1)
							{
								num3 = base.Height * this.dcSizeRatio / 100;
							}
							else
							{
								num3 = num5;
							}
						}
						if (this.hotcommands.Visible)
						{
							num6 = this.hotcommands.GetOptimalHeight(base.Size.Width - PropertyGrid.cyDivider);
							if (this.hotcommands.userSized)
							{
								num4 = this.hotcommands.Size.Height;
							}
							else if (this.hcSizeRatio != -1)
							{
								num4 = base.Height * this.hcSizeRatio / 100;
							}
							else
							{
								num4 = num6;
							}
						}
					}
					if (num3 > 0)
					{
						num2 -= PropertyGrid.cyDivider;
						int num7;
						if (num4 == 0 || num3 + num4 < num2)
						{
							num7 = Math.Min(num3, num2);
						}
						else if (num4 > 0 && num4 < num2)
						{
							num7 = num2 - num4;
						}
						else
						{
							num7 = Math.Min(num3, num2 / 2 - 1);
						}
						num7 = Math.Max(num7, PropertyGrid.cyDivider * 2);
						this.doccomment.SetBounds(0, num - num7, base.Size.Width, num7);
						if (num7 <= num5 && num7 < num3)
						{
							this.doccomment.userSized = false;
						}
						else if (this.dcSizeRatio != -1 || this.doccomment.userSized)
						{
							this.dcSizeRatio = this.doccomment.Height * 100 / base.Height;
						}
						this.doccomment.Invalidate();
						num = this.doccomment.Location.Y - PropertyGrid.cyDivider;
						num2 -= num7;
					}
					if (num4 > 0)
					{
						num2 -= PropertyGrid.cyDivider;
						int num7;
						if (num2 > num4)
						{
							num7 = Math.Min(num4, num2);
						}
						else
						{
							num7 = num2;
						}
						num7 = Math.Max(num7, PropertyGrid.cyDivider * 2);
						if (num7 <= num6 && num7 < num4)
						{
							this.hotcommands.userSized = false;
						}
						else if (this.hcSizeRatio != -1 || this.hotcommands.userSized)
						{
							this.hcSizeRatio = this.hotcommands.Height * 100 / base.Height;
						}
						this.hotcommands.SetBounds(0, num - num7, base.Size.Width, num7);
						this.hotcommands.Invalidate();
						num = this.hotcommands.Location.Y - PropertyGrid.cyDivider;
					}
					this.gridView.Size = new Size(base.Size.Width, num - this.gridView.Location.Y);
				}
			}
			finally
			{
				this.FreezePainting = false;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.</summary>
		/// <param name="me">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06003160 RID: 12640 RVA: 0x000E64A8 File Offset: 0x000E46A8
		protected override void OnMouseDown(MouseEventArgs me)
		{
			PropertyGrid.SnappableControl snappableControl = this.DividerInside(me.X, me.Y);
			if (snappableControl != null && me.Button == MouseButtons.Left)
			{
				base.CaptureInternal = true;
				this.targetMove = snappableControl;
				this.dividerMoveY = me.Y;
				this.DividerDraw(this.dividerMoveY);
			}
			base.OnMouseDown(me);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.</summary>
		/// <param name="me">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06003161 RID: 12641 RVA: 0x000E6508 File Offset: 0x000E4708
		protected override void OnMouseMove(MouseEventArgs me)
		{
			if (this.dividerMoveY != -1)
			{
				int num = this.DividerLimitMove(this.targetMove, me.Y);
				if (num != this.dividerMoveY)
				{
					this.DividerDraw(this.dividerMoveY);
					this.dividerMoveY = num;
					this.DividerDraw(this.dividerMoveY);
				}
				base.OnMouseMove(me);
				return;
			}
			if (this.DividerInside(me.X, me.Y) != null)
			{
				this.Cursor = Cursors.HSplit;
				return;
			}
			this.Cursor = null;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.</summary>
		/// <param name="me">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06003162 RID: 12642 RVA: 0x000E6588 File Offset: 0x000E4788
		protected override void OnMouseUp(MouseEventArgs me)
		{
			if (this.dividerMoveY == -1)
			{
				return;
			}
			this.Cursor = null;
			this.DividerDraw(this.dividerMoveY);
			this.dividerMoveY = this.DividerLimitMove(this.targetMove, me.Y);
			Rectangle bounds = this.targetMove.Bounds;
			if (this.dividerMoveY != bounds.Y)
			{
				int val = bounds.Height + bounds.Y - this.dividerMoveY - PropertyGrid.cyDivider / 2;
				Size size = this.targetMove.Size;
				size.Height = Math.Max(0, val);
				this.targetMove.Size = size;
				this.targetMove.userSized = true;
				this.OnLayoutInternal(true);
				base.Invalidate(new Rectangle(0, me.Y - PropertyGrid.cyDivider, base.Size.Width, me.Y + PropertyGrid.cyDivider));
				this.gridView.Invalidate(new Rectangle(0, this.gridView.Size.Height - PropertyGrid.cyDivider, base.Size.Width, PropertyGrid.cyDivider));
			}
			base.CaptureInternal = false;
			this.dividerMoveY = -1;
			this.targetMove = null;
			base.OnMouseUp(me);
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		int UnsafeNativeMethods.IPropertyNotifySink.OnRequestEdit(int dispID)
		{
			return 0;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003164 RID: 12644 RVA: 0x000E66CB File Offset: 0x000E48CB
		protected override void OnResize(EventArgs e)
		{
			if (base.IsHandleCreated && base.Visible)
			{
				this.OnLayoutInternal(false);
			}
			base.OnResize(e);
		}

		// Token: 0x06003165 RID: 12645 RVA: 0x000E66EB File Offset: 0x000E48EB
		private void OnButtonClick(object sender, EventArgs e)
		{
			if (sender != this.btnViewPropertyPages)
			{
				this.gridView.FocusInternal();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ComponentModel.Com2Interop.IComPropertyBrowser.ComComponentNameChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.Design.ComponentRenameEventArgs" /> that contains the event data. </param>
		// Token: 0x06003166 RID: 12646 RVA: 0x000E6704 File Offset: 0x000E4904
		protected void OnComComponentNameChanged(ComponentRenameEventArgs e)
		{
			ComponentRenameEventHandler componentRenameEventHandler = (ComponentRenameEventHandler)base.Events[PropertyGrid.EventComComponentNameChanged];
			if (componentRenameEventHandler != null)
			{
				componentRenameEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="M:System.Drawing.Design.IPropertyValueUIService.NotifyPropertyValueUIItemsChanged" /> event.</summary>
		/// <param name="sender">The source of the event. </param>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003167 RID: 12647 RVA: 0x000E6732 File Offset: 0x000E4932
		protected void OnNotifyPropertyValueUIItemsChanged(object sender, EventArgs e)
		{
			this.gridView.LabelPaintMargin = 0;
			this.gridView.Invalidate(true);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
		/// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
		// Token: 0x06003168 RID: 12648 RVA: 0x000E674C File Offset: 0x000E494C
		protected override void OnPaint(PaintEventArgs pevent)
		{
			Point location = this.gridView.Location;
			int width = base.Size.Width;
			Brush brush;
			if (this.BackColor.IsSystemColor)
			{
				brush = SystemBrushes.FromSystemColor(this.BackColor);
			}
			else
			{
				brush = new SolidBrush(this.BackColor);
			}
			pevent.Graphics.FillRectangle(brush, new Rectangle(0, 0, width, location.Y));
			int num = location.Y + this.gridView.Size.Height;
			if (this.hotcommands.Visible)
			{
				pevent.Graphics.FillRectangle(brush, new Rectangle(0, num, width, this.hotcommands.Location.Y - num));
				num += this.hotcommands.Size.Height;
			}
			if (this.doccomment.Visible)
			{
				pevent.Graphics.FillRectangle(brush, new Rectangle(0, num, width, this.doccomment.Location.Y - num));
				num += this.doccomment.Size.Height;
			}
			pevent.Graphics.FillRectangle(brush, new Rectangle(0, num, width, base.Size.Height - num));
			if (!this.BackColor.IsSystemColor)
			{
				brush.Dispose();
			}
			base.OnPaint(pevent);
			if (this.lineBrush != null)
			{
				this.lineBrush.Dispose();
				this.lineBrush = null;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.PropertyGrid.PropertySortChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003169 RID: 12649 RVA: 0x000E68D4 File Offset: 0x000E4AD4
		protected virtual void OnPropertySortChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[PropertyGrid.EventPropertySortChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.PropertyGrid.PropertyTabChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PropertyTabChangedEventArgs" /> that contains the event data. </param>
		// Token: 0x0600316A RID: 12650 RVA: 0x000E6904 File Offset: 0x000E4B04
		protected virtual void OnPropertyTabChanged(PropertyTabChangedEventArgs e)
		{
			PropertyTabChangedEventHandler propertyTabChangedEventHandler = (PropertyTabChangedEventHandler)base.Events[PropertyGrid.EventPropertyTabChanged];
			if (propertyTabChangedEventHandler != null)
			{
				propertyTabChangedEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.PropertyGrid.PropertyValueChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PropertyValueChangedEventArgs" /> that contains the event data. </param>
		// Token: 0x0600316B RID: 12651 RVA: 0x000E6934 File Offset: 0x000E4B34
		protected virtual void OnPropertyValueChanged(PropertyValueChangedEventArgs e)
		{
			PropertyValueChangedEventHandler propertyValueChangedEventHandler = (PropertyValueChangedEventHandler)base.Events[PropertyGrid.EventPropertyValueChanged];
			if (propertyValueChangedEventHandler != null)
			{
				propertyValueChangedEventHandler(this, e);
			}
		}

		// Token: 0x0600316C RID: 12652 RVA: 0x000E6964 File Offset: 0x000E4B64
		internal void OnPropertyValueSet(GridItem changedItem, object oldValue)
		{
			this.OnPropertyValueChanged(new PropertyValueChangedEventArgs(changedItem, oldValue));
			if (AccessibilityImprovements.Level3 && changedItem != null)
			{
				bool flag = false;
				Type propertyType = changedItem.PropertyDescriptor.PropertyType;
				UITypeEditor uitypeEditor = (UITypeEditor)TypeDescriptor.GetEditor(propertyType, typeof(UITypeEditor));
				if (uitypeEditor != null)
				{
					flag = (uitypeEditor.GetEditStyle() == UITypeEditorEditStyle.DropDown);
				}
				else
				{
					GridEntry gridEntry = changedItem as GridEntry;
					if (gridEntry != null && gridEntry.Enumerable)
					{
						flag = true;
					}
				}
				if (flag && !this.gridView.DropDownVisible)
				{
					base.AccessibilityObject.RaiseAutomationNotification(AutomationNotificationKind.ActionCompleted, AutomationNotificationProcessing.All, SR.GetString("PropertyGridPropertyValueSelectedFormat", new object[]
					{
						changedItem.Value
					}));
				}
			}
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x000E6A09 File Offset: 0x000E4C09
		internal void OnSelectedGridItemChanged(GridEntry oldEntry, GridEntry newEntry)
		{
			this.OnSelectedGridItemChanged(new SelectedGridItemChangedEventArgs(oldEntry, newEntry));
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.PropertyGrid.SelectedGridItemChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.SelectedGridItemChangedEventArgs" /> that contains the event data. </param>
		// Token: 0x0600316E RID: 12654 RVA: 0x000E6A18 File Offset: 0x000E4C18
		protected virtual void OnSelectedGridItemChanged(SelectedGridItemChangedEventArgs e)
		{
			SelectedGridItemChangedEventHandler selectedGridItemChangedEventHandler = (SelectedGridItemChangedEventHandler)base.Events[PropertyGrid.EventSelectedGridItemChanged];
			if (selectedGridItemChangedEventHandler != null)
			{
				selectedGridItemChangedEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.PropertyGrid.SelectedObjectsChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600316F RID: 12655 RVA: 0x000E6A48 File Offset: 0x000E4C48
		protected virtual void OnSelectedObjectsChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[PropertyGrid.EventSelectedObjectsChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x000E6A78 File Offset: 0x000E4C78
		private void OnTransactionClosed(object sender, DesignerTransactionCloseEventArgs e)
		{
			if (e.LastTransaction)
			{
				IComponent component = this.SelectedObject as IComponent;
				if (component != null && component.Site == null)
				{
					this.SelectedObject = null;
					return;
				}
				this.SetFlag(16, false);
				if (this.GetFlag(128))
				{
					this.SelectedObjects = this.currentObjects;
					this.SetFlag(128, false);
				}
				else if (this.GetFlag(256))
				{
					this.Refresh(false);
				}
				this.SetFlag(256, false);
			}
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x000E6AFC File Offset: 0x000E4CFC
		private void OnTransactionOpened(object sender, EventArgs e)
		{
			this.SetFlag(16, true);
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x000E6B07 File Offset: 0x000E4D07
		private void OnTypeDescriptorRefreshed(RefreshEventArgs e)
		{
			if (base.InvokeRequired)
			{
				base.BeginInvoke(new RefreshEventHandler(this.OnTypeDescriptorRefreshedInvoke), new object[]
				{
					e
				});
				return;
			}
			this.OnTypeDescriptorRefreshedInvoke(e);
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x000E6B38 File Offset: 0x000E4D38
		private void OnTypeDescriptorRefreshedInvoke(RefreshEventArgs e)
		{
			if (this.currentObjects != null)
			{
				for (int i = 0; i < this.currentObjects.Length; i++)
				{
					Type typeChanged = e.TypeChanged;
					if (this.currentObjects[i] == e.ComponentChanged || (typeChanged != null && typeChanged.IsAssignableFrom(this.currentObjects[i].GetType())))
					{
						this.ClearCachedProps();
						this.Refresh(true);
						return;
					}
				}
			}
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x000E6BA4 File Offset: 0x000E4DA4
		private void OnViewSortButtonClick(object sender, EventArgs e)
		{
			try
			{
				this.FreezePainting = true;
				if (sender == this.viewSortButtons[this.selectedViewSort])
				{
					this.viewSortButtons[this.selectedViewSort].Checked = true;
					return;
				}
				this.viewSortButtons[this.selectedViewSort].Checked = false;
				int num = 0;
				while (num < this.viewSortButtons.Length && this.viewSortButtons[num] != sender)
				{
					num++;
				}
				this.selectedViewSort = num;
				this.viewSortButtons[this.selectedViewSort].Checked = true;
				switch (this.selectedViewSort)
				{
				case 0:
					this.propertySortValue = PropertySort.CategorizedAlphabetical;
					break;
				case 1:
					this.propertySortValue = PropertySort.Alphabetical;
					break;
				case 2:
					this.propertySortValue = PropertySort.NoSort;
					break;
				}
				this.OnPropertySortChanged(EventArgs.Empty);
				this.Refresh(false);
				this.OnLayoutInternal(false);
			}
			finally
			{
				this.FreezePainting = false;
			}
			this.OnButtonClick(sender, e);
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x000E6C9C File Offset: 0x000E4E9C
		private void OnViewTabButtonClick(object sender, EventArgs e)
		{
			try
			{
				this.FreezePainting = true;
				this.SelectViewTabButton((ToolStripButton)sender, true);
				this.OnLayoutInternal(false);
				this.SaveTabSelection();
			}
			finally
			{
				this.FreezePainting = false;
			}
			this.OnButtonClick(sender, e);
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x000E6CEC File Offset: 0x000E4EEC
		private void OnViewButtonClickPP(object sender, EventArgs e)
		{
			if (this.btnViewPropertyPages.Enabled && this.currentObjects != null && this.currentObjects.Length != 0)
			{
				object obj = this.currentObjects[0];
				object component = obj;
				bool flag = false;
				IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
				try
				{
					if (iuiservice != null)
					{
						flag = iuiservice.ShowComponentEditor(component, this);
					}
					else
					{
						try
						{
							ComponentEditor componentEditor = (ComponentEditor)TypeDescriptor.GetEditor(component, typeof(ComponentEditor));
							if (componentEditor != null)
							{
								if (componentEditor is WindowsFormsComponentEditor)
								{
									flag = ((WindowsFormsComponentEditor)componentEditor).EditComponent(null, component, this);
								}
								else
								{
									flag = componentEditor.EditComponent(component);
								}
							}
						}
						catch
						{
						}
					}
					if (flag)
					{
						if (obj is IComponent && this.connectionPointCookies[0] == null)
						{
							ISite site = ((IComponent)obj).Site;
							if (site != null)
							{
								IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
								if (componentChangeService != null)
								{
									try
									{
										componentChangeService.OnComponentChanging(obj, null);
									}
									catch (CheckoutException ex)
									{
										if (ex == CheckoutException.Canceled)
										{
											return;
										}
										throw ex;
									}
									try
									{
										this.SetFlag(4, true);
										componentChangeService.OnComponentChanged(obj, null, null, null);
									}
									finally
									{
										this.SetFlag(4, false);
									}
								}
							}
						}
						this.gridView.Refresh();
					}
				}
				catch (Exception ex2)
				{
					string @string = SR.GetString("ErrorPropertyPageFailed");
					if (iuiservice != null)
					{
						iuiservice.ShowError(ex2, @string);
					}
					else
					{
						RTLAwareMessageBox.Show(null, @string, SR.GetString("PropertyGridTitle"), MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
					}
				}
			}
			this.OnButtonClick(sender, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.VisibleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003177 RID: 12663 RVA: 0x000E6E98 File Offset: 0x000E5098
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (base.Visible && base.IsHandleCreated)
			{
				this.OnLayoutInternal(false);
				this.SetupToolbar();
			}
		}

		/// <summary>Processes a dialog key.</summary>
		/// <param name="keyData">Specifies key codes and modifiers.</param>
		/// <returns>
		///     <see langword="true" /> if the key was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003178 RID: 12664 RVA: 0x000E6EC0 File Offset: 0x000E50C0
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			Keys keys = keyData & Keys.KeyCode;
			if (keys != Keys.Tab || (keyData & Keys.Control) != Keys.None || (keyData & Keys.Alt) != Keys.None)
			{
				return base.ProcessDialogKey(keyData);
			}
			if ((keyData & Keys.Shift) != Keys.None)
			{
				if (this.hotcommands.Visible && this.hotcommands.ContainsFocus)
				{
					this.gridView.ReverseFocus();
				}
				else if (this.gridView.FocusInside)
				{
					if (!this.toolStrip.Visible)
					{
						return base.ProcessDialogKey(keyData);
					}
					this.toolStrip.FocusInternal();
					if (AccessibilityImprovements.Level1 && this.toolStrip.Items.Count > 0)
					{
						this.toolStrip.SelectNextToolStripItem(null, true);
					}
				}
				else
				{
					if (this.toolStrip.Focused || !this.toolStrip.Visible)
					{
						return base.ProcessDialogKey(keyData);
					}
					if (this.hotcommands.Visible)
					{
						this.hotcommands.Select(false);
					}
					else if (this.peMain != null)
					{
						this.gridView.ReverseFocus();
					}
					else
					{
						if (!this.toolStrip.Visible)
						{
							return base.ProcessDialogKey(keyData);
						}
						this.toolStrip.FocusInternal();
					}
				}
				return true;
			}
			bool flag = false;
			if (this.toolStrip.Focused)
			{
				if (this.peMain != null)
				{
					this.gridView.FocusInternal();
				}
				else
				{
					base.ProcessDialogKey(keyData);
				}
				return true;
			}
			if (this.gridView.FocusInside)
			{
				if (this.hotcommands.Visible)
				{
					this.hotcommands.Select(true);
					return true;
				}
				flag = true;
			}
			else if (this.hotcommands.ContainsFocus)
			{
				flag = true;
			}
			else if (this.toolStrip.Visible)
			{
				this.toolStrip.FocusInternal();
			}
			else
			{
				this.gridView.FocusInternal();
			}
			if (flag)
			{
				bool flag2 = base.ProcessDialogKey(keyData);
				if (!flag2 && base.Parent == null)
				{
					IntPtr parent = UnsafeNativeMethods.GetParent(new HandleRef(this, base.Handle));
					if (parent != IntPtr.Zero)
					{
						UnsafeNativeMethods.SetFocus(new HandleRef(null, parent));
					}
				}
				return flag2;
			}
			return true;
		}

		/// <summary>Forces the control to invalidate its client area and immediately redraw itself and any child controls.</summary>
		// Token: 0x06003179 RID: 12665 RVA: 0x000E70E0 File Offset: 0x000E52E0
		public override void Refresh()
		{
			if (this.GetFlag(512))
			{
				return;
			}
			this.Refresh(true);
			base.Refresh();
		}

		// Token: 0x0600317A RID: 12666 RVA: 0x000E7100 File Offset: 0x000E5300
		private void Refresh(bool clearCached)
		{
			if (base.Disposing)
			{
				return;
			}
			if (this.GetFlag(512))
			{
				return;
			}
			try
			{
				this.FreezePainting = true;
				this.SetFlag(512, true);
				if (clearCached)
				{
					this.ClearCachedProps();
				}
				this.RefreshProperties(clearCached);
				this.gridView.Refresh();
				this.DisplayHotCommands();
			}
			finally
			{
				this.FreezePainting = false;
				this.SetFlag(512, false);
			}
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x000E7180 File Offset: 0x000E5380
		internal void RefreshProperties(bool clearCached)
		{
			if (clearCached && this.selectedViewTab != -1 && this.viewTabs != null)
			{
				PropertyTab propertyTab = this.viewTabs[this.selectedViewTab];
				if (propertyTab != null && this.viewTabProps != null)
				{
					string key = propertyTab.TabName + this.propertySortValue.ToString();
					this.viewTabProps.Remove(key);
				}
			}
			this.SetFlag(1, true);
			this.UpdateSelection();
		}

		/// <summary>Refreshes the property tabs of the specified scope.</summary>
		/// <param name="tabScope">Either the <see langword="Component" /> or <see langword="Document" /> value of <see cref="T:System.ComponentModel.PropertyTabScope" />. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tabScope" /> parameter is not the <see langword="Component" /> or <see langword="Document" /> value of <see cref="T:System.ComponentModel.PropertyTabScope" />. </exception>
		// Token: 0x0600317C RID: 12668 RVA: 0x000E71F4 File Offset: 0x000E53F4
		public void RefreshTabs(PropertyTabScope tabScope)
		{
			if (tabScope < PropertyTabScope.Document)
			{
				throw new ArgumentException(SR.GetString("PropertyGridTabScope"));
			}
			this.RemoveTabs(tabScope, false);
			if (tabScope <= PropertyTabScope.Component && this.currentObjects != null && this.currentObjects.Length != 0)
			{
				Type[] commonTabs = PropertyGrid.GetCommonTabs(this.currentObjects, PropertyTabScope.Component);
				for (int i = 0; i < commonTabs.Length; i++)
				{
					for (int j = 0; j < this.currentObjects.Length; j++)
					{
						this.AddRefTab(commonTabs[i], this.currentObjects[j], PropertyTabScope.Component, false);
					}
				}
			}
			if (tabScope <= PropertyTabScope.Document && this.designerHost != null)
			{
				IContainer container = this.designerHost.Container;
				if (container != null)
				{
					ComponentCollection components = container.Components;
					if (components != null)
					{
						foreach (object obj in components)
						{
							IComponent component = (IComponent)obj;
							PropertyTabAttribute propertyTabAttribute = (PropertyTabAttribute)TypeDescriptor.GetAttributes(component.GetType())[typeof(PropertyTabAttribute)];
							if (propertyTabAttribute != null)
							{
								for (int k = 0; k < propertyTabAttribute.TabClasses.Length; k++)
								{
									if (propertyTabAttribute.TabScopes[k] == PropertyTabScope.Document)
									{
										this.AddRefTab(propertyTabAttribute.TabClasses[k], component, PropertyTabScope.Document, false);
									}
								}
							}
						}
					}
				}
			}
			this.SetupToolbar();
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x000E7354 File Offset: 0x000E5554
		internal void ReleaseTab(Type tabType, object component)
		{
			PropertyTab propertyTab = null;
			int num = -1;
			for (int i = 0; i < this.viewTabs.Length; i++)
			{
				if (tabType == this.viewTabs[i].GetType())
				{
					propertyTab = this.viewTabs[i];
					num = i;
					break;
				}
			}
			if (propertyTab == null)
			{
				return;
			}
			object[] array = propertyTab.Components;
			bool flag = false;
			try
			{
				int num2 = -1;
				if (array != null)
				{
					num2 = Array.IndexOf<object>(array, component);
				}
				if (num2 >= 0)
				{
					object[] array2 = new object[array.Length - 1];
					Array.Copy(array, 0, array2, 0, num2);
					Array.Copy(array, num2 + 1, array2, num2, array.Length - num2 - 1);
					array = array2;
					propertyTab.Components = array;
				}
				flag = (array.Length == 0);
			}
			catch (Exception ex)
			{
				flag = true;
			}
			if (flag && this.viewTabScopes[num] > PropertyTabScope.Global)
			{
				this.RemoveTab(num, false);
			}
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x000E7430 File Offset: 0x000E5630
		private void RemoveImage(int index)
		{
			this.imageList[0].Images.RemoveAt(index);
			if (this.imageList[1] != null)
			{
				this.imageList[1].Images.RemoveAt(index);
			}
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x000E7464 File Offset: 0x000E5664
		internal void RemoveTabs(PropertyTabScope classification, bool setupToolbar)
		{
			if (classification == PropertyTabScope.Static)
			{
				throw new ArgumentException(SR.GetString("PropertyGridRemoveStaticTabs"));
			}
			if (this.viewTabButtons == null || this.viewTabs == null || this.viewTabScopes == null)
			{
				return;
			}
			ToolStripButton button = (this.selectedViewTab >= 0 && this.selectedViewTab < this.viewTabButtons.Length) ? this.viewTabButtons[this.selectedViewTab] : null;
			for (int i = this.viewTabs.Length - 1; i >= 0; i--)
			{
				if (this.viewTabScopes[i] >= classification)
				{
					if (this.selectedViewTab == i)
					{
						this.selectedViewTab = -1;
					}
					else if (this.selectedViewTab > i)
					{
						this.selectedViewTab--;
					}
					PropertyTab[] destinationArray = new PropertyTab[this.viewTabs.Length - 1];
					Array.Copy(this.viewTabs, 0, destinationArray, 0, i);
					Array.Copy(this.viewTabs, i + 1, destinationArray, i, this.viewTabs.Length - i - 1);
					this.viewTabs = destinationArray;
					PropertyTabScope[] destinationArray2 = new PropertyTabScope[this.viewTabScopes.Length - 1];
					Array.Copy(this.viewTabScopes, 0, destinationArray2, 0, i);
					Array.Copy(this.viewTabScopes, i + 1, destinationArray2, i, this.viewTabScopes.Length - i - 1);
					this.viewTabScopes = destinationArray2;
					this.viewTabsDirty = true;
				}
			}
			if (setupToolbar && this.viewTabsDirty)
			{
				this.SetupToolbar();
				this.selectedViewTab = -1;
				this.SelectViewTabButtonDefault(button);
				for (int j = 0; j < this.viewTabs.Length; j++)
				{
					this.viewTabs[j].Components = new object[0];
				}
			}
		}

		// Token: 0x06003180 RID: 12672 RVA: 0x000E75F0 File Offset: 0x000E57F0
		internal void RemoveTab(int tabIndex, bool setupToolbar)
		{
			if (tabIndex >= this.viewTabs.Length || tabIndex < 0)
			{
				throw new ArgumentOutOfRangeException("tabIndex", SR.GetString("PropertyGridBadTabIndex"));
			}
			if (this.viewTabScopes[tabIndex] == PropertyTabScope.Static)
			{
				throw new ArgumentException(SR.GetString("PropertyGridRemoveStaticTabs"));
			}
			if (this.selectedViewTab == tabIndex)
			{
				this.selectedViewTab = 0;
			}
			if (!this.GetFlag(32) && this.ActiveDesigner != null)
			{
				int hashCode = this.ActiveDesigner.GetHashCode();
				if (this.designerSelections != null && this.designerSelections.ContainsKey(hashCode) && (int)this.designerSelections[hashCode] == tabIndex)
				{
					this.designerSelections.Remove(hashCode);
				}
			}
			ToolStripButton button = this.viewTabButtons[this.selectedViewTab];
			PropertyTab[] destinationArray = new PropertyTab[this.viewTabs.Length - 1];
			Array.Copy(this.viewTabs, 0, destinationArray, 0, tabIndex);
			Array.Copy(this.viewTabs, tabIndex + 1, destinationArray, tabIndex, this.viewTabs.Length - tabIndex - 1);
			this.viewTabs = destinationArray;
			PropertyTabScope[] destinationArray2 = new PropertyTabScope[this.viewTabScopes.Length - 1];
			Array.Copy(this.viewTabScopes, 0, destinationArray2, 0, tabIndex);
			Array.Copy(this.viewTabScopes, tabIndex + 1, destinationArray2, tabIndex, this.viewTabScopes.Length - tabIndex - 1);
			this.viewTabScopes = destinationArray2;
			this.viewTabsDirty = true;
			if (setupToolbar)
			{
				this.SetupToolbar();
				this.selectedViewTab = -1;
				this.SelectViewTabButtonDefault(button);
			}
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x000E7760 File Offset: 0x000E5960
		internal void RemoveTab(Type tabType)
		{
			int num = -1;
			for (int i = 0; i < this.viewTabs.Length; i++)
			{
				if (tabType == this.viewTabs[i].GetType())
				{
					PropertyTab propertyTab = this.viewTabs[i];
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				return;
			}
			PropertyTab[] destinationArray = new PropertyTab[this.viewTabs.Length - 1];
			Array.Copy(this.viewTabs, 0, destinationArray, 0, num);
			Array.Copy(this.viewTabs, num + 1, destinationArray, num, this.viewTabs.Length - num - 1);
			this.viewTabs = destinationArray;
			PropertyTabScope[] destinationArray2 = new PropertyTabScope[this.viewTabScopes.Length - 1];
			Array.Copy(this.viewTabScopes, 0, destinationArray2, 0, num);
			Array.Copy(this.viewTabScopes, num + 1, destinationArray2, num, this.viewTabScopes.Length - num - 1);
			this.viewTabScopes = destinationArray2;
			this.viewTabsDirty = true;
			this.SetupToolbar();
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x000E7843 File Offset: 0x000E5A43
		private void ResetCommandsBackColor()
		{
			this.hotcommands.ResetBackColor();
		}

		// Token: 0x06003183 RID: 12675 RVA: 0x000E7850 File Offset: 0x000E5A50
		private void ResetCommandsForeColor()
		{
			this.hotcommands.ResetForeColor();
		}

		// Token: 0x06003184 RID: 12676 RVA: 0x000E785D File Offset: 0x000E5A5D
		private void ResetCommandsLinkColor()
		{
			this.hotcommands.Label.ResetLinkColor();
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x000E786F File Offset: 0x000E5A6F
		private void ResetCommandsActiveLinkColor()
		{
			this.hotcommands.Label.ResetActiveLinkColor();
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x000E7881 File Offset: 0x000E5A81
		private void ResetCommandsDisabledLinkColor()
		{
			this.hotcommands.Label.ResetDisabledLinkColor();
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x000E7893 File Offset: 0x000E5A93
		private void ResetHelpBackColor()
		{
			this.doccomment.ResetBackColor();
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x000E7893 File Offset: 0x000E5A93
		private void ResetHelpForeColor()
		{
			this.doccomment.ResetBackColor();
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x000E78A0 File Offset: 0x000E5AA0
		internal void ReplaceSelectedObject(object oldObject, object newObject)
		{
			for (int i = 0; i < this.currentObjects.Length; i++)
			{
				if (this.currentObjects[i] == oldObject)
				{
					this.currentObjects[i] = newObject;
					this.Refresh(true);
					return;
				}
			}
		}

		/// <summary>Resets the selected property to its default value.</summary>
		// Token: 0x0600318A RID: 12682 RVA: 0x000E78DC File Offset: 0x000E5ADC
		public void ResetSelectedProperty()
		{
			this.GetPropertyGridView().Reset();
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x000E78EC File Offset: 0x000E5AEC
		private void SaveTabSelection()
		{
			if (this.designerHost != null)
			{
				if (this.designerSelections == null)
				{
					this.designerSelections = new Hashtable();
				}
				this.designerSelections[this.designerHost.GetHashCode()] = this.selectedViewTab;
			}
		}

		/// <summary>Saves user states from the <see cref="T:System.Windows.Forms.PropertyGrid" /> control to the registry. For a description of this member, see <see cref="M:System.Windows.Forms.ComponentModel.Com2Interop.IComPropertyBrowser.SaveState(Microsoft.Win32.RegistryKey)" />.</summary>
		/// <param name="optRoot">The registry key that contains the user states.</param>
		// Token: 0x0600318C RID: 12684 RVA: 0x000E793C File Offset: 0x000E5B3C
		void IComPropertyBrowser.SaveState(RegistryKey optRoot)
		{
			if (optRoot == null)
			{
				return;
			}
			optRoot.SetValue("PbrsAlpha", (this.PropertySort == PropertySort.Alphabetical) ? "1" : "0");
			optRoot.SetValue("PbrsShowDesc", this.HelpVisible ? "1" : "0");
			optRoot.SetValue("PbrsShowCommands", this.CommandsVisibleIfAvailable ? "1" : "0");
			optRoot.SetValue("PbrsDescHeightRatio", this.dcSizeRatio.ToString(CultureInfo.InvariantCulture));
			optRoot.SetValue("PbrsHotCommandHeightRatio", this.hcSizeRatio.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x000E79E4 File Offset: 0x000E5BE4
		private void SetHotCommandColors(bool vscompat)
		{
			if (vscompat)
			{
				this.hotcommands.SetColors(SystemColors.Control, SystemColors.ControlText, SystemColors.ActiveCaption, SystemColors.ActiveCaption, SystemColors.ActiveCaption, SystemColors.ControlDark);
				return;
			}
			this.hotcommands.SetColors(SystemColors.Control, SystemColors.ControlText, Color.Empty, Color.Empty, Color.Empty, Color.Empty);
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x000E7A47 File Offset: 0x000E5C47
		internal void SetStatusBox(string title, string desc)
		{
			this.doccomment.SetComment(title, desc);
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x000E7A58 File Offset: 0x000E5C58
		private void SelectViewTabButton(ToolStripButton button, bool updateSelection)
		{
			int num = this.selectedViewTab;
			this.SelectViewTabButtonDefault(button);
			if (updateSelection)
			{
				this.Refresh(false);
			}
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x000E7A80 File Offset: 0x000E5C80
		private bool SelectViewTabButtonDefault(ToolStripButton button)
		{
			if (this.selectedViewTab >= 0 && this.selectedViewTab >= this.viewTabButtons.Length)
			{
				this.selectedViewTab = -1;
			}
			if (this.selectedViewTab >= 0 && this.selectedViewTab < this.viewTabButtons.Length && button == this.viewTabButtons[this.selectedViewTab])
			{
				this.viewTabButtons[this.selectedViewTab].Checked = true;
				return true;
			}
			PropertyTab oldTab = null;
			if (this.selectedViewTab != -1)
			{
				this.viewTabButtons[this.selectedViewTab].Checked = false;
				oldTab = this.viewTabs[this.selectedViewTab];
			}
			for (int i = 0; i < this.viewTabButtons.Length; i++)
			{
				if (this.viewTabButtons[i] == button)
				{
					this.selectedViewTab = i;
					this.viewTabButtons[i].Checked = true;
					try
					{
						this.SetFlag(8, true);
						this.OnPropertyTabChanged(new PropertyTabChangedEventArgs(oldTab, this.viewTabs[i]));
					}
					finally
					{
						this.SetFlag(8, false);
					}
					return true;
				}
			}
			this.selectedViewTab = 0;
			this.SelectViewTabButton(this.viewTabButtons[0], false);
			return false;
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x000E7B9C File Offset: 0x000E5D9C
		private void SetSelectState(int state)
		{
			if (state >= this.viewTabs.Length * this.viewSortButtons.Length)
			{
				state = 0;
			}
			else if (state < 0)
			{
				state = this.viewTabs.Length * this.viewSortButtons.Length - 1;
			}
			int num = this.viewSortButtons.Length;
			if (num > 0)
			{
				int num2 = state / num;
				int num3 = state % num;
				this.OnViewTabButtonClick(this.viewTabButtons[num2], EventArgs.Empty);
				this.OnViewSortButtonClick(this.viewSortButtons[num3], EventArgs.Empty);
			}
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x000E7C18 File Offset: 0x000E5E18
		private void SetToolStripRenderer()
		{
			if (this.DrawFlatToolbar || (SystemInformation.HighContrast && AccessibilityImprovements.Level1))
			{
				this.ToolStripRenderer = new ToolStripProfessionalRenderer(new ProfessionalColorTable
				{
					UseSystemColors = true
				});
				return;
			}
			this.ToolStripRenderer = new ToolStripSystemRenderer();
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x000E7C60 File Offset: 0x000E5E60
		private void SetupToolbar()
		{
			this.SetupToolbar(false);
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x000E7C6C File Offset: 0x000E5E6C
		private void SetupToolbar(bool fullRebuild)
		{
			if (!this.viewTabsDirty && !fullRebuild)
			{
				return;
			}
			try
			{
				this.FreezePainting = true;
				if (this.imageList[0] == null || fullRebuild)
				{
					this.imageList[0] = new ImageList();
					if (DpiHelper.IsScalingRequired)
					{
						this.imageList[0].ImageSize = PropertyGrid.normalButtonSize;
					}
				}
				EventHandler eventHandler = new EventHandler(this.OnViewTabButtonClick);
				EventHandler eventHandler2 = new EventHandler(this.OnViewSortButtonClick);
				EventHandler eventHandler3 = new EventHandler(this.OnViewButtonClickPP);
				ArrayList arrayList;
				if (fullRebuild)
				{
					arrayList = new ArrayList();
				}
				else
				{
					arrayList = new ArrayList(this.toolStrip.Items);
				}
				if (this.viewSortButtons == null || fullRebuild)
				{
					this.viewSortButtons = new ToolStripButton[3];
					int imageIndex = -1;
					int imageIndex2 = -1;
					try
					{
						if (this.bmpAlpha == null)
						{
							this.bmpAlpha = this.SortByPropertyImage;
						}
						imageIndex = this.AddImage(this.bmpAlpha);
					}
					catch (Exception ex)
					{
					}
					try
					{
						if (this.bmpCategory == null)
						{
							this.bmpCategory = this.SortByCategoryImage;
						}
						imageIndex2 = this.AddImage(this.bmpCategory);
					}
					catch (Exception ex2)
					{
					}
					this.viewSortButtons[1] = this.CreatePushButton(SR.GetString("PBRSToolTipAlphabetic"), imageIndex, eventHandler2, true);
					this.viewSortButtons[0] = this.CreatePushButton(SR.GetString("PBRSToolTipCategorized"), imageIndex2, eventHandler2, true);
					this.viewSortButtons[2] = this.CreatePushButton("", 0, eventHandler2, true);
					this.viewSortButtons[2].Visible = false;
					for (int i = 0; i < this.viewSortButtons.Length; i++)
					{
						arrayList.Add(this.viewSortButtons[i]);
					}
				}
				else
				{
					int count = arrayList.Count;
					for (int i = count - 1; i >= 2; i--)
					{
						arrayList.RemoveAt(i);
					}
					count = this.imageList[0].Images.Count;
					for (int i = count - 1; i >= 2; i--)
					{
						this.RemoveImage(i);
					}
				}
				arrayList.Add(this.separator1);
				this.viewTabButtons = new ToolStripButton[this.viewTabs.Length];
				bool flag = this.viewTabs.Length > 1;
				for (int i = 0; i < this.viewTabs.Length; i++)
				{
					try
					{
						Bitmap bitmap = this.viewTabs[i].Bitmap;
						this.viewTabButtons[i] = this.CreatePushButton(this.viewTabs[i].TabName, this.AddImage(bitmap), eventHandler, true);
						if (flag)
						{
							arrayList.Add(this.viewTabButtons[i]);
						}
					}
					catch (Exception ex3)
					{
					}
				}
				if (flag)
				{
					arrayList.Add(this.separator2);
				}
				int imageIndex3 = 0;
				try
				{
					if (this.bmpPropPage == null)
					{
						this.bmpPropPage = this.ShowPropertyPageImage;
					}
					imageIndex3 = this.AddImage(this.bmpPropPage);
				}
				catch (Exception ex4)
				{
				}
				this.btnViewPropertyPages = this.CreatePushButton(SR.GetString("PBRSToolTipPropertyPages"), imageIndex3, eventHandler3, false);
				this.btnViewPropertyPages.Enabled = false;
				arrayList.Add(this.btnViewPropertyPages);
				if (this.imageList[1] != null)
				{
					this.imageList[1].Dispose();
					this.imageList[1] = null;
				}
				if (this.buttonType != 0)
				{
					this.EnsureLargeButtons();
				}
				this.toolStrip.ImageList = this.imageList[this.buttonType];
				this.toolStrip.SuspendLayout();
				this.toolStrip.Items.Clear();
				for (int j = 0; j < arrayList.Count; j++)
				{
					this.toolStrip.Items.Add(arrayList[j] as ToolStripItem);
				}
				this.toolStrip.ResumeLayout();
				if (this.viewTabsDirty)
				{
					this.OnLayoutInternal(false);
				}
				this.viewTabsDirty = false;
			}
			finally
			{
				this.FreezePainting = false;
			}
		}

		/// <summary>Displays or hides the events button.</summary>
		/// <param name="value">
		///       <see langword="true" /> to show the events button; <see langword="false" /> to hide the events button.</param>
		// Token: 0x06003195 RID: 12693 RVA: 0x000E8098 File Offset: 0x000E6298
		protected void ShowEventsButton(bool value)
		{
			if (this.viewTabs != null && this.viewTabs.Length > 1 && this.viewTabs[1] is EventsTab)
			{
				this.viewTabButtons[1].Visible = value;
				if (!value && this.selectedViewTab == 1)
				{
					this.SelectViewTabButton(this.viewTabButtons[0], true);
				}
			}
			this.UpdatePropertiesViewTabVisibility();
		}

		/// <summary>Gets the image that represents sorting grid items by property name.</summary>
		/// <returns>The image that represents sorting by property.</returns>
		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x06003196 RID: 12694 RVA: 0x000E80F6 File Offset: 0x000E62F6
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected virtual Bitmap SortByPropertyImage
		{
			get
			{
				return new Bitmap(typeof(PropertyGrid), "PBAlpha.bmp");
			}
		}

		/// <summary>Gets the image that represents sorting grid items by category.</summary>
		/// <returns>The image that represents sorting by category.</returns>
		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x06003197 RID: 12695 RVA: 0x000E810C File Offset: 0x000E630C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected virtual Bitmap SortByCategoryImage
		{
			get
			{
				return new Bitmap(typeof(PropertyGrid), "PBCatego.bmp");
			}
		}

		/// <summary>Gets the image that represents the property page.</summary>
		/// <returns>The image that represents the property page.</returns>
		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06003198 RID: 12696 RVA: 0x000E8122 File Offset: 0x000E6322
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected virtual Bitmap ShowPropertyPageImage
		{
			get
			{
				return new Bitmap(typeof(PropertyGrid), "PBPPage.bmp");
			}
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x000E8138 File Offset: 0x000E6338
		private bool ShouldSerializeCommandsBackColor()
		{
			return this.hotcommands.ShouldSerializeBackColor();
		}

		// Token: 0x0600319A RID: 12698 RVA: 0x000E8145 File Offset: 0x000E6345
		private bool ShouldSerializeCommandsForeColor()
		{
			return this.hotcommands.ShouldSerializeForeColor();
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x000E8152 File Offset: 0x000E6352
		private bool ShouldSerializeCommandsLinkColor()
		{
			return this.hotcommands.Label.ShouldSerializeLinkColor();
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x000E8164 File Offset: 0x000E6364
		private bool ShouldSerializeCommandsActiveLinkColor()
		{
			return this.hotcommands.Label.ShouldSerializeActiveLinkColor();
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x000E8176 File Offset: 0x000E6376
		private bool ShouldSerializeCommandsDisabledLinkColor()
		{
			return this.hotcommands.Label.ShouldSerializeDisabledLinkColor();
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x000E8188 File Offset: 0x000E6388
		private void SinkPropertyNotifyEvents()
		{
			int num = 0;
			while (this.connectionPointCookies != null && num < this.connectionPointCookies.Length)
			{
				if (this.connectionPointCookies[num] != null)
				{
					this.connectionPointCookies[num].Disconnect();
					this.connectionPointCookies[num] = null;
				}
				num++;
			}
			if (this.currentObjects == null || this.currentObjects.Length == 0)
			{
				this.connectionPointCookies = null;
				return;
			}
			if (this.connectionPointCookies == null || this.currentObjects.Length > this.connectionPointCookies.Length)
			{
				this.connectionPointCookies = new AxHost.ConnectionPointCookie[this.currentObjects.Length];
			}
			for (int i = 0; i < this.currentObjects.Length; i++)
			{
				try
				{
					object unwrappedObject = this.GetUnwrappedObject(i);
					if (Marshal.IsComObject(unwrappedObject))
					{
						this.connectionPointCookies[i] = new AxHost.ConnectionPointCookie(unwrappedObject, this, typeof(UnsafeNativeMethods.IPropertyNotifySink), false);
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x000E826C File Offset: 0x000E646C
		private bool ShouldForwardChildMouseMessage(Control child, MouseEventArgs me, ref Point pt)
		{
			Size size = child.Size;
			if (me.Y <= 1 || size.Height - me.Y <= 1)
			{
				NativeMethods.POINT point = new NativeMethods.POINT();
				point.x = me.X;
				point.y = me.Y;
				UnsafeNativeMethods.MapWindowPoints(new HandleRef(child, child.Handle), new HandleRef(this, base.Handle), point, 1);
				pt.X = point.x;
				pt.Y = point.y;
				return true;
			}
			return false;
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x000E82F4 File Offset: 0x000E64F4
		private void UpdatePropertiesViewTabVisibility()
		{
			if (this.viewTabButtons != null)
			{
				int num = 0;
				for (int i = 1; i < this.viewTabButtons.Length; i++)
				{
					if (this.viewTabButtons[i].Visible)
					{
						num++;
					}
				}
				if (num > 0)
				{
					this.viewTabButtons[0].Visible = true;
					this.separator2.Visible = true;
					return;
				}
				this.viewTabButtons[0].Visible = false;
				this.separator2.Visible = false;
			}
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x000E836C File Offset: 0x000E656C
		internal void UpdateSelection()
		{
			if (!this.GetFlag(1))
			{
				return;
			}
			if (this.viewTabs == null)
			{
				return;
			}
			string key = this.viewTabs[this.selectedViewTab].TabName + this.propertySortValue.ToString();
			if (this.viewTabProps != null && this.viewTabProps.ContainsKey(key))
			{
				this.peMain = (GridEntry)this.viewTabProps[key];
				if (this.peMain != null)
				{
					this.peMain.Refresh();
				}
			}
			else
			{
				if (this.currentObjects != null && this.currentObjects.Length != 0)
				{
					this.peMain = (GridEntry)GridEntry.Create(this.gridView, this.currentObjects, new PropertyGrid.PropertyGridServiceProvider(this), this.designerHost, this.SelectedTab, this.propertySortValue);
				}
				else
				{
					this.peMain = null;
				}
				if (this.peMain == null)
				{
					this.currentPropEntries = new GridEntryCollection(null, new GridEntry[0]);
					this.gridView.ClearProps();
					return;
				}
				if (this.BrowsableAttributes != null)
				{
					this.peMain.BrowsableAttributes = this.BrowsableAttributes;
				}
				if (this.viewTabProps == null)
				{
					this.viewTabProps = new Hashtable();
				}
				this.viewTabProps[key] = this.peMain;
			}
			this.currentPropEntries = this.peMain.Children;
			this.peDefault = this.peMain.DefaultChild;
			this.gridView.Invalidate();
		}

		/// <summary>Gets or sets a value that determines whether to use the <see cref="T:System.Drawing.Graphics" /> class (GDI+) or the <see cref="T:System.Windows.Forms.TextRenderer" /> class (GDI) to render text.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Drawing.Graphics" /> class should be used to perform text rendering for compatibility with versions 1.0 and 1.1. of the .NET Framework; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x060031A2 RID: 12706 RVA: 0x0001C7CB File Offset: 0x0001A9CB
		// (set) Token: 0x060031A3 RID: 12707 RVA: 0x000E84DC File Offset: 0x000E66DC
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
				this.doccomment.UpdateTextRenderingEngine();
				this.gridView.Invalidate();
			}
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x060031A4 RID: 12708 RVA: 0x00020C1B File Offset: 0x0001EE1B
		internal override bool SupportsUiaProviders
		{
			get
			{
				return AccessibilityImprovements.Level3 && !base.DesignMode;
			}
		}

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x060031A5 RID: 12709 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override bool SupportsUseCompatibleTextRendering
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal override bool AllowsKeyboardToolTip()
		{
			return false;
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x000E84FB File Offset: 0x000E66FB
		internal bool WantsTab(bool forward)
		{
			if (forward)
			{
				return this.toolStrip.Visible && this.toolStrip.Focused;
			}
			return this.gridView.ContainsFocus && this.toolStrip.Visible;
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x000E8538 File Offset: 0x000E6738
		private void GetDataFromCopyData(IntPtr lparam)
		{
			NativeMethods.COPYDATASTRUCT copydatastruct = (NativeMethods.COPYDATASTRUCT)UnsafeNativeMethods.PtrToStructure(lparam, typeof(NativeMethods.COPYDATASTRUCT));
			if (copydatastruct != null && copydatastruct.lpData != IntPtr.Zero)
			{
				this.propName = Marshal.PtrToStringAuto(copydatastruct.lpData);
				this.dwMsg = copydatastruct.dwData;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.SystemColorsChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060031A9 RID: 12713 RVA: 0x000E858D File Offset: 0x000E678D
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			this.SetupToolbar(true);
			if (!this.GetFlag(64))
			{
				this.SetupToolbar(true);
				this.SetFlag(64, true);
			}
			base.OnSystemColorsChanged(e);
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x000E85B8 File Offset: 0x000E67B8
		private void RescaleConstants()
		{
			PropertyGrid.normalButtonSize = base.LogicalToDeviceUnits(PropertyGrid.DEFAULT_NORMAL_BUTTON_SIZE);
			PropertyGrid.largeButtonSize = base.LogicalToDeviceUnits(PropertyGrid.DEFAULT_LARGE_BUTTON_SIZE);
			PropertyGrid.cyDivider = base.LogicalToDeviceUnits(3);
			this.toolStripButtonPaddingY = base.LogicalToDeviceUnits(9);
			if (this.hotcommands != null && this.hotcommands.Visible)
			{
				this.Controls.Remove(this.hotcommands);
				this.Controls.Add(this.hotcommands);
			}
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x000E8636 File Offset: 0x000E6836
		protected override void RescaleConstantsForDpi(int deviceDpiOld, int deviceDpiNew)
		{
			base.RescaleConstantsForDpi(deviceDpiOld, deviceDpiNew);
			if (DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				this.RescaleConstants();
				this.SetupToolbar(true);
			}
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x060031AC RID: 12716 RVA: 0x000E8654 File Offset: 0x000E6854
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int num = m.Msg;
			if (num != 74)
			{
				switch (num)
				{
				case 768:
					if ((long)m.LParam == 0L)
					{
						this.gridView.DoCutCommand();
						return;
					}
					m.Result = (this.CanCut ? ((IntPtr)1) : ((IntPtr)0));
					return;
				case 769:
					if ((long)m.LParam == 0L)
					{
						this.gridView.DoCopyCommand();
						return;
					}
					m.Result = (this.CanCopy ? ((IntPtr)1) : ((IntPtr)0));
					return;
				case 770:
					if ((long)m.LParam == 0L)
					{
						this.gridView.DoPasteCommand();
						return;
					}
					m.Result = (this.CanPaste ? ((IntPtr)1) : ((IntPtr)0));
					return;
				case 771:
					break;
				case 772:
					if ((long)m.LParam == 0L)
					{
						this.gridView.DoUndoCommand();
						return;
					}
					m.Result = (this.CanUndo ? ((IntPtr)1) : ((IntPtr)0));
					return;
				default:
					switch (num)
					{
					case 1104:
						if (this.toolStrip != null)
						{
							m.Result = (IntPtr)this.toolStrip.Items.Count;
							return;
						}
						break;
					case 1105:
						if (this.toolStrip != null)
						{
							int num2 = (int)((long)m.WParam);
							if (num2 >= 0 && num2 < this.toolStrip.Items.Count)
							{
								ToolStripButton toolStripButton = this.toolStrip.Items[num2] as ToolStripButton;
								if (toolStripButton != null)
								{
									toolStripButton.Checked = !toolStripButton.Checked;
									if (toolStripButton == this.btnViewPropertyPages)
									{
										this.OnViewButtonClickPP(toolStripButton, EventArgs.Empty);
										return;
									}
									num = (int)((long)m.WParam);
									if (num <= 1)
									{
										this.OnViewSortButtonClick(toolStripButton, EventArgs.Empty);
										return;
									}
									this.SelectViewTabButton(toolStripButton, true);
								}
							}
							return;
						}
						break;
					case 1106:
						if (this.toolStrip != null)
						{
							int num3 = (int)((long)m.WParam);
							if (num3 >= 0 && num3 < this.toolStrip.Items.Count)
							{
								ToolStripButton toolStripButton2 = this.toolStrip.Items[num3] as ToolStripButton;
								if (toolStripButton2 != null)
								{
									m.Result = (IntPtr)(toolStripButton2.Checked ? 1 : 0);
									return;
								}
								m.Result = IntPtr.Zero;
							}
							return;
						}
						break;
					case 1107:
					case 1108:
						if (this.toolStrip != null)
						{
							int num4 = (int)((long)m.WParam);
							if (num4 >= 0 && num4 < this.toolStrip.Items.Count)
							{
								string text;
								if (m.Msg == 1107)
								{
									text = this.toolStrip.Items[num4].Text;
								}
								else
								{
									text = this.toolStrip.Items[num4].ToolTipText;
								}
								m.Result = AutomationMessages.WriteAutomationText(text);
							}
							return;
						}
						break;
					case 1109:
						if (m.Msg == this.dwMsg)
						{
							m.Result = (IntPtr)this.gridView.GetPropertyLocation(this.propName, m.LParam == IntPtr.Zero, m.WParam == IntPtr.Zero);
							return;
						}
						break;
					case 1110:
					case 1111:
						m.Result = this.gridView.SendMessage(m.Msg, m.WParam, m.LParam);
						return;
					case 1112:
						if (m.LParam != IntPtr.Zero)
						{
							string b = AutomationMessages.ReadAutomationText(m.LParam);
							for (int i = 0; i < this.viewTabs.Length; i++)
							{
								if (this.viewTabs[i].GetType().FullName == b && this.viewTabButtons[i].Visible)
								{
									this.SelectViewTabButtonDefault(this.viewTabButtons[i]);
									m.Result = (IntPtr)1;
									break;
								}
							}
						}
						m.Result = (IntPtr)0;
						return;
					case 1113:
					{
						string testingInfo = this.gridView.GetTestingInfo((int)((long)m.WParam));
						m.Result = AutomationMessages.WriteAutomationText(testingInfo);
						return;
					}
					}
					break;
				}
				base.WndProc(ref m);
				return;
			}
			this.GetDataFromCopyData(m.LParam);
			m.Result = (IntPtr)1;
		}

		// Token: 0x04001DB8 RID: 7608
		private DocComment doccomment;

		// Token: 0x04001DB9 RID: 7609
		private int dcSizeRatio = -1;

		// Token: 0x04001DBA RID: 7610
		private int hcSizeRatio = -1;

		// Token: 0x04001DBB RID: 7611
		private HotCommands hotcommands;

		// Token: 0x04001DBC RID: 7612
		private ToolStrip toolStrip;

		// Token: 0x04001DBD RID: 7613
		private bool helpVisible = true;

		// Token: 0x04001DBE RID: 7614
		private bool toolbarVisible = true;

		// Token: 0x04001DBF RID: 7615
		private ImageList[] imageList = new ImageList[2];

		// Token: 0x04001DC0 RID: 7616
		private Bitmap bmpAlpha;

		// Token: 0x04001DC1 RID: 7617
		private Bitmap bmpCategory;

		// Token: 0x04001DC2 RID: 7618
		private Bitmap bmpPropPage;

		// Token: 0x04001DC3 RID: 7619
		private bool viewTabsDirty = true;

		// Token: 0x04001DC4 RID: 7620
		private bool drawFlatToolBar;

		// Token: 0x04001DC5 RID: 7621
		private PropertyTab[] viewTabs = new PropertyTab[0];

		// Token: 0x04001DC6 RID: 7622
		private PropertyTabScope[] viewTabScopes = new PropertyTabScope[0];

		// Token: 0x04001DC7 RID: 7623
		private Hashtable viewTabProps;

		// Token: 0x04001DC8 RID: 7624
		private ToolStripButton[] viewTabButtons;

		// Token: 0x04001DC9 RID: 7625
		private int selectedViewTab;

		// Token: 0x04001DCA RID: 7626
		private ToolStripButton[] viewSortButtons;

		// Token: 0x04001DCB RID: 7627
		private int selectedViewSort;

		// Token: 0x04001DCC RID: 7628
		private PropertySort propertySortValue;

		// Token: 0x04001DCD RID: 7629
		private ToolStripButton btnViewPropertyPages;

		// Token: 0x04001DCE RID: 7630
		private ToolStripSeparator separator1;

		// Token: 0x04001DCF RID: 7631
		private ToolStripSeparator separator2;

		// Token: 0x04001DD0 RID: 7632
		private int buttonType;

		// Token: 0x04001DD1 RID: 7633
		private PropertyGridView gridView;

		// Token: 0x04001DD2 RID: 7634
		private IDesignerHost designerHost;

		// Token: 0x04001DD3 RID: 7635
		private IDesignerEventService designerEventService;

		// Token: 0x04001DD4 RID: 7636
		private Hashtable designerSelections;

		// Token: 0x04001DD5 RID: 7637
		private GridEntry peDefault;

		// Token: 0x04001DD6 RID: 7638
		private GridEntry peMain;

		// Token: 0x04001DD7 RID: 7639
		private GridEntryCollection currentPropEntries;

		// Token: 0x04001DD8 RID: 7640
		private object[] currentObjects;

		// Token: 0x04001DD9 RID: 7641
		private int paintFrozen;

		// Token: 0x04001DDA RID: 7642
		private Color lineColor = SystemInformation.HighContrast ? (AccessibilityImprovements.Level1 ? SystemColors.ControlDarkDark : SystemColors.ControlDark) : SystemColors.InactiveBorder;

		// Token: 0x04001DDB RID: 7643
		internal bool developerOverride;

		// Token: 0x04001DDC RID: 7644
		internal Brush lineBrush;

		// Token: 0x04001DDD RID: 7645
		private Color categoryForeColor = SystemColors.ControlText;

		// Token: 0x04001DDE RID: 7646
		private Color categorySplitterColor = SystemColors.Control;

		// Token: 0x04001DDF RID: 7647
		private Color viewBorderColor = SystemColors.ControlDark;

		// Token: 0x04001DE0 RID: 7648
		private Color selectedItemWithFocusForeColor = SystemColors.HighlightText;

		// Token: 0x04001DE1 RID: 7649
		private Color selectedItemWithFocusBackColor = SystemColors.Highlight;

		// Token: 0x04001DE2 RID: 7650
		internal Brush selectedItemWithFocusBackBrush;

		// Token: 0x04001DE3 RID: 7651
		private bool canShowVisualStyleGlyphs = true;

		// Token: 0x04001DE4 RID: 7652
		private AttributeCollection browsableAttributes;

		// Token: 0x04001DE5 RID: 7653
		private PropertyGrid.SnappableControl targetMove;

		// Token: 0x04001DE6 RID: 7654
		private int dividerMoveY = -1;

		// Token: 0x04001DE7 RID: 7655
		private const int CYDIVIDER = 3;

		// Token: 0x04001DE8 RID: 7656
		private static int cyDivider = 3;

		// Token: 0x04001DE9 RID: 7657
		private const int CXINDENT = 0;

		// Token: 0x04001DEA RID: 7658
		private const int CYINDENT = 2;

		// Token: 0x04001DEB RID: 7659
		private const int MIN_GRID_HEIGHT = 20;

		// Token: 0x04001DEC RID: 7660
		private const int PROPERTIES = 0;

		// Token: 0x04001DED RID: 7661
		private const int EVENTS = 1;

		// Token: 0x04001DEE RID: 7662
		private const int ALPHA = 1;

		// Token: 0x04001DEF RID: 7663
		private const int CATEGORIES = 0;

		// Token: 0x04001DF0 RID: 7664
		private const int NO_SORT = 2;

		// Token: 0x04001DF1 RID: 7665
		private const int NORMAL_BUTTONS = 0;

		// Token: 0x04001DF2 RID: 7666
		private const int LARGE_BUTTONS = 1;

		// Token: 0x04001DF3 RID: 7667
		private const int TOOLSTRIP_BUTTON_PADDING_Y = 9;

		// Token: 0x04001DF4 RID: 7668
		private int toolStripButtonPaddingY = 9;

		// Token: 0x04001DF5 RID: 7669
		private static readonly Size DEFAULT_LARGE_BUTTON_SIZE = new Size(32, 32);

		// Token: 0x04001DF6 RID: 7670
		private static readonly Size DEFAULT_NORMAL_BUTTON_SIZE = new Size(16, 16);

		// Token: 0x04001DF7 RID: 7671
		private static Size largeButtonSize = PropertyGrid.DEFAULT_LARGE_BUTTON_SIZE;

		// Token: 0x04001DF8 RID: 7672
		private static Size normalButtonSize = PropertyGrid.DEFAULT_NORMAL_BUTTON_SIZE;

		// Token: 0x04001DF9 RID: 7673
		private static bool isScalingInitialized = false;

		// Token: 0x04001DFA RID: 7674
		private const ushort PropertiesChanged = 1;

		// Token: 0x04001DFB RID: 7675
		private const ushort GotDesignerEventService = 2;

		// Token: 0x04001DFC RID: 7676
		private const ushort InternalChange = 4;

		// Token: 0x04001DFD RID: 7677
		private const ushort TabsChanging = 8;

		// Token: 0x04001DFE RID: 7678
		private const ushort BatchMode = 16;

		// Token: 0x04001DFF RID: 7679
		private const ushort ReInitTab = 32;

		// Token: 0x04001E00 RID: 7680
		private const ushort SysColorChangeRefresh = 64;

		// Token: 0x04001E01 RID: 7681
		private const ushort FullRefreshAfterBatch = 128;

		// Token: 0x04001E02 RID: 7682
		private const ushort BatchModeChange = 256;

		// Token: 0x04001E03 RID: 7683
		private const ushort RefreshingProperties = 512;

		// Token: 0x04001E04 RID: 7684
		private ushort flags;

		// Token: 0x04001E05 RID: 7685
		private readonly ComponentEventHandler onComponentAdd;

		// Token: 0x04001E06 RID: 7686
		private readonly ComponentEventHandler onComponentRemove;

		// Token: 0x04001E07 RID: 7687
		private readonly ComponentChangedEventHandler onComponentChanged;

		// Token: 0x04001E08 RID: 7688
		private AxHost.ConnectionPointCookie[] connectionPointCookies;

		// Token: 0x04001E09 RID: 7689
		private static object EventPropertyValueChanged = new object();

		// Token: 0x04001E0A RID: 7690
		private static object EventComComponentNameChanged = new object();

		// Token: 0x04001E0B RID: 7691
		private static object EventPropertyTabChanged = new object();

		// Token: 0x04001E0C RID: 7692
		private static object EventSelectedGridItemChanged = new object();

		// Token: 0x04001E0D RID: 7693
		private static object EventPropertySortChanged = new object();

		// Token: 0x04001E0E RID: 7694
		private static object EventSelectedObjectsChanged = new object();

		// Token: 0x04001E0F RID: 7695
		private string propName;

		// Token: 0x04001E10 RID: 7696
		private int dwMsg;

		// Token: 0x02000702 RID: 1794
		internal abstract class SnappableControl : Control
		{
			// Token: 0x06005FB7 RID: 24503
			public abstract int GetOptimalHeight(int width);

			// Token: 0x06005FB8 RID: 24504
			public abstract int SnapHeightRequest(int request);

			// Token: 0x06005FB9 RID: 24505 RVA: 0x00189424 File Offset: 0x00187624
			public SnappableControl(PropertyGrid ownerGrid)
			{
				this.ownerGrid = ownerGrid;
				base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			}

			// Token: 0x170016E3 RID: 5859
			// (get) Token: 0x06005FBA RID: 24506 RVA: 0x000284A2 File Offset: 0x000266A2
			// (set) Token: 0x06005FBB RID: 24507 RVA: 0x0001203B File Offset: 0x0001023B
			public override Cursor Cursor
			{
				get
				{
					return Cursors.Default;
				}
				set
				{
					base.Cursor = value;
				}
			}

			// Token: 0x06005FBC RID: 24508 RVA: 0x0000701A File Offset: 0x0000521A
			protected override void OnControlAdded(ControlEventArgs ce)
			{
			}

			// Token: 0x170016E4 RID: 5860
			// (get) Token: 0x06005FBD RID: 24509 RVA: 0x0018944A File Offset: 0x0018764A
			// (set) Token: 0x06005FBE RID: 24510 RVA: 0x00189452 File Offset: 0x00187652
			public Color BorderColor
			{
				get
				{
					return this.borderColor;
				}
				set
				{
					this.borderColor = value;
				}
			}

			// Token: 0x06005FBF RID: 24511 RVA: 0x0018945C File Offset: 0x0018765C
			protected override void OnPaint(PaintEventArgs e)
			{
				base.OnPaint(e);
				Rectangle clientRectangle = base.ClientRectangle;
				int num = clientRectangle.Width;
				clientRectangle.Width = num - 1;
				num = clientRectangle.Height;
				clientRectangle.Height = num - 1;
				using (Pen pen = new Pen(this.BorderColor, 1f))
				{
					e.Graphics.DrawRectangle(pen, clientRectangle);
				}
			}

			// Token: 0x0400411B RID: 16667
			private Color borderColor = SystemColors.ControlDark;

			// Token: 0x0400411C RID: 16668
			protected PropertyGrid ownerGrid;

			// Token: 0x0400411D RID: 16669
			internal bool userSized;
		}

		/// <summary>Contains a collection of <see cref="T:System.Windows.Forms.Design.PropertyTab" /> objects.</summary>
		// Token: 0x02000703 RID: 1795
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public class PropertyTabCollection : ICollection, IEnumerable
		{
			// Token: 0x06005FC0 RID: 24512 RVA: 0x001894D4 File Offset: 0x001876D4
			internal PropertyTabCollection(PropertyGrid owner)
			{
				this.owner = owner;
			}

			/// <summary>Gets the number of Property tabs in the collection.</summary>
			/// <returns>The number of Property tabs in the collection.</returns>
			// Token: 0x170016E5 RID: 5861
			// (get) Token: 0x06005FC1 RID: 24513 RVA: 0x001894E3 File Offset: 0x001876E3
			public int Count
			{
				get
				{
					if (this.owner == null)
					{
						return 0;
					}
					return this.owner.viewTabs.Length;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.SyncRoot" />.</summary>
			/// <returns>An object that can be used to synchronize access to the underlying list.</returns>
			// Token: 0x170016E6 RID: 5862
			// (get) Token: 0x06005FC2 RID: 24514 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.IsSynchronized" />.</summary>
			/// <returns>
			///     <see langword="true" /> to indicate the list is synchronized; otherwise <see langword="false" />.</returns>
			// Token: 0x170016E7 RID: 5863
			// (get) Token: 0x06005FC3 RID: 24515 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets the <see cref="T:System.Windows.Forms.Design.PropertyTab" /> at the specified index.</summary>
			/// <param name="index">The index of the <see cref="T:System.Windows.Forms.Design.PropertyTab" /> to return. </param>
			/// <returns>The <see cref="T:System.Windows.Forms.Design.PropertyTab" /> at the specified index.</returns>
			// Token: 0x170016E8 RID: 5864
			public PropertyTab this[int index]
			{
				get
				{
					if (this.owner == null)
					{
						throw new InvalidOperationException(SR.GetString("PropertyGridPropertyTabCollectionReadOnly"));
					}
					return this.owner.viewTabs[index];
				}
			}

			/// <summary>Adds a Property tab of the specified type to the collection.</summary>
			/// <param name="propertyTabType">The Property tab type to add to the grid. </param>
			// Token: 0x06005FC5 RID: 24517 RVA: 0x00189523 File Offset: 0x00187723
			public void AddTabType(Type propertyTabType)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("PropertyGridPropertyTabCollectionReadOnly"));
				}
				this.owner.AddTab(propertyTabType, PropertyTabScope.Global);
			}

			/// <summary>Adds a Property tab of the specified type and with the specified scope to the collection.</summary>
			/// <param name="propertyTabType">The Property tab type to add to the grid. </param>
			/// <param name="tabScope">One of the <see cref="T:System.ComponentModel.PropertyTabScope" /> values. </param>
			// Token: 0x06005FC6 RID: 24518 RVA: 0x0018954A File Offset: 0x0018774A
			public void AddTabType(Type propertyTabType, PropertyTabScope tabScope)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("PropertyGridPropertyTabCollectionReadOnly"));
				}
				this.owner.AddTab(propertyTabType, tabScope);
			}

			/// <summary>Removes all the Property tabs of the specified scope from the collection.</summary>
			/// <param name="tabScope">The scope of the tabs to clear. </param>
			/// <exception cref="T:System.ArgumentException">The assigned value of the <paramref name="tabScope" /> parameter is less than the <see langword="Document" /> value of <see cref="T:System.ComponentModel.PropertyTabScope" />. </exception>
			// Token: 0x06005FC7 RID: 24519 RVA: 0x00189571 File Offset: 0x00187771
			public void Clear(PropertyTabScope tabScope)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("PropertyGridPropertyTabCollectionReadOnly"));
				}
				this.owner.ClearTabs(tabScope);
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.ICollection.CopyTo(System.Array,System.Int32)" />.</summary>
			/// <param name="dest">A zero-based array that receives the copied items from the collection.</param>
			/// <param name="index">The first position in the specified array to receive copied contents.</param>
			// Token: 0x06005FC8 RID: 24520 RVA: 0x00189597 File Offset: 0x00187797
			void ICollection.CopyTo(Array dest, int index)
			{
				if (this.owner == null)
				{
					return;
				}
				if (this.owner.viewTabs.Length != 0)
				{
					Array.Copy(this.owner.viewTabs, 0, dest, index, this.owner.viewTabs.Length);
				}
			}

			/// <summary>Returns an enumeration of all the Property tabs in the collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Windows.Forms.PropertyGrid.PropertyTabCollection" />.</returns>
			// Token: 0x06005FC9 RID: 24521 RVA: 0x001895D0 File Offset: 0x001877D0
			public IEnumerator GetEnumerator()
			{
				if (this.owner == null)
				{
					return new PropertyTab[0].GetEnumerator();
				}
				return this.owner.viewTabs.GetEnumerator();
			}

			/// <summary>Removes the specified tab type from the collection.</summary>
			/// <param name="propertyTabType">The tab type to remove from the collection. </param>
			// Token: 0x06005FCA RID: 24522 RVA: 0x001895F6 File Offset: 0x001877F6
			public void RemoveTabType(Type propertyTabType)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("PropertyGridPropertyTabCollectionReadOnly"));
				}
				this.owner.RemoveTab(propertyTabType);
			}

			// Token: 0x0400411E RID: 16670
			internal static PropertyGrid.PropertyTabCollection Empty = new PropertyGrid.PropertyTabCollection(null);

			// Token: 0x0400411F RID: 16671
			private PropertyGrid owner;
		}

		// Token: 0x02000704 RID: 1796
		private interface IUnimplemented
		{
		}

		// Token: 0x02000705 RID: 1797
		internal class SelectedObjectConverter : ReferenceConverter
		{
			// Token: 0x06005FCC RID: 24524 RVA: 0x00189629 File Offset: 0x00187829
			public SelectedObjectConverter() : base(typeof(IComponent))
			{
			}
		}

		// Token: 0x02000706 RID: 1798
		private class PropertyGridServiceProvider : IServiceProvider
		{
			// Token: 0x06005FCD RID: 24525 RVA: 0x0018963B File Offset: 0x0018783B
			public PropertyGridServiceProvider(PropertyGrid owner)
			{
				this.owner = owner;
			}

			// Token: 0x06005FCE RID: 24526 RVA: 0x0018964C File Offset: 0x0018784C
			public object GetService(Type serviceType)
			{
				object obj = null;
				if (this.owner.ActiveDesigner != null)
				{
					obj = this.owner.ActiveDesigner.GetService(serviceType);
				}
				if (obj == null)
				{
					obj = this.owner.gridView.GetService(serviceType);
				}
				if (obj == null && this.owner.Site != null)
				{
					obj = this.owner.Site.GetService(serviceType);
				}
				return obj;
			}

			// Token: 0x04004120 RID: 16672
			private PropertyGrid owner;
		}

		// Token: 0x02000707 RID: 1799
		internal static class MeasureTextHelper
		{
			// Token: 0x06005FCF RID: 24527 RVA: 0x001896B2 File Offset: 0x001878B2
			public static SizeF MeasureText(PropertyGrid owner, Graphics g, string text, Font font)
			{
				return PropertyGrid.MeasureTextHelper.MeasureTextSimple(owner, g, text, font, new SizeF(0f, 0f));
			}

			// Token: 0x06005FD0 RID: 24528 RVA: 0x001896CC File Offset: 0x001878CC
			public static SizeF MeasureText(PropertyGrid owner, Graphics g, string text, Font font, int width)
			{
				return PropertyGrid.MeasureTextHelper.MeasureText(owner, g, text, font, new SizeF((float)width, 999999f));
			}

			// Token: 0x06005FD1 RID: 24529 RVA: 0x001896E4 File Offset: 0x001878E4
			public static SizeF MeasureTextSimple(PropertyGrid owner, Graphics g, string text, Font font, SizeF size)
			{
				SizeF result;
				if (owner.UseCompatibleTextRendering)
				{
					result = g.MeasureString(text, font, size);
				}
				else
				{
					result = TextRenderer.MeasureText(g, text, font, Size.Ceiling(size), PropertyGrid.MeasureTextHelper.GetTextRendererFlags());
				}
				return result;
			}

			// Token: 0x06005FD2 RID: 24530 RVA: 0x00189724 File Offset: 0x00187924
			public static SizeF MeasureText(PropertyGrid owner, Graphics g, string text, Font font, SizeF size)
			{
				SizeF result;
				if (owner.UseCompatibleTextRendering)
				{
					result = g.MeasureString(text, font, size);
				}
				else
				{
					TextFormatFlags flags = PropertyGrid.MeasureTextHelper.GetTextRendererFlags() | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.WordBreak | TextFormatFlags.NoFullWidthCharacterBreak;
					result = TextRenderer.MeasureText(g, text, font, Size.Ceiling(size), flags);
				}
				return result;
			}

			// Token: 0x06005FD3 RID: 24531 RVA: 0x00189772 File Offset: 0x00187972
			public static TextFormatFlags GetTextRendererFlags()
			{
				return TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform;
			}
		}
	}
}
