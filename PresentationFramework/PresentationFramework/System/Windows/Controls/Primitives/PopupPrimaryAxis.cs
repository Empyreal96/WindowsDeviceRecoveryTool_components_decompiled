using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Describes the direction to move a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control to increase the amount of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> that is visible.</summary>
	// Token: 0x0200059D RID: 1437
	public enum PopupPrimaryAxis
	{
		/// <summary>A <see cref="T:System.Windows.Controls.Primitives.Popup" /> control changes position according to default <see cref="T:System.Windows.Controls.Primitives.Popup" /> behavior. </summary>
		// Token: 0x04003091 RID: 12433
		None,
		/// <summary>A <see cref="T:System.Windows.Controls.Primitives.Popup" /> control changes position by moving along the horizontal axis of the screen before moving along the vertical axis. </summary>
		// Token: 0x04003092 RID: 12434
		Horizontal,
		/// <summary>A <see cref="T:System.Windows.Controls.Primitives.Popup" /> control changes position by moving along the vertical axis of the screen before moving along the horizontal axis.</summary>
		// Token: 0x04003093 RID: 12435
		Vertical
	}
}
