using System;

namespace System.Windows.Forms
{
	/// <summary>Determines how a control validates its data when it loses user input focus.</summary>
	// Token: 0x0200011A RID: 282
	public enum AutoValidate
	{
		/// <summary>Implicit validation will not occur. Setting this value will not interfere with explicit calls to <see cref="M:System.Windows.Forms.ContainerControl.Validate" /> or <see cref="M:System.Windows.Forms.ContainerControl.ValidateChildren" />.</summary>
		// Token: 0x04000565 RID: 1381
		Disable,
		/// <summary>Implicit validation occurs when the control loses focus.</summary>
		// Token: 0x04000566 RID: 1382
		EnablePreventFocusChange,
		/// <summary>Implicit validation occurs, but if validation fails, focus will still change to the new control. If validation fails, the <see cref="E:System.Windows.Forms.Control.Validated" /> event will not fire.</summary>
		// Token: 0x04000567 RID: 1383
		EnableAllowFocusChange,
		/// <summary>The control inherits its <see cref="T:System.Windows.Forms.AutoValidate" /> behavior from its container (such as a form or another control). If there is no container control, it defaults to <see cref="F:System.Windows.Forms.AutoValidate.EnablePreventFocusChange" />.</summary>
		// Token: 0x04000568 RID: 1384
		Inherit = -1
	}
}
