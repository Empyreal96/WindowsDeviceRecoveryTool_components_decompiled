using System;
using System.Windows.Input;

namespace System.Windows.Controls
{
	// Token: 0x0200047A RID: 1146
	internal static class CalendarKeyboardHelper
	{
		// Token: 0x06004326 RID: 17190 RVA: 0x0013357C File Offset: 0x0013177C
		public static void GetMetaKeyState(out bool ctrl, out bool shift)
		{
			ctrl = ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control);
			shift = ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift);
		}
	}
}
