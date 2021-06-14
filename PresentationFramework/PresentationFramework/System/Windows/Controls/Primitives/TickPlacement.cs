using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Specifies the position of tick marks in a <see cref="T:System.Windows.Controls.Slider" /> control with respect to the <see cref="T:System.Windows.Controls.Primitives.Track" /> that the control implements.</summary>
	// Token: 0x020005B0 RID: 1456
	public enum TickPlacement
	{
		/// <summary>No tick marks appear. </summary>
		// Token: 0x0400312B RID: 12587
		None,
		/// <summary>Tick marks appear above the <see cref="T:System.Windows.Controls.Primitives.Track" /> for a horizontal <see cref="T:System.Windows.Controls.Slider" />, or to the left of the <see cref="T:System.Windows.Controls.Primitives.Track" /> for a vertical <see cref="T:System.Windows.Controls.Slider" />. </summary>
		// Token: 0x0400312C RID: 12588
		TopLeft,
		/// <summary>Tick marks appear below the <see cref="T:System.Windows.Controls.Primitives.Track" /> for a horizontal <see cref="T:System.Windows.Controls.Slider" />, or to the right of the <see cref="T:System.Windows.Controls.Primitives.Track" /> for a vertical <see cref="T:System.Windows.Controls.Slider" />. </summary>
		// Token: 0x0400312D RID: 12589
		BottomRight,
		/// <summary>Tick marks appear above and below the <see cref="T:System.Windows.Controls.Primitives.Track" /> bar for a horizontal <see cref="T:System.Windows.Controls.Slider" />, or to the left and right of the <see cref="T:System.Windows.Controls.Primitives.Track" /> for a vertical <see cref="T:System.Windows.Controls.Slider" />.</summary>
		// Token: 0x0400312E RID: 12590
		Both
	}
}
