using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the image to draw when drawing a menu with the <see cref="M:System.Windows.Forms.ControlPaint.DrawMenuGlyph(System.Drawing.Graphics,System.Drawing.Rectangle,System.Windows.Forms.MenuGlyph)" /> method.</summary>
	// Token: 0x020002E3 RID: 739
	public enum MenuGlyph
	{
		/// <summary>Draws a submenu arrow.</summary>
		// Token: 0x040012FD RID: 4861
		Arrow,
		/// <summary>Draws a menu check mark.</summary>
		// Token: 0x040012FE RID: 4862
		Checkmark,
		/// <summary>Draws a menu bullet.</summary>
		// Token: 0x040012FF RID: 4863
		Bullet,
		/// <summary>The minimum value available by this enumeration (equal to the <see cref="F:System.Windows.Forms.MenuGlyph.Arrow" /> value).</summary>
		// Token: 0x04001300 RID: 4864
		Min = 0,
		/// <summary>The maximum value available by this enumeration (equal to the <see cref="F:System.Windows.Forms.MenuGlyph.Bullet" /> value).</summary>
		// Token: 0x04001301 RID: 4865
		Max = 2
	}
}
