using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies the editing mode for the <see cref="T:System.Windows.Controls.InkCanvas" /></summary>
	// Token: 0x02000562 RID: 1378
	public enum InkCanvasEditingMode
	{
		/// <summary>Indicates that no action is taken when the pen sends data to the <see cref="T:System.Windows.Controls.InkCanvas" />.</summary>
		// Token: 0x04002F79 RID: 12153
		None,
		/// <summary>Indicates that ink appears on the <see cref="T:System.Windows.Controls.InkCanvas" /> when the pen sends data to it.</summary>
		// Token: 0x04002F7A RID: 12154
		Ink,
		/// <summary>Indicates that the <see cref="T:System.Windows.Controls.InkCanvas" /> responds to gestures, and does not receive ink.</summary>
		// Token: 0x04002F7B RID: 12155
		GestureOnly,
		/// <summary>Indicates that the <see cref="T:System.Windows.Controls.InkCanvas" /> responds to gestures, and receives ink.</summary>
		// Token: 0x04002F7C RID: 12156
		InkAndGesture,
		/// <summary>Indicates that the pen selects strokes and elements on the <see cref="T:System.Windows.Controls.InkCanvas" />. </summary>
		// Token: 0x04002F7D RID: 12157
		Select,
		/// <summary>Indicates that the pen erases part of a stroke when the pen intersects the stroke.</summary>
		// Token: 0x04002F7E RID: 12158
		EraseByPoint,
		/// <summary>Indicates that the pen erases an entire stroke when the pen intersects the stroke.</summary>
		// Token: 0x04002F7F RID: 12159
		EraseByStroke
	}
}
