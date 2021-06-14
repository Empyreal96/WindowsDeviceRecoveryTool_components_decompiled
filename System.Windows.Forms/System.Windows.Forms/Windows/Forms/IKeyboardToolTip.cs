using System;
using System.Collections.Generic;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x02000280 RID: 640
	internal interface IKeyboardToolTip
	{
		// Token: 0x06002656 RID: 9814
		bool CanShowToolTipsNow();

		// Token: 0x06002657 RID: 9815
		Rectangle GetNativeScreenRectangle();

		// Token: 0x06002658 RID: 9816
		IList<Rectangle> GetNeighboringToolsRectangles();

		// Token: 0x06002659 RID: 9817
		bool IsHoveredWithMouse();

		// Token: 0x0600265A RID: 9818
		bool HasRtlModeEnabled();

		// Token: 0x0600265B RID: 9819
		bool AllowsToolTip();

		// Token: 0x0600265C RID: 9820
		IWin32Window GetOwnerWindow();

		// Token: 0x0600265D RID: 9821
		void OnHooked(ToolTip toolTip);

		// Token: 0x0600265E RID: 9822
		void OnUnhooked(ToolTip toolTip);

		// Token: 0x0600265F RID: 9823
		string GetCaptionForTool(ToolTip toolTip);

		// Token: 0x06002660 RID: 9824
		bool ShowsOwnToolTip();

		// Token: 0x06002661 RID: 9825
		bool IsBeingTabbedTo();

		// Token: 0x06002662 RID: 9826
		bool AllowsChildrenToShowToolTips();
	}
}
