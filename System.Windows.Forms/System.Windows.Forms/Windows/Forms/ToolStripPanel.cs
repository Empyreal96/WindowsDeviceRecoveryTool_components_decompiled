using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Creates a container within which other controls can share horizontal or vertical space.</summary>
	// Token: 0x020003DD RID: 989
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.ToolStripPanelDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ToolboxBitmap(typeof(ToolStripPanel), "ToolStripPanel_standalone.bmp")]
	public class ToolStripPanel : ContainerControl, IArrangedElement, IComponent, IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripPanel" /> class. </summary>
		// Token: 0x060041EF RID: 16879 RVA: 0x0011AA28 File Offset: 0x00118C28
		public ToolStripPanel()
		{
			if (DpiHelper.EnableToolStripHighDpiImprovements)
			{
				this.scaledRowMargin = DpiHelper.LogicalToDeviceUnits(ToolStripPanel.rowMargin, 0);
			}
			base.SuspendLayout();
			base.AutoScaleMode = AutoScaleMode.None;
			this.InitFlowLayout();
			this.AutoSize = true;
			this.MinimumSize = Size.Empty;
			this.state[ToolStripPanel.stateLocked | ToolStripPanel.stateBeginInit | ToolStripPanel.stateChangingZOrder] = false;
			this.TabStop = false;
			ToolStripManager.ToolStripPanels.Add(this);
			base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.Selectable, false);
			base.ResumeLayout(true);
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x0011AAE2 File Offset: 0x00118CE2
		internal ToolStripPanel(ToolStripContainer owner) : this()
		{
			this.owner = owner;
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001088 RID: 4232
		// (get) Token: 0x060041F1 RID: 16881 RVA: 0x000B0BBD File Offset: 0x000AEDBD
		// (set) Token: 0x060041F2 RID: 16882 RVA: 0x000B0BC5 File Offset: 0x000AEDC5
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool AllowDrop
		{
			get
			{
				return base.AllowDrop;
			}
			set
			{
				base.AllowDrop = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001089 RID: 4233
		// (get) Token: 0x060041F3 RID: 16883 RVA: 0x000A87BB File Offset: 0x000A69BB
		// (set) Token: 0x060041F4 RID: 16884 RVA: 0x000E3A46 File Offset: 0x000E1C46
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

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700108A RID: 4234
		// (get) Token: 0x060041F5 RID: 16885 RVA: 0x000F3C48 File Offset: 0x000F1E48
		// (set) Token: 0x060041F6 RID: 16886 RVA: 0x000F3C50 File Offset: 0x000F1E50
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

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700108B RID: 4235
		// (get) Token: 0x060041F7 RID: 16887 RVA: 0x000F3C37 File Offset: 0x000F1E37
		// (set) Token: 0x060041F8 RID: 16888 RVA: 0x000F3C3F File Offset: 0x000F1E3F
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

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripPanel" /> automatically adjusts its size when the form is resized.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripPanel" /> automatically resizes; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700108C RID: 4236
		// (get) Token: 0x060041F9 RID: 16889 RVA: 0x0001BA13 File Offset: 0x00019C13
		// (set) Token: 0x060041FA RID: 16890 RVA: 0x000B0BCE File Offset: 0x000AEDCE
		[DefaultValue(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripPanel.AutoSize" /> property changes. </summary>
		// Token: 0x14000355 RID: 853
		// (add) Token: 0x060041FB RID: 16891 RVA: 0x0001BA2E File Offset: 0x00019C2E
		// (remove) Token: 0x060041FC RID: 16892 RVA: 0x0001BA37 File Offset: 0x00019C37
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		/// <summary>Gets the internal spacing, in pixels, of the contents of a control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> that represents the internal spacing of the contents of a control.</returns>
		// Token: 0x1700108D RID: 4237
		// (get) Token: 0x060041FD RID: 16893 RVA: 0x000119C9 File Offset: 0x0000FBC9
		protected override Padding DefaultPadding
		{
			get
			{
				return Padding.Empty;
			}
		}

		/// <summary>Gets the space, in pixels, that is specified by default between controls.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> that represents the default space between controls.</returns>
		// Token: 0x1700108E RID: 4238
		// (get) Token: 0x060041FE RID: 16894 RVA: 0x000119C9 File Offset: 0x0000FBC9
		protected override Padding DefaultMargin
		{
			get
			{
				return Padding.Empty;
			}
		}

		/// <summary>Gets or sets the spacing, in pixels, between the <see cref="T:System.Windows.Forms.ToolStripPanelRow" />s and the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> value representing the spacing, in pixels.</returns>
		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x060041FF RID: 16895 RVA: 0x0011AAF1 File Offset: 0x00118CF1
		// (set) Token: 0x06004200 RID: 16896 RVA: 0x0011AAF9 File Offset: 0x00118CF9
		public Padding RowMargin
		{
			get
			{
				return this.scaledRowMargin;
			}
			set
			{
				this.scaledRowMargin = value;
				LayoutTransaction.DoLayout(this, this, "RowMargin");
			}
		}

		/// <summary>Gets or sets which control borders are docked to its parent control and determines how a control is resized with its parent.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DockStyle" /> values. The default is None.</returns>
		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x06004201 RID: 16897 RVA: 0x000F3D46 File Offset: 0x000F1F46
		// (set) Token: 0x06004202 RID: 16898 RVA: 0x0011AB0E File Offset: 0x00118D0E
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock = value;
				if (value == DockStyle.Left || value == DockStyle.Right)
				{
					this.Orientation = Orientation.Vertical;
					return;
				}
				this.Orientation = Orientation.Horizontal;
			}
		}

		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x06004203 RID: 16899 RVA: 0x0011AB2E File Offset: 0x00118D2E
		internal Rectangle DragBounds
		{
			get
			{
				return LayoutUtils.InflateRect(base.ClientRectangle, ToolStripPanel.DragMargin);
			}
		}

		// Token: 0x17001092 RID: 4242
		// (get) Token: 0x06004204 RID: 16900 RVA: 0x00105831 File Offset: 0x00103A31
		internal bool IsInDesignMode
		{
			get
			{
				return base.DesignMode;
			}
		}

		/// <summary>Gets a cached instance of the control's layout engine.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Layout.LayoutEngine" /> for the control's contents.</returns>
		// Token: 0x17001093 RID: 4243
		// (get) Token: 0x06004205 RID: 16901 RVA: 0x000A76F4 File Offset: 0x000A58F4
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return FlowLayout.Instance;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripPanel" /> can be moved or resized.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripPanel" /> can be moved or resized; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001094 RID: 4244
		// (get) Token: 0x06004206 RID: 16902 RVA: 0x0011AB40 File Offset: 0x00118D40
		// (set) Token: 0x06004207 RID: 16903 RVA: 0x0011AB52 File Offset: 0x00118D52
		[DefaultValue(false)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool Locked
		{
			get
			{
				return this.state[ToolStripPanel.stateLocked];
			}
			set
			{
				this.state[ToolStripPanel.stateLocked] = value;
			}
		}

		/// <summary>Gets or sets a value indicating the horizontal or vertical orientation of the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.Orientation" /> values.</returns>
		// Token: 0x17001095 RID: 4245
		// (get) Token: 0x06004208 RID: 16904 RVA: 0x0011AB65 File Offset: 0x00118D65
		// (set) Token: 0x06004209 RID: 16905 RVA: 0x0011AB70 File Offset: 0x00118D70
		public Orientation Orientation
		{
			get
			{
				return this.orientation;
			}
			set
			{
				if (this.orientation != value)
				{
					this.orientation = value;
					this.scaledRowMargin = LayoutUtils.FlipPadding(this.scaledRowMargin);
					this.InitFlowLayout();
					foreach (object obj in this.RowsInternal)
					{
						ToolStripPanelRow toolStripPanelRow = (ToolStripPanelRow)obj;
						toolStripPanelRow.OnOrientationChanged();
					}
				}
			}
		}

		// Token: 0x17001096 RID: 4246
		// (get) Token: 0x0600420A RID: 16906 RVA: 0x0011ABF0 File Offset: 0x00118DF0
		private ToolStripRendererSwitcher RendererSwitcher
		{
			get
			{
				if (this.rendererSwitcher == null)
				{
					this.rendererSwitcher = new ToolStripRendererSwitcher(this);
					this.HandleRendererChanged(this, EventArgs.Empty);
					this.rendererSwitcher.RendererChanged += this.HandleRendererChanged;
				}
				return this.rendererSwitcher;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Forms.ToolStripRenderer" /> used to customize the appearance of a <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripRenderer" /> that handles painting.</returns>
		// Token: 0x17001097 RID: 4247
		// (get) Token: 0x0600420B RID: 16907 RVA: 0x0011AC2F File Offset: 0x00118E2F
		// (set) Token: 0x0600420C RID: 16908 RVA: 0x0011AC3C File Offset: 0x00118E3C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ToolStripRenderer Renderer
		{
			get
			{
				return this.RendererSwitcher.Renderer;
			}
			set
			{
				this.RendererSwitcher.Renderer = value;
			}
		}

		/// <summary>Gets or sets the painting styles to be applied to the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripRenderMode" /> values.</returns>
		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x0600420D RID: 16909 RVA: 0x0011AC4A File Offset: 0x00118E4A
		// (set) Token: 0x0600420E RID: 16910 RVA: 0x0011AC57 File Offset: 0x00118E57
		[SRDescription("ToolStripRenderModeDescr")]
		[SRCategory("CatAppearance")]
		public ToolStripRenderMode RenderMode
		{
			get
			{
				return this.RendererSwitcher.RenderMode;
			}
			set
			{
				this.RendererSwitcher.RenderMode = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripPanel.Renderer" /> property changes.</summary>
		// Token: 0x14000356 RID: 854
		// (add) Token: 0x0600420F RID: 16911 RVA: 0x0011AC65 File Offset: 0x00118E65
		// (remove) Token: 0x06004210 RID: 16912 RVA: 0x0011AC78 File Offset: 0x00118E78
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripRendererChanged")]
		public event EventHandler RendererChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripPanel.EventRendererChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripPanel.EventRendererChanged, value);
			}
		}

		// Token: 0x17001099 RID: 4249
		// (get) Token: 0x06004211 RID: 16913 RVA: 0x0011AC8C File Offset: 0x00118E8C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRDescription("ToolStripPanelRowsDescr")]
		internal ToolStripPanel.ToolStripPanelRowCollection RowsInternal
		{
			get
			{
				ToolStripPanel.ToolStripPanelRowCollection toolStripPanelRowCollection = (ToolStripPanel.ToolStripPanelRowCollection)base.Properties.GetObject(ToolStripPanel.PropToolStripPanelRowCollection);
				if (toolStripPanelRowCollection == null)
				{
					toolStripPanelRowCollection = this.CreateToolStripPanelRowCollection();
					base.Properties.SetObject(ToolStripPanel.PropToolStripPanelRowCollection, toolStripPanelRowCollection);
				}
				return toolStripPanelRowCollection;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ToolStripPanelRow" />s in this <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" /> representing the <see cref="T:System.Windows.Forms.ToolStripPanelRow" />s in this <see cref="T:System.Windows.Forms.ToolStripPanel" />.</returns>
		// Token: 0x1700109A RID: 4250
		// (get) Token: 0x06004212 RID: 16914 RVA: 0x0011ACCC File Offset: 0x00118ECC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ToolStripPanelRowsDescr")]
		public ToolStripPanelRow[] Rows
		{
			get
			{
				ToolStripPanelRow[] array = new ToolStripPanelRow[this.RowsInternal.Count];
				this.RowsInternal.CopyTo(array, 0);
				return array;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the tab index.</returns>
		// Token: 0x1700109B RID: 4251
		// (get) Token: 0x06004213 RID: 16915 RVA: 0x000AA0F2 File Offset: 0x000A82F2
		// (set) Token: 0x06004214 RID: 16916 RVA: 0x000AA0FA File Offset: 0x000A82FA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new int TabIndex
		{
			get
			{
				return base.TabIndex;
			}
			set
			{
				base.TabIndex = value;
			}
		}

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x14000357 RID: 855
		// (add) Token: 0x06004215 RID: 16917 RVA: 0x000AA103 File Offset: 0x000A8303
		// (remove) Token: 0x06004216 RID: 16918 RVA: 0x000AA10C File Offset: 0x000A830C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TabIndexChanged
		{
			add
			{
				base.TabIndexChanged += value;
			}
			remove
			{
				base.TabIndexChanged -= value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700109C RID: 4252
		// (get) Token: 0x06004217 RID: 16919 RVA: 0x000AA115 File Offset: 0x000A8315
		// (set) Token: 0x06004218 RID: 16920 RVA: 0x0011ACF8 File Offset: 0x00118EF8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				if (AccessibilityImprovements.Level2)
				{
					base.SetStyle(ControlStyles.Selectable, value);
				}
				base.TabStop = value;
			}
		}

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x14000358 RID: 856
		// (add) Token: 0x06004219 RID: 16921 RVA: 0x000AA126 File Offset: 0x000A8326
		// (remove) Token: 0x0600421A RID: 16922 RVA: 0x000AA12F File Offset: 0x000A832F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TabStopChanged
		{
			add
			{
				base.TabStopChanged += value;
			}
			remove
			{
				base.TabStopChanged -= value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>A <see cref="T:System.String" /> representing the display text.</returns>
		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x0600421B RID: 16923 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x0600421C RID: 16924 RVA: 0x0001BFAD File Offset: 0x0001A1AD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x14000359 RID: 857
		// (add) Token: 0x0600421D RID: 16925 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x0600421E RID: 16926 RVA: 0x0003E43E File Offset: 0x0003C63E
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

		/// <summary>Begins the initialization of a <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
		// Token: 0x0600421F RID: 16927 RVA: 0x0011AD14 File Offset: 0x00118F14
		public void BeginInit()
		{
			this.state[ToolStripPanel.stateBeginInit] = true;
		}

		/// <summary>Ends the initialization of a <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
		// Token: 0x06004220 RID: 16928 RVA: 0x0011AD28 File Offset: 0x00118F28
		public void EndInit()
		{
			this.state[ToolStripPanel.stateBeginInit] = false;
			this.state[ToolStripPanel.stateEndInit] = true;
			try
			{
				if (!this.state[ToolStripPanel.stateInJoin])
				{
					this.JoinControls();
				}
			}
			finally
			{
				this.state[ToolStripPanel.stateEndInit] = false;
			}
		}

		// Token: 0x06004221 RID: 16929 RVA: 0x0011AD94 File Offset: 0x00118F94
		private ToolStripPanel.ToolStripPanelRowCollection CreateToolStripPanelRowCollection()
		{
			return new ToolStripPanel.ToolStripPanelRowCollection(this);
		}

		/// <summary>Retrieves a collection of <see cref="T:System.Windows.Forms.ToolStripPanel" /> controls.</summary>
		/// <returns>A collection of <see cref="T:System.Windows.Forms.ToolStripPanel" /> controls.</returns>
		// Token: 0x06004222 RID: 16930 RVA: 0x0011AD9C File Offset: 0x00118F9C
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new ToolStripPanel.ToolStripPanelControlCollection(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.ToolStripPanel" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06004223 RID: 16931 RVA: 0x0011ADA4 File Offset: 0x00118FA4
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ToolStripManager.ToolStripPanels.Remove(this);
			}
			base.Dispose(disposing);
		}

		// Token: 0x06004224 RID: 16932 RVA: 0x0011ADBB File Offset: 0x00118FBB
		private void InitFlowLayout()
		{
			if (this.Orientation == Orientation.Horizontal)
			{
				FlowLayout.SetFlowDirection(this, FlowDirection.TopDown);
			}
			else
			{
				FlowLayout.SetFlowDirection(this, FlowDirection.LeftToRight);
			}
			FlowLayout.SetWrapContents(this, false);
		}

		// Token: 0x06004225 RID: 16933 RVA: 0x0011ADDC File Offset: 0x00118FDC
		private Point GetStartLocation(ToolStrip toolStripToDrag)
		{
			if (toolStripToDrag.IsCurrentlyDragging && this.Orientation == Orientation.Horizontal && toolStripToDrag.RightToLeft == RightToLeft.Yes)
			{
				return new Point(toolStripToDrag.Right, toolStripToDrag.Top);
			}
			return toolStripToDrag.Location;
		}

		// Token: 0x06004226 RID: 16934 RVA: 0x0011AE0F File Offset: 0x0011900F
		private void HandleRendererChanged(object sender, EventArgs e)
		{
			this.OnRendererChanged(e);
		}

		/// <summary>Paints the background of the control.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		// Token: 0x06004227 RID: 16935 RVA: 0x0011AE18 File Offset: 0x00119018
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			ToolStripPanelRenderEventArgs toolStripPanelRenderEventArgs = new ToolStripPanelRenderEventArgs(e.Graphics, this);
			this.Renderer.DrawToolStripPanelBackground(toolStripPanelRenderEventArgs);
			if (!toolStripPanelRenderEventArgs.Handled)
			{
				base.OnPaintBackground(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStrip.ControlAdded" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ControlEventArgs" /> that contains the event data.</param>
		// Token: 0x06004228 RID: 16936 RVA: 0x0011AE50 File Offset: 0x00119050
		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			if (!this.state[ToolStripPanel.stateBeginInit] && !this.state[ToolStripPanel.stateInJoin])
			{
				if (!this.state[ToolStripPanel.stateLayoutSuspended])
				{
					this.Join(e.Control as ToolStrip, e.Control.Location);
					return;
				}
				this.BeginInit();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStrip.ControlRemoved" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ControlEventArgs" /> that contains the event data.</param>
		// Token: 0x06004229 RID: 16937 RVA: 0x0011AEC0 File Offset: 0x001190C0
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			ISupportToolStripPanel supportToolStripPanel = e.Control as ISupportToolStripPanel;
			if (supportToolStripPanel != null && supportToolStripPanel.ToolStripPanelRow != null)
			{
				supportToolStripPanel.ToolStripPanelRow.ControlsInternal.Remove(e.Control);
			}
			base.OnControlRemoved(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data.</param>
		// Token: 0x0600422A RID: 16938 RVA: 0x0011AF04 File Offset: 0x00119104
		protected override void OnLayout(LayoutEventArgs e)
		{
			if (e.AffectedComponent != this.ParentInternal && e.AffectedComponent is Control)
			{
				ISupportToolStripPanel supportToolStripPanel = e.AffectedComponent as ISupportToolStripPanel;
				if (supportToolStripPanel != null && this.RowsInternal.Contains(supportToolStripPanel.ToolStripPanelRow))
				{
					LayoutTransaction.DoLayout(supportToolStripPanel.ToolStripPanelRow, e.AffectedComponent as IArrangedElement, e.AffectedProperty);
				}
			}
			base.OnLayout(e);
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x0011AF71 File Offset: 0x00119171
		internal override void OnLayoutSuspended()
		{
			base.OnLayoutSuspended();
			this.state[ToolStripPanel.stateLayoutSuspended] = true;
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x0011AF8A File Offset: 0x0011918A
		internal override void OnLayoutResuming(bool resumeLayout)
		{
			base.OnLayoutResuming(resumeLayout);
			this.state[ToolStripPanel.stateLayoutSuspended] = false;
			if (this.state[ToolStripPanel.stateBeginInit])
			{
				this.EndInit();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.RightToLeftChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600422D RID: 16941 RVA: 0x0011AFBC File Offset: 0x001191BC
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			if (!this.state[ToolStripPanel.stateBeginInit])
			{
				if (base.Controls.Count > 0)
				{
					base.SuspendLayout();
					Control[] array = new Control[base.Controls.Count];
					Point[] array2 = new Point[base.Controls.Count];
					int num = 0;
					foreach (object obj in this.RowsInternal)
					{
						ToolStripPanelRow toolStripPanelRow = (ToolStripPanelRow)obj;
						foreach (object obj2 in toolStripPanelRow.ControlsInternal)
						{
							Control control = (Control)obj2;
							array[num] = control;
							array2[num] = new Point(toolStripPanelRow.Bounds.Width - control.Right, control.Top);
							num++;
						}
					}
					base.Controls.Clear();
					for (int i = 0; i < array.Length; i++)
					{
						this.Join(array[i] as ToolStrip, array2[i]);
					}
					base.ResumeLayout(true);
					return;
				}
			}
			else
			{
				this.state[ToolStripPanel.stateRightToLeftChanged] = true;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripPanel.RendererChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600422E RID: 16942 RVA: 0x0011B138 File Offset: 0x00119338
		protected virtual void OnRendererChanged(EventArgs e)
		{
			this.Renderer.InitializePanel(this);
			base.Invalidate();
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripPanel.EventRendererChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.OnParentChanged(System.EventArgs)" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x0600422F RID: 16943 RVA: 0x0011B178 File Offset: 0x00119378
		protected override void OnParentChanged(EventArgs e)
		{
			this.PerformUpdate();
			base.OnParentChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DockChanged" /> event. </summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06004230 RID: 16944 RVA: 0x0011B187 File Offset: 0x00119387
		protected override void OnDockChanged(EventArgs e)
		{
			this.PerformUpdate();
			base.OnDockChanged(e);
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x0011B196 File Offset: 0x00119396
		internal void PerformUpdate()
		{
			this.PerformUpdate(false);
		}

		// Token: 0x06004232 RID: 16946 RVA: 0x0011B19F File Offset: 0x0011939F
		internal void PerformUpdate(bool forceLayout)
		{
			if (!this.state[ToolStripPanel.stateBeginInit] && !this.state[ToolStripPanel.stateInJoin])
			{
				this.JoinControls(forceLayout);
			}
		}

		// Token: 0x06004233 RID: 16947 RVA: 0x0011B1CC File Offset: 0x001193CC
		private void ResetRenderMode()
		{
			this.RendererSwitcher.ResetRenderMode();
		}

		// Token: 0x06004234 RID: 16948 RVA: 0x0011B1D9 File Offset: 0x001193D9
		private bool ShouldSerializeRenderMode()
		{
			return this.RendererSwitcher.ShouldSerializeRenderMode();
		}

		// Token: 0x06004235 RID: 16949 RVA: 0x0011B1E6 File Offset: 0x001193E6
		private bool ShouldSerializeDock()
		{
			return this.owner == null && this.Dock > DockStyle.None;
		}

		// Token: 0x06004236 RID: 16950 RVA: 0x0011B1FB File Offset: 0x001193FB
		private void JoinControls()
		{
			this.JoinControls(false);
		}

		// Token: 0x06004237 RID: 16951 RVA: 0x0011B204 File Offset: 0x00119404
		private void JoinControls(bool forceLayout)
		{
			ToolStripPanel.ToolStripPanelControlCollection toolStripPanelControlCollection = base.Controls as ToolStripPanel.ToolStripPanelControlCollection;
			if (toolStripPanelControlCollection.Count > 0)
			{
				toolStripPanelControlCollection.Sort();
				Control[] array = new Control[toolStripPanelControlCollection.Count];
				toolStripPanelControlCollection.CopyTo(array, 0);
				int i = 0;
				while (i < array.Length)
				{
					int count = this.RowsInternal.Count;
					ISupportToolStripPanel supportToolStripPanel = array[i] as ISupportToolStripPanel;
					if (supportToolStripPanel == null || supportToolStripPanel.ToolStripPanelRow == null || supportToolStripPanel.IsCurrentlyDragging)
					{
						goto IL_8B;
					}
					ToolStripPanelRow toolStripPanelRow = supportToolStripPanel.ToolStripPanelRow;
					if (!toolStripPanelRow.Bounds.Contains(array[i].Location))
					{
						goto IL_8B;
					}
					IL_117:
					i++;
					continue;
					IL_8B:
					if (array[i].AutoSize)
					{
						array[i].Size = array[i].PreferredSize;
					}
					Point location = array[i].Location;
					if (this.state[ToolStripPanel.stateRightToLeftChanged])
					{
						location = new Point(base.Width - array[i].Right, location.Y);
					}
					this.Join(array[i] as ToolStrip, array[i].Location);
					if (count < this.RowsInternal.Count || forceLayout)
					{
						this.OnLayout(new LayoutEventArgs(this, PropertyNames.Rows));
						goto IL_117;
					}
					goto IL_117;
				}
			}
			this.state[ToolStripPanel.stateRightToLeftChanged] = false;
		}

		// Token: 0x06004238 RID: 16952 RVA: 0x0011B348 File Offset: 0x00119548
		private void GiveToolStripPanelFeedback(ToolStrip toolStripToDrag, Point screenLocation)
		{
			if (this.Orientation == Orientation.Horizontal && this.RightToLeft == RightToLeft.Yes)
			{
				screenLocation.Offset(-toolStripToDrag.Width, 0);
			}
			if (ToolStripPanel.CurrentFeedbackRect == null)
			{
				ToolStripPanel.CurrentFeedbackRect = new ToolStripPanel.FeedbackRectangle(toolStripToDrag.ClientRectangle);
			}
			if (!ToolStripPanel.CurrentFeedbackRect.Visible)
			{
				toolStripToDrag.SuspendCaputureMode();
				try
				{
					ToolStripPanel.CurrentFeedbackRect.Show(screenLocation);
					toolStripToDrag.CaptureInternal = true;
					return;
				}
				finally
				{
					toolStripToDrag.ResumeCaputureMode();
				}
			}
			ToolStripPanel.CurrentFeedbackRect.Move(screenLocation);
		}

		// Token: 0x06004239 RID: 16953 RVA: 0x0011B3D4 File Offset: 0x001195D4
		internal static void ClearDragFeedback()
		{
			ToolStripPanel.FeedbackRectangle feedbackRectangle = ToolStripPanel.feedbackRect;
			ToolStripPanel.feedbackRect = null;
			if (feedbackRectangle != null)
			{
				feedbackRectangle.Dispose();
			}
		}

		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x0600423A RID: 16954 RVA: 0x0011B3F6 File Offset: 0x001195F6
		// (set) Token: 0x0600423B RID: 16955 RVA: 0x0011B3FD File Offset: 0x001195FD
		private static ToolStripPanel.FeedbackRectangle CurrentFeedbackRect
		{
			get
			{
				return ToolStripPanel.feedbackRect;
			}
			set
			{
				ToolStripPanel.feedbackRect = value;
			}
		}

		/// <summary>Adds the specified <see cref="T:System.Windows.Forms.ToolStrip" /> to a <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
		/// <param name="toolStripToDrag">The <see cref="T:System.Windows.Forms.ToolStrip" /> to add to the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</param>
		// Token: 0x0600423C RID: 16956 RVA: 0x0011B405 File Offset: 0x00119605
		public void Join(ToolStrip toolStripToDrag)
		{
			this.Join(toolStripToDrag, Point.Empty);
		}

		/// <summary>Adds the specified <see cref="T:System.Windows.Forms.ToolStrip" /> to a <see cref="T:System.Windows.Forms.ToolStripPanel" /> in the specified row.</summary>
		/// <param name="toolStripToDrag">The <see cref="T:System.Windows.Forms.ToolStrip" /> to add to the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</param>
		/// <param name="row">An <see cref="T:System.Int32" /> representing the <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> to which the <see cref="T:System.Windows.Forms.ToolStrip" /> is added.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="row" /> parameter is less than zero (0).</exception>
		// Token: 0x0600423D RID: 16957 RVA: 0x0011B414 File Offset: 0x00119614
		public void Join(ToolStrip toolStripToDrag, int row)
		{
			if (row < 0)
			{
				throw new ArgumentOutOfRangeException("row", SR.GetString("IndexOutOfRange", new object[]
				{
					row.ToString(CultureInfo.CurrentCulture)
				}));
			}
			Point empty = Point.Empty;
			Rectangle rectangle = Rectangle.Empty;
			if (row >= this.RowsInternal.Count)
			{
				rectangle = this.DragBounds;
			}
			else
			{
				rectangle = this.RowsInternal[row].DragBounds;
			}
			if (this.Orientation == Orientation.Horizontal)
			{
				empty = new Point(0, rectangle.Bottom - 1);
			}
			else
			{
				empty = new Point(rectangle.Right - 1, 0);
			}
			this.Join(toolStripToDrag, empty);
		}

		/// <summary>Adds the specified <see cref="T:System.Windows.Forms.ToolStrip" /> to a <see cref="T:System.Windows.Forms.ToolStripPanel" /> at the specified coordinates.</summary>
		/// <param name="toolStripToDrag">The <see cref="T:System.Windows.Forms.ToolStrip" /> to add to the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</param>
		/// <param name="x">The horizontal client coordinate, in pixels.</param>
		/// <param name="y">The vertical client coordinate, in pixels.</param>
		// Token: 0x0600423E RID: 16958 RVA: 0x0011B4B8 File Offset: 0x001196B8
		public void Join(ToolStrip toolStripToDrag, int x, int y)
		{
			this.Join(toolStripToDrag, new Point(x, y));
		}

		/// <summary>Adds the specified <see cref="T:System.Windows.Forms.ToolStrip" /> to a <see cref="T:System.Windows.Forms.ToolStripPanel" /> at the specified location.</summary>
		/// <param name="toolStripToDrag">The <see cref="T:System.Windows.Forms.ToolStrip" /> to add to the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</param>
		/// <param name="location">A <see cref="T:System.Drawing.Point" /> value representing the x- and y-client coordinates, in pixels, of the new location for the <see cref="T:System.Windows.Forms.ToolStrip" />.</param>
		// Token: 0x0600423F RID: 16959 RVA: 0x0011B4C8 File Offset: 0x001196C8
		public void Join(ToolStrip toolStripToDrag, Point location)
		{
			if (toolStripToDrag == null)
			{
				throw new ArgumentNullException("toolStripToDrag");
			}
			if (!this.state[ToolStripPanel.stateBeginInit] && !this.state[ToolStripPanel.stateInJoin])
			{
				try
				{
					this.state[ToolStripPanel.stateInJoin] = true;
					toolStripToDrag.ParentInternal = this;
					this.MoveInsideContainer(toolStripToDrag, location);
					return;
				}
				finally
				{
					this.state[ToolStripPanel.stateInJoin] = false;
				}
			}
			base.Controls.Add(toolStripToDrag);
			toolStripToDrag.Location = location;
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x0011B560 File Offset: 0x00119760
		internal void MoveControl(ToolStrip toolStripToDrag, Point screenLocation)
		{
			if (toolStripToDrag == null)
			{
				return;
			}
			Point point = base.PointToClient(screenLocation);
			if (!this.DragBounds.Contains(point))
			{
				this.MoveOutsideContainer(toolStripToDrag, screenLocation);
				return;
			}
			this.Join(toolStripToDrag, point);
		}

		// Token: 0x06004241 RID: 16961 RVA: 0x0011B5A0 File Offset: 0x001197A0
		private void MoveInsideContainer(ToolStrip toolStripToDrag, Point clientLocation)
		{
			if (((ISupportToolStripPanel)toolStripToDrag).IsCurrentlyDragging && !this.DragBounds.Contains(clientLocation))
			{
				return;
			}
			ToolStripPanel.ClearDragFeedback();
			if (toolStripToDrag.Site != null && toolStripToDrag.Site.DesignMode && base.IsHandleCreated && (clientLocation.X < 0 || clientLocation.Y < 0))
			{
				Point point = base.PointToClient(WindowsFormsUtils.LastCursorPoint);
				if (base.ClientRectangle.Contains(point))
				{
					clientLocation = point;
				}
			}
			ToolStripPanelRow toolStripPanelRow = ((ISupportToolStripPanel)toolStripToDrag).ToolStripPanelRow;
			bool flag = false;
			if (toolStripPanelRow != null && toolStripPanelRow.Visible && toolStripPanelRow.ToolStripPanel == this)
			{
				if (toolStripToDrag.IsCurrentlyDragging)
				{
					flag = toolStripPanelRow.DragBounds.Contains(clientLocation);
				}
				else
				{
					flag = toolStripPanelRow.Bounds.Contains(clientLocation);
				}
			}
			if (flag)
			{
				((ISupportToolStripPanel)toolStripToDrag).ToolStripPanelRow.MoveControl(toolStripToDrag, this.GetStartLocation(toolStripToDrag), clientLocation);
				return;
			}
			ToolStripPanelRow toolStripPanelRow2 = this.PointToRow(clientLocation);
			if (toolStripPanelRow2 == null)
			{
				int num = this.RowsInternal.Count;
				if (this.Orientation == Orientation.Horizontal)
				{
					num = ((clientLocation.Y <= base.Padding.Left) ? 0 : num);
				}
				else
				{
					num = ((clientLocation.X <= base.Padding.Left) ? 0 : num);
				}
				ToolStripPanelRow toolStripPanelRow3 = null;
				if (this.RowsInternal.Count > 0)
				{
					if (num == 0)
					{
						toolStripPanelRow3 = this.RowsInternal[0];
					}
					else if (num > 0)
					{
						toolStripPanelRow3 = this.RowsInternal[num - 1];
					}
				}
				if (toolStripPanelRow3 != null && toolStripPanelRow3.ControlsInternal.Count == 1 && toolStripPanelRow3.ControlsInternal.Contains(toolStripToDrag))
				{
					toolStripPanelRow2 = toolStripPanelRow3;
					if (toolStripToDrag.IsInDesignMode)
					{
						Point endClientLocation = (this.Orientation == Orientation.Horizontal) ? new Point(clientLocation.X, toolStripPanelRow2.Bounds.Y) : new Point(toolStripPanelRow2.Bounds.X, clientLocation.Y);
						((ISupportToolStripPanel)toolStripToDrag).ToolStripPanelRow.MoveControl(toolStripToDrag, this.GetStartLocation(toolStripToDrag), endClientLocation);
					}
				}
				else
				{
					toolStripPanelRow2 = new ToolStripPanelRow(this);
					this.RowsInternal.Insert(num, toolStripPanelRow2);
				}
			}
			else if (!toolStripPanelRow2.CanMove(toolStripToDrag))
			{
				int num2 = this.RowsInternal.IndexOf(toolStripPanelRow2);
				if (toolStripPanelRow != null && toolStripPanelRow.ControlsInternal.Count == 1 && num2 > 0 && num2 - 1 == this.RowsInternal.IndexOf(toolStripPanelRow))
				{
					return;
				}
				toolStripPanelRow2 = new ToolStripPanelRow(this);
				this.RowsInternal.Insert(num2, toolStripPanelRow2);
				clientLocation.Y = toolStripPanelRow2.Bounds.Y;
			}
			bool flag2 = toolStripPanelRow != toolStripPanelRow2;
			if (!flag2 && toolStripPanelRow != null && toolStripPanelRow.ControlsInternal.Count > 1)
			{
				toolStripPanelRow.LeaveRow(toolStripToDrag);
				toolStripPanelRow = null;
				flag2 = true;
			}
			if (flag2)
			{
				if (toolStripPanelRow != null)
				{
					toolStripPanelRow.LeaveRow(toolStripToDrag);
				}
				toolStripPanelRow2.JoinRow(toolStripToDrag, clientLocation);
			}
			if (flag2 && ((ISupportToolStripPanel)toolStripToDrag).IsCurrentlyDragging)
			{
				for (int i = 0; i < this.RowsInternal.Count; i++)
				{
					LayoutTransaction.DoLayout(this.RowsInternal[i], this, PropertyNames.Rows);
				}
				if (this.RowsInternal.IndexOf(toolStripPanelRow2) > 0)
				{
					IntSecurity.AdjustCursorPosition.Assert();
					try
					{
						Point position = toolStripToDrag.PointToScreen(toolStripToDrag.GripRectangle.Location);
						if (this.Orientation == Orientation.Vertical)
						{
							position.X += toolStripToDrag.GripRectangle.Width / 2;
							position.Y = Cursor.Position.Y;
						}
						else
						{
							position.Y += toolStripToDrag.GripRectangle.Height / 2;
							position.X = Cursor.Position.X;
						}
						Cursor.Position = position;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
		}

		// Token: 0x06004242 RID: 16962 RVA: 0x0011B98C File Offset: 0x00119B8C
		private void MoveOutsideContainer(ToolStrip toolStripToDrag, Point screenLocation)
		{
			ToolStripPanel toolStripPanel = ToolStripManager.ToolStripPanelFromPoint(toolStripToDrag, screenLocation);
			if (toolStripPanel != null)
			{
				using (new LayoutTransaction(toolStripPanel, toolStripPanel, null))
				{
					toolStripPanel.MoveControl(toolStripToDrag, screenLocation);
				}
				toolStripToDrag.PerformLayout();
				return;
			}
			this.GiveToolStripPanelFeedback(toolStripToDrag, screenLocation);
		}

		/// <summary>Retrieves the <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> given a point within the <see cref="T:System.Windows.Forms.ToolStripPanel" /> client area.</summary>
		/// <param name="clientLocation">A <see cref="T:System.Drawing.Point" /> used as a reference to find the <see cref="T:System.Windows.Forms.ToolStripPanelRow" />.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> that contains the <paramref name="raftingContainerPoint" />, or <see langword="null" /> if no such <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> exists.</returns>
		// Token: 0x06004243 RID: 16963 RVA: 0x0011B9E0 File Offset: 0x00119BE0
		public ToolStripPanelRow PointToRow(Point clientLocation)
		{
			foreach (object obj in this.RowsInternal)
			{
				ToolStripPanelRow toolStripPanelRow = (ToolStripPanelRow)obj;
				Rectangle rectangle = LayoutUtils.InflateRect(toolStripPanelRow.Bounds, toolStripPanelRow.Margin);
				if (this.ParentInternal != null)
				{
					if (this.Orientation == Orientation.Horizontal && rectangle.Width == 0)
					{
						rectangle.Width = this.ParentInternal.DisplayRectangle.Width;
					}
					else if (this.Orientation == Orientation.Vertical && rectangle.Height == 0)
					{
						rectangle.Height = this.ParentInternal.DisplayRectangle.Height;
					}
				}
				if (rectangle.Contains(clientLocation))
				{
					return toolStripPanelRow;
				}
			}
			return null;
		}

		// Token: 0x06004244 RID: 16964 RVA: 0x0011BAC4 File Offset: 0x00119CC4
		[Conditional("DEBUG")]
		private void Debug_VerifyOneToOneCellRowControlMatchup()
		{
			for (int i = 0; i < this.RowsInternal.Count; i++)
			{
				ToolStripPanelRow toolStripPanelRow = this.RowsInternal[i];
				foreach (object obj in toolStripPanelRow.Cells)
				{
					ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)obj;
					if (toolStripPanelCell.Control != null)
					{
						ToolStripPanelRow toolStripPanelRow2 = ((ISupportToolStripPanel)toolStripPanelCell.Control).ToolStripPanelRow;
						if (toolStripPanelRow2 != toolStripPanelRow)
						{
							int num = (toolStripPanelRow2 != null) ? this.RowsInternal.IndexOf(toolStripPanelRow2) : -1;
						}
					}
				}
			}
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x0011BB7C File Offset: 0x00119D7C
		[Conditional("DEBUG")]
		private void Debug_PrintRows()
		{
			for (int i = 0; i < this.RowsInternal.Count; i++)
			{
				for (int j = 0; j < this.RowsInternal[i].ControlsInternal.Count; j++)
				{
				}
			}
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x0000701A File Offset: 0x0000521A
		[Conditional("DEBUG")]
		private void Debug_VerifyCountRows()
		{
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x0011BBC0 File Offset: 0x00119DC0
		[Conditional("DEBUG")]
		private void Debug_VerifyNoOverlaps()
		{
			foreach (object obj in base.Controls)
			{
				Control control = (Control)obj;
				foreach (object obj2 in base.Controls)
				{
					Control control2 = (Control)obj2;
					if (control != control2)
					{
						Rectangle bounds = control.Bounds;
						bounds.Intersect(control2.Bounds);
						if (!LayoutUtils.IsZeroWidthOrHeight(bounds))
						{
							ISupportToolStripPanel supportToolStripPanel = control as ISupportToolStripPanel;
							ISupportToolStripPanel supportToolStripPanel2 = control2 as ISupportToolStripPanel;
							string str = string.Format(CultureInfo.CurrentCulture, "OVERLAP detection:\r\n{0}: {1} row {2} row bounds {3}", new object[]
							{
								(control.Name == null) ? "" : control.Name,
								control.Bounds,
								(!this.RowsInternal.Contains(supportToolStripPanel.ToolStripPanelRow)) ? "unknown" : this.RowsInternal.IndexOf(supportToolStripPanel.ToolStripPanelRow).ToString(CultureInfo.CurrentCulture),
								supportToolStripPanel.ToolStripPanelRow.Bounds
							});
							str += string.Format(CultureInfo.CurrentCulture, "\r\n{0}: {1} row {2} row bounds {3}", new object[]
							{
								(control2.Name == null) ? "" : control2.Name,
								control2.Bounds,
								(!this.RowsInternal.Contains(supportToolStripPanel2.ToolStripPanelRow)) ? "unknown" : this.RowsInternal.IndexOf(supportToolStripPanel2.ToolStripPanelRow).ToString(CultureInfo.CurrentCulture),
								supportToolStripPanel2.ToolStripPanelRow.Bounds
							});
						}
					}
				}
			}
		}

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x06004248 RID: 16968 RVA: 0x0011BDDC File Offset: 0x00119FDC
		ArrangedElementCollection IArrangedElement.Children
		{
			get
			{
				return this.RowsInternal;
			}
		}

		// Token: 0x04002526 RID: 9510
		private Orientation orientation;

		// Token: 0x04002527 RID: 9511
		private static readonly Padding rowMargin = new Padding(3, 0, 0, 0);

		// Token: 0x04002528 RID: 9512
		private Padding scaledRowMargin = ToolStripPanel.rowMargin;

		// Token: 0x04002529 RID: 9513
		private ToolStripRendererSwitcher rendererSwitcher;

		// Token: 0x0400252A RID: 9514
		private Type currentRendererType = typeof(Type);

		// Token: 0x0400252B RID: 9515
		private BitVector32 state;

		// Token: 0x0400252C RID: 9516
		private ToolStripContainer owner;

		// Token: 0x0400252D RID: 9517
		internal static TraceSwitch ToolStripPanelDebug;

		// Token: 0x0400252E RID: 9518
		internal static TraceSwitch ToolStripPanelFeedbackDebug;

		// Token: 0x0400252F RID: 9519
		internal static TraceSwitch ToolStripPanelMissingRowDebug;

		// Token: 0x04002530 RID: 9520
		[ThreadStatic]
		private static Rectangle lastFeedbackRect = Rectangle.Empty;

		// Token: 0x04002531 RID: 9521
		private static readonly int PropToolStripPanelRowCollection = PropertyStore.CreateKey();

		// Token: 0x04002532 RID: 9522
		private static readonly int stateLocked = BitVector32.CreateMask();

		// Token: 0x04002533 RID: 9523
		private static readonly int stateBeginInit = BitVector32.CreateMask(ToolStripPanel.stateLocked);

		// Token: 0x04002534 RID: 9524
		private static readonly int stateChangingZOrder = BitVector32.CreateMask(ToolStripPanel.stateBeginInit);

		// Token: 0x04002535 RID: 9525
		private static readonly int stateInJoin = BitVector32.CreateMask(ToolStripPanel.stateChangingZOrder);

		// Token: 0x04002536 RID: 9526
		private static readonly int stateEndInit = BitVector32.CreateMask(ToolStripPanel.stateInJoin);

		// Token: 0x04002537 RID: 9527
		private static readonly int stateLayoutSuspended = BitVector32.CreateMask(ToolStripPanel.stateEndInit);

		// Token: 0x04002538 RID: 9528
		private static readonly int stateRightToLeftChanged = BitVector32.CreateMask(ToolStripPanel.stateLayoutSuspended);

		// Token: 0x04002539 RID: 9529
		internal static readonly Padding DragMargin = new Padding(10);

		// Token: 0x0400253A RID: 9530
		private static readonly object EventRendererChanged = new object();

		// Token: 0x0400253B RID: 9531
		[ThreadStatic]
		private static ToolStripPanel.FeedbackRectangle feedbackRect;

		// Token: 0x02000743 RID: 1859
		private class FeedbackRectangle : IDisposable
		{
			// Token: 0x0600616E RID: 24942 RVA: 0x0018EC64 File Offset: 0x0018CE64
			public FeedbackRectangle(Rectangle bounds)
			{
				this.dropDown = new ToolStripPanel.FeedbackRectangle.FeedbackDropDown(bounds);
			}

			// Token: 0x17001747 RID: 5959
			// (get) Token: 0x0600616F RID: 24943 RVA: 0x0018EC78 File Offset: 0x0018CE78
			// (set) Token: 0x06006170 RID: 24944 RVA: 0x0018EC9C File Offset: 0x0018CE9C
			public bool Visible
			{
				get
				{
					return this.dropDown != null && !this.dropDown.IsDisposed && this.dropDown.Visible;
				}
				set
				{
					if (this.dropDown != null && !this.dropDown.IsDisposed)
					{
						this.dropDown.Visible = value;
					}
				}
			}

			// Token: 0x06006171 RID: 24945 RVA: 0x0018ECBF File Offset: 0x0018CEBF
			public void Show(Point newLocation)
			{
				this.dropDown.Show(newLocation);
			}

			// Token: 0x06006172 RID: 24946 RVA: 0x0018ECCD File Offset: 0x0018CECD
			public void Move(Point newLocation)
			{
				this.dropDown.MoveTo(newLocation);
			}

			// Token: 0x06006173 RID: 24947 RVA: 0x0018ECDB File Offset: 0x0018CEDB
			protected void Dispose(bool disposing)
			{
				if (disposing && this.dropDown != null)
				{
					this.Visible = false;
					this.dropDown.Dispose();
					this.dropDown = null;
				}
			}

			// Token: 0x06006174 RID: 24948 RVA: 0x0018ED01 File Offset: 0x0018CF01
			public void Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x06006175 RID: 24949 RVA: 0x0018ED0C File Offset: 0x0018CF0C
			~FeedbackRectangle()
			{
				this.Dispose(false);
			}

			// Token: 0x0400419B RID: 16795
			private ToolStripPanel.FeedbackRectangle.FeedbackDropDown dropDown;

			// Token: 0x020008A1 RID: 2209
			private class FeedbackDropDown : ToolStripDropDown
			{
				// Token: 0x060070E5 RID: 28901 RVA: 0x0019C7F8 File Offset: 0x0019A9F8
				public FeedbackDropDown(Rectangle bounds)
				{
					base.SetStyle(ControlStyles.AllPaintingInWmPaint, false);
					base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
					base.SetStyle(ControlStyles.CacheText, true);
					base.AutoClose = false;
					this.AutoSize = false;
					base.DropShadowEnabled = false;
					base.Bounds = bounds;
					Rectangle rect = bounds;
					rect.Inflate(-1, -1);
					Region region = new Region(bounds);
					region.Exclude(rect);
					IntSecurity.ChangeWindowRegionForTopLevel.Assert();
					base.Region = region;
				}

				// Token: 0x060070E6 RID: 28902 RVA: 0x0019C878 File Offset: 0x0019AA78
				private void ForceSynchronousPaint()
				{
					if (!base.IsDisposed && this._numPaintsServiced == 0)
					{
						try
						{
							NativeMethods.MSG msg = default(NativeMethods.MSG);
							while (UnsafeNativeMethods.PeekMessage(ref msg, new HandleRef(this, IntPtr.Zero), 15, 15, 1))
							{
								SafeNativeMethods.UpdateWindow(new HandleRef(null, msg.hwnd));
								int numPaintsServiced = this._numPaintsServiced;
								this._numPaintsServiced = numPaintsServiced + 1;
								if (numPaintsServiced > 20)
								{
									break;
								}
							}
						}
						finally
						{
							this._numPaintsServiced = 0;
						}
					}
				}

				// Token: 0x060070E7 RID: 28903 RVA: 0x0000701A File Offset: 0x0000521A
				protected override void OnPaint(PaintEventArgs e)
				{
				}

				// Token: 0x060070E8 RID: 28904 RVA: 0x0019C8FC File Offset: 0x0019AAFC
				protected override void OnPaintBackground(PaintEventArgs e)
				{
					base.Renderer.DrawToolStripBackground(new ToolStripRenderEventArgs(e.Graphics, this));
					base.Renderer.DrawToolStripBorder(new ToolStripRenderEventArgs(e.Graphics, this));
				}

				// Token: 0x060070E9 RID: 28905 RVA: 0x0019C92C File Offset: 0x0019AB2C
				protected override void OnOpening(CancelEventArgs e)
				{
					base.OnOpening(e);
					e.Cancel = false;
				}

				// Token: 0x060070EA RID: 28906 RVA: 0x0019C93C File Offset: 0x0019AB3C
				public void MoveTo(Point newLocation)
				{
					base.Location = newLocation;
					this.ForceSynchronousPaint();
				}

				// Token: 0x060070EB RID: 28907 RVA: 0x0019C94B File Offset: 0x0019AB4B
				[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				protected override void WndProc(ref Message m)
				{
					if (m.Msg == 132)
					{
						m.Result = (IntPtr)(-1);
					}
					base.WndProc(ref m);
				}

				// Token: 0x04004409 RID: 17417
				private const int MAX_PAINTS_TO_SERVICE = 20;

				// Token: 0x0400440A RID: 17418
				private int _numPaintsServiced;
			}
		}

		/// <summary>Represents all the <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> objects in a <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
		// Token: 0x02000744 RID: 1860
		[ListBindable(false)]
		[ComVisible(false)]
		public class ToolStripPanelRowCollection : ArrangedElementCollection, IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" /> class in the specified <see cref="T:System.Windows.Forms.ToolStripPanel" />. </summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.ToolStripPanel" /> that holds this <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</param>
			// Token: 0x06006176 RID: 24950 RVA: 0x0018ED3C File Offset: 0x0018CF3C
			public ToolStripPanelRowCollection(ToolStripPanel owner)
			{
				this.owner = owner;
			}

			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" /> class with the specified number of rows in the specified <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.ToolStripPanel" /> that holds this <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</param>
			/// <param name="value">The number of rows in the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</param>
			// Token: 0x06006177 RID: 24951 RVA: 0x0018ED4B File Offset: 0x0018CF4B
			public ToolStripPanelRowCollection(ToolStripPanel owner, ToolStripPanelRow[] value)
			{
				this.owner = owner;
				this.AddRange(value);
			}

			/// <summary>Gets a particular <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> within the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</summary>
			/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> within the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> of the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" /> as specified by the <paramref name="index" /> parameter.</returns>
			// Token: 0x17001748 RID: 5960
			public virtual ToolStripPanelRow this[int index]
			{
				get
				{
					return (ToolStripPanelRow)base.InnerList[index];
				}
			}

			/// <summary>Adds the specified <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> to the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> to add to the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</param>
			/// <returns>The position of the <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> in the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</returns>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="value" /> is <see langword="null" />.</exception>
			// Token: 0x06006179 RID: 24953 RVA: 0x0018ED74 File Offset: 0x0018CF74
			public int Add(ToolStripPanelRow value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				int num = base.InnerList.Add(value);
				this.OnAdd(value, num);
				return num;
			}

			/// <summary>Adds an array of <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> objects to a <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
			/// <param name="value">An array of <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> objects.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="value" /> is <see langword="null" />.</exception>
			// Token: 0x0600617A RID: 24954 RVA: 0x0018EDA8 File Offset: 0x0018CFA8
			public void AddRange(ToolStripPanelRow[] value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				ToolStripPanel toolStripPanel = this.owner;
				if (toolStripPanel != null)
				{
					toolStripPanel.SuspendLayout();
				}
				try
				{
					for (int i = 0; i < value.Length; i++)
					{
						this.Add(value[i]);
					}
				}
				finally
				{
					if (toolStripPanel != null)
					{
						toolStripPanel.ResumeLayout();
					}
				}
			}

			/// <summary>Adds the specified <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" /> to a <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" /> to add to the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="value" /> is <see langword="null" />.</exception>
			// Token: 0x0600617B RID: 24955 RVA: 0x0018EE08 File Offset: 0x0018D008
			public void AddRange(ToolStripPanel.ToolStripPanelRowCollection value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				ToolStripPanel toolStripPanel = this.owner;
				if (toolStripPanel != null)
				{
					toolStripPanel.SuspendLayout();
				}
				try
				{
					int count = value.Count;
					for (int i = 0; i < count; i++)
					{
						this.Add(value[i]);
					}
				}
				finally
				{
					if (toolStripPanel != null)
					{
						toolStripPanel.ResumeLayout();
					}
				}
			}

			/// <summary>Determines whether the specified <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> is in the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> to search for in the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</param>
			/// <returns>
			///     <see langword="true" /> if the specified <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> is in the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />; otherwise, <see langword="false" />.</returns>
			// Token: 0x0600617C RID: 24956 RVA: 0x00115D88 File Offset: 0x00113F88
			public bool Contains(ToolStripPanelRow value)
			{
				return base.InnerList.Contains(value);
			}

			/// <summary>Removes all <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> objects from the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</summary>
			// Token: 0x0600617D RID: 24957 RVA: 0x0018EE74 File Offset: 0x0018D074
			public virtual void Clear()
			{
				if (this.owner != null)
				{
					this.owner.SuspendLayout();
				}
				try
				{
					while (this.Count != 0)
					{
						this.RemoveAt(this.Count - 1);
					}
				}
				finally
				{
					if (this.owner != null)
					{
						this.owner.ResumeLayout();
					}
				}
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Clear" />.</summary>
			// Token: 0x0600617E RID: 24958 RVA: 0x0018EED4 File Offset: 0x0018D0D4
			void IList.Clear()
			{
				this.Clear();
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsFixedSize" />.</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x17001749 RID: 5961
			// (get) Token: 0x0600617F RID: 24959 RVA: 0x00115FFC File Offset: 0x001141FC
			bool IList.IsFixedSize
			{
				get
				{
					return base.InnerList.IsFixedSize;
				}
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Contains(System.Object)" />.</summary>
			/// <param name="value">The item to locate in the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</param>
			/// <returns>
			///     <see langword="true" /> if <paramref name="value" /> is a <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> found in the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />; otherwise, <see langword="false" />.</returns>
			// Token: 0x06006180 RID: 24960 RVA: 0x00115D88 File Offset: 0x00113F88
			bool IList.Contains(object value)
			{
				return base.InnerList.Contains(value);
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsReadOnly" />.</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x1700174A RID: 5962
			// (get) Token: 0x06006181 RID: 24961 RVA: 0x001573CB File Offset: 0x001555CB
			bool IList.IsReadOnly
			{
				get
				{
					return base.InnerList.IsReadOnly;
				}
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.RemoveAt(System.Int32)" />.</summary>
			/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> to remove.</param>
			// Token: 0x06006182 RID: 24962 RVA: 0x0018EEDC File Offset: 0x0018D0DC
			void IList.RemoveAt(int index)
			{
				this.RemoveAt(index);
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Remove(System.Object)" />.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> to remove from the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</param>
			// Token: 0x06006183 RID: 24963 RVA: 0x0018EEE5 File Offset: 0x0018D0E5
			void IList.Remove(object value)
			{
				this.Remove(value as ToolStripPanelRow);
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />.</summary>
			/// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Collections.IList" />.</param>
			/// <returns>The zero-based index of the item to add.</returns>
			// Token: 0x06006184 RID: 24964 RVA: 0x0018EEF3 File Offset: 0x0018D0F3
			int IList.Add(object value)
			{
				return this.Add(value as ToolStripPanelRow);
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.IndexOf(System.Object)" />.</summary>
			/// <param name="value">The object to locate in the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</param>
			/// <returns>The index of <paramref name="value" /> if it is a <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> found in the list; otherwise, -1.</returns>
			// Token: 0x06006185 RID: 24965 RVA: 0x0018EF01 File Offset: 0x0018D101
			int IList.IndexOf(object value)
			{
				return this.IndexOf(value as ToolStripPanelRow);
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" />.</summary>
			/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
			/// <param name="value">The <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> to insert into the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</param>
			// Token: 0x06006186 RID: 24966 RVA: 0x0018EF0F File Offset: 0x0018D10F
			void IList.Insert(int index, object value)
			{
				this.Insert(index, value as ToolStripPanelRow);
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.Item(System.Int32)" />.</summary>
			/// <param name="index">The zero-based index of the element to get.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> at the specified index.</returns>
			// Token: 0x1700174B RID: 5963
			object IList.this[int index]
			{
				get
				{
					return base.InnerList[index];
				}
				set
				{
					throw new NotSupportedException(SR.GetString("ToolStripCollectionMustInsertAndRemove"));
				}
			}

			/// <summary>Gets the index of the specified <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> in the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> to return the index of.</param>
			/// <returns>The index of the specified <see cref="T:System.Windows.Forms.ToolStripPanelRow" />.</returns>
			// Token: 0x06006189 RID: 24969 RVA: 0x001160EC File Offset: 0x001142EC
			public int IndexOf(ToolStripPanelRow value)
			{
				return base.InnerList.IndexOf(value);
			}

			/// <summary>Inserts the specified <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> at the specified location in the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</summary>
			/// <param name="index">The zero-based index at which to insert the <see cref="T:System.Windows.Forms.ToolStripPanelRow" />.</param>
			/// <param name="value">The <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> to insert.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="value" /> is <see langword="null" />.</exception>
			// Token: 0x0600618A RID: 24970 RVA: 0x0018EF1E File Offset: 0x0018D11E
			public void Insert(int index, ToolStripPanelRow value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base.InnerList.Insert(index, value);
				this.OnAdd(value, index);
			}

			// Token: 0x0600618B RID: 24971 RVA: 0x0018EF43 File Offset: 0x0018D143
			private void OnAdd(ToolStripPanelRow value, int index)
			{
				if (this.owner != null)
				{
					LayoutTransaction.DoLayout(this.owner, value, PropertyNames.Parent);
				}
			}

			// Token: 0x0600618C RID: 24972 RVA: 0x0000701A File Offset: 0x0000521A
			private void OnAfterRemove(ToolStripPanelRow row)
			{
			}

			/// <summary>Removes the specified <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> from the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> to remove.</param>
			// Token: 0x0600618D RID: 24973 RVA: 0x0018EF5E File Offset: 0x0018D15E
			public void Remove(ToolStripPanelRow value)
			{
				base.InnerList.Remove(value);
				this.OnAfterRemove(value);
			}

			/// <summary>Removes the <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> at the specified index from the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" />.</summary>
			/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> to remove.</param>
			// Token: 0x0600618E RID: 24974 RVA: 0x0018EF74 File Offset: 0x0018D174
			public void RemoveAt(int index)
			{
				ToolStripPanelRow row = null;
				if (index < this.Count && index >= 0)
				{
					row = (ToolStripPanelRow)base.InnerList[index];
				}
				base.InnerList.RemoveAt(index);
				this.OnAfterRemove(row);
			}

			/// <summary>Copies the entire <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" /> into an existing array at a specified location within the array.</summary>
			/// <param name="array">An <see cref="T:System.Array" /> representing the array to copy the contents of the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" /> to.</param>
			/// <param name="index">The location within the destination array to copy the <see cref="T:System.Windows.Forms.ToolStripPanel.ToolStripPanelRowCollection" /> to.</param>
			// Token: 0x0600618F RID: 24975 RVA: 0x001162C9 File Offset: 0x001144C9
			public void CopyTo(ToolStripPanelRow[] array, int index)
			{
				base.InnerList.CopyTo(array, index);
			}

			// Token: 0x0400419C RID: 16796
			private ToolStripPanel owner;
		}

		// Token: 0x02000745 RID: 1861
		internal class ToolStripPanelControlCollection : WindowsFormsUtils.TypedControlCollection
		{
			// Token: 0x06006190 RID: 24976 RVA: 0x0018EFB5 File Offset: 0x0018D1B5
			public ToolStripPanelControlCollection(ToolStripPanel owner) : base(owner, typeof(ToolStrip))
			{
				this.owner = owner;
			}

			// Token: 0x06006191 RID: 24977 RVA: 0x0018EFD0 File Offset: 0x0018D1D0
			internal override void AddInternal(Control value)
			{
				if (value != null)
				{
					using (new LayoutTransaction(value, value, PropertyNames.Parent))
					{
						base.AddInternal(value);
						return;
					}
				}
				base.AddInternal(value);
			}

			// Token: 0x06006192 RID: 24978 RVA: 0x0018F018 File Offset: 0x0018D218
			internal void Sort()
			{
				if (this.owner.Orientation == Orientation.Horizontal)
				{
					base.InnerList.Sort(new ToolStripPanel.ToolStripPanelControlCollection.YXComparer());
					return;
				}
				base.InnerList.Sort(new ToolStripPanel.ToolStripPanelControlCollection.XYComparer());
			}

			// Token: 0x0400419D RID: 16797
			private ToolStripPanel owner;

			// Token: 0x020008A2 RID: 2210
			public class XYComparer : IComparer
			{
				// Token: 0x060070ED RID: 28909 RVA: 0x0019C970 File Offset: 0x0019AB70
				public int Compare(object first, object second)
				{
					Control control = first as Control;
					Control control2 = second as Control;
					if (control.Bounds.X < control2.Bounds.X)
					{
						return -1;
					}
					if (control.Bounds.X != control2.Bounds.X)
					{
						return 1;
					}
					if (control.Bounds.Y < control2.Bounds.Y)
					{
						return -1;
					}
					return 1;
				}
			}

			// Token: 0x020008A3 RID: 2211
			public class YXComparer : IComparer
			{
				// Token: 0x060070EF RID: 28911 RVA: 0x0019C9EC File Offset: 0x0019ABEC
				public int Compare(object first, object second)
				{
					Control control = first as Control;
					Control control2 = second as Control;
					if (control.Bounds.Y < control2.Bounds.Y)
					{
						return -1;
					}
					if (control.Bounds.Y != control2.Bounds.Y)
					{
						return 1;
					}
					if (control.Bounds.X < control2.Bounds.X)
					{
						return -1;
					}
					return 1;
				}
			}
		}
	}
}
