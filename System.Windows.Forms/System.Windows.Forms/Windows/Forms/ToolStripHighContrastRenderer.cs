using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace System.Windows.Forms
{
	// Token: 0x020003B4 RID: 948
	internal class ToolStripHighContrastRenderer : ToolStripSystemRenderer
	{
		// Token: 0x06003E81 RID: 16001 RVA: 0x00110E2F File Offset: 0x0010F02F
		public ToolStripHighContrastRenderer(bool systemRenderMode)
		{
			this.options[ToolStripHighContrastRenderer.optionsDottedBorder | ToolStripHighContrastRenderer.optionsDottedGrip | ToolStripHighContrastRenderer.optionsFillWhenSelected] = !systemRenderMode;
		}

		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x06003E82 RID: 16002 RVA: 0x00110E57 File Offset: 0x0010F057
		public bool DottedBorder
		{
			get
			{
				return this.options[ToolStripHighContrastRenderer.optionsDottedBorder];
			}
		}

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x06003E83 RID: 16003 RVA: 0x00110E69 File Offset: 0x0010F069
		public bool DottedGrip
		{
			get
			{
				return this.options[ToolStripHighContrastRenderer.optionsDottedGrip];
			}
		}

		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x06003E84 RID: 16004 RVA: 0x00110E7B File Offset: 0x0010F07B
		public bool FillWhenSelected
		{
			get
			{
				return this.options[ToolStripHighContrastRenderer.optionsFillWhenSelected];
			}
		}

		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x06003E85 RID: 16005 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal override ToolStripRenderer RendererOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x00110E8D File Offset: 0x0010F08D
		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
		{
			base.OnRenderArrow(e);
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x00110E98 File Offset: 0x0010F098
		protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
		{
			if (this.DottedGrip)
			{
				Graphics graphics = e.Graphics;
				Rectangle gripBounds = e.GripBounds;
				ToolStrip toolStrip = e.ToolStrip;
				int num = (toolStrip.Orientation == Orientation.Horizontal) ? gripBounds.Height : gripBounds.Width;
				int num2 = (toolStrip.Orientation == Orientation.Horizontal) ? gripBounds.Width : gripBounds.Height;
				int num3 = (num - 8) / 4;
				if (num3 > 0)
				{
					Rectangle[] array = new Rectangle[num3];
					int num4 = 4;
					int num5 = num2 / 2;
					for (int i = 0; i < num3; i++)
					{
						array[i] = ((toolStrip.Orientation == Orientation.Horizontal) ? new Rectangle(num5, num4, 2, 2) : new Rectangle(num4, num5, 2, 2));
						num4 += 4;
					}
					graphics.FillRectangles(SystemBrushes.ControlLight, array);
					return;
				}
			}
			else
			{
				base.OnRenderGrip(e);
			}
		}

		// Token: 0x06003E88 RID: 16008 RVA: 0x00110F6C File Offset: 0x0010F16C
		protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.FillWhenSelected)
			{
				this.RenderItemInternalFilled(e, false);
				return;
			}
			base.OnRenderDropDownButtonBackground(e);
			if (e.Item.Pressed)
			{
				e.Graphics.DrawRectangle(SystemPens.ButtonHighlight, new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1));
			}
		}

		// Token: 0x06003E89 RID: 16009 RVA: 0x00110FD0 File Offset: 0x0010F1D0
		protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
		{
			if (AccessibilityImprovements.Level1)
			{
				Color oldColor = Color.FromArgb(255, 4, 2, 4);
				ColorMap[] array = new ColorMap[]
				{
					new ColorMap()
				};
				array[0].OldColor = oldColor;
				array[0].NewColor = (((e.Item.Selected || e.Item.Pressed) && e.Item.Enabled) ? SystemColors.HighlightText : SystemColors.MenuText);
				ImageAttributes imageAttributes = e.ImageAttributes ?? new ImageAttributes();
				imageAttributes.SetRemapTable(array, ColorAdjustType.Bitmap);
				e.ImageAttributes = imageAttributes;
			}
			base.OnRenderItemCheck(e);
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x0000701A File Offset: 0x0000521A
		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
		{
		}

		// Token: 0x06003E8B RID: 16011 RVA: 0x0011106E File Offset: 0x0010F26E
		protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
		{
			base.OnRenderItemBackground(e);
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x00111078 File Offset: 0x0010F278
		protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStripSplitButton toolStripSplitButton = e.Item as ToolStripSplitButton;
			Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
			Graphics graphics = e.Graphics;
			if (toolStripSplitButton != null)
			{
				Rectangle dropDownButtonBounds = toolStripSplitButton.DropDownButtonBounds;
				if (toolStripSplitButton.Pressed)
				{
					graphics.DrawRectangle(SystemPens.ButtonHighlight, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
				}
				else if (toolStripSplitButton.Selected)
				{
					graphics.FillRectangle(SystemBrushes.Highlight, rect);
					graphics.DrawRectangle(SystemPens.ButtonHighlight, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
					graphics.DrawRectangle(SystemPens.ButtonHighlight, dropDownButtonBounds);
				}
				Color arrowColor = (AccessibilityImprovements.Level2 && toolStripSplitButton.Selected && !toolStripSplitButton.Pressed) ? SystemColors.HighlightText : SystemColors.ControlText;
				base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, toolStripSplitButton, dropDownButtonBounds, arrowColor, ArrowDirection.Down));
			}
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x00111173 File Offset: 0x0010F373
		protected override void OnRenderStatusStripSizingGrip(ToolStripRenderEventArgs e)
		{
			base.OnRenderStatusStripSizingGrip(e);
		}

		// Token: 0x06003E8E RID: 16014 RVA: 0x0011117C File Offset: 0x0010F37C
		protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.FillWhenSelected)
			{
				this.RenderItemInternalFilled(e);
				return;
			}
			base.OnRenderLabelBackground(e);
		}

		// Token: 0x06003E8F RID: 16015 RVA: 0x00111198 File Offset: 0x0010F398
		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			base.OnRenderMenuItemBackground(e);
			if (!e.Item.IsOnDropDown && e.Item.Pressed)
			{
				e.Graphics.DrawRectangle(SystemPens.ButtonHighlight, 0, 0, e.Item.Width - 1, e.Item.Height - 1);
			}
		}

		// Token: 0x06003E90 RID: 16016 RVA: 0x001111F4 File Offset: 0x0010F3F4
		protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.FillWhenSelected)
			{
				this.RenderItemInternalFilled(e, false);
				ToolStripItem item = e.Item;
				Graphics graphics = e.Graphics;
				Color arrowColor = item.Enabled ? SystemColors.ControlText : SystemColors.ControlDark;
				base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, item, new Rectangle(Point.Empty, item.Size), arrowColor, ArrowDirection.Down));
				return;
			}
			base.OnRenderOverflowButtonBackground(e);
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x0011125C File Offset: 0x0010F45C
		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			if (AccessibilityImprovements.Level2 && e.Item.Selected && (!e.Item.Pressed || e.Item is ToolStripButton))
			{
				e.DefaultTextColor = SystemColors.HighlightText;
			}
			else if (e.TextColor != SystemColors.HighlightText && e.TextColor != SystemColors.ControlText)
			{
				if (e.Item.Selected || e.Item.Pressed)
				{
					e.DefaultTextColor = SystemColors.HighlightText;
				}
				else
				{
					e.DefaultTextColor = SystemColors.ControlText;
				}
			}
			if (AccessibilityImprovements.Level1 && typeof(ToolStripButton).IsAssignableFrom(e.Item.GetType()) && ((ToolStripButton)e.Item).DisplayStyle != ToolStripItemDisplayStyle.Image && ((ToolStripButton)e.Item).Checked)
			{
				e.TextColor = SystemColors.HighlightText;
			}
			base.OnRenderItemText(e);
		}

		// Token: 0x06003E92 RID: 16018 RVA: 0x0000701A File Offset: 0x0000521A
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
		}

		// Token: 0x06003E93 RID: 16019 RVA: 0x00111354 File Offset: 0x0010F554
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			Rectangle rectangle = new Rectangle(Point.Empty, e.ToolStrip.Size);
			Graphics graphics = e.Graphics;
			if (e.ToolStrip is ToolStripDropDown)
			{
				graphics.DrawRectangle(SystemPens.ButtonHighlight, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
				if (!(e.ToolStrip is ToolStripOverflow))
				{
					graphics.FillRectangle(SystemBrushes.Control, e.ConnectedArea);
					return;
				}
			}
			else if (!(e.ToolStrip is MenuStrip))
			{
				if (e.ToolStrip is StatusStrip)
				{
					graphics.DrawRectangle(SystemPens.ButtonShadow, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
					return;
				}
				this.RenderToolStripBackgroundInternal(e);
			}
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x00111424 File Offset: 0x0010F624
		private void RenderToolStripBackgroundInternal(ToolStripRenderEventArgs e)
		{
			Rectangle rect = new Rectangle(Point.Empty, e.ToolStrip.Size);
			Graphics graphics = e.Graphics;
			if (this.DottedBorder)
			{
				using (Pen pen = new Pen(SystemColors.ButtonShadow))
				{
					pen.DashStyle = DashStyle.Dot;
					bool flag = (rect.Width & 1) == 1;
					bool flag2 = (rect.Height & 1) == 1;
					int num = 2;
					graphics.DrawLine(pen, rect.X + num, rect.Y, rect.Width - 1, rect.Y);
					graphics.DrawLine(pen, rect.X + num, rect.Height - 1, rect.Width - 1, rect.Height - 1);
					graphics.DrawLine(pen, rect.X, rect.Y + num, rect.X, rect.Height - 1);
					graphics.DrawLine(pen, rect.Width - 1, rect.Y + num, rect.Width - 1, rect.Height - 1);
					graphics.FillRectangle(SystemBrushes.ButtonShadow, new Rectangle(1, 1, 1, 1));
					if (flag)
					{
						graphics.FillRectangle(SystemBrushes.ButtonShadow, new Rectangle(rect.Width - 2, 1, 1, 1));
					}
					if (flag2)
					{
						graphics.FillRectangle(SystemBrushes.ButtonShadow, new Rectangle(1, rect.Height - 2, 1, 1));
					}
					if (flag2 && flag)
					{
						graphics.FillRectangle(SystemBrushes.ButtonShadow, new Rectangle(rect.Width - 2, rect.Height - 2, 1, 1));
					}
					return;
				}
			}
			rect.Width--;
			rect.Height--;
			graphics.DrawRectangle(SystemPens.ButtonShadow, rect);
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x00111604 File Offset: 0x0010F804
		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			Pen buttonShadow = SystemPens.ButtonShadow;
			Graphics graphics = e.Graphics;
			Rectangle rectangle = new Rectangle(Point.Empty, e.Item.Size);
			if (e.Vertical)
			{
				if (rectangle.Height >= 8)
				{
					rectangle.Inflate(0, -4);
				}
				int num = rectangle.Width / 2;
				graphics.DrawLine(buttonShadow, num, rectangle.Top, num, rectangle.Bottom - 1);
				return;
			}
			if (rectangle.Width >= 4)
			{
				rectangle.Inflate(-2, 0);
			}
			int num2 = rectangle.Height / 2;
			graphics.DrawLine(buttonShadow, rectangle.Left, num2, rectangle.Right - 1, num2);
		}

		// Token: 0x06003E96 RID: 16022 RVA: 0x001116B0 File Offset: 0x0010F8B0
		internal static bool IsHighContrastWhiteOnBlack()
		{
			return SystemColors.Control.ToArgb() == Color.Black.ToArgb();
		}

		// Token: 0x06003E97 RID: 16023 RVA: 0x001116DC File Offset: 0x0010F8DC
		protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
		{
			Image image = e.Image;
			if (image != null)
			{
				if (Image.GetPixelFormatSize(image.PixelFormat) > 16)
				{
					base.OnRenderItemImage(e);
					return;
				}
				Graphics graphics = e.Graphics;
				ToolStripItem item = e.Item;
				Rectangle imageRectangle = e.ImageRectangle;
				using (ImageAttributes imageAttributes = new ImageAttributes())
				{
					if (ToolStripHighContrastRenderer.IsHighContrastWhiteOnBlack() && (!this.FillWhenSelected || (!e.Item.Pressed && !e.Item.Selected)))
					{
						ColorMap colorMap = new ColorMap();
						ColorMap colorMap2 = new ColorMap();
						ColorMap colorMap3 = new ColorMap();
						colorMap.OldColor = Color.Black;
						colorMap.NewColor = Color.White;
						colorMap2.OldColor = Color.White;
						colorMap2.NewColor = Color.Black;
						colorMap3.OldColor = Color.FromArgb(0, 0, 128);
						colorMap3.NewColor = Color.White;
						imageAttributes.SetRemapTable(new ColorMap[]
						{
							colorMap,
							colorMap2,
							colorMap3
						}, ColorAdjustType.Bitmap);
					}
					if (item.ImageScaling == ToolStripItemImageScaling.None)
					{
						graphics.DrawImage(image, imageRectangle, 0, 0, imageRectangle.Width, imageRectangle.Height, GraphicsUnit.Pixel, imageAttributes);
					}
					else
					{
						graphics.DrawImage(image, imageRectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
					}
				}
			}
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x00111838 File Offset: 0x0010FA38
		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if (!this.FillWhenSelected)
			{
				base.OnRenderButtonBackground(e);
				return;
			}
			ToolStripButton toolStripButton = e.Item as ToolStripButton;
			if (toolStripButton == null || !toolStripButton.Checked)
			{
				this.RenderItemInternalFilled(e);
				return;
			}
			Graphics graphics = e.Graphics;
			Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
			if (toolStripButton.CheckState == CheckState.Checked)
			{
				graphics.FillRectangle(SystemBrushes.Highlight, rect);
			}
			if (toolStripButton.Selected && AccessibilityImprovements.Level1)
			{
				graphics.DrawRectangle(SystemPens.Highlight, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
				return;
			}
			graphics.DrawRectangle(SystemPens.ControlLight, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x00111916 File Offset: 0x0010FB16
		private void RenderItemInternalFilled(ToolStripItemRenderEventArgs e)
		{
			this.RenderItemInternalFilled(e, true);
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x00111920 File Offset: 0x0010FB20
		private void RenderItemInternalFilled(ToolStripItemRenderEventArgs e, bool pressFill)
		{
			Graphics graphics = e.Graphics;
			Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
			if (!e.Item.Pressed)
			{
				if (e.Item.Selected)
				{
					graphics.FillRectangle(SystemBrushes.Highlight, rect);
					graphics.DrawRectangle(SystemPens.ControlLight, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
				}
				return;
			}
			if (pressFill)
			{
				graphics.FillRectangle(SystemBrushes.Highlight, rect);
				return;
			}
			graphics.DrawRectangle(SystemPens.ControlLight, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
		}

		// Token: 0x0400240C RID: 9228
		private const int GRIP_PADDING = 4;

		// Token: 0x0400240D RID: 9229
		private BitVector32 options;

		// Token: 0x0400240E RID: 9230
		private static readonly int optionsDottedBorder = BitVector32.CreateMask();

		// Token: 0x0400240F RID: 9231
		private static readonly int optionsDottedGrip = BitVector32.CreateMask(ToolStripHighContrastRenderer.optionsDottedBorder);

		// Token: 0x04002410 RID: 9232
		private static readonly int optionsFillWhenSelected = BitVector32.CreateMask(ToolStripHighContrastRenderer.optionsDottedGrip);
	}
}
