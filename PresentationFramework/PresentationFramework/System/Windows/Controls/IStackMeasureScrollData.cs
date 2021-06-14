using System;

namespace System.Windows.Controls
{
	// Token: 0x02000537 RID: 1335
	internal interface IStackMeasureScrollData
	{
		// Token: 0x17001505 RID: 5381
		// (get) Token: 0x06005672 RID: 22130
		// (set) Token: 0x06005673 RID: 22131
		Vector Offset { get; set; }

		// Token: 0x17001506 RID: 5382
		// (get) Token: 0x06005674 RID: 22132
		// (set) Token: 0x06005675 RID: 22133
		Size Viewport { get; set; }

		// Token: 0x17001507 RID: 5383
		// (get) Token: 0x06005676 RID: 22134
		// (set) Token: 0x06005677 RID: 22135
		Size Extent { get; set; }

		// Token: 0x17001508 RID: 5384
		// (get) Token: 0x06005678 RID: 22136
		// (set) Token: 0x06005679 RID: 22137
		Vector ComputedOffset { get; set; }

		// Token: 0x0600567A RID: 22138
		void SetPhysicalViewport(double value);
	}
}
