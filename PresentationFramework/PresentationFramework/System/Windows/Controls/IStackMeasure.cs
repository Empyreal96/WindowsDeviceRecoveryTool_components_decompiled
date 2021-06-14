using System;

namespace System.Windows.Controls
{
	// Token: 0x02000536 RID: 1334
	internal interface IStackMeasure
	{
		// Token: 0x17001500 RID: 5376
		// (get) Token: 0x0600566C RID: 22124
		bool IsScrolling { get; }

		// Token: 0x17001501 RID: 5377
		// (get) Token: 0x0600566D RID: 22125
		UIElementCollection InternalChildren { get; }

		// Token: 0x17001502 RID: 5378
		// (get) Token: 0x0600566E RID: 22126
		Orientation Orientation { get; }

		// Token: 0x17001503 RID: 5379
		// (get) Token: 0x0600566F RID: 22127
		bool CanVerticallyScroll { get; }

		// Token: 0x17001504 RID: 5380
		// (get) Token: 0x06005670 RID: 22128
		bool CanHorizontallyScroll { get; }

		// Token: 0x06005671 RID: 22129
		void OnScrollChange();
	}
}
