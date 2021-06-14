using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ToolTip.Draw" /> event.</summary>
	// Token: 0x02000233 RID: 563
	public class DrawToolTipEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DrawToolTipEventArgs" /> class.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> context used to draw the ToolTip. </param>
		/// <param name="associatedWindow">The <see cref="T:System.Windows.Forms.IWin32Window" /> that the ToolTip is bound to.</param>
		/// <param name="associatedControl">The <see cref="T:System.Windows.Forms.Control" /> that the ToolTip is being created for.</param>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> that outlines the area where the ToolTip is to be displayed.</param>
		/// <param name="toolTipText">A <see cref="T:System.String" /> containing the text for the ToolTip.</param>
		/// <param name="backColor">The <see cref="T:System.Drawing.Color" /> of the ToolTip background.</param>
		/// <param name="foreColor">The <see cref="T:System.Drawing.Color" /> of the ToolTip text. </param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> used to draw the ToolTip text.</param>
		// Token: 0x0600219D RID: 8605 RVA: 0x000A4F14 File Offset: 0x000A3114
		public DrawToolTipEventArgs(Graphics graphics, IWin32Window associatedWindow, Control associatedControl, Rectangle bounds, string toolTipText, Color backColor, Color foreColor, Font font)
		{
			this.graphics = graphics;
			this.associatedWindow = associatedWindow;
			this.associatedControl = associatedControl;
			this.bounds = bounds;
			this.toolTipText = toolTipText;
			this.backColor = backColor;
			this.foreColor = foreColor;
			this.font = font;
		}

		/// <summary>Gets the graphics surface used to draw the <see cref="T:System.Windows.Forms.ToolTip" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Graphics" /> on which to draw the <see cref="T:System.Windows.Forms.ToolTip" />.</returns>
		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x0600219E RID: 8606 RVA: 0x000A4F64 File Offset: 0x000A3164
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		/// <summary>Gets the window to which this <see cref="T:System.Windows.Forms.ToolTip" /> is bound.</summary>
		/// <returns>The window which owns the <see cref="T:System.Windows.Forms.ToolTip" />.</returns>
		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x0600219F RID: 8607 RVA: 0x000A4F6C File Offset: 0x000A316C
		public IWin32Window AssociatedWindow
		{
			get
			{
				return this.associatedWindow;
			}
		}

		/// <summary>Gets the control for which the <see cref="T:System.Windows.Forms.ToolTip" /> is being drawn.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Control" /> that is associated with the <see cref="T:System.Windows.Forms.ToolTip" /> when the <see cref="E:System.Windows.Forms.ToolTip.Draw" /> event occurs. The return value will be <see langword="null" /> if the ToolTip is not associated with a control.</returns>
		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x060021A0 RID: 8608 RVA: 0x000A4F74 File Offset: 0x000A3174
		public Control AssociatedControl
		{
			get
			{
				return this.associatedControl;
			}
		}

		/// <summary>Gets the size and location of the <see cref="T:System.Windows.Forms.ToolTip" /> to draw.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the <see cref="T:System.Windows.Forms.ToolTip" /> to draw.</returns>
		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x060021A1 RID: 8609 RVA: 0x000A4F7C File Offset: 0x000A317C
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		/// <summary>Gets the text for the <see cref="T:System.Windows.Forms.ToolTip" /> that is being drawn.</summary>
		/// <returns>The text that is associated with the <see cref="T:System.Windows.Forms.ToolTip" /> when the <see cref="E:System.Windows.Forms.ToolTip.Draw" /> event occurs.</returns>
		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x060021A2 RID: 8610 RVA: 0x000A4F84 File Offset: 0x000A3184
		public string ToolTipText
		{
			get
			{
				return this.toolTipText;
			}
		}

		/// <summary>Gets the font used to draw the <see cref="T:System.Windows.Forms.ToolTip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Font" /> object.</returns>
		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x060021A3 RID: 8611 RVA: 0x000A4F8C File Offset: 0x000A318C
		public Font Font
		{
			get
			{
				return this.font;
			}
		}

		/// <summary>Draws the background of the <see cref="T:System.Windows.Forms.ToolTip" /> using the system background color.</summary>
		// Token: 0x060021A4 RID: 8612 RVA: 0x000A4F94 File Offset: 0x000A3194
		public void DrawBackground()
		{
			Brush brush = new SolidBrush(this.backColor);
			this.Graphics.FillRectangle(brush, this.bounds);
			brush.Dispose();
		}

		/// <summary>Draws the text of the <see cref="T:System.Windows.Forms.ToolTip" /> using the system text color and font.</summary>
		// Token: 0x060021A5 RID: 8613 RVA: 0x000A4FC5 File Offset: 0x000A31C5
		public void DrawText()
		{
			this.DrawText(TextFormatFlags.HidePrefix | TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter);
		}

		/// <summary>Draws the text of the <see cref="T:System.Windows.Forms.ToolTip" /> using the system text color and font, and the specified text layout.</summary>
		/// <param name="flags">A <see cref="T:System.Windows.Forms.TextFormatFlags" /> containing a bitwise combination of values that specifies the display and layout for the <see cref="P:System.Windows.Forms.DrawToolTipEventArgs.ToolTipText" />.</param>
		// Token: 0x060021A6 RID: 8614 RVA: 0x000A4FD2 File Offset: 0x000A31D2
		public void DrawText(TextFormatFlags flags)
		{
			TextRenderer.DrawText(this.graphics, this.toolTipText, this.font, this.bounds, this.foreColor, flags);
		}

		/// <summary>Draws the border of the <see cref="T:System.Windows.Forms.ToolTip" /> using the system border color.</summary>
		// Token: 0x060021A7 RID: 8615 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		public void DrawBorder()
		{
			ControlPaint.DrawBorder(this.graphics, this.bounds, SystemColors.WindowFrame, ButtonBorderStyle.Solid);
		}

		// Token: 0x04000E9C RID: 3740
		private readonly Graphics graphics;

		// Token: 0x04000E9D RID: 3741
		private readonly IWin32Window associatedWindow;

		// Token: 0x04000E9E RID: 3742
		private readonly Control associatedControl;

		// Token: 0x04000E9F RID: 3743
		private readonly Rectangle bounds;

		// Token: 0x04000EA0 RID: 3744
		private readonly string toolTipText;

		// Token: 0x04000EA1 RID: 3745
		private readonly Color backColor;

		// Token: 0x04000EA2 RID: 3746
		private readonly Color foreColor;

		// Token: 0x04000EA3 RID: 3747
		private readonly Font font;
	}
}
