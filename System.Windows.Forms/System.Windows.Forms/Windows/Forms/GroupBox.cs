using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows control that displays a frame around a group of controls with an optional caption.</summary>
	// Token: 0x0200025C RID: 604
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("Enter")]
	[DefaultProperty("Text")]
	[Designer("System.Windows.Forms.Design.GroupBoxDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionGroupBox")]
	public class GroupBox : Control
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.GroupBox" /> class.</summary>
		// Token: 0x0600245A RID: 9306 RVA: 0x000B0B64 File Offset: 0x000AED64
		public GroupBox()
		{
			base.SetState2(2048, true);
			base.SetStyle(ControlStyles.ContainerControl, true);
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, this.OwnerDraw);
			base.SetStyle(ControlStyles.Selectable, false);
			this.TabStop = false;
		}

		/// <summary>Gets or sets a value that indicates whether the control will allow drag-and-drop operations and events to be used.</summary>
		/// <returns>
		///     <see langword="true" /> to allow drag-and-drop operations and events to be used; otherwise, <see langword="false" />.</returns>
		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x0600245B RID: 9307 RVA: 0x000B0BBD File Offset: 0x000AEDBD
		// (set) Token: 0x0600245C RID: 9308 RVA: 0x000B0BC5 File Offset: 0x000AEDC5
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Forms.GroupBox" /> resizes based on its contents.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.GroupBox" /> automatically resizes based on its contents; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x0600245D RID: 9309 RVA: 0x0001BA13 File Offset: 0x00019C13
		// (set) Token: 0x0600245E RID: 9310 RVA: 0x000B0BCE File Offset: 0x000AEDCE
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.GroupBox.AutoSize" /> property changes.</summary>
		// Token: 0x1400019C RID: 412
		// (add) Token: 0x0600245F RID: 9311 RVA: 0x0001BA2E File Offset: 0x00019C2E
		// (remove) Token: 0x06002460 RID: 9312 RVA: 0x0001BA37 File Offset: 0x00019C37
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnAutoSizeChangedDescr")]
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

		/// <summary>Gets or sets how the <see cref="T:System.Windows.Forms.GroupBox" /> behaves when its <see cref="P:System.Windows.Forms.Control.AutoSize" /> property is enabled. </summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AutoSizeMode" /> values. The default is <see cref="F:System.Windows.Forms.AutoSizeMode.GrowOnly" />.</returns>
		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06002461 RID: 9313 RVA: 0x0001B4F5 File Offset: 0x000196F5
		// (set) Token: 0x06002462 RID: 9314 RVA: 0x000B0BD8 File Offset: 0x000AEDD8
		[SRDescription("ControlAutoSizeModeDescr")]
		[SRCategory("CatLayout")]
		[Browsable(true)]
		[DefaultValue(AutoSizeMode.GrowOnly)]
		[Localizable(true)]
		public AutoSizeMode AutoSizeMode
		{
			get
			{
				return base.GetAutoSizeMode();
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoSizeMode));
				}
				if (base.GetAutoSizeMode() != value)
				{
					base.SetAutoSizeMode(value);
					if (this.ParentInternal != null)
					{
						if (this.ParentInternal.LayoutEngine == DefaultLayout.Instance)
						{
							this.ParentInternal.LayoutEngine.InitLayout(this, BoundsSpecified.Size);
						}
						LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.AutoSize);
					}
				}
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06002463 RID: 9315 RVA: 0x000B0C5C File Offset: 0x000AEE5C
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				if (!this.OwnerDraw)
				{
					createParams.ClassName = "BUTTON";
					createParams.Style |= 7;
				}
				else
				{
					createParams.ClassName = null;
					createParams.Style &= -8;
				}
				createParams.ExStyle |= 65536;
				return createParams;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.Padding" /> structure that contains the default padding settings for a <see cref="T:System.Windows.Forms.GroupBox" /> control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> with all its edges set to three pixels. </returns>
		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06002464 RID: 9316 RVA: 0x000B0CBC File Offset: 0x000AEEBC
		protected override Padding DefaultPadding
		{
			get
			{
				return new Padding(3);
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06002465 RID: 9317 RVA: 0x000B0CC4 File Offset: 0x000AEEC4
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, 100);
			}
		}

		/// <summary>Gets a rectangle that represents the dimensions of the <see cref="T:System.Windows.Forms.GroupBox" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> with the dimensions of the <see cref="T:System.Windows.Forms.GroupBox" />.</returns>
		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06002466 RID: 9318 RVA: 0x000B0CD4 File Offset: 0x000AEED4
		public override Rectangle DisplayRectangle
		{
			get
			{
				Size clientSize = base.ClientSize;
				if (this.fontHeight == -1)
				{
					this.fontHeight = this.Font.Height;
					this.cachedFont = this.Font;
				}
				else if (this.cachedFont != this.Font)
				{
					this.fontHeight = this.Font.Height;
					this.cachedFont = this.Font;
				}
				Padding padding = base.Padding;
				return new Rectangle(padding.Left, this.fontHeight + padding.Top, Math.Max(clientSize.Width - padding.Horizontal, 0), Math.Max(clientSize.Height - this.fontHeight - padding.Vertical, 0));
			}
		}

		/// <summary>Gets or sets the flat style appearance of the group box control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.FlatStyle" /> values. The default value is <see cref="F:System.Windows.Forms.FlatStyle.Standard" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.FlatStyle" /> values. </exception>
		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06002467 RID: 9319 RVA: 0x000B0D8D File Offset: 0x000AEF8D
		// (set) Token: 0x06002468 RID: 9320 RVA: 0x000B0D98 File Offset: 0x000AEF98
		[SRCategory("CatAppearance")]
		[DefaultValue(FlatStyle.Standard)]
		[SRDescription("ButtonFlatStyleDescr")]
		public FlatStyle FlatStyle
		{
			get
			{
				return this.flatStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FlatStyle));
				}
				if (this.flatStyle != value)
				{
					bool ownerDraw = this.OwnerDraw;
					this.flatStyle = value;
					bool flag = this.OwnerDraw != ownerDraw;
					base.SetStyle(ControlStyles.ContainerControl, true);
					base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.UserMouse | ControlStyles.SupportsTransparentBackColor, this.OwnerDraw);
					if (flag)
					{
						base.RecreateHandle();
						return;
					}
					this.Refresh();
				}
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06002469 RID: 9321 RVA: 0x000B0E18 File Offset: 0x000AF018
		private bool OwnerDraw
		{
			get
			{
				return this.FlatStyle != FlatStyle.System;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the user can press the TAB key to give the focus to the <see cref="T:System.Windows.Forms.GroupBox" />.</summary>
		/// <returns>
		///     <see langword="true" /> to allow the user to press the TAB key to give the focus to the <see cref="T:System.Windows.Forms.GroupBox" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x0600246A RID: 9322 RVA: 0x000AA115 File Offset: 0x000A8315
		// (set) Token: 0x0600246B RID: 9323 RVA: 0x000AA11D File Offset: 0x000A831D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.GroupBox.TabStop" /> property changes.</summary>
		// Token: 0x1400019D RID: 413
		// (add) Token: 0x0600246C RID: 9324 RVA: 0x000AA126 File Offset: 0x000A8326
		// (remove) Token: 0x0600246D RID: 9325 RVA: 0x000AA12F File Offset: 0x000A832F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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
		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x0600246E RID: 9326 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x0600246F RID: 9327 RVA: 0x000B0E28 File Offset: 0x000AF028
		[Localizable(true)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				bool visible = base.Visible;
				try
				{
					if (visible && base.IsHandleCreated)
					{
						base.SendMessage(11, 0, 0);
					}
					base.Text = value;
				}
				finally
				{
					if (visible && base.IsHandleCreated)
					{
						base.SendMessage(11, 1, 0);
					}
				}
				base.Invalidate(true);
			}
		}

		/// <summary>Gets or sets a value that determines whether to use the <see cref="T:System.Drawing.Graphics" /> class (GDI+) or the <see cref="T:System.Windows.Forms.TextRenderer" /> class (GDI) to render text</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Drawing.Graphics" /> class should be used to perform text rendering for compatibility with versions 1.0 and 1.1. of the .NET Framework; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06002470 RID: 9328 RVA: 0x0001C7CB File Offset: 0x0001A9CB
		// (set) Token: 0x06002471 RID: 9329 RVA: 0x0001C7D3 File Offset: 0x0001A9D3
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

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06002472 RID: 9330 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override bool SupportsUseCompatibleTextRendering
		{
			get
			{
				return true;
			}
		}

		/// <summary>Occurs when the user clicks the <see cref="T:System.Windows.Forms.GroupBox" /> control.</summary>
		// Token: 0x1400019E RID: 414
		// (add) Token: 0x06002473 RID: 9331 RVA: 0x000A2B72 File Offset: 0x000A0D72
		// (remove) Token: 0x06002474 RID: 9332 RVA: 0x000A2B7B File Offset: 0x000A0D7B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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

		/// <summary>Occurs when the user clicks the <see cref="T:System.Windows.Forms.GroupBox" /> control with the mouse.</summary>
		// Token: 0x1400019F RID: 415
		// (add) Token: 0x06002475 RID: 9333 RVA: 0x000A2FE9 File Offset: 0x000A11E9
		// (remove) Token: 0x06002476 RID: 9334 RVA: 0x000A2FF2 File Offset: 0x000A11F2
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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

		/// <summary>Occurs when the user double-clicks the <see cref="T:System.Windows.Forms.GroupBox" /> control.</summary>
		// Token: 0x140001A0 RID: 416
		// (add) Token: 0x06002477 RID: 9335 RVA: 0x0001B6FB File Offset: 0x000198FB
		// (remove) Token: 0x06002478 RID: 9336 RVA: 0x0001B704 File Offset: 0x00019904
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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

		/// <summary>Occurs when the user double-clicks the <see cref="T:System.Windows.Forms.GroupBox" /> control with the mouse.</summary>
		// Token: 0x140001A1 RID: 417
		// (add) Token: 0x06002479 RID: 9337 RVA: 0x0001B70D File Offset: 0x0001990D
		// (remove) Token: 0x0600247A RID: 9338 RVA: 0x0001B716 File Offset: 0x00019916
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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

		/// <summary>Occurs when the user releases a key while the <see cref="T:System.Windows.Forms.GroupBox" /> control has focus.</summary>
		// Token: 0x140001A2 RID: 418
		// (add) Token: 0x0600247B RID: 9339 RVA: 0x000B0E8C File Offset: 0x000AF08C
		// (remove) Token: 0x0600247C RID: 9340 RVA: 0x000B0E95 File Offset: 0x000AF095
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

		/// <summary>Occurs when the user presses a key while the <see cref="T:System.Windows.Forms.GroupBox" /> control has focus.</summary>
		// Token: 0x140001A3 RID: 419
		// (add) Token: 0x0600247D RID: 9341 RVA: 0x000B0E9E File Offset: 0x000AF09E
		// (remove) Token: 0x0600247E RID: 9342 RVA: 0x000B0EA7 File Offset: 0x000AF0A7
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

		/// <summary>Occurs when the user presses a key while the <see cref="T:System.Windows.Forms.GroupBox" /> control has focus. </summary>
		// Token: 0x140001A4 RID: 420
		// (add) Token: 0x0600247F RID: 9343 RVA: 0x000B0EB0 File Offset: 0x000AF0B0
		// (remove) Token: 0x06002480 RID: 9344 RVA: 0x000B0EB9 File Offset: 0x000AF0B9
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

		/// <summary>Occurs when the user presses a mouse button while the mouse pointer is over the control.</summary>
		// Token: 0x140001A5 RID: 421
		// (add) Token: 0x06002481 RID: 9345 RVA: 0x000B0EC2 File Offset: 0x000AF0C2
		// (remove) Token: 0x06002482 RID: 9346 RVA: 0x000B0ECB File Offset: 0x000AF0CB
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

		/// <summary>Occurs when the user releases a mouse button while the mouse pointer is over the control.</summary>
		// Token: 0x140001A6 RID: 422
		// (add) Token: 0x06002483 RID: 9347 RVA: 0x000B0ED4 File Offset: 0x000AF0D4
		// (remove) Token: 0x06002484 RID: 9348 RVA: 0x000B0EDD File Offset: 0x000AF0DD
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

		/// <summary>Occurs when the user moves the mouse pointer over the control.</summary>
		// Token: 0x140001A7 RID: 423
		// (add) Token: 0x06002485 RID: 9349 RVA: 0x000B0EE6 File Offset: 0x000AF0E6
		// (remove) Token: 0x06002486 RID: 9350 RVA: 0x000B0EEF File Offset: 0x000AF0EF
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
		// Token: 0x140001A8 RID: 424
		// (add) Token: 0x06002487 RID: 9351 RVA: 0x000B0EF8 File Offset: 0x000AF0F8
		// (remove) Token: 0x06002488 RID: 9352 RVA: 0x000B0F01 File Offset: 0x000AF101
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
		// Token: 0x140001A9 RID: 425
		// (add) Token: 0x06002489 RID: 9353 RVA: 0x000B0F0A File Offset: 0x000AF10A
		// (remove) Token: 0x0600248A RID: 9354 RVA: 0x000B0F13 File Offset: 0x000AF113
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

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		// Token: 0x0600248B RID: 9355 RVA: 0x000B0F1C File Offset: 0x000AF11C
		protected override void OnPaint(PaintEventArgs e)
		{
			if (Application.RenderWithVisualStyles && base.Width >= 10 && base.Height >= 10)
			{
				GroupBoxState state = base.Enabled ? GroupBoxState.Normal : GroupBoxState.Disabled;
				TextFormatFlags textFormatFlags = TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak | TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform;
				if (!this.ShowKeyboardCues)
				{
					textFormatFlags |= TextFormatFlags.HidePrefix;
				}
				if (this.RightToLeft == RightToLeft.Yes)
				{
					textFormatFlags |= (TextFormatFlags.Right | TextFormatFlags.RightToLeft);
				}
				if (this.ShouldSerializeForeColor() || !base.Enabled)
				{
					Color textColor = base.Enabled ? this.ForeColor : TextRenderer.DisabledTextColor(this.BackColor);
					GroupBoxRenderer.DrawGroupBox(e.Graphics, new Rectangle(0, 0, base.Width, base.Height), this.Text, this.Font, textColor, textFormatFlags, state);
				}
				else
				{
					GroupBoxRenderer.DrawGroupBox(e.Graphics, new Rectangle(0, 0, base.Width, base.Height), this.Text, this.Font, textFormatFlags, state);
				}
			}
			else
			{
				this.DrawGroupBox(e);
			}
			base.OnPaint(e);
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x000B1018 File Offset: 0x000AF218
		private void DrawGroupBox(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = base.ClientRectangle;
			int num = 8;
			Color disabledColor = base.DisabledColor;
			Pen pen = new Pen(ControlPaint.Light(disabledColor, 1f));
			Pen pen2 = new Pen(ControlPaint.Dark(disabledColor, 0f));
			clientRectangle.X += num;
			clientRectangle.Width -= 2 * num;
			try
			{
				Size size;
				if (this.UseCompatibleTextRendering)
				{
					using (Brush brush = new SolidBrush(this.ForeColor))
					{
						using (StringFormat stringFormat = new StringFormat())
						{
							stringFormat.HotkeyPrefix = (this.ShowKeyboardCues ? HotkeyPrefix.Show : HotkeyPrefix.Hide);
							if (this.RightToLeft == RightToLeft.Yes)
							{
								stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
							}
							size = Size.Ceiling(graphics.MeasureString(this.Text, this.Font, clientRectangle.Width, stringFormat));
							if (base.Enabled)
							{
								graphics.DrawString(this.Text, this.Font, brush, clientRectangle, stringFormat);
								goto IL_1E6;
							}
							ControlPaint.DrawStringDisabled(graphics, this.Text, this.Font, disabledColor, clientRectangle, stringFormat);
							goto IL_1E6;
						}
					}
				}
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(graphics))
				{
					IntTextFormatFlags intTextFormatFlags = IntTextFormatFlags.TextBoxControl | IntTextFormatFlags.WordBreak;
					if (!this.ShowKeyboardCues)
					{
						intTextFormatFlags |= IntTextFormatFlags.HidePrefix;
					}
					if (this.RightToLeft == RightToLeft.Yes)
					{
						intTextFormatFlags |= IntTextFormatFlags.RightToLeft;
						intTextFormatFlags |= IntTextFormatFlags.Right;
					}
					using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(this.Font))
					{
						size = windowsGraphics.MeasureText(this.Text, windowsFont, new Size(clientRectangle.Width, int.MaxValue), intTextFormatFlags);
						if (base.Enabled)
						{
							windowsGraphics.DrawText(this.Text, windowsFont, clientRectangle, this.ForeColor, intTextFormatFlags);
						}
						else
						{
							ControlPaint.DrawStringDisabled(windowsGraphics, this.Text, this.Font, disabledColor, clientRectangle, (TextFormatFlags)intTextFormatFlags);
						}
					}
				}
				IL_1E6:
				int num2 = num;
				if (this.RightToLeft == RightToLeft.Yes)
				{
					num2 += clientRectangle.Width - size.Width;
				}
				int x = Math.Min(num2 + size.Width, base.Width - 6);
				int num3 = base.FontHeight / 2;
				if (SystemInformation.HighContrast && AccessibilityImprovements.Level1)
				{
					Color color;
					if (base.Enabled)
					{
						color = this.ForeColor;
					}
					else
					{
						color = SystemColors.GrayText;
					}
					bool flag = !color.IsSystemColor;
					Pen pen3 = null;
					try
					{
						if (flag)
						{
							pen3 = new Pen(color);
						}
						else
						{
							pen3 = SystemPens.FromSystemColor(color);
						}
						graphics.DrawLine(pen3, 0, num3, 0, base.Height);
						graphics.DrawLine(pen3, 0, base.Height - 1, base.Width, base.Height - 1);
						graphics.DrawLine(pen3, 0, num3, num2, num3);
						graphics.DrawLine(pen3, x, num3, base.Width - 1, num3);
						graphics.DrawLine(pen3, base.Width - 1, num3, base.Width - 1, base.Height - 1);
						return;
					}
					finally
					{
						if (flag && pen3 != null)
						{
							pen3.Dispose();
						}
					}
				}
				graphics.DrawLine(pen, 1, num3, 1, base.Height - 1);
				graphics.DrawLine(pen2, 0, num3, 0, base.Height - 2);
				graphics.DrawLine(pen, 0, base.Height - 1, base.Width, base.Height - 1);
				graphics.DrawLine(pen2, 0, base.Height - 2, base.Width - 1, base.Height - 2);
				graphics.DrawLine(pen2, 0, num3 - 1, num2, num3 - 1);
				graphics.DrawLine(pen, 1, num3, num2, num3);
				graphics.DrawLine(pen2, x, num3 - 1, base.Width - 2, num3 - 1);
				graphics.DrawLine(pen, x, num3, base.Width - 1, num3);
				graphics.DrawLine(pen, base.Width - 1, num3 - 1, base.Width - 1, base.Height - 1);
				graphics.DrawLine(pen2, base.Width - 2, num3, base.Width - 2, base.Height - 2);
			}
			finally
			{
				pen.Dispose();
				pen2.Dispose();
			}
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x000B14D4 File Offset: 0x000AF6D4
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			Size sz = this.SizeFromClientSize(Size.Empty);
			Size sz2 = sz + new Size(0, this.fontHeight) + base.Padding.Size;
			Size preferredSize = this.LayoutEngine.GetPreferredSize(this, proposedSize - sz2);
			return preferredSize + sz2;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600248E RID: 9358 RVA: 0x000B152E File Offset: 0x000AF72E
		protected override void OnFontChanged(EventArgs e)
		{
			this.fontHeight = -1;
			this.cachedFont = null;
			base.Invalidate();
			base.OnFontChanged(e);
		}

		/// <summary>Processes a mnemonic character.</summary>
		/// <param name="charCode">The character to process.</param>
		/// <returns>
		///   <see langword="true" /> if the character was processed as a mnemonic by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600248F RID: 9359 RVA: 0x000B154C File Offset: 0x000AF74C
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (Control.IsMnemonic(charCode, this.Text) && this.CanProcessMnemonic())
			{
				IntSecurity.ModifyFocus.Assert();
				try
				{
					base.SelectNextControl(null, true, true, true, false);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				return true;
			}
			return false;
		}

		/// <summary>Scales the <see cref="T:System.Windows.Forms.GroupBox" /> by the specified factor and scaling instruction.</summary>
		/// <param name="factor">The <see cref="T:System.Drawing.SizeF" /> that indicates the height and width of the scaled control.</param>
		/// <param name="specified">One of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values that indicates how the control should be scaled.</param>
		// Token: 0x06002490 RID: 9360 RVA: 0x000B15A0 File Offset: 0x000AF7A0
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			if (factor.Width != 1f && factor.Height != 1f)
			{
				this.fontHeight = -1;
				this.cachedFont = null;
			}
			base.ScaleControl(factor, specified);
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06002491 RID: 9361 RVA: 0x00020C1B File Offset: 0x0001EE1B
		internal override bool SupportsUiaProviders
		{
			get
			{
				return AccessibilityImprovements.Level3 && !base.DesignMode;
			}
		}

		/// <summary>Returns a <see cref="T:System.String" /> containing the name of the <see cref="T:System.ComponentModel.Component" />, if any. This method should not be overridden.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the name of the <see cref="T:System.ComponentModel.Component" />, if any, or <see langword="null" /> if the <see cref="T:System.ComponentModel.Component" /> is unnamed.</returns>
		// Token: 0x06002492 RID: 9362 RVA: 0x000B15D4 File Offset: 0x000AF7D4
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", Text: " + this.Text;
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x000B15FC File Offset: 0x000AF7FC
		private void WmEraseBkgnd(ref Message m)
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			SafeNativeMethods.GetClientRect(new HandleRef(this, base.Handle), ref rect);
			using (Graphics graphics = Graphics.FromHdcInternal(m.WParam))
			{
				using (Brush brush = new SolidBrush(this.BackColor))
				{
					graphics.FillRectangle(brush, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
				}
			}
			m.Result = (IntPtr)1;
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06002494 RID: 9364 RVA: 0x000B16A8 File Offset: 0x000AF8A8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (this.OwnerDraw)
			{
				base.WndProc(ref m);
				return;
			}
			int msg = m.Msg;
			if (msg != 20)
			{
				if (msg != 61)
				{
					if (msg == 792)
					{
						goto IL_29;
					}
					base.WndProc(ref m);
				}
				else
				{
					base.WndProc(ref m);
					if ((int)((long)m.LParam) == -12)
					{
						m.Result = IntPtr.Zero;
						return;
					}
				}
				return;
			}
			IL_29:
			this.WmEraseBkgnd(ref m);
		}

		/// <summary>Creates a new accessibility object for the <see cref="T:System.Windows.Forms.GroupBox" />.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the <see cref="T:System.Windows.Forms.GroupBox" />.</returns>
		// Token: 0x06002495 RID: 9365 RVA: 0x000B1710 File Offset: 0x000AF910
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new GroupBox.GroupBoxAccessibleObject(this);
		}

		// Token: 0x04000FA4 RID: 4004
		private int fontHeight = -1;

		// Token: 0x04000FA5 RID: 4005
		private Font cachedFont;

		// Token: 0x04000FA6 RID: 4006
		private FlatStyle flatStyle = FlatStyle.Standard;

		// Token: 0x020005E8 RID: 1512
		[ComVisible(true)]
		internal class GroupBoxAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06005AF4 RID: 23284 RVA: 0x00093572 File Offset: 0x00091772
			internal GroupBoxAccessibleObject(GroupBox owner) : base(owner)
			{
			}

			// Token: 0x170015E6 RID: 5606
			// (get) Token: 0x06005AF5 RID: 23285 RVA: 0x0017DCEC File Offset: 0x0017BEEC
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.Grouping;
				}
			}

			// Token: 0x06005AF6 RID: 23286 RVA: 0x0009357B File Offset: 0x0009177B
			internal override bool IsIAccessibleExSupported()
			{
				return AccessibilityImprovements.Level3 || base.IsIAccessibleExSupported();
			}

			// Token: 0x06005AF7 RID: 23287 RVA: 0x0017DD0D File Offset: 0x0017BF0D
			internal override bool IsPatternSupported(int patternId)
			{
				return (AccessibilityImprovements.Level3 && patternId == 10018) || base.IsPatternSupported(patternId);
			}

			// Token: 0x06005AF8 RID: 23288 RVA: 0x0017DD28 File Offset: 0x0017BF28
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID != 30003)
				{
					if (propertyID != 30005)
					{
						if (propertyID == 30009)
						{
							return true;
						}
					}
					else if (AccessibilityImprovements.Level3)
					{
						return this.Name;
					}
					return base.GetPropertyValue(propertyID);
				}
				return 50026;
			}
		}
	}
}
