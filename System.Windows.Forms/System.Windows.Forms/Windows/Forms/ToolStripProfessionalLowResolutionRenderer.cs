using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
	// Token: 0x020003E6 RID: 998
	internal class ToolStripProfessionalLowResolutionRenderer : ToolStripProfessionalRenderer
	{
		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x060042E2 RID: 17122 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal override ToolStripRenderer RendererOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060042E3 RID: 17123 RVA: 0x0011FE87 File Offset: 0x0011E087
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			if (e.ToolStrip is ToolStripDropDown)
			{
				base.OnRenderToolStripBackground(e);
			}
		}

		// Token: 0x060042E4 RID: 17124 RVA: 0x0011FE9D File Offset: 0x0011E09D
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			if (e.ToolStrip is MenuStrip)
			{
				return;
			}
			if (e.ToolStrip is StatusStrip)
			{
				return;
			}
			if (e.ToolStrip is ToolStripDropDown)
			{
				base.OnRenderToolStripBorder(e);
				return;
			}
			this.RenderToolStripBorderInternal(e);
		}

		// Token: 0x060042E5 RID: 17125 RVA: 0x0011FED8 File Offset: 0x0011E0D8
		private void RenderToolStripBorderInternal(ToolStripRenderEventArgs e)
		{
			Rectangle rectangle = new Rectangle(Point.Empty, e.ToolStrip.Size);
			Graphics graphics = e.Graphics;
			using (Pen pen = new Pen(SystemColors.ButtonShadow))
			{
				pen.DashStyle = DashStyle.Dot;
				bool flag = (rectangle.Width & 1) == 1;
				bool flag2 = (rectangle.Height & 1) == 1;
				int num = 2;
				graphics.DrawLine(pen, rectangle.X + num, rectangle.Y, rectangle.Width - 1, rectangle.Y);
				graphics.DrawLine(pen, rectangle.X + num, rectangle.Height - 1, rectangle.Width - 1, rectangle.Height - 1);
				graphics.DrawLine(pen, rectangle.X, rectangle.Y + num, rectangle.X, rectangle.Height - 1);
				graphics.DrawLine(pen, rectangle.Width - 1, rectangle.Y + num, rectangle.Width - 1, rectangle.Height - 1);
				graphics.FillRectangle(SystemBrushes.ButtonShadow, new Rectangle(1, 1, 1, 1));
				if (flag)
				{
					graphics.FillRectangle(SystemBrushes.ButtonShadow, new Rectangle(rectangle.Width - 2, 1, 1, 1));
				}
				if (flag2)
				{
					graphics.FillRectangle(SystemBrushes.ButtonShadow, new Rectangle(1, rectangle.Height - 2, 1, 1));
				}
				if (flag2 && flag)
				{
					graphics.FillRectangle(SystemBrushes.ButtonShadow, new Rectangle(rectangle.Width - 2, rectangle.Height - 2, 1, 1));
				}
			}
		}
	}
}
