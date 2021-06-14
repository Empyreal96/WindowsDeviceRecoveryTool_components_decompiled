using System;

namespace System.Windows.Data
{
	/// <summary>Describes the timing of binding source updates.</summary>
	// Token: 0x0200019C RID: 412
	public enum UpdateSourceTrigger
	{
		/// <summary>The default <see cref="T:System.Windows.Data.UpdateSourceTrigger" /> value of the binding target property. The default value for most dependency properties is <see cref="F:System.Windows.Data.UpdateSourceTrigger.PropertyChanged" />, while the <see cref="P:System.Windows.Controls.TextBox.Text" /> property has a default value of <see cref="F:System.Windows.Data.UpdateSourceTrigger.LostFocus" />.</summary>
		// Token: 0x040012FD RID: 4861
		Default,
		/// <summary>Updates the binding source immediately whenever the binding target property changes.</summary>
		// Token: 0x040012FE RID: 4862
		PropertyChanged,
		/// <summary>Updates the binding source whenever the binding target element loses focus.</summary>
		// Token: 0x040012FF RID: 4863
		LostFocus,
		/// <summary>Updates the binding source only when you call the <see cref="M:System.Windows.Data.BindingExpression.UpdateSource" /> method.</summary>
		// Token: 0x04001300 RID: 4864
		Explicit
	}
}
