using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Handles the painting functionality for <see cref="T:System.Windows.Forms.ToolStrip" /> objects, applying a custom palette and a streamlined style.</summary>
	// Token: 0x020003E5 RID: 997
	public class ToolStripProfessionalRenderer : ToolStripRenderer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripProfessionalRenderer" /> class. </summary>
		// Token: 0x060042AD RID: 17069 RVA: 0x0011D2D0 File Offset: 0x0011B4D0
		public ToolStripProfessionalRenderer()
		{
		}

		// Token: 0x060042AE RID: 17070 RVA: 0x0011D328 File Offset: 0x0011B528
		internal ToolStripProfessionalRenderer(bool isDefault) : base(isDefault)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripProfessionalRenderer" /> class. </summary>
		/// <param name="professionalColorTable">A <see cref="T:System.Windows.Forms.ProfessionalColorTable" /> to be used for painting.</param>
		// Token: 0x060042AF RID: 17071 RVA: 0x0011D380 File Offset: 0x0011B580
		public ToolStripProfessionalRenderer(ProfessionalColorTable professionalColorTable)
		{
			this.professionalColorTable = professionalColorTable;
		}

		/// <summary>Gets the color palette used for painting.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ProfessionalColorTable" /> used for painting.</returns>
		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x060042B0 RID: 17072 RVA: 0x0011D3DD File Offset: 0x0011B5DD
		public ProfessionalColorTable ColorTable
		{
			get
			{
				if (this.professionalColorTable == null)
				{
					return ProfessionalColors.ColorTable;
				}
				return this.professionalColorTable;
			}
		}

		// Token: 0x170010C9 RID: 4297
		// (get) Token: 0x060042B1 RID: 17073 RVA: 0x0011D3F3 File Offset: 0x0011B5F3
		internal override ToolStripRenderer RendererOverride
		{
			get
			{
				if (DisplayInformation.HighContrast)
				{
					return this.HighContrastRenderer;
				}
				if (DisplayInformation.LowResolution)
				{
					return this.LowResolutionRenderer;
				}
				return null;
			}
		}

		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x060042B2 RID: 17074 RVA: 0x0011D412 File Offset: 0x0011B612
		internal ToolStripRenderer HighContrastRenderer
		{
			get
			{
				if (this.toolStripHighContrastRenderer == null)
				{
					this.toolStripHighContrastRenderer = new ToolStripHighContrastRenderer(false);
				}
				return this.toolStripHighContrastRenderer;
			}
		}

		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x060042B3 RID: 17075 RVA: 0x0011D42E File Offset: 0x0011B62E
		internal ToolStripRenderer LowResolutionRenderer
		{
			get
			{
				if (this.toolStripLowResolutionRenderer == null)
				{
					this.toolStripLowResolutionRenderer = new ToolStripProfessionalLowResolutionRenderer();
				}
				return this.toolStripLowResolutionRenderer;
			}
		}

		/// <summary>Gets or sets a value indicating whether edges of controls have a rounded rather than a square or sharp appearance.</summary>
		/// <returns>
		///     <see langword="true" /> to round off control edges; otherwise, <see langword="false" />.</returns>
		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x060042B4 RID: 17076 RVA: 0x0011D449 File Offset: 0x0011B649
		// (set) Token: 0x060042B5 RID: 17077 RVA: 0x0011D451 File Offset: 0x0011B651
		public bool RoundedEdges
		{
			get
			{
				return this.roundedEdges;
			}
			set
			{
				this.roundedEdges = value;
			}
		}

		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x060042B6 RID: 17078 RVA: 0x0011D45A File Offset: 0x0011B65A
		private bool UseSystemColors
		{
			get
			{
				return this.ColorTable.UseSystemColors || !ToolStripManager.VisualStylesEnabled;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderToolStripBackground" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042B7 RID: 17079 RVA: 0x0011D474 File Offset: 0x0011B674
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderToolStripBackground(e);
				return;
			}
			ToolStrip toolStrip = e.ToolStrip;
			if (!base.ShouldPaintBackground(toolStrip))
			{
				return;
			}
			if (toolStrip is ToolStripDropDown)
			{
				this.RenderToolStripDropDownBackground(e);
				return;
			}
			if (toolStrip is MenuStrip)
			{
				this.RenderMenuStripBackground(e);
				return;
			}
			if (toolStrip is StatusStrip)
			{
				this.RenderStatusStripBackground(e);
				return;
			}
			this.RenderToolStripBackgroundInternal(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderOverflowButtonBackground" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042B8 RID: 17080 RVA: 0x0011D4DC File Offset: 0x0011B6DC
		protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
		{
			this.ScaleObjectSizesIfNeeded(e.ToolStrip.DeviceDpi);
			if (this.RendererOverride != null)
			{
				base.OnRenderOverflowButtonBackground(e);
				return;
			}
			ToolStripItem item = e.Item;
			Graphics graphics = e.Graphics;
			bool flag = item.RightToLeft == RightToLeft.Yes;
			this.RenderOverflowBackground(e, flag);
			bool flag2 = e.ToolStrip.Orientation == Orientation.Horizontal;
			Rectangle empty = Rectangle.Empty;
			if (flag)
			{
				empty = new Rectangle(0, item.Height - this.overflowArrowOffsetY, this.overflowArrowWidth, this.overflowArrowHeight);
			}
			else
			{
				empty = new Rectangle(item.Width - this.overflowButtonWidth, item.Height - this.overflowArrowOffsetY, this.overflowArrowWidth, this.overflowArrowHeight);
			}
			ArrowDirection direction = flag2 ? ArrowDirection.Down : ArrowDirection.Right;
			int num = (flag && flag2) ? -1 : 1;
			empty.Offset(num, 1);
			this.RenderArrowInternal(graphics, empty, direction, SystemBrushes.ButtonHighlight);
			empty.Offset(-1 * num, -1);
			Point point = this.RenderArrowInternal(graphics, empty, direction, SystemBrushes.ControlText);
			if (flag2)
			{
				num = (flag ? -2 : 0);
				graphics.DrawLine(SystemPens.ControlText, point.X - ToolStripRenderer.Offset2X, empty.Y - ToolStripRenderer.Offset2Y, point.X + ToolStripRenderer.Offset2X, empty.Y - ToolStripRenderer.Offset2Y);
				graphics.DrawLine(SystemPens.ButtonHighlight, point.X - ToolStripRenderer.Offset2X + 1 + num, empty.Y - ToolStripRenderer.Offset2Y + 1, point.X + ToolStripRenderer.Offset2X + 1 + num, empty.Y - ToolStripRenderer.Offset2Y + 1);
				return;
			}
			graphics.DrawLine(SystemPens.ControlText, empty.X, point.Y - ToolStripRenderer.Offset2Y, empty.X, point.Y + ToolStripRenderer.Offset2Y);
			graphics.DrawLine(SystemPens.ButtonHighlight, empty.X + 1, point.Y - ToolStripRenderer.Offset2Y + 1, empty.X + 1, point.Y + ToolStripRenderer.Offset2Y + 1);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderDropDownButtonBackground" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042B9 RID: 17081 RVA: 0x0011D6F0 File Offset: 0x0011B8F0
		protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderDropDownButtonBackground(e);
				return;
			}
			ToolStripDropDownItem toolStripDropDownItem = e.Item as ToolStripDropDownItem;
			if (toolStripDropDownItem != null && toolStripDropDownItem.Pressed && toolStripDropDownItem.HasDropDownItems)
			{
				Rectangle bounds = new Rectangle(Point.Empty, toolStripDropDownItem.Size);
				this.RenderPressedGradient(e.Graphics, bounds);
				return;
			}
			this.RenderItemInternal(e, true);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderSeparator" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripSeparatorRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042BA RID: 17082 RVA: 0x0011D754 File Offset: 0x0011B954
		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderSeparator(e);
				return;
			}
			this.RenderSeparatorInternal(e.Graphics, e.Item, new Rectangle(Point.Empty, e.Item.Size), e.Vertical);
		}

		/// <summary>Raises the OnRenderSplitButtonBackground event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042BB RID: 17083 RVA: 0x0011D794 File Offset: 0x0011B994
		protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderSplitButtonBackground(e);
				return;
			}
			ToolStripSplitButton toolStripSplitButton = e.Item as ToolStripSplitButton;
			Graphics graphics = e.Graphics;
			if (toolStripSplitButton != null)
			{
				Rectangle rectangle = new Rectangle(Point.Empty, toolStripSplitButton.Size);
				if (toolStripSplitButton.BackgroundImage != null)
				{
					Rectangle clipRect = toolStripSplitButton.Selected ? toolStripSplitButton.ContentRectangle : rectangle;
					ControlPaint.DrawBackgroundImage(graphics, toolStripSplitButton.BackgroundImage, toolStripSplitButton.BackColor, toolStripSplitButton.BackgroundImageLayout, rectangle, clipRect);
				}
				bool flag = toolStripSplitButton.Pressed || toolStripSplitButton.ButtonPressed || toolStripSplitButton.Selected || toolStripSplitButton.ButtonSelected;
				if (flag)
				{
					this.RenderItemInternal(e, true);
				}
				if (toolStripSplitButton.ButtonPressed)
				{
					Rectangle rectangle2 = toolStripSplitButton.ButtonBounds;
					Padding padding = (toolStripSplitButton.RightToLeft == RightToLeft.Yes) ? new Padding(0, 1, 1, 1) : new Padding(1, 1, 0, 1);
					rectangle2 = LayoutUtils.DeflateRect(rectangle2, padding);
					this.RenderPressedButtonFill(graphics, rectangle2);
				}
				else if (toolStripSplitButton.Pressed)
				{
					this.RenderPressedGradient(e.Graphics, rectangle);
				}
				Rectangle dropDownButtonBounds = toolStripSplitButton.DropDownButtonBounds;
				if (flag && !toolStripSplitButton.Pressed)
				{
					using (Brush brush = new SolidBrush(this.ColorTable.ButtonSelectedBorder))
					{
						graphics.FillRectangle(brush, toolStripSplitButton.SplitterBounds);
					}
				}
				base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, toolStripSplitButton, dropDownButtonBounds, SystemColors.ControlText, ArrowDirection.Down));
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderToolStripStatusLabelBackground" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042BC RID: 17084 RVA: 0x0011D900 File Offset: 0x0011BB00
		protected override void OnRenderToolStripStatusLabelBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderToolStripStatusLabelBackground(e);
				return;
			}
			ToolStripProfessionalRenderer.RenderLabelInternal(e);
			ToolStripStatusLabel toolStripStatusLabel = e.Item as ToolStripStatusLabel;
			ControlPaint.DrawBorder3D(e.Graphics, new Rectangle(0, 0, toolStripStatusLabel.Width, toolStripStatusLabel.Height), toolStripStatusLabel.BorderStyle, (Border3DSide)toolStripStatusLabel.BorderSides);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderLabelBackground" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042BD RID: 17085 RVA: 0x0011D959 File Offset: 0x0011BB59
		protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderLabelBackground(e);
				return;
			}
			ToolStripProfessionalRenderer.RenderLabelInternal(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderButtonBackground" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042BE RID: 17086 RVA: 0x0011D974 File Offset: 0x0011BB74
		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderButtonBackground(e);
				return;
			}
			ToolStripButton toolStripButton = e.Item as ToolStripButton;
			Graphics graphics = e.Graphics;
			Rectangle rectangle = new Rectangle(Point.Empty, toolStripButton.Size);
			if (toolStripButton.CheckState == CheckState.Unchecked)
			{
				this.RenderItemInternal(e, true);
				return;
			}
			Rectangle clipRect = toolStripButton.Selected ? toolStripButton.ContentRectangle : rectangle;
			if (toolStripButton.BackgroundImage != null)
			{
				ControlPaint.DrawBackgroundImage(graphics, toolStripButton.BackgroundImage, toolStripButton.BackColor, toolStripButton.BackgroundImageLayout, rectangle, clipRect);
			}
			if (this.UseSystemColors)
			{
				if (toolStripButton.Selected)
				{
					this.RenderPressedButtonFill(graphics, rectangle);
				}
				else
				{
					this.RenderCheckedButtonFill(graphics, rectangle);
				}
				using (Pen pen = new Pen(this.ColorTable.ButtonSelectedBorder))
				{
					graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
					return;
				}
			}
			if (toolStripButton.Selected)
			{
				this.RenderPressedButtonFill(graphics, rectangle);
			}
			else
			{
				this.RenderCheckedButtonFill(graphics, rectangle);
			}
			using (Pen pen2 = new Pen(this.ColorTable.ButtonSelectedBorder))
			{
				graphics.DrawRectangle(pen2, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderToolStripBorder" />  event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042BF RID: 17087 RVA: 0x0011DAE4 File Offset: 0x0011BCE4
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderToolStripBorder(e);
				return;
			}
			ToolStrip toolStrip = e.ToolStrip;
			Graphics graphics = e.Graphics;
			if (toolStrip is ToolStripDropDown)
			{
				this.RenderToolStripDropDownBorder(e);
				return;
			}
			if (!(toolStrip is MenuStrip))
			{
				if (toolStrip is StatusStrip)
				{
					this.RenderStatusStripBorder(e);
					return;
				}
				Rectangle rectangle = new Rectangle(Point.Empty, toolStrip.Size);
				using (Pen pen = new Pen(this.ColorTable.ToolStripBorder))
				{
					if (toolStrip.Orientation == Orientation.Horizontal)
					{
						graphics.DrawLine(pen, rectangle.Left, rectangle.Height - 1, rectangle.Right, rectangle.Height - 1);
						if (this.RoundedEdges)
						{
							graphics.DrawLine(pen, rectangle.Width - 2, rectangle.Height - 2, rectangle.Width - 1, rectangle.Height - 3);
						}
					}
					else
					{
						graphics.DrawLine(pen, rectangle.Width - 1, 0, rectangle.Width - 1, rectangle.Height - 1);
						if (this.RoundedEdges)
						{
							graphics.DrawLine(pen, rectangle.Width - 2, rectangle.Height - 2, rectangle.Width - 1, rectangle.Height - 3);
						}
					}
				}
				if (this.RoundedEdges)
				{
					if (toolStrip.OverflowButton.Visible)
					{
						this.RenderOverflowButtonEffectsOverBorder(e);
						return;
					}
					Rectangle empty = Rectangle.Empty;
					if (toolStrip.Orientation == Orientation.Horizontal)
					{
						empty = new Rectangle(rectangle.Width - 1, 3, 1, rectangle.Height - 3);
					}
					else
					{
						empty = new Rectangle(3, rectangle.Height - 1, rectangle.Width - 3, rectangle.Height - 1);
					}
					this.ScaleObjectSizesIfNeeded(toolStrip.DeviceDpi);
					this.FillWithDoubleGradient(this.ColorTable.OverflowButtonGradientBegin, this.ColorTable.OverflowButtonGradientMiddle, this.ColorTable.OverflowButtonGradientEnd, e.Graphics, empty, this.iconWellGradientWidth, this.iconWellGradientWidth, LinearGradientMode.Vertical, false);
					this.RenderToolStripCurve(e);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderGrip" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripGripRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042C0 RID: 17088 RVA: 0x0011DCF8 File Offset: 0x0011BEF8
		protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderGrip(e);
				return;
			}
			this.ScaleObjectSizesIfNeeded(e.ToolStrip.DeviceDpi);
			Graphics graphics = e.Graphics;
			Rectangle gripBounds = e.GripBounds;
			ToolStrip toolStrip = e.ToolStrip;
			bool flag = e.ToolStrip.RightToLeft == RightToLeft.Yes;
			int num = (toolStrip.Orientation == Orientation.Horizontal) ? gripBounds.Height : gripBounds.Width;
			int num2 = (toolStrip.Orientation == Orientation.Horizontal) ? gripBounds.Width : gripBounds.Height;
			int num3 = (num - this.gripPadding * 2) / 4;
			if (num3 > 0)
			{
				int num4 = (toolStrip is MenuStrip) ? 2 : 0;
				Rectangle[] array = new Rectangle[num3];
				int num5 = this.gripPadding + 1 + num4;
				int num6 = num2 / 2;
				for (int i = 0; i < num3; i++)
				{
					array[i] = ((toolStrip.Orientation == Orientation.Horizontal) ? new Rectangle(num6, num5, 2, 2) : new Rectangle(num5, num6, 2, 2));
					num5 += 4;
				}
				int num7 = flag ? 1 : -1;
				if (flag)
				{
					for (int j = 0; j < num3; j++)
					{
						array[j].Offset(-num7, 0);
					}
				}
				using (Brush brush = new SolidBrush(this.ColorTable.GripLight))
				{
					graphics.FillRectangles(brush, array);
				}
				for (int k = 0; k < num3; k++)
				{
					array[k].Offset(num7, -1);
				}
				using (Brush brush2 = new SolidBrush(this.ColorTable.GripDark))
				{
					graphics.FillRectangles(brush2, array);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderMenuItemBackground" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042C1 RID: 17089 RVA: 0x0011DEC0 File Offset: 0x0011C0C0
		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderMenuItemBackground(e);
				return;
			}
			ToolStripItem item = e.Item;
			Graphics graphics = e.Graphics;
			Rectangle rectangle = new Rectangle(Point.Empty, item.Size);
			if (rectangle.Width == 0 || rectangle.Height == 0)
			{
				return;
			}
			if (item is MdiControlStrip.SystemMenuItem)
			{
				return;
			}
			if (item.IsOnDropDown)
			{
				this.ScaleObjectSizesIfNeeded(item.DeviceDpi);
				rectangle = LayoutUtils.DeflateRect(rectangle, this.scaledDropDownMenuItemPaintPadding);
				if (item.Selected)
				{
					Color color = this.ColorTable.MenuItemBorder;
					if (item.Enabled)
					{
						if (this.UseSystemColors)
						{
							color = SystemColors.Highlight;
							this.RenderSelectedButtonFill(graphics, rectangle);
						}
						else
						{
							using (Brush brush = new SolidBrush(this.ColorTable.MenuItemSelected))
							{
								graphics.FillRectangle(brush, rectangle);
							}
						}
					}
					using (Pen pen = new Pen(color))
					{
						graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
						return;
					}
				}
				Rectangle rectangle2 = rectangle;
				if (item.BackgroundImage != null)
				{
					ControlPaint.DrawBackgroundImage(graphics, item.BackgroundImage, item.BackColor, item.BackgroundImageLayout, rectangle, rectangle2);
					return;
				}
				if (item.Owner == null || !(item.BackColor != item.Owner.BackColor))
				{
					return;
				}
				using (Brush brush2 = new SolidBrush(item.BackColor))
				{
					graphics.FillRectangle(brush2, rectangle2);
					return;
				}
			}
			if (item.Pressed)
			{
				this.RenderPressedGradient(graphics, rectangle);
				return;
			}
			if (item.Selected)
			{
				Color color2 = this.ColorTable.MenuItemBorder;
				if (item.Enabled)
				{
					if (this.UseSystemColors)
					{
						color2 = SystemColors.Highlight;
						this.RenderSelectedButtonFill(graphics, rectangle);
					}
					else
					{
						using (Brush brush3 = new LinearGradientBrush(rectangle, this.ColorTable.MenuItemSelectedGradientBegin, this.ColorTable.MenuItemSelectedGradientEnd, LinearGradientMode.Vertical))
						{
							graphics.FillRectangle(brush3, rectangle);
						}
					}
				}
				using (Pen pen2 = new Pen(color2))
				{
					graphics.DrawRectangle(pen2, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
					return;
				}
			}
			Rectangle rectangle3 = rectangle;
			if (item.BackgroundImage != null)
			{
				ControlPaint.DrawBackgroundImage(graphics, item.BackgroundImage, item.BackColor, item.BackgroundImageLayout, rectangle, rectangle3);
				return;
			}
			if (item.Owner != null && item.BackColor != item.Owner.BackColor)
			{
				using (Brush brush4 = new SolidBrush(item.BackColor))
				{
					graphics.FillRectangle(brush4, rectangle3);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderArrow" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripArrowRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042C2 RID: 17090 RVA: 0x0011E1CC File Offset: 0x0011C3CC
		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderArrow(e);
				return;
			}
			ToolStripItem item = e.Item;
			if (item is ToolStripDropDownItem)
			{
				e.DefaultArrowColor = (item.Enabled ? SystemColors.ControlText : SystemColors.ControlDark);
			}
			base.OnRenderArrow(e);
		}

		/// <summary>Draws the item background.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042C3 RID: 17091 RVA: 0x0011E21C File Offset: 0x0011C41C
		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderImageMargin(e);
				return;
			}
			this.ScaleObjectSizesIfNeeded(e.ToolStrip.DeviceDpi);
			Graphics graphics = e.Graphics;
			Rectangle affectedBounds = e.AffectedBounds;
			affectedBounds.Y += 2;
			affectedBounds.Height -= 4;
			RightToLeft rightToLeft = e.ToolStrip.RightToLeft;
			Color beginColor = (rightToLeft == RightToLeft.No) ? this.ColorTable.ImageMarginGradientBegin : this.ColorTable.ImageMarginGradientEnd;
			Color endColor = (rightToLeft == RightToLeft.No) ? this.ColorTable.ImageMarginGradientEnd : this.ColorTable.ImageMarginGradientBegin;
			this.FillWithDoubleGradient(beginColor, this.ColorTable.ImageMarginGradientMiddle, endColor, e.Graphics, affectedBounds, this.iconWellGradientWidth, this.iconWellGradientWidth, LinearGradientMode.Horizontal, e.ToolStrip.RightToLeft == RightToLeft.Yes);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderItemText" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemTextRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042C4 RID: 17092 RVA: 0x0011E2F4 File Offset: 0x0011C4F4
		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderItemText(e);
				return;
			}
			if (e.Item is ToolStripMenuItem && (e.Item.Selected || e.Item.Pressed))
			{
				e.DefaultTextColor = e.Item.ForeColor;
			}
			base.OnRenderItemText(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderItemCheck" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemImageRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042C5 RID: 17093 RVA: 0x0011E350 File Offset: 0x0011C550
		protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderItemCheck(e);
				return;
			}
			this.RenderCheckBackground(e);
			base.OnRenderItemCheck(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderItemImage" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemImageRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042C6 RID: 17094 RVA: 0x0011E370 File Offset: 0x0011C570
		protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderItemImage(e);
				return;
			}
			Rectangle imageRectangle = e.ImageRectangle;
			Image image = e.Image;
			if (e.Item is ToolStripMenuItem)
			{
				ToolStripMenuItem toolStripMenuItem = e.Item as ToolStripMenuItem;
				if (toolStripMenuItem.CheckState != CheckState.Unchecked)
				{
					ToolStripDropDownMenu toolStripDropDownMenu = toolStripMenuItem.ParentInternal as ToolStripDropDownMenu;
					if (toolStripDropDownMenu != null && !toolStripDropDownMenu.ShowCheckMargin && toolStripDropDownMenu.ShowImageMargin)
					{
						this.RenderCheckBackground(e);
					}
				}
			}
			if (imageRectangle != Rectangle.Empty && image != null)
			{
				if (!e.Item.Enabled)
				{
					base.OnRenderItemImage(e);
					return;
				}
				if (e.Item.ImageScaling == ToolStripItemImageScaling.None)
				{
					e.Graphics.DrawImage(image, imageRectangle, new Rectangle(Point.Empty, imageRectangle.Size), GraphicsUnit.Pixel);
					return;
				}
				e.Graphics.DrawImage(image, imageRectangle);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderToolStripPanelBackground" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripPanelRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042C7 RID: 17095 RVA: 0x0011E444 File Offset: 0x0011C644
		protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderToolStripPanelBackground(e);
				return;
			}
			ToolStripPanel toolStripPanel = e.ToolStripPanel;
			if (!base.ShouldPaintBackground(toolStripPanel))
			{
				return;
			}
			e.Handled = true;
			this.RenderBackgroundGradient(e.Graphics, toolStripPanel, this.ColorTable.ToolStripPanelGradientBegin, this.ColorTable.ToolStripPanelGradientEnd);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderToolStripContentPanelBackground" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripContentPanelRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x060042C8 RID: 17096 RVA: 0x0011E49C File Offset: 0x0011C69C
		protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderToolStripContentPanelBackground(e);
				return;
			}
			ToolStripContentPanel toolStripContentPanel = e.ToolStripContentPanel;
			if (!base.ShouldPaintBackground(toolStripContentPanel))
			{
				return;
			}
			if (SystemInformation.InLockedTerminalSession())
			{
				return;
			}
			e.Handled = true;
			e.Graphics.Clear(this.ColorTable.ToolStripContentPanelGradientEnd);
		}

		// Token: 0x060042C9 RID: 17097 RVA: 0x0011E4F0 File Offset: 0x0011C6F0
		internal override Region GetTransparentRegion(ToolStrip toolStrip)
		{
			if (toolStrip is ToolStripDropDown || toolStrip is MenuStrip || toolStrip is StatusStrip)
			{
				return null;
			}
			if (!this.RoundedEdges)
			{
				return null;
			}
			Rectangle rectangle = new Rectangle(Point.Empty, toolStrip.Size);
			if (toolStrip.ParentInternal != null)
			{
				Point empty = Point.Empty;
				Point point = new Point(rectangle.Width - 1, 0);
				Point location = new Point(0, rectangle.Height - 1);
				Point point2 = new Point(rectangle.Width - 1, rectangle.Height - 1);
				Rectangle rect = new Rectangle(empty, ToolStripProfessionalRenderer.onePix);
				Rectangle rect2 = new Rectangle(location, new Size(2, 1));
				Rectangle rect3 = new Rectangle(location.X, location.Y - 1, 1, 2);
				Rectangle rect4 = new Rectangle(point2.X - 1, point2.Y, 2, 1);
				Rectangle rect5 = new Rectangle(point2.X, point2.Y - 1, 1, 2);
				Rectangle rect6;
				Rectangle rect7;
				if (toolStrip.OverflowButton.Visible)
				{
					rect6 = new Rectangle(point.X - 1, point.Y, 1, 1);
					rect7 = new Rectangle(point.X, point.Y, 1, 2);
				}
				else
				{
					rect6 = new Rectangle(point.X - 2, point.Y, 2, 1);
					rect7 = new Rectangle(point.X, point.Y, 1, 3);
				}
				Region region = new Region(rect);
				region.Union(rect);
				region.Union(rect2);
				region.Union(rect3);
				region.Union(rect4);
				region.Union(rect5);
				region.Union(rect6);
				region.Union(rect7);
				return region;
			}
			return null;
		}

		// Token: 0x060042CA RID: 17098 RVA: 0x0011E6A4 File Offset: 0x0011C8A4
		private void RenderOverflowButtonEffectsOverBorder(ToolStripRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			ToolStripItem overflowButton = toolStrip.OverflowButton;
			if (!overflowButton.Visible)
			{
				return;
			}
			Graphics graphics = e.Graphics;
			Color color;
			Color color2;
			if (overflowButton.Pressed)
			{
				color = this.ColorTable.ButtonPressedGradientBegin;
				color2 = color;
			}
			else if (overflowButton.Selected)
			{
				color = this.ColorTable.ButtonSelectedGradientMiddle;
				color2 = color;
			}
			else
			{
				color = this.ColorTable.ToolStripBorder;
				color2 = this.ColorTable.ToolStripGradientMiddle;
			}
			using (Brush brush = new SolidBrush(color))
			{
				graphics.FillRectangle(brush, toolStrip.Width - 1, toolStrip.Height - 2, 1, 1);
				graphics.FillRectangle(brush, toolStrip.Width - 2, toolStrip.Height - 1, 1, 1);
			}
			using (Brush brush2 = new SolidBrush(color2))
			{
				graphics.FillRectangle(brush2, toolStrip.Width - 2, 0, 1, 1);
				graphics.FillRectangle(brush2, toolStrip.Width - 1, 1, 1, 1);
			}
		}

		// Token: 0x060042CB RID: 17099 RVA: 0x0011E7C0 File Offset: 0x0011C9C0
		private void FillWithDoubleGradient(Color beginColor, Color middleColor, Color endColor, Graphics g, Rectangle bounds, int firstGradientWidth, int secondGradientWidth, LinearGradientMode mode, bool flipHorizontal)
		{
			if (bounds.Width == 0 || bounds.Height == 0)
			{
				return;
			}
			Rectangle rect = bounds;
			Rectangle rect2 = bounds;
			bool flag;
			if (mode == LinearGradientMode.Horizontal)
			{
				if (flipHorizontal)
				{
					Color color = endColor;
					endColor = beginColor;
					beginColor = color;
				}
				rect2.Width = firstGradientWidth;
				rect.Width = secondGradientWidth + 1;
				rect.X = bounds.Right - rect.Width;
				flag = (bounds.Width > firstGradientWidth + secondGradientWidth);
			}
			else
			{
				rect2.Height = firstGradientWidth;
				rect.Height = secondGradientWidth + 1;
				rect.Y = bounds.Bottom - rect.Height;
				flag = (bounds.Height > firstGradientWidth + secondGradientWidth);
			}
			if (flag)
			{
				using (Brush brush = new SolidBrush(middleColor))
				{
					g.FillRectangle(brush, bounds);
				}
				using (Brush brush2 = new LinearGradientBrush(rect2, beginColor, middleColor, mode))
				{
					g.FillRectangle(brush2, rect2);
				}
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect, middleColor, endColor, mode))
				{
					if (mode == LinearGradientMode.Horizontal)
					{
						rect.X++;
						rect.Width--;
					}
					else
					{
						rect.Y++;
						rect.Height--;
					}
					g.FillRectangle(linearGradientBrush, rect);
					return;
				}
			}
			using (Brush brush3 = new LinearGradientBrush(bounds, beginColor, endColor, mode))
			{
				g.FillRectangle(brush3, bounds);
			}
		}

		// Token: 0x060042CC RID: 17100 RVA: 0x0011E978 File Offset: 0x0011CB78
		private void RenderStatusStripBorder(ToolStripRenderEventArgs e)
		{
			e.Graphics.DrawLine(SystemPens.ButtonHighlight, 0, 0, e.ToolStrip.Width, 0);
		}

		// Token: 0x060042CD RID: 17101 RVA: 0x0011E998 File Offset: 0x0011CB98
		private void RenderStatusStripBackground(ToolStripRenderEventArgs e)
		{
			StatusStrip statusStrip = e.ToolStrip as StatusStrip;
			this.RenderBackgroundGradient(e.Graphics, statusStrip, this.ColorTable.StatusStripGradientBegin, this.ColorTable.StatusStripGradientEnd, statusStrip.Orientation);
		}

		// Token: 0x060042CE RID: 17102 RVA: 0x0011E9DC File Offset: 0x0011CBDC
		private void RenderCheckBackground(ToolStripItemImageRenderEventArgs e)
		{
			Rectangle rectangle = DpiHelper.IsScalingRequired ? new Rectangle(e.ImageRectangle.Left - 2, (e.Item.Height - e.ImageRectangle.Height) / 2 - 1, e.ImageRectangle.Width + 4, e.ImageRectangle.Height + 2) : new Rectangle(e.ImageRectangle.Left - 2, 1, e.ImageRectangle.Width + 4, e.Item.Height - 2);
			Graphics graphics = e.Graphics;
			if (!this.UseSystemColors)
			{
				Color color = e.Item.Selected ? this.ColorTable.CheckSelectedBackground : this.ColorTable.CheckBackground;
				color = (e.Item.Pressed ? this.ColorTable.CheckPressedBackground : color);
				using (Brush brush = new SolidBrush(color))
				{
					graphics.FillRectangle(brush, rectangle);
				}
				using (Pen pen = new Pen(this.ColorTable.ButtonSelectedBorder))
				{
					graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
					return;
				}
			}
			if (e.Item.Pressed)
			{
				this.RenderPressedButtonFill(graphics, rectangle);
			}
			else
			{
				this.RenderSelectedButtonFill(graphics, rectangle);
			}
			graphics.DrawRectangle(SystemPens.Highlight, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x0011EB9C File Offset: 0x0011CD9C
		private void RenderPressedGradient(Graphics g, Rectangle bounds)
		{
			if (bounds.Width == 0 || bounds.Height == 0)
			{
				return;
			}
			using (Brush brush = new LinearGradientBrush(bounds, this.ColorTable.MenuItemPressedGradientBegin, this.ColorTable.MenuItemPressedGradientEnd, LinearGradientMode.Vertical))
			{
				g.FillRectangle(brush, bounds);
			}
			using (Pen pen = new Pen(this.ColorTable.MenuBorder))
			{
				g.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
			}
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x0011EC50 File Offset: 0x0011CE50
		private void RenderMenuStripBackground(ToolStripRenderEventArgs e)
		{
			this.RenderBackgroundGradient(e.Graphics, e.ToolStrip, this.ColorTable.MenuStripGradientBegin, this.ColorTable.MenuStripGradientEnd, e.ToolStrip.Orientation);
		}

		// Token: 0x060042D1 RID: 17105 RVA: 0x0011EC88 File Offset: 0x0011CE88
		private static void RenderLabelInternal(ToolStripItemRenderEventArgs e)
		{
			Graphics graphics = e.Graphics;
			ToolStripItem item = e.Item;
			Rectangle rectangle = new Rectangle(Point.Empty, item.Size);
			Rectangle clipRect = item.Selected ? item.ContentRectangle : rectangle;
			if (item.BackgroundImage != null)
			{
				ControlPaint.DrawBackgroundImage(graphics, item.BackgroundImage, item.BackColor, item.BackgroundImageLayout, rectangle, clipRect);
			}
		}

		// Token: 0x060042D2 RID: 17106 RVA: 0x0011ECE9 File Offset: 0x0011CEE9
		private void RenderBackgroundGradient(Graphics g, Control control, Color beginColor, Color endColor)
		{
			this.RenderBackgroundGradient(g, control, beginColor, endColor, Orientation.Horizontal);
		}

		// Token: 0x060042D3 RID: 17107 RVA: 0x0011ECF8 File Offset: 0x0011CEF8
		private void RenderBackgroundGradient(Graphics g, Control control, Color beginColor, Color endColor, Orientation orientation)
		{
			if (control.RightToLeft == RightToLeft.Yes)
			{
				Color color = beginColor;
				beginColor = endColor;
				endColor = color;
			}
			if (orientation == Orientation.Horizontal)
			{
				Control parentInternal = control.ParentInternal;
				if (parentInternal != null)
				{
					Rectangle rectangle = new Rectangle(Point.Empty, parentInternal.Size);
					if (LayoutUtils.IsZeroWidthOrHeight(rectangle))
					{
						return;
					}
					using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, beginColor, endColor, LinearGradientMode.Horizontal))
					{
						linearGradientBrush.TranslateTransform((float)(parentInternal.Width - control.Location.X), (float)(parentInternal.Height - control.Location.Y));
						g.FillRectangle(linearGradientBrush, new Rectangle(Point.Empty, control.Size));
						return;
					}
				}
				Rectangle rectangle2 = new Rectangle(Point.Empty, control.Size);
				if (LayoutUtils.IsZeroWidthOrHeight(rectangle2))
				{
					return;
				}
				using (LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(rectangle2, beginColor, endColor, LinearGradientMode.Horizontal))
				{
					g.FillRectangle(linearGradientBrush2, rectangle2);
					return;
				}
			}
			using (Brush brush = new SolidBrush(beginColor))
			{
				g.FillRectangle(brush, new Rectangle(Point.Empty, control.Size));
			}
		}

		// Token: 0x060042D4 RID: 17108 RVA: 0x0011EE40 File Offset: 0x0011D040
		private void RenderToolStripBackgroundInternal(ToolStripRenderEventArgs e)
		{
			this.ScaleObjectSizesIfNeeded(e.ToolStrip.DeviceDpi);
			ToolStrip toolStrip = e.ToolStrip;
			Graphics graphics = e.Graphics;
			Rectangle bounds = new Rectangle(Point.Empty, e.ToolStrip.Size);
			LinearGradientMode mode = (toolStrip.Orientation == Orientation.Horizontal) ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
			this.FillWithDoubleGradient(this.ColorTable.ToolStripGradientBegin, this.ColorTable.ToolStripGradientMiddle, this.ColorTable.ToolStripGradientEnd, e.Graphics, bounds, this.iconWellGradientWidth, this.iconWellGradientWidth, mode, false);
		}

		// Token: 0x060042D5 RID: 17109 RVA: 0x0011EECC File Offset: 0x0011D0CC
		private void RenderToolStripDropDownBackground(ToolStripRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			Rectangle rect = new Rectangle(Point.Empty, e.ToolStrip.Size);
			using (Brush brush = new SolidBrush(this.ColorTable.ToolStripDropDownBackground))
			{
				e.Graphics.FillRectangle(brush, rect);
			}
		}

		// Token: 0x060042D6 RID: 17110 RVA: 0x0011EF34 File Offset: 0x0011D134
		private void RenderToolStripDropDownBorder(ToolStripRenderEventArgs e)
		{
			ToolStripDropDown toolStripDropDown = e.ToolStrip as ToolStripDropDown;
			Graphics graphics = e.Graphics;
			if (toolStripDropDown != null)
			{
				Rectangle rectangle = new Rectangle(Point.Empty, toolStripDropDown.Size);
				using (Pen pen = new Pen(this.ColorTable.MenuBorder))
				{
					graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
				}
				if (!(toolStripDropDown is ToolStripOverflow))
				{
					using (Brush brush = new SolidBrush(this.ColorTable.ToolStripDropDownBackground))
					{
						graphics.FillRectangle(brush, e.ConnectedArea);
					}
				}
			}
		}

		// Token: 0x060042D7 RID: 17111 RVA: 0x0011F004 File Offset: 0x0011D204
		private void RenderOverflowBackground(ToolStripItemRenderEventArgs e, bool rightToLeft)
		{
			this.ScaleObjectSizesIfNeeded(e.Item.DeviceDpi);
			Graphics graphics = e.Graphics;
			ToolStripOverflowButton toolStripOverflowButton = e.Item as ToolStripOverflowButton;
			Rectangle rectangle = new Rectangle(Point.Empty, e.Item.Size);
			Rectangle withinBounds = rectangle;
			bool flag = this.RoundedEdges && !(toolStripOverflowButton.GetCurrentParent() is MenuStrip);
			bool flag2 = e.ToolStrip.Orientation == Orientation.Horizontal;
			if (flag2)
			{
				rectangle.X += rectangle.Width - this.overflowButtonWidth + 1;
				rectangle.Width = this.overflowButtonWidth;
				if (rightToLeft)
				{
					rectangle = LayoutUtils.RTLTranslate(rectangle, withinBounds);
				}
			}
			else
			{
				rectangle.Y = rectangle.Height - this.overflowButtonWidth + 1;
				rectangle.Height = this.overflowButtonWidth;
			}
			Color color;
			Color middleColor;
			Color endColor;
			Color color2;
			Color color3;
			if (toolStripOverflowButton.Pressed)
			{
				color = this.ColorTable.ButtonPressedGradientBegin;
				middleColor = this.ColorTable.ButtonPressedGradientMiddle;
				endColor = this.ColorTable.ButtonPressedGradientEnd;
				color2 = this.ColorTable.ButtonPressedGradientBegin;
				color3 = color2;
			}
			else if (toolStripOverflowButton.Selected)
			{
				color = this.ColorTable.ButtonSelectedGradientBegin;
				middleColor = this.ColorTable.ButtonSelectedGradientMiddle;
				endColor = this.ColorTable.ButtonSelectedGradientEnd;
				color2 = this.ColorTable.ButtonSelectedGradientMiddle;
				color3 = color2;
			}
			else
			{
				color = this.ColorTable.OverflowButtonGradientBegin;
				middleColor = this.ColorTable.OverflowButtonGradientMiddle;
				endColor = this.ColorTable.OverflowButtonGradientEnd;
				color2 = this.ColorTable.ToolStripBorder;
				color3 = (flag2 ? this.ColorTable.ToolStripGradientMiddle : this.ColorTable.ToolStripGradientEnd);
			}
			if (flag)
			{
				using (Pen pen = new Pen(color2))
				{
					Point pt = new Point(rectangle.Left - 1, rectangle.Height - 2);
					Point pt2 = new Point(rectangle.Left, rectangle.Height - 2);
					if (rightToLeft)
					{
						pt.X = rectangle.Right + 1;
						pt2.X = rectangle.Right;
					}
					graphics.DrawLine(pen, pt, pt2);
				}
			}
			LinearGradientMode mode = flag2 ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
			this.FillWithDoubleGradient(color, middleColor, endColor, graphics, rectangle, this.iconWellGradientWidth, this.iconWellGradientWidth, mode, false);
			if (flag)
			{
				using (Brush brush = new SolidBrush(color3))
				{
					if (flag2)
					{
						Point point = new Point(rectangle.X - 2, 0);
						Point point2 = new Point(rectangle.X - 1, 1);
						if (rightToLeft)
						{
							point.X = rectangle.Right + 1;
							point2.X = rectangle.Right;
						}
						graphics.FillRectangle(brush, point.X, point.Y, 1, 1);
						graphics.FillRectangle(brush, point2.X, point2.Y, 1, 1);
					}
					else
					{
						graphics.FillRectangle(brush, rectangle.Width - 3, rectangle.Top - 1, 1, 1);
						graphics.FillRectangle(brush, rectangle.Width - 2, rectangle.Top - 2, 1, 1);
					}
				}
				using (Brush brush2 = new SolidBrush(color))
				{
					if (flag2)
					{
						Rectangle rect = new Rectangle(rectangle.X - 1, 0, 1, 1);
						if (rightToLeft)
						{
							rect.X = rectangle.Right;
						}
						graphics.FillRectangle(brush2, rect);
					}
					else
					{
						graphics.FillRectangle(brush2, rectangle.X, rectangle.Top - 1, 1, 1);
					}
				}
			}
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x0011F3B0 File Offset: 0x0011D5B0
		private void RenderToolStripCurve(ToolStripRenderEventArgs e)
		{
			Rectangle rectangle = new Rectangle(Point.Empty, e.ToolStrip.Size);
			ToolStrip toolStrip = e.ToolStrip;
			Rectangle displayRectangle = toolStrip.DisplayRectangle;
			Graphics graphics = e.Graphics;
			Point empty = Point.Empty;
			Point location = new Point(rectangle.Width - 1, 0);
			Point point = new Point(0, rectangle.Height - 1);
			using (Brush brush = new SolidBrush(this.ColorTable.ToolStripGradientMiddle))
			{
				Rectangle rectangle2 = new Rectangle(empty, ToolStripProfessionalRenderer.onePix);
				rectangle2.X++;
				Rectangle rectangle3 = new Rectangle(empty, ToolStripProfessionalRenderer.onePix);
				rectangle3.Y++;
				Rectangle rectangle4 = new Rectangle(location, ToolStripProfessionalRenderer.onePix);
				rectangle4.X -= 2;
				Rectangle rectangle5 = rectangle4;
				rectangle5.Y++;
				rectangle5.X++;
				Rectangle[] array = new Rectangle[]
				{
					rectangle2,
					rectangle3,
					rectangle4,
					rectangle5
				};
				for (int i = 0; i < array.Length; i++)
				{
					if (displayRectangle.IntersectsWith(array[i]))
					{
						array[i] = Rectangle.Empty;
					}
				}
				graphics.FillRectangles(brush, array);
			}
			using (Brush brush2 = new SolidBrush(this.ColorTable.ToolStripGradientEnd))
			{
				Point point2 = point;
				point2.Offset(1, -1);
				if (!displayRectangle.Contains(point2))
				{
					graphics.FillRectangle(brush2, new Rectangle(point2, ToolStripProfessionalRenderer.onePix));
				}
				Rectangle rect = new Rectangle(point.X, point.Y - 2, 1, 1);
				if (!displayRectangle.IntersectsWith(rect))
				{
					graphics.FillRectangle(brush2, rect);
				}
			}
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x0011F5AC File Offset: 0x0011D7AC
		private void RenderSelectedButtonFill(Graphics g, Rectangle bounds)
		{
			if (bounds.Width == 0 || bounds.Height == 0)
			{
				return;
			}
			if (!this.UseSystemColors)
			{
				using (Brush brush = new LinearGradientBrush(bounds, this.ColorTable.ButtonSelectedGradientBegin, this.ColorTable.ButtonSelectedGradientEnd, LinearGradientMode.Vertical))
				{
					g.FillRectangle(brush, bounds);
					return;
				}
			}
			Color buttonSelectedHighlight = this.ColorTable.ButtonSelectedHighlight;
			using (Brush brush2 = new SolidBrush(buttonSelectedHighlight))
			{
				g.FillRectangle(brush2, bounds);
			}
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x0011F64C File Offset: 0x0011D84C
		private void RenderCheckedButtonFill(Graphics g, Rectangle bounds)
		{
			if (bounds.Width == 0 || bounds.Height == 0)
			{
				return;
			}
			if (!this.UseSystemColors)
			{
				using (Brush brush = new LinearGradientBrush(bounds, this.ColorTable.ButtonCheckedGradientBegin, this.ColorTable.ButtonCheckedGradientEnd, LinearGradientMode.Vertical))
				{
					g.FillRectangle(brush, bounds);
					return;
				}
			}
			Color buttonCheckedHighlight = this.ColorTable.ButtonCheckedHighlight;
			using (Brush brush2 = new SolidBrush(buttonCheckedHighlight))
			{
				g.FillRectangle(brush2, bounds);
			}
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x0011F6EC File Offset: 0x0011D8EC
		private void RenderSeparatorInternal(Graphics g, ToolStripItem item, Rectangle bounds, bool vertical)
		{
			Color separatorDark = this.ColorTable.SeparatorDark;
			Color separatorLight = this.ColorTable.SeparatorLight;
			Pen pen = new Pen(separatorDark);
			Pen pen2 = new Pen(separatorLight);
			bool flag = true;
			bool flag2 = true;
			bool flag3 = item is ToolStripSeparator;
			bool flag4 = false;
			if (flag3)
			{
				if (vertical)
				{
					if (!item.IsOnDropDown)
					{
						bounds.Y += 3;
						bounds.Height = Math.Max(0, bounds.Height - 6);
					}
				}
				else
				{
					ToolStripDropDownMenu toolStripDropDownMenu = item.GetCurrentParent() as ToolStripDropDownMenu;
					if (toolStripDropDownMenu != null)
					{
						if (toolStripDropDownMenu.RightToLeft == RightToLeft.No)
						{
							bounds.X += toolStripDropDownMenu.Padding.Left - 2;
							bounds.Width = toolStripDropDownMenu.Width - bounds.X;
						}
						else
						{
							bounds.X += 2;
							bounds.Width = toolStripDropDownMenu.Width - bounds.X - toolStripDropDownMenu.Padding.Right;
						}
					}
					else
					{
						flag4 = true;
					}
				}
			}
			try
			{
				if (vertical)
				{
					if (bounds.Height >= 4)
					{
						bounds.Inflate(0, -2);
					}
					bool flag5 = item.RightToLeft == RightToLeft.Yes;
					Pen pen3 = flag5 ? pen2 : pen;
					Pen pen4 = flag5 ? pen : pen2;
					int num = bounds.Width / 2;
					g.DrawLine(pen3, num, bounds.Top, num, bounds.Bottom - 1);
					num++;
					g.DrawLine(pen4, num, bounds.Top + 1, num, bounds.Bottom);
				}
				else
				{
					if (flag4 && bounds.Width >= 4)
					{
						bounds.Inflate(-2, 0);
					}
					int num2 = bounds.Height / 2;
					g.DrawLine(pen, bounds.Left, num2, bounds.Right - 1, num2);
					if (!flag3 || flag4)
					{
						num2++;
						g.DrawLine(pen2, bounds.Left + 1, num2, bounds.Right - 1, num2);
					}
				}
			}
			finally
			{
				if (flag && pen != null)
				{
					pen.Dispose();
				}
				if (flag2 && pen2 != null)
				{
					pen2.Dispose();
				}
			}
		}

		// Token: 0x060042DC RID: 17116 RVA: 0x0011F924 File Offset: 0x0011DB24
		private void RenderPressedButtonFill(Graphics g, Rectangle bounds)
		{
			if (bounds.Width == 0 || bounds.Height == 0)
			{
				return;
			}
			if (!this.UseSystemColors)
			{
				using (Brush brush = new LinearGradientBrush(bounds, this.ColorTable.ButtonPressedGradientBegin, this.ColorTable.ButtonPressedGradientEnd, LinearGradientMode.Vertical))
				{
					g.FillRectangle(brush, bounds);
					return;
				}
			}
			Color buttonPressedHighlight = this.ColorTable.ButtonPressedHighlight;
			using (Brush brush2 = new SolidBrush(buttonPressedHighlight))
			{
				g.FillRectangle(brush2, bounds);
			}
		}

		// Token: 0x060042DD RID: 17117 RVA: 0x0011F9C4 File Offset: 0x0011DBC4
		private void RenderItemInternal(ToolStripItemRenderEventArgs e, bool useHotBorder)
		{
			Graphics graphics = e.Graphics;
			ToolStripItem item = e.Item;
			Rectangle rectangle = new Rectangle(Point.Empty, item.Size);
			bool flag = false;
			Rectangle clipRect = item.Selected ? item.ContentRectangle : rectangle;
			if (item.BackgroundImage != null)
			{
				ControlPaint.DrawBackgroundImage(graphics, item.BackgroundImage, item.BackColor, item.BackgroundImageLayout, rectangle, clipRect);
			}
			if (item.Pressed)
			{
				this.RenderPressedButtonFill(graphics, rectangle);
				flag = useHotBorder;
			}
			else if (item.Selected)
			{
				this.RenderSelectedButtonFill(graphics, rectangle);
				flag = useHotBorder;
			}
			else if (item.Owner != null && item.BackColor != item.Owner.BackColor)
			{
				using (Brush brush = new SolidBrush(item.BackColor))
				{
					graphics.FillRectangle(brush, rectangle);
				}
			}
			if (flag)
			{
				using (Pen pen = new Pen(this.ColorTable.ButtonSelectedBorder))
				{
					graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
				}
			}
		}

		// Token: 0x060042DE RID: 17118 RVA: 0x0011FAFC File Offset: 0x0011DCFC
		private void ScaleObjectSizesIfNeeded(int currentDeviceDpi)
		{
			if (DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements && this.previousDeviceDpi != currentDeviceDpi)
			{
				ToolStripRenderer.ScaleArrowOffsetsIfNeeded(currentDeviceDpi);
				this.overflowButtonWidth = DpiHelper.LogicalToDeviceUnits(12, currentDeviceDpi);
				this.overflowArrowWidth = DpiHelper.LogicalToDeviceUnits(9, currentDeviceDpi);
				this.overflowArrowHeight = DpiHelper.LogicalToDeviceUnits(5, currentDeviceDpi);
				this.overflowArrowOffsetY = DpiHelper.LogicalToDeviceUnits(8, currentDeviceDpi);
				this.gripPadding = DpiHelper.LogicalToDeviceUnits(4, currentDeviceDpi);
				this.iconWellGradientWidth = DpiHelper.LogicalToDeviceUnits(12, currentDeviceDpi);
				int num = DpiHelper.LogicalToDeviceUnits(1, currentDeviceDpi);
				this.scaledDropDownMenuItemPaintPadding = new Padding(num + 1, 0, num, 0);
				this.previousDeviceDpi = currentDeviceDpi;
				this.isScalingInitialized = true;
				return;
			}
			if (this.isScalingInitialized)
			{
				return;
			}
			if (DpiHelper.IsScalingRequired)
			{
				ToolStripRenderer.ScaleArrowOffsetsIfNeeded();
				this.overflowButtonWidth = DpiHelper.LogicalToDeviceUnitsX(12);
				this.overflowArrowWidth = DpiHelper.LogicalToDeviceUnitsX(9);
				this.overflowArrowHeight = DpiHelper.LogicalToDeviceUnitsY(5);
				this.overflowArrowOffsetY = DpiHelper.LogicalToDeviceUnitsY(8);
				if (DpiHelper.EnableToolStripHighDpiImprovements)
				{
					this.gripPadding = DpiHelper.LogicalToDeviceUnitsY(4);
					this.iconWellGradientWidth = DpiHelper.LogicalToDeviceUnitsX(12);
					int num2 = DpiHelper.LogicalToDeviceUnitsX(1);
					this.scaledDropDownMenuItemPaintPadding = new Padding(num2 + 1, 0, num2, 0);
				}
			}
			this.isScalingInitialized = true;
		}

		// Token: 0x060042DF RID: 17119 RVA: 0x0011FC24 File Offset: 0x0011DE24
		private Point RenderArrowInternal(Graphics g, Rectangle dropDownRect, ArrowDirection direction, Brush brush)
		{
			Point result = new Point(dropDownRect.Left + dropDownRect.Width / 2, dropDownRect.Top + dropDownRect.Height / 2);
			result.X += dropDownRect.Width % 2;
			Point[] points;
			if (direction <= ArrowDirection.Up)
			{
				if (direction == ArrowDirection.Left)
				{
					points = new Point[]
					{
						new Point(result.X + ToolStripRenderer.Offset2X, result.Y - ToolStripRenderer.Offset2Y - 1),
						new Point(result.X + ToolStripRenderer.Offset2X, result.Y + ToolStripRenderer.Offset2Y + 1),
						new Point(result.X - 1, result.Y)
					};
					goto IL_236;
				}
				if (direction == ArrowDirection.Up)
				{
					points = new Point[]
					{
						new Point(result.X - ToolStripRenderer.Offset2X, result.Y + 1),
						new Point(result.X + ToolStripRenderer.Offset2X + 1, result.Y + 1),
						new Point(result.X, result.Y - ToolStripRenderer.Offset2Y)
					};
					goto IL_236;
				}
			}
			else
			{
				if (direction == ArrowDirection.Right)
				{
					points = new Point[]
					{
						new Point(result.X - ToolStripRenderer.Offset2X, result.Y - ToolStripRenderer.Offset2Y - 1),
						new Point(result.X - ToolStripRenderer.Offset2X, result.Y + ToolStripRenderer.Offset2Y + 1),
						new Point(result.X + 1, result.Y)
					};
					goto IL_236;
				}
				if (direction != ArrowDirection.Down)
				{
				}
			}
			points = new Point[]
			{
				new Point(result.X - ToolStripRenderer.Offset2X, result.Y - 1),
				new Point(result.X + ToolStripRenderer.Offset2X + 1, result.Y - 1),
				new Point(result.X, result.Y + ToolStripRenderer.Offset2Y)
			};
			IL_236:
			g.FillPolygon(brush, points);
			return result;
		}

		// Token: 0x0400255E RID: 9566
		private const int GRIP_PADDING = 4;

		// Token: 0x0400255F RID: 9567
		private int gripPadding = 4;

		// Token: 0x04002560 RID: 9568
		private const int ICON_WELL_GRADIENT_WIDTH = 12;

		// Token: 0x04002561 RID: 9569
		private int iconWellGradientWidth = 12;

		// Token: 0x04002562 RID: 9570
		private static readonly Size onePix = new Size(1, 1);

		// Token: 0x04002563 RID: 9571
		private bool isScalingInitialized;

		// Token: 0x04002564 RID: 9572
		private const int OVERFLOW_BUTTON_WIDTH = 12;

		// Token: 0x04002565 RID: 9573
		private const int OVERFLOW_ARROW_WIDTH = 9;

		// Token: 0x04002566 RID: 9574
		private const int OVERFLOW_ARROW_HEIGHT = 5;

		// Token: 0x04002567 RID: 9575
		private const int OVERFLOW_ARROW_OFFSETY = 8;

		// Token: 0x04002568 RID: 9576
		private int overflowButtonWidth = 12;

		// Token: 0x04002569 RID: 9577
		private int overflowArrowWidth = 9;

		// Token: 0x0400256A RID: 9578
		private int overflowArrowHeight = 5;

		// Token: 0x0400256B RID: 9579
		private int overflowArrowOffsetY = 8;

		// Token: 0x0400256C RID: 9580
		private const int DROP_DOWN_MENU_ITEM_PAINT_PADDING_SIZE = 1;

		// Token: 0x0400256D RID: 9581
		private Padding scaledDropDownMenuItemPaintPadding = new Padding(2, 0, 1, 0);

		// Token: 0x0400256E RID: 9582
		private ProfessionalColorTable professionalColorTable;

		// Token: 0x0400256F RID: 9583
		private bool roundedEdges = true;

		// Token: 0x04002570 RID: 9584
		private ToolStripRenderer toolStripHighContrastRenderer;

		// Token: 0x04002571 RID: 9585
		private ToolStripRenderer toolStripLowResolutionRenderer;
	}
}
