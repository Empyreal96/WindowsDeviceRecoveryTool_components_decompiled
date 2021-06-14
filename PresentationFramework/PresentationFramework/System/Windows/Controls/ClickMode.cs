using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies when the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event should be raised. </summary>
	// Token: 0x02000484 RID: 1156
	public enum ClickMode
	{
		/// <summary>Specifies that the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event should be raised when a button is pressed and released.</summary>
		// Token: 0x04002841 RID: 10305
		Release,
		/// <summary>Specifies that the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event should be raised as soon as a button is pressed.</summary>
		// Token: 0x04002842 RID: 10306
		Press,
		/// <summary>Specifies that the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event should be raised when the mouse hovers over a control.</summary>
		// Token: 0x04002843 RID: 10307
		Hover
	}
}
