using System;

namespace System.Windows
{
	/// <summary>Specifies whether a window can be resized and, if so, how it can be resized. Used by the <see cref="P:System.Windows.Window.ResizeMode" /> property.</summary>
	// Token: 0x0200013F RID: 319
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public enum ResizeMode
	{
		/// <summary>A window cannot be resized. The Minimize and Maximize buttons are not displayed in the title bar.</summary>
		// Token: 0x04000B9E RID: 2974
		NoResize,
		/// <summary>A window can only be minimized and restored. The Minimize and Maximize buttons are both shown, but only the Minimize button is enabled.</summary>
		// Token: 0x04000B9F RID: 2975
		CanMinimize,
		/// <summary>A window can be resized. The Minimize and Maximize buttons are both shown and enabled.</summary>
		// Token: 0x04000BA0 RID: 2976
		CanResize,
		/// <summary>A window can be resized. The Minimize and Maximize buttons are both shown and enabled. A resize grip appears in the bottom-right corner of the window.</summary>
		// Token: 0x04000BA1 RID: 2977
		CanResizeWithGrip
	}
}
