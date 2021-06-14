using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the state of the user interface.</summary>
	// Token: 0x02000417 RID: 1047
	[Flags]
	public enum UICues
	{
		/// <summary>Focus rectangles are displayed after the change.</summary>
		// Token: 0x040026BC RID: 9916
		ShowFocus = 1,
		/// <summary>Keyboard cues are underlined after the change.</summary>
		// Token: 0x040026BD RID: 9917
		ShowKeyboard = 2,
		/// <summary>Focus rectangles are displayed and keyboard cues are underlined after the change.</summary>
		// Token: 0x040026BE RID: 9918
		Shown = 3,
		/// <summary>The state of the focus cues has changed.</summary>
		// Token: 0x040026BF RID: 9919
		ChangeFocus = 4,
		/// <summary>The state of the keyboard cues has changed.</summary>
		// Token: 0x040026C0 RID: 9920
		ChangeKeyboard = 8,
		/// <summary>The state of the focus cues and keyboard cues has changed.</summary>
		// Token: 0x040026C1 RID: 9921
		Changed = 12,
		/// <summary>No change was made.</summary>
		// Token: 0x040026C2 RID: 9922
		None = 0
	}
}
