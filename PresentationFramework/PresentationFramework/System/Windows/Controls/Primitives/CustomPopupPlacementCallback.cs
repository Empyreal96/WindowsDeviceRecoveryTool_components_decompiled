using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents a method that provides custom positioning for a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control. </summary>
	/// <param name="popupSize">The <see cref="T:System.Windows.Size" /> of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control.</param>
	/// <param name="targetSize">The <see cref="T:System.Windows.Size" /> of the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" />.</param>
	/// <param name="offset">The <see cref="T:System.Windows.Point" /> computed from the <see cref="P:System.Windows.Controls.Primitives.Popup.HorizontalOffset" /> and <see cref="P:System.Windows.Controls.Primitives.Popup.VerticalOffset" /> property values.</param>
	/// <returns>An array of possible <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> positions for the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control relative to the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" />.</returns>
	// Token: 0x0200057B RID: 1403
	// (Invoke) Token: 0x06005C90 RID: 23696
	public delegate CustomPopupPlacement[] CustomPopupPlacementCallback(Size popupSize, Size targetSize, Point offset);
}
