using System;

namespace System.Windows.Forms.Internal
{
	// Token: 0x020004EC RID: 1260
	[Flags]
	internal enum DeviceContextLayout
	{
		// Token: 0x04003546 RID: 13638
		Normal = 0,
		// Token: 0x04003547 RID: 13639
		RightToLeft = 1,
		// Token: 0x04003548 RID: 13640
		BottomToTop = 2,
		// Token: 0x04003549 RID: 13641
		VerticalBeforeHorizontal = 4,
		// Token: 0x0400354A RID: 13642
		BitmapOrientationPreserved = 8
	}
}
