using System;

namespace System.Windows.Controls
{
	// Token: 0x02000563 RID: 1379
	internal static class EditingModeHelper
	{
		// Token: 0x06005B60 RID: 23392 RVA: 0x0019C42D File Offset: 0x0019A62D
		internal static bool IsDefined(InkCanvasEditingMode InkCanvasEditingMode)
		{
			return InkCanvasEditingMode >= InkCanvasEditingMode.None && InkCanvasEditingMode <= InkCanvasEditingMode.EraseByStroke;
		}
	}
}
