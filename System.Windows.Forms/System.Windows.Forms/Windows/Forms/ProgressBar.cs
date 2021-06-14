using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Layout;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows progress bar control.</summary>
	// Token: 0x02000317 RID: 791
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Value")]
	[DefaultBindingProperty("Value")]
	[SRDescription("DescriptionProgressBar")]
	public class ProgressBar : Control
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ProgressBar" /> class.</summary>
		// Token: 0x0600303C RID: 12348 RVA: 0x000E2ACC File Offset: 0x000E0CCC
		public ProgressBar()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.Selectable | ControlStyles.UseTextForAccessibility, false);
			this.ForeColor = this.defaultForeColor;
		}

		/// <summary>Overrides <see cref="P:System.Windows.Forms.Control.CreateParams" />.</summary>
		/// <returns>Information needed when you create a control.</returns>
		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x0600303D RID: 12349 RVA: 0x000E2B1C File Offset: 0x000E0D1C
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "msctls_progress32";
				if (this.Style == ProgressBarStyle.Continuous)
				{
					createParams.Style |= 1;
				}
				else if (this.Style == ProgressBarStyle.Marquee && !base.DesignMode)
				{
					createParams.Style |= 8;
				}
				if (this.RightToLeft == RightToLeft.Yes && this.RightToLeftLayout)
				{
					createParams.ExStyle |= 4194304;
					createParams.ExStyle &= -28673;
				}
				return createParams;
			}
		}

		/// <summary>Gets or sets a value indicating whether the control can accept data that the user drags onto it.</summary>
		/// <returns>
		///     <see langword="true" /> if drag-and-drop operations are allowed in the control; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x0600303E RID: 12350 RVA: 0x000B0BBD File Offset: 0x000AEDBD
		// (set) Token: 0x0600303F RID: 12351 RVA: 0x000B0BC5 File Offset: 0x000AEDC5
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>Gets or sets the background image for the <see cref="T:System.Windows.Forms.ProgressBar" /> control.</summary>
		/// <returns>The current background image.</returns>
		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x06003040 RID: 12352 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x06003041 RID: 12353 RVA: 0x00011FCA File Offset: 0x000101CA
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

		/// <summary>Gets or sets the manner in which progress should be indicated on the progress bar.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ProgressBarStyle" /> values. The default is <see cref="F:System.Windows.Forms.ProgressBarStyle.Blocks" /></returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value is not a member of the <see cref="T:System.Windows.Forms.ProgressBarStyle" /> enumeration. </exception>
		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x06003042 RID: 12354 RVA: 0x000E2BA9 File Offset: 0x000E0DA9
		// (set) Token: 0x06003043 RID: 12355 RVA: 0x000E2BB4 File Offset: 0x000E0DB4
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(ProgressBarStyle.Blocks)]
		[SRCategory("CatBehavior")]
		[SRDescription("ProgressBarStyleDescr")]
		public ProgressBarStyle Style
		{
			get
			{
				return this.style;
			}
			set
			{
				if (this.style != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(ProgressBarStyle));
					}
					this.style = value;
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
					if (this.style == ProgressBarStyle.Marquee)
					{
						this.StartMarquee();
					}
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ProgressBar.BackgroundImage" /> property changes.</summary>
		// Token: 0x14000244 RID: 580
		// (add) Token: 0x06003044 RID: 12356 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x06003045 RID: 12357 RVA: 0x0001FD8A File Offset: 0x0001DF8A
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

		/// <summary>Gets or sets the layout of the background image of the progress bar.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImageLayout" /> values.</returns>
		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x06003046 RID: 12358 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x06003047 RID: 12359 RVA: 0x00011FDB File Offset: 0x000101DB
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ProgressBar.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x14000245 RID: 581
		// (add) Token: 0x06003048 RID: 12360 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x06003049 RID: 12361 RVA: 0x0001FD9C File Offset: 0x0001DF9C
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

		/// <summary>Gets or sets a value indicating whether the control, when it receives focus, causes validation to be performed on any controls that require validation.</summary>
		/// <returns>
		///     <see langword="true" /> if the control, when it receives focus, causes validation to be performed on any controls that require validation; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x0600304A RID: 12362 RVA: 0x000DA227 File Offset: 0x000D8427
		// (set) Token: 0x0600304B RID: 12363 RVA: 0x000DA22F File Offset: 0x000D842F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ProgressBar.CausesValidation" /> property changes.</summary>
		// Token: 0x14000246 RID: 582
		// (add) Token: 0x0600304C RID: 12364 RVA: 0x000DA238 File Offset: 0x000D8438
		// (remove) Token: 0x0600304D RID: 12365 RVA: 0x000DA241 File Offset: 0x000D8441
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

		/// <summary>Gets the default Input Method Editor (IME) mode supported by the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x0600304E RID: 12366 RVA: 0x0001BB93 File Offset: 0x00019D93
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the default size of the control.</returns>
		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x0600304F RID: 12367 RVA: 0x000E2C14 File Offset: 0x000E0E14
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 23);
			}
		}

		/// <summary>Gets or sets a value indicating whether the control should redraw its surface using a secondary buffer.</summary>
		/// <returns>
		///     <see langword="true" /> if a secondary buffer should be used, <see langword="false" /> otherwise.</returns>
		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x06003050 RID: 12368 RVA: 0x000A2CB2 File Offset: 0x000A0EB2
		// (set) Token: 0x06003051 RID: 12369 RVA: 0x000A2CBA File Offset: 0x000A0EBA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool DoubleBuffered
		{
			get
			{
				return base.DoubleBuffered;
			}
			set
			{
				base.DoubleBuffered = value;
			}
		}

		/// <summary>Gets or sets the font of text in the <see cref="T:System.Windows.Forms.ProgressBar" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> of the text. The default is the font set by the container.</returns>
		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x06003052 RID: 12370 RVA: 0x00012071 File Offset: 0x00010271
		// (set) Token: 0x06003053 RID: 12371 RVA: 0x00012079 File Offset: 0x00010279
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ProgressBar.Font" /> property changes.</summary>
		// Token: 0x14000247 RID: 583
		// (add) Token: 0x06003054 RID: 12372 RVA: 0x00052778 File Offset: 0x00050978
		// (remove) Token: 0x06003055 RID: 12373 RVA: 0x00052781 File Offset: 0x00050981
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler FontChanged
		{
			add
			{
				base.FontChanged += value;
			}
			remove
			{
				base.FontChanged -= value;
			}
		}

		/// <summary>Gets or sets the input method editor (IME) for the <see cref="T:System.Windows.Forms.ProgressBar" /></summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x06003056 RID: 12374 RVA: 0x00011FE4 File Offset: 0x000101E4
		// (set) Token: 0x06003057 RID: 12375 RVA: 0x00011FEC File Offset: 0x000101EC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ImeMode ImeMode
		{
			get
			{
				return base.ImeMode;
			}
			set
			{
				base.ImeMode = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ProgressBar.ImeMode" /> property changes.</summary>
		// Token: 0x14000248 RID: 584
		// (add) Token: 0x06003058 RID: 12376 RVA: 0x0001BF2C File Offset: 0x0001A12C
		// (remove) Token: 0x06003059 RID: 12377 RVA: 0x0001BF35 File Offset: 0x0001A135
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ImeModeChanged
		{
			add
			{
				base.ImeModeChanged += value;
			}
			remove
			{
				base.ImeModeChanged -= value;
			}
		}

		/// <summary>Gets or sets the time period, in milliseconds, that it takes the progress block to scroll across the progress bar.</summary>
		/// <returns>The time period, in milliseconds, that it takes the progress block to scroll across the progress bar.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The indicated time period is less than 0.</exception>
		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x0600305A RID: 12378 RVA: 0x000E2C1F File Offset: 0x000E0E1F
		// (set) Token: 0x0600305B RID: 12379 RVA: 0x000E2C27 File Offset: 0x000E0E27
		[DefaultValue(100)]
		[SRCategory("CatBehavior")]
		[SRDescription("ProgressBarMarqueeAnimationSpeed")]
		public int MarqueeAnimationSpeed
		{
			get
			{
				return this.marqueeSpeed;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("MarqueeAnimationSpeed must be non-negative");
				}
				this.marqueeSpeed = value;
				if (!base.DesignMode)
				{
					this.StartMarquee();
				}
			}
		}

		// Token: 0x0600305C RID: 12380 RVA: 0x000E2C50 File Offset: 0x000E0E50
		private void StartMarquee()
		{
			if (base.IsHandleCreated && this.style == ProgressBarStyle.Marquee)
			{
				if (this.marqueeSpeed == 0)
				{
					base.SendMessage(1034, 0, this.marqueeSpeed);
					return;
				}
				base.SendMessage(1034, 1, this.marqueeSpeed);
			}
		}

		/// <summary>Gets or sets the maximum value of the range of the control.</summary>
		/// <returns>The maximum value of the range. The default is 100.</returns>
		/// <exception cref="T:System.ArgumentException">The value specified is less than 0. </exception>
		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x0600305D RID: 12381 RVA: 0x000E2C9D File Offset: 0x000E0E9D
		// (set) Token: 0x0600305E RID: 12382 RVA: 0x000E2CA8 File Offset: 0x000E0EA8
		[DefaultValue(100)]
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ProgressBarMaximumDescr")]
		public int Maximum
		{
			get
			{
				return this.maximum;
			}
			set
			{
				if (this.maximum != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("Maximum", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"Maximum",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.minimum > value)
					{
						this.minimum = value;
					}
					this.maximum = value;
					if (this.value > this.maximum)
					{
						this.value = this.maximum;
					}
					if (base.IsHandleCreated)
					{
						base.SendMessage(1030, this.minimum, this.maximum);
						this.UpdatePos();
					}
				}
			}
		}

		/// <summary>Gets or sets the minimum value of the range of the control.</summary>
		/// <returns>The minimum value of the range. The default is 0.</returns>
		/// <exception cref="T:System.ArgumentException">The value specified for the property is less than 0. </exception>
		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x0600305F RID: 12383 RVA: 0x000E2D5F File Offset: 0x000E0F5F
		// (set) Token: 0x06003060 RID: 12384 RVA: 0x000E2D68 File Offset: 0x000E0F68
		[DefaultValue(0)]
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ProgressBarMinimumDescr")]
		public int Minimum
		{
			get
			{
				return this.minimum;
			}
			set
			{
				if (this.minimum != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("Minimum", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"Minimum",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.maximum < value)
					{
						this.maximum = value;
					}
					this.minimum = value;
					if (this.value < this.minimum)
					{
						this.value = this.minimum;
					}
					if (base.IsHandleCreated)
					{
						base.SendMessage(1030, this.minimum, this.maximum);
						this.UpdatePos();
					}
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackColorChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003061 RID: 12385 RVA: 0x000E2E1F File Offset: 0x000E101F
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			if (base.IsHandleCreated)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 8193, 0, ColorTranslator.ToWin32(this.BackColor));
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ForeColorChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003062 RID: 12386 RVA: 0x000E2E53 File Offset: 0x000E1053
		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			if (base.IsHandleCreated)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1033, 0, ColorTranslator.ToWin32(this.ForeColor));
			}
		}

		/// <summary>Gets or sets the space between the edges of a <see cref="T:System.Windows.Forms.ProgressBar" /> control and its contents.</summary>
		/// <returns>
		///     <see cref="F:System.Windows.Forms.Padding.Empty" /> in all cases.</returns>
		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x06003063 RID: 12387 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x06003064 RID: 12388 RVA: 0x000204A2 File Offset: 0x0001E6A2
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ProgressBar.Padding" /> property changes.</summary>
		// Token: 0x14000249 RID: 585
		// (add) Token: 0x06003065 RID: 12389 RVA: 0x000204AB File Offset: 0x0001E6AB
		// (remove) Token: 0x06003066 RID: 12390 RVA: 0x000204B4 File Offset: 0x0001E6B4
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

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ProgressBar" /> and any text it contains is displayed from right to left. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ProgressBar" /> is displayed from right to left; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x06003067 RID: 12391 RVA: 0x000E2E87 File Offset: 0x000E1087
		// (set) Token: 0x06003068 RID: 12392 RVA: 0x000E2E90 File Offset: 0x000E1090
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ProgressBar.RightToLeftLayout" /> property changes.</summary>
		// Token: 0x1400024A RID: 586
		// (add) Token: 0x06003069 RID: 12393 RVA: 0x000E2EE4 File Offset: 0x000E10E4
		// (remove) Token: 0x0600306A RID: 12394 RVA: 0x000E2EFD File Offset: 0x000E10FD
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		public event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				this.onRightToLeftLayoutChanged = (EventHandler)Delegate.Combine(this.onRightToLeftLayoutChanged, value);
			}
			remove
			{
				this.onRightToLeftLayoutChanged = (EventHandler)Delegate.Remove(this.onRightToLeftLayoutChanged, value);
			}
		}

		/// <summary>Gets or sets the amount by which a call to the <see cref="M:System.Windows.Forms.ProgressBar.PerformStep" /> method increases the current position of the progress bar.</summary>
		/// <returns>The amount by which to increment the progress bar with each call to the <see cref="M:System.Windows.Forms.ProgressBar.PerformStep" /> method. The default is 10.</returns>
		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x0600306B RID: 12395 RVA: 0x000E2F16 File Offset: 0x000E1116
		// (set) Token: 0x0600306C RID: 12396 RVA: 0x000E2F1E File Offset: 0x000E111E
		[DefaultValue(10)]
		[SRCategory("CatBehavior")]
		[SRDescription("ProgressBarStepDescr")]
		public int Step
		{
			get
			{
				return this.step;
			}
			set
			{
				this.step = value;
				if (base.IsHandleCreated)
				{
					base.SendMessage(1028, this.step, 0);
				}
			}
		}

		/// <summary>Overrides <see cref="P:System.Windows.Forms.Control.TabStop" />.</summary>
		/// <returns>true if the user can set the focus to the control by using the TAB key; otherwise, false. The default is true.</returns>
		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x0600306D RID: 12397 RVA: 0x000AA115 File Offset: 0x000A8315
		// (set) Token: 0x0600306E RID: 12398 RVA: 0x000AA11D File Offset: 0x000A831D
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ProgressBar.TabStop" /> property changes.</summary>
		// Token: 0x1400024B RID: 587
		// (add) Token: 0x0600306F RID: 12399 RVA: 0x000AA126 File Offset: 0x000A8326
		// (remove) Token: 0x06003070 RID: 12400 RVA: 0x000AA12F File Offset: 0x000A832F
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

		/// <summary>Gets or sets the text associated with this control.</summary>
		/// <returns>The text associated with this control.</returns>
		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06003071 RID: 12401 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x06003072 RID: 12402 RVA: 0x0001BFAD File Offset: 0x0001A1AD
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ProgressBar.Text" /> property changes.</summary>
		// Token: 0x1400024C RID: 588
		// (add) Token: 0x06003073 RID: 12403 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x06003074 RID: 12404 RVA: 0x0003E43E File Offset: 0x0003C63E
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

		/// <summary>Gets or sets the current position of the progress bar.</summary>
		/// <returns>The position within the range of the progress bar. The default is 0.</returns>
		/// <exception cref="T:System.ArgumentException">The value specified is greater than the value of the <see cref="P:System.Windows.Forms.ProgressBar.Maximum" /> property.-or- The value specified is less than the value of the <see cref="P:System.Windows.Forms.ProgressBar.Minimum" /> property. </exception>
		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x06003075 RID: 12405 RVA: 0x000E2F42 File Offset: 0x000E1142
		// (set) Token: 0x06003076 RID: 12406 RVA: 0x000E2F4C File Offset: 0x000E114C
		[DefaultValue(0)]
		[SRCategory("CatBehavior")]
		[Bindable(true)]
		[SRDescription("ProgressBarValueDescr")]
		public int Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (this.value != value)
				{
					if (value < this.minimum || value > this.maximum)
					{
						throw new ArgumentOutOfRangeException("Value", SR.GetString("InvalidBoundArgument", new object[]
						{
							"Value",
							value.ToString(CultureInfo.CurrentCulture),
							"'minimum'",
							"'maximum'"
						}));
					}
					this.value = value;
					this.UpdatePos();
				}
			}
		}

		/// <summary>Occurs when the user double-clicks the control.</summary>
		// Token: 0x1400024D RID: 589
		// (add) Token: 0x06003077 RID: 12407 RVA: 0x0001B6FB File Offset: 0x000198FB
		// (remove) Token: 0x06003078 RID: 12408 RVA: 0x0001B704 File Offset: 0x00019904
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DoubleClick
		{
			add
			{
				base.DoubleClick += value;
			}
			remove
			{
				base.DoubleClick -= value;
			}
		}

		/// <summary>Occurs when the user double-clicks the control.</summary>
		// Token: 0x1400024E RID: 590
		// (add) Token: 0x06003079 RID: 12409 RVA: 0x0001B70D File Offset: 0x0001990D
		// (remove) Token: 0x0600307A RID: 12410 RVA: 0x0001B716 File Offset: 0x00019916
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseDoubleClick
		{
			add
			{
				base.MouseDoubleClick += value;
			}
			remove
			{
				base.MouseDoubleClick -= value;
			}
		}

		/// <summary>Occurs when the user releases a key while the control has focus.</summary>
		// Token: 0x1400024F RID: 591
		// (add) Token: 0x0600307B RID: 12411 RVA: 0x000B0E8C File Offset: 0x000AF08C
		// (remove) Token: 0x0600307C RID: 12412 RVA: 0x000B0E95 File Offset: 0x000AF095
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

		/// <summary>Occurs when the user presses a key while the control has focus.</summary>
		// Token: 0x14000250 RID: 592
		// (add) Token: 0x0600307D RID: 12413 RVA: 0x000B0E9E File Offset: 0x000AF09E
		// (remove) Token: 0x0600307E RID: 12414 RVA: 0x000B0EA7 File Offset: 0x000AF0A7
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

		/// <summary>Occurs when the user presses a key while the control has focus.</summary>
		// Token: 0x14000251 RID: 593
		// (add) Token: 0x0600307F RID: 12415 RVA: 0x000B0EB0 File Offset: 0x000AF0B0
		// (remove) Token: 0x06003080 RID: 12416 RVA: 0x000B0EB9 File Offset: 0x000AF0B9
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

		/// <summary>Occurs when focus enters the <see cref="T:System.Windows.Forms.ProgressBar" /> control.</summary>
		// Token: 0x14000252 RID: 594
		// (add) Token: 0x06003081 RID: 12417 RVA: 0x000DAC88 File Offset: 0x000D8E88
		// (remove) Token: 0x06003082 RID: 12418 RVA: 0x000DAC91 File Offset: 0x000D8E91
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Enter
		{
			add
			{
				base.Enter += value;
			}
			remove
			{
				base.Enter -= value;
			}
		}

		/// <summary>Occurs when focus leaves the <see cref="T:System.Windows.Forms.ProgressBar" /> control.</summary>
		// Token: 0x14000253 RID: 595
		// (add) Token: 0x06003083 RID: 12419 RVA: 0x000DAC9A File Offset: 0x000D8E9A
		// (remove) Token: 0x06003084 RID: 12420 RVA: 0x000DACA3 File Offset: 0x000D8EA3
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Leave
		{
			add
			{
				base.Leave += value;
			}
			remove
			{
				base.Leave -= value;
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.ProgressBar" /> is drawn.</summary>
		// Token: 0x14000254 RID: 596
		// (add) Token: 0x06003085 RID: 12421 RVA: 0x00020D37 File Offset: 0x0001EF37
		// (remove) Token: 0x06003086 RID: 12422 RVA: 0x00020D40 File Offset: 0x0001EF40
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

		/// <summary>Creates a handle for the control.</summary>
		// Token: 0x06003087 RID: 12423 RVA: 0x000E2FC4 File Offset: 0x000E11C4
		protected override void CreateHandle()
		{
			if (!base.RecreatingHandle)
			{
				IntPtr userCookie = UnsafeNativeMethods.ThemingScope.Activate();
				try
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 32
					});
				}
				finally
				{
					UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
				}
			}
			base.CreateHandle();
		}

		/// <summary>Advances the current position of the progress bar by the specified amount.</summary>
		/// <param name="value">The amount by which to increment the progress bar's current position. </param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Forms.ProgressBar.Style" /> property is set to <see cref="F:System.Windows.Forms.ProgressBarStyle.Marquee" /></exception>
		// Token: 0x06003088 RID: 12424 RVA: 0x000E3014 File Offset: 0x000E1214
		public void Increment(int value)
		{
			if (this.Style == ProgressBarStyle.Marquee)
			{
				throw new InvalidOperationException(SR.GetString("ProgressBarIncrementMarqueeException"));
			}
			this.value += value;
			if (this.value < this.minimum)
			{
				this.value = this.minimum;
			}
			if (this.value > this.maximum)
			{
				this.value = this.maximum;
			}
			this.UpdatePos();
		}

		/// <summary>Overrides <see cref="M:System.Windows.Forms.Control.OnHandleCreated(System.EventArgs)" /></summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003089 RID: 12425 RVA: 0x000E3084 File Offset: 0x000E1284
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			base.SendMessage(1030, this.minimum, this.maximum);
			base.SendMessage(1028, this.step, 0);
			base.SendMessage(1026, this.value, 0);
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 8193, 0, ColorTranslator.ToWin32(this.BackColor));
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1033, 0, ColorTranslator.ToWin32(this.ForeColor));
			this.StartMarquee();
			SystemEvents.UserPreferenceChanged += this.UserPreferenceChangedHandler;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600308A RID: 12426 RVA: 0x000E3133 File Offset: 0x000E1333
		protected override void OnHandleDestroyed(EventArgs e)
		{
			SystemEvents.UserPreferenceChanged -= this.UserPreferenceChangedHandler;
			base.OnHandleDestroyed(e);
		}

		/// <summary>Raises the <see cref="P:System.Windows.Forms.ProgressBar.RightToLeftLayout" /> event. </summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600308B RID: 12427 RVA: 0x000E314D File Offset: 0x000E134D
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRightToLeftLayoutChanged(EventArgs e)
		{
			if (base.GetAnyDisposingInHierarchy())
			{
				return;
			}
			if (this.RightToLeft == RightToLeft.Yes)
			{
				base.RecreateHandle();
			}
			if (this.onRightToLeftLayoutChanged != null)
			{
				this.onRightToLeftLayoutChanged(this, e);
			}
		}

		/// <summary>Advances the current position of the progress bar by the amount of the <see cref="P:System.Windows.Forms.ProgressBar.Step" /> property.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.ProgressBar.Style" /> is set to <see cref="F:System.Windows.Forms.ProgressBarStyle.Marquee" />.</exception>
		// Token: 0x0600308C RID: 12428 RVA: 0x000E317C File Offset: 0x000E137C
		public void PerformStep()
		{
			if (this.Style == ProgressBarStyle.Marquee)
			{
				throw new InvalidOperationException(SR.GetString("ProgressBarPerformStepMarqueeException"));
			}
			this.Increment(this.step);
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.Control.ForeColor" /> to its default value.</summary>
		// Token: 0x0600308D RID: 12429 RVA: 0x000E31A3 File Offset: 0x000E13A3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void ResetForeColor()
		{
			this.ForeColor = this.defaultForeColor;
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x000E31B1 File Offset: 0x000E13B1
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal override bool ShouldSerializeForeColor()
		{
			return this.ForeColor != this.defaultForeColor;
		}

		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x0600308F RID: 12431 RVA: 0x00020C1B File Offset: 0x0001EE1B
		internal override bool SupportsUiaProviders
		{
			get
			{
				return AccessibilityImprovements.Level3 && !base.DesignMode;
			}
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.ProgressBar" /> control.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.ProgressBar" />. </returns>
		// Token: 0x06003090 RID: 12432 RVA: 0x000E31C4 File Offset: 0x000E13C4
		public override string ToString()
		{
			string text = base.ToString();
			return string.Concat(new string[]
			{
				text,
				", Minimum: ",
				this.Minimum.ToString(CultureInfo.CurrentCulture),
				", Maximum: ",
				this.Maximum.ToString(CultureInfo.CurrentCulture),
				", Value: ",
				this.Value.ToString(CultureInfo.CurrentCulture)
			});
		}

		// Token: 0x06003091 RID: 12433 RVA: 0x000E3241 File Offset: 0x000E1441
		private void UpdatePos()
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(1026, this.value, 0);
			}
		}

		// Token: 0x06003092 RID: 12434 RVA: 0x000E3260 File Offset: 0x000E1460
		private void UserPreferenceChangedHandler(object o, UserPreferenceChangedEventArgs e)
		{
			if (base.IsHandleCreated)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1033, 0, ColorTranslator.ToWin32(this.ForeColor));
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 8193, 0, ColorTranslator.ToWin32(this.BackColor));
			}
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x000E32BB File Offset: 0x000E14BB
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level3)
			{
				return new ProgressBar.ProgressBarAccessibleObject(this);
			}
			return base.CreateAccessibilityInstance();
		}

		// Token: 0x04001DAA RID: 7594
		private int minimum;

		// Token: 0x04001DAB RID: 7595
		private int maximum = 100;

		// Token: 0x04001DAC RID: 7596
		private int step = 10;

		// Token: 0x04001DAD RID: 7597
		private int value;

		// Token: 0x04001DAE RID: 7598
		private int marqueeSpeed = 100;

		// Token: 0x04001DAF RID: 7599
		private Color defaultForeColor = SystemColors.Highlight;

		// Token: 0x04001DB0 RID: 7600
		private ProgressBarStyle style;

		// Token: 0x04001DB1 RID: 7601
		private EventHandler onRightToLeftLayoutChanged;

		// Token: 0x04001DB2 RID: 7602
		private bool rightToLeftLayout;

		// Token: 0x02000701 RID: 1793
		[ComVisible(true)]
		internal class ProgressBarAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06005FAB RID: 24491 RVA: 0x00093572 File Offset: 0x00091772
			internal ProgressBarAccessibleObject(ProgressBar owner) : base(owner)
			{
			}

			// Token: 0x170016DC RID: 5852
			// (get) Token: 0x06005FAC RID: 24492 RVA: 0x0018927C File Offset: 0x0018747C
			private ProgressBar OwningProgressBar
			{
				get
				{
					return base.Owner as ProgressBar;
				}
			}

			// Token: 0x06005FAD RID: 24493 RVA: 0x0009357B File Offset: 0x0009177B
			internal override bool IsIAccessibleExSupported()
			{
				return AccessibilityImprovements.Level3 || base.IsIAccessibleExSupported();
			}

			// Token: 0x06005FAE RID: 24494 RVA: 0x00189289 File Offset: 0x00187489
			internal override bool IsPatternSupported(int patternId)
			{
				return patternId == 10002 || patternId == 10003 || patternId == 10018 || base.IsPatternSupported(patternId);
			}

			// Token: 0x06005FAF RID: 24495 RVA: 0x001892AC File Offset: 0x001874AC
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID > 30009)
				{
					if (propertyID <= 30043)
					{
						if (propertyID != 30033 && propertyID != 30043)
						{
							goto IL_7F;
						}
					}
					else if (propertyID != 30048)
					{
						if (propertyID - 30051 > 1)
						{
							goto IL_7F;
						}
						return double.NaN;
					}
					return true;
				}
				if (propertyID == 30003)
				{
					return 50012;
				}
				if (propertyID == 30005)
				{
					return this.Name;
				}
				if (propertyID == 30009)
				{
					return true;
				}
				IL_7F:
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x06005FB0 RID: 24496 RVA: 0x0018933F File Offset: 0x0018753F
			internal override void SetValue(double newValue)
			{
				throw new InvalidOperationException("Progress Bar is read-only.");
			}

			// Token: 0x170016DD RID: 5853
			// (get) Token: 0x06005FB1 RID: 24497 RVA: 0x0000E228 File Offset: 0x0000C428
			internal override double LargeChange
			{
				get
				{
					return double.NaN;
				}
			}

			// Token: 0x170016DE RID: 5854
			// (get) Token: 0x06005FB2 RID: 24498 RVA: 0x0018934C File Offset: 0x0018754C
			internal override double Maximum
			{
				get
				{
					ProgressBar owningProgressBar = this.OwningProgressBar;
					int? num = (owningProgressBar != null) ? new int?(owningProgressBar.Maximum) : null;
					if (num == null)
					{
						return double.NaN;
					}
					return (double)num.GetValueOrDefault();
				}
			}

			// Token: 0x170016DF RID: 5855
			// (get) Token: 0x06005FB3 RID: 24499 RVA: 0x00189394 File Offset: 0x00187594
			internal override double Minimum
			{
				get
				{
					ProgressBar owningProgressBar = this.OwningProgressBar;
					int? num = (owningProgressBar != null) ? new int?(owningProgressBar.Minimum) : null;
					if (num == null)
					{
						return double.NaN;
					}
					return (double)num.GetValueOrDefault();
				}
			}

			// Token: 0x170016E0 RID: 5856
			// (get) Token: 0x06005FB4 RID: 24500 RVA: 0x0000E228 File Offset: 0x0000C428
			internal override double SmallChange
			{
				get
				{
					return double.NaN;
				}
			}

			// Token: 0x170016E1 RID: 5857
			// (get) Token: 0x06005FB5 RID: 24501 RVA: 0x001893DC File Offset: 0x001875DC
			internal override double RangeValue
			{
				get
				{
					ProgressBar owningProgressBar = this.OwningProgressBar;
					int? num = (owningProgressBar != null) ? new int?(owningProgressBar.Value) : null;
					if (num == null)
					{
						return double.NaN;
					}
					return (double)num.GetValueOrDefault();
				}
			}

			// Token: 0x170016E2 RID: 5858
			// (get) Token: 0x06005FB6 RID: 24502 RVA: 0x0000E214 File Offset: 0x0000C414
			internal override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}
		}
	}
}
