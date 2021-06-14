using System;

namespace System.Windows.Forms
{
	// Token: 0x02000294 RID: 660
	internal interface ISupportToolStripPanel
	{
		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x060026E0 RID: 9952
		// (set) Token: 0x060026E1 RID: 9953
		ToolStripPanelRow ToolStripPanelRow { get; set; }

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x060026E2 RID: 9954
		ToolStripPanelCell ToolStripPanelCell { get; }

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x060026E3 RID: 9955
		// (set) Token: 0x060026E4 RID: 9956
		bool Stretch { get; set; }

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x060026E5 RID: 9957
		bool IsCurrentlyDragging { get; }

		// Token: 0x060026E6 RID: 9958
		void BeginDrag();

		// Token: 0x060026E7 RID: 9959
		void EndDrag();
	}
}
