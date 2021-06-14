using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Handles the painting functionality for <see cref="T:System.Windows.Forms.ToolStrip" /> objects, using system colors and a flat visual style.</summary>
	// Token: 0x020003F7 RID: 1015
	public class ToolStripSystemRenderer : ToolStripRenderer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripSystemRenderer" /> class. </summary>
		// Token: 0x06004469 RID: 17513 RVA: 0x0012463E File Offset: 0x0012283E
		public ToolStripSystemRenderer()
		{
		}

		// Token: 0x0600446A RID: 17514 RVA: 0x00124646 File Offset: 0x00122846
		internal ToolStripSystemRenderer(bool isDefault) : base(isDefault)
		{
		}

		// Token: 0x17001129 RID: 4393
		// (get) Token: 0x0600446B RID: 17515 RVA: 0x0012464F File Offset: 0x0012284F
		internal override ToolStripRenderer RendererOverride
		{
			get
			{
				if (DisplayInformation.HighContrast)
				{
					return this.HighContrastRenderer;
				}
				return null;
			}
		}

		// Token: 0x1700112A RID: 4394
		// (get) Token: 0x0600446C RID: 17516 RVA: 0x00124660 File Offset: 0x00122860
		internal ToolStripRenderer HighContrastRenderer
		{
			get
			{
				if (this.toolStripHighContrastRenderer == null)
				{
					this.toolStripHighContrastRenderer = new ToolStripHighContrastRenderer(true);
				}
				return this.toolStripHighContrastRenderer;
			}
		}

		// Token: 0x1700112B RID: 4395
		// (get) Token: 0x0600446D RID: 17517 RVA: 0x0012467C File Offset: 0x0012287C
		private static VisualStyleRenderer VisualStyleRenderer
		{
			get
			{
				if (Application.RenderWithVisualStyles)
				{
					if (ToolStripSystemRenderer.renderer == null && VisualStyleRenderer.IsElementDefined(VisualStyleElement.ToolBar.Button.Normal))
					{
						ToolStripSystemRenderer.renderer = new VisualStyleRenderer(VisualStyleElement.ToolBar.Button.Normal);
					}
				}
				else
				{
					ToolStripSystemRenderer.renderer = null;
				}
				return ToolStripSystemRenderer.renderer;
			}
		}

		// Token: 0x0600446E RID: 17518 RVA: 0x001246B4 File Offset: 0x001228B4
		private static void FillBackground(Graphics g, Rectangle bounds, Color backColor)
		{
			if (backColor.IsSystemColor)
			{
				g.FillRectangle(SystemBrushes.FromSystemColor(backColor), bounds);
				return;
			}
			using (Brush brush = new SolidBrush(backColor))
			{
				g.FillRectangle(brush, bounds);
			}
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x00124704 File Offset: 0x00122904
		private static bool GetPen(Color color, ref Pen pen)
		{
			if (color.IsSystemColor)
			{
				pen = SystemPens.FromSystemColor(color);
				return false;
			}
			pen = new Pen(color);
			return true;
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x00124722 File Offset: 0x00122922
		private static int GetItemState(ToolStripItem item)
		{
			return (int)ToolStripSystemRenderer.GetToolBarState(item);
		}

		// Token: 0x06004471 RID: 17521 RVA: 0x0012472A File Offset: 0x0012292A
		private static int GetSplitButtonDropDownItemState(ToolStripSplitButton item)
		{
			return (int)ToolStripSystemRenderer.GetSplitButtonToolBarState(item, true);
		}

		// Token: 0x06004472 RID: 17522 RVA: 0x00124733 File Offset: 0x00122933
		private static int GetSplitButtonItemState(ToolStripSplitButton item)
		{
			return (int)ToolStripSystemRenderer.GetSplitButtonToolBarState(item, false);
		}

		// Token: 0x06004473 RID: 17523 RVA: 0x0012473C File Offset: 0x0012293C
		private static ToolBarState GetSplitButtonToolBarState(ToolStripSplitButton button, bool dropDownButton)
		{
			ToolBarState result = ToolBarState.Normal;
			if (button != null)
			{
				if (!button.Enabled)
				{
					result = ToolBarState.Disabled;
				}
				else if (dropDownButton)
				{
					if (button.DropDownButtonPressed || button.ButtonPressed)
					{
						result = ToolBarState.Pressed;
					}
					else if (button.DropDownButtonSelected || button.ButtonSelected)
					{
						result = ToolBarState.Hot;
					}
				}
				else if (button.ButtonPressed)
				{
					result = ToolBarState.Pressed;
				}
				else if (button.ButtonSelected)
				{
					result = ToolBarState.Hot;
				}
			}
			return result;
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x0012479C File Offset: 0x0012299C
		private static ToolBarState GetToolBarState(ToolStripItem item)
		{
			ToolBarState result = ToolBarState.Normal;
			if (item != null)
			{
				if (!item.Enabled)
				{
					result = ToolBarState.Disabled;
				}
				if (item is ToolStripButton && ((ToolStripButton)item).Checked)
				{
					if (((ToolStripButton)item).Selected && AccessibilityImprovements.Level1)
					{
						result = ToolBarState.Hot;
					}
					else
					{
						result = ToolBarState.Checked;
					}
				}
				else if (item.Pressed)
				{
					result = ToolBarState.Pressed;
				}
				else if (item.Selected)
				{
					result = ToolBarState.Hot;
				}
			}
			return result;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderToolStripBackground" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x06004475 RID: 17525 RVA: 0x00124800 File Offset: 0x00122A00
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			Graphics graphics = e.Graphics;
			Rectangle affectedBounds = e.AffectedBounds;
			if (!base.ShouldPaintBackground(toolStrip))
			{
				return;
			}
			if (toolStrip is StatusStrip)
			{
				ToolStripSystemRenderer.RenderStatusStripBackground(e);
				return;
			}
			if (DisplayInformation.HighContrast)
			{
				ToolStripSystemRenderer.FillBackground(graphics, affectedBounds, SystemColors.ButtonFace);
				return;
			}
			if (DisplayInformation.LowResolution)
			{
				ToolStripSystemRenderer.FillBackground(graphics, affectedBounds, (toolStrip is ToolStripDropDown) ? SystemColors.ControlLight : e.BackColor);
				return;
			}
			if (toolStrip.IsDropDown)
			{
				ToolStripSystemRenderer.FillBackground(graphics, affectedBounds, (!ToolStripManager.VisualStylesEnabled) ? e.BackColor : SystemColors.Menu);
				return;
			}
			if (toolStrip is MenuStrip)
			{
				ToolStripSystemRenderer.FillBackground(graphics, affectedBounds, (!ToolStripManager.VisualStylesEnabled) ? e.BackColor : SystemColors.MenuBar);
				return;
			}
			if (ToolStripManager.VisualStylesEnabled && VisualStyleRenderer.IsElementDefined(VisualStyleElement.Rebar.Band.Normal))
			{
				VisualStyleRenderer visualStyleRenderer = ToolStripSystemRenderer.VisualStyleRenderer;
				visualStyleRenderer.SetParameters(VisualStyleElement.ToolBar.Bar.Normal);
				visualStyleRenderer.DrawBackground(graphics, affectedBounds);
				return;
			}
			ToolStripSystemRenderer.FillBackground(graphics, affectedBounds, (!ToolStripManager.VisualStylesEnabled) ? e.BackColor : SystemColors.MenuBar);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderToolStripBorder" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x06004476 RID: 17526 RVA: 0x00124904 File Offset: 0x00122B04
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = e.ToolStrip.ClientRectangle;
			if (toolStrip is StatusStrip)
			{
				this.RenderStatusStripBorder(e);
				return;
			}
			if (toolStrip is ToolStripDropDown)
			{
				ToolStripDropDown toolStripDropDown = toolStrip as ToolStripDropDown;
				if (toolStripDropDown.DropShadowEnabled && ToolStripManager.VisualStylesEnabled)
				{
					clientRectangle.Width--;
					clientRectangle.Height--;
					e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark), clientRectangle);
					return;
				}
				ControlPaint.DrawBorder3D(e.Graphics, clientRectangle, Border3DStyle.Raised);
				return;
			}
			else
			{
				if (ToolStripManager.VisualStylesEnabled)
				{
					e.Graphics.DrawLine(SystemPens.ButtonHighlight, 0, clientRectangle.Bottom - 1, clientRectangle.Width, clientRectangle.Bottom - 1);
					e.Graphics.DrawLine(SystemPens.InactiveBorder, 0, clientRectangle.Bottom - 2, clientRectangle.Width, clientRectangle.Bottom - 2);
					return;
				}
				e.Graphics.DrawLine(SystemPens.ButtonHighlight, 0, clientRectangle.Bottom - 1, clientRectangle.Width, clientRectangle.Bottom - 1);
				e.Graphics.DrawLine(SystemPens.ButtonShadow, 0, clientRectangle.Bottom - 2, clientRectangle.Width, clientRectangle.Bottom - 2);
				return;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderGrip" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripGripRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x06004477 RID: 17527 RVA: 0x00124A4C File Offset: 0x00122C4C
		protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
		{
			Graphics graphics = e.Graphics;
			Rectangle bounds = new Rectangle(Point.Empty, e.GripBounds.Size);
			bool flag = e.GripDisplayStyle == ToolStripGripDisplayStyle.Vertical;
			if (ToolStripManager.VisualStylesEnabled && VisualStyleRenderer.IsElementDefined(VisualStyleElement.Rebar.Gripper.Normal))
			{
				VisualStyleRenderer visualStyleRenderer = ToolStripSystemRenderer.VisualStyleRenderer;
				if (flag)
				{
					visualStyleRenderer.SetParameters(VisualStyleElement.Rebar.Gripper.Normal);
					bounds.Height = (bounds.Height - 2) / 4 * 4;
					bounds.Y = Math.Max(0, (e.GripBounds.Height - bounds.Height - 2) / 2);
				}
				else
				{
					visualStyleRenderer.SetParameters(VisualStyleElement.Rebar.GripperVertical.Normal);
				}
				visualStyleRenderer.DrawBackground(graphics, bounds);
				return;
			}
			Color backColor = e.ToolStrip.BackColor;
			ToolStripSystemRenderer.FillBackground(graphics, bounds, backColor);
			if (flag)
			{
				if (bounds.Height >= 4)
				{
					bounds.Inflate(0, -2);
				}
				bounds.Width = 3;
			}
			else
			{
				if (bounds.Width >= 4)
				{
					bounds.Inflate(-2, 0);
				}
				bounds.Height = 3;
			}
			this.RenderSmall3DBorderInternal(graphics, bounds, ToolBarState.Hot, e.ToolStrip.RightToLeft == RightToLeft.Yes);
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.ToolStripSystemRenderer.OnRenderItemBackground(System.Windows.Forms.ToolStripItemRenderEventArgs)" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x06004478 RID: 17528 RVA: 0x0000701A File Offset: 0x0000521A
		protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
		{
		}

		/// <summary>Draws the item background.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x06004479 RID: 17529 RVA: 0x0000701A File Offset: 0x0000521A
		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderButtonBackground" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x0600447A RID: 17530 RVA: 0x00124B6A File Offset: 0x00122D6A
		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			this.RenderItemInternal(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderDropDownButtonBackground" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x0600447B RID: 17531 RVA: 0x00124B6A File Offset: 0x00122D6A
		protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
		{
			this.RenderItemInternal(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderOverflowButtonBackground" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> that contains the event data. </param>
		// Token: 0x0600447C RID: 17532 RVA: 0x00124B74 File Offset: 0x00122D74
		protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStripItem item = e.Item;
			Graphics graphics = e.Graphics;
			if (ToolStripManager.VisualStylesEnabled && VisualStyleRenderer.IsElementDefined(VisualStyleElement.Rebar.Chevron.Normal))
			{
				VisualStyleElement normal = VisualStyleElement.Rebar.Chevron.Normal;
				VisualStyleRenderer visualStyleRenderer = ToolStripSystemRenderer.VisualStyleRenderer;
				visualStyleRenderer.SetParameters(normal.ClassName, normal.Part, ToolStripSystemRenderer.GetItemState(item));
				visualStyleRenderer.DrawBackground(graphics, new Rectangle(Point.Empty, item.Size));
				return;
			}
			this.RenderItemInternal(e);
			Color arrowColor = item.Enabled ? SystemColors.ControlText : SystemColors.ControlDark;
			base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, item, new Rectangle(Point.Empty, item.Size), arrowColor, ArrowDirection.Down));
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderLabelBackground" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x0600447D RID: 17533 RVA: 0x00124C1C File Offset: 0x00122E1C
		protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStripSystemRenderer.RenderLabelInternal(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderMenuItemBackground" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x0600447E RID: 17534 RVA: 0x00124C24 File Offset: 0x00122E24
		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStripMenuItem toolStripMenuItem = e.Item as ToolStripMenuItem;
			Graphics graphics = e.Graphics;
			if (toolStripMenuItem is MdiControlStrip.SystemMenuItem)
			{
				return;
			}
			if (toolStripMenuItem != null)
			{
				Rectangle bounds = new Rectangle(Point.Empty, toolStripMenuItem.Size);
				if (toolStripMenuItem.IsTopLevel && !ToolStripManager.VisualStylesEnabled)
				{
					if (toolStripMenuItem.BackgroundImage != null)
					{
						ControlPaint.DrawBackgroundImage(graphics, toolStripMenuItem.BackgroundImage, toolStripMenuItem.BackColor, toolStripMenuItem.BackgroundImageLayout, toolStripMenuItem.ContentRectangle, toolStripMenuItem.ContentRectangle);
					}
					else if (toolStripMenuItem.RawBackColor != Color.Empty)
					{
						ToolStripSystemRenderer.FillBackground(graphics, toolStripMenuItem.ContentRectangle, toolStripMenuItem.BackColor);
					}
					ToolBarState toolBarState = ToolStripSystemRenderer.GetToolBarState(toolStripMenuItem);
					this.RenderSmall3DBorderInternal(graphics, bounds, toolBarState, toolStripMenuItem.RightToLeft == RightToLeft.Yes);
					return;
				}
				Rectangle rectangle = new Rectangle(Point.Empty, toolStripMenuItem.Size);
				if (toolStripMenuItem.IsOnDropDown)
				{
					rectangle.X += 2;
					rectangle.Width -= 3;
				}
				if (toolStripMenuItem.Selected || toolStripMenuItem.Pressed)
				{
					if (!AccessibilityImprovements.Level1 || toolStripMenuItem.Enabled)
					{
						graphics.FillRectangle(SystemBrushes.Highlight, rectangle);
					}
					if (!AccessibilityImprovements.Level1)
					{
						return;
					}
					Color color = ToolStripManager.VisualStylesEnabled ? SystemColors.Highlight : ProfessionalColors.MenuItemBorder;
					using (Pen pen = new Pen(color))
					{
						graphics.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
						return;
					}
				}
				if (toolStripMenuItem.BackgroundImage != null)
				{
					ControlPaint.DrawBackgroundImage(graphics, toolStripMenuItem.BackgroundImage, toolStripMenuItem.BackColor, toolStripMenuItem.BackgroundImageLayout, toolStripMenuItem.ContentRectangle, rectangle);
					return;
				}
				if (!ToolStripManager.VisualStylesEnabled && toolStripMenuItem.RawBackColor != Color.Empty)
				{
					ToolStripSystemRenderer.FillBackground(graphics, rectangle, toolStripMenuItem.BackColor);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderSeparator" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripSeparatorRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x0600447F RID: 17535 RVA: 0x00124E04 File Offset: 0x00123004
		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			this.RenderSeparatorInternal(e.Graphics, e.Item, new Rectangle(Point.Empty, e.Item.Size), e.Vertical);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderToolStripStatusLabelBackground" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x06004480 RID: 17536 RVA: 0x00124E34 File Offset: 0x00123034
		protected override void OnRenderToolStripStatusLabelBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStripSystemRenderer.RenderLabelInternal(e);
			ToolStripStatusLabel toolStripStatusLabel = e.Item as ToolStripStatusLabel;
			ControlPaint.DrawBorder3D(e.Graphics, new Rectangle(0, 0, toolStripStatusLabel.Width - 1, toolStripStatusLabel.Height - 1), toolStripStatusLabel.BorderStyle, (Border3DSide)toolStripStatusLabel.BorderSides);
		}

		/// <summary>Raises the OnRenderSplitButtonBackground event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> that contains the event data.</param>
		// Token: 0x06004481 RID: 17537 RVA: 0x00124E84 File Offset: 0x00123084
		protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStripSplitButton toolStripSplitButton = e.Item as ToolStripSplitButton;
			Graphics graphics = e.Graphics;
			bool flag = toolStripSplitButton.RightToLeft == RightToLeft.Yes;
			Color arrowColor = toolStripSplitButton.Enabled ? SystemColors.ControlText : SystemColors.ControlDark;
			VisualStyleElement visualStyleElement = flag ? VisualStyleElement.ToolBar.SplitButton.Normal : VisualStyleElement.ToolBar.SplitButtonDropDown.Normal;
			VisualStyleElement visualStyleElement2 = flag ? VisualStyleElement.ToolBar.DropDownButton.Normal : VisualStyleElement.ToolBar.SplitButton.Normal;
			Rectangle rectangle = new Rectangle(Point.Empty, toolStripSplitButton.Size);
			if (ToolStripManager.VisualStylesEnabled && VisualStyleRenderer.IsElementDefined(visualStyleElement) && VisualStyleRenderer.IsElementDefined(visualStyleElement2))
			{
				VisualStyleRenderer visualStyleRenderer = ToolStripSystemRenderer.VisualStyleRenderer;
				visualStyleRenderer.SetParameters(visualStyleElement2.ClassName, visualStyleElement2.Part, ToolStripSystemRenderer.GetSplitButtonItemState(toolStripSplitButton));
				Rectangle buttonBounds = toolStripSplitButton.ButtonBounds;
				if (flag)
				{
					buttonBounds.Inflate(2, 0);
				}
				visualStyleRenderer.DrawBackground(graphics, buttonBounds);
				visualStyleRenderer.SetParameters(visualStyleElement.ClassName, visualStyleElement.Part, ToolStripSystemRenderer.GetSplitButtonDropDownItemState(toolStripSplitButton));
				visualStyleRenderer.DrawBackground(graphics, toolStripSplitButton.DropDownButtonBounds);
				Rectangle contentRectangle = toolStripSplitButton.ContentRectangle;
				if (toolStripSplitButton.BackgroundImage != null)
				{
					ControlPaint.DrawBackgroundImage(graphics, toolStripSplitButton.BackgroundImage, toolStripSplitButton.BackColor, toolStripSplitButton.BackgroundImageLayout, contentRectangle, contentRectangle);
				}
				this.RenderSeparatorInternal(graphics, toolStripSplitButton, toolStripSplitButton.SplitterBounds, true);
				if (flag || toolStripSplitButton.BackgroundImage != null)
				{
					base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, toolStripSplitButton, toolStripSplitButton.DropDownButtonBounds, arrowColor, ArrowDirection.Down));
					return;
				}
			}
			else
			{
				Rectangle buttonBounds2 = toolStripSplitButton.ButtonBounds;
				if (toolStripSplitButton.BackgroundImage != null)
				{
					Rectangle clipRect = toolStripSplitButton.Selected ? toolStripSplitButton.ContentRectangle : rectangle;
					if (toolStripSplitButton.BackgroundImage != null)
					{
						ControlPaint.DrawBackgroundImage(graphics, toolStripSplitButton.BackgroundImage, toolStripSplitButton.BackColor, toolStripSplitButton.BackgroundImageLayout, rectangle, clipRect);
					}
				}
				else
				{
					ToolStripSystemRenderer.FillBackground(graphics, buttonBounds2, toolStripSplitButton.BackColor);
				}
				ToolBarState splitButtonToolBarState = ToolStripSystemRenderer.GetSplitButtonToolBarState(toolStripSplitButton, false);
				this.RenderSmall3DBorderInternal(graphics, buttonBounds2, splitButtonToolBarState, flag);
				Rectangle dropDownButtonBounds = toolStripSplitButton.DropDownButtonBounds;
				if (toolStripSplitButton.BackgroundImage == null)
				{
					ToolStripSystemRenderer.FillBackground(graphics, dropDownButtonBounds, toolStripSplitButton.BackColor);
				}
				splitButtonToolBarState = ToolStripSystemRenderer.GetSplitButtonToolBarState(toolStripSplitButton, true);
				if (splitButtonToolBarState == ToolBarState.Pressed || splitButtonToolBarState == ToolBarState.Hot)
				{
					this.RenderSmall3DBorderInternal(graphics, dropDownButtonBounds, splitButtonToolBarState, flag);
				}
				base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, toolStripSplitButton, dropDownButtonBounds, arrowColor, ArrowDirection.Down));
			}
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x001250A0 File Offset: 0x001232A0
		private void RenderItemInternal(ToolStripItemRenderEventArgs e)
		{
			ToolStripItem item = e.Item;
			Graphics graphics = e.Graphics;
			ToolBarState toolBarState = ToolStripSystemRenderer.GetToolBarState(item);
			VisualStyleElement normal = VisualStyleElement.ToolBar.Button.Normal;
			if (ToolStripManager.VisualStylesEnabled && VisualStyleRenderer.IsElementDefined(normal))
			{
				VisualStyleRenderer visualStyleRenderer = ToolStripSystemRenderer.VisualStyleRenderer;
				visualStyleRenderer.SetParameters(normal.ClassName, normal.Part, (int)toolBarState);
				visualStyleRenderer.DrawBackground(graphics, new Rectangle(Point.Empty, item.Size));
			}
			else
			{
				this.RenderSmall3DBorderInternal(graphics, new Rectangle(Point.Empty, item.Size), toolBarState, item.RightToLeft == RightToLeft.Yes);
			}
			Rectangle contentRectangle = item.ContentRectangle;
			if (item.BackgroundImage != null)
			{
				ControlPaint.DrawBackgroundImage(graphics, item.BackgroundImage, item.BackColor, item.BackgroundImageLayout, contentRectangle, contentRectangle);
				return;
			}
			ToolStrip currentParent = item.GetCurrentParent();
			if (currentParent != null && toolBarState != ToolBarState.Checked && item.BackColor != currentParent.BackColor)
			{
				ToolStripSystemRenderer.FillBackground(graphics, contentRectangle, item.BackColor);
			}
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x0012518C File Offset: 0x0012338C
		private void RenderSeparatorInternal(Graphics g, ToolStripItem item, Rectangle bounds, bool vertical)
		{
			VisualStyleElement visualStyleElement = vertical ? VisualStyleElement.ToolBar.SeparatorHorizontal.Normal : VisualStyleElement.ToolBar.SeparatorVertical.Normal;
			if (ToolStripManager.VisualStylesEnabled && VisualStyleRenderer.IsElementDefined(visualStyleElement))
			{
				VisualStyleRenderer visualStyleRenderer = ToolStripSystemRenderer.VisualStyleRenderer;
				visualStyleRenderer.SetParameters(visualStyleElement.ClassName, visualStyleElement.Part, ToolStripSystemRenderer.GetItemState(item));
				visualStyleRenderer.DrawBackground(g, bounds);
				return;
			}
			Color foreColor = item.ForeColor;
			Color backColor = item.BackColor;
			Pen controlDark = SystemPens.ControlDark;
			bool pen = ToolStripSystemRenderer.GetPen(foreColor, ref controlDark);
			try
			{
				if (vertical)
				{
					if (bounds.Height >= 4)
					{
						bounds.Inflate(0, -2);
					}
					bool flag = item.RightToLeft == RightToLeft.Yes;
					Pen pen2 = flag ? SystemPens.ButtonHighlight : controlDark;
					Pen pen3 = flag ? controlDark : SystemPens.ButtonHighlight;
					int num = bounds.Width / 2;
					g.DrawLine(pen2, num, bounds.Top, num, bounds.Bottom);
					num++;
					g.DrawLine(pen3, num, bounds.Top, num, bounds.Bottom);
				}
				else
				{
					if (bounds.Width >= 4)
					{
						bounds.Inflate(-2, 0);
					}
					int num2 = bounds.Height / 2;
					g.DrawLine(controlDark, bounds.Left, num2, bounds.Right, num2);
					num2++;
					g.DrawLine(SystemPens.ButtonHighlight, bounds.Left, num2, bounds.Right, num2);
				}
			}
			finally
			{
				if (pen && controlDark != null)
				{
					controlDark.Dispose();
				}
			}
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x00125308 File Offset: 0x00123508
		private void RenderSmall3DBorderInternal(Graphics g, Rectangle bounds, ToolBarState state, bool rightToLeft)
		{
			if (state == ToolBarState.Hot || state == ToolBarState.Pressed || state == ToolBarState.Checked)
			{
				Pen pen = (state == ToolBarState.Hot) ? SystemPens.ButtonHighlight : SystemPens.ButtonShadow;
				Pen pen2 = (state == ToolBarState.Hot) ? SystemPens.ButtonShadow : SystemPens.ButtonHighlight;
				Pen pen3 = rightToLeft ? pen2 : pen;
				Pen pen4 = rightToLeft ? pen : pen2;
				g.DrawLine(pen, bounds.Left, bounds.Top, bounds.Right - 1, bounds.Top);
				g.DrawLine(pen3, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 1);
				g.DrawLine(pen4, bounds.Right - 1, bounds.Top, bounds.Right - 1, bounds.Bottom - 1);
				g.DrawLine(pen2, bounds.Left, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
			}
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x001253F4 File Offset: 0x001235F4
		private void RenderStatusStripBorder(ToolStripRenderEventArgs e)
		{
			if (!Application.RenderWithVisualStyles)
			{
				e.Graphics.DrawLine(SystemPens.ButtonHighlight, 0, 0, e.ToolStrip.Width, 0);
			}
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x0012541C File Offset: 0x0012361C
		private static void RenderStatusStripBackground(ToolStripRenderEventArgs e)
		{
			if (Application.RenderWithVisualStyles)
			{
				VisualStyleRenderer visualStyleRenderer = ToolStripSystemRenderer.VisualStyleRenderer;
				visualStyleRenderer.SetParameters(VisualStyleElement.Status.Bar.Normal);
				visualStyleRenderer.DrawBackground(e.Graphics, new Rectangle(0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1));
				return;
			}
			if (!SystemInformation.InLockedTerminalSession())
			{
				e.Graphics.Clear(e.BackColor);
			}
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x00125488 File Offset: 0x00123688
		private static void RenderLabelInternal(ToolStripItemRenderEventArgs e)
		{
			ToolStripItem item = e.Item;
			Graphics graphics = e.Graphics;
			Rectangle contentRectangle = item.ContentRectangle;
			if (item.BackgroundImage != null)
			{
				ControlPaint.DrawBackgroundImage(graphics, item.BackgroundImage, item.BackColor, item.BackgroundImageLayout, contentRectangle, contentRectangle);
				return;
			}
			VisualStyleRenderer visualStyleRenderer = ToolStripSystemRenderer.VisualStyleRenderer;
			if (visualStyleRenderer == null || item.BackColor != SystemColors.Control)
			{
				ToolStripSystemRenderer.FillBackground(graphics, contentRectangle, item.BackColor);
			}
		}

		// Token: 0x040025D9 RID: 9689
		[ThreadStatic]
		private static VisualStyleRenderer renderer;

		// Token: 0x040025DA RID: 9690
		private ToolStripRenderer toolStripHighContrastRenderer;
	}
}
