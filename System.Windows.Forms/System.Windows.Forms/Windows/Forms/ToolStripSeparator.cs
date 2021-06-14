using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
	/// <summary>Represents a line used to group items of a <see cref="T:System.Windows.Forms.ToolStrip" /> or the drop-down items of a <see cref="T:System.Windows.Forms.MenuStrip" /> or <see cref="T:System.Windows.Forms.ContextMenuStrip" /> or other <see cref="T:System.Windows.Forms.ToolStripDropDown" /> control.</summary>
	// Token: 0x020003EE RID: 1006
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
	public class ToolStripSeparator : ToolStripItem
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripSeparator" /> class. </summary>
		// Token: 0x060043A9 RID: 17321 RVA: 0x00121F99 File Offset: 0x00120199
		public ToolStripSeparator()
		{
			this.ForeColor = SystemColors.ControlDark;
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />. </returns>
		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x060043AA RID: 17322 RVA: 0x0010A478 File Offset: 0x00108678
		// (set) Token: 0x060043AB RID: 17323 RVA: 0x0010A480 File Offset: 0x00108680
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool AutoToolTip
		{
			get
			{
				return base.AutoToolTip;
			}
			set
			{
				base.AutoToolTip = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The image to display in the background of the separator.</returns>
		// Token: 0x170010EE RID: 4334
		// (get) Token: 0x060043AC RID: 17324 RVA: 0x00121FAC File Offset: 0x001201AC
		// (set) Token: 0x060043AD RID: 17325 RVA: 0x00121FB4 File Offset: 0x001201B4
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
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImageLayout" /> values.</returns>
		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x060043AE RID: 17326 RVA: 0x00121FBD File Offset: 0x001201BD
		// (set) Token: 0x060043AF RID: 17327 RVA: 0x00121FC5 File Offset: 0x001201C5
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

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripSeparator" /> can be selected. </summary>
		/// <returns>
		///     <see langword="true" /> if the component using the <see cref="T:System.Windows.Forms.ToolStripSeparator" /> is in design mode; otherwise, <see langword="false" />.</returns>
		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x060043B0 RID: 17328 RVA: 0x00105831 File Offset: 0x00103A31
		public override bool CanSelect
		{
			get
			{
				return base.DesignMode;
			}
		}

		/// <summary>Gets the default size of the <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Size" /> of the <see cref="T:System.Windows.Forms.ToolStripSeparator" />, measured in pixels. The default is 100 pixels horizontally.</returns>
		// Token: 0x170010F1 RID: 4337
		// (get) Token: 0x060043B1 RID: 17329 RVA: 0x00121FCE File Offset: 0x001201CE
		protected override Size DefaultSize
		{
			get
			{
				return new Size(6, 6);
			}
		}

		/// <summary>Gets the spacing between the <see cref="T:System.Windows.Forms.ToolStripSeparator" /> and an adjacent item.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> value representing the spacing.</returns>
		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x060043B2 RID: 17330 RVA: 0x000119C9 File Offset: 0x0000FBC9
		protected internal override Padding DefaultMargin
		{
			get
			{
				return Padding.Empty;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />. </returns>
		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x060043B3 RID: 17331 RVA: 0x0010B03D File Offset: 0x0010923D
		// (set) Token: 0x060043B4 RID: 17332 RVA: 0x0010B045 File Offset: 0x00109245
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool DoubleClickEnabled
		{
			get
			{
				return base.DoubleClickEnabled;
			}
			set
			{
				base.DoubleClickEnabled = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x060043B5 RID: 17333 RVA: 0x00121FD7 File Offset: 0x001201D7
		// (set) Token: 0x060043B6 RID: 17334 RVA: 0x0011895D File Offset: 0x00116B5D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x14000378 RID: 888
		// (add) Token: 0x060043B7 RID: 17335 RVA: 0x00121FDF File Offset: 0x001201DF
		// (remove) Token: 0x060043B8 RID: 17336 RVA: 0x00121FE8 File Offset: 0x001201E8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler EnabledChanged
		{
			add
			{
				base.EnabledChanged += value;
			}
			remove
			{
				base.EnabledChanged -= value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripItemDisplayStyle" /> values.</returns>
		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x060043B9 RID: 17337 RVA: 0x0010B006 File Offset: 0x00109206
		// (set) Token: 0x060043BA RID: 17338 RVA: 0x0010B00E File Offset: 0x0010920E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ToolStripItemDisplayStyle DisplayStyle
		{
			get
			{
				return base.DisplayStyle;
			}
			set
			{
				base.DisplayStyle = value;
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x14000379 RID: 889
		// (add) Token: 0x060043BB RID: 17339 RVA: 0x00121FF1 File Offset: 0x001201F1
		// (remove) Token: 0x060043BC RID: 17340 RVA: 0x00121FFA File Offset: 0x001201FA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DisplayStyleChanged
		{
			add
			{
				base.DisplayStyleChanged += value;
			}
			remove
			{
				base.DisplayStyleChanged -= value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> to apply to the text displayed by the <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</returns>
		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x060043BD RID: 17341 RVA: 0x00122003 File Offset: 0x00120203
		// (set) Token: 0x060043BE RID: 17342 RVA: 0x0012200B File Offset: 0x0012020B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.ContentAlignment" /> values.</returns>
		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x060043BF RID: 17343 RVA: 0x0010B12B File Offset: 0x0010932B
		// (set) Token: 0x060043C0 RID: 17344 RVA: 0x0010B133 File Offset: 0x00109333
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ContentAlignment ImageAlign
		{
			get
			{
				return base.ImageAlign;
			}
			set
			{
				base.ImageAlign = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The image to be displayed.</returns>
		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x060043C1 RID: 17345 RVA: 0x0010B0F8 File Offset: 0x001092F8
		// (set) Token: 0x060043C2 RID: 17346 RVA: 0x0010B100 File Offset: 0x00109300
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Image Image
		{
			get
			{
				return base.Image;
			}
			set
			{
				base.Image = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The index of the image that is displayed.</returns>
		// Token: 0x170010F9 RID: 4345
		// (get) Token: 0x060043C3 RID: 17347 RVA: 0x00122014 File Offset: 0x00120214
		// (set) Token: 0x060043C4 RID: 17348 RVA: 0x0012201C File Offset: 0x0012021C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new int ImageIndex
		{
			get
			{
				return base.ImageIndex;
			}
			set
			{
				base.ImageIndex = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The key for the image that is displayed for the <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</returns>
		// Token: 0x170010FA RID: 4346
		// (get) Token: 0x060043C5 RID: 17349 RVA: 0x00122025 File Offset: 0x00120225
		// (set) Token: 0x060043C6 RID: 17350 RVA: 0x0012202D File Offset: 0x0012022D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string ImageKey
		{
			get
			{
				return base.ImageKey;
			}
			set
			{
				base.ImageKey = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Color" /> values.</returns>
		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x060043C7 RID: 17351 RVA: 0x0010B11A File Offset: 0x0010931A
		// (set) Token: 0x060043C8 RID: 17352 RVA: 0x0010B122 File Offset: 0x00109322
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color ImageTransparentColor
		{
			get
			{
				return base.ImageTransparentColor;
			}
			set
			{
				base.ImageTransparentColor = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripItemImageScaling" /> value.</returns>
		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x060043C9 RID: 17353 RVA: 0x0010B109 File Offset: 0x00109309
		// (set) Token: 0x060043CA RID: 17354 RVA: 0x0010B111 File Offset: 0x00109311
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ToolStripItemImageScaling ImageScaling
		{
			get
			{
				return base.ImageScaling;
			}
			set
			{
				base.ImageScaling = value;
			}
		}

		// Token: 0x170010FD RID: 4349
		// (get) Token: 0x060043CB RID: 17355 RVA: 0x00122038 File Offset: 0x00120238
		private bool IsVertical
		{
			get
			{
				ToolStrip toolStrip = base.ParentInternal;
				if (toolStrip == null)
				{
					toolStrip = base.Owner;
				}
				ToolStripDropDownMenu toolStripDropDownMenu = toolStrip as ToolStripDropDownMenu;
				if (toolStripDropDownMenu != null)
				{
					return false;
				}
				switch (toolStrip.LayoutStyle)
				{
				case ToolStripLayoutStyle.VerticalStackWithOverflow:
					return false;
				}
				return true;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>A <see cref="T:System.String" /> representing the item’s text.</returns>
		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x060043CC RID: 17356 RVA: 0x00122086 File Offset: 0x00120286
		// (set) Token: 0x060043CD RID: 17357 RVA: 0x0012208E File Offset: 0x0012028E
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

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x1400037A RID: 890
		// (add) Token: 0x060043CE RID: 17358 RVA: 0x00120334 File Offset: 0x0011E534
		// (remove) Token: 0x060043CF RID: 17359 RVA: 0x0012033D File Offset: 0x0011E53D
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

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.ContentAlignment" /> value.</returns>
		// Token: 0x170010FF RID: 4351
		// (get) Token: 0x060043D0 RID: 17360 RVA: 0x0010B348 File Offset: 0x00109548
		// (set) Token: 0x060043D1 RID: 17361 RVA: 0x0010B350 File Offset: 0x00109550
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ContentAlignment TextAlign
		{
			get
			{
				return base.TextAlign;
			}
			set
			{
				base.TextAlign = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripTextDirection" /> values.</returns>
		// Token: 0x17001100 RID: 4352
		// (get) Token: 0x060043D2 RID: 17362 RVA: 0x0010B359 File Offset: 0x00109559
		// (set) Token: 0x060043D3 RID: 17363 RVA: 0x0010B361 File Offset: 0x00109561
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue(ToolStripTextDirection.Horizontal)]
		public override ToolStripTextDirection TextDirection
		{
			get
			{
				return base.TextDirection;
			}
			set
			{
				base.TextDirection = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.TextImageRelation" /> values.</returns>
		// Token: 0x17001101 RID: 4353
		// (get) Token: 0x060043D4 RID: 17364 RVA: 0x0010B36A File Offset: 0x0010956A
		// (set) Token: 0x060043D5 RID: 17365 RVA: 0x0010B372 File Offset: 0x00109572
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new TextImageRelation TextImageRelation
		{
			get
			{
				return base.TextImageRelation;
			}
			set
			{
				base.TextImageRelation = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>A string representing the ToolTip text.</returns>
		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x060043D6 RID: 17366 RVA: 0x001153D1 File Offset: 0x001135D1
		// (set) Token: 0x060043D7 RID: 17367 RVA: 0x00122097 File Offset: 0x00120297
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string ToolTipText
		{
			get
			{
				return base.ToolTipText;
			}
			set
			{
				base.ToolTipText = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x060043D8 RID: 17368 RVA: 0x0010B22C File Offset: 0x0010942C
		// (set) Token: 0x060043D9 RID: 17369 RVA: 0x0010B234 File Offset: 0x00109434
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool RightToLeftAutoMirrorImage
		{
			get
			{
				return base.RightToLeftAutoMirrorImage;
			}
			set
			{
				base.RightToLeftAutoMirrorImage = value;
			}
		}

		/// <summary>Creates a new accessibility object for the <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.</returns>
		// Token: 0x060043DA RID: 17370 RVA: 0x001220A0 File Offset: 0x001202A0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripSeparator.ToolStripSeparatorAccessibleObject(this);
		}

		/// <summary>Retrieves the size of a rectangular area into which a <see cref="T:System.Windows.Forms.ToolStripSeparator" /> can be fitted.</summary>
		/// <param name="constrainingSize">A <see cref="T:System.Drawing.Size" /> representing the height and width of the <see cref="T:System.Windows.Forms.ToolStripSeparator" />, in pixels.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> representing the height and width of the <see cref="T:System.Windows.Forms.ToolStripSeparator" />, in pixels.</returns>
		// Token: 0x060043DB RID: 17371 RVA: 0x001220A8 File Offset: 0x001202A8
		public override Size GetPreferredSize(Size constrainingSize)
		{
			ToolStrip toolStrip = base.ParentInternal;
			if (toolStrip == null)
			{
				toolStrip = base.Owner;
			}
			if (toolStrip == null)
			{
				return new Size(6, 6);
			}
			ToolStripDropDownMenu toolStripDropDownMenu = toolStrip as ToolStripDropDownMenu;
			if (toolStripDropDownMenu != null)
			{
				return new Size(toolStrip.Width - (toolStrip.Padding.Horizontal - toolStripDropDownMenu.ImageMargin.Width), 6);
			}
			if (toolStrip.LayoutStyle != ToolStripLayoutStyle.HorizontalStackWithOverflow || toolStrip.LayoutStyle != ToolStripLayoutStyle.VerticalStackWithOverflow)
			{
				constrainingSize.Width = 23;
				constrainingSize.Height = 23;
			}
			if (this.IsVertical)
			{
				return new Size(6, constrainingSize.Height);
			}
			return new Size(constrainingSize.Width, 6);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060043DC RID: 17372 RVA: 0x0012214D File Offset: 0x0012034D
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.Owner != null && base.ParentInternal != null)
			{
				base.Renderer.DrawSeparator(new ToolStripSeparatorRenderEventArgs(e.Graphics, this, this.IsVertical));
			}
		}

		/// <summary>This method is not relevant to this class.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060043DD RID: 17373 RVA: 0x0012217C File Offset: 0x0012037C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void OnFontChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripItem.EventFontChanged, e);
		}

		// Token: 0x060043DE RID: 17374 RVA: 0x0012218A File Offset: 0x0012038A
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal override bool ShouldSerializeForeColor()
		{
			return this.ForeColor != SystemColors.ControlDark;
		}

		/// <summary>Sets the size and location of the <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> specifying the size and location of the <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</param>
		// Token: 0x060043DF RID: 17375 RVA: 0x0012219C File Offset: 0x0012039C
		protected internal override void SetBounds(Rectangle rect)
		{
			ToolStripDropDownMenu toolStripDropDownMenu = base.Owner as ToolStripDropDownMenu;
			if (toolStripDropDownMenu != null && toolStripDropDownMenu != null)
			{
				rect.X = 2;
				rect.Width = toolStripDropDownMenu.Width - 4;
			}
			base.SetBounds(rect);
		}

		// Token: 0x040025AE RID: 9646
		private const int WINBAR_SEPARATORTHICKNESS = 6;

		// Token: 0x040025AF RID: 9647
		private const int WINBAR_SEPARATORHEIGHT = 23;

		// Token: 0x0200074E RID: 1870
		[ComVisible(true)]
		internal class ToolStripSeparatorAccessibleObject : ToolStripItem.ToolStripItemAccessibleObject
		{
			// Token: 0x060061EB RID: 25067 RVA: 0x001912BF File Offset: 0x0018F4BF
			public ToolStripSeparatorAccessibleObject(ToolStripSeparator ownerItem) : base(ownerItem)
			{
				this.ownerItem = ownerItem;
			}

			// Token: 0x1700175F RID: 5983
			// (get) Token: 0x060061EC RID: 25068 RVA: 0x001912D0 File Offset: 0x0018F4D0
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = this.ownerItem.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.Separator;
				}
			}

			// Token: 0x060061ED RID: 25069 RVA: 0x001912F1 File Offset: 0x0018F4F1
			internal override object GetPropertyValue(int propertyID)
			{
				if (AccessibilityImprovements.Level3 && propertyID == 30003)
				{
					return 50038;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x040041A7 RID: 16807
			private ToolStripSeparator ownerItem;
		}
	}
}
