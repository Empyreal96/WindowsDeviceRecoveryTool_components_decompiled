using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows progress bar control contained in a <see cref="T:System.Windows.Forms.StatusStrip" />.</summary>
	// Token: 0x020003E7 RID: 999
	[DefaultProperty("Value")]
	public class ToolStripProgressBar : ToolStripControlHost
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripProgressBar" /> class. </summary>
		// Token: 0x060042E6 RID: 17126 RVA: 0x00120080 File Offset: 0x0011E280
		public ToolStripProgressBar() : base(ToolStripProgressBar.CreateControlInstance())
		{
			ToolStripProgressBar.ToolStripProgressBarControl toolStripProgressBarControl = base.Control as ToolStripProgressBar.ToolStripProgressBarControl;
			if (toolStripProgressBarControl != null)
			{
				toolStripProgressBarControl.Owner = this;
			}
			if (DpiHelper.EnableToolStripHighDpiImprovements)
			{
				this.scaledDefaultMargin = DpiHelper.LogicalToDeviceUnits(ToolStripProgressBar.defaultMargin, 0);
				this.scaledDefaultStatusStripMargin = DpiHelper.LogicalToDeviceUnits(ToolStripProgressBar.defaultStatusStripMargin, 0);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripProgressBar" /> class with specified name. </summary>
		/// <param name="name">The name of the <see cref="T:System.Windows.Forms.ToolStripProgressBar" />.</param>
		// Token: 0x060042E7 RID: 17127 RVA: 0x001200ED File Offset: 0x0011E2ED
		public ToolStripProgressBar(string name) : this()
		{
			base.Name = name;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ProgressBar" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ProgressBar" /> object associated with the control.</returns>
		// Token: 0x170010CF RID: 4303
		// (get) Token: 0x060042E8 RID: 17128 RVA: 0x001200FC File Offset: 0x0011E2FC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ProgressBar ProgressBar
		{
			get
			{
				return base.Control as ProgressBar;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The background image displayed in the control.</returns>
		// Token: 0x170010D0 RID: 4304
		// (get) Token: 0x060042E9 RID: 17129 RVA: 0x0010A87B File Offset: 0x00108A7B
		// (set) Token: 0x060042EA RID: 17130 RVA: 0x0010A883 File Offset: 0x00108A83
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.ImageLayout" /> value.</returns>
		// Token: 0x170010D1 RID: 4305
		// (get) Token: 0x060042EB RID: 17131 RVA: 0x0010A88C File Offset: 0x00108A8C
		// (set) Token: 0x060042EC RID: 17132 RVA: 0x0010A894 File Offset: 0x00108A94
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

		/// <summary>Gets the height and width of the <see cref="T:System.Windows.Forms.ToolStripProgressBar" /> in pixels.</summary>
		/// <returns>A <see cref="M:System.Drawing.Point.#ctor(System.Drawing.Size)" /> value representing the height and width.</returns>
		// Token: 0x170010D2 RID: 4306
		// (get) Token: 0x060042ED RID: 17133 RVA: 0x00120109 File Offset: 0x0011E309
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 15);
			}
		}

		/// <summary>Gets the spacing between the <see cref="T:System.Windows.Forms.ToolStripProgressBar" /> and adjacent items.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> value representing the spacing.</returns>
		// Token: 0x170010D3 RID: 4307
		// (get) Token: 0x060042EE RID: 17134 RVA: 0x00120114 File Offset: 0x0011E314
		protected internal override Padding DefaultMargin
		{
			get
			{
				if (base.Owner != null && base.Owner is StatusStrip)
				{
					return this.scaledDefaultStatusStripMargin;
				}
				return this.scaledDefaultMargin;
			}
		}

		/// <summary>Gets or sets a value representing the delay between each <see cref="F:System.Windows.Forms.ProgressBarStyle.Marquee" /> display update, in milliseconds.</summary>
		/// <returns>An integer representing the delay, in milliseconds.</returns>
		// Token: 0x170010D4 RID: 4308
		// (get) Token: 0x060042EF RID: 17135 RVA: 0x00120138 File Offset: 0x0011E338
		// (set) Token: 0x060042F0 RID: 17136 RVA: 0x00120145 File Offset: 0x0011E345
		[DefaultValue(100)]
		[SRCategory("CatBehavior")]
		[SRDescription("ProgressBarMarqueeAnimationSpeed")]
		public int MarqueeAnimationSpeed
		{
			get
			{
				return this.ProgressBar.MarqueeAnimationSpeed;
			}
			set
			{
				this.ProgressBar.MarqueeAnimationSpeed = value;
			}
		}

		/// <summary>Gets or sets the upper bound of the range that is defined for this <see cref="T:System.Windows.Forms.ToolStripProgressBar" />.</summary>
		/// <returns>An integer representing the upper bound of the range. The default is 100.</returns>
		// Token: 0x170010D5 RID: 4309
		// (get) Token: 0x060042F1 RID: 17137 RVA: 0x00120153 File Offset: 0x0011E353
		// (set) Token: 0x060042F2 RID: 17138 RVA: 0x00120160 File Offset: 0x0011E360
		[DefaultValue(100)]
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ProgressBarMaximumDescr")]
		public int Maximum
		{
			get
			{
				return this.ProgressBar.Maximum;
			}
			set
			{
				this.ProgressBar.Maximum = value;
			}
		}

		/// <summary>Gets or sets the lower bound of the range that is defined for this <see cref="T:System.Windows.Forms.ToolStripProgressBar" />.</summary>
		/// <returns>An integer representing the lower bound of the range. The default is 0.</returns>
		// Token: 0x170010D6 RID: 4310
		// (get) Token: 0x060042F3 RID: 17139 RVA: 0x0012016E File Offset: 0x0011E36E
		// (set) Token: 0x060042F4 RID: 17140 RVA: 0x0012017B File Offset: 0x0011E37B
		[DefaultValue(0)]
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ProgressBarMinimumDescr")]
		public int Minimum
		{
			get
			{
				return this.ProgressBar.Minimum;
			}
			set
			{
				this.ProgressBar.Minimum = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripProgressBar" /> layout is right-to-left or left-to-right when the <see cref="T:System.Windows.Forms.RightToLeft" /> property is set to <see cref="F:System.Windows.Forms.RightToLeft.Yes" />. </summary>
		/// <returns>
		///     <see langword="true" /> to turn on mirroring and lay out control from right to left when the <see cref="T:System.Windows.Forms.RightToLeft" /> property is set to <see cref="F:System.Windows.Forms.RightToLeft.Yes" />; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170010D7 RID: 4311
		// (get) Token: 0x060042F5 RID: 17141 RVA: 0x00120189 File Offset: 0x0011E389
		// (set) Token: 0x060042F6 RID: 17142 RVA: 0x00120196 File Offset: 0x0011E396
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue(false)]
		[SRDescription("ControlRightToLeftLayoutDescr")]
		public virtual bool RightToLeftLayout
		{
			get
			{
				return this.ProgressBar.RightToLeftLayout;
			}
			set
			{
				this.ProgressBar.RightToLeftLayout = value;
			}
		}

		/// <summary>Gets or sets the amount by which to increment the current value of the <see cref="T:System.Windows.Forms.ToolStripProgressBar" /> when the <see cref="M:System.Windows.Forms.ToolStripProgressBar.PerformStep" /> method is called.</summary>
		/// <returns>An integer representing the incremental amount. The default value is 10.</returns>
		// Token: 0x170010D8 RID: 4312
		// (get) Token: 0x060042F7 RID: 17143 RVA: 0x001201A4 File Offset: 0x0011E3A4
		// (set) Token: 0x060042F8 RID: 17144 RVA: 0x001201B1 File Offset: 0x0011E3B1
		[DefaultValue(10)]
		[SRCategory("CatBehavior")]
		[SRDescription("ProgressBarStepDescr")]
		public int Step
		{
			get
			{
				return this.ProgressBar.Step;
			}
			set
			{
				this.ProgressBar.Step = value;
			}
		}

		/// <summary>Gets or sets the style of the <see cref="T:System.Windows.Forms.ToolStripProgressBar" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ProgressBarStyle" /> values. The default value is <see cref="F:System.Windows.Forms.ProgressBarStyle.Blocks" />.</returns>
		// Token: 0x170010D9 RID: 4313
		// (get) Token: 0x060042F9 RID: 17145 RVA: 0x001201BF File Offset: 0x0011E3BF
		// (set) Token: 0x060042FA RID: 17146 RVA: 0x001201CC File Offset: 0x0011E3CC
		[DefaultValue(ProgressBarStyle.Blocks)]
		[SRCategory("CatBehavior")]
		[SRDescription("ProgressBarStyleDescr")]
		public ProgressBarStyle Style
		{
			get
			{
				return this.ProgressBar.Style;
			}
			set
			{
				this.ProgressBar.Style = value;
			}
		}

		/// <summary>Gets or sets the text displayed on the <see cref="T:System.Windows.Forms.ToolStripProgressBar" />.</summary>
		/// <returns>A <see cref="T:System.String" /> representing the display text.</returns>
		// Token: 0x170010DA RID: 4314
		// (get) Token: 0x060042FB RID: 17147 RVA: 0x0010B32D File Offset: 0x0010952D
		// (set) Token: 0x060042FC RID: 17148 RVA: 0x0010B33A File Offset: 0x0010953A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string Text
		{
			get
			{
				return base.Control.Text;
			}
			set
			{
				base.Control.Text = value;
			}
		}

		/// <summary>Gets or sets the current value of the <see cref="T:System.Windows.Forms.ToolStripProgressBar" />.</summary>
		/// <returns>An integer representing the current value.</returns>
		// Token: 0x170010DB RID: 4315
		// (get) Token: 0x060042FD RID: 17149 RVA: 0x001201DA File Offset: 0x0011E3DA
		// (set) Token: 0x060042FE RID: 17150 RVA: 0x001201E7 File Offset: 0x0011E3E7
		[DefaultValue(0)]
		[SRCategory("CatBehavior")]
		[Bindable(true)]
		[SRDescription("ProgressBarValueDescr")]
		public int Value
		{
			get
			{
				return this.ProgressBar.Value;
			}
			set
			{
				this.ProgressBar.Value = value;
			}
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x001201F5 File Offset: 0x0011E3F5
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level3)
			{
				return new ToolStripProgressBar.ToolStripProgressBarAccessibleObject(this);
			}
			return base.CreateAccessibilityInstance();
		}

		// Token: 0x06004300 RID: 17152 RVA: 0x0012020C File Offset: 0x0011E40C
		private static Control CreateControlInstance()
		{
			ProgressBar progressBar = AccessibilityImprovements.Level3 ? new ToolStripProgressBar.ToolStripProgressBarControl() : new ProgressBar();
			progressBar.Size = new Size(100, 15);
			return progressBar;
		}

		// Token: 0x06004301 RID: 17153 RVA: 0x0012023D File Offset: 0x0011E43D
		private void HandleRightToLeftLayoutChanged(object sender, EventArgs e)
		{
			this.OnRightToLeftLayoutChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ProgressBar.RightToLeftLayoutChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06004302 RID: 17154 RVA: 0x00120246 File Offset: 0x0011E446
		protected virtual void OnRightToLeftLayoutChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripProgressBar.EventRightToLeftLayoutChanged, e);
		}

		/// <summary>Subscribes events from the hosted control.</summary>
		/// <param name="control">The control from which to subscribe events.</param>
		// Token: 0x06004303 RID: 17155 RVA: 0x00120254 File Offset: 0x0011E454
		protected override void OnSubscribeControlEvents(Control control)
		{
			ProgressBar progressBar = control as ProgressBar;
			if (progressBar != null)
			{
				progressBar.RightToLeftLayoutChanged += this.HandleRightToLeftLayoutChanged;
			}
			base.OnSubscribeControlEvents(control);
		}

		/// <summary>Unsubscribes events from the hosted control.</summary>
		/// <param name="control">The control from which to unsubscribe events.</param>
		// Token: 0x06004304 RID: 17156 RVA: 0x00120284 File Offset: 0x0011E484
		protected override void OnUnsubscribeControlEvents(Control control)
		{
			ProgressBar progressBar = control as ProgressBar;
			if (progressBar != null)
			{
				progressBar.RightToLeftLayoutChanged -= this.HandleRightToLeftLayoutChanged;
			}
			base.OnUnsubscribeControlEvents(control);
		}

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x1400035A RID: 858
		// (add) Token: 0x06004305 RID: 17157 RVA: 0x001202B4 File Offset: 0x0011E4B4
		// (remove) Token: 0x06004306 RID: 17158 RVA: 0x001202BD File Offset: 0x0011E4BD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x1400035B RID: 859
		// (add) Token: 0x06004307 RID: 17159 RVA: 0x001202C6 File Offset: 0x0011E4C6
		// (remove) Token: 0x06004308 RID: 17160 RVA: 0x001202CF File Offset: 0x0011E4CF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x1400035C RID: 860
		// (add) Token: 0x06004309 RID: 17161 RVA: 0x001202D8 File Offset: 0x0011E4D8
		// (remove) Token: 0x0600430A RID: 17162 RVA: 0x001202E1 File Offset: 0x0011E4E1
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x1400035D RID: 861
		// (add) Token: 0x0600430B RID: 17163 RVA: 0x001202EA File Offset: 0x0011E4EA
		// (remove) Token: 0x0600430C RID: 17164 RVA: 0x001202F3 File Offset: 0x0011E4F3
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x1400035E RID: 862
		// (add) Token: 0x0600430D RID: 17165 RVA: 0x001202FC File Offset: 0x0011E4FC
		// (remove) Token: 0x0600430E RID: 17166 RVA: 0x00120305 File Offset: 0x0011E505
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler OwnerChanged
		{
			add
			{
				base.OwnerChanged += value;
			}
			remove
			{
				base.OwnerChanged -= value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripProgressBar.RightToLeftLayout" /> property changes.</summary>
		// Token: 0x1400035F RID: 863
		// (add) Token: 0x0600430F RID: 17167 RVA: 0x0012030E File Offset: 0x0011E50E
		// (remove) Token: 0x06004310 RID: 17168 RVA: 0x00120321 File Offset: 0x0011E521
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		public event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripProgressBar.EventRightToLeftLayoutChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripProgressBar.EventRightToLeftLayoutChanged, value);
			}
		}

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x14000360 RID: 864
		// (add) Token: 0x06004311 RID: 17169 RVA: 0x00120334 File Offset: 0x0011E534
		// (remove) Token: 0x06004312 RID: 17170 RVA: 0x0012033D File Offset: 0x0011E53D
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

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x14000361 RID: 865
		// (add) Token: 0x06004313 RID: 17171 RVA: 0x00120346 File Offset: 0x0011E546
		// (remove) Token: 0x06004314 RID: 17172 RVA: 0x0012034F File Offset: 0x0011E54F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Validated
		{
			add
			{
				base.Validated += value;
			}
			remove
			{
				base.Validated -= value;
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x14000362 RID: 866
		// (add) Token: 0x06004315 RID: 17173 RVA: 0x00120358 File Offset: 0x0011E558
		// (remove) Token: 0x06004316 RID: 17174 RVA: 0x00120361 File Offset: 0x0011E561
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event CancelEventHandler Validating
		{
			add
			{
				base.Validating += value;
			}
			remove
			{
				base.Validating -= value;
			}
		}

		/// <summary>Advances the current position of the progress bar by the specified amount.</summary>
		/// <param name="value">The amount by which to increment the progress bar's current position.</param>
		// Token: 0x06004317 RID: 17175 RVA: 0x0012036A File Offset: 0x0011E56A
		public void Increment(int value)
		{
			this.ProgressBar.Increment(value);
		}

		/// <summary>Advances the current position of the progress bar by the amount of the <see cref="P:System.Windows.Forms.ToolStripProgressBar.Step" /> property.</summary>
		// Token: 0x06004318 RID: 17176 RVA: 0x00120378 File Offset: 0x0011E578
		public void PerformStep()
		{
			this.ProgressBar.PerformStep();
		}

		// Token: 0x04002572 RID: 9586
		internal static readonly object EventRightToLeftLayoutChanged = new object();

		// Token: 0x04002573 RID: 9587
		private static readonly Padding defaultMargin = new Padding(1, 2, 1, 1);

		// Token: 0x04002574 RID: 9588
		private static readonly Padding defaultStatusStripMargin = new Padding(1, 3, 1, 3);

		// Token: 0x04002575 RID: 9589
		private Padding scaledDefaultMargin = ToolStripProgressBar.defaultMargin;

		// Token: 0x04002576 RID: 9590
		private Padding scaledDefaultStatusStripMargin = ToolStripProgressBar.defaultStatusStripMargin;

		// Token: 0x0200074A RID: 1866
		[ComVisible(true)]
		internal class ToolStripProgressBarAccessibleObject : ToolStripItem.ToolStripItemAccessibleObject
		{
			// Token: 0x060061DB RID: 25051 RVA: 0x00191163 File Offset: 0x0018F363
			public ToolStripProgressBarAccessibleObject(ToolStripProgressBar ownerItem) : base(ownerItem)
			{
				this.ownerItem = ownerItem;
			}

			// Token: 0x1700175A RID: 5978
			// (get) Token: 0x060061DC RID: 25052 RVA: 0x00191174 File Offset: 0x0018F374
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.ProgressBar;
				}
			}

			// Token: 0x060061DD RID: 25053 RVA: 0x00191195 File Offset: 0x0018F395
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (direction == UnsafeNativeMethods.NavigateDirection.FirstChild || direction == UnsafeNativeMethods.NavigateDirection.LastChild)
				{
					return this.ownerItem.ProgressBar.AccessibilityObject;
				}
				return base.FragmentNavigate(direction);
			}

			// Token: 0x040041A4 RID: 16804
			private ToolStripProgressBar ownerItem;
		}

		// Token: 0x0200074B RID: 1867
		internal class ToolStripProgressBarControl : ProgressBar
		{
			// Token: 0x1700175B RID: 5979
			// (get) Token: 0x060061DE RID: 25054 RVA: 0x001911B7 File Offset: 0x0018F3B7
			// (set) Token: 0x060061DF RID: 25055 RVA: 0x001911BF File Offset: 0x0018F3BF
			public ToolStripProgressBar Owner
			{
				get
				{
					return this.ownerItem;
				}
				set
				{
					this.ownerItem = value;
				}
			}

			// Token: 0x1700175C RID: 5980
			// (get) Token: 0x060061E0 RID: 25056 RVA: 0x000A010F File Offset: 0x0009E30F
			internal override bool SupportsUiaProviders
			{
				get
				{
					return AccessibilityImprovements.Level3;
				}
			}

			// Token: 0x060061E1 RID: 25057 RVA: 0x001911C8 File Offset: 0x0018F3C8
			protected override AccessibleObject CreateAccessibilityInstance()
			{
				if (AccessibilityImprovements.Level3)
				{
					return new ToolStripProgressBar.ToolStripProgressBarControlAccessibleObject(this);
				}
				return base.CreateAccessibilityInstance();
			}

			// Token: 0x040041A5 RID: 16805
			private ToolStripProgressBar ownerItem;
		}

		// Token: 0x0200074C RID: 1868
		internal class ToolStripProgressBarControlAccessibleObject : ProgressBar.ProgressBarAccessibleObject
		{
			// Token: 0x060061E3 RID: 25059 RVA: 0x001911E6 File Offset: 0x0018F3E6
			public ToolStripProgressBarControlAccessibleObject(ToolStripProgressBar.ToolStripProgressBarControl toolStripProgressBarControl) : base(toolStripProgressBarControl)
			{
			}

			// Token: 0x1700175D RID: 5981
			// (get) Token: 0x060061E4 RID: 25060 RVA: 0x001911F0 File Offset: 0x0018F3F0
			internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
			{
				get
				{
					ToolStripProgressBar.ToolStripProgressBarControl toolStripProgressBarControl = base.Owner as ToolStripProgressBar.ToolStripProgressBarControl;
					if (toolStripProgressBarControl != null)
					{
						return toolStripProgressBarControl.Owner.Owner.AccessibilityObject;
					}
					return base.FragmentRoot;
				}
			}

			// Token: 0x060061E5 RID: 25061 RVA: 0x00191224 File Offset: 0x0018F424
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (direction <= UnsafeNativeMethods.NavigateDirection.PreviousSibling)
				{
					ToolStripProgressBar.ToolStripProgressBarControl toolStripProgressBarControl = base.Owner as ToolStripProgressBar.ToolStripProgressBarControl;
					if (toolStripProgressBarControl != null)
					{
						return toolStripProgressBarControl.Owner.AccessibilityObject.FragmentNavigate(direction);
					}
				}
				return base.FragmentNavigate(direction);
			}

			// Token: 0x060061E6 RID: 25062 RVA: 0x0019125D File Offset: 0x0018F45D
			internal override object GetPropertyValue(int propertyID)
			{
				return base.GetPropertyValue(propertyID);
			}
		}
	}
}
