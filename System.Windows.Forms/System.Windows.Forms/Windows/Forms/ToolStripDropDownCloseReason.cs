using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the reason that a <see cref="T:System.Windows.Forms.ToolStripDropDown" /> control was closed.</summary>
	// Token: 0x020003A9 RID: 937
	public enum ToolStripDropDownCloseReason
	{
		/// <summary>Specifies that the <see cref="T:System.Windows.Forms.ToolStripDropDown" /> control was closed because another application has received the focus.</summary>
		// Token: 0x040023D0 RID: 9168
		AppFocusChange,
		/// <summary>Specifies that the <see cref="T:System.Windows.Forms.ToolStripDropDown" /> control was closed because an application was launched.</summary>
		// Token: 0x040023D1 RID: 9169
		AppClicked,
		/// <summary>Specifies that the <see cref="T:System.Windows.Forms.ToolStripDropDown" /> control was closed because one of its items was clicked.</summary>
		// Token: 0x040023D2 RID: 9170
		ItemClicked,
		/// <summary>Specifies that the <see cref="T:System.Windows.Forms.ToolStripDropDown" /> control was closed because of keyboard activity, such as the ESC key being pressed.</summary>
		// Token: 0x040023D3 RID: 9171
		Keyboard,
		/// <summary>Specifies that the <see cref="T:System.Windows.Forms.ToolStripDropDown" /> control was closed because the <see cref="M:System.Windows.Forms.ToolStripDropDown.Close" /> method was called.</summary>
		// Token: 0x040023D4 RID: 9172
		CloseCalled
	}
}
