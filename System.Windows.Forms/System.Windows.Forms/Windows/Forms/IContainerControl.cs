using System;

namespace System.Windows.Forms
{
	/// <summary>Provides the functionality for a control to act as a parent for other controls.</summary>
	// Token: 0x0200027A RID: 634
	public interface IContainerControl
	{
		/// <summary>Gets or sets the control that is active on the container control.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Control" /> that is currently active on the container control.</returns>
		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x0600263F RID: 9791
		// (set) Token: 0x06002640 RID: 9792
		Control ActiveControl { get; set; }

		/// <summary>Activates a specified control.</summary>
		/// <param name="active">The <see cref="T:System.Windows.Forms.Control" /> being activated. </param>
		/// <returns>
		///     <see langword="true" /> if the control is successfully activated; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002641 RID: 9793
		bool ActivateControl(Control active);
	}
}
