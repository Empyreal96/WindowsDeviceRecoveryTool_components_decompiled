using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Represents the center panel of a <see cref="T:System.Windows.Forms.ToolStripContainer" /> control.</summary>
	// Token: 0x020003DC RID: 988
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.ToolStripContentPanelDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("Load")]
	[Docking(DockingBehavior.Never)]
	[InitializationEvent("Load")]
	[ToolboxItem(false)]
	public class ToolStripContentPanel : Panel
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripContentPanel" /> class. </summary>
		// Token: 0x060041B3 RID: 16819 RVA: 0x0011A7CC File Offset: 0x001189CC
		public ToolStripContentPanel()
		{
			base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The mode by which the content panel automatically resizes itself.</returns>
		// Token: 0x17001076 RID: 4214
		// (get) Token: 0x060041B4 RID: 16820 RVA: 0x0000E214 File Offset: 0x0000C414
		// (set) Token: 0x060041B5 RID: 16821 RVA: 0x0000701A File Offset: 0x0000521A
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[Localizable(false)]
		public override AutoSizeMode AutoSizeMode
		{
			get
			{
				return AutoSizeMode.GrowOnly;
			}
			set
			{
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The edges of the container to which a control is bound and determines how a control is resized with its parent.</returns>
		// Token: 0x17001077 RID: 4215
		// (get) Token: 0x060041B6 RID: 16822 RVA: 0x000F7554 File Offset: 0x000F5754
		// (set) Token: 0x060041B7 RID: 16823 RVA: 0x000F755C File Offset: 0x000F575C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override AnchorStyles Anchor
		{
			get
			{
				return base.Anchor;
			}
			set
			{
				base.Anchor = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> to enable automatic scrolling; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001078 RID: 4216
		// (get) Token: 0x060041B8 RID: 16824 RVA: 0x000A87BB File Offset: 0x000A69BB
		// (set) Token: 0x060041B9 RID: 16825 RVA: 0x000E3A46 File Offset: 0x000E1C46
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

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The distance between any child controls and the edges of the scrollable parent control.</returns>
		// Token: 0x17001079 RID: 4217
		// (get) Token: 0x060041BA RID: 16826 RVA: 0x000F3C48 File Offset: 0x000F1E48
		// (set) Token: 0x060041BB RID: 16827 RVA: 0x000F3C50 File Offset: 0x000F1E50
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
		/// <returns>The minimum size of the auto-scroll.</returns>
		// Token: 0x1700107A RID: 4218
		// (get) Token: 0x060041BC RID: 16828 RVA: 0x000F3C37 File Offset: 0x000F1E37
		// (set) Token: 0x060041BD RID: 16829 RVA: 0x000F3C3F File Offset: 0x000F1E3F
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

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> to enable automatic sizing; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700107B RID: 4219
		// (get) Token: 0x060041BE RID: 16830 RVA: 0x000F7531 File Offset: 0x000F5731
		// (set) Token: 0x060041BF RID: 16831 RVA: 0x000F7539 File Offset: 0x000F5739
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>Overridden to ensure that the background color of the <see cref="T:System.Windows.Forms.ToolStripContainer" /> reflects the background color of the <see cref="T:System.Windows.Forms.ToolStripContentPanel" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> structure representing the background color of the <see cref="T:System.Windows.Forms.ToolStripContentPanel" />.</returns>
		// Token: 0x1700107C RID: 4220
		// (get) Token: 0x060041C0 RID: 16832 RVA: 0x00011FB1 File Offset: 0x000101B1
		// (set) Token: 0x060041C1 RID: 16833 RVA: 0x0011A7E0 File Offset: 0x001189E0
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				if (this.ParentInternal is ToolStripContainer && value == Color.Transparent)
				{
					this.ParentInternal.BackColor = Color.Transparent;
				}
				base.BackColor = value;
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x1400034D RID: 845
		// (add) Token: 0x060041C2 RID: 16834 RVA: 0x000F7542 File Offset: 0x000F5742
		// (remove) Token: 0x060041C3 RID: 16835 RVA: 0x000F754B File Offset: 0x000F574B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if the control causes validation; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700107D RID: 4221
		// (get) Token: 0x060041C4 RID: 16836 RVA: 0x000DA227 File Offset: 0x000D8427
		// (set) Token: 0x060041C5 RID: 16837 RVA: 0x000DA22F File Offset: 0x000D842F
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

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x1400034E RID: 846
		// (add) Token: 0x060041C6 RID: 16838 RVA: 0x000DA238 File Offset: 0x000D8438
		// (remove) Token: 0x060041C7 RID: 16839 RVA: 0x000DA241 File Offset: 0x000D8441
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

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DockStyle" /> values.</returns>
		// Token: 0x1700107E RID: 4222
		// (get) Token: 0x060041C8 RID: 16840 RVA: 0x000F3D46 File Offset: 0x000F1F46
		// (set) Token: 0x060041C9 RID: 16841 RVA: 0x000F7576 File Offset: 0x000F5776
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock = value;
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x1400034F RID: 847
		// (add) Token: 0x060041CA RID: 16842 RVA: 0x000F7680 File Offset: 0x000F5880
		// (remove) Token: 0x060041CB RID: 16843 RVA: 0x000F7689 File Offset: 0x000F5889
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new event EventHandler DockChanged
		{
			add
			{
				base.DockChanged += value;
			}
			remove
			{
				base.DockChanged -= value;
			}
		}

		/// <summary>Occurs when the content panel loads.</summary>
		// Token: 0x14000350 RID: 848
		// (add) Token: 0x060041CC RID: 16844 RVA: 0x0011A813 File Offset: 0x00118A13
		// (remove) Token: 0x060041CD RID: 16845 RVA: 0x0011A826 File Offset: 0x00118A26
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripContentPanelOnLoadDescr")]
		public event EventHandler Load
		{
			add
			{
				base.Events.AddHandler(ToolStripContentPanel.EventLoad, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripContentPanel.EventLoad, value);
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The coordinates of the upper-left corner of the control relative to the upper-left corner of its container.</returns>
		// Token: 0x1700107F RID: 4223
		// (get) Token: 0x060041CE RID: 16846 RVA: 0x000A9351 File Offset: 0x000A7551
		// (set) Token: 0x060041CF RID: 16847 RVA: 0x000A9359 File Offset: 0x000A7559
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Point Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				base.Location = value;
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x14000351 RID: 849
		// (add) Token: 0x060041D0 RID: 16848 RVA: 0x000F7692 File Offset: 0x000F5892
		// (remove) Token: 0x060041D1 RID: 16849 RVA: 0x000F769B File Offset: 0x000F589B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new event EventHandler LocationChanged
		{
			add
			{
				base.LocationChanged += value;
			}
			remove
			{
				base.LocationChanged -= value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The size that is the lower limit that GetPreferredSize can specify.</returns>
		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x060041D2 RID: 16850 RVA: 0x000203F4 File Offset: 0x0001E5F4
		// (set) Token: 0x060041D3 RID: 16851 RVA: 0x000F75C6 File Offset: 0x000F57C6
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Size MinimumSize
		{
			get
			{
				return base.MinimumSize;
			}
			set
			{
				base.MinimumSize = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The size that is the upper limit that GetPreferredSize can specify.</returns>
		// Token: 0x17001081 RID: 4225
		// (get) Token: 0x060041D4 RID: 16852 RVA: 0x000203D7 File Offset: 0x0001E5D7
		// (set) Token: 0x060041D5 RID: 16853 RVA: 0x000F75CF File Offset: 0x000F57CF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Size MaximumSize
		{
			get
			{
				return base.MaximumSize;
			}
			set
			{
				base.MaximumSize = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The name of the control.</returns>
		// Token: 0x17001082 RID: 4226
		// (get) Token: 0x060041D6 RID: 16854 RVA: 0x000F75D8 File Offset: 0x000F57D8
		// (set) Token: 0x060041D7 RID: 16855 RVA: 0x000F75E0 File Offset: 0x000F57E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				base.Name = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The tab order of the control within its container.</returns>
		// Token: 0x17001083 RID: 4227
		// (get) Token: 0x060041D8 RID: 16856 RVA: 0x000AA0F2 File Offset: 0x000A82F2
		// (set) Token: 0x060041D9 RID: 16857 RVA: 0x000AA0FA File Offset: 0x000A82FA
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
		// Token: 0x14000352 RID: 850
		// (add) Token: 0x060041DA RID: 16858 RVA: 0x000AA103 File Offset: 0x000A8303
		// (remove) Token: 0x060041DB RID: 16859 RVA: 0x000AA10C File Offset: 0x000A830C
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
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripContentPanel" /> can be tabbed to; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001084 RID: 4228
		// (get) Token: 0x060041DC RID: 16860 RVA: 0x000F7618 File Offset: 0x000F5818
		// (set) Token: 0x060041DD RID: 16861 RVA: 0x000F7620 File Offset: 0x000F5820
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
				base.TabStop = value;
			}
		}

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x14000353 RID: 851
		// (add) Token: 0x060041DE RID: 16862 RVA: 0x000AA126 File Offset: 0x000A8326
		// (remove) Token: 0x060041DF RID: 16863 RVA: 0x000AA12F File Offset: 0x000A832F
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

		// Token: 0x17001085 RID: 4229
		// (get) Token: 0x060041E0 RID: 16864 RVA: 0x0011A839 File Offset: 0x00118A39
		private ToolStripRendererSwitcher RendererSwitcher
		{
			get
			{
				if (this.rendererSwitcher == null)
				{
					this.rendererSwitcher = new ToolStripRendererSwitcher(this, ToolStripRenderMode.System);
					this.HandleRendererChanged(this, EventArgs.Empty);
					this.rendererSwitcher.RendererChanged += this.HandleRendererChanged;
				}
				return this.rendererSwitcher;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Forms.ToolStripRenderer" /> used to customize the appearance of a <see cref="T:System.Windows.Forms.ToolStripContentPanel" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripRenderer" /> that handles painting.</returns>
		// Token: 0x17001086 RID: 4230
		// (get) Token: 0x060041E1 RID: 16865 RVA: 0x0011A879 File Offset: 0x00118A79
		// (set) Token: 0x060041E2 RID: 16866 RVA: 0x0011A886 File Offset: 0x00118A86
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

		/// <summary>Gets or sets the painting styles to be applied to the <see cref="T:System.Windows.Forms.ToolStripContentPanel" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripRenderMode" /> values. </returns>
		// Token: 0x17001087 RID: 4231
		// (get) Token: 0x060041E3 RID: 16867 RVA: 0x0011A894 File Offset: 0x00118A94
		// (set) Token: 0x060041E4 RID: 16868 RVA: 0x0011A8A1 File Offset: 0x00118AA1
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripContentPanel.Renderer" /> property changes.</summary>
		// Token: 0x14000354 RID: 852
		// (add) Token: 0x060041E5 RID: 16869 RVA: 0x0011A8AF File Offset: 0x00118AAF
		// (remove) Token: 0x060041E6 RID: 16870 RVA: 0x0011A8C2 File Offset: 0x00118AC2
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripRendererChanged")]
		public event EventHandler RendererChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripContentPanel.EventRendererChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripContentPanel.EventRendererChanged, value);
			}
		}

		// Token: 0x060041E7 RID: 16871 RVA: 0x0011A8D5 File Offset: 0x00118AD5
		private void HandleRendererChanged(object sender, EventArgs e)
		{
			this.OnRendererChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060041E8 RID: 16872 RVA: 0x0011A8DE File Offset: 0x00118ADE
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (!base.RecreatingHandle)
			{
				this.OnLoad(EventArgs.Empty);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060041E9 RID: 16873 RVA: 0x0011A8FC File Offset: 0x00118AFC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnLoad(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripContentPanel.EventLoad];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Renders the <see cref="T:System.Windows.Forms.ToolStripContentPanel" />.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		// Token: 0x060041EA RID: 16874 RVA: 0x0011A92C File Offset: 0x00118B2C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			ToolStripContentPanelRenderEventArgs toolStripContentPanelRenderEventArgs = new ToolStripContentPanelRenderEventArgs(e.Graphics, this);
			this.Renderer.DrawToolStripContentPanelBackground(toolStripContentPanelRenderEventArgs);
			if (!toolStripContentPanelRenderEventArgs.Handled)
			{
				base.OnPaintBackground(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripContentPanel.RendererChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060041EB RID: 16875 RVA: 0x0011A964 File Offset: 0x00118B64
		protected virtual void OnRendererChanged(EventArgs e)
		{
			if (this.Renderer is ToolStripProfessionalRenderer)
			{
				this.state[ToolStripContentPanel.stateLastDoubleBuffer] = this.DoubleBuffered;
				base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			}
			else
			{
				this.DoubleBuffered = this.state[ToolStripContentPanel.stateLastDoubleBuffer];
			}
			this.Renderer.InitializeContentPanel(this);
			base.Invalidate();
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripContentPanel.EventRendererChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x0011A9EB File Offset: 0x00118BEB
		private void ResetRenderMode()
		{
			this.RendererSwitcher.ResetRenderMode();
		}

		// Token: 0x060041ED RID: 16877 RVA: 0x0011A9F8 File Offset: 0x00118BF8
		private bool ShouldSerializeRenderMode()
		{
			return this.RendererSwitcher.ShouldSerializeRenderMode();
		}

		// Token: 0x04002521 RID: 9505
		private ToolStripRendererSwitcher rendererSwitcher;

		// Token: 0x04002522 RID: 9506
		private BitVector32 state;

		// Token: 0x04002523 RID: 9507
		private static readonly int stateLastDoubleBuffer = BitVector32.CreateMask();

		// Token: 0x04002524 RID: 9508
		private static readonly object EventRendererChanged = new object();

		// Token: 0x04002525 RID: 9509
		private static readonly object EventLoad = new object();
	}
}
