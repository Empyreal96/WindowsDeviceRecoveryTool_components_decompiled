using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides panels on each side of the form and a central panel that can hold one or more controls.</summary>
	// Token: 0x020003DB RID: 987
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.ToolStripContainerDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("ToolStripContainerDesc")]
	public class ToolStripContainer : ContainerControl
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripContainer" /> class. </summary>
		// Token: 0x0600417C RID: 16764 RVA: 0x0011A4B8 File Offset: 0x001186B8
		public ToolStripContainer()
		{
			base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
			base.SuspendLayout();
			try
			{
				this.topPanel = new ToolStripPanel(this);
				this.bottomPanel = new ToolStripPanel(this);
				this.leftPanel = new ToolStripPanel(this);
				this.rightPanel = new ToolStripPanel(this);
				this.contentPanel = new ToolStripContentPanel();
				this.contentPanel.Dock = DockStyle.Fill;
				this.topPanel.Dock = DockStyle.Top;
				this.bottomPanel.Dock = DockStyle.Bottom;
				this.rightPanel.Dock = DockStyle.Right;
				this.leftPanel.Dock = DockStyle.Left;
				ToolStripContainer.ToolStripContainerTypedControlCollection toolStripContainerTypedControlCollection = this.Controls as ToolStripContainer.ToolStripContainerTypedControlCollection;
				if (toolStripContainerTypedControlCollection != null)
				{
					toolStripContainerTypedControlCollection.AddInternal(this.contentPanel);
					toolStripContainerTypedControlCollection.AddInternal(this.leftPanel);
					toolStripContainerTypedControlCollection.AddInternal(this.rightPanel);
					toolStripContainerTypedControlCollection.AddInternal(this.topPanel);
					toolStripContainerTypedControlCollection.AddInternal(this.bottomPanel);
				}
			}
			finally
			{
				base.ResumeLayout(true);
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>
		///     <see langword="true" /> to enable automatic scrolling; otherwise, <see langword="false" />. </returns>
		// Token: 0x17001061 RID: 4193
		// (get) Token: 0x0600417D RID: 16765 RVA: 0x000A87BB File Offset: 0x000A69BB
		// (set) Token: 0x0600417E RID: 16766 RVA: 0x000E3A46 File Offset: 0x000E1C46
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> value.</returns>
		// Token: 0x17001062 RID: 4194
		// (get) Token: 0x0600417F RID: 16767 RVA: 0x000F3C48 File Offset: 0x000F1E48
		// (set) Token: 0x06004180 RID: 16768 RVA: 0x000F3C50 File Offset: 0x000F1E50
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size AutoScrollMargin
		{
			get
			{
				return base.AutoScrollMargin;
			}
			set
			{
				base.AutoScrollMargin = value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> value.</returns>
		// Token: 0x17001063 RID: 4195
		// (get) Token: 0x06004181 RID: 16769 RVA: 0x000F3C37 File Offset: 0x000F1E37
		// (set) Token: 0x06004182 RID: 16770 RVA: 0x000F3C3F File Offset: 0x000F1E3F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size AutoScrollMinSize
		{
			get
			{
				return base.AutoScrollMinSize;
			}
			set
			{
				base.AutoScrollMinSize = value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> value.</returns>
		// Token: 0x17001064 RID: 4196
		// (get) Token: 0x06004183 RID: 16771 RVA: 0x00011FB1 File Offset: 0x000101B1
		// (set) Token: 0x06004184 RID: 16772 RVA: 0x00011FB9 File Offset: 0x000101B9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x14000346 RID: 838
		// (add) Token: 0x06004185 RID: 16773 RVA: 0x00050A7A File Offset: 0x0004EC7A
		// (remove) Token: 0x06004186 RID: 16774 RVA: 0x00050A83 File Offset: 0x0004EC83
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new event EventHandler BackColorChanged
		{
			add
			{
				base.BackColorChanged += value;
			}
			remove
			{
				base.BackColorChanged -= value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>TTThe background image displayed in the control.</returns>
		// Token: 0x17001065 RID: 4197
		// (get) Token: 0x06004187 RID: 16775 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x06004188 RID: 16776 RVA: 0x00011FCA File Offset: 0x000101CA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image BackgroundImage
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

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x14000347 RID: 839
		// (add) Token: 0x06004189 RID: 16777 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x0600418A RID: 16778 RVA: 0x0001FD8A File Offset: 0x0001DF8A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
		/// <returns>The background image layout as defined in the ImageLayout enumeration.</returns>
		// Token: 0x17001066 RID: 4198
		// (get) Token: 0x0600418B RID: 16779 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x0600418C RID: 16780 RVA: 0x00011FDB File Offset: 0x000101DB
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x14000348 RID: 840
		// (add) Token: 0x0600418D RID: 16781 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x0600418E RID: 16782 RVA: 0x0001FD93 File Offset: 0x0001DF93
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				base.BackgroundImageLayoutChanged += value;
			}
		}

		/// <summary>Gets the bottom panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripPanel" /> representing the bottom panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</returns>
		// Token: 0x17001067 RID: 4199
		// (get) Token: 0x0600418F RID: 16783 RVA: 0x0011A5BC File Offset: 0x001187BC
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripContainerBottomToolStripPanelDescr")]
		[Localizable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ToolStripPanel BottomToolStripPanel
		{
			get
			{
				return this.bottomPanel;
			}
		}

		/// <summary>Gets or sets a value indicating whether the bottom panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" /> is visible. </summary>
		/// <returns>
		///     <see langword="true" /> if the bottom panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" /> is visible; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17001068 RID: 4200
		// (get) Token: 0x06004190 RID: 16784 RVA: 0x0011A5C4 File Offset: 0x001187C4
		// (set) Token: 0x06004191 RID: 16785 RVA: 0x0011A5D1 File Offset: 0x001187D1
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripContainerBottomToolStripPanelVisibleDescr")]
		[DefaultValue(true)]
		public bool BottomToolStripPanelVisible
		{
			get
			{
				return this.BottomToolStripPanel.Visible;
			}
			set
			{
				this.BottomToolStripPanel.Visible = value;
			}
		}

		/// <summary>Gets the center panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripContentPanel" /> representing the center panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</returns>
		// Token: 0x17001069 RID: 4201
		// (get) Token: 0x06004192 RID: 16786 RVA: 0x0011A5DF File Offset: 0x001187DF
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripContainerContentPanelDescr")]
		[Localizable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ToolStripContentPanel ContentPanel
		{
			get
			{
				return this.contentPanel;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>
		///     <see langword="true" /> if the control causes validation; otherwise, <see langword="false" />. </returns>
		// Token: 0x1700106A RID: 4202
		// (get) Token: 0x06004193 RID: 16787 RVA: 0x000DA227 File Offset: 0x000D8427
		// (set) Token: 0x06004194 RID: 16788 RVA: 0x000DA22F File Offset: 0x000D842F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool CausesValidation
		{
			get
			{
				return base.CausesValidation;
			}
			set
			{
				base.CausesValidation = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripContainer.CausesValidation" /> property changes.</summary>
		// Token: 0x14000349 RID: 841
		// (add) Token: 0x06004195 RID: 16789 RVA: 0x000DA238 File Offset: 0x000D8438
		// (remove) Token: 0x06004196 RID: 16790 RVA: 0x000DA241 File Offset: 0x000D8441
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler CausesValidationChanged
		{
			add
			{
				base.CausesValidationChanged += value;
			}
			remove
			{
				base.CausesValidationChanged -= value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The ContextMenuStrip associated with this control.</returns>
		// Token: 0x1700106B RID: 4203
		// (get) Token: 0x06004197 RID: 16791 RVA: 0x0010C0FA File Offset: 0x0010A2FA
		// (set) Token: 0x06004198 RID: 16792 RVA: 0x0010C102 File Offset: 0x0010A302
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return base.ContextMenuStrip;
			}
			set
			{
				base.ContextMenuStrip = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripContainer.ContextMenuStrip" /> property changes.</summary>
		// Token: 0x1400034A RID: 842
		// (add) Token: 0x06004199 RID: 16793 RVA: 0x0010C10B File Offset: 0x0010A30B
		// (remove) Token: 0x0600419A RID: 16794 RVA: 0x0010C114 File Offset: 0x0010A314
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ContextMenuStripChanged
		{
			add
			{
				base.ContextMenuStripChanged += value;
			}
			remove
			{
				base.ContextMenuStripChanged -= value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The cursor that is displayed when the mouse pointer is over the control.</returns>
		// Token: 0x1700106C RID: 4204
		// (get) Token: 0x0600419B RID: 16795 RVA: 0x00012033 File Offset: 0x00010233
		// (set) Token: 0x0600419C RID: 16796 RVA: 0x0001203B File Offset: 0x0001023B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Cursor Cursor
		{
			get
			{
				return base.Cursor;
			}
			set
			{
				base.Cursor = value;
			}
		}

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x1400034B RID: 843
		// (add) Token: 0x0600419D RID: 16797 RVA: 0x0003E0B3 File Offset: 0x0003C2B3
		// (remove) Token: 0x0600419E RID: 16798 RVA: 0x0003E0BC File Offset: 0x0003C2BC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new event EventHandler CursorChanged
		{
			add
			{
				base.CursorChanged += value;
			}
			remove
			{
				base.CursorChanged -= value;
			}
		}

		/// <summary>Gets the default size of the <see cref="T:System.Windows.Forms.ToolStripContainer" />, in pixels.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> representing the horizontal and vertical dimensions of the <see cref="T:System.Windows.Forms.ToolStripContainer" />, in pixels.</returns>
		// Token: 0x1700106D RID: 4205
		// (get) Token: 0x0600419F RID: 16799 RVA: 0x0011A5E7 File Offset: 0x001187E7
		protected override Size DefaultSize
		{
			get
			{
				return new Size(150, 175);
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The foreground color of the control.</returns>
		// Token: 0x1700106E RID: 4206
		// (get) Token: 0x060041A0 RID: 16800 RVA: 0x00012082 File Offset: 0x00010282
		// (set) Token: 0x060041A1 RID: 16801 RVA: 0x0001208A File Offset: 0x0001028A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color ForeColor
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

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x1400034C RID: 844
		// (add) Token: 0x060041A2 RID: 16802 RVA: 0x00052766 File Offset: 0x00050966
		// (remove) Token: 0x060041A3 RID: 16803 RVA: 0x0005276F File Offset: 0x0005096F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>Gets the left panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripPanel" /> representing the left panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</returns>
		// Token: 0x1700106F RID: 4207
		// (get) Token: 0x060041A4 RID: 16804 RVA: 0x0011A5F8 File Offset: 0x001187F8
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripContainerLeftToolStripPanelDescr")]
		[Localizable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ToolStripPanel LeftToolStripPanel
		{
			get
			{
				return this.leftPanel;
			}
		}

		/// <summary>Gets or sets a value indicating whether the left panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" /> is visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the left panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" /> is visible; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17001070 RID: 4208
		// (get) Token: 0x060041A5 RID: 16805 RVA: 0x0011A600 File Offset: 0x00118800
		// (set) Token: 0x060041A6 RID: 16806 RVA: 0x0011A60D File Offset: 0x0011880D
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripContainerLeftToolStripPanelVisibleDescr")]
		[DefaultValue(true)]
		public bool LeftToolStripPanelVisible
		{
			get
			{
				return this.LeftToolStripPanel.Visible;
			}
			set
			{
				this.LeftToolStripPanel.Visible = value;
			}
		}

		/// <summary>Gets the right panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripPanel" /> representing the right panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</returns>
		// Token: 0x17001071 RID: 4209
		// (get) Token: 0x060041A7 RID: 16807 RVA: 0x0011A61B File Offset: 0x0011881B
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripContainerRightToolStripPanelDescr")]
		[Localizable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ToolStripPanel RightToolStripPanel
		{
			get
			{
				return this.rightPanel;
			}
		}

		/// <summary>Gets or sets a value indicating whether the right panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" /> is visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the right panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" /> is visible; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17001072 RID: 4210
		// (get) Token: 0x060041A8 RID: 16808 RVA: 0x0011A623 File Offset: 0x00118823
		// (set) Token: 0x060041A9 RID: 16809 RVA: 0x0011A630 File Offset: 0x00118830
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripContainerRightToolStripPanelVisibleDescr")]
		[DefaultValue(true)]
		public bool RightToolStripPanelVisible
		{
			get
			{
				return this.RightToolStripPanel.Visible;
			}
			set
			{
				this.RightToolStripPanel.Visible = value;
			}
		}

		/// <summary>Gets the top panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripPanel" /> representing the top panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</returns>
		// Token: 0x17001073 RID: 4211
		// (get) Token: 0x060041AA RID: 16810 RVA: 0x0011A63E File Offset: 0x0011883E
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripContainerTopToolStripPanelDescr")]
		[Localizable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ToolStripPanel TopToolStripPanel
		{
			get
			{
				return this.topPanel;
			}
		}

		/// <summary>Gets or sets a value indicating whether the top panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" /> is visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the top panel of the <see cref="T:System.Windows.Forms.ToolStripContainer" /> is visible; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17001074 RID: 4212
		// (get) Token: 0x060041AB RID: 16811 RVA: 0x0011A646 File Offset: 0x00118846
		// (set) Token: 0x060041AC RID: 16812 RVA: 0x0011A653 File Offset: 0x00118853
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripContainerTopToolStripPanelVisibleDescr")]
		[DefaultValue(true)]
		public bool TopToolStripPanelVisible
		{
			get
			{
				return this.TopToolStripPanel.Visible;
			}
			set
			{
				this.TopToolStripPanel.Visible = value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The collection of controls contained within the control.</returns>
		// Token: 0x17001075 RID: 4213
		// (get) Token: 0x060041AD RID: 16813 RVA: 0x000E3CDA File Offset: 0x000E1EDA
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Control.ControlCollection Controls
		{
			get
			{
				return base.Controls;
			}
		}

		/// <summary>Creates and returns a <see cref="T:System.Windows.Forms.ToolStripContainer" /> collection.</summary>
		/// <returns>A read-only <see cref="T:System.Windows.Forms.ToolStripContainer" /> collection.</returns>
		// Token: 0x060041AE RID: 16814 RVA: 0x0011A661 File Offset: 0x00118861
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new ToolStripContainer.ToolStripContainerTypedControlCollection(this, true);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.RightToLeftChanged" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x060041AF RID: 16815 RVA: 0x0011A66C File Offset: 0x0011886C
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			RightToLeft rightToLeft = this.RightToLeft;
			if (rightToLeft == RightToLeft.Yes)
			{
				this.RightToolStripPanel.Dock = DockStyle.Left;
				this.LeftToolStripPanel.Dock = DockStyle.Right;
				return;
			}
			this.RightToolStripPanel.Dock = DockStyle.Right;
			this.LeftToolStripPanel.Dock = DockStyle.Left;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060041B0 RID: 16816 RVA: 0x0011A6BC File Offset: 0x001188BC
		protected override void OnSizeChanged(EventArgs e)
		{
			foreach (object obj in this.Controls)
			{
				Control control = (Control)obj;
				control.SuspendLayout();
			}
			base.OnSizeChanged(e);
			foreach (object obj2 in this.Controls)
			{
				Control control2 = (Control)obj2;
				control2.ResumeLayout();
			}
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x0011A764 File Offset: 0x00118964
		internal override void RecreateHandleCore()
		{
			if (base.IsHandleCreated)
			{
				foreach (object obj in this.Controls)
				{
					Control control = (Control)obj;
					control.CreateControl(true);
				}
			}
			base.RecreateHandleCore();
		}

		// Token: 0x060041B2 RID: 16818 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal override bool AllowsKeyboardToolTip()
		{
			return false;
		}

		// Token: 0x0400251C RID: 9500
		private ToolStripPanel topPanel;

		// Token: 0x0400251D RID: 9501
		private ToolStripPanel bottomPanel;

		// Token: 0x0400251E RID: 9502
		private ToolStripPanel leftPanel;

		// Token: 0x0400251F RID: 9503
		private ToolStripPanel rightPanel;

		// Token: 0x04002520 RID: 9504
		private ToolStripContentPanel contentPanel;

		// Token: 0x02000742 RID: 1858
		internal class ToolStripContainerTypedControlCollection : WindowsFormsUtils.ReadOnlyControlCollection
		{
			// Token: 0x0600616A RID: 24938 RVA: 0x0018EB04 File Offset: 0x0018CD04
			public ToolStripContainerTypedControlCollection(Control c, bool isReadOnly) : base(c, isReadOnly)
			{
				this.owner = (c as ToolStripContainer);
			}

			// Token: 0x0600616B RID: 24939 RVA: 0x0018EB3C File Offset: 0x0018CD3C
			public override void Add(Control value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.IsReadOnly)
				{
					throw new NotSupportedException(SR.GetString("ToolStripContainerUseContentPanel"));
				}
				Type type = value.GetType();
				if (!this.contentPanelType.IsAssignableFrom(type) && !this.panelType.IsAssignableFrom(type))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("TypedControlCollectionShouldBeOfTypes", new object[]
					{
						this.contentPanelType.Name,
						this.panelType.Name
					}), new object[0]), value.GetType().Name);
				}
				base.Add(value);
			}

			// Token: 0x0600616C RID: 24940 RVA: 0x0018EBE6 File Offset: 0x0018CDE6
			public override void Remove(Control value)
			{
				if ((value is ToolStripPanel || value is ToolStripContentPanel) && !this.owner.DesignMode && this.IsReadOnly)
				{
					throw new NotSupportedException(SR.GetString("ReadonlyControlsCollection"));
				}
				base.Remove(value);
			}

			// Token: 0x0600616D RID: 24941 RVA: 0x0018EC24 File Offset: 0x0018CE24
			internal override void SetChildIndexInternal(Control child, int newIndex)
			{
				if (child is ToolStripPanel || child is ToolStripContentPanel)
				{
					if (this.owner.DesignMode)
					{
						return;
					}
					if (this.IsReadOnly)
					{
						throw new NotSupportedException(SR.GetString("ReadonlyControlsCollection"));
					}
				}
				base.SetChildIndexInternal(child, newIndex);
			}

			// Token: 0x04004198 RID: 16792
			private ToolStripContainer owner;

			// Token: 0x04004199 RID: 16793
			private Type contentPanelType = typeof(ToolStripContentPanel);

			// Token: 0x0400419A RID: 16794
			private Type panelType = typeof(ToolStripPanel);
		}
	}
}
