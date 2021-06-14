using System;

namespace System.Windows.Forms
{
	/// <summary>Allows a control to act like a button on a form.</summary>
	// Token: 0x02000278 RID: 632
	public interface IButtonControl
	{
		/// <summary>Gets or sets the value returned to the parent form when the button is clicked.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x0600263A RID: 9786
		// (set) Token: 0x0600263B RID: 9787
		DialogResult DialogResult { get; set; }

		/// <summary>Notifies a control that it is the default button so that its appearance and behavior is adjusted accordingly.</summary>
		/// <param name="value">
		///       <see langword="true" /> if the control should behave as a default button; otherwise <see langword="false" />. </param>
		// Token: 0x0600263C RID: 9788
		void NotifyDefault(bool value);

		/// <summary>Generates a <see cref="E:System.Windows.Forms.Control.Click" /> event for the control.</summary>
		// Token: 0x0600263D RID: 9789
		void PerformClick();
	}
}
